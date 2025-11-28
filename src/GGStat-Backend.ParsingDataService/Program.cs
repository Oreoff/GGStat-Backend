
using GGStatParsingDataService;
using Microsoft.Extensions.Hosting;

internal class Program
{
	private static async Task Main(string[] args)
	{
		var host = Host.CreateDefaultBuilder(args).ConfigureServices((services) =>
		{
			services.GGStatParserRegister();
		}).Build();
		await host.RunAsync();
	}
}