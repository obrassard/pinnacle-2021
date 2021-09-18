using System.Collections.Generic;

namespace Pinnacle_2021.Contracts.Responses
{
	public record InventoryDetailResponse
	{
		public string Title { get; set; }

		public IEnumerable<ItemListResponse> Items { get; set; }
	}
}
