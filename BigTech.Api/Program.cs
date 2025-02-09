using BigTech.Api;
using BigTech.Api.Middlewares;
using BigTech.Application.DependencyInjection;
using BigTech.DAL.DependencyInjection;
using BigTech.Domain.Settings;
using Microsoft.AspNetCore.HttpsPolicy;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultSection));

builder.Services.AddControllers();
builder.Services.AddAuthentificationAndAuthorization(builder);
builder.Services.AddSwagger();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDataAcessLayer(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("swagger/v1/swagger.json", "BigTech Swagger v1.0");
        o.SwaggerEndpoint("swagger/v2/swagger.json", "BigTech Swagger v2.0");
        o.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.

app.UseCors(c =>
{
    c.AllowAnyOrigin();
    c.AllowAnyMethod();
    c.AllowAnyHeader();
});

app.UseHttpsRedirection();
app.MapControllers();
app.Run();