using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Pinnacle_2021.Api.Services.Domain;

namespace Pinnacle_2021.Api.Controllers
{
	[ApiController]
	public class AlertController : ControllerBase
	{
		private readonly IAlertService _alertService;

		public AlertController(IAlertService alertService)
		{
			_alertService = alertService;
		}

		[HttpPost("api/alert")]
		public async Task<IActionResult> AlertAll()
		{
			await _alertService.AlertAll();
			return Ok();
		}
	}
}
