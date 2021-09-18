
using System;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using HtmlAgilityPack;

using Microsoft.EntityFrameworkCore;

using OneOf;

using Pinnacle_2021.Api.DAL;
using Pinnacle_2021.Api.Entities;
using Pinnacle_2021.Api.Models;
using Pinnacle_2021.Api.Services.System;
using Pinnacle_2021.Contracts.Requests;
using Pinnacle_2021.Contracts.Responses;

namespace Pinnacle_2021.Api.Services.Domain
{
	public class ItemService : BaseDomainService, IItemService
	{
		private readonly IUpcApiClient _upcApiClient;

		public ItemService(IMapper mapper, PinnacleContext context, IUpcApiClient upcApiClient)
			: base(mapper, context)
		{
			_upcApiClient = upcApiClient;
		}

		#region Get

		private async Task<Item> Get(string upc)
		{
			return await Context.Items.Where(i => i.UPC == upc).FirstOrDefaultAsync();
		}

		private async Task<OneOf<Item, EntityNotFound>> GetOrCreateItem(string upc)
		{
			var item = await Get(upc);
			if (item == null)
			{
				try
				{
					item = _upcApiClient.ScrapeUPCData(upc);
					await CreateItem(item);
				}
				catch (Exception)
				{
					return new EntityNotFound();
				}
			}
			return item;
		}

		#endregion

		#region Post

		private async Task CreateItem(Item item)
		{
			await Context.Items.AddAsync(item);
			await Context.SaveChangesAsync();
		}

		public async Task<OneOf<AddItemResponse, EntityNotFound>> Create(Guid inventoryId, AddItemRequest itemRequest)
		{
			var oneOfResponse = await GetOrCreateItem(itemRequest.Upc);
			if (oneOfResponse.IsT1)
				return oneOfResponse.AsT1;

			var item = oneOfResponse.AsT0;
			var invItem = Mapper.Map<InventoryItem>(itemRequest);
			invItem.InventoryId = inventoryId;
			invItem.ItemId = item.Id;

			await Context.InventoryItems.AddAsync(invItem);
			await Context.SaveChangesAsync();

			return new AddItemResponse
			{
				Id = invItem.Id,
				Image = item.Image,
				Title = item.Title
			};
		}

		#endregion
	}

	public class UpcApiClient : IUpcApiClient
	{
		public Item ScrapeUPCData(string upcCode)
		{
			HtmlWeb web = new();
			HtmlDocument doc = web.Load($"https://www.upcitemdb.com/upc/{upcCode}");


			var name = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div[1]/div[3]/div/ol/li[1]").InnerText;
			var imgSrc = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div[1]/div[1]/img").Attributes["src"].Value;


			return new Item
			{
				UPC = upcCode,
				Title = name,
				Image = imgSrc
			};
		}
	}
}
