/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 28/07/2020 14:12:23
 *
 */

using System;

using FundacionOlivar.DDD.Core;

using Modelo.Ef.Eventos;

namespace Modelo.Ef
{
    /// <summary>
    /// Sustituimos el tipo de Autor por GUID
    /// </summary>
    public class Autor : AggregateRoot<AutorID>
    {

        private Autor(AutorID id,int edad)
        {
            Identity = id;
            Edad = edad;
        }

        protected Autor() { }


        public string Nombre { get; private set; }

        public int Edad { get; private set; }

        public Direccion Direccion { get; private set; }


        public void EstablecerNombre(string nombreAutor)
        {
            Nombre = nombreAutor;
        }

        public void EstablecerEdad(int edad)
        {
            Edad = edad;
        }


        public void EstablecerDireccion(Direccion direccion)
        {
            Direccion = direccion;
        }

        public override string ToString()
        {
            return $"{Nombre} [{Identity}]";
        }



        public static Autor NewAutor(int edad)
        {

            var a = new Autor(AutorID.FromGuid(Guid.NewGuid()),edad);
           
            a.AddDomainEvent(new AutorCreadoEvent(a));
            return a;
        }

    }
}