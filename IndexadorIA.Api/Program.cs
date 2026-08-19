using System.Text;
using IndexadorIA.Api.Middleware;
using IndexadorIA.Api.Servicios;
using IndexadorIA.Datos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Se toma la cadena de conexión de appsettings.json y se la asigna a la
// clase estática Configuracion usada por todas las capas de IndexadorIA.Core.
string? cadenaConexion = builder.Configuration.GetConnectionString("IndexadorIA");
if (!string.IsNullOrWhiteSpace(cadenaConexion))
{
    Configuracion.CadenaConexion = cadenaConexion;
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new OpenApiInfo { Title = "IndexadorIA API", Version = "v1" });

    opciones.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Name = "X-Api-Key",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "API Key requerida en el header X-Api-Key"
    });

    opciones.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Token JWT obtenido desde /api/auth/login"
    });

    opciones.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" } },
            Array.Empty<string>()
        },
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddSingleton<JwtServicio>();

string claveSecretaJwt = builder.Configuration["SeguridadApi:Jwt:ClaveSecreta"]
    ?? throw new InvalidOperationException("Falta configurar SeguridadApi:Jwt:ClaveSecreta");

builder.Services.AddAuthentication(opciones =>
{
    opciones.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opciones.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opciones =>
{
    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["SeguridadApi:Jwt:Emisor"],
        ValidAudience = builder.Configuration["SeguridadApi:Jwt:Audiencia"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveSecretaJwt))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseApiKey();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
