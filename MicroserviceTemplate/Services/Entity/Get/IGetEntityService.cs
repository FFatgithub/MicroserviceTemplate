// <copyright file="IGetEntityService.cs" company="Frank Friedrich">
// Copyright (c) Firma Frank Friedrich. All rights reserved.
// </copyright>

namespace MicroserviceTemplate.Services.Entity.Get
{
    using MicroserviceTemplate.Data.Models;
    using MicroserviceTemplate.Models.Entities;

    /// <summary>
    ///   A service interface for handling CRUD operations for entities of type>.
    /// </summary>
    /// <typeparam name="TDataBaseObjectEntity">The type of the entity.</typeparam>
    public interface IGetEntityService
    {
        /// <summary>
        /// Gets the entity by identifier.
        /// </summary>
        /// <param name="entityById">The entity by identifier.</param>
        /// <returns>
        /// Returns the entity: <see cref="TEntity" />.
        /// </returns>
        Task<DataBaseObjectEntity?> GetEntityByIdAsync(GetEntityById entityById);
    }
}