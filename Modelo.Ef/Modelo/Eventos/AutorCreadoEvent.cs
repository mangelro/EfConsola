/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 31/07/2020 9:28:13
 *
 */

using System;
using System.Collections.Generic;

using System.Text;

using FundacionOlivar.DDD.SharedKernel;

namespace Modelo.Ef.Eventos
{
    /// <summary>
    /// AutorCreadoEvent
    /// </summary>
    public class AutorCreadoEvent : IDomainEvent
    {
        public DateTime DateOccurred => DateTime.UtcNow;

        public Autor AutorCreado { get; }


        public AutorCreadoEvent(Autor creado)
        {
            AutorCreado = creado;
        }
    }
}