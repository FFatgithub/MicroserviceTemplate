// <copyright file="GetEntityService.cs" company="Frank Friedrich">
// Copyright (c) Firma Frank Friedrich. All rights reserved.
// </copyright>

namespace MicroserviceTemplate.Services.Entity.Get
{
    using System.Threading.Tasks;
    using Data.Models;
    using MediatR;
    using MicroserviceTemplate.Models.Entities;

    /// <summary>
    /// A service for getting entities of type <see cref="DataBaseObjectEntity"/>.
    /// </summary>
    /// <seealso cref="MicroserviceTemplate.Services.Entity.Get.IGetEntityService" />
    public class GetEntityService : IGetEntityService
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetEntityService"/> class.
        /// </summary>
        /// <param name="mediatorinstance">The mediatorinstance.</param>
        public GetEntityService(IMediator mediatorinstance)
        {
            mediator = mediatorinstance;
        }

        /// <summary>
        /// Gets the entity by identifier.
        /// </summary>
        /// <param name="entityById">The entity by identifier.</param>
        /// <returns>
        /// Returns the entity: <see cref="TEntity" />.
        /// </returns>
        public Task<DataBaseObjectEntity?> GetEntityByIdAsync(GetEntityById entityById) => mediator.Send(entityById);
    }
}