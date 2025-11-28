using GGStat_Backend.controllers;
using GGStat_Backend.data;
using Microsoft.EntityFrameworkCore;
using PortWrapper;

namespace GGStat_Backend
{
	public class Program(IPortParser portParser)
	{
		public static async Task Main(string[] args)
		{
			
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddDbContext<PlayersDBContext>(options =>
				options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowLocalhost", policy =>
				{
					policy.WithOrigins("http://localhost:3000")
						  .AllowAnyHeader()
						  .AllowAnyMethod();
				});
			});
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSingleton<IPortParser, PortParser>();
			builder.Services.AddSwaggerGen();
			builder.Services.AddControllers().AddNewtonsoftJson();
			var app = builder.Build();
			using (var scope = app.Services.CreateScope())
			{
				var parser = scope.ServiceProvider.GetRequiredService<IPortParser>();
				Settings.Port = await parser.GetPort(); 
			}

			using (var scope = app.Services.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<PlayersDBContext>(); 
			}
			app.UseCors("AllowLocalhost");
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.MapControllers();
			app.Run();
		}
	}
}