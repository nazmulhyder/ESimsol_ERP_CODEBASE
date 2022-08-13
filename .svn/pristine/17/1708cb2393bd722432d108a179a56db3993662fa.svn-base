using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.Reports
{
    public class rptSaleQuotationFooter : PdfPageEventHelper
    {
        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate template;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;
        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;
        #region Properties
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        public int TotalPageNumber
        {
            get;
            set;
        }

        bool _PrintTotal = true;

        private string _HeaderLeft;
        public string HeaderLeft
        {
            get { return _HeaderLeft; }
            set { _HeaderLeft = value; }
        }
        private string _HeaderRight;
        public string HeaderRight
        {
            get { return _HeaderRight; }
            set { _HeaderRight = value; }
        }
        private Font _HeaderFont;
        public Font HeaderFont
        {
            get { return _HeaderFont; }
            set { _HeaderFont = value; }
        }
        private Font _FooterFont;
        public Font FooterFont
        {
            get { return _FooterFont; }
            set { _FooterFont = value; }
        }
        #endregion
        // we override the onOpenDocument method
        string _sRef = "";
        public rptSaleQuotationFooter(string sRef)
        {
            _sRef = sRef;
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                template = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
            }
            catch (System.IO.IOException ioe)
            {
            }
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            int pageN = writer.PageNumber;
           
            Rectangle pageSize = document.PageSize;
          
            //cb.BeginText();
            //cb.SetFontAndSize(bf, 11);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "________________", pageSize.GetLeft((125)), pageSize.GetBottom(37), 0);
            //cb.EndText();

            //cb.BeginText();
            //cb.SetFontAndSize(bf, 11);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Checked By", pageSize.GetLeft((125)), pageSize.GetBottom(25), 0);
            //cb.EndText();


            //cb.BeginText();
            //cb.SetFontAndSize(bf, 11);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "______________", pageSize.GetRight(125), pageSize.GetBottom(37), 0);
            //cb.EndText();

            //cb.BeginText();
            //cb.SetFontAndSize(bf, 11);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Approved By", pageSize.GetRight(125), pageSize.GetBottom(25), 0);
            //cb.EndText();

            if (pageN > 1)
            {
                String text ="Page "+ pageN+"";
                float len = bf.GetWidthPoint(text, 7);
                cb.SetRGBColorFill(100, 100, 100);

                cb.BeginText();
                cb.SetFontAndSize(bf, 7);
                cb.SetTextMatrix(pageSize.GetLeft(465), pageSize.GetTop(130));
                cb.ShowText(text);
                cb.EndText();

                cb.BeginText();
                cb.SetFontAndSize(bf, 7);
                cb.SetTextMatrix(pageSize.GetLeft(465), pageSize.GetTop(145));
                cb.ShowText(_sRef);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, _sRef, pageSize.GetLeft(512), pageSize.GetTop(145), 0);
                cb.EndText();

                cb.AddTemplate(template, pageSize.GetLeft(465) + len, pageSize.GetTop(130));
            }else 
            {
                //String text =  ""+pageN;
                //float len = bf.GetWidthPoint(text, 7);
                //cb.SetRGBColorFill(00, 00, 00);
                //cb.BeginText();
                //cb.SetFontAndSize(bf, 8);
                //cb.SetTextMatrix(pageSize.GetLeft(400), pageSize.GetTop(220));
                //cb.ShowText(text);
                //cb.EndText();
                //cb.AddTemplate(template, pageSize.GetLeft(490) + len, pageSize.GetTop(130));
                //_PrintTotal = true;

                String text = "";
                float len = bf.GetWidthPoint(text, 7);
                cb.SetRGBColorFill(100, 100, 100);

                cb.BeginText();
                cb.SetFontAndSize(bf, 7);
                cb.SetTextMatrix(pageSize.GetLeft(465), pageSize.GetTop(200));
                cb.ShowText(text);
                cb.EndText();

                cb.AddTemplate(template, pageSize.GetLeft(465) + len, pageSize.GetTop(200));
            }
        }
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            template.BeginText();
            template.SetFontAndSize(bf, 7);
            template.SetTextMatrix(0, 0);
            template.ShowText(" ");//+(writer.PageNumber - 1)+"     "+_sRef
            template.EndText();
        }
    }
}
