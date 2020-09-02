/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 30/07/2020 9:44:56
 *
 */

using System;
using System.Collections.Generic;

using FundacionOlivar.DDD.SharedKernel;

namespace FundacionOlivar.DDD.Core
{
    /// <summary>
    /// BaseEntity
    /// </summary>
    public class SurrogateEntity<TIdentity> : BaseEntity<TIdentity> , IAggregateRoot
        where TIdentity : IEquatable<TIdentity>
    {

        private readonly List<IDomainEvent> _events = new List<IDomainEvent>();


        /// <summary>
        /// Clave sustituta en la BD
        /// </summary>
        public int? Key { get; } = null;


        public bool IsTransient()
        {
            return !Key.HasValue;
        }


        /// <summary>
        /// Elimina todos los eventos de Dominio de la Entidad
        /// </summary>
        public virtual void ClearEvents()
        {
            _events.Clear();
        }

        /// <summary>
        /// Eventos de Dominio asociados a la Entidad
        /// </summary>
        public IReadOnlyList<IDomainEvent> Events => _events.AsReadOnly();


        /// <summary>
        /// Añade un Evento de Dominio a la entidad
        /// </summary>
        /// <param name="e"></param>
        protected void AddDomainEvent(IDomainEvent e)
        {
            _events.Add(e);
        }


    }

}