/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 25/09/2020 9:43:20
 *
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Modelo.Ef;

namespace Datos.Ef.Configuracion
{
    /// <summary>
    /// ItemBlogMap
    /// </summary>
    public class ItemProyectoMap : EntityMap<ItemProyecto, int>
    {
        public ItemProyectoMap() : base("ITEM_PROJECT_DB")
        { }



        protected override void CustomConfigure(EntityTypeBuilder<ItemProyecto> builder)
        {
            builder.Property(x => x.Identity)
           .HasColumnName("ItemId")
           .IsRequired()
           .ValueGeneratedOnAdd();



            builder.Property(x => x.Nombre)
           .HasColumnName("ItemName");
        }
    }
}