using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Nastavnik_protokol
    {
        [Key]
        public int Id { get; set; }
        public int Id_nastavnik { get; set; }
        public int Id_pedagog { get; set; }
        public int Id_skola { get; set; }
        public int Sk_godina { get; set; }
        [Required]
        public int Id_odjel { get; set; }
        public string Nastavni_predmet { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Datum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Datum { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime Vrijeme { get; set; }
        public string Broj_nastavnog_sata { get; set; }
        public string Mjesto_izvodjenja { get; set; }
        public string Nastavna_jedinica { get; set; }
        public string Nastavna_cjelina { get; set; }
        //Priprava za nastavni sat
        public int Cilj_i_zadaci_za_nastavnu_jedinicu { get; set; }
        public int Struktura_sadrzaja { get; set; }
        public int Plan_i_shvatljiv_koncept { get; set; }
        public int Plan_ploce { get; set; }
        //Priprava za nastavni sat - KRAJ
        public int Tip_nastavnog_sata { get; set; }
        //Primjena nastavnih metoda
        public int Verbalne { get; set; }
        public int Vizualno_dokumentacijske { get; set; }
        public int Demonstracijske { get; set; }
        public int Prakseoloske { get; set; }
        public int Kombinirane { get; set; }
        //Primjena nastavnih metoda - KRAJ
        public int Socioloski_oblici_rada { get; set; }
        public string Koristenje_nastavnih_sredstava { get; set; }
        public int Funkcionalna_pripremljenost { get; set; }
        //Motivacijska priprema učenika
        public int Uvod_i_najava_cilja { get; set; }
        public int Uspostavljanje_komunikacije { get; set; }
        public int Humor_u_nastavi { get; set; }
        public int Odrzavanje_paznje { get; set; }
        //Motivacijska priprema učenika - KRAJ
        //Prezentacijske vještine - nastavnik
        public string Sto_nastavnik_radi { get; set; }
        public string Nas_strukturne_komponente { get; set; }
        public string Nas_tijek_aktivnosti { get; set; }
        //Prezentacijske vještine - učenici
        public string Sto_ucenici_rade { get; set; }
        public string Uc_strukturne_komponente { get; set; }
        public string Uc_tijek_aktivnosti { get; set; }
        //Prezentacijske vještine - KRAJ
        //Procjena nastavnikove komunikacije s učenicima
        public int Razgovor_s_ucenicima { get; set; }
        public int Ohrabruje_ucenike_za_iznosenje_misljenja { get; set; }
        public int Obeshrabruje_ucenikovu_aktivnost { get; set; }
        public int Uvazava_ucenicke_primjedbe { get; set; }
        public int Kritizira_ili_se_poziva_na_svoj_autoritet { get; set; }
        public int Pokazuje_empatiju { get; set; }
        public int Pomaze_ucenicima_koji_imaju_teskoce { get; set; }
        public int Neverbalnim_porukama_pridonosi_pozitivnom_radnom_ozracju { get; set; }
        public int Ucenik_ima_mogucnost_i_inicijativu_slobodnog_iznosenja_stavova { get; set; }
        //Domaća zadaća
        public int Nastavnik_redovito_provjerava_uratke { get; set; }
        public int Daje_povratnu_informaciju { get; set; }
        public int Koristi_se_domacom_zadacom_kao_podlogom { get; set; }
        public int Daje_ocjenu_za_ucenje_u_razredu { get; set; }
        //Domaća zadaća - KRAJ
        public string Kratki_komentar_nastavnika { get; set; }
        public string Prijedlozi_za_daljnje_unapredjenje_rada { get; set; }
    }
}