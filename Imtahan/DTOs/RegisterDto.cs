﻿using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;


namespace Imtahan.DTOs
{
	public class RegisterDto
	{
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		public string Name { get; set; }
		[Required]
		[MinLength(5)]
		[MaxLength(50)]
		public string Surname { get; set; }
		[Required]
		[MinLength(3)]
		[MaxLength(100)]
		public string UserName { get; set; }
		[Required]
		[MinLength(6)]
		[MaxLength(50)]
		public string Email { get; set; }
		[Required]
		[MinLength(8)]
		[MaxLength(100)]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required]
		[DataType(DataType.Password),Compare("Password")]
		public string ConfirmPassword { get; set; }

	}
}
