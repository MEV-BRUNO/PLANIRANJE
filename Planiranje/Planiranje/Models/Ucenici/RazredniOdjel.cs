using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class RazredniOdjel
    {
        [Key]
        public int Id { get; set; }
        public int Id_skola { get; set; }
        public int Id_skGodina { get; set; }
        public string Naziv { get; set; }
        public int Razred { get; set; }
        public int Id_razrednik { get; set; }
    }
}