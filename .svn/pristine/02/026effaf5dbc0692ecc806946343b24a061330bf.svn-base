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
using System.Linq;

namespace ESimSol.Reports
{
    public class rptDUSoftWinding
    {
        #region Declaration
        Document _oDocument;
        PdfWriter _oWriter;
        iTextSharp.text.Font _oFontStyle;
        
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyle_Bold_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(11);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<DUSoftWinding> _oDUSoftWindingList = new List<DUSoftWinding>();
        Company _oCompany = new Company();
        List<InventoryTraking> _oInventoryTrakings = new List<InventoryTraking>();
        List<DURequisitionDetail> _oDURequisitionDetails = new List<DURequisitionDetail>();
        List<RSRawLot> _oRSRawLots = new List<RSRawLot>();
        List<Lot> _oLots = new List<Lot>();
        BusinessUnit _oBusinessUnit;
        int _nColspan = 11;
        double _nQty = 0;
        #endregion

        #region Order Sheet
        public byte[] PrepareReport(List<DUSoftWinding> oDUSoftWindingList, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName, string sStartEndDate)
        {
            _oDUSoftWindingList = oDUSoftWindingList;
            _oBusinessUnit = oBusinessUnit;
            _oCompany = oCompany;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());//842*595
            _oDocument.SetMargins(30f, 30f, 45f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            #region ESimSolFooter
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            #endregion

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                    35f,  //SL 
                                                    60f,  //ReceiveDate
                                                    60f, //OrderNo
                                                    140f,  //Buyer
                                                    60f,  //LotNo                                                
                                                    160f,  //Product                                               
                                                    60f,  //OrderQty
                                                    60f,  //RcvQty
                                                    60f,  //DelQty
                                                    60f,  //NoOfCone
                                                    60f,  //Status
                                             });
            #endregion
            this.PrintHeader(sHeaderName, sStartEndDate);
            this.PrintBody();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        public byte[] PrepareReport_Open(List<DUSoftWinding> oDUSoftWindingList, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName, string sStartEndDate)
        {
            _oDUSoftWindingList = oDUSoftWindingList;
            _oBusinessUnit = oBusinessUnit;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(15f, 15f, 15f, 15f);
           

            _oPdfPTable = new PdfPTable(9); _oPdfPTable.WidthPercentage = 100; 
            _nColspan = 9;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            #region ESimSolFooter
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            #endregion

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                    35f,  //SL 
                                                    60f,  //ReceiveDate
                                                    //60f, //OrderNo
                                                    //140f,  //Buyer
                                                    60f,  //LotNo                                                
                                                    160,  //Product                                               
                                                    //60f,  //OrderQty
                                                    60f,  //RcvQty
                                                    60f,  //DelQty
                                                    60f,  //DelQty
                                                    60f,  //NoOfCone
                                                    60f,  //Status
                                             });
            #endregion
            this.PrintHeader(sHeaderName, sStartEndDate);
            this.PrintBody_Open();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #endregion

        #region Report Header
        private void PrintHeader(string sHeaderName, string sDateRange)
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
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));//_oBusinessUnit.PringReportHead
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColspan; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.Colspan = _nColspan; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Date Range
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sDateRange, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.Colspan = _nColspan; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            #region HEADER
            this.AddCell("SL#", "CENTER");
            this.AddCell("Rcv Date", "CENTER");
            this.AddCell("Order No", "CENTER");
            this.AddCell("Buyer", "CENTER");
            this.AddCell("Lot No", "CENTER");
            this.AddCell("Yarn Type", "CENTER");
            this.AddCell("Order Qty", "CENTER");
            this.AddCell("Rcvd Qty", "CENTER");
            this.AddCell("Issue Qty", "CENTER");
            this.AddCell("Balance", "CENTER");
            this.AddCell("No Of Cone", "CENTER");
          
            _oPdfPTable.CompleteRow();

            #endregion

            int nSL = 0;
            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (DUSoftWinding oItem in _oDUSoftWindingList)
            {
                nSL++;
                this.AddCell(nSL.ToString(), "RIGHT");
                this.AddCell(oItem.ReceiveDateST, "CENTER");
                this.AddCell(oItem.DyeingOrderNo, "LEFT");
                this.AddCell(oItem.ContractorName, "LEFT");
                this.AddCell(oItem.LotNo, "LEFT");
                this.AddCell(oItem.ProductName, "LEFT");
                this.AddCell(Global.MillionFormat(oItem.Qty_Order), "RIGHT");
                this.AddCell(Global.MillionFormat(oItem.Qty), "RIGHT");
                this.AddCell(Global.MillionFormat(oItem.Qty_RSOut), "RIGHT");
                this.AddCell(Global.MillionFormat(oItem.Balance), "RIGHT");
                this.AddCell(Global.MillionFormat(oItem.BagNo), "RIGHT");
                //this.AddCell(oItem.StatusST, "LEFT");
                _oPdfPTable.CompleteRow();
            }
            #endregion
            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            this.AddCell("Total", "RIGHT",6,0);
            this.AddCell(Global.MillionFormat(_oDUSoftWindingList.Sum(x => x.Qty_Order)), "RIGHT");
            this.AddCell(Global.MillionFormat(_oDUSoftWindingList.Sum(x => x.Qty)), "RIGHT");
            this.AddCell(Global.MillionFormat(_oDUSoftWindingList.Sum(x => x.Qty_RSOut)), "RIGHT");
            this.AddCell(Global.MillionFormat(_oDUSoftWindingList.Sum(x => x.Balance)), "RIGHT");
            this.AddCell("", "LEFT");
            //this.AddCell("", "LEFT");
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body [OPEN]
        private void PrintBody_Open()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            #region HEADER
            this.AddCell("SL#", "CENTER");
            this.AddCell("Rcv Date", "CENTER");
            //this.AddCell("Order No", "CENTER");
            //this.AddCell("Buyer", "CENTER");
            this.AddCell("Lot No", "CENTER");
            this.AddCell("Yarn Type", "CENTER");
            //this.AddCell("Order Qty", "CENTER");
            this.AddCell("Rcvd Qty", "CENTER");
            this.AddCell("Issue Qty", "CENTER");
            this.AddCell("Balance", "CENTER");
            this.AddCell("No Of Cone", "CENTER");
            this.AddCell("Status", "CENTER");

            _oPdfPTable.CompleteRow();

            #endregion

            int nSL = 0;

            int nPreviousLotID = -99;
            double nTotal_Qty_RSOut = 0, nTotal_Balance = 0;

            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (DUSoftWinding oItem in _oDUSoftWindingList.OrderBy(x=>x.LotID))
            {
                nSL++;
                this.AddCell(nSL.ToString(), "RIGHT");
                this.AddCell(oItem.ReceiveDateST, "CENTER");
                //this.AddCell(oItem.DyeingOrderNo, "LEFT");
                //this.AddCell(oItem.ContractorName, "LEFT");
                this.AddCell(oItem.LotNo, "LEFT");
                this.AddCell(oItem.ProductName, "LEFT");
                //this.AddCell(Global.MillionFormat(oItem.Qty_Order), "RIGHT");
                this.AddCell(Global.MillionFormat(oItem.Qty), "RIGHT");

                if (nPreviousLotID != oItem.LotID)
                {
                    int nRowSpan = _oDUSoftWindingList.Where(x => x.LotID == oItem.LotID).Count();

                    var nQty_RSOut = _oDUSoftWindingList.Where(x => x.LotID == oItem.LotID).Sum(x => x.Qty_RSOut);
                    var nBalance = _oDUSoftWindingList.Where(x => x.LotID == oItem.LotID).Sum(x => x.Balance);
                    nTotal_Qty_RSOut += nQty_RSOut;
                    nTotal_Balance += nBalance;

                    ESimSolPdfHelper.FontStyle = _oFontStyle;
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nTotal_Qty_RSOut), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nTotal_Balance), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                }
                this.AddCell(Global.MillionFormat(oItem.BagNo), "RIGHT");
                this.AddCell(oItem.StatusST, "LEFT");
                //this.AddCell(oItem.StatusST, "LEFT");
                _oPdfPTable.CompleteRow();
            }
            #endregion
            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            this.AddCell("Total", "RIGHT", 4, 0);
            //this.AddCell(Global.MillionFormat(_oDUSoftWindingList.Sum(x => x.Qty_Order)), "RIGHT");
            this.AddCell(Global.MillionFormat(_oDUSoftWindingList.Sum(x => x.Qty)), "RIGHT");
            this.AddCell(Global.MillionFormat(nTotal_Qty_RSOut), "RIGHT");
            this.AddCell(Global.MillionFormat(nTotal_Balance), "RIGHT");
            this.AddCell("", "LEFT");
            this.AddCell("", "LEFT");
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region PDF HELPER
        public void AddCell(string sHeader, string sAlignment, int nColSpan, int nRowSpan)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;//Default
            if (sAlignment == "LEFT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (sAlignment == "RIGHT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (sAlignment == "CENTER")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            if (nColSpan > 0)
                _oPdfPCell.Colspan = nColSpan;
            if (nRowSpan > 0)
                _oPdfPCell.Rowspan = nRowSpan;

            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddCell(string sHeader, string sAlignment)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;//Default
            if (sAlignment == "LEFT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (sAlignment == "RIGHT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (sAlignment == "CENTER")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void PrintRow(string sHeader)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nColspan;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion

        #region Leadger
        public byte[] PrepareReport_Leadger(List<InventoryTraking> oInventoryTrakings, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName, string sStartEndDate)
        {
            _oInventoryTrakings = oInventoryTrakings;
            _oBusinessUnit = oBusinessUnit;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(15f, 15f, 15f, 15f);


            _oPdfPTable = new PdfPTable(9); _oPdfPTable.WidthPercentage = 100;
            _nColspan = 9;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            #region ESimSolFooter
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            #endregion

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                    35f,  //SL 
                                                    60f,  //ReceiveDate
                                                    //60f, //OrderNo
                                                    //140f,  //Buyer
                                                    60f,  //LotNo                                                
                                                    160,  //Product                                               
                                                    //60f,  //OrderQty
                                                    60f,  //RcvQty
                                                    60f,  //DelQty
                                                    60f,  //DelQty
                                                    60f,  //NoOfCone
                                                    60f,  //Status
                                             });
            #endregion
            this.PrintHeader(sHeaderName, sStartEndDate);
            this.SetDUSoftingLedger();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void SetDUSoftingLedger()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);

            var oLots = _oInventoryTrakings.GroupBy(x => new { x.LotNo, x.LotID, x.ProductName, x.ProductID }, (key, grp) =>
                                  new
                                  {
                                      LotNo  = key.LotNo,
                                      LotID = key.LotID,
                                      ProductName = key.ProductName,
                                      ProductID = key.ProductID
                                  }).ToList();

            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 80f, 100f, 60f, 80f, 80f,80f, 80f });

            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Soft winding Stock Ledger", 0, 7, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);

            oPdfPTable.CompleteRow();


            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion
            foreach (var oItem in oLots)
            {
                var oITrs = _oInventoryTrakings.Where(x => x.LotID == oItem.LotID).ToList();
                #region Fabric Info
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.SetWidths(new float[] { 80f, 100f, 60f, 80f, 80f, 80f, 80f });
                if (oITrs.Count > 0)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Lot No :" + oItem.LotNo + "  Yarn:" + oItem.ProductName, 0, 12, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    oPdfPTable.CompleteRow();
                }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Type", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Ref No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Opening", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "In Qty", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Out Qty", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Balance", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
              
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion
                string sTType = "";
                oITrs = oITrs.OrderBy(o => o.StartDate).ToList();
                foreach (var oItem1 in oITrs)
                {
                    oPdfPTable = new PdfPTable(7);
                    oPdfPTable.SetWidths(new float[] { 80f, 100f, 60f, 80f, 80f, 80f, 80f });
                    if( oItem1.ParentType==EnumTriggerParentsType.RouteSheet)
                    {
                        sTType = "Dyeing Card";
                    }
                    else if (oItem1.ParentType == EnumTriggerParentsType.TransferRequisitionDetail)
                    {
                        sTType = "Requsation";
                    }
                    else if (oItem1.ParentType == EnumTriggerParentsType.RequisitionDetail)
                    {
                        sTType = "Requsation";
                    }
                    else 
                    {
                        sTType = oItem1.ParentType.ToString();
                    }

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.StartDateSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, sTType, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.RefNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.OpeningQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.InQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.OutQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.ClosingQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                }
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.SetWidths(new float[] { 80f, 100f, 60f, 80f, 80f, 80f, 80f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
               
                ESimSolItexSharp.SetCellValue(ref oPdfPTable,"", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nQty = oITrs.Select(c => c.InQty).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nQty = oITrs.Select(c => c.OutQty).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable,"", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
               
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }

        }
        #endregion
        #region current stock
        public byte[] PrepareReport_Stock(List<Lot> oLots, List<InventoryTraking> oInventoryTrakings, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName, string sStartEndDate)
        {
            _oLots = oLots;
            _oInventoryTrakings = oInventoryTrakings;
            _oBusinessUnit = oBusinessUnit;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(15f, 15f, 15f, 15f);


            _oPdfPTable = new PdfPTable(9); _oPdfPTable.WidthPercentage = 100;
            _nColspan = 9;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            #region ESimSolFooter
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            #endregion

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                    35f,  //SL 
                                                    60f,  //ReceiveDate
                                                    //60f, //OrderNo
                                                    //140f,  //Buyer
                                                    60f,  //LotNo                                                
                                                    160,  //Product                                               
                                                    //60f,  //OrderQty
                                                    60f,  //RcvQty
                                                    60f,  //DelQty
                                                    60f,  //DelQty
                                                    60f,  //NoOfCone
                                                    60f,  //Status
                                             });
            #endregion
            this.PrintHeader(sHeaderName, sStartEndDate);
            this.SetDUSoftingstock();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void SetDUSoftingstock()
        {
            int nSL = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);

            var oProducts = _oLots.GroupBy(x => new { x.ProductCode, x.ProductName, x.ProductID }, (key, grp) =>
                                  new
                                  {
                                      ProductName = key.ProductName,
                                      ProductCode= key.ProductCode,
                                      ProductID = key.ProductID
                                  }).ToList();

            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 80f, 100f, 60f, 80f, 80f, 80f, 80f });

            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Soft Winding Stock Report", 0, 7, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);

            oPdfPTable.CompleteRow();


            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion
            foreach (var oItem in oProducts)
            {
                var oLots = _oLots.Where(x => x.ProductID == oItem.ProductID).ToList();
                #region Fabric Info
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 30f, 100f,30f, 80f, 80f, 80f });
                if (oProducts.Count > 0)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable,  "  Yarn:" + oItem.ProductName, 0, 6, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    oPdfPTable.CompleteRow();
                }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Lot No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Unit", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Received Qty", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Issued Qty", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Balance", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion
                nSL = 0;
                oLots = oLots.OrderBy(o => o.LotNo).ToList();
                foreach (var oItem1 in oLots)
                {
                    oPdfPTable = new PdfPTable(6);
                    oPdfPTable.SetWidths(new float[] { 30f, 100f, 30f, 80f, 80f, 80f });

                    nSL++;
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSL.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.MUName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

                    _nQty = _oInventoryTrakings.Where(x => x.LotID == oItem1.LotID ).Select(c => c.InQty).Sum();
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    _nQty = _oInventoryTrakings.Where(x => x.LotID == oItem1.LotID ).Select(c => c.OutQty).Sum();
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Balance), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                }
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 30f, 100f, 30f, 80f, 80f, 80f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                _nQty = _oInventoryTrakings.Where(x => x.ProductID == oItem.ProductID).Select(c => c.InQty).Sum();
               
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nQty = _oInventoryTrakings.Where(x => x.ProductID == oItem.ProductID).Select(c => c.OutQty).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nQty = oLots.Select(c => c.Balance).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
              

                oPdfPTable.CompleteRow();

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 30f, 100f, 30f, 80f, 80f, 80f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Grand Total", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            _nQty = _oInventoryTrakings.Where(x => x.LotID>0).Select(c => c.InQty).Sum();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            _nQty = _oInventoryTrakings.Where(x => x.LotID >0).Select(c => c.OutQty).Sum();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            _nQty = _oLots.Select(c => c.Balance).Sum();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);


            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

        }
        #endregion

        #region Order Statement
        public byte[] PrepareReportOrderStatement(string sHeaderName,  List<DURequisitionDetail> oDURequisitionDetails, List<RSRawLot> oRSRawLots, BusinessUnit oBusinessUnit, Company oCompany)
        {
            _oDURequisitionDetails = oDURequisitionDetails;
            _oRSRawLots = oRSRawLots;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(30f, 30f, 10f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, 1);

            #region ESimSolFooter
            PdfWriter oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler    
            #endregion

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                    35f,  //SL 
                                                    60f,  //ReceiveDate
                                                    60f, //OrderNo
                                                    140f,  //Buyer
                                                    60f,  //LotNo                                                
                                                    160f,  //Product                                               
                                                    60f,  //OrderQty
                                                    60f,  //RcvQty
                                                    60f,  //DelQty
                                                    60f,  //NoOfCone
                                                    60f,  //Status
                                             });
            #endregion

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 10);
            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oBusinessUnit, oCompany, sHeaderName, _oPdfPTable.NumberOfColumns);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 10);

            List<DURequisitionDetail> oSRSs = _oDURequisitionDetails.Where(x => x.RequisitionType == EnumInOutType.Receive).ToList();
            List<DURequisitionDetail> oSRMs = _oDURequisitionDetails.Where(x => x.RequisitionType == EnumInOutType.Disburse).ToList();

            //this.PrintReportHeader(oRPT_Dispos);

            //PrintLotWiseSummary(oRPT_DisposReportBalance);

            if (oSRSs != null && oSRSs.Count > 0)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Store Requisition Slip (SRS)", Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Store Requisition Slip (SRS)", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD));
                _oPdfPTable.CompleteRow();
                this.PrintBody(oSRSs, EnumInOutType.Receive);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 10);
            }
            if (oSRMs != null && oSRMs.Count > 0)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Store Return Memo (SRM)", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD));
                _oPdfPTable.CompleteRow();
                this.PrintBody(oSRMs, EnumInOutType.Disburse);
            }

            this.PrintBodyRSRawOut();
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        public void PrintBody(List<DURequisitionDetail> oDURequisitionDetails, EnumInOutType eInOutType)
        {
            int nCount = 1;
            oDURequisitionDetails = oDURequisitionDetails.OrderBy(p => p.ProductID).ToList();
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;

            //SL, ReqDate, ReqNo, Product, LotNo, Qty, RcvDate
            oPdfPTable.SetWidths(new float[] { 15, 40f, 35, 95, 50, 40, 40 });

            if (eInOutType == EnumInOutType.Receive)
                ESimSolPdfHelper.AddCells(ref oPdfPTable, ("#SL, Req. Date, Req. No, Count, LotNo, Qty, Rcv. Date").Split(','), Element.ALIGN_LEFT, 15);
            if (eInOutType == EnumInOutType.Disburse)
                ESimSolPdfHelper.AddCells(ref oPdfPTable, ("#SL, Req. Date, Req. No, Count, LotNo, Qty, Return Date").Split(','), Element.ALIGN_LEFT, 15);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            foreach (DURequisitionDetail oDURequisitionDetail in oDURequisitionDetails)
            {
                #region Data Row
                ESimSolPdfHelper.AddCell(ref oPdfPTable, (nCount++).ToString(), Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oDURequisitionDetail.ReqDateST, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oDURequisitionDetail.RequisitionNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oDURequisitionDetail.ProductName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oDURequisitionDetail.LotNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oDURequisitionDetail.Qty, Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oDURequisitionDetail.ReceiveDateST, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                #endregion
            }
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Total ", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 5, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, oDURequisitionDetails.Sum(x => x.Qty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            //ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
        }
        public void PrintBodyRSRawOut()
        {
            int nCount = 1;

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;

            //SL, ReqDate, ReqNo, Product, LotNo, Qty, RcvDate
            oPdfPTable.SetWidths(new float[] { 15, 100f, 100f });



            var oLots = _oRSRawLots.GroupBy(x => new { x.LotID, x.LotNo, x.ProductName }, (key, grp) =>
                                       new RSRawLot
                                       {
                                           LotID = key.LotID,
                                           LotNo = key.LotNo,
                                           ProductName = key.ProductName,
                                           Qty = grp.Sum(p => p.Qty),
                                       }).ToList();

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);


            foreach (var oLot in oLots)
            {
                var oRSRawLots = _oRSRawLots.Where(x => x.LotID == oLot.LotID).ToList();

                oPdfPTable = new PdfPTable(3);
                oPdfPTable.SetWidths(new float[] { 15, 100f, 100f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Type:" + oLot.ProductName + ", Lot No:" + oLot.LotNo, 0, 3, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                foreach (var oRSRawLot in oRSRawLots)
                {
                    #region Data Row
                    oPdfPTable = new PdfPTable(3);
                    oPdfPTable.SetWidths(new float[] { 15, 100f, 100f });
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nCount++).ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oRSRawLot.RouteSheetNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oRSRawLot.Qty), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                    #endregion
                }
                oPdfPTable = new PdfPTable(3);
                oPdfPTable.SetWidths(new float[] { 15, 100f, 100f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oLot.Qty), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }


        }


        #endregion 


    }
}