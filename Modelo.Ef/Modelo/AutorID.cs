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
    public class AutorID : ValueObject<AutorID>
    {

        private readonly Guid _id;
        private AutorID(Guid id)
        {
            _id = id;
        }


        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return _id;
        }

        public override string ToString()
        {
            return _id.ToString();
        }

        public static AutorID FromGuid(Guid id) => new AutorID(id);

        public static AutorID FromString(string id) => new AutorID(Guid.Parse(id));
    }
}