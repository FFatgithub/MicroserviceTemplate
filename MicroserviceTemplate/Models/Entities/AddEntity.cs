// <copyright file="AddEntity.cs" company="Frank Friedrich">
// Copyright (c) Firma Frank Friedrich. All rights reserved.
// </copyright>

namespace MicroserviceTemplate.Models.Entities
{
    using System.ComponentModel.DataAnnotations;
    using MediatR;

    /// <summary>
    /// A command to create a new entity with specified properties.
    /// </summary>
    public class AddEntity : IRequest<Guid>
    {
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
    }
}