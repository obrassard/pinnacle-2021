using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Pinnacle_2021.Api.DAL;
using Pinnacle_2021.Api.Services.System;

using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Pinnacle_2021.Api.Services.Domain
{
	public class AlertService : BaseDomainService, IAlertService
	{
		public AlertService(IMapper mapper, PinnacleContext pinnacleContext, IConfiguration configuration)
			: base(mapper, pinnacleContext)
		{
			string accountSid = configuration["TWILIO_ACCOUNT_SID"];
			string authToken = configuration["TWILIO_AUTH_TOKEN"];

			TwilioClient.Init(accountSid, authToken);
		}

		public async Task AlertAll()
		{
			var users = await Context.Users
				.Where(u => !string.IsNullOrEmpty(u.PhoneNumber) &&
								u.Inventories.Any(i =>
									i.InventoryItems.Any(u =>
										u.Expiration.HasValue &&
										DateTime.UtcNow >= u.Expiration.Value.AddDays(3))
									)
								)
				.Select(u => new
				{
					PhoneNumber = u.PhoneNumber,
					InvalidProducts = u.Inventories
										.Where(inv => inv.InventoryItems.Any(
														ii => ii.Expiration.HasValue &&
														DateTime.UtcNow >= ii.Expiration.Value.AddDays(3)))
										.Select(inv => new
										{
											Title = inv.Title,
											Items = inv.InventoryItems
														.Where(i => i.Expiration.HasValue &&
															DateTime.UtcNow >= i.Expiration.Value.AddDays(3))
														.Select(ii => new
														{
															ii.Item.Title,
															ii.Expiration
														})
										})
				}).ToListAsync();

			foreach (var user in users)
			{
				StringBuilder strBuilder = new();
				strBuilder.AppendLine("Hello, this sms is a reminder that the following products are expired or about to expired.");

				foreach (var product in user.InvalidProducts)
				{
					strBuilder.AppendLine();
					strBuilder.AppendLine(product.Title);
					foreach (var inv in product.Items)
					{
						strBuilder.AppendLine($"| {inv.Title} - {inv.Expiration.Value.ToString("yyyy-MM-dd")}");
					}
				}

				var str = strBuilder.ToString();

				var message = MessageResource.Create(
					body: str,
					from: new Twilio.Types.PhoneNumber("+12138066877"),
					to: new Twilio.Types.PhoneNumber(user.PhoneNumber)
				);
			}

		}
	}
}
