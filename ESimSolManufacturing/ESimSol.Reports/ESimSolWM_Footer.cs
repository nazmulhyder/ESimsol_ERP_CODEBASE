using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace ESimSol.Reports
{
    public class ESimSolWM_Footer : PdfPageEventHelper
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

        private string _WM;
        public static int WMFontSize=8;
        public static int WMRotation = 0;
        public static BaseFont WMFont= BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        public string WaterMark
        {
            get { return _WM; }
            set { _WM = value; }
        }
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
        private bool _PrintDocumentGenerator=true;
        public bool PrintDocumentGenerator
        {
            get { return _PrintDocumentGenerator; }
            set { _PrintDocumentGenerator = value; }
        }
        private bool _PrintPrintingDateTime=true;
        public bool PrintPrintingDateTime
        {
            get { return _PrintPrintingDateTime; }
            set { _PrintPrintingDateTime = value; }
        }
        int _nFontSize = 7;
        public int nFontSize
        {
            get { return _nFontSize; }
            set { _nFontSize = value; }
        }
        public List<string> signatures { get; set; }
        public List<string> SignatureName { get; set; }
        public string FooterNote { get; set; }
        #endregion
        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                Rectangle pageSize = document.PageSize;
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
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
            Rectangle pageSize = document.PageSize;
            //if (!string.IsNullOrEmpty(_WM))
            //{
            //    cb.SetRGBColorFill(200, 200, 200);
            //    cb.BeginText();
            //    cb.SetFontAndSize(WMFont, WMFontSize);
            //    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, _WM, pageSize.GetRight((pageSize.Width / 2)), pageSize.GetBottom(pageSize.Height / 2), WMRotation);
            //    cb.EndText();
            //    cb.SetRGBColorFill(100, 100, 100);
            //}
        }
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            int pageN = writer.PageNumber;
            String text = "Page " + pageN + " of ";
            float len = bf.GetWidthPoint(text, _nFontSize);
            Rectangle pageSize = document.PageSize;
            cb.SetRGBColorFill(100, 100, 100);

            float bottomMarginTopBar = 42,  bottomMarginSig = 30;
            if (_nFontSize >= 15 && _nFontSize <= 25) { bottomMarginTopBar = 65; bottomMarginSig = 50; }
            else if (_nFontSize >=26  && _nFontSize < 45) { bottomMarginTopBar = 110; bottomMarginSig = 70; }

            if (FooterNote != null && FooterNote.Any())
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, _nFontSize);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, FooterNote, pageSize.GetLeft(100), pageSize.GetBottom(bottomMarginSig), 0);
                cb.EndText();
            }
            if (signatures != null && signatures.Any())
            {
                float nWidthPerSection = 0;

                float nleft = 0;
                for (int i = 0; i < signatures.Count(); i++)
                {
                    nWidthPerSection = (document.PageSize.Width - (document.LeftMargin + document.RightMargin)) / signatures.Count();
                  
                    string topBar = "";
                    if (signatures[i].Length < 12)
                    {
                        topBar = "____________";
                    }
                    else
                    {
                        topBar = "__";
                        signatures[i].ToList().ForEach(x => { topBar += "_"; });
                    }



                    int nSiglen = (topBar.Length > signatures[i].Length) ? topBar.Length : signatures[i].Length;
                    float nSpan = nWidthPerSection - (nSiglen * 5); // Per Character as print in 5px
                    nleft = (document.LeftMargin * 2) + (nWidthPerSection * i) + ((nSpan > 0) ? nSpan / 2 : 0); // left shift find with cummulative left margin of signature

                    if (i == 0)
                    {
                        cb.BeginText();
                        cb.SaveState();
                        cb.SetFontAndSize(bf, _nFontSize);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, signatures[i], pageSize.GetLeft(nleft),  pageSize.GetBottom(bottomMarginSig), 0);
                        if (SignatureName != null && SignatureName.Any())
                        {
                            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, SignatureName[i], pageSize.GetLeft(nleft), pageSize.GetBottom(bottomMarginSig - 8), 0);
                        }
                        cb.Clip();
                        cb.RestoreState();
                        cb.EndText();

                        cb.BeginText();
                        cb.SetFontAndSize(bf, _nFontSize);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, topBar, pageSize.GetLeft(nleft), pageSize.GetBottom(bottomMarginTopBar), 0);
                        cb.EndText();
                    }
                    else if (i != 0 && i < signatures.Count() - 1)
                    {
                        cb.BeginText();
                        cb.SaveState();
                        cb.SetFontAndSize(bf, _nFontSize);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, signatures[i], pageSize.GetLeft(nleft), pageSize.GetBottom(bottomMarginSig), 0);
                        if (SignatureName != null && SignatureName.Any())
                        {
                            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, SignatureName[i], pageSize.GetLeft(nleft), pageSize.GetBottom(bottomMarginSig - 8), 0);
                        }
                        cb.RestoreState();
                        cb.EndText();

                        cb.BeginText();
                        cb.SetFontAndSize(bf, _nFontSize);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, topBar, pageSize.GetLeft(nleft), pageSize.GetBottom(bottomMarginTopBar), 0);
                        cb.EndText();
                    }
                    else
                    {
                        cb.BeginText();
                        cb.SaveState();
                        cb.SetFontAndSize(bf, _nFontSize);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, signatures[i], pageSize.GetLeft(nleft), pageSize.GetBottom(bottomMarginSig), 0);
                        if (SignatureName != null && SignatureName.Any())
                        {
                            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, SignatureName[i], pageSize.GetLeft(nleft), pageSize.GetBottom(bottomMarginSig - 8), 0);
                        }
                        cb.RestoreState();
                        cb.EndText();

                        cb.BeginText();
                        cb.SetFontAndSize(bf, _nFontSize);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, topBar, pageSize.GetLeft(nleft), pageSize.GetBottom(bottomMarginTopBar), 0);
                        cb.EndText();
                    }
                }
            }

            if (!string.IsNullOrEmpty(_WM))
            {

                PdfGState gstate = new PdfGState(); 
                gstate.FillOpacity=0.3f; 
                gstate.StrokeOpacity= 0.3f; 

                cb.SaveState(); 
                cb.SetGState(gstate);

                cb.SetRGBColorFill(180, 180, 180);
                cb.BeginText();
                cb.SetFontAndSize(WMFont, WMFontSize);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, _WM, pageSize.GetRight((pageSize.Width / 2)), pageSize.GetBottom(pageSize.Height / 2), WMRotation);
                cb.EndText();
                cb.SetRGBColorFill(100, 100, 100);
                
                cb.RestoreState(); 
            }

            cb.BeginText();
            cb.SetFontAndSize(bf, _nFontSize);
            cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetBottom(10));

            cb.ShowText(text);
            cb.EndText();
            cb.AddTemplate(template, pageSize.GetLeft(40) + len, pageSize.GetBottom(10));


            cb.BeginText();
            cb.SetFontAndSize(bf, _nFontSize);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, this.PrintDocumentGenerator ? "ESimSol generated document" : "", pageSize.GetRight((pageSize.Width / 2 - 20)), pageSize.GetBottom(10), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, _nFontSize);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, this.PrintPrintingDateTime ? "Print at " + PrintTime.ToString("dd-MMM-yyyy hh:mm tt") : "", pageSize.GetRight(40), pageSize.GetBottom(10), 0);
            cb.EndText();
        }
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            template.BeginText();
            template.SetFontAndSize(bf, _nFontSize);
            template.SetTextMatrix(0, 0);
            template.ShowText("" + (writer.PageNumber - 1));
            template.EndText();
        }
    }
}
