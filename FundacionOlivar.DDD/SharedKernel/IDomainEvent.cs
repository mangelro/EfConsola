/*
 * Copyright © 2020 Fundación del Olivar
 * Todos los derechos reservados
 * Autor: Miguel A. Romera
 * Fecha: 02/01/2020 11:09:11
 */

using System;

namespace FundacionOlivar.DDD.SharedKernel
{
    /// <summary>
    /// IDomainEvent
    /// </summary>
    public interface IDomainEvent
    {
        DateTime DateOccurred { get; }
    }
}
