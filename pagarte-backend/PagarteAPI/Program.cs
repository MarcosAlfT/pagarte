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

            // Add authentication to trust IdentityServer as an authority.

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = builder.Configuration["Jwt:Authority"];
				options.Audience = builder.Configuration["Jwt:Audience"];

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
					ValidateIssuer = true,
					ValidateLifetime = true,
                };

            });

			// Add services to the container.

            builder.Services.AddDbContext<PaymentsDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PaymentsDb")));
			
            // Register repositories and services

			builder.Services.AddControllers();
            builder.Services.AddScoped<IPaymentRepository, PaymentsRepository>();
            builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
			builder.Services.AddScoped<IPaymentService, PaymentService>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
