using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using ProductManagementAPI.Data;
using ProductManagementAPI.Middleware;
using ProductManagementAPI.Models;
using ProductManagementAPI.Services;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);


var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://+:{port}");

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .SelectMany(e => e.Value!.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return new BadRequestObjectResult(
                ApiResponse<object>.FailResult(errors));
        };
    });
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));
builder.Services.AddScoped<
    IProductService,
    ProductService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product Management API",
        Version = "v1",
        Description = "RESTful API for managing products using ASP.NET Core and Entity Framework Core."
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});
var app = builder.Build();

// Global exception handler must be first in the pipeline.
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swagger available in all environments — required for production deployments on Render.
app.UseSwagger();
app.UseSwaggerUI(options =>
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Management API v1"));

// HTTPS redirection applies locally only.
// On Render, TLS is terminated at the edge proxy; the app receives plain HTTP.
// Enabling this unconditionally causes a redirect loop in production.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();