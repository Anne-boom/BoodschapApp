using IngredientDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BoodschappenApp.Controllers
{
    public class RecipeController : Controller
    {
        bool correctUser = false;

        //private DBingredient db = new DBingredient();

        // GET: Recipe
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

                List<Recipe> lijst = context.Recipes.ToList();
                

                foreach (Recipe recipe in lijst)
                {
                    //recipeLijst = recipe.RecipeIngredients;
                    List<RecipeIngredient> recipeLijst = new List<RecipeIngredient>();

                    foreach (RecipeIngredient recipeIngredient in recipe.RecipeIngredients)
                    {
                        //int recipeIngredientID = recipeIngredient.RecipeIngredientID;
                        //RecipeIngredient it = context.TotalRecipeIngredients.Find(recipeIngredientID);

                        //int ingredientID = recipeIngredient.ingredient.ingredientID;
                        //Ingredient ig = context.Ingredients.Find(ingredientID);
                        //recipeIngredient.ingredient = ig;

                        recipeLijst.Add(recipeIngredient);
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

            //User user = (User)Session["user"];

            //if (user == null)
            //{
            //    return RedirectToAction("Index", "User");
            //}

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
                    User user = (User)Session["user"];
                    int userID = user.UserID;
                    recipe.user = context.Users.Find(userID);
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
            correctUser = false;
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
                int userID = recipe.user.UserID;
                User user = context.Users.Find(userID);
                recipe.user = user;

                User currentUser = (User)Session["user"];

                if (recipe.user.UserID == currentUser.UserID)
                {
                    correctUser = true;
                }
                ViewBag.User = correctUser;

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
            correctUser = false;
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

                int userID = recipe.user.UserID;
                User user = context.Users.Find(userID);
                recipe.user = user;

                User currentUser = (User)Session["user"];

                if (recipe.user.UserID == currentUser.UserID)
                {
                    correctUser = true;
                }
                ViewBag.User = correctUser;

                return View();
            }
        }

        [HttpPost]
        public ActionResult AddIngredient(string naam)
        {
            DBingredient context = new DBingredient();
            Recipe recipe = ViewBag.Recipe;
            List<Ingredient> lijst = context.Ingredients.ToList<Ingredient>();
            correctUser = true;
            ViewBag.User = correctUser;
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

        
        public ActionResult Details(int? id)
        {
            correctUser = false;
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

                int userID = recipe.user.UserID;
                User user = context.Users.Find(userID);
                recipe.user = user;

                List<RecipeIngredient> lijst = new List<RecipeIngredient>();

                foreach (RecipeIngredient recipeIngredient in recipe.RecipeIngredients)
                {
                    int recipeIngredientID = recipeIngredient.RecipeIngredientID;
                    RecipeIngredient recipeIngredientLijst = context.TotalRecipeIngredients.Find(recipeIngredientID);
                    int ingredientID = recipeIngredientLijst.ingredient.ingredientID;
                    Ingredient ingredientLijst = context.Ingredients.Find(ingredientID);
                    recipeIngredientLijst.ingredient = ingredientLijst;
                    lijst.Add(recipeIngredientLijst);

                }

                 
                User currentUser = (User)Session["user"];

                if(recipe.user.UserID == currentUser.UserID)
                {
                    correctUser = true;
                }

                ViewBag.User = correctUser;
                recipe.RecipeIngredients = lijst;
                ViewBag.Lijst = lijst;

                TempData["RecipeID"] = recipe.RecipeID;

                return View(recipe);
            }


        }

        public ActionResult EditRecipe(int? id)
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

                int userID = recipe.user.UserID;
                recipe.user = context.Users.Find(userID);

                List<RecipeIngredient> lijst = new List<RecipeIngredient>();
                
                foreach(RecipeIngredient recipeIngredient in recipe.RecipeIngredients)
                {
                    int recipeIngredientID = recipeIngredient.RecipeIngredientID;
                    RecipeIngredient foundIngredient = context.TotalRecipeIngredients.Find(recipeIngredientID);
                    lijst.Add(foundIngredient);
                }

                //TempData["recipeIngredients"] = lijst;
                recipe.RecipeIngredients = lijst;

                return View(recipe);
            }

            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRecipe([Bind(Include = "RecipeID,user,Naam,Beschrijving,Calorien")] Recipe recipe)
        {
            using (DBingredient context = new DBingredient())
            {
                if (ModelState.IsValid)
                {
                    int recipeID = recipe.RecipeID;
                    Recipe origineel = context.Recipes.Find(recipeID);
                    recipe.RecipeIngredients = origineel.RecipeIngredients;
                    int userID = recipe.user.UserID;
                    User user = context.Users.Find(userID);
                    recipe.user = user;

                    context.Set<Recipe>().AddOrUpdate(recipe);
                
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(recipe);
            }
        }

        // GET: Ingredients/Edit/5
        public ActionResult EditAmount(int? id)
        {
            //DBingredient context = new DBingredient();
            using (DBingredient context = new DBingredient())
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                RecipeIngredient recipeIngredient = context.TotalRecipeIngredients.Find(id);
                if (recipeIngredient == null)
                {
                    return HttpNotFound();
                }

                int ingredientID = recipeIngredient.ingredient.ingredientID;
                Ingredient ig = context.Ingredients.Find(ingredientID);
                recipeIngredient.ingredient = ig;

                return View(recipeIngredient);
            }
        }

        // POST: Ingredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAmount([Bind(Include = "RecipeIngredientID,Ingredient,Hoeveelheid,Eenheid")] RecipeIngredient recipeIngredient)
        {
            using (DBingredient context = new DBingredient())
            {
                if (ModelState.IsValid)
                {
                    Ingredient ig = context.Ingredients.Find(recipeIngredient.ingredient.ingredientID);
                    recipeIngredient.ingredient = ig;
                    context.Entry(recipeIngredient).State = EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(recipeIngredient);
            }
        }

        public ActionResult EditDelete(int? id)
        {
            //DBingredient context = new DBingredient();
            using (DBingredient context = new DBingredient())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                RecipeIngredient recipeIngredient = context.TotalRecipeIngredients.Find(id);
                if (recipeIngredient == null)
                {
                    return HttpNotFound();
                }

                int ingredientID = recipeIngredient.ingredient.ingredientID;
                Ingredient ig = context.Ingredients.Find(ingredientID);
                recipeIngredient.ingredient = ig;

                return View(recipeIngredient);
            }
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("EditDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedEdit(int id)
        {
            using (DBingredient context = new DBingredient())
            {
                int? recipeIDInput = (int)TempData["RecipeID"];

                if ( recipeIDInput == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Recipe recipe = context.Recipes.Find(recipeIDInput);

                if(recipe == null)
                {
                    return HttpNotFound();
                }

                RecipeIngredient ingredient = context.TotalRecipeIngredients.Find(id);
                recipe.RecipeIngredients.Remove(ingredient);
                context.Entry(recipe).State = EntityState.Modified;
                context.SaveChanges();

                context.TotalRecipeIngredients.Remove(ingredient);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}