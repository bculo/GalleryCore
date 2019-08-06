using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.EntityFramework
{
    /// <summary>
    /// Entityframework database context
    /// </summary>
    public class ImageGalleryDbContext : DbContext
    {
        public DbSet<Uploader> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ImageGalleryDbContext(DbContextOptions<ImageGalleryDbContext> options) : base(options) { }

        /// <summary>
        /// Prepare database model
        /// </summary>
        /// <param name="builder">instance of ModelBuilder class</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ImageGalleryDbContext).Assembly);
        }
    }
}
