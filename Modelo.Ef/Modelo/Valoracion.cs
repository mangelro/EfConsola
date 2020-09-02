/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 28/07/2020 13:52:41
 *
 */

using System;
using System.Collections.Generic;

using FundacionOlivar.DDD.SharedKernel;

namespace Modelo.Ef
{
    /// <summary>
    /// Valoracion Value Object
    /// </summary>
    public sealed class Valoracion : ValueObject<Valoracion>
    {
        private readonly int _valoracion;

        private Valoracion(int valoracion)
        {
            _valoracion = valoracion;
        }



        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return _valoracion;
        }


        public override string ToString()
        {
            return _valoracion.ToString();
        }

        public static Valoracion FromInteger(int valoracion)
        {
            if (valoracion < 0 || valoracion > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(valoracion));
            }

            return new Valoracion(valoracion);
        }


        public static implicit operator int(Valoracion valoracion) => valoracion._valoracion;
    }
}