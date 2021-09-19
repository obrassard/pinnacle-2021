using System;
using System.Collections.Generic;

namespace Pinnacle_2021.Contracts.Responses
{
	public record BaseItemResponse
	{
		public Guid ItemId { get; set; }
		public string Title { get; set; }
		public string Image { get; set; }
	}


	public record ItemListResponse : BaseItemResponse
	{
		public int Count { get; set; }
		public bool ExpiredSoon { get; set; }
	}

	public record ItemDetailResponse : BaseItemResponse
	{
		public IEnumerable<InventoryItemDetail> Inventory { get; set; }
	}

	public record InventoryItemDetail
	{
		public Guid InvItemID { get; set; }
		public int Quantity { get; set; }
		public DateTime AddedAt { get; set; }
		public DateTime? Expiration { get; set; }
	}
}
