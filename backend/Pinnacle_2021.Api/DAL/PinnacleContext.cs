
using Microsoft.EntityFrameworkCore;

using Pinnacle_2021.Api.Models;

namespace Pinnacle_2021.Api.DAL
{
	public class PinnacleContext : DbContext
	{
		public virtual DbSet<User> Users { get; set; }

		public PinnacleContext(DbContextOptions<PinnacleContext> options) : base(options)
		{
		}
	}
}
