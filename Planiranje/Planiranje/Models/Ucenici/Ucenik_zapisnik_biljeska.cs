using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Ucenik_zapisnik_biljeska
    {
        [Key]
        public int Id { get; set; }
        public int Id_ucenik_zapisnik { get; set; }
        public DateTime Datum { get; set; }
        public string Sadržaj { get; set; }
        public string Dogovor { get; set; }
    }
}