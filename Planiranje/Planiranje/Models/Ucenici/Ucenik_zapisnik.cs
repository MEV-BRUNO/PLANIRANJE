using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Ucenik_zapisnik
    {
        [Key]
        public int Id { get; set; }
        public int Id_ucenik_razred { get; set; }
        public int Id_pedagog { get; set; }
        [DisplayName("Razlog zboj kojeg je učenik upućen stručnom suradniku")]
        public string Razlog { get; set; }
        [DisplayName("Odgojni utjecaj majke")]
        public int Odgojni_utjecaj_majka { get; set; }
        [DisplayName("Odgojni utjecaj oca")]
        public int Odgojni_utjecaj_otac { get; set; }
        
        [DisplayName("Procjena socioekonomskog statusa obitelji")]
        public string Procjena_statusa_obitelji { get; set; }
        [DisplayName("Odnos majke prema učenju i obrazovanju")]
        public int Odnos_prema_ucenju_majka { get; set; }
        [DisplayName("Odnos oca prema učenju i obrazovanju")]
        public int Odnos_prema_ucenju_otac { get; set; }
        
        [DisplayName("Suradnja majke sa školom")]
        public int Suradnja_roditelja_majka { get; set; }
        [DisplayName("Suradnja oca sa školom")]
        public int Suradnja_roditelja_otac { get; set; }
        
        [DisplayName("Odnos sa prijateljima")]
        public string Odnos_s_prijateljima { get; set; }
        [DisplayName("Kako najčešće provodi slobodno vrijeme")]
        public string Kako_provodi_slobodno_vrijeme { get; set; }
        [DisplayName("Procjena mogućih loših utjecaja obitelji, prijatelja, okoline, društva na učenje")]
        public string Procjena_mogucih_losih_utjecaja { get; set; }
        [DisplayName("Zdravstvene poteškoće učenika i opis ekstremnog oblika ponašanja")]
        public string Zdravstvene_poteskoce_ucenika { get; set; }
        [DisplayName("Podaci o naglim i iznenadnim promjenama u ponašanju učenika")]
        public string Podaci_o_naglim_promjenama { get; set; }
        [DisplayName("Izrečene pedagoške mjere")]
        public string Izrecene_pedagoske_mjere { get; set; }
    }
}