using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngredientDB
{
    public class DBingredient : DbContext
    {

        
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<InventoryIngredient> InventoryIngredients { get; set; }
        public DbSet<BoodschapIngredient> BoodschapIngredients { get; set; }
        public DbSet<RecipeIngredient> TotalRecipeIngredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<Inventory> Inventories { get; set; }

        public DbSet<BoodschapLijst> BoodschapLijsts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
