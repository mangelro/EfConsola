/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 30/07/2020 9:44:56
 *
 */

using System;

using FundacionOlivar.DDD.SharedKernel;

namespace FundacionOlivar.DDD.Core
{
    /// <summary>
    /// BaseEntity
    /// </summary>
    public class BaseEntity<TIdentity> : IEntity<TIdentity>
        where TIdentity : IEquatable<TIdentity>
    {
        /// <summary>
        /// Identidad de la Enitidad
        /// </summary>
        public TIdentity Identity { get; protected set; }

        public bool Equals(IEntity<TIdentity> other)
        {

            if ((other == null) || (GetType() != other.GetType()))
                return false;


            return Identity.Equals(other.Identity);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IEntity<TIdentity>);
        }

        public override int GetHashCode()
        {
            return Identity.GetHashCode();
        }

        public static bool operator ==(BaseEntity<TIdentity> left, BaseEntity<TIdentity> right)
        {
            if (object.Equals(left, null))
                return object.Equals(right, null) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(BaseEntity<TIdentity> left, BaseEntity<TIdentity> right)
        {
            return !(left == right);
        }
    }

}