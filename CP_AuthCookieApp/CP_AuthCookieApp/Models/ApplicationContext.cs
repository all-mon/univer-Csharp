using Microsoft.EntityFrameworkCore;

namespace CP_AuthCookieApp.Models
{
    //Данный класс наследуется от DbContext, к-рый имеет необходимые для работы с БД методы
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //если БД еще не существует, то она создается
            Database.EnsureCreated();
        }
        //Для инициализации базы данных в методе OnModelCreating()
        //добавляются в бд две роли и один пользователь - администратора.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "admin@mail.ru";
            string adminPassword = "123456";

            // добавляем роли
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            User adminUser = new User { Id = 1, Email = adminEmail,
                Password = adminPassword, RoleId = adminRole.Id ,
                DateOfRegistration = System.DateTime.Now };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}

