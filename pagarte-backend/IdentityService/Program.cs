
using DataAccess.Shared;
using Email.Shared;
using IdentityService.Application.Interfaces;
using IdentityService.Application.Services;
using IdentityService.Infrastructure.Persistence.Repositories;
using IdentityService.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
			builder.Services.AddScoped<IAuthService, AuthService>(); 
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddSharedDataAccess();
			builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
            builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator> ();
            builder.Services.AddSingleton<IEmailConfirmationTokenGenerator, EmailConfirmationTokenGenerator>();
            builder.Services.AddSharedEmail(host: "smtp.provider.com", port: 587, username: "info@pagarte.com", password: "pass12345");

            builder.Services.AddAuthentication (options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
						ValidAudience = builder.Configuration["JwtSettings:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))

						//Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
						//System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
					};
				});

			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

            app.Run();
        }
    }
}
