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
using ESimSol.Reports;

namespace ESimSol.BusinessObjects
{
    public class rptHWStatement
    {
        #region Declaration
        PdfWriter _oWriter;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyle_Bold_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        MemoryStream _oMemoryStream = new MemoryStream();

        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<DUHardWinding> _oDUHardWindings = new List<DUHardWinding>();
        List<DyeingOrder> _oDyeingOrders = new List<DyeingOrder>();
        List<DyeingOrderDetail> _oDyeingOrderDetails = new List<DyeingOrderDetail>();
        List<DyeingOrderFabricDetail> _oDyeingOrderFabricDetails = new List<DyeingOrderFabricDetail>();
        List<FabricExecutionOrderYarnReceive> _oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
        List<RouteSheetPacking> _oRouteSheetPackings = new List<RouteSheetPacking>();
        FabricExecutionOrderSpecification _oFabricExecutionOrderSpecification = new FabricExecutionOrderSpecification();
        #endregion

        public byte[] PrepareReport(List<DUHardWinding> oDUHardWindings, List<DyeingOrder> oDyeingOrders, List<DyeingOrderDetail> oDyeingOrderDetails, List<DyeingOrderFabricDetail> oDyeingOrderFabricDetail, BusinessUnit oBusinessUnit, Company oCompany, List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives, List<RouteSheetPacking> oRouteSheetPackings, FabricExecutionOrderSpecification oFabricExecutionOrderSpecification)
        {
            _oDUHardWindings = oDUHardWindings;
            _oDyeingOrders = oDyeingOrders;
            _oDyeingOrderDetails = oDyeingOrderDetails;
            _oDyeingOrderFabricDetails = oDyeingOrderFabricDetail;
            _oFabricExecutionOrderSpecification = oFabricExecutionOrderSpecification;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oFabricExecutionOrderYarnReceives = oFabricExecutionOrderYarnReceives;
            _oRouteSheetPackings = oRouteSheetPackings;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 10f, 10f, 5f, 20f);
            
