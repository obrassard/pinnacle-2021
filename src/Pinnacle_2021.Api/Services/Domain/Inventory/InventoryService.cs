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

		#region Select
		private Expression<Func<Inventory, InventoryResponse>> GetSelectInventories()
		{
			return i => new InventoryResponse
			{
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
