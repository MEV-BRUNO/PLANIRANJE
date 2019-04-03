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
    public class NastavnikAnalizaReport
    {
        public byte[] Podaci { get; private set; }
        public NastavnikAnalizaReport(Nastavnik_analiza model, Skola skola, Nastavnik razrednik, Pedagog pedagog, RazredniOdjel odjel,
            Nastavnik nastavnik)
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

            Paragraph p = new Paragraph("Analiza nastavnog sata", naslov);
            p.Alignment = Element.ALIGN_CENTER;
            p.SpacingBefore = 20;
            p.SpacingAfter = 20;
            pdfDokument.Add(p);

            p = new Paragraph("ŠKOLA: " + skola.Naziv, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("RAZREDNI ODJEL: " + odjel.Naziv, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("ŠKOLSKA GODINA: " + odjel.Sk_godina + "./" + (odjel.Sk_godina + 1).ToString() + ".", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("RAZREDNIK: " + razrednik.ImePrezime, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);            
            p = new Paragraph("STRUČNI SURADNIK PEDAGOG: " + pedagog.Ime+" "+pedagog.Prezime+", "+pedagog.Titula, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 20;
            pdfDokument.Add(p);

            p = new Paragraph("Cilj posjete: " + model.Cilj_posjete, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingBefore = 10;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            p = new Paragraph("Nadnevak: " + model.Datum.ToShortDateString(), tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            p = new Paragraph("Nastavni sat: " + model.Nastavni_sat, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            p = new Paragraph("Nastavnik: " + nastavnik.ImePrezime, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            p = new Paragraph("Predmet: " + model.Predmet, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            p = new Paragraph("Nastavna cjelina / nastavna jedinica", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            PdfPTable t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Nastavna_jedinica))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Nastavna_jedinica, tekst, false, BaseColor.WHITE));
            }

            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("Vrsta nastavnog sata: " + model.Vrsta_nastavnog_sata, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            p = new Paragraph("Planiranje i priprema", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Planiranje_priprema))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Planiranje_priprema, tekst, false, BaseColor.WHITE));
            }

            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("Izvedba nastavnog sata", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Izvedba_nastavnog_sata))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Izvedba_nastavnog_sata, tekst, false, BaseColor.WHITE));
            }

            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("Vođenje i tijek nastavnog sata", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Vodjenje_nastavnog_sata))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Vodjenje_nastavnog_sata, tekst, false, BaseColor.WHITE));
            }

            t.SpacingAfter = 14;
            pdfDokument.Add(t);

            p = new Paragraph("Razredni ugođaj", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Razredni_ugodjaj))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Razredni_ugodjaj, tekst, false, BaseColor.WHITE));
            }

            t.SpacingAfter = 14;
            pdfDokument.Add(t);

            p = new Paragraph("Disciplina", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Disciplina))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Disciplina, tekst, false, BaseColor.WHITE));
            }

            t.SpacingAfter = 14;
            pdfDokument.Add(t);

            p = new Paragraph("Ocjenjivanje napredovanja učenika", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Ocjenjivanje_ucenika))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Ocjenjivanje_ucenika, tekst, false, BaseColor.WHITE));
            }

            t.SpacingAfter = 14;
            pdfDokument.Add(t);

            p = new Paragraph("Osvrt i prosudba vlastitog rada", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Osvrt))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Osvrt, tekst, false, BaseColor.WHITE));
            }

            t.SpacingAfter = 14;
            pdfDokument.Add(t);

            p = new Paragraph("Prijedlozi za unapređenje", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Prijedlozi))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Prijedlozi, tekst, false, BaseColor.WHITE));
            }

            t.SpacingAfter = 14;
            pdfDokument.Add(t);

            p = new Paragraph("Uvid u vođenje pedagoške dokumentacije", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Uvid))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Uvid, tekst, false, BaseColor.WHITE));
            }

            t.SpacingAfter = 14;
            pdfDokument.Add(t);

            p = new Paragraph("Stručni suradnik pedagog: " + pedagog.Ime+" "+pedagog.Prezime+", "+pedagog.Titula , tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);          

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
            return c1;
        }
        private PdfPCell VratiCeliju2(string labela, Font font,
            bool nowrap, BaseColor boja)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(labela, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            c1.Padding = 2;
            c1.NoWrap = nowrap;
            return c1;
        }
    }
}