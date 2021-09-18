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

		#region Ge

		[HttpGet(ApiRoutes.Users.GET)]
		public async Task<ActionResult<UserResponse>> Create([FromQuery] string email)
		{
			var oneOfResponse = await _userService.Get(email);
			return oneOfResponse.Match<ActionResult<UserResponse>>(
				userResponse => Ok(userResponse),
				notFound => NotFound()
			);
		}

		#endregion

		#region Post

		[HttpPost(ApiRoutes.Users.CREATE)]
		public async Task<ActionResult<UserCreationResponse>> Create(UserForCreation userForCreation)
		{
			var response = await _userService.Create(userForCreation);
			return CreatedAtAction(null, null, response);
		}

		#endregion
	}
}
