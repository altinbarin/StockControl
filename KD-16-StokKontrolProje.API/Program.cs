using Microsoft.EntityFrameworkCore;
using StokKontrolProje.Repositories.Abstract;
using StokKontrolProje.Repositories.Concrete;
using StokKontrolProje.Repositories.Context;
using StokKontrolProje.Service.Abstract;
using StokKontrolProje.Service.Concrete;

namespace KD_16_StokKontrolProje.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddControllers().AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            builder.Services.AddDbContext<StokKontrolContext>(option =>
            {
                option.UseSqlServer("Data Source=DESKTOP-PE0VBS8;Database=StokTakipDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            });
            builder.Services.AddTransient(typeof(IGenericService<>),typeof(GenericManager<>));
            builder.Services.AddTransient(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
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