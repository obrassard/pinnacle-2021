using System.Threading.Tasks;

using Pinnacle_2021.Contracts.Requests;
using Pinnacle_2021.Contracts.Responses;

namespace Pinnacle_2021.Api.Services.Domain
{
	public interface IUserService
	{
		Task<UserCreationResponse> Create(UserForCreation userRequest);
	}
}