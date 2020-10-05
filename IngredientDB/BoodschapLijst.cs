using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngredientDB
{
    public class BoodschapLijst
    {
        public int BoodschapLijstID { get; set; }
        public virtual List<BoodschapIngredient> BoodschapIngredients { get; set; }

        public string comments { get; set; }
    }
}
