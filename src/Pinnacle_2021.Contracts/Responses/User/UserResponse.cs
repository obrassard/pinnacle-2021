using System;

namespace Pinnacle_2021.Contracts.Responses
{
	public record UserResponse
	{
		public Guid Id { get; set; }
		public string CompleteName { get; set; }
	}
}
