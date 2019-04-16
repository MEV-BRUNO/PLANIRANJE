using iTextSharp.text;
using iTextSharp.text.pdf;
using Planiranje.Models;
using Planiranje.Models.Ucenici;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Planiranje.Reports
{
    public class NastavnikObrazacReport
    {
        public byte[] Podaci { get; private set; }
        public NastavnikObrazacReport(Nastavnik_obrazac model, Skola skola, Nastavnik nastavnik, RazredniOdjel odjel, Pedagog pedagog)
        {
            Document pdfDokument = new Document(
               PageSize.A4, 30, 30, 50, 50);

            MemoryStream memStream = new MemoryStream();
            PdfWriter.GetInstance(pdfDokument, memStream).
                CloseStream = false;
            pdfDokument.Open();
            BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA,
                BaseFont.CP1250, false);
            Font header = new Font(font, 12, Font.NORMAL, BaseColor.DARK_GRAY);
            Font naslov = new Font(font, 14, Font.BOLDITALIC, BaseColor.BLACK);
            Font tekst = new Font(font, 10, Font.NORMAL, BaseColor.BLACK);
            Font bold = new Font(font, 9, Font.BOLD, BaseColor.BLACK);
            Font blueBold = new Font(font, 9, Font.BOLD, BaseColor.BLUE);

            Paragraph p = new Paragraph("Obrazac za promatranje nastave i intervju za procjenjivanje kvalitete učenja i poučavanja", naslov);
            p.Alignment = Element.ALIGN_CENTER;
            p.SpacingBefore = 20;
            p.SpacingAfter = 20;
            pdfDokument.Add(p);

            p = new Paragraph("ŠKOLA: " + skola.Naziv, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("RAZREDNI ODJEL: " + odjel.Naziv, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("ŠKOLSKA GODINA: " + odjel.Sk_godina + "./" + (odjel.Sk_godina + 1).ToString() + ".", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("NASTAVNIK: " + nastavnik.ImePrezime, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);    
            p = new Paragraph("PREDMET: " + model.Predmet, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("SUPERVIZOR: " + model.Supervizor, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 20;
            pdfDokument.Add(p);

            List<string> selectDaNe = new List<string> { "-", "Da", "Ne" };
            List<string> select4 = new List<string> { "-","Pretežno slabosti", "Više slabosti nego prednosti", "Više prednosti nego slabosti",
            "Pretežno prednosti"};

            PdfPTable t = new PdfPTable(4);
            t.WidthPercentage = 100;
            t.SpacingBefore = 10;
            t.SetWidths(new float[] { 2, 1, 2, 1 });
            int i = OdrediI(model.Nastavnik_pocetnik);
            t.AddCell(VratiCeliju2("Nastavnik početnik:", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            i = OdrediI(model.Mjesovita_dobna_skupina);
            t.AddCell(VratiCeliju2("Mješovita dobna skupina:", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            var i2 = model.Br_ucenika_razred;
            t.AddCell(VratiCeliju2("Broj učenika u razredu:", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju6(i2.ToString(), tekst, false, BaseColor.WHITE));

            i2 = model.Br_ucenika_skola;
            t.AddCell(VratiCeliju2("Broj učenika u školi:", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju6(i2.ToString(), tekst, false, BaseColor.WHITE));

            i2 = model.Br_stanovnika_zajednica;
            t.AddCell(VratiCeliju2("Broj stanovnika u zajednici:", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju6(i2.ToString(), tekst, false, BaseColor.WHITE));
                        
            t.AddCell(VratiCeliju2("Dobna skupina:", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju6(model.Dobna_skupina, tekst, false, BaseColor.WHITE));

            t.AddCell(SpojiCelijeCol(VratiCeliju("\n", tekst, false, BaseColor.WHITE), 4));

            i2 = model.Postotak_ucenika_obitelj;
            t.AddCell(SpojiCelijeCol(VratiCeliju2("Postotak učenika iz socijalno ugroženih obitelji:", tekst, false, BaseColor.WHITE),3));
            t.AddCell(VratiCeliju6(i2.ToString(), tekst, false, BaseColor.WHITE));

            i2 = model.Postotak_ucenika_jezik;
            t.AddCell(SpojiCelijeCol(VratiCeliju2("Postotak učenika za koje jezik poučavanja nije materinski jezik:", tekst, false, BaseColor.WHITE), 3));
            t.AddCell(VratiCeliju6(i2.ToString(), tekst, false, BaseColor.WHITE));

            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            t = new PdfPTable(3);
            t.WidthPercentage = 100;
            t.SpacingBefore = 10;
            t.SetWidths(new float[] { 1, 8, 2 });
            i = OdrediI(model.Koriste_li_se_cesto_udzbenici);
            t.AddCell(SpojiCelijeCol(VratiCeliju3("Pitajte nastavnika sljedeća pitanja:", tekst, false, BaseColor.WHITE),3));
            t.AddCell(SpojiCelijeRow(VratiCeliju5("Mogućnost ostvarivanja\nminimalnih ciljeva", tekst, false, BaseColor.WHITE),5));
            t.AddCell(VratiCeliju3("Koriste li se često udžbenici i metode poučavanja... u drugim školama?", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i),tekst, false, BaseColor.WHITE));

            i = OdrediI(model.Pokrivaju_li_udzbenici_i_metode);
            t.AddCell(VratiCeliju3("Pokrivaju li udžbenici i metode... minimalne ciljeve za taj razred?", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            i = OdrediI(model.Jesu_li_udzbenici_i_metode_poucavanja);
            t.AddCell(VratiCeliju3("Jesu li udžbenici i metode poučavanja... zastarjeli ili se rijetko koriste?", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            i2 = model.Koliko_se_sati_na_tjedan_posvecuje;
            t.AddCell(VratiCeliju3("Koliko se sati na tjedan posvećuje...?", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4(i2.ToString(), tekst, false, BaseColor.WHITE));

            i2 = model.Koliko_se_ucenika_koristi_postupcima;
            t.AddCell(VratiCeliju3("Koliko se učenika koristi postupcima primjerenim učenicima nižih razreda?", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4(i2.ToString(), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju5("Praćenje", tekst, false, BaseColor.WHITE));

            i2 = model.Koliko_se_puta_godisnje_testiraju_postignuca;
            t.AddCell(VratiCeliju3("Koliko se puta godišnje testiraju postignuća učenika standardiziranim testovima?", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4(i2.ToString(), tekst, false, BaseColor.WHITE));

            t.AddCell(SpojiCelijeRow(VratiCeliju5("Mjere pomoći\nza učenike s\npoteškoćama", tekst, false, BaseColor.WHITE), 3));

            i = OdrediI(model.Dijagnosticira_li_nastavnik_probleme_u_ucenju);
            t.AddCell(VratiCeliju3("Dijagnosticira li nastavnik probleme u učenju učenika s poteškoćama?", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            i = OdrediI(model.Ima_li_nastavnik_propisane_nastavne_planove);
            t.AddCell(VratiCeliju3("Ima li nastavnik propisane nastavne planove za učenike s poteškoćama?", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            i = OdrediI(model.Provodi_li_nastavnik_propisane_planove);
            t.AddCell(VratiCeliju3("Provodi li nastavnik propisane planove za učenike s poteškoćama?", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.SpacingAfter = 20;
            pdfDokument.Add(t);
            pdfDokument.NewPage();

            t = new PdfPTable(5);
            t.WidthPercentage = 100;
            t.SpacingBefore = 10;
            t.SetWidths(new float[] { 0.7F, 4, 3, 4, 1 });
            t.AddCell(SpojiCelijeCol(VratiCeliju3("Komentirajte sljedeća ponašanja:", tekst, false, BaseColor.WHITE), 5));
            t.AddCell(SpojiCelijeRow(VratiCeliju5("Sigurno i poticajno ozračje za učenje", tekst, false, BaseColor.WHITE),18));
            t.AddCell(VratiCeliju3("... stvara opuštenu atmosferu", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Stvara_opuštenu_atmosferu);
            t.AddCell(VratiCeliju4(select4.ElementAt(i), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju3("... djeci se obraća na pozitivan način", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Djeci_se_obraca_na_pozitivan_nacin);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(SpojiCelijeCol(SpojiCelijeRow(VratiCeliju3(string.Empty, tekst, false, BaseColor.WHITE), 3), 2));

            //t.AddCell(SpojiCelijeCol(VratiCeliju(string.Empty, tekst, false, BaseColor.WHITE),2));
            i = OdrediI(model.Reagira_s_humorom_i_potice_humor);
            t.AddCell(VratiCeliju3("... reagira s humorom i potiče humor", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));
            //t.AddCell(SpojiCelijeCol(VratiCeliju(string.Empty, tekst, false, BaseColor.WHITE), 2));
            i = OdrediI(model.Dopusta_djeci_da_cine_pogreske);
            t.AddCell(VratiCeliju3("... dopušta djeci da čine pogreške", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Iskazuje_toplinu_i_empatiju);
            t.AddCell(VratiCeliju3("... iskazuje toplinu i empatiju prema svim učenicima", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... učenicima iskazuje poštovanje riječima i ponašanjem", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Ucenicima_iskazuje_postovanje_rijecima_i_ponasanjem);
            t.AddCell(VratiCeliju4(select4.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... dopušta učenicima da započeto izgovore do kraja", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Dopusta_ucenicima_da_zapoceto_izgovore_do_kraja);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(SpojiCelijeCol(SpojiCelijeRow(VratiCeliju3(string.Empty, tekst, false, BaseColor.WHITE), 2), 2));

            t.AddCell(VratiCeliju3("... sluša što učenici imaju za reći", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Slusa_sto_ucenici_imaju_za_reci);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... ne daje primjedbe kojima naglašava", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Ne_daje_primjedbe_kojima_naglasava_svoju_dominantnu_ulogu);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... promiče uzajamno poštovanje i interes učenika", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Promice_uzajamno_postovanje);
            t.AddCell(VratiCeliju4(select4.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... potiče djecu da slušaju jedni druge", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Potice_djecu_da_slusaju_jedni_druge);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(SpojiCelijeCol(SpojiCelijeRow(VratiCeliju3(string.Empty, tekst, false, BaseColor.WHITE), 4), 2));

            t.AddCell(VratiCeliju3("... intervenira kad su djeca ismijavana", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Intervenira_kad_su_djeca_ismijavana);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... uzima u obzir (kulturalne) razlike i posebnosti", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Uzima_u_obzir_razlike_i_posebnosti);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... potiče solidarnost među učenicima", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Potice_solidarnost_medju_ucenicima);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... omogućuje djeci da događaje i aktivnosti doživljavaju kao zajedničke", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Omogucuje_djeci_da_dogadjaje_i_aktivnosti_dozivljavaju);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... potiče samopouzdanje učenika", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Potice_samopouzdanje_ucenika);
            t.AddCell(VratiCeliju4(select4.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... pozitivno reagira na pitanja i odgovore učenika", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Pozitivno_reagira_na_pitanja_i_odgovore_ucenika);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(SpojiCelijeCol(SpojiCelijeRow(VratiCeliju3(string.Empty, tekst, false, BaseColor.WHITE), 2), 2));

            t.AddCell(VratiCeliju3("... pohvaljuje rezultate učenika", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Pohvaljuje_rezultate_ucenika);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... pokazuje da cijeni doprinos učenika", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Pokazuje_da_cijeni_doprinos_ucenika);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... ohrabruje učenike da daju sve od sebe", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Ohrabruje_ucenike_da_daju_sve_od_sebe);
            t.AddCell(VratiCeliju4(select4.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... hvali učenike kada daju sve od sebe", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Hvali_ucenike_kada_daju_sve_od_sebe);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(SpojiCelijeCol(SpojiCelijeRow(VratiCeliju3(string.Empty, tekst, false, BaseColor.WHITE), 2), 2));

            t.AddCell(VratiCeliju3("... jasno pokazuje da od svih učenika očekuje da daju sve od sebe", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Jasno_pokazuje_da_od_svih_ucenika_ocekuje_da_daju_sve_od_sebe);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... učenicima iskazuje pozitivna očekivanja o onome što oni mogu postići", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Ucenicima_iskazuje_pozitivna_ocekivanja);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));



            t.AddCell(SpojiCelijeRow(VratiCeliju5("Jasno i poticajno poučavanje", tekst, false, BaseColor.WHITE), 30));
            t.AddCell(VratiCeliju3("... na početku sata pojašnjava nastavne ciljeve", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Na_pocetku_sata_pojasnjava_nastavne_ciljeve);
            t.AddCell(VratiCeliju4(select4.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... na početku sata informira učenike o ciljevima sata", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Na_pocetku_sata_informira_ucenike_o_ciljevima_sata);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(SpojiCelijeCol(VratiCeliju3(string.Empty, tekst, false, BaseColor.WHITE), 2));

            t.AddCell(VratiCeliju3("... razjašnjava cilj zadatka i ono što će učenici iz njega naučiti", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Razjasnjava_cilj_zadatka);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... na kraju sata procjenjuje jesu li postignuti nastavni ciljevi", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Na_kraju_sata_procjenjuje_jesu_li_postignuti_nastavni_ciljevi);
            t.AddCell(VratiCeliju4(select4.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... provjerava postignuća učenika", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Provjerava_postignuca_ucenika);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(SpojiCelijeCol(VratiCeliju3(string.Empty, tekst, false, BaseColor.WHITE), 2));

            t.AddCell(VratiCeliju3("... provjerava i/ili procjenjuje jesu li postignuti ciljevi sata", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Provjerava_i_procjenjuje_jesu_li_postignuti_ciljevi_sata);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... daje jasne upute i objašnjenja", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Daje_jasne_upute_i_objasnjenja);
            t.AddCell(VratiCeliju4(select4.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... aktivira prethodno znanje učenika", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Aktivira_prethodno_znanje_ucenika);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(SpojiCelijeCol(SpojiCelijeRow(VratiCeliju3(string.Empty, tekst, false, BaseColor.WHITE), 3), 2));

            t.AddCell(VratiCeliju3("... postupno objašnjava", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Postupno_objasnjava);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... postavlja pitanja koja učenici razumiju", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Postavlja_pitanja_koja_ucenici);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... povremeno sažima nastavno gradivo", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Povremeno_sazima_nastavno_gradivo);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... daje jasna objašnjenja materijala za učenje i zadataka", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Daje_jasna_objasnjenja_materijala_za_ucenje);
            t.AddCell(VratiCeliju4(select4.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... vodi računa o tome da svaki učenik zna što treba činiti", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Vodi_racuna_o_tome_da_svaki_ucenik);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(SpojiCelijeCol(SpojiCelijeRow(VratiCeliju3(string.Empty, tekst, false, BaseColor.WHITE), 2), 2));

            t.AddCell(VratiCeliju3("... objašnjava kako su zadatci usklađeni s ciljevima sata", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Objasnjava_kaku_su_zadaci_uskladjeni);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... jasno navodi koji se materijali mogu koristiti kao pomoć u učenju", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Jasno_navodi_koji_se_materijali_mogu_koristiti);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... uključuje sve učenike na satu", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Ukljucuje_sve_ucenike_na_satu);
            t.AddCell(VratiCeliju4(select4.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... daje zadatke koji potiču učenike za aktivno uključivanje", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Daje_zadatke_koji_poticu_ucenike);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(SpojiCelijeCol(SpojiCelijeRow(VratiCeliju3(string.Empty, tekst, false, BaseColor.WHITE), 4), 2));

            t.AddCell(VratiCeliju3("... uključuje i one učenike koji sami ne sudjeluju u aktivnostima na satu", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Ukljucuje_i_one_ucenike_koji_sami_ne_sudjeluju);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... potiče učenike da pažljivo slušaju i kontinuirano rade", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Potice_ucenike_da_pazljivo_slusaju);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... nakon postavljenog pitanja čeka dovoljno dugo kako bi omogućio učenicima da razmisle", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Nakon_postavljenog_pitanja_ceka_dovoljno_dugo);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... daje priliku učenicima koji nisu podigli ruku da daju odgovor", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Daje_priliku_ucenicima_koji_nisu_podigli_ruku);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... koristi se nastavnim metodama koje aktiviraju učenike", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Koristi_se_nastavnim_metodama);
            t.AddCell(VratiCeliju4(select4.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... koristi se različitim oblicima razgovora i rasprava", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Koristi_se_razlicitim_oblicima_razgovora);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(SpojiCelijeCol(SpojiCelijeRow(VratiCeliju3(string.Empty, tekst, false, BaseColor.WHITE), 8), 2));

            t.AddCell(VratiCeliju3("... daje sve složenije zadatke", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Daje_sve_slozenije_zadatke);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... dopušta rad u manjim grupama", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Dopusta_rad_u_manjim_grupama);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... koristi se informacijsko-komunikacijskom tehnologijom", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Koristi_se_informacijskom);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... koristi se raznim strategijama poučavanja", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Koristi_se_raznim_strategijama);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... zadaje različite zadatke", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Zadaje_razlicite_zadatke);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... koristi se različitim nastavnim materijalima", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Koristi_se_razlicitim_nastavnim_materijalima);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... koristi se materijalima i primjerima iz svakodnevnog života učenika", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Koristi_se_materijalima_i_primjerima_iz_zivota);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... postavlja puno pitanja", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Postavlja_puno_pitanja);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... postavlja pitanja koja potiču razmišljanje", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Postavlja_pitanja_koja_poticu_razmisljanje);
            t.AddCell(VratiCeliju4(select4.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... daje svim učenicima dovoljno vremena za razmišljanje nakon postavljenog pitanja", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Daje_svim_ucenicima_dovoljno_vremena_za_razmisljanje);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(SpojiCelijeCol(SpojiCelijeRow(VratiCeliju3(string.Empty, tekst, false, BaseColor.WHITE), 4), 2));

            t.AddCell(VratiCeliju3("... ohrabruje učenike da postavljaju jedni drugima pitanja", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Ohrabruje_ucenike_da_postavljaju_jedni_drugima_pitanja);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... traži učenike da objasne jedni drugima kako su shvatili temu", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Trazi_ucenike_da_objasne_jedni_drugima);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... redovito provjerava jesu li učenici razumjeli", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Redovito_provjerava_jesu_li_ucenici_razumjeli);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju3("... postavlja pitanja koja potiču davanje povratne informacije", tekst, false, BaseColor.WHITE));
            i = OdrediI(model.Postavlja_pitanja_koja_poticu_davanje_povratne_informacije);
            t.AddCell(VratiCeliju4(selectDaNe.ElementAt(i), tekst, false, BaseColor.WHITE));

            t.SpacingAfter = 10;
            pdfDokument.Add(t);

            t = new PdfPTable(3);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1, 1, 1 });
            t.AddCell(VratiCeliju("Obrazac ispunila/ispunio:", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(string.Empty, tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("Nadnevak:", tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 0;
            pdfDokument.Add(t);

            t = new PdfPTable(3);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1, 1, 1 });
            t.AddCell(VratiCeliju3(string.Empty, tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(string.Empty, tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju3(string.Empty, tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 14;
            pdfDokument.Add(t);

            pdfDokument.Close();
            Podaci = memStream.ToArray();
        }
        /// <summary>
        /// metoda vraća ćeliju bez obruba s centriranim tekstom
        /// </summary>
        /// <param name="labela"></param>
        /// <param name="font"></param>
        /// <param name="nowrap"></param>
        /// <param name="boja"></param>
        /// <returns></returns>
        private PdfPCell VratiCeliju(string labela, Font font,
           bool nowrap, BaseColor boja)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(labela, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            c1.Padding = 5;
            c1.NoWrap = nowrap;
            c1.Border = PdfPCell.NO_BORDER;
            return c1;
        }
        /// <summary>
        /// metoda vraća ćeliju bez obruba (tekst nije centriran)
        /// </summary>
        /// <param name="labela"></param>
        /// <param name="font"></param>
        /// <param name="nowrap"></param>
        /// <param name="boja"></param>
        /// <returns></returns>
        private PdfPCell VratiCeliju2(string labela, Font font,
            bool nowrap, BaseColor boja)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(labela, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            c1.Padding = 5;
            c1.NoWrap = nowrap;
            c1.Border = PdfPCell.NO_BORDER;
            return c1;
        }
        /// <summary>
        /// metoda vraća ćeliju s obrubom (tekst nije centriran)
        /// </summary>
        /// <param name="labela"></param>
        /// <param name="font"></param>
        /// <param name="nowrap"></param>
        /// <param name="boja"></param>
        /// <returns></returns>
        private PdfPCell VratiCeliju3(string labela, Font font,
            bool nowrap, BaseColor boja)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(labela, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            c1.Padding = 5;
            c1.NoWrap = nowrap;
            //c1.Border = PdfPCell.BOTTOM_BORDER;
            return c1;
        }
        /// <summary>
        /// metoda vraća ćeliju s obrubom i centriranim tekstom
        /// </summary>
        /// <param name="labela"></param>
        /// <param name="font"></param>
        /// <param name="nowrap"></param>
        /// <param name="boja"></param>
        /// <returns></returns>
        private PdfPCell VratiCeliju4(string labela, Font font,
           bool nowrap, BaseColor boja)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(labela, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            c1.Padding = 5;
            c1.NoWrap = nowrap;
            //c1.Border = PdfPCell.NO_BORDER;
            
            return c1;
        }
        /// <summary>
        /// vraća rotiranu ćeliju s obrubom i centriranim tekstom
        /// </summary>
        /// <param name="labela"></param>
        /// <param name="font"></param>
        /// <param name="nowrap"></param>
        /// <param name="boja"></param>
        /// <returns></returns>
        private PdfPCell VratiCeliju5(string labela, Font font,
           bool nowrap, BaseColor boja)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(labela, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            c1.Padding = 5;
            c1.NoWrap = nowrap;
            c1.Rotation = 90;
            c1.Rotate();
            return c1;
        }
        /// <summary>
        /// vraća ćeliju koja ima donji obrub ukoliko je string empty ili null
        /// </summary>
        /// <param name="labela"></param>
        /// <param name="font"></param>
        /// <param name="nowrap"></param>
        /// <param name="boja"></param>
        /// <returns></returns>
        private PdfPCell VratiCeliju6(string labela, Font font,
           bool nowrap, BaseColor boja)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(labela, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            c1.Padding = 5;
            c1.NoWrap = nowrap;
            if (string.IsNullOrWhiteSpace(labela))
            {
                c1.Border = PdfPCell.BOTTOM_BORDER;
            }
            else
            {
                c1.Border = PdfPCell.NO_BORDER;
            }

            return c1;
        }
        private PdfPCell SpojiCelijeRow(PdfPCell cell, int broj)
        {
            cell.Rowspan = broj;
            return cell;
        }
        private PdfPCell SpojiCelijeCol (PdfPCell cell, int broj)
        {
            cell.Colspan = broj;
            return cell;
        }
        /// <summary>
        /// metoda koja provjerava ulaznu vrijednost pripremljena za moguću nadogradnju zbog sigurnosti
        /// </summary>
        /// <param name="broj"></param>
        /// <returns></returns>
        private int OdrediI(int broj)
        {
            return broj;
        }
    }
}