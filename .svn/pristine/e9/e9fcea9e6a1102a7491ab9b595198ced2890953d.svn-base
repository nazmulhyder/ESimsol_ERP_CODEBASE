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
    public class rptPrintDUBatchCostFromDyesChemical
    {
        #region Declaration
        int _nTotalColumn = 1;
        float _nfixedHight = 5f;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        DUBatchCost _oDUBatchCost = new DUBatchCost();
        List<DUBatchCost> _oDUBatchCostDetail = new List<DUBatchCost>();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        DUOrderSetup _oDUOrderSetup = new DUOrderSetup();
        DeliverySetup _oDeliverySetup = new DeliverySetup();
        List<RSRawLot> _oRSRawLots = new List<RSRawLot>();
        Company _oCompany = new Company();
        string _sMUnit = "";
        Phrase _oPhrase = new Phrase();
        int _nCount = 0;
        double _nTotalQty = 0;
        double _nTotalAmount = 0;
        double _nQty_Recycle = 0;
        double _nValue_Recycle = 0;
        double _nTotalFreshYarn = 0;
        double _nValue_RawYarn = 0;
        #endregion




        #region
        public byte[] PrepareReport(DUBatchCost oDUBatchCost, List<DUBatchCost> oDUBatchCostDetail, List<RSRawLot> oRSRawLots, Company oCompany, BusinessUnit oBusinessUnit)
        {
            _oDUBatchCost = oDUBatchCost;
            _oDUBatchCostDetail = oDUBatchCostDetail;
            _oBusinessUnit = oBusinessUnit;
            _oRSRawLots = oRSRawLots;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 10f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oPdfPTable.SetWidths(new float[] { 595f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            #endregion
            this.PrintHeader();
            this.ReporttHeader();            
            SetBatchInfo();
            Blank(10);
            SetRawMaterialUsed();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #region Print Header
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


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        #endregion
        #region Report Header
        private void ReporttHeader()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 150f, 200f, 150f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Consumption Statement", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch No.: " + _oDUBatchCost.RSNo, 0, 5, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, true);
        #endregion
        }

        #endregion
        #region
        private void SetRawMaterialUsed()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC);
            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 50f, 180f, 30f, 40f, 40f, 40f, 40f });

            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Material Used", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Material Name  ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Unit", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Add Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Return Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "U/P", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Value", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);


            #endregion
            #region Yarn
            #region Head Col
            double nGrantTotalQty = 0;
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 50f, 180f, 30f, 40f, 40f, 40f, 40f });
            #endregion
            foreach (var oItem1 in _oRSRawLots)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn\n(" + Global.MillionFormat(_oRSRawLots.Select(c => (c.Qty)).Sum()) + ")", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ProductName + ", Lot No:" + oItem1.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.MUnit, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oDUBatchCost.CurrencySymbol + "" + Global.MillionFormatActualDigit(oItem1.UnitPrice), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oDUBatchCost.CurrencySymbol + "" + Global.MillionFormat(oItem1.Qty * oItem1.UnitPrice), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                _nValue_RawYarn = _nValue_RawYarn + (oItem1.Qty * oItem1.UnitPrice);
                nGrantTotalQty += oItem1.Qty;
                oPdfPTable.CompleteRow();
            }

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            _oDUBatchCostDetail = _oDUBatchCostDetail.OrderBy(o => o.ProductCategoryName).ToList();

            var oDUBatchCosts = _oDUBatchCostDetail.GroupBy(x => new { x.ProductCategoryName }, (key, grp) => new DUBatchCost()
            {
                ProductCategoryName = key.ProductCategoryName,

            }).ToList();

            #region Dyes Chemical
            #region Head Col
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 50f, 180f, 30f, 40f, 40f, 40f, 40f });            
            #endregion
            foreach (var oDUBatchCost in oDUBatchCosts)
            {
                _nCount = 0;
                var oDUBatchCostDetail = _oDUBatchCostDetail.Where(x => x.ProductCategoryName == oDUBatchCost.ProductCategoryName).ToList();
                foreach (var oItem1 in oDUBatchCostDetail)
                {
                    _nCount++;
                    if (_nCount == 1)
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ProductCategoryName + "\n(" + Global.MillionFormatActualDigit(Math.Round(oDUBatchCostDetail.Select(c => (c.TotalQty)).Sum(), 3)) + ")", oDUBatchCostDetail.Count, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    }
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ProductName + ", Lot No:" + oItem1.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.MUnit, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    //  oItem1.Qty= oItem1.TotalQty + oItem1.AddOne + oItem1.AddTwo + oItem1.AddThree - oItem1.ReturnQty;
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem1.Qty > 0) ? Global.MillionFormatActualDigit(Math.Round(oItem1.Qty, 5)) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem1.AdditionalQty > 0) ? Global.MillionFormatActualDigit(Math.Round(oItem1.AdditionalQty, 5)) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem1.ReturnQty > 0) ? Global.MillionFormatActualDigit(Math.Round(oItem1.ReturnQty, 5)) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem1.TotalQty > 0) ? Global.MillionFormatActualDigit(Math.Round(oItem1.TotalQty, 5)) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    nGrantTotalQty += oItem1.TotalQty;
                    //if (oItem1.TotalRate > 0)
                    //{ ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oDUBatchCost.CurrencySymbol + "" + Global.MillionFormatActualDigit(Math.Round(oItem1.TotalRate, 5)), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle); }
                    //else
                    //{
                    //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    //}
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oDUBatchCost.CurrencySymbol + "" + Global.MillionFormat(oItem1.Value), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    oPdfPTable.CompleteRow();

                }
            }
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Cost", 0, 6, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //_nTotalAmount = _oDUBatchCostDetail.Select(c => c.Value).Sum();
            //if (_oDUBatchCost.IsInHouse == true)
            //{
            //    _nTotalAmount = _nTotalAmount + _oRSRawLots.Select(c => (c.Qty * c.UnitPrice)).Sum();
            //}
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nGrantTotalQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oDUBatchCost.CurrencySymbol + "" + Global.MillionFormat(_nTotalAmount / _oDUBatchCost.Qty_Yarn), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oDUBatchCost.CurrencySymbol + "" + Global.MillionFormat(_nTotalAmount), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

        }
        private void SetBatchInfo()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC);
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 80f, 100f, 80f, 100f, 80f, 100f });

            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch & Order Information", 0, 6, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oDUBatchCost.RSNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oDUBatchCost.RSDateSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Status", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((EnumRSState)_oDUBatchCost.RSState).ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oDUBatchCost.OrderNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oDUBatchCost.ColorName, 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Status", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":" + _oDUBatchCost.RSState.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oDUBatchCost.ContractorName, 0, 5, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":" + _oDUBatchCost.ColorName, 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Status", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":" + _oDUBatchCost.RSState.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oDUBatchCost.IsReDyeingST, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oDUBatchCost.IsInHouseSt, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);


            #endregion
        }


        #endregion

        private void Blank(int nFixedHeight)
        {

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 1; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nFixedHeight; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

    }
}
