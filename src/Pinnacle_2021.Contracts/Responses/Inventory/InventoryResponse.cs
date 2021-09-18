using System;

namespace Pinnacle_2021.Contracts.Responses
{
	public record InventoryResponse
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public int CountOfItems { get; set; }
	}
}
