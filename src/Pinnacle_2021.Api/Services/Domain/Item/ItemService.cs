
using System;
using System.Linq;
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
	public class ItemService : BaseDomainService, IItemService
	{
		public ItemService(IMapper mapper, PinnacleContext context)
			: base(mapper, context)
		{

		}

		#region Get

		public async Task<ItemResponse> Get(string upc)
		{
			var item = await Context.Items.Where(i => i.UPC == upc).FirstOrDefaultAsync();
			return Mapper.Map<ItemResponse>(item);
			//TODO: Get and Create from external API.
		}

		#endregion

		#region Post

		public async Task<AddItemResponse> Create(Guid inventoryId, AddItemRequest itemRequest)
		{
			var item = Mapper.Map<InventoryItem>(itemRequest);
			item.InventoryId = inventoryId;

			await Context.InventoryItems.AddAsync(item);
			await Context.SaveChangesAsync();

			return Mapper.Map<AddItemResponse>(item);
		}

		#endregion
	}
}
