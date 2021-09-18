
using System;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using HtmlAgilityPack;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using OneOf;

using Pinnacle_2021.Api.DAL;
using Pinnacle_2021.Api.Entities;
using Pinnacle_2021.Api.Models;
using Pinnacle_2021.Api.Services.System;
using Pinnacle_2021.Contracts.Requests;
using Pinnacle_2021.Contracts.Responses;

using Unsplash;

namespace Pinnacle_2021.Api.Services.Domain
{
	public class ItemService : BaseDomainService, IItemService
	{
		private readonly IUpcApiClient _upcApiClient;
		private readonly IImageApiClient _imageApiClient;

		public ItemService(IMapper mapper, PinnacleContext context, IUpcApiClient upcApiClient, IImageApiClient imageApiClient)
			: base(mapper, context)
		{
			_upcApiClient = upcApiClient;
			_imageApiClient = imageApiClient;
		}

		#region Get

		//Get Or Create Items
		private async Task<Item> GetByUpc(string upc)
		{
			return await Context.Items.Where(i => i.UPC == upc).FirstOrDefaultAsync();
		}

		private async Task<Item> GetByTitle(string title)
		{
			return await Context.Items.Where(i => i.Title.ToLower() == title.ToLower().Trim())
									  .FirstOrDefaultAsync();
		}

		private async Task<OneOf<Item, EntityNotFound>> GetOrCreateItem(AddItemRequest itemRequest)
		{
			OneOf<Item, EntityNotFound> oneOfResponse;
			if (!string.IsNullOrEmpty(itemRequest.Upc))
				oneOfResponse = await GetOrCreateItemByUpc(itemRequest.Upc);
			else
				oneOfResponse = await GetOrCreateItemByName(itemRequest.Title!);
			return oneOfResponse;
		}

		private async Task<OneOf<Item, EntityNotFound>> GetOrCreateItemByName(string title)
		{
			var item = await GetByTitle(title);
			if (item == null)
			{
				try
				{
					item = await _imageApiClient.GetImage(title);
					await CreateItem(item);
				}
				catch (Exception)
				{
					return new EntityNotFound();
				}
			}
			return item;
		}

		private async Task<OneOf<Item, EntityNotFound>> GetOrCreateItemByUpc(string upc)
		{
			var item = await GetByUpc(upc);
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

		private async Task<InventoryItem> GetItem(Guid inventoryItemId)
		{
			return await Context.InventoryItems.FindAsync(inventoryItemId);
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
			var oneOfResponse = await GetOrCreateItem(itemRequest);

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

		#region Patch

		public async Task ChangeQuantity(Guid inventoryItemId, ChangeQuantityRequest changeQuantityRequest)
		{
			var invItem = await GetItem(inventoryItemId);
			invItem.Quantity = changeQuantityRequest.Quantity;
			await Context.SaveChangesAsync();
		}

		#endregion

		#region Delete

		public async Task Consume(Guid inventoryItemId, ConsumeItemRequest consumeRequest)
		{
			var inventoryItem = await GetItem(inventoryItemId);
			if (inventoryItem.Quantity > consumeRequest.Quantity)
			{
				inventoryItem.Quantity -= consumeRequest.Quantity;
			}
			else
			{
				inventoryItem.Quantity = 0;
			}
			await Context.SaveChangesAsync();
		}

		#endregion
	}

	#region APIs
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

	public class ImageApiClient : IImageApiClient
	{
		private readonly UnsplashClient _client;

		public ImageApiClient(IConfiguration configuration)
		{
			_client = new UnsplashClient(new ClientOptions
			{
				AccessKey = configuration["Unsplash:ApiKey"]
			});
		}

		public async Task<Item> GetImage(string title)
		{

			var img = await _client.Search.PhotosAsync(title);

			return new Item
			{
				Title = title,
				Image = img.Results.First().Urls.Regular
			};
		}
	}

	public interface IImageApiClient
	{
		Task<Item> GetImage(string title);
	}
	#endregion
}
