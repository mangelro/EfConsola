/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 31/07/2020 13:04:40
 *
 */

using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FundacionOlivar.DDD.SharedKernel;

namespace EfConsola.Infraestructura
{
    /// <summary>
    /// FakeEventPublisher
    /// </summary>
    public class FakeEventPublisher : IDomainEventPublisher
    {



        public void Publish<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
        {
            System.Diagnostics.Debug.WriteLine($"Publicando evento {domainEvent.GetType().FullName} [{domainEvent.When}]");
        }

        public Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default) where TEvent : IDomainEvent
        {
            throw new NotImplementedException();
        }
    }
}