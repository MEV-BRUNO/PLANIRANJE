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
    public class RoditeljBiljeskaReport
    {
        public byte[] Podaci { get; private set; }
        public RoditeljBiljeskaReport(Roditelj_biljeska model, RazredniOdjel odjel, Nastavnik razrednik, Skola skola,
            Pedagog pedagog, Ucenik ucenik, Obitelj roditelj)
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

            Paragraph p = new Paragraph("Bilješke o radu s roditeljem / skrbnikom", naslov);
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
            p = new Paragraph("UČENIK: " + ucenik.ImePrezime, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("RODITELJ: " + roditelj.ImePrezime, tekst);
            p.Alignment = Element.ALIGN_LEFT;            
            p.SpacingAfter = 15;
            pdfDokument.Add(p);

            p = new Paragraph("NASLOV DOKUMENTA: " + model.Naslov, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 15;
            pdfDokument.Add(p);

            PdfPTable t = new PdfPTable(2);
            t.SpacingBefore = 10;
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 1.5F, 5 });
            t.AddCell(VratiCeliju2("MJESEC", bold, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("MJESEČNA BILJEŠKA", bold, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju2("RUJAN", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Rujan, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju2("LISTOPAD", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Listopad, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju2("STUDENI", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Studeni, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju2("PROSINAC", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Prosinac, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju2("SIJEČANJ", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Sijecanj, tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 5;
            pdfDokument.Add(t);

            t = new PdfPTable(2);
            t.SpacingBefore = 3;
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 1.5F, 5 });
            t.AddCell(VratiCeliju2("ZAKLJUČCI", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Zakljucak1, tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 5;
            pdfDokument.Add(t);

            t = new PdfPTable(2);
            t.SpacingBefore = 7;
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 1.5F, 5 });
            t.AddCell(VratiCeliju2("MJESEC", bold, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("MJESEČNA BILJEŠKA", bold, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju2("VELJAČA", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Veljaca, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju2("OŽUJAK", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Ozujak, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju2("TRAVANJ", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Travanj, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju2("SVIBANJ", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Svibanj, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju2("LIPANJ", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Lipanj, tekst, false, BaseColor.WHITE));            
            t.SpacingAfter = 5;
            pdfDokument.Add(t);

            t = new PdfPTable(2);
            t.SpacingBefore = 3;
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 1.5F, 5 });
            t.AddCell(VratiCeliju2("ZAKLJUČCI", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Zakljucak2, tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 10;
            pdfDokument.Add(t); 

            p = new Paragraph("Ostala zapažanja", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Zapazanje))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Zapazanje, tekst, false, BaseColor.WHITE));
            }            
            t.SpacingAfter = 15;
            pdfDokument.Add(t);     

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
            c1.Padding = 20;
            c1.NoWrap = nowrap;
            return c1;
        }
    }
}