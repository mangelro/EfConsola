/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 25/09/2020 12:59:42
 *
 */

using System;


namespace Datos.Ef
{

    /// <summary>
    /// Multi-tenant es una arquitectura de software que permite a una sola instancia 
    /// de la aplicación pero no se personaliza el código como tal.
    /// </summary>
    public interface ITenantProvider
    {
        Guid ApplicationId { get; }
    }

    /// <summary>
    /// TenantProvider
    /// </summary>
    public class TenantProvider : ITenantProvider
    {
        public Guid ApplicationId => Guid.Parse("96F59479-9FAC-4E9A-B505-FA5142334D42");
        //public Guid ApplicationId => Guid.NewGuid();
    }
}