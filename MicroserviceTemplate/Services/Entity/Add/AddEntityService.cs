// <copyright file="AddEntityService.cs" company="Frank Friedrich">
// Copyright (c) Firma Frank Friedrich. All rights reserved.
// </copyright>

namespace MicroserviceTemplate.Services.Entity.Add
{
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using MicroserviceTemplate.Models.Entities;

    /// <summary>
    /// A service for adding entities of type <see cref="Guid"/>.
    /// </summary>
    /// <seealso cref="MicroserviceTemplate.Services.Entity.Add.IAddEntityService" />
    public class AddEntityService : IAddEntityService
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddEntityService"/> class.
        /// </summary>
        /// <param name="mediatorInstance">The mediator.</param>
        public AddEntityService(IMediator mediatorInstance)
        {
            mediator = mediatorInstance;
        }

        /// <summary>
        /// Adds the entity asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// Returns the entity.
        /// </returns>
        public Task<Guid> AddEntityAsync(AddEntity entity) => mediator.Send(entity);
    }
}