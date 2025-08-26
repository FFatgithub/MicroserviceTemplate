// <copyright file="GetEntityById.cs" company="Frank Friedrich">
// Copyright (c) Firma Frank Friedrich. All rights reserved.
// </copyright>

namespace MicroserviceTemplate.Models.Entities
{
    using MediatR;
    using MicroserviceTemplate.Data.Models;

    /// <summary>
    /// A class representing an entity with properties for article number, purchase date, and color.
    /// </summary>
    public class GetEntityById : IRequest<DataBaseObjectEntity?>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }
    }
}