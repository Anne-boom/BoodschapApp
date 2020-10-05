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
        public IEnumerable<BoodschapIngredient> Get()
        {
            List<BoodschapIngredient> boodschapLijst = new List<BoodschapIngredient>();
            boodschapLijst.Add();
            return new BoodschapIngredient[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }
    }
}
