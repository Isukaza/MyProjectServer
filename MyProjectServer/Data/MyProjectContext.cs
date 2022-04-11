#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Data
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
                .HasForeignKey(p => p.CompanyId);

            modelBuilder.Entity<StaffProfile>()
                .HasOne(p => p.Staff)
                .WithOne(p => p.Profile)
                .HasForeignKey<Staff>(p => p.ProfileId);
            
            //Данные для базовго заполнения БД
           modelBuilder.Entity<Staff>().HasData(
                new Staff[]{
                    new Staff { Id = 1, Name="Tom", Depts = null, CompanyId =1, ProfileId = 1 },
                    new Staff { Id = 2, Name="Alice", Depts = null, CompanyId =2, ProfileId = 2 }
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
                    new StaffProfile { Id = 1, Login="Tom", Password = "123dd3"},
                    new StaffProfile { Id = 2, Login="Alice", Password = "3g223g32"}
               });
           
        }
        
        public static DbContextOptions<MyProjectContext> CreateOption()
        {
            ConfigurationBuilder builder = new();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла dbsetting.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            IConfigurationRoot config = builder.Build();

            return new DbContextOptionsBuilder<MyProjectContext>().UseSqlServer(config.GetConnectionString("MyProjectContext")).Options;
        }
    }
}
