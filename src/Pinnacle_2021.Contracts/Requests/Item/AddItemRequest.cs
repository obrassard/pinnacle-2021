namespace Pinnacle_2021.Contracts.Requests
{
	public record AddItemRequest
	{
		public string? Upc { get; set; }
		public string? Title { get; set; }
	}
}
