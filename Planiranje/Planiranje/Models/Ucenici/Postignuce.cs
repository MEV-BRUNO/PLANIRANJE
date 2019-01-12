using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace Planiranje.Models.Ucenici
{
    public class Postignuce
    {
        [Key]
        public int Id_postignuce { get; set; }
        public int Id_ucenik_razred { get; set; }  
        public int Id_pedagog { get; set; }
        public int Razred { get; set; }
        public int Godina { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        public string Napomena { get; set; }
    }
}