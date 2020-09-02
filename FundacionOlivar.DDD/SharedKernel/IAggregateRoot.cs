using System.Collections.Generic;

namespace FundacionOlivar.DDD.SharedKernel
{
    public interface IAggregateRoot
    {

        IReadOnlyList<IDomainEvent> Events { get; }
        void ClearEvents();

    }
}
