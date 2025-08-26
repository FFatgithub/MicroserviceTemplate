// <copyright file="DataBaseObjectEntity.cs" company="Frank Friedrich">
// Copyright (c) Firma Frank Friedrich. All rights reserved.
// </copyright>

namespace MicroserviceTemplate.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Just a dummy class to have a folder for database related classes.
    /// The original class will be in the real environment in the database.
    /// </summary>
    public class DataBaseObjectEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [MaxLength(2000)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the created at UTC.
        /// </summary>
        /// <value>
        /// The created at UTC.
        /// </value>
        public DateTime CreatedAtUtc { get; set; }

        /// <summary>
        /// Gets or sets the updated at UTC.
        /// </summary>
        /// <value>
        /// The updated at UTC.
        /// </value>
        public DateTime? UpdatedAtUtc { get; set; }
    }
}