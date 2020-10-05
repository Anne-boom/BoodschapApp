using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngredientDB
{
    public class Inventory
    {
        [Key]
        public int InventoryID { get; set; }
        public virtual List<InventoryIngredient> InventoryIngredients { get; set; }

        public string comments { get; set; }
    }
}
