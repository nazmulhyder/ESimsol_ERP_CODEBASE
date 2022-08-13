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
    public class RouteSheetDCOutController : Controller
    {
        #region Declaration
        RouteSheetDCOut _oRouteSheetDCOut = new RouteSheetDCOut();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<RouteSheetDCOut> _oRouteSheetDCOuts = new List<RouteSheetDCOut>();
        string _sDateRange = "";
        #endregion

        #region RouteSheetDCOut
        public ActionResult View_RouteSheetDCOut(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            string sSQL = "SELECT * FROM View_RouteSheetDCOutV2 where isnull(ApprovedByID,0)<>0 and [ApprovedDate]>='" + DateTime.Now.ToString("dd MMM yyyy") + "' and [ApprovedDate]<'" + DateTime.Now.AddDays(1).ToString("dd MMM yyyy") + "'";
            _oRouteSheetDCOuts = RouteSheetDCOut.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperator = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.RouteSheetDetail, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            ViewBag.WorkingUnits = oWorkingUnits;
            ViewBag.RSShifts = RSShift.GetsByModule(buid, "" + (int)EnumModuleName.RouteSheet, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oRouteSheetDCOuts);
        }
        public ActionResult View_RouteSheetDCOut_Summary(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            
            DateTime nCurrentDate = DateTime.Now;

            // query for the summary of running month
            //string sSQL = MakeSQL_Summary(0, new DateTime(nCurrentDate.Year, nCurrentDate.Month, 1), new DateTime(nCurrentDate.Year, nCurrentDate.Month + 1, 1).AddDays(-1));
            string sSQL = MakeSQL_Summary(0, nCurrentDate, nCurrentDate.AddDays(-1), 0);
            
            _oRouteSheetDCOuts = RouteSheetDCOut.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperator = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.RouteSheetDetail, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            ViewBag.WorkingUnits = oWorkingUnits;
            ViewBag.RSShifts = RSShift.GetsByModule(buid, "" + (int)EnumModuleName.RouteSheet, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ProductTypes = EnumObject.jGets(typeof(EnumProductNature)).Where(x => x.id == (int)EnumProductNature.Dyes || x.id == (int)EnumProductNature.Chemical);
            return View(_oRouteSheetDCOuts);
        }
      
        [HttpPost]
        public JsonResult AdvSearch(RouteSheetDCOut oRouteSheetDCOut)
        {
            _oRouteSheetDCOuts = new List<RouteSheetDCOut>();
            try
            {
                string sSQL = MakeSQL_V2(oRouteSheetDCOut);
                _oRouteSheetDCOuts = RouteSheetDCOut.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRouteSheetDCOuts = new List<RouteSheetDCOut>();
                _oRouteSheetDCOut.ErrorMessage = ex.Message;
                _oRouteSheetDCOuts.Add(_oRouteSheetDCOut);
            }
            var jsonResult = Json(_oRouteSheetDCOuts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult AdvSearch_Summary(RouteSheetDCOut oRouteSheetDCOut)
        {
            _oRouteSheetDCOuts = new List<RouteSheetDCOut>();
            try
            {
                int nProductID = Convert.ToInt32(oRouteSheetDCOut.ErrorMessage.Split('~')[0]);
                DateTime startDate = Convert.ToDateTime(oRouteSheetDCOut.ErrorMessage.Split('~')[1]);
                DateTime endDate = Convert.ToDateTime(oRouteSheetDCOut.ErrorMessage.Split('~')[2]);
                int nBUID = Convert.ToInt32(oRouteSheetDCOut.ErrorMessage.Split('~')[3]);
                int nProductCategory = Convert.ToInt32(oRouteSheetDCOut.ErrorMessage.Split('~')[4]);

                if (startDate > endDate) throw new Exception("Invalid Date. Start date can not be greater than end date!!");

                string sSQL = MakeSQL_Summary(nProductID, startDate, endDate, nProductCategory);
                _oRouteSheetDCOuts = RouteSheetDCOut.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRouteSheetDCOuts = new List<RouteSheetDCOut>();
                _oRouteSheetDCOut.ErrorMessage = ex.Message;
                _oRouteSheetDCOuts.Add(_oRouteSheetDCOut);
            }
            var jsonResult = Json(_oRouteSheetDCOuts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL_Summary(int nProductID, DateTime startDateTime, DateTime endDateTime, int nProductCategory) 
        {
            string sSQL = " SELECT ProductID, ProductCode,  ProductName, ProductCategoryName, ProductType, MUName,  SUM(Qty) as Qty,    SUM(QtyAdd) as QtyAdd,  SUM(QtyRet) as QtyRet, (SUM(Qty) + SUM(QtyAdd) - SUM(QtyRet)) as Balance from ( "
                        + " SELECT     "
                        + "    Pro.ProductID,    "
                        + "    Pro.ProductCode,  "
                        + "    Pro.ProductName,  "
                        + "    PC.ProductCategoryName, RsA.ProductType, "
                        + "   MU.symbol as MUName,  "
                        + "    case when RsA.SequenceNo in (-1,0) and RsA.InOutType in (" + (int)EnumInOutType.Disburse + ") then  RsA.Qty else 0 end as Qty,    "
                        + "    case when RsA.SequenceNo > 0 and RsA.InOutType in (" + (int)EnumInOutType.Disburse + ") then  RsA.Qty else 0 end as QtyAdd,   "
                        + "    case when RsA.SequenceNo > 0 and RsA.InOutType in (" + (int)EnumInOutType.Receive + ") then  RsA.Qty else 0 end as QtyRet    "
                        + " FROM  [dbo].[RSDetailAdditonal] as RsA WITH (NOLOCK)    "
                        + " LEFT JOIN dbo.Lot as Lot ON RsA.LotID = Lot.LotID  "
                        + "  LEFT JOIN dbo.MeasurementUnit as MU ON MU.MeasurementUnitID = Lot.MunitID"
                        + " LEFT JOIN dbo.Product as  Pro ON Pro.ProductID = Lot.ProductID  "
                        + " LEFT JOIN dbo.ProductCategory PC ON PC.ProductCategoryID = Pro.ProductCategoryID  "
                        + " WHERE RsA.ApprovedByID <> 0 AND RsA.ProductType <> 0 ";
                        
            if(nProductID>0)
                  sSQL += " AND Lot.ProductID = " + nProductID;
            if (startDateTime != DateTime.MinValue || endDateTime != DateTime.MinValue)
            {    
                  //sSQL += " AND CONVERT(DATE,CONVERT(VARCHAR(12),RsA.ApprovedDate,106)) >= CONVERT(DATE,CONVERT(VARCHAR(12),'" + startDateTime.ToString("dd MMM yyyy") + "')) ";
                  //sSQL += " AND CONVERT(DATE,CONVERT(VARCHAR(12),RsA.ApprovedDate,106)) <= CONVERT(DATE,CONVERT(VARCHAR(12),'" + endDateTime.ToString("dd MMM yyyy") + "')) ";
                sSQL += " AND RsA.ApprovedDate BETWEEN CONVERT(VARCHAR, '" + startDateTime.ToString("dd MMM yyyy hh:mm tt") + "', 100) AND CONVERT(VARCHAR, '" + endDateTime.ToString("dd MMM yyyy hh:mm tt") + "', 100) ";
            }
            if (nProductCategory > 0)
                sSQL += "  AND RsA.ProductType = " + nProductCategory;

                  sSQL += " ) as ff ";
                  sSQL += " GROUP BY ProductID, ProductCode, ProductName, MUName, ProductCategoryName, ProductType ";
                  sSQL += " ORDER BY ProductType, ProductName";
            return sSQL;
        }
        private string MakeSQL(RouteSheetDCOut oRouteSheetDCOut)
        {
            int ncboDate = 0;
            DateTime dStartDate = DateTime.Today;
            DateTime dEndDate = DateTime.Today;

            int nAdvDateCbo = 0;
            DateTime dAdvStartDate = DateTime.Today;
            DateTime dAdvEndDate = DateTime.Today;

            string sParams = oRouteSheetDCOut.ErrorMessage;
            string sRSNo = "";
            int nOperationUnitName = 0;
            int nShiftName = 0;
            int nBUID = 0;

            if (sParams==null || sParams=="")
            {
                _oRouteSheetDCOut.ProductName = "";
                _oRouteSheetDCOut.OperationUnitName = "";
               
            }
            else
            {
                _oRouteSheetDCOut.ProductName = Convert.ToString(sParams.Split('~')[0]);
                _oRouteSheetDCOut.LotNo = Convert.ToString(sParams.Split('~')[1]);
                _oRouteSheetDCOut.OperationUnitName = Convert.ToString(sParams.Split('~')[2]);
                ncboDate = Convert.ToInt32(sParams.Split('~')[3]);
                dStartDate = Convert.ToDateTime(sParams.Split('~')[4]);
                dEndDate = Convert.ToDateTime(sParams.Split('~')[5]);
                sRSNo = Convert.ToString(sParams.Split('~')[6]);
                nOperationUnitName = Convert.ToInt32(sParams.Split('~')[7]); // Store Name
                nShiftName = Convert.ToInt32(sParams.Split('~')[8]);             //Shift
                _oRouteSheetDCOut.RouteSheetNo = Convert.ToString(sParams.Split('~')[9]);      //Dye Lot No
                nAdvDateCbo = Convert.ToInt32(sParams.Split('~')[10]);
                dAdvStartDate = Convert.ToDateTime(sParams.Split('~')[11]);
                dAdvEndDate = Convert.ToDateTime(sParams.Split('~')[12]);
            }


            string sReturn1 = "";
            string sReturn = "";

            sReturn1 = "SELECT * FROM View_RouteSheetDCOut ";

            #region Product
            if (!String.IsNullOrEmpty(sRSNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RouteSheetNo Like '%" + sRSNo + "%'";
            }
            #endregion

            #region Product
            if (!String.IsNullOrEmpty(_oRouteSheetDCOut.ProductName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID in(" + _oRouteSheetDCOut.ProductName + ")";
            }
            #endregion
            
            #region Lot
            if (!String.IsNullOrEmpty(_oRouteSheetDCOut.LotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "LotID in(" + _oRouteSheetDCOut.LotNo + ")";
            }
            #endregion
            
            //#region ProductName 
            //if (!string.IsNullOrEmpty(_oRouteSheetDCOut.OperationUnitName))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " WorkingUnitID in ("+ _oRouteSheetDCOut.OperationUnitName + ")";
            //}
            //#endregion
            #region  Date
            if (ncboDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (ncboDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),[DateTime],106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: " + dStartDate.ToString("dd MMM yyyy");
                }
                else if (ncboDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),[DateTime],106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotEqualTo->" + dStartDate.ToString("dd MMM yyyy");
                }
                else if (ncboDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),[DateTime],106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: GreaterThen->" + dStartDate.ToString("dd MMM yyyy");
                }
                else if (ncboDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),[DateTime],106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: SmallerThen->" + dStartDate.ToString("dd MMM yyyy");
                }
                else if (ncboDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),[DateTime],106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: From " + dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy");
                }
                else if (ncboDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),[DateTime],106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotBetween " + dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region Store Name
            if (nOperationUnitName>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " WorkingUnitID =" + nOperationUnitName;
            }
            #endregion

            #region Shift
            if (nShiftName>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RSShiftID =" + nShiftName;
            }
            #endregion

            #region Dye Lot No
            if (!String.IsNullOrEmpty(_oRouteSheetDCOut.RouteSheetNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RouteSheetNo Like '%" + _oRouteSheetDCOut.RouteSheetNo + "%' ";
            }
            #endregion

            #region  Adv Date
            if (nAdvDateCbo != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nAdvDateCbo == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),[DateTime],106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dAdvStartDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: " + dAdvStartDate.ToString("dd MMM yyyy");
                }
                else if (nAdvDateCbo == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),[DateTime],106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dAdvStartDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotEqualTo->" + dAdvStartDate.ToString("dd MMM yyyy");
                }
                else if (nAdvDateCbo == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),[DateTime],106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dAdvStartDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: GreaterThen->" + dAdvStartDate.ToString("dd MMM yyyy");
                }
                else if (nAdvDateCbo == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),[DateTime],106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dAdvStartDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: SmallerThen->" + dAdvStartDate.ToString("dd MMM yyyy");
                }
                else if (nAdvDateCbo == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),[DateTime],106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dAdvStartDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dAdvEndDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: From " + dAdvStartDate.ToString("dd MMM yyyy") + " To " + dAdvEndDate.ToString("dd MMM yyyy");
                }
                else if (nAdvDateCbo == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),[DateTime],106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dAdvStartDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dAdvEndDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotBetween " + dAdvStartDate.ToString("dd MMM yyyy") + " To " + dAdvEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            //#region Business Unit
            //if (nBUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " BUID = " + nBUID;
            //}
            //#endregion
            string sSQL = sReturn1 + " " + sReturn;
            return sSQL;
        }

        private string MakeSQL_V2(RouteSheetDCOut oRouteSheetDCOut)
        {
            int ncboDate = 0;
            DateTime dStartDate = DateTime.Today;
            DateTime dEndDate = DateTime.Today;

            int nAdvDateCbo = 0;
            DateTime dAdvStartDate = DateTime.Today;
            DateTime dAdvEndDate = DateTime.Today;

            string sParams = oRouteSheetDCOut.ErrorMessage;
            string sRSNo = "";
            int nOperationUnitName = 0;
            int nShiftName = 0;
            int nBUID = 0;
            int nRSDCType = 0;

            if (sParams == null || sParams == "")
            {
                _oRouteSheetDCOut.ProductName = "";
                _oRouteSheetDCOut.OperationUnitName = "";
            }
            else
            {
                _oRouteSheetDCOut.ProductName = Convert.ToString(sParams.Split('~')[0]);
                _oRouteSheetDCOut.LotNo = Convert.ToString(sParams.Split('~')[1]);
                _oRouteSheetDCOut.OperationUnitName = Convert.ToString(sParams.Split('~')[2]);

                ncboDate = Convert.ToInt32(sParams.Split('~')[3]);
                dStartDate = Convert.ToDateTime(sParams.Split('~')[4]);
                dEndDate = Convert.ToDateTime(sParams.Split('~')[5]);
                sRSNo = Convert.ToString(sParams.Split('~')[6]);
                nOperationUnitName = Convert.ToInt32(sParams.Split('~')[7]); // Store Name
                nShiftName = Convert.ToInt32(sParams.Split('~')[8]);             //Shift
                _oRouteSheetDCOut.RouteSheetNo = Convert.ToString(sParams.Split('~')[9]);      //Dye Lot No

                nAdvDateCbo = Convert.ToInt32(sParams.Split('~')[10]);
                dAdvStartDate = Convert.ToDateTime(sParams.Split('~')[11]);
                dAdvEndDate = Convert.ToDateTime(sParams.Split('~')[12]);
                //nBUID = [13]
                
                if(sParams.Split('~').Count()>14)
                    nRSDCType = Convert.ToInt16(sParams.Split('~')[14]);
            }

            string sReturn1 = "";
            string sReturn = "";

            sReturn1 = "SELECT * FROM View_RouteSheetDCOutV2 ";

            #region RSNo
            if (!String.IsNullOrEmpty(sRSNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RouteSheetNo Like '%" + sRSNo + "%'";
            }
            #endregion

            #region Product
            if (!String.IsNullOrEmpty(_oRouteSheetDCOut.ProductName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID in(" + _oRouteSheetDCOut.ProductName + ")";
            }
            #endregion

            #region Lot
            if (!String.IsNullOrEmpty(_oRouteSheetDCOut.LotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "LotID in(" + _oRouteSheetDCOut.LotNo + ")";
            }
            #endregion

            //#region ProductName 
            //if (!string.IsNullOrEmpty(_oRouteSheetDCOut.OperationUnitName))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " WorkingUnitID in ("+ _oRouteSheetDCOut.OperationUnitName + ")";
            //}
            //#endregion

            #region  Date
            if (ncboDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (ncboDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApprovedDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: " + dStartDate.ToString("dd MMM yyyy hh:mm tt");
                }
                else if (ncboDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApprovedDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotEqualTo->" + dStartDate.ToString("dd MMM yyyy hh:mm tt");
                }
                else if (ncboDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApprovedDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: GreaterThen->" + dStartDate.ToString("dd MMM yyyy hh:mm tt");
                }
                else if (ncboDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApprovedDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: SmallerThen->" + dStartDate.ToString("dd MMM yyyy hh:mm tt");
                }
                else if (ncboDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApprovedDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: From " + dStartDate.ToString("dd MMM yyyy hh:mm tt") + " To " + dEndDate.ToString("dd MMM yyyy hh:mm tt");
                }
                else if (ncboDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApprovedDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotBetween " + dStartDate.ToString("dd MMM yyyy hh:mm tt") + " To " + dEndDate.ToString("dd MMM yyyy hh:mm tt");
                }
            }
            #endregion

            #region Store Name
            if (nOperationUnitName > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " WorkingUnitID =" + nOperationUnitName;
            }
            #endregion

            #region Shift
            if (nShiftName > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RSShiftID =" + nShiftName;
            }
            #endregion

            #region Dye Lot No
            if (!String.IsNullOrEmpty(_oRouteSheetDCOut.RouteSheetNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RouteSheetNo Like '%" + _oRouteSheetDCOut.RouteSheetNo + "%' ";
            }
            #endregion

            #region  Adv Date
            
            if (nAdvDateCbo != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nAdvDateCbo == (int)EnumCompareOperator.EqualTo)
                {
                    //sReturn = sReturn + " ApprovedDate BETWEEN CONVERT(VARCHAR, '" + dAdvStartDate + "', 100) AND CONVERT(VARCHAR, '" + dAdvStartDate.AddDays(1) + "', 100) ";
                    sReturn = sReturn + " ApprovedDate >= '" + dAdvStartDate.ToString("dd MMM yyyy hh:mm tt") + "' AND ApprovedDate < '" + dAdvStartDate.AddDays(1).ToString("dd MMM yyyy hh:mm tt") + "' ";
                    _sDateRange = "Date: " + dAdvStartDate.ToString("dd MMM yyyy hh:mm tt");
                }
                else if (nAdvDateCbo == (int)EnumCompareOperator.NotEqualTo)
                {
                    //sReturn = sReturn + " ApprovedDate != CONVERT(VARCHAR, '" + dAdvStartDate + "', 100)";
                    sReturn = sReturn + " ApprovedDate != '" + dAdvStartDate.ToString("dd MMM yyyy hh:mm tt") + "'";
                    _sDateRange = "Date: NotEqualTo->" + dAdvStartDate.ToString("dd MMM yyyy hh:mm tt");
                }
                else if (nAdvDateCbo == (int)EnumCompareOperator.GreaterThan)
                {
                    //sReturn = sReturn + " ApprovedDate > CONVERT(VARCHAR, '" + dAdvStartDate + "', 100)";
                    sReturn = sReturn + " ApprovedDate > '" + dAdvStartDate.ToString("dd MMM yyyy hh:mm tt") + "'";
                    _sDateRange = "Date: GreaterThen->" + dAdvStartDate.ToString("dd MMM yyyy hh:mm tt");
                }
                else if (nAdvDateCbo == (int)EnumCompareOperator.SmallerThan)
                {
                    //sReturn = sReturn + " ApprovedDate < CONVERT(VARCHAR, '" + dAdvStartDate + "', 100)";
                    sReturn = sReturn + " ApprovedDate < '" + dAdvStartDate.ToString("dd MMM yyyy hh:mm tt") + "'";
                    _sDateRange = "Date: SmallerThen->" + dAdvStartDate.ToString("dd MMM yyyy hh:mm tt");
                }
                else if (nAdvDateCbo == (int)EnumCompareOperator.Between)
                {
                    //sReturn = sReturn + " ApprovedDate BETWEEN CONVERT(VARCHAR, '" + dAdvStartDate + "', 100) AND CONVERT(VARCHAR, '" + dAdvEndDate + "', 100) ";
                    sReturn = sReturn + " ApprovedDate >= '" + dAdvStartDate.ToString("dd MMM yyyy hh:mm tt") + "' AND ApprovedDate <= '" + dAdvEndDate.ToString("dd MMM yyyy hh:mm tt") + "' ";
                    _sDateRange = "Date: From " + dAdvStartDate.ToString("dd MMM yyyy hh:mm tt") + " To " + dAdvEndDate.ToString("dd MMM yyyy hh:mm tt");
                }
                else if (nAdvDateCbo == (int)EnumCompareOperator.NotBetween)
                {
                    //sReturn = sReturn + " ApprovedDate NOT BETWEEN CONVERT(VARCHAR, '" + dAdvStartDate + "', 100) AND CONVERT(VARCHAR, '" + dAdvEndDate + "', 100) ";
                    sReturn = sReturn + " ApprovedDate < '" + dAdvStartDate.ToString("dd MMM yyyy hh:mm tt") + "' OR ApprovedDate > '" + dAdvEndDate.ToString("dd MMM yyyy hh:mm tt") + "' ";
                    _sDateRange = "Date: NotBetween " + dAdvStartDate.ToString("dd MMM yyyy hh:mm tt") + " To " + dAdvEndDate.ToString("dd MMM yyyy hh:mm tt");
                }
            }
            #endregion

            #region RS DC Type
            if (nRSDCType > 0)
            {
                Global.TagSQL(ref sReturn);
                if (nRSDCType == 100)
                {
                    sReturn = sReturn + " ISNULL(SequenceNo,0) = 0 AND InOutType = " + (int)EnumInOutType.Disburse;
                }
                else if (nRSDCType == 102)
                {
                    sReturn = sReturn + " ISNULL(SequenceNo,0) > 0 AND InOutType = " + (int)EnumInOutType.Disburse;
                }
                else if (nRSDCType == 101)
                {
                    sReturn = sReturn + " InOutType = " + (int)EnumInOutType.Receive;
                }
            }
            #endregion

            //#region Business Unit
            //if (nBUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " BUID = " + nBUID;
            //}
            //#endregion
            string sSQL = sReturn1 + " " + sReturn;
            return sSQL;
        }

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

        #endregion

        #region Print Report
        public ActionResult Print_DCOut(string sParams)
        {
            _oRouteSheetDCOut.ErrorMessage = sParams;
            string sSQL = MakeSQL_V2(_oRouteSheetDCOut);
            _oRouteSheetDCOuts = RouteSheetDCOut.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
           
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptRouteSheetDCOut oReport = new rptRouteSheetDCOut();
            byte[] abytes = oReport.PrepareReport(_oRouteSheetDCOuts, oCompanys.First(), oBusinessUnit, _sDateRange);
            return File(abytes, "application/pdf");
        }
        #endregion

        public ActionResult Print_RSDC_Summary(string sTempString)
        {
            _oRouteSheetDCOuts = new List<RouteSheetDCOut>();

            if (!string.IsNullOrEmpty(sTempString))
            {

                int nProductID = Convert.ToInt32(sTempString.Split('~')[0]);
                DateTime startDate = Convert.ToDateTime(sTempString.Split('~')[1]);
                DateTime endDate = Convert.ToDateTime(sTempString.Split('~')[2]);
                int nBUID = Convert.ToInt32(sTempString.Split('~')[3]);
                int nProductCategory = Convert.ToInt32(sTempString.Split('~')[4]);

                if (startDate > endDate) throw new Exception("Invalid Date. Start date can not be greater than end date!!");
                _sDateRange = "Between: " + startDate.ToString("dd MMM yyyy") + "  To  " + endDate.ToString("dd MMM yyyy");
                string sSQL = MakeSQL_Summary(nProductID, startDate, endDate, nProductCategory);
                _oRouteSheetDCOuts = RouteSheetDCOut.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompanys.First();
            if (oBusinessUnit.BusinessUnitID > 0)
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (_oRouteSheetDCOuts.Count == 0)
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("Sorry, No Data Found!!");
                return File(abytes, "application/pdf");
            }
            else
            {
                //List<SelectedField> oSelectedFields = new List<SelectedField>();
                //SelectedField oSelectedField = new SelectedField("ProductCode", "Product Code", 40, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("ProductName", "Dyes/Chemical Name", 120, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("MUName", "Unit", 20, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("Qty", " Qty", 40, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("QtyAdd", "Addition Qty", 40, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("QtyRet", "Return Qty", 40, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("Balance", "Total Qty", 40, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
               
                //rptDynamicReport oReport = new rptDynamicReport(595, 842);
                //oReport.FirstColumn = 16f;
                //oReport.SLNo = "#SL";
                //oReport.SpanTotal = 3;//ColSpanForTotal
                //byte[] abytes = oReport.PrepareReport(_oRouteSheetDCOuts.Cast<object>().ToList(), oBusinessUnit, oCompany, sHeader, oSelectedFields);
                //return File(abytes, "application/pdf");                
                rptRouteSheetDCOutList oReport = new rptRouteSheetDCOutList();
                byte[] abytes = oReport.PrepareReport(_oRouteSheetDCOuts, oCompany, _sDateRange);
                return File(abytes, "application/pdf");
            }
        }

        #region Print XlX
        public void PrintRpt_Excel(string sTempString)
        {
          
            string sMunit = "";

            _oRouteSheetDCOuts = new List<RouteSheetDCOut>();
            RouteSheetDCOut oRouteSheetDCOut = new RouteSheetDCOut();
            oRouteSheetDCOut.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oRouteSheetDCOut);
            _oRouteSheetDCOuts = RouteSheetDCOut.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oRouteSheetDCOuts.Count > 0)
            {
                sMunit = _oRouteSheetDCOuts[0].MUName;
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            int nSL = 0;
            double nTotalQty = 0;
          

            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 7;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("CHEMICAL_DYESTAFFISSUE");
                sheet.Name = "ISSUE Report(CHEMICAL & DYESTAFF)";
                sheet.Column(2).Width = 15;
                sheet.Column(3).Width = 40;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 30;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 30;
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
                cell.Value = "CHEMICAL & DYESTAFF ISSUE REPORT"; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion
                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "DATE:"+_sDateRange; cell.Style.Font.Bold = false;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Dyes/Chemical"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Qty(" + sMunit+")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Store Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Dyeing Lot No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "REMARKS"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #region Data
                foreach (RouteSheetDCOut oItem in _oRouteSheetDCOuts)
                {


                    //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.ProductCode; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ProductName.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00000)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.LocationName+"["+oItem.OperationUnitName+"]"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = (oItem.RouteSheetNo); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = (oItem.Note); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nTotalQty = nTotalQty + oItem.Qty;
                 
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

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
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

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = true;
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
                Response.AddHeader("content-disposition", "attachment; filename=IssueReport(DyesChemical).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        public void PrintRpt_Excel_V2(string sTempString)
        {
            string sMunit = ""; //View_RouteSheetDCOutV2

            _oRouteSheetDCOuts = new List<RouteSheetDCOut>();
            RouteSheetDCOut oRouteSheetDCOut = new RouteSheetDCOut();
            oRouteSheetDCOut.ErrorMessage = sTempString;

            string sSQL = MakeSQL_V2(oRouteSheetDCOut);
            _oRouteSheetDCOuts = RouteSheetDCOut.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oRouteSheetDCOuts.Count > 0)
            {
                sMunit = _oRouteSheetDCOuts[0].MUName;
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Export_To_Excel_V2(oBusinessUnit);
        }

        public void PrintRpt_Excel_Summary(string sTempString)
        {
            string sMunit = ""; //View_RouteSheetDCOutV2

            _oRouteSheetDCOuts = new List<RouteSheetDCOut>();
            RouteSheetDCOut oRouteSheetDCOut = new RouteSheetDCOut();
            oRouteSheetDCOut.ErrorMessage = sTempString;

            int nProductID = Convert.ToInt32(oRouteSheetDCOut.ErrorMessage.Split('~')[0]);
            DateTime startDate = Convert.ToDateTime(oRouteSheetDCOut.ErrorMessage.Split('~')[1]);
            DateTime endDate = Convert.ToDateTime(oRouteSheetDCOut.ErrorMessage.Split('~')[2]);

            if (startDate > endDate) throw new Exception("Invalid Date. Start date can not be greater than end date!!");


            string sReturn1 = "";
            string sReturn = "";

            sReturn1 = "SELECT * FROM View_RouteSheetDCOutV2 ";

            #region Product
            if (nProductID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID In (" + nProductID + ")";
            }
            #endregion

            #region Date
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApprovedDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + startDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + endDate.ToString("dd MMM yyyy") + "',106)) ";
            _sDateRange = "Date: From " + startDate.ToString("dd MMM yyyy") + " To " + endDate.ToString("dd MMM yyyy");
            #endregion

            string sSQL = sReturn1 + sReturn;
            _oRouteSheetDCOuts = RouteSheetDCOut.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oRouteSheetDCOuts.Count > 0)
            {
                sMunit = _oRouteSheetDCOuts[0].MUName;
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Export_To_Excel_V2(oBusinessUnit);
        }
        
        private void Export_To_Excel_V2(BusinessUnit oBusinessUnit)
        {
            #region Export Excel
            string sMUnit = "";
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 7;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("CHEMICAL_DYESTAFFISSUE");
                sheet.Name = "ISSUE Report(CHEMICAL & DYESTAFF)";


                //SL	Date	Batch No	Code	Dyes	Lot No	Qty	Unit	Store Name 	Shift	Request By	Issue By	Addition/Fresh/
                #region COLUMNS
                int nColCount = 2;
                sheet.Column(nColCount++).Width = 10;
                sheet.Column(nColCount++).Width = 15;
                sheet.Column(nColCount++).Width = 15;
                sheet.Column(nColCount++).Width = 20;

                sheet.Column(nColCount++).Width = 30;
                sheet.Column(nColCount++).Width = 20;
                sheet.Column(nColCount++).Width = 15;
                sheet.Column(nColCount++).Width = 10;
                
                sheet.Column(nColCount++).Width = 20;
                sheet.Column(nColCount++).Width = 20;
                sheet.Column(nColCount++).Width = 20;
                sheet.Column(nColCount++).Width = 20;
                sheet.Column(nColCount).Width = 15;
                nEndCol = nColCount;
                #endregion

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
                cell.Value = "CHEMICAL & DYESTAFF ISSUE REPORT"; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "DATE:" + _sDateRange; cell.Style.Font.Bold = false;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region HEADERS

                nStartCol = 2;
                //SL	Date	Batch No	Code	Dyes	Lot No	Qty	Unit	Store Name 	Shift	Request By	Issue By	Addition/Fresh/
                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Batch No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Dyes/Chemical"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Unit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Store Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Shift"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Request By"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Issue By"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Addition/Fresh"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #region Data
                int nSL = 0;
                double nTotalQty = 0;
                foreach (RouteSheetDCOut oItem in _oRouteSheetDCOuts)
                {
                    //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                    nStartCol = 2;

                    //SL	Date	       Batch No	    Code	    Dyes	    Lot No	Qty	    Unit	    Store Name 	      Shift	Request By	 Issue By	    Addition/Fresh/
                    //      ApprovedDateSt RouteSheetNo ProductCode ProductName LotNo   Qty     MUName      OperationUnitName Shift IssuedByName ApprovedByName InOutTypeSt
             
                    nSL++;
                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "" + oItem.ApprovedDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = (oItem.RouteSheetNo); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = (oItem.ProductCode); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = oItem.ProductName.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = oItem.LotNo.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00000)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = oItem.MUName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = oItem.LocationName + "[" + oItem.OperationUnitName + "]"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = (oItem.Shift); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = (oItem.IssuedByName); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = (oItem.ApprovedByName); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = (oItem.InOutTypeSt); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (oItem.InOutType == EnumInOutType.Disburse && oItem.SequenceNo > 0)
                        nTotalQty = nTotalQty + oItem.Qty;
                    else if (oItem.InOutType == EnumInOutType.Disburse && oItem.SequenceNo <= 0)
                        nTotalQty = nTotalQty + oItem.Qty;
                    else if (oItem.InOutType == EnumInOutType.Receive)
                        nTotalQty = nTotalQty - oItem.Qty;

                    sMUnit = oItem.MUName;
                    nEndRow = nRowIndex;
                    nRowIndex++;
                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Merge = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00) ";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = sMUnit; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "" + ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = ""; cell.Style.Font.Bold = true;
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
                Response.AddHeader("content-disposition", "attachment; filename=IssueReport(DyesChemical).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion

        }

        #endregion
    }
}
