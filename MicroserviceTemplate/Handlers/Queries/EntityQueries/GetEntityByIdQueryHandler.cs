// <copyright file="GetEntityByIdQueryHandler.cs" company="Frank Friedrich">
// Copyright (c) Firma Frank Friedrich. All rights reserved.
// </copyright>

namespace MicroserviceTemplate.Handlers.Queries.EntityQueries
{
    using MediatR;
    using MicroserviceTemplate.Data;
    using MicroserviceTemplate.Data.Models;
    using MicroserviceTemplate.Models.Entities;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// A query handler for retrieving an entity by its ID.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;MicroserviceTemplate.Models.Entities.GetEntityById, MicroserviceTemplate.Data.Models.DataBaseObjectEntity&gt;" />
    public class GetEntityByIdQueryHandler : IRequestHandler<GetEntityById, DataBaseObjectEntity?>
    {
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetEntityByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public GetEntityByIdQueryHandler(ApplicationDbContext dbContext)
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
        public async Task<DataBaseObjectEntity?> Handle(GetEntityById request, CancellationToken cancellationToken)
        {
            return await context.DataBaseObjectEntities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }
    }
}