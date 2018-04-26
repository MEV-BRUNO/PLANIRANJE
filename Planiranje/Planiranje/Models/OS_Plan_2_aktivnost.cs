using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class OS_Plan_2_aktivnost
    {
        public int Id_plan { get; set; }
        public int Red_br_podrucje { get; set; }
        public int Red_br_aktivnost { get; set; }
        public string Opis_aktivnost { get; set; }
        public string Cilj { get; set; }
        public string Zadatci { get; set; }
        public string Subjekti { get; set; }
        public string Oblici { get; set; }
        public DateTime Vrijeme { get; set; }
        public double Sati { get; set; }
    
}
}