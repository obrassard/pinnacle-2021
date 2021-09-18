using System;
using System.ComponentModel.DataAnnotations;

namespace Pinnacle_2021.Api.Models.Base
{
	public class BaseEntityWithKey : BaseEntity
	{
		[Key]
		public Guid Id { get; set; }
	}
}
