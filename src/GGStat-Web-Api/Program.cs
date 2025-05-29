using GGStat_Backend.data;
using Microsoft.EntityFrameworkCore;

namespace GGStat_Backend
{
	public class Program
	{
		public static void Main(string[] args)
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
			builder.Services.AddSwaggerGen();
			builder.Services.AddControllers().AddNewtonsoftJson();
			var app = builder.Build();
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