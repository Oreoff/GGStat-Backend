using data;
using GGStat_Backend.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GGStat_Backend.controllers;
public record RefreshReadStoreCommand : IRequest;
public class RefreshReadStoreHandler(IDbContextFactory<PlayersDBContext> dbContextFactory, IReadStore readStore) : IRequestHandler<RefreshReadStoreCommand>
{
    public async Task Handle(RefreshReadStoreCommand request, CancellationToken ct)
    {
        var context = await dbContextFactory.CreateDbContextAsync();
        var players = await context.PlayerData
            .AsNoTracking()
            .OrderByDescending(p => p.points)
            .ToListAsync(ct);

        var countryTops = players
            .Where(p => !string.IsNullOrEmpty(p.code))
            .GroupBy(p => p.code)
            .ToDictionary(
                g => g.Key,
                g => g
                    .OrderByDescending(p => p.points)
                    .Select(p => new CountryTop
                    {
                        code = p.code,
                        flag = p.flag,
                        name = p.name,
                        region = p.region,
                        avatar = p.avatar,
                        alias = p.alias,
                        points = p.points
                    })
                    .First()
            );

        readStore.SetPlayers(players);
        readStore.SetCountryTops(countryTops);
    }
}