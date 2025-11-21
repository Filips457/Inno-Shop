using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserApi.Exceptions;
using UserApplication.DTOs;
using UserApplication.Interfaces;
using UserApplication.Services;
using UserInfrastructure.DataSources;
using UserInfrastructure.Repositories;
using UserInfrastructure.Security;

namespace UserApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddProblemDetails(configure =>
            {
                configure.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Extensions.TryAdd("requstId", context.HttpContext.TraceIdentifier);
                };
            });
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IPasswordHasher, PasswordHasherBCrypt>();

            var jwtSection = builder.Configuration.GetSection("JwtSettings");
            var jwtSettings = jwtSection.Get<JwtSettings>();

            if (jwtSettings == null)
                throw new InvalidOperationException("Строка jwt не найдена");

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });

            builder.Services.AddAuthorization();


            builder.Services.Configure<JwtSettings>(jwtSection);
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();


            var connStr = builder.Configuration.GetConnectionString("MySqlConnection");

            if (string.IsNullOrEmpty(connStr))
                throw new InvalidOperationException("Строка подключения не найдена");

            builder.Services.AddDbContext<UserContextMySql>(options => options.UseMySQL(connStr));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseExceptionHandler();
            //app.UseMiddleware<GlobalExceptionHandler>();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
