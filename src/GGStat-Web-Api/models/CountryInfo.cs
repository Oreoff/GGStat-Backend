﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace GGStat_Backend.models
{
	public class CountryInfo
	{
		[Key]
		public int id { get; set; }
		public string? code { get; set; }
		public string? flag { get; set; }
	}
}
