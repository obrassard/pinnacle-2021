namespace Pinnacle_2021.Contracts.Requests
{
	public record ConsumeItemRequest
	{
		public string? Upc { get; set; }
		public string? Title { get; set; }
		public int Quantity { get; set; } = 1;
	}
}
