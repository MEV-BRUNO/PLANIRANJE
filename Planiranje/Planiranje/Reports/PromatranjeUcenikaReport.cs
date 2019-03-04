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
    public class PromatranjeUcenikaReport
    {
        public byte[] Podaci { get; private set; }
        public PromatranjeUcenikaReport(PromatranjeUcenikaModel model, Nastavnik razrednik, Pedagog pedagog, Skola skola)
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

            Paragraph p = new Paragraph("Protokol promatranja učenika - procjenjivanje socijalnog karaktera učenika", naslov);
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
            p = new Paragraph("RAZREDNIK: " +razrednik.ImePrezime, tekst);
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
            p = new Paragraph("NADNEVAK PROMATRANJA: " + model.PromatranjeUcenika.Nadnevak.ToShortDateString()+
                " VRIJEME: "+model.PromatranjeUcenika.Vrijeme.ToShortTimeString(), tekst);
            p.Alignment = Element.ALIGN_LEFT;            
            pdfDokument.Add(p);

            p = new Paragraph("SOCIOEKONOMSKI STATUS UČENIKA: " + model.PromatranjeUcenika.SocStatusUcenika, tekst);
            p.Alignment = Element.ALIGN_LEFT;            
            pdfDokument.Add(p);

            p = new Paragraph("CILJ PROMATRANJA: " + model.PromatranjeUcenika.Cilj, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 20;
            pdfDokument.Add(p);            

            PdfPTable t = new PdfPTable(2);
            t.WidthPercentage = 100;
            t.SetWidths(new float[] { 0.8F, 1 });
            
            t.AddCell(VratiCeliju("Spremnost za kontaktiranje: kako se učenik odnosi prema drugim učenicima," +
                " je li rezerviran, povučen, osamljen, spreman za kontakt", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.PromatranjeUcenika.Spremnost, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju("Prilagodljivost: popustljiv, vođa, svojeglav", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.PromatranjeUcenika.Prilagodljivost, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju("Odnos prema drugima: bezobziran, pun ljubavi, grub, proračunat, mijenja" +
                " prijatelje...",tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.PromatranjeUcenika.Odnos, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju("Doprinos životu grupe: aktivan, kritizira, napada, ogovara," +
                " pouzdan je, spreman pomoći", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.PromatranjeUcenika.Doprinos, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju("Opis promatrane situacije", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.PromatranjeUcenika.Opis, tekst, false, BaseColor.WHITE));

            t.AddCell(VratiCeliju("Zaključak\n(podaci o učenju, vanjski i unutarnji utjecaji praćenja)", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(model.PromatranjeUcenika.Zakljucak, tekst, false, BaseColor.WHITE));

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