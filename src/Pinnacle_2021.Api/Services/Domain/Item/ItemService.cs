
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using AutoMapper;

using HtmlAgilityPack;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

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

		public async Task<ItemDetailResponse> GetItem(Guid inventoryId, Guid itemId)
		{
			return await Context.Items.Where(i => i.Id == itemId)
							.Select(i => new ItemDetailResponse
							{
								ItemId = i.Id,
								Image = i.Image,
								Title = i.Title,
								Inventory = i.InventorieItems
												.Where(ii => ii.InventoryId == inventoryId && ii.Quantity > 0)
												.Select(ii => new InventoryItemDetail
												{
													AddedAt = ii.CreatedAt,
													Expiration = ii.Expiration,
													InvItemID = ii.Id,
													Quantity = ii.Quantity
												})
							}).FirstOrDefaultAsync();
		}

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
					item = await _upcApiClient.ScrapeUPCData(upc);
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

			if (!invItem.Expiration.HasValue && item.TTL.HasValue)
				invItem.Expiration = DateTime.UtcNow.AddDays(item.TTL.Value);

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

		public async Task ChangeQuantity(Guid inventoryItemId, ChangeItemRequest changeItemRequest)
		{
			var invItem = await GetItem(inventoryItemId);
			if (changeItemRequest.Quantity.HasValue)
				invItem.Quantity = changeItemRequest.Quantity.Value;

			if (changeItemRequest.Expiration.HasValue)
				invItem.Expiration = changeItemRequest.Expiration;

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
		private readonly IImageApiClient _imageApiClient;

		public UpcApiClient(IImageApiClient imageApiClient)
		{
			_imageApiClient = imageApiClient;
		}

		public async Task<Item> ScrapeUPCData(string upcCode)
		{
			HtmlWeb web = new();
			HtmlDocument doc = web.Load($"https://www.upcitemdb.com/upc/{upcCode}");


			var name = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div[1]/div[3]/div/ol/li[1]").InnerText;
			var imgSrc = await _imageApiClient.GetImageUrl(name);


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

		public HttpClient _bingClient { get; }
		public string _baseUri { get; }

		public ImageApiClient(IConfiguration configuration)
		{
			_client = new UnsplashClient(new ClientOptions
			{
				AccessKey = configuration["Unsplash:ApiKey"]
			});

			string subscriptionKey = configuration["BingSearchApiKey"];
			_bingClient = new HttpClient();

			// Request headers. The subscription key is the only required header but you should
			// include User-Agent (especially for mobile), X-MSEdge-ClientID, X-Search-Location
			// and X-MSEdge-ClientIP (especially for local aware queries).

			_bingClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

			_baseUri = "https://api.bing.microsoft.com/v7.0/images/search";
		}

		public async Task<string> GetImageUrl(string title)
		{
			try
			{
				var item = await _bingClient.GetAsync(_baseUri + "?q=" + title);
				var body = await item.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<ImageResponse>(body);
				return result.Value.First().ThumbnailUrl;
			}
			catch (Exception)
			{
				Console.WriteLine("An error occured in GetImageUrl");
				return "";
			}
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

	public record ImageResponse
	{
		[JsonProperty("value")]
		public List<ImageResponseItem> Value { get; set; }
	}

	public record ImageResponseItem
	{
		[JsonProperty("thumbnailUrl")]
		public string ThumbnailUrl { get; set; }
	}

	public interface IImageApiClient
	{
		Task<Item> GetImage(string title);
		Task<string> GetImageUrl(string title);
	}
	#endregion
}
