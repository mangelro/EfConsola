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

using EfConsola.Datos.Configuracion;
using EfConsola.Datos.Repositorios;
using EfConsola.Modelo;

using FundacionOlivar.DDD.SharedKernel;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfConsola.Datos
{
    /// <summary>
    /// Contexto 
    /// </summary>
    public sealed class EfUoW : DbContext, IUnitOfWork
    {


        private readonly IEventPublisher _publisher;

        public EfUoW(string connectionString)
            : base(GetOptions(connectionString))
        { }


        //public EfUoW(DbContextOptions options)
        //    : base(options)
        //{ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AutorConfiguration());

            modelBuilder.ApplyConfiguration(new BlogConfiguration());

            modelBuilder.ApplyConfiguration(new PostConfiguration());




            modelBuilder.AddAuditableProperties();

            base.OnModelCreating(modelBuilder);
        }





        public int Commit()
        {

            //.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            //foreach (var entry in GetEntriesAffected(e => e.State == EntityState.Added))

            foreach (var entry in GetEntriesAffected(e=>e.State== EntityState.Added))
            {
                if (entry.Entity is IAuditable)
                {
                    entry.Property("CreadoPor").CurrentValue = "miguel";
                    entry.Property("CreadoEn").CurrentValue = DateTimeOffset.UtcNow;
                }
            }

            //AggregateRoots afectados en la consulta
            var changedRoots = GetAggregateRootAffected();

            int changedEntities = 0;

            try
            {
                changedEntities = base.SaveChanges();

                if (changedEntities > 0)
                    PublishingEvents(changedRoots);

                return changedEntities;
            }
            catch
            {
                throw;
            }
        }


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
                System.Diagnostics.Debug.WriteLine("Evento publicado - " + e.GetType().FullName);
                _publisher?.Publish(e);
            }
        }


        /// <summary>
        /// Retorna los EntityEntry afectados. 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// 

        
        private IEnumerable<EntityEntry> GetEntriesAffected(Expression<Func<EntityEntry, bool>> query)
        {



            IEnumerable<EntityEntry> affected = ChangeTracker.Entries();
            return affected.Where(query.Compile());
        }

        /// <summary>
        /// Retorna el Repositorio asociado a la Entidad
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IRepository GetRepository<TEntity>() where TEntity : IEntity, IAggregateRoot
        {
            Type entityType = typeof(TEntity);


            if (entityType == typeof(Autor))
                return new EfAutorRepository(Set<Autor>());

            else if (entityType == typeof(Blog))
                return new EfBlogRepository(Set<Blog>());

            throw new ArgumentException(entityType.Name + "Repositorio no implementado");
        }






        public TRepository GetCustomRepository<TRepository>() where TRepository : IRepository
        {

            Type repoType = typeof(TRepository);


            //if (repoType == typeof(Autor))
            //    return new EfAutorRepository(Set<Autor>());

            //else if (repoType == typeof(Blog))
            //    return new EfBlogRepository(Set<Blog>());


            throw new ArgumentException(repoType.Name + "no implementado");
        }






        private static DbContextOptions GetOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder();


            optionsBuilder.UseLoggerFactory(new NLog.Extensions.Logging.NLogLoggerFactory());
            optionsBuilder.EnableSensitiveDataLogging();


            var connBuilder = new SqliteConnectionStringBuilder(connectionString);

            connBuilder.Mode = SqliteOpenMode.ReadWrite;


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
                entity.AddProperty("CreadoPor", typeof(string));

                entity.AddProperty("CreadoEn", typeof(DateTimeOffset))
                    .SetValueConverter(ConversionHelper.DateTimeOffsetConverter);
            }

            return modelBuilder;

        }




    }



}