// <copyright file="IAddEntityService.cs" company="Frank Friedrich">
// Copyright (c) Firma Frank Friedrich. All rights reserved.
// </copyright>

namespace MicroserviceTemplate.Services.Entity.Add
{
    using MicroserviceTemplate.Models.Entities;

    /// <summary>
    /// A service interface for adding entities of type <typeparamref name="Guid"/>.
    /// </summary>
    /// <typeparam name="TGuid">The type of the uid.</typeparam>
    public interface IAddEntityService
    {
        /// <summary>
        /// Adds the entity asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Returns the entity.</returns>
        Task<Guid> AddEntityAsync(AddEntity entity);
    }
}