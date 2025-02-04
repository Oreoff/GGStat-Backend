﻿using System.ComponentModel.DataAnnotations;

namespace GGStat_Backend.models
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
