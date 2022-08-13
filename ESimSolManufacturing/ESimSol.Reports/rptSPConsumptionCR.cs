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

namespace ESimSol.Reports
{

    public class rptSPConsumptionCR
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        SparePartsConsumptionRegister _oSPCRegister = new SparePartsConsumptionRegister();
        List<SparePartsConsumptionRegister> _oSPCRegisters = new List<SparePartsConsumptionRegister>();
        #endregion

        public byte[] PrepareReport(SparePartsConsumptionRegister oSPCRegister, Company oCompany, BusinessUnit oBusinessUnit)
        {
            _oSPCRegister = oSPCRegister;
            _oSPCRegisters = oSPCRegister.SparePartsConsumptionRegisters;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(30f, 30f, 15f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 842f });
            #endregion

            this.PrintHeader("Capital Report Wise Consumption Report", "");
            this.PrintEmptyRow("", 5);
            this.PrintUnderline("", 10);

            #region Report Type Wise Data List
            if (oSPCRegister.ReportLayout == EnumReportLayout.SPConsumptionCR || oSPCRegister.ReportLayout == EnumReportLayout.SPConsumptionPartsWiseCR) this.DataListForSPConsumptionCR();
            else if (oSPCRegister.ReportLayout == EnumReportLayout.SPConsumptionCRWiseParts || oSPCRegister.ReportLayout == EnumReportLayout.SPConsumptionParts) this.DataListForSPConsumptionCRWiseParts();
            else if (oSPCRegister.ReportLayout == EnumReportLayout.SPConsumptionTransaction) this.DataListForSPConsumptionTransaction();

