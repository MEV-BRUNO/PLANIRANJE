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
        [DisplayName("Naziv datoteke")]
        public string Path { get; set; }
        public string Opis { get; set; }
    }
}