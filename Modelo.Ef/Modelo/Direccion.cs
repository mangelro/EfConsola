/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 28/07/2020 13:52:41
 *
 */

using System.Collections.Generic;

using FundacionOlivar.DDD.SharedKernel;

namespace Modelo.Ef
{
    /// <summary>
    /// Valoracion Value Object
    /// </summary>
    public class Direccion : ValueObject<Direccion>
    {

        public Direccion(string calle, string poblacion, string codigoPostal, string provincia, string pais)
        {
            Calle = calle;
            Poblacion = poblacion;
            CodigoPostal = codigoPostal;
            Provincia = provincia;
            Pais = pais;
        }


        public string Calle { get; set; }

        public string Poblacion { get; set; }

        public string CodigoPostal { get; set; }

        public string Provincia { get; set; }

        public string Pais { get; set; }




        public override string ToString()
        {
            //TODO
            return $"{Calle}, {Poblacion}, {CodigoPostal}, {Provincia}, {Pais}";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Calle;
            yield return Poblacion;
            yield return CodigoPostal;
            yield return Provincia;
            yield return Pais;
        }
    }
}