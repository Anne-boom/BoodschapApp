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
        public ActionResult Index(string id)
        {
            using (DBingredient context = new DBingredient())
            {
                int ID;
                string message = "";
                bool isParsable = Int32.TryParse(id, out ID);

                if (isParsable == false)
                {
                    message = "Invalid number";

                }
                else 
                {
                    ID = Int32.Parse(id);
                    user = context.Users.Find(ID);

                    if (user == null)
                    {
                        message = "user is not in the database";
                    }
                    else
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