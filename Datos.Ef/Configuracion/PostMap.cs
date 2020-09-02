/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 29/07/2020 8:39:40
 *
 */

using Datos.Ef.Configuracion.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Modelo.Ef;

namespace Datos.Ef.Configuracion
{
    /// <summary>
    /// PostConfiguration
    /// </summary>
    public class PostMap : EntityMap<Post, int>
    {


        public PostMap() : base("POST_DB")
        { }

        protected override void CustomConfigure(EntityTypeBuilder<Post> builder)
        {

            builder.Property(x => x.Identity)
             .HasColumnName("PostId")
             .IsRequired()
             .ValueGeneratedOnAdd();


            /*
             * Solo me interesan aquellos que están activos
             * cuando se incluyen en las colecciones
             * Soft delete
             */
            builder.HasQueryFilter(x => (x.Activo));

            builder.Property(x => x.Titulo)
                .HasColumnName("Title")
                .IsRequired();

            builder.Property(x => x.Contenido)
                .HasColumnName("Content")
                .IsRequired();


            builder.Property(x => x.BlogId)
                .HasColumnName("BlogId")
                .IsRequired();


            builder.Property(x => x.Activo)
                .HasField("_activo")
                .UsePropertyAccessMode( PropertyAccessMode.Field)
                .HasConversion<bool>();


            builder.Property(x => x.Valoracion)
                .HasConversion(ConversionHelper.ValoracionConverter);
        }
    }
}