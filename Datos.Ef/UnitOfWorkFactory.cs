/*
 * Copyright © 2020 Fundación del Olivar
 * Todos los derechos reservados
 * Autor: Miguel A. Romera
 * Fecha: 23/08/2020 11:49:01
 */

using FundacionOlivar.DDD.SharedKernel;

namespace Datos.Ef
{
    /// <summary>
    /// Puede ser util para tener multiples instancias de UoW para ejecutar en distintas Task, 
    /// debido a las limitaciones de paralelismo de DbConext.
    /// Para ello, si utlizamos DI debería ulizarse un alcance (scope) Transient
    /// </summary>
    public delegate IUnitOfWork UnitOfWorkFactory();

}