using System;
using System.ComponentModel.DataAnnotations.Schema;

using Pinnacle_2021.Api.Models.Base;

namespace Pinnacle_2021.Api.Entities
{
	[Table("RecipeItem")]
	public class RecipeItem : BaseEntityWithKey
	{
		public Guid RecipeId { get; set; }
		public Guid ItemId { get; set; }

		public int Quantity { get; set; }

		#region Relation

		[ForeignKey(nameof(RecipeId))]
		public virtual Recipe Recipe { get; set; }

		[ForeignKey(nameof(ItemId))]
		public virtual Item Item { get; set; }

		#endregion
	}
}
