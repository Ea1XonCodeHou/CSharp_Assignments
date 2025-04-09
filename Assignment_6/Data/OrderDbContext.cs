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
            // ����MySQL�����ַ���
            string connectionString = "server=localhost;port=3306;database=order_system;user=root;password=Hyc20040825";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ����Order��OrderDetail֮��Ĺ�ϵ
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Details)
                .WithOne(d => d.Order)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            base.OnModelCreating(modelBuilder);
        }
    }
} 