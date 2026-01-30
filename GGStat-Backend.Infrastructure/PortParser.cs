using System.Net;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;

namespace PortWrapper;

public interface IPortParser
{
    Task<int> GetPort();
}

public class PortParser(ILogger<PortParser> logger) : IPortParser
{
    public async Task<int> GetPort()
    {
        using var httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(3)
        };

        var ipProps = IPGlobalProperties.GetIPGlobalProperties();

        var loopbackPorts = ipProps
            .GetActiveTcpListeners()
            .Where(ep =>
                ep.Address.Equals(IPAddress.Loopback) ||
                ep.Address.Equals(IPAddress.IPv6Loopback))
            .Select(ep => ep.Port)
            .ToHashSet();

        var portsToCheck = ipProps
            .GetActiveTcpConnections()
            .Where(conn =>
                conn.LocalEndPoint.Address.Equals(IPAddress.Loopback) &&
                loopbackPorts.Contains(conn.LocalEndPoint.Port))
            .Select(conn => conn.LocalEndPoint.Port)   
            .Distinct()
            .Order()
            .ToList();

        foreach (var port in portsToCheck)
        {
            logger.LogInformation("Checking port {Port}", port);

            var url =
                $"http://localhost:{port}/web-api/v1/leaderboard/12972?offset=0&length=25";

            try
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation(
                        "StarCraft local web UI found on port {Port}",
                        port);

                    return port; 
                }
            }
            catch (HttpRequestException) { }
            catch (TaskCanceledException) { }
        }

        logger.LogWarning("Could not find SCR web UI server");
        return 0;
    }
}