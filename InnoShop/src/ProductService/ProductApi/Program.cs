
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ProductApi.Exceptions;
using ProductApplication.Interfaces;
using ProductApplication.Services;
using ProductApplication.Validation;
using ProductInfrastructure.DataSources;
using ProductInfrastructure.Repositories;

namespace ProductApi
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

            builder.Services.AddSwaggerGen();

            builder.Services.AddProblemDetails(configure =>
            {
                configure.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Extensions.TryAdd("requstId", context.HttpContext.TraceIdentifier);
                };
            });
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();


            var connStr = builder.Configuration.GetConnectionString("MySqlConnection");

            if (string.IsNullOrEmpty(connStr))
                throw new InvalidOperationException("Строка подключения не найдена");

            builder.Services.AddDbContext<ProductContextMySql>(options => options.UseMySQL(connStr));

            builder.Services.AddControllers().AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<ProductRequestDtoValidator>();
            });


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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
