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
    public class PopisUcenikaReport
    {
        public byte[] Podaci { get; private set; }

        public PopisUcenikaReport(List<Popis_ucenika> ListaPopisaUcenika, List<Ucenik> ListaUcenika, Skola skola, Nastavnik razrednik, RazredniOdjel odjel,
            List<Obitelj> obitelji, List<Ucenik_razred> ListaUR)
        {
            Document pdfDokument = new Document(
                PageSize.A4, 25, 25, 20, 25);

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

            Paragraph p = new Paragraph("Popis učenika u razrednom odjelu ", naslov);
            p.Alignment = Element.ALIGN_CENTER;
            p.SpacingBefore = 30;
            p.SpacingAfter = 30;
            pdfDokument.Add(p);

            p = new Paragraph("ŠKOLA: "+skola.Naziv, tekst);
            p.Alignment = Element.ALIGN_CENTER;
            pdfDokument.Add(p);
            p = new Paragraph("RAZREDNI ODJEL: " +odjel.Naziv, tekst);
            p.Alignment = Element.ALIGN_CENTER;
            pdfDokument.Add(p);
            p = new Paragraph("ŠKOLSKA GODINA: " + odjel.Sk_godina+"./"+(odjel.Sk_godina+1).ToString()+".", tekst);
            p.Alignment = Element.ALIGN_CENTER;
            pdfDokument.Add(p);
            p = new Paragraph("RAZREDNIK: " + razrednik.ImePrezime, tekst);
            p.Alignment = Element.ALIGN_CENTER;
            p.SpacingAfter = 50;
            pdfDokument.Add(p);


            PdfPTable t = new PdfPTable(7);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 1, 4, 4, 2, 2, 2, 2 });

            t.AddCell(VratiCeliju2("R.br.", bold, true, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("IME I PREZIME\nUČENIKA", bold, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("RODITELJI / SKRBNICI\nIME I PREZIME", bold, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("ADRESA/\nBROJ TELEFONA", bold, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("PONAVLJA RAZRED\n(DA/NE)", bold, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("PUTNIK\n(DA/NE)", bold, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("POSEBNA\nZADUŽENJA\nUČENIKA", bold, false, BaseColor.WHITE));

            int br = 1;
            foreach(var item in ListaUcenika)
            {
                t.AddCell(VratiCeliju((br++).ToString()+".", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.ImePrezime, tekst, false, BaseColor.WHITE));
                List<Obitelj> roditelji = new List<Obitelj>();
                roditelji = obitelji.Where(w => w.Id_ucenik == item.Id_ucenik).ToList();
                string imena = "";
                int a = 0;
                foreach(var o in roditelji)
                {
                    a++;
                    if (roditelji.Count == a)
                    {
                        imena += o.ImePrezime;
                    }
                    else
                    {
                        imena += o.ImePrezime + ", ";
                    }
                    
                }
                t.AddCell(VratiCeliju(imena, tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Adresa, tekst, false, BaseColor.WHITE));
                Popis_ucenika pu = new Popis_ucenika();

                pu = ListaPopisaUcenika.SingleOrDefault(s => s.Id_ucenik_razred == ListaUR.Single
                (w => w.Id_ucenik == item.Id_ucenik && w.Id_razred == odjel.Id).Id);
                if (pu != null)
                {
                    t.AddCell(VratiCeliju(pu.Ponavlja_razred == 1?"Da":"Ne", tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(pu.Putnik == 1 ? "Da" : "Ne", tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju(pu.Zaduzenje, tekst, false, BaseColor.WHITE));
                }
                else
                {
                    t.AddCell(VratiCeliju("", tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju("", tekst, false, BaseColor.WHITE));
                    t.AddCell(VratiCeliju("", tekst, false, BaseColor.WHITE));
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
            c1.Padding = 2;
            c1.NoWrap = nowrap;
            return c1;
        }
    }
}