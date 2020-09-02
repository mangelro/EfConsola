/*
 * Copyright © 2019 Fundación del Olivar
 * Todos los derechos reservados
 * Autor: Miguel A. Romera
 * Fecha: 13/12/2019 11:07:11
 */

using System;
using System.Collections.Generic;
using System.Linq;

using FundacionOlivar.DDD.Utiles;

namespace FundacionOlivar.DDD.SharedKernel
{
    /*
     * Esta implementación es muy útil para
     * implementar por reflexión la comparación de 
     * la propiedades públicas de <T>
     * 
     * Permite que las clases derivadas no tengan porqué implementar
     * la interfaz IEquatable ya que en la clase base se conoce el tipo 
     * de la clase hija <T>.
     * 
     * Si no se utilizara este tipo "fantasma": clase_hija : clase_base<clase_hija>
     * La clase base no tendría forma de definir la interfaz IEquatable de las
     * clases derivadas.
     * 
     */


    [Serializable]
    public abstract class ValueObject<T>: IEquatable<T>
        where T: ValueObject<T>
    {

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return Equals(obj as T);
        }

        public bool Equals(T other)
        {
            if (other is null)
                return false;

            if (Object.ReferenceEquals(this, other))
                return true;


            return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
        }

        public override int GetHashCode()
        {

            return HashCodeHelper.CombineHashCodes(GetAtomicValues());
        }

        /// <summary>
        /// Componentes para la igualdad
        /// </summary>
        protected abstract IEnumerable<object> GetAtomicValues();

        public static bool operator ==(ValueObject<T> left, ValueObject<T> right)
        {
            if (object.Equals(left, null))
                return object.Equals(right, null) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(ValueObject<T> left, ValueObject<T> right)
        {
            return !(left == right);
        }

    }
}