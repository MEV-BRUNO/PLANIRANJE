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
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Razredni odjel")]
        public int Id_odjel { get; set; }
        [DisplayName("Nastavni predmet")]
        [Required(ErrorMessage ="Obavezno polje")]
        public string Nastavni_predmet { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Datum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Datum { get; set; }      
        [Required(ErrorMessage =" ")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime Vrijeme { get; set; }
        [DisplayName("Broj nastavnog sata")]
        public string Broj_nastavnog_sata { get; set; }
        [DisplayName("Mjesto izvođenja")]
        public string Mjesto_izvodjenja { get; set; }
        [DisplayName("Nastavna jedinica")]
        public string Nastavna_jedinica { get; set; }
        [DisplayName("Nastavna cjelina")]
        public string Nastavna_cjelina { get; set; }
        //Priprava za nastavni sat
        [DisplayName("Cilj i zadaci za nastavnu jedinicu")]
        public int Cilj_i_zadaci_za_nastavnu_jedinicu { get; set; }
        [DisplayName("Struktura sadržaja")]
        public int Struktura_sadrzaja { get; set; }
        [DisplayName("Plan i shvatljiv koncept")]
        public int Plan_i_shvatljiv_koncept { get; set; }
        [DisplayName("Plan ploče")]
        public int Plan_ploce { get; set; }
        //Priprava za nastavni sat - KRAJ
        [DisplayName("Tip nastavnog sata")]
        public int Tip_nastavnog_sata { get; set; }
        //Primjena nastavnih metoda
        [DisplayName("Verbalne")]
        public int Verbalne { get; set; }
        [DisplayName("Vizualno-dokumentacijske")]
        public int Vizualno_dokumentacijske { get; set; }
        [DisplayName("Demonstracijske")]
        public int Demonstracijske { get; set; }
        [DisplayName("Prakseološke")]
        public int Prakseoloske { get; set; }
        [DisplayName("Kombinirane")]
        public int Kombinirane { get; set; }
        //Primjena nastavnih metoda - KRAJ
        [DisplayName("Sociološki oblici rada")]
        public int Socioloski_oblici_rada { get; set; }
        [DisplayName("Korištenje nastavnih sredstava, pomagala i tehnologije")]
        public string Koristenje_nastavnih_sredstava { get; set; }
        [DisplayName("Funkcionalna pripremljenost")]
        public int Funkcionalna_pripremljenost { get; set; }
        //Motivacijska priprema učenika
        [DisplayName("Uvod i najava cilja")]
        public int Uvod_i_najava_cilja { get; set; }
        [DisplayName("Uspostavljanje komunikacije")]
        public int Uspostavljanje_komunikacije { get; set; }
        [DisplayName("Humor u nastavi")]
        public int Humor_u_nastavi { get; set; }
        [DisplayName("Održavanje pažnje")]
        public int Odrzavanje_paznje { get; set; }
        //Motivacijska priprema učenika - KRAJ
        //Prezentacijske vještine - nastavnik
        [DisplayName("Što nastavnik radi")]
        public string Sto_nastavnik_radi { get; set; }
        [DisplayName("Strukturne komponente")]
        public string Nas_strukturne_komponente { get; set; }
        [DisplayName("Tijek aktivnosti")]
        public string Nas_tijek_aktivnosti { get; set; }
        //Prezentacijske vještine - učenici
        [DisplayName("Što učenici rade")]
        public string Sto_ucenici_rade { get; set; }
        [DisplayName("Strukturne komponente")]
        public string Uc_strukturne_komponente { get; set; }
        [DisplayName("Tijek aktivnosti")]
        public string Uc_tijek_aktivnosti { get; set; }
        //Prezentacijske vještine - KRAJ
        //Procjena nastavnikove komunikacije s učenicima
        [DisplayName("Razgovor s učenicima")]
        public int Razgovor_s_ucenicima { get; set; }
        [DisplayName("Ohrabruje učenike za iznošenje mišljenja i hvali ponašanje")]
        public int Ohrabruje_ucenike_za_iznosenje_misljenja { get; set; }
        [DisplayName("Obeshrabruje učenikovu aktivnost ili ponašanje")]
        public int Obeshrabruje_ucenikovu_aktivnost { get; set; }
        [DisplayName("Uvažava učeničke primjedbe, pitanja i odgovore")]
        public int Uvazava_ucenicke_primjedbe { get; set; }
        [DisplayName("Kritizira ili se poziva na svoj autoritet")]
        public int Kritizira_ili_se_poziva_na_svoj_autoritet { get; set; }
        [DisplayName("Pokazuje empatiju")]
        public int Pokazuje_empatiju { get; set; }
        [DisplayName("Pomaže učenicima koji imaju teškoće u radu")]
        public int Pomaze_ucenicima_koji_imaju_teskoce { get; set; }
        [DisplayName("Neverbalnim porukama pridonosi pozitivnom radnom ozračju u učionici")]
        public int Neverbalnim_porukama_pridonosi_pozitivnom_radnom_ozracju { get; set; }
        [DisplayName("Učenik ima mogućnost i inicijativu slobodnog iznošenja stavova i mišljenja")]
        public int Ucenik_ima_mogucnost_i_inicijativu_slobodnog_iznosenja_stavova { get; set; }
        //Domaća zadaća
        [DisplayName("Nastavnik redovito provjerava uratke")]
        public int Nastavnik_redovito_provjerava_uratke { get; set; }
        [DisplayName("Daje povratnu informaciju")]
        public int Daje_povratnu_informaciju { get; set; }
        [DisplayName("Koristi se domaćom zadaćom kao podlogom za redovnu raspravu")]
        public int Koristi_se_domacom_zadacom_kao_podlogom { get; set; }
        [DisplayName("Daje ocjenu za učenje u razredu")]
        public int Daje_ocjenu_za_ucenje_u_razredu { get; set; }
        //Domaća zadaća - KRAJ
        [DisplayName("Kratki komentar nastavnika na održani nastavni sat")]
        public string Kratki_komentar_nastavnika { get; set; }
        [DisplayName("Prijedlozi za daljnje unapređenje rada")]
        public string Prijedlozi_za_daljnje_unapredjenje_rada { get; set; }
    }
}