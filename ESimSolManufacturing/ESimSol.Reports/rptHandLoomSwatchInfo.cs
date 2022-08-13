using System;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol;
using ICS.Core;
using System.Linq;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.Reports
{
    public class rptHandLoomSwatchInfo
    {
        #region Declaration
        private int _nColumn = 2;
        private Document _oDocument;
        private iTextSharp.text.Font _oFontStyle;
        private PdfPTable _oPdfPTable = new PdfPTable(2);
        private PdfPCell _oPdfPCell;
        private iTextSharp.text.Image _oImag;
        private MemoryStream _oMemoryStream = new MemoryStream();
        FabricSCReport _oFSR = new FabricSCReport();
        private Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        #endregion

        public byte[] PrepareReport(FabricSCReport oFSR, Company oCompany, BusinessUnit oBU)
        {
            _oFSR = oFSR;
            _oCompany = oCompany;
            _oBusinessUnit = oBU;
            #region Page Setup

            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 280f, 280f});

            #endregion

            this.PrintHeader();
            this.ReporttHeader();

            this.PrintBody();
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        //private void PrintHeader()
        //{
        //    #region CompanyHeader

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD);
        //    _oPdfPCell = new PdfPCell(PrintHeaderWithLogo());
        //    _oPdfPCell.Colspan = _nColumn;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.Border = 0;
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    _oPdfPCell.ExtraParagraphSpace = 0;
        //    _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();
        //    #endregion
        //}

        //private PdfPTable PrintCompanyInfo()
        //{
        //    PdfPTable oPdfPTable = new PdfPTable(1);
        //    oPdfPTable.SetWidths(new float[] { 400f });
        //    PdfPCell oPdfPCell;
        //    _oFontStyle = FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD);
        //    oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    oPdfPCell.Border = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.ExtraParagraphSpace = 0;
        //    oPdfPTable.AddCell(oPdfPCell);
        //    oPdfPTable.CompleteRow();

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
        //    oPdfPCell = new PdfPCell(new Phrase(_oCompany.FactoryAddress, _oFontStyle));
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    oPdfPCell.Border = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.ExtraParagraphSpace = 0;
        //    oPdfPTable.AddCell(oPdfPCell);
        //    oPdfPTable.CompleteRow();

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.UNDERLINE | iTextSharp.text.Font.BOLD);
        //    oPdfPCell = new PdfPCell(new Phrase("Hand Loom Swatch Information (Factory)", _oFontStyle));
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    oPdfPCell.Border = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPTable.AddCell(oPdfPCell);
        //    oPdfPTable.CompleteRow();

        //    return oPdfPTable;
        //}

        //private PdfPTable PrintHeaderWithLogo()
        //{
        //    PdfPTable oPdfPTable = new PdfPTable(2);
        //    oPdfPTable.SetWidths(new float[] { 10f, 200f });
        //    PdfPCell oPdfPCell;

        //    if (_oCompany.CompanyLogo != null)
        //    {
        //        _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        _oImag.ScaleAbsolute(160f, 35f);
        //        oPdfPCell = new PdfPCell(_oImag);
        //        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
        //        oPdfPCell.Border = 0;
        //        oPdfPCell.FixedHeight = 35;
        //        oPdfPTable.AddCell(oPdfPCell);
        //    }

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD);
        //    oPdfPCell = new PdfPCell(PrintCompanyInfo());
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    oPdfPCell.Border = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.ExtraParagraphSpace = 0;
        //    if (_oCompany.CompanyLogo == null)
        //    {
        //        oPdfPCell.Colspan = 2;
        //    }
        //    oPdfPTable.AddCell(oPdfPCell);
        //    oPdfPTable.CompleteRow();
        //    return oPdfPTable;
        //}

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
                _oImag.ScaleAbsolute(70f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _oPdfPTable.NumberOfColumns;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _oPdfPTable.NumberOfColumns;
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        private void ReporttHeader()
        {
            #region Proforma Invoice Heading Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Hand Loom Swatch Information (Factory)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Colspan = _oPdfPTable.NumberOfColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
     

        #endregion

        #region Body

        private void PrintBody()
        {

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15f, _oFontStyle, true);


            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Delivery Date: "+ DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Option: " + _oFSR.OptionNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            _oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Customer Name: " + _oFSR.BuyerName, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle, true);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Hand Loom Number: " + _oFSR.HandLoomNo, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle, true);

            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "MKTG Reference: " + _oFSR.FabricNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Composition: " + _oFSR.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            _oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Construction: " + _oFSR.Construction, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle, true);
            
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Weave: " + _oFSR.FabricWeaveName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Frame: " + _oFSR.NoOfFrame.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            _oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Finish Type: " + _oFSR.FinishTypeName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Weft Color: " + _oFSR.WeftColor, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            _oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Signature: " + "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Bulk/ Sample: " + _oFSR.SCNoFull, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            _oPdfPTable.CompleteRow();


            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 10f, _oFontStyle, true);


            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, 550f, _oFontStyle, true);

        }
        #endregion

        


    }
}
