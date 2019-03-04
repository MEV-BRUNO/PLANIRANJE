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
    public class RoditeljRazgovorReport
    {
        public byte[] Podaci { get; private set; }
        public RoditeljRazgovorReport(Roditelj_razgovor model, Ucenik ucenik, Skola skola, RazredniOdjel odjel, Pedagog pedagog,
            Obitelj roditelj)
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

            Paragraph p = new Paragraph("Obrazac razgovora s roditeljima / skrbnicima", naslov);
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
            p = new Paragraph("UČENIK: " + ucenik.ImePrezime, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("RODITELJ: " + roditelj.ImePrezime, tekst);
            p.Alignment = Element.ALIGN_LEFT;            
            pdfDokument.Add(p);
            p = new Paragraph("NADNEVAK I VRIJEME SUSRETA: " + model.Datum.ToShortDateString() + " " + model.Vrijeme.ToShortTimeString(), tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 20;
            pdfDokument.Add(p);

            p = new Paragraph("Razgovor traži: " + model.Trazi, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingBefore = 10;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            p = new Paragraph("Razlog: " + model.Razlog, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);            

            p = new Paragraph("Roditelj / skrbnik došao je " + model.Dolazak, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);            

            p = new Paragraph("Bilješke o razgovoru", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            PdfPTable t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Biljeska))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Biljeska, tekst, false, BaseColor.WHITE));
            }

            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("Prijedlog roditelja / skrbnika", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Prijedlog_roditelja))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Prijedlog_roditelja, tekst, false, BaseColor.WHITE));
            }

            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("Prijedlog škole", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Prijedlog_skole))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Prijedlog_skole, tekst, false, BaseColor.WHITE));
            }

            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("Dogovoreno", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Dogovor))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Dogovor, tekst, false, BaseColor.WHITE));
            }

            t.SpacingAfter = 14;
            pdfDokument.Add(t);

            p = new Paragraph("O poduzetom treba izvijestiti: " + model.Izvjestiti, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);            

            p = new Paragraph("Vrijeme slijedećeg susreta: " + model.Datum_slijedeci.ToShortDateString(), tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);             

            p = new Paragraph("Stručni suradnik: " + pedagog.Ime + " " + pedagog.Prezime + ", " + pedagog.Titula, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingBefore = 14;
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