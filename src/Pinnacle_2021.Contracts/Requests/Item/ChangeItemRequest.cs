using System;

namespace Pinnacle_2021.Contracts.Requests
{
	public record ChangeItemRequest
	{
		public int? Quantity { get; set; }
		public DateTime? Expiration { get; set; }
	}
}
