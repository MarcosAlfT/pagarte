using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PagarteAPI.Application.Interfaces;
using PagarteAPI.Application.Services;
using PagarteAPI.Infrastructure.Persistence;
using PagarteAPI.Infrastructure.Persistence.Repository;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace PagarteAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
			var configuration = builder.Configuration;

			// Add services to the container.

			builder.Services.AddDbContext<PaymentsDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("PaymentsDb")));
			
            // Register repositories and services

			builder.Services.AddControllers();
            builder.Services.AddScoped<IPaymentRepository, PaymentsRepository>();
            builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
			builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
			builder.Services.AddScoped<IPaymentService, PaymentService>();

			// --- Swagger/OpenAPI Configuration (for .NET 8.0) ---
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "PagarteAPI",
					Version = "v1",
					Description = "API for managing payments and payment methods."
				});

				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
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
			// --- End Swagger/OpenAPI Configuration ---


			// Add authentication to trust IdentityServer as an authority.

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
                var strAuthority = configuration.GetValue<string>("AuthSettings:Authority");
                var strAudience = configuration.GetValue<string>("AuthSettings:Audience");

				options.Authority = strAuthority;
                options.Audience = strAudience;
				options.IncludeErrorDetails = true;

				options.BackchannelHttpHandler = new HttpClientHandler
				{
					ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
				};

				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateAudience = true,
					ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
				};

				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						Console.WriteLine("---------------------------------------------");
						// This event is called for every request where the JwtBearer handler is active
						string? authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
						if (string.IsNullOrEmpty(authorizationHeader))
						{
							Console.WriteLine("DEBUG: OnMessageReceived - NO Authorization header found.");
						}
						else
						{
							Console.WriteLine($"DEBUG: OnMessageReceived - Authorization header found: '{authorizationHeader.Substring(0, Math.Min(authorizationHeader.Length, 50))}...'"); // Log first 50 chars
							if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
							{
								Console.WriteLine("DEBUG: OnMessageReceived - Authorization header does NOT start with 'Bearer '.");
							}
						}
						return Task.CompletedTask;
					},
					OnAuthenticationFailed = context =>
					{
						Console.WriteLine("---------------------------------------------");
						Console.WriteLine($"Authentication Failed: {context.Exception.GetType().Name}");
						Console.WriteLine($"Message: {context.Exception.Message}");
						if (context.Exception.InnerException != null)
						{
							Console.WriteLine($"Inner Exception: {context.Exception.InnerException.GetType().Name}");
							Console.WriteLine($"Inner Message: {context.Exception.InnerException.Message}");
						}
						Console.WriteLine("---------------------------------------------");
						return Task.CompletedTask;
					},
					OnTokenValidated = context =>
					{
						Console.WriteLine("Token successfully validated.");
						return Task.CompletedTask;
					}
				};
			});


			builder.Services.AddAuthorization();

			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			//builder.Services.AddOpenApi();

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			//if (app.Environment.IsDevelopment())
			//{
			//    app.MapOpenApi();
			//}

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger(); // Serves the OpenAPI JSON document
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "PagarteAPI V1");
					options.RoutePrefix = "swagger"; // Access at /swagger
					options.DocExpansion(DocExpansion.None); // Collapse all by default
				});
				// The original app.MapOpenApi(); is commented out
				// app.MapOpenApi();
			}

			app.UseHttpsRedirection();
            app.UseAuthentication();
			app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
