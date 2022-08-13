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
    public class rptInventoryTrackingWIPDetail
    {
        #region Declaration
        int _nTotalColumn = 1;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        InventoryTrackingWIP _oInventoryTrackingWIP = new InventoryTrackingWIP();
        List<InventoryTrackingWIP> _oInventoryTrackingWIPs = new List<InventoryTrackingWIP>();
        Company _oCompany = new Company();
        string _sDateRangeMsg = "";
        Product _oProduct = new Product();
        double _nOpeningQty = 0;
        #endregion

        public byte[] PrepareReport(InventoryTrackingWIP oInventoryTrackingWIP, List<InventoryTrackingWIP> oInventoryTrackingWIPs, Product oProduct, Company oCompany, string sDateRangeMsg, double nOpeningQty)
        {
            _oInventoryTrackingWIP = oInventoryTrackingWIP;
            _oInventoryTrackingWIPs = oInventoryTrackingWIPs.OrderBy(x => x.TransactionTime).ToList();
            _oCompany = oCompany;
            _sDateRangeMsg = sDateRangeMsg;
            _nOpeningQty = nOpeningQty;
            _oProduct = oProduct;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(25f, 25f, 5f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 842f });
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
                _oImag.ScaleAbsolute(70f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        private void PrintHeaderDetail(string sReportHead, string sOrderType, string sDate)
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 170f, 200f, 170f });
            #region
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sReportHead, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sDate, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sOrderType, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLDITALIC));
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reporting Date:" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            this.PrintHeaderDetail("Detail List", " ", _sDateRangeMsg);
            this.SetData();
        }
        #endregion

        private void SetData()
        {
            int nSLNo = 0;
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 34f, 33f, 33f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Product Name: " + _oProduct.ProductName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Opening Qty: " + _nOpeningQty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Closing Qty: " + (((_nOpeningQty + _oInventoryTrackingWIPs.Where(x => x.InOutType == 101).Sum(x => x.Qty)) - _oInventoryTrackingWIPs.Where(x => x.InOutType == 102).Sum(x => x.Qty))).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 10, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            #region Heder Info
            oPdfPTable = new PdfPTable(13);
            oPdfPTable.SetWidths(new float[] { 20f, 80f, 75f, 65f, 80f, 35f, 50f, 50f, 50f, 50f, 50f, 35f, 35f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Lot", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dye Lot", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Store", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "M. Unit", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "In-Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Out-Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Fresh Packing", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Re-Cycle", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Wastage", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Gain", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Loss", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            #region data
            double nTotalQtyIn = 0, nTotalQtyOut = 0, nTotalQtyPacking = 0, nTotalQtyRecycle = 0, nTotalQtyWastage = 0, nTotalQtyGain = 0, nTotalQtyLoss = 0;
            var data = _oInventoryTrackingWIPs.GroupBy(x => new { x.TriggerParentID, x.InOutType }, (key, grp) => new
            {
                TriggerParentID = key.TriggerParentID,
                InOutType = key.InOutType,
                TransactionTimeInSt = (grp.ToList().Count > 0) ? grp.ToList().Max(z => z.TransactionTime).ToString("dd MMM yyyy hh:mm tt") : "",
                LotNo = (grp.ToList().Count > 0) ? string.Join(",", grp.ToList().Select(z=>z.LotNo)) : "",
                RefNo = (grp.ToList().Count > 0) ? grp.ToList().Select(z=>z.RefNo).FirstOrDefault() : "",
                StoreName = (grp.ToList().Count > 0) ? grp.ToList().Select(z => z.StoreName).FirstOrDefault() : "",
                USymbol = (grp.ToList().Count > 0) ? grp.ToList().Select(z => z.USymbol).FirstOrDefault() : "",
                QtyIn = (grp.ToList().Where(z => z.InOutType == 101).ToList().Count() > 0) ? grp.ToList().Where(z => z.InOutType == 101).Sum(y=>y.Qty) : 0,
                QtyOut = (grp.ToList().Where(z => z.InOutType == 102).ToList().Count() > 0) ? grp.ToList().Where(z => z.InOutType == 102).Sum(y => y.Qty) : 0,
                QtyPacking = (grp.ToList().Where(z => z.InOutType == 102).ToList().Count() > 0) ?  grp.ToList().Where(z => z.InOutType == 102).Max(y => y.QtyPacking) : 0,
                QtyRecycle = (grp.ToList().Where(z => z.InOutType == 102).ToList().Count() > 0) ? grp.ToList().Where(z => z.InOutType == 102).Max(y => y.QtyRecycle) : 0,
                QtyWastage = (grp.ToList().Where(z => z.InOutType == 102).ToList().Count() > 0) ? grp.ToList().Where(z => z.InOutType == 102).Max(y => y.QtyWastage) : 0,
                //QtyGain = (grp.ToList().Where(z => z.InOutType == 102).ToList().Count() > 0) ? grp.ToList().Where(z => z.InOutType == 102).Min(y => y.QtyShort) : 0,
                QtyLossGain = (grp.ToList().Where(z => z.InOutType == 102).ToList().Count() > 0) ? grp.ToList().Where(z => z.InOutType == 102).Max(y => y.QtyShort) : 0                
            });

            foreach (var oItem in data)
            {
                oPdfPTable = new PdfPTable(13);
                oPdfPTable.SetWidths(new float[] { 20f, 80f, 75f, 65f, 80f, 35f, 50f, 50f, 50f, 50f, 50f, 35f, 35f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSLNo).ToString("00"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.TransactionTimeInSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.RefNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.StoreName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.USymbol, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InOutType == 101) ? ((oItem.QtyIn).ToString("#,##0.00;(#,##0.00)")) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                if (oItem.InOutType == 101) nTotalQtyIn += oItem.QtyIn;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InOutType == 102) ? ((oItem.QtyOut).ToString("#,##0.00;(#,##0.00)")) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                if (oItem.InOutType == 102) nTotalQtyOut += oItem.QtyOut;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InOutType == 102) ? ((oItem.QtyPacking).ToString("#,##0.00;(#,##0.00)")) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                if (oItem.InOutType == 102) nTotalQtyPacking += oItem.QtyPacking;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InOutType == 102) ? ((oItem.QtyRecycle).ToString("#,##0.00;(#,##0.00)")) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                if (oItem.InOutType == 102) nTotalQtyRecycle += oItem.QtyRecycle;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InOutType == 102) ? ((oItem.QtyWastage).ToString("#,##0.00;(#,##0.00)")) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                if (oItem.InOutType == 102) nTotalQtyWastage += oItem.QtyWastage;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InOutType == 102) ? ((((oItem.QtyLossGain < 0) ? (oItem.QtyLossGain * -1) : 0)).ToString("#,##0.00;(#,##0.00)")) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                if (oItem.InOutType == 102) nTotalQtyGain += (oItem.QtyLossGain < 0) ? (oItem.QtyLossGain * -1) : 0;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InOutType == 102) ? ((((oItem.QtyLossGain > 0) ? oItem.QtyLossGain : 0)).ToString("#,##0.00;(#,##0.00)")) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                if (oItem.InOutType == 102) nTotalQtyLoss += (oItem.QtyLossGain > 0) ? oItem.QtyLossGain : 0;

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }

            //foreach (var oItem in _oInventoryTrackingWIPs)
            //{
            //    oPdfPTable = new PdfPTable(11);
            //    oPdfPTable.SetWidths(new float[] { 25f, 85f, 75f, 75f, 90f, 40f, 50f, 50f, 60f, 60f, 60f });
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSLNo).ToString("00"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.TransactionTimeInSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.RefNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.StoreName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.USymbol, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InOutType == 101) ? Global.MillionFormat(oItem.Qty) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InOutType == 102) ? Global.MillionFormat(oItem.Qty) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InOutType == 102) ? Global.MillionFormat(oItem.QtyPacking) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InOutType == 102) ? Global.MillionFormat(oItem.QtyRecycle) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InOutType == 102) ? Global.MillionFormat(oItem.QtyWastage) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.CurrentBalance), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    oPdfPTable.CompleteRow();
            //    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            //}
            #endregion

            #region Total Info
            oPdfPTable = new PdfPTable(13);
            oPdfPTable.SetWidths(new float[] { 20f, 80f, 75f, 65f, 80f, 35f, 50f, 50f, 50f, 50f, 50f, 35f, 35f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 6, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nTotalQtyIn).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nTotalQtyOut).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nTotalQtyPacking).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nTotalQtyRecycle).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nTotalQtyWastage).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nTotalQtyGain).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nTotalQtyLoss).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

        }

    }
}
