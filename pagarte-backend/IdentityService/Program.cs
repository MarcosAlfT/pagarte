
using DataAccess.Shared;
using Email.Shared;
using IdentityService.Application.Interfaces;
using IdentityService.Application.Services;
using IdentityService.Infrastructure.Persistence.Repositories;
using IdentityService.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;

namespace IdentityService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

			// Add services to the container.

			builder.Services.AddControllers();
			builder.Services.AddScoped<IAuthService, AuthService>(); 
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddSharedDataAccess();
			builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
            builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator> ();
            builder.Services.AddSingleton<IEmailConfirmationTokenGenerator, EmailConfirmationTokenGenerator>();
            builder.Services.AddSharedEmail(host: "smtp.provider.com", port: 587, username: "info@pagarte.com", password: "pass12345");


            builder.Services.AddAuthentication("Bearer").AddJwtBearer();

            builder.Services.AddOpenIddict()
				.AddCore(options =>
				{
					//options.UseEntityFrameworkCore()
					//	.UseDbContext<IdentityDbContext>();
					//options.SetDefaultScope("api");
					//options.SetDefaultAccessTokenLifetime(TimeSpan.FromHours(1));
				})
				.AddServer(options =>
				{
                    options.SetTokenEndpointUris("/connect/token");
                    options.AllowPasswordFlow().AllowRefreshTokenFlow();
                    options.AddSigningKey(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!)));
                    options.UseAspNetCore().EnableTokenEndpointPassthrough();
                    options.SetAccessTokenLifetime(TimeSpan.FromMinutes(10));
                    options.SetRefreshTokenLifetime(TimeSpan.FromDays(2));
				}).AddValidation(options =>
				{
					options.UseLocalServer();
					options.UseAspNetCore();
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
