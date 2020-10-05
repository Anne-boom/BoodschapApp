using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngredientDB
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string inlognaam { get; set; }
        public string wachtwoord { get; set; }

       virtual public Inventory inventory { get; set; }

        virtual public BoodschapLijst boodschapLijst { get; set; }
    }
}
