/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 29/07/2020 14:35:53
 *
 */

using System;

using FundacionOlivar.DDD.SharedKernel;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Ef.Configuracion
{
    /// <summary>
    /// EntityConfiguration
    /// </summary>
    public abstract class EntityMap<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {

        private readonly string _tableName;

        protected EntityMap(string tableName)
        {
            _tableName = tableName;
        }

        void IEntityTypeConfiguration<TEntity>.Configure(EntityTypeBuilder<TEntity> builder)
        {

            builder.ToTable(_tableName)
                .HasKey(e => e.Identity);


            CustomConfigure(builder);
        }

        protected abstract void CustomConfigure(EntityTypeBuilder<TEntity> builder);

    }
}