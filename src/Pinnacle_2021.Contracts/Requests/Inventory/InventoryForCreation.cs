using System.ComponentModel.DataAnnotations;

namespace Pinnacle_2021.Contracts.Requests
{
	public record InventoryForCreation
	{
		[Required]
		[MaxLength(255)]
		public string Title { get; set; }
	}
}
