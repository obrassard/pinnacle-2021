using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Pinnacle_2021.Api.Models.Base;

namespace Pinnacle_2021.Api.Entities
{
	[Table("Item")]
	public class Item : BaseEntityWithKey
	{
		public string? UPC { get; set; }
		public string Title { get; set; }
		public int QuantityInPackage { get; set; }
		public Guid? ChildItemId { get; set; }

		#region

		[ForeignKey(nameof(ChildItemId))]
		public Item? ChildItem { get; set; }

		public virtual ICollection<RecipeItem> RecipeItems { get; set; }
		public virtual ICollection<InventoryItem> InventorieItems { get; set; }


		#endregion
	}
}
