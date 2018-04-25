using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Models
{
    public class Aktivnost_akcija
    {
        public int Id_akcija { get; set; }
        public string Naziv { get; set; }
        public int Id_aktivnost { get; set; }
    }
}