/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 29/07/2020 8:37:48
 *
 */

using Datos.Ef.Configuracion.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Modelo.Ef;

namespace Datos.Ef.Configuracion
{
    /// <summary>
    /// BlogConfiguration
    /// </summary>
    public class BlogMap : EntityMap<Blog, int>
    {

        public BlogMap() : base("BLOG_DB")
        { }

        protected override void CustomConfigure(EntityTypeBuilder<Blog> builder)
        {

            builder.Property(x => x.Identity)
                .HasColumnName("BlogId")
                .IsRequired()
                .ValueGeneratedOnAdd();


            builder.Property(x => x.Url)
                .HasColumnName("Url")
                .HasMaxLength(255)
                .IsRequired();

            /*
             * Clave foranea de Autor.
             * Hay que declararla explícitamente ya que no sigue las convenciones de clave <class_name>Id sino que en 
             * la clase padre (Autor) la clave es la propiedad Identity
             */
            builder.Property(x => x.PublicadorId)
                .HasColumnName("AuthorId")
                .HasConversion(ConversionHelper.GuidStringConverter)
                .IsRequired();


            builder.Property(x => x.Posts)
                .HasField("_posts");

            /*
             * Configura la propiedades de navegacion
             */

            builder.HasOne(b => b.Publicador)
                .WithOne()
                .HasPrincipalKey<Blog>(a => a.PublicadorId);


            builder.Ignore(x => x.Posts);

            builder.HasMany("_posts")
                .WithOne();

        }
    }
}