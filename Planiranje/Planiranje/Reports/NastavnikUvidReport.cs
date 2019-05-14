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
    public class NastavnikUvidReport
    {
        public byte[] Podaci { get; private set; }
        public NastavnikUvidReport(Nastavnik_uvid model, Nastavnik nastavnik, RazredniOdjel odjel, Skola skola, Pedagog pedagog)
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

            Paragraph p = new Paragraph("Protokol za uvid u neposredni odgojno-obrazovni rad učitelja/nastavnika", naslov);
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
            p = new Paragraph("NASTAVNI PREDMET: " + model.Nastavni_predmet + "  BROJ SATA NAZOČNOSTI: " + model.Broj_sati_nazocnosti, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("NADNEVAK: " + model.Datum.ToShortDateString() + " VRIJEME: " + model.Vrijeme.ToShortTimeString(), tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);            
            p = new Paragraph("NASTAVNA CJELINA: " + model.Nastavna_cjelina, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("NASTAVNA JEDINICA: " + model.Nastavna_jedinica, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 20;
            pdfDokument.Add(p);

            p = new Paragraph("1. UVJETI ZA REALIZACIJU KURIKULUMA", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingBefore = 10;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);

            PdfPTable t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 1 });
            string s = model.Nastava_se_izvodi;
            t.AddCell(VratiCeliju4("1.1. Nastava se izvodi u:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Prostor_i_oprema;
            t.AddCell(VratiCeliju4("1.2. Prostor i oprema za izvođenje nastave je:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Estetsko_higijensko_stanje_ucionice;
            t.AddCell(VratiCeliju4("1.3. Estetsko-higijensko stanje učionice:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Materijalno_tehnicka_priprema_za_nastavu;
            t.AddCell(VratiCeliju4("1.4. Materijalno-tehnička priprema za nastavu:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("2. PLANIRANJE I PROGRAMIRANJE ODGOJNO-OBRAZOVNOG PROCESA", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingBefore = 10;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 1 });
            s = model.Nastavnik_se_redovito_priprema_za_nastavu;
            t.AddCell(VratiCeliju4("2.1. Učitelj/nastavnik se redovito priprema za nastavu i priprava je u pisanom obliku.", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Nastavnikova_priprava_je;
            t.AddCell(VratiCeliju4("2.2. Učiteljeva/nastavnikova priprava je:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Priprava_sadrzi;
            t.AddCell(VratiCeliju4("2.3. Priprava sadrži:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Pripremanje_nastavnika_je_bilo_u_skladu_s_postignucima;
            t.AddCell(VratiCeliju4("2.4. Pripremanje učitelja/nastavnika je bilo u skladu s postignućima na nastavnom satu.", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Plan_ploce_u_pisanoj_pripravi;
            t.AddCell(VratiCeliju4("2.5. Plan ploče u pismenoj pripravi:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Pismena_priprava_sadrzi;
            t.AddCell(VratiCeliju4("2.6. Pismena priprava sadrži:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }

            t.SpacingAfter = 15;
            pdfDokument.Add(t);                               

            p = new Paragraph("3. ANALIZA NAKON UVIDA U NEPOSREDNI PROCES NASTAVNOG SATA", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingBefore = 10;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 1 });
            s = model.Didakticni_model_nastavnog_sata;
            t.AddCell(VratiCeliju4("3.1. Didaktički model nastavnoga sata", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Socioloski_oblici_rada;
            t.AddCell(VratiCeliju4("3.2. Sociološki oblici rada: skupni, individualni, rad u paru", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Nastavne_metode;
            t.AddCell(VratiCeliju4("3.4. Nastavne metode", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Metodicke_strategije_postupci_i_oblici;
            t.AddCell(VratiCeliju4("3.4. Metodičke strategije, postupci i oblici", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Nastavna_sredstva_i_pomagala;
            t.AddCell(VratiCeliju4("3.5. Nastavna sredstva i pomagala", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Odgojno_obrazovni_sadrzaji_broj_novih_pojmova;
            t.AddCell(VratiCeliju4("3.6. Odgojno-obrazovni sadržaji, broj novih pojmova i obavijesti s obzirom" +
                " na ciljeve nastavnog sata i odgojno obrazovna postignuća učenika te kompetencije" +
                " koje se žele razvijati su:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Nastavne_metode_metodicki_postupci;
            t.AddCell(VratiCeliju4("3.7. Nastavne metode, metodički postupci i oblici:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Ciljevi_postignuca_i_kompetencije_ucenika;
            t.AddCell(VratiCeliju4("3.8. Ciljevi, postignuća i kompetencije učenika (odgojni i obrazovni)" +
                " ostvareni su:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Odnos_nastavnika_prema_ucenicima;
            t.AddCell(VratiCeliju4("3.9. Odnos učitelja/nastavnika prema učenicima:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Nastavnik_posvecuje_pozornost;
            t.AddCell(VratiCeliju4("3.10. Učitelj/nastavnik posvećuje pozornost:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Nastavnikov_nastup;
            t.AddCell(VratiCeliju4("3.11. Učiteljev/nastavnikov nastup:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Govor_nastavnika_u_skladu_je;
            t.AddCell(VratiCeliju4("3.12. Govor učitelja/nastavnika u skladu je sa standardnim hrvatskim jezikom:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Kakvim_se_stilom_poucavanja_nastavnik_koristi;
            t.AddCell(VratiCeliju4("3.13. Kakvim se stilom poučavanja učitelj/nastavnik koristi?", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("4. AKTIVNOSTI UČENIKA TIJEKOM NASTAVNOG PROCESA", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingBefore = 10;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 1 });
            s = model.Je_li_na_satu_dosao_do_izrazaja_ucenikov_rad;
            t.AddCell(VratiCeliju4("4.1. Je li na satu došao do izražaja učenikov samostalan rad?", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Postignuca_ucenika_i_produktivnost_sata_nastavnika;
            t.AddCell(VratiCeliju4("4.2. Posignuća učenika i produktivnost sata učitelj/nastavnik je provjerio:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));               
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Domaca_zadaca_zadana_je;
            t.AddCell(VratiCeliju4("4.3. Domaća zadaća zadana je:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));                
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Karakter_domace_zadace;
            t.AddCell(VratiCeliju4("4.4. Karakter domaće zadaće:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Domaca_zadaca_je_provjerena;
            t.AddCell(VratiCeliju4("4.5. Domaća zadaća je provjerena:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Zapis_na_skolskoj_ploci_bio_je;
            t.AddCell(VratiCeliju4("4.6. Zapis na školskoj ploči bio je:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Procjena_uspjesnosti_nastavnog_sata;
            t.AddCell(VratiCeliju4("4.7. Procjena uspješnosti nastavnog sata:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Evaluacija_nastavnog_sata;
            t.AddCell(VratiCeliju4("4.8. Evaluacija/samoevaluacija nastavnog sata:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Ostala_zapazanja;
            t.AddCell(VratiCeliju4("Ostala zapažanja:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("5. NASTAVNA DOKUMENTACIJA", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingBefore = 10;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 1 });
            s = model.Nastavnik_ima_i_vodi_pedagosku_dokumentaciju;
            t.AddCell(VratiCeliju4("5.1. Učitelj/nastavnik ima i vodi pedagošku dokumentaciju:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.U_dnevniku_rada_upisani_su;
            t.AddCell(VratiCeliju4("5.2. U dneviku rada upisani su:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Pripreme_nastavnika_za_nastavu_su;
            t.AddCell(VratiCeliju4("5.3. Pripreme učitelja/nastavnika za nastavu su:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Iz_imenika_je_vidljivo_da_nastavnik;
            t.AddCell(VratiCeliju4("5.4. Iz imenika je vidljivo da učitelj/nastavnik:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Ocjene_u_imeniku_su;
            t.AddCell(VratiCeliju4("5.5. Ocjene u imeniku su:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Poslovi_razrednika_izvijesca_i_analize;
            t.AddCell(VratiCeliju4("5.6. Poslovi razrednika, izviješća i analize:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }
            s = model.Procjena_vodjenja_nastavne_dokumentacije;
            t.AddCell(VratiCeliju4("5.7. Procjena vođenja nastavne dokumentacije:", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrWhiteSpace(s))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju4(s, tekst, false, BaseColor.WHITE));
            }

            t.SpacingAfter = 15;
            pdfDokument.Add(t);


            t = new PdfPTable(3);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1, 1, 1 });
            t.AddCell(VratiCeliju("Uvid obavila/obavio:", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(string.Empty, tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("Nadnevak:", tekst, false, BaseColor.WHITE));            
            t.SpacingAfter = 17;
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
        /// metoda vraća ćeliju u kojoj je tekst centriran i bez obruba
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
        /// metoda vraća ćeliju u kojoj je text centriran (vertikalno i horizontalno)
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
            c1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            c1.Padding = 5;
            c1.NoWrap = nowrap;
            return c1;
        }
        /// <summary>
        /// metoda vraća ćeliju koja ima samo donji obrub
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
            c1.Border = PdfPCell.BOTTOM_BORDER;
            return c1;
        }
        /// <summary>
        /// metoda vraća ćeliju koja nema obruba
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
            c1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c1.Padding = 5;
            c1.NoWrap = nowrap;
            c1.Border = PdfPCell.NO_BORDER;
            return c1;
        }
    }
}