#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyProjectServer.Models;

namespace MyProjectServer.Data
{
    public class MyProjectContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<StaffProfile> StaffProfiles { get; set; }
        public DbSet<Dept> Depts { get; set; }

        public MyProjectContext(DbContextOptions<MyProjectContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            //Описание первичных ключей 
            modelBuilder.Entity<Company>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Staff>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<StaffProfile>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Dept>()
                .HasKey(c => c.Id);

            //Описание связей таблиц
            modelBuilder.Entity<Staff>()
                .HasOne(p => p.Company)
                .WithMany(p => p.Staffs)
                .HasForeignKey(p => p.CompanyForeignKey);

            modelBuilder.Entity<Staff>()
                .HasOne(p => p.Profile)
                .WithOne(p => p.Staff)
                .HasForeignKey<StaffProfile>(p => p.StaffForeignKey);
            
            //Данные для базовго заполнения БД
           modelBuilder.Entity<Staff>().HasData(
                new Staff[]{
                    new Staff { Id = 1, Name="Tom", Depts = null, CompanyForeignKey =1},
                    new Staff { Id = 2, Name="Alice", Depts = null, CompanyForeignKey =2}
                });


            modelBuilder.Entity<Company>().HasData(
                new Company[]{
                    new Company{ Id = 1, Name = "Microsoft"},
                    new Company{ Id = 2, Name = "Google" }
                });

            modelBuilder.Entity<Dept>().HasData(
                new Dept[] {
                    new Dept { Id = 1, Department = "Programmer" },
                    new Dept { Id = 2, Department = "Designer" }
                });

            modelBuilder.Entity<StaffProfile>().HasData(
               new StaffProfile[]{
                    new StaffProfile { Id = 1, Login="Tom", Password = "123dd3", StaffForeignKey = 1},
                    new StaffProfile { Id = 2, Login="Alice", Password = "3g223g32", StaffForeignKey = 2}
               });
           
        }
        
        public static DbContextOptions<MyProjectContext> CreateOption()
        {
            ConfigurationBuilder builder = (ConfigurationBuilder)new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            // создаем конфигурацию
            IConfigurationRoot config = builder.Build();

            return new DbContextOptionsBuilder<MyProjectContext>().UseSqlServer(config.GetConnectionString("MyProjectContext")).Options;
        }
    }
}
