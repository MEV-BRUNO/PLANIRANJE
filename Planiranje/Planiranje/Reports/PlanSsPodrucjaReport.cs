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
				PageSize.A4, 50, 50, 20, 50);

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

			p = new Paragraph("Plan srednja skola", naslov);
			p.Alignment = Element.ALIGN_CENTER;
			p.SpacingBefore = 30;
			p.SpacingAfter = 30;
			pdfDokument.Add(p);

			PdfPTable t = new PdfPTable(4);
			t.WidthPercentage = 100;
			t.SetWidths(new float[] { 1, 3, 4, 5 });

			t.AddCell(VratiCeliju("R.br.", tekst, true, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju("Ak. godina", tekst, false, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju("Naziv", tekst, true, BaseColor.LIGHT_GRAY));
			t.AddCell(VratiCeliju("Opis", tekst, true, BaseColor.LIGHT_GRAY));

			int i = 1;
			/*foreach (SS_Plan plan in ss_plan)
			{
				t.AddCell(VratiCeliju((i++).ToString(), tekst, true, BaseColor.WHITE));
				t.AddCell(VratiCeliju(plan.Id_godina.ToString(), tekst, false, BaseColor.WHITE));
				t.AddCell(VratiCeliju(plan.Naziv, tekst, true, BaseColor.WHITE));
				t.AddCell(VratiCeliju(plan.Opis, tekst, true, BaseColor.WHITE));
			}*/

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