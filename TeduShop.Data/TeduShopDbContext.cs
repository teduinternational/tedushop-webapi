using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using TeduShop.Model.Models;

namespace TeduShop.Data
{
    public class TeduShopDbContext : IdentityDbContext<AppUser>
    {
        public TeduShopDbContext() : base("TeduShopConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Footer> Footers { set; get; }
        public DbSet<Order> Orders { set; get; }
        public DbSet<OrderDetail> OrderDetails { set; get; }
        public DbSet<Page> Pages { set; get; }
        public DbSet<Post> Posts { set; get; }
        public DbSet<PostCategory> PostCategories { set; get; }
        public DbSet<PostTag> PostTags { set; get; }
        public DbSet<Product> Products { set; get; }

        public DbSet<ProductCategory> ProductCategories { set; get; }
        public DbSet<ProductTag> ProductTags { set; get; }
        public DbSet<Slide> Slides { set; get; }
        public DbSet<SupportOnline> SupportOnlines { set; get; }
        public DbSet<SystemConfig> SystemConfigs { set; get; }

        public DbSet<Tag> Tags { set; get; }

        public DbSet<VisitorStatistic> VisitorStatistics { set; get; }
        public DbSet<Error> Errors { set; get; }
        public DbSet<ContactDetail> ContactDetails { set; get; }
        public DbSet<Feedback> Feedbacks { set; get; }

        public DbSet<Function> Functions { set; get; }
        public DbSet<Permission> Permissions { set; get; }
        public DbSet<AppRole> AppRoles { set; get; }
        public DbSet<IdentityUserRole> UserRoles { set; get; }


        public DbSet<Color> Colors { set; get; }
        public DbSet<Size> Sizes { set; get; }
        public DbSet<ProductQuantity> ProductQuantities { set; get; }
        public DbSet<ProductImage> ProductImages { set; get; }

        public DbSet<Announcement> Announcements { set; get; }
        public DbSet<AnnouncementUser> AnnouncementUsers { set; get; }

        public static TeduShopDbContext Create()
        {
            return new TeduShopDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasKey<string>(r => r.Id).ToTable("AppRoles");
            builder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId }).ToTable("AppUserRoles");
            builder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("AppUserLogins");
            builder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("AppUserClaims");
        }
    }
}