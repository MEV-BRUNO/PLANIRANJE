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

        public GodisnjiPlanReport(List<Godisnji_plan> godisnji_plan, List<Godisnji_detalji> detalji)
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
			
            p = new Paragraph("Godišnji planovi", naslov);
            p.Alignment = Element.ALIGN_CENTER;
            p.SpacingBefore = 30;
            p.SpacingAfter = 30;
            pdfDokument.Add(p);
			
            //PdfPTable t = new PdfPTable(6);
            //t.WidthPercentage = 100;
            //t.SetWidths(new float[] { 1, 2, 2, 2, 2, 3 });
			
   //         t.AddCell(VratiCeliju("R.br.", tekst, true, BaseColor.LIGHT_GRAY));
   //         t.AddCell(VratiCeliju("Ak. godina", tekst, false, BaseColor.LIGHT_GRAY));
   //         t.AddCell(VratiCeliju("Br. rad. dana", tekst, true, BaseColor.LIGHT_GRAY));
			//t.AddCell(VratiCeliju("God. odmor", tekst, true, BaseColor.LIGHT_GRAY));
			//t.AddCell(VratiCeliju("Ukupno dana", tekst, true, BaseColor.LIGHT_GRAY));
			//t.AddCell(VratiCeliju("God. fond sati", tekst, true, BaseColor.LIGHT_GRAY));

			int i = 1;
            foreach (Godisnji_plan plan in godisnji_plan)
            {
                PdfPTable t = new PdfPTable(6);
                t.WidthPercentage = 100;
                t.SetWidths(new float[] { 1, 2, 2, 2, 2, 3 });

                t.AddCell(VratiCeliju("R.br.", tekst, true, BaseColor.LIGHT_GRAY));
                t.AddCell(VratiCeliju("Ak. godina", tekst, false, BaseColor.LIGHT_GRAY));
                t.AddCell(VratiCeliju("Br. rad. dana", tekst, true, BaseColor.LIGHT_GRAY));
                t.AddCell(VratiCeliju("God. odmor", tekst, true, BaseColor.LIGHT_GRAY));
                t.AddCell(VratiCeliju("Ukupno dana", tekst, true, BaseColor.LIGHT_GRAY));
                t.AddCell(VratiCeliju("God. fond sati", tekst, true, BaseColor.LIGHT_GRAY));

                t.AddCell(VratiCeliju((i++).ToString(), tekst, true, BaseColor.WHITE));
                t.AddCell(VratiCeliju(plan.Ak_godina, tekst, false, BaseColor.WHITE));
                t.AddCell(VratiCeliju(plan.Br_radnih_dana.ToString(), tekst, true, BaseColor.WHITE));
				t.AddCell(VratiCeliju(plan.Br_dana_godina_odmor.ToString(), tekst, true, BaseColor.WHITE));
				t.AddCell(VratiCeliju(plan.Ukupni_rad_dana.ToString(), tekst, true, BaseColor.WHITE));
				t.AddCell(VratiCeliju(plan.God_fond_sati.ToString(), tekst, true, BaseColor.WHITE));

                pdfDokument.Add(t);
                Godisnji_detalji g = detalji.SingleOrDefault(detalj => detalj.Id_god == plan.Id_god);
                if (g != null)
                {
                    
                    PdfPTable tt = new PdfPTable(16);
                    tt.WidthPercentage = 100;
                    //tt.SetWidths(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
                                        
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
                    tt.AddCell(VratiCeliju("Br rad dana sk god", tekst, true, BaseColor.LIGHT_GRAY));
                    tt.AddCell(VratiCeliju("Br dana god odmora", tekst, true, BaseColor.LIGHT_GRAY));
                    tt.AddCell(VratiCeliju("Uk rad dana", tekst, true, BaseColor.LIGHT_GRAY));
                    tt.AddCell(VratiCeliju("God fond sati", tekst, true, BaseColor.LIGHT_GRAY));
                                        
                    tt.AddCell(VratiCeliju(g.Mjesec.ToString(), tekst, false, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(g.Naziv_mjeseca.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(g.Ukupno_dana.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(g.Radnih_dana.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(g.Subota_dana.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(g.Blagdana_dana.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(g.Nastavnih_dana.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(g.Praznika_dana.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(g.Br_sati.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(g.Odmor_dana.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(g.Odmor_sati.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(g.Mj_fond_sati.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(g.Br_rad_dana_sk_god.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(g.Br_dana_god_odmor.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(g.Ukupno_rad_dana.ToString(), tekst, true, BaseColor.WHITE));
                    tt.AddCell(VratiCeliju(g.God_fond_sati.ToString(), tekst, true, BaseColor.WHITE));

                    pdfDokument.Add(tt);
                    
                }
                p = new Paragraph();
                p.Alignment = Element.ALIGN_CENTER;
                p.SpacingBefore = 30;
                p.SpacingAfter = 30;
                pdfDokument.Add(p);
            }

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