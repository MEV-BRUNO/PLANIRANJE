using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_2_akcija
    {
        public int Id_plan { get; set; }
        public int Red_br_podrucje { get; set; }
        public int Red_br_aktivnost { get; set; }
        public int Red_br_akcija { get; set; }
        public string Opis_akcija { get; set; }
        public double Sati { get; set; }
    }
}