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

        public PlanOs1DetailsReport(PlanOs1View plan, Pedagog ped)
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

            Paragraph p = new Paragraph(ped.Ime+" "+ped.Prezime+", "+ped.Titula, header);
            pdfDokument.Add(p);

            p = new Paragraph("Naziv plana: " + plan.OsPlan1.Naziv, header);
            pdfDokument.Add(p);

            p = new Paragraph("Okvirni plan i program rada stručnog suradnika za osnovnu školu u šk. god. "+plan.OsPlan1.Ak_godina, naslov);
            p.Alignment = Element.ALIGN_CENTER;
            p.SpacingBefore = 30;
            p.SpacingAfter = 30;
            pdfDokument.Add(p);

            PdfPTable t = new PdfPTable(17);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] {1, 5, 2, 4,1,1,1,1,1,1,1,1,1,1,1,1,1 });

            t.AddCell(VratiCeliju2("REDNI BROJ", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("PODRUČJE RADA/AKTIVNOSTI", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("POTREBNO SATI", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("CILJ (po područjima)", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("BROJ SATI", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("IX", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("X", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("XI", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("XII", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("I", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("II", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("III", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("IV", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("V", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("VI", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("VII", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("VIII", bold, false, BaseColor.CYAN));
            

            int i = 0;
            plan.OsPlan1Podrucje = plan.OsPlan1Podrucje.OrderBy(o => o.Red_br_podrucje).ToList();

            foreach (var item in plan.OsPlan1Podrucje)
            {
                i++;
                
                t.AddCell(VratiCeliju(i.ToString()+".", bold, true, BaseColor.WHITE));
                t.AddCell(VratiCeliju(plan.PodrucjeRada.Single(s => s.Id_podrucje == item.Opis_Podrucje).Naziv, bold, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Potrebno_sati, bold, false, BaseColor.WHITE));         

                List<OS_Plan_1_aktivnost> aktivnosti = new List<OS_Plan_1_aktivnost>();
                aktivnosti = plan.OsPlan1Aktivnost.Where(w => w.Id_podrucje == item.Id_plan).ToList();
                aktivnosti = aktivnosti.OrderBy(o => o.Red_broj_aktivnost).ToList();

                PdfPCell cell = new PdfPCell(new Phrase(plan.Ciljevi.Single(s => s.ID_cilj == item.Cilj).Naziv,tekst));
                cell.Rowspan = aktivnosti.Count + 1;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell.NoWrap = false;
                t.AddCell(cell);

                t.AddCell(VratiCeliju(item.Br_sati.ToString(), bold, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Mj_9.ToString(), bold, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Mj_10.ToString(), bold, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Mj_11.ToString(), bold, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Mj_12.ToString(), bold, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Mj_1.ToString(), bold, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Mj_2.ToString(), bold, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Mj_3.ToString(), bold, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Mj_4.ToString(), bold, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Mj_5.ToString(), bold, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Mj_6.ToString(), bold, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Mj_7.ToString(), bold, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Mj_8.ToString(), bold, false, BaseColor.WHITE));

                int x = 1;
                foreach(var ak in aktivnosti)
                {                    
                    t.AddCell(VratiCeliju(i.ToString() + "." + (x++).ToString(), tekst, true, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(plan.Aktivnosti.Single(s => s.Id_aktivnost == ak.Opis_aktivnost).Naziv, tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(ak.Potrebno_sati, tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(ak.Br_sati.ToString(), tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(ak.Mj_9.ToString(), tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(ak.Mj_10.ToString(), tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(ak.Mj_11.ToString(), tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(ak.Mj_12.ToString(), tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(ak.Mj_1.ToString(), tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(ak.Mj_2.ToString(), tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(ak.Mj_3.ToString(), tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(ak.Mj_4.ToString(), tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(ak.Mj_5.ToString(), tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(ak.Mj_6.ToString(), tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(ak.Mj_7.ToString(), tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(ak.Mj_8.ToString(), tekst, false, BaseColor.WHITE));
                }           
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
            c1.Padding = 5;
            c1.NoWrap = nowrap;
            return c1;
        }
    }
}
