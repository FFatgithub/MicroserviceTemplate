// <copyright file="EntityController.cs" company="Frank Friedrich">
// Copyright (c) Firma Frank Friedrich. All rights reserved.
// </copyright>

namespace MikroserviceTemplate.Controllers
{
    using System;
    using MicroserviceTemplate.Models.Entities;
    using MicroserviceTemplate.Services.Entity.Add;
    using MicroserviceTemplate.Services.Entity.Get;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// REST-API für Entitäten.
    /// </summary>
    [ApiController]
    [Route("api/entities")]
    [Produces("application/json")]
    public class EntityController : ControllerBase
    {
        private readonly IGetEntityService getService;
        private readonly IAddEntityService addService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityController"/> class.
        /// </summary>
        /// <param name="getEntityService">The get entity service.</param>
        /// <param name="addEntityService">The add entity service.</param>
        public EntityController(IGetEntityService getEntityService,
            IAddEntityService addEntityService)
        {
            getService = getEntityService;
            addService = addEntityService;
        }

        /// <summary>
        /// Gets the entity by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns DataBaseObjectEntity.</returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEntityByIdAsync(Guid id)
        {
            var query = new GetEntityById { Id = id };
            var entity = await getService.GetEntityByIdAsync(query);
            if (entity is null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        /// <summary>
        /// Adds the entity asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Returns the Guid.</returns>
        [HttpPost]
        public async Task<IActionResult> AddEntityAsync([FromBody] AddEntity request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var newId = await addService.AddEntityAsync(request);
            return CreatedAtAction(nameof(GetEntityByIdAsync), new { id = newId }, newId);
        }
    }
}