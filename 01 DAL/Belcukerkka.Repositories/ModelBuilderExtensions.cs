using Belcukerkka.Models.Entities;
using Belcukerkka.Security;
using Microsoft.EntityFrameworkCore;

namespace Belcukerkka.Repositories
{
    internal static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoxPackage>().HasData(
                    new BoxPackage { Id = 1, Name = "Картон" },
                    new BoxPackage { Id = 2, Name = "Текстиль" },
                    new BoxPackage { Id = 3, Name = "Дерево" },
                    new BoxPackage { Id = 4, Name = "Туба" },
                    new BoxPackage { Id = 5, Name = "Мягкая игрушка" }
                );

            modelBuilder.Entity<WeightType>().HasData(
                    new WeightType { Id = 1, Name = "Классический" },
                    new WeightType { Id = 2, Name = "Премиум" },
                    new WeightType { Id = 3, Name = "Эксклюзивный" },
                    new WeightType { Id = 4, Name = "Белорусский" }
                );
        }

        public static void SeedUsers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                    new User { Id = 1, UserName = "devadmin", Password = PasswordHandler.HashPassword("[ENTER ANY SUITABLE PASSWORD AND IT WILL BE HASHED]") },
                    new User { Id = 2, UserName = "admin", Password = PasswordHandler.HashPassword("[ENTER ANY SUITABLE PASSWORD AND IT WILL BE HASHED]") }
                );
        }
    }
}
