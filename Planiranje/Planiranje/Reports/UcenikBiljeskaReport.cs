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
    public class UcenikBiljeskaReport
    {
        public byte[] Podaci { get; private set; }
        public UcenikBiljeskaReport(UcenikBiljeskaModel model, Skola skola, RazredniOdjel odjel, Pedagog pedagog)
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

            Paragraph p = new Paragraph("Bilješke o radu s učenikom ", naslov);
            p.Alignment = Element.ALIGN_CENTER;
            p.SpacingBefore = 30;
            p.SpacingAfter = 30;
            pdfDokument.Add(p);

            p = new Paragraph("ŠKOLA: " + skola.Naziv, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("IME I PREZIME UČENIKA: " + model.Ucenik.ImePrezime, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("RAZREDNI ODJEL: " + odjel.Naziv, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("ŠKOLSKA GODINA: " + odjel.Sk_godina + "./" + (odjel.Sk_godina + 1).ToString() + ".", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("DAN, MJESEC I GODINA ROĐENJA: " + model.Ucenik.Datum.ToShortDateString(), tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("ADRESA STANOVANJA: " + model.Ucenik.Adresa, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("MJESTO STANOVANJA: " + model.Ucenik.Grad, tekst);
            p.Alignment = Element.ALIGN_LEFT;            
            p.SpacingAfter = 25;
            pdfDokument.Add(p);
            p = new Paragraph("PODACI O OBITELJI", bold);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            PdfPTable t = new PdfPTable(3);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 5, 2.5F, 2.5F });

            //t.AddCell(VratiCeliju2("R.br.", bold, true, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("IME I PREZIME", bold, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("ZANIMANJE", bold, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("KONTAKT", bold, false, BaseColor.WHITE));

            foreach(var item in model.ListaObitelji)
            {
                t.AddCell(VratiCeliju(item.ImePrezime, tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Zanimanje, tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Kontakt, tekst, false, BaseColor.WHITE));
            }
            if (model.ListaObitelji.Count == 0)
            {
                t.AddCell(VratiCeliju(" ", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(" ", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(" ", tekst, false, BaseColor.WHITE));
            }

            pdfDokument.Add(t);

            p = new Paragraph("INICIJALNI PODACI O UČENIKU (postignut uspjeh i dr.)", bold);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingBefore = 14;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.UcenikBiljeska.Inicijalni_podaci))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.UcenikBiljeska.Inicijalni_podaci, tekst, false, BaseColor.WHITE));
            }            
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            t = new PdfPTable(2);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1, 4 });
            t.AddCell(VratiCeliju2("MJESEC", bold, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("MJESEČNA BILJEŠKA", bold, false, BaseColor.WHITE));

            List<string> mjeseci = new List<string>() { "", "Siječanj", "Veljača", "Ožujak", "Travanj", "Svibanj", "Lipanj", "Srpanj", "Kolovoz", "Rujan", "Listopad", "Studeni", "Prosinac" };

            List<Mjesecna_biljeska> biljeske = model.MjesecneBiljeske.Where(w => w.Mjesec >= 9 && w.Mjesec <= 12).ToList();
            biljeske = biljeske.OrderBy(o => o.Mjesec).ToList();
            foreach (var item in biljeske)
            {
                //t.AddCell(VratiCeliju((br++).ToString() + ".", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(mjeseci.ElementAt(item.Mjesec).ToString(), tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Biljeska, tekst, false, BaseColor.WHITE));
            }
            biljeske = model.MjesecneBiljeske.Where(w => w.Mjesec >= 1 && w.Mjesec < 9).OrderBy(o => o.Mjesec).ToList();
            foreach (var item in biljeske)
            {                
                t.AddCell(VratiCeliju(mjeseci.ElementAt(item.Mjesec).ToString(), tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Biljeska, tekst, false, BaseColor.WHITE));
            }
            if (biljeske.Count == 0)
            {
                t.AddCell(VratiCeliju(" ", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(" ", tekst, false, BaseColor.WHITE));
            }

            pdfDokument.Add(t);

            p = new Paragraph("OSTALA ZAPAŽANJA", bold);
            p.SpacingBefore = 14;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            if (string.IsNullOrEmpty(model.UcenikBiljeska.Zapazanje))
            {
                t.AddCell(VratiCeliju("\n\n\n", tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju(model.UcenikBiljeska.Zapazanje, tekst, false, BaseColor.WHITE));
            }
                    
            pdfDokument.Add(t);

            p = new Paragraph("Stručni suradnik: "+pedagog.Ime+" "+pedagog.Prezime+", "+pedagog.Titula, tekst);
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