using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class SS_Plan_podrucje
    {
        int ID_plan { get; set; }
        int Red_br_podrucje { get; set; }
        int Opis_podrucje { get; set; }
        string Svrha { get; set; }
        string Zadaca { get; set; }
        string Sadrzaj { get; set; }
        string Oblici { get; set; }
        string Suradnici { get; set; }
        string Mjesto { get; set; }
        DateTime Vrijeme { get; set; }
        string Ishodi { get; set; }
        double Sati { get; set; }
    }
}