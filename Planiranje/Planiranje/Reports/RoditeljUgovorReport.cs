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
    public class RoditeljUgovorReport
    {
        public byte[] Podaci { get; private set; }
        public RoditeljUgovorReport(Roditelj_ugovor model, Skola skola, Nastavnik razrednik, Pedagog pedagog, RazredniOdjel odjel,
            Ucenik ucenik, Obitelj roditelj)
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

            Paragraph p = new Paragraph("Ugovor o ponašanju", naslov);
            p.Alignment = Element.ALIGN_CENTER;
            p.SpacingBefore = 20;
            p.SpacingAfter = 20;
            pdfDokument.Add(p);
            //dodano
            IFormatProvider culture = new System.Globalization.CultureInfo("hr-HR", true);
            string ime = ucenik.Spol == 1 ? "Učenik" : "Učenica";
            string ime1 = roditelj.Svojstvo < 3 ? "Roditelj" : "Skrbnik";
            //dodano-kraj
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
            p = new Paragraph(ime.ToUpper()+": " + ucenik.ImePrezime, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph(ime1.ToUpper()+": " + roditelj.ImePrezime, tekst);
            p.Alignment = Element.ALIGN_LEFT;            
            pdfDokument.Add(p);
            p = new Paragraph("PREDSTAVNIK ŠKOLE: " + model.Predstavnik_skole, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("UGOVOR SKLOPLJEN DANA: " + model.Datum.GetDateTimeFormats(culture).ElementAt(8), tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 15;
            pdfDokument.Add(p);

            p = new Paragraph("S obzirom na moje buduće ponašanje slažem se da ću", tekst);
            p.Alignment = Element.ALIGN_LEFT;            
            pdfDokument.Add(p);
            p = new Paragraph("PRVI CILJ: "+model.Cilj1, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("DRUGI CILJ: "+model.Cilj2, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 7;
            pdfDokument.Add(p);

            p = new Paragraph("Kako bih pomogla / pomogao popravljanju ponašanja, ja " + roditelj.ImePrezime 
                + " poduzeti ću slijedeće: " + model.Poduzeto, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 7;
            pdfDokument.Add(p);

            
            p = new Paragraph(ime+":_____________________________", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("Predstavnik škole:_____________________________", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph(ime1+":_____________________________", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("Slijedeći susret: "+ model.Slijedeci_susret.ToShortDateString(), tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 15;
            pdfDokument.Add(p);

            p = new Paragraph("Ostala zapažanja", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);

            PdfPTable t = new PdfPTable(1);
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

            t = new PdfPTable(2);
            t.SpacingBefore = 10;
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 1.5F, 5 });
            t.AddCell(VratiCeliju2("R. BR.", bold, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("MJESEČNA BILJEŠKA", bold, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju2("1", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Biljeska1, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju2("2", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Biljeska2, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju2("3", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Biljeska3, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju2("4", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Biljeska4, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju2("5", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Biljeska5, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju2("6", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.Biljeska6, tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("Izvješće na kraju isteka ugovora", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Izvjesce))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Izvjesce, tekst, false, BaseColor.WHITE));
            }
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("Ostala zapažanja", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.Ostala_zapazanja))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.Ostala_zapazanja, tekst, false, BaseColor.WHITE));
            }
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            //t = new PdfPTable(2);
            //t.SpacingBefore = 3;
            //t.WidthPercentage = 100;
            //t.SetWidths(new float[] { 1.5F, 5 });
            //t.AddCell(VratiCeliju2("ZAKLJUČCI", tekst, false, BaseColor.WHITE));
            //t.AddCell(VratiCeliju(model.Zakljucak1, tekst, false, BaseColor.WHITE));
            //t.SpacingAfter = 5;
            //pdfDokument.Add(t);

            //t = new PdfPTable(2);
            //t.SpacingBefore = 7;
            //t.WidthPercentage = 100;
            //t.SetWidths(new float[] { 1.5F, 5 });
            //t.AddCell(VratiCeliju2("MJESEC", bold, false, BaseColor.WHITE));
            //t.AddCell(VratiCeliju2("MJESEČNA BILJEŠKA", bold, false, BaseColor.WHITE));

            //t.AddCell(VratiCeliju2("VELJAČA", tekst, false, BaseColor.WHITE));
            //t.AddCell(VratiCeliju(model.Veljaca, tekst, false, BaseColor.WHITE));

            //t.AddCell(VratiCeliju2("OŽUJAK", tekst, false, BaseColor.WHITE));
            //t.AddCell(VratiCeliju(model.Ozujak, tekst, false, BaseColor.WHITE));

            //t.AddCell(VratiCeliju2("TRAVANJ", tekst, false, BaseColor.WHITE));
            //t.AddCell(VratiCeliju(model.Travanj, tekst, false, BaseColor.WHITE));

            //t.AddCell(VratiCeliju2("SVIBANJ", tekst, false, BaseColor.WHITE));
            //t.AddCell(VratiCeliju(model.Svibanj, tekst, false, BaseColor.WHITE));

            //t.AddCell(VratiCeliju2("LIPANJ", tekst, false, BaseColor.WHITE));
            //t.AddCell(VratiCeliju(model.Lipanj, tekst, false, BaseColor.WHITE));
            //t.SpacingAfter = 5;
            //pdfDokument.Add(t);

            //t = new PdfPTable(2);
            //t.SpacingBefore = 3;
            //t.WidthPercentage = 100;
            //t.SetWidths(new float[] { 1.5F, 5 });
            //t.AddCell(VratiCeliju2("ZAKLJUČCI", tekst, false, BaseColor.WHITE));
            //t.AddCell(VratiCeliju(model.Zakljucak2, tekst, false, BaseColor.WHITE));
            //t.SpacingAfter = 10;
            //pdfDokument.Add(t);            

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