using System;

namespace FundacionOlivar.DDD.SharedKernel
{


    public interface IEntity<TIdentity> : IEntity, IEquatable<IEntity<TIdentity>>
        where TIdentity : IEquatable<TIdentity>
    {
        TIdentity Identity { get; }
    }

    public interface IEntity
    {
    }
}
