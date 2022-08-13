using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using ESimSol.BusinessObjects;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class DUPScheduleController : PdfViewController
    {
        #region Declartion

        DUPSchedule _oDUPSchedule = new DUPSchedule();
        List<DUPSchedule> _oDUPSchedules = new List<DUPSchedule>();
        List<DUPScheduleDetail> _oDUPScheduleDetails = new List<DUPScheduleDetail>();
        string _sSQL = "";
        string sDateRange = "";

        #endregion

        #region Actions
        public ActionResult ViewDUPSchedule(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
              List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
             oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DUPSchedule).ToString() + "," + ((int)EnumModuleName.RouteSheet).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, oARMs);

            string sLocationIDs = "";
         
            int nCount = 0;
          
            List<Location> oLocations = new List<Location>();
            List<Machine> oMachines = new List<Machine>();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
         
            _sSQL = "SELECT * FROM View_Location where LocationID in (Select LocationID from WorkingUnit where BUID=" + buid + ")";
            //_sSQL = "SELECT * FROM Location where LocationID in(SELECT Distinct LocationID FROM VIEW_CapitalResource where ISNULL(IsActive,1)=1 AND BUID = " + buid + " AND ResourcesType = " + (int)EnumResourcesType.Machine + " )";
            oLocations = Location.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oMachines = Machine.GetsByModule(buid,((int)EnumModuleName.DUPSchedule).ToString(),  ((User)Session[SessionInfo.CurrentUser]).UserID);
            sLocationIDs = Location.IDInString(oLocations);

            //DateTime today = DateTime.Today;
            //DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
            //DateTime endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            DateTime startOfMonth = DateTime.Today;
            //startOfMonth = startOfMonth.AddHours(oRouteSheetSetup.BatchTime);
            DateTime endOfMonth = startOfMonth.AddDays(1);
            _sSQL = "Select * from View_DUPSchedule where LocationID in (" + sLocationIDs + ") and ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')) order by MachineID,StartTime ";
            _oDUPSchedule.DUPSchedules = DUPSchedule.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oDUPSchedule.DUPSchedules.Count > 0)
            {
                _sSQL = "Select * from View_DUPScheduleDetail Where DUPScheduleID IN (Select DUPScheduleID from View_DUPSchedule where LocationID in(" + sLocationIDs + ") and ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') ) )";  // RSState=7  UnloadFromDyemachine
                _oDUPSchedule.DUPScheduleDetails = DUPScheduleDetail.GetsSqL(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            foreach (Machine oItem in oMachines)
            {
                nCount = (from oPS in _oDUPSchedule.DUPSchedules where oPS.MachineID == oItem.MachineID && oPS.BUID == oItem.BUID select oPS).Count();
                oItem.MachineNoWithCapacityAndTotalSchedule = "[" + oItem.Code + "]" + oItem.Name + "[" + ((oItem.Capacity > 0) ? oItem.Capacity.ToString() : "") + "" + (!string.IsNullOrEmpty(oItem.Capacity2) ? oItem.Capacity2.ToString() : "") + "]" + " (" + Convert.ToInt32(nCount) + ")";
            }
            _sSQL = string.Join(",", _oDUPSchedule.DUPSchedules.Select(x => x.DUPScheduleID).Distinct().ToList());
            //if (!string.IsNullOrEmpty(_sSQL))
            //{
            //    _sSQL = "SELECT * FROM View_DUPSLot where DUPScheduleID in (" + _sSQL + ")";
            //    oDUPSLots = DUPSLot.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //}

            foreach (DUPSchedule oItem in _oDUPSchedule.DUPSchedules)
            {
                oItem.DUPScheduleDetails = new List<DUPScheduleDetail>();
                oItem.DUPScheduleDetails = _oDUPSchedule.DUPScheduleDetails.Where(x => x.DUPScheduleID == oItem.DUPScheduleID).ToList();
                sDateRange = oItem.StartTime.ToString("dd MMM yy")+"(" +oItem.StartTime.ToString("HH:mm tt") + "-" + oItem.EndTime.ToString("HH:mm tt")+")";
                nCount = 0;
                if (oItem.DUPScheduleDetails.Count == 0 || oItem.DUPScheduleDetails == null) { oItem.OrderInfo = oItem.ScheduleNo + " " + sDateRange; }

                else
                {
                  //  oItem.LotNo = string.Join("+", oDUPSLots.Where(x => x.DUPScheduleID == oItem.DUPScheduleID).Select(x => x.LotNo+" ("+ Global.MillionFormat(x.Qty)+")").Distinct().ToList());
                    oItem.RSStatus = (int)oItem.DUPScheduleDetails.FirstOrDefault().RSState;
                    if (oRouteSheetSetup.IsShowBuyer)
                    {
                        oItem.OrderInfo = sDateRange + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.BuyerName).Distinct().ToList());
                    }
                    else
                    {
                        oItem.OrderInfo = sDateRange + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ContractorName).Distinct().ToList());
                    }
                    oItem.OrderInfo = oItem.OrderInfo + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.OrderNo).Distinct().ToList());
                    oItem.OrderInfo = oItem.OrderInfo + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ProductName).Distinct().ToList());
                    if (oItem.DUPScheduleDetails[0].IsInHouse==false)
                    { oItem.OrderInfo = oItem.OrderInfo + "</br> Lot: " +  string.Join("+", oItem.DUPScheduleDetails.Select(x => x.BuyerRef).ToList()); }
                    oItem.OrderInfo = oItem.OrderInfo + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ColorName + (string.IsNullOrEmpty(x.PantonNo) ? "" : "(" + x.PantonNo + ")")).Distinct().ToList());
                    oItem.OrderInfo = oItem.OrderInfo + "</br> Color No:" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ColorNo).Distinct().ToList());

                    DUPScheduleDetail oSelectedItem = oItem.DUPScheduleDetails.FirstOrDefault();
                    //oItem.OrderInfo = oItem.OrderInfo + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => Global.MillionFormat(x.Qty) + " " + oRouteSheetSetup.MUnit + ((x.BagCount <= 0) ? "" : " (" + x.BagCount.ToString() + " " + x.HankorConeST + ")")).ToList());
                    oItem.OrderInfo = oItem.OrderInfo + "</br>" +  Global.MillionFormat(oItem.DUPScheduleDetails.Sum(x =>x.Qty)) + " " + oRouteSheetSetup.MUnit + ((oSelectedItem.BagCount <= 0) ? "" : " (" + oSelectedItem.BagCount.ToString() + " " + oSelectedItem.HankorConeST + ")");
                    if (!string.IsNullOrEmpty(oRouteSheetSetup.BatchCode))
                    {
                        oItem.OrderInfo = oItem.OrderInfo + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => oRouteSheetSetup.BatchCode + "-" + x.PSBatchNo).ToList());
                    }
                    //oRouteSheetSetup.BatchCode + "-" + oItem2.PSBatchNo
                    if (!String.IsNullOrEmpty(oItem.Note))
                    {
                        oItem.OrderInfo = oItem.OrderInfo + "</br>" + oItem.Note;
                    }
                    if (!String.IsNullOrEmpty(oItem.DUPScheduleDetails[0].RouteSheetNo))
                    {
                        oItem.OrderInfo = oItem.OrderInfo + "</br>Batch No: " + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.RouteSheetNo).Distinct().ToList());
                    }

                }

            }

            #region Issue Stores
            List<WorkingUnit> oStores = new List<WorkingUnit>();
            oStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.DUPSchedule, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

            #region MachineType
            List<MachineType> oMachineTypes = new List<MachineType>();

            string sSQL = "SELECT * FROM MachineType WHERE BUID=" + buid + " AND MachineTypeID IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID =" + (int)EnumModuleName.DUPSchedule + ")";

            oMachineTypes = MachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            ViewBag.WorkingUnits = oStores;
            ViewBag.MachineTypes = oMachineTypes;
            ViewBag.OrderType = EnumObject.jGets(typeof(EnumOrderType));
            ViewBag.ScheduleStatuses = EnumObject.jGets(typeof(EnumProductionScheduleStatus));
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUOrderSetups = oDUOrderSetups;
            ViewBag.DUPSchedules = _oDUPSchedule.DUPSchedules;
            ViewBag.CapitalResources = oMachines;
            ViewBag.Locations = oLocations;

            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            ViewBag.Buid = buid;
            ViewBag.BusinessUnits = oBusinessUnits;            
            return View(_oDUPSchedule);
        }
     
        [HttpPost]
        public JsonResult Refresh(DUPScheduleDetail oDUPScheduleDetail)
        {

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            DateTime startOfMonth = oDUPScheduleDetail.StartTime;
            DateTime endOfMonth = startOfMonth.AddDays(1);
            
            List<DUPScheduleDetail> oDUPScheduleDetails = new List<DUPScheduleDetail>();
            string sSql = "select * from View_DUPScheduleDetail WHERE ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "'))";
            oDUPScheduleDetails = DUPScheduleDetail.GetsSqL(sSql, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = sjson = serializer.Serialize(oDUPScheduleDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPS(DUPSchedule oDUPSchedule)
        {
            _sSQL = "";
             Machine oMachine = new Machine();
             _oDUPSchedule = new DUPSchedule();
            _oDUPSchedule.DUPScheduleDetails=new List<DUPScheduleDetail>();
            if(oDUPSchedule.DUPScheduleID>0)
            {
              _oDUPSchedule = _oDUPSchedule.Get(oDUPSchedule.DUPScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
               _oDUPSchedule.DUPScheduleDetails = DUPScheduleDetail.Gets(oDUPSchedule.DUPScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
               _oDUPSchedule.DUPSLots = DUPSLot.GetsBy(_oDUPSchedule.DUPScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                 List<DUPSchedule> oPSs = new List<DUPSchedule>();
                 oMachine = oMachine.Get(oDUPSchedule.MachineID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 _sSQL = "Select * from View_DUPSchedule Where MachineID=" + oDUPSchedule.MachineID + " and " + " EndTime = (Select MAX(EndTime) from DUPSchedule Where MachineID=" + oDUPSchedule.MachineID + ")";
                 oPSs = DUPSchedule.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
                if (oPSs.Count > 0)
                {
                    _oDUPSchedule.StartTime=oPSs[0].EndTime.AddMinutes(1);
                }
                _oDUPSchedule.LocationID = oMachine.LocationID;
                _oDUPSchedule.MachineID = oMachine.MachineID;
                _oDUPSchedule.MachineName = oMachine.Name;
                _oDUPSchedule.MachineNo = oMachine.Code;
                _oDUPSchedule.Capacity = oMachine.Capacity;
                _oDUPSchedule.Capacity2 = oMachine.Capacity2;
                _oDUPSchedule.BUID = oMachine.BUID;
            }
            _sSQL = "SELECT * FROM View_Location where LocationID in (Select LocationID from WorkingUnit where BUID=" + oDUPSchedule.BUID + ")";
            _oDUPSchedule.Locations = Location.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oDUPSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsDyeingOrder(DUPScheduleDetail oPSDetail)// For Delivery Order
        {
            string sSQL = "";
            List<DUPScheduleDetail> oDUPScheduleDetails = new List<DUPScheduleDetail>();
          
            List<RouteSheetDO> oRouteSheetDO = new List<RouteSheetDO>();
            DUPScheduleDetail oPSD = new DUPScheduleDetail();
            try
            {
                    sSQL = "Select top(100)* from View_DyeingOrderDetailWaitingForRS ";
                    string sReturn = "";
                    if (!String.IsNullOrEmpty(oPSDetail.OrderNo))
                    {
                        oPSDetail.OrderNo = oPSDetail.OrderNo.Trim();
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "OrderNo Like'%" + oPSDetail.OrderNo + "%'";
                    }
                    if (!String.IsNullOrEmpty(oPSDetail.RouteSheetNo))
                    {
                        oPSDetail.RouteSheetNo = oPSDetail.RouteSheetNo.Trim();
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "RouteSheetNo Like'%" + oPSDetail.RouteSheetNo + "%'";
                    }
                    if (oPSDetail.OrderType > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "OrderType=" + oPSDetail.OrderType + "";
                    }
                    if (oPSDetail.LabDipDetailID > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "LabDipDetailID=" + oPSDetail.LabDipDetailID + "";
                    }
                    if (oPSDetail.DyeingOrderID > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "DyeingOrderID=" + oPSDetail.DyeingOrderID + "";
                    }
                    //if (oPSDetail.DODID > 0)
                    //{
                    //    Global.TagSQL(ref sReturn);
                    //    sReturn = sReturn + "DyeingOrderDetailID=" + oPSDetail.DODID + "";
                    //}
                    if (!String.IsNullOrEmpty(oPSDetail.ContractorName))
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "ContractorID in (" + oPSDetail.ContractorName + ")";
                    }

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "[Status] in (0,1,2,3,4,5,6,7,8)" + " and isnull(DyeingStepType,0) in (" + (int)EnumDyeingStepType.None + "," + (int)EnumDyeingStepType.Knitting_CK + "," + (int)EnumDyeingStepType.Knitting_CS + "," + (int)EnumDyeingStepType.Dyeing + ")";

                    sSQL = sSQL + "" + sReturn + " Order BY   CONVERT(int,dbo.udf_GetNumeric(OrderNo))  DESC, DyeingOrderID,SL ASC";
                    oRouteSheetDO = RouteSheetDO.GetsDOYetTORS(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach(RouteSheetDO oItem in oRouteSheetDO)
                    {
                        oPSD = new DUPScheduleDetail();
                        oPSD.DODID = oItem.DyeingOrderDetailID;
                        oPSD.LabDipDetailID = oItem.LabDipDetailID;
                        //oPSD.PTUID = oItem.PTUID;
                        oPSD.OrderNo = oItem.OrderNoFull;
                        //oPSD.OrderType = oItem.OrderTypeInt;
                        oPSD.OrderQty = oItem.OrderQty;
                        oPSD.Qty_Pro = oItem.Qty_Pro;
                        oPSD.Qty_PSD = oItem.Qty_PSD;
                        oPSD.OrderTypest = oItem.OrderTypeSt;
                        oPSD.ColorName = oItem.ColorName;
                        oPSD.SLNo = oItem.SLNo;
                        oPSD.ColorNo = oItem.ColorNo;
                        oPSD.ProductID = oItem.ProductID;
                        oPSD.ProductName = oItem.ProductName;
                        oPSD.ContractorName = oItem.ContractorName;
                        oPSD.BuyerName = oItem.DeliveryToName;
                        oPSD.HankorCone = oItem.HankorCone;
                        if ((oItem.OrderQty - oItem.Qty_PSD)>0)
                        {
                            oPSD.Qty = oItem.OrderQty - oItem.Qty_PSD;
                        }
                        oPSD.ContractorID = oItem.ContractorID;
                        oDUPScheduleDetails.Add(oPSD);
                    }
               

            }
            catch (Exception ex)
            {
                oDUPScheduleDetails = new List<DUPScheduleDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUPScheduleDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsMachine(Machine oMachine)// For Delivery Order
        {
            string sSQL = "";
            List<Machine> oMachines = new List<Machine>();
            try
            {
                sSQL = "Select top(500)* from View_Machine";
                string sReturn = "";
                if (!String.IsNullOrEmpty(oMachine.Name))
                {
                    oMachine.Name = oMachine.Name.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "Name+isnull(Code,'') Like'%" + oMachine.Name + "%'";
                }
                if (oMachine.LocationID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "LocationID=" + oMachine.LocationID + "";
                }
                if (oMachine.BUID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "BUID=" + oMachine.BUID + "";
                }
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "MachineTypeID IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID IN ( " + ((int)EnumModuleName.DUPSchedule).ToString() + ","+ ((int)EnumModuleName.RouteSheet).ToString() +"  )) and Activity=1";

                sSQL = sSQL + "" + sReturn + " order by SequenceNo,Name";
               
                oMachines = Machine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oMachines = new List<Machine>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchMachineType(Machine oMachine)
        {
            Machine objMachine = new Machine();
            List<Machine> oMachines = new List<Machine>();

            string sSQL="SELECT * FROM View_Machine WHERE MachineID <> 0 ";

            if (oMachine.MachineTypeID > 0)
            {
                sSQL += " AND MachineTypeID = " + oMachine.MachineTypeID;
            }
            else
            {
                sSQL += " AND MachineTypeID in( SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID =" + (int)EnumModuleName.DUPSchedule + ")";
            }
            if (!string.IsNullOrEmpty(oMachine.Name))
                sSQL += " AND Name+isnull(Code,'')  LIKE '%" + oMachine.Name + "%' ";
            if (oMachine.BUID>0)
                sSQL += " AND BUID = " + oMachine.BUID;
            sSQL += " AND Activity=1 ORDER BY SequenceNo";
            oMachines = Machine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPSByMachine(int nMachineID, int nLocationID, double nts)
        {
            string sSQL = "Select top(1)* from View_DUPSchedule Where MachineID=" + nMachineID + " and LocationID=" + nLocationID + " Order by StartTime  DESC";

            List<DUPSchedule> oPSs = new List<DUPSchedule>();
            DUPSchedule oPS = new DUPSchedule();
            string sStartTime = "";
            try
            {
                oPSs = DUPSchedule.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPSs.Count > 0)
                {
                    sStartTime = oPSs[0].EndTime.AddMinutes(1).ToString("MM'/'dd'/'yyyy HH:mm");
                }
            }
            catch (Exception ex)
            {
                sStartTime = "";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sStartTime);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Number of Schedules In Given Time Period.
        [HttpPost]
        public JsonResult ScheduleInTimePeriod(DUPSchedule oDUPSchedule, double nts)
        {

            int nScheduleCount = NumberofScheduleInTimePeriod(oDUPSchedule);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(nScheduleCount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public int NumberofScheduleInTimePeriod(DUPSchedule oDUPSchedule)
        {
            int nScheduleCount = 0;
            DateTime dStartTime = oDUPSchedule.StartTime;
            DateTime dEndTime = oDUPSchedule.EndTime.AddMinutes(-1);
            int nLocationID = oDUPSchedule.LocationID;
            int nMachineID = oDUPSchedule.MachineID;
            string sSQL = "Select * from View_DUPSchedule where EndTime >= '" + dStartTime.ToString("dd MMM yyyy HH:mm") + "' and StartTime <='" + dEndTime.ToString("dd MMM yyyy HH:mm") + "' and LocationID=" + nLocationID + " and MachineID=" + nMachineID + " and DUPScheduleID NOT IN (" + oDUPSchedule.DUPScheduleID + ")";
            if (nLocationID > 0 && nMachineID > 0)
            {
                List<DUPSchedule> oPS = new List<DUPSchedule>();
                oPS = DUPSchedule.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oPS.Count() > 0)
                {
                    if (oDUPSchedule.StartTime <= oPS.Min(x => x.EndTime))
                    {
                        nScheduleCount = oPS.Count();
                    }
                }

            }
            return nScheduleCount;
        }
        #endregion

        [HttpPost]
        public JsonResult Save(DUPSchedule oDUPSchedule)
        {
            try
            {
                _oDUPSchedule = oDUPSchedule.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDUPSchedule.DUPScheduleDetails = DUPScheduleDetail.Gets(_oDUPSchedule.DUPScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDUPSchedule.DUPSLots = DUPSLot.GetsBy(_oDUPSchedule.DUPScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sDateRange = _oDUPSchedule.StartTime.ToString("HH:mm tt") + "-" + _oDUPSchedule.EndTime.ToString("HH:mm tt");

                 RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
                 oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
         
                if ( _oDUPSchedule.DUPScheduleDetails == null || _oDUPSchedule.DUPScheduleDetails.Count == 0 ) 
                { 
                    _oDUPSchedule.OrderInfo = _oDUPSchedule.ScheduleNo + " " + sDateRange; 
                }
                else
                {
                    if (oRouteSheetSetup.IsShowBuyer)
                    {
                        _oDUPSchedule.OrderInfo = sDateRange + "</br>" + string.Join("+", _oDUPSchedule.DUPScheduleDetails.Select(x => x.BuyerName).Distinct().ToList());
                    }
                    else
                    {
                        _oDUPSchedule.OrderInfo = sDateRange + "</br>" + string.Join("+", _oDUPSchedule.DUPScheduleDetails.Select(x => x.ContractorName).Distinct().ToList());
                    }
                  
                    _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br>" + string.Join("+", _oDUPSchedule.DUPScheduleDetails.Select(x => x.OrderNo).Distinct().ToList());
                    _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br>" + string.Join("+", _oDUPSchedule.DUPScheduleDetails.Select(x => x.ProductName).Distinct().ToList());
                    if (!string.IsNullOrEmpty(_oDUPSchedule.LotNo))
                    { _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br> Lot: " + _oDUPSchedule.LotNo; }// string.Join("+", oItem.DUPScheduleDetails.Select(x => x.LotNo).ToList()); }
                    _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br>" + string.Join("+", _oDUPSchedule.DUPScheduleDetails.Select(x => x.ColorName).Distinct().ToList());
                    _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br> Color No:" + string.Join("+", _oDUPSchedule.DUPScheduleDetails.Select(x => x.ColorNo).Distinct().ToList());
                     //_oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br>" + string.Join("+", _oDUPSchedule.DUPScheduleDetails.Select(x => Global.MillionFormat(x.Qty) + " " + oRouteSheetSetup.MUnit + ((x.BagCount <= 0) ? "" : " (" + x.BagCount.ToString() + " " + x.HankorConeST + ")")).ToList());

                    DUPScheduleDetail oSelectedItem = _oDUPSchedule.DUPScheduleDetails.FirstOrDefault();
                    //oItem.OrderInfo = oItem.OrderInfo + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => Global.MillionFormat(x.Qty) + " " + oRouteSheetSetup.MUnit + ((x.BagCount <= 0) ? "" : " (" + x.BagCount.ToString() + " " + x.HankorConeST + ")")).ToList());
                    _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br>" + Global.MillionFormat(_oDUPSchedule.DUPScheduleDetails.Sum(x => x.Qty)) + " " + oRouteSheetSetup.MUnit + ((oSelectedItem.BagCount <= 0) ? "" : " (" + oSelectedItem.BagCount.ToString() + " " + oSelectedItem.HankorConeST + ")");
                   
                    
                    if (!string.IsNullOrEmpty(oRouteSheetSetup.BatchCode))
                    {
                        _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br>" + string.Join("+", _oDUPSchedule.DUPScheduleDetails.Select(x => oRouteSheetSetup.BatchCode + "-" + x.PSBatchNo).ToList());
                    }
                    //oRouteSheetSetup.BatchCode + "-" + oItem2.PSBatchNo
                    if (!String.IsNullOrEmpty(_oDUPSchedule.Note))
                    {
                        _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br>" + _oDUPSchedule.Note;
                    }
                    if (!String.IsNullOrEmpty(_oDUPSchedule.DUPScheduleDetails[0].RouteSheetNo))
                    {
                        _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br>Batch No: " + string.Join("+", _oDUPSchedule.DUPScheduleDetails.Select(x => x.RouteSheetNo).Distinct().ToList());
                    }

                }
                //else foreach (DUPScheduleDetail oItem2 in _oDUPSchedule.DUPScheduleDetails)
                //    {
                //        _oDUPSchedule.OrderInfo = "" + sDateRange + " </br> " + oItem2.ContractorName + "" + "</br>" + oItem2.OrderNo
                //                                    + "</br>" + oItem2.ProductName + (string.IsNullOrEmpty(_oDUPSchedule.LotNo) ? "" : "</br> Lot:" + _oDUPSchedule.LotNo) + "</br>" + oItem2.ColorName + "</br>" + oItem2.Qty + " " + oRouteSheetSetup.MUnit + " " + "</br>" + oRouteSheetSetup.BatchCode + "-" + oItem2.PSBatchNo + ((string.IsNullOrEmpty(_oDUPSchedule.Note)) ? "" : "</br>" + _oDUPSchedule.Note);

                //    }
               
            }
            catch (Exception ex)
            {
                _oDUPSchedule = new DUPSchedule();
                _oDUPSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUPSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveRS(DUPSchedule oDUPSchedule)
        {
            RouteSheet oRouteSheet = new RouteSheet();
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            try
            {
                oRouteSheet = oRouteSheet.GetByPS(oDUPSchedule.DUPScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oRouteSheet.RouteSheetID > 0)
                {
                    oRouteSheetDOs = RouteSheetDO.GetsBy(oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDUPSchedule.DUPScheduleDetails.ForEach(x =>
                    {
                        if (oRouteSheetDOs.FirstOrDefault() != null && oRouteSheetDOs.FirstOrDefault().DyeingOrderDetailID > 0 && oRouteSheetDOs.Where(b => b.DyeingOrderDetailID == x.DODID).Count() > 0)
                        {
                            x.RouteSheetDOID = oRouteSheetDOs.Where(p => p.DyeingOrderDetailID == x.DODID && p.DyeingOrderDetailID > 0).FirstOrDefault().RouteSheetDOID;
                        }
                    });
                }

                oDUPSchedule.RouteSheetID = oRouteSheet.RouteSheetID;
                oDUPSchedule.RouteSheetNo = oRouteSheet.RouteSheetNo;
                _oDUPSchedule = oDUPSchedule.SaveRS(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (string.IsNullOrEmpty(_oDUPSchedule.ErrorMessage))
                {
                    _oDUPSchedule = _oDUPSchedule.Get(oDUPSchedule.DUPScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oDUPSchedule.DUPScheduleDetails = DUPScheduleDetail.Gets(oDUPSchedule.DUPScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    sDateRange = _oDUPSchedule.StartTime.ToString("HH:mm tt") + "-" + _oDUPSchedule.EndTime.ToString("HH:mm tt");
                    if (_oDUPSchedule.DUPScheduleDetails.Count == 0 || _oDUPSchedule.DUPScheduleDetails == null) { _oDUPSchedule.OrderInfo = _oDUPSchedule.ScheduleNo + " " + sDateRange; }
                    foreach (DUPScheduleDetail oItem2 in _oDUPSchedule.DUPScheduleDetails)
                    {
                        //_oDUPSchedule.OrderInfo = "" + sDateRange + " </br> " + oItem2.ContractorName + "" + "</br>" + oItem2.OrderNo
                        //                              + "</br>" + oItem2.ProductName + (string.IsNullOrEmpty(_oDUPSchedule.LotNo) ? "" : "</br> Lot:" + _oDUPSchedule.LotNo) + "</br>" + oItem2.ColorName + "</br>" + oItem2.Qty + " " + oRouteSheetSetup.MUnit + " " + "</br>" + oRouteSheetSetup.BatchCode + "-" + oItem2.PSBatchNo + ((string.IsNullOrEmpty(_oDUPSchedule.Note)) ? "" : "</br>" + _oDUPSchedule.Note);


                        ////_oDUPSchedule.OrderInfo = "" + sDateRange + " </br> " + oItem2.OrderNo + "" + "</br>" + oItem2.ContractorName;
                        //if (!String.IsNullOrEmpty(oItem2.RouteSheetNo))
                        //{
                        //    _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br> Batch No: " + oItem2.RouteSheetNo;
                        //}

                        if (oRouteSheetSetup.IsShowBuyer)
                        {
                            _oDUPSchedule.OrderInfo = sDateRange + "</br>" + string.Join("+", _oDUPSchedule.DUPScheduleDetails.Select(x => x.BuyerName).Distinct().ToList());
                        }
                        else
                        {
                            _oDUPSchedule.OrderInfo = sDateRange + "</br>" + string.Join("+", _oDUPSchedule.DUPScheduleDetails.Select(x => x.ContractorName).Distinct().ToList());
                        }

                        _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br>" + string.Join("+", _oDUPSchedule.DUPScheduleDetails.Select(x => x.OrderNo).Distinct().ToList());
                        _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br>" + string.Join("+", _oDUPSchedule.DUPScheduleDetails.Select(x => x.ProductName).Distinct().ToList());
                        if (!string.IsNullOrEmpty(_oDUPSchedule.LotNo))
                        { _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br> Lot: " + _oDUPSchedule.LotNo; }// string.Join("+", oItem.DUPScheduleDetails.Select(x => x.LotNo).ToList()); }
                        _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br>" + string.Join("+", _oDUPSchedule.DUPScheduleDetails.Select(x => x.ColorName).Distinct().ToList());
                        _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br> Color No:" + string.Join("+", _oDUPSchedule.DUPScheduleDetails.Select(x => x.ColorNo).Distinct().ToList());
                        _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br>" + string.Join("+", _oDUPSchedule.DUPScheduleDetails.Select(x => Global.MillionFormat(x.Qty) + " " + oRouteSheetSetup.MUnit + ((x.BagCount <= 0) ? "" : " (" + x.BagCount.ToString() + " " + x.HankorConeST + ")")).ToList());
                        if (!string.IsNullOrEmpty(oRouteSheetSetup.BatchCode))
                        {
                            _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br>" + string.Join("+", _oDUPSchedule.DUPScheduleDetails.Select(x => oRouteSheetSetup.BatchCode + "-" + x.PSBatchNo).ToList());
                        }
                        //oRouteSheetSetup.BatchCode + "-" + oItem2.PSBatchNo
                        if (!String.IsNullOrEmpty(_oDUPSchedule.Note))
                        {
                            _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br>" + _oDUPSchedule.Note;
                        }
                        if (!String.IsNullOrEmpty(oItem2.RouteSheetNo))
                        {
                            _oDUPSchedule.OrderInfo = _oDUPSchedule.OrderInfo + "</br> Batch No: " + oItem2.RouteSheetNo;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _oDUPSchedule = new DUPSchedule();
                _oDUPSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUPSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_Requisition(DUPSchedule oDUPSchedule)
        {
            DURequisitionSetup oDURequisitionSetup = new DURequisitionSetup();
            DyeingOrderDetail oDyeingOrderDetail = new DyeingOrderDetail();
            DUPScheduleDetail oDUPScheduleDetail = new DUPScheduleDetail();

            DURequisition oDURequisition = new DURequisition();
            
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<DURequisitionDetail> oDURequisitionDetails = new List<DURequisitionDetail>();

            try
            {
                if (!string.IsNullOrEmpty(oDUPSchedule.Params)) 
                {
                    #region GET SCHEDULE & DETAILS
                    oDURequisitionSetup = oDURequisitionSetup.GetByType((int)EnumInOutType.Receive, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                    _oDUPSchedules = DUPSchedule.Gets("SELECT * FROM DUPSchedule WHERE DUPScheduleID IN (SELECT DUPScheduleID FROM DUPScheduleDetail WHERE DUPScheduleDetailID IN (" + oDUPSchedule.Params + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oDUPScheduleDetails = DUPScheduleDetail.GetsSqL("SELECT * FROM View_DUPScheduleDetail WHERE ISNULL(IsRequistion,0)=0 AND DUPScheduleID IN (" + string.Join(",",_oDUPSchedules.Select(x=>x.DUPScheduleID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    #endregion

                    if (_oDUPScheduleDetails.Any())
                    {
                        string sDODIDs = string.Join(",", _oDUPScheduleDetails.Select(x => x.DODID));
                        oDyeingOrderDetails = DyeingOrderDetail.Gets("SELECT * FROM DyeingOrderDetail WHERE DyeingOrderDetailID IN (" + sDODIDs + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oDUPSLots = DUPSLot.Gets("SELECT * FROM View_DUPSLot  WHERE DUPScheduleID IN (SELECT DUPScheduleID FROM DUPScheduleDetail WHERE DUPScheduleDetailID IN (" + oDUPSchedule.Params + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);
                  
                        #region DU REQUISITION
                        oDURequisition = new DURequisition();
                        oDURequisition.BUID = _oDUPSchedules.Select(x => x.BUID).FirstOrDefault();
                        oDURequisition.IssueDate = DateTime.Now;
                        oDURequisition.OpeartionUnitType = EnumOperationUnitType.SoftWinding;
                        oDURequisition.RequisitionbyID = ((User)Session[SessionInfo.CurrentUser]).UserID;
                        oDURequisition.RequisitionType = EnumInOutType.Receive;
                        oDURequisition.WorkingUnitID_Issue = (_oDUPSchedules.Select(x => x.WorkingUnitID).FirstOrDefault() == 0 ? oDURequisitionSetup.WorkingUnitID_Issue : _oDUPSchedules.Select(x => x.WorkingUnitID).FirstOrDefault());
                        oDURequisition.WorkingUnitID_Receive = oDURequisitionSetup.WorkingUnitID_Receive;
                        #endregion

                        foreach (var oDUPSDetail in _oDUPScheduleDetails)
                        {
                            #region DU REQUISITION DETAILS

                            oDyeingOrderDetail = oDyeingOrderDetails.Where(x => x.DyeingOrderDetailID == oDUPSDetail.DODID).FirstOrDefault();
                            if (_oDUPSLots.Where(x => x.DODID == oDUPSDetail.DODID).Any())
                            {
                                _oDUPSLots.Where(x => x.DODID == oDUPSDetail.DODID).ToList().ForEach(oDUPSLot =>
                                {
                                    oDURequisition.DURequisitionDetails.Add(new DURequisitionDetail()
                                    {
                                        DURequisitionID = 0,
                                        BagNo = 0, //DyeingOrderDetail.BagNo,
                                        //ContractorID = oDyeingOrderDetail.ContractorID,
                                        //CurrencyID = oDyeingOrderDetail.CurrencyID,

                                        LotID = oDUPSLot.LotID,
                                        Qty = oDUPSLot.Qty,
                                        ProductID = oDUPSDetail.ProductID,
                                        DyeingOrderID = oDyeingOrderDetail.DyeingOrderID,
                                    });
                                });
                            }
                            else
                            {
                                oDURequisition.DURequisitionDetails.Add(new DURequisitionDetail()
                                {
                                    DURequisitionID = 0,
                                    BagNo = 0,
                                    //ContractorID = oDyeingOrder.ContractorID,
                                    //CurrencyID = oDyeingOrder.CurrencyID,

                                    LotID = 0,
                                    Qty = oDUPSDetail.Qty,
                                    DyeingOrderID = oDyeingOrderDetail.DyeingOrderID,
                                    ProductID = oDUPSDetail.ProductID,
                                });
                            }
                            #endregion
                        }
                        oDURequisition = oDURequisition.Save(((User)Session[SessionInfo.CurrentUser]).UserID);

                        oDUPScheduleDetail = new DUPScheduleDetail() { Params =  string.Join(",",_oDUPScheduleDetails.Select(x=>x.DUPScheduleDetailID)) };
                        oDUPScheduleDetail.Update_Requisition(oDUPScheduleDetail, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    else throw new Exception("No Schedule Found. Requisition may already be created!!");
                }
            }
            catch (Exception ex)
            {
                oDURequisition = new DURequisition();
                oDURequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDURequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update_Status(DUPSchedule oDUPSchedule)
        {
            try
            {
                if (oDUPSchedule.DUPScheduleID > 0)
                {
                    _oDUPSchedule = oDUPSchedule.Update_Status(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else throw new Exception("Invalid Scedule To Update Status!");
            }
            catch (Exception ex)
            {
                _oDUPSchedule = new DUPSchedule();
                _oDUPSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUPSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(DUPSchedule oDUPSchedule)
        {
            try
            {
                if (oDUPSchedule.DUPScheduleID <= 0) { throw new Exception("Please select an valid item."); }
                oDUPSchedule.ErrorMessage = oDUPSchedule.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDUPSchedule = new DUPSchedule();
                oDUPSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUPSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteDetail(DUPScheduleDetail oDUPScheduleDetail)
        {
            try
            {
                if (oDUPScheduleDetail.DUPScheduleDetailID <= 0) { throw new Exception("Please select an valid item."); }
                oDUPScheduleDetail.ErrorMessage = oDUPScheduleDetail.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDUPScheduleDetail = new DUPScheduleDetail();
                oDUPScheduleDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUPScheduleDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SwapSchedule(string sPSIDs, double nts)
        {
            List<DUPScheduleDetail> oDUPScheduleDetails = new List<DUPScheduleDetail>();
            TimeSpan oTimeSpan = new TimeSpan();
            int nDUpScID=0;
            int nDUpScIDTwo=0;
            string sMessage = "";
            try
            {
                nDUpScID = Convert.ToInt32(sPSIDs.Split('~')[0]);
                nDUpScIDTwo = Convert.ToInt32(sPSIDs.Split('~')[1]);

                if (nDUpScID <= 0) { throw new Exception("Please select an valid item."); }
                if (nDUpScIDTwo <= 0) { throw new Exception("Please select an valid item."); }

                DUPSchedule oDUPSchedule = new DUPSchedule();
                DUPSchedule oDUPScheduleOne = new DUPSchedule();
                DUPSchedule oDUPScheduleTwo = new DUPSchedule();
                oDUPScheduleOne = oDUPScheduleOne.Get(nDUpScID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDUPScheduleTwo = oDUPScheduleTwo.Get(nDUpScIDTwo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                /// Mapping One
                oDUPSchedule.DUPScheduleID = oDUPScheduleOne.DUPScheduleID;
                oDUPSchedule.MachineID = oDUPScheduleTwo.MachineID;
                oDUPSchedule.LocationID = oDUPScheduleTwo.LocationID;
                oDUPScheduleOne.EndTime = oDUPScheduleOne.EndTime.AddMinutes(1);
                oTimeSpan = oDUPScheduleOne.EndTime - oDUPScheduleOne.StartTime;
                oDUPSchedule.StartTime = oDUPScheduleTwo.StartTime;
                oDUPSchedule.EndTime = oDUPScheduleTwo.StartTime + oTimeSpan;
                _oDUPSchedules.Add(oDUPSchedule);

                ///Mapping Two
                oDUPSchedule = new DUPSchedule();
                oDUPSchedule.DUPScheduleID = oDUPScheduleTwo.DUPScheduleID;
                oDUPSchedule.MachineID = oDUPScheduleOne.MachineID;
                oDUPSchedule.LocationID = oDUPScheduleOne.LocationID;
                oDUPScheduleTwo.EndTime = oDUPScheduleTwo.EndTime.AddMinutes(1);
                oTimeSpan = oDUPScheduleTwo.EndTime - oDUPScheduleTwo.StartTime;
                oDUPSchedule.StartTime = oDUPScheduleOne.StartTime;
                oDUPSchedule.EndTime = oDUPScheduleOne.StartTime + oTimeSpan;
                _oDUPSchedules.Add(oDUPSchedule);

                oDUPScheduleDetails = DUPScheduleDetail.Swap(_oDUPSchedules, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                DUPScheduleDetail oDUPScheduleDetail = new DUPScheduleDetail();
                oDUPScheduleDetail.ErrorMessage = ex.Message;
                oDUPScheduleDetails.Add(oDUPScheduleDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUPScheduleDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PastSchedule(DUPSchedule oDUPSchedule)
        {
            List<DUPScheduleDetail> oDUPScheduleDetails = new List<DUPScheduleDetail>();
            TimeSpan oTimeSpan = new TimeSpan();
            int nDUpScID = 0;
            int nDUpScIDTwo = 0;
            string sTime = "";
            string sPSIDs = oDUPSchedule.Params;

            DateTime oDateTime = DateTime.MinValue;
            try
            {
                nDUpScID = Convert.ToInt32(sPSIDs.Split('~')[0]);
                nDUpScIDTwo = Convert.ToInt32(sPSIDs.Split('~')[1]);

                if (nDUpScID <= 0)
                {
                    oDateTime = Convert.ToDateTime(sPSIDs.Split('~')[2]);
                    sTime = Convert.ToString(sPSIDs.Split('~')[3]);
                    oDateTime = new DateTime(oDateTime.Year, oDateTime.Month, oDateTime.Day, int.Parse(sTime.Split(':')[0]),int.Parse(sTime.Split(':')[1]),0);
                    //oDateTime = Convert.ToDateTime(oDateTime.ToString("dd MMM yyyy: " + sTime)); //Convert.ToDateTime(sPSIDs.Split('~')[3]);
                }
                //if (nDUpScID <= 0) { throw new Exception("Please select an valid item."); }
                if (nDUpScIDTwo <= 0) { throw new Exception("Please select an valid item."); }

                //DUPSchedule oDUPSchedule = new DUPSchedule();
                DUPSchedule oDUPScheduleOne = new DUPSchedule();
                oDUPScheduleOne = oDUPSchedule;
                DUPSchedule oDUPScheduleTwo = new DUPSchedule();
                oDUPScheduleOne.EndTime = oDateTime;
                if (nDUpScID > 0)
                {
                    oDUPScheduleOne = oDUPScheduleOne.Get(nDUpScID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                oDUPScheduleTwo = oDUPScheduleTwo.Get(nDUpScIDTwo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                /// Mapping One
              
                ///Mapping Two
                //oDUPSchedule = new DUPSchedule();
                oDUPScheduleTwo.DUPScheduleID = oDUPScheduleTwo.DUPScheduleID;
                oDUPScheduleTwo.MachineID = oDUPScheduleOne.MachineID;
                oDUPScheduleTwo.LocationID = oDUPScheduleOne.LocationID;

                oTimeSpan = oDUPScheduleTwo.EndTime - oDUPScheduleTwo.StartTime;
                oDUPScheduleTwo.StartTime = oDUPScheduleOne.EndTime.AddMinutes(1);
                oDUPScheduleTwo.EndTime = oDUPScheduleTwo.StartTime + oTimeSpan.Duration();
                _oDUPSchedules.Add(oDUPScheduleTwo);

                oDUPScheduleDetails = DUPScheduleDetail.Swap(_oDUPSchedules, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                DUPScheduleDetail oDUPScheduleDetail = new DUPScheduleDetail();
                oDUPScheduleDetail.ErrorMessage = ex.Message;
                oDUPScheduleDetails.Add(oDUPScheduleDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUPScheduleDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPSByDU(DUPScheduleDetail oDUPScheduleDetail)
        {
            _sSQL = "";
            Machine oMachine = new Machine();
            List<DUPSchedule> _oDUPSchedules = new List<DUPSchedule>();
            if (oDUPScheduleDetail.DODID > 0)
            {
                List<DUPSchedule> oPSs = new List<DUPSchedule>();
                _sSQL = "SELECT * FROM DUPSchedule WHERE DUPScheduleID IN (SELECT DUPScheduleID FROM DUPScheduleDetail WHERE DODID =" + oDUPScheduleDetail.DODID + ")";
                _oDUPSchedules = DUPSchedule.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUPSchedules);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPS_Lot(DUPScheduleDetail oDUPScheduleDetail)
        {
            _sSQL = "";
            List<DUPSLot> oDUPSLots = new List<DUPSLot>();
            if (oDUPScheduleDetail.DUPScheduleDetailID > 0)
            {
                List<DUPSchedule> oPSs = new List<DUPSchedule>();
                _sSQL = "SELECT * FROM View_DUPSLot WHERE DUPScheduleDetailID =" + oDUPScheduleDetail.DUPScheduleDetailID;
                oDUPSLots = DUPSLot.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUPSLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Gets Lot
        [HttpPost]
        public JsonResult GetsLotOld(Lot oLot)
        {
            string sLotIDs = "";
            List<Lot> oLots = new List<Lot>();
            List<DUProGuideLineDetail> oDUProGuideLineDetails = new List<DUProGuideLineDetail>();
            List<LotParent> oLotParents = new List<LotParent>();
            List<FabricLotAssign> oFabricLotAssigns = new List<FabricLotAssign>();
            string sSQL = "";
            try
            {
                //If is Open Lot
                //oLot.LotNo = (!string.IsNullOrEmpty(oLot.LotNo)) ? oLot.LotNo.Trim() : "";

                //if (oLot.Params.Contains("NotInHouse"))
                //{
                //    oDUProGuideLineDetails = DUProGuideLineDetail.Gets("SELECT * FROM DUProGuideLineDetail AS HH WHERE HH.DUProGuideLineID IN (SELECT DD.DUProGuideLineID FROM DUProGuideLine AS DD WHERE DD.DyeingOrderID IN (SELECT TT.DyeingOrderID FROM DyeingOrderDetail AS TT WHERE TT.DyeingOrderDetailID =" + oLot.ModelReferenceID + "))", (int)Session[SessionInfo.currentUserID]);
                //    sLotIDs = string.Join(",", oDUProGuideLineDetails.Where(x => x.LotID>0).Select(x => x.LotID).ToList());
                //}
                //string sSQL = "Select * from View_Lot Where  Balance>0 and  LotID<>0 ";

                //if (oLot.Params.Contains("NotInHouse"))
                //{
                //    sSQL = sSQL + " AND ParentType = " + (int)EnumTriggerParentsType.DUProGuideLineDetail + " AND ContractorID =" + oLot.ContractorID;
                //    if (!string.IsNullOrEmpty(sLotIDs)) { sSQL = sSQL + " AND (LotID in ("+sLotIDs +") or ParentLotID in ("+sLotIDs +"))";  }
                //}
                ////+ "  AND ParentID IN (SELECT ISNULL(HH.DUProGuideLineDetailID,0) FROM DUProGuideLineDetail AS HH WHERE HH.DUProGuideLineID IN (SELECT DD.DUProGuideLineID FROM DUProGuideLine AS DD WHERE DD.DyeingOrderID IN (SELECT TT.DyeingOrderID FROM DyeingOrderDetail AS TT WHERE TT.DyeingOrderDetailID =" + oLot.ModelReferenceID + ")))";
                //else if (oLot.Params.Contains("IsInHouse"))
                //    sSQL = sSQL + " AND ParentType != " + (int)EnumTriggerParentsType.DUProGuideLineDetail;
                 
                //if (!string.IsNullOrEmpty(oLot.LotNo))
                //    sSQL = sSQL + " And LotNo Like '%" + oLot.LotNo + "%'";
                //if (oLot.ProductID > 0)
                //    sSQL = sSQL + " And ProductID=" + oLot.ProductID;
                //if (oLot.WorkingUnitID > 0)
                //    sSQL = sSQL + " And WorkingUnitID=" + oLot.WorkingUnitID;
                //if (oLot.BUID > 0)
                //    sSQL = sSQL + " And BUID=" + oLot.BUID;
        
                //oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                if (oLot.Params.Contains("NotInHouse"))
                {
                    sSQL = "Select * from View_LotParent Where LotID<>0 and isnull(Balance,0)>0 and isnull(BalanceLot,0)>0.1";

                    if (!string.IsNullOrEmpty(oLot.LotNo))
                        sSQL = sSQL + " And LotNo Like '%" + oLot.LotNo + "%'";
                    if (oLot.ProductID > 0)
                        sSQL = sSQL + " And ProductID=" + oLot.ProductID;
                    if (oLot.ModelReferenceID > 0)
                        sSQL = sSQL + " And DyeingOrderID IN (SELECT TT.DyeingOrderID FROM DyeingOrderDetail AS TT WHERE TT.DyeingOrderDetailID =" + oLot.ModelReferenceID + ")";
                    oLotParents = LotParent.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                    foreach (LotParent oLotParent in oLotParents)
                    {
                        oLot = new Lot();
                        oLot.LotNo = oLotParent.LotNo;
                        //oLot.LogNo = oLotParent.LogNo;
                        oLot.LotID = oLotParent.LotID;
                        oLot.Balance = oLotParent.Balance;
                        oLot.WorkingUnitID = oLotParent.WorkingUnitID;
                        oLot.OperationUnitName = oLotParent.StoreName;
                        oLot.ProductID = oLotParent.ProductID;
                        oLot.ProductName = oLotParent.ProductName;
                        oLot.MUName = oLotParent.MUName;
                        oLots.Add(oLot);
                    }
                }
                else
                {
                    sSQL = "Select * from View_FabricLotAssign where Qty>0.0 and FEOSDID in (Select FEOSDID from DyeingOrderFabricDetail where DyeingOrderDetailID=" + oLot.ModelReferenceID + " )";
                    //if (!string.IsNullOrEmpty(oLotParent.LotNo))
                    //    sSQL = sSQL + " And LotNo Like '%" + oLotParent.LotNo + "%'";
                    //if (oLot.ProductID > 0)
                    //    sSQL = sSQL + " And ProductID=" + oLot.ProductID;
                    //if (oLotParent.DyeingOrderID > 0)
                    //    sSQL = sSQL + " And DyeingOrderID=" + oLotParent.DyeingOrderID;
                    oFabricLotAssigns = FabricLotAssign.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    foreach (FabricLotAssign oLotParent in oFabricLotAssigns)
                    {
                        oLot = new Lot();
                        oLot.LotNo = oLotParent.LotNo;
                        //oLot.LogNo = oLotParent.LogNo;
                        oLot.LotID = oLotParent.LotID;
                        oLot.Balance = oLotParent.Balance;
                        oLot.WorkingUnitID = oLotParent.WorkingUnitID;
                        oLot.OperationUnitName = oLotParent.OperationUnitName;
                        oLot.ProductID = oLotParent.ProductID;
                        oLot.ProductName = oLotParent.ProductName;
                        oLot.MUName = "Kg";
                        oLots.Add(oLot);
                    }
                }

            }
            catch (Exception ex)
            {
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                oLots = new List<Lot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsLot(Lot oLot)
        {
            List<Lot> oLots = new List<Lot>();
            List<LotParent> oLotParents = new List<LotParent>();
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<FabricLotAssign> oFabricLotAssigns = new List<FabricLotAssign>();
            string sLotID = "";
            string sParentLotID = "";
            string sSQL = "";
            string sWUIDs = "";
            int nDyeingOrderDetailID = 0;
            int nDyeingOrderID = 0;



            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits = WorkingUnit.GetsPermittedStore(0, EnumModuleName.DUPSchedule, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            sWUIDs = string.Join(",", oWorkingUnits.Select(x => x.WorkingUnitID).Distinct().ToList());
            if (oLot.ModelReferenceID>0)
            {
                nDyeingOrderDetailID = oLot.ModelReferenceID;
            }
            try
            {

                if (nDyeingOrderDetailID>0)
                {
                    sSQL = "Select * from View_DyeingOrderDetailWaitingForRS WHERE DyeingOrderDetailID=" + nDyeingOrderDetailID + "";
                    oRouteSheetDOs = RouteSheetDO.GetsDOYetTORS(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                if (oRouteSheetDOs.Count <= 0)
                {
                    throw new Exception("order not found for this Dyeing card!!");
                }

                if (oRouteSheetDOs.Count > 0)
                {
                    oDUOrderSetup = oDUOrderSetup.GetByType((int)oRouteSheetDOs[0].OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    nDyeingOrderID = oRouteSheetDOs[0].DyeingOrderID;

                }
                oLot.LotNo = (!string.IsNullOrEmpty(oLot.LotNo)) ? oLot.LotNo.Trim() : "";
                if (oDUOrderSetup.IsOpenRawLot == false)
                {
                    if (oDUOrderSetup.IsInHouse)
                    {
                        sSQL = "Select * from View_FabricLotAssign where FEOSDID in (Select FEOSDID from DyeingOrderFabricDetail where DyeingOrderID=" + nDyeingOrderID + " )";
                        //if (!string.IsNullOrEmpty(oLotParent.LotNo))
                        //    sSQL = sSQL + " And LotNo Like '%" + oLotParent.LotNo + "%'";
                        if (oLot.ProductID > 0)
                            sSQL = sSQL + " And ProductID=" + oLot.ProductID;
                        //if (oLotParent.DyeingOrderID > 0)
                        //    sSQL = sSQL + " And DyeingOrderID=" + oLotParent.DyeingOrderID;
                        oFabricLotAssigns = FabricLotAssign.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        sLotID = string.Join(",", oFabricLotAssigns.Select(x => x.LotID).Distinct().ToList());
                        sParentLotID = string.Join(",", oFabricLotAssigns.Where(b => b.ParentLotID > 0).Select(x => x.ParentLotID).Distinct().ToList());

                        if (!string.IsNullOrEmpty(sParentLotID))
                        {
                            sLotID = sLotID + "," + sParentLotID;
                        }

                        if (oFabricLotAssigns.Count <= 0)
                        {
                            throw new Exception("Lot yet not assign with this order!!");
                        }
                    }
                    else
                    {
                        sSQL = "Select * from View_LotParent Where LotID<>0";
                        //if (!string.IsNullOrEmpty(oLotParent.LotNo))
                        //    sSQL = sSQL + " And LotNo Like '%" + oLotParent.LotNo + "%'";
                        if (oLot.ProductID > 0)
                            sSQL = sSQL + " And ProductID=" + oLot.ProductID;
                        if (nDyeingOrderID > 0)
                            sSQL = sSQL + " And DyeingOrderID=" + nDyeingOrderID;
                        oLotParents = LotParent.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        sLotID = string.Join(",", oLotParents.Select(x => x.LotID).Distinct().ToList());
                        sParentLotID = string.Join(",", oLotParents.Where(b => b.ParentLotID > 0).Select(x => x.ParentLotID).Distinct().ToList());

                        if (!string.IsNullOrEmpty(sParentLotID))
                        {
                            sLotID = sLotID + "," + sParentLotID;
                        }
                        if (nDyeingOrderID <= 0)
                        {
                            throw new Exception("Lot yet not assign with this order!!");
                        }
                    }
                    if (string.IsNullOrEmpty(sLotID))
                    {
                        throw new Exception("Lot yet not assign with this order!!");
                    }

                }
                sSQL = "Select * from View_Lot Where Balance>0 "; //and WorkingUnitID in (Select WorkingUnitID from WorkingUnit where UnitType=" + (int)EnumWoringUnitType.Raw + ")

                if (oLot.WorkingUnitID > 0)
                    sSQL = sSQL + " And WorkingUnitID=" + oLot.WorkingUnitID;

                if (!string.IsNullOrEmpty(sWUIDs))
                    sSQL = sSQL + " And WorkingUnitID in (" + sWUIDs+")";

                if (oDUOrderSetup.IsOpenRawLot == true)
                {
                    if (oLot.ProductID > 0)
                        sSQL = sSQL + " And ProductID=" + oLot.ProductID;

                }
                //if (oLotParent.ContractorID > 0) { sSQL = sSQL + " And ContractorID=" + oLotParent.ContractorID; }

                if (!string.IsNullOrEmpty(sLotID))
                    sSQL = sSQL + " And  (LotID in (" + sLotID + ") or ParentLotID in (" + sLotID + "))";


                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oDUOrderSetup.IsOpenRawLot == false)
                {
                    if (oDUOrderSetup.IsInHouse)
                    {
                        oLots.ForEach(x =>
                        {
                            if (oFabricLotAssigns.FirstOrDefault() != null && oFabricLotAssigns.FirstOrDefault().ProductID > 0 && oFabricLotAssigns.Where(b => (b.LotID == x.LotID || b.ParentLotID == x.LotID || b.LotID == x.ParentLotID)).Count() > 0)
                            {
                                x.OrderRecapNo = oFabricLotAssigns.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.ParentLotID == x.LotID) && p.ProductID > 0).FirstOrDefault().ExeNo;
                                x.StockValue = oFabricLotAssigns.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.ParentLotID == x.LotID) && p.ProductID > 0).FirstOrDefault().Balance;
                                // x.Qt = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                            }
                        });
                    }
                    else
                    {
                        oLots.ForEach(x =>
                        {
                            if (oLotParents.FirstOrDefault() != null && oLotParents.FirstOrDefault().DyeingOrderID > 0 && oLotParents.Where(b => (b.LotID == x.LotID || b.LotID == x.ParentLotID || b.LotID == x.ParentLotID)).Count() > 0)
                            {
                                x.OrderRecapNo = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                                x.StockValue = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().Balance;
                                // x.Qt = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                            }
                        });
                    }
                }
                else
                {
                    oLots.ForEach(o => o.StockValue = (o.Balance));
                }
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
                oLot.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        
        #region Get Search Value
        [HttpPost]
        public JsonResult RefreshMachineWise(DUPSchedule oDUPSchedule, bool bUptoLoadDyeMachine, int sScheduleDateType)
        {
            string sLocationIDs = "";
            var nCount = 0;
            List<DUPSLot> oDUPSLots = new List<DUPSLot>();
            List<Location> oLocations = new List<Location>();
            List<Machine> oMachines = new List<Machine>();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            //_sSQL = "SELECT * FROM Location where LocationID in(SELECT Distinct LocationID FROM VIEW_CapitalResource where ISNULL(IsActive,1)=1 AND BUID = " + oDUPSchedule.BUID + " AND ResourcesType = " + (int)EnumResourcesType.Machine + " )";
            _sSQL = "SELECT * FROM View_Location where LocationID in (Select LocationID from WorkingUnit where BUID=" + oDUPSchedule.BUID + ")";
            oLocations = Location.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
         
                #region Machine
                string sSQL = "SELECT * FROM Machine WHERE BUID=" + oDUPSchedule.BUID;

                if (!string.IsNullOrEmpty(oDUPSchedule.MachineNo))
                {
                    sSQL += " AND MachineID IN (" + oDUPSchedule.MachineNo + ")";
                }
                 if (!string.IsNullOrEmpty(oDUPSchedule.MachineName))
           
                    sSQL += " AND MachineID IN (SELECT MachineID FROM Machine WHERE MachineTypeID=" + oDUPSchedule.MachineName + ")";
                
                else
                    sSQL += " AND MachineTypeID IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID =" + (int)EnumModuleName.DUPSchedule + ")";
                sSQL += " AND Activity=1 ORDER BY SequenceNo";
                oMachines = Machine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                #endregion
    
            sLocationIDs = Location.IDInString(oLocations);

            //DateTime today = oDUPSchedule.StartTime;
            DateTime startOfMonth = oDUPSchedule.StartTime;
            DateTime endOfMonth = startOfMonth.AddDays(1);
            if (bUptoLoadDyeMachine)
            {
                _sSQL = "Select * from View_DUPSchedule where  MachineID IN (" + string.Join(",", oMachines.Select(x => x.MachineID)) + ") and ScheduleStatus in (" + (int)EnumProductionScheduleStatus.Publish + "," + (int)EnumProductionScheduleStatus.Running + "," + (int)EnumProductionScheduleStatus.Urgent + ") and LocationID in (" + sLocationIDs + ") " + " order by MachineID,StartTime ";
            }
            else
            {
                _sSQL = "Select * from View_DUPSchedule where LocationID in (" + sLocationIDs + ") "
               + " and MachineID IN (" + string.Join(",", oMachines.Select(x => x.MachineID)) + ") and ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')) order by MachineID,StartTime ";
            }
             _oDUPSchedule.DUPSchedules = DUPSchedule.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oDUPSchedule.DUPSchedules.Count > 0)
            {
                if (bUptoLoadDyeMachine)
                {
                    _sSQL = "Select * from View_DUPScheduleDetail Where DUPScheduleID IN (Select DUPScheduleID from View_DUPSchedule where MachineID IN (" + string.Join(",", oMachines.Select(x => x.MachineID)) + ") and LocationID in (" + sLocationIDs + ")   and ScheduleStatus in (" + (int)EnumProductionScheduleStatus.Publish + "," + (int)EnumProductionScheduleStatus.Running + "," + (int)EnumProductionScheduleStatus.Urgent + "))";  // RSState=7  UnloadFromDyemachine
                }
                else
                {
                    _sSQL = "Select * from View_DUPScheduleDetail Where DUPScheduleID IN (Select DUPScheduleID from View_DUPSchedule where LocationID in (" + sLocationIDs + ")  and MachineID IN (" + string.Join(",", oMachines.Select(x => x.MachineID)) + ") and ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') ) )";  // RSState=7  UnloadFromDyemachine
                }
                _oDUPSchedule.DUPScheduleDetails = DUPScheduleDetail.GetsSqL(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            //if (bUptoLoadDyeMachine)
            //{
            //    #region Machine
            //    string sSQL = "SELECT * FROM Machine WHERE BUID=" + oDUPSchedule.BUID;

            //    if (_oDUPSchedule.DUPSchedules.Count > 0)
            //    {
            //        sSQL += " AND MachineID IN (" + string.Join(",", _oDUPSchedule.DUPSchedules.Select(x => x.MachineID)) + ")";
            //    }
            //    sSQL += " ORDER BY SequenceNo";
            //    oMachines = Machine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //    #endregion
            //}

            foreach (Machine oItem in oMachines)
            {
                 nCount = (from oPS in _oDUPSchedule.DUPSchedules where oPS.MachineID == oItem.MachineID && oPS.BUID == oItem.BUID select oPS).Count();
                 oItem.MachineNoWithCapacityAndTotalSchedule = "[" + oItem.Code + "]" + oItem.Name + "[" + ((oItem.Capacity > 0) ? oItem.Capacity.ToString() : "") + "" + (!string.IsNullOrEmpty(oItem.Capacity2) ? oItem.Capacity2.ToString() : "") + "]" + " (" + Convert.ToInt32(nCount) + ")"; 
            }
            //_sSQL = string.Join(",", _oDUPSchedule.DUPSchedules.Select(x => x.DUPScheduleID).Distinct().ToList());
            //if (!string.IsNullOrEmpty(_sSQL))
            //{
            //    _sSQL = "SELECT * FROM View_DUPSLot where DUPScheduleID in (" + _sSQL + ")";
            //    oDUPSLots = DUPSLot.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //}


            foreach (DUPSchedule oItem in _oDUPSchedule.DUPSchedules)
            {

                oItem.DUPScheduleDetails = new List<DUPScheduleDetail>();
                oItem.DUPScheduleDetails = _oDUPSchedule.DUPScheduleDetails.Where(x => x.DUPScheduleID == oItem.DUPScheduleID).ToList();
                sDateRange = oItem.StartTime.ToString("dd MMM yy") + "(" + oItem.StartTime.ToString("HH:mm tt") + "-" + oItem.EndTime.ToString("HH:mm tt")+")";
                if (oItem.DUPScheduleDetails.Count == 0 || oItem.DUPScheduleDetails == null) { oItem.OrderInfo = oItem.ScheduleNo + " " + sDateRange; }
                else
                {
                   // oItem.LotNo = string.Join("+", oDUPSLots.Where(x => x.DUPScheduleID == oItem.DUPScheduleID).Select(x => x.LotNo + " (" + Global.MillionFormat(x.Qty) + ")").Distinct().ToList());
                    oItem.RSStatus = (int)oItem.DUPScheduleDetails.FirstOrDefault().RSState;
                    if (oRouteSheetSetup.IsShowBuyer)
                    { oItem.OrderInfo = sDateRange + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.BuyerName).Distinct().ToList()); }
                    else { oItem.OrderInfo = sDateRange + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ContractorName).Distinct().ToList()); }
                   
                    oItem.OrderInfo = oItem.OrderInfo + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.OrderNo).Distinct().ToList());
                    oItem.OrderInfo = oItem.OrderInfo + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ProductName).Distinct().ToList());
                    if (string.IsNullOrEmpty(oItem.LotNo) && oItem.DUPScheduleDetails[0].IsInHouse == false)
                    { oItem.OrderInfo = oItem.OrderInfo + "</br> Lot: " + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.BuyerRef).ToList()); }
                    
                    oItem.OrderInfo = oItem.OrderInfo + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ColorName).Distinct().ToList());
                    oItem.OrderInfo = oItem.OrderInfo + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => Global.MillionFormat(x.Qty) + " " + oRouteSheetSetup.MUnit + ((x.BagCount <= 0) ? "" : " (" + x.BagCount.ToString() + " " + x.HankorConeST + ")")).ToList());
                    if (!string.IsNullOrEmpty(oRouteSheetSetup.BatchCode))
                    {
                        oItem.OrderInfo = oItem.OrderInfo + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => oRouteSheetSetup.BatchCode + "-" + x.PSBatchNo).ToList());
                    }
                    //oRouteSheetSetup.BatchCode + "-" + oItem2.PSBatchNo
                    if (!String.IsNullOrEmpty(oItem.Note))
                    {
                        oItem.OrderInfo = oItem.OrderInfo + "</br>" + oItem.Note;
                    }
                    if (!String.IsNullOrEmpty(oItem.DUPScheduleDetails[0].RouteSheetNo))
                    {
                        oItem.OrderInfo = oItem.OrderInfo + "</br>Batch No: " + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.RouteSheetNo).Distinct().ToList());
                    }

                }

            }
            _oDUPSchedule.DyeMachines = oMachines;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUPSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsForMachineView(DUPSchedule oDUPSchedule, bool bUptoLoadDyeMachine, int sScheduleDateType)
        {
            string sLocationIDs = "";
            //var nCount = 0;
            //bUptoLoadDyeMachine = true;
            List<Location> oLocations = new List<Location>();
            List<Machine> oMachines = new List<Machine>();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ////_sSQL = "SELECT * FROM Location where LocationID in(SELECT Distinct LocationID FROM VIEW_CapitalResource where ISNULL(IsActive,1)=1 AND BUID = " + oDUPSchedule.BUID + " AND ResourcesType = " + (int)EnumResourcesType.Machine + " )";
            _sSQL = "SELECT * FROM View_Location where LocationID in (Select LocationID from WorkingUnit where BUID=" + oDUPSchedule.BUID + ")";
            oLocations = Location.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            sLocationIDs = Location.IDInString(oLocations);
            DateTime startOfMonth = oDUPSchedule.StartTime;
            DateTime endOfMonth = startOfMonth.AddDays(1);
          
                #region Machine
                string sSQL = "SELECT * FROM Machine WHERE BUID=" + oDUPSchedule.BUID;

                if (!string.IsNullOrEmpty(oDUPSchedule.MachineNo))
                    sSQL += " AND MachineID IN (" + oDUPSchedule.MachineNo + ")";
                if (!string.IsNullOrEmpty(oDUPSchedule.MachineName))
                    sSQL += " AND MachineID IN (SELECT MachineID FROM Machine WHERE MachineTypeID=" + oDUPSchedule.MachineName + ")";
                else
                    sSQL += " AND MachineTypeID IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID =" + (int)EnumModuleName.DUPSchedule + ")";
                sSQL += " ORDER BY SequenceNo";
                oMachines = Machine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                #endregion

                if (!bUptoLoadDyeMachine)
                {
                _sSQL = "Select * from View_DUPScheduleDetail Where DUPScheduleID IN (Select DUPScheduleID from View_DUPSchedule where LocationID in (" + sLocationIDs + ") "
                      + " and MachineID IN (" + string.Join(",", oMachines.Select(x => x.MachineID)) + ") and ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') ) ) order by MachineID,StartTime";  // RSState=7  UnloadFromDyemachine
                _oDUPSchedule.DUPScheduleDetails = DUPScheduleDetail.GetsSqL(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                _sSQL = "Select * from View_DUPScheduleDetail Where LocationID in (" + sLocationIDs + ") "
                   + " and MachineID IN (" + string.Join(",", oMachines.Select(x => x.MachineID)) + ") and ScheduleStatus in (" + (int)EnumProductionScheduleStatus.Publish + "," + (int)EnumProductionScheduleStatus.Running + "," + (int)EnumProductionScheduleStatus.Urgent + ") order by MachineID,StartTime";  // RSState=7  UnloadFromDyemachine
                _oDUPSchedule.DUPScheduleDetails = DUPScheduleDetail.GetsSqL(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUPSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RefreshManthWise(DUPSchedule oDUPSchedule, bool bUptoLoadDyeMachine, int sScheduleDateType)
        {

            string sLocationIDs = "";
            List<Location> oLocations = new List<Location>();
            List<Machine> oMachines = new List<Machine>();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            _sSQL = "SELECT * FROM View_Location where LocationID in (Select LocationID from WorkingUnit where BUID=" + oDUPSchedule.BUID + ")";
            oLocations = Location.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Machine
            string sSQL = "SELECT * FROM Machine WHERE BUID=" + oDUPSchedule.BUID;

            if (!string.IsNullOrEmpty(oDUPSchedule.MachineNo))
                sSQL += " AND MachineID IN (" + oDUPSchedule.MachineNo + ")";
            if (!string.IsNullOrEmpty(oDUPSchedule.MachineName))
                sSQL += " AND MachineID IN (SELECT MachineID FROM Machine WHERE MachineTypeID=" + oDUPSchedule.MachineName + ")";
            else
                sSQL += " AND MachineTypeID IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID =" + (int)EnumModuleName.DUPSchedule + ")";
            sSQL += "AND Activity=1 ORDER BY SequenceNo";
            oMachines = Machine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            sLocationIDs = Location.IDInString(oLocations);

            DateTime today = oDUPSchedule.StartTime;
            DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            //select sum(PS.OrderCount)as ScheduleCount,MachineID,Convert(Date,(Convert(varchar(4),SCYear)+'-'+Convert(varchar(4),SCMonth)+'-'+Convert(varchar(4),SCDay)+'00:00:00')) as StartTime from (Select MachineID ,Day(StartTime) as SCDay,month(StartTime) as SCMonth,Year(StartTime) as SCYear,(select Count(*) from DUPScheduleDetail where DUPScheduleDetail.DUPScheduleID=DUPS.DUPScheduleID ) as OrderCount from DUPSchedule as DUPS where LocationID in (3) and  ((StartTime>='5/1/2017 12:00:00 AM' and StartTime<='5/31/2017 12:00:00 AM') or (EndTime>='5/1/2017 12:00:00 AM' and EndTime<='5/31/2017 12:00:00 AM')) )as PS Group by MachineID,SCDay,SCMonth,SCYear
            string sql = "select sum(PS.OrderCount)as OrderCount,MachineID, Convert(Date,(Convert(varchar(4),SCYear)+'-'+Convert(varchar(4),SCMonth)+'-'+Convert(varchar(4),SCDay)+' 00:00:00')) as StartTime from (Select MachineID ,Day(StartTime) as SCDay,month(StartTime) as SCMonth,Year(StartTime) as SCYear,(select Count(*) from DUPScheduleDetail where DUPScheduleDetail.DUPScheduleID=DUPS.DUPScheduleID ) as OrderCount from DUPSchedule as DUPS where LocationID in (" + sLocationIDs + ") "
                        +" and MachineID IN ("+ string.Join(",",oMachines.Select(x=>x.MachineID)) +")" 
                        +" and  ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')) )as PS Group by MachineID,SCDay,SCMonth,SCYear";
            _oDUPSchedule.DUPSchedules = DUPSchedule.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //if (_oDUPSchedule.DUPSchedules.Count > 0)
            //{
            //    sql = "Select * from View_DUPScheduleDetail Where DUPScheduleID IN (Select DUPScheduleID from View_DUPSchedule where LocationID=" + sLocationIDs + " and DUPScheduleID in ( Select distinct(DUPScheduleID) from View_DUPScheduleDetail Where ((StartTime>='" + startOfMonth + "' and StartTime<='" + endOfMonth + "') or (EndTime>='" + startOfMonth + "' and EndTime<='" + endOfMonth + "'))  and RSState<7 ) and Activity='1' )";  // RSState=7  UnloadFromDyemachine
            //    _oDUPSchedule.DUPScheduleDetails = DUPScheduleDetail.GetsSqL(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //}

            foreach (Machine oItem in oMachines)
            {
                var nCount = (from oPS in _oDUPSchedule.DUPSchedules where oPS.MachineID == oItem.MachineID && oPS.BUID == oItem.BUID select oPS).Count();
                oItem.MachineNoWithCapacityAndTotalSchedule = "[" + oItem.Code + "]" + oItem.Name + "[" + ((oItem.Capacity > 0) ? oItem.Capacity.ToString() : "") + "" + (!string.IsNullOrEmpty(oItem.Capacity2) ? oItem.Capacity2.ToString() : "") + "]" + " (" + Convert.ToInt32(nCount) + ")";
            }

            //foreach (DUPSchedule oItem in _oDUPSchedule.DUPSchedules)
            //{
            //    oItem.DUPScheduleDetails = new List<DUPScheduleDetail>();
            //    oItem.DUPScheduleDetails = _oDUPSchedule.DUPScheduleDetails.Where(x => x.DUPScheduleID == oItem.DUPScheduleID).ToList();
            //    foreach (DUPScheduleDetail oItem2 in oItem.DUPScheduleDetails)
            //    {
            //        oItem.ScheduleNo = "Time:" + oItem.StartTime.ToString("dd MMM yy HH:MM") + "-" + oItem.EndTime.ToString("dd MMM yy HH:MM") + "   </br> Batch:" + oItem.ScheduleNo + "</br> Order No:" + oItem2.OrderNo + "" + "</br>" + oItem2.BuyerName;
            //    }

            //}

            _oDUPSchedule.DyeMachines = oMachines;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUPSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsDetailForCalendar(DUPSchedule oDUPSchedule)
        {
            string sLocationIDs = "";
            List<Location> oLocations = new List<Location>();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            List<Machine> oMachines = new List<Machine>();

            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _sSQL = "SELECT * FROM View_Location where LocationID in (Select LocationID from WorkingUnit where BUID=" + oDUPSchedule.BUID + ")";
            oLocations = Location.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            sLocationIDs = Location.IDInString(oLocations);

            #region Machine
            string sSQL = "SELECT * FROM Machine WHERE BUID=" + oDUPSchedule.BUID;

            if (!string.IsNullOrEmpty(oDUPSchedule.MachineNo))
                sSQL += " AND MachineID IN (" + oDUPSchedule.MachineNo + ")";
            if (!string.IsNullOrEmpty(oDUPSchedule.MachineName))
                sSQL += " AND MachineID IN (SELECT MachineID FROM Machine WHERE MachineTypeID=" + oDUPSchedule.MachineName + ")";
            else
                sSQL += " AND MachineTypeID IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID =" + (int)EnumModuleName.DUPSchedule + ")";
            sSQL += " ORDER BY SequenceNo";
            oMachines = Machine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            //DateTime today = oDUPSchedule.StartTime;
            DateTime startOfMonth = oDUPSchedule.StartTime;
            DateTime endOfMonth = startOfMonth.AddDays(1);
            //select sum(PS.OrderCount)as ScheduleCount,MachineID,Convert(Date,(Convert(varchar(4),SCYear)+'-'+Convert(varchar(4),SCMonth)+'-'+Convert(varchar(4),SCDay)+'00:00:00')) as StartTime from (Select MachineID ,Day(StartTime) as SCDay,month(StartTime) as SCMonth,Year(StartTime) as SCYear,(select Count(*) from DUPScheduleDetail where DUPScheduleDetail.DUPScheduleID=DUPS.DUPScheduleID ) as OrderCount from DUPSchedule as DUPS where LocationID in (3) and  ((StartTime>='5/1/2017 12:00:00 AM' and StartTime<='5/31/2017 12:00:00 AM') or (EndTime>='5/1/2017 12:00:00 AM' and EndTime<='5/31/2017 12:00:00 AM')) )as PS Group by MachineID,SCDay,SCMonth,SCYear
            string sql = "Select MachineID,MachineName,SUM(OrderCount) as OrderCount,SUM(Qty_D) as Qty  from (Select *,(select Count(*) from DUPScheduleDetail where DUPScheduleDetail.DUPScheduleID=DUPS.DUPScheduleID ) as OrderCount ,(select SUM(Qty) from DUPScheduleDetail where DUPScheduleDetail.DUPScheduleID=DUPS.DUPScheduleID ) as Qty_D from view_DUPSchedule as DUPS  where LocationID in (" + sLocationIDs + ") "
                       + " and MachineID IN ("+ string.Join(",",oMachines.Select(x=>x.MachineID)) +") and  ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')) ) as dd Group by MachineID,MachineName";
            _oDUPSchedules = DUPSchedule.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUPSchedules);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RefreshOrderWise(DUPSchedule oDUPSchedule)
        {

            string sLocationIDs = "";
            var nCount = 0;
            List<DUPSLot> oDUPSLots = new List<DUPSLot>();
            List<Location> oLocations = new List<Location>();
            List<Machine> oMachines = new List<Machine>();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            //_sSQL = "SELECT * FROM Location where LocationID in(SELECT Distinct LocationID FROM VIEW_CapitalResource where ISNULL(IsActive,1)=1 AND BUID = " + oDUPSchedule.BUID + " AND ResourcesType = " + (int)EnumResourcesType.Machine + " )";
            _sSQL = "SELECT * FROM View_Location where LocationID in (Select LocationID from WorkingUnit where BUID=" + oDUPSchedule.BUID + ")";
            oLocations = Location.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sLocationIDs = Location.IDInString(oLocations);

            if (!string.IsNullOrEmpty(oDUPSchedule.Params))
                _sSQL = "Select * from View_DUPSchedule where  LocationID in (" + sLocationIDs + ") and DUPScheduleID  in (select DUPScheduleID from DUPScheduleDetail where DUPScheduleDetail.DODID in (" + oDUPSchedule.Params + ") )  order by MachineID,StartTime ";
            else 
                if (!string.IsNullOrEmpty(oDUPSchedule.RouteSheetNo))
                _sSQL = "Select * from View_DUPSchedule where  LocationID in (" + sLocationIDs + ") and DUPScheduleID  in (select DUPScheduleID from View_DUPScheduleDetail WHERE RouteSheetNo LIKE '%" + oDUPSchedule.RouteSheetNo + "%' )  order by MachineID,StartTime ";
            
            _oDUPSchedule.DUPSchedules = DUPSchedule.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oDUPSchedule.DUPSchedules.Count > 0)
            {
                _sSQL = "Select * from View_DUPScheduleDetail Where DUPScheduleID in (" + string.Join(",", _oDUPSchedule.DUPSchedules.Select(x => x.DUPScheduleID)) + ") ";  // RSState=7  UnloadFromDyemachine
                _oDUPSchedule.DUPScheduleDetails = DUPScheduleDetail.GetsSqL(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
          
                #region Machine
                string sSQL = "SELECT * FROM Machine WHERE BUID=" + oDUPSchedule.BUID;

                if (_oDUPSchedule.DUPSchedules.Count > 0)
                {
                    sSQL += " AND MachineID IN (" + string.Join(",", _oDUPSchedule.DUPSchedules.Select(x => x.MachineID)) + ")";
                }
                sSQL += " ORDER BY SequenceNo";
                oMachines = Machine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                #endregion
          
            foreach (Machine oItem in oMachines)
            {
                nCount = (from oPS in _oDUPSchedule.DUPSchedules where oPS.MachineID == oItem.MachineID && oPS.BUID == oItem.BUID select oPS).Count();
                oItem.MachineNoWithCapacityAndTotalSchedule = "[" + oItem.Code + "]" + oItem.Name + "[" + ((oItem.Capacity > 0) ? oItem.Capacity.ToString() : "") + "" + (!string.IsNullOrEmpty(oItem.Capacity2) ? oItem.Capacity2.ToString() : "") + "]" + " (" + Convert.ToInt32(nCount) + ")";
            }

            _sSQL = string.Join(",", _oDUPSchedule.DUPSchedules.Select(x => x.DUPScheduleID).Distinct().ToList());
            if (!string.IsNullOrEmpty(_sSQL))
            {
                _sSQL = "SELECT * FROM View_DUPSLot where DUPScheduleID in (" + _sSQL + ")";
                oDUPSLots = DUPSLot.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            foreach (DUPSchedule oItem in _oDUPSchedule.DUPSchedules)
            {

                oItem.DUPScheduleDetails = new List<DUPScheduleDetail>();
                oItem.DUPScheduleDetails = _oDUPSchedule.DUPScheduleDetails.Where(x => x.DUPScheduleID == oItem.DUPScheduleID).ToList();
                sDateRange = oItem.StartTime.ToString("dd MMM yy") + "(" + oItem.StartTime.ToString("HH:mm tt") + "-" + oItem.EndTime.ToString("HH:mm tt") + ")";
                if (oItem.DUPScheduleDetails.Count == 0 || oItem.DUPScheduleDetails == null) { oItem.OrderInfo = oItem.ScheduleNo + " " + sDateRange; }
                else
                {
                    //oItem.LotNo = string.Join("+", oDUPSLots.Where(x => x.DUPScheduleID == oItem.DUPScheduleID).Select(x => x.LotNo + " (" + Global.MillionFormat(x.Qty) + ")").Distinct().ToList());
                    oItem.RSStatus = (int)oItem.DUPScheduleDetails.FirstOrDefault().RSState;
                    if (oRouteSheetSetup.IsShowBuyer)
                    { oItem.OrderInfo = sDateRange + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.BuyerName).Distinct().ToList()); }
                    else { oItem.OrderInfo = sDateRange + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ContractorName).Distinct().ToList()); }

                    oItem.OrderInfo = oItem.OrderInfo + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.OrderNo).Distinct().ToList());
                    oItem.OrderInfo = oItem.OrderInfo + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ProductName).Distinct().ToList());
                    //if (!string.IsNullOrEmpty(oItem.LotNo))
                    //{ oItem.OrderInfo = oItem.OrderInfo + "</br> lot: " + oItem.LotNo; }// string.Join("+", oItem.DUPScheduleDetails.Select(x => x.LotNo).ToList()); }
                    if (string.IsNullOrEmpty(oItem.LotNo) && oItem.DUPScheduleDetails[0].IsInHouse == false)
                    { oItem.OrderInfo = oItem.OrderInfo + "</br> Lot: " + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.BuyerRef).ToList()); }
                    
                    oItem.OrderInfo = oItem.OrderInfo + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ColorName).Distinct().ToList());
                    oItem.OrderInfo = oItem.OrderInfo + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => Global.MillionFormat( x.Qty) + " " + oRouteSheetSetup.MUnit + ((x.BagCount <= 0) ? "" : " (" + x.BagCount.ToString() + " " + x.HankorConeST + ")")).ToList());
                    if (!string.IsNullOrEmpty(oRouteSheetSetup.BatchCode))
                    {
                        oItem.OrderInfo = oItem.OrderInfo + "</br>" + string.Join("+", oItem.DUPScheduleDetails.Select(x => oRouteSheetSetup.BatchCode + "-" + x.PSBatchNo).ToList());
                    }
                    //oRouteSheetSetup.BatchCode + "-" + oItem2.PSBatchNo
                    if (!String.IsNullOrEmpty(oItem.Note))
                    {
                        oItem.OrderInfo = oItem.OrderInfo + "</br>" + oItem.Note;
                    }
                    if (!String.IsNullOrEmpty(oItem.DUPScheduleDetails[0].RouteSheetNo))
                    {
                        oItem.OrderInfo = oItem.OrderInfo + "</br>Batch No: " + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.RouteSheetNo).Distinct().ToList());
                    }

                }

                //nCount = 0;
                //foreach (DUPScheduleDetail oItem2 in oItem.DUPScheduleDetails)
                //{
                //    nCount++;
                //    if (nCount > 1)
                //    {
                //        oItem.OrderInfo = oItem.OrderInfo + "</br>" + oItem2.ContractorName + "" + "</br>" + oItem2.OrderNo + "</br>" + oItem2.ProductName + (string.IsNullOrEmpty(oItem.LotNo) ? "" : "</br>" + oItem.LotNo) + "\n" + oItem2.ColorName + "</br>" + ((oItem2.Qty <= 0) ? "" : "\n" + oItem2.Qty) + " " + oRouteSheetSetup.MUnit + " " + ((oItem2.BagCount <= 0) ? "" : "(" + oItem2.BagCount.ToString() + " " + oItem2.HankorConeST + ")") + "</br>" + oRouteSheetSetup.BatchCode + "-" + oItem2.PSBatchNo + ((string.IsNullOrEmpty(oItem.Note)) ? "" : "</br>" + oItem.Note);
                //    }
                //    else
                //    { oItem.OrderInfo = sDateRange + "</br>" + oItem2.ContractorName + "" + "</br>" + oItem2.OrderNo + "</br>" + oItem2.ProductName + (string.IsNullOrEmpty(oItem.LotNo) ? "" : "</br> Lot:" + oItem.LotNo) + "</br>" + oItem2.ColorName + "</br>" + ((oItem2.Qty <= 0) ? "" : "\n" + oItem2.Qty) + " " + oRouteSheetSetup.MUnit + " " + ((oItem2.BagCount <= 0) ? "" : "(" + oItem2.BagCount.ToString() + " " + oItem2.HankorConeST + ")") + "</br>" + oRouteSheetSetup.BatchCode + "-" + oItem2.PSBatchNo + ((string.IsNullOrEmpty(oItem.Note)) ? "" : "</br>" + oItem.Note); }
                //    if (!String.IsNullOrEmpty(oItem2.RouteSheetNo))
                //    {
                //        oItem.OrderInfo = oItem.OrderInfo + "</br> Batch No: " + oItem2.RouteSheetNo;
                //    }
                //}
            }
            _oDUPSchedule.DyeMachines = oMachines;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUPSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Print Schedule
        public ActionResult PrintDUPSchedule(string newstring, bool bPortrait)
        {
            _oDUPSchedule = new DUPSchedule();
            string sLocationIDs = "";
            int nCount = 0;
            List<Location> oLocations = new List<Location>();
            List<Machine> oMachines = new List<Machine>();
            List<DUPSLot> oDUPSLots = new List<DUPSLot>();
            List<LotParent> oLotParents = new List<LotParent>();
            List<FabricLotAssign> oFabricLotAssigns = new List<FabricLotAssign>();
            int cSpace = Convert.ToInt32(newstring.Split('~')[0]);
            DateTime StartSearchTime = Convert.ToDateTime(newstring.Split('~')[1]);
            DateTime EndSearchTime = Convert.ToDateTime(newstring.Split('~')[2]);
            int LocationId = Convert.ToInt32(newstring.Split('~')[3]);
            string DyeMachineIds = Convert.ToString(newstring.Split('~')[4]);
            string DateDUPScheduleOf = Convert.ToString(newstring.Split('~')[5]);
            string sPrintView = Convert.ToString(newstring.Split('~')[6]);
            string sDUPScheduleIDs = Convert.ToString(newstring.Split('~')[7]);
            bool bUptoLoadDyeMachine = Convert.ToBoolean(newstring.Split('~')[8]);
            int buid = Convert.ToInt32(newstring.Split('~')[9]);
            int nMachineType = Convert.ToInt32(newstring.Split('~')[10]);
            string sMachineIDs = newstring.Split('~')[11];

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            string sTemp = "";
            int nMaxValue = 0;
            DateTime startTime = DateTime.Now, endTime = DateTime.Now, today = DateTime.Now;
            startTime = new DateTime(StartSearchTime.Year, StartSearchTime.Month, StartSearchTime.Day, 0, 0, 0);
            endTime = new DateTime(StartSearchTime.Year, StartSearchTime.Month, StartSearchTime.Day, 23, 59, 59);
            endTime = endTime.AddDays(1);
            try
            {
                _sSQL = "SELECT * FROM View_Location where LocationID in (Select LocationID from WorkingUnit where BUID=" + buid + ")";
                //_sSQL = "SELECT * FROM Location where LocationID in(SELECT Distinct LocationID FROM VIEW_CapitalResource where ISNULL(IsActive,1)=1 AND BUID = " + buid + " AND ResourcesType = " + (int)EnumResourcesType.Machine + " )";
                oLocations = Location.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
                #region Machine

                string sSQL = "SELECT * FROM Machine WHERE  Activity=1 and BUID=" + buid;
                    if (!string.IsNullOrEmpty(sMachineIDs))
                        sSQL += " AND MachineID IN (" + sMachineIDs + ")";
                    else if (nMachineType > 0)
                        sSQL += " AND MachineID IN (SELECT MachineID FROM Machine WHERE MachineTypeID=" + nMachineType + ")";
                    else
                        sSQL += " AND MachineTypeID IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID =" + (int)EnumModuleName.DUPSchedule + ")";
                    sSQL += " ORDER BY SequenceNo";
                    oMachines = Machine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
          
                #endregion

                sLocationIDs = Location.IDInString(oLocations);
                if (bUptoLoadDyeMachine)
                {
                    _sSQL = "Select * from View_DUPSchedule where  MachineID IN (" + string.Join(",", oMachines.Select(x => x.MachineID)) + ")  and  LocationID in(" + sLocationIDs + ") and ScheduleStatus in (" + (int)EnumProductionScheduleStatus.Publish + "," + (int)EnumProductionScheduleStatus.Running + "," + (int)EnumProductionScheduleStatus.Urgent + ") order by MachineID,StartTime ";
                    _oDUPSchedule.DUPSchedules = DUPSchedule.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (_oDUPSchedule.DUPSchedules.Count > 0)
                    {
                        _sSQL = "Select * from View_DUPScheduleDetail Where  DUPScheduleID IN (Select DUPScheduleID from View_DUPSchedule where MachineID IN (" + string.Join(",", oMachines.Select(x => x.MachineID)) + ") and LocationID in(" + sLocationIDs + ") and  ScheduleStatus in (" + (int)EnumProductionScheduleStatus.Publish + "," + (int)EnumProductionScheduleStatus.Running + "," + (int)EnumProductionScheduleStatus.Urgent + "))";  // RSState=7  UnloadFromDyemachine
                        _oDUPSchedule.DUPScheduleDetails = DUPScheduleDetail.GetsSqL(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
                else
                {
                    _sSQL = "Select * from View_DUPSchedule where  LocationID in(" + sLocationIDs + ") " + (nMachineType <= 0 ? "" : " and MachineID IN (SELECT MachineID FROM Machine WHERE MachineTypeID=" + nMachineType + ") ") + " and ((StartTime>='" + startTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')) order by MachineID,StartTime ";
                    _oDUPSchedule.DUPSchedules = DUPSchedule.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (_oDUPSchedule.DUPSchedules.Count > 0)
                    {
                        _sSQL = "Select * from View_DUPScheduleDetail Where  DUPScheduleID IN (Select DUPScheduleID from View_DUPSchedule where LocationID in(" + sLocationIDs + ")  " + (nMachineType <= 0 ? "" : " and MachineID IN (" + string.Join(",", oMachines.Select(x => x.MachineID)) + ") ") + "  and ((StartTime>='" + startTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')))";  // RSState=7  UnloadFromDyemachine
                        _oDUPSchedule.DUPScheduleDetails = DUPScheduleDetail.GetsSqL(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }

                #region Machine
                //if (bUptoLoadDyeMachine)
                //{
                //    _sSQL = "SELECT * FROM Machine WHERE BUID=" + buid;

                //    if (_oDUPSchedule.DUPSchedules.Count > 0)
                //    {
                //        _sSQL += " AND MachineID IN (" + string.Join(",", _oDUPSchedule.DUPSchedules.Select(x => x.MachineID)) + ")";
                //    }
                //    _sSQL += " ORDER BY SequenceNo";
                //    oMachines = Machine.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                  
                //}
                #endregion

                foreach (Machine oItem in oMachines)
                {
                    nCount = (from oPS in _oDUPSchedule.DUPSchedules where oPS.MachineID == oItem.MachineID && oPS.BUID == oItem.BUID select oPS).Count();
                    oItem.MachineNoWithCapacityAndTotalSchedule = "[" + oItem.Code + "]" + oItem.Name + "[" + ((oItem.Capacity > 0) ? oItem.Capacity.ToString() : "") + "" + (!string.IsNullOrEmpty(oItem.Capacity2) ? oItem.Capacity2.ToString() : "") + "]" + " (" + Convert.ToInt32(nCount) + ")";
                }

                 sTemp = string.Join(",", _oDUPSchedule.DUPScheduleDetails.Select(x => x.DUPScheduleID).Distinct().ToList());
                 if (!string.IsNullOrEmpty(sTemp))
                 {
                    _sSQL = "SELECT * FROM View_DUPSLot WHERE DUPScheduleID in (" + sTemp+")";
                    oDUPSLots = DUPSLot.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    _oDUPSchedule.DUPScheduleDetails.ForEach(x =>
                    {
                        if (oDUPSLots.FirstOrDefault() != null && oDUPSLots.FirstOrDefault().DUPScheduleID > 0 && oDUPSLots.Where(b => (b.DUPScheduleID == x.DUPScheduleID && b.DUPScheduleDetailID == x.DUPScheduleDetailID && b.DODID == x.DODID)).Count() > 0)
                        {
                            x.BuyerRef = string.Join("+", oDUPSLots.Where(p => (p.DUPScheduleID == x.DUPScheduleID) && p.DUPScheduleDetailID == x.DUPScheduleDetailID && p.DODID == x.DODID && p.DODID > 0).Select(m => m.LotNo).Distinct().ToList());
                            //x.BuyerRef = oDUPSLots.Where(p => (p.DUPScheduleID == x.DUPScheduleID) &&  p.DUPScheduleDetailID == x.DUPScheduleDetailID && p.DODID == x.DODID && p.DODID > 0).FirstOrDefault().LotNo;
                        }
                    });
                 }
                sTemp = string.Join(",", _oDUPSchedule.DUPScheduleDetails.Select(x => x.DODID).Distinct().ToList());
                if (!string.IsNullOrEmpty(sTemp))
                {
                 
                    //_sSQL = "Select * from View_LoTParent where DyeingOrderID in (Select DyeingOrderID from DyeingOrderDetail where DyeingOrderDetailID in (" + sTemp + "))";
                    //oLotParents = LotParent.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _sSQL = "Select FLA.LotID,LotNo,DyeingOrderID,DOF.ProductID,DOF.FEOSDID,DOF.FEOSID,DOF.FEOSDID,DOF.DyeingOrderDetailID from FabricLotAssign as FLA left join DyeingOrderFabricDetail as DOF ON FLA.FEOSDID =DOF.FEOSDID left join Lot as Lt ON FLA.LotID =Lt.LotID where isnull(FLA.Qty,0)>0 and DyeingOrderID in (Select DyeingOrderID from DyeingOrderDetail where DyeingOrderDetailID in (" + sTemp + "))";
                    oFabricLotAssigns = FabricLotAssign.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                #region ORDER INFO
               
                #endregion

                if (_oDUPSchedule.DUPSchedules.Count() > 0)
                {
                    nMaxValue = _oDUPSchedule.DUPSchedules.GroupBy(x => x.MachineID).Max(t => t.Count());
                }
            }
            catch (Exception ex)
            {
                _oDUPSchedule = new DUPSchedule();
                _oDUPSchedule.ErrorMessage = ex.Message;
            }

      

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
          
            byte[] abytes;
            rptDUPSchedule oReport = new rptDUPSchedule();
            oReport.DUPSLots = oDUPSLots;
            oReport.LotParents = oLotParents;
            oReport.FabricLotAssigns = oFabricLotAssigns;

            abytes = oReport.PrepareReport(oMachines, oRouteSheetSetup, _oDUPSchedule, oCompany, nMaxValue);
            return File(abytes, "application/pdf");
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

        #region adv search
        public ActionResult ViewDUPScheduleReport(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            string sLocationIDs = "";
            int nCount = 0;
            List<Location> oLocations = new List<Location>();
            List<Machine> oMachines = new List<Machine>();

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            _sSQL = "SELECT * FROM View_Location where LocationID in (Select LocationID from WorkingUnit where BUID=" + buid + ")";
            oLocations = Location.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oMachines = Machine.GetsByModule(buid, ((int)EnumModuleName.DUPSchedule).ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            sLocationIDs = Location.IDInString(oLocations);

            DateTime startOfMonth = DateTime.Today;// new DateTime(today.Year, today.Month, 1);
            //DateTime endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            //DateTime startOfMonth = oRouteSheetSetup.BatchTime;
            DateTime endOfMonth = startOfMonth.AddDays(1);
            _sSQL = "Select * from View_DUPSchedule where LocationID in (" + sLocationIDs + ") and ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')) order by MachineID,StartTime ";
            _oDUPSchedule.DUPSchedules = DUPSchedule.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oDUPSchedule.DUPSchedules.Count > 0)
            {
                _sSQL = "Select * from View_DUPScheduleDetail Where DUPScheduleID IN (Select DUPScheduleID from View_DUPSchedule where LocationID in (" + sLocationIDs + ") and ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') ) )";  // RSState=7  UnloadFromDyemachine
                _oDUPSchedule.DUPScheduleDetails = DUPScheduleDetail.GetsSqL(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }


            foreach (Machine oItem in oMachines)
            {
                nCount = (from oPS in _oDUPSchedule.DUPSchedules where oPS.MachineID == oItem.MachineID && oPS.BUID == oItem.BUID select oPS).Count();
                oItem.MachineNoWithCapacityAndTotalSchedule = "[" + oItem.Code + "]" + oItem.Name + "[" + ((oItem.Capacity > 0) ? oItem.Capacity.ToString() : "") + "" + (!string.IsNullOrEmpty(oItem.Capacity2) ? oItem.Capacity2.ToString() : "") + "]" + " (" + Convert.ToInt32(nCount) + ")";
            }

          
            ViewBag.OrderType = EnumObject.jGets(typeof(EnumOrderType));
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUOrderSetups = oDUOrderSetups;
            ViewBag.DUPSchedules = _oDUPSchedule.DUPSchedules;
            ViewBag.CapitalResources = oMachines;
            ViewBag.Locations = oLocations;

            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            #region MachineType
            List<MachineType> oMachineTypes = new List<MachineType>();

            string sSQL = "SELECT * FROM MachineType WHERE BUID=" + buid + " AND MachineTypeID IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID =" + (int)EnumModuleName.DUPSchedule + ")";

            oMachineTypes = MachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            ViewBag.MachineTypes = oMachineTypes;
            ViewBag.Buid = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.ProductionScheduleStatus = EnumObject.jGets(typeof(EnumProductionScheduleStatus));
            return View(_oDUPSchedule);
        }

        public ActionResult AdvDUPSchedule()
        {
            return PartialView();
        }
        public JsonResult AdvSearchDUPS(string sTemp)
        {

            var tuple = new Tuple<List<DUPScheduleDetail>, List<DUPScheduleDetail>, List<DUPScheduleDetail>, List<DUPScheduleDetail>>(new List<DUPScheduleDetail>(), new List<DUPScheduleDetail>(), new List<DUPScheduleDetail>(), new List<DUPScheduleDetail>());
            try
            {
                string sSQL = GetSQL(sTemp);
                var oDUPS = DUPScheduleDetail.GetsSqL(sSQL, (int)Session[SessionInfo.currentUserID]);

                List<DUPScheduleDetail> oBuyerWIse = new List<DUPScheduleDetail>();

                oBuyerWIse = oDUPS.GroupBy(x => new { x.ContractorName }, (key, grp) =>
                    new DUPScheduleDetail
                    {
                        ContractorName = key.ContractorName,
                        Qty = grp.Sum(p => p.Qty)

                    }).ToList();

                List<DUPScheduleDetail> oOrderWise = new List<DUPScheduleDetail>();

                oOrderWise = oDUPS.GroupBy(x => new { x.OrderNo }, (key, grp) =>
                new DUPScheduleDetail
                {
                    OrderNo = key.OrderNo,
                    Qty = grp.Sum(p => p.Qty)
                }).ToList();

                List<DUPScheduleDetail> oYarnWise = new List<DUPScheduleDetail>();

                oYarnWise = oDUPS.GroupBy(x => new { x.ProductName }, (key, grp) =>
                new DUPScheduleDetail
                {
                    ProductName = key.ProductName,
                    Qty = grp.Sum(p => p.Qty)
                }).ToList();
                tuple = new Tuple<List<DUPScheduleDetail>, List<DUPScheduleDetail>, List<DUPScheduleDetail>, List<DUPScheduleDetail>>(oDUPS, oBuyerWIse, oOrderWise, oYarnWise);

            }
            catch (Exception e)
            {
                //tuple.Item1; //total list List
                //tuple.Item2;//GroupByContactPersonnel List
                //tuple.Item3;
                tuple = new Tuple<List<DUPScheduleDetail>, List<DUPScheduleDetail>, List<DUPScheduleDetail>, List<DUPScheduleDetail>>(new List<DUPScheduleDetail>(), new List<DUPScheduleDetail>(), new List<DUPScheduleDetail>(), new List<DUPScheduleDetail>());
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = sjson = serializer.Serialize(tuple);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }

        private string GetSQL(string sTemp)
        {
            //PI Date
            int cboDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime StartTime = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime EndTime = Convert.ToDateTime(sTemp.Split('~')[2]);
            string LocationName = Convert.ToString(sTemp.Split('~')[3]);
            string MachineIds = Convert.ToString(sTemp.Split('~')[4]);
            string DODID = Convert.ToString(sTemp.Split('~')[5]);
            int MachineTypeID = Convert.ToInt16(sTemp.Split('~')[6]);
            string ScheduleStatus = sTemp.Split('~')[7];
            string sRSStatus = "";
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            string sReturn1 = "SELECT * FROM View_DUPScheduleDetail";
            string sReturn = "";


            if (cboDate > 0)
            {
                if (cboDate == 1)
                {
                    EndTime = StartTime.AddDays(1);

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "(StartTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')";
                    // sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),StartTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + StartTime.ToString("dd MMM yyyy") + "',106))";
                }
                if (cboDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " ((StartTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")+"'))" );
                    sReturn = sReturn + "(StartTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')";
                }
            }

            if (!string.IsNullOrEmpty(LocationName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LocationID = " + LocationName;

            }

            if (!string.IsNullOrEmpty(ScheduleStatus))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DUPScheduleID IN (SELECT DUPScheduleID FROM DUPSchedule WHERE ScheduleStatus IN (" + ScheduleStatus + "))";

            }
            if (!string.IsNullOrEmpty(sRSStatus))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DUPScheduleID IN (SELECT DUPScheduleID FROM Routesheet WHERE DUPScheduleID>0 and RoutesheetID in (select RoutesheetID from RouteSheetHistory where CurrentStatus in (1) and EventTime>='1 jan 2016' and EventTime<'1 Sep 2018' ))";

            }
            if (!string.IsNullOrEmpty(MachineIds))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MachineID IN ( " + MachineIds + ")";

            }
            else if (MachineTypeID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  MachineID IN (SELECT MachineID FROM Machine WHERE MachineTypeID=" + MachineTypeID + ")";

            }

            if (!string.IsNullOrEmpty(DODID))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DODID IN(" + DODID + ")";
            }
            sReturn = sReturn1 + sReturn + " order by MachineID,StartTime";
            return sReturn;
        }

        #endregion
        #region report

        public ActionResult Print_DUPSReport(string ids)
        {
            List<DUPScheduleDetail> oDUPSD = new List<DUPScheduleDetail>();
            string sSQL = "", sDateRange = "";
            try
            {
                if (ids.Trim() != "")
                {
                    sSQL = "Select * from  View_DUPScheduleDetail where DUPScheduleDetailID IN (" + ids + ") Order By StartTime DESC";
                    oDUPSD = DUPScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oDUPSD.Count() > 0)
                    {
                        oDUPSD = oDUPSD.OrderBy(x => x.MachineName).ToList();
                        if (oDUPSD.Min(x => x.StartTime).ToString("dd MMM yyyy") == oDUPSD.Max(x => x.StartTime).ToString("dd MMM yyyy")) { sDateRange = oDUPSD.Min(x => x.StartTime).ToString("dd MMM yyyy"); }
                        else { sDateRange = oDUPSD.Min(x => x.StartTime).ToString("dd MMM yyyy") + " to " + oDUPSD.Max(x => x.StartTime).ToString("dd MMM yyyy"); }
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptDUPscheduleReport oReport = new rptDUPscheduleReport();
            byte[] abytes = oReport.PrepareReport(oDUPSD, oCompany, sDateRange);
            return File(abytes, "application/pdf");

        }

        #endregion
        #region Print Schedule From Report
        public ActionResult PrintDUPSchedule_FR(string sTemp, int buid)
        {
            #region Declaration

            string sLocationIDs = "", sDateRange = "";
            int nCount = 0;

            DUPSchedule oDUPSchedule = new DUPSchedule();
            List<Location> oLocations = new List<Location>();
            List<Machine> oMachines = new List<Machine>();
            List<DUPSLot> oDUPSLots = new List<DUPSLot>();

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            #endregion

            try
            {
                string sSQL = "";
                if (sTemp.Trim() != "")
                {
                    sSQL = GetSQL(sTemp);
                    oDUPSchedule.DUPScheduleDetails = DUPScheduleDetail.GetsSqL(sSQL, (int)Session[SessionInfo.currentUserID]);

                    if (oDUPSchedule.DUPScheduleDetails.Count > 0)
                    {
                        sSQL = "Select * from View_DUPSchedule Where DUPScheduleID IN (" + string.Join(",", oDUPSchedule.DUPScheduleDetails.Select(x => x.DUPScheduleID)) + ") ";  // RSState=7  UnloadFromDyemachine
                        oDUPSchedule.DUPSchedules = DUPSchedule.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }

                    oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                    sSQL = "SELECT * FROM View_Location where LocationID in (Select LocationID from WorkingUnit where BUID=" + buid + ")";
                    oLocations = Location.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    #region Machine
                    sSQL = "SELECT * FROM Machine WHERE BUID=" + buid;
                    sSQL += " AND MachineTypeID IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID =" + (int)EnumModuleName.DUPSchedule + ")";
                    sSQL += " ORDER BY SequenceNo";
                    oMachines = Machine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    #endregion

                    #region Get DUPSchedules
                    sLocationIDs = Location.IDInString(oLocations);

                    foreach (Machine oItem in oMachines)
                    {
                        nCount = (from oPS in oDUPSchedule.DUPSchedules where oPS.MachineID == oItem.MachineID && oPS.BUID == oItem.BUID select oPS).Count();
                        oItem.MachineNoWithCapacityAndTotalSchedule = "[" + oItem.Code + "]" + oItem.Name + "[" + ((oItem.Capacity > 0) ? oItem.Capacity.ToString() : "") + "" + (!string.IsNullOrEmpty(oItem.Capacity2) ? oItem.Capacity2.ToString() : "") + "]" + " (" + Convert.ToInt32(nCount) + ")";
                    }

                    #endregion

                    //sSQL = string.Join(",", oDUPSchedule.DUPSchedules.Select(x => x.DUPScheduleID).Distinct().ToList());
                    //if (!string.IsNullOrEmpty(sSQL))
                    //{
                    //    sSQL = "SELECT * FROM View_DUPSLot where DUPScheduleID in (" + sSQL + ")";
                    //    oDUPSLots = DUPSLot.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //}

                }
            }
            catch (Exception ex)
            {
                byte[] error_abytes;
                rptErrorMessage oErrorReport = new rptErrorMessage();
                error_abytes = oErrorReport.PrepareReport("Failed to Print. For " + ex.Message);
                return File(error_abytes, "application/pdf");
            }

            int nMaxValue = 0;
           
            #region Parse DUPSchedules
            foreach (DUPSchedule oItem in oDUPSchedule.DUPSchedules)
            {
                oItem.DUPScheduleDetails = new List<DUPScheduleDetail>();
                oItem.DUPScheduleDetails = oDUPSchedule.DUPScheduleDetails.Where(x => x.DUPScheduleID == oItem.DUPScheduleID).ToList();
                sDateRange = oItem.StartTime.ToString("dd MMM yy") + "(" + oItem.StartTime.ToString("HH:mm tt") + "-" + oItem.EndTime.ToString("HH:mm tt") + ")";
                nCount = 0;
                sTemp = "";
                if (oItem.DUPScheduleDetails.Count == 0 || oItem.DUPScheduleDetails == null) { oItem.OrderInfo = oItem.ScheduleNo + " " + sDateRange; }
                else
                {
                   // oItem.LotNo = string.Join("+", oDUPSLots.Where(x => x.DUPScheduleID == oItem.DUPScheduleID).Select(x => x.LotNo + " (" + Global.MillionFormat(x.Qty) + ")").Distinct().ToList());
                    if (oRouteSheetSetup.IsShowBuyer)
                    { oItem.OrderInfo = sDateRange + "\n" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.BuyerName).Distinct().ToList()); }
                    else { oItem.OrderInfo = sDateRange + "\n" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ContractorName).Distinct().ToList()); }

                    oItem.OrderInfo = oItem.OrderInfo + "\n" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.OrderNo).Distinct().ToList());
                    oItem.OrderInfo = oItem.OrderInfo + "\n" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ProductName).Distinct().ToList());
                    if (string.IsNullOrEmpty(oItem.LotNo) && oItem.DUPScheduleDetails[0].IsInHouse == false)
                    { oItem.OrderInfo = oItem.OrderInfo + "</br> Lot: " + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.BuyerRef).ToList()); }
                    
                    oItem.OrderInfo = oItem.OrderInfo + "\n" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ColorName).Distinct().ToList());

                    sTemp = string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ColorNo).Distinct().ToList());
                    if (!string.IsNullOrEmpty(sTemp))
                    {
                        oItem.OrderInfo = oItem.OrderInfo + "\n Color No: " + sTemp;
                    }
                    sTemp = string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ApproveLotNo).Distinct().ToList());
                    if (!string.IsNullOrEmpty(sTemp))
                    {
                        oItem.OrderInfo = oItem.OrderInfo + " Match To: " + sTemp;
                    }

                    oItem.OrderInfo = oItem.OrderInfo + "\n" + string.Join("+", oItem.DUPScheduleDetails.Select(x => Global.MillionFormat(x.Qty) + " " + oRouteSheetSetup.MUnit + ((x.BagCount <= 0) ? "" : " (" + x.BagCount.ToString() + " " + x.HankorConeST + ")")).ToList());
                    if (!string.IsNullOrEmpty(oRouteSheetSetup.BatchCode))
                    {
                        oItem.OrderInfo = oItem.OrderInfo + "\n" + string.Join("+", oItem.DUPScheduleDetails.Select(x => oRouteSheetSetup.BatchCode + "-" + x.PSBatchNo).ToList());
                    }
                    if (!String.IsNullOrEmpty(oItem.Note))
                    {
                        oItem.OrderInfo = oItem.OrderInfo + "\n" + oItem.Note;
                    }
                    if (!String.IsNullOrEmpty(oItem.DUPScheduleDetails[0].RouteSheetNo))
                    {
                        oItem.OrderInfo = oItem.OrderInfo + "\nBatch No: " + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.RouteSheetNo).Distinct().ToList());
                    }
                }
            }
            #endregion
            /// Get Max Order In One Machain
            if (oDUPSchedule.DUPSchedules.Count() > 0)
            {
                nMaxValue = oDUPSchedule.DUPSchedules.GroupBy(x => x.MachineID).Max(t => t.Count());
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if(oDUPSchedule.DUPSchedules.Count() <= 0) 
            {
                byte[] abytes_error;
                rptErrorMessage oReport_Error = new rptErrorMessage();
                abytes_error = oReport_Error.PrepareReport("No Data Found!");
                return File(abytes_error, "application/pdf");
            }

            byte[] abytes;
            rptDUPSchedule oReport = new rptDUPSchedule();
            abytes = oReport.PrepareReport(oMachines, oRouteSheetSetup, oDUPSchedule, oCompany, nMaxValue);
            return File(abytes, "application/pdf");
        }
        #endregion
        #region SNED TO PRODUCTION --> DUPS

        public ActionResult ViewDyeingSchedule(int buid, int nID)
        {
            DyeingOrder oDyeingOrder = new DyeingOrder();
            List<Location> oLocations = new List<Location>();
            List<Machine> oMachines = new List<Machine>();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

           _sSQL = "SELECT * FROM View_Location where LocationID in (Select LocationID from WorkingUnit where BUID=" + buid + ")";
            oLocations = Location.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oMachines = Machine.GetsByModule(buid, ((int)EnumModuleName.DUPSchedule).ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sLocationIDs = Location.IDInString(oLocations);

            DateTime startOfMonth = DateTime.Today;
            DateTime endOfMonth = startOfMonth.AddDays(1);
            //_sSQL = "Select * from View_DUPSchedule where LocationID in (" + sLocationIDs + ") and ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')) order by MachineID,StartTime ";
            //_sSQL = "SELECT DD.* FROM View_DUPScheduleDetail AS DD WHERE DD.DUPScheduleID IN (SELECT DUPScheduleID FROM DUPScheduleDetail WHERE DODID IN (SELECT DyeingOrderDetailID FROM DyeingOrderDetail WHERE DyeingOrderID=" + nID + "))";
            //_oDUPSchedule.DUPScheduleDetails = DUPScheduleDetail.GetsSqL(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oDyeingOrder = DyeingOrder.Get(nID, (int)Session[SessionInfo.currentUserID]);

            _sSQL = "SELECT DD.* FROM View_DUPSchedule AS DD WHERE DD.DUPScheduleID IN (SELECT DUPScheduleID FROM DUPScheduleDetail WHERE DODID IN (SELECT DyeingOrderDetailID FROM DyeingOrderDetail WHERE DyeingOrderID=" + nID + "))";
            _oDUPSchedules = DUPSchedule.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Issue Stores
            List<WorkingUnit> oStores = new List<WorkingUnit>();
            oStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.DUPSchedule, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

            #region MachineType
            List<MachineType> oMachineTypes = new List<MachineType>();
            string sSQL = "SELECT * FROM MachineType WHERE BUID=" + buid + " AND MachineTypeID IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID =" + (int)EnumModuleName.DUPSchedule + ")";
            oMachineTypes = MachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            ViewBag.DyeingOrder = oDyeingOrder;
            ViewBag.WorkingUnits = oStores;
            ViewBag.MachineTypes = oMachineTypes;
            ViewBag.OrderType = EnumObject.jGets(typeof(EnumOrderType));
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUOrderSetups = oDUOrderSetups;
            //ViewBag.DUPSchedules = _oDUPSchedule.DUPSchedules;
            ViewBag.CapitalResources = oMachines;
            ViewBag.Locations = oLocations;

            ViewBag.Buid = buid;
            //ViewBag.BusinessUnits = oBusinessUnits;

            return View(_oDUPSchedules);
        }
        #endregion
        #region Actions DUPSLot

        DUPSLot _oDUPSLot = new DUPSLot();
        List<DUPSLot> _oDUPSLots = new List<DUPSLot>();
       
        [HttpPost]
        public JsonResult Save_Lot(DUPSLot oDUPSLot)
        {
            _oDUPSLot = new DUPSLot();
            try
            {
                _oDUPSLot = oDUPSLot;
                _oDUPSLot = _oDUPSLot.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUPSLot = new DUPSLot();
                _oDUPSLot.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUPSLot);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete_Lot(DUPSLot oDUPSLot)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oDUPSLot.Delete(oDUPSLot.DUPSLotID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region  M/C STOPAGE REPORT Daily Yarn Dyeing Production Report
        public ActionResult ViewDUPScheduleReportRS(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<Location> oLocations = new List<Location>();
            string sSQL = "SELECT * FROM View_Location where ParentID !=0 and IsActive =1 and LocationID in (Select LocationID from view_WorkingUnit where IsActive=1 and BUID='" + buid + "')";
            oLocations = Location.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<Machine> oMachines = new List<Machine>();

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            DateTime startOfMonth = DateTime.Now; // new DateTime(2018, 09,21);
            DateTime endOfMonth = startOfMonth.AddDays(1);

            #region Get Report For CurrentDate
            //string sSearchString = " WHERE RoutesheetID IN (select RoutesheetID from RouteSheetHistory where CurrentStatus in (" + (int)EnumRSState.UnloadedFromDyeMachine + ") and EventTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "'   and EventTime<'" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')";
            string sSearchString = "where RoutesheetID in (select RoutesheetID from RouteSheetHistory where CurrentStatus in (" + (int)EnumRSState.UnloadedFromDyeMachine + ") and EventTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "'   and EventTime<'" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' )";
            var oDUPS = DUPScheduleDetail.Gets_RS(sSearchString, (int)Session[SessionInfo.currentUserID]);

            List<DUPScheduleDetail> oBuyerWIse = new List<DUPScheduleDetail>();

            oBuyerWIse = oDUPS.GroupBy(x => new { x.ContractorName }, (key, grp) =>
                new DUPScheduleDetail
                {
                    ContractorName = key.ContractorName,
                    Qty = grp.Sum(p => p.Qty)

                }).ToList();

            List<DUPScheduleDetail> oOrderWise = new List<DUPScheduleDetail>();

            oOrderWise = oDUPS.GroupBy(x => new { x.OrderNo, x.ContractorName }, (key, grp) =>
            new DUPScheduleDetail
            {
                OrderNo = key.OrderNo,
                ContractorName = key.ContractorName,
                Qty = grp.Sum(p => p.Qty)
            }).ToList();

            List<DUPScheduleDetail> oYarnWise = new List<DUPScheduleDetail>();

            oYarnWise = oDUPS.GroupBy(x => new { x.ProductName }, (key, grp) =>
            new DUPScheduleDetail
            {
                ProductName = key.ProductName,
                Qty = grp.Sum(p => p.Qty)
            }).ToList();

            ViewBag.DUPS = oDUPS;
            ViewBag.DUPS_BuyerWise = oBuyerWIse;
            ViewBag.DUPS_OrderWise = oYarnWise;
            #endregion

            ViewBag.OrderType = EnumObject.jGets(typeof(EnumOrderType));
            ViewBag.Locations = oLocations;
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            #region MachineType
            List<MachineType> oMachineTypes = new List<MachineType>();

            sSQL = "SELECT * FROM MachineType WHERE BUID=" + buid + " AND MachineTypeID IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID =" + (int)EnumModuleName.DUPSchedule + ")";

            oMachineTypes = MachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            ViewBag.MachineTypes = oMachineTypes;
            ViewBag.Buid = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.RSStates = EnumObject.jGets(typeof(EnumRSState));
            ViewBag.ProductionScheduleStatus = EnumObject.jGets(typeof(EnumProductionScheduleStatus));
            ViewBag.InOutTypes = EnumObject.jGets(typeof(EnumInOutType)).Where(x=>x.id!= (int)EnumInOutType.None).OrderBy(x=>x.id);
            ViewBag.RSShifts = RSShift.GetsByModule(buid, ""+(int)EnumModuleName.RouteSheet, ((User)Session[SessionInfo.CurrentUser]).UserID); 
            return View(_oDUPSchedule);
        }
        public ActionResult AdvDUPScheduleRS()
        {
            return PartialView();
        }
        public JsonResult AdvSearchDUPSRS(string sTemp)
        {
            var tuple = new Tuple<List<DUPScheduleDetail>, List<DUPScheduleDetail>, List<DUPScheduleDetail>, List<DUPScheduleDetail>>(new List<DUPScheduleDetail>(), new List<DUPScheduleDetail>(), new List<DUPScheduleDetail>(), new List<DUPScheduleDetail>());
            try
            {
                string sSQL = GetSQLTwo(sTemp);
                var oDUPS = DUPScheduleDetail.Gets_RS(sSQL, (int)Session[SessionInfo.currentUserID]);

                List<DUPScheduleDetail> oBuyerWIse = new List<DUPScheduleDetail>();

                oBuyerWIse = oDUPS.GroupBy(x => new { x.ContractorName }, (key, grp) =>
                    new DUPScheduleDetail
                    {
                        ContractorName = key.ContractorName,
                        Qty = grp.Sum(p => p.Qty)

                    }).ToList();

                List<DUPScheduleDetail> oOrderWise = new List<DUPScheduleDetail>();

                oOrderWise = oDUPS.GroupBy(x => new { x.OrderNo,x.ContractorName }, (key, grp) =>
                new DUPScheduleDetail
                {
                    OrderNo = key.OrderNo,
                    ContractorName = key.ContractorName,
                    Qty = grp.Sum(p => p.Qty)
                }).ToList();

                List<DUPScheduleDetail> oYarnWise = new List<DUPScheduleDetail>();

                oYarnWise = oDUPS.GroupBy(x => new { x.ProductName }, (key, grp) =>
                new DUPScheduleDetail
                {
                    ProductName = key.ProductName,
                    Qty = grp.Sum(p => p.Qty)
                }).ToList();
                tuple = new Tuple<List<DUPScheduleDetail>, List<DUPScheduleDetail>, List<DUPScheduleDetail>, List<DUPScheduleDetail>>(oDUPS, oBuyerWIse, oOrderWise, oYarnWise);

            }
            catch (Exception e)
            {
                //tuple.Item1; //total list List
                //tuple.Item2;//GroupByContactPersonnel List
                //tuple.Item3;
                tuple = new Tuple<List<DUPScheduleDetail>, List<DUPScheduleDetail>, List<DUPScheduleDetail>, List<DUPScheduleDetail>>(new List<DUPScheduleDetail>(), new List<DUPScheduleDetail>(), new List<DUPScheduleDetail>(), new List<DUPScheduleDetail>());
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = sjson = serializer.Serialize(tuple);
            //return Json(sjson, JsonRequestBehavior.AllowGet);

            var jsonResult = Json(tuple, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string GetSQLTwo(string sTemp)
        {
            //PI Date
            int cboDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime StartTime = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime EndTime = Convert.ToDateTime(sTemp.Split('~')[2]);
            int nLocationID = Convert.ToInt32(sTemp.Split('~')[3]);
            string MachineIds = Convert.ToString(sTemp.Split('~')[4]);
            string DODID = Convert.ToString(sTemp.Split('~')[5]);
            int MachineTypeID = Convert.ToInt16(sTemp.Split('~')[6]);
            string ScheduleStatus = sTemp.Split('~')[7];
            string sRSStatus = sTemp.Split('~')[8];
            int cboDate_RS = Convert.ToInt32(sTemp.Split('~')[9]);
            DateTime StartTime_RS = Convert.ToDateTime(sTemp.Split('~')[10]);
            DateTime EndTime_RS = Convert.ToDateTime(sTemp.Split('~')[11]);
            int nRSSHiftID = Convert.ToInt16(sTemp.Split('~')[12]);
            int nInOutType = Convert.ToInt16(sTemp.Split('~')[13]);

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            string sReturn1 = "";
            string sReturn = "";


            if (cboDate > 0)
            {
                if (cboDate == 1)
                {
                    EndTime = StartTime.AddDays(1);
                    sDateRange = "Date: " + StartTime.ToString("dd MMM yyyy") ; 
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheetHistory where CurrentStatus in (" + (int)EnumRSState.UnloadedFromDyeMachine + ") and EventTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:00")) + "'   and EventTime<'" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:00")) + "')";
                    // sReturn = sReturn + "(StartTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')";
                    // sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),StartTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + StartTime.ToString("dd MMM yyyy") + "',106))";
                    sDateRange =  EndTime.ToString("dd MMM yyyy") ; 
                }
                if (cboDate == 5)
                {
                    sDateRange = "Date: "+StartTime.ToString("dd MMM yyyy") + " to " + EndTime.ToString("dd MMM yyyy"); 
                    EndTime = EndTime.AddDays(1);
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheetHistory where CurrentStatus in (" + (int)EnumRSState.UnloadedFromDyeMachine + ") and EventTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:00")) + "'   and EventTime<'" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:00")) + "' )";
                    //sReturn = sReturn + " ((StartTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")+"'))" );
                    //  sReturn = sReturn + "(StartTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + StartTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + EndTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM"))+"')";
                   
                }
            }

            if (!string.IsNullOrEmpty(ScheduleStatus))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  RouteSheetID in (Select RouteSheetID from Routesheet where DUPScheduleID>0 and DUPScheduleID in(SELECT DUPScheduleID FROM DUPSchedule WHERE ScheduleStatus IN (" + ScheduleStatus + ")) )";
                // sReturn = sReturn + " DUPScheduleID IN (SELECT DUPScheduleID FROM DUPSchedule WHERE ScheduleStatus IN (" + ScheduleStatus + "))";
            }
            if (!string.IsNullOrEmpty(sRSStatus))
            {
                Global.TagSQL(ref sReturn);

                if (cboDate_RS == 1) { EndTime_RS = StartTime_RS.AddDays(1); sDateRange ="Date "+ StartTime_RS.ToString("dd MMM yyyy") ; }
                else { sDateRange ="Date "+ StartTime_RS.ToString("dd MMM yyyy") + " to " + EndTime_RS.ToString("dd MMM yyyy"); EndTime_RS = EndTime_RS.AddDays(1); };
                if (cboDate_RS > 0)
                {
                    sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheetHistory where CurrentStatus in (" + sRSStatus + ") and EventTime>='" + StartTime_RS.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:00")) + "'   and EventTime<'" + EndTime_RS.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:00")) + "' )";
                }
                else { sReturn = sReturn + "RSState in (" + sRSStatus + ") "; }
                
            }
            if (!string.IsNullOrEmpty(MachineIds))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RoutesheetID in ( select RoutesheetID from RouteSheet where  MachineID IN ( " + MachineIds + "))";
            }
            else if (MachineTypeID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RoutesheetID in (select RoutesheetID from RouteSheet where  MachineID IN (SELECT MachineID FROM Machine WHERE MachineTypeID=" + MachineTypeID + "))";
            }
            if (nLocationID > 0)
            {
                Global.TagSQL(ref sReturn);
              //  sReturn = sReturn + " LocationID = " + nLocationID;
                sReturn = sReturn + "RoutesheetID in ( select RoutesheetID from RouteSheet where  LocationID =" + nLocationID + ")";
            }
            if (nRSSHiftID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RoutesheetID in ( select RoutesheetID from RouteSheet where  RSSHiftID =" + nRSSHiftID + ")";
            }
            if (nInOutType > 0)
            {
                nInOutType = (nInOutType == (int)EnumInOutType.Receive ? 1 : 0);

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RoutesheetID in ( SELECT RouteSheetID FROM RouteSheet WHERE OrderType IN (SELECT OrderType FROM DUOrderSetup WHERE IsInHouse =" + nInOutType + "))";
            }
            if (!string.IsNullOrEmpty(DODID))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderDetailID IN(" + DODID + ")";
            }

            Global.TagSQL(ref sReturn);
            sReturn = sReturn + "RoutesheetID in ( select RoutesheetID from RouteSheet where isnull(RouteSheet.IsReDyeing,0)=0 )";

            sReturn = sReturn1 + sReturn + "";
            return sReturn;
        }
        #endregion

        #region report RS
        public ActionResult Print_DUPSReport_RS(string sTemp, int buid)
        {
            List<DUPScheduleDetail> oDUPSD = new List<DUPScheduleDetail>();
            string sSQL = "",  sRSStates="";
            try
            {
                if (sTemp.Trim() != "")
                {
                    sSQL = GetSQLTwo(sTemp);
                    oDUPSD = DUPScheduleDetail.Gets_RS(sSQL, (int)Session[SessionInfo.currentUserID]);

                    if (oDUPSD.Count() > 0)
                    {
                        oDUPSD = oDUPSD.OrderBy(x => x.MachineName).ToList();

                        string sTemp_RSSate = sTemp.Split('~')[8];
                        if (!string.IsNullOrEmpty(sTemp_RSSate))
                        {
                            foreach (var sState in sTemp_RSSate.Split(','))
                                sRSStates += EnumObject.jGet((EnumRSState)Convert.ToInt16(sState)) + ", ";

                            sRSStates = sRSStates.Remove(sRSStates.LastIndexOf(","), 1);
                        }

                        //int cboDate = Convert.ToInt32(sTemp.Split('~')[0]);
                        //DateTime StartTime = Convert.ToDateTime(sTemp.Split('~')[1]);
                        //DateTime EndTime = Convert.ToDateTime(sTemp.Split('~')[2]);

                        //sDateRange = "";
                        //if (cboDate > 0) { sDateRange = (cboDate == 1 ? StartTime.ToString("dd MMM yyyy") : StartTime.ToString("dd MMM yyyy") + " to " + EndTime.ToString("dd MMM yyyy")); }

                        //if (oDUPSD.Min(x => x.StartTime).ToString("dd MMM yyyy") == oDUPSD.Max(x => x.StartTime).ToString("dd MMM yyyy")) { sDateRange = oDUPSD.Min(x => x.StartTime).ToString("dd MMM yyyy"); }
                        //else { sDateRange = oDUPSD.Min(x => x.StartTime).ToString("dd MMM yyyy") + " to " + oDUPSD.Max(x => x.StartTime).ToString("dd MMM yyyy"); }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();

            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptDUPscheduleReport_RS oReport = new rptDUPscheduleReport_RS();
            byte[] abytes = oReport.PrepareReport(oDUPSD, oCompany, sDateRange,sRSStates, oBusinessUnit);
            return File(abytes, "application/pdf");
        }

        public void Print_DUPS_RS_Excel(string sTemp, int buid)
        {
            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();

            List<DUPScheduleDetail> oDUPSD = new List<DUPScheduleDetail>();
            string sSQL = "",  sRSStates="";

            if (sTemp.Trim() != "")
            {
                sSQL = GetSQLTwo(sTemp);
                oDUPSD = DUPScheduleDetail.Gets_RS(sSQL, (int)Session[SessionInfo.currentUserID]);

                if (oDUPSD.Count() > 0)
                {
                    oDUPSD = oDUPSD.OrderBy(x => x.MachineName).ToList();

                    string sTemp_RSSate=sTemp.Split('~')[8];
                    if (!string.IsNullOrEmpty(sTemp_RSSate))
                    {
                        foreach (var sState in sTemp_RSSate.Split(','))
                            sRSStates += EnumObject.jGet((EnumRSState)Convert.ToInt16(sState))+", ";

                        sRSStates = sRSStates.Remove(sRSStates.LastIndexOf(","), 1);
                    }

                    int cboDate = Convert.ToInt32(sTemp.Split('~')[0]);
                    DateTime StartTime = Convert.ToDateTime(sTemp.Split('~')[1]);
                    DateTime EndTime = Convert.ToDateTime(sTemp.Split('~')[2]);

                    //sDateRange = "";
                    //if (cboDate > 0) { sDateRange = (cboDate == 1 ? EndTime.ToString("dd MMM yyyy") : EndTime.ToString("dd MMM yyyy") + " to " + EndTime.ToString("dd MMM yyyy")); }

                    //if (oDUPSD.Min(x => x.StartTime).ToString("dd MMM yyyy") == oDUPSD.Max(x => x.StartTime).ToString("dd MMM yyyy")) { sDateRange = oDUPSD.Min(x => x.StartTime).ToString("dd MMM yyyy"); }
                    //else { sDateRange = oDUPSD.Min(x => x.StartTime).ToString("dd MMM yyyy") + " to " + oDUPSD.Max(x => x.StartTime).ToString("dd MMM yyyy"); }
                }
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            if (oDUPSD.Count > 0)
            {
                #region Header
                //M/C No.	Buyer	 Order No	Batch No	Yarn Type	Color	Batch Qty(KG)	Batch Qty(KG)	From Stock	Remarks
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Left });
               
                table_header.Add(new TableHeader { Header = "Machine No", Width = 20f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Un Load Date", Width = 12f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Buyer Name", Width = 35f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Order No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "YP", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Batch No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Yarn Type", Width = 40f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Color", Width = 20f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Batch Qty(KG)", Width = 15f, IsRotate = false, Align = TextAlign.Right });
                table_header.Add(new TableHeader { Header = "Batch Qty(KG)", Width = 20f, IsRotate = false, Align = TextAlign.Right });
                //table_header.Add(new TableHeader { Header = "From Stock", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Remarks", Width = 25f, IsRotate = false, Align = TextAlign.Left });
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Daily Yarn Dyeing Production Report");
                    sheet.Name = "Daily Yarn Dyeing Production Report";


                    ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                    nEndCol = 13;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Daily Yarn Dyeing Production Report"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;
                    if (string.IsNullOrEmpty(sRSStates))
                    {
                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = ( string.IsNullOrEmpty(sRSStates) ? "" : "RS State: " + sRSStates); cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex++;
                    }

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = sDateRange; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;
                    #endregion

                    #region Data
                    nRowIndex++;
                    nStartCol = 2;
                    ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                    int nCount = 0; nEndCol = table_header.Count() + nStartCol;
                    oDUPSD.ForEach(o => o.EndTime = Convert.ToDateTime(o.EndTime.ToString("dd MMM yy")));
                    var data = oDUPSD.GroupBy(x => new {x.EndDateSt,x.EndTime, x.MachineID }, (key, grp) => new
                    {
                        EndDateSt = grp.Select(x => x.EndDateSt).First(),
                        EndTime = grp.Select(x => x.EndTime).First(),
                        HeaderName = grp.Select(x => x.MachineName).First(),
                        TotalBatchQty = grp.Sum(x => x.Qty),
                        Results = grp.OrderBy(x => x.RouteSheetID).ToList()
                    });
                    data = data.OrderBy(x => x.EndDateSt).ThenBy(x => x.HeaderName).ToList();
                    foreach (var oItem in data)
                    {
                        nCount++;
                        int nSpan = oItem.Results.Count() - 1;
                        nStartCol = 2;

                        ExcelTool.FillCellMerge(ref sheet, (nCount).ToString(), nRowIndex, nRowIndex + nSpan, nStartCol, nStartCol++, (nCount % 2 == 2), ExcelHorizontalAlignment.Center, false);
                        //ExcelTool.FillCellMerge(ref sheet, oItem.EndDateSt, nRowIndex, nRowIndex + nSpan, nStartCol, nStartCol++, (nCount % 2 == 2), ExcelHorizontalAlignment.Center, true);
                        ExcelTool.FillCellMerge(ref sheet, oItem.HeaderName, nRowIndex, nRowIndex + nSpan, nStartCol, nStartCol++, (nCount % 2 == 2), ExcelHorizontalAlignment.Left, true);
                        int nRSID = -99;

                        foreach (var obj in oItem.Results)
                        {
                            nStartCol = 4;
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.EndDateSt, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.BuyerName, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.OrderNo, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.PSBatchNo, false); 
                            
                            int nSpan_Row = oItem.Results.Where(x => x.RouteSheetID == obj.RouteSheetID).Count()-1;

                            if (nRSID != obj.RouteSheetID) ExcelTool.FillCellMerge(ref sheet, obj.RouteSheetNo, nRowIndex, nRowIndex + nSpan_Row, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, false);
                            else nStartCol = 9;

                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ProductName.ToString(), false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ColorName.ToString(), false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty.ToString(), true);

                            ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                            if (nRSID != obj.RouteSheetID) ExcelTool.FillCellMerge(ref sheet, oItem.Results.Where(x => x.RouteSheetID == obj.RouteSheetID).Sum(x => x.Qty), nRowIndex, nRowIndex + nSpan_Row, nStartCol, nStartCol++, false);
                            else nStartCol = 13;

                            //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false);


                            string sRemarks = obj.Remarks;

                            if (!string.IsNullOrEmpty(obj.DyeLoadNote))
                                sRemarks = obj.DyeLoadNote;
                            if (!string.IsNullOrEmpty(obj.DyeUnloadNote))
                                sRemarks += (!string.IsNullOrEmpty(sRemarks) ? ", " : "") + obj.DyeUnloadNote;

                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, sRemarks, false, false);
                            nRSID = obj.RouteSheetID;
                            nRowIndex++;
                        }
                        //nRowIndex++;
                    }

                    ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                    #region Grand Total
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, " Total:", nRowIndex, nRowIndex, nStartCol, 10, true, ExcelHorizontalAlignment.Right, true);
                    ExcelTool.FillCellMerge(ref sheet, oDUPSD.Sum(x => x.Qty).ToString(), nRowIndex, nRowIndex, 11, 11, true, ExcelHorizontalAlignment.Right, true);
                    ExcelTool.FillCellMerge(ref sheet, oDUPSD.Sum(x => x.Qty).ToString(), nRowIndex, nRowIndex, 12, 12, true, ExcelHorizontalAlignment.Right, true);
                    //ExcelTool.FillCell(sheet, nRowIndex, 11, "", false);
                    ExcelTool.FillCell(sheet, nRowIndex, 13, "", false);
                    nRowIndex++;
                    #endregion

                    nRowIndex++;

                    #region Summary

                    double total_batch_Qty = oDUPSD.Sum(x => x.Qty), percentage = 0;

                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, " InHouse Production:", nRowIndex, nRowIndex, nStartCol+4, 8, true, ExcelHorizontalAlignment.Right, true);
                    ExcelTool.FillCellMerge(ref sheet, Global.MillionFormat(oDUPSD.Where(x => x.IsInHouse).Sum(x => x.Qty)), nRowIndex, nRowIndex, 9, 9, true, ExcelHorizontalAlignment.Right, true);
                    if (total_batch_Qty > 0) percentage = (oDUPSD.Where(x => x.IsInHouse).Sum(x => x.Qty) * 100 / total_batch_Qty); else percentage = 0;
                    ExcelTool.FillCellMerge(ref sheet, Global.MillionFormat(percentage), nRowIndex, nRowIndex, 10, 10, true, ExcelHorizontalAlignment.Right, true);
                    //ExcelTool.FillCell(sheet, nRowIndex, 11, "", false);
                    //ExcelTool.FillCell(sheet, nRowIndex, 12, "", false);
                    nRowIndex++;

                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, " Out-Side Production:", nRowIndex, nRowIndex, nStartCol+4, 8, true, ExcelHorizontalAlignment.Right, true);
                    ExcelTool.FillCellMerge(ref sheet, Global.MillionFormat(oDUPSD.Where(x => !x.IsInHouse).Sum(x => x.Qty)), nRowIndex, nRowIndex, 9, 9, true, ExcelHorizontalAlignment.Right, true);
                    if (total_batch_Qty > 0) percentage = (oDUPSD.Where(x => !x.IsInHouse).Sum(x => x.Qty) * 100 / total_batch_Qty); else percentage = 0;
                    ExcelTool.FillCellMerge(ref sheet, Global.MillionFormat(percentage), nRowIndex, nRowIndex, 10, 10, true, ExcelHorizontalAlignment.Right, true);
                    //ExcelTool.FillCell(sheet, nRowIndex, 11, "", false);
                    //ExcelTool.FillCell(sheet, nRowIndex, 12, "", false);
                    nRowIndex++;

                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, " Total Production:", nRowIndex, nRowIndex, nStartCol + 4, 8, true, ExcelHorizontalAlignment.Right, true);
                    ExcelTool.FillCellMerge(ref sheet, Global.MillionFormat(oDUPSD.Sum(x => x.Qty)), nRowIndex, nRowIndex, 9, 9, true, ExcelHorizontalAlignment.Right, true);
                    if (total_batch_Qty > 0) percentage = (oDUPSD.Sum(x => x.Qty) * 100 / total_batch_Qty); else percentage = 0;
                    ExcelTool.FillCellMerge(ref sheet, Global.MillionFormat(percentage), nRowIndex, nRowIndex, 10, 10, true, ExcelHorizontalAlignment.Right, true);
                    //ExcelTool.FillCell(sheet, nRowIndex, 11, "", false);
                    //ExcelTool.FillCell(sheet, nRowIndex, 12, "", false);
                    nRowIndex++;

                    #endregion

                    #endregion

                    nStartCol = 2;
                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Daily-Yarn-Dyeing-Production-Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion



        #region Print_XL
        public void PrintDUPScheduleXL(string newstring, bool bPortrait)
        {
            _oDUPSchedule = new DUPSchedule();
            string sLocationIDs = "";
            int nCount = 0;
            List<Location> oLocations = new List<Location>();
            List<Machine> oMachines = new List<Machine>();
            List<DUPSLot> oDUPSLots = new List<DUPSLot>();
            int cSpace = Convert.ToInt32(newstring.Split('~')[0]);
            DateTime StartSearchTime = Convert.ToDateTime(newstring.Split('~')[1]);
            DateTime EndSearchTime = Convert.ToDateTime(newstring.Split('~')[2]);
            int LocationId = Convert.ToInt32(newstring.Split('~')[3]);
            string DyeMachineIds = Convert.ToString(newstring.Split('~')[4]);
            string DateDUPScheduleOf = Convert.ToString(newstring.Split('~')[5]);
            string sPrintView = Convert.ToString(newstring.Split('~')[6]);
            string sDUPScheduleIDs = Convert.ToString(newstring.Split('~')[7]);
            bool bUptoLoadDyeMachine = Convert.ToBoolean(newstring.Split('~')[8]);
            int buid = Convert.ToInt32(newstring.Split('~')[9]);
            int nMachineType = Convert.ToInt32(newstring.Split('~')[10]);
            string sMachineIDs = newstring.Split('~')[11];

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            string sTemp = "";
            int nMaxValue = 0;
            DateTime startTime = DateTime.Now, endTime = DateTime.Now, today = DateTime.Now;
            startTime = new DateTime(StartSearchTime.Year, StartSearchTime.Month, StartSearchTime.Day, 0, 0, 0);
            endTime = new DateTime(StartSearchTime.Year, StartSearchTime.Month, StartSearchTime.Day, 23, 59, 59);
            endTime = endTime.AddDays(1);
            try
            {
                _sSQL = "SELECT * FROM View_Location where LocationID in (Select LocationID from WorkingUnit where BUID=" + buid + ")";
                oLocations = Location.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                #region Machine

                string sSQL = "SELECT * FROM Machine WHERE BUID=" + buid;
                if (!string.IsNullOrEmpty(sMachineIDs))
                    sSQL += " AND MachineID IN (" + sMachineIDs + ")";
                else if (nMachineType > 0)
                    sSQL += " AND MachineID IN (SELECT MachineID FROM Machine WHERE MachineTypeID=" + nMachineType + ")";
                else
                    sSQL += " AND MachineTypeID IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID =" + (int)EnumModuleName.DUPSchedule + ")";
                sSQL += " ORDER BY SequenceNo";
                oMachines = Machine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                #endregion

                sLocationIDs = Location.IDInString(oLocations);
                if (bUptoLoadDyeMachine)
                {
                    _sSQL = "Select * from View_DUPSchedule where MachineID IN (" + string.Join(",", oMachines.Select(x => x.MachineID)) + ")  and  LocationID in(" + sLocationIDs + ") and ScheduleStatus in (" + (int)EnumProductionScheduleStatus.Publish + "," + (int)EnumProductionScheduleStatus.Running + "," + (int)EnumProductionScheduleStatus.Urgent + ") order by MachineID,StartTime ";
                    _oDUPSchedule.DUPSchedules = DUPSchedule.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (_oDUPSchedule.DUPSchedules.Count > 0)
                    {
                        _sSQL = "Select * from View_DUPScheduleDetail Where DUPScheduleID IN (Select DUPScheduleID from View_DUPSchedule where MachineID IN (" + string.Join(",", oMachines.Select(x => x.MachineID)) + ") and LocationID in(" + sLocationIDs + ") and  ScheduleStatus in (" + (int)EnumProductionScheduleStatus.Publish + "," + (int)EnumProductionScheduleStatus.Running + "," + (int)EnumProductionScheduleStatus.Urgent + "))";  // RSState=7  UnloadFromDyemachine
                        _oDUPSchedule.DUPScheduleDetails = DUPScheduleDetail.GetsSqL(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
                else
                {
                    _sSQL = "Select * from View_DUPSchedule where LocationID in(" + sLocationIDs + ") " + (nMachineType <= 0 ? "" : " and MachineID IN (SELECT MachineID FROM Machine WHERE MachineTypeID=" + nMachineType + ") ") + " and ((StartTime>='" + startTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:00")) + "' and StartTime<='" + endTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')) order by MachineID,StartTime ";
                    _oDUPSchedule.DUPSchedules = DUPSchedule.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (_oDUPSchedule.DUPSchedules.Count > 0)
                    {
                        _sSQL = "Select * from View_DUPScheduleDetail Where DUPScheduleID IN (Select DUPScheduleID from View_DUPSchedule where LocationID in(" + sLocationIDs + ")  " + (nMachineType <= 0 ? "" : " and MachineID IN (" + string.Join(",", oMachines.Select(x => x.MachineID)) + ") ") + "  and ((StartTime>='" + startTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:00")) + "' and StartTime<='" + endTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endTime.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')))";  // RSState=7  UnloadFromDyemachine
                        _oDUPSchedule.DUPScheduleDetails = DUPScheduleDetail.GetsSqL(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
                #endregion

                foreach (Machine oItem in oMachines)
                {
                    nCount = (from oPS in _oDUPSchedule.DUPSchedules where oPS.MachineID == oItem.MachineID && oPS.BUID == oItem.BUID select oPS).Count();
                    oItem.MachineNoWithCapacityAndTotalSchedule = "[" + oItem.Code + "]" + oItem.Name + "[" + ((oItem.Capacity > 0) ? oItem.Capacity.ToString() : "") + "" + (!string.IsNullOrEmpty(oItem.Capacity2) ? oItem.Capacity2.ToString() : "") + "]" + " (" + Convert.ToInt32(nCount) + ")";
                }

                //_sSQL = string.Join(",", _oDUPSchedule.DUPSchedules.Select(x => x.DUPScheduleID).Distinct().ToList());
                //if (!string.IsNullOrEmpty(_sSQL))
                //{
                //    _sSQL = "SELECT * FROM View_DUPSLot where DUPScheduleID in (" + _sSQL + ")";
                //    oDUPSLots = DUPSLot.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //}

                foreach (DUPSchedule oItem in _oDUPSchedule.DUPSchedules)
                {
                    oItem.DUPScheduleDetails = new List<DUPScheduleDetail>();
                    oItem.DUPScheduleDetails = _oDUPSchedule.DUPScheduleDetails.Where(x => x.DUPScheduleID == oItem.DUPScheduleID).ToList();
                    sDateRange = oItem.StartTime.ToString("dd MMM yy") + "(" + oItem.StartTime.ToString("HH:mm tt") + "-" + oItem.EndTime.ToString("HH:mm tt") + ")";
                    nCount = 0;
                    sTemp="";
                    if (oItem.DUPScheduleDetails.Count == 0 || oItem.DUPScheduleDetails == null) { oItem.OrderInfo = oItem.ScheduleNo + " " + sDateRange; }
                    else
                    {
                        //oItem.LotNo = string.Join("+", oDUPSLots.Where(x => x.DUPScheduleID == oItem.DUPScheduleID).Select(x => x.LotNo + " (" + Global.MillionFormat(x.Qty) + ")").Distinct().ToList());
                        if (oRouteSheetSetup.IsShowBuyer)
                        { oItem.OrderInfo = sDateRange + "\n" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.BuyerName).Distinct().ToList()); }
                        else { oItem.OrderInfo = sDateRange + "\n" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ContractorName).Distinct().ToList()); }

                        oItem.OrderInfo = oItem.OrderInfo + "\n" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.OrderNo).Distinct().ToList());
                        oItem.OrderInfo = oItem.OrderInfo + "\n" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ProductName).Distinct().ToList());
                        if ( oItem.DUPScheduleDetails[0].IsInHouse == false)
                        { oItem.OrderInfo = oItem.OrderInfo + "</br> Lot: " + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.BuyerRef).ToList()); }
                    
                        oItem.OrderInfo = oItem.OrderInfo + "\n" + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ColorName).Distinct().ToList());
                        sTemp = string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ColorNo).Distinct().ToList());
                        if (!string.IsNullOrEmpty(sTemp))
                        {
                        oItem.OrderInfo = oItem.OrderInfo + "\n " + sTemp;
                        }
                        sTemp = string.Join("+", oItem.DUPScheduleDetails.Select(x => x.ApproveLotNo).Distinct().ToList());
                        if (!string.IsNullOrEmpty(sTemp))
                        {
                        oItem.OrderInfo = oItem.OrderInfo + " (" + sTemp+")";
                        }
                        oItem.OrderInfo = oItem.OrderInfo + "\n" + string.Join("+", oItem.DUPScheduleDetails.Select(x => Global.MillionFormat(x.Qty) + " " + oRouteSheetSetup.MUnit + ((x.BagCount <= 0) ? "" : " (" + x.BagCount.ToString() + " " + x.HankorConeST + ")")).ToList());
                        if (!string.IsNullOrEmpty(oRouteSheetSetup.BatchCode))
                        {
                            oItem.OrderInfo = oItem.OrderInfo + "\n" + string.Join("+", oItem.DUPScheduleDetails.Select(x => oRouteSheetSetup.BatchCode + "-" + x.PSBatchNo).Distinct().ToList());
                        }
                        if (!String.IsNullOrEmpty(oItem.Note))
                        {
                            oItem.OrderInfo = oItem.OrderInfo + "\n" + oItem.Note;
                        }
                        if (!String.IsNullOrEmpty(oItem.DUPScheduleDetails[0].RouteSheetNo))
                        {
                            oItem.OrderInfo = oItem.OrderInfo + "\nBatch No: " + string.Join("+", oItem.DUPScheduleDetails.Select(x => x.RouteSheetNo).Distinct().ToList());
                        }
                    }
                }
                if (_oDUPSchedule.DUPSchedules.Count() > 0)
                {
                    nMaxValue = _oDUPSchedule.DUPSchedules.GroupBy(x => x.MachineID).Max(t => t.Count());
                }
            }
            catch (Exception ex)
            {
                _oDUPSchedule = new DUPSchedule();
                _oDUPSchedule.ErrorMessage = ex.Message;
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            #region Print_XL
            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            for (int i = 0; i < oRouteSheetSetup.MachinePerDoc; i++ )
            {
                table_header.Add(new TableHeader { Header = "", Width = 35f, IsRotate = false, Align = TextAlign.Center });
            }
            //foreach (Machine oMachine in oMachines)   // table header load
            //{
            //    table_header.Add(new TableHeader { Header = oMachine.MachineNoWithCapacityAndTotalSchedule, Width = 35f, IsRotate = false, Align = TextAlign.Center });
            //}
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = oRouteSheetSetup.MachinePerDoc + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Production Schedule Report";
                var sheet = excelPackage.Workbook.Worksheets.Add("Production Schedule Report");
                sheet.Name = "Production Schedule Report";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Production Schedule Report"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                #endregion

                #region Data
                nRowIndex++;
                nStartCol = 2;
                //nEndCol = table_header.Count() + nStartCol;
                nEndCol = oRouteSheetSetup.MachinePerDoc + nStartCol;

                double _nTotalQtyInHouse = _oDUPSchedule.DUPScheduleDetails.Where(x => x.IsInHouse = true).Sum(x => x.Qty);
                double _nTotalQtyOutside = _oDUPSchedule.DUPScheduleDetails.Where(x => x.IsInHouse = false).Sum(x => x.Qty);
                double _nTotalPSQty = _oDUPSchedule.DUPScheduleDetails.Sum(x => x.Qty);
                string sQuantity = "Schedule of:                                                                                                        ";
                sQuantity = sQuantity + "In House Qty: " + Global.MillionFormat(_nTotalQtyInHouse) + " " + oRouteSheetSetup.MUnit + " Out Side Qty: " + Global.MillionFormat(_nTotalQtyOutside) +
                                    " " + oRouteSheetSetup.MUnit + " Total Qty: " + Global.MillionFormat(_nTotalPSQty) + "   " + DateTime.Now.ToString("dd MMM yyy HH:mm");
                //ExcelTool.FillCellMerge(ref sheet, sQuantity, nRowIndex, nRowIndex++, 2, nEndCol-1, true, ExcelHorizontalAlignment.Left, true);
                //ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                nStartCol = 2;
                int nfirstRowIndex = nRowIndex;
                ExcelTool.FillCellMerge(ref sheet, sQuantity, nRowIndex, nRowIndex++, 2, oRouteSheetSetup.MachinePerDoc + 1, true, ExcelHorizontalAlignment.Left, true);
                foreach(Machine obj in oMachines)
                {
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol, obj.MachineNoWithCapacityAndTotalSchedule.ToString(), false);
                    nfirstRowIndex = nRowIndex;
                    nRowIndex++;
                    foreach (var childObj in _oDUPSchedule.DUPSchedules)
                    {
                        if (childObj.MachineID == obj.MachineID)
                        {
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol, childObj.OrderInfo.ToString(), false);
                            nRowIndex++;
                        }
                    }
                    
                    if(nStartCol>oRouteSheetSetup.MachinePerDoc)
                    {
                        nRowIndex = nRowIndex + 12;
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, sQuantity, nRowIndex, nRowIndex++, 2, oRouteSheetSetup.MachinePerDoc + 1, true, ExcelHorizontalAlignment.Left, true);
                        //ExcelTool.FillCell(sheet, nRowIndex, nStartCol, obj.MachineNoWithCapacityAndTotalSchedule.ToString(), false);
                    }
                    else
                    {
                        nStartCol++;
                        nRowIndex = nfirstRowIndex;
                    }
                }

                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Production Schedule Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


            #endregion

        }

    }
}
