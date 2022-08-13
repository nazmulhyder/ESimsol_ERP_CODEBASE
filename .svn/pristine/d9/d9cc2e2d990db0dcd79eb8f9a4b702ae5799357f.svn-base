using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class DUOrderTrackerController : Controller
    {
        DUOrderTracker _oDUOrderTracker = new DUOrderTracker();
        List<DUOrderTracker> _oDUOrderTrackers = new List<DUOrderTracker>();
        List<DUOrderRS> _oDUOrderRSs = new List<DUOrderRS>();
        List<DUDeliveryChallanRegister> _DUDeliveryChallanRegisters = new List<DUDeliveryChallanRegister>();
        List<DUReturnChallanDetail> _DUReturnChallanDetails = new List<DUReturnChallanDetail>();
        List<DUReturnChallan> _DUReturnChallans = new List<DUReturnChallan>();
        List<DULotDistribution> _oDULotDistributions = new List<DULotDistribution>();
        DUOrderTracker oDUOrderTracker = new DUOrderTracker();
        DyeingOrder _oDyeingOrder = new DyeingOrder();

        string _sDateRange = "";
        public ActionResult ViewDUOrderTrackers(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDUOrderTrackers = new List<DUOrderTracker>();
           
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PTUStateObj = EnumObject.jGets(typeof(EnumPTUState));
            ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.OrderTypes = oOrderTypes;
            ViewBag.BUID = buid;
            return View(_oDUOrderTrackers);
        }
        public ActionResult ViewDUOrderTrackers_Sample(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDUOrderTrackers = new List<DUOrderTracker>();
            //_oDUOrderTrackers = DUOrderTracker.Gets("", 1, 1, (int)EnumOrderType.None, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PTUStateObj = EnumObject.jGets(typeof(EnumPTUState));
            ViewBag.OrderTypes = DUOrderSetup.GetsActive(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oDUOrderTrackers);
        }
        public ActionResult ViewDUOrderRS(int id, int buid)
        {
            List<DUOrderRS> oDUOrderRSs = new List<DUOrderRS>();
            oDUOrderRSs = DUOrderRS.Gets(id.ToString(), 0, 0, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oDUOrderRSs);
        }
  
        #region Search
        [HttpPost]
        public JsonResult AdvanchSearch(DUOrderTracker oDUOrderTracker)
        {
            _oDUOrderTrackers = new List<DUOrderTracker>();
            try
            {
                string sSQL = MakeSQL(oDUOrderTracker);
                _oDUOrderTrackers = DUOrderTracker.Gets(sSQL, oDUOrderTracker.ReportLevelType, oDUOrderTracker.IsSample, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUOrderTrackers = new List<DUOrderTracker>();
            }
            var jsonResult = Json(_oDUOrderTrackers, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(DUOrderTracker oDUOrderTracker)
        {
            string sParams = oDUOrderTracker.Params;

            int ncboDODate = 0;
            DateTime dFromDODate = DateTime.Today;
            DateTime dToDODate = DateTime.Today;
            int cboPIDate = 0;
            DateTime dFromPIDate = DateTime.Today;
            DateTime dToPIDate = DateTime.Today;
        
            int nOrderType = 0;
            string sPINo = "";
            string sOrderNo = "";
            int nBUID = 0;
            int nProStatus = 0;

            if (!string.IsNullOrEmpty(sParams))
            {
             
                ncboDODate = Convert.ToInt32(sParams.Split('~')[0]);
                if (ncboDODate > 0)
                {
                    dFromDODate = Convert.ToDateTime(sParams.Split('~')[1]);
                    dToDODate = Convert.ToDateTime(sParams.Split('~')[2]);
                }
                cboPIDate = Convert.ToInt32(sParams.Split('~')[3]);
                if (cboPIDate > 0)
                {
                    dFromPIDate = Convert.ToDateTime(sParams.Split('~')[4]);
                    dToPIDate = Convert.ToDateTime(sParams.Split('~')[5]);
                }
                _oDUOrderTracker.ContractorName = Convert.ToString(sParams.Split('~')[6]);
                _oDUOrderTracker.ProductName = Convert.ToString(sParams.Split('~')[7]);
                nOrderType = Convert.ToInt32(sParams.Split('~')[8]);
                sPINo = Convert.ToString(sParams.Split('~')[9]);
                sOrderNo = Convert.ToString(sParams.Split('~')[10]);
               
                if (sParams.Split('~').Length > 11)
                    Int32.TryParse(sParams.Split('~')[11], out nProStatus);

                if (sParams.Split('~').Length > 12)
                    Int32.TryParse(sParams.Split('~')[12], out nBUID);
            }


            string sReturn1 = "";
            string sReturn = "";

            Global.TagSQL(ref sReturn);
            if (oDUOrderTracker.IsSample)
            {
                sReturn = sReturn + "DyeingOrderType not in (" + (int)EnumOrderType.LoanOrder + "," + (int)EnumOrderType.BulkOrder + "," + (int)EnumOrderType.DyeingOnly + ")";
            }
            else
            {
                sReturn = sReturn + "DyeingOrderType in (" + (int)EnumOrderType.BulkOrder + "," + (int)EnumOrderType.DyeingOnly + "," + (int)EnumOrderType.ClaimOrder + ")";
            }
            #region Order Type
            if (nOrderType>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DyeingOrderType in (" + nOrderType + ")";
            }
            #endregion

            #region Contractor
            if (!String.IsNullOrEmpty(_oDUOrderTracker.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID in(" + _oDUOrderTracker.ContractorName + ")";
            }
            #endregion
          
            #region Product Id
            if (!String.IsNullOrEmpty(_oDUOrderTracker.ProductName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID in(" + _oDUOrderTracker.ProductName + ")";
            }
            #endregion

            #region OrderDate Date
            if (ncboDODate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (ncboDODate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromDODate.ToString("dd MMM yyyy") + "'',106)) ";
                    _sDateRange = "Date: " + dFromDODate.ToString("dd MMM yyyy");
                }
                else if (ncboDODate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromDODate.ToString("dd MMM yyyy") + "'',106)) ";
                    _sDateRange = "Date: NotEqualTo->" + dFromDODate.ToString("dd MMM yyyy");
                }
                else if (ncboDODate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromDODate.ToString("dd MMM yyyy") + "'',106)) ";
                    _sDateRange = "Date: GreaterThen->" + dFromDODate.ToString("dd MMM yyyy");
                }
                else if (ncboDODate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromDODate.ToString("dd MMM yyyy") + "'',106)) ";
                    _sDateRange = "Date: SmallerThen->" + dFromDODate.ToString("dd MMM yyyy");
                }
                else if (ncboDODate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromDODate.ToString("dd MMM yyyy") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dToDODate.ToString("dd MMM yyyy") + "'',106)) ";
                    _sDateRange = "Date: From " + dFromDODate.ToString("dd MMM yyyy") + " To " + dToDODate.ToString("dd MMM yyyy");
                }
                else if (ncboDODate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromDODate.ToString("dd MMM yyyy") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dToDODate.ToString("dd MMM yyyy") + "'',106)) ";
                    _sDateRange = "Date: NotBetween " + dFromDODate.ToString("dd MMM yyyy") + " To " + dToDODate.ToString("dd MMM yyyy");
                }
            }
            #endregion
            #region PINo No
            if (!string.IsNullOrEmpty(sPINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ExportPIID in (Select ExportPIID from ExportPI where PINo like ''%" + sPINo + "%'')";
            }
            #endregion
            #region BPO No
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  OrderNo like''%" + sOrderNo + "%''";
            }
            #endregion
            #region Business Unit
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  DyeingOrderType in (Select OrderType from DUOrderSetup where BUID=" + nBUID + ")";
            }
            #endregion
            #region Status
            if (nProStatus > 0)
            {
                Global.TagSQL(ref sReturn);
                if (nProStatus==1)// WIP
                {
                    sReturn = sReturn + " DyeingOrderDetailID in (Select RouteSheetDO.DyeingOrderDetailID from RouteSheetDO where IsOut=1 and isnull(RSState,0)<>13)";
                }
                if (nProStatus == 2)// Stock in Hand
                {
                    sReturn = sReturn + " DyeingOrderDetailID in (Select DODID from DULotDistribution where Qty>0.2)";
                }
                if (nProStatus ==3)// Claim Aganist Order
                {
                    sReturn = sReturn + "DyeingOrderDetailID in (Select DUClaimOrderDetail.ParentDODetailID from DUClaimOrderDetail)";
                }
                if (nProStatus == 4)// Pending Delivery
                {
                    sReturn = sReturn + "DyeingOrderDetailID in ( Select DyeingOrderDetailID from ( Select DyeingOrderDetailID, Qty, isnull((Select SUM(Isnull(RSD.QtyDC,0)-Isnull(RSD.QtyRC,0)) from RouteSheetDO as RSD where RSD.DyeingOrderDetailID=DyeingOrderDetail.DyeingOrderDetailID),0) as QTYDC from DyeingOrderDetail) as HH where HH.Qty>isnull(HH.QTYDC,0))";
                }
            }
            #endregion

            string sSQL = sReturn1 + "where DyeingOrderID in (select DyeingOrderID from DyeingOrder " + sReturn+")";
            return sSQL;
        }
        #endregion
        
        [HttpPost]
        public JsonResult GetsPODetails(DUOrderTracker oDUOrderTracker)
        {
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<DyeingOrderDetail> oDyeingOrderDetails_Temp = new List<DyeingOrderDetail>();
            try
            {
                //if (oDUOrderTracker.OrderType == (int)EnumOrderType.BulkOrder)
                //{
                //    oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where ExportPIID in (Select ExportPIID from ExportSC where ExportSCID=" + oDUOrderTracker.OrderID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                //    oDyeingOrderDetails_Temp = DyeingOrderDetail.Gets("SELECT * FROM View_DyeingOrderDetail where isnull([Status],0)<>4 and PTUID=" + oDUOrderTracker.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //}
                //if (oDUOrderTracker.OrderType == (int)EnumOrderType.SampleOrder)
                //{
                    oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where DyeingOrderID=" + oDUOrderTracker.DyeingOrderID + "", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDyeingOrderDetails_Temp = DyeingOrderDetail.Gets("SELECT * FROM View_DyeingOrderDetail where isnull([Status],0)<>4 and DyeingOrderID=" + oDUOrderTracker.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
                    foreach (DyeingOrder oItem in oDyeingOrders)
                    {
                        List<DyeingOrderDetail> oDODetails = new List<DyeingOrderDetail>();
                        oDODetails = oDyeingOrderDetails_Temp.Where(o => o.DyeingOrderID == oItem.DyeingOrderID).ToList();
                        foreach (DyeingOrderDetail oItemDOD in oDODetails)
                        {
                            oItemDOD.OrderNo = oItem.OrderNoFull;
                            //oItemDOD.OrderDate = oItem.OrderDateSt;
                            oDyeingOrderDetails.Add(oItemDOD);
                        }
                    }
                
              
            }
            catch (Exception ex)
            {
                oDyeingOrderDetails = new List<DyeingOrderDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsRSs(DUOrderTracker oDUOrderTracker)
        {
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            try
            {
                string sSQL = "Select * from View_RouteSheet Where PTUID =" + oDUOrderTracker.PTUID;
                oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheets = new List<RouteSheet>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsLots(DUOrderTracker oDUOrderTracker)
        {
            List<Lot> oLots = new List<Lot>();
            try
            {
                oLots = Lot.Gets("Select * from View_Lot where Balance>0.2 and parentType=106 and ParentID in (Select RouteSheetID from RouteSheet where PTUID=" + oDUOrderTracker.PTUID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        #region Print XL
        public void PrintRpt_Excel_Bulk(string sTempString, int nReportLevel, bool bIsSample)
        {
            string sMunit = "";
            _oDUOrderTracker.Params = sTempString;
            string sSQL = MakeSQL(_oDUOrderTracker);
            _oDUOrderTrackers = DUOrderTracker.Gets(sSQL, nReportLevel, bIsSample, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oDUOrderTrackers.Count > 0)
            {
                sMunit = _oDUOrderTrackers[0].MUName;
                if(String.IsNullOrEmpty(sMunit))
                {
                    sMunit = "Lbs";
                }
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nQty_PI = 0;
            double nOrderQty = 0;
            double nPro_Issue = 0;
            double nProductionPipeLineQty = 0;
            double nProductionFinishedQty = 0;
            double nYetToProduction = 0;
            double nActualDeliveryQty = 0;
            double nYetToDelivery = 0;
            double nReturnQty = 0;
            double nReOrderQty = 0;
            double nStockInHand = 0;
            double nStockInAval = 0;


            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 17;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Production Status(Bulk)");
                sheet.Name = "Production Status(Bulk)";
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;
                sheet.Column(10).Width = 20;
                sheet.Column(11).Width = 20;
                sheet.Column(12).Width = 20;
                sheet.Column(13).Width = 20;
                sheet.Column(14).Width = 20;
                sheet.Column(15).Width = 20;
                sheet.Column(16).Width = 20;
                sheet.Column(17).Width = 30;
                sheet.Column(18).Width = 30;
                sheet.Column(19).Width = 30;
                sheet.Column(20).Width = 30;
                //   nEndCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Production Status(Bulk)   Date: " + DateTime.Today.ToString("dd MMM yyyy hh:mm:tt") + ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region

                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Order Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "P/I No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Color No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "P/I Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Dyeing Order Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Dyeing Card Issue(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "WIP Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Pro Finished(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Pending Production(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = "Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Pending Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 17]; cell.Value = "Stock in Hand(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Yet To Delivery(" + sMunit + ")"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

             

                cell = sheet.Cells[nRowIndex, 18]; cell.Value = "Claim Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 19]; cell.Value = "Return Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 20]; cell.Value = "Concern Person(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 21]; cell.Value = "Stock in Hand(Exces Dyeing) (" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

           
               

                nRowIndex++;
                #endregion

                #region Data
                foreach (DUOrderTracker oItem in _oDUOrderTrackers)
                {


                    //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.OrderNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "" + oItem.OrderDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "" + oItem.PINo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ContractorName.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ColorNameShade; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.ColorNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.Qty_PI; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.Qty_ProIssue; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.Pro_PipeLineQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.ProductionFinishedQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.YetToProduction; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.ActualDeliveryQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = oItem.YetToDelivery; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = oItem.StockInHand; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 18]; cell.Value = oItem.ClaimOrderQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 19]; cell.Value = oItem.ReturnQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

               

                    cell = sheet.Cells[nRowIndex, 20]; cell.Value = oItem.MKT; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 21]; cell.Value = oItem.StockInAval; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nQty_PI = nQty_PI + oItem.Qty_PI;
                    nOrderQty = nOrderQty + oItem.OrderQty;
                    nPro_Issue = nPro_Issue + oItem.Qty_ProIssue;
                    nProductionPipeLineQty = nProductionPipeLineQty + oItem.Pro_PipeLineQty;
                    nProductionFinishedQty = nProductionFinishedQty + oItem.ProductionFinishedQty;
                    nYetToProduction = nYetToProduction + oItem.YetToProduction;
                    nActualDeliveryQty = nActualDeliveryQty + oItem.ActualDeliveryQty;
                    nReturnQty = nReturnQty + oItem.ReturnQty;
                    nYetToDelivery = nYetToDelivery + oItem.YetToDelivery;
                    nReOrderQty = nReOrderQty + oItem.ReturnQty;
                    nStockInHand = nStockInHand + oItem.StockInHand;
                    nStockInAval = nStockInAval + oItem.StockInAval;

                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "" + ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "" + ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "" + ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = nQty_PI; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = nOrderQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = nPro_Issue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = nProductionPipeLineQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = nProductionFinishedQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = nYetToProduction; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = nActualDeliveryQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 16]; cell.Value = nYetToDelivery; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 17]; cell.Value = nStockInHand; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            

                cell = sheet.Cells[nRowIndex, 18]; cell.Value = nReOrderQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 19]; cell.Value = nReturnQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

             

                cell = sheet.Cells[nRowIndex, 20]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 21]; cell.Value = nStockInAval; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

               

                nRowIndex = nRowIndex + 1;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ProductionStatus(Bulk).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


        }

        public void PrintRpt_Excel_Sample(string sTempString, int nReportLevel, bool bIsSample)
        {

            string sMunit = "";
            _oDUOrderTracker.Params = sTempString;
            _oDUOrderTracker.IsSample = bIsSample;
            string sSQL = MakeSQL(_oDUOrderTracker);
            _oDUOrderTrackers = DUOrderTracker.Gets(sSQL, nReportLevel, bIsSample, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oDUOrderTrackers.Count > 0)
            {
                sMunit = _oDUOrderTrackers[0].MUName;
                if (String.IsNullOrEmpty(sMunit))
                {
                    sMunit = "Lbs";
                }
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nOrderQty = 0;
            double nProductionPipeLineQty = 0;
            double nQty_ProIssue = 0;
            double nProductionFinishedQty = 0;
            double nYetToProduction = 0;
            double nActualDeliveryQty = 0;
            double nYetToDelivery = 0;
            double nReturnQty = 0;
            double nReOrderQty = 0;
            double nStockInHand = 0;
            double nStockInAval = 0;


            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 17;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Production Status(Sample)");
                sheet.Name = "Production Status(Sample)";
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;
                sheet.Column(10).Width = 20;
                sheet.Column(11).Width = 20;
                sheet.Column(12).Width = 20;
                sheet.Column(13).Width = 20;
                sheet.Column(14).Width = 20;
                sheet.Column(15).Width = 20;
                sheet.Column(16).Width = 20;
                sheet.Column(17).Width = 30;
                sheet.Column(18).Width = 30;
                //   nEndCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data
                #region Date Print
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Production Status(Sample Order) "; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value =  _sDateRange; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

              

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region

                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Sample Order No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Order Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Color No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Labdip No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
             

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Order Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Dyeing Card Issue(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "WIP Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Pro Finished(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Pending Pro.(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = "Stock in Hand(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Concern Person(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 17]; cell.Value = "Pending Delivery(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 18]; cell.Value = "Claim Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 19]; cell.Value = "Return Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 20]; cell.Value = "Stock in Hand Avl(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

              


                nRowIndex++;
                #endregion

                #region Data
                foreach (DUOrderTracker oItem in _oDUOrderTrackers)
                {


                    //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = oItem.OrderNo.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.OrderDateSt.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ContractorName.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.ColorNameShade; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ColorNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.LabdipNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.Qty_ProIssue; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.Pro_PipeLineQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.ProductionFinishedQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.YetToProduction; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.ActualDeliveryQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.StockInHand; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = oItem.MKT; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = (oItem.OrderQty-oItem.ActualDeliveryQty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                     


                    cell = sheet.Cells[nRowIndex, 18]; cell.Value = oItem.ClaimOrderQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 19]; cell.Value = oItem.ReturnQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                 

                    cell = sheet.Cells[nRowIndex, 20]; cell.Value = oItem.StockInAval; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                  



                    nOrderQty = nOrderQty + oItem.OrderQty;
                    nQty_ProIssue = nQty_ProIssue + oItem.Qty_ProIssue;
                    nProductionPipeLineQty = nProductionPipeLineQty + oItem.Pro_PipeLineQty;
                    nProductionFinishedQty = nProductionFinishedQty + oItem.ProductionFinishedQty;
                    nYetToProduction = nYetToProduction + oItem.YetToProduction;
                    nActualDeliveryQty = nActualDeliveryQty + oItem.ActualDeliveryQty;
                    nReturnQty = nReturnQty + oItem.ReturnQty;
                    nYetToDelivery = nYetToDelivery + oItem.YetToDelivery;
                    nReOrderQty = nReOrderQty + oItem.ReturnQty;
                    nStockInHand = nStockInHand + oItem.StockInHand;
                    nStockInAval = nStockInAval + oItem.StockInAval;
                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "" + ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "" + ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = nOrderQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = nQty_ProIssue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = nProductionPipeLineQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = nProductionFinishedQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = nYetToProduction; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = nActualDeliveryQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value =nStockInHand ; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 16]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            

                cell = sheet.Cells[nRowIndex, 17]; cell.Value =nYetToDelivery ; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 18]; cell.Value = nReOrderQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 19]; cell.Value = nReturnQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 20]; cell.Value = nStockInAval; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

           



                nRowIndex = nRowIndex + 1;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ProductionStatus(Bulk).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


        }

        #endregion

        #region Routesheet & RS QC Report
        public ActionResult ViewRSReport(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDUOrderRSs = new List<DUOrderRS>();
            
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PTUStateObj = EnumObject.jGets(typeof(EnumPTUState));
            ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.OrderTypes = oOrderTypes;
            ViewBag.BUID = buid;
            return View(_oDUOrderRSs);
        }
        public ActionResult ViewRSQCReport(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDUOrderRSs = new List<DUOrderRS>();

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PTUStateObj = EnumObject.jGets(typeof(EnumPTUState));
            ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.OrderTypes = oOrderTypes;
            ViewBag.BUID = buid;
            return View(_oDUOrderRSs);
        }

        [HttpPost]
        public JsonResult GetsLot(Lot oLot)
        {
            List<Lot> oLots = new List<Lot>();
            try
            {
                oLot.LotNo = (!string.IsNullOrEmpty(oLot.LotNo)) ? oLot.LotNo.Trim() : "";

                string sSQL = "Select * from View_Lot Where LotID<>0 ";

                if (!string.IsNullOrEmpty(oLot.LotNo))
                    sSQL = sSQL + " And LotNo Like '%" + oLot.LotNo + "%'"; 
                if (!string.IsNullOrEmpty(oLot.ProductName)) // --- Assigned With Product ID
                    sSQL = sSQL + " And ProductID IN (" + oLot.ProductName + ")";
                if (oLot.BUID > 0)
                    sSQL = sSQL + " And BUID=" + oLot.BUID;

                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message; oLots.Add(oLot);
            }

            var jsonResult = Json(oLots, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AdvanchSearchRS(DUOrderRS oDUOrderRS)
        {
            _oDUOrderRSs = new List<DUOrderRS>();
            try
            {
                string sSQL = MakeSQL_RS(oDUOrderRS);
                _oDUOrderRSs = DUOrderRS.Gets(sSQL, oDUOrderRS.ReportLevelType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUOrderRSs = new List<DUOrderRS>();
            }
            var jsonResult = Json(_oDUOrderRSs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AdvanchSearchRSQC(DUOrderRS oDUOrderRS)
        {
            _oDUOrderRSs = new List<DUOrderRS>();
            try
            { 
                int cboDate = 0;
                if(oDUOrderRS.ErrorMessage !="")
                {
                    cboDate = Convert.ToInt32(oDUOrderRS.ErrorMessage.Split('~')[0]);
                }
                if(cboDate>0)
                {
                    string sSQL = MakeSQL_RSQC(oDUOrderRS);
                    _oDUOrderRSs = DUOrderRS.GetsQC(sSQL, oDUOrderRS.ReportLevelType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    throw new Exception("Please Select The QC Date");
                }
                
            }
            catch (Exception ex)
            {
                _oDUOrderRSs = new List<DUOrderRS>();
                _oDUOrderRSs.Add(new DUOrderRS() { ErrorMessage = ex.Message });
            }
            var jsonResult = Json(_oDUOrderRSs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL_RS(DUOrderRS oDUOrderRS)
        {
            string sParams = oDUOrderRS.ErrorMessage;

            int ncboRSDate = 0;
            DateTime dFromRSDate = DateTime.Today;
            DateTime dToRSDate = DateTime.Today;
            int nCboQCDate = 0;
            DateTime dFromQCDate = DateTime.Today;
            DateTime dToQCDate = DateTime.Today;

            int nOrderType = 0;
            string sRSNo = "";
            string sOrderIDs = "";
            int nBUID = 0;

            if (!string.IsNullOrEmpty(sParams))
            {

                ncboRSDate = Convert.ToInt32(sParams.Split('~')[0]);
                if (ncboRSDate > 0)
                {
                    dFromRSDate = Convert.ToDateTime(sParams.Split('~')[1]);
                    dToRSDate = Convert.ToDateTime(sParams.Split('~')[2]);
                }
                nCboQCDate = Convert.ToInt32(sParams.Split('~')[3]);
                if (nCboQCDate > 0)
                {
                    dFromQCDate = Convert.ToDateTime(sParams.Split('~')[4]);
                    dToQCDate = Convert.ToDateTime(sParams.Split('~')[5]);
                }
                oDUOrderRS.ProductName = Convert.ToString(sParams.Split('~')[6]);
                nOrderType = Convert.ToInt32(sParams.Split('~')[7]);
                sOrderIDs = Convert.ToString(sParams.Split('~')[8]);
                sRSNo = Convert.ToString(sParams.Split('~')[9]);
            }

            string sReturn1 = "";
            string sReturn = "";

            #region RS No
            if (!string.IsNullOrEmpty(sRSNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RouteSheetNo like ''%" + sRSNo + "%''";
            }
            #endregion
            #region RouteSheetDate Date
            if (ncboRSDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (ncboRSDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromRSDate.ToString("dd MMM yyyy") + "'',106)) ";
                    _sDateRange = "Date: " + dFromRSDate.ToString("dd MMM yyyy");
                }
                else if (ncboRSDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromRSDate.ToString("dd MMM yyyy") + "'',106)) ";
                    _sDateRange = "Date: NotEqualTo->" + dFromRSDate.ToString("dd MMM yyyy");
                }
                else if (ncboRSDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromRSDate.ToString("dd MMM yyyy") + "'',106)) ";
                    _sDateRange = "Date: GreaterThen->" + dFromRSDate.ToString("dd MMM yyyy");
                }
                else if (ncboRSDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromRSDate.ToString("dd MMM yyyy") + "'',106)) ";
                    _sDateRange = "Date: SmallerThen->" + dFromRSDate.ToString("dd MMM yyyy");
                }
                else if (ncboRSDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromRSDate.ToString("dd MMM yyyy") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dToRSDate.ToString("dd MMM yyyy") + "'',106)) ";
                    _sDateRange = "Date: From " + dFromRSDate.ToString("dd MMM yyyy") + " To " + dToRSDate.ToString("dd MMM yyyy");
                }
                else if (ncboRSDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromRSDate.ToString("dd MMM yyyy") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dToRSDate.ToString("dd MMM yyyy") + "'',106)) ";
                    _sDateRange = "Date: NotBetween " + dFromRSDate.ToString("dd MMM yyyy") + " To " + dToRSDate.ToString("dd MMM yyyy");
                }
            }
            #endregion
            #region QC Date
            if (nCboQCDate != (int)EnumCompareOperator.None)
            {
                string stemp = "RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus=13 and";

                Global.TagSQL(ref sReturn);
                if (nCboQCDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + stemp + " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromQCDate.ToString("dd MMM yyyy") + "'',106)) )";
                }
                else if (nCboQCDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + stemp + " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromQCDate.ToString("dd MMM yyyy") + "'',106)) ) ";
                }
                else if (nCboQCDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + stemp + " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromQCDate.ToString("dd MMM yyyy") + "'',106))) ";
                }
                else if (nCboQCDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + stemp + " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromQCDate.ToString("dd MMM yyyy") + "'',106)) )";
                }
                else if (nCboQCDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + stemp + " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromQCDate.ToString("dd MMM yyyy") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dToQCDate.ToString("dd MMM yyyy") + "'',106))) ";
                }
                else if (nCboQCDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + stemp + " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromQCDate.ToString("dd MMM yyyy") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dToQCDate.ToString("dd MMM yyyy") + "'',106)) )";
                }
            }
            #endregion
            #region Product Id
            if (!String.IsNullOrEmpty(oDUOrderRS.ProductName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID_Raw in(" + oDUOrderRS.ProductName + ")";
            }
            #endregion
            #region Contractor
            if (!String.IsNullOrEmpty(oDUOrderRS.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID in(" + oDUOrderRS.ContractorName + ")";
            }
            #endregion

            #region Order Type No
            if (nOrderType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  OrderType In (" + nOrderType + ")";
            }
            #endregion
         
            #region BPO No
            if (!string.IsNullOrEmpty(sOrderIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RouteSheetID in (select RouteSheetID from RouteSheetDO where  DyeingOrderDetailID in (" + sOrderIDs + "))";
                //if (sOrderNo != "") sSQL = sSQL + " And RouteSheetID in (select RouteSheetID from [View_RouteSheetDO] where  OrderNo like '" + sOrderNo + "%')";
            }
            #endregion
            #region Business Unit
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPI.BUID = " + nBUID;
            }
            #endregion
            string sSQL = sReturn1 + "" + sReturn + "";
            return sSQL;
        }
        private string MakeSQL_RSQC(DUOrderRS oDUOrderRS)
        {
            string sParams = oDUOrderRS.ErrorMessage;

            int ncboRSDate = 0;
            DateTime dFromRSDate = DateTime.Today;
            DateTime dToRSDate = DateTime.Today;
            int nCboQCDate = 0;
            DateTime dFromQCDate = DateTime.Today;
            DateTime dToQCDate = DateTime.Today;

            int nOrderType = 0;
            string sRSNo = "";
            string sRSLotNo = "";
            string sOrderIDs = "";
            int nBUID = 0;
            int nHanksCone = 0;

            if (!string.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                nCboQCDate = Convert.ToInt32(sParams.Split('~')[nCount++]);
                if (nCboQCDate > 0)
                {
                    dFromQCDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                    dToQCDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                }
                nCount = 3;
                oDUOrderRS.ProductName = Convert.ToString(sParams.Split('~')[nCount++]);
                nOrderType = Convert.ToInt32(sParams.Split('~')[nCount++]);
                sOrderIDs = Convert.ToString(sParams.Split('~')[nCount++]);
                nHanksCone = Convert.ToInt32(sParams.Split('~')[nCount++]);

                sRSNo = Convert.ToString(sParams.Split('~')[nCount++]);
                sRSLotNo = Convert.ToString(sParams.Split('~')[nCount++]);
            }

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            dFromRSDate = new DateTime(dFromRSDate.Year, dFromRSDate.Month, dFromRSDate.Day, oRouteSheetSetup.BatchTime.Hour, oRouteSheetSetup.BatchTime.Minute, 0);
            dToRSDate = new DateTime(dToRSDate.Year, dToRSDate.Month, dToRSDate.Day, oRouteSheetSetup.BatchTime.Hour, oRouteSheetSetup.BatchTime.Minute, 0);
            dFromQCDate = new DateTime(dFromQCDate.Year, dFromQCDate.Month, dFromQCDate.Day, oRouteSheetSetup.BatchTime.Hour, oRouteSheetSetup.BatchTime.Minute, 0);
            dToQCDate = new DateTime(dToQCDate.Year, dToQCDate.Month, dToQCDate.Day, oRouteSheetSetup.BatchTime.Hour, oRouteSheetSetup.BatchTime.Minute, 0);

            string sReturn1 = "";
            string sReturn = "";

            #region RS No
            if (!string.IsNullOrEmpty(sRSNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  RouteSheetID IN (SELECT RouteSheetID FROM RouteSheet WHERE RouteSheetNo like ''%" + sRSNo + "%'' )";
            }
            #endregion

            #region Hanks Cone
            if (nHanksCone>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  RouteSheetID IN (SELECT RouteSheetID FROM RouteSheet WHERE HanksCone =" + (nHanksCone-1) + ")";
            }
            #endregion

            #region RS Lot
            if (!string.IsNullOrEmpty(sRSLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  RouteSheetID IN (SELECT RouteSheetID FROM RouteSheet WHERE LotID IN (" + sRSLotNo + "))";
            }
            #endregion

            #region RouteSheetDate Date
            if (ncboRSDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (ncboRSDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " RouteSheetID IN (SELECT RouteSheetID FROM RouteSheet WHERE CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromRSDate.ToString("dd MMM yyyy") + "'',106)) )";
                    _sDateRange = "Date: " + dFromRSDate.ToString("dd MMM yyyy");
                }
                else if (ncboRSDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " RouteSheetID IN (SELECT RouteSheetID FROM RouteSheet WHERE CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromRSDate.ToString("dd MMM yyyy") + "'',106)) )";
                    _sDateRange = "Date: NotEqualTo->" + dFromRSDate.ToString("dd MMM yyyy");
                }
                else if (ncboRSDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " RouteSheetID IN (SELECT RouteSheetID FROM RouteSheet WHERE CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromRSDate.ToString("dd MMM yyyy") + "'',106)) )";
                    _sDateRange = "Date: GreaterThen->" + dFromRSDate.ToString("dd MMM yyyy");
                }
                else if (ncboRSDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " RouteSheetID IN (SELECT RouteSheetID FROM RouteSheet WHERE CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromRSDate.ToString("dd MMM yyyy") + "'',106)) )";
                    _sDateRange = "Date: SmallerThen->" + dFromRSDate.ToString("dd MMM yyyy");
                }
                else if (ncboRSDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " RouteSheetID IN (SELECT RouteSheetID FROM RouteSheet WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromRSDate.ToString("dd MMM yyyy") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dToRSDate.ToString("dd MMM yyyy") + "'',106)) )";
                    _sDateRange = "Date: From " + dFromRSDate.ToString("dd MMM yyyy") + " To " + dToRSDate.ToString("dd MMM yyyy");
                }
                else if (ncboRSDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " RouteSheetID IN (SELECT RouteSheetID FROM RouteSheet WHERE CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromRSDate.ToString("dd MMM yyyy") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dToRSDate.ToString("dd MMM yyyy") + "'',106)) )";
                    _sDateRange = "Date: NotBetween " + dFromRSDate.ToString("dd MMM yyyy") + " To " + dToRSDate.ToString("dd MMM yyyy");
                }
            }
            #endregion
            #region QC Date
            if (nCboQCDate != (int)EnumCompareOperator.None)
            {
                string stemp = "RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus=13 and";

                Global.TagSQL(ref sReturn);
                if (nCboQCDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + stemp + " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromQCDate.ToString("dd MMM yyyy HH:mm") + "'',106)) )";
                    _sDateRange = "Date: " + dFromRSDate.ToString("dd MMM yyyy");
                }
                else if (nCboQCDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + stemp + " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromQCDate.ToString("dd MMM yyyy HH:mm") + "'',106)) ) ";
                    _sDateRange = "Date (NotEqualTo): " + dFromRSDate.ToString("dd MMM yyyy");
                }
                else if (nCboQCDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + stemp + " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromQCDate.ToString("dd MMM yyyy HH:mm") + "'',106))) ";
                    _sDateRange = "Date (GreaterThan): " + dFromRSDate.ToString("dd MMM yyyy");
                }
                else if (nCboQCDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + stemp + " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromQCDate.ToString("dd MMM yyyy HH:mm") + "'',106)) )";
                    _sDateRange = "Date (SmallerThan): " + dFromRSDate.ToString("dd MMM yyyy");
                }
                else if (nCboQCDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + stemp + " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromQCDate.ToString("dd MMM yyyy HH:mm") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dToQCDate.ToString("dd MMM yyyy HH:mm") + "'',106))) ";
                    _sDateRange = "Date: " + dFromRSDate.ToString("dd MMM yyyy") + " To " + dToRSDate.ToString("dd MMM yyyy");
                }
                else if (nCboQCDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + stemp + " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromQCDate.ToString("dd MMM yyyy HH:mm") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dToQCDate.ToString("dd MMM yyyy HH:mm") + "'',106)) )";
                    _sDateRange = "Date (NotBetween): " + dFromRSDate.ToString("dd MMM yyyy") + " To " + dToRSDate.ToString("dd MMM yyyy");
                }
            }
            #endregion
            #region Product Id
            if (!String.IsNullOrEmpty(oDUOrderRS.ProductName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RouteSheetID IN (SELECT RS.RouteSheetID FROM RouteSheet RS WHERE RS.ProductID_Raw IN (" + oDUOrderRS.ProductName + "))";
            }
            #endregion
            #region Contractor
            if (!String.IsNullOrEmpty(oDUOrderRS.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RouteSheetID IN (SELECT RS.RouteSheetID FROM RouteSheet RS WHERE RS.ContractorID in(" + oDUOrderRS.ContractorName + "))";
            }
            #endregion

            #region Order Type No
            if (nOrderType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RouteSheetID IN (SELECT RS.RouteSheetID FROM View_RouteSheet RS WHERE RS.OrderType In (" + nOrderType + "))";
            }
            #endregion

            #region BPO No
            if (!string.IsNullOrEmpty(sOrderIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderDetailID in (" + sOrderIDs + ")";
                //if (sOrderNo != "") sSQL = sSQL + " And RouteSheetID in (select RouteSheetID from [View_RouteSheetDO] where  OrderNo like '" + sOrderNo + "%')";
            }
            #endregion
            #region Business Unit
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  RouteSheetID IN (SELECT RS.RouteSheetID FROM RouteSheet RS WHERE RS.BUID = " + nBUID + ")";
            }
            #endregion
            string sSQL = sReturn1 + "" + sReturn + "";
            return sSQL;
        }

        #region PDF & Export To Excel (Date & Shift Wise)

        public ActionResult Print_DateWise(string sParams, int buid, double ts)
        {
            string Header = "", sErrorMesage = ""; int nReportType = 0;

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUOrderRS oDUOrderRS = new DUOrderRS();
            oDUOrderRS.ErrorMessage = sParams;
            try
            {
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                _oDUOrderRSs = new List<DUOrderRS>();
                string sSQL = MakeSQL_RSQC(oDUOrderRS);
                _oDUOrderRSs = DUOrderRS.GetsQC(sSQL, oDUOrderRS.ReportLevelType, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oDUOrderRSs.Count <= 0)
                {
                    sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oDUOrderRSs = new List<DUOrderRS>();
                sErrorMesage = ex.Message;
            }
             
            if (!string.IsNullOrEmpty(sErrorMesage))
            {
                string sMessage = "There is no data for print";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }
            else
            {
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

                _oDUOrderRSs = _oDUOrderRSs.OrderBy(x => x.QCDate).ToList();
                
                if ( !string.IsNullOrEmpty(sParams.Split('~')[3]) && sParams.Split('~')[3].Split(',').Length==1)      { Header+= _oDUOrderRSs.Select(x=>x.ProductName).FirstOrDefault(); }
                if (Convert.ToInt16(sParams.Split('~')[6]) > 0) { _sDateRange += ("  " + ( (Convert.ToInt16(sParams.Split('~')[6]) == 1) ? "Hanks" : "Cone") ); }
                if ( !string.IsNullOrEmpty(sParams.Split('~')[3]) && sParams.Split('~')[8].Split(',').Length == 1)   { Header +=  "  "+_oDUOrderRSs.Select(x => x.LotNo).FirstOrDefault(); }

                Header = _sDateRange + (string.IsNullOrEmpty(_sDateRange) ? "" : Environment.NewLine) + Header;
                Header = "QC Report" + (string.IsNullOrEmpty(Header) ? "" : Environment.NewLine) + Header;

                rptRouteSheetQCReport oReport = new rptRouteSheetQCReport();
                byte[] abytes = oReport.PrepareReport(_oDUOrderRSs, oBusinessUnit, oCompany, Header);
                return File(abytes, "application/pdf");
            }
        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public void ExportToExcel_DateWise(string sParams, int buid, double ts)
        {
            string Header = "", sErrorMesage=""; int nReportType = 0;

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUOrderRS oDUOrderRS = new DUOrderRS();
            oDUOrderRS.ErrorMessage = sParams;
            try
            {
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDUOrderRSs = new List<DUOrderRS>();
                string sSQL = MakeSQL_RSQC(oDUOrderRS);
                _oDUOrderRSs = DUOrderRS.GetsQC(sSQL, oDUOrderRS.ReportLevelType, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oDUOrderRSs.Count <= 0)
                {
                    sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oDUOrderRSs = new List<DUOrderRS>();
                sErrorMesage = ex.Message;
            }

            if (sErrorMesage == "")
            {
                _oDUOrderRSs = _oDUOrderRSs.OrderBy(x => x.QCDate).ToList();
                var dataGrpList = _oDUOrderRSs.GroupBy(x => new { QCDateSt = x.QCDateSt, ShiftName = x.ShiftName }, (key, grp) => new
                {
                    HeaderName = key.QCDateSt + " [" + key.ShiftName + "]",
                    Results = grp.OrderBy(x => x.ProductName).ThenBy(x => x.ProductID).ToList()
                });

                Header = "Day Basis Report (Shift Wise)";

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = 0;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Day_Basis_Report_Shift_Wise");
                    sheet.Name = "Day_Basis_Report_Shift_Wise";

                    #region Col Def
                    int nCount_Column = 2;
                    sheet.Column(nCount_Column++).Width = 10; //SL
                    sheet.Column(nCount_Column++).Width = 40; //Product

                    sheet.Column(nCount_Column++).Width = 20; //LotNo
                    sheet.Column(nCount_Column++).Width = 20; //Order
                    sheet.Column(nCount_Column++).Width = 20; //OrderQty
                    
                    sheet.Column(nCount_Column++).Width = 30; //Buyer
                    sheet.Column(nCount_Column++).Width = 20; //Color
                    sheet.Column(nCount_Column++).Width = 20; //RawLot
                    sheet.Column(nCount_Column++).Width = 20; //DY-Q
                    sheet.Column(nCount_Column++).Width = 20; //FNQ
                    sheet.Column(nCount_Column++).Width = 20; //PKQ
                    sheet.Column(nCount_Column++).Width = 20; //SQ
                    sheet.Column(nCount_Column++).Width = 20; //WQ
                    sheet.Column(nCount_Column++).Width = 20; //CMQ
                    sheet.Column(nCount_Column++).Width = 20; //DCQ
                    sheet.Column(nCount_Column++).Width = 20; //BQ
                    sheet.Column(nCount_Column++).Width = 20; //Ex/SrtQ
                    sheet.Column(nCount_Column++).Width = 20; //Ex/SrtQ
                    nEndCol = nCount_Column - 1;
                    #endregion

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol / 2]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 16; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nEndCol / 2 + 1, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = Header; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol / 2]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nEndCol / 2 + 1, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    nRowIndex++;


                    #region Date Wise Loop
                    foreach (var oRSQC in dataGrpList)
                    {
                        #region Column Header
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "Date :" + oRSQC.HeaderName, nRowIndex, nRowIndex++, nStartCol, nEndCol, true, ExcelHorizontalAlignment.Left);
                        
                        nStartCol = 2;
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "#SL", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Product", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Lot No", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Order No", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Order Qty", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Buyer Name", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Color Name", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Dye Lot No", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Dyeing Qty", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Packing Qty", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Shading Qty", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Wastage Qty", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Color Not Match", false, true);
                        //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Delivery Qty", false, true);
                        //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Balance Qty", false, true);

                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Finishing Qty", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Excess/Shot Qty", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Excess/Shot (%)", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Fresh Yarn G/L Qty", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "Fresh Yarn G/L (%)", false, true);
                        nRowIndex++;
                        #endregion

                        #region Data List Wise Loop
                      
                        int nCount = 0;
                        foreach (var oItem in oRSQC.Results)
                        {
                            nStartCol = 2; nCount++;

                            #region DATA
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, nCount.ToString(), false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.ProductName, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.LotNo, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.OrderNo, false);

                            ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.OrderQty.ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.BuyerName, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.ColorName, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.RouteSheetNo, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty.ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.PackingQty.ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.ShadingQty.ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.WastageQty.ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.ColorMisQty.ToString(), true);
                
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.FinishQty.ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.ExcessShortQty.ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (oItem.Qty>0 ? (oItem.ExcessShortQty*100/oItem.Qty) : 0).ToString(), true);

                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.BalanceQty.ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (oItem.Qty > 0 ? (oItem.BalanceQty * 100 / oItem.Qty) : 0).ToString(), true);

                            nRowIndex++;
                            #endregion
                        }
                       #endregion

                        nRowIndex++;
                    }

                    //int nCol_Total = 4;
                    //ExcelTool.FillCellMerge(ref sheet, "Total", nRowIndex, nRowIndex, 2, 3, true, ExcelHorizontalAlignment.Center);
                    //foreach (var oDataGrp in dataGrpList)
                    //{
                    //    ExcelTool.FillCell(sheet, nRowIndex, nCol_Total, oDataGrp.Results.Sum(x => x.Qty_Batch).ToString(), true, true);
                    //    ExcelTool.FillCell(sheet, nRowIndex, nCol_Total + 1, oDataGrp.Results.Sum(x => x.Value).ToString(), true, true);
                    //    ExcelTool.FillCell(sheet, nRowIndex, nCol_Total + 2, oDataGrp.Results.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)).ToString(), true, true);
                    //    nCol_Total += 3;
                    //}
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Day_Basis_Report_Shift_Wise.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion

        public void PrintRpt_Excel_DUOrderRS(string sTempString, int nReportLevel)
        {

            string sMunit = "";
            DUOrderRS oDUOrderRS = new DUOrderRS();
            oDUOrderRS.ErrorMessage = sTempString;
            string sSQL = MakeSQL_RS(oDUOrderRS);
            _oDUOrderRSs = DUOrderRS.Gets(sSQL, nReportLevel, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oDUOrderRSs.Count > 0)
            {
                sMunit = _oDUOrderRSs[0].MUName;
                if (String.IsNullOrEmpty(sMunit))
                {
                    sMunit = "Lbs";
                }
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nTotalQty = 0;


            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 19;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Dyeing lot Management");
                sheet.Name = "Dyeing lot Management";
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;
                sheet.Column(10).Width = 20;
                sheet.Column(11).Width = 20;
                sheet.Column(12).Width = 20;
                sheet.Column(13).Width = 20;
                sheet.Column(14).Width = 20;
                sheet.Column(15).Width = 30;
                sheet.Column(16).Width = 30;
                sheet.Column(17).Width = 30;
                sheet.Column(18).Width = 30;
                sheet.Column(19).Width = 30;
                sheet.Column(20).Width = 30;
                //   nEndCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data
                #region Date Print
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Dyeing Lot Management Report"; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = _sDateRange; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion



                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Dye Lot No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Color No"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Labdip No"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Yarn out Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Packing Qty (" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "B/Q "; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "WIP (" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Stock InHand(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = "UnManagedQty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = "ManagedQty (" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 17]; cell.Value = "Loss/Gain  (" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 18]; cell.Value = "Order Type"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 19]; cell.Value = "Raw Lot No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 20]; cell.Value = "Raw Lot Store"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #region Data
                foreach (DUOrderRS oItem in _oDUOrderRSs)
                {

                    //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                    nSL++;
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.OrderNo.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.RouteSheetNo.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.RSDateSt.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.RSStateSt.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.Qty_RS; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.FreshDyedYarnQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.BagCount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.Pro_Pipline; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.StockInHand; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.UnManagedQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.ManagedQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = oItem.DeliveryQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = (oItem.GainLoss); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 18]; cell.Value = oItem.OrderTypeSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 19]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 20]; cell.Value = oItem.WUName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion

                #region Total
                
                cell = sheet.Cells[nRowIndex,2,nRowIndex, 8]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nTotalQty = _oDUOrderRSs.Select(c => c.Qty_RS).Sum();
                cell = sheet.Cells[nRowIndex, 9]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nTotalQty = _oDUOrderRSs.Select(c => c.FreshDyedYarnQty).Sum();

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nTotalQty = _oDUOrderRSs.Select(c => c.BagCount).Sum();

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nTotalQty = _oDUOrderRSs.Select(c => c.Pro_Pipline).Sum();

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                nTotalQty = _oDUOrderRSs.Select(c => c.StockInHand).Sum();
                cell = sheet.Cells[nRowIndex, 13]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nTotalQty = _oDUOrderRSs.Select(c => c.UnManagedQty).Sum();
                cell = sheet.Cells[nRowIndex, 14]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nTotalQty = _oDUOrderRSs.Select(c => c.ManagedQty).Sum();
                cell = sheet.Cells[nRowIndex, 15]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nTotalQty = _oDUOrderRSs.Select(c => c.DeliveryQty).Sum();
                cell = sheet.Cells[nRowIndex, 16]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                nTotalQty = _oDUOrderRSs.Select(c => c.GainLoss).Sum();
                cell = sheet.Cells[nRowIndex, 17]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

             
                cell = sheet.Cells[nRowIndex, 18]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 19]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 20]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                nRowIndex = nRowIndex + 1;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=DyeingLotManagement(Color).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


        }
        public void PrintRpt_Excel_DUProductOrder(string sTempString, int nReportLevel)
        {
            string sMunit = "";
            _oDUOrderTracker.Params = sTempString;
            DUOrderRS oDUOrderRS = new DUOrderRS();
            oDUOrderRS.ErrorMessage = sTempString;
            oDUOrderRS.ReportLevelType = nReportLevel;
            string sSQL = MakeSQL_RS(oDUOrderRS);
            _oDUOrderRSs = DUOrderRS.Gets(sSQL, oDUOrderRS.ReportLevelType, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oDUOrderRSs.Count > 0)
            {
                sMunit = _oDUOrderRSs[0].MUName;
                if (String.IsNullOrEmpty(sMunit))
                {
                    sMunit = "Lbs";
                }
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;


            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 11;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("DyeingLotManagement(Product)");
                sheet.Name = "Dyeing Lot Management";
                sheet.Column(3).Width = 40;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;
                sheet.Column(10).Width = 20;
                sheet.Column(11).Width = 20;
                sheet.Column(12).Width = 20;
                sheet.Column(13).Width = 20;
               
                //   nEndCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Dyeing Lot Management Report   Date: " + DateTime.Today.ToString("dd MMM yyyy hh:mm:tt") + ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Dyeing Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Packing Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "B/Q"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Pro. Pipeline (" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Stock In Hand (" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Unmanaged Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Managed Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Loss/Gain"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #region Data
                foreach (DUOrderRS oItem in _oDUOrderRSs)
                {
                    //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                    nSL++;
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "" + oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "" + oItem.Qty_RS; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "" + oItem.FreshDyedYarnQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                   
                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.BagCount.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.Pro_Pipline; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.StockInHand; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.UnManagedQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex,10]; cell.Value = oItem.ManagedQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.GainLoss; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion

                #region Total
                
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                double nTotalQty = _oDUOrderRSs.Select(c => c.Qty_RS).Sum();
                cell = sheet.Cells[nRowIndex, 4]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nTotalQty = _oDUOrderRSs.Select(c => c.FreshDyedYarnQty).Sum();
                cell = sheet.Cells[nRowIndex, 5]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nTotalQty = _oDUOrderRSs.Select(c => c.BagCount).Sum();
                cell = sheet.Cells[nRowIndex, 6]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                nTotalQty = _oDUOrderRSs.Select(c => c.Pro_Pipline).Sum();
                cell = sheet.Cells[nRowIndex, 7]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nTotalQty = _oDUOrderRSs.Select(c => c.StockInHand).Sum();
                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nTotalQty = _oDUOrderRSs.Select(c => c.UnManagedQty).Sum();
                cell = sheet.Cells[nRowIndex, 9]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nTotalQty = _oDUOrderRSs.Select(c => c.ManagedQty).Sum();
                cell = sheet.Cells[nRowIndex, 10]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nTotalQty = _oDUOrderRSs.Select(c => c.GainLoss).Sum();
                cell = sheet.Cells[nRowIndex, 11]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex = nRowIndex + 1;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=DyeingLotManagement(Product).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        #endregion
 
        public ActionResult SetSessionSearchCriterias(DUOrderTracker oDUOrderTracker)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oDUOrderTracker);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeliveryDetailPreview(int id)
        {
            oDUOrderTracker = (DUOrderTracker)Session[SessionInfo.ParamObj];
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DULotDistribution> _oDULotDistributionsDes = new List<DULotDistribution>();
            _oDyeingOrder = DyeingOrder.Get(oDUOrderTracker.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUOrderSetup = oDUOrderSetup.GetByType(_oDyeingOrder.DyeingOrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.Get(oDUOrderSetup.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);  
            string sSQL = "SELECT * FROM View_DUDeliveryChallanRegister AS HH WHERE HH.DyeingOrderDetailID = " + id;
            _DUDeliveryChallanRegisters = DUDeliveryChallanRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            string _sSQL = "SELECT * FROM View_DUReturnChallanDetail AS HH WHERE HH.DyeingOrderDetailID = " + id + " ORDER BY HH.DUReturnChallanID";
            _DUReturnChallanDetails = DUReturnChallanDetail.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);
            string _ssSQL = "SELECT * FROM View_DUReturnChallan AS HH WHERE HH.DUReturnChallanID IN (SELECT KK.DUReturnChallanID FROM View_DUReturnChallanDetail AS KK WHERE KK.DyeingOrderDetailID = " + id + ") ORDER BY HH.DUReturnChallanID";
            _DUReturnChallans = DUReturnChallan.Gets(_ssSQL, (int)Session[SessionInfo.currentUserID]);
            string ssSQL = "Select * from view_DULotDistribution where DULotDistributionID in (Select DULotDistributionID from DULotDistributionLog where  DODID_Source="+ id +")";
            _oDULotDistributions = DULotDistribution.Gets(ssSQL, (int)Session[SessionInfo.currentUserID]);

            ssSQL = "Select * from view_DULotDistribution where DULotDistributionID in (Select DULotDistributionID from DULotDistributionLog where  DODID_Des=" + id + ")";
             _oDULotDistributionsDes = DULotDistribution.Gets(ssSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptDelieryDetailPreview oReport = new rptDelieryDetailPreview();
            byte[] abytes = oReport.PrepareReport(oDUOrderTracker, _DUDeliveryChallanRegisters, _DUReturnChallanDetails, _DUReturnChallans, _oDULotDistributions, oBusinessUnit, oCompany, "", _oDULotDistributionsDes);
            return File(abytes, "application/pdf");
                 
        
            
           
        }
    }
}
