using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Pinnacle_2021.Api.Services.Domain;
using Pinnacle_2021.Contracts;
using Pinnacle_2021.Contracts.Requests;
using Pinnacle_2021.Contracts.Responses;

namespace Pinnacle_2021.Api.Controllers
{
	[ApiController]
	public class InventoryController : ControllerBase
	{
		private readonly IInventoryService _inventoryService;

		public InventoryController(IInventoryService inventoryService)
		{
			_inventoryService = inventoryService;
		}

		#region Get

		[HttpGet(ApiRoutes.Inventories.GET)]
		public async Task<ActionResult<IEnumerable<InventoryResponse>>> Get(Guid userId)
		{
			return Ok(await _inventoryService.GetAll(userId));
		}

		#endregion

		#region Post

		[HttpPost(ApiRoutes.Inventories.CREATE)]
		public async Task<ActionResult<InventoryCreationResponse>> Create(Guid userId, InventoryForCreation request)
		{
			return CreatedAtAction(null, null, await _inventoryService.Create(userId, request));
		}

		#endregion
	}
}
