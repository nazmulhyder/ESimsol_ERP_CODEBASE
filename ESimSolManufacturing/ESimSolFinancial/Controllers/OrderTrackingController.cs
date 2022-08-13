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
    public class OrderTrackingController : Controller
    {
        OrderTracking _oOrderTracking = new OrderTracking();
        List<OrderTracking> _oOrderTrackings = new List<OrderTracking>();
        ReportConfigure _oReportConfigure = new ReportConfigure();
        string _sDateRange = "";
        public ActionResult ViewOrderTrackings(int ProductNature, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oOrderTrackings = new List<OrderTracking>();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            ViewBag.ProductNature = ProductNature;
            ViewBag.ReportConfigure = _oReportConfigure.Get((int)EnumReportType.Order_Tracking_ColorView, (int)Session[SessionInfo.currentUserID]);
            return View(_oOrderTrackings);
        }

        #region Search
        [HttpPost]
        public JsonResult AdvanchSearch(OrderTracking oOrderTracking)
        {
            _oOrderTrackings = new List<OrderTracking>();
            List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            try
            {
                string sSQL = MakeSQL(oOrderTracking);
                _oOrderTrackings = OrderTracking.Gets(sSQL, oOrderTracking.LayoutType, oOrderTracking.bIsYetToDO, oOrderTracking.bIsYetToChallan, oOrderTracking.bIsYetToChallanWithDOEntry, oOrderTracking.bIsPTUTransferQty,(int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oOrderTrackings = new List<OrderTracking>();
            }
            var jsonResult = Json(_oOrderTrackings, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(OrderTracking oOrderTracking)
        {
            string sParams = oOrderTracking.Params;
            int ncboDODate = 0;
            DateTime dFromDODate = DateTime.Today;
            DateTime dToDODate = DateTime.Today;
            int cboPIDate = 0;
            DateTime dFromPIDate = DateTime.Today;
            DateTime dToPIDate = DateTime.Today;

            int ncboPODate = 0;
            DateTime dFromPODate = DateTime.Today;
            DateTime dToPODate = DateTime.Today;
            int ncboChallanDate = 0;
            DateTime dFromChallanDate = DateTime.Today;
            DateTime dToChallanDate = DateTime.Today;

            string sPINo = "";
            string sDONo = "";
            string sMKTEmpIDs = "";
            string sLCNo = "";
            string sPONo = "";
            string sChallanNo = "";
            string sRawMaterialIDs = "";
        
            if (!string.IsNullOrEmpty(sParams))
            {
                cboPIDate = Convert.ToInt32(sParams.Split('~')[0]);
                if (cboPIDate > 0)
                {
                    dFromPIDate = Convert.ToDateTime(sParams.Split('~')[1]);
                    dToPIDate = Convert.ToDateTime(sParams.Split('~')[2]);
                }

                ncboPODate = Convert.ToInt32(sParams.Split('~')[3]);
                if (ncboPODate > 0)
                {
                    dFromPODate = Convert.ToDateTime(sParams.Split('~')[4]);
                    dToPODate = Convert.ToDateTime(sParams.Split('~')[5]);
                }

                ncboDODate = Convert.ToInt32(sParams.Split('~')[6]);
                if (ncboDODate > 0)
                {
                    dFromDODate = Convert.ToDateTime(sParams.Split('~')[7]);
                    dToDODate = Convert.ToDateTime(sParams.Split('~')[8]);
                }

                ncboChallanDate = Convert.ToInt32(sParams.Split('~')[9]);
                if (ncboChallanDate > 0)
                {
                    dFromChallanDate = Convert.ToDateTime(sParams.Split('~')[10]);
                    dToChallanDate = Convert.ToDateTime(sParams.Split('~')[11]);
                }

                oOrderTracking.ContractorName = Convert.ToString(sParams.Split('~')[12]);
                oOrderTracking.ProductName = Convert.ToString(sParams.Split('~')[13]);
                sPINo = Convert.ToString(sParams.Split('~')[14]);
                sDONo = Convert.ToString(sParams.Split('~')[15]);
                sPONo = Convert.ToString(sParams.Split('~')[16]);
                sChallanNo = Convert.ToString(sParams.Split('~')[17]);
                sLCNo = Convert.ToString(sParams.Split('~')[18]);
                sMKTEmpIDs = Convert.ToString(sParams.Split('~')[19]);
                sRawMaterialIDs = Convert.ToString(sParams.Split('~')[20]);
                oOrderTracking.BUID = Convert.ToInt32(sParams.Split('~')[21]);
                oOrderTracking.ProductNatureInInt = Convert.ToInt32(sParams.Split('~')[22]);
                oOrderTracking.BuyerName = Convert.ToString(sParams.Split('~')[23]);
                
            }


            string sReturn1 = "SELECT ExportSCDetailID FROM ExportSCDetail ";
            string sReturn = "";

            string sForPickerLoadReturn1 = "SELECT ExportSCDetailID FROM ExportSCDetail ";
            string sForPickerLoadReturn = "";

            #region Business Unit
            if (oOrderTracking.BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM View_ExportSC WHERE BUID = " + oOrderTracking.BUID + "   AND ProductNature = " + oOrderTracking.ProductNatureInInt + " )";                
            }
            #endregion

            #region Contractor
            if (!String.IsNullOrEmpty(oOrderTracking.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM View_ExportSC WHERE ContractorID IN(" + oOrderTracking.ContractorName + "))";
            }
            #endregion

            #region Mother Buyer
            if (!String.IsNullOrEmpty(oOrderTracking.BuyerName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM View_ExportSC WHERE BuyerID IN(" + oOrderTracking.BuyerName + "))";                
            }
            #endregion

            #region Product Id
            if (!String.IsNullOrEmpty(oOrderTracking.ProductName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID in(" + oOrderTracking.ProductName + ")";                
            }
            #endregion

            #region Raw Material
            if (!String.IsNullOrEmpty(sRawMaterialIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ExportSCID IN (SELECT ExportSCID FROM PTUUnit2 WHERE PTUUnit2ID IN (SELECT PTUUnit2ID FROM ProductionSheet WHERE ProductionSheetID In (SELECT DISTINCT ProductID FROM ProductionRecipe WHERE ProductID IN(" + sRawMaterialIDs + ")))))";                
            }
            #endregion
            
            #region Mkt person
            if (!String.IsNullOrEmpty(sMKTEmpIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExportSCID IN(SELECT ExportSCID FROM View_ExportSC WHERE MKTEmpID IN (" + sMKTEmpIDs + "))";                
            }
            #endregion
            sForPickerLoadReturn = sReturn; //don't remove this picker

            #region PINo No
            if (!string.IsNullOrEmpty(sPINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM View_ExportSC WHERE PINo LIKE ''%" + sPINo + "%'')";

                Global.TagSQL(ref sForPickerLoadReturn);
                sForPickerLoadReturn = sForPickerLoadReturn + "ExportSCID IN(SELECT ExportSCID FROM View_ExportSC WHERE PINo LIKE '%" + sPINo + "%')";
            }
            #endregion

            #region DO No
            if (!string.IsNullOrEmpty(sDONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ExportSCID IN(SELECT RefID FROM DeliveryOrder WHERE DONo LIKE ''%" + sDONo + "%'')";
                
                Global.TagSQL(ref sForPickerLoadReturn);
                sForPickerLoadReturn = sForPickerLoadReturn + "ExportSCID IN(SELECT RefID FROM DeliveryOrder WHERE DONo LIKE ''%" + sDONo + "%'')";
            }
            #endregion

            #region PO No
            if (!string.IsNullOrEmpty(sPONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM ProductionOrder WHERE PONo LIKE ''%" + sPONo + "%'')";
                
                Global.TagSQL(ref sForPickerLoadReturn);
                sForPickerLoadReturn = sForPickerLoadReturn + "ExportSCID IN(SELECT ExportSCID FROM ProductionOrder WHERE PONo LIKE '%" + sPONo + "%')";
            }
            #endregion

            #region Challan No
            if (!string.IsNullOrEmpty(sChallanNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM View_DeliveryChallan WHERE ChallanNo LIKE ''%" + sChallanNo + "'')";
                
                Global.TagSQL(ref sForPickerLoadReturn);
                sForPickerLoadReturn = sForPickerLoadReturn + "ExportSCID IN(SELECT ExportSCID FROM View_DeliveryChallan WHERE ChallanNo LIKE '%" + sChallanNo + "')";
            }
            #endregion

            #region LC No
            if (!string.IsNullOrEmpty(sLCNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM ExportSC WHERE ExportPIID IN (SELECT ExportPIID FROM ExportPILCMapping WHERE ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE ExportLCNo LIKE ''%" + sLCNo + "%'')))";
                
                Global.TagSQL(ref sForPickerLoadReturn);
                sForPickerLoadReturn = sForPickerLoadReturn + "ExportSCID IN(SELECT ExportSCID FROM ExportSC WHERE ExportPIID IN (SELECT ExportPIID FROM ExportPILCMapping WHERE ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE ExportLCNo LIKE '%" + sLCNo + "%')))";
            }
            #endregion

            #region PIDate Date
            if (cboPIDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sForPickerLoadReturn);
                sForPickerLoadReturn = sForPickerLoadReturn + "ExportSCID IN(SELECT ExportSCID FROM View_ExportSC WHERE " + Global.DateSQLGenerator("IssueDate", (short)cboPIDate, dFromPIDate, dToPIDate, false) + ")";

                Global.TagSQL(ref sReturn);
                if (cboPIDate == (int)EnumCompareOperator.EqualTo)
                {
                    _sDateRange = "Date: " + dFromDODate.ToString("dd MMM yyyy");
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM View_ExportSC WHERE IssueDate = ''" + dFromPIDate.ToString("dd MMM yyy") + "'')";
                }
                else if (cboPIDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    _sDateRange = "Date: NotEqualTo->" + dFromDODate.ToString("dd MMM yyyy");
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM View_ExportSC WHERE IssueDate != ''" + dFromPIDate.ToString("dd MMM yyy") + "'')";
                }
                else if (cboPIDate == (int)EnumCompareOperator.GreaterThan)
                {
                    _sDateRange = "Date: GreaterThen->" + dFromDODate.ToString("dd MMM yyyy");
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM View_ExportSC WHERE IssueDate > ''" + dFromPIDate.ToString("dd MMM yyy") + "'')";
                }
                else if (cboPIDate == (int)EnumCompareOperator.SmallerThan)
                {
                    _sDateRange = "Date: SmallerThen->" + dFromDODate.ToString("dd MMM yyyy");
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM View_ExportSC WHERE IssueDate < ''" + dFromPIDate.ToString("dd MMM yyy") + "'')";
                }
                else if (cboPIDate == (int)EnumCompareOperator.Between)
                {
                    _sDateRange = "Date: From " + dFromDODate.ToString("dd MMM yyyy") + " To " + dToDODate.ToString("dd MMM yyyy");
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM View_ExportSC WHERE IssueDate BETWEEN ''" + dFromPIDate.ToString("dd MMM yyy") + "'' AND ''" + dToPIDate.ToString("dd MMM yyy") + "'')";
                }
                else if (cboPIDate == (int)EnumCompareOperator.NotBetween)
                {
                    _sDateRange = "Date: NotBetween " + dFromDODate.ToString("dd MMM yyyy") + " To " + dToDODate.ToString("dd MMM yyyy");
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM View_ExportSC WHERE IssueDate NOT BETWEEN ''" + dFromPIDate.ToString("dd MMM yyy") + "'' AND ''" + dToPIDate.ToString("dd MMM yyy") + "'')";
                }              
            }
            #endregion

            #region DO Date
            if (ncboDODate != (int)EnumCompareOperator.None)
            {

                Global.TagSQL(ref sForPickerLoadReturn); //don't remove this url for picker
                sForPickerLoadReturn = sForPickerLoadReturn + "ExportSCID IN(SELECT RefID FROM DeliveryOrder WHERE " + Global.DateSQLGenerator("DODate", (short)ncboDODate, dFromDODate, dToDODate, false) + ")";

                Global.TagSQL(ref sReturn);
                if (ncboDODate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT RefID FROM DeliveryOrder WHERE DODate = ''" + dFromDODate.ToString("dd MMM yyy") + "'')";
                }
                else if (ncboDODate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT RefID FROM DeliveryOrder WHERE DODate != ''" + dFromDODate.ToString("dd MMM yyy") + "'')";
                }
                else if (ncboDODate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT RefID FROM DeliveryOrder WHERE DODate >''" + dFromDODate.ToString("dd MMM yyy") + "'')";
                }
                else if (ncboDODate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT RefID FROM DeliveryOrder WHERE DODate < ''" + dFromDODate.ToString("dd MMM yyy") + "'')";
                }
                else if (ncboDODate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT RefID FROM DeliveryOrder WHERE DODate  BETWEEN ''" + dFromDODate.ToString("dd MMM yyy") + "'' AND ''" + dToDODate.ToString("dd MMM yyy") + "'')";
                }
                else if (ncboDODate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT RefID FROM DeliveryOrder WHERE DODate NOT BETWEEN ''" + dFromDODate.ToString("dd MMM yyy") + "'' AND ''" + dToDODate.ToString("dd MMM yyy") + "'')";
                }
            }
            #endregion

            #region PO Date
            if (ncboPODate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sForPickerLoadReturn); //don't remove this url for picker
                sForPickerLoadReturn = sForPickerLoadReturn + "ExportSCID IN(SELECT ExportSCID FROM ProductionOrder WHERE " + Global.DateSQLGenerator("OrderDate", (short)ncboPODate, dFromPODate, dToPODate, false) + ")";
                
                Global.TagSQL(ref sReturn);
                if (ncboPODate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM ProductionOrder WHERE OrderDate = ''" + dFromPODate.ToString("dd MMM yyy") + "'')";
                }
                else if (ncboPODate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM ProductionOrder WHERE OrderDate != ''" + dFromPODate.ToString("dd MMM yyy") + "'')";
                }
                else if (ncboPODate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM ProductionOrder WHERE OrderDate > ''" + dFromPODate.ToString("dd MMM yyy") + "'')";
                }
                else if (ncboPODate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM ProductionOrder WHERE OrderDate < ''" + dFromPODate.ToString("dd MMM yyy") + "'')";
                }
                else if (ncboPODate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM ProductionOrder WHERE OrderDate BETWEEN ''" + dFromPODate.ToString("dd MMM yyy") + "'' AND ''" + dToPODate.ToString("dd MMM yyy") + "'')";
                }
                else if (ncboPODate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM ProductionOrder WHERE OrderDate NOT BETWEEN ''" + dFromPODate.ToString("dd MMM yyy") + "'' AND ''" + dToPODate.ToString("dd MMM yyy") + "'')";
                }
            }
            #endregion

            #region Challan Date
            if (ncboChallanDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sForPickerLoadReturn); //don't remove this url for picker
                sForPickerLoadReturn = sForPickerLoadReturn + "ExportSCID IN(SELECT ExportSCID FROM View_DeliveryChallan WHERE " + Global.DateSQLGenerator("ChallanDate", (short)ncboChallanDate, dFromChallanDate, dToChallanDate, false) + ")";

                Global.TagSQL(ref sReturn);
                if (ncboChallanDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM View_DeliveryChallan WHERE ChallanDate = ''" + dFromChallanDate.ToString("dd MMM yyy") + "'')";
                }
                else if (ncboChallanDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM View_DeliveryChallan WHERE ChallanDate != ''" + dFromChallanDate.ToString("dd MMM yyy") + "'')";
                }
                else if (ncboChallanDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM View_DeliveryChallan WHERE ChallanDate > ''" + dFromChallanDate.ToString("dd MMM yyy") + "'')";
                }
                else if (ncboChallanDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM View_DeliveryChallan WHERE ChallanDate < ''" + dFromChallanDate.ToString("dd MMM yyy") + "'')";
                }
                else if (ncboChallanDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM View_DeliveryChallan WHERE ChallanDate BETWEEN ''" + dFromChallanDate.ToString("dd MMM yyy") + "'' AND ''" + dToChallanDate.ToString("dd MMM yyy") + "'')";
                }
                else if (ncboChallanDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + "ExportSCID IN(SELECT ExportSCID FROM View_DeliveryChallan WHERE ChallanDate NOT BETWEEN ''" + dFromChallanDate.ToString("dd MMM yyy") + "'' AND ''" + dToChallanDate.ToString("dd MMM yyy") + "'')";
                }
            }
            #endregion

            this.Session.Add(SessionInfo.ParamObj, sForPickerLoadReturn1 + sForPickerLoadReturn);//don't remove this picker
         
            string sSQL = sReturn1 + sReturn;
            return sSQL;
        }
        #endregion

        [HttpPost]
        public JsonResult GetReferenceData(OrderTracking oOrderTracking)
        {
            _oOrderTracking = new OrderTracking();
            ExportSCDetail oExportSCDetail = new ExportSCDetail();
            Product oProduct = new Product();
            //Ref for PO:1;PS:2;Pipelline:3; DO:4;Challan:5;Return:6;PIQty:7
            int nRefType = Convert.ToInt32(oOrderTracking.Params.Split('~')[0]);
            int nLayoutType = Convert.ToInt32(oOrderTracking.Params.Split('~')[1]);//0: color View;1:PI View;2:Product View
            string sSQL = "";

            try
            {
                #region common Get
                if (nLayoutType == 0)//color view
                {
                    oExportSCDetail = oExportSCDetail.Get(oOrderTracking.ExportSCDetailID, (int)Session[SessionInfo.currentUserID]);
                    _oOrderTracking.ProductCode = oExportSCDetail.ProductCode;
                    _oOrderTracking.ProductName = oExportSCDetail.ProductName;
                    _oOrderTracking.PINo = oExportSCDetail.PINo;
                    _oOrderTracking.ColorName = oExportSCDetail.ColorName;
                }
                else
                {
                    oProduct = oProduct.Get(oOrderTracking.ProductID, (int)Session[SessionInfo.currentUserID]);
                    _oOrderTracking.ProductCode = oProduct.ProductCode;
                    _oOrderTracking.ProductName = oProduct.ProductName;
                }
                #endregion

                #region PO Region
                if (nRefType == 1)//Production Order
                {
                    sSQL = "SELECT PONO, OrderDate , SUM(ISNULL(Qty,0))AS Qty , UnitSymbol FROM View_ProductionOrderDetail WHERE ";
                    if (nLayoutType == 0)//color view
                    {
                        sSQL += "ExportSCDetailID IN (" + oOrderTracking.ExportSCDetailID + ") Group By ProductionOrderID, ProductID, PONo, OrderDate , UnitSymbol";
                    }
                    else if (nLayoutType == 1)//pi View
                    {
                        sSQL += "ProductionOrderID IN (SELECT ProductionOrderID FROM ProductionOrder WHERE ExportSCID IN(SELECT ExportSCID FROM ExportSC WHERE ExportPIID = " + oOrderTracking.ExportPIID + " )) AND ProductID = " + oOrderTracking.ProductID + " Group By ProductID, ProductionOrderID, PONo, OrderDate , UnitSymbol";
                    }
                    else if (nLayoutType == 2)//product View
                    {
                        sSQL += " ProductID = " + oOrderTracking.ProductID + " AND ExportSCDetailID IN (" + Session[SessionInfo.ParamObj] + ")";
                        sSQL += " Group By ProductID, ProductionOrderID, PONo, OrderDate , UnitSymbol";
                    }
                    _oOrderTracking.ProductionOrderDetails = ProductionOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                #endregion

                #region PS Region
                if (nRefType == 2)//Production Sheet
                {
                    sSQL = "SELECT SheetNo , IssueDate, SUM(ISNULL(Quantity,0))AS Quantity , UnitSymbol FROM View_ProductionSheet WHERE ";
                    if (nLayoutType == 0)//color view
                    {
                        sSQL += "PTUUnit2ID IN (SELECT PTUUnit2ID FROM PTUUnit2 WHERE ExportSCDetailID IN (" + oOrderTracking.ExportSCDetailID + ")) Group By ProductionSheetID, ProductID, SheetNo, IssueDate , UnitSymbol";
                    }
                    else if (nLayoutType == 1)//pi View
                    {
                        sSQL += "PTUUnit2ID IN (SELECT PTUUnit2ID FROM PTUUnit2 WHERE ExportSCID IN (SELECT  ExportSCID FROM ExportSC WHERE ExportPIID = " + oOrderTracking.ExportPIID + ")) AND ProductID = " + oOrderTracking.ProductID + " Group By ProductID, ProductionSheetID,  SheetNo, IssueDate , UnitSymbol";
                    }
                    else if (nLayoutType == 2)//product View
                    {
                        sSQL += "ProductID = " + oOrderTracking.ProductID + " AND PTUUnit2ID IN (SELECT PTUUnit2ID FROM PTUUnit2 WHERE ExportSCDetailID IN (" + Session[SessionInfo.ParamObj] + "))";
                        sSQL += " Group By ProductID, ProductionSheetID,  SheetNo, IssueDate , UnitSymbol";
                    }
                    _oOrderTracking.ProductionSheets = ProductionSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                #endregion

                #region Pipe line Region
                if (nRefType == 3)//Pipe line
                {
                    sSQL = "SELECT RefNo , DBServerDateTime, SUM(ISNULL(YetToPipeLine,0))AS Qty , MUSymbol FROM View_PTUUnit2Log WHERE PTUUnit2RefType = 2 AND ";
                    if (nLayoutType == 0)//color view
                    {
                        sSQL += "PTUUnit2ID IN (SELECT PTUUnit2ID FROM PTUUnit2 WHERE ExportSCDetailID IN (" + oOrderTracking.ExportSCDetailID + ")) Group By PTUUnit2ID, ProductID, RefNo, DBServerDateTime , MUSymbol";
                    }
                    else if (nLayoutType == 1)//pi View
                    {
                        sSQL += "PTUUnit2ID IN (SELECT PTUUnit2ID FROM PTUUnit2 WHERE ExportSCID IN (SELECT  ExportSCID FROM ExportSC WHERE ExportPIID = " + oOrderTracking.ExportPIID + ")) AND ProductID = " + oOrderTracking.ProductID + " Group By ProductID, PTUUnit2ID, RefNo, DBServerDateTime , MUSymbol";
                    }
                    else if (nLayoutType == 2)//product View
                    {
                        sSQL += " PTUUnit2ID IN (SELECT PTUUnit2ID FROM PTUUnit2 WHERE ExportSCDetailID IN (" + Session[SessionInfo.ParamObj] + ") AND ProductID = " + oOrderTracking.ProductID + ")";
                        sSQL += " Group By ProductID, PTUUnit2ID, RefNo, DBServerDateTime , MUSymbol";
                    }
                    _oOrderTracking.PTUUnit2Logs = PTUUnit2Log.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                #endregion

                #region DO Region
                if (nRefType == 4)//Delivery Order
                {
                    sSQL = "SELECT DONo, DeliveryDate,  SUM(ISNULL(Qty,0))AS Qty , MUnit FROM View_DeliveryOrderDetail WHERE ";
                    if (nLayoutType == 0)//color view
                    {
                        sSQL += "RefDetailID IN (" + oOrderTracking.ExportSCDetailID + ") Group By DeliveryOrderID, ProductID, DONo, DeliveryDate , MUnit";
                    }
                    else if (nLayoutType == 1)//pi View
                    {
                        sSQL += "DeliveryOrderID IN (SELECT DeliveryOrderID FROM DeliveryOrder WHERE RefID IN(SELECT ExportSCID FROM ExportSC WHERE ExportPIID = " + oOrderTracking.ExportPIID + " )) AND ProductID = " + oOrderTracking.ProductID + " Group By ProductID,DeliveryOrderID, DONo, DeliveryDate , MUnit";
                    }
                    else if (nLayoutType == 2)//product View
                    {
                        sSQL += "ProductID = " + oOrderTracking.ProductID + " AND RefDetailID IN (" + Session[SessionInfo.ParamObj] + ")";
                        sSQL += " Group By ProductID, DeliveryOrderID, DONo, DeliveryDate , MUnit  ";
                    }
                    _oOrderTracking.DeliveryOrderDetails = DeliveryOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                #endregion

                #region Delivery Challan Region
                if (nRefType == 5)//Delivery Challan
                {
                    sSQL = "SELECT ChallanNo, ChallanDate,   SUM(ISNULL(Qty,0))AS Qty , MUnit FROM View_DeliveryChallanDetail WHERE ";
                    if (nLayoutType == 0)//color view
                    {
                        sSQL += "RefDetailID IN (" + oOrderTracking.ExportSCDetailID + ") Group By DeliveryChallanID, ProductID, ChallanNo, ChallanDate , MUnit";
                    }
                    else if (nLayoutType == 1)//pi View
                    {
                        sSQL += "DeliveryChallanID IN (SELECT DC.DeliveryChallanID FROM View_DeliveryChallan AS DC WHERE DC.ExportSCID IN(SELECT ExportSCID FROM ExportSC WHERE ExportPIID = " + oOrderTracking.ExportPIID + " )) AND ProductID = " + oOrderTracking.ProductID + " Group By  ProductID, DeliveryChallanID, ChallanNo, ChallanDate , MUnit";
                    }
                    else if (nLayoutType == 2)//product View
                    {
                        sSQL += "ProductID = " + oOrderTracking.ProductID + " AND RefDetailID IN (" + Session[SessionInfo.ParamObj] + ")";
                        sSQL += " Group By  ProductID, DeliveryChallanID, ChallanNo, ChallanDate , MUnit";
                    }
                    _oOrderTracking.DeliveryChallanDetails = DeliveryChallanDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                #endregion

                #region Return Challan Region
                if (nRefType == 6)//Return Challan
                {
                    sSQL = "SELECT ReturnChallanNo, ReturnDate,   SUM(ISNULL(Qty,0))AS Qty , MUnit FROM View_ReturnChallanDetail WHERE ";
                    if (nLayoutType == 0)//color view
                    {
                        sSQL += "DeliveryChallanDetailID IN (SELECT DeliveryChallanDetailID FROM View_DeliveryChallanDetail WHERE RefDetailID IN (" + oOrderTracking.ExportSCDetailID + ")) Group By ReturnChallanID, ProductID, ReturnChallanNo, ReturnDate, MUnit";
                    }
                    else if (nLayoutType == 1)//pi View
                    {
                        sSQL += "DeliveryChallanID IN (SELECT DC.DeliveryChallanID FROM View_DeliveryChallan AS DC WHERE DC.ExportSCID IN(SELECT ExportSCID FROM ExportSC WHERE ExportPIID = " + oOrderTracking.ExportPIID + " )) AND ProductID = " + oOrderTracking.ProductID + " Group By  ProductID, ReturnChallanID, ReturnChallanNo, ReturnDate, MUnit";
                    }
                    else if (nLayoutType == 2)//product View
                    {
                        sSQL += "ProductID = " + oOrderTracking.ProductID + " AND DeliveryChallanDetailID IN (SELECT DeliveryChallanDetailID FROM View_DeliveryChallanDetail WHERE RefDetailID IN (" + Session[SessionInfo.ParamObj] + "))";
                        sSQL += " Group By  ProductID, ReturnChallanID, ReturnChallanNo, ReturnDate, MUnit";
                    }
                    _oOrderTracking.ReturnChallanDetails = ReturnChallanDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                #endregion

                #region PI Region
                if (nRefType == 7)//PI 
                {
                    sSQL = "SELECT PINo, IssueDate,   SUM(ISNULL(Qty,0)) AS Qty, MUName FROM View_ExportPIDetail WHERE ";
                    if (nLayoutType == 2)//product View
                    {                        
                        sSQL += "ProductID = " + oOrderTracking.ProductID + " AND ExportPIDetailID IN (SELECT ExportPIDetailID FROM ExportSCDetail WHERE ExportSCDetailID IN (" + Session[SessionInfo.ParamObj] + "))";                        
                        sSQL += " Group By ExportPIID, ProductID, PINo, IssueDate, MUName";
                    }
                    _oOrderTracking.ExportPIDetails = ExportPIDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                #endregion
                #region PTU Transfer Region
                if (nRefType == 8)//PTU Transfer 
                {
                    sSQL = "SELECT PINo, LotNo, TransactionTime, SUM(ISNULL(Qty,0))AS Qty, MUName FROM View_PTUUnit2DistributionLog WHERE ";
                    if (nLayoutType == 0)//color view
                    {
                        sSQL += "ISNULL(PTUUnit2DistributionRefType,0) = " + (int)EnumPTUUnit2Ref.PTU_Disburse + " AND ExportSCDetailID =  " + oOrderTracking.ExportSCDetailID + "  Group By ExportSCDetailID, PINo, LotNo, TransactionTime, MUName";
                    }
                    else if (nLayoutType == 1)//pi View
                    {
                        sSQL += "ISNULL(PTUUnit2DistributionRefType,0) = " + (int)EnumPTUUnit2Ref.PTU_Disburse + " AND ExportPIID =" + oOrderTracking.ExportPIID + " AND ProductID = " + oOrderTracking.ProductID + "  Group By ExportPIID, ProductID, PINo  LotNo, TransactionTime, MUName";
                    }
                    else if (nLayoutType == 2)//product View
                    {
                        sSQL += "ISNULL(PTUUnit2DistributionRefType,0) = " + (int)EnumPTUUnit2Ref.PTU_Disburse + " AND ProductID = " + oOrderTracking.ProductID + " AND ExportSCDetailID IN (" + Session[SessionInfo.ParamObj] + ")  Group By ProductID, PINo, LotNo, TransactionTime, MUName";
                    }
                    _oOrderTracking.PTUUnit2DistributionLogs = PTUUnit2Distribution.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                #endregion

            }
            catch (Exception ex)
            {
                _oOrderTracking = new OrderTracking();
                _oOrderTracking.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderTracking);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsRSs(OrderTracking oOrderTracking)
        {
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            try
            {
                string sSQL = "Select * from View_RouteSheet Where PTUID =" + oOrderTracking.ExportSCDetailID;
                oRouteSheets = RouteSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetsLots(OrderTracking oOrderTracking)
        {
            List<Lot> oLots = new List<Lot>();
            try
            {
                oLots = Lot.Gets("Select * from View_Lot where Balance>0.2 and parentType=106 and ParentID in (Select RouteSheetID from RouteSheet where PTUID=" + oOrderTracking.ExportSCDetailID + ")", (int)Session[SessionInfo.currentUserID]);
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
        public void PrintRpt_Excel_Bulk(string sTempString, int nLayoutType, int buid, bool bIsYetToDO, bool bIsYetToChallan, bool bIsYetToChallanWithDOEntry, bool bIsPTUTransferQty)
        {

            double nPIQty = 0;
            double nPOQty = 0;
            double nYetToPO = 0;
            double nPSQty = 0;
            double nPipeLineQty = 0;
            double nDOQty = 0;
            double nYetToDO = 0;
            double nChallanQty = 0;
            double nYetToChallanQty = 0;
            double nReturnQty = 0;
            double nPTUTransferQty = 0;
            double nStockQty = 0;
            double nHangerWeight = 0;
            double nWastagePercentWithWeight = 0;
            double nTotalWeight = 0;

            List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            _oReportConfigure = new ReportConfigure();
            _oReportConfigure = _oReportConfigure.Get(nLayoutType, (int)Session[SessionInfo.currentUserID]);
            
            _oOrderTracking.Params = sTempString;
            string sSQL = MakeSQL(_oOrderTracking);
            _oOrderTrackings = OrderTracking.Gets(sSQL, nLayoutType, bIsYetToDO, bIsYetToChallan, bIsYetToChallanWithDOEntry, bIsPTUTransferQty,(int)Session[SessionInfo.currentUserID]);
            
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);
            int nSL = 0;
            if (_oReportConfigure.ReportConfigureID > 0)
            {
                #region configure with Export Excel
                string[] FieldNames, ColumnNames;
                FieldNames = _oReportConfigure.FieldNames.Split(',');
                ColumnNames = _oReportConfigure.CaptionNames.Split(',');

                int nRowIndex = 2, nEndRow = 0, nStartCol = 1, nEndCol = FieldNames.Count(), nTempCol = 1;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Order Tracking(Bulk)");
                    sheet.Name = "Order Tracking(Bulk)";
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//sl
                    for (int i = 0; i < nEndCol; i++)
                    {
                        sheet.Column(nTempCol).Width = 30; nTempCol++;
                    }

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
                    cell.Value = "Order Tracking(Bulk)   Date: " + DateTime.Today.ToString("dd MMM yyyy hh:mm:tt") + ""; cell.Style.Font.Bold = true;
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

                    #region column title
                    nTempCol = 1;//REset
                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    for (int i = 0; i < ColumnNames.Count(); i++)
                    {
                        string[] sColumnNameValues = ColumnNames[i].Split('~');
                        if (sColumnNameValues.Count() > 1)//for Number
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = sColumnNameValues[0]; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        else
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ColumnNames[i]; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                    }
                    nRowIndex++;
                    #endregion

                    #region Data
                    foreach (OrderTracking oItem in _oOrderTrackings)
                    {
                        nTempCol = 1;//reset
                        nSL++;
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (FieldNames.Contains("PINo"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "" + oItem.PINo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("PIDateInString"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.PIDateInString; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("ExportLCFileNo"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.ExportLCFileNo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (FieldNames.Contains("ExportLCNo"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("BuyerName"))
                        {

                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (FieldNames.Contains("ContractorName"))
                        {

                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (FieldNames.Contains("ProductCode"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (FieldNames.Contains("ProductName"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (FieldNames.Contains("ColorName"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("StyleRef"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.StyleRef; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("SizeName"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.SizeName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }                        
                        if (FieldNames.Contains("MUName"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.MUName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("PIQtyWithOutFormatting") || FieldNames.Contains("PIQtyInString"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.PIQty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("POQtyInString"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.POQty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("YetToPOInString"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.YetToPO; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("PSQtyInString"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.PSQty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("PipeLineQtyInString"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.PipeLineQty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("DOQtyInString"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.DOQty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("YetToDOInString"))
                        {

                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.YetToDO; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("ChallanQtyInString"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.ChallanQty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("YetToChallanQtyInString"))
                        {

                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.YetToChallanQty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("ReturnQtyInString"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.ReturnQty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("PTUTransferQtyInString"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.PTUTransferQty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("StockQtyInString"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.StockQty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("HangerWeight"))
                        {

                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.HangerWeight; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("WastagePercentWithWeight"))
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.WastagePercentWithWeight; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("TotalWeight"))
                        {

                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.TotalWeight; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (FieldNames.Contains("Remarks"))
                        {

                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        nEndRow = nRowIndex;
                        nRowIndex++;

                        nPIQty = nPIQty + oItem.PIQty;
                        nPOQty = nPOQty + oItem.POQty;
                        nYetToPO = nYetToPO + oItem.YetToPO;
                        nPSQty = nPSQty + oItem.PSQty;
                        nPipeLineQty = nPipeLineQty + oItem.PipeLineQty;
                        nDOQty = nDOQty + oItem.DOQty;
                        nYetToDO = nYetToDO + oItem.YetToDO;
                        nChallanQty = nChallanQty + oItem.ChallanQty;
                        nYetToChallanQty = nYetToChallanQty + oItem.YetToChallanQty;
                        nReturnQty = nReturnQty + oItem.ReturnQty;
                        nPTUTransferQty = nPTUTransferQty + oItem.PTUTransferQty;
                        nStockQty = nStockQty + oItem.StockQty;
                        nHangerWeight = nHangerWeight + oItem.HangerWeight;
                        nWastagePercentWithWeight = nWastagePercentWithWeight + oItem.WastagePercentWithWeight;
                        nTotalWeight = nTotalWeight + oItem.TotalWeight;
                    }


                    #region Total
                    nTempCol = 1;//reset                    
                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (FieldNames.Contains("PINo"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("PIDateInString"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("ExportLCFileNo"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    if (FieldNames.Contains("ExportLCNo"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("BuyerName"))
                    {

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    if (FieldNames.Contains("ContractorName"))
                    {

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    if (FieldNames.Contains("ProductCode"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    if (FieldNames.Contains("ProductName"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    if (FieldNames.Contains("ColorName"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    if (FieldNames.Contains("SizeName"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("StyleRef"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("MUName"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("PIQtyWithOutFormatting") || FieldNames.Contains("PIQtyInString"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nPIQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("POQtyInString"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nPOQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("YetToPOInString"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nYetToPO; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("PSQtyInString"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nPSQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("PipeLineQtyInString"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nPipeLineQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("DOQtyInString"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nDOQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("YetToDOInString"))
                    {

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nYetToDO; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("ChallanQtyInString"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nChallanQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("YetToChallanQtyInString"))
                    {

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nYetToChallanQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("ReturnQtyInString"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nReturnQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("PTUTransferQtyInString"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nPTUTransferQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("StockQtyInString"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nStockQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("HangerWeight"))
                    {

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nHangerWeight; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("WastagePercentWithWeight"))
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nWastagePercentWithWeight; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("TotalWeight"))
                    {

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nTotalWeight; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (FieldNames.Contains("Remarks"))
                    {

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nEndRow = nRowIndex;
                    nRowIndex++;
                    #endregion

                    #endregion


                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=OrderTracking(Bulk).xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            else
            {
                #region without configure Export Excel
                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 33, nTempCol = 2;
                ExcelRange cell;
                ExcelRange HeaderCell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Order Tracking(Bulk)");
                    sheet.Name = "Order Tracking(Bulk)";
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//sl
                    if (nLayoutType == 0 || nLayoutType == 1)
                    {
                        sheet.Column(nTempCol).Width = 50; nTempCol++;
                        sheet.Column(nTempCol).Width = 30; nTempCol++;
                        sheet.Column(nTempCol).Width = 30; nTempCol++;
                        sheet.Column(nTempCol).Width = 20; nTempCol++;
                        sheet.Column(nTempCol).Width = 20; nTempCol++;
                        sheet.Column(nTempCol).Width = 20; nTempCol++;
                    }
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//p.code
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//p name
                    if (nLayoutType == 0)
                    {
                        sheet.Column(nTempCol).Width = 20; nTempCol++;//color
                    }
                    sheet.Column(nTempCol).Width = 20; nTempCol++;
                    if (nLayoutType == 0)
                    {
                        sheet.Column(nTempCol).Width = 30; nTempCol++;//style
                    }
                    sheet.Column(nTempCol).Width = 30; nTempCol++;
                    sheet.Column(nTempCol).Width = 30; nTempCol++;
                    sheet.Column(nTempCol).Width = 30; nTempCol++;
                    sheet.Column(nTempCol).Width = 30; nTempCol++;
                    sheet.Column(nTempCol).Width = 30; nTempCol++;
                    sheet.Column(nTempCol).Width = 30; nTempCol++;
                    sheet.Column(nTempCol).Width = 30; nTempCol++;
                    sheet.Column(nTempCol).Width = 30; nTempCol++;
                    sheet.Column(nTempCol).Width = 30; nTempCol++;
                    sheet.Column(nTempCol).Width = 30; nTempCol++;
                    sheet.Column(nTempCol).Width = 30; nTempCol++;
                    sheet.Column(nTempCol).Width = 30; nTempCol++;
                    sheet.Column(nTempCol).Width = 30; nTempCol++;
                    sheet.Column(nTempCol).Width = 30; nTempCol++;
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//total weight
                    if (nLayoutType == 0 || nLayoutType == 1)
                    {
                        sheet.Column(nTempCol).Width = 30; nTempCol++;//Remarks
                    }
                    nEndCol = nTempCol + 1;

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
                    cell.Value = "Order Tracking(Bulk)   Date: " + DateTime.Today.ToString("dd MMM yyyy hh:mm:tt") + ""; cell.Style.Font.Bold = true;
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

                    #region column title
                    nTempCol = 1;//REset
                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    if (nLayoutType == 0 || nLayoutType == 1)
                    {

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "P/I No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "P/I Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "LC File No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "LC  No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Customer Name"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Product Code"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    if (nLayoutType == 0)
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Size"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    if (nLayoutType == 0)
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Style"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Unit Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "P/I Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "PO Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Yet To PO Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Sheet Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "PipeLine Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "DO Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Yet To DO"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Challan Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Yet To Challan"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Return Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "PTU Transfer Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Stock"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Hanger Weight(gm)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Wastage 2% with Weight"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Total Weight(gm)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    if (nLayoutType == 0 || nLayoutType == 1)
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nRowIndex++;
                    #endregion

                    #region Data
                    foreach (OrderTracking oItem in _oOrderTrackings)
                    {
                        nTempCol = 1;//reset
                        nSL++;
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        if (nLayoutType == 0 || nLayoutType == 1)
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "" + oItem.PINo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.PIDateInString; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.ExportLCFileNo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        if (nLayoutType == 0)
                        {

                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.SizeName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        if (nLayoutType == 0)
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.StyleRef; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.MUName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.PIQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.POQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.YetToPO; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.PSQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.PipeLineQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.DOQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.YetToDO; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.ChallanQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.YetToChallanQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.ReturnQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.PTUTransferQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.StockQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.HangerWeight; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.WastagePercentWithWeight; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.TotalWeight; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (nLayoutType == 0 || nLayoutType == 1)
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        nEndRow = nRowIndex;
                        nRowIndex++;

                        nPIQty = nPIQty + oItem.PIQty;
                        nPOQty = nPOQty + oItem.POQty;
                        nYetToPO = nYetToPO + oItem.YetToPO;
                        nPSQty = nPSQty + oItem.PSQty;
                        nPipeLineQty = nPipeLineQty + oItem.PipeLineQty;
                        nDOQty = nDOQty + oItem.DOQty;
                        nYetToDO = nYetToDO + oItem.YetToDO;
                        nChallanQty = nChallanQty + oItem.ChallanQty;
                        nYetToChallanQty = nYetToChallanQty + oItem.YetToChallanQty;
                        nReturnQty = nReturnQty + oItem.ReturnQty;
                        nPTUTransferQty = nPTUTransferQty + oItem.PTUTransferQty;
                        nStockQty = nStockQty + oItem.StockQty;
                        nHangerWeight = nHangerWeight + oItem.HangerWeight;
                        nWastagePercentWithWeight = nWastagePercentWithWeight + oItem.WastagePercentWithWeight;
                        nTotalWeight = nTotalWeight + oItem.TotalWeight;

                    }

                    #region Total
                    nTempCol = 1;//reset                    
                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nPIQty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nPOQty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nYetToPO; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nPSQty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nPipeLineQty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nDOQty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nYetToDO; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nChallanQty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nYetToChallanQty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nReturnQty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nPTUTransferQty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nStockQty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nHangerWeight; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nWastagePercentWithWeight; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = nTotalWeight; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nEndRow = nRowIndex;
                    nRowIndex++;
                    #endregion
                    #endregion


                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=OrderTracking(Bulk).xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion
    }
}