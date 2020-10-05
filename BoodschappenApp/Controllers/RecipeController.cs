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
    public class RecipeController : Controller
    {

        //private DBingredient db = new DBingredient();

        // GET: Recipe
        public ActionResult Index()
        {
            //DBingredient context = new DBingredient();
            using (DBingredient context = new DBingredient())
            {

                List<Recipe> lijst = context.Recipes.ToList();
                List<RecipeIngredient> recipeLijst = new List<RecipeIngredient>() ;

                foreach(Recipe recipe in lijst)
                {
                    //recipeLijst = recipe.RecipeIngredients;
                                       
                    foreach(RecipeIngredient recipeIngredient in recipe.RecipeIngredients)
                    {
                        int recipeIngredientID = recipeIngredient.RecipeIngredientID;
                        RecipeIngredient it = context.TotalRecipeIngredients.Find(recipeIngredientID);
                        
                        int ingredientID = it.ingredient.ingredientID;
                        Ingredient ig = context.Ingredients.Find(ingredientID);
                        it.ingredient = ig;

                        recipeLijst.Add(it);
                    }
                    ViewBag.recipeIngredient = new RecipeIngredient();
                    recipe.RecipeIngredients = recipeLijst;
                }
                return View(lijst);
            }

        }

        // GET: Ingredients/Create
        public ActionResult Create()
        {
            using (DBingredient context = new DBingredient())
            {
              return View();
            }
                
        }

        // POST: Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecipeID,Naam,Beschrijving,Calorien")] Recipe recipe)
        {
            using (DBingredient context = new DBingredient())
            {
                if (ModelState.IsValid)
                {
                    context.Recipes.Add(recipe);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.Recipe = recipe;
                return View();
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
                Recipe recipe = context.Recipes.Find(id);
                if (recipe == null)
                {
                    return HttpNotFound();
                }

                //List<RecipeIngredient> lijst = new List<RecipeIngredient>();


                return View(recipe);
            }
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (DBingredient context = new DBingredient())
            {
 Recipe recipe = context.Recipes.Find(id);
                List<RecipeIngredient> lijst = new List<RecipeIngredient>();
                lijst = recipe.RecipeIngredients;
                foreach (RecipeIngredient recipeIngredient in lijst.ToList())
                {
                    int recipeIngredientID = recipeIngredient.RecipeIngredientID;
                    RecipeIngredient removeIngredient = context.TotalRecipeIngredients.Find(recipeIngredientID);
                    context.TotalRecipeIngredients.Remove(removeIngredient);

                }

            context.Recipes.Remove(recipe);
            context.SaveChanges();
            return RedirectToAction("Index");
            }

               
        }

        public ActionResult AddIngredient(int? id)
        {
            using (DBingredient context = new DBingredient())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Recipe recipe = context.Recipes.Find(id);
                if (recipe == null)
                {
                    return HttpNotFound();
                }
                TempData["recipe"] = recipe;
                return View();
            }
        }

        [HttpPost]
        public ActionResult AddIngredient(string naam)
        {
            DBingredient context = new DBingredient();
            Recipe recipe = ViewBag.Recipe;
            List<Ingredient> lijst = context.Ingredients.ToList<Ingredient>();

            
            List<Ingredient> filter = lijst.Where(e => e.name.Contains(naam)).ToList();

           //ViewBag.lijst = filter;

            return View(filter);
        }

       

        public ActionResult AddRecipeIngredient(int? id)
        {
            using (DBingredient context = new DBingredient())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                RecipeIngredient recipeIngredient = new RecipeIngredient();
                recipeIngredient.ingredient = context.Ingredients.Find(id);
                
                return View(recipeIngredient);
            }
            
        }


        // POST: Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRecipeIngredient([Bind(Include = "RecipeIngredientID,ingredient,Hoeveelheid,Eenheid")] RecipeIngredient recipeIngredient)
        {
            using (DBingredient context = new DBingredient())
            {
                if (ModelState.IsValid)
                {
                    //User user = (User)Session["user"];
                    //int UserID = user.UserID;
                    //User indexUser = context.Users.Find(UserID);
                    //int inventoryID = indexUser.inventory.InventoryID;

                    //Inventory inventory = context.Inventories.Find(inventoryID);
                    Ingredient ig = context.Ingredients.Find(recipeIngredient.ingredient.ingredientID);
                    recipeIngredient.ingredient = ig;
                    context.TotalRecipeIngredients.Add(recipeIngredient);
                    context.SaveChanges();

                    Recipe tempRecipe = (Recipe)TempData["recipe"];
                    int recipeID = tempRecipe.RecipeID;
                    Recipe recipe = context.Recipes.Find(recipeID);
                    recipe.RecipeIngredients.Add(recipeIngredient);
                    //context.Entry(recipe.RecipeIngredients).State = EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(recipeIngredient);
            }
        }


    }
}