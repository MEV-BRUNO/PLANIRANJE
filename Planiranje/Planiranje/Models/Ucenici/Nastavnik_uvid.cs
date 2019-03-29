using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Nastavnik_uvid
    {
        [Key]
        public int Id { get; set; }
        public int Id_nastavnik { get; set; }
        public int Id_pedagog { get; set; }
        public int Id_skola { get; set; }
        public int Sk_godina { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Razredni odjel")]
        public int Id_odjel { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Datum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Datum { get; set; }
        [Required(ErrorMessage = " ")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime Vrijeme { get; set; }
        [DisplayName("Nastavni predmet")]
        public string Nastavni_predmet { get; set; }
        [DisplayName("Nastavna cjelina")]
        public string Nastavna_cjelina { get; set; }
        [DisplayName("Nastavna jedinica")]
        public string Nastavna_jedinica { get; set; }
        [DisplayName("Broj sati nazočnosti")]
        public string Broj_sati_nazocnosti { get; set; }
        //1.uvjeti za realizaciju kurikuluma
        [DisplayName("Nastava se izvodi u")]
        public string Nastava_se_izvodi { get; set; }
        [DisplayName("Prostor i oprema")]
        public string Prostor_i_oprema { get; set; }
        [DisplayName("Estetsko-higijensko stanje učionice")]
        public string Estetsko_higijensko_stanje_ucionice { get; set; }
        [DisplayName("Materijalno-tehnička priprema za nastavu")]
        public string Materijalno_tehnicka_priprema_za_nastavu { get; set; }
        //2.planiranje i programiranje odgojno-obrazovnog procesa
        [DisplayName("Nastavnik se redovito priprema za nastavu i priprava je u pismenom obliku")]
        public string Nastavnik_se_redovito_priprema_za_nastavu { get; set; }
        [DisplayName("Nastavnikova priprava je")]
        public string Nastavnikova_priprava_je { get; set; }
        [DisplayName("Priprava sadrži")]
        public string Priprava_sadrzi { get; set; }
        [DisplayName("Pripremanje nastavnika je bilo u skladu s postignućima na nastavnom satu")]
        public string Pripremanje_nastavnika_je_bilo_u_skladu_s_postignucima { get; set; }
        [DisplayName("Plan ploče u pisanoj pripravi")]
        public string Plan_ploce_u_pisanoj_pripravi { get; set; }
        [DisplayName("Pismena priprava sadrži")]
        public string Pismena_priprava_sadrzi { get; set; }
        //3.analiza nakon uvida u neposredni proces nastavnog sata
        [DisplayName("Didaktični model nastavnog sata")]
        public string Didakticni_model_nastavnog_sata { get; set; }
        [DisplayName("Sociološki oblici rada")]
        public string Socioloski_oblici_rada { get; set; }
        [DisplayName("Nastavne metode")]
        public string Nastavne_metode { get; set; }
        [DisplayName("Metodičke strategije, postupci i oblici")]
        public string Metodicke_strategije_postupci_i_oblici { get; set; }
        [DisplayName("Nastavna sredstva i pomagala")]
        public string Nastavna_sredstva_i_pomagala { get; set; }
        [DisplayName("Odgojno-obrazovni sadržaji, broj novih pojmova i obavijesti")]
        public string Odgojno_obrazovni_sadrzaji_broj_novih_pojmova { get; set; }
        [DisplayName("Nastavne metode, metodički postupci i oblici")]
        public string Nastavne_metode_metodicki_postupci { get; set; }
        [DisplayName("Ciljevi, postignuća i kompetencije učenika ostvareni su")]
        public string Ciljevi_postignuca_i_kompetencije_ucenika { get; set; }
        [DisplayName("Odnos nastavnika prema učenicima")]
        public string Odnos_nastavnika_prema_ucenicima { get; set; }
        [DisplayName("Nastavnik posvećuje pozornost")]
        public string Nastavnik_posvecuje_pozornost { get; set; }
        [DisplayName("Nastavnikov nastup")]
        public string Nastavnikov_nastup { get; set; }
        [DisplayName("Govor nastavnika u skladu je")]
        public string Govor_nastavnika_u_skladu_je { get; set; }
        [DisplayName("Kakvim se stilom poučavanja nastavnik koristi")]
        public string Kakvim_se_stilom_poucavanja_nastavnik_koristi { get; set; }
        //4.aktivnosti učenika tijekom nastavnog procesa
        [DisplayName("Je li na satu došao do izražaja učenikov samostalan rad")]
        public string Je_li_na_satu_dosao_do_izrazaja_ucenikov_rad { get; set; }
        [DisplayName("Postignuća učenika i produktivnost sata nastavnika je provjerio")]
        public string Postignuca_ucenika_i_produktivnost_sata_nastavnika { get; set; }
        [DisplayName("Domaća zadaća zadana je")]
        public string Domaca_zadaca_zadana_je { get; set; }
        [DisplayName("Karakter domaće zadaće")]
        public string Karakter_domace_zadace { get; set; }
        [DisplayName("Domaća zadaća je provjerena")]
        public string Domaca_zadaca_je_provjerena { get; set; }
        [DisplayName("Zapis na školskoj ploči bio je")]
        public string Zapis_na_skolskoj_ploci_bio_je { get; set; }
        [DisplayName("Procjena uspješnosti nastavnog sata")]
        public string Procjena_uspjesnosti_nastavnog_sata { get; set; }
        [DisplayName("Evaluacija/samoevaluacija nastavnog sata")]
        public string Evaluacija_nastavnog_sata { get; set; }
        [DisplayName("Ostala zapažanja")]
        public string Ostala_zapazanja { get; set; }
        //5.nastavna dokumentacija
        [DisplayName("Nastavnik ima i vodi pedagošku dokumentaciju")]
        public string Nastavnik_ima_i_vodi_pedagosku_dokumentaciju { get; set; }
        [DisplayName("U dnevniku rada upisani su")]
        public string U_dnevniku_rada_upisani_su { get; set; }
        [DisplayName("Pripreme nastavnika za nastavu su")]
        public string Pripreme_nastavnika_za_nastavu_su { get; set; }
        [DisplayName("Iz imenika je vidljivo da nastavnik")]
        public string Iz_imenika_je_vidljivo_da_nastavnik { get; set; }
        [DisplayName("Ocjene u imeniku su")]
        public string Ocjene_u_imeniku_su { get; set; }
        [DisplayName("Poslovi razrednika, izviješća i analize")]
        public string Poslovi_razrednika_izvijesca_i_analize { get; set; }
        [DisplayName("Procjena vođenja nastavne dokumentacije")]
        public string Procjena_vodjenja_nastavne_dokumentacije { get; set; }
    }
}