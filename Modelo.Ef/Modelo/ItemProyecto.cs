/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 25/09/2020 9:39:29
 *
 */

using System.Data.Common;
using System.Runtime.CompilerServices;

using FundacionOlivar.DDD.Core;


namespace Modelo.Ef
{
    /// <summary>
    /// ItemBlog
    /// </summary>
    /// 
    public class ItemProyecto : Entity<ItemProyecto, int>
    {

        protected ItemProyecto() { }

        public ItemProyecto(string nombre)
        {
            Nombre = nombre;
        }

        public string Nombre { get; private set; }
    }
}