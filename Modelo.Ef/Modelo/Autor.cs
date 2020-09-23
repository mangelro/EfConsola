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
    public class Autor : AggregateRoot<Autor,Guid>
    {

        DateTimeOffset _fechaNacimiento;

        private Autor(Guid id,DateTimeOffset fechaNacimiento)
        {
            Identity = id;
            _fechaNacimiento = fechaNacimiento;
        }

        protected Autor() { }


        public string Nombre { get; private set; }


        public int Edad => (int)((DateTimeOffset.Now - _fechaNacimiento).TotalDays / 365);
        //public int Edad { get; set; }

        public Direccion Direccion { get; private set; }

        public void EstablecerNombre(string nombreAutor)
        {
            Nombre = nombreAutor;
        }
    

        public void EstablecerDireccion(Direccion direccion)
        {
            Direccion = direccion;
        }

        public void EstablecerFechaNacimiento(DateTimeOffset fechaNacimiento)
        {
            _fechaNacimiento = fechaNacimiento;
        }

        public override string ToString()
        {
            return $"{Nombre} [{Identity}]";
        }


        public static Autor NewAutor(DateTime fechaNacimiento)
        {

            //var a = new Autor(AutorID.FromGuid(Guid.NewGuid()),edad);
            var a = new Autor(Guid.NewGuid(),new DateTimeOffset( fechaNacimiento));
            a.AddEvents(new AutorCreadoEvent(a));
            return a;
        }

    }
}