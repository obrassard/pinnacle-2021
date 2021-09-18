using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Pinnacle_2021.Api.Models.Base;

namespace Pinnacle_2021.Api.Entities
{
	[Table("Inventory")]
	public class Inventory : BaseEntityWithKey
	{
		public Guid UserId { get; set; }

		[MaxLength(255)]
		public string Title { get; set; }

		#region Relations

		[ForeignKey(nameof(UserId))]
		public User User { get; set; }

		public virtual ICollection<InventoryItem> InventoryItems { get; set; }

		#endregion
	}
}
