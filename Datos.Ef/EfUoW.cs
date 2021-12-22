/*
 * Copyright © 2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 28/07/2020 8:50:17
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Datos.Ef.Configuracion;
using Datos.Ef.Configuracion.Core;

using FundacionOlivar.DDD.SharedKernel;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

using Modelo.Ef.Core;

namespace Datos.Ef
{
    /// <summary>
    /// Contexto
    /// </summary>
    public sealed class EfUoW : DbContext, IUnitOfWork
    {
        private readonly IEventPublisher _publisher;

        private readonly AutoResetEvent _sincronizar = new AutoResetEvent(true);

        private readonly ITenantProvider _tenantProvider;

        private readonly IEnumerable<IEntityTypeConfiguration<IEntity>> _mapeados;

        // private readonly IUoWConfig _config;
        private TransactionWrapper _wrapper;


        public EfUoW(ITenantProvider tenantProvider,
            IUoWConfig config,
            IEventPublisher publisher)
            : base(GetOptions(config.ConnectionString))
        {
            _publisher = publisher;
            _tenantProvider = tenantProvider;
        }

        public EfUoW(DbContextOptions<EfUoW> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AutorMap());

            modelBuilder.ApplyConfiguration(new BlogMap());

            modelBuilder.ApplyConfiguration(new PostMap());

            modelBuilder.ApplyConfiguration(new ProyectoMap());

            modelBuilder.ApplyConfiguration(new ItemProyectoMap());

            //-----------------------------------------------------

            modelBuilder.AddAuditableProperties();

            modelBuilder.AddTenantProperty();

            modelBuilder.SetTenantFilter(_tenantProvider.ApplicationId);

            base.OnModelCreating(modelBuilder);
        }

        public void Begin()
        {
            //EF Core no admite que varias operaciones en paralelo se ejecuten en la misma instancia de contexto.
            //Necesita crear instancias únicas de DbContext para cada Task.
            _sincronizar.WaitOne();

            ThrowIfTransaction();

            _wrapper = new TransactionWrapper(Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted));
        }

        public async Task CommitAsync()
        {
            ThrowIfNotTransaction();

            SetAuditableInfo();

            //AggregateRoots afectados en la consulta
            var changedRoots = GetAggregateRootAffected();
            try
            {
                int changedEntities = await base.SaveChangesAsync()
                                .ConfigureAwait(false);

                System.Diagnostics.Debug.WriteLine($"{changedEntities} afectadas en la transacción");

                await _wrapper.Transaction.CommitAsync()
                                            .ConfigureAwait(false);

                await PublishingEventsAsync(changedRoots);
            }
            catch
            {
                await _wrapper.Transaction.RollbackAsync();
                throw;
            }
            finally
            {
                _wrapper.Dispose();
                _sincronizar.Set();
            }
        }

        public void Commit()
        {
            ThrowIfNotTransaction();

            SetTenantInfo(_tenantProvider.ApplicationId);

            SetAuditableInfo();

            //AggregateRoots afectados en la consulta
            var changedRoots = GetAggregateRootAffected();

            try
            {

                /*
                 * http://lostechies.com/jimmybogard/2014/05/13/a-better-domain-events-pattern/
                 * Se propone publicar los eventos antes de confirmar la trasacción
                 * An event is something that happened, it's in the past. 
                 * Thus, an event handler simply can't (it shouldn't) change the event, 
                 * it's like changing the past.
                 * Domain Events pattern: an operation is simply a chain of command -> event -> command -> event -> etc.
                 */

                int changedEntities = base.SaveChanges();
                System.Diagnostics.Debug.WriteLine($"{changedEntities} afectadas en la transacción");

                _wrapper.Transaction.Commit();

                PublishingEvents(changedRoots);
            }
            catch
            {
                _wrapper.Transaction.Rollback();
                throw;
            }
            finally
            {
                _wrapper.Dispose();
                _sincronizar.Set();
            }
        }

        /// <summary>
        /// Determina si la trasacción ha sido inicializada o no
        /// </summary>
        public bool IsBeginning => _wrapper?.IsDisposed == false; //COMORRRRR

        private void ThrowIfNotTransaction()
        {
            if (!IsBeginning)
                throw new Exception("No existe una Transacción activa");
        }

        private void ThrowIfTransaction()
        {
            if (IsBeginning)
                throw new Exception("Ya existe una Transacción activa");
        }

        /// <summary>
        /// Establece la información de auditoria en la entidades auditables
        /// </summary>
        private void SetAuditableInfo()
        {
            //TODO: Falta info de modificación
            //.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            foreach (var entry in GetEntriesAffected(e => e.State == EntityState.Added))
            {
                if (entry.Entity is IAuditable)
                {
                    entry.Property(AuditableField.CREADOPOR_FIELD).CurrentValue = "miguel";
                    entry.Property(AuditableField.CREADOEN_FIELD).CurrentValue = DateTimeOffset.UtcNow;
                }
            }
        }

        private void SetTenantInfo(Guid tenant)
        {
            foreach (var entry in GetEntriesAffected(e => e.State == EntityState.Added))
            {
                if (entry.Entity is ITenant)
                {
                    entry.Property(TenantField.TENANT_FIELD).CurrentValue = tenant.ToString();
                }
            }
        }

        //public void Rollback()
        //{
        //        ChangeTracker
        //        .Entries()
        //        .ToList()
        //        .ForEach(x => x.Reload());
        //}

        /// <summary>
        /// Retorna los Aggregates Root afectados en la transacción
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IAggregateRoot> GetAggregateRootAffected()
        {
            //IEnumerable<EntityEntry> affected = ChangeTracker.Entries<>();

            //return affected.Select(e => e.Entity)
            //    .OfType<IAggregateRoot>()
            //    .ToList();

            return ChangeTracker.Entries<IAggregateRoot>().Select(e => e.Entity);
        }

        /// <summary>
        /// Publica los eventos,si existen, de los Aggregates afectados en la trasacción
        /// </summary>
        /// <param name="changedRoots"></param>
        private void PublishingEvents(IEnumerable<IAggregateRoot> changedRoots)
        {
            List<IDomainEvent> events = changedRoots
                                                    .Where(r => r.Events.Any())
                                                    .SelectMany(r => r.Events)
                                                    .ToList();

            changedRoots.ToList().ForEach(r => r.ClearEvents());

            foreach (var e in events)
            {
                _publisher.Publish(e);
            }
        }

        /// <summary>
        /// Publica los eventos,si existen, de los Aggregates afectados en la trasacción
        /// </summary>
        /// <param name="changedRoots"></param>
        /// <returns></returns>
        private Task PublishingEventsAsync(IEnumerable<IAggregateRoot> changedRoots)
        {
            IEnumerable<IDomainEvent> events = changedRoots
                                                    .Where(r => r.Events.Any())
                                                    .SelectMany(r => r.Events)
                                                    .ToList();

            changedRoots.ToList().ForEach(r => r.ClearEvents());

            List<Task> tasks = new List<Task>();

            foreach (var e in events)
            {
                tasks.Add(_publisher.PublishAsync(e));
            }

            return Task.WhenAll(tasks);
        }

        /// <summary>
        /// Retorna los EntityEntry afectados.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private IEnumerable<EntityEntry> GetEntriesAffected(Expression<Func<EntityEntry, bool>> query)
        {
            IEnumerable<EntityEntry> affected = ChangeTracker.Entries();
            return affected.Where(query.Compile());
        }


        public override void Dispose()
        {
            _sincronizar.Dispose();
            base.Dispose();
        }

        private static DbContextOptions<EfUoW> GetOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EfUoW>();

            optionsBuilder.UseLoggerFactory(new NLog.Extensions.Logging.NLogLoggerFactory());
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
#endif

            SqliteConnectionStringBuilder connBuilder = new SqliteConnectionStringBuilder(connectionString)
            {
                Mode = SqliteOpenMode.ReadWrite
            };

            return optionsBuilder.UseSqlite(connBuilder.ConnectionString).Options;
        }
    }

    public static class ModelBuilderHelper
    {
        /// <summary>
        /// Agrega los campo shadows de auditoria a las entidades que están marcadas 
        /// con la interfaz IAuditable
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder AddAuditableProperties(this ModelBuilder modelBuilder)
        {
            // loop over all entities
            foreach (var entity in modelBuilder.Model.GetEntityTypes()
                .Where(x => typeof(IAuditable).IsAssignableFrom(x.ClrType)))
            {

                //entity.AddProperty("Timestamp", typeof(long))
                //    .AddAnnotation("Timestamp",null);

                //entity.AddProperty(AuditableField.CREADOPOR_FIELD, typeof(string));
                modelBuilder.Entity(entity.ClrType).Property<string>(AuditableField.CREADOPOR_FIELD);


                //entity.AddProperty(AuditableField.CREADOEN_FIELD, typeof(DateTimeOffset))
                //    .SetValueConverter(ConversionHelper.DateTimeOffsetConverter);
                modelBuilder.Entity(entity.ClrType).Property<DateTimeOffset>(AuditableField.CREADOEN_FIELD)
                    .HasConversion(ConversionHelper.DateTimeOffsetConverter);

            }

            return modelBuilder;
        }

        /// <summary>
        /// Agrega los campo shadows de Tenant a las entidades que están marcadas 
        /// con la interfaz ITenant.
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder AddTenantProperty(this ModelBuilder modelBuilder)
        {

            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(x => typeof(ITenant).IsAssignableFrom(x.ClrType)))
            {
                // entity.AddProperty(TenantField.TENANT_FIELD, typeof(string));

                modelBuilder.Entity(entityType.ClrType).Property<string>(TenantField.TENANT_FIELD);
            }
            return modelBuilder;
        }


        /// <summary>
        /// Además agrega el filtro de consulta para entidades marcadas con 
        /// la interfaz ITenant
        /// del sistema
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="tenant"></param>
        /// <returns></returns>
        public static ModelBuilder SetTenantFilter(this ModelBuilder modelBuilder, Guid tenant)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(x => typeof(ITenant).IsAssignableFrom(x.ClrType)))
            {
                /*
                 * Se define para la propiedad Shadows el filtro de consulta.
                 * Todas las entidades marcadas con la interfaz ITenant se consultaran
                 * con el filtro Tenant Id.
                 */

                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var body = Expression.Equal(
                    Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(string) }, parameter, Expression.Constant(TenantField.TENANT_FIELD)),
                    Expression.Constant(tenant.ToString()));

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
            }

            return modelBuilder;
        }

    }

    internal class TransactionWrapper : IDisposable
    {
        public TransactionWrapper(IDbContextTransaction trasaction)
        {
            Transaction = trasaction;
            OnDisposing = () => { };
        }

        public Action OnDisposing { get; set; }

        public IDbContextTransaction Transaction { get; }

        public bool IsDisposed { get; private set; } = false;

        public void Dispose()
        {
            OnDisposing();
            Transaction.Dispose();
            IsDisposed = true;
        }
    }
}