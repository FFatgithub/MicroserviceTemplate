// <copyright file="AddEntityCommandHandler.cs" company="Frank Friedrich">
// Copyright (c) Firma Frank Friedrich. All rights reserved.
// </copyright>

namespace MicroserviceTemplate.Handlers.Commands.EntityCommands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using MicroserviceTemplate.Data;
    using MicroserviceTemplate.Data.Models;
    using MicroserviceTemplate.Models.Entities;

    /// <summary>
    /// A command handler for creating a new entity.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;CreateEntity, System.Guid&gt;" />
    public class AddEntityCommandHandler : IRequestHandler<AddEntity, Guid>
    {
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddEntityCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The context.</param>
        public AddEntityCommandHandler(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        /// <summary>
        /// Handles a request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// Response from the request.
        /// </returns>
        public async Task<Guid> Handle(AddEntity request, CancellationToken cancellationToken)
        {
            var entity = new DataBaseObjectEntity
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                CreatedAtUtc = DateTime.UtcNow,
            };

            await context.DataBaseObjectEntities.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}