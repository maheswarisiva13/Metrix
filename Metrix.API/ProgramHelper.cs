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

        // ================= EMAIL SETTINGS =================
        services.Configure<EmailSettings>(
            configuration.GetSection("EmailSettings"));

        // ================= REPOSITORIES =================
        services.AddScoped<IHRRepository, HRRepository>();
        services.AddScoped<ISecurityUserRepository, SecurityUserRepository>();
        services.AddScoped<IInvitationRepository, InvitationRepository>();

        // ================= SERVICES =================
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtGenerator, JwtGenerator>();
        services.AddScoped<IHrService, HrService>();
        services.AddScoped<ISecurityService, SecurityService>();
        services.AddScoped<IInvitationService, InvitationService>();
        services.AddScoped<IEmailService, SmtpEmailService>();

        // ================= HANDLERS =================
        services.AddScoped<RegisterHrHandler>();
        services.AddScoped<RegisterSecurityHandler>();
        services.AddScoped<SendInvitationHandler>();

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

        services.AddAuthorization();

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

        app.UseAuthentication();
        app.UseAuthorization();

        // ================= ENDPOINTS =================
        app.MapAuthEndpoints();
        app.MapHrManagementEndpoints();
        app.MapSecurityManagementEndpoints();
        app.MapInvitationEndpoints();

        return app;
    }
}
