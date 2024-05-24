using Database.Models;

namespace Database.Data
{
    public static class Seed
    {
        public static async Task SeedData(DataContext context)
        {
            if (context.Tables.Any()) return;

            List<Table> tables = new List<Table>();

            const int numberOfTables = 37;

            string trait;
            for (int i = 0; i < numberOfTables; i++)
            {
                if (i < 4)
                {
                    trait = "Outdoor seating";
                } else if (i % 4 == 0)
                {
                    trait = "Window seat";
                } else
                {
                    List<string> traits = new List<string> { "Handicapped support", "Child support", "VIP" };
                    Random random = new Random();
                    trait = traits[random.Next(0, traits.Count)];
                }
                tables.Add(new Table { Id = i+1 ,Traits = trait });
            }
            await context.Tables.AddRangeAsync(tables);
            await context.SaveChangesAsync();
        }

        public static async Task SeedMenuItemCategory(DataContext context)
        {
            if (context.MenuItemCategory.Any()) return;

            List<MenuItemCategory> menuItemCategories = new List<MenuItemCategory>
            {
                new MenuItemCategory { Name = "Main" },
                new MenuItemCategory { Name = "Side" },
                new MenuItemCategory { Name = "Dessert" },
                new MenuItemCategory { Name = "Drinks" }
            };

            await context.MenuItemCategory.AddRangeAsync(menuItemCategories);
            await context.SaveChangesAsync();
        }
    }
}