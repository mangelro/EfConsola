/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 29/07/2020 11:37:21
 *
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using FundacionOlivar.DDD.SharedKernel;

namespace Modelo.Ef
{
    /// <summary>
    /// AutorID
    /// </summary>
    public class ProyectoCode : ValueObject<ProyectoCode>
    {

        private readonly string _code;
        private ProyectoCode(string id)
        {
            _code = id;
        }


        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return _code;
        }

        public override string ToString()
        {
            return _code;
        }


        public static ProyectoCode FromString(string code) => new ProyectoCode(code);
    }
}