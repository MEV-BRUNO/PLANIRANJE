using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_2
    {
        public int Id_plan { get; set; }
        public int Id_pedagog{get;set;}
        public string Akademska_godina { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }


    }
}