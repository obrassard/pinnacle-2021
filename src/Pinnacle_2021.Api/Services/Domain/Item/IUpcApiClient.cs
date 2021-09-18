using Pinnacle_2021.Api.Entities;

namespace Pinnacle_2021.Api.Services.Domain
{
	public interface IUpcApiClient
	{
		Item ScrapeUPCData(string upcCode);
	}
}