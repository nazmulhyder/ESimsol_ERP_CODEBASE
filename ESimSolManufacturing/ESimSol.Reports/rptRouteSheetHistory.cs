using System;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ICS.Core.Framework;
using System.Linq;
namespace ESimSol.Reports
{
    public class rptRouteSheetHistory
    {
        #region Declaration
        Document _oDocument;
        int _nColumns = 0;
        int _nPageWidth = 0;
        int _npageHeight = 550;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyle_Bold_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        public iTextSharp.text.Image _oImag { get; set; }
        MemoryStream _oMemoryStream = new MemoryStream();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        Company _oCompany = new Company();
        List<Lot> _oLots = new List<Lot>();
        List<WorkingUnit> _oWorkingUnits = new List<WorkingUnit>();
        List<ITransaction> _oITransactions = new List<ITransaction>();
        RouteSheet _oRouteSheet = new RouteSheet();
        double _nTotalQty = 0;
        #endregion
        #region Lot Traking
        public byte[] PrepareReport(RouteSheet oRouteSheet, List<ITransaction> oITransactions, Company oCompany, BusinessUnit oBusinessUnit, List<Lot> oLots)
        {
            _oRouteSheet = oRouteSheet;
            _oITransactions = oITransactions;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oLots = oLots;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 5f, 5f, 2f, 2f);
            //_oDocument.SetPageSize(PageSize.A4);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(5f, 5f, 10f, 10f);
            PdfWriter _oWriter;
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler   

            _oDocument.Open();
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 98;
            _oPdfPTable.SetWidths(new float[] { 595f });//height:842   width:595
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);


            #endregion

            this.PrintHeader();

            this.Print_StockTracking();
        
