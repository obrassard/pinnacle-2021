﻿using System;
using System.Threading.Tasks;

using OneOf;

using Pinnacle_2021.Api.Models;
using Pinnacle_2021.Contracts.Requests;
using Pinnacle_2021.Contracts.Responses;

namespace Pinnacle_2021.Api.Services.Domain
{
	public interface IItemService
	{
		Task<OneOf<AddOrConsumeItemResponse, EntityNotFound>> Create(Guid inventoryId, AddItemRequest itemRequest);
		Task<AddOrConsumeItemResponse> Consume(Guid inventoryItemId, ConsumeItemRequest consumeRequest);
		Task ChangeQuantity(Guid inventoryItemId, ChangeItemRequest changeQuantityRequest);
		Task<ItemDetailResponse> GetItem(Guid inventoryId, Guid itemId);
	}
}