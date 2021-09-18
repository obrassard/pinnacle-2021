using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Pinnacle_2021.Api.Services.Domain;
using Pinnacle_2021.Contracts;
using Pinnacle_2021.Contracts.Requests;
using Pinnacle_2021.Contracts.Responses;

namespace Pinnacle_2021.Api.Controllers
{
	[ApiController]
	public class ItemController : ControllerBase
	{
		private readonly IItemService _itemService;

		public ItemController(IItemService itemService)
		{
			_itemService = itemService;
		}

		#region Get

		[HttpGet(ApiRoutes.Items.GET)]
		public async Task<ActionResult<ItemResponse>> Get([FromQuery] string upc)
		{
			return Ok(await _itemService.Get(upc));
		}

		#endregion

		#region Post

		[HttpPost(ApiRoutes.Items.ADD_TO_INVENTORY)]
		public async Task<IActionResult> AddToInventory(Guid inventoryId, AddItemRequest addItemRequest)
		{
			return CreatedAtAction(null, null, await _itemService.Create(inventoryId, addItemRequest));
		}

		#endregion
	}
}
