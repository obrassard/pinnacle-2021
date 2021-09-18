using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Pinnacle_2021.Api.Models.Base;

namespace Pinnacle_2021.Api.Models
{
	[Table("Recipe")]
	public class Recipe : BaseEntityWithKey
	{
		[MaxLength(255)]
		public string Title { get; set; }

		[MaxLength(255)]
		public string Link { get; set; }

		#region

		public virtual ICollection<RecipeItem> RecipeItems { get; set; }

		#endregion
	}
}
