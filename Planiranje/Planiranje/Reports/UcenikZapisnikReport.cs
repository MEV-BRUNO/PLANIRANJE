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
    public class UcenikZapisnikReport
    {
        public byte[] Podaci { get; private set; }
        public UcenikZapisnikReport(Skola skola, Ucenik ucenik, RazredniOdjel odjel, Ucenik_zapisnik model,
            List<Ucenik_zapisnik_biljeska> biljeske, Pedagog pedagog, Nastavnik nastavnik, List<Obitelj> roditelji)
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
            Font bold = new Font(font, 10, Font.BOLD, BaseColor.BLACK);
            Font blueBold = new Font(font, 9, Font.BOLD, BaseColor.BLUE);

            List<Obitelj> otac = roditelji.Where(w => w.Svojstvo == 1).ToList();
            List<Obitelj> majka = roditelji.Where(w => w.Svojstvo == 2).ToList();
            if (otac.Count == 0)
            {
                roditelji.Add(new Obitelj() { Svojstvo = 1 });
            }
            if (majka.Count == 0)
            {
                roditelji.Add(new Obitelj() { Svojstvo = 2 });
            }

            Paragraph p = new Paragraph("Zapisnik praćenja napredovanja učenika", naslov);
            p.Alignment = Element.ALIGN_CENTER;
            p.SpacingBefore = 20;
            p.SpacingAfter = 20;
            pdfDokument.Add(p);

            p = new Paragraph("ŠKOLA: " + skola.Naziv, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("RAZREDNI ODJEL: " + odjel.Naziv, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("ŠKOLSKA GODINA: " + odjel.Sk_godina + "./" + (odjel.Sk_godina + 1).ToString() + ".", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("NASTAVNIK: " + nastavnik.ImePrezime, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("UČENIK: " + ucenik.ImePrezime, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("NADNEVAK ROĐENJA: " + ucenik.Datum.ToShortDateString(), tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("ADRESA: " + ucenik.Adresa, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 5;
            pdfDokument.Add(p);
            p = new Paragraph("RAZLOG ZBOG KOJEG JE UČENIK UPUĆEN STRUČNOM SURADNIKU: " + model.Razlog, tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 20;
            pdfDokument.Add(p);

            p = new Paragraph("OBITELJSKA ANAMNEZA", bold);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingBefore = 10;
            p.SpacingAfter = 10;
            pdfDokument.Add(p);

            p = new Paragraph("1.RODITELJI/SKRBNICI", tekst);
            p.Alignment = Element.ALIGN_LEFT;            
            p.SpacingAfter = 10;
            pdfDokument.Add(p);

            List<Obitelj> skrbnici = roditelji.Where(w => w.Svojstvo == 3).Take(2).ToList();

            int brojStupaca = 3 + skrbnici.Count;
            PdfPTable t = new PdfPTable(brojStupaca);
            t.WidthPercentage = 100;            
            float[] polje = new float[brojStupaca];
            for(int i = 0; i < brojStupaca; i++)
            {
                polje[i] = 1;
            }
            t.SetWidths(polje);
            t.AddCell(VratiCeliju3(string.Empty, tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju3("MAJKA", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju3("OTAC", tekst, false, BaseColor.WHITE));
            for(int i = 1; i <= skrbnici.Count; i++)
            {
                t.AddCell(VratiCeliju3("SKRBNIK "+i, tekst, false, BaseColor.WHITE));
            }
            t.AddCell(VratiCeliju3("Ime i prezime", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju3(roditelji.FirstOrDefault(s=>s.Svojstvo==2).ImePrezime, tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju3(roditelji.FirstOrDefault(s=>s.Svojstvo==1).ImePrezime, tekst, false, BaseColor.WHITE));
            for(int i = 0; i < skrbnici.Count; i++)
            {
                t.AddCell(VratiCeliju3(skrbnici.ElementAt(i).ImePrezime, tekst, false, BaseColor.WHITE));
            }
            t.AddCell(VratiCeliju3("Adresa", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju3(roditelji.FirstOrDefault(s => s.Svojstvo == 1).Adresa, tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju3(roditelji.FirstOrDefault(s => s.Svojstvo == 2).Adresa, tekst, false, BaseColor.WHITE));
            for (int i = 0; i < skrbnici.Count; i++)
            {
                t.AddCell(VratiCeliju3(skrbnici.ElementAt(i).Adresa, tekst, false, BaseColor.WHITE));
            }
            t.AddCell(VratiCeliju3("Kontakt", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju3(roditelji.FirstOrDefault(s => s.Svojstvo == 1).Kontakt, tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju3(roditelji.FirstOrDefault(s => s.Svojstvo == 2).Kontakt, tekst, false, BaseColor.WHITE));
            for (int i = 0; i < skrbnici.Count; i++)
            {
                t.AddCell(VratiCeliju3(skrbnici.ElementAt(i).Kontakt, tekst, false, BaseColor.WHITE));
            }
            t.AddCell(VratiCeliju3("Zanimanje", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju3(roditelji.FirstOrDefault(s => s.Svojstvo == 1).Zanimanje, tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju3(roditelji.FirstOrDefault(s => s.Svojstvo == 2).Zanimanje, tekst, false, BaseColor.WHITE));
            for (int i = 0; i < skrbnici.Count; i++)
            {
                t.AddCell(VratiCeliju3(skrbnici.ElementAt(i).Zanimanje, tekst, false, BaseColor.WHITE));
            }
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("2.BRAĆA I SESTRE", tekst);
            p.Alignment = Element.ALIGN_LEFT;            
            p.SpacingAfter = 10;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;
            t.SpacingBefore = 10;            
            t.SetWidths(new float[] { 1 });
            string braca = string.Empty;
            int x = 0;
            foreach(var item in roditelji.Where(w=>w.Svojstvo==4 || w.Svojstvo == 5))
            {
                x++;
                braca = braca + item.ImePrezime;
                if(x!=roditelji.Where(w=>w.Svojstvo==4 || w.Svojstvo == 5).ToList().Count)
                {
                    braca += ", ";
                }
            }
            t.AddCell(VratiCeliju6(braca, tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 10;
            pdfDokument.Add(t);

            p = new Paragraph("3.ODGOJNI UTJECAJ OBITELJI", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 10;
            pdfDokument.Add(p);

            brojStupaca = 2 + skrbnici.Count;
            polje = new float[brojStupaca];
            t = new PdfPTable(brojStupaca);
            t.WidthPercentage = 100;
            t.SpacingBefore = 10;
            for (int i = 0; i < brojStupaca; i++)
            {
                polje[i] = 1;
            }
            t.SetWidths(polje);

            List<string> select = new List<string>() { "-", "Strogi", "Ravnodušni", "Blagi", "Autoritativni"};

            t.AddCell(VratiCeliju3("MAJKA", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju3("OTAC", tekst, false, BaseColor.WHITE));
            for(int i = 0; i < skrbnici.Count; i++)
            {
                t.AddCell(VratiCeliju3("SKRBNIK "+(i+1), tekst, false, BaseColor.WHITE));
            }
            t.AddCell(VratiCeliju4(select.ElementAt(model.Odgojni_utjecaj_majka), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4(select.ElementAt(model.Odgojni_utjecaj_otac), tekst, false, BaseColor.WHITE));
            for(int i = 0; i < skrbnici.Count; i++)
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            t.SpacingAfter = 10;
            pdfDokument.Add(t);

            p = new Paragraph("4.PROCJENA SOCIOEKONOMSKOG STATUSA OBITELJI", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 10;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;            
            t.SetWidths(new float[] { 1 });
            if (!string.IsNullOrWhiteSpace(model.Procjena_statusa_obitelji))
            {
                t.AddCell(VratiCeliju2(model.Procjena_statusa_obitelji, tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju5("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju5("\n", tekst, false, BaseColor.WHITE));
            }
            
            t.SpacingAfter = 10;
            pdfDokument.Add(t);

            p = new Paragraph("5.ODNOS RODITELJA PREMA UČENJU I OBRAZOVANJU", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 10;
            pdfDokument.Add(p);

            brojStupaca = 2 + skrbnici.Count;
            polje = new float[brojStupaca];
            t = new PdfPTable(brojStupaca);
            t.WidthPercentage = 100;
            t.SpacingBefore = 10;
            for (int i = 0; i < brojStupaca; i++)
            {
                polje[i] = 1;
            }
            t.SetWidths(polje);

            select = new List<string>() { "-", "Ne pokazuje zanimanje", "Povremeno pokazuje zanimanje", "Redovito surađuju i potiču na učenje" };

            t.AddCell(VratiCeliju3("MAJKA", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju3("OTAC", tekst, false, BaseColor.WHITE));
            for (int i = 0; i < skrbnici.Count; i++)
            {
                t.AddCell(VratiCeliju3("SKRBNIK " + (i+1), tekst, false, BaseColor.WHITE));
            }
            t.AddCell(VratiCeliju4(select.ElementAt(model.Odnos_prema_ucenju_majka), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4(select.ElementAt(model.Odnos_prema_ucenju_otac), tekst, false, BaseColor.WHITE));
            for (int i = 0; i < skrbnici.Count; i++)
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            t.SpacingAfter = 10;
            pdfDokument.Add(t);

            p = new Paragraph("6.SURADNJA RODITELJA SA ŠKOLOM", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 10;
            pdfDokument.Add(p);

            brojStupaca = 2 + skrbnici.Count;
            polje = new float[brojStupaca];
            t = new PdfPTable(brojStupaca);
            t.WidthPercentage = 100;
            t.SpacingBefore = 10;
            for (int i = 0; i < brojStupaca; i++)
            {
                polje[i] = 1;
            }
            t.SetWidths(polje);

            select = new List<string>() { "-", "Redovita", "Povremena", "Ne surađuju" };

            t.AddCell(VratiCeliju3("MAJKA", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju3("OTAC", tekst, false, BaseColor.WHITE));
            for (int i = 0; i < skrbnici.Count; i++)
            {
                t.AddCell(VratiCeliju3("SKRBNIK " + (i+1), tekst, false, BaseColor.WHITE));
            }
            t.AddCell(VratiCeliju4(select.ElementAt(model.Suradnja_roditelja_majka), tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4(select.ElementAt(model.Suradnja_roditelja_otac), tekst, false, BaseColor.WHITE));
            for (int i = 0; i < skrbnici.Count; i++)
            {
                t.AddCell(VratiCeliju3("\n", tekst, false, BaseColor.WHITE));
            }
            t.SpacingAfter = 10;
            pdfDokument.Add(t);

            p = new Paragraph("7.ODNOS S PRIJATELJIMA", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 10;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;                  
            t.SetWidths(new float[] { 1 });
            string str = model.Odnos_s_prijateljima;
            if (!string.IsNullOrWhiteSpace(str))
            {
                t.AddCell(VratiCeliju2(str,tekst,false,BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju5("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju5("\n", tekst, false, BaseColor.WHITE));
            }
            t.SpacingAfter = 10;
            pdfDokument.Add(t);

            p = new Paragraph("8.KAKO NAJČEŠĆE PROVODI SLOBODNO VRIJEME?", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 10;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;            
            t.SetWidths(new float[] { 1 });
            str = model.Kako_provodi_slobodno_vrijeme;
            if (!string.IsNullOrWhiteSpace(str))
            {
                t.AddCell(VratiCeliju2(str, tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju5("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju5("\n", tekst, false, BaseColor.WHITE));
            }
            t.SpacingAfter = 10;
            pdfDokument.Add(t);

            p = new Paragraph("9.PROCJENA MOGUĆIH LOŠIH UTJECAJA OBITELJI, PRIJATELJA, OKOLINE, DRUŠTVA NA UČENJE", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 10;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;            
            t.SetWidths(new float[] { 1 });
            str = model.Procjena_mogucih_losih_utjecaja;
            if (!string.IsNullOrWhiteSpace(str))
            {
                t.AddCell(VratiCeliju2(str, tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju5("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju5("\n", tekst, false, BaseColor.WHITE));
            }
            t.SpacingAfter = 10;
            pdfDokument.Add(t);

            p = new Paragraph("10.ZDRAVSTVENE POTEŠKOĆE UČENIKA I OPIS EKSTREMNOG OBLIKA PONAŠANJA" +
                " (EKSTREMI:INTROVERTIRANOST, KONTROLA EMOCIJA, PRETJERANJA OSJETLJIVOST...)", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 10;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;            
            t.SetWidths(new float[] { 1 });
            str = model.Zdravstvene_poteskoce_ucenika;
            if (!string.IsNullOrWhiteSpace(str))
            {
                t.AddCell(VratiCeliju2(str, tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju5("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju5("\n", tekst, false, BaseColor.WHITE));
            }
            t.SpacingAfter = 10;
            pdfDokument.Add(t);

            p = new Paragraph("11.PODACI O NAGLIM I IZNENADNIM PROMJENAMA U PONAŠANJU UČENIKA (POVEZANOST S" +
                " ODREĐENIM DOGAĐAJIMA)", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 10;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;            
            t.SetWidths(new float[] { 1 });
            str = model.Podaci_o_naglim_promjenama;
            if (!string.IsNullOrWhiteSpace(str))
            {
                t.AddCell(VratiCeliju2(str, tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju5("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju5("\n", tekst, false, BaseColor.WHITE));
            }
            t.SpacingAfter = 10;
            pdfDokument.Add(t);

            p = new Paragraph("12.IZREČENE PEDAGOŠKE MJERE (VRSTA, UZROK, NADNEVAK)", tekst);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 10;
            pdfDokument.Add(p);

            t = new PdfPTable(1);
            t.WidthPercentage = 100;           
            t.SetWidths(new float[] { 1 });
            str = model.Izrecene_pedagoske_mjere;
            if (!string.IsNullOrWhiteSpace(str))
            {
                t.AddCell(VratiCeliju2(str, tekst, false, BaseColor.WHITE));
            }
            else
            {
                t.AddCell(VratiCeliju5("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju5("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju5("\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju5("\n", tekst, false, BaseColor.WHITE));
            }
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            p = new Paragraph("TIJEK PRAĆENJA", bold);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 10;
            pdfDokument.Add(p);

            t = new PdfPTable(3);
            t.WidthPercentage = 100;
            t.SpacingBefore = 10;
            t.SetWidths(new float[] { 1.25F, 5, 3.5F });
            t.AddCell(VratiCeliju4("NADNEVAK", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4("SADRŽAJ", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju4("DOGOVOR/ZAKLJUČAK", tekst, false, BaseColor.WHITE));
            foreach (var item in biljeske)
            {
                t.AddCell(VratiCeliju3(item.Datum.ToShortDateString(), tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3(item.Sadrzaj, tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3(item.Dogovor, tekst, false, BaseColor.WHITE));
            }
            if (biljeske.Count==0)
            {
                t.AddCell(VratiCeliju3("\n\n\n\n\n\n\n\n\n\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n\n\n\n\n\n\n\n\n\n", tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju3("\n\n\n\n\n\n\n\n\n\n", tekst, false, BaseColor.WHITE));
            }            
            t.SpacingAfter = 15;
            pdfDokument.Add(t);

            t = new PdfPTable(3);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1, 1, 1 });
            t.AddCell(VratiCeliju("Obrazac ispunila/ispunio:", tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(string.Empty, tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju("Nadnevak:", tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 10;
            pdfDokument.Add(t);

            t = new PdfPTable(3);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 1, 1, 1 });
            t.AddCell(VratiCeliju3(string.Empty, tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju(string.Empty, tekst, false, BaseColor.WHITE));
            t.AddCell(VratiCeliju3(string.Empty, tekst, false, BaseColor.WHITE));
            t.SpacingAfter = 14;
            pdfDokument.Add(t);

            pdfDokument.Close();
            Podaci = memStream.ToArray();
        }
        /// <summary>
        /// metoda vraća ćeliju bez obruba s centriranim tekstom
        /// </summary>
        /// <param name="labela"></param>
        /// <param name="font"></param>
        /// <param name="nowrap"></param>
        /// <param name="boja"></param>
        /// <returns></returns>
        private PdfPCell VratiCeliju(string labela, Font font,
           bool nowrap, BaseColor boja)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(labela, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            c1.Padding = 5;
            c1.NoWrap = nowrap;
            c1.Border = PdfPCell.NO_BORDER;
            return c1;
        }
        /// <summary>
        /// metoda vraća ćeliju bez obruba (tekst nije centriran)
        /// </summary>
        /// <param name="labela"></param>
        /// <param name="font"></param>
        /// <param name="nowrap"></param>
        /// <param name="boja"></param>
        /// <returns></returns>
        private PdfPCell VratiCeliju2(string labela, Font font,
            bool nowrap, BaseColor boja)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(labela, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            c1.Padding = 5;
            c1.NoWrap = nowrap;
            c1.Border = PdfPCell.NO_BORDER;
            return c1;
        }
        /// <summary>
        /// metoda vraća ćeliju s obrubom (tekst nije centriran)
        /// </summary>
        /// <param name="labela"></param>
        /// <param name="font"></param>
        /// <param name="nowrap"></param>
        /// <param name="boja"></param>
        /// <returns></returns>
        private PdfPCell VratiCeliju3(string labela, Font font,
            bool nowrap, BaseColor boja)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(labela, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            c1.Padding = 5;
            c1.NoWrap = nowrap;
            //c1.Border = PdfPCell.BOTTOM_BORDER;
            return c1;
        }
        /// <summary>
        /// metoda vraća ćeliju s obrubom i centriranim tekstom
        /// </summary>
        /// <param name="labela"></param>
        /// <param name="font"></param>
        /// <param name="nowrap"></param>
        /// <param name="boja"></param>
        /// <returns></returns>
        private PdfPCell VratiCeliju4(string labela, Font font,
           bool nowrap, BaseColor boja)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(labela, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            c1.Padding = 5;
            c1.NoWrap = nowrap;
            //c1.Border = PdfPCell.NO_BORDER;

            return c1;
        }
        /// <summary>
        /// vraća ćeliju s donjim obrubom i centriranim tekstom
        /// </summary>
        /// <param name="labela"></param>
        /// <param name="font"></param>
        /// <param name="nowrap"></param>
        /// <param name="boja"></param>
        /// <returns></returns>
        private PdfPCell VratiCeliju5(string labela, Font font,
           bool nowrap, BaseColor boja)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(labela, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            c1.Padding = 5;
            c1.NoWrap = nowrap;
            c1.Border = PdfPCell.BOTTOM_BORDER;
            return c1;
        }
        /// <summary>
        /// vraća ćeliju koja ima donji obrub ukoliko je string empty ili null
        /// </summary>
        /// <param name="labela"></param>
        /// <param name="font"></param>
        /// <param name="nowrap"></param>
        /// <param name="boja"></param>
        /// <returns></returns>
        private PdfPCell VratiCeliju6(string labela, Font font,
           bool nowrap, BaseColor boja)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(labela, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            c1.Padding = 5;
            c1.NoWrap = nowrap;
            if (string.IsNullOrWhiteSpace(labela))
            {
                c1.Border = PdfPCell.BOTTOM_BORDER;
            }
            else
            {
                c1.Border = PdfPCell.NO_BORDER;
            }

            return c1;
        }
    }
}