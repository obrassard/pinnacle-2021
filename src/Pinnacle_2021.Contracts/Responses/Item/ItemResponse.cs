using System;

namespace Pinnacle_2021.Contracts.Responses
{
	public record ItemResponse
	{
		public Guid Id { get; set; }

		public string Title { get; set; }
	}
}
