using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Pinnacle_2021.Api.Services.Domain;
using Pinnacle_2021.Contracts;
using Pinnacle_2021.Contracts.Requests;
using Pinnacle_2021.Contracts.Responses;

namespace Pinnacle_2021.Api.Controllers
{
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		#region Post

		[HttpPost(ApiRoutes.Users.CREATE)]
		public async Task<ActionResult<UserCreationResponse>> Create(UserForCreation userForCreation)
		{
			return await _userService.Create(userForCreation);
		}

		#endregion
	}
}
