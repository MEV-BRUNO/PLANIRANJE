using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Ucenik_razred
    {
        [Key]
        public int Id { get; set; }
        public int Id_razred { get; set; }
        public int Id_ucenik { get; set; }
    }
}