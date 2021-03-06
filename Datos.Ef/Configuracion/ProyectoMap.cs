/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 29/07/2020 8:07:07
 *
 */

using Datos.Ef.Configuracion.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Modelo.Ef;

namespace Datos.Ef.Configuracion
{
    /// <summary>
    /// AutorConfiguration
    /// </summary>
    public class ProyectoMap : SurrogateEntityConfiguration<Proyecto, ProyectoCode>
    {

        public ProyectoMap() : base("PROJECT_DB","ProjectId")
        { }


        protected override void CustomConfigure(EntityTypeBuilder<Proyecto> builder)
        {


            NaturalId(b => 
            {

                b.Property(a => a.Identity)
                .HasColumnName("ProjectCode")
                .HasConversion(ConversionHelper.ProyectoCodeConverter);

                //b.Property(a => a.Aplicacion)
                //    .HasColumnName("ProjectApp");
            });


            builder.Property(a => a.Nombre)
                .HasColumnName("Name");


            builder.Property(p=> p.FechaFinalizacion)
                .HasColumnName("FechaFin")
                .HasConversion(ConversionHelper.DateTimeOffsetConverter);


            builder.Property(x => x.Items)
                   .HasField("_items");

            builder.Ignore(x => x.Items);

            builder.HasMany("_items")
                .WithOne()
                .HasForeignKey("ProjectId"); 

            /*
             * Hay que indicar explicitamente el nombre de la propiedad
             * ya que por defecto usará DBKey (ProyectoDBKey) que es el 
             * nombre de la propiedad clave en proyecto
             * Creara una propiedad Shadow ProjectId
             */



        }
    }
}