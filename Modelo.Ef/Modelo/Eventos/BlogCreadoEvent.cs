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

        public Blog BlogCreado { get; }

        public Guid EventID { get; }

        public DateTime When { get; }

        public BlogCreadoEvent(Blog creado)
        {
            BlogCreado = creado;
            When = DateTime.UtcNow;
            EventID = Guid.NewGuid();
        }

    }
}