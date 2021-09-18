using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Pinnacle_2021.Api.Models.Base;

namespace Pinnacle_2021.Api.Models
{
	[Table("User")]
	public class User : BaseEntityWithKey
	{
		[MaxLength(255)]
		public string LastName { get; set; }

		[MaxLength(255)]
		public string FirstName { get; set; }

		[MaxLength(255)]
		[EmailAddress]
		public string Email { get; set; }

		[MaxLength(15)]
		[Phone]
		public string PhoneNumber { get; set; }

		#region Relation

		public virtual ICollection<Inventory> Inventories { get; set; }

		#endregion
	}
}
