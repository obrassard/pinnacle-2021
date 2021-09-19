using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Pinnacle_2021.Api.DAL;
using Pinnacle_2021.Api.Entities;
using Pinnacle_2021.Api.Services.System;
using Pinnacle_2021.Contracts.Requests;
using Pinnacle_2021.Contracts.Responses;

namespace Pinnacle_2021.Api.Services.Domain
{
	public class InventoryService : BaseDomainService, IInventoryService
	{
		public InventoryService(IMapper mapper, PinnacleContext pinnacleContext)
			: base(mapper, pinnacleContext)
		{

		}

		#region Get

		public async Task<IEnumerable<InventoryResponse>> GetAll(Guid userId)
		{
			var inventories = await Context.Inventories.Where(i => i.UserId == userId)
										.Select(GetSelectInventories())
										.ToListAsync();
			return inventories;
		}

		public async Task<InventoryDetailResponse> Get(Guid inventoryId, bool includeExpiredItems = false)
		{
			var title = await Context.Inventories.Where(i => i.Id == inventoryId).Select(i => i.Title).FirstOrDefaultAsync();

			var items = await Context.Items.Where(ii => ii.InventorieItems.Any(c => c.InventoryId == inventoryId))
											.Select(ii => new ItemListResponse
											{
												Title = ii.Title,
												Image = ii.Image,
												ItemId = ii.Id,
												ExpiredSoon = ii.InventorieItems.Any(i => i.Expiration.HasValue && i.Expiration.Value >= DateTime.UtcNow.AddDays(3)),
												Count = ii.InventorieItems.Where(i => i.InventoryId == inventoryId).Sum(i => i.Quantity)
											}).ToListAsync();

			return new InventoryDetailResponse
			{
				Title = title,
				Items = items
			};
		}

		#region Select
		private Expression<Func<Inventory, InventoryResponse>> GetSelectInventories()
		{
			return i => new InventoryResponse
			{
				Id = i.Id,
				Title = i.Title,
				CountOfItems = i.InventoryItems.Count
			};
		}
		#endregion

		#endregion

		#region Post

		public async Task<InventoryCreationResponse> Create(Guid userId, InventoryForCreation request)
		{
			var inventory = Mapper.Map<Inventory>(request);
			inventory.UserId = userId;
			await Context.Inventories.AddAsync(inventory);
			await Context.SaveChangesAsync();

			return Mapper.Map<InventoryCreationResponse>(inventory);
		}

		#endregion
	}
}
