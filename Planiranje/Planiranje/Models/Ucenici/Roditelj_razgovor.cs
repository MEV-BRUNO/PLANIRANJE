using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Roditelj_razgovor
    {
        [Key]
        public int Id { get; set; }
        public int Id_ucenik_razred { get; set; }
        public int Id_pedagog { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Roditelj / skrbnik")]
        [Range(1,int.MaxValue,ErrorMessage ="Obavezno polje")]
        public int Id_roditelj { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Datum { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime Vrijeme { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Razgovor traži")]
        public string Trazi { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Razlog")]
        public string Razlog { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Došao je")]
        public string Dolazak { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Bilješka o razgovoru")]
        public string Biljeska { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Prijedlog roditelja / skrbnika")]
        public string Prijedlog_roditelja { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Prijedlog škole")]
        public string Prijedlog_skole { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Dogovoreno")]
        public string Dogovor { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("O poduzetom treba izvijestiti")]
        public string Izvjestiti { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Vrijeme slijedećeg sastanka")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Datum_slijedeci { get; set; }
    }
}