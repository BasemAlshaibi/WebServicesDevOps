using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

  
        public DbSet<News> News { get; set; }

        public DbSet<Category> Categories { get; set; }

 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // التالي لانشاء العلاقة في النفجيشن بروبرتي بين جدول المستخدم وجدول اليوزر رولز وبالمثل
            // جدول الرول وجدول اليوزر رول
            builder.Entity<AppUser>()
             .HasMany(ur => ur.UserRoles)
             .WithOne(u => u.User)
             .HasForeignKey(ur => ur.UserId)
             .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            /*
الكود التالي يعمل لنا العلاقة بين جدول الاخبار وجدول المستخدم ومين العمود بالجدول ذا 
اللي يرتبط بالجدول الاخر
*/

            builder.Entity<News>()
                .HasOne(n => n.CreatedUser)
                .WithMany(m => m.CreatedUsers)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<News>()
                .HasOne(n => n.UpdatedUser)
                .WithMany(u => u.UpdatedUsers)
                .OnDelete(DeleteBehavior.Restrict);

 

        builder.HasSequence<int>("OrderCategory")
        .StartsAt(100)
        .IncrementsBy(5);

         builder.Entity<Category>()
        .Property(o => o.Order)
        .HasDefaultValueSql("NEXT VALUE FOR OrderCategory");

        }
    }
}
 
 