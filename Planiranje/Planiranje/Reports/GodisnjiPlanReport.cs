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
            PdfWriter.GetInstance(pdfDokument, memStream).CloseStream = false;
            pdfDokument.Open();
            BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
            Font header = new Font(font, 12, Font.NORMAL, BaseColor.DARK_GRAY);
            Font naslov = new Font(font, 14, Font.BOLDITALIC, BaseColor.BLACK);
            Font tekst = new Font(font, 10, Font.NORMAL, BaseColor.BLACK);
			
            Paragraph p = new Paragraph();
            pdfDokument.Add(p);
			
            p = new Paragraph("Godišnji plan za akademsku godinu " + model.GodisnjiPlan.Ak_godina, naslov);
            p.Alignment = Element.ALIGN_CENTER;
            p.SpacingBefore = 10;
            p.SpacingAfter = 10;
            pdfDokument.Add(p);
			
			PdfPTable tt = new PdfPTable(12);
			tt.SetWidths(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
			tt.AddCell(VratiCeliju("Mjesec", tekst, false, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Ukupno", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Radnih", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Subota", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Nedjelja", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Blagdana", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Nastavnih", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Praznika", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Broj sati", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Odmor dana", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Odmor sati", tekst, true, BaseColor.LIGHT_GRAY));
			tt.AddCell(VratiCeliju("Mj. fond sati", tekst, true, BaseColor.LIGHT_GRAY));
			int uk_dana = 0;
			int uk_rad_dana = 0;
			int uk_sub_dana = 0;
			int uk_ned_dana = 0;
			int uk_blag_dana = 0;
			int uk_nast_dana = 0;
			int uk_praz_dana = 0;
			int uk_br_sati = 0;
			int uk_odm_dana = 0;
			int uk_odm_sati = 0;
			int uk_mj_fond_sati = 0;

			pdfDokument.Add(tt);
			foreach (Godisnji_detalji detalj in model.GodisnjiDetalji)
			{
				tt = new PdfPTable(12);
				tt.SetWidths(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
				tt.AddCell(VratiCeliju(detalj.Naziv_mjeseca.ToString(), tekst, false, BaseColor.WHITE));
				tt.AddCell(VratiCeliju(detalj.Ukupno_dana.ToString(), tekst, true, BaseColor.WHITE));
				tt.AddCell(VratiCeliju(detalj.Radnih_dana.ToString(), tekst, true, BaseColor.WHITE));
				tt.AddCell(VratiCeliju(detalj.Subota_dana.ToString(), tekst, true, BaseColor.WHITE));
				tt.AddCell(VratiCeliju(detalj.Nedjelja_dana.ToString(), tekst, true, BaseColor.WHITE));
				tt.AddCell(VratiCeliju(detalj.Blagdana_dana.ToString(), tekst, true, BaseColor.WHITE));
				tt.AddCell(VratiCeliju(detalj.Nastavnih_dana.ToString(), tekst, true, BaseColor.WHITE));
				tt.AddCell(VratiCeliju(detalj.Praznika_dana.ToString(), tekst, true, BaseColor.WHITE));
				tt.AddCell(VratiCeliju(detalj.Br_sati.ToString(), tekst, true, BaseColor.WHITE));
				tt.AddCell(VratiCeliju(detalj.Odmor_dana.ToString(), tekst, true, BaseColor.WHITE));
				tt.AddCell(VratiCeliju(detalj.Odmor_sati.ToString(), tekst, true, BaseColor.WHITE));
				tt.AddCell(VratiCeliju(detalj.Mj_fond_sati.ToString(), tekst, true, BaseColor.WHITE));

				uk_dana += detalj.Ukupno_dana;
				uk_rad_dana += detalj.Radnih_dana;
				uk_sub_dana += detalj.Subota_dana;
				uk_ned_dana += detalj.Nedjelja_dana;
				uk_blag_dana += detalj.Blagdana_dana;
				uk_nast_dana += detalj.Nastavnih_dana;
				uk_praz_dana += detalj.Praznika_dana;
				uk_br_sati += detalj.Br_sati;
				uk_odm_dana += detalj.Odmor_dana;
				uk_odm_sati += detalj.Odmor_sati;
				uk_mj_fond_sati += detalj.Mj_fond_sati;

				pdfDokument.Add(tt);

			}

			tt.AddCell(VratiCeliju("Ukupno" , tekst, false, BaseColor.WHITE));
			tt.AddCell(VratiCeliju(uk_dana.ToString(), tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju(uk_rad_dana.ToString(), tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju(uk_sub_dana.ToString(), tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju(uk_ned_dana.ToString(), tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju(uk_blag_dana.ToString(), tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju(uk_nast_dana.ToString(), tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju(uk_praz_dana.ToString(), tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju(uk_br_sati.ToString(), tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju(uk_odm_dana.ToString(), tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju(uk_odm_sati.ToString(), tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju(uk_mj_fond_sati.ToString(), tekst, true, BaseColor.WHITE));

			pdfDokument.Add(tt);

			tt.AddCell(VratiCeliju("Sati" , tekst, false, BaseColor.WHITE));
			tt.AddCell(VratiCeliju((uk_dana * 8).ToString(), tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju((uk_rad_dana * 8).ToString(), tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju((uk_sub_dana * 8).ToString(), tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju((uk_ned_dana * 8).ToString(), tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju((uk_blag_dana * 8).ToString(), tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju((uk_nast_dana * 8).ToString(), tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju((uk_praz_dana * 8).ToString(), tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju("", tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju("" , tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju("" , tekst, true, BaseColor.WHITE));
			tt.AddCell(VratiCeliju("" , tekst, true, BaseColor.WHITE));

			pdfDokument.Add(tt);
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