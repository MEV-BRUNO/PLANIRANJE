using iTextSharp.text;
using iTextSharp.text.pdf;
using Planiranje.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Planiranje.Reports
{
    public class MjesecniPlanReport
    {
        public byte[] Podaci { get; private set; }

        public MjesecniPlanReport(MjesecniModel model)
        {
            // generiranje pdf-a

            // novi dokument, sa veličinom stranice i marginama
            // mjere u iText# -> point = 1/72 inch
            Document pdfDokument = new Document(
                PageSize.A4, 50, 50, 20, 50);

            MemoryStream memStream = new MemoryStream();
            PdfWriter.GetInstance(pdfDokument, memStream).
                CloseStream = false;

            // otvaranje dokumenta
            pdfDokument.Open();

            // dodamo neki sadržaj za test
            //pdfDokument.Add(new Paragraph("Test 123..."));

            // fontovi
            BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA,
                BaseFont.CP1250, false);
            Font header = new Font(font, 12, Font.NORMAL, BaseColor.DARK_GRAY);
            Font naslov = new Font(font, 14, Font.BOLDITALIC, BaseColor.BLACK);
            Font tekst = new Font(font, 10, Font.NORMAL, BaseColor.BLACK);

            // logo
            /*
            var logo = iTextSharp.text.Image.GetInstance(
                HostingEnvironment.MapPath("~/Content/MEV_LOGO.jpg"));
            logo.Alignment = Element.ALIGN_LEFT;
            logo.ScaleAbsoluteHeight(100);
            logo.ScaleAbsoluteWidth(100);
            pdfDokument.Add(logo);*/

            // header
            Paragraph p = new Paragraph("MJESEČNI (TJEDNI) PLAN I PROGRAM za: " + model.MjesecniPlan.Ak_godina, header);
            pdfDokument.Add(p);

            // naslov 
            p = new Paragraph("Mjesečni plan", naslov);
            p.Alignment = Element.ALIGN_CENTER;
            p.SpacingBefore = 30;
            p.SpacingAfter = 30;
            pdfDokument.Add(p);

            // tablica sa popisom studenata
            PdfPTable t = new PdfPTable(6); // 5 kolona
            t.WidthPercentage = 100; // širina tablice
            t.SetWidths(new float[] { 2, 2, 2, 2, 1, 3 });

            // dodati zaglavlje
            t.AddCell(VratiCeliju("PODRUČJE/\nSUBJEKT RADA", tekst, true, BaseColor.LIGHT_GRAY));
            t.AddCell(VratiCeliju("AKTIVNOSTI/\nSADRŽAJI", tekst, false, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju("SURADNICI", tekst, true, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju("VRIJEME\nOSTVARENJA", tekst, true, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju("BROJ\nSATI", tekst, true, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju("BILJEŠKA O\nREALIZACIJI", tekst, true, BaseColor.LIGHT_GRAY));


			// dodajemo popis studenata
			//int i = 1;
            foreach (Mjesecni_detalji detalj in model.MjesecniDetalji)
            {
                t.AddCell(VratiCeliju(detalj.Podrucje, tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(detalj.Aktivnost, tekst, false, BaseColor.WHITE));
				t.AddCell(VratiCeliju(detalj.Suradnici, tekst, false, BaseColor.WHITE));
				t.AddCell(VratiCeliju(detalj.Vrijeme.ToShortDateString(), tekst, false, BaseColor.WHITE));
				t.AddCell(VratiCeliju(detalj.Br_sati.ToString(), tekst, false, BaseColor.WHITE));
				t.AddCell(VratiCeliju(detalj.Biljeska, tekst, false, BaseColor.WHITE));
			}

            // dodati tablicu na dokument
            pdfDokument.Add(t);

            // zatvaranje dokumenta
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
    }
}