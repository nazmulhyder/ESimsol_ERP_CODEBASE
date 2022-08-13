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
    public class rpt_RPT_Dispo_Stock
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<RSRawLot> _oRSRawLots = new List<RSRawLot>();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<DURequisitionDetail> _oDURequisitionDetails = new List<DURequisitionDetail>();
        List<FabricExecutionOrderYarnReceive> _oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
        #endregion

        #region Date Wise
        public byte[] PrepareReport(string sHeaderName, List<RPT_Dispo> oRPT_Dispos, List<DURequisitionDetail> oDURequisitionDetails, List<RSRawLot> oRSRawLots, BusinessUnit oBusinessUnit, Company oCompany, List<RPT_Dispo> oRPT_DisposReportBalance, List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives)
        {
            _oDURequisitionDetails = oDURequisitionDetails;
            _oRSRawLots = oRSRawLots;
            _oFabricExecutionOrderYarnReceives = oFabricExecutionOrderYarnReceives;
           
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
            _oPdfPTable.SetWidths(new float[] { 595});
            #endregion

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 10);
            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oBusinessUnit, oCompany, sHeaderName, 0);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 10);
            
            List<DURequisitionDetail> oSRSs = _oDURequisitionDetails.Where(x => x.RequisitionType == EnumInOutType.Receive).ToList();
            List<DURequisitionDetail> oSRMs = _oDURequisitionDetails.Where(x => x.RequisitionType == EnumInOutType.Disburse).ToList();

            this.PrintReportHeader(oRPT_Dispos, oRPT_DisposReportBalance);

            PrintLotWiseSummary(oRPT_DisposReportBalance);

            if (oSRSs != null && oSRSs.Count > 0)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Store Requisition Slip (SRS)", Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                this.PrintBody(oSRSs, EnumInOutType.Receive); 
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 10);
            }
            if (oSRMs != null && oSRMs.Count > 0)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Store Return Memo (SRM)", Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                this.PrintBody(oSRMs, EnumInOutType.Disburse);
            }

            List<FabricExecutionOrderYarnReceive> oSRS_WUs = _oFabricExecutionOrderYarnReceives.Where(x => x.RequisitionType == EnumInOutType.Receive).ToList();
            if (oSRS_WUs != null && oSRS_WUs.Count > 0)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Store Requisition Slip (SRS) WU", Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                this.PrintBodyWeaving(oSRS_WUs, EnumInOutType.Receive);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 10);
            }
           oSRS_WUs = _oFabricExecutionOrderYarnReceives.Where(x => x.RequisitionType == EnumInOutType.Disburse).ToList();
            if (oSRS_WUs != null && oSRS_WUs.Count > 0)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Store Requisition Slip (SRM) WU", Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                this.PrintBodyWeaving(oSRS_WUs, EnumInOutType.Receive);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 10);
            }
           
            PrintBodyRSRawOut();
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        public void PrintReportHeader(List<RPT_Dispo> oRPT_Dispos, List<RPT_Dispo> oRPT_DisposReportBalance) 
        {
            PdfPTable oPdfPTable = new PdfPTable(5);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;

            //Product, ReqQty, SRS, SRM, Balance
            oPdfPTable.SetWidths(new float[] { 120f, 50f, 50, 50, 50});

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            #region Data Row
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Dispo No : " + oRPT_Dispos.First().ExeNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Buyer Name : " + oRPT_Dispos.First().BuyerName, Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 0, 0, 4, 0);
            #endregion

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCells(ref oPdfPTable, ("Count, Required Qty, SRS Qty, SRM Qty, Balance").Split(','), Element.ALIGN_LEFT, 15);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            foreach (RPT_Dispo oRPT_Dispo in oRPT_Dispos)
            {
                oRPT_Dispo.Qty_SRS = oRPT_DisposReportBalance.Where(x => x.ProductID == oRPT_Dispo.ProductID).Sum(x => x.Qty_SRS);
                oRPT_Dispo.Qty_SRM = oRPT_DisposReportBalance.Where(x => x.ProductID == oRPT_Dispo.ProductID).Sum(x => x.Qty_SRM);

                oRPT_Dispo.Qty_SRS = oRPT_Dispo.Qty_SRS + _oFabricExecutionOrderYarnReceives.Where(x => x.ProductID == oRPT_Dispo.ProductID && x.RequisitionType == EnumInOutType.Receive).Sum(x => x.ReceiveQty);
                oRPT_Dispo.Qty_SRM = oRPT_Dispo.Qty_SRM + _oFabricExecutionOrderYarnReceives.Where(x => x.ProductID == oRPT_Dispo.ProductID && x.RequisitionType == EnumInOutType.Disburse).Sum(x => x.ReceiveQty);
                //oRPT_Dispo.Qty_SRM = oRPT_DisposReportBalance.Where(x => x.ProductID == oRPT_Dispo.ProductID).Sum(x => x.Qty_SRM);
               // oRPT_Dispo.Qty_Balance = oRPT_Dispo.Qty_SRS - oRPT_Dispo.Qty_SRM;
                #region Data Row
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oRPT_Dispo.ProductName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oRPT_Dispo.Qty_Greige, Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oRPT_Dispo.Qty_SRS, Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oRPT_Dispo.Qty_SRM, Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oRPT_Dispo.Qty_Balance, Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                #endregion
            }

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0); 
          

             oPdfPTable = new PdfPTable(5);
             oPdfPTable.SetWidths(new float[] { 120f, 50f, 50, 50, 50 });
             ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
             ESimSolPdfHelper.AddCell(ref oPdfPTable, "Total", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);


             ESimSolPdfHelper.AddCell(ref oPdfPTable, oRPT_Dispos.Sum(x => x.Qty_Greige), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
             ESimSolPdfHelper.AddCell(ref oPdfPTable, oRPT_Dispos.Sum(x => x.Qty_SRS), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
             ESimSolPdfHelper.AddCell(ref oPdfPTable, oRPT_Dispos.Sum(x => x.Qty_SRM), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
             ESimSolPdfHelper.AddCell(ref oPdfPTable, oRPT_Dispos.Sum(x => x.Qty_Balance), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
             ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0);
             ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 10);

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
            oPdfPTable.SetWidths(new float[]{15, 40f, 35, 95, 50, 40, 40});

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
            ESimSolPdfHelper.AddCell(ref oPdfPTable, oDURequisitionDetails.Sum(x=>x.Qty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
        }

        public void PrintBodyWeaving(List<FabricExecutionOrderYarnReceive> oFEOYRs, EnumInOutType eInOutType)
        {
            int nCount = 1;
            oFEOYRs = oFEOYRs.OrderBy(p => p.ProductID).ToList();
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;

            //SL, ReqDate, ReqNo, Product, LotNo, Qty, RcvDate
            oPdfPTable.SetWidths(new float[] { 15, 40f, 35, 90, 50, 30, 55 });

            if (eInOutType == EnumInOutType.Receive)
                ESimSolPdfHelper.AddCells(ref oPdfPTable, ("#SL, Req. Date, Req. No, Count, LotNo, Qty, Rcv. Date").Split(','), Element.ALIGN_LEFT, 15);
            if (eInOutType == EnumInOutType.Disburse)
                ESimSolPdfHelper.AddCells(ref oPdfPTable, ("#SL, Req. Date, Req. No, Count, LotNo, Qty, Return Date").Split(','), Element.ALIGN_LEFT, 15);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            foreach (FabricExecutionOrderYarnReceive oDURequisitionDetail in oFEOYRs)
            {
                #region Data Row
                ESimSolPdfHelper.AddCell(ref oPdfPTable, (nCount++).ToString(), Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oDURequisitionDetail.IssueDate.ToString("dd MMM yy"), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oDURequisitionDetail.RequisitionNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oDURequisitionDetail.ProductName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oDURequisitionDetail.LotNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oDURequisitionDetail.ReceiveQty, Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oDURequisitionDetail.ReceiveDateInStr, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                #endregion
            }
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Total ", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 5, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, oFEOYRs.Sum(x => x.ReceiveQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
        }



        public void PrintBodyRSRawOut()
        {
            int nCount = 1;

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oFontStyle=FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
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

        public void PrintLotWiseSummary(List<RPT_Dispo> oRPT_DisposLotWiseSummary)
        {
            int nCount = 1;
            double nQTY_SRS = 0;
            double nQTY_SRM = 0;
            double nReq_GreyYarn = 0, nQty_Dye=0;
            
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 100f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Lot Wise Summary", 0, 9, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 100f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f });

            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Type", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Lot No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Assign Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SRS Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SRM Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Due SRS", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty Dyeing", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Balance", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            foreach (var oItem in oRPT_DisposLotWiseSummary)
            {

                oItem.Qty_SRS = oItem.Qty_SRS + _oFabricExecutionOrderYarnReceives.Where(x => x.ProductID == oItem.ProductID && x.LotNo == oItem.StyleNo && x.RequisitionType == EnumInOutType.Receive).Sum(x => x.ReceiveQty);
                oItem.Qty_SRM = oItem.Qty_SRM + _oFabricExecutionOrderYarnReceives.Where(x => x.ProductID == oItem.ProductID && x.LotNo == oItem.StyleNo && x.RequisitionType == EnumInOutType.Disburse).Sum(x => x.ReceiveQty);

                #region Data Row
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] { 100f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable,  Global.MillionFormat(oItem.Qty_Dispo), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.StyleNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Req_GreyYarn), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty_SRS), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty_SRM), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Req_GreyYarn - oItem.Qty_SRS + oItem.Qty_SRM), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty_Dye), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty_SRS - oItem.Qty_SRM - oItem.Qty_Dye), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion
            }
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 100f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable,"Total", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            nReq_GreyYarn = oRPT_DisposLotWiseSummary.Sum(x => x.Req_GreyYarn);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nReq_GreyYarn), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            nQTY_SRS = oRPT_DisposLotWiseSummary.Sum(x => x.Qty_SRS);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQTY_SRS), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            nQTY_SRM = oRPT_DisposLotWiseSummary.Sum(x => x.Qty_SRM);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQTY_SRM), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nReq_GreyYarn - nQTY_SRS + nQTY_SRM), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            nQty_Dye = oRPT_DisposLotWiseSummary.Sum(x => x.Qty_Dye);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQty_Dye), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQTY_SRS + nQTY_SRM - nQty_Dye), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);


        }
        #endregion

    }
}

