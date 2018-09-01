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
    public class PlanOs2DetailsReport
    {
        public byte[] Podaci { get; private set; }

        public PlanOs2DetailsReport(PlanOs2View plan, Pedagog pedagog)
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
            Font tekst = new Font(font, 10, Font.NORMAL, BaseColor.BLACK);
            Font bold = new Font(font, 9, Font.BOLD, BaseColor.BLACK);
            Font blueBold = new Font(font, 9, Font.BOLD, BaseColor.BLUE);

            Paragraph p = new Paragraph(pedagog.Ime+" "+pedagog.Prezime+", "+pedagog.Titula, header);
            pdfDokument.Add(p);
            p = new Paragraph("Naziv plana: " + plan.OsPlan2.Naziv, header);
            pdfDokument.Add(p);

            p = new Paragraph("Godišnji plan i program rada stručnog suradnika pedagoga za osnovnu školu u šk.god. "+plan.OsPlan2.Ak_godina, naslov);
            p.Alignment = Element.ALIGN_CENTER;
            p.SpacingBefore = 30;
            p.SpacingAfter = 30;
            pdfDokument.Add(p);

            PdfPTable t = new PdfPTable(8);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 1, 5, 3, 3, 3, 3, 2, 1 });

            t.AddCell(VratiCeliju2("R.br.", bold, true, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("POSLOVI I ZADACI", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("CILJEVI", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("ZADACI", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("SUBJEKTI", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("OBLICI I METODE RADA", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("VRIJEME REALIZACIJE", bold, false, BaseColor.CYAN));
            t.AddCell(VratiCeliju2("SATI", bold, false, BaseColor.CYAN));

            int a = 0;
            foreach (var podrucje in plan.OsPlan2Podrucja)
            {
                a++;
                List<OS_Plan_2_aktivnost> aktivnost = new List<OS_Plan_2_aktivnost>();
                aktivnost = plan.OsPlan2Aktivnosti.Where(w => w.Id_podrucje == podrucje.Id_plan).ToList();
                aktivnost = aktivnost.OrderBy(o => o.Red_br_aktivnost).ToList();

                t.AddCell(VratiCeliju(a.ToString(), blueBold, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(podrucje.Opis_podrucje, blueBold, false, BaseColor.WHITE));

                List<OS_Plan_2_akcija> listaAkcija = new List<OS_Plan_2_akcija>();
                
                foreach(var item in aktivnost)
                {
                    listaAkcija.AddRange(plan.OsPlan2Akcije.Where(w => w.Id_aktivnost == item.Id_plan));
                }
                int spoji = aktivnost.Count + listaAkcija.Count + 1;
                PdfPCell cell = new PdfPCell(new Phrase(plan.Ciljevi.Single(s => s.ID_cilj == podrucje.Cilj).Naziv, tekst));
                cell.BackgroundColor = BaseColor.WHITE;
                cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;                
                cell.Padding = 5;
                cell.NoWrap = false;
                cell.Rowspan = spoji;
                t.AddCell(cell);

                cell = new PdfPCell(new Phrase(plan.Zadaci.Single(s => s.ID_zadatak == podrucje.Zadaci).Naziv, tekst));
                cell.BackgroundColor = BaseColor.WHITE;
                cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell.Padding = 5;
                cell.NoWrap = false;
                cell.Rowspan = spoji;
                t.AddCell(cell);

                cell = new PdfPCell(new Phrase(plan.Subjekti.Single(s => s.ID_subjekt == podrucje.Subjekti).Naziv, tekst));
                cell.BackgroundColor = BaseColor.WHITE;
                cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell.Padding = 5;
                cell.NoWrap = false;
                cell.Rowspan = spoji;
                t.AddCell(cell);

                cell = new PdfPCell(new Phrase(plan.Oblici.Single(s => s.Id_oblici == podrucje.Oblici).Naziv, tekst));
                cell.BackgroundColor = BaseColor.WHITE;
                cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell.Padding = 5;
                cell.NoWrap = false;
                cell.Rowspan = spoji;
                t.AddCell(cell);

                cell = new PdfPCell(new Phrase(podrucje.Vrijeme, tekst));
                cell.BackgroundColor = BaseColor.WHITE;
                cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell.Padding = 5;
                cell.NoWrap = false;
                cell.Rowspan = spoji;
                t.AddCell(cell);

                t.AddCell(VratiCeliju(podrucje.Sati.ToString(), blueBold, false, BaseColor.WHITE));

                int b = 0;
                foreach(var akt in aktivnost)
                {
                    b++;
                    t.AddCell(VratiCeliju(a+"."+b, bold, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(akt.Opis_aktivnost, bold, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(akt.Sati.ToString(), bold, false, BaseColor.WHITE));

                    List<OS_Plan_2_akcija> akcije = new List<OS_Plan_2_akcija>();
                    akcije = plan.OsPlan2Akcije.Where(w => w.Id_aktivnost == akt.Id_plan).ToList();
                    akcije = akcije.OrderBy(o => o.Red_br_akcija).ToList();

                    int c = 0;
                    foreach(var ac in akcije)
                    {
                        c++;
                        t.AddCell(VratiCeliju(a+"."+b+"."+c, tekst, false, BaseColor.WHITE));
                        t.AddCell(VratiCeliju(ac.Opis_akcija, tekst, false, BaseColor.WHITE));
                        t.AddCell(VratiCeliju(ac.Sati.ToString(), tekst, false, BaseColor.WHITE));
                    }
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