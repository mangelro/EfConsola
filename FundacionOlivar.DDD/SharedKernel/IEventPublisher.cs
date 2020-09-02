/*
 * Copyright © 2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 02/01/2020 14:52:21
 *
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FundacionOlivar.DDD.SharedKernel
{

    /// <summary>
    /// Publicador de eventos de dominio
    /// </summary>
    public interface IEventPublisher
    {

        void Publish<TEvent>(TEvent domainEvents) where TEvent : IDomainEvent;


        Task Publish<TEvent>(TEvent domainEvents, CancellationToken cancellationToken = default(CancellationToken)) where TEvent : IDomainEvent;
    }


}