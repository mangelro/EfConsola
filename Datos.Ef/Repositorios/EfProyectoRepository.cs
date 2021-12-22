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

using Datos.Ef.Excepciones;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using Modelo.Ef;

namespace Datos.Ef.Repositorios
{
    /// <summary>
    /// EfAutorRepository
    /// </summary>
    public class EfProyectoRepository : EfRepositoryBase<Proyecto, ProyectoCode>, IProyectoRepository
    {
        public EfProyectoRepository(EfUoW uow) : base(uow)
        { }

        public override Proyecto GetById(ProyectoCode id)
        {
            return Set.Where(b => b.Identity == id)
                .Include("_items") //Eager loading
                .FirstOrDefault();

        }
    }
}