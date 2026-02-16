using data;
using GGStat_Backend.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GGStat_Backend.controllers;
public record GetLeaderboardQuery(
    int Offset,
    int Limit,
    string Country,
    List<string> Leagues,
    string Race,
    bool IsUnique
) : IRequest<(List<PlayerData>, int)>;
public class GetLeaderboardHandler(IReadStore readStore)
    : IRequestHandler<GetLeaderboardQuery, (List<PlayerData>, int)>
{
   

    public Task<(List<PlayerData>, int)> Handle(GetLeaderboardQuery q, CancellationToken ct)
    {
        var query = readStore.Players.AsQueryable();

        if (!string.IsNullOrEmpty(q.Country))
        {
            if (q.Country.StartsWith("!"))
            {
                var excludeCode = q.Country.Substring(1); 
                query = query.Where(p => p.code != excludeCode);
            }
            else
            {
                query = query.Where(p => p.code == q.Country);
            }
        }

        if (!string.IsNullOrEmpty(q.Race))
            query = query.Where(p => p.race == q.Race);

        if (q.Leagues?.Any() == true)
            query = query.Where(p => q.Leagues.Contains(p.league));

        if (q.IsUnique)
        {
            query = query
                .GroupBy(p => p.alias)
                .Select(g => g.MaxBy(p => p.points));
        }

        var total = query.Count();

        var result = query
            .Skip(q.Offset)
            .Take(q.Limit)
            .ToList();

        return Task.FromResult((result, total));
    }
}