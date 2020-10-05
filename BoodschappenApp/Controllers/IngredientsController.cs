using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IngredientDB;

namespace BoodschappenApp.Controllers
{
    public class IngredientsController : Controller
    {
        private DBingredient db = new DBingredient();

        // GET: Ingredients
        public ActionResult Index()
        {
            using(DBingredient context = new DBingredient())
            {
                return View();
            }
            
        }

        [HttpPost]
        public ActionResult Index(string input)
        {
            using (DBingredient context = new DBingredient())
            {
                List<Ingredient> lijstIngredients = context.Ingredients.ToList();

                List<Ingredient> filterNaam = lijstIngredients.Where(e => e.name.Contains(input)).ToList();
                List<Ingredient> filterMerk = lijstIngredients.Where(e => e.merk.Contains(input)).ToList();

                foreach(Ingredient ingredient in filterMerk)
                {
                    int ID = ingredient.ingredientID;

                    if(!filterNaam.Exists(x => x.ingredientID == ID))
                    {
                        filterNaam.Add(ingredient);
                    }
                }
                return View(filterNaam);
            }

        }

        // GET: Ingredients/Details/5
        public ActionResult Details(int? id)
        {
            using (DBingredient context = new DBingredient())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Ingredient ingredient = context.Ingredients.Find(id);
                if (ingredient == null)
                {
                    return HttpNotFound();
                }
                return View(ingredient);
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
        public ActionResult Create([Bind(Include = "ingredientID,name,merk")] Ingredient ingredient)
        {
            using (DBingredient context = new DBingredient())
            {
                if (ModelState.IsValid)
                {
                    context.Ingredients.Add(ingredient);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(ingredient);
            }
        }

        // GET: Ingredients/Edit/5
        public ActionResult Edit(int? id)
        {

            using (DBingredient context = new DBingredient())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Ingredient ingredient = context.Ingredients.Find(id);
                if (ingredient == null)
                {
                    return HttpNotFound();
                }
                return View(ingredient);
            }
        }

        // POST: Ingredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ingredientID,name,merk")] Ingredient ingredient)
        {
            using (DBingredient context = new DBingredient())
            {
                if (ModelState.IsValid)
                {
                    context.Entry(ingredient).State = EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(ingredient);
            }
        }

        // GET: Ingredients/Delete/5
        public ActionResult Delete(int? id)
        {
            using (DBingredient context = new DBingredient())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Ingredient ingredient = context.Ingredients.Find(id);
                if (ingredient == null)
                {
                    return HttpNotFound();
                }
                return View(ingredient);
            }
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (DBingredient context = new DBingredient())
            {
                Ingredient ingredient = context.Ingredients.Find(id);
                context.Ingredients.Remove(ingredient);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
