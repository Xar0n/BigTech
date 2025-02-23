using BigTech.Api;
using BigTech.Api.Middlewares;
using BigTech.Application.DependencyInjection;
using BigTech.DAL.DependencyInjection;
using BigTech.Domain.Settings;
using BigTech.Consumer.DependencyInjection;
using BigTech.Producer.DependencyInjection;
using Serilog;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultSection));
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection(RabbitMqSettings.DefaultSection));
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection(RedisSettings.DefaultSection));

builder.Services.AddControllers();
builder.Services.AddAuthentificationAndAuthorization(builder);
builder.Services.AddSwagger(builder.Configuration);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDataAcessLayer(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddProducer();
builder.Services.AddConsumer();

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

app.MapGet("users/me", (ClaimsPrincipal cp) =>
{
    return cp.Claims.ToDictionary(c => c.Type, c => c.Value);
}).RequireAuthorization();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();