/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 29/07/2020 12:46:45
 *
 */

using System;
using System.Threading.Tasks.Sources;

using Datos.Ef.Excepciones;

using FundacionOlivar.DDD.SharedKernel;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Datos.Ef.Repositorios
{
    /// <summary>
    /// EfAutorRepository
    /// </summary>
    public class EfRepositoryBase<TEntity, TIdentity> : IRepository<TEntity, TIdentity>
        where TEntity : class, IAggregateRoot<TEntity,TIdentity>
        where TIdentity : IEquatable<TIdentity>
    {


        protected EfRepositoryBase(EfUoW context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Set =  Context.Set<TEntity>();
        }

        protected EfUoW Context { get; }

        protected DbSet<TEntity> Set { get; }


        public virtual TEntity GetById(TIdentity id)
        {
            return Set.Find(id)??throw new EntidadNoEncontrada($"La Entidad tipo {typeof(TEntity).ShortDisplayName()} [{id}] no existe en el Sistema.");
        }


        protected virtual void Adding(TEntity entity) { }

        protected virtual void Added(TEntity entity) { }

        public void Add(TEntity entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));


            if (!Context.IsBeginning) throw new Exception("Debe encontrarse dentro de una Transacción activa.");

            Adding(entity);

            /* SOLO PARA EF6.x
            var entry = Context.Entry(entity);

            if (entry.State == EntityState.Detached)
                Set.Add(entity);
            */

            Set.Add(entity);

            Added(entity);
        }

        public void Delete(TEntity entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));


            if (!Context.IsBeginning) throw new Exception("Debe encontrarse dentro de una Transacción activa.");


            //Permite liberar recursos de la entidad
            if (entity is IDisposable disposable)
                disposable.Dispose();

            /* SOLO PARA EF6.x
                var entry = Context.Entry(entity);
            
                if (entry.State == EntityState.Detached)
                    Set.Attach(entity);
            */
            Set.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));

            if (!Context.IsBeginning) throw new Exception("Debe encontrarse dentro de una Transacción activa.");

            /* SOLO PARA EF6.x
                var entry = Context.Entry(entity);
            
                if (entry.State == EntityState.Detached)
                    Set.Attach(entity);

                entry.State = EntityState.Modified;
            */

            Set.Update(entity);
        }

        public TIdentity NextIdentity()
        {
            throw new NotImplementedException();
        }
    }
}