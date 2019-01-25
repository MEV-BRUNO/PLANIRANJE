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
    public class PracenjeUcenikaReport
    {
        public byte[] Podaci { get; private set; }
        public PracenjeUcenikaReport(PracenjeUcenikaModel model, Skola skola, Pedagog pedagog)
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

            Paragraph p = new Paragraph("Lista praćenja učenika - pedagoška obrada i anamneza", naslov);
            p.Alignment = Element.ALIGN_CENTER;
            p.SpacingBefore = 20;
            p.SpacingAfter = 20;
            pdfDokument.Add(p);

            p = new Paragraph("ŠKOLA: " + skola.Naziv, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("IME I PREZIME UČENIKA: " + model.Ucenik.ImePrezime, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("RAZREDNI ODJEL: " + model.Razred.Naziv, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("RAZREDNIK: " + model.Razrednik.ImePrezime, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            pdfDokument.Add(p);
            p = new Paragraph("ŠKOLSKA GODINA: " + model.Razred.Sk_godina + "./" + (model.Razred.Sk_godina + 1).ToString() + ".", tekst);
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
            pdfDokument.Add(p);
            p = new Paragraph("NADNEVAK POČETKA PRAĆENJA: " + model.PracenjeUcenika.Pocetak_pracenja.ToShortDateString(), tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 20;
            pdfDokument.Add(p);

            p = new Paragraph("PODACI O OBITELJI", bold);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            PdfPTable t = new PdfPTable(4);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 1.5F, 5, 2.5F, 2.5F });

            //t.AddCell(VratiCeliju2("R.br.", bold, true, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("SVOJSTVO", bold, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("IME I PREZIME", bold, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("ZANIMANJE", bold, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("KONTAKT", bold, false, BaseColor.WHITE));

            model.ListaObitelji = model.ListaObitelji.OrderBy(o => o.Svojstvo).ToList();
            List<string> svojstva = new List<string>() { "", "Otac", "Majka", "Skrbnik", "Brat", "Sestra" };
            foreach (var item in model.ListaObitelji)
            {
                t.AddCell(VratiCeliju(svojstva.ElementAt(item.Svojstvo), tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.ImePrezime, tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Zanimanje, tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Kontakt, tekst, false, BaseColor.WHITE));
            }

            pdfDokument.Add(t);

            p = new Paragraph("RAZLOG POKRETANJA PRAĆENJA UČENIKA", bold);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingBefore = 14;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            t.AddCell(VratiCeliju(model.PracenjeUcenika.Razlog, tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("PROCJENA UČENIKA", bold);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingBefore = 14;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            t.AddCell(VratiCeliju("INICIJALNA PROCJENA - razgovor s učenikom", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.PracenjeUcenika.Inic_Procjena_ucenik, tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("INICIJALNA PROCJENA - razgovor s roditeljem/skrbnikom", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.PracenjeUcenika.Inic_Procjena_roditelj, tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("INICIJALNA PROCJENA - razgovor s razrednikom", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.PracenjeUcenika.Inic_Procjena_razrednik, tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("SOCIOEKONOMSKI UVJETI I ODGOJNO-OBRAZOVNI UTJECAJ OKRUŽJA", bold);
            p.Alignment = Element.ALIGN_LEFT;            
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            t.AddCell(VratiCeliju(model.PracenjeUcenika.Soc_uvjeti, tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("UČENJE I SAZRIJEVANJE", bold);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            t.AddCell(VratiCeliju(model.PracenjeUcenika.Ucenje, tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("SOCIJALNE VJEŠTINE UČENIKA I ODNOS PREMA OBVEZAMA", bold);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            t.AddCell(VratiCeliju(model.PracenjeUcenika.Soc_vjestine, tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 14;
            pdfDokument.Add(t);

            p = new Paragraph("POSTIGNUĆA", bold);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 15;
            pdfDokument.Add(p);

            t = new PdfPTable(3);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 2, 1, 7 });
            t.AddCell(VratiCeliju2("ŠKOLSKA GODINA", bold, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("RAZRED", bold, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("NAPOMENA", bold, false, BaseColor.WHITE));            

            
            foreach (var item in model.Postignuca)
            {
                //t.AddCell(VratiCeliju((br++).ToString() + ".", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Godina+"./"+(item.Godina+1).ToString()+".", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(model.Razred.Naziv, tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Napomena, tekst, false, BaseColor.WHITE));
            }           

            pdfDokument.Add(t);

            p = new Paragraph("NEPOSREDNI RAD PEDAGOGA", bold);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingBefore = 20;
            p.SpacingAfter = 15;
            pdfDokument.Add(p);

            t = new PdfPTable(2);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1, 5});
            t.AddCell(VratiCeliju2("NADNEVAK", bold, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju2("OBLICI I NAČINI RADA S UČENIKOM, UČITELJIMA, RODITELJIMA/STARATELJIMA" +
                " I DRUGIMA", bold, false, BaseColor.WHITE));

            foreach(var item in model.NeposredniRadovi)
            {
                t.AddCell(VratiCeliju(item.Datum.ToShortDateString(), tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(item.Napomena, tekst, false, BaseColor.WHITE));
            }
            t.SpacingAfter = 14;
            pdfDokument.Add(t);

            p = new Paragraph("ZAKLJUČAK", bold);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 14;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1 });
            t.AddCell(VratiCeliju(model.PracenjeUcenika.Zakljucak, tekst, false, BaseColor.WHITE));            
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
            c1.Padding = 2;
            c1.NoWrap = nowrap;
            return c1;
        }
    }
}