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
using FundacionOlivar.DDD.SharedKernel;

namespace Modelo.Ef
{
    /// <summary>
    /// Proyecto
    /// </summary>
    public class Proyecto : SurrogateEntity<ProyectoCode>,IAuditable
    {

        protected Proyecto() { }

        protected Proyecto(ProyectoCode code, Guid app)
        {
            Identity = code;
            Aplicacion = app;
        }


        public Guid Aplicacion { get; }

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





        public static Proyecto NewProyecto(ProyectoCode code, Guid app)
        {
            var p = new Proyecto(code, app);
            //TODO:EVENTO
            return p;
        }

    }
}