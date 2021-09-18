
using Microsoft.EntityFrameworkCore;

using Pinnacle_2021.Api.Models;

namespace Pinnacle_2021.Api.DAL
{
	public class PinnacleContext : DbContext
	{
		public virtual DbSet<Item> Items { get; set; }
		public virtual DbSet<Inventory> Inventories { get; set; }
		public virtual DbSet<InventoryItem> InventoryItems { get; set; }
		public virtual DbSet<Recipe> Recipes { get; set; }
		public virtual DbSet<RecipeItem> RecipeItems { get; set; }
		public virtual DbSet<User> Users { get; set; }

		public PinnacleContext(DbContextOptions<PinnacleContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Items
			modelBuilder.Entity<Item>(entity =>
			{
				entity.Property(i => i.QuantityInPackage).HasDefaultValue(1);
			});

			modelBuilder.Entity<InventoryItem>(entity =>
			{
				entity.Property(i => i.Quantity).HasDefaultValue(1);
				entity.Property(i => i.Consumed).HasDefaultValue(false);
			});

			base.OnModelCreating(modelBuilder);
		}
	}
}
