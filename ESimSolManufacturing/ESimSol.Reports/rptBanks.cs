using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using System.Linq;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System.Text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;


namespace ESimSol.Reports
{

    public class rptBanks
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(5);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Bank _oBank = new Bank();
        List<Bank> _oBanks = new List<Bank>();

        Company _oCompany = new Company();
        double _nSumBankAmount = 0;
        string _sMessage = "";

        #endregion

        public byte[] PrepareReport(Bank oBank, string sMessage)
        {
            _oBanks = oBank.Banks;
            _oCompany = oBank.Company;
            _sMessage = sMessage;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            //SL No,    Bank No, Bank Name,   Narration,  Date,   Authorized By,  Amount,Con. Rate,   Amount
            _oPdfPTable.SetWidths(new float[] { 45f, 60f, 150f, 120f, 160f});
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(160f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.Colspan = 5;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
                _oPdfPCell.Colspan = 5;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address + "\n" + _oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Colspan = 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_sMessage, _oFontStyle));
            _oPdfPCell.Colspan = 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            #region Bangla Text With Image
            //var img1 = iTextSharp.text.Image.GetInstance(CreateImage("ব্লক রেইডে হয়রানি করলে ব্যবস্থা: আছাদুজ্জামান", 24, 600, 50));            
            //_oImag.ScaleAbsolute(160f, 35f);
            //_oPdfPCell = new PdfPCell(img1);
            //_oPdfPCell.Colspan = 5;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.FixedHeight = 35;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion

            #region Bangla Text
            //Sample HTML
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(@"<p>This is a test: <strong>α,β</strong> <strong>বাংলাদেশ কুমিল্লা প্রারম্ভিক</strong></p>");

            //Path to our font
            string arialuniTff = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIALUNI.TTF");
            //Register the font with iTextSharp
            iTextSharp.text.FontFactory.Register(arialuniTff);

            //Create a new stylesheet
            iTextSharp.text.html.simpleparser.StyleSheet ST = new iTextSharp.text.html.simpleparser.StyleSheet();
            //Set the default body font to our registered font's internal name
            ST.LoadTagStyle(HtmlTags.BODY, HtmlTags.FACE, "Arial Unicode MS");
            //Set the default encoding to support Unicode characters
            ST.LoadTagStyle(HtmlTags.BODY, HtmlTags.ENCODING, BaseFont.IDENTITY_H);

            //Parse our HTML using the stylesheet created above
            List<IElement> list = HTMLWorker.ParseToList(new StringReader(stringBuilder.ToString()), ST);

            //Loop through each element, don't bother wrapping in P tags
            foreach (var element in list)
            {
                _oDocument.Add(element);
            }

            //_oFontStyle = FontFactory.GetFont("Arial Unicode MS", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            ////_oFontStyle = FontFactory.GetFont("Arial Unicode MS", "encoding"); 
            ////_oFontStyle = FontFactory.GetFont("Arial Unicode MS", 8f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase("ব্লক রেইডে হয়রানি করলে ব্যবস্থা: আছাদুজ্জামান", _oFontStyle));
            //_oPdfPCell.Colspan = 5; _oPdfPCell.FixedHeight = 25f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Bank Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Short Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("FaxNo", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();


            int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            foreach (Bank oItem in _oBanks)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Code, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Name, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ShortName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.FaxNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
            }

        }

        private static byte[] CreateImage(string text, int fontSize, int width, int height)
        {
            using (var b = new Bitmap(width, height))
            {
                using (var g = Graphics.FromImage(b))
                {
                    using (var br = new SolidBrush(System.Drawing.Color.Red))
                    {
                        using (var f = new System.Drawing.Font("Arial Unicode MS", fontSize))
                        {
                            g.DrawString(text, f, br, 10, 10);
                            using (var ms = new System.IO.MemoryStream())
                            {
                                b.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                                return ms.ToArray();
                            }
                        }
                    }
                }
            }
        }
        #endregion       
    }
}
