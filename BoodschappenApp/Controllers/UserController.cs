using IngredientDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoodschappenApp.Controllers
{
    public class UserController : Controller
    {

        public static User user = new User();


        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "inlognaam, wachtwoord")] User userInput)
        {
            using (DBingredient context = new DBingredient())
            {
                string message = "";

                if (userInput == null)
                {
                    message = "Verkeerde input";
                }

                //User user = new User();
                List<User> UsersLijst = context.Users.ToList<User>();
                var user = UsersLijst.FirstOrDefault(x => x.inlognaam == userInput.inlognaam && x.wachtwoord == userInput.wachtwoord);

                if(user!= null)
                {

                    int inventoryID = user.inventory.InventoryID;
                    Inventory inventory = context.Inventories.Find(inventoryID);
                    user.inventory = inventory;
                    int boodschapID = user.boodschapLijst.BoodschapLijstID;
                    BoodschapLijst boodschapLijst = context.BoodschapLijsts.Find(boodschapID);
                    user.boodschapLijst = boodschapLijst;
                    Session["user"] = user;
                    message = $"Welkom {user.inlognaam}";
                }
                else
                {
                    message = "user is not in the database";
                }

                ViewBag.Message = message;
                return View();
            }


        }

        public ActionResult Create()
        {
            using(DBingredient context = new DBingredient())
            {
                return View();

            }
        }

        // POST: Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,inlognaam,wachtwoord")] User user)
        {
            using (DBingredient context = new DBingredient())
            {
                if (ModelState.IsValid)
                {
                    Inventory inventory = new Inventory();
                    BoodschapLijst boodschapLijst = new BoodschapLijst();
                    user.inventory = inventory;
                    user.boodschapLijst = boodschapLijst;
                    context.Users.Add(user);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(user);
            }
        }




    }
}