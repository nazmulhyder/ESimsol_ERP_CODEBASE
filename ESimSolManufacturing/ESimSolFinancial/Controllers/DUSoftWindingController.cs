using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;

using ICS.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using iTextSharp.text.pdf;
using System.Web.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class DUSoftWindingController : Controller
    {
        #region Declaration
        DUSoftWinding _oDUSoftWinding = new DUSoftWinding();
        List<DUSoftWinding> _oDUSoftWindings = new List<DUSoftWinding>();
        #endregion

        #region Functions
        public ActionResult ViewDUSoftWindings(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DUSoftWinding).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            _oDUSoftWindings = new List<DUSoftWinding>();
            _oDUSoftWindings = DUSoftWinding.Gets("SELECT top(300)* FROM View_DUSoftWinding WHERE [Status] NOT IN (" + (int)EnumWindingStatus.Completed + "," + (int)EnumWindingStatus.Delivered + ")", (int)Session[SessionInfo.currentUserID]);
            return View(_oDUSoftWindings);
        }
        public ActionResult ViewDUSoftWinding(int id, int buid)
        {
            _oDUSoftWinding = new DUSoftWinding();
            if (id > 0)
            {
                _oDUSoftWinding = _oDUSoftWinding.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.DUSoftWindingTypes = EnumObject.jGets(typeof(EnumColorType));

            #region Machine
            List<Machine> oMachines = new List<Machine>();
            oMachines = Machine.GetsByModule(buid, ""+(int)EnumModuleName.DUSoftWinding,  ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion
            
            ViewBag.RSShifts = RSShift.GetsByModule(buid, (int)EnumModuleName.DUSoftWinding + "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Machines = oMachines;
            List<EnumObject> oEnumObjects = new List<EnumObject>();
            oEnumObjects = EnumObject.jGets(typeof(EnumInOutType));
            foreach(EnumObject oItem in  oEnumObjects)
            {
                if (oItem.id == 101) { oItem.Value = "Gain"; }
                if (oItem.id == 102) { oItem.Value = "Loss"; }
            }
            ViewBag.GainLossTypes = oEnumObjects;
            return View(_oDUSoftWinding);
        }

        public ActionResult ViewDUSoftWindingYarnOut(int id, int buid)
        {
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            string sSQL = "";
            //if (buid <= 0)
            //{
            //    //_oDUSoftWinding.LotNo = _oDUSoftWinding.UpdateRSLot(id, (int)Session[SessionInfo.currentUserID]);
            //    _oDUSoftWinding = _oDUSoftWinding.Get(id, (int)Session[SessionInfo.currentUserID]);
            //    sSQL = "Select * from View_RouteSheet Where RSState>0 ";
            //    sSQL = sSQL + "AND RouteSheetID in (Select RoutesheetID from RSRawLot where LotID=" + _oDUSoftWinding .LotID+ ") and RouteSheetID in (select RouteSheetID from RouteSheetDO where  DyeingOrderDetailID in (select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID=" + _oDUSoftWinding.DyeingOrderID + " ))"; //ProductID=" + _oDUSoftWinding.ProductID + " and 
            //    oRouteSheets = RouteSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            //}
            //else
            //{
            if (id > 0)
            {
                _oDUSoftWinding.LotNo = _oDUSoftWinding.UpdateRSLot(id, (int)Session[SessionInfo.currentUserID]);
                _oDUSoftWinding = _oDUSoftWinding.Get(id, (int)Session[SessionInfo.currentUserID]);
                sSQL = "Select * from View_RouteSheet Where RSState >=" + (int)EnumRSState.InFloor + " ";
                sSQL = sSQL + "AND  RouteSheetID in (Select RoutesheetID from RSRawLot where LotID=" + _oDUSoftWinding.LotID + ") and RouteSheetID in (select RouteSheetID from RouteSheetDO where  DyeingOrderDetailID in (select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID=" + _oDUSoftWinding.DyeingOrderID + " )) order by RSState"; //ProductID=" + _oDUSoftWinding.ProductID + " and 

                oRouteSheets = RouteSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach (RouteSheet oItem in oRouteSheets)
                {
                    if (oItem.RSState == EnumRSState.InFloor)
                    {
                        oItem.LotID = _oDUSoftWinding.LotID;
                        oItem.WorkingUnitID = _oDUSoftWinding.WorkingUnitID;
                        oItem.ProductID = _oDUSoftWinding.ProductID;
                        oItem.ProductID_Raw = _oDUSoftWinding.ProductID;
                    }
                }
            }
            else
            {
                sSQL = "Select * from View_RouteSheet Where RSState in (" + (int)EnumRSState.InFloor + ")";// and LotID in (Select LotID from DUSoftWinding)
                oRouteSheets = RouteSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }


            #region Issue Stores
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.RouteSheet, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

            ViewBag.RSShifts = RSShift.GetsByModule(buid, (int)EnumModuleName.RouteSheet + "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.WorkingUnits = oWorkingUnits;
            return View(oRouteSheets);
        }
        public ActionResult AdvSearchDUSoftWinding()
        {
            return PartialView();
        }

        #region SW OPEN
        public ActionResult ViewDUSoftWindings_Open(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DUSoftWinding).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.RouteSheetSetup = oRouteSheetSetup;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            _oDUSoftWindings = new List<DUSoftWinding>();
            _oDUSoftWindings = DUSoftWinding.Gets("SELECT * FROM View_DUSoftWinding_Open WHERE [Status] NOT IN (" + (int)EnumWindingStatus.Completed + "," + (int)EnumWindingStatus.Delivered + ")", (int)Session[SessionInfo.currentUserID]);
            return View(_oDUSoftWindings);
        }
        public ActionResult ViewDUSoftWinding_Open(int id, int buid)
        {
            _oDUSoftWinding = new DUSoftWinding();
            if (id > 0)
            {
                _oDUSoftWinding = _oDUSoftWinding.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.DUSoftWindingTypes = EnumObject.jGets(typeof(EnumColorType));

            #region Machine
            List<Machine> oMachines = new List<Machine>();
            oMachines = Machine.GetsByModule(buid, "" + (int)EnumModuleName.DUSoftWinding, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            ViewBag.RSShifts = RSShift.GetsByModule(buid, (int)EnumModuleName.DUSoftWinding + "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Machines = oMachines;
            return View(_oDUSoftWinding);
        }
        public ActionResult ViewDUSoftWindingYarnOut_Open(int id, int buid)
        {
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            string sSQL = "";
         
            if (id > 0)
            {
               // _oDUSoftWinding.LotNo = _oDUSoftWinding.UpdateRSLot(id, (int)Session[SessionInfo.currentUserID]);
                _oDUSoftWinding = _oDUSoftWinding.Get(id, (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM View_RouteSheet WHERE RSState >=" + (int)EnumRSState.InFloor + " ";
                sSQL = sSQL + "AND RouteSheetID IN (SELECT RoutesheetID FROM RSRawLot WHERE LotID=" + _oDUSoftWinding.LotID + ") ORDER BY RSState"; //ProductID=" + _oDUSoftWinding.ProductID + " and 
                
                oRouteSheets = RouteSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach (RouteSheet oItem in oRouteSheets)
                {
                    if (oItem.RSState == EnumRSState.InFloor)
                    {
                        oItem.LotID = _oDUSoftWinding.LotID;
                        oItem.WorkingUnitID = _oDUSoftWinding.WorkingUnitID;
                        oItem.ProductID = _oDUSoftWinding.ProductID;
                        oItem.ProductID_Raw = _oDUSoftWinding.ProductID;
                    }
                }
            }
            else
            {
                sSQL = "Select * from View_RouteSheet Where HanksCone IN (" + (int)EumDyeingType.Cone + ") AND RSState in (" + (int)EnumRSState.InFloor + ")";// and LotID in (Select LotID from DUSoftWinding)
                oRouteSheets = RouteSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }

            #region Issue Stores
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.RouteSheet, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

            ViewBag.RSShifts = RSShift.GetsByModule(buid, (int)EnumModuleName.RouteSheet + "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.WorkingUnits = oWorkingUnits;
            ViewBag.BUID = buid;
            return View(oRouteSheets);
        }
        public ActionResult AdvSearchDUSoftWinding_Open()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult SearchByLotNo(DUSoftWinding oDUSoftWinding)
        {
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            try
            {
                string sSQL = "Select * from View_RouteSheet Where RSState in (" + (int)EnumRSState.InFloor + ") ";

                if (!string.IsNullOrEmpty(oDUSoftWinding.LotNo))
                {
                    sSQL = sSQL + " AND ISNULL(LotNo,'')+ISNULL(RouteSheetNo,'')  LIKE '%" + oDUSoftWinding.LotNo + "%' ";
                }
               
                //sSQL = sSQL + " AND RouteSheetID IN (select RouteSheetID from RouteSheetDO where  DyeingOrderDetailID in (select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID=" + _oDUSoftWinding.DyeingOrderID + " ))";
                
                oRouteSheets = RouteSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDUSoftWinding.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult Save(DUSoftWinding oDUSoftWinding)
        {
            _oDUSoftWinding = new DUSoftWinding();
            try
            {
                _oDUSoftWinding = oDUSoftWinding;
                _oDUSoftWinding = _oDUSoftWinding.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUSoftWinding = new DUSoftWinding();
                _oDUSoftWinding.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUSoftWinding);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                DUSoftWinding oDUSoftWinding = new DUSoftWinding();
                sFeedBackMessage = oDUSoftWinding.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult RouteSheetYarnOut(string sTemp)
        {
            DUSoftWinding oDUSoftWinding = new DUSoftWinding();
            RouteSheet oRouteSheet = new RouteSheet();
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            try
            {
                if (string.IsNullOrEmpty(sTemp)) throw new Exception("Please select a valid routesheet.");

                //int.TryParse(oRouteSheet.Params, out nEventEmpID);
                //string ErrorMessage = RSStateValidation(oRouteSheet.RSState, EnumRSState.YarnOut);
                //if (ErrorMessage != "") { throw new Exception(ErrorMessage); }

                oRouteSheets = oDUSoftWinding.YarnOut(sTemp, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheets = new List<RouteSheet>();
                oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = ex.Message;
                oRouteSheets.Add(oRouteSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchByNo(DUSoftWinding oDUSoftWinding)
        {
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            try
            {
                string sSQL = "Select * from View_RouteSheet Where RSState in (" + (int)EnumRSState.InFloor + ") ";

                if (oDUSoftWinding.DyeingOrderID> 0)
                {
                    sSQL = sSQL + " AND RouteSheetID in (select RouteSheetID from RouteSheetDO where  DyeingOrderDetailID in (select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID=" + _oDUSoftWinding.DyeingOrderID + " ))";
                }
                if(!string.IsNullOrEmpty(oDUSoftWinding.DyeingOrderNo))
                {
                    sSQL = sSQL + " AND ISNULL(OrderNo,'')+ISNULL(RouteSheetNo,'')  LIKE '%" + oDUSoftWinding.DyeingOrderNo + "%' "; 
                }
                oRouteSheets = RouteSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDUSoftWinding.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult SWSearchByNo(DUSoftWinding oDUSoftWinding)
        {
            List<DUSoftWinding> oDUSoftWindings = new List<DUSoftWinding>();
            try
            {
                string sReturn1 = "SELECT * FROM View_DUSoftWinding";
                string sReturn = "";

                #region Order No
                if (!string.IsNullOrEmpty(oDUSoftWinding.DyeingOrderNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DyeingOrderNo Like '%" + oDUSoftWinding.DyeingOrderNo + "%'";
                }
                #endregion

                #region Lot No Only For Open
                if (!string.IsNullOrEmpty(oDUSoftWinding.LotNo))
                {
                    sReturn1 = "SELECT * FROM View_DUSoftWinding_Open";
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LotNo Like '%" + oDUSoftWinding.LotNo + "%'";
                }
                #endregion

                string sSQL = sReturn1 + sReturn + " Order by LotID,ReceiveDate";
                
                oDUSoftWindings = DUSoftWinding.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDUSoftWindings = new List<DUSoftWinding>();
                oDUSoftWindings.Add(new DUSoftWinding() {ErrorMessage= ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUSoftWindings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
         
        #endregion

        #region View Report & Actions
        public ActionResult ViewDUSoftWindings_Rpt(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DUSoftWinding).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            _oDUSoftWindings = new List<DUSoftWinding>();
            return View(_oDUSoftWindings);
        }
        [HttpPost]
        public JsonResult SearchByNo_Rpt(DUSoftWinding oDUSoftWinding)
        {
            List<DUSoftWinding> oDUSoftWindings = new List<DUSoftWinding>();
            try
            {
                string sReturn1 = "SELECT DyeingOrderID FROM DyeingOrder";
                string sReturn = "";

                #region Order No
                if (!string.IsNullOrEmpty(oDUSoftWinding.DyeingOrderNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderNo Like '%" + oDUSoftWinding.DyeingOrderNo + "%'";
                }
                #endregion

                #region Lot No Only For Open
                if (!string.IsNullOrEmpty(oDUSoftWinding.LotNo))
                {
                    sReturn1 = "SELECT DyeingOrderID FROM View_DUSoftWinding_Open";
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LotNo Like '%" + oDUSoftWinding.LotNo + "%'";
                }
                #endregion

                string sSQL = " AND DURD.DyeingOrderID IN ("+ sReturn1 + sReturn + ")";

                oDUSoftWindings = DUSoftWinding.Gets_Report(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDUSoftWindings = new List<DUSoftWinding>();
                oDUSoftWindings.Add(new DUSoftWinding() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUSoftWindings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        #region Advance Search [report]
        public ActionResult AdvSearchDUSoftWinding_Report()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult GetLots_Rpt(Lot oLot)
        {
            List<Lot> oLots = new List<Lot>();
            try
            {
                string sReturn1 = "SELECT * FROM View_Lot";
                string sReturn = "";

                #region LotNo No
                if (!string.IsNullOrEmpty(oLot.LotNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LotID IN (SELECT DestinationLotID FROM View_DURequisitionDetail WHERE DesignationLotNo LIKE '%" + oLot.LotNo + "%' )";
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LotID IN (SELECT DestinationLotID FROM DURequisitionDetail)";
                }
                #endregion

                string sSQL = sReturn1 + sReturn;

                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
                oLots.Add(new Lot() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult AdvSearch_Rpt(string sTemp)
        {
            List<DUSoftWinding> oDUSoftWindings = new List<DUSoftWinding>();
            DUSoftWinding oDUSoftWinding = new DUSoftWinding();
            try
            {
                string sSQL = GetSQL_Rpt(sTemp);
                oDUSoftWindings = DUSoftWinding.Gets_Report(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDUSoftWinding.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUSoftWindings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQL_Rpt(string sTemp)
        {
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();

            int nCount = 0;
            int nOrderDateCompare = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dtOrderDateStart = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            DateTime dtDUSoftWindingEndDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            int nReceiveDateCompare = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dtReceiveDateStart = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            DateTime dtReceiveEndDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            int nIssueDateCompare = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dtIssueDateStart = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            DateTime dtIssueEndDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);

            string sOrderNo = Convert.ToString(sTemp.Split('~')[nCount++]);
            string sLotIDs = Convert.ToString(sTemp.Split('~')[nCount++]);
            bool nYTStart = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            bool nYTComplete = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            bool nYTDelivery = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            string sBuyerIDs = Convert.ToString(sTemp.Split('~')[nCount++]);
            //int nRequisitionType = Convert.ToInt32(sTemp.Split('~')[9]);
            //string sRequisitionNo = Convert.ToString(sTemp.Split('~')[10]);

            string sReturn1 = ""; // "SELECT DyeingOrderID FROM View_DUSoftWinding";
            string sReturn = " AND DURD.DyeingOrderID > 0";

            //#region BUID
            //if (nBUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "BUID = " + nBUID;
            //}
            //#endregion

            #region Order No
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DURD.DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE OrderNo Like '%" + sOrderNo + "%' )";
            }

            #endregion
            #region LotNo
            if (!string.IsNullOrEmpty(sLotIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DestinationLotID IN ("+ sLotIDs + ")";
            }
            #endregion
            #region nYTStart
            //if (nYTStart)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "ISNULL(Status,0)= " + (int)EnumWindingStatus.Initialize;
            //}
            #endregion
            #region nYTComplete
            //if (nYTComplete)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "ISNULL(Status,0)= " + (int)EnumWindingStatus.Running;
            //}
            #endregion
            #region nYTDelivery
            //if (nYTDelivery)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "(ISNULL(Qty,0)-ISNULL(Qty_RSOut,0))>0.5";
            //}
            #endregion
            #region Order Date Wise
            if (nOrderDateCompare > 0)
            {
                if (nOrderDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDUSoftWindingEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDUSoftWindingEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region Issue Date Wise
            if (nIssueDateCompare > 0)
            {
                if (nIssueDateCompare == 1)
                {
                    dtIssueEndDate = dtIssueDateStart;
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReqDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtIssueDateStart.ToString("dd MMM yyyy") + "',106))";
                    sReturn = sReturn + "ReqDate>='" + dtIssueDateStart.ToString("dd MMM yyyy 10:00") + "' and  ReqDate<'" + dtIssueEndDate.AddDays(1).ToString("dd MMM yyyy 10:00") + "'";
                }
                if (nIssueDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReqDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtIssueDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nIssueDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReqDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtIssueDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nIssueDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReqDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtIssueDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nIssueDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ReqDate>='" + dtIssueDateStart.ToString("dd MMM yyyy 10:00") + "' and  ReqDate<'" + dtIssueEndDate.AddDays(1).ToString("dd MMM yyyy 10:00") + "'";

                }
                if (nIssueDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReqDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtIssueDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtIssueEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region ReceiveDate Date Wise
            if (nReceiveDateCompare > 0)
            {
                if (nReceiveDateCompare == 1)
                {
                    dtReceiveEndDate = dtReceiveDateStart;
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ReceiveDate>='" + dtReceiveDateStart.ToString("dd MMM yyyy 10:00") + "' and  ReceiveDate<'" + dtReceiveEndDate.AddDays(1).ToString("dd MMM yyyy 10:00") + "'";
                }
                if (nReceiveDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ReceiveDate>='" + dtReceiveDateStart.ToString("dd MMM yyyy 10:00") + "' and  ReceiveDate<'" + dtReceiveEndDate.AddDays(1).ToString("dd MMM yyyy 10:00") + "'";
                }
                if (nReceiveDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region Buyer Ids
            if (!string.IsNullOrEmpty(sBuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + sBuyerIDs + ")";
            }
            #endregion
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        public ActionResult PrintDUSoftWinding_Report(string sTemp)
        {
            List<DUSoftWinding> oDUSoftWindingList = new List<DUSoftWinding>();
            DUSoftWinding oDUSoftWinding = new DUSoftWinding();

            int nBUID; string sDateRange = "";
            if (string.IsNullOrEmpty(sTemp))
            {
                throw new Exception("Nothing To Print.");
            }
            else
            {
                string sSQL = GetSQL_Rpt(sTemp);
                oDUSoftWindingList = DUSoftWinding.Gets_Report(sSQL, (int)Session[SessionInfo.currentUserID]);

                int nOrderDateCompare = Convert.ToInt32(sTemp.Split('~')[0]);
                DateTime dtOrderDateStart = Convert.ToDateTime(sTemp.Split('~')[1]);
                DateTime dtDUSoftWindingEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

                if (nOrderDateCompare == (int)EnumCompareOperator.EqualTo)
                    sDateRange = "Order Date: " + dtOrderDateStart.ToString("dd MMM yyyy");
                if (nOrderDateCompare == (int)EnumCompareOperator.Between)
                    sDateRange = "Order Date: " + dtOrderDateStart.ToString("dd MMM yyyy") + " To " + dtDUSoftWindingEndDate.ToString("dd MMM yyyy");

                nBUID = Convert.ToInt32(sTemp.Split('~')[14]);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptDUSoftWindingReport oReport = new rptDUSoftWindingReport();
            byte[] abytes = oReport.PrepareReport(oDUSoftWindingList, oCompany, oBusinessUnit, "Soft Winding Report", sDateRange);
            return File(abytes, "application/pdf");
        }
        #region Print PDF
        public ActionResult PrintOrderSWStatement(int nID,  int nBUID) //nID :: FEOSID
        {
            List<RPT_Dispo> oRPT_Dispos = new List<RPT_Dispo>();
            List<RPT_Dispo> oRPT_DisposReport = new List<RPT_Dispo>();
            List<DURequisitionDetail> oDURequisitionDetails = new List<DURequisitionDetail>();
            List<RSRawLot> oRSRawLots = new List<RSRawLot>();

            string sSQL = "SELECT * FROM View_DURequisitionDetail WHERE DyeingOrderID =" + nID;
            // + " AND ProductID = "+ nPID ;
            oDURequisitionDetails = DURequisitionDetail.Gets(sSQL + " ORDER BY ReqDate, RequisitionNo", ((User)Session[SessionInfo.CurrentUser]).UserID);

            string sTemo = string.Join(",", oDURequisitionDetails.Select(x => x.DestinationLotID).ToList());

            if (string.IsNullOrEmpty(sTemo))
                sTemo = "0";
            sSQL = "Select * from View_RSRawLot where LotID in (" + sTemo + ")  and RouteSheetID in (Select RouteSheetID from View_RouteSheetDO where DyeingOrderID =" + nID + ")";
            oRSRawLots = RSRawLot.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);


//            sSQL = " WHERE DODF.FEOSID = " + nID;
//            oRPT_Dispos = RPT_Dispo.Gets_FYStockDispoWise(sSQL, 1, (int)Session[SessionInfo.currentUserID]);
//            sSQL = "select * ,(Select SUM(Qty) from DURequisitionDetail as DUR where   DURequisitionID in (SELECT DURequisitionID FROM DURequisition WHERE RequisitionType=101 AND ISNULL(ReceiveByID,0)<>0) AND  LotID  in (Select LotID from Lot where LotID=HH.LotID or ParentLotID=HH.LotID) and   DyeingOrderID =HH.DyeingOrderID )as Qty_SRS"
//+ " ,(Select SUM(Qty) from DURequisitionDetail as DUR where   DURequisitionID in (SELECT DURequisitionID FROM DURequisition WHERE RequisitionType=102 AND ISNULL(ReceiveByID,0)<>0) AND  DestinationLotID in (Select LotID from Lot where LotID=HH.LotID or ParentLotID=HH.LotID) and   DyeingOrderID =HH.DyeingOrderID) as Qty_SRM"
//+ ",(Select SUM(Qty) from DyeingOrderFabricDetail as DODF where DODF.FEOSID=HH.FEOSID and DODF.ProductID=HH.ProductID ) as Qty_Dispo"
//+ ",isnull((Select SUM(Qty) from RSRawLot where LotID  in (Select LotID from Lot where LotID=HH.LotID or ParentLotID=HH.LotID) and RouteSheetID in (Select RouteSheetID from View_RouteSheetDO where  DyeingOrderID=HH.DyeingOrderID )),0) as Qty_Dye"
//+ " from (Select FLA.LotID, DOF.DyeingOrderID,DOF.ProductID, DOF.FEOSID, Lot.LotNo as StyleNo, Product.ProductName,SUM(FLA.Qty)as Req_GreyYarn from FabricLotAssign as FLA left join DyeingOrderFabricDetail as DOF ON FLA.FEOSDID =DOF.FEOSDID left join Lot as Lot ON Lot.LotID =FLA.LotID"
//+ " left join Product as Product ON Product.ProductID =DOF.ProductID where DOF.FEOSID=" + nID + "  group by FEOSID, FLA.LotID, DOF.DyeingOrderID,DOF.ProductID,Lot.LotNo,Product.ProductName) as HH order by HH.DyeingOrderID";
//            oRPT_DisposReport = RPT_Dispo.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            rptDUSoftWinding oReport = new rptDUSoftWinding();
            rptErrorMessage oErrorReport = new rptErrorMessage();
            byte[] abytes = new byte[1];

            if (oDURequisitionDetails.Count > 0)
            {
                var sDispo = "(" + oDURequisitionDetails.Select(x => x.DyeingOrderNo).FirstOrDefault() + ")";
                abytes = oReport.PrepareReportOrderStatement(" Dispo Wise Store Report " + sDispo,  oDURequisitionDetails, oRSRawLots, oBusinessUnit, oCompany);
            }
            else
            {
                abytes = oErrorReport.PrepareReport("No Data");
            }

            return File(abytes, "application/pdf");
        }
        #endregion

        #endregion
        #endregion

        #region Validation
        private string RSStateValidation(EnumRSState CurrentState, EnumRSState NextState)
        {
            try
            {
                if (CurrentState == EnumRSState.None)
                    throw new Exception("Invalid routesheet status.");

                if (CurrentState == EnumRSState.Initialized && NextState != EnumRSState.InFloor)
                    throw new Exception("Reqired to send in floor.");

                if (CurrentState == EnumRSState.InFloor && NextState != EnumRSState.Initialized && NextState != EnumRSState.YarnOut)
                    throw new Exception("Reqired to yarn In Floor status.");
                //if (CurrentState == EnumRSState.Approved &&  NextState != EnumRSState.InYarnStore)
                //    throw new Exception("Reqired to yarn In Yarn Store.");

                if (CurrentState == EnumRSState.YarnOut && NextState != EnumRSState.LoadedInDyeMachine)
                    throw new Exception("Reqired to loaded in dye machine.");

                if (CurrentState == EnumRSState.LoadedInDyeMachine && NextState != EnumRSState.UnloadedFromDyeMachine)
                    throw new Exception("Reqired to unload from dye machine.");

                if (CurrentState == EnumRSState.UnloadedFromDyeMachine && NextState != EnumRSState.LoadedInHydro)
                    throw new Exception("Reqired to loaded in hydro machine.");

                if (CurrentState == EnumRSState.LoadedInHydro && NextState != EnumRSState.UnloadedFromHydro)
                    throw new Exception("Reqired to unload from hydro machine.");

                if (CurrentState == EnumRSState.UnloadedFromHydro && NextState != EnumRSState.LoadedInDrier)
                    throw new Exception("Reqired to loaded in drier machine.");

                if (CurrentState == EnumRSState.LoadedInDrier && NextState != EnumRSState.UnLoadedFromDrier)
                    throw new Exception("Reqired to unload from drier machine.");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }

        #endregion

        #region Advance Search
        [HttpGet]
        public JsonResult AdvSearch(string sTemp)
        {
            List<DUSoftWinding> oDUSoftWindings = new List<DUSoftWinding>();
            DUSoftWinding oDUSoftWinding = new DUSoftWinding();
            try
            {
                string sSQL = GetSQL(sTemp);
                oDUSoftWindings = DUSoftWinding.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDUSoftWinding.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUSoftWindings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQL(string sTemp)
        {
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();

            int nCount = 0;
            int nOrderDateCompare = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dtOrderDateStart = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            DateTime dtDUSoftWindingEndDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            int nReceiveDateCompare = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dtReceiveDateStart = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            DateTime dtReceiveEndDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            string sOrderNo = Convert.ToString(sTemp.Split('~')[nCount++]);
            string sLotNo = Convert.ToString(sTemp.Split('~')[nCount++]);
            bool nYTStart = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            bool nYTComplete = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            bool nYTDelivery = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            string sBuyerIDs = Convert.ToString(sTemp.Split('~')[nCount++]);
            //int nRequisitionType = Convert.ToInt32(sTemp.Split('~')[9]);
            //string sRequisitionNo = Convert.ToString(sTemp.Split('~')[10]);

            string sReturn1 = "SELECT * FROM View_DUSoftWinding";
            string sReturn = "";

            //#region BUID
            //if (nBUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "BUID = " + nBUID;
            //}
            //#endregion

            #region Order No
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderNo Like '%" + sOrderNo + "%'";
            }
           
            #endregion
            #region LotNo
            if (!string.IsNullOrEmpty(sLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LotNo Like '%" + sLotNo + "%'";
            }
            #endregion
            #region nYTStart
            if (nYTStart)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ISNULL(Status,0)= " + (int)EnumWindingStatus.Initialize;
            }
            #endregion
            #region nYTComplete
            if (nYTComplete)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ISNULL(Status,0)= " + (int)EnumWindingStatus.Running;
            }
            #endregion
            #region nYTDelivery
            if (nYTDelivery)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "(ISNULL(Qty,0)-ISNULL(Qty_RSOut,0))>0.5";
            }
            #endregion
            #region Order Date Wise
            if (nOrderDateCompare > 0)
            {
                if (nOrderDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDUSoftWindingEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtOrderDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDUSoftWindingEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region Receive Date Wise
            if (nReceiveDateCompare > 0)
            {
                if (nReceiveDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region Buyer Ids
            if (!string.IsNullOrEmpty(sBuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE ContractorID IN ("+ sBuyerIDs +"))";
            }
            #endregion
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region Advance Search [OPEN]
        [HttpGet]
        public JsonResult AdvSearch_Open(string sTemp)
        {
            List<DUSoftWinding> oDUSoftWindings = new List<DUSoftWinding>();
            DUSoftWinding oDUSoftWinding = new DUSoftWinding();
            try
            {
                string sSQL = GetSQL_Open(sTemp);
                oDUSoftWindings = DUSoftWinding.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDUSoftWinding.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUSoftWindings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQL_Open(string sTemp)
        {
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();

            int nCount = 0;
            int nReceiveDateCompare = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dtReceiveDateStart = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            DateTime dtReceiveEndDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
          
            string sLotNo = Convert.ToString(sTemp.Split('~')[nCount++]);
            bool nYTStart = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            bool nYTComplete = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            bool nYTDelivery = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            int nRequisitionType = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            string sYarnIDs = Convert.ToString(sTemp.Split('~')[nCount++]);

            string sReturn1 = "SELECT * FROM View_DUSoftWinding_Open";
            string sReturn = "";

            //#region BUID
            //if (nBUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "BUID = " + nBUID;
            //}
            //#endregion

            #region LotNo
            if (!string.IsNullOrEmpty(sLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LotNo Like '%" + sLotNo + "%'";
            }
            #endregion

            #region nYTStart
            if (nYTStart)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ISNULL(Status,0)= " + (int)EnumWindingStatus.Initialize;
            }
            #endregion
            #region nYTComplete
            if (nYTComplete)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ISNULL(Status,0)= " + (int)EnumWindingStatus.Running;
            }
            #endregion
            #region nYTDelivery
            if (nYTDelivery)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "(ISNULL(Qty,0)-ISNULL(Qty_RSOut,0))>0.5";
            }
            #endregion

            #region Receive Date Wise
            if (nReceiveDateCompare > 0)
            {
                if (nReceiveDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                    dtReceiveEndDate = dtReceiveDateStart;
                    sReturn += " ReceiveDate>='" + dtReceiveDateStart.ToString("dd MMM yyyy HH:00") + "' AND ReceiveDate<'" + dtReceiveEndDate.AddDays(1).ToString("dd MMM yyyy HH:00") + "'";
                }
                if (nReceiveDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                    sReturn = sReturn + " ReceiveDate != CONVERT(DATETIME,convert(varchar, '" + dtReceiveDateStart + "',105))";
                }
                if (nReceiveDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                    sReturn = sReturn + " ReceiveDate > CONVERT(DATETIME,convert(varchar, '" + dtReceiveDateStart + "',105))";
                }
                if (nReceiveDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                    sReturn = sReturn + " ReceiveDate < CONVERT(DATETIME,convert(varchar, '" + dtReceiveDateStart + "',105))";
                }
                if (nReceiveDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveEndDate.ToString("dd MMM yyyy") + "',106))";
                    sReturn = sReturn + " ReceiveDate>='" + dtReceiveDateStart.ToString("dd MMM yyyy HH:mm") + "' AND ReceiveDate<'" + dtReceiveEndDate.ToString("dd MMM yyyy HH:mm") + "'";
                }
                if (nReceiveDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveEndDate.ToString("dd MMM yyyy") + "',106))";
                    sReturn = sReturn + " ReceiveDate<='" + dtReceiveDateStart.ToString("dd MMM yyyy HH:mm") + "' AND ReceiveDate>'" + dtReceiveEndDate.ToString("dd MMM yyyy HH:mm") + "'";
                }
            }
            #endregion

            #region Yarn
            if (!string.IsNullOrEmpty(sYarnIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductID IN (" + sYarnIDs + ")";
            }
            #endregion

            sReturn = sReturn1 + sReturn + " Order by LotID,ReceiveDate";
            return sReturn;
        }
        #endregion

        #region Report
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
        public ActionResult PrintDUSoftWinding(string sTemp)
        {
            List<DUSoftWinding> oDUSoftWindingList = new List<DUSoftWinding>();
            DUSoftWinding oDUSoftWinding = new DUSoftWinding();

            int nBUID; string sDateRange = "";
            if (string.IsNullOrEmpty(sTemp))
            {
                throw new Exception("Nothing To Print.");
            }
            else
            {
                string sSQL = GetSQL(sTemp);
                oDUSoftWindingList = DUSoftWinding.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                int nOrderDateCompare = Convert.ToInt32(sTemp.Split('~')[0]);
                DateTime dtOrderDateStart = Convert.ToDateTime(sTemp.Split('~')[1]);
                DateTime dtDUSoftWindingEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

                if (nOrderDateCompare == (int)EnumCompareOperator.EqualTo)
                    sDateRange = "Order Date: " + dtOrderDateStart.ToString("dd MMM yyyy");
                if (nOrderDateCompare == (int)EnumCompareOperator.Between)
                    sDateRange = "Order Date: " + dtOrderDateStart.ToString("dd MMM yyyy") + " To " + dtDUSoftWindingEndDate.ToString("dd MMM yyyy");

                nBUID = Convert.ToInt32(sTemp.Split('~')[11]);
            }
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptDUSoftWinding oReport = new rptDUSoftWinding();
            byte[] abytes = oReport.PrepareReport(oDUSoftWindingList, oCompany, oBusinessUnit, "Soft Winding Report", sDateRange);
            return File(abytes, "application/pdf");
        }

        public void ExportToExcelDUSoftWinding(string sTemp)
        {
            DUSoftWinding oDUSoftWinding = new DUSoftWinding();
            string _sErrorMesage;
            int nBUID=0;
            try
            {
                _sErrorMesage = "";
                _oDUSoftWindings = new List<DUSoftWinding>();
                string sSQL = GetSQL(sTemp);
                _oDUSoftWindings = DUSoftWinding.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                nBUID = Convert.ToInt32(sTemp.Split('~')[11]);
                
                if (_oDUSoftWindings.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oDUSoftWindings = new List<DUSoftWinding>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                #region Print XL

                int nRowIndex = 2, nStartRow = 2, nStartCol = 2, nEndCol = 12, nColumn = 1, nCount = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("DUSoftWinding");
                    sheet.Name = "Soft Winding ";
                    sheet.Column(++nColumn).Width = 10; //SL No
                    sheet.Column(++nColumn).Width = 15; //Recv Date
                    sheet.Column(++nColumn).Width = 15; //Order No
                    sheet.Column(++nColumn).Width = 35; //Buyer
                    sheet.Column(++nColumn).Width = 15; //Lot
                    sheet.Column(++nColumn).Width = 35; //Product
                    sheet.Column(++nColumn).Width = 15; //OrderQty
                    sheet.Column(++nColumn).Width = 15; //RcvQty
                    sheet.Column(++nColumn).Width = 15; //DlvQy
                    sheet.Column(++nColumn).Width = 15; //DlvQy
                    sheet.Column(++nColumn).Width = 15; //NoOfCone
                    sheet.Column(++nColumn).Width = 15; //Status
                    //   nEndCol = 12;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 7, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Soft Winding Report"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol-4]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //cell = sheet.Cells[nRowIndex, nStartCol + 10, nRowIndex, nEndCol]; cell.Merge = true;
                    //cell.Value = ""; cell.Style.Font.Bold = false;
                    //cell.Style.WrapText = true;
                    //cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion


                    #region Report Data

                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region
                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Receive Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Total Receive Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Issue Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Balance Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "No Of Cone"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion

                    #region Data
                    foreach (DUSoftWinding oItem in _oDUSoftWindings)
                    {
                        nCount++;
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ReceiveDateST; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.DyeingOrderNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_Order; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value =oItem.Qty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_RSOut; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Balance; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BagNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.StatusST; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 5]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double nValue = _oDUSoftWindings.Select(c => c.Qty_Order).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 4]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = _oDUSoftWindings.Select(c => c.Qty).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 3]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = _oDUSoftWindings.Select(c => c.Qty_RSOut).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = _oDUSoftWindings.Select(c => c.Balance).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol- 0]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=DUSoftWinding.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion

        #region SoftWinding Reports [OPEN]
        public ActionResult PrintDUSoftWinding_Open(string sTemp)
        {
            List<DUSoftWinding> oDUSoftWindingList = new List<DUSoftWinding>();
            DUSoftWinding oDUSoftWinding = new DUSoftWinding();

            int nBUID; string sDateRange = "";
            if (string.IsNullOrEmpty(sTemp))
            {
                throw new Exception("Nothing To Print.");
            }
            else
            {
                string sSQL = GetSQL_Open(sTemp);
                oDUSoftWindingList = DUSoftWinding.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                int nRcvDateCompare = Convert.ToInt32(sTemp.Split('~')[0]);
                DateTime dtRcvDateStart = Convert.ToDateTime(sTemp.Split('~')[1]);
                DateTime dtRcvEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

                if (nRcvDateCompare == (int)EnumCompareOperator.EqualTo)
                    sDateRange = "Receive Date: " + dtRcvDateStart.ToString("dd MMM yyyy");
                if (nRcvDateCompare == (int)EnumCompareOperator.Between)
                    sDateRange = "Receive Date: " + dtRcvDateStart.ToString("dd MMM yyyy") + " To " + dtRcvEndDate.ToString("dd MMM yyyy");

                nBUID = Convert.ToInt32(sTemp.Split('~')[7]);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptDUSoftWinding oReport = new rptDUSoftWinding();
            byte[] abytes = oReport.PrepareReport_Open(oDUSoftWindingList, oCompany, oBusinessUnit, "Soft Winding Report", sDateRange);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintDUSoftWinding_Leadger(string sTemp)
        {
          
            DUSoftWinding oDUSoftWinding = new DUSoftWinding();
            List<InventoryTraking> oInventoryTrakings = new List<InventoryTraking>();
            int nBUID; string sDateRange = "";
            if (string.IsNullOrEmpty(sTemp))
            {
                throw new Exception("Nothing To Print.");
            }
            else
            {
                oDUSoftWinding = oDUSoftWinding.Get(3, ((User)Session[SessionInfo.CurrentUser]).UserID);

               
                int nWorkingUnit = 0;
                int nRcvDateCompare = Convert.ToInt32(sTemp.Split('~')[0]);
                DateTime dtRcvDateStart = Convert.ToDateTime(sTemp.Split('~')[1]);
                DateTime dtRcvEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
                nBUID = Convert.ToInt32(sTemp.Split('~')[8]);

                nWorkingUnit = oDUSoftWinding.WorkingUnitID;

                string sSql= "SELECT [Datetime] , WorkingUnitID,  ProductID, LotID, TriggerParentType,  TriggerParentID,		InOutType,	PreviousBalance,	CASE WHEN InOutType=101 THEN Qty ELSE 0 END In_Qty, CASE WHEN InOutType=102 THEN Qty ELSE 0 END Out_Qty,	CurrentBalance, DBUserID FROM ITransaction";
                string sReturn = "";
                #region BUID
                if (nBUID != null && nBUID != 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LotID IN(SELECT LotID FROM Lot WHERE BUID =" + nBUID + ")";
                }
                #endregion

                #region Date Time
                if (nRcvDateCompare == (int)EnumCompareOperatorTwo.EqualTo)
                {
                    DateObject.CompareDateQuery(ref sReturn, "[Datetime]", nRcvDateCompare, dtRcvDateStart, dtRcvDateStart);
                }
                else
                    if (nRcvDateCompare == (int)EnumCompareOperatorTwo.Between)
                    {
                        DateObject.CompareDateQuery(ref sReturn, "[Datetime]", nRcvDateCompare, dtRcvDateStart, dtRcvEndDate);
                    }
                #endregion

                #region _sWorkingUnit
                if (nWorkingUnit>0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WorkingUnitID =" + nWorkingUnit + "";
                }
                #endregion
                sSql = sSql + "" + sReturn;
                oInventoryTrakings = InventoryTraking.Gets_ITransactions(dtRcvDateStart, dtRcvEndDate, sSql, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (nRcvDateCompare == (int)EnumCompareOperator.EqualTo)
                    sDateRange = "Date: " + dtRcvDateStart.ToString("dd MMM yyyy");
                if (nRcvDateCompare == (int)EnumCompareOperator.Between)
                    sDateRange = "Date: " + dtRcvDateStart.ToString("dd MMM yyyy") + " To " + dtRcvEndDate.ToString("dd MMM yyyy");

              
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptDUSoftWinding oReport = new rptDUSoftWinding();
            byte[] abytes = oReport.PrepareReport_Leadger(oInventoryTrakings, oCompany, oBusinessUnit, "Soft Winding Report", sDateRange);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintDUSoftWinding_Stock(string sTemp)
        {
            DUSoftWinding oDUSoftWinding = new DUSoftWinding();
            List<InventoryTraking> oInventoryTrakings = new List<InventoryTraking>();
            List<Lot> oLots = new List<Lot>();
            int nBUID; string sDateRange = "";
            if (string.IsNullOrEmpty(sTemp))
            {
                throw new Exception("Nothing To Print.");
            }
            else
            {
                oDUSoftWinding = oDUSoftWinding.Get(3, ((User)Session[SessionInfo.CurrentUser]).UserID);

                int nWorkingUnit = 0;
                nBUID = Convert.ToInt32(sTemp.Split('~')[0]);

                nWorkingUnit = oDUSoftWinding.WorkingUnitID;

                string sSql = "Select * from view_Lot where Balance>0.3 ";
                string sReturn = " ";
                #region BUID
                if (nBUID != null && nBUID != 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "BUID =" + nBUID + "";
                }
                #endregion
                #region _sWorkingUnit
                if (nWorkingUnit > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WorkingUnitID =" + nWorkingUnit + "";
                }
                #endregion
                sSql = sSql + "" + sReturn;
                oLots = Lot.Gets( sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oLots.Count > 0)
                {
                    sSql = "SELECT [Datetime] , WorkingUnitID,  ProductID, LotID, TriggerParentType,  TriggerParentID,		InOutType,	PreviousBalance,	CASE WHEN InOutType=101 THEN Qty ELSE 0 END In_Qty, CASE WHEN InOutType=102 THEN Qty ELSE 0 END Out_Qty,	CurrentBalance, DBUserID FROM ITransaction where  WorkingUnitID =" + nWorkingUnit + " and lotID in (" + string.Join(",", oLots.Select(x => x.LotID).Distinct().ToList()) + ")";
                    oInventoryTrakings = InventoryTraking.Gets_ITransactions(DateTime.Now, DateTime.Now, sSql, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                 sDateRange = "Date: " + DateTime.Now.ToString("dd MMM yyyy");
              

            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptDUSoftWinding oReport = new rptDUSoftWinding();
            byte[] abytes = oReport.PrepareReport_Stock(oLots,oInventoryTrakings, oCompany, oBusinessUnit, "Soft Winding Report", sDateRange);
            return File(abytes, "application/pdf");
        }

        public void ExportToExcelDUSoftWinding_Open(string sTemp)
        {
            DUSoftWinding oDUSoftWinding = new DUSoftWinding();
            string _sErrorMesage;
            int nBUID = 0;
            try
            {
                _sErrorMesage = "";
                _oDUSoftWindings = new List<DUSoftWinding>();
                string sSQL = GetSQL_Open(sTemp);
                _oDUSoftWindings = DUSoftWinding.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                nBUID = Convert.ToInt32(sTemp.Split('~')[7]);

                if (_oDUSoftWindings.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oDUSoftWindings = new List<DUSoftWinding>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                #region Print XL

                int nRowIndex = 2, nStartRow = 2, nStartCol = 2, nEndCol = 10, nColumn = 1, nCount = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("DUSoftWinding");
                    sheet.Name = "Soft Winding ";
                    sheet.Column(++nColumn).Width = 10; //SL No
                    sheet.Column(++nColumn).Width = 15; //Recv Date
                    //sheet.Column(++nColumn).Width = 15; //Order No
                    //sheet.Column(++nColumn).Width = 35; //Buyer
                    sheet.Column(++nColumn).Width = 15; //Lot
                    sheet.Column(++nColumn).Width = 35; //Product
                    sheet.Column(++nColumn).Width = 15; //OrderQty
                    //sheet.Column(++nColumn).Width = 15; //RcvQty
                    sheet.Column(++nColumn).Width = 15; //DlvQy
                    sheet.Column(++nColumn).Width = 15; //DlvQy
                    sheet.Column(++nColumn).Width = 15; //NoOfCone
                    sheet.Column(++nColumn).Width = 15; //Status
                    //   nEndCol = 12;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 7, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Soft Winding Report"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //cell = sheet.Cells[nRowIndex, nStartCol + 10, nRowIndex, nEndCol]; cell.Merge = true;
                    //cell.Value = ""; cell.Style.Font.Bold = false;
                    //cell.Style.WrapText = true;
                    //cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion


                    #region Report Data

                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region
                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Receive Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Total Receive Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Issue Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Balance Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "No Of Cone"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion

                    #region Data
                    int nPreviousLotID = -99;
                    double nTotal_Qty_RSOut=0, nTotal_Balance = 0;
                    foreach (DUSoftWinding oItem in _oDUSoftWindings.OrderBy(x=>x.LotID))
                    {
                        nCount++;
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ReceiveDateST; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (nPreviousLotID != oItem.LotID)
                        {
                            int nRowSpan = _oDUSoftWindings.Where(x => x.LotID == oItem.LotID).Count() - 1 ;

                            var nQty_RSOut = _oDUSoftWindings.Where(x => x.LotID == oItem.LotID).Sum(x=>x.Qty_RSOut);
                            var nBalance = _oDUSoftWindings.Where(x => x.LotID == oItem.LotID).Sum(x=>x.Balance);

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Merge = true;
                            cell.Value = nQty_RSOut; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "# #,##0.00";
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Merge = true;
                            cell.Value = nBalance; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "# #,##0.00";
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nTotal_Qty_RSOut += nQty_RSOut;
                            nTotal_Balance +=nBalance;
                        }

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BagNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.StatusST; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        
                        nRowIndex++;
                        nPreviousLotID = oItem.LotID;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 5]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //double nValue = _oDUSoftWindings.Select(c => c.Qty_Order).Sum();
                    //cell = sheet.Cells[nRowIndex, nEndCol - 4]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    //cell.Style.Numberformat.Format = "# #,##0.00";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double nValue = _oDUSoftWindings.Select(c => c.Qty).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 4]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue =nTotal_Qty_RSOut;
                    cell = sheet.Cells[nRowIndex, nEndCol - 3]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = nTotal_Balance;
                    cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=DUSoftWinding.xlsx");
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

