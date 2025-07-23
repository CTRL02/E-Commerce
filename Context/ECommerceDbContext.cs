using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using E_Commerce.Models;

namespace E_Commerce.Context
{
    public class ECommerceDbContext : IdentityDbContext<User>
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<ProductConfiguration> ProductConfigurations { get; set; }
        public DbSet<ProductVariationCategory> ProductVariationCategories { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionCategory> PromotionCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }
        public DbSet<Variation> Variations { get; set; }
        public DbSet<VariationOption> VariationOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.ProductItem)
                .WithMany(pi => pi.CartItems)
                .HasForeignKey(ci => ci.ProductItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductItem>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductItems)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductItem>()
                .HasOne(pi => pi.Seller)
                .WithMany(s => s.ProductItems)
                .HasForeignKey(pi => pi.SellerId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<VariationOption>()
                .HasOne(vo => vo.Variation)
                .WithMany(v => v.VariationOptions)
                .HasForeignKey(vo => vo.VariationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductConfiguration>()
                .HasOne(pc => pc.ProductItem)
                .WithMany(pi => pi.ProductConfigurations)
                .HasForeignKey(pc => pc.ProductItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductConfiguration>()
                .HasOne(pc => pc.VariationOption)
                .WithMany(vo => vo.ProductConfigurations)
                .HasForeignKey(pc => pc.VariationOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductVariationCategory>()
                .HasKey(pvc => new { pvc.CategoryId, pvc.VariationId });

            modelBuilder.Entity<ProductVariationCategory>()
                .HasOne(pvc => pvc.Category)
                .WithMany(c => c.ProductVariationCategories)
                .HasForeignKey(pvc => pvc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductVariationCategory>()
                .HasOne(pvc => pvc.Variation)
                .WithMany(v => v.productVariationCategories)
                .HasForeignKey(pvc => pvc.VariationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PromotionCategory>()
                .HasKey(pc => new { pc.PromotionId, pc.CategoryId});

            modelBuilder.Entity<ProductConfiguration>()
                .HasOne(pc => pc.VariationOption)
                .WithMany(vo => vo.ProductConfigurations)
                .HasForeignKey(pc => pc.VariationOptionId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ShippingAddress>()
                  .HasOne(ua => ua.User)
                  .WithMany(u => u.Addresses)
                  .HasForeignKey(ua => ua.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // UserAddress - Country (Many-to-One)
            modelBuilder.Entity<ShippingAddress>()
                .HasOne(ua => ua.Country)
                .WithMany(c => c.UserAddresses)
                .HasForeignKey(ua => ua.CountryId)
                .OnDelete(DeleteBehavior.Cascade);


            // UserPaymentMethod - User (Many-to-One)
            modelBuilder.Entity<PaymentMethod>()
                .HasOne(upm => upm.User)
                .WithMany(u => u.PaymentMethods)
                .HasForeignKey(upm => upm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserPaymentMethod - PaymentType (Many-to-One)
            modelBuilder.Entity<PaymentMethod>()
                .HasOne(upm => upm.PaymentType)
                .WithMany(pt => pt.UserPaymentMethods)
                .HasForeignKey(upm => upm.PaymentTypeId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Order>()
                .HasOne(so => so.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(so => so.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(so => so.PaymentMethod)
                .WithMany(pm => pm.orders)
                .HasForeignKey(so => so.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(so => so.ShippingAddress)
                .WithMany(sa => sa.orders)
                .HasForeignKey(so => so.ShippingAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(so => so.ShippingMethod)
                .WithMany(sm => sm.Orders)
                .HasForeignKey(so => so.ShippingMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(so => so.OrderStatus)
                .WithMany(os => os.Orders)
                .HasForeignKey(so => so.OrderStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderLine>()
                .HasOne(ol => ol.Order)
                .WithMany(so => so.OrderLines)
                .HasForeignKey(ol => ol.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderLine>()
                .HasOne(ol => ol.ProductItem)
                .WithMany(pi => pi.OrderLines)
                .HasForeignKey(ol => ol.ProductItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserReview - User (Many-to-One)
            modelBuilder.Entity<Review>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserReview - OrderLine (Many-to-One)
            modelBuilder.Entity<Review>()
                .HasOne(ur => ur.OrderLine)
                .WithMany(ol => ol.Reviews)
                .HasForeignKey(ur => ur.OrderLineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
