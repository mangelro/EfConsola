/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 31/07/2020 12:08:14
 *
 */

using System;
using System.Collections.Generic;

using System.Text;


namespace Datos.Ef
{
    /// <summary>
    /// IoWConfig
    /// </summary>
    public interface IUoWConfig
    {
        string ConnectionString {get;}
       
    }


    public class UoWconfig : IUoWConfig
    {
        public string ConnectionString => @"Data Source=F:\Visual Studio 2019\Proyectos\EfConsola\Consola.Ef\AppData\modelDB.db ";

    }

}