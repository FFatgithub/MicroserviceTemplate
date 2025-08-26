// <copyright file="Program.cs" company="Frank Friedrich">
// Copyright (c) Firma Frank Friedrich. All rights reserved.
// </copyright>

using MicroserviceTemplate.Data;
using MicroserviceTemplate.Services.Entity.Add;
using MicroserviceTemplate.Services.Entity.Get;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Application entry point.
/// </summary>
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // EF Core (nur Microsoft.EntityFrameworkCore verwenden; kein System.Data.Entity!)
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // MediatR
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        // Services
        builder.Services.AddScoped<IAddEntityService, AddEntityService>();
        builder.Services.AddScoped<IGetEntityService, GetEntityService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();
        app.Run();
    }
}