using System;
using System.ComponentModel.DataAnnotations.Schema;

using Pinnacle_2021.Api.Models.Base;

namespace Pinnacle_2021.Api.Entities
{
	[Table("InventoryItem")]
	public class InventoryItem : BaseEntityWithKey
	{
		public Guid InventoryId { get; set; }
		public Guid ItemId { get; set; }
		public int Quantity { get; set; }

		public DateTime? Expiration { get; set; }

		#region

		[ForeignKey(nameof(ItemId))]
		public virtual Item Item { get; set; }

		[ForeignKey(nameof(InventoryId))]
		public virtual Inventory Inventory { get; set; }

		#endregion
	}
}
