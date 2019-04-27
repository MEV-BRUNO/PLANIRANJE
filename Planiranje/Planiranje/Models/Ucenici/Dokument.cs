using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Dokument
    {
        [Key]
        public int Id { get; set; }
        public int Id_pedagog { get; set; }
        public int Id_skola { get; set; }        
        public string Path { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Naziv datoteke")]
        public string Opis { get; set; }
    }
}