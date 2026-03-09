using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Diagnostics;
using System.Text;
using WsElecciones.Api.Endpoints;
using WsElecciones.Api.Extensions;  
using WsElecciones.Api.Middleware;
using WsElecciones.Application;
using WsElecciones.Application.Features;
using WsElecciones.CrossCutting;
using WsElecciones.CrossCutting.Security;
using WsElecciones.CrossCutting.Storage;
using WsElecciones.Domain.Interface;
using WsElecciones.Persistence;
using WsElecciones.Persistence.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.AddLogging();

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key no configurada.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero 
        };

        // Devuelve 401 con cuerpo JSON en lugar de respuesta vacía
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async ctx =>
            {
                ctx.HandleResponse();
                ctx.Response.StatusCode = 401;
                ctx.Response.ContentType = "application/json";
                await ctx.Response.WriteAsJsonAsync(new { success = false, message = "No autorizado por el sistema de Elecciones." });
            },
            OnForbidden = async ctx =>
            {
                ctx.Response.StatusCode = 403;
                ctx.Response.ContentType = "application/json";
                await ctx.Response.WriteAsJsonAsync(new { success = false, message = "Acceso denegado por el sistema de Elecciones." });
            }
        };
    });


// ── AUTORIZACIÓN CON POLÍTICAS POR ROL ──────────────────────────────────────

builder.Services.AddAuthorization(options =>
{
    // Solo Administrador
    options.AddPolicy("SoloAdministrador", p => p.RequireRole("Administrador"));

    // Administrador y Empresa
    options.AddPolicy("AdminEmpresa", p => p.RequireRole("Administrador", "Empresa"));

});


// ── INYECCIÓN DE DEPENDENCIAS — AUTH ────────────────────────────────────────

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<UserHandler>();
builder.Services.AddScoped<EleccionesHandler>();
builder.Services.AddScoped<ClienteHandler>();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerWithBearer();

builder.Services.AddInfraestructureService(builder.Configuration);
builder.Services.AddApplicationServices();
const string corsPolicyName = "AllowAngularDev";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ── CORS ─────────────────────────────────────────────────────────────────────
//builder.Services.AddCors(options =>
//{
//    // Desarrollo — permite cualquier origen
//    options.AddPolicy("Development", policy =>
//        policy.AllowAnyOrigin()
//              .AllowAnyMethod()
//              .AllowAnyHeader());

//    // Producción — solo orígenes específicos
//    options.AddPolicy("Production", policy =>
//        policy.WithOrigins(
//                  "https://tudominio.com",
//                  "https://www.tudominio.com")
//              .AllowAnyMethod()
//              .AllowAnyHeader()
//              .AllowCredentials());
//});

// Agregar MIddleware de PollyRetry para la Base de datos
var app = builder.Build();

// Configure the HTTP request pipeline.
Constants.IsInDevelopment = app.Environment.IsDevelopment();
if (Constants.IsInDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//if (app.Environment.IsDevelopment())
//    app.UseCors("Development");  // ← cualquier origen en desarrollo
//else
//    app.UseCors("Production");
app.UseCors(corsPolicyName);

app.UseAuthentication(); 
app.UseAuthorization();

app.UseMiddleware<HttpRequestMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.MapAuthEndpoints();

app.MapEleccionesEndpoints();
app.MapUserEndpoints();
app.MapPagoIntegracionesEndpoints();
app.MapPagoEndpoints();
app.MapClienteEndpoints();
app.MapProgramacionCuentaCorrienteEndpoints();
try
{
    Log.Logger = LogginConfiguration.AddLoggerConfiguration(app);
    Serilog.Debugging.SelfLog.Enable(msg =>
    {
        Debug.Print(msg);
    });
    if (Constants.IsInDevelopment)
    {
        Serilog.Debugging.SelfLog.Enable(msg =>
        {
            Debug.Print(msg);
        });
    }
    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}

    