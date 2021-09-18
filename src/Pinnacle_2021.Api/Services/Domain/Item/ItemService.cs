
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Pinnacle_2021.Api.DAL;
using Pinnacle_2021.Api.Services.System;
using Pinnacle_2021.Contracts.Responses;

namespace Pinnacle_2021.Api.Services.Domain
{
	public class ItemService : BaseDomainService, IItemService
	{
		public ItemService(IMapper mapper, PinnacleContext context)
			: base(mapper, context)
		{

		}

		public async Task<ItemResponse> Get(string upc)
		{
			var item = await Context.Items.Where(i => i.UPC.ToLower() == upc.ToLower()).FirstOrDefaultAsync();
			return Mapper.Map<ItemResponse>(item);
			//TODO: Get and Create from external API.
		}
	}
}
