using GGStat_Backend.models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
namespace GGStat_Backend
{
    [Route("api/players")]
    [ApiController]
    public class SetData : ControllerBase
    {
        private readonly string _filePath = "data/players.json";
        [HttpGet]
        public IActionResult GetPlayers()
        {
            if (!System.IO.File.Exists(_filePath))
            {
                return NotFound("File not found");
            }

            string jsonData = System.IO.File.ReadAllText(_filePath);
            List<PlayerData> players = JsonConvert.DeserializeObject<List<PlayerData>>(jsonData);
            return Ok(players);
        }
    }
}
