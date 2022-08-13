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
    public class FabricWarpPlanController : PdfViewController
    {
        #region Declartion

        FabricWarpPlan _oFabricWarpPlan = new FabricWarpPlan();
        List<FabricWarpPlan> _oFabricWarpPlans = new List<FabricWarpPlan>();
        string _sSQL = "";
        string sDateRange = "";

        #endregion

        #region Actions
        public ActionResult ViewFabricWarpPlan(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricWarpPlan).ToString() + "," + ((int)EnumModuleName.FabricBatch).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            string sTemps = "";
            //List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            // oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricWarpPlan).ToString() , (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, oARMs);
            int nCount = 0;
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
            _sSQL = "Select * from View_FabricWarpPlan where ((StartTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + startOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + endOfMonth.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')) order by FMID,StartTime ";
            _oFabricWarpPlans = FabricWarpPlan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oFabricWarpPlans.Count > 0)
            {
                sTemps = string.Join(",", _oFabricWarpPlans.Select(x => x.FabricWarpPlanID).ToList());
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
            ViewBag.PlanTypes = EnumObject.jGets(typeof(EnumPlanType));
            return View(_oFabricWarpPlans);
        }
     
        [HttpPost]
        public JsonResult GetPS(FabricWarpPlan oFabricWarpPlan)
        {
            _sSQL = "";
            FabricMachine oFabricMachine = new FabricMachine();
             _oFabricWarpPlan = new FabricWarpPlan();
           
            if(oFabricWarpPlan.FabricWarpPlanID>0)
            {
              _oFabricWarpPlan = _oFabricWarpPlan.Get(oFabricWarpPlan.FabricWarpPlanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
              
            }
            else
            {
                 List<FabricWarpPlan> oPSs = new List<FabricWarpPlan>();
                 oFabricMachine = oFabricMachine.Get(oFabricWarpPlan.FMID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 _sSQL = "Select * from View_FabricWarpPlan Where FMID=" + oFabricWarpPlan.FMID + " and " + " EndTime = (Select MAX(EndTime) from FabricWarpPlan Where FMID=" + oFabricWarpPlan.FMID + ")";
                 oPSs = FabricWarpPlan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
                if (oPSs.Count > 0)
                {
                    _oFabricWarpPlan.StartTime=oPSs[0].EndTime.AddMinutes(1);
                }
                _oFabricWarpPlan.FMID = oFabricMachine.FMID;
                //_oFabricWarpPlan.MachineID = oMachine.MachineID;
                _oFabricWarpPlan.MachineName = oFabricMachine.Name;
                //_oFabricWarpPlan.MachineNo = oMachine.Code;
                //_oFabricWarpPlan.Capacity = oMachine.Capacity;
                //_oFabricWarpPlan.BUID = oMachine.BUID;
            }
          
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oFabricWarpPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsFabricExecutionSpe(FabricWarpPlan oFabricWarpPlan)// For Delivery Order
        {
            string sSQL = "";
            List<FabricWarpPlan> oFabricWarpPlans = new List<FabricWarpPlan>();
            List<FabricExecutionOrderSpecification> oFEOSs = new List<FabricExecutionOrderSpecification>();
            FabricWarpPlan oFsp = new FabricWarpPlan();
            try
            {
                string sReturn = "";

                 List<RPT_Dispo> oRPT_Dispos = new List<RPT_Dispo>();
                 //#region DispoNo
                 if (!string.IsNullOrEmpty(oFabricWarpPlan.ExeNo))
                 {
                     Global.TagSQL(ref sReturn);
                     //  sReturn = sReturn + "ExeNo like '%" + ExeNo + "%'";
                     sReturn = sReturn + "FSCDID in (Select dd.FabricsalesContractDetailID from FabricsalesContractDetail as dd where ExeNo like '%" + oFabricWarpPlan.ExeNo + "%')";
                 }
                 if (oFabricWarpPlan.PlanType == EnumPlanType.General)
                 {
                     Global.TagSQL(ref sReturn);
                     sReturn = sReturn + "FEOSID in (select FEOSID from (Select FEOS.FEOSID,FEOS.RequiredWarpLength,isnull((Select sum( FP.Qty) from FabricWarpPlan as FP where  PlanStatus<>" + (int)EnumFabricPlanStatus.Cancel + " and  FEOSID=FEOS.FEOSID ),0) as QtyPlane from FabricExecutionOrderSpecification as FEOS  where  ProdtionType in (" + (int)EnumDispoProType.General + "," + (int)EnumDispoProType.ExcessFabric + ") and FEOS.FEOSID in (Select FEOSID from FabricExecutionOrderSpecificationDetail where FEOSDID >0)) as HH where HH.RequiredWarpLength>HH.QtyPlane)";
                 }
                 else
                 {
                     Global.TagSQL(ref sReturn);
                     sReturn = sReturn + "FEOSID in (select FEOSID from (Select FEOS.FEOSID,FEOS.RequiredWarpLength,ReqWarpLenLB, isnull((Select sum( FP.Qty) from FabricWarpPlan as FP where  PlanStatus<>" + (int)EnumFabricPlanStatus.Cancel + " and  FEOSID=FEOS.FEOSID ),0) as QtyPlane from FabricExecutionOrderSpecification as FEOS  where  ProdtionType in (" + (int)EnumDispoProType.General + "," + (int)EnumDispoProType.ExcessFabric + ") and FEOS.FEOSID in (Select FEOSID from FabricExecutionOrderSpecificationDetail where FEOSDID >0)) as HH where HH.RequiredWarpLength>HH.QtyPlane AND ISNULL(HH.ReqWarpLenLB,0) > 0) ";
                 }

                 oRPT_Dispos = RPT_Dispo.Gets_Weaving(sReturn, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (RPT_Dispo oItem in oRPT_Dispos)
                    {
                        oFsp = new FabricWarpPlan();
                        oFsp.FabricWarpPlanID =0;
                        oFsp.FEOSID = oItem.FEOSID;
                        oFsp.FSCDID = oItem.FSCDID;
                        oFsp.FMID = oFabricWarpPlan.FMID;
                        oFsp.ExeNo = oItem.ExeNo;
                        oFsp.ContractorName = oItem.BuyerName;
                        oFsp.Construction = oItem.Construction;
                        oFsp.Composition = oItem.ProductName;
                        oFsp.BatchNo = "";
                        oFsp.Weave = oItem.FabricWeaveName;                        
                        oFsp.GreigeDemand = oItem.ReqGreyFabrics;
                        oFsp.OrderQty = oItem.Qty_Order;
                        oFsp.TFLength = oItem.WarpLengthRecd;
                        if (oFabricWarpPlan.PlanType == EnumPlanType.General)
                        {
                            oFsp.RequiredWarpLength = oItem.WarpLength;
                            oFsp.Qty_FWP = oItem.Qty_FWP;
                            oFsp.Qty = oItem.WarpLength - oItem.Qty_FWP;
                        }
                        else
                        {
                            oFsp.RequiredWarpLength = oItem.RequiredWarpLengthLB;
                            oFsp.Qty_FWP = oItem.Qty_FWP_LB;
                            oFsp.Qty = oItem.RequiredWarpLengthLB - oItem.Qty_FWP_LB;
                        }
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
                        oFabricWarpPlans.Add(oFsp);
                    }
               

            }
            catch (Exception ex)
            {
                oFabricWarpPlans = new List<FabricWarpPlan>();
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oFabricWarpPlans);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(oFabricWarpPlans, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetsOrderDetails(FabricWarpPlan oFabricWarpPlan)
        {
            List<FabricExecutionOrderSpecificationDetail> oFEOSDs = new List<FabricExecutionOrderSpecificationDetail>();
            
            List<FabricExecutionOrderYarnReceive> oFabricEOYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            try
            {
                if (oFabricWarpPlan.FEOSID > 0)
                {
                    oFEOSDs = FabricExecutionOrderSpecificationDetail.Gets(oFabricWarpPlan.FEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oFEOSDs.Count > 0)
                    {
                        oFabricWarpPlan.WarpColor = oFEOSDs.Where(x => x.IsWarp == true).ToList().Count;
                        oFabricWarpPlan.WeftColor = oFEOSDs.Where(x => x.IsWarp == false).ToList().Count;
                    }

                    _sSQL = "SELECT * FROM View_FabricExecutionOrderYarnReceive as FF WHERE FEOSID=" + oFabricWarpPlan.FEOSID + " order by WYRequisitionID";
                    oFabricEOYarnReceives = FabricExecutionOrderYarnReceive.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);

                    oFabricEOYarnReceives = oFabricEOYarnReceives.GroupBy(x => new { x.WYRequisitionID, x.TFLength }, (key, grp) =>
                                                    new FabricExecutionOrderYarnReceive
                                                    {
                                                        WYRequisitionID = key.WYRequisitionID,
                                                        TFLength = key.TFLength
                                                    }).ToList();
                    oFabricWarpPlan.TFLength = oFabricEOYarnReceives.Sum(p => p.TFLength);

                }
            }
            catch (Exception ex)
            {
                oFabricWarpPlan = new  FabricWarpPlan();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricWarpPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsMachine(FabricMachine oMachine)// For Delivery Order
        {
            string sSQL = "";
            List<FabricMachine> oMachines = new List<FabricMachine>();
            try
            {
                 //AND WeavingProcess = " + (int)EnumWeavingProcess.Warping
                sSQL = "SELECT * FROM View_FabricMachine where WeavingProcess=" + (int)EnumWeavingProcess.Warping + " AND IsBeam=0 AND IsActive = 1";
                string sReturn = " ";
                #region MachineName
                if (oMachine.Name != null && oMachine.Name != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Name LIKE '%" + oMachine.Name + "%'";
                }
                #endregion
                #region Type
                if (oMachine.ErrorMessage != null && oMachine.ErrorMessage != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WeavingProcess = " + oMachine.ErrorMessage.ToString();
                }
                #endregion

              
                sSQL = sSQL + "" + sReturn + "  Order By Code";

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
                sSQL = "SELECT * FROM View_Contractor WHERE ContractorID IN (SELECT BuyerID FROM View_FabricWarpPlan) ";//
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
        public JsonResult GetBeams(Machine oMachine)// For Delivery Order
        {
            string sSQL = "";
            List<FabricMachine> oMachines = new List<FabricMachine>();
            try
            {
                sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 AND isnull(IsBeam,0)=1 and WeavingProcess = " + (int)EnumWeavingProcess.Loom + "";
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
     
        [HttpGet]
        public JsonResult GetPSByMachine(int nFMID, double nts)
        {
            string sSQL = "Select top(1)* from View_FabricWarpPlan Where FMID=" + nFMID + "  Order by StartTime  DESC";

            List<FabricWarpPlan> oPSs = new List<FabricWarpPlan>();
            FabricWarpPlan oPS = new FabricWarpPlan();
            string sStartTime = "";
            try
            {
                oPSs = FabricWarpPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult ScheduleInTimePeriod(FabricWarpPlan oFabricWarpPlan, double nts)
        {

            int nScheduleCount = NumberofScheduleInTimePeriod(oFabricWarpPlan);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(nScheduleCount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public int NumberofScheduleInTimePeriod(FabricWarpPlan oFabricWarpPlan)
        {
            int nScheduleCount = 0;
            DateTime dStartTime = oFabricWarpPlan.StartTime;
            DateTime dEndTime = oFabricWarpPlan.EndTime.AddMinutes(-1);
            //int nLocationID = oFabricWarpPlan.LocationID;
            int nMachineID = oFabricWarpPlan.FMID;
            string sSQL = "Select * from View_FabricWarpPlan where EndTime >= '" + dStartTime.ToString("dd MMM yyyy HH:mm") + "' and StartTime <='" + dEndTime.ToString("dd MMM yyyy HH:mm") + "'  and FMID=" + nMachineID + " and FabricWarpPlanID NOT IN (" + oFabricWarpPlan.FabricWarpPlanID + ")";
            if (nMachineID > 0)
            {
                List<FabricWarpPlan> oPS = new List<FabricWarpPlan>();
                oPS = FabricWarpPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oPS.Count() > 0)
                {
                    if (oFabricWarpPlan.StartTime <= oPS.Min(x => x.EndTime))
                    {
                        nScheduleCount = oPS.Count();
                    }
                }

            }
            return nScheduleCount;
        }
        #endregion

        [HttpPost]
        public JsonResult Save(FabricWarpPlan oFabricWarpPlan)
        {
            try
            {
                _oFabricWarpPlan = oFabricWarpPlan.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                //_oFabricWarpPlan.FabricWarpPlanDetails = FabricWarpPlanDetail.Gets(_oFabricWarpPlan.FabricWarpPlanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
                sDateRange = _oFabricWarpPlan.StartTime.ToString("HH:mm tt") + "-" + _oFabricWarpPlan.EndTime.ToString("HH:mm tt");

                 RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
                 oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oFabricWarpPlan = new FabricWarpPlan();
                _oFabricWarpPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricWarpPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveRS(FabricWarpPlan oFabricWarpPlan)
        {
            RouteSheet oRouteSheet = new RouteSheet();
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            try
            {
                oRouteSheet = oRouteSheet.GetByPS(oFabricWarpPlan.FabricWarpPlanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            
            }
            catch (Exception ex)
            {
                _oFabricWarpPlan = new FabricWarpPlan();
                _oFabricWarpPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricWarpPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdatePlanStatus(FabricWarpPlan oFabricWarpPlan)
        {
            _oFabricWarpPlan = new FabricWarpPlan();
            try
            {
                //if ((int)oFabricWarpPlan.Status > (int)EnumFabricBatchState.InFloor && (int)oFabricWarpPlan.PlanStatus == (int)EnumFabricPlanStatus.Hold)
                //{
                //    throw new Exception("Hold is not possible, Because Batch status is " + oFabricWarpPlan.StatusSt);
                //}
                _oFabricWarpPlan = oFabricWarpPlan.UpdatePlanStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricWarpPlan = new FabricWarpPlan();
                _oFabricWarpPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricWarpPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult Delete(FabricWarpPlan oFabricWarpPlan)
        {
            try
            {
                if (oFabricWarpPlan.FabricWarpPlanID <= 0) { throw new Exception("Please select an valid item."); }
                oFabricWarpPlan.ErrorMessage = oFabricWarpPlan.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricWarpPlan = new FabricWarpPlan();
                oFabricWarpPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricWarpPlan.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       

        [HttpPost]
        public JsonResult SwapSchedule(string sPSIDs, double nts)
        {
          
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

                FabricWarpPlan oFabricWarpPlan = new FabricWarpPlan();
                FabricWarpPlan oFabricWarpPlanOne = new FabricWarpPlan();
                FabricWarpPlan oFabricWarpPlanTwo = new FabricWarpPlan();
                oFabricWarpPlanOne = oFabricWarpPlanOne.Get(nDUpScID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricWarpPlanTwo = oFabricWarpPlanTwo.Get(nDUpScIDTwo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                /// Mapping One
                oFabricWarpPlan.FabricWarpPlanID = oFabricWarpPlanOne.FabricWarpPlanID;
                oFabricWarpPlan.FMID = oFabricWarpPlanTwo.FMID;
                //oFabricWarpPlan.LocationID = oFabricWarpPlanTwo.LocationID;
                oFabricWarpPlanOne.EndTime = oFabricWarpPlanOne.EndTime.AddMinutes(1);
                oTimeSpan = oFabricWarpPlanOne.EndTime - oFabricWarpPlanOne.StartTime;
                oFabricWarpPlan.StartTime = oFabricWarpPlanTwo.StartTime;
                oFabricWarpPlan.EndTime = oFabricWarpPlanTwo.StartTime + oTimeSpan;
                _oFabricWarpPlans.Add(oFabricWarpPlan);

                ///Mapping Two
                oFabricWarpPlan = new FabricWarpPlan();
                oFabricWarpPlan.FabricWarpPlanID = oFabricWarpPlanTwo.FabricWarpPlanID;
                oFabricWarpPlan.FMID = oFabricWarpPlanOne.FMID;
                //oFabricWarpPlan.LocationID = oFabricWarpPlanOne.LocationID;
                oFabricWarpPlanTwo.EndTime = oFabricWarpPlanTwo.EndTime.AddMinutes(1);
                oTimeSpan = oFabricWarpPlanTwo.EndTime - oFabricWarpPlanTwo.StartTime;
                oFabricWarpPlan.StartTime = oFabricWarpPlanOne.StartTime;
                oFabricWarpPlan.EndTime = oFabricWarpPlanOne.StartTime + oTimeSpan;
                _oFabricWarpPlans.Add(oFabricWarpPlan);

              //  oFabricWarpPlanDetails = FabricWarpPlanDetail.Swap(_oFabricWarpPlans, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                FabricWarpPlan oFabricWarpPlan = new FabricWarpPlan();
                oFabricWarpPlan.ErrorMessage = ex.Message;
                _oFabricWarpPlans.Add(oFabricWarpPlan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricWarpPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PastSchedule(FabricWarpPlan oFabricWarpPlan)
        {
           
            TimeSpan oTimeSpan = new TimeSpan();
            int nDUpScID = 0;
            int nDUpScIDTwo = 0;
            string sTime = "";
            string sPSIDs = oFabricWarpPlan.Params;

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
                }
                //if (nDUpScID <= 0) { throw new Exception("Please select an valid item."); }
                if (nDUpScIDTwo <= 0) { throw new Exception("Please select an valid item."); }

                //FabricWarpPlan oFabricWarpPlan = new FabricWarpPlan();
                FabricWarpPlan oFabricWarpPlanOne = new FabricWarpPlan();
                oFabricWarpPlanOne = oFabricWarpPlan;
                FabricWarpPlan oFabricWarpPlanTwo = new FabricWarpPlan();
                oFabricWarpPlanOne.EndTime = oDateTime;
                if (nDUpScID > 0)
                {
                    oFabricWarpPlanOne = oFabricWarpPlanOne.Get(nDUpScID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                oFabricWarpPlanTwo = oFabricWarpPlanTwo.Get(nDUpScIDTwo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                /// Mapping One
              
                ///Mapping Two
                //oFabricWarpPlan = new FabricWarpPlan();
                oFabricWarpPlanTwo.FabricWarpPlanID = oFabricWarpPlanTwo.FabricWarpPlanID;
                oFabricWarpPlanTwo.FMID = oFabricWarpPlanOne.FMID;
                //oFabricWarpPlanTwo.LocationID = oFabricWarpPlanOne.LocationID;

                oTimeSpan = oFabricWarpPlanTwo.EndTime - oFabricWarpPlanTwo.StartTime;
                oFabricWarpPlanTwo.StartTime = oFabricWarpPlanOne.EndTime.AddMinutes(1);
                oFabricWarpPlanTwo.EndTime = oFabricWarpPlanTwo.StartTime + oTimeSpan.Duration();
                _oFabricWarpPlans.Add(oFabricWarpPlanTwo);

                _oFabricWarpPlans = FabricWarpPlan.Swap(_oFabricWarpPlans, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                FabricWarpPlan oFabricWarpPlanTemp = new FabricWarpPlan();
                oFabricWarpPlanTemp.ErrorMessage = ex.Message;
                _oFabricWarpPlans.Add(oFabricWarpPlanTemp);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricWarpPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SwapTwoSchedule(FabricWarpPlan oFabricWarpPlan)
        {
            TimeSpan oTimeSpan = new TimeSpan();
            int nDUpScID = 0;
            int nDUpScIDTwo = 0;
            string sTime = "";
            string sPSIDs = oFabricWarpPlan.Params;

            DateTime oDateTime = DateTime.MinValue;
            try
            {
                nDUpScID = Convert.ToInt32(sPSIDs.Split('~')[0]);
                nDUpScIDTwo = Convert.ToInt32(sPSIDs.Split('~')[1]);

                if (nDUpScID <= 0 || nDUpScIDTwo <= 0) { throw new Exception("Please select valid items."); }

                FabricWarpPlan oFabricWarpPlanOne = new FabricWarpPlan();
                FabricWarpPlan oFabricWarpPlanTwo = new FabricWarpPlan();

                oFabricWarpPlanOne = oFabricWarpPlanOne.Get(nDUpScID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricWarpPlanTwo = oFabricWarpPlanTwo.Get(nDUpScIDTwo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                /// Mapping
                DateTime dStartTime1 = oFabricWarpPlanOne.StartTime;
                DateTime dStartTime2 = oFabricWarpPlanTwo.StartTime;
                DateTime dEndTime1 = oFabricWarpPlanOne.EndTime;
                DateTime dEndTime2 = oFabricWarpPlanTwo.EndTime;

                oFabricWarpPlanOne.StartTime = dStartTime2;
                oFabricWarpPlanTwo.StartTime = dStartTime1;

                oFabricWarpPlanOne.EndTime = dStartTime2 + (dEndTime1 - dStartTime1).Duration();
                oFabricWarpPlanTwo.EndTime = dStartTime1 + (dEndTime2 - dStartTime2).Duration();

                _oFabricWarpPlans.Add(oFabricWarpPlanOne);
                _oFabricWarpPlans.Add(oFabricWarpPlanTwo);
                _oFabricWarpPlans = FabricWarpPlan.Swap(_oFabricWarpPlans, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                FabricWarpPlan oFabricWarpPlanTemp = new FabricWarpPlan();
                oFabricWarpPlanTemp.ErrorMessage = ex.Message;
                _oFabricWarpPlans.Add(oFabricWarpPlanTemp);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricWarpPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult GetPSByDU(FabricWarpPlan oFabricWarpPlanDetail)
        //{
        //    _sSQL = "";
        //    Machine oMachine = new Machine();
        //    List<FabricWarpPlan> _oFabricWarpPlans = new List<FabricWarpPlan>();
        //    if (oFabricWarpPlanDetail.FSPDID > 0)
        //    {
        //        List<FabricWarpPlan> oPSs = new List<FabricWarpPlan>();
        //       // _sSQL = "SELECT * FROM FabricWarpPlan WHERE FabricWarpPlanID IN (SELECT FabricWarpPlanID FROM FabricWarpPlanDetail WHERE DODID =" + oFabricWarpPlanDetail.DODID + ")";
        //        _oFabricWarpPlans = FabricWarpPlan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
            
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oFabricWarpPlans);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
       


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
        public JsonResult AdvSearch(FabricWarpPlan oFabricWarpPlan)
        {
            _oFabricWarpPlans = new List<FabricWarpPlan>();
            string sTemps = "";
            try
            {
                string sSQL = MakeSQL(oFabricWarpPlan);
                _oFabricWarpPlans = FabricWarpPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
                
            }
            catch (Exception ex)
            {
                _oFabricWarpPlans = new List<FabricWarpPlan>();
                oFabricWarpPlan.ErrorMessage = ex.Message;
                _oFabricWarpPlans.Add(oFabricWarpPlan);
            }
            var jsonResult = Json(_oFabricWarpPlans, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(FabricWarpPlan oFabricWarpPlan)
        {
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            string sParams = oFabricWarpPlan.ErrorMessage;

            int ncboOrderDate = 0, nDesign = 0, nPlanStatus = 0;
            DateTime dFromDateDate = DateTime.Today;
            DateTime dToDate = DateTime.Today;
            string sExeNo = "", sConstruction = "", sMachineIDs = "", sCustomerIDs = "", sBatchNo = "";
            bool bIsPending = false;
            string sReturn1 = "";
            string sReturn = "";
            sReturn1 = "Select * from View_FabricWarpPlan";
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
                sReturn1 = "Select * from View_FabricWarpPlan where ((StartTime>='" + dFromDateDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and StartTime<='" + dToDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "') or (EndTime>='" + dFromDateDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and EndTime<='" + dToDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "')) ";

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
        public ActionResult PrintFabricWarpingPlan_Machine(string sTemp)
        {
            FabricWarpPlan oFabricWarpPlan = new FabricWarpPlan();
            oFabricWarpPlan.ErrorMessage = sTemp;
            string sSQL = MakeSQL(oFabricWarpPlan);
            _oFabricWarpPlans = FabricWarpPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            //string sDateRange = "Date: " + dFromDate.ToString("dd MMM yyyy");
            //if (dFromDate != dToDate)
            //{
            //    sDateRange = "Date: " + dFromDate.ToString("dd MMM yyyy") + " to " + dToDate.ToString("dd MMM yyyy");
            //}

            string sTitle = "WARPING PROGRAM";
            rptFabricWarpingPlan oReport = new rptFabricWarpingPlan();
            byte[] abytes = oReport.PrepareReport_WarpingPlaneTwo(_oFabricWarpPlans, oCompany, oBusinessUnit, sTitle, sDateRange);
            return File(abytes, "application/pdf");
        }

        public void ExcelFabricWarpingPlan_Machine(string sTemp)
        {
            FabricWarpPlan oFabricWarpPlan = new FabricWarpPlan();
            oFabricWarpPlan.ErrorMessage = sTemp;
            string sSQL = MakeSQL(oFabricWarpPlan);
            _oFabricWarpPlans = FabricWarpPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            string sTitle = "WARPING PROGRAM";
            if (_oFabricWarpPlans.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();

                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Customer", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Construction", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Composition", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Design", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Warp Color", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Weft Color", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Reed No", Width = 8f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "R/S", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Warp Length", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Warp Beam", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Priority", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Remarks", Width = 12f, IsRotate = false });
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("WARPING PROGRAM");

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

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = sDateRange; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 2;

                    #region Data
                    nRowIndex++;

                    var oMachaines = _oFabricWarpPlans.GroupBy(x => new { x.FMID, x.MachineName, x.MachineCode }, (key, grp) => new
                                      {
                                          MachineName = key.MachineName,
                                          MachineCode = key.MachineCode,
                                          FMID = key.FMID,
                                      }).ToList();
                    int nSL = 0;
                    foreach (var oItem in oMachaines)
                    {
                        nStartCol = 2;
                        var oFabricWarpingPlanDetails = _oFabricWarpPlans.Where(x => x.FMID == oItem.FMID).ToList();
                        if (oMachaines.Count > 0)
                        {
                            ExcelTool.FillCellMerge(ref sheet, oItem.MachineName + "" + (string.IsNullOrEmpty(oItem.MachineCode) ? "" : oItem.MachineCode), nRowIndex, nRowIndex++, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left, false);
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
                        oFabricWarpingPlanDetails = oFabricWarpingPlanDetails.OrderBy(o => o.StartTime).ToList();
                        foreach (var oItem1 in oFabricWarpingPlanDetails)
                        {
                            nSL++; nStartCol = 2;
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, nSL.ToString(), false, ExcelHorizontalAlignment.Center, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem1.ContractorName, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem1.Construction, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem1.Composition, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem1.ExeNo, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (string.IsNullOrEmpty(oItem1.Weave) ? "" : oItem1.Weave), false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, ((oItem1.WarpColor) > 0 ? oItem1.WarpColor.ToString() : ""), false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, ((oItem1.WeftColor) > 0 ? oItem1.WeftColor.ToString() : ""), false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (!string.IsNullOrEmpty(oItem1.ReedNo) ? oItem1.ReedNo : ""), false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, ((oItem1.REEDWidth) > 0 ? oItem1.REEDWidth.ToString() : ""), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem1.Qty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (oItem1.WarpBeam != "" ? oItem1.WarpBeam.ToString() : ""), false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem1.PrioritySt, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem1.Note, false, ExcelHorizontalAlignment.Left, false, false);

                            nRowIndex++;
                        }
                        #endregion

                        #region Total
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "Total", nRowIndex, nRowIndex, nStartCol, nStartCol+=9, true, ExcelHorizontalAlignment.Right, false);
                        ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                        ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, oFabricWarpingPlanDetails.Where(x => x.FMID == oItem.FMID).Select(c => c.Qty).Sum().ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nEndCol+1, true, ExcelHorizontalAlignment.Right, false);
                        nRowIndex++;
                        #endregion

                    }

                    #region Grand Total
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Grand Total", nRowIndex, nRowIndex, nStartCol, nStartCol += 9, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFabricWarpPlans.Select(c => c.Qty).Sum().ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Right, false);
                    nRowIndex++;
                    #endregion
                    

                    cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=WARPING_PROGRAM.xlsx");
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
