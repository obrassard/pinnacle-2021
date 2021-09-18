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

		#region Post

		[HttpPost(ApiRoutes.Items.ADD_TO_INVENTORY)]
		public async Task<ActionResult<AddItemResponse>> AddToInventory(Guid inventoryId, AddItemRequest addItemRequest)
		{
			var oneOfResponse = await _itemService.Create(inventoryId, addItemRequest);
			return oneOfResponse.Match<ActionResult<AddItemResponse>>(
				addItemResponse => CreatedAtAction(null, null, addItemResponse),
				notFound => NotFound()
			);
		}

		[HttpDelete(ApiRoutes.Items.CONSUME)]
		public async Task<IActionResult> ConsumeItem(Guid inventoryItemId, ConsumeItemRequest consumeRequest)
		{
			await _itemService.Consume(inventoryItemId, consumeRequest);
			return NoContent();
		}

		#endregion
	}
}
