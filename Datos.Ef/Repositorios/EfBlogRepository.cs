/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 29/07/2020 12:46:45
 *
 */

using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using Modelo.Ef;

namespace Datos.Ef.Repositorios
{
    /// <summary>
    /// EfAutorRepository
    /// </summary>
    public class EfBlogRepository : EfRepositoryBase<Blog, int>, IBlogRepository
    {



        public EfBlogRepository(DbContext uow) : base(uow)
        { }

        public override Blog GetById(int id)
        {
            return Set.Where(b => b.Identity == id)
                .Include(b=>b.Publicador) //Eager loading
                .FirstOrDefault();
        }

        /// <summary>
        /// Realiza la carga explicita de los Post cuando no se quiere sobrecargar la 
        /// búsqueda por ID
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        public void LoadPosts(Blog blog)
        {
            var entry = Context.Entry(blog);

            if (entry.State == EntityState.Detached)
                throw new Exception();

            /*
             * Explicit loading: Nos permite cargar las colecciones cuando sean 
             * absolutamente necesarias. De esta manera, evitamos la sobrecarga
             * de Eager Loading : Include(..).
             */
            entry.Collection("_posts").Load();

            //return blog;

        }
    }
}