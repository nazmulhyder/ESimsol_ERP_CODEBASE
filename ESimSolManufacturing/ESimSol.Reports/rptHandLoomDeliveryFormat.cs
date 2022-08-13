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
    public class rptHandLoomDeliveryFormat
    {
        #region Declaration
        private int _nColumn = 1;
        private Document _oDocument;
        private iTextSharp.text.Font _oFontStyle;
        private PdfPTable _oPdfPTable = new PdfPTable(1);
        private PdfPCell _oPdfPCell;
        private iTextSharp.text.Image _oImag;
        private MemoryStream _oMemoryStream = new MemoryStream();
        FabricSCReport _oFSR = new FabricSCReport();
        private Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        #endregion

        public byte[] PrepareReport(FabricSCReport oFSR, BusinessUnit oBU, Company oCompany)
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

            _oPdfPTable.SetWidths(new float[] { 595f });

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
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        private void ReporttHeader()
        {
            #region Proforma Invoice Heading Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Hand Loom Delivery Format", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Technical Information Sheet", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
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

            var oFontBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            var oFontNormal = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15f, oFontBold, true);


            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Date: " + DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontNormal, true);



            #region Info Table
            var columns = new string[] { "Customer", "Marketing Reference", "Dispo/ Hand Loom No", "Option No", "Buyer Required Construction", "Weave", "Actual Swatch Construction", "NO. OF Frame", "Weft Color", "Composition" };
            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.SetWidths(new float[] { 55f, 55f, 55f, 50f, 70f, 50f, 70f, 50f, 50f, 55f });

            for (var i = 0; i < columns.Length; i++)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, columns[i], 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontBold);
            }
            oPdfPTable.CompleteRow();



            var oPdfPCell = ESimSolItexSharp.SetCellValue(_oFSR.BuyerName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontNormal);
            oPdfPCell.Rotation = 90; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = ESimSolItexSharp.SetCellValue(_oFSR.FabricNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontNormal);
            oPdfPCell.Rotation = 90; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = ESimSolItexSharp.SetCellValue(_oFSR.HLReference, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontNormal);
            oPdfPCell.Rotation = 90; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = ESimSolItexSharp.SetCellValue("", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontNormal);
            oPdfPCell.Rotation = 90; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = ESimSolItexSharp.SetCellValue(_oFSR.ConstructionPI, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontNormal);
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = ESimSolItexSharp.SetCellValue(_oFSR.FabricWeaveName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontNormal);
            oPdfPCell.Rotation = 90; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = ESimSolItexSharp.SetCellValue(_oFSR.Construction, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontNormal);
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = ESimSolItexSharp.SetCellValue(_oFSR.NoOfFrame.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontNormal);
            oPdfPCell.Rotation = 90; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = ESimSolItexSharp.SetCellValue(_oFSR.WeftColor, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontNormal);
            oPdfPCell.Rotation = 90; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = ESimSolItexSharp.SetCellValue(_oFSR.ProductName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontNormal);
            oPdfPCell.Rotation = 90; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, false);

            #endregion

            #region
            

            //Swatch
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15f, _oFontStyle, true);

            oPdfPTable = new PdfPTable(1); oPdfPTable.SetWidths(new float[] { 565f });
            var oFontSwatch = FontFactory.GetFont("Tahoma", 16f, iTextSharp.text.Font.BOLD);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SWATCH", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, 180f, oFontSwatch, true);
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, true);

            //Special Note of Yarn Dyeing
            oPdfPTable = new PdfPTable(1); oPdfPTable.SetWidths(new float[] { 565f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Special Note of Yarn Dyeing", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontNormal, true);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 50f, oFontNormal, true);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Approve by The Head of The Department", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, oFontNormal, true);
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, true);

            //Special Note of Dyeing House & QC
            oPdfPTable = new PdfPTable(1); oPdfPTable.SetWidths(new float[] { 565f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Special Note of Dyeing House & QC", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontNormal, true);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 80f, oFontNormal, true);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Approve by The Head of The Department", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, oFontNormal, true);
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, true);

            //Special Note of R&D: Weave & Pattern Is Ok According To Buyer Requirement
            oPdfPTable = new PdfPTable(1); oPdfPTable.SetWidths(new float[] { 565f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Special Note of R&D: Weave & Pattern Is Ok According To Buyer Requirement", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontNormal, true);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 95f, oFontNormal, true);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Approve by The Head of The Department", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, oFontNormal, true);
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, true);

            #endregion

        }
        #endregion

        


    }
}

