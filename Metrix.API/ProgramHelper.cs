using Metrix.API.Endpoints;
using Metrix.API.Handlers;
using Metrix.Application.Interfaces;
using Metrix.Application.Interfaces.Repositories;
using Metrix.Application.Interfaces.Services;
using Metrix.Application.Services;
using Metrix.Infrastructure.Configuration;
using Metrix.Infrastructure.Helpers;
using Metrix.Infrastructure.Persistence;
using Metrix.Infrastructure.Repositories;
using Metrix.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static Metrix.API.Constants.ApiRoutes;

namespace Metrix.API;

public static class ProgramHelper
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ================= DATABASE =================
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));

        // ================= AUTHORIZATION (FIXED) =================
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));
        });

        // ================= EMAIL SETTINGS =================
        services.Configure<EmailSettings>(
            configuration.GetSection("EmailSettings"));

        // ================= REPOSITORIES =================
        services.AddScoped<IHRRepository, HRRepository>();
        services.AddScoped<ISecurityUserRepository, SecurityUserRepository>();
        services.AddScoped<IInvitationRepository, InvitationRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();

        // ================= SERVICES =================
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtGenerator, JwtGenerator>();
        services.AddScoped<IHrService, HrService>();
        services.AddScoped<ISecurityService, SecurityService>();
        services.AddScoped<IInvitationService, InvitationService>();
        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddScoped<IAdminService, AdminService>();

        // ================= HANDLERS =================
        services.AddScoped<RegisterHrHandler>();
        services.AddScoped<RegisterSecurityHandler>();
        services.AddScoped<SendInvitationHandler>();
        services.AddScoped<CreateAdminHandler>();

        // ================= JWT AUTHENTICATION =================
        var jwtKey = configuration["JwtSettings:Key"];
        var jwtIssuer = configuration["JwtSettings:Issuer"];
        var jwtAudience = configuration["JwtSettings:Audience"];

        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtKey!))
                    };
            });

        // ================= CORS =================
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                policy => policy.WithOrigins("http://localhost:5174")
                                .AllowAnyMethod()
                                .AllowAnyHeader());
        });

        // ================= SWAGGER =================
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static WebApplication UseApplicationPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowFrontend");

        app.UseAuthentication();   // MUST come before Authorization
        app.UseAuthorization();

        // ================= ENDPOINTS =================
        app.MapAuthEndpoints();
        app.MapHrManagementEndpoints();
        app.MapSecurityManagementEndpoints();
        app.MapInvitationEndpoints();
        app.MapAdminEndpoint();

        return app;
    }
}