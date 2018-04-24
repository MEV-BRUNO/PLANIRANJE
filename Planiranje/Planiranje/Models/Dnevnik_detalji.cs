using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Dnevnik_detalji
    {
        int ID_dnevnik { get; set; }
        string Subjekt { get; set; }
        DateTime Vrijeme_od { get; set; }
        DateTime Vrijeme_do { get; set; }
        int Aktivnost { get; set; }
        string Suradnja { get; set; }
        string Zakljucak { get; set; }

    }
}