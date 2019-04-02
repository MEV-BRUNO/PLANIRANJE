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
        public int Id_odjel { get; set; }
        public int Sk_godina { get; set; }
        public int Id_skola { get; set; }
        public string Predmet { get; set; }
        public string Supervizor { get; set; }
        public int Nastavnik_pocetnik { get; set; }
        public int Mjesovita_dobna_skupina { get; set; }
        public int Br_ucenika_razred { get; set; }
        public int Br_stanovnika_zajednica { get; set; }
        public int Br_ucenika_skola { get; set; }
        public string Dobna_skupina { get; set; }
        public int Postotak_ucenika_obitelj { get; set; }
        public int Postotak_ucenika_jezik { get; set; }
        //pitanja za nastavnika
        public int Koriste_li_se_cesto_udzbenici { get; set; }
        public int Pokrivaju_li_udzbenici_i_metode { get; set; }
        public int Jesu_li_udzbenici_i_metode_poucavanja { get; set; }
        public int Koliko_se_sati_na_tjedan_posvecuje { get; set; }
        public int Koliko_se_ucenika_koristi_postupcima { get; set; }
        public int Koliko_se_puta_godisnje_testiraju_postignuca { get; set; }
        public int Dijagnosticira_li_nastavnik_probleme_u_ucenju { get; set; }
        public int Ima_li_nastavnik_propisane_nastavne_planove { get; set; }
        public int Provodi_li_nastavnik_propisane_planove { get; set; }
        //komentiranje ponašanja
        public int Stvara_opuštenu_atmosferu { get; set; }
        public int Djeci_se_obraca_na_pozitivan_nacin { get; set; }
        public int Reagira_s_humorom_i_potice_humor { get; set; }
        public int Dopusta_djeci_da_cine_pogreske { get; set; }
        public int Iskazuje_toplinu_i_empatiju { get; set; }
        public int Ucenicima_iskazuje_postovanje_rijecima_i_ponasanjem { get; set; }
        public int Dopusta_ucenicima_da_zapoceto_izgovore_do_kraja { get; set; }
        public int Slusa_sto_ucenici_imaju_za_reci { get; set; }
        public int Ne_daje_primjedbe_kojima_naglasava_svoju_dominantnu_ulogu { get; set; }
        public int Promice_uzajamno_postovanje { get; set; }
        public int Potice_djecu_da_slusaju_jedni_druge { get; set; }
        public int Intervenira_kad_su_djeca_ismijavana { get; set; }
        public int Uzima_u_obzir_razlike_i_posebnosti { get; set; }
        public int Potice_solidarnost_medju_ucenicima { get; set; }
        public int Omogucuje_djeci_da_dogadjaje_i_aktivnosti_dozivljavaju { get; set; }
        public int Potice_samopouzdanje_ucenika { get; set; }
        public int Pozitivno_reagira_na_pitanja_i_odgovore_ucenika { get; set; }
        public int Pohvaljuje_rezultate_ucenika { get; set; }
        public int Pokazuje_da_cijeni_doprinos_ucenika { get; set; }
        public int Ohrabruje_ucenike_da_daju_sve_od_sebe { get; set; }
        public int Hvali_ucenike_kada_daju_sve_od_sebe { get; set; }
        public int Jasno_pokazuje_da_od_svih_ucenika_ocekuje_da_daju_sve_od_sebe { get; set; }
        public int Ucenicima_iskazuje_pozitivna_ocekivanja { get; set; }
        public int Na_pocetku_sata_pojasnjava_nastavne_ciljeve { get; set; }
        public int Na_pocetku_sata_informira_ucenike_o_ciljevima_sata { get; set; }
        public int Razjasnjava_cilj_zadatka { get; set; }
        public int Na_kraju_sata_procjenjuje_jesu_li_postignuti_nastavni_ciljevi { get; set; }
        public int Provjerava_postignuca_ucenika { get; set; }
        public int Provjerava_i_procjenjuje_jesu_li_postignuti_ciljevi_sata { get; set; }
        public int Daje_jasne_upute_i_objasnjenja { get; set; }
        public int Aktivira_prethodno_znanje_ucenika { get; set; }
        public int Postupno_objasnjava { get; set; }
        public int Postavlja_pitanja_koja_ucenici { get; set; }
        public int Povremeno_sazima_nastavno_gradivo { get; set; }
        public int Daje_jasna_objasnjenja_materijala_za_ucenje { get; set; }
        public int Vodi_racuna_o_tome_da_svaki_ucenik { get; set; }
        public int Objasnjava_kaku_su_zadaci_uskladjeni { get; set; }
        public int Jasno_navodi_koji_se_materijali_mogu_koristiti { get; set; }
        public int Ukljucuje_sve_ucenike_na_satu { get; set; }
        public int Daje_zadatke_koji_poticu_ucenike { get; set; }
        public int Ukljucuje_i_one_ucenike_koji_sami_ne_sudjeluju { get; set; }
        public int Potice_ucenike_da_pazljivo_slusaju { get; set; }
        public int Nakon_postavljenog_pitanja_ceka_dovoljno_dugo { get; set; }
        public int Daje_priliku_ucenicima_koji_nisu_podigli_ruku { get; set; }
        public int Koristi_se_nastavnim_metodama { get; set; }
        public int Koristi_se_razlicitim_oblicima_razgovora { get; set; }
        public int Daje_sve_slozenije_zadatke { get; set; }
        public int Dopusta_rad_u_manjim_grupama { get; set; }
        public int Koristi_se_informacijskom { get; set; }
        public int Koristi_se_raznim_strategijama { get; set; }
        public int Zadaje_razlicite_zadatke { get; set; }
        public int Koristi_se_razlicitim_nastavnim_materijalima { get; set; }
        public int Koristi_se_materijalima_i_primjerima_iz_zivota { get; set; }
        public int Postavlja_puno_pitanja { get; set; }
        public int Postavlja_pitanja_koja_poticu_razmisljanje { get; set; }
        public int Daje_svim_ucenicima_dovoljno_vremena_za_razmisljanje { get; set; }
        public int Ohrabruje_ucenike_da_postavljaju_jedni_drugima_pitanja { get; set; }
        public int Trazi_ucenike_da_objasne_jedni_drugima { get; set; }
        public int Redovito_provjerava_jesu_li_ucenici_razumjeli { get; set; }
        public int Postavlja_pitanja_koja_poticu_davanje_povratne_informacije { get; set; }
    }
}