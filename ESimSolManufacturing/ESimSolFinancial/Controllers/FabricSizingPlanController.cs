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
    public class FabricSizingPlanController : PdfViewController
    {
        #region Declartion

        FabricSizingPlan _oFabricSizingPlan = new FabricSizingPlan();
        List<FabricSizingPlan> _oFabricSizingPlans = new List<FabricSizingPlan>();
        List<FabricSizingPlanDetail> _oFabricSizingPlanDetails = new List<FabricSizingPlanDetail>();
        string _sSQL = "";
        string sDateRange = "";

        #endregion

        #region Actions
        public ActionResult ViewFabricSizing(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
              List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
              List<FabricSizingPlanDetail> oFabricSizingPlanDetails = new List<FabricSizingPlanDetail>();
              string sTemps = "";
             oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricSizingPlan).ToString() , (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, oARMs);
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            List<FabricMachine> oFabricMachines = new List<FabricMachine>();
            _sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 AND isnull(IsBeam,0)=0 and WeavingProcess = " + (int)EnumWeavingProcess.Sizing + " Order By Code";
            oFabricMachines = FabricMachine.Gets(_sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);

            //DateTime today = DateTime.Today;
            //DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
            //DateTime endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            DateTime startOfMonth = DateTime.Today;
            //startOfMonth = startOfMonth.AddHours(oRouteSheetSetup.BatchTime);
            DateTime endOfMonth = startOfMonth.AddDays(1);
            _sSQL = "Select * from View_FabricSizingPlan where ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')) order by FMID,StartTime ";
            _oFabricSizingPlans = FabricSizingPlan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oFabricSizingPlans.Count > 0)
            {
                sTemps = string.Join(",", _oFabricSizingPlans.Select(x => x.FabricSizingPlanID).ToList());
                if (!string.IsNullOrEmpty(sTemps))
                {
                    _sSQL = "Select * from View_FabricSizingPlanDetail where FabricSizingPlanID in (" + sTemps + ")";
                    _oFabricSizingPlanDetails = FabricSizingPlanDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oFabricSizingPlans.ForEach(x =>
                    {
                        if (_oFabricSizingPlanDetails.FirstOrDefault() != null && _oFabricSizingPlanDetails.FirstOrDefault().FabricSizingPlanID > 0 && _oFabricSizingPlanDetails.Where(b => b.FabricSizingPlanID == x.FabricSizingPlanID).Count() > 0)
                        {
                            oFabricSizingPlanDetails = _oFabricSizingPlanDetails.Where(p => p.FabricSizingPlanID == x.FabricSizingPlanID).ToList();

                            sTemps = string.Join("+", oFabricSizingPlanDetails.Select(m => m.FabricMachineTypeName).ToList());
                            x.BeamName = sTemps;
                            sTemps = string.Join("+", oFabricSizingPlanDetails.Select(m => m.SizingBeamNo).ToList());
                            x.SzingBeam = sTemps;

                        }
                    });
                }
            }

         
            ViewBag.ScheduleStatuses = EnumObject.jGets(typeof(EnumProductionScheduleStatus));
            ViewBag.FabricMachines = oFabricMachines;
         
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
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperatorTwo));
            ViewBag.Buid = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.PlanStatusList = EnumObject.jGets(typeof(EnumFabricPlanStatus));
            return View(_oFabricSizingPlans);
        }
        public ActionResult ViewFabricSizingPlan(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricSizingPlan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, oARMs);
            int nCount = 0;
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            List<FabricMachine> oFabricMachines = new List<FabricMachine>();
            //_sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 AND WeavingProcess = " + (int)EnumWeavingProcess.Sizing + " Order By Code";
            //oFabricMachines = FabricMachine.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 AND isnull(IsBeam,0)=0 and WeavingProcess = " + (int)EnumWeavingProcess.Sizing + " Order By Code";
            oFabricMachines = FabricMachine.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //DateTime today = DateTime.Today;
            //DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
            //DateTime endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            DateTime startOfMonth = DateTime.Today;
            //startOfMonth = startOfMonth.AddHours(oRouteSheetSetup.BatchTime);
            DateTime endOfMonth = startOfMonth.AddDays(1);
            _sSQL = "Select * from View_FabricSizingPlan where ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')) order by FMID,StartTime ";
            _oFabricSizingPlan.FabricSizingPlans = FabricSizingPlan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oFabricSizingPlan.FabricSizingPlans.Count > 0)
            {
                _sSQL = "Select * from View_FabricSizingPlanDetail Where FabricSizingPlanID IN (Select FabricSizingPlanID from View_FabricSizingPlan where ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')))";
                _oFabricSizingPlan.FabricSizingPlanDetails = FabricSizingPlanDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }



            foreach (FabricSizingPlan oItem in _oFabricSizingPlan.FabricSizingPlans)
            {
                oItem.FabricSizingPlanDetails = new List<FabricSizingPlanDetail>();
                oItem.FabricSizingPlanDetails = _oFabricSizingPlan.FabricSizingPlanDetails.Where(x => x.FabricSizingPlanID == oItem.FabricSizingPlanID).ToList();
                sDateRange = oItem.StartTime.ToString("dd MMM yy") + "(" + oItem.StartTime.ToString("HH:mm tt") + "-" + oItem.EndTime.ToString("HH:mm tt") + ")";
                nCount = 0;
                if (oItem.FabricSizingPlanDetails.Count == 0 || oItem.FabricSizingPlanDetails == null)
                {
                    oItem.OrderInfo = oItem.ScheduleNo + " " + sDateRange;
                    oItem.OrderInfo = sDateRange + "</br>" + oItem.ContractorName;

                    oItem.OrderInfo = oItem.OrderInfo + "</br>" + oItem.ExeNo;
                    oItem.OrderInfo = oItem.OrderInfo + "</br>" + oItem.BatchNo;
                }

                else
                {
                    //oItem.LotNo = string.Join("+", oDUPSLots.Where(x => x.FabricSizingPlanID == oItem.FabricSizingPlanID).Select(x => x.LotNo+" ("+ Global.MillionFormat(x.Qty)+")").Distinct().ToList());
                    //oItem.RSStatus = (int)oItem.FabricSizingPlanDetails.FirstOrDefault().RSState;

                    oItem.OrderInfo = sDateRange + "</br>" + oItem.ContractorName;

                    oItem.OrderInfo = oItem.OrderInfo + "</br>" + oItem.ExeNo;
                    oItem.OrderInfo = oItem.OrderInfo + "</br>" + oItem.BatchNo;

                    FabricSizingPlanDetail oSelectedItem = oItem.FabricSizingPlanDetails.FirstOrDefault();
                    //oItem.OrderInfo = oItem.OrderInfo + "</br>" + string.Join("+", oItem.FabricSizingPlanDetails.Select(x => Global.MillionFormat(x.Qty) + " " + oRouteSheetSetup.MUnit + ((x.BagCount <= 0) ? "" : " (" + x.BagCount.ToString() + " " + x.HankorConeST + ")")).ToList());
                    oItem.OrderInfo = oItem.OrderInfo + "</br>" + Global.MillionFormat(oItem.FabricSizingPlanDetails.Sum(x => x.Qty));

                    if (!String.IsNullOrEmpty(oItem.Note))
                    {
                        oItem.OrderInfo = oItem.OrderInfo + "</br>" + oItem.Note;
                    }
                    //if (!String.IsNullOrEmpty(oItem.FabricSizingPlanDetails[0].RouteSheetNo))
                    //{
                    //    oItem.OrderInfo = oItem.OrderInfo + "</br>Batch No: " + string.Join("+", oItem.FabricSizingPlanDetails.Select(x => x.RouteSheetNo).Distinct().ToList());
                    //}

                }

            }



            #region MachineType
            List<MachineType> oMachineTypes = new List<MachineType>();

            string sSQL = "SELECT * FROM MachineType WHERE BUID=" + buid + " AND MachineTypeID IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID =" + (int)EnumModuleName.FabricSizingPlan + ")";

            oMachineTypes = MachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            ViewBag.MachineTypes = oMachineTypes;
            ViewBag.OrderType = EnumObject.jGets(typeof(EnumOrderType));
            ViewBag.ScheduleStatuses = EnumObject.jGets(typeof(EnumProductionScheduleStatus));
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUOrderSetups = oDUOrderSetups;
            ViewBag.FabricSizingPlans = _oFabricSizingPlan.FabricSizingPlans;
            ViewBag.FabricMachines = oFabricMachines;

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
            return View(_oFabricSizingPlan);
        }

        public ActionResult ViewFabricChemicalPlan(int nFabricSizingPlanID, int buid)
        {
            List<FabricChemicalPlan> _oFabricChemicalPlans = new List<FabricChemicalPlan>();
            _oFabricChemicalPlans = FabricChemicalPlan.Gets("SELECT * FROM View_FabricChemicalPlan WHERE FabricSizingPlanID = " + nFabricSizingPlanID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            FabricSizingPlan oFSP = new FabricSizingPlan();
            ViewBag.FabricSizingPlan = oFSP.Get(nFabricSizingPlanID, (int)Session[SessionInfo.currentUserID]);
            return View(_oFabricChemicalPlans);
        }
     
        [HttpPost]
        public JsonResult Refresh(FabricSizingPlanDetail oFabricSizingPlanDetail)
        {

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
           // oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            DateTime startOfMonth = oFabricSizingPlanDetail.StartTime;
            DateTime endOfMonth = startOfMonth.AddDays(1);
            
            List<FabricSizingPlanDetail> oFabricSizingPlanDetails = new List<FabricSizingPlanDetail>();
            string sSql = "select * from View_FabricSizingPlanDetail WHERE ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "'))";
            oFabricSizingPlanDetails = FabricSizingPlanDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = sjson = serializer.Serialize(oFabricSizingPlanDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPS(FabricSizingPlan oFabricSizingPlan)
        {
            _sSQL = "";
            FabricMachine oFabricMachine = new FabricMachine();
             _oFabricSizingPlan = new FabricSizingPlan();
            _oFabricSizingPlan.FabricSizingPlanDetails=new List<FabricSizingPlanDetail>();
            if(oFabricSizingPlan.FabricSizingPlanID>0)
            {
              _oFabricSizingPlan = _oFabricSizingPlan.Get(oFabricSizingPlan.FabricSizingPlanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
               _oFabricSizingPlan.FabricSizingPlanDetails = FabricSizingPlanDetail.Gets(oFabricSizingPlan.FabricSizingPlanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
             
            }
            else
            {
                 List<FabricSizingPlan> oPSs = new List<FabricSizingPlan>();
                 oFabricMachine = oFabricMachine.Get(oFabricSizingPlan.FMID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 _sSQL = "Select * from View_FabricSizingPlan Where FMID=" + oFabricSizingPlan.FMID + " and " + " EndTime = (Select MAX(EndTime) from FabricSizingPlan Where FMID=" + oFabricSizingPlan.FMID + ")";
                 oPSs = FabricSizingPlan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
                if (oPSs.Count > 0)
                {
                    _oFabricSizingPlan.StartTime=oPSs[0].EndTime.AddMinutes(1);
                }
                _oFabricSizingPlan.FMID = oFabricMachine.FMID;
                //_oFabricSizingPlan.MachineID = oMachine.MachineID;
                _oFabricSizingPlan.MachineName = oFabricMachine.Name;
                //_oFabricSizingPlan.MachineNo = oMachine.Code;
                //_oFabricSizingPlan.Capacity = oMachine.Capacity;
                //_oFabricSizingPlan.BUID = oMachine.BUID;
            }
          
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oFabricSizingPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsFabricExecutionSpe(FabricSizingPlan oFabricSizingPlan)// For Delivery Order
        {
            string sSQL = "";
            List<FabricSizingPlan> oFabricSizingPlans = new List<FabricSizingPlan>();

            List<FabricExecutionOrderSpecification> oFEOSs = new List<FabricExecutionOrderSpecification>();
            FabricSizingPlan oFsp = new FabricSizingPlan();
            try
            {
                sSQL = "Select top(100)* from view_FabricExecutionOrderSpecification";
                    string sReturn = "";
                    if (!String.IsNullOrEmpty(oFabricSizingPlan.ExeNo))
                    {
                        oFabricSizingPlan.ExeNo = oFabricSizingPlan.ExeNo.Trim();
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "ExeNo Like'%" + oFabricSizingPlan.ExeNo + "%'";
                    }
                    

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ProdtionType in (" + (int)EnumDispoProType.General + "," + (int)EnumDispoProType.ExcessFabric + ")";

                    sSQL = sSQL + "" + sReturn + " order by IssueDate DESC";
                    oFEOSs = FabricExecutionOrderSpecification.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (FabricExecutionOrderSpecification oItem in oFEOSs)
                    {
                        oFsp = new FabricSizingPlan();
                        oFsp.FabricSizingPlanID =0;
                        oFsp.FEOSID = oItem.FEOSID;
                        oFsp.FSCDID = oItem.FSCDID;
                        oFsp.FMID = oFabricSizingPlan.FMID;
                        oFsp.ExeNo = oItem.ExeNo;
                        oFsp.ContractorName = oItem.BuyerName;
                        oFsp.Construction = oItem.Construction;
                        oFsp.Composition = oItem.Composition;
                        oFsp.BatchNo = "";
                        oFsp.Weave = oItem.Weave;
                        oFsp.RequiredWarpLength = oItem.RequiredWarpLength;
                        //oPSD.OrderTypest = oItem.OrderTypeSt;
                        //oPSD.ColorName = oItem.ColorName;
                        //oPSD.SLNo = oItem.SLNo;
                        //oPSD.ColorNo = oItem.ColorNo;
                        //oPSD.ProductID = oItem.ProductID;
                        //oPSD.ProductName = oItem.ProductName;
                        //oPSD.ContractorName = oItem.ContractorName;
                        //oPSD.BuyerName = oItem.DeliveryToName;
                        //oPSD.HankorCone = oItem.HankorCone;
                        //if ((oItem.OrderQty - oItem.Qty_PSD)>0)
                        //{
                        //    oPSD.Qty = oItem.OrderQty - oItem.Qty_PSD;
                        //}
                        //oPSD.ContractorID = oItem.ContractorID;
                        oFabricSizingPlans.Add(oFsp);
                    }
               

            }
            catch (Exception ex)
            {
                oFabricSizingPlans = new List<FabricSizingPlan>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSizingPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsDispoFromWarp(FabricSizingPlan oFabricSizingPlan)// For Delivery Order
        {
            string sSQL = "";
            List<FabricSizingPlan> oFabricSizingPlans = new List<FabricSizingPlan>();

            List<FabricWarpPlan> oFabricWarpPlans = new List<FabricWarpPlan>();
            FabricSizingPlan oFsp = new FabricSizingPlan();
            try
            {
                sSQL = "Select * from View_FabricWarpPlan WHERE ISNULL(FEOSID,0) != 0 and ISNULL(FEOSID,0) != 0";
               
                if (!String.IsNullOrEmpty(oFabricSizingPlan.ExeNo))
                {
                    oFabricSizingPlan.ExeNo = oFabricSizingPlan.ExeNo.Trim();
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + "ExeNo Like'%" + oFabricSizingPlan.ExeNo + "%'";
                }

                oFabricWarpPlans = FabricWarpPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (FabricWarpPlan oItem in oFabricWarpPlans)
                {
                    oFsp = new FabricSizingPlan();
                    oFsp.FabricSizingPlanID = 0;
                    oFsp.FabricWarpPlanID = oItem.FabricWarpPlanID;
                    oFsp.FEOSID = oItem.FEOSID;
                    oFsp.FSCDID = oItem.FSCDID;
                    oFsp.FMID = oFabricSizingPlan.FMID;
                    oFsp.ExeNo = oItem.ExeNo;
                    oFsp.PlanType = oItem.PlanType;
                    oFsp.FSpcType = oItem.FSpcType;
                    oFsp.ContractorName = oItem.ContractorName;
                    oFsp.Construction = oItem.Construction;
                    oFsp.Composition = oItem.Composition;
                    oFsp.BatchNo = oItem.BatchNo;
                    oFsp.Weave = oItem.Weave;
                    oFsp.RequiredWarpLength = oItem.RequiredWarpLength;
                    oFsp.SzingBeam = Global.MillionFormat(oItem.QtySizing);
                    oFsp.Qty = oItem.Qty;
                    oFsp.BalanceQty = oItem.RequiredWarpLength - oItem.QtySizing;
                    if (oFsp.Qty < 0) { oFsp.Qty = 0; }
                    oFabricSizingPlans.Add(oFsp);
                }
            }
            catch (Exception ex)
            {
                oFabricSizingPlans = new List<FabricSizingPlan>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSizingPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsMachine(Machine oMachine)// For Delivery Order
        {
            string sSQL = "";
            List<FabricMachine> oMachines = new List<FabricMachine>();
            try
            {
                sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 AND isnull(IsBeam,0)=0 and WeavingProcess = " + (int)EnumWeavingProcess.Sizing + "  ";//
                string sReturn = "";
                if (!String.IsNullOrEmpty(oMachine.Name))
                {
                    oMachine.Name = oMachine.Name.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "isnull(Code,'')+Name Like'%" + oMachine.Name + "%'";
                }
                //if (oMachine.BUID > 0)
                //{
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + "BUID=" + oMachine.BUID + "";
                //}
                sSQL = sSQL + "" + sReturn + " Order By Code";

                oMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oMachines = new List<FabricMachine>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsCustomer(Contractor oContractor)
        {
            string sSQL = "";
            List<Contractor> oContractors = new List<Contractor>();
            try
            {
                sSQL = "SELECT * FROM View_Contractor WHERE ContractorID IN (SELECT BuyerID FROM View_FabricSizingPlan) ";//
                string sReturn = " ";
                if (!String.IsNullOrEmpty(oContractor.Name))
                {
                    oContractor.Name = oContractor.Name.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Name LIKE '%" + oContractor.Name + "%'";
                }
                sSQL = sSQL + sReturn;
                oContractors = Contractor.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oContractors = new List<Contractor>();
            }
            var jSonResult = Json(oContractors, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        [HttpPost]
        public JsonResult GetFabricMachineTypes(FabricMachineType oFabricMachineType)
        {
            string sSQL = "";
            List<FabricMachineType> oFabricMachineTypes = new List<FabricMachineType>();
            try
            {
                sSQL = "SELECT * FROM [View_FabricMachineType] WHERE FabricMachineTypeID IN (SELECT DISTINCT ChildMachineTypeID FROM FabricMachine WHERE IsBeam=1 AND IsActive=1 AND WeavingProcess=1)";
                if (!String.IsNullOrEmpty(oFabricMachineType.Name))
                {
                    sSQL += " AND Name LIKE '%" + oFabricMachineType.Name + "%'";
                }
                oFabricMachineTypes = FabricMachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricMachineType = new FabricMachineType();
                oFabricMachineTypes = new List<FabricMachineType>();
                oFabricMachineType.ErrorMessage = ex.Message;
                oFabricMachineTypes.Add(oFabricMachineType);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricMachineTypes);
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
                sSQL += " AND MachineTypeID in( SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID =" + (int)EnumModuleName.FabricSizingPlan + ")";
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
        public JsonResult GetPSByMachine(int nFMID, double nts)
        {
            string sSQL = "Select top(1)* from View_FabricSizingPlan Where FMID=" + nFMID + "  Order by StartTime  DESC";

            List<FabricSizingPlan> oPSs = new List<FabricSizingPlan>();
            FabricSizingPlan oPS = new FabricSizingPlan();
            string sStartTime = "";
            try
            {
                oPSs = FabricSizingPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult ScheduleInTimePeriod(FabricSizingPlan oFabricSizingPlan, double nts)
        {

            int nScheduleCount = NumberofScheduleInTimePeriod(oFabricSizingPlan);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(nScheduleCount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public int NumberofScheduleInTimePeriod(FabricSizingPlan oFabricSizingPlan)
        {
            int nScheduleCount = 0;
            DateTime dStartTime = oFabricSizingPlan.StartTime;
            DateTime dEndTime = oFabricSizingPlan.EndTime.AddMinutes(-1);
            //int nLocationID = oFabricSizingPlan.LocationID;
            int nMachineID = oFabricSizingPlan.FMID;
            string sSQL = "Select * from View_FabricSizingPlan where EndTime >= '" + dStartTime.ToString("dd MMM yyyy HH:mm") + "' and StartTime <='" + dEndTime.ToString("dd MMM yyyy HH:mm") + "'  and FMID=" + nMachineID + " and FabricSizingPlanID NOT IN (" + oFabricSizingPlan.FabricSizingPlanID + ")";
            if (nMachineID > 0)
            {
                List<FabricSizingPlan> oPS = new List<FabricSizingPlan>();
                oPS = FabricSizingPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oPS.Count() > 0)
                {
                    if (oFabricSizingPlan.StartTime <= oPS.Min(x => x.EndTime))
                    {
                        nScheduleCount = oPS.Count();
                    }
                }

            }
            return nScheduleCount;
        }
        #endregion

        [HttpPost]
        public JsonResult Save(FabricSizingPlan oFabricSizingPlan)
        {
            string sTemps = "";
            try
            {
                _oFabricSizingPlan = oFabricSizingPlan.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricSizingPlan.FabricSizingPlanDetails = FabricSizingPlanDetail.Gets(_oFabricSizingPlan.FabricSizingPlanID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oFabricSizingPlan.FabricSizingPlanDetails.FirstOrDefault() != null && _oFabricSizingPlan.FabricSizingPlanDetails.FirstOrDefault().FabricSizingPlanID > 0 && _oFabricSizingPlan.FabricSizingPlanDetails.Where(b => b.FabricSizingPlanID == _oFabricSizingPlan.FabricSizingPlanID).Count() > 0)
                {
                    sTemps = string.Join("+", _oFabricSizingPlan.FabricSizingPlanDetails.Select(m => m.FabricMachineTypeName).ToList());
                    _oFabricSizingPlan.BeamName = sTemps;
                    sTemps = string.Join("+", _oFabricSizingPlan.FabricSizingPlanDetails.Select(m => m.SizingBeamNo).ToList());
                    _oFabricSizingPlan.SzingBeam = sTemps;
                }
            }
            catch (Exception ex)
            {
                _oFabricSizingPlan = new FabricSizingPlan();
                _oFabricSizingPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSizingPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveRS(FabricSizingPlan oFabricSizingPlan)
        {
            RouteSheet oRouteSheet = new RouteSheet();
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            try
            {
                oRouteSheet = oRouteSheet.GetByPS(oFabricSizingPlan.FabricSizingPlanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
                //oFabricSizingPlan.RouteSheetID = oRouteSheet.RouteSheetID;
                //oFabricSizingPlan.RouteSheetNo = oRouteSheet.RouteSheetNo;
                //_oFabricSizingPlan = oFabricSizingPlan.SaveRS(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (string.IsNullOrEmpty(_oFabricSizingPlan.ErrorMessage))
                {
                    _oFabricSizingPlan = _oFabricSizingPlan.Get(oFabricSizingPlan.FabricSizingPlanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oFabricSizingPlan.FabricSizingPlanDetails = FabricSizingPlanDetail.Gets(oFabricSizingPlan.FabricSizingPlanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    sDateRange = _oFabricSizingPlan.StartTime.ToString("HH:mm tt") + "-" + _oFabricSizingPlan.EndTime.ToString("HH:mm tt");
                    if (_oFabricSizingPlan.FabricSizingPlanDetails.Count == 0 || _oFabricSizingPlan.FabricSizingPlanDetails == null) { _oFabricSizingPlan.OrderInfo = _oFabricSizingPlan.ScheduleNo + " " + sDateRange; }
                    //foreach (FabricSizingPlanDetail oItem2 in _oFabricSizingPlan.FabricSizingPlanDetails)
                    //{

                    //    _oFabricSizingPlan.OrderInfo = _oFabricSizingPlan.OrderInfo + "</br>" + string.Join("+", _oFabricSizingPlan.FabricSizingPlanDetails.Select(x => x.OrderNo).Distinct().ToList());
                    //    _oFabricSizingPlan.OrderInfo = _oFabricSizingPlan.OrderInfo + "</br>" + string.Join("+", _oFabricSizingPlan.FabricSizingPlanDetails.Select(x => x.ProductName).Distinct().ToList());
                    //                          _oFabricSizingPlan.OrderInfo = _oFabricSizingPlan.OrderInfo + "</br>" + string.Join("+", _oFabricSizingPlan.FabricSizingPlanDetails.Select(x => x.ColorName).Distinct().ToList());
                    //    _oFabricSizingPlan.OrderInfo = _oFabricSizingPlan.OrderInfo + "</br> Color No:" + string.Join("+", _oFabricSizingPlan.FabricSizingPlanDetails.Select(x => x.ColorNo).Distinct().ToList());
                    //    _oFabricSizingPlan.OrderInfo = _oFabricSizingPlan.OrderInfo + "</br>" + string.Join("+", _oFabricSizingPlan.FabricSizingPlanDetails.Select(x => Global.MillionFormat(x.Qty) + " " + oRouteSheetSetup.MUnit + ((x.BagCount <= 0) ? "" : " (" + x.BagCount.ToString() + " " + x.HankorConeST + ")")).ToList());

                    //}
                }
            }
            catch (Exception ex)
            {
                _oFabricSizingPlan = new FabricSizingPlan();
                _oFabricSizingPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSizingPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveChemicalPlan(List<FabricChemicalPlan> oFabricChemicalPlans)
        {
            FabricChemicalPlan _oFabricChemicalPlan = new FabricChemicalPlan();
            List<FabricChemicalPlan> _oFCPs = new List<FabricChemicalPlan>();
            try
            {
                _oFCPs = _oFabricChemicalPlan.SaveMultiple(oFabricChemicalPlans, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFCPs = new List<FabricChemicalPlan>();
                _oFabricChemicalPlan = new FabricChemicalPlan();
                _oFabricChemicalPlan.ErrorMessage = ex.Message;
                _oFCPs.Add(_oFabricChemicalPlan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFCPs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeleteChemicalPlan(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                FabricChemicalPlan oFabricChemicalPlan = new FabricChemicalPlan();
                sFeedBackMessage = oFabricChemicalPlan.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdatePlanStatus(FabricSizingPlan oFabricSizingPlan)
        {
            _oFabricSizingPlan = new FabricSizingPlan();
            try
            {
                //if ((int)oFabricSizingPlan.Status > (int)EnumFabricBatchState.InFloor && (int)oFabricSizingPlan.PlanStatus == (int)EnumFabricPlanStatus.Hold)
                //{
                //    throw new Exception("Hold is not possible, Because Batch status is " + oFabricSizingPlan.StatusSt);
                //}
                _oFabricSizingPlan = oFabricSizingPlan.UpdatePlanStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricSizingPlan = new FabricSizingPlan();
                _oFabricSizingPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSizingPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateWaterQty(FabricSizingPlan oFabricSizingPlan)
        {
            _oFabricSizingPlan = new FabricSizingPlan();
            try
            {
                _oFabricSizingPlan = oFabricSizingPlan.UpdateWaterQty((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricSizingPlan = new FabricSizingPlan();
                _oFabricSizingPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSizingPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(FabricSizingPlan oFabricSizingPlan)
        {
            try
            {
                if (oFabricSizingPlan.FabricSizingPlanID <= 0) { throw new Exception("Please select an valid item."); }
                oFabricSizingPlan.ErrorMessage = oFabricSizingPlan.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricSizingPlan = new FabricSizingPlan();
                oFabricSizingPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSizingPlan.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteDetail(FabricSizingPlanDetail oFabricSizingPlanDetail)
        {
            try
            {
                if (oFabricSizingPlanDetail.FSPDID <= 0) { throw new Exception("Please select an valid item."); }
                oFabricSizingPlanDetail.ErrorMessage = oFabricSizingPlanDetail.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricSizingPlanDetail = new FabricSizingPlanDetail();
                oFabricSizingPlanDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSizingPlanDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SwapSchedule(string sPSIDs, double nts)
        {
            List<FabricSizingPlanDetail> oFabricSizingPlanDetails = new List<FabricSizingPlanDetail>();
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

                FabricSizingPlan oFabricSizingPlan = new FabricSizingPlan();
                FabricSizingPlan oFabricSizingPlanOne = new FabricSizingPlan();
                FabricSizingPlan oFabricSizingPlanTwo = new FabricSizingPlan();
                oFabricSizingPlanOne = oFabricSizingPlanOne.Get(nDUpScID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricSizingPlanTwo = oFabricSizingPlanTwo.Get(nDUpScIDTwo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                /// Mapping One
                oFabricSizingPlan.FabricSizingPlanID = oFabricSizingPlanOne.FabricSizingPlanID;
                oFabricSizingPlan.FMID = oFabricSizingPlanTwo.FMID;
                //oFabricSizingPlan.LocationID = oFabricSizingPlanTwo.LocationID;
                oFabricSizingPlanOne.EndTime = oFabricSizingPlanOne.EndTime.AddMinutes(1);
                oTimeSpan = oFabricSizingPlanOne.EndTime - oFabricSizingPlanOne.StartTime;
                oFabricSizingPlan.StartTime = oFabricSizingPlanTwo.StartTime;
                oFabricSizingPlan.EndTime = oFabricSizingPlanTwo.StartTime + oTimeSpan;
                _oFabricSizingPlans.Add(oFabricSizingPlan);

                ///Mapping Two
                oFabricSizingPlan = new FabricSizingPlan();
                oFabricSizingPlan.FabricSizingPlanID = oFabricSizingPlanTwo.FabricSizingPlanID;
                oFabricSizingPlan.FMID = oFabricSizingPlanOne.FMID;
                //oFabricSizingPlan.LocationID = oFabricSizingPlanOne.LocationID;
                oFabricSizingPlanTwo.EndTime = oFabricSizingPlanTwo.EndTime.AddMinutes(1);
                oTimeSpan = oFabricSizingPlanTwo.EndTime - oFabricSizingPlanTwo.StartTime;
                oFabricSizingPlan.StartTime = oFabricSizingPlanOne.StartTime;
                oFabricSizingPlan.EndTime = oFabricSizingPlanOne.StartTime + oTimeSpan;
                _oFabricSizingPlans.Add(oFabricSizingPlan);

              //  oFabricSizingPlanDetails = FabricSizingPlanDetail.Swap(_oFabricSizingPlans, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                FabricSizingPlanDetail oFabricSizingPlanDetail = new FabricSizingPlanDetail();
                oFabricSizingPlanDetail.ErrorMessage = ex.Message;
                oFabricSizingPlanDetails.Add(oFabricSizingPlanDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSizingPlanDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PastSchedule(FabricSizingPlan oFabricSizingPlan)
        {

            TimeSpan oTimeSpan = new TimeSpan();
            int nDUpScID = 0;
            int nDUpScIDTwo = 0;
            string sTime = "";
            string sPSIDs = oFabricSizingPlan.Params;

            DateTime oDateTime = DateTime.MinValue;
            try
            {
                nDUpScID = Convert.ToInt32(sPSIDs.Split('~')[0]);
                nDUpScIDTwo = Convert.ToInt32(sPSIDs.Split('~')[1]);

                if (nDUpScID <= 0)
                {
                    oDateTime = Convert.ToDateTime(sPSIDs.Split('~')[2]);
                    sTime = Convert.ToString(sPSIDs.Split('~')[3]);
                    oDateTime = new DateTime(oDateTime.Year, oDateTime.Month, oDateTime.Day, int.Parse(sTime.Split(':')[0]), int.Parse(sTime.Split(':')[1]), 0);
                }
                //if (nDUpScID <= 0) { throw new Exception("Please select an valid item."); }
                if (nDUpScIDTwo <= 0) { throw new Exception("Please select an valid item."); }

                //FabricSizingPlan oFabricSizingPlan = new FabricSizingPlan();
                FabricSizingPlan oFabricSizingPlanOne = new FabricSizingPlan();
                oFabricSizingPlanOne = oFabricSizingPlan;
                FabricSizingPlan oFabricSizingPlanTwo = new FabricSizingPlan();
                oFabricSizingPlanOne.EndTime = oDateTime;
                if (nDUpScID > 0)
                {
                    oFabricSizingPlanOne = oFabricSizingPlanOne.Get(nDUpScID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                oFabricSizingPlanTwo = oFabricSizingPlanTwo.Get(nDUpScIDTwo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                /// Mapping One

                ///Mapping Two
                //oFabricSizingPlan = new FabricSizingPlan();
                oFabricSizingPlanTwo.FabricSizingPlanID = oFabricSizingPlanTwo.FabricSizingPlanID;
                oFabricSizingPlanTwo.FMID = oFabricSizingPlanOne.FMID;
                //oFabricSizingPlanTwo.LocationID = oFabricSizingPlanOne.LocationID;

                oTimeSpan = oFabricSizingPlanTwo.EndTime - oFabricSizingPlanTwo.StartTime;
                oFabricSizingPlanTwo.StartTime = oFabricSizingPlanOne.EndTime.AddMinutes(1);
                oFabricSizingPlanTwo.EndTime = oFabricSizingPlanTwo.StartTime + oTimeSpan.Duration();
                _oFabricSizingPlans.Add(oFabricSizingPlanTwo);
                _oFabricSizingPlans = FabricSizingPlan.Swap(_oFabricSizingPlans, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                FabricSizingPlan oFabricSizingPlanTemp = new FabricSizingPlan();
                oFabricSizingPlanTemp.ErrorMessage = ex.Message;
                _oFabricSizingPlans.Add(oFabricSizingPlanTemp);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSizingPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SwapTwoSchedule(FabricSizingPlan oFabricSizingPlan)
        {
            TimeSpan oTimeSpan = new TimeSpan();
            int nDUpScID = 0;
            int nDUpScIDTwo = 0;
            string sTime = "";
            string sPSIDs = oFabricSizingPlan.Params;

            DateTime oDateTime = DateTime.MinValue;
            try
            {
                nDUpScID = Convert.ToInt32(sPSIDs.Split('~')[0]);
                nDUpScIDTwo = Convert.ToInt32(sPSIDs.Split('~')[1]);

                if (nDUpScID <= 0 || nDUpScIDTwo <= 0) { throw new Exception("Please select valid items."); }

                FabricSizingPlan oFabricSizingPlanOne = new FabricSizingPlan();
                FabricSizingPlan oFabricSizingPlanTwo = new FabricSizingPlan();

                oFabricSizingPlanOne = oFabricSizingPlanOne.Get(nDUpScID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricSizingPlanTwo = oFabricSizingPlanTwo.Get(nDUpScIDTwo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                /// Mapping
                DateTime dStartTime1 = oFabricSizingPlanOne.StartTime;
                DateTime dStartTime2 = oFabricSizingPlanTwo.StartTime;
                DateTime dEndTime1 = oFabricSizingPlanOne.EndTime;
                DateTime dEndTime2 = oFabricSizingPlanTwo.EndTime;

                oFabricSizingPlanOne.StartTime = dStartTime2;
                oFabricSizingPlanTwo.StartTime = dStartTime1;

                oFabricSizingPlanOne.EndTime = dStartTime2 + (dEndTime1 - dStartTime1).Duration();
                oFabricSizingPlanTwo.EndTime = dStartTime1 + (dEndTime2 - dStartTime2).Duration();

                _oFabricSizingPlans.Add(oFabricSizingPlanOne);
                _oFabricSizingPlans.Add(oFabricSizingPlanTwo);
                _oFabricSizingPlans = FabricSizingPlan.Swap(_oFabricSizingPlans, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                FabricSizingPlan oFabricSizingPlanTemp = new FabricSizingPlan();
                oFabricSizingPlanTemp.ErrorMessage = ex.Message;
                _oFabricSizingPlans.Add(oFabricSizingPlanTemp);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSizingPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPSByDU(FabricSizingPlanDetail oFabricSizingPlanDetail)
        {
            _sSQL = "";
            Machine oMachine = new Machine();
            List<FabricSizingPlan> _oFabricSizingPlans = new List<FabricSizingPlan>();
            if (oFabricSizingPlanDetail.FSPDID > 0)
            {
                List<FabricSizingPlan> oPSs = new List<FabricSizingPlan>();
               // _sSQL = "SELECT * FROM FabricSizingPlan WHERE FabricSizingPlanID IN (SELECT FabricSizingPlanID FROM FabricSizingPlanDetail WHERE DODID =" + oFabricSizingPlanDetail.DODID + ")";
                _oFabricSizingPlans = FabricSizingPlan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSizingPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            List<Product> _oProducts = new List<Product>();
            Product _oProduct = new Product();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    _oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.FabricChemicalPlan, EnumProductUsages.Regular, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.FabricChemicalPlan, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oProduct = new Product();
                _oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(_oProduct);
            }
            var jSonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
       

        #region Get Search Value
        [HttpPost]
        public JsonResult RefreshMachineWise(FabricSizingPlan oFabricSizingPlan, bool bUptoLoadDyeMachine, int sScheduleDateType)
        {
            var nCount = 0;

            List<FabricMachine> oFabricMachines = new List<FabricMachine>();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
          //  oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            //_sSQL = "SELECT * FROM Location where LocationID in(SELECT Distinct LocationID FROM VIEW_CapitalResource where ISNULL(IsActive,1)=1 AND BUID = " + oFabricSizingPlan.BUID + " AND ResourcesType = " + (int)EnumResourcesType.Machine + " )";
       
            #region Machine
            

            string sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 AND isnull(IsBeam,0)=0 and WeavingProcess = " + (int)EnumWeavingProcess.Sizing + "";
            

            if (!string.IsNullOrEmpty(oFabricSizingPlan.MachineName))
            {
                sSQL += " AND MachineID IN (" + oFabricSizingPlan.MachineName + ")";
            }
            _sSQL = " Order By Code";
            oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

         

            //DateTime today = oFabricSizingPlan.StartTime;
            DateTime startOfMonth = oFabricSizingPlan.StartTime;
            DateTime endOfMonth = startOfMonth.AddDays(1);
            if (bUptoLoadDyeMachine)
            {
                _sSQL = "Select * from View_FabricSizingPlan where  FMID IN (" + string.Join(",", oFabricMachines.Select(x => x.FMID)) + ") and ScheduleStatus in (" + (int)EnumProductionScheduleStatus.Publish + "," + (int)EnumProductionScheduleStatus.Running + "," + (int)EnumProductionScheduleStatus.Urgent + ")  order by MachineID,StartTime ";
            }
            else
            {
                _sSQL = "Select * from View_FabricSizingPlan where FMID IN (" + string.Join(",", oFabricMachines.Select(x => x.FMID)) + ") and ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')) order by FMID,StartTime ";
            }
            _oFabricSizingPlan.FabricSizingPlans = FabricSizingPlan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oFabricSizingPlan.FabricSizingPlans.Count > 0)
            {
                if (bUptoLoadDyeMachine)
                {
                    _sSQL = "Select * from View_FabricSizingPlanDetail Where FabricSizingPlanID IN (Select FabricSizingPlanID from View_FabricSizingPlan where FMID IN (" + string.Join(",", oFabricMachines.Select(x => x.FMID)) + ")    and ScheduleStatus in (" + (int)EnumProductionScheduleStatus.Publish + "," + (int)EnumProductionScheduleStatus.Running + "," + (int)EnumProductionScheduleStatus.Urgent + "))";  // RSState=7  UnloadFromDyemachine
                }
                else
                {
                    _sSQL = "Select * from View_FabricSizingPlanDetail Where FabricSizingPlanID IN (Select FabricSizingPlanID from View_FabricSizingPlan where  FMID IN (" + string.Join(",", oFabricMachines.Select(x => x.FMID)) + ") and ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') ) )";  // RSState=7  UnloadFromDyemachine
                }
                _oFabricSizingPlan.FabricSizingPlanDetails = FabricSizingPlanDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
          
            foreach (FabricSizingPlan oItem in _oFabricSizingPlan.FabricSizingPlans)
            {

                oItem.FabricSizingPlanDetails = new List<FabricSizingPlanDetail>();
                oItem.FabricSizingPlanDetails = _oFabricSizingPlan.FabricSizingPlanDetails.Where(x => x.FabricSizingPlanID == oItem.FabricSizingPlanID).ToList();
                sDateRange = oItem.StartTime.ToString("dd MMM yy") + "(" + oItem.StartTime.ToString("HH:mm tt") + "-" + oItem.EndTime.ToString("HH:mm tt") + ")";
                if (oItem.FabricSizingPlanDetails.Count == 0 || oItem.FabricSizingPlanDetails == null) { oItem.OrderInfo = oItem.ScheduleNo + " " + sDateRange;
                oItem.OrderInfo = sDateRange + "</br>" + oItem.ContractorName;
                oItem.OrderInfo = oItem.OrderInfo + "</br>" + oItem.ExeNo;
                oItem.OrderInfo = oItem.OrderInfo + "</br>" + oItem.BatchNo;
                }
                else
                {
                    //oRouteSheetSetup.BatchCode + "-" + oItem2.PSBatchNo
                    if (!String.IsNullOrEmpty(oItem.Note))
                    {
                        oItem.OrderInfo = oItem.OrderInfo + "</br>" + oItem.Note;
                    }
                   // if (!String.IsNullOrEmpty(oItem.FabricSizingPlanDetails[0].RouteSheetNo))
                    //{
                    //    oItem.OrderInfo = oItem.OrderInfo + "</br>Batch No: " + string.Join("+", oItem.FabricSizingPlanDetails.Select(x => x.RouteSheetNo).Distinct().ToList());
                    //}

                }

            
            }
            _oFabricSizingPlan.FabricMachines = oFabricMachines;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSizingPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Print Schedule
   
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

        #region Print Schedule

        [HttpPost]
        public JsonResult AdvSearch(FabricSizingPlan oFabricSizingPlan)
        {
            _oFabricSizingPlans = new List<FabricSizingPlan>();
            List<FabricSizingPlanDetail> oFabricSizingPlanDetails = new List<FabricSizingPlanDetail>();
            string sTemps = "";
            try
            {
                string sSQL = MakeSQL(oFabricSizingPlan);
                _oFabricSizingPlans = FabricSizingPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
                sTemps = string.Join(",", _oFabricSizingPlans.Select(x => x.FabricSizingPlanID).ToList());
                if (!string.IsNullOrEmpty(sTemps))
                {
                    sSQL = "Select * from View_FabricSizingPlanDetail where FabricSizingPlanID in (" + sTemps + ")";
                    _oFabricSizingPlanDetails = FabricSizingPlanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oFabricSizingPlans.ForEach(x =>
                    {
                        if (_oFabricSizingPlanDetails.FirstOrDefault() != null && _oFabricSizingPlanDetails.FirstOrDefault().FabricSizingPlanID > 0 && _oFabricSizingPlanDetails.Where(b => b.FabricSizingPlanID == x.FabricSizingPlanID).Count() > 0)
                        {
                            oFabricSizingPlanDetails = _oFabricSizingPlanDetails.Where(p => p.FabricSizingPlanID == x.FabricSizingPlanID).ToList();

                            sTemps = string.Join("+", oFabricSizingPlanDetails.Select(m => m.FabricMachineTypeName).ToList());
                            x.BeamName = sTemps;
                            sTemps = string.Join("+", oFabricSizingPlanDetails.Select(m => m.SizingBeamNo).ToList());
                            x.SzingBeam = sTemps;
                           
                        }
                    });
                }

                
            }
            catch (Exception ex)
            {
                _oFabricSizingPlans = new List<FabricSizingPlan>();
                oFabricSizingPlan.ErrorMessage = ex.Message;
                _oFabricSizingPlans.Add(oFabricSizingPlan);
            }
            var jsonResult = Json(_oFabricSizingPlans, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(FabricSizingPlan oFabricSizingPlan)
        {
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            string sParams = oFabricSizingPlan.ErrorMessage;

            int ncboOrderDate = 0, nDesign = 0, nPlanStatus = 0;
            DateTime dFromDateDate = DateTime.Today;
            DateTime dToDate = DateTime.Today;
            string sExeNo = "", sConstruction = "", sMachineIDs = "", sCustomerIDs = "", sBatchNo = "";
            string sReturn1 = "";
            string sReturn = "";
            bool bIsPending = false;
            sReturn1 = "Select * from View_FabricSizingPlan";
            if (!string.IsNullOrEmpty(sParams))
            {
                ncboOrderDate = Convert.ToInt32(sParams.Split('~')[0]);
                dFromDateDate = Convert.ToDateTime(sParams.Split('~')[1]);
                dToDate = Convert.ToDateTime(sParams.Split('~')[2]);
                sExeNo = Convert.ToString(sParams.Split('~')[3]);
                sConstruction = Convert.ToString(sParams.Split('~')[4]);
                sMachineIDs = Convert.ToString(sParams.Split('~')[5]);
                sCustomerIDs = Convert.ToString(sParams.Split('~')[6]);
                nDesign = Convert.ToInt32(sParams.Split('~')[7]);
                nPlanStatus = Convert.ToInt32(sParams.Split('~')[8]);
                sBatchNo = sParams.Split('~')[9];
                bIsPending = Convert.ToBoolean(sParams.Split('~')[10]);
            }
            else
            {
                dFromDateDate = DateTime.Today;
                dToDate = dFromDateDate.AddDays(1);
                sReturn1 = "Select * from View_FabricSizingPlan where ((StartTime>='" + dFromDateDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + dToDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + dFromDateDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + dToDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')) ";

            }
         
           #region Order Date
            if (ncboOrderDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (ncboOrderDate == (int)EnumCompareOperator.EqualTo)
                {
                    dToDate = dFromDateDate.AddDays(1);
                    sReturn = sReturn + " ((StartTime>='" + dFromDateDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + dToDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + dFromDateDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + dToDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "'))";
                    sDateRange = "Date: " + dFromDateDate.ToString("dd MMM yyyy");
                }
                else if (ncboOrderDate == (int)EnumCompareOperator.Between)
                {
                    sDateRange = "Date: From " + dFromDateDate.ToString("dd MMM yyyy") + " To " + dToDate.ToString("dd MMM yyyy");
                    dToDate = dToDate.AddDays(1);
                    sReturn = sReturn + "((StartTime>='" + dFromDateDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + dToDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + dFromDateDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + dToDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "'))";
                    
                }
            }
            #endregion

            #region sExeNo
            if (!string.IsNullOrEmpty(sExeNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExeNo LIKE '%" + sExeNo + "%' ";
            }
            #endregion

            #region Construction
            if (!string.IsNullOrEmpty(sConstruction))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Construction LIKE '%" + sConstruction + "%' ";
            }
            #endregion

            #region Machine
            if (!string.IsNullOrEmpty(sMachineIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MachineID IN (" + sMachineIDs + ") ";
            }
            #endregion

            #region Customer
            if (!string.IsNullOrEmpty(sCustomerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + sCustomerIDs + ") ";
            }
            #endregion

            #region Design
            if (nDesign > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricWeave = " + nDesign;
            }
            #endregion

            #region Plan Status
            if (nPlanStatus > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PlanStatus = " + nPlanStatus;
            }
            #endregion

            #region Is Pending
            if (bIsPending)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PlanStatus = 1 ";
            }
            #endregion

            #region BatchNo
            if (!string.IsNullOrEmpty(sBatchNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BatchNo LIKE '%" + sBatchNo + "%' ";
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn + "  order by FMID,StartTime";
            return sSQL;
        }
        #endregion
        public ActionResult PrintFabricWarpingPlan_Machine(string sTemp)
        {
            List<FabricSizingPlanDetail> oFabricSizingPlanDetails = new List<FabricSizingPlanDetail>();
            string sTemps = "";
            _oFabricSizingPlan.ErrorMessage = sTemp;

            string sSQL = MakeSQL(_oFabricSizingPlan);
            _oFabricSizingPlans = FabricSizingPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sTemps = string.Join(",", _oFabricSizingPlans.Select(x => x.FabricSizingPlanID).ToList());
            if (!string.IsNullOrEmpty(sTemps))
            {
                sSQL = "Select * from View_FabricSizingPlanDetail where FabricSizingPlanID in (" + sTemps + ")";
                _oFabricSizingPlanDetails = FabricSizingPlanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricSizingPlans.ForEach(x =>
                {
                    if (_oFabricSizingPlanDetails.FirstOrDefault() != null && _oFabricSizingPlanDetails.FirstOrDefault().FabricSizingPlanID > 0 && _oFabricSizingPlanDetails.Where(b => b.FabricSizingPlanID == x.FabricSizingPlanID).Count() > 0)
                    {
                        oFabricSizingPlanDetails = _oFabricSizingPlanDetails.Where(p => p.FabricSizingPlanID == x.FabricSizingPlanID).ToList();

                        sTemps = string.Join("+", oFabricSizingPlanDetails.Select(m => m.FabricMachineTypeName+ " " + m.BeamName).ToList());
                        x.BeamName = sTemps;
                        sTemps = string.Join("+", oFabricSizingPlanDetails.Select(m => m.SizingBeamNo).ToList());
                        x.SzingBeam = sTemps;

                    }
                });
            }


            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
           

            string sTitle = "SIZING PROGRAM";
            rptFabricSizingPlan oReport = new rptFabricSizingPlan();
            byte[] abytes = oReport.PrepareReport_SzingPlane(_oFabricSizingPlans, oCompany, oBusinessUnit, sTitle, sDateRange);
            return File(abytes, "application/pdf");
        }

        public void ExcelFabricWarpingPlan_Machine(string sTemp)
        {
            List<FabricSizingPlanDetail> oFabricSizingPlanDetails = new List<FabricSizingPlanDetail>();
            string sTemps = "";
            _oFabricSizingPlan.ErrorMessage = sTemp;
            string sSQL = MakeSQL(_oFabricSizingPlan);
            _oFabricSizingPlans = FabricSizingPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sTemps = string.Join(",", _oFabricSizingPlans.Select(x => x.FabricSizingPlanID).ToList());
            if (!string.IsNullOrEmpty(sTemps))
            {
                sSQL = "Select * from View_FabricSizingPlanDetail where FabricSizingPlanID in (" + sTemps + ")";
                _oFabricSizingPlanDetails = FabricSizingPlanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricSizingPlans.ForEach(x =>
                {
                    if (_oFabricSizingPlanDetails.FirstOrDefault() != null && _oFabricSizingPlanDetails.FirstOrDefault().FabricSizingPlanID > 0 && _oFabricSizingPlanDetails.Where(b => b.FabricSizingPlanID == x.FabricSizingPlanID).Count() > 0)
                    {
                        oFabricSizingPlanDetails = _oFabricSizingPlanDetails.Where(p => p.FabricSizingPlanID == x.FabricSizingPlanID).ToList();

                        sTemps = string.Join("+", oFabricSizingPlanDetails.Select(m => m.FabricMachineTypeName + " " + m.BeamName).ToList());
                        x.BeamName = sTemps;
                        sTemps = string.Join("+", oFabricSizingPlanDetails.Select(m => m.SizingBeamNo).ToList());
                        x.SzingBeam = sTemps;
                    }
                });
            }
            string sTitle = "SIZING PROGRAM";
            if (_oFabricSizingPlans.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();

                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Customer", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Construction", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Design", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Warp Color", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Weft Color", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Reed No", Width = 8f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "R. Width", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Warp Length", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Warp Beam", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Sizing Beam", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Beam Dia", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Priority", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "L/F", Width = 12f, IsRotate = false });
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("SIZING PROGRAM");
                    
                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    #endregion
                    nRowIndex++;
                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = sTitle; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 1;

                    if (!string.IsNullOrEmpty(sDateRange))
                    {
                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                        cell.Value = sDateRange; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex += 1;
                    }
                    
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = "Grand Total: " + _oFabricSizingPlans.Sum(x => x.Qty); cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 1;
                   
                    #region Data
                    nRowIndex++;

                    var oMachaines = _oFabricSizingPlans.GroupBy(x => new { x.FMID, x.MachineName }, (key, grp) => new
                    {
                        MachineName = key.MachineName,
                        FMID = key.FMID
                    }).ToList();
                    int nSL = 0;
                    foreach (var oItem in oMachaines)
                    {
                        nStartCol = 2;
                        var oFabricSizingPlans = _oFabricSizingPlans.Where(x => x.FMID == oItem.FMID).ToList();
                        if (oMachaines.Count > 0)
                        {
                            ExcelTool.FillCellMerge(ref sheet, oItem.MachineName, nRowIndex, nRowIndex++, nStartCol, nEndCol+1, true, ExcelHorizontalAlignment.Left, false);
                        }

                        #region Column Header
                        nStartCol = 2;
                        foreach (TableHeader listItem in table_header)
                        {
                            cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        nRowIndex++;
                        #endregion

                        #region DATA
                        nSL = 0;
                        oFabricSizingPlans = oFabricSizingPlans.OrderBy(o => o.Sequence).ToList();
                        foreach (var oItem1 in oFabricSizingPlans)
                        {
                            nSL++; nStartCol = 2;
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, nSL.ToString(), false, ExcelHorizontalAlignment.Center, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem1.ContractorName, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem1.Construction, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem1.ExeNo, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (string.IsNullOrEmpty(oItem1.Weave) ? "" : oItem1.Weave), false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, ((oItem1.WarpColor) > 0 ? oItem1.WarpColor.ToString() : ""), false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, ((oItem1.WeftColor) > 0 ? oItem1.WeftColor.ToString() : ""), false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (string.IsNullOrEmpty(oItem1.ReedNo) ? oItem1.ReedNo : ""), false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, ((oItem1.REEDWidth) > 0 ? oItem1.REEDWidth.ToString() : ""), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem1.Qty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (oItem1.WarpBeam != "" ? oItem1.WarpBeam.ToString() : ""), false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem1.SzingBeam, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem1.BeamName, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem1.PrioritySt, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem1.LF, false, ExcelHorizontalAlignment.Left, false, false);

                            nRowIndex++;
                        }
                        
                        #endregion

                        #region Total
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "Total ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, oFabricSizingPlans.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), true, ExcelHorizontalAlignment.Right, true, false);
                        ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex++, ++nStartCol, nStartCol += 4, true, ExcelHorizontalAlignment.Right, false);
                        #endregion
                        //nRowIndex++;

                    }


                    cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=SIZING_PROGRAM.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }

        }

        public ActionResult PrintChemicalPlan(int nFabricSizingPlanID, int nBUID)
        {
            _oFabricSizingPlan = new FabricSizingPlan();
            List<FabricChemicalPlan> oFabricChemicalPlans = new List<FabricChemicalPlan>();
            List<FabricBatchRawMaterial> oFabricBatchRawMaterials = new List<FabricBatchRawMaterial>();
            Company oCompany = new Company();
            FabricBatchProduction oFabricBatchProduction = new FabricBatchProduction();
            List<FabricBatchProduction> oFabricBatchProductions = new List<FabricBatchProduction>();
            if (nFabricSizingPlanID > 0)
            {
                _oFabricSizingPlan = _oFabricSizingPlan.Get(nFabricSizingPlanID, (int)Session[SessionInfo.currentUserID]);
                //_oFabricSizingPlan.FabricSizingPlanDetails = FabricSizingPlanDetail.Gets(_oFabricSizingPlan.FabricSizingPlanID, (int)Session[SessionInfo.currentUserID]);
                oFabricBatchRawMaterials = FabricBatchRawMaterial.Gets("SELECT * FROM View_FabricBatchRawMaterial WHERE FBID = " + _oFabricSizingPlan.FBID, (int)Session[SessionInfo.currentUserID]);
                oFabricChemicalPlans = FabricChemicalPlan.Gets("SELECT * FROM View_FabricChemicalPlan WHERE FabricSizingPlanID=" + nFabricSizingPlanID + " AND FabricChemicalPlanID NOT IN (SELECT FabricChemicalPlanID FROM View_FabricBatchRawMaterial WHERE FBID = " + _oFabricSizingPlan.FBID + ")", (int)Session[SessionInfo.currentUserID]);

                oFabricBatchProduction = oFabricBatchProduction.GetByBatchAndWeavingType(_oFabricSizingPlan.FBID, EnumWeavingProcess.Sizing, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricBatchProduction.FBPBs1 = FabricBatchProductionBeam.GetsByFabricBatchProduction(oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricBatchProductions = new List<FabricBatchProduction>();
                string sSQL = "SELECT * FROM View_FabricBatchProduction WHERE FBID=" + _oFabricSizingPlan.FBID + " AND WeavingProcess=" + (int)EnumWeavingProcess.Warping;
                oFabricBatchProductions = FabricBatchProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFabricBatchProductions.Count > 0)
                    oFabricBatchProduction.FBPBs = FabricBatchProductionBeam.GetsByFabricBatchProduction(oFabricBatchProductions[0].FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (nBUID > 0)
            {
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            byte[] abytes;
            rptPrintChemicalPlan oReport = new rptPrintChemicalPlan();
            abytes = oReport.PrepareReport(_oFabricSizingPlan, oFabricBatchRawMaterials, oFabricChemicalPlans, oCompany, oFabricBatchProduction);
            return File(abytes, "application/pdf");
        }

    }
}
