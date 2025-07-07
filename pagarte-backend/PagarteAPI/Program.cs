using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PagarteAPI.Application.Interfaces;
using PagarteAPI.Application.Services;
using PagarteAPI.Infrastructure.Persistence;
using PagarteAPI.Infrastructure.Persistence.Repository;

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
			builder.Services.AddScoped<IPaymentService, PaymentService>();

			// Add authentication to trust IdentityServer as an authority.

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
                var strAuthority = configuration.GetValue<string>("AuthSettings:Authority");
                var strAudience = configuration.GetValue<string>("AuthSettings:Audience");

				options.Authority = strAuthority;
                options.Audience = strAudience;

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
