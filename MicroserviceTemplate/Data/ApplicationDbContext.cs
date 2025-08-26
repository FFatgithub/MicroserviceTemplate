// <copyright file="ApplicationDbContext.cs" company="Frank Friedrich">
// Copyright (c) Firma Frank Friedrich. All rights reserved.
// </copyright>

namespace MicroserviceTemplate.Data
{
    using MicroserviceTemplate.Data.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    ///  Application database context for Entity Framework Core,
    ///  managing the connection to the database and providing access to entity sets.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets the data base object entities.
        /// </summary>
        /// <value>
        /// The data base object entities.
        /// </value>
        public DbSet<DataBaseObjectEntity> DataBaseObjectEntities => Set<DataBaseObjectEntity>();

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// <para>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run. However, it will still run when creating a compiled model.
        /// </para>
        /// <para>
        /// See <see href="https://aka.ms/efcore-docs-modeling">Modeling entity types and relationships</see> for more information and
        /// examples.
        /// </para>
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Warum: klare Constraints & Mapping.
            var entity = modelBuilder.Entity<DataBaseObjectEntity>();
            entity.ToTable("Entities");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Description).HasMaxLength(2000);
            entity.Property(x => x.CreatedAtUtc).IsRequired();
            entity.Property(x => x.UpdatedAtUtc);
        }
    }
}