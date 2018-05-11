using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class SS_Plan
    {
        public int ID_plan { get; set; }
        public int ID_pedagog { get; set; }
        public string Ak_godina { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
    }
}