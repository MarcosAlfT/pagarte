using IdentityService.Application.Interfaces;
using IdentityService.Application.Services;
using IdentityService.Infrastructure.Persistence;
using IdentityService.Infrastructure.Persistence.Repositories;
using IdentityService.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;

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
            builder.Services.AddDbContext<IdentityDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("AuthDB")));

            builder.Services.AddDbContext<OpenIddictDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("AuthDB")));

			builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
            builder.Services.AddSingleton<IEmailConfirmationTokenGenerator, EmailConfirmationTokenGenerator>();
            //builder.Services.AddSharedEmail(host: "smtp.provider.com", port: 587, username: "info@pagarte.com", password: "pass12345");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer();

			builder.Services.AddOpenIddict()
				.AddCore(options =>
				{
					options.UseEntityFrameworkCore()
                        .UseDbContext<OpenIddictDbContext>();
				})
				.AddServer(options =>
				{
					var lifetime = configuration.GetValue<double>("AuthSettings:AccessTokenLifetimeMinutes");
					var strAudience = configuration.GetValue<string>("AuthSettings:Audience") ?? string.Empty;

					options.SetTokenEndpointUris("/connect/token");
                    options.AllowPasswordFlow().AllowRefreshTokenFlow();
                    options.AcceptAnonymousClients();

					options.AddDevelopmentEncryptionCertificate();
					options.AddDevelopmentSigningCertificate();
					options.DisableAccessTokenEncryption();

					options.UseAspNetCore().EnableTokenEndpointPassthrough();
					options.SetAccessTokenLifetime(TimeSpan.FromMinutes(lifetime));
					options.SetRefreshTokenLifetime(TimeSpan.FromDays(2));

				}).AddValidation(options =>
				{
					options.UseLocalServer();
					options.UseAspNetCore();
				});


			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			//builder.Services.AddOpenApi();


			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			{
				options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = Microsoft.OpenApi.Models.ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
				});

				options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
				{
					{
						new Microsoft.OpenApi.Models.OpenApiSecurityScheme
						{
							Reference = new Microsoft.OpenApi.Models.OpenApiReference
							{
								Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						Array.Empty<string>()
					}
				});
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			//if (app.Environment.IsDevelopment())
			//{
			//    app.MapOpenApi();
			//}

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger(); // Serves the OpenAPI JSON document at /swagger/v1/swagger.json
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityService API V1");
					options.RoutePrefix = "swagger"; 
					options.DocExpansion(DocExpansion.None); 
				});
			}

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

            app.Run();
        }
    }
}
