using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ESimSol.Services.DataAccess;
using System.Dynamic;

namespace ESimSolFinancial.Controllers
{
    public class DUProductionStatusReportController : PdfViewController
    {
        DUProductionStatusReport _oDUProductionStatusReport = new DUProductionStatusReport();
        List<DUProductionStatusReport> _oDUProductionStatusReports = new List<DUProductionStatusReport>();

        int GOrderDate;
        DateTime d1;
        DateTime d2;

        public ActionResult ViewDUProductionStatusReport(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<DUProductionStatusReport> oDUProductionStatusReports = new List<DUProductionStatusReport>();
            
            List<DUProductionStatusReport> oYarnWIse = new List<DUProductionStatusReport>();

            List<DUProductionStatusReport> oBuyerWIse = new List<DUProductionStatusReport>();


            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Issue Stores
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            oIssueStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.LotMixing, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

            ViewBag.IssueStores = oIssueStores;

            ViewBag.YarnWise = oYarnWIse;
            ViewBag.BuyerWise = oBuyerWIse;
            ViewBag.OrderTypes = oDUOrderSetups;
            
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(oDUProductionStatusReports);
        }
        public ActionResult ViewDUProductionDetail(int nDOID, int nProductID)
        {
            List<DUOrderRS> oDUOrderRSs = new List<DUOrderRS>();
           
            string sSQL = " BUID=" + nProductID;
            //_oDUProductionStatusReports = DUProductionStatusReport.Gets(sSQL, EnumReportLayout.BankWise, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = " where RouteSheetID in (select RouteSheetID from RouteSheetDO where  DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID="+ nDOID +" and ProductID="+ nProductID+" ))";
            oDUOrderRSs = DUOrderRS.Gets(sSQL, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);

            return View(oDUOrderRSs);
        }
        public JsonResult Gets()
        {
            List<DUProductionStatusReport> oDUProductionStatusReports = new List<DUProductionStatusReport>();
            oDUProductionStatusReports = DUProductionStatusReport.Gets(" BUID=1 ", EnumReportLayout.BankWise, ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDUProductionStatusReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetsOrderType()
        {
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            string sSql = "select * from DUOrderSetup where OrderType IN(2, 3, 4, 5)";
            oDUOrderSetups = DUOrderSetup.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDUOrderSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetsChallanDetail(DyeingOrderDetail oDyeingOrderDetail)
        {
            List<DUDeliveryChallanDetail> oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
            string sSQL = "Select *  from View_DUDeliveryChallanDetail where View_DUDeliveryChallanDetail.DUDeliveryChallanID IN (Select DUDeliveryChallanID from DUDeliveryChallan where DUDeliveryChallan.IsDelivered=1 ) and  View_DUDeliveryChallanDetail.DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID=" + oDyeingOrderDetail.DyeingOrderID + " and ProductID=" + oDyeingOrderDetail.ProductID + ")";
            //sSQL = "Select *  from View_DUDeliveryChallanDetail where View_DUDeliveryChallanDetail.DUDeliveryChallanID IN (Select DUDeliveryChallanID from DUDeliveryChallan where DUDeliveryChallan.IsDelivered=1 AND DOID=" + nDOID + " ) and  View_DUDeliveryChallanDetail.LotID in (Select RouteSheetDO.LotID_Finish from RouteSheetDO where RouteSheetDO.DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where ProductID=" + nProductID + " ))";
            oDUDeliveryChallanDetails = DUDeliveryChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUDeliveryChallanDetails = oDUDeliveryChallanDetails;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDUDeliveryChallanDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult AdvSearchDUP(DUProductionStatusReport oDUProductionStatusReport)
        {
            //var tuple = new Tuple<List<DUProductionStatusReport>, List<DUProductionStatusReport>, List<DUProductionStatusReport>>(new List<DUProductionStatusReport>(), new List<DUProductionStatusReport>(), new List<DUProductionStatusReport>());

            dynamic ResultList = new ExpandoObject();
            try
            {
                string sSQL = GetSQL(oDUProductionStatusReport.Params);
                var oDUPS = DUProductionStatusReport.Gets(sSQL,EnumReportLayout.BankWise, (int)Session[SessionInfo.currentUserID]);

                List<DUProductionStatusReport> oYarnWIse = new List<DUProductionStatusReport>();

                oYarnWIse = oDUPS.GroupBy(x => new { x.ProductID, x.ProductName }, (key, grp) =>
                new DUProductionStatusReport
                {
                    ProductName = key.ProductName,
                    ProductID = key.ProductID,

                    Qty_Order = grp.Sum(p => p.Qty_Order),
                    Qty_Rcv = grp.Sum(p => p.Qty_Rcv),
                    Qty_YetToRcv = grp.Sum(p => p.Qty_YetToRcv),
                    Qty_SW = grp.Sum(p => p.Qty_SW),
                    Qty_YetToSW = grp.Sum(p => p.Qty_YetToSW)
                }).ToList();
                List<DUProductionStatusReport> oBuyerWIse = new List<DUProductionStatusReport>();

                oBuyerWIse = oDUPS.GroupBy(x => new { x.BuyerName}, (key, grp) =>
                    new DUProductionStatusReport
                    {
                        BuyerName = key.BuyerName,
                        OrderNo = grp.First().OrderNo,
                        Qty_Order = grp.Sum(p => p.Qty_Order),
                        Qty_Rcv = grp.Sum(p => p.Qty_Rcv),
                        Qty_YetToRcv = grp.Sum(p => p.Qty_YetToRcv),
                        Qty_SW = grp.Sum(p => p.Qty_SW),
                        Qty_YetToSW = grp.Sum(p => p.Qty_YetToSW)

                    }).ToList();

                //tuple = new Tuple<List<DUProductionStatusReport>, List<DUProductionStatusReport>, List<DUProductionStatusReport>>(oDUPS, oYarnWIse, oBuyerWIse);

                ResultList.DUPS = oDUPS;
                ResultList.YarnWIse = oYarnWIse;
                ResultList.BuyerWIse = oBuyerWIse;
            }
            catch (Exception e)
            {
                //tuple.Item1; //total list List
                //tuple.Item2;//GroupByContactPersonnel List
                //tuple.Item3;
                ResultList = new ExpandoObject();
                ResultList.ErrorMessage = e.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = sjson = serializer.Serialize(ResultList);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sParams)
        {
            int nBUID=0,
                nOrderType = 0,
                nDateCriteria_Order = 0,
                nDateCriteria_Rcv= 0,
                nStore = 0;
            
            string sOrderNo = "",
                   sStyleNo = "",
                   sLotNo = "",
                   sProductIDs = "",
                   sBuyerIDs = "";

            DateTime dStart_Order = DateTime.Today,
                     dEnd_Order = DateTime.Today;
            DateTime dStart_Rcv = DateTime.Today,
                     dEnd_Rcv = DateTime.Today;

            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                sOrderNo = sParams.Split('~')[nCount++];
                sStyleNo = sParams.Split('~')[nCount++];
                sLotNo = sParams.Split('~')[nCount++];
                sProductIDs = sParams.Split('~')[nCount++];
                sBuyerIDs= sParams.Split('~')[nCount++];
                nOrderType = Convert.ToInt32(sParams.Split('~')[nCount++]);
                nStore = Convert.ToInt32(sParams.Split('~')[nCount++]);
                nDateCriteria_Order = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_Order = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_Order = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                nDateCriteria_Rcv = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_Rcv = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_Rcv = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                nBUID = Convert.ToInt32(sParams.Split('~')[nCount++]);

                d1 = dStart_Order; d2 = dEnd_Order; GOrderDate = nDateCriteria_Order;
            }

            string sReturn1 = "";
            string sReturn = " WHERE DyeingOrderID <>0 "; // + nBUID;

            #region RCV DATE SEARCH
            if (nDateCriteria_Rcv > 0) 
            { 
                string sDate = "";
                DateObject.CompareDateQuery(ref sDate, " IssueDate", nDateCriteria_Rcv, dStart_Rcv, dEnd_Rcv);
                sReturn = sReturn+ " AND DyeingOrderID IN (SELECT DyeingOrderID FROM DUProGuideLine " + sDate + ")";
            }
            #endregion

            #region ORDER DATE SEARCH
            if(nDateCriteria_Order>0)
            {
                string sOrderDate= "";
                DateObject.CompareDateQuery(ref sOrderDate, "OrderDate", nDateCriteria_Order, dStart_Order, dEnd_Order);
                sReturn = sReturn + " AND DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder " + sOrderDate + ")";
            }
            #endregion

            #region sSLNo
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE OrderNo LIKE '%" + sOrderNo + "%')";
            }
            #endregion

            #region StyleNo
            if (!string.IsNullOrEmpty(sStyleNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE StyleNo LIKE '%" + sStyleNo + "%')";
            }
            #endregion

            #region Contractor/Buyer
            if (!string.IsNullOrEmpty(sBuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE ContractorID IN (" + sBuyerIDs + "))";
            }
            #endregion

            #region LotNo
            if (!string.IsNullOrEmpty(sLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  DyeingOrderID IN (SELECT DyeingOrderID FROM DUProGuideLine WHERE DUProGuideLineID IN ( SELECT DUProGuideLineID FROM DUProGuideLineDetail WHERE LotNo LIKE '%" + sLotNo + "%' ))";
            }
            #endregion

            #region WorkingUnitID
            if (nStore > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DyeingOrderID IN (SELECT DyeingOrderID FROM DUProGuideLine WHERE WorkingUnitID =" + nStore+")";
            }
            #endregion

            #region OrderType
            if (nOrderType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderID IN (SELECT DyeingOrderID FROM DUProGuideLine WHERE OrderType =" + nOrderType + " )";
;
            }
            #endregion

            #region Product IDs
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductID IN (" + sProductIDs + ") "; //DUProGuideLineID IN ( SELECT DUProGuideLineID FROM DUProGuideLineDetail WHERE ProductID IN 
            }
            #endregion

            #region IsInHouse = 0
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " DyeingOrderID IN (SELECT DyeingOrderID FROM DUProGuideLine WHERE OrderType IN (SELECT DUOrderSetup.OrderType FROM DUOrderSetup WHERE ISNULL(IsInHouse,0) = 0))";
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        [HttpPost]
        public JsonResult SearchEntryUnit(Product oProduct)
        {
            List<DUDyeingTypeMapping> oDUDyeingTypeMappings = new List<DUDyeingTypeMapping>();
            string sSql = "select * from DUDyeingTypeMapping where ProductID = " + oProduct.ProductID;
            oDUDyeingTypeMappings = DUDyeingTypeMapping.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            var oDUDyeingTypeMappingDist = oDUDyeingTypeMappings.GroupBy(x => (int)x.DyeingType).Select(y => y.Last());
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = sjson = serializer.Serialize(oDUDyeingTypeMappingDist);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveEntry(DUDyeingTypeMapping oDUDyeingTypeMapping)
        {
             //oDUDyeingTypeMapping = new DUDyeingTypeMapping();
            try
            {
                oDUDyeingTypeMapping = oDUDyeingTypeMapping.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oDUDyeingTypeMapping = new DUDyeingTypeMapping();
                oDUDyeingTypeMapping.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUDyeingTypeMapping);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void Print_ReportXL(string sTempString, int BUID, int trackid, int formate)
        {
            List<LotParent> oLotParents = new List<LotParent>();
            List<DUProductionStatusReport> _oDUProductionStatusReports = new List<DUProductionStatusReport>();
            List<DUProductionStatusReport> oDUProductionStatusReport = new List<DUProductionStatusReport>();
           
            if (string.IsNullOrEmpty(sTempString))
            {
                _oDUProductionStatusReports = new List<DUProductionStatusReport>();
            }
            else
            {
                string sSQL = GetSQL(sTempString);
                _oDUProductionStatusReports = DUProductionStatusReport.Gets(sSQL, EnumReportLayout.BankWise,((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            _oDUProductionStatusReports = _oDUProductionStatusReports.OrderBy(x => x.BuyerName).ToList();

            if (trackid == 2)
            {
                _oDUProductionStatusReports = _oDUProductionStatusReports.GroupBy(x => new { x.ProductID, x.ProductName }, (key, grp) =>
                                                new DUProductionStatusReport
                                                {
                                                    ProductName = key.ProductName,
                                                    ProductID = key.ProductID,
                                                    Qty_Order = grp.Sum(p => p.Qty_Order),
                                                    Qty_Rcv = grp.Sum(p => p.Qty_Rcv),
                                                    Qty_YetToRcv = grp.Sum(p => p.Qty_YetToRcv),
                                                    Qty_SW = grp.Sum(p => p.Qty_SW),
                                                    Qty_YetToSW = grp.Sum(p => p.Qty_YetToSW)
                                                }).ToList();
            }
            else if (trackid == 3) 
            {
                _oDUProductionStatusReports = _oDUProductionStatusReports.GroupBy(x => new { x.BuyerName }, (key, grp) =>
                                               new DUProductionStatusReport
                                               {
                                                   BuyerName = key.BuyerName,
                                                   OrderNo = grp.First().OrderNo,
                                                   OrderDate = grp.First().OrderDate,
                                                   Qty_Order = grp.Sum(p => p.Qty_Order),
                                                   Qty_Rcv = grp.Sum(p => p.Qty_Rcv),
                                                   Qty_YetToRcv = grp.Sum(p => p.Qty_YetToRcv),
                                                   Qty_SW = grp.Sum(p => p.Qty_SW),
                                                   Qty_YetToSW = grp.Sum(p => p.Qty_YetToSW)
                                               }).ToList();
            }
            oLotParents = LotParent.Gets("SELECT * FROM View_LotParent LP WHERE LP.DyeingOrderID IN (" + string.Join(",", _oDUProductionStatusReports.Select(x => x.DyingOrderID)) + ") AND LP.ProductID IN (" + string.Join(",", _oDUProductionStatusReports.Select(x => x.ProductID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            #region Export Excel
            int nRowIndex = 2, nEndRow = 0, nStartCol = 1, nEndCol = 16;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Yarn Receiving Report");
                sheet.Name = "Yarn Receiving Report";

                #region Sheet Columns
                sheet.Column(1).Width = 8;//SL
                sheet.Column(2).Width = 20;//Order No
                sheet.Column(3).Width = 20;//Order Date
                sheet.Column(4).Width = 20;//PI No
                sheet.Column(5).Width = 30;//Buyer Name
                sheet.Column(6).Width = 20;//Order Qty
                sheet.Column(7).Width = 20;//TodayOrder Qty
                sheet.Column(8).Width = 20;//Production Qty
                sheet.Column(9).Width = 20;//Yet To Production
                sheet.Column(10).Width = 20;//TodayDelivery Qty
                sheet.Column(11).Width = 20;//Stock In Hand
                sheet.Column(12).Width = 20;//Tran in
                sheet.Column(13).Width = 20;//Tran out
                sheet.Column(14).Width = 20;//Delivery Qty
                sheet.Column(15).Width = 20;//Yet To Delivery
                sheet.Column(16).Width = 20;//Category Name
                //sheet.Column(17).Width = 20;//Category Name
                //sheet.Column(18).Width = 20;//Category Name
                #endregion

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Yarn Receiving Report";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                if (GOrderDate > 0)
                {
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Order Date Between " + d1.ToString("dd MMM yyyy") + " to " + d2.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.Size = 10;
                    nRowIndex = nRowIndex + 1;
                }

                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region HEADER
                nStartCol = 1;
                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Order Type"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Order Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Buyer Ref"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Order Qty(" + _oDUProductionStatusReports[0].MUnit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Rcv Qty(" + _oDUProductionStatusReports[0].MUnit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Transfer In(" + _oDUProductionStatusReports[0].MUnit + ")"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Transfer Out(" + _oDUProductionStatusReports[0].MUnit + ")"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Yet To Rcv(" + _oDUProductionStatusReports[0].MUnit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "SW Qty(" + _oDUProductionStatusReports[0].MUnit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Yet To SW (" + _oDUProductionStatusReports[0].MUnit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Return Qty (" + _oDUProductionStatusReports[0].MUnit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex = nRowIndex + 1;
                #endregion

                #region Data
                foreach (DUProductionStatusReport oItem in _oDUProductionStatusReports)
                {
                    var oLots = oLotParents.Where(x => x.DyeingOrderID == oItem.DyingOrderID && x.ProductID == oItem.ProductID).ToList();

                    nSL++;
                    int nRowSpan = (oLots == null || oLots.Count()<=0) ? 0 : oLots.Count() - 1;
                    int endRowIndex = nRowIndex + nRowSpan;

                    cell = sheet.Cells[nRowIndex, 1, endRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2, endRowIndex, 2]; cell.Value = "" + oItem.OrderName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3, endRowIndex, 3]; cell.Value = "" + oItem.OrderNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4, endRowIndex, 4]; cell.Value = oItem.OrderDateST; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5, endRowIndex, 5]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6, endRowIndex, 6]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7, endRowIndex, 7]; cell.Value = oItem.BuyerRef; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8, endRowIndex, 8]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    // MERGE CELL BETWEEN 9 TO 11 
                    cell = sheet.Cells[nRowIndex, 10, endRowIndex, 10]; cell.Value = oItem.Qty_Order; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double nQty_Yet_To = oItem.Qty_Order - oLots.Sum(x => x.Qty);
                    cell = sheet.Cells[nRowIndex, 12, endRowIndex, 12]; cell.Value = nQty_Yet_To; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //END MERGE CELL
                    cell = sheet.Cells[nRowIndex, 15, endRowIndex, 15]; cell.Value = oItem.Qty_Return; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16, endRowIndex, 16]; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (oLots == null || oLots.Count() <= 0) 
                    {
                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }
                    foreach (var oLot in oLots)
                    {
                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oLot.LotNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oLot.Qty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.Qty_Transfer_In; cell.Style.Font.Bold = false;
                        //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.Qty_Transfer_Out; cell.Style.Font.Bold = false;
                        //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //cell = sheet.Cells[nRowIndex, 12]; cell.Value =oLot.Balance; cell.Style.Font.Bold = false;
                        //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = oLot.Qty_Soft; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = (oLot.Qty - oLot.Qty_Soft); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.Qty_Return; cell.Style.Font.Bold = false;
                        //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex++;
                    }
                    
                    nEndRow = nRowIndex;
                    //nRowIndex++;
                }

                cell = sheet.Cells[nRowIndex, 1, nRowIndex, 9]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = _oDUProductionStatusReports.Sum(x=>x.Qty_Order); cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = oLotParents.Sum(x => x.Qty); cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 12]; cell.Value = _oDUProductionStatusReports.Sum(x => x.Qty_Transfer_In); cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 13]; cell.Value = _oDUProductionStatusReports.Sum(x => x.Qty_Transfer_Out); cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = (_oDUProductionStatusReports.Sum(x => x.Qty_Order) - oLotParents.Sum(x => x.Qty)); cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = oLotParents.Sum(x => x.Qty_Soft); cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = oLotParents.Sum(x => x.Qty - x.Qty_Soft); cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = _oDUProductionStatusReports.Sum(x => x.Qty_Return); cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        
                cell = sheet.Cells[nRowIndex, 16]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                #endregion

                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Yarn Receiving Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }


        #region

        private string GetSQL_DODetails(string sParams)
        {
            int nBUID = 0,
                nOrderType = 0,
                nDateCriteria_Order = 0,
                nDateCriteria_Rcv = 0,
                nStore = 0;

            string sOrderNo = "",
                   sStyleNo = "",
                   sLotNo = "",
                   sProductIDs = "",
                   sBuyerIDs = "";

            DateTime dStart_Order = DateTime.Today,
                     dEnd_Order = DateTime.Today;
            DateTime dStart_Rcv = DateTime.Today,
                     dEnd_Rcv = DateTime.Today;

            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                sOrderNo = sParams.Split('~')[nCount++];
                sStyleNo = sParams.Split('~')[nCount++];
                sLotNo = sParams.Split('~')[nCount++];
                sProductIDs = sParams.Split('~')[nCount++];
                sBuyerIDs = sParams.Split('~')[nCount++];
                nOrderType = Convert.ToInt32(sParams.Split('~')[nCount++]);
                nStore = Convert.ToInt32(sParams.Split('~')[nCount++]);
                nDateCriteria_Order = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_Order = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_Order = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                nDateCriteria_Rcv = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_Rcv = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_Rcv = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                nBUID = Convert.ToInt32(sParams.Split('~')[nCount++]);

                d1 = dStart_Order; d2 = dEnd_Order; GOrderDate = nDateCriteria_Order;
            }

            string sReturn1 = "SELECT DyeingOrderID, OrderNo, OrderDate, ProductID, ProductCode, ProductName, ContractorID, ContractorName, StyleNo, SUM(Qty) AS Qty FROM View_DyeingOrderReport ";
            string sReturn = " WHERE DyeingOrderID <>0 "; // + nBUID;

            #region RCV DATE SEARCH
            if (nDateCriteria_Rcv > 0)
            {
                string sDate = "";
                DateObject.CompareDateQuery(ref sDate, " IssueDate", nDateCriteria_Rcv, dStart_Rcv, dEnd_Rcv);
                sReturn = sReturn + " AND DyeingOrderID IN (SELECT DyeingOrderID FROM DUProGuideLine " + sDate + ")";
            }
            #endregion

            #region ORDER DATE SEARCH
            if (nDateCriteria_Order > 0)
            {
                string sOrderDate = "";
                DateObject.CompareDateQuery(ref sOrderDate, "OrderDate", nDateCriteria_Order, dStart_Order, dEnd_Order);
                sReturn = sReturn + " AND DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder " + sOrderDate + ")";
            }
            #endregion

            #region sSLNo
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE OrderNo LIKE '%" + sOrderNo + "%')";
            }
            #endregion

            #region StyleNo
            if (!string.IsNullOrEmpty(sStyleNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE StyleNo LIKE '%" + sStyleNo + "%')";
            }
            #endregion

            #region Contractor/Buyer
            if (!string.IsNullOrEmpty(sBuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE ContractorID IN (" + sBuyerIDs + "))";
            }
            #endregion

            #region LotNo
            if (!string.IsNullOrEmpty(sLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  DyeingOrderID IN (SELECT DyeingOrderID FROM DUProGuideLine WHERE DUProGuideLineID IN ( SELECT DUProGuideLineID FROM DUProGuideLineDetail WHERE LotNo LIKE '%" + sLotNo + "%' ))";
            }
            #endregion

            #region WorkingUnitID
            if (nStore > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DyeingOrderID IN (SELECT DyeingOrderID FROM DUProGuideLine WHERE WorkingUnitID =" + nStore + ")";
            }
            #endregion

            #region OrderType
            if (nOrderType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderID IN (SELECT DyeingOrderID FROM DUProGuideLine WHERE OrderType =" + nOrderType + " )";
                ;
            }
            #endregion

            #region Product IDs
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductID IN (" + sProductIDs + ") "; //DUProGuideLineID IN ( SELECT DUProGuideLineID FROM DUProGuideLineDetail WHERE ProductID IN 
            }
            #endregion

            #region IsInHouse = 0
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " DyeingOrderID IN (SELECT DyeingOrderID FROM DUProGuideLine WHERE OrderType IN (SELECT DUOrderSetup.OrderType FROM DUOrderSetup WHERE ISNULL(IsInHouse,0) = 0))";
            #endregion

            sReturn = sReturn1 + sReturn + " GROUP BY DyeingOrderID, OrderNo, OrderDate, ProductID, ProductCode, ProductName, ContractorID, ContractorName, StyleNo";
            return sReturn;
        }

        public void Print_ReportXL_Details(string sTempString, int BUID, int trackid, int formate)
        {
            List<LotParent> oLotParents = new List<LotParent>();
            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            List<DUProGuideLineDetail> oDUProGuideLineDetails_Receive = new List<DUProGuideLineDetail>();
            List<DUProGuideLineDetail> oDUProGuideLineDetails_Return = new List<DUProGuideLineDetail>();
            List<DURequisitionDetail> oDURequisitionDetails_SRS = new List<DURequisitionDetail>();
            List<DURequisitionDetail> oDURequisitionDetails_SRM = new List<DURequisitionDetail>();
            List<DUDeliveryChallanDetail> oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
            List<DUReturnChallanDetail> oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
            //List<DUSoftWinding> oDUSoftWindings = new List<DUSoftWinding>();

            if (string.IsNullOrEmpty(sTempString))
            {
                _oDUProductionStatusReports = new List<DUProductionStatusReport>();
            }
            else
            {
                string sSQL = GetSQL_DODetails(sTempString);

                oDyeingOrderReports = DyeingOrderReport.Gets(sSQL + " ORDER BY ContractorName", ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oDyeingOrderReports.Any())
                {
                    string sDyeingOrderIDs = string.Join(",", oDyeingOrderReports.Select(x => x.DyeingOrderID));
                    oDUProGuideLineDetails_Receive = DUProGuideLineDetail.Gets("SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLine WHERE ISNULL(ReceiveByID,0) !=0 AND ISNULL(InOutType,0) != " + (int)EnumInOutType.Disburse + " AND  DyeingOrderID IN (" + sDyeingOrderIDs + ")) ORDER BY ProductID", (int)Session[SessionInfo.currentUserID]);
                    oDUProGuideLineDetails_Return = DUProGuideLineDetail.Gets("SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLine WHERE ISNULL(ReceiveByID,0) !=0 AND ISNULL(InOutType,0) = " + (int)EnumInOutType.Disburse + " AND  DyeingOrderID IN (" + sDyeingOrderIDs + ")) ORDER BY ProductID", (int)Session[SessionInfo.currentUserID]);

                    oDURequisitionDetails_SRS = DURequisitionDetail.Gets("SELECT * FROM View_DURequisitionDetail WHERE DyeingOrderID IN (" + sDyeingOrderIDs + ")  AND DURequisitionID IN (SELECT DURequisitionID FROM DURequisition WHERE ISNULL(ReceiveByID,0)<>0 and RequisitionType=" + (int)EnumInOutType.Receive + " ) ", (int)Session[SessionInfo.currentUserID]);
                    oDURequisitionDetails_SRM = DURequisitionDetail.Gets("SELECT * FROM View_DURequisitionDetail WHERE DyeingOrderID IN (" + sDyeingOrderIDs + ")  AND DURequisitionID IN (SELECT DURequisitionID FROM DURequisition WHERE ISNULL(ReceiveByID,0)<>0 and RequisitionType=" + (int)EnumInOutType.Disburse + " ) ", (int)Session[SessionInfo.currentUserID]);
                    oLotParents = LotParent.Gets("SELECT * FROM View_LotParent WHERE DyeingOrderID IN (" + sDyeingOrderIDs + ") OR DyeingOrderID_Out IN (" + sDyeingOrderIDs + ")", (int)Session[SessionInfo.currentUserID]);
                   
                    oDUDeliveryChallanDetails = DUDeliveryChallanDetail.Gets("SELECT * FROM View_DUDeliveryChallanDetail WHERE OrderID IN (" + sDyeingOrderIDs + ")", (int)Session[SessionInfo.currentUserID]);

                    if (oDUDeliveryChallanDetails.Any())
                        oDUReturnChallanDetails = DUReturnChallanDetail.Gets("SELECT * FROM View_DUReturnChallanDetail WHERE DUDeliveryChallanDetailID IN (" + string.Join(",", oDUDeliveryChallanDetails.Select(x => x.DUDeliveryChallanDetailID)) + ")", (int)Session[SessionInfo.currentUserID]);
                }
            }

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            #region Export Excel
            int nRowIndex = 2, nEndRow = 0, nStartCol = 2, nEndCol = 16;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Yarn Receiving Report");
                sheet.Name = "Yarn Receiving Report";

                #region Sheet Columns

                //Order No	Order Date	Buyer Name	Style No	Yarn Type	Order Qty	Lot No	GRN Date	GRN Qty	SRM Qty	Transfer Qty	Received Due	Issued To S/W	Transfer Out	Return Qty	Balance	Date	Challan No	Lot No	Delivery Qty	Return Qty	

                sheet.Column(nStartCol++).Width = 8;//SL
                sheet.Column(nStartCol++).Width = 20;//Order No
                sheet.Column(nStartCol++).Width = 20;//Order Date
                sheet.Column(nStartCol++).Width = 30;//Buyer Name
                sheet.Column(nStartCol++).Width = 20;//Style No

                sheet.Column(nStartCol++).Width = 30;//Yarn Type
                sheet.Column(nStartCol++).Width = 20;//Order Qty

                sheet.Column(nStartCol++).Width = 20;//Lot No
                sheet.Column(nStartCol++).Width = 20;//GRN Date
                sheet.Column(nStartCol++).Width = 20;//GRN Qty
                sheet.Column(nStartCol++).Width = 20;//SRM Qty
                sheet.Column(nStartCol++).Width = 20;//Transfer Qty

                sheet.Column(nStartCol++).Width = 20;//Due
                sheet.Column(nStartCol++).Width = 20;//Issued To S/W
                sheet.Column(nStartCol++).Width = 20;//Transfer Out
                sheet.Column(nStartCol++).Width = 20;//Return
                sheet.Column(nStartCol++).Width = 20;//Balance

                sheet.Column(nStartCol++).Width = 20;//DC Date
                sheet.Column(nStartCol++).Width = 20;//Challan No
                sheet.Column(nStartCol++).Width = 20;//Lot No
                sheet.Column(nStartCol++).Width = 20;//Delivery Qty 
                sheet.Column(nStartCol).Width = 20;//Return Qty 
                nEndCol = nStartCol;
                #endregion

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                nStartCol = 2;
                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Yarn Receiving Report";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                if (GOrderDate > 0)
                {
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Order Date Between " + d1.ToString("dd MMM yyyy") + " to " + d2.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.Size = 10;
                    nRowIndex = nRowIndex + 1;
                }

                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region HEADER
                nStartCol = 2;

                ExcelTool.FillCellMerge(ref sheet, "Order Info", nRowIndex, nRowIndex, nStartCol, nStartCol += 6, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                ExcelTool.FillCellMerge(ref sheet, "Received Status", nRowIndex, nRowIndex, ++nStartCol, nStartCol+=5, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                ExcelTool.FillCellMerge(ref sheet, "Issued Info", nRowIndex, nRowIndex, ++nStartCol, nStartCol+=3, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                ExcelTool.FillCellMerge(ref sheet, "Delivery Status", nRowIndex, nRowIndex, ++nStartCol, nStartCol += 4, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                nRowIndex++;

                nStartCol = 2;

                var sHeaders = ("SL No,Order No,Order Date,Buyer Name,Style No,Yarn Type,Order Qty,Lot No,GRN Date,GRN Qty,SRM Qty,Transfer Qty,Received Due,Issued To S/W,Transfer Out,Return Qty,Balance,Challan Date,Challan No,Lot No,Delivery Qty,Return Qty").Split(',').ToList();

                foreach (var oHead in sHeaders)
                {
                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = oHead; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                nRowIndex = nRowIndex + 1;
                #endregion


                #region DATA
                var data = oDyeingOrderReports.GroupBy(x => new { x.DyeingOrderID, x.ProductID }, (key, grp) => new
                {
                    DyeingOrderID = key.DyeingOrderID,
                    ProductID = key.ProductID,
                    Results = grp.ToList()
                });

                foreach (var oData in data)
                {
                    nSL = 0; int nMaxRow = 0;
                    foreach (var oItem in oData.Results)
                    {
                        #region Gets Data Product Wise
                        List<DyeingOrderReport> oItem_oDyeingOrderReports = new List<DyeingOrderReport>();
                        List<LotParent> oItem_oLotParents_In = new List<LotParent>();
                        List<LotParent> oItem_oLotParents_Out = new List<LotParent>();
                        List<DURequisitionDetail> oItem_oDURequisitionDetails_SRM = new List<DURequisitionDetail>();
                        List<DURequisitionDetail> oItem_oDURequisitionDetails_SRS = new List<DURequisitionDetail>();
                        List<DUProGuideLineDetail> oItem_oDUProGuideLineDetails_Receive = new List<DUProGuideLineDetail>();
                        List<DUProGuideLineDetail> oItem_oDUProGuideLineDetails_Return = new List<DUProGuideLineDetail>();
                        List<DUDeliveryChallanDetail> oItem_oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();

                        oItem_oDyeingOrderReports = oDyeingOrderReports.Where(p => p.DyeingOrderID == oItem.DyeingOrderID && p.ProductID == oItem.ProductID).ToList();
                        oItem_oLotParents_In = oLotParents.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();
                        oItem_oLotParents_Out = oLotParents.Where(x => x.DyeingOrderID_Out == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();
                        oItem_oDURequisitionDetails_SRM = oDURequisitionDetails_SRM.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();
                        oItem_oDURequisitionDetails_SRS = oDURequisitionDetails_SRS.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();
                        oItem_oDUProGuideLineDetails_Return = oDUProGuideLineDetails_Return.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();
                        oItem_oDUProGuideLineDetails_Receive = oDUProGuideLineDetails_Receive.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).OrderBy(z=>z.LotID).ToList();
                        oItem_oDUDeliveryChallanDetails = oDUDeliveryChallanDetails.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();

                        int nRowSpan = (oItem_oDyeingOrderReports == null) ? 0 : oItem_oDyeingOrderReports.Count();
                        //if (((oItem_oLotParents_In == null) ? 0 : oItem_oLotParents_In.Count()) > nRowSpan) nRowSpan = oItem_oLotParents_In.Count;
                        //if (((oItem_oLotParents_Out == null) ? 0 : oItem_oLotParents_Out.Count()) > nRowSpan) nRowSpan = oItem_oLotParents_Out.Count;
                        //if (((oItem_oDURequisitionDetails_SRM == null) ? 0 : oItem_oDURequisitionDetails_SRM.Count()) > nRowSpan) nRowSpan = oItem_oDURequisitionDetails_SRM.Count;
                        //if (((oItem_oDURequisitionDetails_SRS == null) ? 0 : oItem_oDURequisitionDetails_SRS.Count()) > nRowSpan) nRowSpan = oItem_oDURequisitionDetails_SRS.Count;
                        //if (((oItem_oDUProGuideLineDetails_Return == null) ? 0 : oItem_oDUProGuideLineDetails_Return.Count()) > nRowSpan) nRowSpan = oItem_oDUProGuideLineDetails_Return.Count;
                        if (((oItem_oDUProGuideLineDetails_Receive == null) ? 0 : oItem_oDUProGuideLineDetails_Receive.Count()) > nRowSpan) nRowSpan = oItem_oDUProGuideLineDetails_Receive.Count;
                        if (((oItem_oDUDeliveryChallanDetails == null) ? 0 : oItem_oDUDeliveryChallanDetails.Count()) > nRowSpan) nRowSpan = oItem_oDUDeliveryChallanDetails.Count;

                        nMaxRow = nRowSpan;
                        nRowSpan = nRowSpan - 1;
                        if (nRowSpan < 0) nRowSpan = 0;
                        #endregion

                        nSL++; nStartCol = 2;
                        ExcelTool.Formatter = "#,##0.00;(#,##0.00)";

                        #region Order Info
                        ExcelTool.FillCellMerge(ref sheet, nSL.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oItem.OrderNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oItem.OrderDateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oItem.ContractorName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oItem.StyleNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oItem.Qty.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                        #endregion

                        int nR = 0;
                        if (nMaxRow > 0)
                        {
                            int nTempRowIndex = nRowIndex; //0; 
                            int nLotID = -999; int nLotRowIndex = 0;
                            #region Receive & Issued info
                            foreach (var obj in oItem_oDUProGuideLineDetails_Receive)
                            {
                                nR++; nStartCol = 9; 
                                nLotRowIndex = (oItem_oDUProGuideLineDetails_Receive.Where(x => x.LotID == obj.LotID).Count()-1);
                                if (nLotRowIndex <= 0) nLotRowIndex = 0;

                                //if (nLotID != obj.LotID)
                                //    ExcelTool.FillCellMerge(ref sheet, obj.LotNo, nRowIndex, nRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                                //else
                                //    nStartCol = 10;
                                if (nLotID != obj.LotID)
                                    ExcelTool.FillCellMerge(ref sheet, obj.LotNo, nTempRowIndex, nTempRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                                else
                                    nStartCol = 10;  
                                //ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ReceiveDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                                //ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                                //ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.Qty.ToString(), false, ExcelHorizontalAlignment.Right, false, false);
                                ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, obj.ReceiveDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                                ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, obj.Qty.ToString(), false, ExcelHorizontalAlignment.Right, false, false);

                                //if (nLotID != obj.LotID)
                                //{
                                //    #region SRM
                                //    if (oItem_oDURequisitionDetails_SRM != null)
                                //        ExcelTool.FillCellMerge(ref sheet, oItem_oDURequisitionDetails_SRM.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty).ToString(), nRowIndex, nRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                //    else
                                //        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                //    #endregion
                                //}                                
                                if (nLotID != obj.LotID)
                                {
                                    #region SRM
                                    if (oItem_oDURequisitionDetails_SRM != null)
                                        ExcelTool.FillCellMerge(ref sheet, oItem_oDURequisitionDetails_SRM.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty).ToString(), nTempRowIndex, nTempRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                    else
                                        ExcelTool.FillCellMerge(ref sheet, "", nTempRowIndex, nTempRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                    #endregion
                                } 

                                //if (nLotID != obj.LotID)
                                //{
                                //    #region Transfer In
                                //    if (oItem_oLotParents_In != null)
                                //        ExcelTool.FillCellMerge(ref sheet, oItem_oLotParents_In.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty).ToString(), nRowIndex, nRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                //    else
                                //        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                //    #endregion
                                //}
                                if (nLotID != obj.LotID)
                                {
                                    #region Transfer In
                                    if (oItem_oLotParents_In != null)
                                        ExcelTool.FillCellMerge(ref sheet, oItem_oLotParents_In.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty).ToString(), nTempRowIndex, nTempRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                    else
                                        ExcelTool.FillCellMerge(ref sheet, "", nTempRowIndex, nTempRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                    #endregion
                                }

                                //if (nLotID != obj.LotID)
                                //{
                                //    #region Received Due
                                //    var nReceived_Due = oItem_oDyeingOrderReports.Sum(x => x.Qty) - oItem_oDUProGuideLineDetails_Receive.Sum(x => x.Qty);
                                //    //if (oItem_oDURequisitionDetails_SRM != null) nReceived_Due = nReceived_Due - oItem_oDURequisitionDetails_SRM.Sum(x => x.Qty);
                                //    if (oItem_oLotParents_In != null) nReceived_Due = nReceived_Due - oItem_oLotParents_In.Sum(x => x.Qty);

                                //    //ExcelTool.FillCellMerge(ref sheet, nReceived_Due, nRowIndex, nRowIndex + nLotRowIndex, nStartCol, nStartCol++, true);
                                //    ExcelTool.FillCellMerge(ref sheet, nReceived_Due.ToString(), nRowIndex, nRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                //    #endregion
                                //}
                                if (nLotID != obj.LotID)
                                {
                                    #region Received Due
                                    var nReceived_Due = oItem_oDyeingOrderReports.Sum(x => x.Qty) - oItem_oDUProGuideLineDetails_Receive.Sum(x => x.Qty);
                                    if (oItem_oLotParents_In != null) nReceived_Due = nReceived_Due - oItem_oLotParents_In.Sum(x => x.Qty);
                                    ExcelTool.FillCellMerge(ref sheet, nReceived_Due.ToString(), nTempRowIndex, nTempRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                    #endregion
                                }
                                

                                var nBalance = obj.Qty;
                                //if (nLotID != obj.LotID)
                                //{
                                //    #region SRS
                                //    if (oItem_oDURequisitionDetails_SRS != null)
                                //    {
                                //        ExcelTool.FillCellMerge(ref sheet, oItem_oDURequisitionDetails_SRS.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty).ToString(), nRowIndex, nRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                //        nBalance = nBalance - oItem_oDURequisitionDetails_SRS.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty);
                                //    }
                                //    else
                                //    {
                                //        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                //    }
                                //    #endregion
                                //}
                                if (nLotID != obj.LotID)
                                {
                                    #region SRS
                                    if (oItem_oDURequisitionDetails_SRS != null)
                                    {
                                        ExcelTool.FillCellMerge(ref sheet, oItem_oDURequisitionDetails_SRS.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty).ToString(), nTempRowIndex, nTempRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                        nBalance = nBalance - oItem_oDURequisitionDetails_SRS.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty);
                                    }
                                    else
                                    {
                                        ExcelTool.FillCellMerge(ref sheet, "", nTempRowIndex, nTempRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                    }
                                    #endregion
                                }

                                //if (nLotID != obj.LotID)
                                //{
                                //    #region Transfer Out
                                //    if (oItem_oLotParents_Out != null)
                                //    {
                                //        ExcelTool.FillCellMerge(ref sheet, oItem_oLotParents_Out.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty).ToString(), nRowIndex, nRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                //        nBalance = nBalance - oItem_oLotParents_Out.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty);
                                //    }
                                //    else
                                //    {
                                //        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                //    }
                                //    #endregion
                                //}
                                if (nLotID != obj.LotID)
                                {
                                    #region Transfer Out
                                    if (oItem_oLotParents_Out != null)
                                    {
                                        ExcelTool.FillCellMerge(ref sheet, oItem_oLotParents_Out.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty).ToString(), nTempRowIndex, nTempRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                        nBalance = nBalance - oItem_oLotParents_Out.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty);
                                    }
                                    else
                                    {
                                        ExcelTool.FillCellMerge(ref sheet, "", nTempRowIndex, nTempRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                    }
                                    #endregion
                                }

                                //if (nLotID != obj.LotID)
                                //{
                                //    #region Return
                                //    if (oItem_oDUProGuideLineDetails_Return != null)
                                //    {
                                //        ExcelTool.FillCellMerge(ref sheet, oItem_oDUProGuideLineDetails_Return.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty).ToString(), nRowIndex, nRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                //        nBalance = nBalance - oItem_oDUProGuideLineDetails_Return.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty);
                                //    }
                                //    else
                                //    {
                                //        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                //    }
                                //    #endregion
                                //}
                                if (nLotID != obj.LotID)
                                {
                                    #region Return
                                    if (oItem_oDUProGuideLineDetails_Return != null)
                                    {
                                        ExcelTool.FillCellMerge(ref sheet, oItem_oDUProGuideLineDetails_Return.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty).ToString(), nTempRowIndex, nTempRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                        nBalance = nBalance - oItem_oDUProGuideLineDetails_Return.Where(x => x.LotID == obj.LotID).Sum(x => x.Qty);
                                    }
                                    else
                                    {
                                        ExcelTool.FillCellMerge(ref sheet, "", nTempRowIndex, nTempRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                    }
                                    #endregion
                                }

                                //if (nLotID != obj.LotID)
                                //{
                                //    #region Balance
                                //    ExcelTool.FillCellMerge(ref sheet, nBalance.ToString(), nRowIndex, nRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                //    #endregion
                                //}
                                if (nLotID != obj.LotID)
                                {
                                    #region Balance
                                    ExcelTool.FillCellMerge(ref sheet, nBalance.ToString(), nTempRowIndex, nTempRowIndex + nLotRowIndex, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                    #endregion
                                }

                                nTempRowIndex++; nLotID = obj.LotID;
                            }
                            if (nR < nMaxRow)
                            {
                                while (nMaxRow != nR)
                                {
                                    //nR++; nStartCol = 9;
                                    //ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    //ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    //ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                                    //ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    //ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    //ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    //ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    //ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    //ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    //ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    //ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    //nTempRowIndex++;
                                    nR++; nStartCol = 9;
                                    ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                                    ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    nTempRowIndex++;
                                }
                            }

                            #endregion

                            #region Delivery Status
                            nR = 0; nTempRowIndex = 0;
                            foreach (var obj in oItem_oDUDeliveryChallanDetails)
                            {
                                nR++; nStartCol = 19;
                                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ChallanDate, false, ExcelHorizontalAlignment.Center, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ChallanNo, false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.LotNo, false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.Qty.ToString(), false, ExcelHorizontalAlignment.Right, false, false);
                                var nReturn_Qty = oDUReturnChallanDetails.Where(x => x.DUDeliveryChallanDetailID == obj.DUDeliveryChallanDetailID).Sum(x => x.Qty);
                                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, nReturn_Qty.ToString(), false, ExcelHorizontalAlignment.Right, false, false);
                                nTempRowIndex++;
                            }
                            if (nR < nMaxRow)
                            {
                                while (nMaxRow != nR)
                                {
                                    nR++; nStartCol = 19;
                                    ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    nTempRowIndex++;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            nStartCol = 9;
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                        }

                        nRowIndex += nMaxRow;
                    }
                    
                }
                #endregion

                #region Comment Code
                //#region Data

                //#region Insert Revised Products
                //oLotParents.ForEach(x =>
                //{
                //    var oDORpts = oDyeingOrderReports.Where(d => d.DyeingOrderID == x.DyeingOrderID && d.ProductID == x.ProductID);
                //    if (oDORpts == null || oDORpts.Count() == 0)
                //    {
                //        oDyeingOrderReports.Add(new DyeingOrderReport()
                //        {
                //            DyeingOrderID = x.DyeingOrderID,
                //            OrderNo = x.DyeingOrderNo,
                //            OrderDate = x.OrderDate,
                //            ProductID = x.ProductID,
                //            ProductName = x.ProductName
                //        });
                //    }
                //});
                //#endregion





                //int nDOID = -99;
                //foreach (DyeingOrderReport oItem in oDyeingOrderReports.OrderBy(x => x.DyeingOrderID))
                //{
                //    int nRowSpan = oDyeingOrderReports.Where(p => p.DyeingOrderID == oItem.DyeingOrderID).Count() - 1;

                //    //Order No	Order Date	Buyer Name	Style No	Yarn Type	Order Qty	Lot No	GRN Date	GRN Qty	SRM Qty	Transfer Qty	Received Due	Issued To S/W	Transfer Out	Return Qty	Balance	Date	Challan No	Lot No	Delivery Qty	Return Qty	

                //    nStartCol = 2;

                //    #region Dyeing Order Info
                //    if (nDOID != oItem.DyeingOrderID)
                //    {
                //        nSL++;
                //        ExcelTool.FillCellMerge(ref sheet, nSL.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                //        ExcelTool.FillCellMerge(ref sheet, oItem.OrderNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                //        ExcelTool.FillCellMerge(ref sheet, oItem.OrderDateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                //        ExcelTool.FillCellMerge(ref sheet, oItem.ContractorName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                //        ExcelTool.FillCellMerge(ref sheet, oItem.StyleNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                //    }
                //    #endregion
                //    nStartCol = 7;

                //    #region Gets Data Product Wise 
                //    List<DyeingOrderReport> oItem_oDyeingOrderReports = new List<DyeingOrderReport>();
                //    List<LotParent> oItem_oLotParents_In = new List<LotParent>();
                //    List<LotParent> oItem_oLotParents_Out = new List<LotParent>();
                //    List<DURequisitionDetail> oItem_oDURequisitionDetails_SRM = new List<DURequisitionDetail>();
                //    List<DURequisitionDetail> oItem_oDURequisitionDetails_SRS = new List<DURequisitionDetail>();
                //    List<DUProGuideLineDetail> oItem_oDUProGuideLineDetails_Receive = new List<DUProGuideLineDetail>();
                //    List<DUProGuideLineDetail> oItem_oDUProGuideLineDetails_Return = new List<DUProGuideLineDetail>();
                //    List<DUDeliveryChallanDetail> oItem_oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();

                //    oItem_oDyeingOrderReports = oDyeingOrderReports.Where(p => p.DyeingOrderID == oItem.DyeingOrderID && p.ProductID == oItem.ProductID).ToList();
                //    oItem_oLotParents_In = oLotParents.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();
                //    oItem_oLotParents_Out = oLotParents.Where(x => x.DyeingOrderID_Out == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();
                //    oItem_oDURequisitionDetails_SRM = oDURequisitionDetails_SRM.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();
                //    oItem_oDURequisitionDetails_SRS = oDURequisitionDetails_SRS.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();
                //    oItem_oDUProGuideLineDetails_Return = oDUProGuideLineDetails_Return.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();
                //    oItem_oDUProGuideLineDetails_Receive = oDUProGuideLineDetails_Receive.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();
                //    oItem_oDUDeliveryChallanDetails = oDUDeliveryChallanDetails.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).ToList();

                //    nRowSpan = (oItem_oDyeingOrderReports == null) ? 0 : oItem_oDyeingOrderReports.Count();
                //    //if (((oItem_oLotParents_In == null) ? 0 : oItem_oLotParents_In.Count()) > nRowSpan) nRowSpan = oItem_oLotParents_In.Count;
                //    //if (((oItem_oLotParents_Out == null) ? 0 : oItem_oLotParents_Out.Count()) > nRowSpan) nRowSpan = oItem_oLotParents_Out.Count;
                //    if (((oItem_oDURequisitionDetails_SRM == null) ? 0 : oItem_oDURequisitionDetails_SRM.Count()) > nRowSpan) nRowSpan = oItem_oDURequisitionDetails_SRM.Count;
                //    if (((oItem_oDURequisitionDetails_SRS == null) ? 0 : oItem_oDURequisitionDetails_SRS.Count()) > nRowSpan) nRowSpan = oItem_oDURequisitionDetails_SRS.Count;
                //    if (((oItem_oDUProGuideLineDetails_Return == null) ? 0 : oItem_oDUProGuideLineDetails_Return.Count()) > nRowSpan) nRowSpan = oItem_oDUProGuideLineDetails_Return.Count;
                //    if (((oItem_oDUProGuideLineDetails_Receive == null) ? 0 : oItem_oDUProGuideLineDetails_Receive.Count()) > nRowSpan) nRowSpan = oItem_oDUProGuideLineDetails_Receive.Count;
                //    if (((oItem_oDUDeliveryChallanDetails == null) ? 0 : oItem_oDUDeliveryChallanDetails.Count()) > nRowSpan) nRowSpan = oItem_oDUDeliveryChallanDetails.Count;

                //    nRowSpan = nRowSpan - 1;
                //    #endregion

                //    ExcelTool.FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                //    ExcelTool.FillCellMerge(ref sheet, oItem.Qty, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, true);

                //    int nTempRowIndex = 0, nProductID = 0 ;

                //    var nCol_Termp = nStartCol;
                //        nTempRowIndex = 0;

                //    foreach (var oProduct in oItem_oDyeingOrderReports)
                //    {
                //        #region Lot Wise Print
                //        foreach (var oDUPGL_Receive in oItem_oDUProGuideLineDetails_Receive)
                //        {
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oDUPGL_Receive.LotNo, false, ExcelHorizontalAlignment.Left, false, false);
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oDUPGL_Receive.ReceiveDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oDUPGL_Receive.Qty.ToString(), true, ExcelHorizontalAlignment.Left, false, false);

                //            #region SRM
                //            if (oItem_oDURequisitionDetails_SRM != null)
                //            {
                //                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oItem_oDURequisitionDetails_SRM.Where(x => x.LotID == oDUPGL_Receive.LotID).Sum(x => x.Qty).ToString(), true, ExcelHorizontalAlignment.Left, false, false);
                //            }
                //            else
                //                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                //            #endregion

                //            #region Transfer In
                //            if (oItem_oLotParents_In != null)
                //            {
                //                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oItem_oLotParents_In.Where(x => x.LotID == oDUPGL_Receive.LotID).Sum(x => x.Qty).ToString(), true, ExcelHorizontalAlignment.Left, false, false);
                //            }
                //            else
                //                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                //            #endregion

                //            #region Received Due
                //            if (oDUPGL_Receive.ProductID != nProductID)
                //            {
                //                var nReceived_Due = oItem_oDyeingOrderReports.Sum(x => x.Qty) - oItem_oDUProGuideLineDetails_Receive.Sum(x => x.Qty);

                //                //if (oItem_oDURequisitionDetails_SRM != null) nReceived_Due = nReceived_Due - oItem_oDURequisitionDetails_SRM.Sum(x => x.Qty);
                //                if (oItem_oLotParents_In != null) nReceived_Due = nReceived_Due - oItem_oLotParents_In.Sum(x => x.Qty);

                //                ExcelTool.FillCellMerge(ref sheet, nReceived_Due, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, true);
                //            }
                //            else nStartCol++;
                //            #endregion

                //            var nBalance = oDUPGL_Receive.Qty;

                //            #region SRS
                //            if (oItem_oDURequisitionDetails_SRS != null)
                //            {
                //                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oItem_oDURequisitionDetails_SRS.Where(x => x.LotID == oDUPGL_Receive.LotID).Sum(x => x.Qty).ToString(), true, ExcelHorizontalAlignment.Left, false, false);
                //                nBalance = nBalance - oItem_oDURequisitionDetails_SRS.Where(x => x.LotID == oDUPGL_Receive.LotID).Sum(x => x.Qty);
                //            }
                //            else
                //            {
                //                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                //            }
                //            #endregion

                //            #region Transfer Out
                //            if (oItem_oLotParents_Out != null)
                //            {
                //                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oItem_oLotParents_Out.Where(x => x.LotID == oDUPGL_Receive.LotID).Sum(x => x.Qty).ToString(), true, ExcelHorizontalAlignment.Left, false, false);
                //                nBalance = nBalance - oItem_oLotParents_Out.Where(x => x.LotID == oDUPGL_Receive.LotID).Sum(x => x.Qty);
                //            }
                //            else
                //            {
                //                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                //            }
                //            #endregion

                //            #region Return
                //            if (oItem_oDUProGuideLineDetails_Return != null)
                //            {
                //                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oItem_oDUProGuideLineDetails_Return.Where(x => x.LotID == oDUPGL_Receive.LotID).Sum(x => x.Qty).ToString(), true, ExcelHorizontalAlignment.Left, false, false);
                //                nBalance = nBalance - oItem_oDUProGuideLineDetails_Return.Where(x => x.LotID == oDUPGL_Receive.LotID).Sum(x => x.Qty);
                //            }
                //            else
                //            {
                //                ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                //            }
                //            #endregion

                //            nProductID = oDUPGL_Receive.ProductID;
                //            nTempRowIndex++;
                //        }
                //        #endregion

                //        nStartCol = 19;

                //        #region Challan Report
                //        foreach (var oChallan in oItem_oDUDeliveryChallanDetails)
                //        {
                //            //Date	Challan No	Lot No	Delivery Qty	Return Qty

                //            nTempRowIndex = 0;
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oChallan.ChallanDate, false, ExcelHorizontalAlignment.Left, false, false);
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oChallan.ChallanNo, false, ExcelHorizontalAlignment.Left, false, false);
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oChallan.LotNo, false, ExcelHorizontalAlignment.Left, false, false);
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oChallan.Qty.ToString(), true, ExcelHorizontalAlignment.Left, false, false);

                //            var nReturn_Qty = oDUReturnChallanDetails.Where(x=>x.DUDeliveryChallanDetailID == oChallan.DUDeliveryChallanDetailID).Sum(x=>x.Qty);
                //            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, nReturn_Qty.ToString(), true, ExcelHorizontalAlignment.Left, false, false);

                //            nTempRowIndex++;
                //        }
                //        #endregion

                //    }

                //    nDOID = oItem.DyeingOrderID;

                //    nRowIndex = nRowIndex + nRowSpan + 1;
                //    nEndRow = nRowIndex;
                //    //nRowIndex++;
                //}
                //#endregion
                #endregion

                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Yarn_Receiving_Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion

        }
        #endregion

        public System.Drawing.Image GetCompanyLogo(Company oCompany)
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
    }
}
