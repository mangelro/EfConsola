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

using Modelo.Ef;

namespace Datos.Ef
{
    /// <summary>
    /// Contexto
    /// </summary>
    public sealed class EfUoW : DbContext, IUnitOfWork
    {
        private readonly IDomainEventPublisher _publisher;

        private readonly AutoResetEvent _sincronizar = new AutoResetEvent(true);

        // private readonly IUoWConfig _config;
        private TransactionWrapper _wrapper;

        public EfUoW(IUoWConfig config, IDomainEventPublisher publisher)
            : base(GetOptions(config.ConnectionString))
        {
            _publisher = publisher;
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

            modelBuilder.AddAuditableProperties();

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

            SetAuditableInfo();

            //AggregateRoots afectados en la consulta
            var changedRoots = GetAggregateRootAffected();

            try
            {
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

        private void ThrowIfNotTransaction()
        {
            if (_wrapper == null || _wrapper.IsDisposed)
                throw new Exception("No existe una Transacción activa");
        }

        private void ThrowIfTransaction()
        {
            if (_wrapper != null && !_wrapper.IsDisposed)
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

        //public void Rollback()
        //{
        //        ChangeTracker
        //        .Entries()
        //        .ToList()
        //        .ForEach(x => x.Reload());
        //}

        /// <summary>
        /// Retorna los Aggregates afectados en la transacción
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IAggregateRoot> GetAggregateRootAffected()
        {
            IEnumerable<EntityEntry> affected = ChangeTracker.Entries();

            return affected.Select(e => e.Entity)
                .OfType<IAggregateRoot>()
                .ToList();
        }

        /// <summary>
        /// Publica los eventos,si existen, de los Aggregates afectados en la trasacción
        /// </summary>
        /// <param name="changedRoots"></param>
        private void PublishingEvents(IEnumerable<IAggregateRoot> changedRoots)
        {
            IEnumerable<IDomainEvent> events = changedRoots
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
        ///
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
        public static ModelBuilder AddAuditableProperties(this ModelBuilder modelBuilder)
        {
            // loop over all entities
            foreach (var entity in modelBuilder.Model.GetEntityTypes()
                .Where(x => typeof(IAuditable).IsAssignableFrom(x.ClrType)))
            {
                entity.AddProperty(AuditableField.CREADOPOR_FIELD, typeof(string));

                entity.AddProperty(AuditableField.CREADOEN_FIELD, typeof(DateTimeOffset))
                    .SetValueConverter(ConversionHelper.DateTimeOffsetConverter);
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