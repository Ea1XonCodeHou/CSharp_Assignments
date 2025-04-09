using System;
using Microsoft.EntityFrameworkCore;
using Assignment_6.Models;

namespace Assignment_6.Data
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 配置MySQL连接字符串
            string connectionString = "server=localhost;port=3306;database=order_system;user=root;password=Hyc20040825";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 配置Order和OrderDetail之间的关系
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Details)
                .WithOne(d => d.Order)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            base.OnModelCreating(modelBuilder);
        }
    }
} 