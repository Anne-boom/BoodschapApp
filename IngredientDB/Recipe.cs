using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngredientDB
{
    public class Recipe
    {
        [Key]
        public int RecipeID { get; set; }
        [Required]
        public string Naam { get; set; }
       virtual public List<RecipeIngredient> RecipeIngredients { get; set; }

        public string Beschrijving { get; set; }
        public double Calorien { get; set; }

        virtual public User user { get; set; }

      

    }
}
