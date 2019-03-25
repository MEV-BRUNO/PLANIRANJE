using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Roditelj_procjena
    {
        [Key]
        public int Id { get; set; }
        public int Id_ucenik_razred { get; set; }
        public int Id_pedagog { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Roditelj / skrbnik")]
        [Range(1,int.MaxValue,ErrorMessage ="Obavezno polje")]
        public int Id_roditelj { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Naziv dokumenta")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Opišite svoje dijete kako ga Vi vidite")]
        public string Opis { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Kakav interes Vaše dijete pokazuje za pojedine školske predmete?")]
        public string Interes { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("U kojem predmetu je Vaše dijete najaktivnije?")]
        public string Predmet { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Ima li Vaše dijete poteškoća u savladavanju gradiva pojedinih predmeta?")]
        public string Gradivo { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Ima li Vaše dijete neke poteškoće vezane za boravak u školi?")]
        public string Boravak { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Kakve odnose ima s članovima obitelji i prijateljima?")]
        public string Odnos { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Koje školske aktivnosti zanimaju Vaše dijete?")]
        public string Aktivnosti { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Koje ima hobije i interese?")]
        public string Hobiji { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Koja su Vaša očekivanja, što biste željeli da postigne u ovoj školskoj godini?")]
        public string Ocekivanja { get; set; }        
        [DisplayName("Želite li dodati podatke koji su važni za Vaše dijete?")]
        public string Dodatni_podaci { get; set; }
    }
}