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
    public class GodisnjiReport
    {
        public byte[] Podaci { get; private set; }

        public GodisnjiReport(GodisnjiModel model)
        {
			Document document = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
			MemoryStream memoryStream = new MemoryStream();
            PdfWriter.GetInstance(document, memoryStream).CloseStream = false;
			document.Open();
            BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
            Font header = new Font(font, 12, Font.NORMAL, BaseColor.DARK_GRAY);
            Font title = new Font(font, 14, Font.BOLDITALIC, BaseColor.BLACK);
            Font body = new Font(font, 10, Font.NORMAL, BaseColor.BLACK);
			
            Paragraph p = new Paragraph();
			document.Add(p);
			
            p = new Paragraph("Godišnji plan za školsku godinu " + model.GodisnjiPlan.Ak_godina+"./"+(model.GodisnjiPlan.Ak_godina+1)+".", title);
            p.Alignment = Element.ALIGN_CENTER;
            p.SpacingBefore = 10;
            p.SpacingAfter = 10;
			document.Add(p);
			
			PdfPTable table = new PdfPTable(12);
			table.SetWidths(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
			table.AddCell(Cell("Mjesec", body, BaseColor.LIGHT_GRAY));
			table.AddCell(Cell("Ukupno", body, BaseColor.LIGHT_GRAY));
			table.AddCell(Cell("Radnih", body, BaseColor.LIGHT_GRAY));
			table.AddCell(Cell("Subota", body, BaseColor.LIGHT_GRAY));
			table.AddCell(Cell("Nedjelja", body, BaseColor.LIGHT_GRAY));
			table.AddCell(Cell("Blagdana", body, BaseColor.LIGHT_GRAY));
			table.AddCell(Cell("Nastavnih", body, BaseColor.LIGHT_GRAY));
			table.AddCell(Cell("Praznika", body, BaseColor.LIGHT_GRAY));
			table.AddCell(Cell("Broj sati", body, BaseColor.LIGHT_GRAY));
			table.AddCell(Cell("Odmor dana", body, BaseColor.LIGHT_GRAY));
			table.AddCell(Cell("Odmor sati", body, BaseColor.LIGHT_GRAY));
			table.AddCell(Cell("Mj. fond sati", body, BaseColor.LIGHT_GRAY));
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

			foreach (Godisnji_detalji detalj in model.GodisnjiDetalji)
			{
				table.AddCell(Cell(detalj.Naziv_mjeseca.ToString(), body, BaseColor.WHITE));
				table.AddCell(Cell(detalj.Ukupno_dana.ToString(), body, BaseColor.WHITE));
				table.AddCell(Cell(detalj.Radnih_dana.ToString(), body, BaseColor.WHITE));
				table.AddCell(Cell(detalj.Subota_dana.ToString(), body, BaseColor.WHITE));
				table.AddCell(Cell(detalj.Nedjelja_dana.ToString(), body, BaseColor.WHITE));
				table.AddCell(Cell(detalj.Blagdana_dana.ToString(), body, BaseColor.WHITE));
				table.AddCell(Cell(detalj.Nastavnih_dana.ToString(), body, BaseColor.WHITE));
				table.AddCell(Cell(detalj.Praznika_dana.ToString(), body, BaseColor.WHITE));
				table.AddCell(Cell(detalj.Br_sati.ToString(), body, BaseColor.WHITE));
				table.AddCell(Cell(detalj.Odmor_dana.ToString(), body, BaseColor.WHITE));
				table.AddCell(Cell(detalj.Odmor_sati.ToString(), body, BaseColor.WHITE));
				table.AddCell(Cell(detalj.Mj_fond_sati.ToString(), body, BaseColor.WHITE));

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

			}

			table.AddCell(Cell("Ukupno" , body, BaseColor.WHITE));
			table.AddCell(Cell(uk_dana.ToString(), body, BaseColor.WHITE));
			table.AddCell(Cell(uk_rad_dana.ToString(), body, BaseColor.WHITE));
			table.AddCell(Cell(uk_sub_dana.ToString(), body, BaseColor.WHITE));
			table.AddCell(Cell(uk_ned_dana.ToString(), body, BaseColor.WHITE));
			table.AddCell(Cell(uk_blag_dana.ToString(), body, BaseColor.WHITE));
			table.AddCell(Cell(uk_nast_dana.ToString(), body, BaseColor.WHITE));
			table.AddCell(Cell(uk_praz_dana.ToString(), body, BaseColor.WHITE));
			table.AddCell(Cell(uk_br_sati.ToString(), body, BaseColor.WHITE));
			table.AddCell(Cell(uk_odm_dana.ToString(), body, BaseColor.WHITE));
			table.AddCell(Cell(uk_odm_sati.ToString(), body, BaseColor.WHITE));
			table.AddCell(Cell(uk_mj_fond_sati.ToString(), body, BaseColor.WHITE));

			table.AddCell(Cell("Sati" , body, BaseColor.WHITE));
			table.AddCell(Cell((uk_dana * 8).ToString(), body, BaseColor.WHITE));
			table.AddCell(Cell((uk_rad_dana * 8).ToString(), body, BaseColor.WHITE));
			table.AddCell(Cell((uk_sub_dana * 8).ToString(), body, BaseColor.WHITE));
			table.AddCell(Cell((uk_ned_dana * 8).ToString(), body, BaseColor.WHITE));
			table.AddCell(Cell((uk_blag_dana * 8).ToString(), body, BaseColor.WHITE));
			table.AddCell(Cell((uk_nast_dana * 8).ToString(), body, BaseColor.WHITE));
			table.AddCell(Cell((uk_praz_dana * 8).ToString(), body, BaseColor.WHITE));
			table.AddCell(Cell("", body, BaseColor.WHITE));
			table.AddCell(Cell("" , body, BaseColor.WHITE));
			table.AddCell(Cell("" , body, BaseColor.WHITE));
			table.AddCell(Cell("" , body, BaseColor.WHITE));

			document.Add(table);
			p = new Paragraph();
			p.Alignment = Element.ALIGN_CENTER;
			p.SpacingBefore = 30;
			p.SpacingAfter = 30;
			document.Add(p);

			Godisnji_plan plan = model.GodisnjiPlan;
			PdfPTable table2 = new PdfPTable(2);
			table2.WidthPercentage = 33;
			table2.SetWidths(new float[] { 4, 2 });

			table2.AddCell(Cell("Broj radnih dana", body, BaseColor.LIGHT_GRAY));
			table2.AddCell(Cell(plan.Br_radnih_dana.ToString(), body,  BaseColor.WHITE));
			table2.AddCell(Cell("Broj dana godisnjeg odmora", body, BaseColor.LIGHT_GRAY));
			table2.AddCell(Cell(plan.Br_dana_godina_odmor.ToString(), body, BaseColor.WHITE));
			table2.AddCell(Cell("Ukupno radnih dana", body, BaseColor.LIGHT_GRAY));
			table2.AddCell(Cell(plan.Ukupni_rad_dana.ToString(), body, BaseColor.WHITE));
			table2.AddCell(Cell("Godisnji fond sati", body, BaseColor.LIGHT_GRAY));
			table2.AddCell(Cell(plan.God_fond_sati.ToString(), body, BaseColor.WHITE));

			document.Add(table2);
			document.Close();
            Podaci = memoryStream.ToArray();
        }

        private PdfPCell Cell(string label, Font font, BaseColor color)
        {
            PdfPCell cell = new PdfPCell(new Phrase(label, font));
			cell.BackgroundColor = color;
			cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
			cell.Padding = 5;
			cell.NoWrap = false;
            return cell;
        }
    }
}