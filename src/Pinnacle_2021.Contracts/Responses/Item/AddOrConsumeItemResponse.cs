namespace Pinnacle_2021.Contracts.Responses
{
	public record AddOrConsumeItemResponse : CreationResponse
	{
		public string Title { get; set; }
		public string Image { get; set; }
	}
}
