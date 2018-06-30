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
    public class GodisnjiPlanReport
    {
        public byte[] Podaci { get; private set; }

        public GodisnjiPlanReport(ViewModel model)
        {
            Document pdfDokument = new Document(PageSize.A4.Rotate(),10,10,10,10);
            

            MemoryStream memStream = new MemoryStream();
            PdfWriter.GetInstance(pdfDokument, memStream).
                CloseStream = false;
            pdfDokument.Open();
            BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA,
                BaseFont.CP1250, false);
            Font header = new Font(font, 12, Font.NORMAL, BaseColor.DARK_GRAY);
            Font naslov = new Font(font, 14, Font.BOLDITALIC, BaseColor.BLACK);
            Font tekst = new Font(font, 10, Font.NORMAL, BaseColor.BLACK);
			
            Paragraph p = new Paragraph("IZVJEŠTAJ", header);
            pdfDokument.Add(p);
			
            p = new Paragraph("Godišnji plan za akademsku godinu " + model.GodisnjiPlan.Ak_godina, naslov);
            p.Alignment = Element.ALIGN_CENTER;
            p.SpacingBefore = 30;
            p.SpacingAfter = 30;
            pdfDokument.Add(p);
			
			
			//foreach (Godisnji_detalji detalji in model.GodisnjiDetalji)
			//{
			/**/
			//Godisnji_detalji g = model.GodisnjiDetalji[0];//detalji.SingleOrDefault(detalj => detalj.Id_god == plan.Id_god);

			PdfPTable tt = new PdfPTable(12);
			//tt.WidthPercentage = 100;
			tt.SetWidths(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
			tt.AddCell(VratiCeliju("Mjesec", tekst, false, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Naziv mjeseca", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Uk dana", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Radnih dana", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Subota dana", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Blagdana dana", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Nastavnih dana", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Praznika dana", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Broj sati", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Odmor dana", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Odmor sati", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Mj fond sati", tekst, true, BaseColor.LIGHT_GRAY));

			pdfDokument.Add(tt);
			foreach (Godisnji_detalji detalj in model.GodisnjiDetalji)
			{
                 tt = new PdfPTable(12);
                    //tt.WidthPercentage = 100;
				tt.SetWidths(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });


				tt.AddCell(VratiCeliju(detalj.Mjesec.ToString(), tekst, false, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(detalj.Naziv_mjeseca.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(detalj.Ukupno_dana.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(detalj.Radnih_dana.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(detalj.Subota_dana.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(detalj.Blagdana_dana.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(detalj.Nastavnih_dana.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(detalj.Praznika_dana.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(detalj.Br_sati.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(detalj.Odmor_dana.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(detalj.Odmor_sati.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(detalj.Mj_fond_sati.ToString(), tekst, true, BaseColor.WHITE));

                    pdfDokument.Add(tt);

			}
			p = new Paragraph();
			p.Alignment = Element.ALIGN_CENTER;
			p.SpacingBefore = 30;
			p.SpacingAfter = 30;
			pdfDokument.Add(p);

			Godisnji_plan plan = model.GodisnjiPlan;
			PdfPTable t = new PdfPTable(2);
			t.WidthPercentage = 33;
			t.SetWidths(new float[] { 4, 2 });
			
			t.AddCell(VratiCeliju("Broj radnih dana", tekst, true, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju(plan.Br_radnih_dana.ToString(), tekst, true, BaseColor.WHITE));
			t.AddCell(VratiCeliju("Broj dana godisnjeg odmora", tekst, true, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju(plan.Br_dana_godina_odmor.ToString(), tekst, true, BaseColor.WHITE));
			t.AddCell(VratiCeliju("Ukupno radnih dana", tekst, true, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju(plan.Ukupni_rad_dana.ToString(), tekst, true, BaseColor.WHITE));
			t.AddCell(VratiCeliju("Godisnji fond sati", tekst, true, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju(plan.God_fond_sati.ToString(), tekst, true, BaseColor.WHITE));

			pdfDokument.Add(t);

			// dodati tablicu na dokument
			//pdfDokument.Add(t);

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
            c1.NoWrap = false;
            return c1;
        }
    }
}