            _oPdfPTable = new PdfPTable(1);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] {842});
            #endregion

            #region Report Body & Header
            oCompany.Name = _oBusinessUnit.Name;

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oCompany, "Dispo Statement", 1);

            foreach (DyeingOrder oItem in _oDyeingOrders)
            {
                this.PrintBody(oItem);
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Dispo In Hard wending", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 0, 15);
                this.PrintBody_Transfer(oItem);
            }
            if (_oDyeingOrderFabricDetails.Count > 0)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Weaving Transfer Information", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 0, 15);
                this.PrintWarpingInfo();
            }            
           
            #endregion

            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Table Defination
        public PdfPTable GetDetailsTable()
        {
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            //"SL No", "Yarn", "LD No", "Color", "Batch No", "Order Qty", "Batch Qty", "In H. Winding", "Balance" 
            oPdfPTable.SetWidths(new float[]{
                                                20f,
                                                90f,
                                                90f,
                                                60f,
                                                60f,

                                                50f,
                                                50f,
                                                50f,
                                                50f,
                                                //65f,
                                                
                                                //60f,
                                                //100f
            });
            return oPdfPTable;
        }
        public PdfPTable GetInfoTable() 
        {
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[]{
                                                50f,
                                                05f,
                                                50f,

                                                50f,
                                                05f,
                                                120,

                                                //60f,
                                                //60f,
                                                //60f,
                                                //120,
            });
            return oPdfPTable;
        }

        public PdfPTable GetTransferInfoTable()
        {
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            //"  "#SL", "Yarn", "Color", "Warp/Weft", "Req Length", "Isssue Length", "Req Cone","Issue Cone", "Req Qty", "Rece'd Qty", "Issue Qty", "Balance"
            oPdfPTable.SetWidths(new float[]{
                                                20f,
                                                120f,
                                                70f,
                                                45f,
                                                50f,
                                                50f,
                                                50f,
                                                50f,
                                                50f,
                                                50f,
                                                50f,
                                                50f
                                               
            });
            return oPdfPTable;
        }

        public PdfPTable GetWarpingInfoTable()
        {
            PdfPTable oPdfPTable = new PdfPTable(11);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            oPdfPTable.SetWidths(new float[]{
                                                20f,//sl
                                                110f,//yarn
                                                70f,//color
                                                50f,//order qty
                                                40f,//warp/weft
                                                65f,//lot no
                                                90f,
                                                52f,//type
                                                48f,//in qty
                                                48f,//out qty
                                                48f//balance
                                               
            });
            return oPdfPTable;
        }
        #endregion
        
        #region Report Body Sale Invoice
        private void PrintBody(DyeingOrder oDyeingOrder)
        {
            #region Order Info

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            string[] Infos = new string[] {  "Dispo No",        ":", oDyeingOrder.OrderNoFull,
                                             "Customer Name",   ":", oDyeingOrder.ContractorName, 
                                             "Order Date",      ":", oDyeingOrder.OrderDateSt, 
                                             "MKTP Name",       ":", oDyeingOrder.MKTPName};

            PdfPTable oPdfPTable = GetInfoTable();
            ESimSolPdfHelper.AddCells(ref oPdfPTable, Infos, Element.ALIGN_LEFT,0);

            oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable); oPdfPTable = new PdfPTable(8);
            #endregion

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Color Info", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 0, 15);

            #region Column Header

            oPdfPTable = GetDetailsTable();
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);

            string[] Headers = new string[] { "#SL", "Yarn",  "Color", "Batch No","Status", "Order Qty", "Batch Qty", "In H. Winding", "Balance" }; //"Color Name",

            ESimSolPdfHelper.PrintHeaders(ref oPdfPTable, Headers);
           
            oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
            #endregion

            #region Data List Wise Loop
            oPdfPTable = new PdfPTable(10);
            oPdfPTable = GetDetailsTable();

            int nCount = 0;
            int nDODID = 0;
            foreach (var obj in _oDUHardWindings.Where(x => x.DyeingOrderID == oDyeingOrder.DyeingOrderID).OrderBy(x=>x.DyeingOrderDetailID))
            {
                #region DATA
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
               
                int nRowSpan = 1;
                if (nDODID != obj.DyeingOrderDetailID)
                {
                    if (nCount > 0)
                    {
                        var nOrderQty = _oDyeingOrderDetails.Where(x => x.DyeingOrderDetailID == nDODID).Select(x=>x.Qty).FirstOrDefault();
                        var oDUHW = _oDUHardWindings.Where(x => x.DyeingOrderDetailID == nDODID).ToList();

                        ESimSolPdfHelper.AddCell(ref oPdfPTable, "Total", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 5, 10);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nOrderQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oDUHW.Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oDUHW.Sum(x => x.Qty_RSOut)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oDUHW.Sum(x => (x.Qty - x.Qty_RSOut))), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        oPdfPTable.CompleteRow();

                        ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable); 
                        oPdfPTable = GetDetailsTable();
                    }
                    ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
                    nRowSpan = _oDUHardWindings.Where(x => x.DyeingOrderDetailID == obj.DyeingOrderDetailID).Count();
                    var oDOD = _oDyeingOrderDetails.Where(x => x.DyeingOrderDetailID == obj.DyeingOrderDetailID).FirstOrDefault();

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (++nCount).ToString(), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 10);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ProductName, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 10);
                    //ESimSolPdfHelper.AddCell(ref oPdfPTable, (oDOD != null ? oDOD.LabdipNo : ""), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 10);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ColorName, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 10);
                   
                }
              

                ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.RouteSheetNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.RSStateSt, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);

                if (nDODID != obj.DyeingOrderDetailID)
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.Qty_Order), Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 10);
                }

                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.Qty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.Qty_RSOut), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.Qty - obj.Qty_RSOut), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                
                nDODID = obj.DyeingOrderDetailID;
                oPdfPTable.CompleteRow();
                #endregion
            }
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            var nOrder_Qty = _oDyeingOrderDetails.Where(x => x.DyeingOrderDetailID == nDODID).Select(x => x.Qty).FirstOrDefault();
            var oDUHW_Total = _oDUHardWindings.Where(x => x.DyeingOrderDetailID == nDODID).ToList();
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Total", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 5, 10);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nOrder_Qty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oDUHW_Total.Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oDUHW_Total.Sum(x => x.Qty_RSOut)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oDUHW_Total.Sum(x => (x.Qty - x.Qty_RSOut))), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            
            oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0, 0, 0, 15);
            #endregion
        }

        private void PrintBody_Transfer(DyeingOrder oDyeingOrder)
        {
            #region Column Header
            _oFontStyle = FontFactory.GetFont("Tahoma",8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);

            
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 
                                                100f,
                                                150f,
                                                100f,
                                                150f,
                                                100f,
                                                150f,
                                               
                                                 });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Required Length", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "" + Global.MillionFormat_Round(_oFabricExecutionOrderSpecification.RequiredWarpLength)+" Mtr", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finish Type", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "" + _oFabricExecutionOrderSpecification.FinishType, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Weave Type", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "" + _oFabricExecutionOrderSpecification.Weave, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reed Width", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "" + Global.MillionFormat_Round(_oFabricExecutionOrderSpecification.ReedWidth), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reed", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "" + _oFabricExecutionOrderSpecification.Reed.ToString() + "/" + _oFabricExecutionOrderSpecification.Dent.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Composition", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "" + _oFabricExecutionOrderSpecification.Composition, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Construction", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "" + _oFabricExecutionOrderSpecification.Construction, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ""  , 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);





            oPdfPTable = new PdfPTable(11);
            oPdfPTable.SetWidths(new float[] { 20f,
                                                120f,
                                                70f,
                                                60f,
                                                50f,
                                                50f,
                                                50f,
                                                50f,
                                                50f,
                                                50f,
                                                50f
                                                 });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Warp/Weft", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Isssue Length", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "DU Length", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dyeing Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Transfer Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Receive Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Balance", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);


            #endregion

            #region Data List Wise Loop

            double nQty = 0;
            double nTotalPackQty = 0;
            double nTotalSRSQty = 0;
            double nTotalSRMQty = 0;
            double nBalance = 0;
            List<DyeingOrderFabricDetail> oTempDyeingOrderFabricDetails = new List<DyeingOrderFabricDetail>();
            oTempDyeingOrderFabricDetails=_oDyeingOrderFabricDetails.Where(x => x.DyeingOrderID == oDyeingOrder.DyeingOrderID).OrderBy(x => x.SLNo).ToList();
            foreach (var obj in oTempDyeingOrderFabricDetails)
            {
                 oPdfPTable = new PdfPTable(11);
                oPdfPTable.SetWidths(new float[] { 20f,
                                                120f,
                                                70f,
                                                60f,
                                                50f,
                                                50f,
                                                50f,
                                                50f,
                                                50f,
                                                50f,
                                                50f
                                                 });

                nBalance = 0;
                #region DATA
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.SLNo.ToString(), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ProductName, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ColorName, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.WarpWeftTypeSt, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);


                if (_oFabricExecutionOrderYarnReceives.FirstOrDefault() != null && _oFabricExecutionOrderYarnReceives.FirstOrDefault().DyeingOrderID > 0 && _oFabricExecutionOrderYarnReceives.Where(b => (b.FEOSDID == obj.FEOSDID && b.WarpWeftType == EnumWarpWeft.Warp)).Count() > 0)
                {
                    nQty = _oFabricExecutionOrderYarnReceives.Where(x => x.FEOSDID == obj.FEOSDID && x.WarpWeftType == EnumWarpWeft.Warp).FirstOrDefault().IssuedLength;
                }
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(_oFabricExecutionOrderSpecification.RequiredWarpLength - nQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.QtyDyed), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                nQty = _oRouteSheetPackings.Where(x => x.DyeingOrderDetailID == obj.DyeingOrderDetailID && x.Warp == obj.WarpWeftType).Sum(x => x.Weight);
                nTotalPackQty = nTotalPackQty + nQty;
                nBalance = nQty;
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                nQty = _oFabricExecutionOrderYarnReceives.Where(x => x.FEOSDID == obj.FEOSDID && x.WarpWeftType == obj.WarpWeftType && x.RequisitionType == EnumInOutType.Receive).Sum(x => x.ReceiveQty);
                nTotalSRSQty = nTotalSRSQty + nQty;
                nBalance = nBalance - nQty;
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                nQty = _oFabricExecutionOrderYarnReceives.Where(x => x.FEOSDID == obj.FEOSDID && x.WarpWeftType == obj.WarpWeftType && x.RequisitionType == EnumInOutType.Disburse).Sum(x => x.ReceiveQty);
                nTotalSRMQty = nTotalSRMQty + nQty;
                nBalance = nBalance + nQty;
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nBalance), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

              
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion
            }
            oPdfPTable = new PdfPTable(11);
            oPdfPTable.SetWidths(new float[] { 20f,
                                                120f,
                                                70f,
                                                60f,
                                                50f,
                                                50f,
                                                50f,
                                                50f,
                                                50f,
                                                50f,
                                                50f
                                                 });
            var data = _oDyeingOrderFabricDetails.Where(x => x.DyeingOrderID == oDyeingOrder.DyeingOrderID);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Total", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 4, 10);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(data.Sum(x => x.LengthReq)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(data.Sum(x => x.Length)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(data.Sum(x => x.ConeReq)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(data.Sum(x => x.Cone)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(data.Sum(x => x.QtyDyed)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nTotalPackQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nTotalSRSQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nTotalSRMQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nTotalPackQty - nTotalSRSQty + nTotalSRMQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion
        }

        private void PrintWarpingInfo()
        {
            #region Column Header
            PdfPTable oPdfPTable = GetWarpingInfoTable();
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            string[] Headers = new string[] { "#SL", "Yarn", "Color", "Order Qty", "Warp/Weft", "Lot No", "Receive Date", "Type", "Issue Qty", "Rec'd Qty", "Balance" }; 
            ESimSolPdfHelper.PrintHeaders(ref oPdfPTable, Headers);
            oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
            #endregion

            #region Data List Wise Loop

            _oDyeingOrderFabricDetails = _oDyeingOrderFabricDetails.OrderBy(x => x.SLNo).ToList();
            foreach (var obj in _oDyeingOrderFabricDetails)
            {
                oPdfPTable = GetWarpingInfoTable();
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
                List<FabricExecutionOrderYarnReceive> oTempFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
                oTempFabricExecutionOrderYarnReceives = _oFabricExecutionOrderYarnReceives.Where(x => x.FEOSDID == obj.FEOSDID && x.WarpWeftType == obj.WarpWeftType).ToList();
                int nRowSpan = oTempFabricExecutionOrderYarnReceives.Count();
                if (nRowSpan <= 0) nRowSpan = 1;
                
                #region DATA
                ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.SLNo.ToString(), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ProductName, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ColorName, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.QtyDyed), Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.WarpWeftTypeSt, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);

                if (oTempFabricExecutionOrderYarnReceives.Count > 0)
                {
                    int nCount = 0;
                    foreach (FabricExecutionOrderYarnReceive oItem in oTempFabricExecutionOrderYarnReceives)
                    {
                        //ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.WarpWeftTypeSt, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.LotNo, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.ReceiveDateInStr, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.WYarnTypeStr, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, (oItem.RequisitionType == EnumInOutType.Receive) ? Global.MillionFormat(oItem.ReceiveQty) : "", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, (oItem.RequisitionType == EnumInOutType.Disburse) ? Global.MillionFormat(oItem.ReceiveQty) : "", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                        if (nCount == 0)
                        {
                            double nBalance = (obj.QtyDyed - oTempFabricExecutionOrderYarnReceives.Where(x => x.RequisitionType == EnumInOutType.Receive).Sum(y => y.ReceiveQty)) + oTempFabricExecutionOrderYarnReceives.Where(x => x.RequisitionType == EnumInOutType.Disburse).Sum(y => y.ReceiveQty);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nBalance), Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        }                            
                        oPdfPTable.CompleteRow();
                        nCount++;
                    }
                }
                else
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                    oPdfPTable.CompleteRow();
                }

                #endregion

                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
            }

      

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0, 0, 0, 25);
            #endregion
        }

        #endregion
    }
}
