/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 29/07/2020 12:46:45
 *
 */

using EfConsola.Modelo;

using Microsoft.EntityFrameworkCore;

namespace EfConsola.Datos.Repositorios
{
    /// <summary>
    /// EfAutorRepository
    /// </summary>
    public class EfAutorRepository : EfRepositoryBase<Autor, AutorID>, IAutorRepository
    {

        public EfAutorRepository(DbSet<Autor> store) : base(store)
        { }

    }
}