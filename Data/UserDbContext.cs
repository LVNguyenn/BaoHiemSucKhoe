using InsuranceManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.Data
{
    public class UserDbContext:DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        #region DbSet
        public DbSet<User> users { get; set; }
        public DbSet<Insurance> insurances { get; set; }
        public DbSet<Purchase> purchases { get; set; }
        public DbSet<Feedback> feedbacks { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.ToTable("purchases");
                entity.HasKey(e => new { e.id, e.userID});

                entity.HasOne(e => e.User)
                    .WithMany(e => e.Purchases)
                    .HasForeignKey(e => e.userID)
                    .HasConstraintName("FK_Purchase_User");

                entity.HasOne(e => e.Insurance)
                    .WithMany(e => e.Purchases)
                    .HasForeignKey(e => e.id)
                    .HasConstraintName("FK_Purchase_Insurance");
            });
        }
    }
}
