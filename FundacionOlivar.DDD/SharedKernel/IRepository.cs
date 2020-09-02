using System;

namespace FundacionOlivar.DDD.SharedKernel
{
    /// <summary>
    /// The Repository mediates between the domain and data mapping layers,
    /// acting like an in-memory collection of domain objects.
    /// </summary>
    /// <typeparam name="TEntity">Tipo de la entidad que da soporte el repositorio</typeparam>
    /// <typeparam name="TIdentity">Tipo de identidad de la entidad</typeparam>
    /// <see cref="https://martinfowler.com/eaaCatalog/repository.html"/>
    public interface IRepository<TEntity, TIdentity> : IRepository
        where TEntity : IEntity<TIdentity>, IAggregateRoot
        where TIdentity : IEquatable<TIdentity>
    {
        TEntity GetById(TIdentity id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }

    public interface IRepository
    {
    }
}