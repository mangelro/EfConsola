/*
 * Copyright ©2020 Fundación del Olivar
 * Todos los derechos reservados
 *
 * Autor: Miguel A. Romera  - miguel
 * Fecha: 29/07/2020 14:35:53
 *
 */

using System;

using FundacionOlivar.DDD.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Ef.Configuracion.Core
{
    /// <summary>
    /// EntityConfiguration
    /// </summary>
    public abstract class SurrogateEntityConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : SurrogateEntity<TEntity, TKey>
        where TKey : IEquatable<TKey>
    {

        private readonly string _tableName;
        private readonly string _keyColumnName;

        private EntityTypeBuilder<TEntity> _builder;

        protected SurrogateEntityConfiguration(string tableName, string keyColumnName)
        {
            _tableName = tableName;
            _keyColumnName = keyColumnName;
        }

        void IEntityTypeConfiguration<TEntity>.Configure(EntityTypeBuilder<TEntity> builder)
        {

            _builder = builder;

            _builder.ToTable(_tableName)
                .HasKey(e => e.DBKey);

            _builder.Property(e => e.DBKey)
                .HasColumnName(_keyColumnName)
                .ValueGeneratedOnAdd();

            CustomConfigure(_builder);

        }

        protected abstract void CustomConfigure(EntityTypeBuilder<TEntity> builder);


        protected void NaturalId(Action<EntityTypeBuilder<TEntity>> action)
        {
            action(_builder);
        }
    }
}