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
    public class NastavnikProtokolReport
    {
        public byte[] Podaci { get; private set; }
        public NastavnikProtokolReport(Nastavnik_protokol model, Pedagog pedagog, Skola skola, RazredniOdjel odjel, Nastavnik nastavnik)
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

            Paragraph p = new Paragraph("Protokol praćenja odgojno-obrazovnog procesa", naslov);
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
            p = new Paragraph("UČITELJ/NASTAVNIK: " + nastavnik.ImePrezime, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("NASTAVNI PREDMET: " + model.Nastavni_predmet, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("NADNEVAK: " + model.Datum.ToShortDateString()+" VRIJEME: "+model.Vrijeme.ToShortTimeString()
                +" RED.BR. NASTAVNOG SATA: "+ model.Broj_nastavnog_sata, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("MJESTO IZVOĐENJA: " + model.Mjesto_izvodjenja, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("NASTAVNA CJELINA: " + model.Nastavna_cjelina, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("NASTAVNA JEDINICA: "+ model.Nastavna_jedinica, tekst);
            p.Alignment = Element.ALIGN_LEFT;            
            p.SpacingAfter = 20;
            pdfDokument.Add(p);

            List<string> select = new List<string> { "-", "ne", "donekle", "uglavnom", "potpuno" };
            List<string> select2 = new List<string> { "-", "Obrada novog gradiva", "Ponavljanje", "Vježbanje", "Provjeravanje," +
                " vrjednovanje i ocjenjivanje", "Kombinirani sat" };
                            
            p = new Paragraph("Priprava za nastavni sat sadržava (ne, donekle, uglavnom, potpuno):", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingBefore = 14;
            p.SpacingAfter = 7;
            pdfDokument.Add(p);

            PdfPTable t = new PdfPTable(2);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 1, 1 });
            t.AddCell(VratiCeliju("Cilj i zadatke za nastavnu jedinicu", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Cilj_i_zadaci_za_nastavnu_jedinicu), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("Strukturu sadržaja", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Struktura_sadrzaja), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("Plan i shvatljiv koncept", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Plan_i_shvatljiv_koncept), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("Plan ploče / prozirnice / PPT prezentacije", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Plan_ploce), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(" ", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(" ", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("Tip nastavnog sata", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select2.ElementAt(model.Tip_nastavnog_sata), tekst, false, BaseColor.WHITE));            
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            select = new List<string> {"-", "nije uočeno", "često uočeno", "istaknuto" };
            select2 = new List<string> {"-", "Frontalni", "Individualni", "Rad u skupinama", "Rad u parovima" };
            p = new Paragraph("Primjena nastavnih metoda (nije uočeno, često uočeno, istaknuto):", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 7;
            pdfDokument.Add(p);

            t = new PdfPTable(2);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 1, 1 });
            t.AddCell(VratiCeliju("verbalnih", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Verbalne), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("vizualno-dokumentacijskih", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Vizualno_dokumentacijske), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("demonstracijskih", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Demonstracijske), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("prakseoloških", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Prakseoloske), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("kombiniranih", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Kombinirane), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(" ", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(" ", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("Sociološki oblici rada", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select2.ElementAt(model.Socioloski_oblici_rada), tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("Korištenje nastavnih sredstava, pomagala i tehnologija:", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 7;
            pdfDokument.Add(p);          

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Koristenje_nastavnih_sredstava))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Koristenje_nastavnih_sredstava, tekst, false, BaseColor.WHITE));
            }

            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            select2 = new List<string> {"-", "nepripremljeno", "donekle", "uglavnom", "potpuno" };
            p = new Paragraph("Funkcionalna pripremljenost (nepripremljeno, donekle, uglavnom, potpuno):    "+select2.ElementAt(model.Funkcionalna_pripremljenost), tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 15;
            pdfDokument.Add(p);

            select = new List<string> {"-", "nije uočeno", "često uočeno", "istaknuto" };
            p = new Paragraph("Motivacijska priprema učenika (nije uočeno, često uočeno, istaknuto):", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 7;
            pdfDokument.Add(p);

            t = new PdfPTable(2);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 1, 1 });
            t.AddCell(VratiCeliju("Uvod i najava cilja", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Uvod_i_najava_cilja), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("Uspostavljanje komunikacije", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Uspostavljanje_komunikacije), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("Humor u nastavi", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Humor_u_nastavi), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("Održavanje pažnje", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Odrzavanje_paznje), tekst, false, BaseColor.WHITE));            
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("Prezentacijske vještine", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 7;
            pdfDokument.Add(p);

            t = new PdfPTable(3);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 1, 1.25F, 1 });
            t.AddCell(VratiCeliju2("ŠTO UČITELJ/NASTAVNIK RADI", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("STRUKTURNE KOMPONENTE I PRIBLIŽNO VREMENSKO TRAJANJE", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("TIJEK AKTIVNOSTI", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrEmpty(model.Sto_nastavnik_radi))
            {
                t.AddCell(VratiCeliju2("\n\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju2(model.Sto_nastavnik_radi, tekst, false, BaseColor.WHITE));
            }
            if (string.IsNullOrEmpty(model.Nas_strukturne_komponente))
            {
                t.AddCell(VratiCeliju2("\n\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju2(model.Nas_strukturne_komponente, tekst, false, BaseColor.WHITE));
            }
            if (string.IsNullOrEmpty(model.Nas_tijek_aktivnosti))
            {
                t.AddCell(VratiCeliju2("\n\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju2(model.Nas_tijek_aktivnosti, tekst, false, BaseColor.WHITE));
            }            
            t.AddCell(VratiCeliju2("ŠTO UČENICI RADI", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("STRUKTURNE KOMPONENTE I PRIBLIŽNO VREMENSKO TRAJANJE", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("TIJEK AKTIVNOSTI", tekst, false, BaseColor.WHITE));
            if (string.IsNullOrEmpty(model.Sto_ucenici_rade))
            {
                t.AddCell(VratiCeliju2("\n\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju2(model.Sto_ucenici_rade, tekst, false, BaseColor.WHITE));
            }
            if (string.IsNullOrEmpty(model.Uc_strukturne_komponente))
            {
                t.AddCell(VratiCeliju2("\n\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju2(model.Uc_strukturne_komponente, tekst, false, BaseColor.WHITE));
            }
            if (string.IsNullOrEmpty(model.Uc_tijek_aktivnosti))
            {
                t.AddCell(VratiCeliju2("\n\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju2(model.Uc_tijek_aktivnosti, tekst, false, BaseColor.WHITE));
            }            
            t.SpacingAfter = 14;
            pdfDokument.Add(t);

            select = new List<string> {"-", "ne", "donekle", "uglavnom", "potpuno" };
            select2 = new List<string> { "-", "Da", "Ne" };
            p = new Paragraph("Procjena učiteljeve/nastavnikove komunikacije s učenicima (ne, donekle, uglavnom, potpuno):", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 7;
            pdfDokument.Add(p);

            t = new PdfPTable(2);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 2.25F, 1 });
            t.AddCell(VratiCeliju("razgovara s učenicima", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Razgovor_s_ucenicima), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("ohrabruje učenike za iznošenje mišljenja i hvali ponašanje", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Ohrabruje_ucenike_za_iznosenje_misljenja), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("obeshrabruje učenikovu aktivnost ili ponašanje", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Obeshrabruje_ucenikovu_aktivnost), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("uvažava učeničke primjedbe, pitanja i odgovore", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Uvazava_ucenicke_primjedbe), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("kritizira ili se poziva na svoj autoritet", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Kritizira_ili_se_poziva_na_svoj_autoritet), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("pokazuje empatiju", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Pokazuje_empatiju), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("pomaže učenicima koji imaju teškoće u radu", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Pomaze_ucenicima_koji_imaju_teskoce), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("neverbalnim porukama pridonosi pozitivnom radnom ozračju u učionici", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Neverbalnim_porukama_pridonosi_pozitivnom_radnom_ozracju), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("učenik ima mogućnost i inicijativu slobodnog iznošenja stavova i mišljenja", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select.ElementAt(model.Ucenik_ima_mogucnost_i_inicijativu_slobodnog_iznosenja_stavova), tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("Domaća zadaća:", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 7;
            pdfDokument.Add(p);

            t = new PdfPTable(2);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 2.25F, 1 });
            t.AddCell(VratiCeliju("Učitelj/nastavnik redovito provjerava uratke", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select2.ElementAt(model.Nastavnik_redovito_provjerava_uratke), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("Daje povratnu informaciju", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select2.ElementAt(model.Daje_povratnu_informaciju), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("Koristi se domaćom zadaćom kao podlogom za razrednu raspravu", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select2.ElementAt(model.Koristi_se_domacom_zadacom_kao_podlogom), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(" ", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(" ", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("Učitelj/nastavnik daje ocjenu za učenje u razredu", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(select2.ElementAt(model.Daje_ocjenu_za_ucenje_u_razredu), tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("Kratak komentar učitelja/nastavnika na održani nastavni sat:", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 7;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Kratki_komentar_nastavnika))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Kratki_komentar_nastavnika, tekst, false, BaseColor.WHITE));
            }
            t.SpacingAfter = 14;
            pdfDokument.Add(t);

            p = new Paragraph("Prijedlozi za daljnje unapređenje rada:", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 7;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Prijedlozi_za_daljnje_unapredjenje_rada))
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Prijedlozi_za_daljnje_unapredjenje_rada, tekst, false, BaseColor.WHITE));
            }
                    
            t.SpacingAfter = 20;
            pdfDokument.Add(t);     

            t = new PdfPTable(3);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1, 1, 1 });
            t.AddCell(VratiCeliju4("Stručni suradnik:", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4("Učitelj/nastavnik:", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4("Ravnatelj:", tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 17;
            pdfDokument.Add(t);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            t.AddCell(VratiCeliju2(string.Empty, tekst, false, BaseColor.WHITE));            
            t.SpacingAfter = 14;
            pdfDokument.Add(t);

            pdfDokument.Close();
            Podaci = memStream.ToArray();
        }
        private PdfPCell VratiCeliju(string labela, Font font,
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
        private PdfPCell VratiCeliju4(string labela, Font font,
           bool nowrap, BaseColor boja)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(labela, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c1.Padding = 5;
            c1.NoWrap = nowrap;
            c1.Border = PdfPCell.NO_BORDER;
            return c1;
        }
    }
}