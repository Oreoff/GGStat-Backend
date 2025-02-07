using GGStat_Backend.data;
using Microsoft.EntityFrameworkCore;

namespace GGStat_Backend
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// Створюємо об'єкт веб-застосунку
			var builder = WebApplication.CreateBuilder(args);

			// Налаштування підключення до БД
			builder.Services.AddDbContext<PlayersDBContext>(options =>
				options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

			// Налаштування CORS для дозволу запитів з localhost:3000
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowLocalhost", policy =>
				{
					policy.WithOrigins("http://localhost:3000")
						  .AllowAnyHeader()
						  .AllowAnyMethod();
				});
			});

			// Налаштування Swagger
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// Налаштування контролерів з Newtonsoft.Json
			builder.Services.AddControllers().AddNewtonsoftJson();

			var app = builder.Build();

			// Міграції БД при старті застосунку
			using (var scope = app.Services.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<PlayersDBContext>(); 
			}

			// Налаштування CORS
			app.UseCors("AllowLocalhost");

			// Налаштування Swagger в режимі розробки
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			// Налаштування маршрутизації
			app.MapControllers();

			// Запуск веб-застосунку
			app.Run();
		}
	}
}