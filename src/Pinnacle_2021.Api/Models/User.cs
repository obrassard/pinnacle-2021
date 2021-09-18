using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pinnacle_2021.Api.Models
{
	[Table("User")]
	public class User
	{
		[Key]
		public Guid Id { get; set; }

		[MaxLength(255)]
		public string LastName { get; set; }

		[MaxLength(255)]
		public string FirstName { get; set; }
	}
}
