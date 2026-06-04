using LayeredArchitecture_Task2_Catalog_Service.Business;
using LayeredArchitecture_Task2_Catalog_Service.MessageQueue;
using LayeredArchitecture_Task2_Catalog_Service.MessageQueue.Interfaces;
using LayeredArchitecture_Task2_Catalog_Service.Middlewares;
using LayeredArchitecture_Task2_Catalog_Service.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddBusinessServices();
builder.Services.AddRepositoryServices(builder.Configuration);

builder.Services.Configure<RabbitMQOptions>(builder.Configuration.GetSection(RabbitMQOptions.SectionName));
builder.Services.AddMessageQueue();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Catalog Service API",
        Description = "REST API for managing catalog categories and products."
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter token: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:8080/realms/store-realm";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateAudience = false // for Keycloak
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var identity = context.Principal.Identity as ClaimsIdentity;

                var realmAccess = context.Principal.FindFirst("realm_access")?.Value;

                if (realmAccess != null)
                {
                    var roles = JsonDocument.Parse(realmAccess)
                        .RootElement
                        .GetProperty("roles");

                    foreach (var role in roles.EnumerateArray())
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, role.GetString()));
                    }
                }

                return Task.CompletedTask;
            }
        };

    });
var app = builder.Build();

// Force RabbitMQ connection and topology creation at startup
app.Services.GetRequiredService<IMessagePublisher>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog Service API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<TokenLoggingMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
