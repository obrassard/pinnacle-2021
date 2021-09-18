namespace Pinnacle_2021.Contracts.Responses
{
	public record AddItemResponse : CreationResponse
	{
		public string Title { get; set; }
		public string Image { get; set; }
	}
}
