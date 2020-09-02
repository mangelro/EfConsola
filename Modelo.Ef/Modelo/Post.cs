/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 28/07/2020 9:44:32
 *
 */

using System;

using FundacionOlivar.DDD.Core;

namespace Modelo.Ef
{
    /// <summary>
    /// Post
    /// </summary>
    public class Post : BaseEntity<int>
    {

        protected internal Post() { }

        private bool _activo = true;

        public string Titulo { get; private set; }

        public string Contenido { get; private set; }

        public Valoracion Valoracion { get; private set; } = Valoracion.FromInteger(0);


        /// <summary>
        /// soft delete
        /// </summary>
        public bool Activo => _activo;

        public int BlogId { get; }

        public Blog Blog { get; }



        public void EstablecerTitulo(string titulo) => Titulo = titulo;

        public void EstablecerContenido(string contenido) => Contenido = contenido;


        public void Activar() => _activo = true;
        public void Desactivar() => _activo = false;


        public void EstablecerValoracion(Valoracion valoracion)
        {

            if (!Activo)
                throw new Exception("El post se encuentra desactivado");

            Valoracion = valoracion;
        }



    }
}