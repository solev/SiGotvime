namespace Food_App_Service.Migrations
{
    using SiGotvime.Data;
    using SiGotvime.Data.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FoodDatabase>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(FoodDatabase context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //



            context.Ingredients.AddOrUpdate(ing => ing.Name,
                    new Ingredient { Name = "Сол" },
                    new Ingredient { Name = "Бибер" },
                    new Ingredient { Name = "Оригано" },
                    new Ingredient { Name = "Јајца" },
                    new Ingredient { Name = "Шеќер" },
                    new Ingredient { Name = "Масло" },
                    new Ingredient { Name = "Кашкавал" },
                    new Ingredient { Name = "Сирење" },
                    new Ingredient { Name = "Млеко" },
                    new Ingredient { Name = "Јогурт" },
                    new Ingredient { Name = "Кисела Павлака" },
                    new Ingredient { Name = "Блага Павлака" },
                    new Ingredient { Name = "Хопла" },
                    new Ingredient { Name = "Брашно" },
                    new Ingredient { Name = "Лебни трошки" },
                    new Ingredient { Name = "Цимет" },
                    new Ingredient { Name = "Шеќер во прав" },
                    new Ingredient { Name = "Домати" },
                    new Ingredient { Name = "Краставици" },
                    new Ingredient { Name = "Лук" },
                    new Ingredient { Name = "Кромид" },
                    new Ingredient { Name = "Магдонос" }
                 );
        }
    }
}
