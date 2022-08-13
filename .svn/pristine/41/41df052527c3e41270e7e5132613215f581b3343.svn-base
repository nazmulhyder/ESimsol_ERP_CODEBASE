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
    public class rptLotTraking
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
        List<LotTraking> _oLotTrakings = new List<LotTraking>();
        FabricLotAssign _oFabricLotAssign = new FabricLotAssign();
        LotTraking _oLotTraking = new LotTraking();
        List<WorkingUnit> _oWorkingUnits = new List<WorkingUnit>();
        List<LotTraking> _oLotTrakings_Temp = new List<LotTraking>();
        List<RPT_LotTrackings> _oRPT_LotTrackings = new List<RPT_LotTrackings>();
        List<DUOrderRS> _oDUOrderRSs = new List<DUOrderRS>();

        List<LotStockReport> _oLotStockReports = new List<LotStockReport>();
        List<FabricLotAssign> _oFabricLotAssigns = new List<FabricLotAssign>();
        List<DURequisitionDetail> _oDURequisitionDetails = new List<DURequisitionDetail>();
        List<RSRawLot> _oRSRawLots = new List<RSRawLot>();
        List<FabricExecutionOrderYarnReceive> _oFabricEOYarnReceives = new List<FabricExecutionOrderYarnReceive>();
        double _nTotalQty = 0;
        #endregion
        #region Lot Traking
        public byte[] PrepareReport(List<Lot> oLots, List<LotTraking> oLotTrakings, Company oCompany, BusinessUnit oBusinessUnit)
        {
            _oLots = oLots;
            _oLotTrakings = oLotTrakings;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;

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
            this.PrintStockinDetail();
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
            Lot oParentLot = new Lot();
            if (_oLots.Count > 0)
            {
                if (_oLots.FirstOrDefault() != null && _oLots.FirstOrDefault().ParentLotID == 0 && _oLots.Where(b => b.ParentLotID == 0).Count() > 0)
                {
                    oParentLot = _oLots.Where(x => x.ParentLotID == 0).First();
                }
            }

            oPdfPCell = new PdfPCell(new Phrase("Raw Lot No", _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" :  " + oParentLot.LotNo + "   Lot Date: " + oParentLot.LastDateSt, _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 12;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Yarn Type", _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" :  " + oParentLot.ProductName + " [ " + oParentLot.ProductCode + " ] ", _oFontStyleBold));
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
            oPdfPCell.Colspan=6;
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
                StockInByGrn = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Receive) && (x.TriggerParentType == (int)EnumTriggerParentsType.GRNDetailDetail || x.TriggerParentType == (int)EnumTriggerParentsType.DUProGuideLineDetail) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                ProductionOut = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Disburse) && (x.TriggerParentType == (int)EnumTriggerParentsType.RouteSheet) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                ProductionIn = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Receive) && (x.TriggerParentType == (int)EnumTriggerParentsType.RouteSheet) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                TranIn = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Receive) && (x.TriggerParentType == (int)EnumTriggerParentsType.TransferRequisitionDetail || x.TriggerParentType == (int)EnumTriggerParentsType.RequisitionDetail || x.TriggerParentType == (int)EnumTriggerParentsType.FabricExecutionOrderYarnReceive) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                TranOut = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Disburse) && (x.TriggerParentType == (int)EnumTriggerParentsType.TransferRequisitionDetail || x.TriggerParentType == (int)EnumTriggerParentsType.RequisitionDetail || x.TriggerParentType == (int)EnumTriggerParentsType.FabricExecutionOrderYarnReceive) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                Delivary = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Disburse) && (x.TriggerParentType == (int)EnumTriggerParentsType.DeliveryChallanDetail) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                Return = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Receive) && (x.TriggerParentType == (int)EnumTriggerParentsType.ReturnChallanDetail) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                AdjIn = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Receive) && (x.TriggerParentType == (int)EnumTriggerParentsType.AdjustmentDetail) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                Adjout = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Disburse) && (x.TriggerParentType == (int)EnumTriggerParentsType.AdjustmentDetail) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                CompReq = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Disburse) && (x.TriggerParentType == (int)EnumTriggerParentsType.ConsumptionRequsition) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
                Other = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Receive) && (x.TriggerParentType == (int)EnumTriggerParentsType.SoftWindingProduction || x.TriggerParentType == (int)EnumTriggerParentsType.FabricExecutionOrderYarnReceive) && (x.WorkingUnitID == grp.Key)).ToList().Sum(x => x.Qty),
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
                TtlConReqOut=TtlConReqOut+oItem.CompReq;
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
            _nTotalQty = (TtlStockInByGrn - TtlProductionOut + TtlProductionIn + TtlTranIn -TtlConReqOut- TtlTranOut - TtlDelivary + TtlReturn + TtlAdjIn - TtlAdjout - TtlBalance);
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

        #region StockinDetail
        private void PrintStockinDetail()
        {
            PdfPTable oPdfPTable = new PdfPTable(5);
            PdfPCell oPdfPCell;

            oPdfPTable.SetWidths(new float[] { 70f, 70f, 150f, 90f, 60f });
            int RowHeight = 18;
            List<LotTraking> oLotTrackingStockinDetail = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Receive) && (x.TriggerParentType == (int)EnumTriggerParentsType.GRNDetailDetail || x.TriggerParentType == (int)EnumTriggerParentsType.DUProGuideLineDetail)).ToList();
            if (oLotTrackingStockinDetail.Any())
            {
                #region Stock Tracking Header


                oPdfPCell = new PdfPCell(new Phrase("Stock In Detail", _oFontStyleBold));
                oPdfPCell.Colspan = 9; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                oPdfPCell.Colspan = 9; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.FixedHeight = 3;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();
                oPdfPCell = new PdfPCell(new Phrase("GRN No", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Supplier Name", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Invoice No", _oFontStyleBold));
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

                foreach (LotTraking oItem in oLotTrackingStockinDetail)
                {


                    oPdfPCell = new PdfPCell(new Phrase(oItem.TriggerNo, _oFontStyle));
                    oPdfPCell.FixedHeight = RowHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.DateTimeSt, _oFontStyle));
                    oPdfPCell.FixedHeight = RowHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                    oPdfPCell.FixedHeight = RowHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo, _oFontStyle));
                    oPdfPCell.FixedHeight = RowHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty).ToString(), _oFontStyle));
                    oPdfPCell.FixedHeight = RowHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);
                    oPdfPTable.CompleteRow();
                    TtlQty = TtlQty + oItem.Qty;

                }

                oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
                oPdfPCell.FixedHeight = RowHeight;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.Colspan = 4;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlQty).ToString(), _oFontStyleBold));
                oPdfPCell.FixedHeight = RowHeight;
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

        #region ProductionCOnsumption
        private void PrintProductionConsumption()
        {
            PdfPTable oPdfPTable = new PdfPTable(5);
            PdfPCell oPdfPCell;

            oPdfPTable.SetWidths(new float[] { 70f, 70f, 150f, 90f, 60f });

            List<LotTraking> oLotTrackingProductionConsumption = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Disburse) && (x.TriggerParentType == (int)EnumTriggerParentsType.RouteSheet)).ToList();
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

                foreach (LotTraking oItem in oLotTrackingProductionConsumption)
                {


                    oPdfPCell = new PdfPCell(new Phrase(oItem.TriggerNo, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.DateTimeSt, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo, _oFontStyle));
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

            List<LotTraking> oLotTrackingProductionConsumption = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Disburse) && (x.TriggerParentType == (int)EnumTriggerParentsType.DeliveryChallanDetail)).ToList();
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

                foreach (LotTraking oItem in oLotTrackingProductionConsumption)
                {


                    oPdfPCell = new PdfPCell(new Phrase(oItem.TriggerNo, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.DateTimeSt, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo, _oFontStyle));
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
        #region Lot Traking
        public byte[] PrepareReportStoreWise(List<LotTraking> oLotTrakings, Company oCompany, List<WorkingUnit> oWorkingUnits, LotTraking oLotTraking, BusinessUnit oBusinessUnit)
        {

            _oLotTraking = oLotTraking;
            _oLotTrakings = oLotTrakings;
            _oWorkingUnits = oWorkingUnits;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 5f, 5f, 2f, 2f);
            _oDocument.SetPageSize(PageSize.A4);
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
            // this.Print_StockTracking();
            this.PrintHead_Bulk();
            this.PrintHead_Two();
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private void PrintHead_Bulk()
        {
            _oLotTrakings_Temp = new List<LotTraking>();
            _oLotTrakings_Temp = _oLotTrakings.Where(x => x.TriggerParentType != (int)EnumTriggerParentsType.RouteSheet).ToList();
            _oLotTrakings_Temp = _oLotTrakings;

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            _nColumns = 2;
            for (var i = 0; i < _oWorkingUnits.Count; i++)
            {
                _nColumns = _nColumns + 3;
            }


            float[] tablecolumns = new float[_nColumns];
            if (_nColumns <= 5)
            {
                _nPageWidth = 500;
                tablecolumns[0] = 60f;

            }
            else
            {
                _nPageWidth = 62 * (_nColumns);
                tablecolumns[0] = 60f;
                tablecolumns[1] = 70f;

            }

            for (int i = 1; i < _nColumns; i++)
            {
                if (i > 1)
                {
                    tablecolumns[i] = _nPageWidth / _nColumns;
                }
            }

            PdfPTable oPdfPTable = new PdfPTable(_nColumns);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(tablecolumns);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            //// Print Date
            _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            for (var i = 0; i < _oWorkingUnits.Count; i++)
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oWorkingUnits[i].LocationName, _oFontStyleBold));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();

            //oPdfPTable.CompleteRow();
            /////////////
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            for (var i = 0; i < _oWorkingUnits.Count; i++)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Opening", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("Issue Qty", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("Closing", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            /////////
            //oPdfPTable.DeleteBodyRows();
            // oPdfPTable = new PdfPTable(_nColumns);
            //oPdfPTable.SetWidths(tablecolumns);

           
            int nCount = 0;
            double nOpeningQty = 0;
            for (var i = 0; i < _oLotTrakings_Temp.Count; i++)
            {
                nCount++;
                oPdfPTable = new PdfPTable(_nColumns);
                oPdfPTable.SetWidths(tablecolumns);
                _oPdfPCell = new PdfPCell(new Phrase(_oLotTrakings_Temp[i].DateTimeSt, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oLotTrakings_Temp[i].TriggerNo, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                for (var j = 0; j < _oWorkingUnits.Count; j++)
                {
                    if (_oLotTrakings_Temp[i].WorkingUnitID == _oWorkingUnits[j].WorkingUnitID)
                    {
                        nOpeningQty = _oLotTrakings_Temp.Where(x => x.WorkingUnitID == _oLotTrakings_Temp[i].WorkingUnitID && x.TriggerParentID == _oLotTrakings_Temp[i].TriggerParentID).Sum(x => x.OpeningQty);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oLotTrakings_Temp[i].OpeningQty), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oLotTrakings_Temp[i].Qty), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oLotTrakings_Temp[i].ClosingQty), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        nOpeningQty = 0;
                    }

                }
                if (nCount < _oLotTrakings_Temp.Count)
                {
                    if (_oLotTrakings_Temp[i].TriggerParentID != _oLotTrakings_Temp[i + 1].TriggerParentID)
                    {
                        oPdfPTable.CompleteRow();
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        //oPdfPTable.DeleteBodyRows();
                    }
                }
                else
                {
                    oPdfPTable.CompleteRow();
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    //oPdfPTable.DeleteBodyRows();
                }
                //nTriggerParentID = _oLotTrakings[i].TriggerParentID;
            }


            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void PrintHead_Two()
        {
            _oLotTrakings_Temp = new List<LotTraking>();
            _oLotTrakings_Temp = _oLotTrakings.Where(x => x.TriggerParentType == (int)EnumTriggerParentsType.RouteSheet).ToList();
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            _nColumns = 2;
            for (var i = 0; i < _oWorkingUnits.Count; i++)
            {
                _nColumns = _nColumns + 3;
            }


            float[] tablecolumns = new float[_nColumns];
            if (_nColumns <= 1)
            {
                _nPageWidth = 500;
                tablecolumns[0] = 60f;

            }
            else
            {
                _nPageWidth = 62 * (_nColumns);
                tablecolumns[0] = 60f;
                tablecolumns[1] = 70f;

            }

            for (int i = 1; i < _nColumns; i++)
            {
                if (i > 1)
                {
                    tablecolumns[i] = _nPageWidth / _nColumns;
                }
            }

            PdfPTable oPdfPTable = new PdfPTable(_nColumns);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(tablecolumns);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            //// Print Date
            _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Dye Lot No", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            for (var i = 0; i < _oWorkingUnits.Count; i++)
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oWorkingUnits[i].LocationName, _oFontStyleBold));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();

            //oPdfPTable.CompleteRow();
            /////////////
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            for (var i = 0; i < _oWorkingUnits.Count; i++)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Opening", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("Issue Qty", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("Closing", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            /////////
            //oPdfPTable.DeleteBodyRows();
            // oPdfPTable = new PdfPTable(_nColumns);
            //oPdfPTable.SetWidths(tablecolumns);

            int nTriggerParentID = 0;
            int nCount = 0;
            double nOpeningQty = 0;
            for (var i = 0; i < _oLotTrakings_Temp.Count; i++)
            {
                nCount++;
                oPdfPTable = new PdfPTable(_nColumns);
                oPdfPTable.SetWidths(tablecolumns);
                _oPdfPCell = new PdfPCell(new Phrase(_oLotTrakings_Temp[i].DateTimeSt, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oLotTrakings_Temp[i].TriggerNo, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                for (var j = 0; j < _oWorkingUnits.Count; j++)
                {
                    if (_oLotTrakings_Temp[i].WorkingUnitID == _oWorkingUnits[j].WorkingUnitID)
                    {
                        //nOpeningQty = _oLotTrakings_Temp.Where(x => x.WorkingUnitID == _oLotTrakings[i].WorkingUnitID && x.TriggerParentID == _oLotTrakings[i].TriggerParentID).Sum(x => x.OpeningQty);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oLotTrakings_Temp[i].OpeningQty), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oLotTrakings_Temp[i].Qty), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oLotTrakings_Temp[i].ClosingQty), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        nOpeningQty = 0;
                    }

                }
                if (nCount < _oLotTrakings_Temp.Count)
                {
                    if (_oLotTrakings_Temp[i].TriggerParentID != _oLotTrakings_Temp[i + 1].TriggerParentID)
                    {
                        oPdfPTable.CompleteRow();
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        //oPdfPTable.DeleteBodyRows();
                    }
                }
                else
                {
                    oPdfPTable.CompleteRow();
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    //oPdfPTable.DeleteBodyRows();
                }
                //nTriggerParentID = _oLotTrakings[i].TriggerParentID;
            }


            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion

        #region LotWiseOrder
        public byte[] PrepareLotWiseOrderReport(Lot oLot, FabricLotAssign oFabricLotAssign, Company oCompany, BusinessUnit oBusinessUnit, string sLCno)
        {
            _oLots.Add(oLot);
            _oFabricLotAssign = oFabricLotAssign;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 5f, 5f, 2f, 2f);
            _oDocument.SetPageSize(PageSize.A4);
            _oDocument.SetMargins(5f, 5f, 10f, 15f);
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
            this.Print_StockTracking_Lot(sLCno);
            this.PrintOrderWiseConsumption();
           // _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private void Print_StockTracking_Lot(string sLCno)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);
            PdfPTable oPdfPTable = new PdfPTable(10);
            PdfPCell oPdfPCell;

            oPdfPTable.SetWidths(new float[] { 110f, 90f, 90f, 90f, 60f, 60f, 60f, 60f, 60f, 80f });

            oPdfPCell = new PdfPCell(new Phrase("RAW LOT FOLLOW UP ", FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD)));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 10;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            #region Stock Tracking Header
            oPdfPCell = new PdfPCell(new Phrase("Raw Lot No", _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" :  " + _oFabricLotAssign.LotNo, _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 4;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Supplier Name", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" :  " + _oFabricLotAssign.BuyerName, _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Yarn Type", _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" :  " + _oFabricLotAssign.ProductName , _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 4;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("LC No", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" :  " + sLCno, _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        private void PushIntoMainTable(PdfPTable oPdfPTable)
        {
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; 
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        #region ProductionCOnsumption
        private void PrintOrderWiseConsumption()
        {
            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);

            oPdfPTable.SetWidths(new float[] { 70f,  150f, 90f, 70f });

            if (_oFabricLotAssign.FabricLotAssigns.Any())
            {
                _oFabricLotAssign.FabricLotAssigns= _oFabricLotAssign.FabricLotAssigns.OrderBy(x => x.ExeNo).ToList();

                #region Stock Tracking Header
                
                oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                oPdfPCell.Colspan = 4; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.FixedHeight = 3;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();
                PushIntoMainTable(oPdfPTable);
                oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 70f, 150f, 90f, 70f });

                oPdfPCell = new PdfPCell(new Phrase("Total Received Qty", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("ORDER USED IN", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("QTY(kg)", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("BALANCE QTY(kg)", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();
                PushIntoMainTable(oPdfPTable);
                oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 70f, 150f, 90f, 70f });

                #endregion
                #region Stock in Detail Body
                double TtlQty = 0;
                int nCount = 0;

                _oFabricLotAssign.FabricLotAssigns = _oFabricLotAssign.FabricLotAssigns.GroupBy(x => new { x.ExeNo }, (key, grp) => new FabricLotAssign()
                {
                    ExeNo = key.ExeNo,
                    Qty = grp.Sum(x => x.Qty)
              
                }).ToList();
                int nTotalRows = _oFabricLotAssign.FabricLotAssigns.Count();
              
                TtlQty = _oFabricLotAssign.FabricLotAssigns.Sum(x => x.Qty);
                //nCount = _oFabricLotAssign.FabricLotAssigns.Count;
                //if (nCount <= 0) { nCount = 1; }
                bool bflag=true;
                foreach (FabricLotAssign oItem in _oFabricLotAssign.FabricLotAssigns)
                {
                    nCount++;
                    if ((nTotalRows / 2)+1 == nCount)
                    {
                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oFabricLotAssign.Qty) + " kg", _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; //oPdfPCell.Rowspan = nCount;
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthLeft = 1; oPdfPTable.AddCell(oPdfPCell); 
                    }
                    else 
                    {
                        oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; //oPdfPCell.Rowspan = nCount;
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthLeft = 1;
                        oPdfPTable.AddCell(oPdfPCell);
                    }
                    oPdfPCell = new PdfPCell(new Phrase(oItem.ExeNo, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty) + "  ", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);
                    if ((nTotalRows / 2)+1 == nCount)
                    {
                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oFabricLotAssign.Qty - TtlQty) + " kg", _oFontStyle));
                        oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; //oPdfPCell.Rowspan = nCount;
                        oPdfPCell.BorderWidthRight = 1; 
                        oPdfPTable.AddCell(oPdfPCell);
                    }
                    else
                    {
                        oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; //oPdfPCell.Rowspan = nCount;
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthRight = 1; 
                        oPdfPTable.AddCell(oPdfPCell);
                    }
                    oPdfPTable.CompleteRow();
                    bflag = false;
                    PushIntoMainTable(oPdfPTable);
                    oPdfPTable = new PdfPTable(4);
                    oPdfPTable.SetWidths(new float[] { 70f, 150f, 90f, 70f });
                }
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlQty)+" kg", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();
                PushIntoMainTable(oPdfPTable);
                oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 70f, 150f, 90f, 70f });
                #endregion

                //_oPdfPCell = new PdfPCell(oPdfPTable);
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
        }
        #endregion

        #endregion
        #region 
        public byte[] PrepareReportRS( List<DUOrderRS> oDUOrderRSs, List<RPT_LotTrackings> oRPT_LotTrackings, List<LotTraking> oLotTrakings, Company oCompany, BusinessUnit oBusinessUnit)
        {
            _oRPT_LotTrackings = oRPT_LotTrackings;
            _oLotTrakings = oLotTrakings;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oDUOrderRSs = oDUOrderRSs;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 5f, 5f, 2f, 2f);
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
            this.ReporttHeader();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC);
         
            this.PrintStockinDetailTwo();
            this.SetLotDetail();
            this.SetLotDetail_Recycle();
            this.SetOrderRS();
            //this.PrintProductionConsumption();
            //this.PrintDeliveryChallan();
            // _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #region Report Header
        private void ReporttHeader()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 200f, 100f, 200f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Lot History", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            if (_oLotTrakings.Any())
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Lot No.: " + _oLotTrakings[0].LotNo, 0, 5, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            }
            else
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 5, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            }


            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, true);
              
        }
      #endregion
   
        private void SetLotDetail()
        {

            PdfPTable oPdfPTable = new PdfPTable(15);
            oPdfPTable.SetWidths(new float[] { 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });

            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Yarn Lot  History Report (Primary)", 0, 15, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

             oPdfPTable = new PdfPTable(15);
             oPdfPTable.SetWidths(new float[] { 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Received", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Issue to Soft Cone", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Issue To Dyeing Floor", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dyed Yarn Received", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Recycle", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Westage", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Loss/Gain", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "WIP", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Return", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Tranfer From Delivery", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dyed Yarn", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Yarn", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Soft Cone Store ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "A", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "B", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "C", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "D", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "E", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "F", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "G", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "H=D+E+F+G", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "I=C-H", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "J", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "k", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "L", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "M=D-J+K-L", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "N=A-C", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "O=B-C", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                foreach (var oItem in _oRPT_LotTrackings)
                {
                    oPdfPTable = new PdfPTable(15);
                    oPdfPTable.SetWidths(new float[] { 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.RawReced), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtySoft), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyProIn), 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyFinish), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyRecycle), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyWestage), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyShort), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Total), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyWIP), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyDelivery), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyReturn), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyTR), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.DyedYarnStock), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.RawYarnStock), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                   // ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                }
                oPdfPTable = new PdfPTable(15);
                oPdfPTable.SetWidths(new float[] { 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dyeing Card Cancel", 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
               
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                foreach (var oItem in _oRPT_LotTrackings)
                {
                    oPdfPTable = new PdfPTable(15);
                    oPdfPTable.SetWidths(new float[] { 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyRSCancel), 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable,"", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable,"", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                }
            #endregion
        }
        private void SetLotDetail_Recycle()
        {

            PdfPTable oPdfPTable = new PdfPTable(15);
            oPdfPTable.SetWidths(new float[] { 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });

            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Re-Cycle Yarn Lot  History Report (Secondary)", 0, 15, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            oPdfPTable = new PdfPTable(15);
            oPdfPTable.SetWidths(new float[] { 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Recycle  Received", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Recycle to Soft Cone", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Recycle To Dyeing Floor", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dyed Yarn Received", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Recycle", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Westage", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Loss/Gain", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "WIP", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Return", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Tranfer From Delivery", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dyed Yarn", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Yarn", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Soft Cone Store ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "A", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "B", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "C", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "D", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "E", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "F", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "G", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "H=D+E+F+G", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "I=C-H", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "J", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "k", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "L", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "M=D-J+K-L", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "N=A-C", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "O=B-C", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            foreach (var oItem in _oRPT_LotTrackings)
            {
                oPdfPTable = new PdfPTable(15);
                oPdfPTable.SetWidths(new float[] { 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Recycle_Recd), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtySoft_Dye), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyProIn_Dye), 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyFinish_Dye), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyRecycle_Dye), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyWestage_Dye), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyShort_Dye), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.TotalDye), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyWIPDye), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyDelivery_Dye), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyReturn_Dye), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.DyedYarnStockDye), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Recycle_Recd - oItem.QtyProIn_Dye), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }
            #endregion
        }
        private void SetOrderRS()
        {
            var oDUOrderRSsWU = _oDUOrderRSs.GroupBy(x => new { x.WUName, x.WorkingUnitID }, (key, grp) =>
                                  new
                                  {
                                      WUName = key.WUName,
                                      WorkingUnitID = key.WorkingUnitID,
                                  }).ToList();

            PdfPTable oPdfPTable = new PdfPTable(14);
            oPdfPTable.SetWidths(new float[] { 40f, 60f, 100f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f,42f, 42f });

            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Issue information", 0, 14, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);

            oPdfPTable.CompleteRow();


            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion
            foreach (var oItem in oDUOrderRSsWU)
            {
                var oDUOrderRSs = _oDUOrderRSs.Where(x => x.WorkingUnitID == oItem.WorkingUnitID).ToList();
                #region Fabric Info
                oPdfPTable = new PdfPTable(14);
                oPdfPTable.SetWidths(new float[] { 40f, 60f, 100f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });
                if (oDUOrderRSsWU.Count > 0)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Store :" + oItem.WUName, 0, 12, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    oPdfPTable.CompleteRow();
                }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Issue", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dyed Yarn Received", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Recycle", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Westage", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Loss/Gain", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Status", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "WIP", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Return", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Tranfer From Delivery", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dyed Yarn(Stock)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                foreach (var oItem1 in oDUOrderRSs)
                {
                    oPdfPTable = new PdfPTable(14);
                    oPdfPTable.SetWidths(new float[] { 40f, 60f, 100f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.RouteSheetNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.OrderNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ContractorName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.PackingQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.RecycleQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.WastageQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItem1.PackingQty>0)? Global.MillionFormatActualDigit(oItem1.Qty - oItem1.FinishQty):""), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.RSStateSt, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat((oItem1.Qty - oItem1.FinishQty) + (oItem1.Qty - oItem1.FinishQty)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.QtyDC), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.QtyRC), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.QtyTR), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.PackingQty - oItem1.QtyDC + oItem1.QtyRC - oItem1.QtyTR), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                }
                oPdfPTable = new PdfPTable(14);
                oPdfPTable.SetWidths(new float[] { 40f, 60f, 100f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTotalQty = oDUOrderRSs.Select(c => c.Qty).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTotalQty = oDUOrderRSs.Select(c => c.PackingQty).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTotalQty = oDUOrderRSs.Select(c => c.RecycleQty).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTotalQty = oDUOrderRSs.Select(c => c.WastageQty).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTotalQty = oDUOrderRSs.Select(c => (c.Qty-c.FinishQty)).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTotalQty = oDUOrderRSs.Select(c => c.FinishQty).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                _nTotalQty = oDUOrderRSs.Select(c => (c.Qty - c.FinishQty) + (c.Qty - c.FinishQty)).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
              
                _nTotalQty = oDUOrderRSs.Select(c => c.QtyDC).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                _nTotalQty = oDUOrderRSs.Select(c => c.QtyRC).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                _nTotalQty = oDUOrderRSs.Select(c => c.QtyTR).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
               
                _nTotalQty = oDUOrderRSs.Select(c => (c.PackingQty - c.QtyDC+c.QtyRC-c.QtyTR)).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }

        }
      
        private void PrintStockinDetailTwo()
        {
            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;

            oPdfPTable.SetWidths(new float[] {120f, 50f, 40f, 150f, 72f, 72f, 50f });
            int RowHeight = 10;
            List<LotTraking> oLotTrackingStockinDetail = _oLotTrakings.Where(x => (x.InOutType == (int)EnumInOutType.Receive) && (x.TriggerParentType == (int)EnumTriggerParentsType.GRNDetailDetail || x.TriggerParentType == (int)EnumTriggerParentsType.DUProGuideLineDetail)).ToList();
            if (oLotTrackingStockinDetail.Any())
            {
                #region Stock Tracking Header

                oPdfPCell = new PdfPCell(new Phrase("Stock In Detail", _oFontStyleBold));
                oPdfPCell.Colspan = 7; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                oPdfPCell = new PdfPCell(new Phrase("Yarn Type", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("GRN No", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Supplier Name", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Invoice No", _oFontStyleBold));
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

                foreach (LotTraking oItem in oLotTrackingStockinDetail)
                {
                    if (_oRPT_LotTrackings.Count > 0)
                    {
                        oPdfPCell = new PdfPCell(new Phrase(_oRPT_LotTrackings[0].ProductName, _oFontStyle));
                    }
                    else
                    {
                        oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    }
                    oPdfPCell.MinimumHeight = RowHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.TriggerNo, _oFontStyle));
                    oPdfPCell.MinimumHeight = RowHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.DateTimeSt, _oFontStyle));
                    oPdfPCell.MinimumHeight = RowHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                    oPdfPCell.MinimumHeight = RowHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    if (_oRPT_LotTrackings.Count > 0)
                    {
                        oPdfPCell = new PdfPCell(new Phrase(_oRPT_LotTrackings[0].LCNo, _oFontStyle));
                    }
                    else
                    {
                        oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    }
                    
                    oPdfPCell.MinimumHeight = RowHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo, _oFontStyle));
                    oPdfPCell.MinimumHeight = RowHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty).ToString(), _oFontStyle));
                    oPdfPCell.MinimumHeight = RowHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);
                    oPdfPTable.CompleteRow();
                    TtlQty = TtlQty + oItem.Qty;

                }

                oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
                oPdfPCell.FixedHeight = RowHeight;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.Colspan = 6;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(TtlQty).ToString(), _oFontStyleBold));
                oPdfPCell.FixedHeight = RowHeight;
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
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }




        }

        #endregion

        #region
        public byte[] PrepareRawLot_Order_Req_RS(List<LotStockReport> oRPT_LotTrackings, List<FabricLotAssign> oFabricLotAssigns, List<DURequisitionDetail> oDURequisitionDetails, List<RSRawLot> oRSRawLots, Company oCompany, BusinessUnit oBusinessUnit, List<FabricExecutionOrderYarnReceive> oFabricEOYarnReceives)
        {
           
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;

            _oLotStockReports = oRPT_LotTrackings;
            _oFabricLotAssigns = oFabricLotAssigns;
            _oDURequisitionDetails = oDURequisitionDetails;
            _oRSRawLots = oRSRawLots;
            _oFabricEOYarnReceives = oFabricEOYarnReceives;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 5f, 5f, 2f, 2f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(5f, 5f, 10f, 20f);
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
            //this.ReporttHeader();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC);
            this.SetOrder_DUReq_RSHeader();
            this.SetOrder_DUReq_RS();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void SetOrder_DUReq_RS()
        {

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC);

            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 40f, 100f,35f, 35f, 35f, 35f, 35f, 35f,35f });

            #region PO Info
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Issue information", 0, 8, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);
            //oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Type", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Assign Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SRS Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SRM Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SRS Balance", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dyeing Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dyeing Balance", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            double nQtySRSQty = 0, nQtySRMQty = 0, nDyeingQty = 0;
            #endregion
            foreach (var oItem in _oFabricLotAssigns)
            {
                    nQtySRSQty = 0; nQtySRMQty = 0;
                    oPdfPTable = new PdfPTable(9);
                    oPdfPTable.SetWidths(new float[] { 40f, 100f, 35f, 35f, 35f, 35f, 35f, 35f, 35f });
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ExeNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty_Order), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    nQtySRSQty = _oDURequisitionDetails.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID==oItem.ProductID).ToList().Sum(x => x.Qty);
                    nQtySRSQty = nQtySRSQty + _oFabricEOYarnReceives.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList().Sum(x => x.ReceiveQty);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQtySRSQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    nQtySRMQty = _oDURequisitionDetails.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList().Sum(x => x.LotQty);
                    nQtySRMQty = nQtySRMQty + _oFabricEOYarnReceives.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList().Sum(x => x.ReqQty);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nQtySRMQty > 0) ? Global.MillionFormat(nQtySRMQty) : ""), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQtySRSQty - nQtySRMQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    nDyeingQty = _oRSRawLots.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList().Sum(x => x.Qty); 
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nDyeingQty > 0) ? Global.MillionFormat(nDyeingQty) : ""), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormatActualDigit(nQtySRSQty - nQtySRMQty - nDyeingQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
              
            }
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 40f, 100f, 35f, 35f, 35f, 35f, 35f, 35f, 35f });
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable,"", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 2, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_oFabricLotAssigns.Sum(x => x.Qty_Order)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_oFabricLotAssigns.Sum(x => x.Qty)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            nQtySRSQty = _oDURequisitionDetails.ToList().Sum(x => x.Qty);
            nQtySRSQty = nQtySRSQty + _oFabricEOYarnReceives.ToList().Sum(x => x.ReceiveQty);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQtySRSQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            nQtySRMQty = _oDURequisitionDetails.Sum(x => x.LotQty);
            nQtySRMQty = nQtySRMQty + _oFabricEOYarnReceives.ToList().Sum(x => x.ReqQty);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nQtySRMQty > 0) ? Global.MillionFormat(nQtySRMQty) : ""), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQtySRSQty - nQtySRMQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            nDyeingQty = _oRSRawLots.Sum(x => x.Qty);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nDyeingQty > 0) ? Global.MillionFormat(nDyeingQty) : ""), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormatActualDigit(nQtySRSQty - nQtySRMQty - nDyeingQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
           

        }
        private void SetOrder_DUReq_RSHeader()
        {
            _oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC);
            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 80f, 150f, 80f, 170f });

            #region PO Info
            if (_oLotStockReports.Count > 0)
            {
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Lot No" + _oLotStockReports[0].LotNo, 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyle_Bold_UnLine);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Lot Consumption Details [Lot No:" + _oLotStockReports[0].LotNo+"]", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD));
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
           
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Lot No", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":" + _oLotStockReports[0].LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Supplier Name", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":" + _oLotStockReports[0].ContractorName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "L/C No", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":" + _oLotStockReports[0].LCNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":" + "[" + _oLotStockReports[0].ProductCode + "]" + _oLotStockReports[0].ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, true);
            }
            #endregion
          

        }
      
        #endregion
    }
}
