using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BoodschappenAPI.Models
{
    [DataContract]
    public class BoodschapIngredientModel
    {

        
        public int BoodschapIngredientID { get; set; }
        [DataMember]
        public string naam { get; set; }
        [DataMember]
        public string merk { get; set; }
        [DataMember]
        public double Hoeveelheid { get; set; }
        [DataMember]
        public string Eenheid { get; set; }
    }
}