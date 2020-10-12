using BoodschappenAPI.Models;
using IngredientDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BoodschappenAPI.Controllers
{
    public class BoodschapIngredientController : ApiController
    {

        // GET api/values
        public IEnumerable<BoodschapIngredientModel> Get()
        {
            List<BoodschapIngredient> boodschapLijst = new List<BoodschapIngredient>();
            List<BoodschapIngredientModel> boodschapModelLijst = new List<BoodschapIngredientModel>();
            using (DBingredient context = new DBingredient())
            {
             
                boodschapLijst = context.BoodschapIngredients.ToList(); 
                
                foreach (BoodschapIngredient boodschapIngredient in boodschapLijst)
                {
                    BoodschapIngredientModel model = new BoodschapIngredientModel();
                    model.BoodschapIngredientID = boodschapIngredient.BoodschapIngredientID;
                    model.naam = boodschapIngredient.ingredient.name;
                    model.merk = boodschapIngredient.ingredient.merk;
                    model.Hoeveelheid = boodschapIngredient.Hoeveelheid;
                    model.Eenheid = boodschapIngredient.Eenheid;
                    boodschapModelLijst.Add(model);
                }
            }
                
            

           return boodschapModelLijst;
        }

        // GET api/values/5
        public IEnumerable<BoodschapIngredientModel>  Get(int id)
        {
            List<BoodschapIngredient> boodschapLijst = new List<BoodschapIngredient>();
            List<BoodschapIngredientModel> boodschapModelLijst = new List<BoodschapIngredientModel>();
            using (DBingredient context = new DBingredient())
            {
                User user = context.Users.Find(id);
                int boodschapLijstID = user.boodschapLijst.BoodschapLijstID;
                BoodschapLijst boodschaplijstUser = context.BoodschapLijsts.Find(boodschapLijstID);
                boodschapLijst = boodschaplijstUser.BoodschapIngredients;

                foreach (BoodschapIngredient boodschapIngredient in boodschapLijst)
                {
                    BoodschapIngredientModel model = new BoodschapIngredientModel();
                    model.BoodschapIngredientID = boodschapIngredient.BoodschapIngredientID;
                    model.naam = boodschapIngredient.ingredient.name;
                    model.merk = boodschapIngredient.ingredient.merk;
                    model.Hoeveelheid = boodschapIngredient.Hoeveelheid;
                    model.Eenheid = boodschapIngredient.Eenheid;
                    boodschapModelLijst.Add(model);
                }
            }



            return boodschapModelLijst;
        }
    }
}
