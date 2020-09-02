/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 29/07/2020 8:39:40
 *
 */

using EfConsola.Modelo;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfConsola.Datos.Configuracion
{
    /// <summary>
    /// PostConfiguration
    /// </summary>
    public class PostConfiguration : EntityConfiguration<Post,int>
    {


        public PostConfiguration() : base("POST_DB")
        { }

        protected override void CustomConfigure(EntityTypeBuilder<Post> builder)
        {

            builder.Property(x => x.Identity)
             .HasColumnName("PostId")
             .IsRequired()
             .ValueGeneratedOnAdd();


            //Solo me interesan aquellos que están activos
            //cuando se incluyen en las colecciones
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
                .HasConversion<bool>();


            builder.Property(x => x.Valoracion)
                .HasConversion(ConversionHelper.ValoracionConverter);
        }
    }
}