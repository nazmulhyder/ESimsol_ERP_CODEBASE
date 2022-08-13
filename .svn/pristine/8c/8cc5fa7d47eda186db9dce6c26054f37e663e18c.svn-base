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
    public class PTUTrackerController : Controller
    {
        PTUTracker _oPTUTracker = new PTUTracker();
        List<PTUTracker> _oPTUTrackers = new List<PTUTracker>();
        string _sErrorMessage = "";
        string _sDateRange = "";
        public ActionResult ViewPTUTrackers(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oPTUTrackers = new List<PTUTracker>();
            _oPTUTrackers = PTUTracker.Gets(" and OrderType!="+(int)EnumOrderType.SampleOrder, 1, 1, (int)EnumOrderType.None, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PTUStateObj = EnumObject.jGets(typeof(EnumPTUState));
            ViewBag.BUID = buid;
            return View(_oPTUTrackers);
        }
        public ActionResult ViewPTUTrackersSample(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oPTUTrackers = new List<PTUTracker>();
            _oPTUTrackers = PTUTracker.Gets(" and OrderType="+(int)EnumOrderType.SampleOrder, 1, 1, (int)EnumOrderType.SampleOrder, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PTUStateObj = EnumObject.jGets(typeof(EnumPTUState));
            ViewBag.BUID = buid;
            return View(_oPTUTrackers);
        }
        #region Search
        [HttpPost]
        public JsonResult AdvSearch(PTUTracker oPTUTracker)
        {
            _oPTUTrackers = new List<PTUTracker>();
            try
            {
                string sSQL = MakeSQL(oPTUTracker);
                _oPTUTrackers = PTUTracker.Gets(sSQL, 1, 1, oPTUTracker.OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPTUTrackers = new List<PTUTracker>();
            }
            var jsonResult = Json(_oPTUTrackers, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(PTUTracker oPTUTracker)
        {
            string sParams = oPTUTracker.ErrorMessage;
           
            int nCboIssueDate = 0;
            DateTime dFromIssueDate = DateTime.Today;
            DateTime dToIssueDate = DateTime.Today;
            int nCboPODate = 0;
            DateTime dFromDODate = DateTime.Today;
            DateTime dToDODate = DateTime.Today;
            int nCboLCDate = 0;
            DateTime dFromLCDate = DateTime.Today;
            DateTime dToLCDate = DateTime.Today;
            int nCboMkPerson = 0;
            string sCurrentStatus = "";
            bool bExportLCIsntCreateYet = false;
            int nOrderType = 0;
            bool bYetNotMakeFEO = false;
            string sTemp = "";
            string sPINo = "";
            string sOrderNo = "";
            string sColorNo = "";
            string sPTUStatus = "";
            int nBUID = 0;

            int nCboStatusDate = 0;
            DateTime dFromStatusDate = DateTime.Today;
            DateTime dToStatusDate = DateTime.Today;

            nOrderType = oPTUTracker.OrderType;

            if (!string.IsNullOrEmpty(sParams))
            {
                _oPTUTracker.ContractorName = Convert.ToString(sParams.Split('~')[0]);
                _oPTUTracker.BuyerName = Convert.ToString(sParams.Split('~')[1]);
                _oPTUTracker.ProductName = Convert.ToString(sParams.Split('~')[2]);
                nCboIssueDate = Convert.ToInt32(sParams.Split('~')[3]);
                dFromIssueDate = Convert.ToDateTime(sParams.Split('~')[4]);
                dToIssueDate = Convert.ToDateTime(sParams.Split('~')[5]);
                nCboPODate = Convert.ToInt32(sParams.Split('~')[6]);
                dFromDODate = Convert.ToDateTime(sParams.Split('~')[7]);
                dToDODate = Convert.ToDateTime(sParams.Split('~')[8]);
                sPTUStatus = Convert.ToString(sParams.Split('~')[9]);
                 sPINo = Convert.ToString(sParams.Split('~')[10]);
                 sOrderNo = Convert.ToString(sParams.Split('~')[11]);
                 sColorNo = Convert.ToString(sParams.Split('~')[12]);
                //sCurrentStatus = Convert.ToString(sParams.Split('~')[13]);
                //nPaymentType = Convert.ToInt32(sParams.Split('~')[14]);

                //nPIType = Convert.ToInt32(sParams.Split('~')[15]);
                //sStyleNo = Convert.ToString(sParams.Split('~')[16]);

                //nCboStatusDate = Convert.ToInt32(sParams.Split('~')[17]);
                //sTemp = sParams.Split('~')[18];
                //if (!String.IsNullOrEmpty(sTemp))
                //{
                //    dFromStatusDate = Convert.ToDateTime(sParams.Split('~')[18]);
                //}
                //sTemp = sParams.Split('~')[19];
                //if (!String.IsNullOrEmpty(sTemp))
                //{
                //    dToStatusDate = Convert.ToDateTime(sParams.Split('~')[19]);
                //}

                //nBUID = Convert.ToInt32(sParams.Split('~')[20]);
            }


            string sReturn1 = "";
            string sReturn = " ";

            #region Contractor
            if (!String.IsNullOrEmpty(_oPTUTracker.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID in(" + _oPTUTracker.ContractorName + ")";
            }
            #endregion
            #region Buyer Id
            if (!String.IsNullOrEmpty(_oPTUTracker.BuyerName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "BuyerID in(" + _oPTUTracker.BuyerName + ")";
            }
            #endregion
            #region Product Id
            if (!String.IsNullOrEmpty(_oPTUTracker.ProductName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID in(" + _oPTUTracker.ProductName + ")";
            }
            #endregion
            #region Issue Date
            if (nCboIssueDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboIssueDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " OrderType=3 and OrderID in (Select ExportSCID from View_ExportSC where CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromIssueDate.ToString("dd MMM yyyy") + "'',106)))";
                    _sDateRange = "Date: " + dFromIssueDate.ToString("dd MMM yyyy");
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + "OrderType=3 and OrderID in (Select ExportSCID from View_ExportSC where CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromIssueDate.ToString("dd MMM yyyy") + "'',106))) ";
                    _sDateRange = "Date: NotEqualTo->" + dFromIssueDate.ToString("dd MMM yyyy");
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + "OrderType=3 and OrderID in (Select ExportSCID from View_ExportSC where CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromIssueDate.ToString("dd MMM yyyy") + "'',106))) ";
                    _sDateRange = "Date: GreaterThen->" + dFromIssueDate.ToString("dd MMM yyyy");
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + "OrderType=3 and OrderID in (Select ExportSCID from View_ExportSC where CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromIssueDate.ToString("dd MMM yyyy") + "'',106))) ";
                    _sDateRange = "Date: SmallerThen->" + dFromIssueDate.ToString("dd MMM yyyy");
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + "OrderType=3 and OrderID in (Select ExportSCID from View_ExportSC where CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromIssueDate.ToString("dd MMM yyyy") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dToIssueDate.ToString("dd MMM yyyy") + "'',106)))";
                    _sDateRange = "Date: From " + dFromIssueDate.ToString("dd MMM yyyy") + " To " + dToIssueDate.ToString("dd MMM yyyy");
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + "OrderType=3 and OrderID in (Select ExportSCID from View_ExportSC where CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromIssueDate.ToString("dd MMM yyyy") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dToIssueDate.ToString("dd MMM yyyy") + "'',106))) ";
                    _sDateRange = "Date: NotBetween " + dFromIssueDate.ToString("dd MMM yyyy") + " To " + dToIssueDate.ToString("dd MMM yyyy");
                }
            }
            #endregion
            #region PO Date
            if (nCboPODate != (int)EnumCompareOperator.None)
            {
                if (nOrderType != (int)EnumOrderType.SampleOrder)
                {
                    sTemp = "OrderType=3 and OrderID in (Select ExportSCID from ExportSC where ExportPIID in (Select ExportPIID from DyeingOrder where ";
                }
                else
                {
                    sTemp = "OrderType=2 and OrderID in ((Select DyeingOrderID from DyeingOrder where ";
                }

                Global.TagSQL(ref sReturn);
                if (nCboPODate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + sTemp+" CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106)))) ";
                }
                else if (nCboPODate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + sTemp + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106)) ))";
                }
                else if (nCboPODate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + sTemp + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106)))) ";
                }
                else if (nCboPODate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + sTemp + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106)))) ";
                }
                else if (nCboPODate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + sTemp + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromDODate.ToString("dd MMM yyyy") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dToDODate.ToString("dd MMM yyyy") + "'',106)) ))";
                }
                else if (nCboPODate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + sTemp + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromDODate.ToString("dd MMM yyyy") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dToDODate.ToString("dd MMM yyyy") + "'',106)) )) ";
                }
            }
            #endregion
            #region Status With Date
            if (nCboStatusDate != (int)EnumCompareOperator.None)
            {
                if (!String.IsNullOrEmpty(sCurrentStatus))
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboStatusDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " EPI.ExportPIID in (Select ExportPIID from ExportPIHistory where PIStatus in (" + sCurrentStatus + ") and CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromStatusDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    if (nCboStatusDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " EPI.ExportPIID in (Select ExportPIID from ExportPIHistory where PIStatus in (" + sCurrentStatus + ") and CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromStatusDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToStatusDate.ToString("dd MMM yyyy") + "',106)) ) ";
                    }
                }

            }
            #endregion
            #region Current Status
            if (!string.IsNullOrEmpty(sCurrentStatus))
            {
                if (nCboStatusDate == (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " EPI.PIStatus IN (" + sCurrentStatus + ")";
                }
            }
            #endregion
            #region LC Pending
            if (bExportLCIsntCreateYet)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LCID=0 AND PIStatus>" + (int)EnumPIStatus.RequestForApproved + " ";
            }
            #endregion
          
            #region Color No
            if (!String.IsNullOrEmpty(sColorNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LabDipDetailID in (Select LabdipDetail.LabDipDetailID from LabdipDetail where ColorNo  Like ''%" + sColorNo + "%'')";
            }
            #endregion
            if (nOrderType == (int)EnumOrderType.SampleOrder)
            {
                if (!String.IsNullOrEmpty(sPINo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderType=2 and OrderID in (Select DyeingOrder.DyeingOrderID from DyeingOrder where  DyeingOrder.ExportPIID in  (Select ExportPIID from View_ExportSC where PINo Like ''%" + sPINo + "%''))";
                }
                if (!String.IsNullOrEmpty(sOrderNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "  OrderType=2 and OrderID in (Select DyeingOrder.DyeingOrderID from DyeingOrder where OrderNo like ''%" + sOrderNo + "%'')";
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(sPINo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderType=3 and OrderID in (Select ExportSCID from View_ExportSC where PINo Like ''%" + sPINo + "%'')";
                }
                if (!String.IsNullOrEmpty(sOrderNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "  OrderType=3 And PTUID in (Select PTUID from DyeingOrderDetail where DyeingOrderID in (Select DyeingOrder.DyeingOrderID from DyeingOrder where OrderNo Like ''%" + sOrderNo + "%''))";
                }
            }
            #region Business Unit
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPI.BUID = " + nBUID;
            }
            #endregion
            string sSQL = sReturn1 + " " + sReturn;
            return sSQL;
        }
        
      
        #endregion
        

        [HttpPost]
        public JsonResult GetsPODetails(PTUTracker oPTUTracker)
        {
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<DyeingOrderDetail> oDyeingOrderDetails_Temp = new List<DyeingOrderDetail>();
            try
            {
                if (oPTUTracker.OrderType == (int)EnumOrderType.BulkOrder)
                {
                    oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where ExportPIID in (Select ExportPIID from ExportSC where ExportSCID=" + oPTUTracker.OrderID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDyeingOrderDetails_Temp = DyeingOrderDetail.Gets("SELECT * FROM View_DyeingOrderDetail where isnull([Status],0)<>4 and PTUID=" + oPTUTracker.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (oPTUTracker.OrderType == (int)EnumOrderType.SampleOrder)
                {
                    oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where DyeingOrderID=" + oPTUTracker.OrderID + "", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDyeingOrderDetails_Temp = DyeingOrderDetail.Gets("SELECT * FROM View_DyeingOrderDetail where isnull([Status],0)<>4 and PTUID=" + oPTUTracker.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
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
        public JsonResult GetsRSs(PTUTracker oPTUTracker)
        {
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            try
            {
                string sSQL = "Select * from View_RouteSheet Where PTUID =" + oPTUTracker.PTUID;
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
        public JsonResult GetsLots(PTUTracker oPTUTracker)
        {
            List<Lot> oLots = new List<Lot>();
            try
            {
                oLots = Lot.Gets("Select * from View_Lot where Balance>0.2 and parentType=106 and ParentID in (Select RouteSheetID from RouteSheet where PTUID=" + oPTUTracker.PTUID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region Print XlX
        public void PrintRpt_Excel_Bulk(string sTempString)
        {

            string sMunit = "";
            _oPTUTracker.ErrorMessage = sTempString;
            string sSQL = MakeSQL(_oPTUTracker);
            _oPTUTrackers = PTUTracker.Gets(sSQL, 1, 1, (int)EnumOrderType.BulkOrder, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oPTUTrackers.Count > 0)
            {
                sMunit = _oPTUTrackers[0].MUName;
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
            double nOrderQty = 0;
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
                sheet.Column(17).Width = 30;
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

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "P/I No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Color No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "PI Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Dyeing Order Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Pro Pipline Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Pro Finishing(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Yet To Production(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Yet To Delivery(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Return Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = "Claim Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;

                cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Stock in Hand(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;

                cell = sheet.Cells[nRowIndex, 17]; cell.Value = "Stock in Hand Avl(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;


                cell = sheet.Cells[nRowIndex, 18]; cell.Value = "Concern Person"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
              

                nRowIndex++;
                #endregion

                #region Data
                foreach (PTUTracker oItem in _oPTUTrackers)
                {


                    //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.PINo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ContractorName.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ColorNameShade; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.ColorNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.Qty_PI; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ProductionPipeLineQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ProductionFinishedQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.YetToProduction; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.ActualDeliveryQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.YetToDelivery; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.ReturnQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.ReOrderQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = oItem.StockInHand; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = oItem.StockInAval; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 18]; cell.Value = oItem.MKT; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                 
                    
                    nOrderQty = nOrderQty + oItem.OrderQty;
                    nProductionPipeLineQty = nProductionPipeLineQty + oItem.ProductionPipeLineQty;
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

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
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

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nOrderQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = nProductionPipeLineQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = nProductionFinishedQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = nYetToProduction; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = nActualDeliveryQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = nYetToDelivery; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = nReturnQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = nReOrderQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 16]; cell.Value = nStockInHand; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 17]; cell.Value = nStockInAval; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 18]; cell.Value = ""; cell.Style.Font.Bold = true;
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
        public void PrintRpt_Excel_Sample(string sTempString)
        {

            string sMunit = "";
            _oPTUTracker.ErrorMessage = sTempString;
            string sSQL = MakeSQL(_oPTUTracker);
            _oPTUTrackers = PTUTracker.Gets(sSQL, 1, 1, (int)EnumOrderType.SampleOrder, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oPTUTrackers.Count > 0)
            {
                sMunit = _oPTUTrackers[0].MUName;
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
                sheet.Name = "Production Status(Sample Order)";
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 30;
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
                sheet.Column(17).Width = 20;
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

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Sample Order No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Color No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Order Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Pro Pipline Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Pro Finishing(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Yet To Production(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Yet To Delivery(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Return Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Claim Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = "Stock in Hand(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;

                cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Stock in Hand Avl(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;


                cell = sheet.Cells[nRowIndex, 17]; cell.Value = "Concern Person"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Delivery Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;


                nRowIndex++;
                #endregion

                #region Data
                foreach (PTUTracker oItem in _oPTUTrackers)
                {


                    //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.PINo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ContractorName.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ColorNameShade; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.ColorNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                   

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.ProductionPipeLineQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ProductionFinishedQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.YetToProduction; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.ActualDeliveryQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.YetToDelivery; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.ReturnQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.ReOrderQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.StockInHand; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = oItem.StockInAval; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = oItem.MKT; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    nOrderQty = nOrderQty + oItem.OrderQty;
                    nProductionPipeLineQty = nProductionPipeLineQty + oItem.ProductionPipeLineQty;
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

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
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

              
                cell = sheet.Cells[nRowIndex, 7]; cell.Value = nOrderQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nProductionPipeLineQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = nProductionFinishedQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = nYetToProduction; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = nActualDeliveryQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = nYetToDelivery; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = nReturnQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = nReOrderQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = nStockInHand; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 16]; cell.Value = nStockInAval; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 17]; cell.Value = ""; cell.Style.Font.Bold = true;
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
                Response.AddHeader("content-disposition", "attachment; filename=ProductionStatus(Sample).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


        }

        #endregion

    }
}
