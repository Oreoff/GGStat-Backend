using System.Net;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;

namespace PortWrapper;

public interface IPortParser
{
    Task<int> GetPort();
}
public class PortParser(ILogger<PortParser> logger): IPortParser
{
    public async Task<int> GetPort()
    {
        using var httpClient = new HttpClient();

        var loopbackPorts = IPGlobalProperties
            .GetIPGlobalProperties()
            .GetActiveTcpListeners()
            .Where(ep =>
                ep.Address.Equals(IPAddress.Loopback) ||
                ep.Address.Equals(IPAddress.IPv6Loopback))
            .Select(ep => ep.Port)
            .ToHashSet(); 

        var localConnections = IPGlobalProperties
            .GetIPGlobalProperties()
            .GetActiveTcpConnections()
            .Where(conn =>
                (conn.LocalEndPoint.Address.Equals(IPAddress.Loopback) &&
                 loopbackPorts.Contains(conn.LocalEndPoint.Port) ))
            .ToList();
        var i = 0;
			
        for (int port = 49152; port <= 65535; port++)
        {
            logger.LogInformation($"Found port: {port}");
            i++;

            string url = $"http://host.docker.internal:{port}/web-api/v1/leaderboard/12972?offset=0&length=25";
            logger.LogInformation(url);
            try
            {
                var ct = new CancellationTokenSource(3000);
                var response = await httpClient.GetAsync(url, ct.Token);
                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation($"StarCraft local web UI found on port {port}");
                    return port;
                }
            }
            catch (HttpRequestException)
            {
                
            }
            catch (TaskCanceledException)
            {
            }
        }

        Console.WriteLine(" Could not find SCR web UI server.");
        return 0;
    }
}