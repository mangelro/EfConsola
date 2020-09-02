/*
 * Copyright © 2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 02/01/2020 14:52:21
 *
 */

using System.Threading;
using System.Threading.Tasks;

namespace FundacionOlivar.DDD.SharedKernel
{
    /// <summary>
    /// Publicador de eventos de dominio
    /// </summary>
    public interface IDomainEventPublisher
    {
        void Publish<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;

        Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default(CancellationToken)) where TEvent : IDomainEvent;
    }
}