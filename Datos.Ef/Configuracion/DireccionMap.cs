/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 30/07/2020 12:00:13
 *
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Modelo.Ef;

namespace Datos.Ef.Configuracion
{
    /// <summary>
    /// DireccionConfiguration
    /// </summary>
    public class DireccionMap
    {


        public void ConfigureComplexType<TEntity>(OwnedNavigationBuilder<TEntity, Direccion> builder) where TEntity : class
        {

            builder.Property(a => a.Calle)
                .HasColumnName("Street");

            builder.Property(a => a.Poblacion)
                .HasColumnName("Location");

            builder.Property(a => a.CodigoPostal)
                .HasColumnName("ZipCode");

            builder.Property(a => a.Provincia)
                .HasColumnName("Province");

            builder.Property(a => a.Pais)
                .HasColumnName("Country")
                .HasAnnotation("charset", "UTF-16");

        }

    }
}