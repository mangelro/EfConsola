/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 31/07/2020 13:27:18
 *
 */

using System;
using System.Collections.Generic;

using FundacionOlivar.DDD.Core;

using Modelo.Ef.Core;


namespace Modelo.Ef
{
    /// <summary>
    /// Proyecto
    /// </summary>
    public class Proyecto : SurrogateEntity<Proyecto, ProyectoCode>, IAuditable, ITenant
    {

        protected Proyecto() { }

        protected Proyecto(ProyectoCode code)
        {
            Identity = code;
            //Aplicacion = app;
        }


        //public Guid Aplicacion { get; }

        public DateTimeOffset FechaFinalizacion { get; private set; }

        public string Nombre { get; private set; }

        public void EstablecerNombre(string nombre)
        {
            Nombre = nombre;
        }

        public void EstablecerFechaFinalizacion(DateTimeOffset fecha)
        {
            FechaFinalizacion = fecha;
        }

        private readonly List<ItemProyecto> _items = new List<ItemProyecto>();
    
        public void AddItem(string nombreItem)
        {
            _items.Add(new ItemProyecto(nombreItem));
        }

        public IReadOnlyCollection<ItemProyecto> Items => _items;


        public override string ToString()
        {
            return $"Proyecto {Nombre} [{Identity}]";
        }


        public static Proyecto NewProyecto(ProyectoCode code)
        {
            var p = new Proyecto(code);

            p.AddItem("Item Inicial");

            //TODO:EVENTO
            return p;
        }

    }
}