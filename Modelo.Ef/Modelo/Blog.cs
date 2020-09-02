/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 28/07/2020 9:44:09
 *
 */

using System;
using System.Collections.Generic;

using FundacionOlivar.DDD.Core;

using Modelo.Ef.Eventos;

namespace Modelo.Ef
{
    /// <summary>
    /// Blog
    /// </summary>
    /// 
    public class Blog : AggregateRoot<int>, IAuditable
    {

        protected Blog()
        {
        }

        private readonly List<Post> _posts = new List<Post>();

        public string Url { get; private set; }


        /// <summary>
        /// Clave foranea de Autor
        /// </summary>
        public Guid PublicadorId { get; private set; }

        /// <summary>
        /// Propiedad de navegacion
        /// </summary>
        public Autor Publicador { get; private set; }

        public void EstablecerUrl(string url)
        {
            Url = url;
        }

        public void EstablecerAutor(Autor autor)
        {
           Publicador = autor;
           PublicadorId= autor.Identity;
        }

        public Post AddPost(string titulo, string contenido)
        {



            var post = new Post();

            post.EstablecerTitulo(titulo);
            post.EstablecerContenido(contenido);

            _posts.Add(post);

            AddDomainEvent(new PostAddedEvent(post));

            return post;
        }

        public IReadOnlyList<Post> Posts => _posts;


        public void ClearPost()
        {
            _posts.Clear();
        }

        public override string ToString()
        {
            return $"{Url} [{Identity}] - {Publicador}";
        }

        public static Blog NewBlog()
        {
            var b = new Blog();
            b.AddDomainEvent(new BlogCreadoEvent(b));
            return b;
        }
    }
}