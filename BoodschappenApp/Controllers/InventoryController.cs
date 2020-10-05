using IngredientDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace BoodschappenApp.Controllers
{
    public class InventoryController : Controller
    {
        //private DBingredient db = new DBingredient();
        

        // GET: Inventory
        public ActionResult Index()
        {
            User user = (User)Session["user"];
            
            if (user == null)
            {
                return RedirectToAction("Index", "User");
            }
            using (DBingredient context = new DBingredient()) {

                int inventoryID = user.inventory.InventoryID;
                              
                Inventory inventory = context.Inventories.Find(inventoryID);
                //List<InventoryIngredient> lijst = context.Users.Fin;
                if (inventory != null)
                {
                    foreach (InventoryIngredient inventoryIngredient in inventory.InventoryIngredients)
                    {
                        int ingredientID = inventoryIngredient.ingredient.ingredientID;
                        Ingredient ig = context.Ingredients.Find(ingredientID);
                        inventoryIngredient.ingredient = ig;
                        
                    }
                    return View(inventory.InventoryIngredients);
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

                int inventoryID = user.inventory.InventoryID;

                Inventory inventory = context.Inventories.Find(inventoryID);
                //List<InventoryIngredient> lijst = context.Users.Fin;
                if (inventory != null)
                {
                    foreach (InventoryIngredient inventoryIngredient in inventory.InventoryIngredients)
                    {

                        
                        int ingredientID = inventoryIngredient.ingredient.ingredientID;
                        Ingredient ig = context.Ingredients.Find(ingredientID);
                        inventoryIngredient.ingredient = ig;

                    }

                    List<InventoryIngredient> filter = inventory.InventoryIngredients.Where(e => e.ingredient.name.Contains(input)).ToList();

                    List<InventoryIngredient> filter2 = inventory.InventoryIngredients.Where(e => e.ingredient.merk.Contains(input)).ToList();

                    foreach(InventoryIngredient inventoryingredient2 in filter2)
                    {
                        int ID = inventoryingredient2.InventoryIngredientID;
                        

                        if(!filter.Exists(x => x.InventoryIngredientID == ID))
                        {
                            filter.Add(inventoryingredient2);
                        }
                    }

                    return View(filter);
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

                InventoryIngredient inventoryIngredient = new InventoryIngredient();

                inventoryIngredient.ingredient = context.Ingredients.Find(id);

                ViewBag.IngredientID = id;

                return View(inventoryIngredient);
            }
        }

        // POST: Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddInventory([Bind(Include = "InventoryingredientID,ingredient,Hoeveelheid,Eenheid")] InventoryIngredient inventoryIngredient)
        {
            using (DBingredient context = new DBingredient())
            {
                if (ModelState.IsValid)
                {
                    User user = (User)Session["user"];
                    int UserID = user.UserID;
                    User indexUser = context.Users.Find(UserID);
                    //int inventoryID = indexUser.inventory.InventoryID;

                    //Inventory inventory = context.Inventories.Find(inventoryID);
                    Ingredient ig = context.Ingredients.Find(inventoryIngredient.ingredient.ingredientID);
                    inventoryIngredient.ingredient = ig;
                    context.InventoryIngredients.Add(inventoryIngredient);
                    context.SaveChanges();

                    indexUser.inventory.InventoryIngredients.Add(inventoryIngredient);
                    context.Entry(indexUser.inventory).State = EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(inventoryIngredient);
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
                InventoryIngredient inventoryIngredient = context.InventoryIngredients.Find(id);
                if (inventoryIngredient == null)
                {
                    return HttpNotFound();
                }

                int ingredientID = inventoryIngredient.ingredient.ingredientID;
                Ingredient ig = context.Ingredients.Find(ingredientID);
                inventoryIngredient.ingredient = ig;

                return View(inventoryIngredient);
            }
        }

        // POST: Ingredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InventoryingredientID,Ingredient,Hoeveelheid,Eenheid")] InventoryIngredient inventoryIngredient)
        {
            using (DBingredient context = new DBingredient())
            {
                if (ModelState.IsValid)
                {
                    Ingredient ig = context.Ingredients.Find(inventoryIngredient.ingredient.ingredientID);
                    inventoryIngredient.ingredient = ig;
                    context.Entry(inventoryIngredient).State = EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(inventoryIngredient);
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
                InventoryIngredient inventoryIngredient = context.InventoryIngredients.Find(id);
                if (inventoryIngredient == null)
                {
                    return HttpNotFound();
                }

                int ingredientID = inventoryIngredient.ingredient.ingredientID;
                Ingredient ig = context.Ingredients.Find(ingredientID);
                inventoryIngredient.ingredient = ig;

                return View(inventoryIngredient);
            }
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (DBingredient context = new DBingredient())
            {
                InventoryIngredient ingredient = context.InventoryIngredients.Find(id);
                context.InventoryIngredients.Remove(ingredient);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        
    }
}