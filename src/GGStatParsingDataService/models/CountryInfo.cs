﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace GGStatParsingDataService.models
{
    public class CountryInfo
    {
        [Key]
        public int id { get; set; }
        public string? code { get; set; }
        public string? flag { get; set; }
    }
}
