using data;
using Microsoft.EntityFrameworkCore;

namespace GGStat_Backend.Data;

public interface IApiRequestToDb
{
   Task<List<string>> GetCountries();
   Task<List<string>> SearchPlayersByName(string name);

   Task<List<CountryTop>> GetCountryTop();
   
   Task<PlayerData> GetPlayer(string name);
   Task<(List<PlayerData>, int)> GetLeaderboard(int offset, int limit, string country_code, List<string> league,string race);
}
public class ApiRequestsToDb: IApiRequestToDb
{
   private IDbContextFactory<PlayersDBContext> _contextFactory;
   public ApiRequestsToDb(IDbContextFactory<PlayersDBContext> contextFactory)
   {
      _contextFactory = contextFactory;
   }
   public async Task<List<string>> GetCountries()
   {
      await using var context = await _contextFactory.CreateDbContextAsync();
      return context.PlayerDatas.Select(c => c.code).Distinct().ToList();
   }

   public async Task<List<string>> SearchPlayersByName(string name)
   {
      await using var context = await _contextFactory.CreateDbContextAsync();

      if (string.IsNullOrWhiteSpace(name))
         return new List<string>();
      return await context.PlayerDatas
         .Where(pd => pd.name.ToLower().Contains(name.ToLower()))
         .Select(pd => pd.name)
         .Take(5)
         .ToListAsync();
   }

   public async Task<List<CountryTop>> GetCountryTop()
   {
      await using var context = await _contextFactory.CreateDbContextAsync();
      var players = await context.PlayerDatas
         .Select(pd => new
         {
            pd.name,
            pd.region,
            pd.avatar,
            pd.code,
            pd.flag,
            pd.points,
            pd.alias
         })
         .ToListAsync();

      var topPlayers = players
         .GroupBy(p => p.code)
         .Select(g => g.OrderByDescending(p => p.points).First())
         .Select(p => new CountryTop
         {
            name = p.name,
            region = p.region,
            alias = p.alias,
            avatar = p.avatar,
            code = p.code,
            flag = p.flag,
            points = p.points
         })
         .ToList();

      return topPlayers;
   }

   public async Task<PlayerData> GetPlayer(string name)
   {
      await using var context = await _contextFactory.CreateDbContextAsync();
      var player = await context.PlayerDatas
         .Include(pd => pd.matches)
         .ThenInclude(m => m.chat)
         .FirstOrDefaultAsync(p => p.name == name);

      return player;
   }

   public async Task<(List<PlayerData>, int)> GetLeaderboard(int offset, int limit, string country_code, List<string> league,string race)
   {
         await using var context = await _contextFactory.CreateDbContextAsync();
         var query = context.PlayerDatas.AsQueryable();

         if (!string.IsNullOrEmpty(country_code))
         {
            if (country_code.StartsWith("!"))
            {
               var excludeCode = country_code.Substring(1); 
               query = query.Where(p => p.code != excludeCode);
            }
            else
            {
               query = query.Where(p => p.code == country_code);
            }
         }
         if (!string.IsNullOrEmpty(race))
         {
            query = query.Where(p => p.race == race);
         }

         if (league != null && league.Count() > 0)
         {
            query = query.Where(p => league.Contains(p.league));
         }
         var totalCount = await query.CountAsync();
         var players = await query
            .OrderByDescending(p => p.points)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

         return (players, totalCount);
      
   }
}