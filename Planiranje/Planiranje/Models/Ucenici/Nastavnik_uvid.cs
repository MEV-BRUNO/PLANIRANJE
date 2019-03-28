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
        public string Nastavni_predmet { get; set; }
        public string Nastavna_cjelina { get; set; }
        public string Nastavna_jedinica { get; set; }
        public string Broj_sati_nazocnosti { get; set; }
        //1.uvjeti za realizaciju kurikuluma
        public string Nastava_se_izvodi { get; set; }
        public string Prostor_i_oprema { get; set; }
        public string Estetsko_higijensko_stanje_ucionice { get; set; }
        public string Materijalno_tehnicka_priprema_za_nastavu { get; set; }
        //2.planiranje i programiranje odgojno-obrazovnog procesa
        public string Nastavnik_se_redovito_priprema_za_nastavu { get; set; }
        public string Nastavnikova_priprava_je { get; set; }
        public string Priprava_sadrzi { get; set; }
        public string Pripremanje_nastavnika_je_bilo_u_skladu_s_postignucima { get; set; }
        public string Plan_ploce_u_pisanoj_pripravi { get; set; }
        public string Pismena_priprava_sadrzi { get; set; }
        //3.analiza nakon uvida u neposredni proces nastavnog sata
        public string Didakticni_model_nastavnog_sata { get; set; }
        public string Socioloski_oblici_rada { get; set; }
        public string Nastavne_metode { get; set; }
        public string Metodicke_strategije_postupci_i_oblici { get; set; }
        public string Nastavna_sredstva_i_pomagala { get; set; }
        public string Odgojno_obrazovni_sadrzaji_broj_novih_pojmova { get; set; }
        public string Nastavne_metode_metodicki_postupci { get; set; }
        public string Ciljevi_postignuca_i_kompetencije_ucenika { get; set; }
        public string Odnos_nastavnika_prema_ucenicima { get; set; }
        public string Nastavnik_posvecuje_pozornost { get; set; }
        public string Nastavnikov_nastup { get; set; }
        public string Govor_nastavnika_u_skladu_je { get; set; }
        public string Kakvim_se_stilom_poucavanja_nastavnik_koristi { get; set; }
        //4.aktivnosti učenika tijekom nastavnog procesa
        public string Je_li_na_satu_dosao_do_izrazaja_ucenikov_rad { get; set; }
        public string Postignuca_ucenika_i_produktivnost_sata_nastavnika { get; set; }
        public string Domaca_zadaca_zadana_je { get; set; }
        public string Karakter_domace_zadace { get; set; }
        public string Domaca_zadaca_je_provjerena { get; set; }
        public string Zapis_na_skolskoj_ploci_bio_je { get; set; }
        public string Procjena_uspjesnosti_nastavnog_sata { get; set; }
        public string Evaluacija_nastavnog_sata { get; set; }
        public string Ostala_zapazanja { get; set; }
        //5.nastavna dokumentacija
        public string Nastavnik_ima_i_vodi_pedagosku_dokumentaciju { get; set; }
        public string U_dnevniku_rada_upisani_su { get; set; }
        public string Pripreme_nastavnika_za_nastavu_su { get; set; }
        public string Iz_imenika_je_vidljivo_da_nastavnik { get; set; }
        public string Ocjene_u_imeniku_su { get; set; }
        public string Poslovi_razrednika_izvijesca_i_analize { get; set; }
        public string Procjena_vodjenja_nastavne_dokumentacije { get; set; }
    }
}