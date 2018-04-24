using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Mjesecni_detalji
    {
        int ID_plan { get; set; }
        int Red_broj { get; set; }
        int Podrucje { get; set; }
        int Aktivnost { get; set; }
        string Suradnici { get; set; }
        DateTime Vrijeme { get; set; }
        double Br_sati { get; set; }
        string Biljeska { get; set; }
    }
}