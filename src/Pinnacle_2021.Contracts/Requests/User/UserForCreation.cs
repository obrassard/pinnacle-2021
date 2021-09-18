using System.ComponentModel.DataAnnotations;

namespace Pinnacle_2021.Contracts.Requests
{
	public record UserForCreation
	{
		[Required]
		[MaxLength(255)]
		public string FirstName { get; init; }

		[Required]
		[MaxLength(255)]
		public string LastName { get; init; }

		[Required]
		[MaxLength(255)]
		[EmailAddress]
		public string Email { get; init; }

		[Required]
		[MaxLength(255)]
		[Phone]
		public string PhoneNumber { get; set; }
	}
}
