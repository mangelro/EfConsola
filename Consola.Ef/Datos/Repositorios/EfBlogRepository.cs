/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 29/07/2020 12:46:45
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EfConsola.Modelo;

using Microsoft.EntityFrameworkCore;

namespace EfConsola.Datos.Repositorios
{
    /// <summary>
    /// EfAutorRepository
    /// </summary>
    public class EfBlogRepository :EfRepositoryBase<Blog,int>, IBlogRepository
    {
        


        public EfBlogRepository(DbSet<Blog> store):base(store)
        {}

        public override Blog Find(int id)
        {
            return _store
                .Where(b=>b.Identity==id)
                .Include("_posts")
                .FirstOrDefault();
        }

    }
}