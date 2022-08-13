using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace ESimSol.Reports
{
    public class rptCertificateOfOrigin
    {
        #region Declaration
        private int _nColumn = 1;
        private Document _oDocument;
        private iTextSharp.text.Font _oFontStyle;
        private PdfPTable _oPdfPTable = new PdfPTable(1);
        private PdfPCell _oPdfPCell;
        private iTextSharp.text.Image _oImag;
        private MemoryStream _oMemoryStream = new MemoryStream();
        private BDYEAC _oBDYEAC = new BDYEAC();
        private List<BDYEACDetail> _oBDYEACDetails = new List<BDYEACDetail>();
        private Company _oCompany = new Company();
        Phrase _oPhrase = new Phrase();
        Chunk _oChunk = new Chunk();
        #endregion

        public byte[] PrepareReport(BDYEAC oBDYEAC, Company oCompany)
        {
            _oBDYEAC = oBDYEAC;
            _oBDYEACDetails = oBDYEAC.BDYEACDetails;
            _oCompany = oCompany;

            #region Page Setup

            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(50f, 50f, 20f, 10f);
            _oPdfPTable.WidthPercentage = 95;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] {535f });//535

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

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 80f, 300.5f, 80f });


            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 25f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBDYEAC.BUName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBDYEAC.BusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Date print
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Dated : "+DateTime.Today.ToString("dd MMM yyyy"), _oFontStyle));
            _oPdfPCell.FixedHeight = 50f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Lucida Calligraphy", 18f, iTextSharp.text.Font.ITALIC|iTextSharp.text.Font.UNDERLINE|iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Certificate of Origin", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 35f;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion


        #region Report Body

        private void PrintBody()
        {
            Phrase oPhrase = new Phrase();

            #region Certificate of origin
            #region First Pararagraph
            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.NORMAL);
            string sPropertyName = ""; double nTotalQty = 0;
            foreach(BDYEACDetail oItem in _oBDYEACDetails )
            {
                sPropertyName += oItem.ProductName+",";
                nTotalQty += oItem.Qty;
            }
            sPropertyName = sPropertyName.Trim();
            sPropertyName = sPropertyName.Substring(0, sPropertyName.Length-1);
            string sParagraph = "THIS IS TO CERTIFY THAT, WE M/S. " + _oBDYEAC.BUName + "., " +_oBDYEAC.BUAddress + " HAVE EXPORTED " + Global.MillionFormat_Round(nTotalQty) + " LBS " + sPropertyName + " (DYED IN HANK) TO " + _oBDYEAC.PartyName + "," + _oBDYEAC.PartyAddress + " AGAINST BACK TO BACK L/C #" + _oBDYEAC.ExportLCNo + " DTD:" + _oBDYEAC.LCOpeningDateInString + " FOR US $" + Global.MillionFormat(_oBDYEAC.Amount) +" UNDER Export L/C "+_oBDYEAC.MasterLCNos+" DTD: "+_oBDYEAC.MasterLCDates;
            sParagraph = sParagraph.ToUpper();
            Paragraph oParagraph = new Paragraph();
            oParagraph = new Paragraph(new Phrase(sParagraph, _oFontStyle));
            oParagraph.SetLeading(0f, 1.2f);
            oParagraph.Alignment = Element.ALIGN_JUSTIFIED;

            _oPdfPCell = new PdfPCell();
            _oPdfPCell.AddElement(oParagraph);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Second Paragaraph
            sParagraph = "WE FURTHER CERTIFY THAT, we have imported "+sPropertyName+" RAW WHITE HANK FROM "+_oBDYEAC.SupplierName+" under L/C #"+_oBDYEAC.ImportLCNo+" DTD: "+_oBDYEAC.InvoiceDate.ToString("dd.MM.yyyy")+" we have dyed and finished only in our factory.";
            sParagraph = sParagraph.ToUpper();

            oParagraph = new Paragraph(new Phrase(sParagraph, _oFontStyle));
            oParagraph.SetLeading(0f, 1.2f);
            oParagraph.Alignment = Element.ALIGN_JUSTIFIED;

            _oPdfPCell = new PdfPCell();
            _oPdfPCell.AddElement(oParagraph);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion


        }
        #endregion
    }
}
