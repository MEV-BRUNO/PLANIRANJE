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
	public class PlanSsPodrucjaReport
	{
		public byte[] Podaci { get; private set; }

		public PlanSsPodrucjaReport(List<SS_Plan_podrucje> ss_plan_podrucja)
		{
			Document pdfDokument = new Document(
				PageSize.A4.Rotate(), 20, 20, 20, 20);

			MemoryStream memStream = new MemoryStream();
			PdfWriter.GetInstance(pdfDokument, memStream).
				CloseStream = false;
			pdfDokument.Open();
			BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA,
				BaseFont.CP1250, false);
			Font header = new Font(font, 11, Font.NORMAL, BaseColor.DARK_GRAY);
			Font naslov = new Font(font, 13, Font.BOLDITALIC, BaseColor.BLACK);
			Font tekst = new Font(font, 9, Font.NORMAL, BaseColor.BLACK);

			Paragraph p = new Paragraph("IZVJEŠTAJ", header);
			pdfDokument.Add(p);

			p = new Paragraph("Detalji plana za srednju školu", naslov);
			p.Alignment = Element.ALIGN_CENTER;
			p.SpacingBefore = 30;
			p.SpacingAfter = 30;
			pdfDokument.Add(p);

			PdfPTable t = new PdfPTable(11);
			t.WidthPercentage = 100;
			t.SetWidths(new float[] { 1, 2, 2, 3, 2, 2, 2, 2, 2, 3, 1 });

			t.AddCell(VratiCeliju("R.br.", tekst, false, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju("Područja rada", tekst, false, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju("Svrha/cilj", tekst, false, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju("Zadaće", tekst, false, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju("Sadržaj", tekst, false, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju("Oblici i metode rada", tekst, false, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju("Suradnici/subjekti", tekst, false, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju("Mjesto ostvarenja", tekst, false, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju("Vrijeme/broj sati", tekst, false, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju("Ishodi", tekst, false, BaseColor.LIGHT_GRAY));
            t.AddCell(VratiCeliju("Sati", tekst, false, BaseColor.LIGHT_GRAY));

            int i = 1;
			foreach (SS_Plan_podrucje plan in ss_plan_podrucja)
			{
				t.AddCell(VratiCeliju((i++).ToString(), tekst, false, BaseColor.WHITE));
				t.AddCell(VratiCeliju(plan.Opis_podrucje.ToString(), tekst, false, BaseColor.WHITE));
				t.AddCell(VratiCeliju(plan.Svrha, tekst, false, BaseColor.WHITE));
				t.AddCell(VratiCeliju(plan.Zadaca, tekst, false, BaseColor.WHITE));
				t.AddCell(VratiCeliju(plan.Sadrzaj, tekst, false, BaseColor.WHITE));
				t.AddCell(VratiCeliju(plan.Oblici, tekst, false, BaseColor.WHITE));
				t.AddCell(VratiCeliju(plan.Suradnici, tekst, false, BaseColor.WHITE));
				t.AddCell(VratiCeliju(plan.Mjesto, tekst, false, BaseColor.WHITE));
				t.AddCell(VratiCeliju(plan.Vrijeme, tekst, false, BaseColor.WHITE));
				t.AddCell(VratiCeliju(plan.Ishodi, tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(plan.Sati.ToString(), tekst, false, BaseColor.WHITE));
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
	}
}