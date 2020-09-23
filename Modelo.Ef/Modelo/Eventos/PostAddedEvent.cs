/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 31/07/2020 10:07:45
 *
 */

using System;
using System.Collections.Generic;

using System.Text;

using FundacionOlivar.DDD.SharedKernel;

namespace Modelo.Ef.Eventos
{
    /// <summary>
    /// PostAddedEvent
    /// </summary>
    public class PostAddedEvent: IDomainEvent
    {

        public Post PostAdded { get; }

        public Guid EventID { get; }

        public DateTime When { get; }

        public  PostAddedEvent(Post added)
        {

            PostAdded = added;
            When = DateTime.UtcNow;
            EventID = Guid.NewGuid();
        }

    }
}