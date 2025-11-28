using System.ComponentModel.DataAnnotations;
namespace GGStatParsingDataService.Models
{
    public class Rank
    {
        [Key]
        public int Id { get; set; }
        public int points { get; set; }
        public string league { get; set; }

    }
}
