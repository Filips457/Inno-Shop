
using Microsoft.EntityFrameworkCore;
using UserApplication.Interfaces;
using UserApplication.Services;
using UserInfrastructure.DataSources;
using UserInfrastructure.Repositories;

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

            builder.Services.AddScoped<IUserRepository, MySqlRepository>();
            builder.Services.AddScoped<IUserService, UserService>();


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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
