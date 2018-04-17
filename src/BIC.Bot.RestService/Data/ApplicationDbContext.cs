using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BIC.Bot.RestService.Models;
using BIC.Bot.RestService.Data.Entities;

namespace BIC.Bot.RestService.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BotLogin> BotLogin { get; set; }

        public DbSet<AuthyAuthorization> AuthyAuthorization { get; set; }

        public DbSet<Bill> Bill { get; set; }

        public DbSet<BotProactiveMessages> BotProactiveMessage { get; set; }

        public DbSet<SmsUser> SmsUser { get; set; }
        public DbSet<SmsLogin> SmsLogin { get; set; }

        public DbSet<DirectLineUser> DirectLineUser { get; set; }
        public DbSet<DirectLineLogins> DirectLineLogins { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BotLogin>().ToTable("BotLogins");
            builder.Entity<AuthyAuthorization>().ToTable("AuthyAuthorizatios");
            builder.Entity<Bill>().ToTable("Bill");
            builder.Entity<BotProactiveMessages>().ToTable("BotProactiveMessages");
            builder.Entity<SmsUser>().ToTable("SmsUsers");
            builder.Entity<SmsLogin>().ToTable("SmsLogins");
            builder.Entity<DirectLineUser>().ToTable("DirectLineUsers");
            builder.Entity<DirectLineLogins>().ToTable("DirectLineLogins");



            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