            this.PrintProductionConsumption();
            this.PrintDeliveryChallan();
            // _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #region Report Header
        private void PrintHeader()
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 80f, 300.5f, 80f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
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
        }
        #endregion

        #region Stock Tracking
        private void Print_StockTracking()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.UNDERLINE);
            PdfPTable oPdfPTable = new PdfPTable(13);
            PdfPCell oPdfPCell;

            oPdfPTable.SetWidths(new float[] { 110f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f, 60f });


            oPdfPCell = new PdfPCell(new Phrase("RAW LOT FOLLOW UP ", FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD)));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 13;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            #region Stock Tracking Header

          

            oPdfPCell = new PdfPCell(new Phrase("Batch No", _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" :  " + _oRouteSheet.RouteSheetNo + "    Date: " + _oRouteSheet.RouteSheetDateStr, _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 12;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Yarn Type", _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" :  " + _oRouteSheet.ProductName + " [ " + _oRouteSheet.ProductCode + " ] ", _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 12;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Store", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Received", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.Colspan = 6;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Issued", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.Colspan = 5;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Colsing", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Name", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("GRN", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Adj. In", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Dyed Re'd", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Tran In", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Return", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Other", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Adj. Out", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);



            oPdfPCell = new PdfPCell(new Phrase("Raw Issue", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Tran Out", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Delivery", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Consump'n", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Stock", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion
            #region Stock Tracking Body
            double TtlStockInByGrn = 0;
            double TtlProductionOut = 0;
            double TtlProductionIn = 0;
            double TtlTranIn = 0;
            double TtlTranOut = 0;
            double TtlDelivary = 0;
            double TtlReturn = 0;
            double TtlAdjIn = 0;
            double TtlAdjout = 0;
            double TtlBalance = 0;
            double TtlConReqOut = 0;

            var summary = _oLots.GroupBy(p => p.WorkingUnitID).Select(grp => new
            {
                WorkingUnitID = grp.Key,
                WorkingUnitName = grp.First().OperationUnitName,
                StockInByGrn = _oITransactions.Where(x => (x.InOutType == EnumInOutType.Receive) && (x.TriggerParentType == EnumTriggerParentsType.GRNDetailDetail || x.TriggerParentType == EnumTriggerParentsType.DUProGuideLineDetail) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                ProductionOut = _oITransactions.Where(x => (x.InOutType == EnumInOutType.Disburse) && (x.TriggerParentType == EnumTriggerParentsType.RouteSheet) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                ProductionIn = _oITransactions.Where(x => (x.InOutType ==EnumInOutType.Receive) && (x.TriggerParentType == EnumTriggerParentsType.RouteSheet) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                TranIn = _oITransactions.Where(x => (x.InOutType == EnumInOutType.Receive) && (x.TriggerParentType == EnumTriggerParentsType.TransferRequisitionDetail || x.TriggerParentType == EnumTriggerParentsType.RequisitionDetail) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                TranOut = _oITransactions.Where(x => (x.InOutType == EnumInOutType.Disburse) && (x.TriggerParentType == EnumTriggerParentsType.TransferRequisitionDetail || x.TriggerParentType == EnumTriggerParentsType.RequisitionDetail) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                Delivary = _oITransactions.Where(x => (x.InOutType == EnumInOutType.Disburse) && (x.TriggerParentType == EnumTriggerParentsType.DeliveryChallanDetail) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                Return = _oITransactions.Where(x => (x.InOutType ==EnumInOutType.Receive) && (x.TriggerParentType == EnumTriggerParentsType.ReturnChallanDetail) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                AdjIn = _oITransactions.Where(x => (x.InOutType == EnumInOutType.Receive) && (x.TriggerParentType == EnumTriggerParentsType.AdjustmentDetail) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                Adjout = _oITransactions.Where(x => (x.InOutType == EnumInOutType.Disburse) && (x.TriggerParentType == EnumTriggerParentsType.AdjustmentDetail) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                CompReq = _oITransactions.Where(x => (x.InOutType == EnumInOutType.Disburse) && (x.TriggerParentType == EnumTriggerParentsType.ConsumptionRequsition) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                Other = _oITransactions.Where(x => (x.InOutType == EnumInOutType.Receive) && (x.TriggerParentType == EnumTriggerParentsType.SoftWindingProduction || x.TriggerParentType == EnumTriggerParentsType.FabricExecutionOrderYarnReceive) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                Balance = grp.Sum(x => x.Balance)
            }).ToList();

            foreach (var oItem in summary)
            {


                oPdfPCell = new PdfPCell(new Phrase(oItem.WorkingUnitName, _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                //double StockInByGrn = _oLotTrakings.Where(x => (x.InOutType ==(int) EnumInOutType.Receive) && (x.TriggerParentType == (int)EnumTriggerParentsType.GRNDetailDetail) && (x.WorkingUnitID == oItem.WorkingUnitID)).ToList().Sum(x => x.Qty);
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.StockInByGrn).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);


                // double AdjIn = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Receive) && (x.TriggerParentType == (int)EnumTriggerParentsType.AdjustmentDetail) && (x.WorkingUnitID == oItem.WorkingUnitID)).ToList().Sum(x => x.Qty);
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.AdjIn).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                //double ProductionIn = _oLotTrakings.Where(x => (x.InOutType == (int) EnumInOutType.Receive) && (x.TriggerParentType == (int)EnumTriggerParentsType.RouteSheet) && (x.WorkingUnitID == oItem.WorkingUnitID)).ToList().Sum(x => x.Qty);
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.ProductionIn).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                //double TranIn = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Receive) && (x.TriggerParentType == (int)EnumTriggerParentsType.TransferRequisitionDetail) && (x.WorkingUnitID == oItem.WorkingUnitID)).ToList().Sum(x => x.Qty);
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.TranIn).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                //double Delivary = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Disburse) && (x.TriggerParentType == (int)EnumTriggerParentsType.DeliveryChallanDetail) && (x.WorkingUnitID == oItem.WorkingUnitID)).ToList().Sum(x => x.Qty);
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Return).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Other).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);


                // double Adjout = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Receive) && (x.TriggerParentType == (int)EnumTriggerParentsType.DeliveryChallanDetail) && (x.WorkingUnitID == oItem.WorkingUnitID)).ToList().Sum(x => x.Qty);
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Adjout).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);


                // double ProductionOut = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Disburse) && (x.TriggerParentType == (int)EnumTriggerParentsType.RouteSheet) && (x.WorkingUnitID == oItem.WorkingUnitID)).ToList().Sum(x => x.Qty);
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.ProductionOut).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);


                // double TranOut = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Receive) && (x.TriggerParentType == (int)EnumTriggerParentsType.TransferRequisitionDetail) && (x.WorkingUnitID == oItem.WorkingUnitID)).ToList().Sum(x => x.Qty);
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.TranOut).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                //double Delivary = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Disburse) && (x.TriggerParentType == (int)EnumTriggerParentsType.DeliveryChallanDetail) && (x.WorkingUnitID == oItem.WorkingUnitID)).ToList().Sum(x => x.Qty);
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Delivary).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.CompReq), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Balance).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                TtlStockInByGrn = TtlStockInByGrn + oItem.StockInByGrn;
                TtlProductionIn = TtlProductionIn + oItem.ProductionIn;
                TtlProductionOut = TtlProductionOut + oItem.ProductionOut;
                TtlTranIn = TtlTranIn + oItem.TranIn;
                TtlTranOut = TtlTranOut + oItem.TranOut;
                TtlDelivary = TtlDelivary + oItem.Delivary;
                TtlReturn = TtlReturn + oItem.Return;
                TtlAdjIn = TtlAdjIn + oItem.AdjIn;
                TtlAdjout = TtlAdjout + oItem.Adjout;
                TtlConReqOut = TtlConReqOut + oItem.CompReq;
                TtlBalance = TtlBalance + oItem.Balance;


            }

            oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Colspan = 1;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlStockInByGrn).ToString(), _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlAdjIn).ToString(), _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlProductionIn).ToString(), _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlTranIn).ToString(), _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlReturn).ToString(), _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlAdjout).ToString(), _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlProductionOut).ToString(), _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlTranOut).ToString(), _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlDelivary).ToString(), _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlConReqOut).ToString(), _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlBalance).ToString(), _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            _nTotalQty = (TtlStockInByGrn - TtlProductionOut + TtlProductionIn + TtlTranIn - TtlConReqOut - TtlTranOut - TtlDelivary + TtlReturn + TtlAdjIn - TtlAdjout - TtlBalance);
            if ((Math.Abs(_nTotalQty) > 0.2))
            {
                oPdfPCell = new PdfPCell(new Phrase("Stock in Hand", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);
                /// GRNQty-Pro Out+Pro In+Tr in -Tr Out-TtlDelivary
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlStockInByGrn - TtlProductionOut + TtlProductionIn + TtlTranIn - TtlTranOut - TtlDelivary + TtlReturn + +TtlAdjIn - TtlAdjout - TtlConReqOut).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlBalance).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlStockInByGrn - TtlProductionOut + TtlProductionIn + TtlTranIn - TtlTranOut - TtlDelivary + TtlReturn + TtlAdjIn - TtlAdjout - TtlConReqOut - TtlBalance).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Colspan = 6;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();
            }
            #endregion


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }
        #endregion

      

        #region ProductionCOnsumption
        private void PrintProductionConsumption()
        {
            PdfPTable oPdfPTable = new PdfPTable(5);
            PdfPCell oPdfPCell;

            oPdfPTable.SetWidths(new float[] { 70f, 70f, 150f, 90f, 60f });

            List<ITransaction> oLotTrackingProductionConsumption = _oITransactions.Where(x => (x.InOutType == EnumInOutType.Disburse) && (x.TriggerParentType == EnumTriggerParentsType.RouteSheet)).ToList();
            if (oLotTrackingProductionConsumption.Any())
            {
                #region Stock Tracking Header


                oPdfPCell = new PdfPCell(new Phrase("Production Consumption", _oFontStyleBold));
                oPdfPCell.Colspan = 5; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                oPdfPCell.Colspan = 5; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.FixedHeight = 3;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                oPdfPCell = new PdfPCell(new Phrase("Dye Lot No", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Oder No", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion
                #region Stock in Detail Body
                double TtlQty = 0;

                foreach (ITransaction oItem in oLotTrackingProductionConsumption)
                {


                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.TransactionTimeSt, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty).ToString(), _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);
                    oPdfPTable.CompleteRow();
                    TtlQty = TtlQty + oItem.Qty;

                }

                oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.Colspan = 4;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlQty).ToString(), _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion


                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }




        }
        #endregion

        #region Delivery Challan
        private void PrintDeliveryChallan()
        {
            PdfPTable oPdfPTable = new PdfPTable(5);
            PdfPCell oPdfPCell;

            oPdfPTable.SetWidths(new float[] { 70f, 70f, 150f, 90f, 60f });

            List<ITransaction> oLotTrackingProductionConsumption = _oITransactions.Where(x => (x.InOutType == EnumInOutType.Disburse) && (x.TriggerParentType == EnumTriggerParentsType.DeliveryChallanDetail)).ToList();
            if (oLotTrackingProductionConsumption.Any())
            {
                #region Stock Tracking Header


                oPdfPCell = new PdfPCell(new Phrase("Delivery Challan", _oFontStyleBold));
                oPdfPCell.Colspan = 5; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                oPdfPCell.Colspan = 5; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.FixedHeight = 3;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Oder No", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion
                #region Stock in Detail Body
                double TtlQty = 0;

                foreach (ITransaction oItem in oLotTrackingProductionConsumption)
                {


                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.TransactionTimeSt, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty).ToString(), _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);
                    oPdfPTable.CompleteRow();
                    TtlQty = TtlQty + oItem.Qty;

                }

                oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.Colspan = 4;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlQty).ToString(), _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion


                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }




        }
        #endregion

        #endregion
     
        

      
    }
}
