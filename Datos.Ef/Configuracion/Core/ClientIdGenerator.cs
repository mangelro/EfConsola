/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 11/09/2020 13:58:03
 *
 */

using System;
using System.Collections.Generic;

using System.Text;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

using Modelo.Ef;

namespace Datos.Ef.Configuracion.Core
{
    /// <summary>
    /// ClientIdGenerator
    /// </summary>
    public class ClientIdGenerator : ValueGenerator<Guid>
    {
        public override bool GeneratesTemporaryValues => false;

        public override Guid Next(EntityEntry entry)
        {
            //var name = entry.Property(nameof(Autor.GetType)).CurrentValue;
            //var uniqueNum = DateTime.UtcNow.Ticks;
            //return $"{name}-{uniqueNum}";
            return Guid.NewGuid();
        }
    }
}