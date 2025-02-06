using BigTech.Api;
using BigTech.Application.DependencyInjection;
using BigTech.DAL.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.MapControllers();
app.Run();