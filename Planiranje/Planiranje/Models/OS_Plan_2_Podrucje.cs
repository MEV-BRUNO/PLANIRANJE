using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_2_Podrucje
    {
        public int Id_plan { get; set; }
        public int Red_br_podrucje { get; set; }
        public int Opis_podrucje { get; set; }
        public string Cilj { get; set; }
        public string Zadaci { get; set; }
        public string Subjekti { get; set; }
        public string Oblici { get; set; }
        public DateTime Vrijeme { get; set; }
        public double Sati { get; set; }
    }
}
