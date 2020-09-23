/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 29/07/2020 8:07:07
 *
 */

using System;

using Datos.Ef.Configuracion.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Modelo.Ef;

namespace Datos.Ef.Configuracion
{
    /// <summary>
    /// AutorConfiguration
    /// </summary>
    public class AutorMap : EntityMap<Autor, Guid>
    {
        public AutorMap() : base("AUTHOR_DB")
        { }

        protected override void CustomConfigure(EntityTypeBuilder<Autor> builder)
        {
            builder.Property(a => a.Identity)
                .HasColumnName("AuthorId")
                .HasConversion(ConversionHelper.GuidStringConverter);


            builder.Property(a => a.Nombre)
                .HasColumnName("Name")
                .HasMaxLength(256)
                .IsRequired()
                .IsUnicode();


            builder.Property("_fechaNacimiento")
                .HasColumnName("DateOfBirth")
                .HasConversion(ConversionHelper.DateTimeOffsetConverter);



            builder.Ignore(a => a.Edad);

            /*
             * SQLite no admite HasComputedColumnSql
             * 
             */

            //builder.Property(a => a.Edad)
            //    .ValueGeneratedOnAddOrUpdate()
            //    .HasComputedColumnSql("strftime('%Y', [DateOfBirth]))");


            //builder.OwnsOne(
            //        x => x.Direccion,
            //        address =>
            //        {
            //            address.Property(a => a.Calle).HasColumnName("Street");
            //            address.Property(a => a.Poblacion).HasColumnName("Location");
            //            address.Property(a => a.CodigoPostal).HasColumnName("ZipCode");
            //            address.Property(a => a.Provincia).HasColumnName("Province");
            //            address.Property(a => a.Pais).HasColumnName("Country");
            //        });

            var b = new DireccionMap();
            builder.OwnsOne(x => x.Direccion, address => b.ConfigureComplexType(address));
        }
    }
}