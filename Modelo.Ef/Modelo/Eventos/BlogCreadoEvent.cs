/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 31/07/2020 9:53:22
 *
 */

using System;
using System.Collections.Generic;

using System.Text;

using FundacionOlivar.DDD.SharedKernel;

namespace Modelo.Ef.Eventos
{
    /// <summary>
    /// BlogCreadoEvent
    /// </summary>
    public class BlogCreadoEvent : IDomainEvent
    {
        public DateTime DateOccurred => DateTime.UtcNow;

        public Blog BlogCreado { get; }

        public BlogCreadoEvent(Blog creado)
        {
            BlogCreado = creado;
        }

    }
}