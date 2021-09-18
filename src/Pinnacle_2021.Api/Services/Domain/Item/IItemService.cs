using System.Threading.Tasks;

using Pinnacle_2021.Contracts.Responses;

namespace Pinnacle_2021.Api.Services.Domain
{
	public interface IItemService
	{
		Task<ItemResponse> Get(string upc);
	}
}