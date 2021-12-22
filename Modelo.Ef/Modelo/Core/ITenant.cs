/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 25/09/2020 13:15:06
 *
 */

using System;
using System.Collections.Generic;

using System.Text;


namespace Modelo.Ef.Core
{
    /// <summary>
    /// Determina la identidad de la aplicación
    /// </summary>
    public interface ITenant
    {
        //Solo para marcado Ef crea propiedades Shadows
    }


    public static class TenantField
    {
        public const string TENANT_FIELD = "ApplicationId";
    }
}