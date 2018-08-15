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
    public class PlanOs1DetailsReport
    {
        public byte[] Podaci { get; private set; }

        public PlanOs1DetailsReport(PlanOs1View plan)
        {
            Document pdfDokument = new Document(
                PageSize.A4.Rotate(), 50, 50, 20, 50);

            MemoryStream memStream = new MemoryStream();
            PdfWriter.GetInstance(pdfDokument, memStream).
                CloseStream = false;

            pdfDokument.Open();

            BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA,
                BaseFont.CP1250, false);
            Font header = new Font(font, 12, Font.NORMAL, BaseColor.DARK_GRAY);
            Font naslov = new Font(font, 14, Font.BOLDITALIC, BaseColor.BLACK);
            Font tekst = new Font(font, 9, Font.NORMAL, BaseColor.BLACK);
            Font bold = new Font(font, 9, Font.BOLD, BaseColor.BLACK);

            Paragraph p = new Paragraph("IZVJEŠTAJ", header);
            pdfDokument.Add(p);

            p = new Paragraph("Plan osnovna skola 1 - detalji", naslov);
            p.Alignment = Element.ALIGN_CENTER;
            p.SpacingBefore = 30;
            p.SpacingAfter = 30;
            pdfDokument.Add(p);

            PdfPTable t = new PdfPTable(3);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] {6, 3, 6 });

            //druga tablica
            PdfPTable table = new PdfPTable(3);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 2, 7, 3 });
            PdfPCell c = new PdfPCell(new Phrase("REDNI BROJ", bold));
            c.BackgroundColor = BaseColor.CYAN;
            c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            PdfPCell c1 = new PdfPCell(new Phrase("PODRUČJE RADA/AKTIVNOSTI", bold));
            c1.BackgroundColor = BaseColor.CYAN;
            c1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            PdfPCell c2 = new PdfPCell(new Phrase("POTREBNO SATI", bold));
            c2.BackgroundColor = BaseColor.CYAN;
            c2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            table.AddCell(c);
            table.AddCell(c1);
            table.AddCell(c2);

            t.AddCell(table);

            PdfPCell c3 = new PdfPCell(new Phrase("CILJ (po područjima)", bold));
            c3.BackgroundColor = BaseColor.CYAN;
            c3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c3.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            t.AddCell(c3);

            PdfPTable t1 = new PdfPTable(13);
            t1.WidthPercentage = 100;

            PdfPCell c4 = new PdfPCell(new Phrase("BROJ SATI", bold));
            c4.BackgroundColor = BaseColor.CYAN;
            c4.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c4.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            t1.AddCell(c4);
            PdfPCell c5 = new PdfPCell(new Phrase("IX", bold));
            c5.BackgroundColor = BaseColor.CYAN;
            c5.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c5.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            t1.AddCell(c5);
            PdfPCell c6 = new PdfPCell(new Phrase("X", bold));
            c6.BackgroundColor = BaseColor.CYAN;
            c6.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c6.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            t1.AddCell(c6);
            PdfPCell c7 = new PdfPCell(new Phrase("XI", bold));
            c7.BackgroundColor = BaseColor.CYAN;
            c7.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c7.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            t1.AddCell(c7);
            PdfPCell c8 = new PdfPCell(new Phrase("XII", bold));
            c8.BackgroundColor = BaseColor.CYAN;
            c8.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c8.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            t1.AddCell(c8);
            PdfPCell c9 = new PdfPCell(new Phrase("I", bold));
            c9.BackgroundColor = BaseColor.CYAN;
            c9.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c9.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            t1.AddCell(c9);
            PdfPCell c10 = new PdfPCell(new Phrase("II", bold));
            c10.BackgroundColor = BaseColor.CYAN;
            c10.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c10.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            t1.AddCell(c10);
            PdfPCell c11 = new PdfPCell(new Phrase("III", bold));
            c11.BackgroundColor = BaseColor.CYAN;
            c11.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c11.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            t1.AddCell(c11);
            PdfPCell c12 = new PdfPCell(new Phrase("IV", bold));
            c12.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c12.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            c12.BackgroundColor = BaseColor.CYAN;
            t1.AddCell(c12);
            PdfPCell c13 = new PdfPCell(new Phrase("V", bold));
            c13.BackgroundColor = BaseColor.CYAN;
            c13.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c13.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            t1.AddCell(c13);
            PdfPCell c14 = new PdfPCell(new Phrase("VI", bold));
            c14.BackgroundColor = BaseColor.CYAN;
            c14.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c14.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            t1.AddCell(c14);
            PdfPCell c15 = new PdfPCell(new Phrase("VII", bold));
            c15.BackgroundColor = BaseColor.CYAN;
            c15.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c15.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            t1.AddCell(c15);
            PdfPCell c16 = new PdfPCell(new Phrase("VIII", bold));
            c16.BackgroundColor = BaseColor.CYAN;
            c16.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c16.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            t1.AddCell(c16);

            t.AddCell(t1);
            //druga tablica

            //t.AddCell(VratiCeliju("R.br.", tekst, true, BaseColor.LIGHT_GRAY));
            //t.AddCell(VratiCeliju("Ak. godina", tekst, false, BaseColor.LIGHT_GRAY));
            //t.AddCell(VratiCeliju("Naziv", tekst, true, BaseColor.LIGHT_GRAY));
            //t.AddCell(VratiCeliju("Opis", tekst, true, BaseColor.LIGHT_GRAY));

            int i = 0;
            plan.OsPlan1Podrucje = plan.OsPlan1Podrucje.OrderBy(o => o.Red_br_podrucje).ToList();

            foreach (var item in plan.OsPlan1Podrucje)
            {
                i++;
                PdfPTable t2 = new PdfPTable(3);
                t2.WidthPercentage = 100;
                t2.SetWidths(new float[] { 2, 7, 3 });
                t2.AddCell(VratiCeliju(i.ToString()+".", bold, true, BaseColor.WHITE));
                t2.AddCell(VratiCeliju(plan.PodrucjeRada.Single(s => s.Id_podrucje == item.Opis_Podrucje).Naziv, bold, false, BaseColor.WHITE));
                t2.AddCell(VratiCeliju(item.Potrebno_sati, bold, false, BaseColor.WHITE));

                List<OS_Plan_1_aktivnost> aktivnosti = new List<OS_Plan_1_aktivnost>();
                aktivnosti = plan.OsPlan1Aktivnost.Where(w => w.Id_podrucje == item.Id_plan).ToList();
                aktivnosti = aktivnosti.OrderBy(o => o.Red_broj_aktivnost).ToList();

                int x = 1;
                foreach(var ak in aktivnosti)
                {                    
                    t2.AddCell(VratiCeliju(i.ToString() + "." + (x++).ToString(), tekst, true, BaseColor.WHITE));
                    t2.AddCell(VratiCeliju(plan.Aktivnosti.Single(s => s.Id_aktivnost == ak.Opis_aktivnost).Naziv, tekst, false, BaseColor.WHITE));
                    t2.AddCell(VratiCeliju(ak.Potrebno_sati, tekst, false, BaseColor.WHITE));
                }

                t.AddCell(t2);
                t.AddCell(VratiCeliju(plan.Ciljevi.Single(s => s.ID_cilj == item.Cilj).Naziv, tekst, false, BaseColor.WHITE));

                PdfPTable t3 = new PdfPTable(13);
                t3.WidthPercentage = 100;
                t3.AddCell(VratiCeliju(item.Br_sati.ToString(), bold, false, BaseColor.WHITE));
                t3.AddCell(VratiCeliju(item.Mj_9.ToString(), bold, false, BaseColor.WHITE));
                t3.AddCell(VratiCeliju(item.Mj_10.ToString(), bold, false, BaseColor.WHITE));
                t3.AddCell(VratiCeliju(item.Mj_11.ToString(), bold, false, BaseColor.WHITE));
                t3.AddCell(VratiCeliju(item.Mj_12.ToString(), bold, false, BaseColor.WHITE));
                t3.AddCell(VratiCeliju(item.Mj_1.ToString(), bold, false, BaseColor.WHITE));
                t3.AddCell(VratiCeliju(item.Mj_2.ToString(), bold, false, BaseColor.WHITE));
                t3.AddCell(VratiCeliju(item.Mj_3.ToString(), bold, false, BaseColor.WHITE));
                t3.AddCell(VratiCeliju(item.Mj_4.ToString(), bold, false, BaseColor.WHITE));
                t3.AddCell(VratiCeliju(item.Mj_5.ToString(), bold, false, BaseColor.WHITE));
                t3.AddCell(VratiCeliju(item.Mj_6.ToString(), bold, false, BaseColor.WHITE));
                t3.AddCell(VratiCeliju(item.Mj_7.ToString(), bold, false, BaseColor.WHITE));
                t3.AddCell(VratiCeliju(item.Mj_8.ToString(), bold, false, BaseColor.WHITE));
                foreach (var akt in aktivnosti)
                {
                    t3.AddCell(VratiCeliju(akt.Br_sati.ToString(), tekst, false, BaseColor.WHITE));                    
                    t3.AddCell(VratiCeliju(akt.Mj_9.ToString(), tekst, false, BaseColor.WHITE));
                    t3.AddCell(VratiCeliju(akt.Mj_10.ToString(), tekst, false, BaseColor.WHITE));
                    t3.AddCell(VratiCeliju(akt.Mj_11.ToString(), tekst, false, BaseColor.WHITE));
                    t3.AddCell(VratiCeliju(akt.Mj_12.ToString(), tekst, false, BaseColor.WHITE));
                    t3.AddCell(VratiCeliju(akt.Mj_1.ToString(), tekst, false, BaseColor.WHITE));
                    t3.AddCell(VratiCeliju(akt.Mj_2.ToString(), tekst, false, BaseColor.WHITE));
                    t3.AddCell(VratiCeliju(akt.Mj_3.ToString(), tekst, false, BaseColor.WHITE));
                    t3.AddCell(VratiCeliju(akt.Mj_4.ToString(), tekst, false, BaseColor.WHITE));
                    t3.AddCell(VratiCeliju(akt.Mj_5.ToString(), tekst, false, BaseColor.WHITE));
                    t3.AddCell(VratiCeliju(akt.Mj_6.ToString(), tekst, false, BaseColor.WHITE));
                    t3.AddCell(VratiCeliju(akt.Mj_7.ToString(), tekst, false, BaseColor.WHITE));
                    t3.AddCell(VratiCeliju(akt.Mj_8.ToString(), tekst, false, BaseColor.WHITE));
                }                
                t.AddCell(t3);
            }

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
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            c1.Padding = 5;
            c1.NoWrap = false;                      
            return c1;
        }
    }
}
