using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PagarteAPI.Application.Interfaces;
using PagarteAPI.Application.Services;
using PagarteAPI.Infrastructure.Persistence;
using PagarteAPI.Infrastructure.Persistence.Repository;
//using Swashbuckle.AspNetCore.Swagger;
//using Swashbuckle.AspNetCore.SwaggerGen;
//using Swashbuckle.AspNetCore.SwaggerUI;

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

			//---Swagger / OpenAPI Configuration

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
					Description = "JWT Authorization header using the Bearer scheme."
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
						Array.Empty<string>()
					}
				});
			});
			//	End Swagger / OpenAPI Configuration

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
			});

			builder.Services.AddAuthorization();

			var app = builder.Build();

			//Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger(); // Serves the generated Swagger JSON
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "PagarteAPI v1");
					options.RoutePrefix = string.Empty; // Makes Swagger UI accessible at the root URL (e.g., https://localhost:7112/)
				});
			}

			app.UseHttpsRedirection();
            app.UseAuthentication();
			app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
