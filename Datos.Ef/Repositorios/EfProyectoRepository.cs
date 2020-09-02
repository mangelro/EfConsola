/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 29/07/2020 12:46:45
 *
 */

using FundacionOlivar.DDD.SharedKernel;

using Microsoft.EntityFrameworkCore;

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
    }
}