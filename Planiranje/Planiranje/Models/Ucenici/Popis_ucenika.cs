using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Popis_ucenika
    {
        [Key]
        public int Id { get; set; }
        public int Id_ucenik_razred { get; set; }
        public int Ponavlja_razred { get; set; }
        public int Putnik { get; set; }
        public string Zaduzenje { get; set; }
    }
}