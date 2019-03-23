using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Nastavnik_analiza
    {
        public int Id { get; set; }
        public int Id_pedagog { get; set; }
        public int Id_nastavnik { get; set; }
        public int Id_skola { get; set; }
        public int Sk_godina { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Razredni odjel")]
        public string Odjel { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Datum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Datum { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Nastavni sat")]
        public string Nastavni_sat { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Cilj posjete")]
        public string Cilj_posjete { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Predmet")]
        public string Predmet { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Nastavna jedinica")]
        public string Nastavna_jedinica { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Vrsta nastavnog sata")]
        public string Vrsta_nastavnog_sata { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Planiranje i priprema")]
        public string Planiranje_priprema { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Izvedba nastavnog sata")]
        public string Izvedba_nastavnog_sata { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Vođenje i tijek nastavnog sata")]
        public string Vodjenje_nastavnog_sata { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Razredni ugođaj")]
        public string Razredni_ugodjaj { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Disciplina")]
        public string Disciplina { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Ocjenjivanje napredovanja učenika")]
        public string Ocjenjivanje_ucenika { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Osvrt i prosudba vlastitog rada")]
        public string Osvrt { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Prijedlozi za unapređenje")]
        public string Prijedlozi { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Uvid u vođenje pedagoške dokumentacije")]
        public string Uvid { get; set; }
    }
}