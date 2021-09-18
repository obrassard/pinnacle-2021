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

		[HttpGet(ApiRoutes.Items.GET_BY_ID)]
		public async Task<ActionResult<ItemDetailResponse>> GetProductDetail(Guid inventoryId, Guid itemId)
		{
			return Ok(await _itemService.GetItem(inventoryId, itemId));
		}

		#endregion

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

		#endregion

		#region Patch

		[HttpPatch(ApiRoutes.Items.CHANGE_QTY)]
		public async Task<IActionResult> ChangeQty(Guid inventoryItemId, ChangeQuantityRequest changeQuantityRequest)
		{
			await _itemService.ChangeQuantity(inventoryItemId, changeQuantityRequest);
			return NoContent();
		}
		#endregion


		#region Delete

		[HttpDelete(ApiRoutes.Items.CONSUME)]
		public async Task<IActionResult> ConsumeItem(Guid inventoryItemId, ConsumeItemRequest consumeRequest)
		{
			await _itemService.Consume(inventoryItemId, consumeRequest);
			return NoContent();
		}

		#endregion
	}
}
