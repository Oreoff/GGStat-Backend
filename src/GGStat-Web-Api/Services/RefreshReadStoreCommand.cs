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
            .Select(g => 
            {
                var top = g.First(); // вже відсортовано по points ↓
                return new CountryTop
                {
                    code = top.code,
                    flag = top.flag,
                    name = top.name,
                    region = top.region,
                    avatar = top.avatar,
                    alias = top.alias,
                    points = top.points,
                    playersCount = g.Count()
                };
            })
            .ToList();

        readStore.SetPlayers(players);
        readStore.SetCountryTops(countryTops);
    }
}