using data;
using GGStat_Backend.controllers;
using GGStat_Backend.Data;
using Microsoft.EntityFrameworkCore;
using PortWrapper;

namespace GGStat_Backend
{
	public class Program(IPortParser portParser)
	{
		public static async Task Main(string[] args)
		{
			
			var builder = WebApplication.CreateBuilder(args);
			var DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
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
			builder.Services.DataRegister(DefaultConnection);
			builder.Services.AddSwaggerGen();
			builder.Services.AddControllers().AddNewtonsoftJson();
			var app = builder.Build();
			using (var scope = app.Services.CreateScope())
			{
				var parser = scope.ServiceProvider.GetRequiredService<IPortParser>();
				Settings.Port = await parser.GetPort(); 
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