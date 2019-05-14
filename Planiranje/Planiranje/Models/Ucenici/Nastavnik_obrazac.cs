using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planiranje.Models.Ucenici
{
    public class Nastavnik_obrazac
    {
        [Key]
        public int Id { get; set; }
        public int Id_nastavnik { get; set; }
        public int Id_pedagog { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Razredni odjel")]
        [Range(1, int.MaxValue, ErrorMessage ="Obavezno polje")]
        public int Id_odjel { get; set; }
        public int Sk_godina { get; set; }
        public int Id_skola { get; set; }
        [Required(ErrorMessage ="Obavezno polje")]
        [DisplayName("Predmet")]
        public string Predmet { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        public string Supervizor { get; set; }
        [DisplayName("Nastavnik početnik")]
        public int Nastavnik_pocetnik { get; set; }
        [DisplayName("Mješovita dobna skupina")]
        public int Mjesovita_dobna_skupina { get; set; }        
        [Range(0, int.MaxValue, ErrorMessage ="Minimalna vrijednost:0")]
        [DisplayName("Broj učenika u razredu")]
        public int? Br_ucenika_razred { get; set; }        
        [Range(0, int.MaxValue, ErrorMessage = "Minimalna vrijednost:0")]
        [DisplayName("Broj stanovnika u zajednici")]
        public int? Br_stanovnika_zajednica { get; set; }        
        [Range(0, int.MaxValue, ErrorMessage = "Minimalna vrijednost:0")]
        [DisplayName("Broj učenika u školi")]
        public int? Br_ucenika_skola { get; set; }
        [DisplayName("Dobna skupina")]
        public string Dobna_skupina { get; set; }        
        [Range(0, int.MaxValue, ErrorMessage = "Minimalna vrijednost:0")]
        [DisplayName("Postotak učenika iz socijalno ugroženih obitelji")]
        public int? Postotak_ucenika_obitelj { get; set; }        
        [Range(0, int.MaxValue, ErrorMessage = "Minimalna vrijednost:0")]
        [DisplayName("Postotak učenika za koje jezik poučavanja nije materinski jezik")]
        public int? Postotak_ucenika_jezik { get; set; }
        //pitanja za nastavnika
        [DisplayName("Koriste li se često udžbenici i metode poučavanja... u drugim školama")]
        public int Koriste_li_se_cesto_udzbenici { get; set; }
        [DisplayName("Pokrivaju li udžbenici i metode... minimalne ciljeve za taj razred")]
        public int Pokrivaju_li_udzbenici_i_metode { get; set; }
        [DisplayName("Jesu li udžbenici i metode poučavanja... zastarjeli ili se rijetko koriste")]
        public int Jesu_li_udzbenici_i_metode_poucavanja { get; set; }
        [DisplayName("Koliko se sati na tjedan posvećuje...")]        
        public int? Koliko_se_sati_na_tjedan_posvecuje { get; set; }
        [DisplayName("Koliko se učenika koristi postupcima primjerenim učenicima nižih razreda")]
        [Range(0, int.MaxValue, ErrorMessage = "Minimalna vrijednost:0")]
        public int? Koliko_se_ucenika_koristi_postupcima { get; set; }
        [DisplayName("Koliko se puta godišnje testiraju postignuća učenika standardiziranim testovima")]
        [Range(0, int.MaxValue, ErrorMessage = "Minimalna vrijednost:0")]
        public int? Koliko_se_puta_godisnje_testiraju_postignuca { get; set; }
        [DisplayName("Dijagnosticira li nastavnik probleme u učenju učenika s poteškoćama")]
        public int Dijagnosticira_li_nastavnik_probleme_u_ucenju { get; set; }
        [DisplayName("Ima li nastavnik propisane nastavne planove za učenike s poteškoćama")]
        public int Ima_li_nastavnik_propisane_nastavne_planove { get; set; }
        [DisplayName("Provodi li nastavnik propisane planove za učenike s poteškoćama")]
        public int Provodi_li_nastavnik_propisane_planove { get; set; }
        //komentiranje ponašanja
        [DisplayName("Stvara opuštenu atmosferu")]
        public int Stvara_opuštenu_atmosferu { get; set; }
        [DisplayName("Djeci se obraća na pozitivan način")]
        public int Djeci_se_obraca_na_pozitivan_nacin { get; set; }
        [DisplayName("Reagira s humorom i potiče humor")]
        public int Reagira_s_humorom_i_potice_humor { get; set; }
        [DisplayName("Dopušta djeci da čine pogreške")]
        public int Dopusta_djeci_da_cine_pogreske { get; set; }
        [DisplayName("Iskazuje toplinu i empatiju prema svim učenicima")]
        public int Iskazuje_toplinu_i_empatiju { get; set; }
        [DisplayName("Učenicima iskazuje poštovanje riječima i ponašanjem")]
        public int Ucenicima_iskazuje_postovanje_rijecima_i_ponasanjem { get; set; }
        [DisplayName("Dopušta učenicima da započeto izgovore do kraja")]
        public int Dopusta_ucenicima_da_zapoceto_izgovore_do_kraja { get; set; }
        [DisplayName("Sluša što učenici imaju za reći")]
        public int Slusa_sto_ucenici_imaju_za_reci { get; set; }
        [DisplayName("Ne daje primjedbe kojima naglašava svoju dominantnu ulogu")]
        public int Ne_daje_primjedbe_kojima_naglasava_svoju_dominantnu_ulogu { get; set; }
        [DisplayName("Promiče uzajamno poštovanje i interes učenika")]
        public int Promice_uzajamno_postovanje { get; set; }
        [DisplayName("Potiče djecu da slušaju jedni druge")]
        public int Potice_djecu_da_slusaju_jedni_druge { get; set; }
        [DisplayName("Intervenira kad su djeca ismijavana")]
        public int Intervenira_kad_su_djeca_ismijavana { get; set; }
        [DisplayName("Uzima u obzir (kulturalne) razlike i posebnosti")]
        public int Uzima_u_obzir_razlike_i_posebnosti { get; set; }
        [DisplayName("Potiče solidarnost među učenicima")]
        public int Potice_solidarnost_medju_ucenicima { get; set; }
        [DisplayName("Omogućuje djeci da događaje i aktivnosti doživljavaju kao zajedničke")]
        public int Omogucuje_djeci_da_dogadjaje_i_aktivnosti_dozivljavaju { get; set; }
        [DisplayName("Potiče samopouzdanje učenika")]
        public int Potice_samopouzdanje_ucenika { get; set; }
        [DisplayName("Pozitivno reagira na pitanja i odgovore učenika")]
        public int Pozitivno_reagira_na_pitanja_i_odgovore_ucenika { get; set; }
        [DisplayName("Pohvaljuje rezultate učenika")]
        public int Pohvaljuje_rezultate_ucenika { get; set; }
        [DisplayName("Pokazuje da cijeni doprinos učenika")]
        public int Pokazuje_da_cijeni_doprinos_ucenika { get; set; }
        [DisplayName("Ohrabruje učenike da daju sve od sebe")]
        public int Ohrabruje_ucenike_da_daju_sve_od_sebe { get; set; }
        [DisplayName("Hvali učenike kada daju sve od sebe")]
        public int Hvali_ucenike_kada_daju_sve_od_sebe { get; set; }
        [DisplayName("Jasno pokazuje da od svih učenika očekuje da daju sve od sebe")]
        public int Jasno_pokazuje_da_od_svih_ucenika_ocekuje_da_daju_sve_od_sebe { get; set; }
        [DisplayName("Učenicima iskazuje pozitivna očekivanja o onome što oni mogu postići")]
        public int Ucenicima_iskazuje_pozitivna_ocekivanja { get; set; }
        [DisplayName("Na početku sata pojašnjava nastavne ciljeve")]
        public int Na_pocetku_sata_pojasnjava_nastavne_ciljeve { get; set; }
        [DisplayName("Na početku sata informira učenike o ciljevima sata")]
        public int Na_pocetku_sata_informira_ucenike_o_ciljevima_sata { get; set; }
        [DisplayName("Razjašnjava cilj zadatka i ono što će učenici iz njega naučiti")]
        public int Razjasnjava_cilj_zadatka { get; set; }
        [DisplayName("Na kraju sata procjenjuje jesu li postignuti nastavni ciljevi")]
        public int Na_kraju_sata_procjenjuje_jesu_li_postignuti_nastavni_ciljevi { get; set; }
        [DisplayName("Provjerava postignuća učenika")]
        public int Provjerava_postignuca_ucenika { get; set; }
        [DisplayName("Provjerava i/ili procjenjuje jesu li postignuti ciljevi sata")]
        public int Provjerava_i_procjenjuje_jesu_li_postignuti_ciljevi_sata { get; set; }
        [DisplayName("Daje jasne upute i objašnjenja")]
        public int Daje_jasne_upute_i_objasnjenja { get; set; }
        [DisplayName("Aktivira prethodno znanje učenika")]
        public int Aktivira_prethodno_znanje_ucenika { get; set; }
        [DisplayName("Postupno objašnjava")]
        public int Postupno_objasnjava { get; set; }
        [DisplayName("Postavlja pitanja koja učenici razumiju")]
        public int Postavlja_pitanja_koja_ucenici { get; set; }
        [DisplayName("Povremeno sažima nastavno gradivo")]
        public int Povremeno_sazima_nastavno_gradivo { get; set; }
        [DisplayName("Daje jasna objašnjenja materijala za učenje i zadataka")]
        public int Daje_jasna_objasnjenja_materijala_za_ucenje { get; set; }
        [DisplayName("Vodi računa o tome da svaki učenik zna što treba činiti")]
        public int Vodi_racuna_o_tome_da_svaki_ucenik { get; set; }
        [DisplayName("Objašnjava kako su zadatci usklađeni s ciljevima sata")]
        public int Objasnjava_kaku_su_zadaci_uskladjeni { get; set; }
        [DisplayName("Jasno navodi koji se materijali mogu koristiti kao pomoć u učenju")]
        public int Jasno_navodi_koji_se_materijali_mogu_koristiti { get; set; }
        [DisplayName("Uključuje sve učenike na satu")]
        public int Ukljucuje_sve_ucenike_na_satu { get; set; }
        [DisplayName("Daje zadatke koji potiču učenike na aktivno uključivanje")]
        public int Daje_zadatke_koji_poticu_ucenike { get; set; }
        [DisplayName("Uključuje i one učenike koji sami ne sudjeluju u aktivnostima na satu")]
        public int Ukljucuje_i_one_ucenike_koji_sami_ne_sudjeluju { get; set; }
        [DisplayName("Potiče učenike da pažljivo slušaju i kontinuirano rade")]
        public int Potice_ucenike_da_pazljivo_slusaju { get; set; }
        [DisplayName("Nakon postavljenog pitanja čeka dovoljno dugo kako bi omogućio učenicima da razmisle")]
        public int Nakon_postavljenog_pitanja_ceka_dovoljno_dugo { get; set; }
        [DisplayName("Daje priliku učenicima koji nisu podigli ruku da daju odgovor")]
        public int Daje_priliku_ucenicima_koji_nisu_podigli_ruku { get; set; }
        [DisplayName("Koristi se nastavnim metodama koje aktiviraju učenike")]
        public int Koristi_se_nastavnim_metodama { get; set; }
        [DisplayName("Koristi se različitim oblicima razgovora i rasprava")]
        public int Koristi_se_razlicitim_oblicima_razgovora { get; set; }
        [DisplayName("Daje sve složenije zadatke")]
        public int Daje_sve_slozenije_zadatke { get; set; }
        [DisplayName("Dopušta rad u manjim grupama")]
        public int Dopusta_rad_u_manjim_grupama { get; set; }
        [DisplayName("Koristi se informacijsko-komunikacijskom tehnologijom")]
        public int Koristi_se_informacijskom { get; set; }
        [DisplayName("Koristi se raznim strategijama poučavanja")]
        public int Koristi_se_raznim_strategijama { get; set; }
        [DisplayName("Zadaje različite zadatke")]
        public int Zadaje_razlicite_zadatke { get; set; }
        [DisplayName("Koristi se različitim nastavnim materijalima")]
        public int Koristi_se_razlicitim_nastavnim_materijalima { get; set; }
        [DisplayName("Koristi se materijalima i primjerima iz svakodnevnog života učenika")]
        public int Koristi_se_materijalima_i_primjerima_iz_zivota { get; set; }
        [DisplayName("Postavlja puno pitanja")]
        public int Postavlja_puno_pitanja { get; set; }
        [DisplayName("Postavlja pitanja koja potiču razmišljanje")]
        public int Postavlja_pitanja_koja_poticu_razmisljanje { get; set; }
        [DisplayName("Daje svim učenicima dovoljno vremena za razmišljanje nakon postavljenog pitanja")]
        public int Daje_svim_ucenicima_dovoljno_vremena_za_razmisljanje { get; set; }
        [DisplayName("Ohrabruje učenike da postavljaju jedni drugima pitanja")]
        public int Ohrabruje_ucenike_da_postavljaju_jedni_drugima_pitanja { get; set; }
        [DisplayName("Traži učenike da objasne jedni drugima kako su shvatili temu")]
        public int Trazi_ucenike_da_objasne_jedni_drugima { get; set; }
        [DisplayName("Redovito provjerava jesu li učenici razumjeli")]
        public int Redovito_provjerava_jesu_li_ucenici_razumjeli { get; set; }
        [DisplayName("Postavlja pitanja koja potiču davanje povratne informacije")]
        public int Postavlja_pitanja_koja_poticu_davanje_povratne_informacije { get; set; }
    }
}