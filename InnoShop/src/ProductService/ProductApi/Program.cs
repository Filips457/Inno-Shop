
using Microsoft.EntityFrameworkCore;
using ProductApplication.Interfaces;
using ProductApplication.Services;
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

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();


            var connStr = builder.Configuration.GetConnectionString("MySqlConnection");

            if (string.IsNullOrEmpty(connStr))
                throw new InvalidOperationException("Строка подключения не найдена");

            builder.Services.AddDbContext<ProductContextMySql>(options => options.UseMySQL(connStr));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
