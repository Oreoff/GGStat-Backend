using System.ComponentModel.DataAnnotations;

namespace GGStatParsingDataService.models
{
    public class Chat
    {
        [Key]
        public int id { get; set; }
        public string? time { get; set; }
        public string? player { get; set; }
        public string? message { get; set; }
    }
}
