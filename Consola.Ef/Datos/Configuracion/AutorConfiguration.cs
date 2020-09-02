/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 29/07/2020 8:07:07
 *
 */

using System;

using EfConsola.Modelo;

using FundacionOlivar.Datos.Ef.Configuracion;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfConsola.Datos.Configuracion
{
    /// <summary>
    /// AutorConfiguration
    /// </summary>
    public class AutorConfiguration : EntityConfiguration<Autor, AutorID> 
    {

        public AutorConfiguration() : base("AUTHOR_DB")
        { }

   
        protected override void CustomConfigure(EntityTypeBuilder<Autor> builder)
        {

            builder.Property(a => a.Identity)
                .HasColumnName("AuthorId")
                .HasConversion(ConversionHelper.AutorIDConverter)
                .ValueGeneratedNever();

            builder.Property(a => a.Nombre)
                .HasColumnName("Name");




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


            var b = new DireccionConfiguration();
            builder.OwnsOne(x => x.Direccion, address => b.ConfigureComplexType(address));


        }
    }
}