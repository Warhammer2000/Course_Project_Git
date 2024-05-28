using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CourseProjectItems.Models;

namespace CourseProjectItems.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<Collection> Collections { get; set; }
		public DbSet<Item> Items { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Like> Likes { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<TagConnection> TagConnections { get; set; }
		public DbSet<StyleConnections> StyleConnections { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			

			modelBuilder.Entity<Collection>()
				.HasMany(c => c.Items)
				.WithOne(i => i.Collection)
				.HasForeignKey(i => i.CollectionId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Item>()
				.HasOne(i => i.Collection)
				.WithMany(c => c.Items)
				.HasForeignKey(i => i.CollectionId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Item>()
				.HasMany(i => i.Comments)
				.WithOne(c => c.Item)
				.HasForeignKey(c => c.ItemId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Item>()
				.HasMany(i => i.Likes)
				.WithOne(l => l.Item)
				.HasForeignKey(l => l.ItemId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
