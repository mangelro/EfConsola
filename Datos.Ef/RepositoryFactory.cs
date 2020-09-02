/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 31/07/2020 11:48:20
 *
 */

using System;

namespace Datos.Ef
{

    public delegate object RepositoryFactory(Type serviceType);

    public static class RepositoryFactoryExtensions
    {
        public static TRepository GetInstance<TRepository>(this RepositoryFactory factory)
            => (TRepository)factory(typeof(TRepository));


        //    //public static IEnumerable<T> GetInstances<T>(this RepositoryFactory factory)
        //    //    => (IEnumerable<T>)factory(typeof(IEnumerable<T>));

    }

}