            #endregion

            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintHeader(string sReportHeader, string sDateRange)
        {
            #region Company & Report Header
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 450f, 250f });

            #region Company Name & Report Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(95f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sReportHeader, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Address & Date Range
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sDateRange, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Phone Number
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
        }
        public void PrintEmptyRow(string sString, int nHeight)
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 842 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.NORMAL);

            #region Row
            _oPdfPCell = new PdfPCell(new Phrase(sString, _oFontStyleBold)); _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void PrintTableHeader(string sString, int nHeight)
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 842 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.NORMAL);

            #region Row
            _oPdfPCell = new PdfPCell(new Phrase(sString, _oFontStyleBold)); _oPdfPCell.FixedHeight = nHeight; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void PrintUnderline(string sString, int nHeight)
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 842 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.NORMAL);

            #region Row
            _oPdfPCell = new PdfPCell(new Phrase(sString, _oFontStyleBold)); _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void DataListForSPConsumptionCR()
        {
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 5, 20, 12, 40, 12, 20 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            this.PrintTableHeader(_oSPCRegister.SearchingCiteria, 18);

            if (_oSPCRegisters.Count > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Resource Type", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Resource Type Name", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Model", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Consumption Amount", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            int nCount = 1;
            foreach (SparePartsConsumptionRegister oItem in _oSPCRegisters)
            {
                PdfPTable oCPdfPTable = new PdfPTable(6);
                oCPdfPTable.WidthPercentage = 100;
                oCPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oCPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                oCPdfPTable.SetWidths(new float[] { 5, 20, 12, 40, 12, 20 });

                _oPdfPCell = new PdfPCell(new Phrase(nCount++.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ResourceTypeName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CRCode, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CRName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CRModel, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.TakaFormat(oItem.Amount).ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);
                oCPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oCPdfPTable);
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            PdfPTable oGrandTotalCell = new PdfPTable(6);
            oGrandTotalCell.WidthPercentage = 100;
            oGrandTotalCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oGrandTotalCell.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oGrandTotalCell.SetWidths(new float[] { 5, 20, 12, 40, 12, 20 });

            _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyleBold)); _oPdfPCell.Colspan = 5;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oGrandTotalCell.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.TakaFormat(_oSPCRegisters.Sum(x => x.Amount)).ToString(), _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oGrandTotalCell.AddCell(_oPdfPCell);
            oGrandTotalCell.CompleteRow();

            _oPdfPCell = new PdfPCell(oGrandTotalCell);
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void DataListForSPConsumptionCRWiseParts()
        {
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 5, 10, 40, 15, 10, 20 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

            this.PrintTableHeader(_oSPCRegister.SearchingCiteria, 18);
            if (_oSPCRegisters.Count > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Spare Parts Name", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Consumption Qty", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Consumption Amount", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            int nCount = 1;
            foreach (SparePartsConsumptionRegister oItem in _oSPCRegisters)
            {
                PdfPTable oCPdfPTable = new PdfPTable(6);
                oCPdfPTable.WidthPercentage = 100;
                oCPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oCPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                oCPdfPTable.SetWidths(new float[] { 5, 10, 40, 15, 10, 20 });

                _oPdfPCell = new PdfPCell(new Phrase(nCount++.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductCode, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Math.Round(oItem.ConsumptionQty, 2).ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUnitName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.TakaFormat(oItem.Amount).ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);
                oCPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oCPdfPTable);
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            PdfPTable oGrandTotalCell = new PdfPTable(6);
            oGrandTotalCell.WidthPercentage = 100;
            oGrandTotalCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oGrandTotalCell.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oGrandTotalCell.SetWidths(new float[] { 5, 10, 40, 15, 10, 20 });

            _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyleBold)); _oPdfPCell.Colspan = 5;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oGrandTotalCell.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.TakaFormat(_oSPCRegisters.Sum(x => x.Amount)).ToString(), _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oGrandTotalCell.AddCell(_oPdfPCell);
            oGrandTotalCell.CompleteRow();

            _oPdfPCell = new PdfPCell(oGrandTotalCell);
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void DataListForSPConsumptionTransaction()
        {
            PdfPTable oPdfPTable = new PdfPTable(13);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 4, 12, 9, 12, 20, 13, 20, 5, 4, 6, 8, 8, 10 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);

            this.PrintTableHeader(_oSPCRegister.SearchingCiteria, 18);
            if (_oSPCRegisters.Count > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Requisition No", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Capital Resource/ Code", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Spare Parts/ Code", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Price", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Currency", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Store Name", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            int nCount = 1;
            foreach (SparePartsConsumptionRegister oItem in _oSPCRegisters)
            {
                PdfPTable oCPdfPTable = new PdfPTable(13);
                oCPdfPTable.WidthPercentage = 100;
                oCPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oCPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                oCPdfPTable.SetWidths(new float[] { 4, 12, 9, 12, 20, 13, 20, 5, 4, 6, 8, 8, 10 });
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

                _oPdfPCell = new PdfPCell(new Phrase(nCount++.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ChallanNo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ChallanDateSt, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.RequisitionNo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CRName + " (" + oItem.CRCode + ")", _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName + " (" + oItem.ProductCode + ")", _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Math.Round(oItem.ConsumptionQty, 2).ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUnitName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Math.Round(oItem.UnitPrice, 2).ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.TakaFormat(oItem.Amount).ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.StoreName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);
                oCPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oCPdfPTable);
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            PdfPTable oGrandTotalCell = new PdfPTable(13);
            oGrandTotalCell.WidthPercentage = 100;
            oGrandTotalCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oGrandTotalCell.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oGrandTotalCell.SetWidths(new float[] { 4, 12, 9, 12, 20, 13, 20, 5, 4, 6, 8, 8, 10 });


            _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyleBold)); _oPdfPCell.Colspan = 11;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oGrandTotalCell.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.TakaFormat(_oSPCRegisters.Sum(x => x.Amount)).ToString(), _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oGrandTotalCell.AddCell(_oPdfPCell);
            
             _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oGrandTotalCell.AddCell(_oPdfPCell);
            oGrandTotalCell.CompleteRow();

            _oPdfPCell = new PdfPCell(oGrandTotalCell);
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }


    }
}

