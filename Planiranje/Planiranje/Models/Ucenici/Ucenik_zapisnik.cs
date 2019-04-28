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
        public string Razlog { get; set; }
        public int Odgojni_utjecaj_majka { get; set; }
        public int Odgojni_utjecaj_otac { get; set; }
        
        public string Procjena_statusa_obitelji { get; set; }
        public int Odnos_prema_ucenju_majka { get; set; }
        public int Odnos_prema_ucenju_otac { get; set; }
        
        public int Suradnja_roditelja_majka { get; set; }
        public int Suradnja_roditelja_otac { get; set; }
        
        public string Odnos_s_prijateljima { get; set; }
        public string Kako_provodi_slobodno_vrijeme { get; set; }
        public string Procjena_mogucih_losih_utjecaja { get; set; }
        public string Zdravstvene_poteskoce_ucenika { get; set; }
        public string Podaci_o_naglim_promjenama { get; set; }
        public string Izrecene_pedagoske_mjere { get; set; }
    }
}