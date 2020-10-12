using IngredientDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace BoodschappenApp.Controllers
{
    public class BoodschapController : Controller
    {

        //private DBingredient db = new DBingredient();

        // GET: Boodschap
        public ActionResult Index()
        {
            User user = (User)Session["user"];

            if (user == null)
            {
                return RedirectToAction("Index", "User");
            }
            //DBingredient context = new DBingredient();
            using (DBingredient context = new DBingredient())
            {
                int boodschapLijstID = user.boodschapLijst.BoodschapLijstID;
                BoodschapLijst boodschapLijst = context.BoodschapLijsts.Find(boodschapLijstID);
                //List<BoodschapIngredient> lijst = context.BoodschapIngredients.ToList();

                if (boodschapLijst != null)
                {

                    foreach (BoodschapIngredient boodschapIngredient in boodschapLijst.BoodschapIngredients)
                    {
                        int ingredientID = boodschapIngredient.ingredient.ingredientID;
                        Ingredient ig = context.Ingredients.Find(ingredientID);
                        boodschapIngredient.ingredient = ig;
                    }
                    return View(boodschapLijst.BoodschapIngredients);
                }

                return View();
            }

        }


        [HttpPost]
        public ActionResult Index(string input)
        {
            User user = (User)Session["user"];

            if (user == null)
            {
                return RedirectToAction("Index", "User");
            }
            using (DBingredient context = new DBingredient())
            {

                int boodschapLijstID = user.boodschapLijst.BoodschapLijstID;

                BoodschapLijst boodschapLijst = context.BoodschapLijsts.Find(boodschapLijstID);
                //List<InventoryIngredient> lijst = context.Users.Fin;
                if (boodschapLijst != null)
                {
                    foreach (BoodschapIngredient boodschapIngredient in boodschapLijst.BoodschapIngredients)
                    {


                        int ingredientID = boodschapIngredient.ingredient.ingredientID;
                        Ingredient ig = context.Ingredients.Find(ingredientID);
                        boodschapIngredient.ingredient = ig;

                    }

                    List<BoodschapIngredient> filterNaam = boodschapLijst.BoodschapIngredients.Where(e => e.ingredient.name.Contains(input)).ToList();

                    List<BoodschapIngredient> filterMerk = boodschapLijst.BoodschapIngredients.Where(e => e.ingredient.merk.Contains(input)).ToList();

                    foreach (BoodschapIngredient boodschapIngredient2 in filterMerk)
                    {
                        int ID = boodschapIngredient2.BoodschapIngredientID;


                        if (!filterNaam.Exists(x => x.BoodschapIngredientID == ID))
                        {
                            filterNaam.Add(boodschapIngredient2);
                        }
                    }

                    return View(filterNaam);
                }

                return View();
            }
        }


        // GET: Ingredients/Create
        public ActionResult Toevoegen()
        {
            using (DBingredient context = new DBingredient())
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult Toevoegen(string input)
        {
            using (DBingredient context = new DBingredient())
            {
                List<Ingredient> lijstIngredients = context.Ingredients.ToList();

                List<Ingredient> filterNaam = lijstIngredients.Where(e => e.name.Contains(input)).ToList();
                List<Ingredient> filterMerk = lijstIngredients.Where(e => e.merk.Contains(input)).ToList();

                foreach (Ingredient ingredient in filterMerk)
                {
                    int ID = ingredient.ingredientID;

                    if (!filterNaam.Exists(x => x.ingredientID == ID))
                    {
                        filterNaam.Add(ingredient);
                    }
                }
                return View(filterNaam);
            }

        }


        public ActionResult AddInventory(int? id)
        {
            using (DBingredient context = new DBingredient())
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }

                BoodschapIngredient boodschapIngredient = new BoodschapIngredient();

                boodschapIngredient.ingredient = context.Ingredients.Find(id);

                ViewBag.IngredientID = id;

                return View(boodschapIngredient);
            }

        }

        // POST: Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddInventory([Bind(Include = "BoodschapLijstID,ingredient,Hoeveelheid,Eenheid")] BoodschapIngredient boodschapIngredient)
        {
            using (DBingredient context = new DBingredient())
            {

                if (ModelState.IsValid)
                {
                    User user = (User)Session["user"];
                    int UserID = user.UserID;
                    User indexUser = context.Users.Find(UserID);

                    Ingredient ig = context.Ingredients.Find(boodschapIngredient.ingredient.ingredientID);
                    boodschapIngredient.ingredient = ig;
                    context.BoodschapIngredients.Add(boodschapIngredient);
                    context.SaveChanges();

                    indexUser.boodschapLijst.BoodschapIngredients.Add(boodschapIngredient);
                    context.Entry(indexUser.boodschapLijst).State = EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(boodschapIngredient);
            }
        }


        // GET: Ingredients/Edit/5
        public ActionResult Edit(int? id)
        {
            //DBingredient context = new DBingredient();
            using (DBingredient context = new DBingredient())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                BoodschapIngredient boodschapIngredient = context.BoodschapIngredients.Find(id);
                if (boodschapIngredient == null)
                {
                    return HttpNotFound();
                }
                int ingredientID = boodschapIngredient.ingredient.ingredientID;
                Ingredient ig = context.Ingredients.Find(ingredientID);
                boodschapIngredient.ingredient = ig;

                return View(boodschapIngredient);
            }
        }

        // POST: Ingredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BoodschapIngredientID,Ingredient,Hoeveelheid,Eenheid")] BoodschapIngredient boodschapIngredient)
        {
            using (DBingredient context = new DBingredient())
            {
                if (ModelState.IsValid)
                {
                    Ingredient ig = context.Ingredients.Find(boodschapIngredient.ingredient.ingredientID);
                    boodschapIngredient.ingredient = ig;
                    context.Entry(boodschapIngredient).State = EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(boodschapIngredient);
            }
        }

        // GET: Ingredients/Delete/5
        public ActionResult Delete(int? id)
        {
            //DBingredient context = new DBingredient();
            using (DBingredient context = new DBingredient())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                BoodschapIngredient boodschapIngredient = context.BoodschapIngredients.Find(id);
                if (boodschapIngredient == null)
                {
                    return HttpNotFound();
                }

                int ingredientID = boodschapIngredient.ingredient.ingredientID;
                Ingredient ig = context.Ingredients.Find(ingredientID);
                boodschapIngredient.ingredient = ig;

                return View(boodschapIngredient);
            }
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (DBingredient context = new DBingredient())
            {
                BoodschapIngredient ingredient = context.BoodschapIngredients.Find(id);
                context.BoodschapIngredients.Remove(ingredient);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Gekocht(int? id)
        {
            using (DBingredient context = new DBingredient())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                BoodschapIngredient boodschapIngredient = context.BoodschapIngredients.Find(id);
                if (boodschapIngredient == null)
                {
                    return HttpNotFound();
                }

                User user = (User)Session["user"];

                InventoryIngredient inventoryIngredient = new InventoryIngredient();
                inventoryIngredient.ingredient = boodschapIngredient.ingredient;
                inventoryIngredient.Hoeveelheid = boodschapIngredient.Hoeveelheid;
                inventoryIngredient.Eenheid = boodschapIngredient.Eenheid;

                int inventoryID = user.inventory.InventoryID;
                Inventory inventory = context.Inventories.Find(inventoryID);
                int boodschapLijstID = user.boodschapLijst.BoodschapLijstID;
                BoodschapLijst boodschapLijst = context.BoodschapLijsts.Find(boodschapLijstID);

                inventory.InventoryIngredients.Add(inventoryIngredient);
                //context.Entry(inventory.InventoryIngredients).State = EntityState.Modified;

                boodschapLijst.BoodschapIngredients.Remove(boodschapIngredient);
                //context.Entry(user.boodschapLijst).State = EntityState.Modified;

                context.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Print()
        {
            User user = (User)Session["user"];

            using (DBingredient context = new DBingredient())
            {
                int boodschapLijstID = user.boodschapLijst.BoodschapLijstID;
                BoodschapLijst boodschapLijst = context.BoodschapLijsts.Find(boodschapLijstID);
                //List<BoodschapIngredient> lijst = context.BoodschapIngredients.ToList();

                if (boodschapLijst != null)
                {

                    foreach (BoodschapIngredient boodschapIngredient in boodschapLijst.BoodschapIngredients)
                    {
                        int ingredientID = boodschapIngredient.ingredient.ingredientID;
                        Ingredient ig = context.Ingredients.Find(ingredientID);
                        boodschapIngredient.ingredient = ig;
                    }
                    return View(boodschapLijst.BoodschapIngredients);
                }

                return View();
            }

        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Print")]
        [ValidateAntiForgeryToken]
        public ActionResult PrintConfirmed()
        {
            using (DBingredient context = new DBingredient())
            {
                User user = (User)Session["user"];

                int boodschapLijstID = user.boodschapLijst.BoodschapLijstID;
                BoodschapLijst boodschapLijst = context.BoodschapLijsts.Find(boodschapLijstID);
                
                string filePath = @"C:\Users\Public\BoodschappenLijst.txt";

                 
                string[] lijnen = new string[boodschapLijst.BoodschapIngredients.Count + 1];
                int arraycount = 0;

               
                if (boodschapLijst != null)
                {
                    lijnen[0] = "Naam Merk Hoeveelheid Eenheid";
                     

                    foreach (BoodschapIngredient boodschapIngredient in boodschapLijst.BoodschapIngredients)
                    {
                        arraycount = arraycount+1;
                        lijnen[arraycount] = $"{boodschapIngredient.ingredient.name} {boodschapIngredient.ingredient.merk} {boodschapIngredient.Hoeveelheid} {boodschapIngredient.Eenheid}";
                        //int ingredientID = boodschapIngredient.ingredient.ingredientID;
                        //Ingredient ig = context.Ingredients.Find(ingredientID);
                        //boodschapIngredient.ingredient = ig;
                    }

                    try
                    {
                        System.IO.File.WriteAllLines(filePath, lijnen);
                    }
                    catch 
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    
                    return RedirectToAction("Index");
                    
                }

                return View();
            }
        }
    }
}