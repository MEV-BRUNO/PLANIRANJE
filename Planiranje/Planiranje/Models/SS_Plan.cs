using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class SS_Plan
    {
        int ID_plan { get; set; }
        int ID_pedagog { get; set; }
        string Akademska_godina { get; set; }
        string Naziv { get; set; }
        string Opis { get; set; }
    }
}