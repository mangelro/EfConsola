/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 29/07/2020 8:37:48
 *
 */

using EfConsola.Modelo;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfConsola.Datos.Configuracion
{
    /// <summary>
    /// BlogConfiguration
    /// </summary>
    public class BlogConfiguration : EntityConfiguration<Blog,int>
    {

        public BlogConfiguration() : base("BLOG_DB")
        { }

        protected  override void CustomConfigure(EntityTypeBuilder<Blog> builder)
        {
            
            builder.Property(x => x.Identity)
                .HasColumnName("Blogid")
                .IsRequired()
                .ValueGeneratedOnAdd();


            builder.Property(x => x.Url)
                .HasColumnName("Url")
                .HasMaxLength(255)
                .IsRequired();


            builder.Property(x => x.AutorId)
                .HasColumnName("AuthorId")
                .HasConversion(ConversionHelper.AutorIDConverter)
                .IsRequired();


            builder.Property(x => x.Posts)
                .HasField("_posts");

            /*
             * Configura la relacion sin especificar propiedades de navegacion
             */

            builder.HasOne<Autor>()    // <---
                .WithMany()       // <---
                .HasForeignKey(c => c.AutorId);


            builder.Ignore(x => x.Posts);


            builder.HasMany("_posts")
                .WithOne();

        }
    }
}