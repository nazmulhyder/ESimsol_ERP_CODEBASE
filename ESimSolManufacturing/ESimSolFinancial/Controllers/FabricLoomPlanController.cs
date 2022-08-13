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
    public class FabricLoomPlanController : PdfViewController
    {
        #region Declartion

        FabricLoomPlan _oFabricLoomPlan = new FabricLoomPlan();
        List<FabricLoomPlan> _oFabricLoomPlans = new List<FabricLoomPlan>();
        string _sSQL = "";
        string sDateRange = "";

        #endregion

        #region Actions
        public ActionResult ViewFabricLoomPlan(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricLoomPlan).ToString() + "," + ((int)EnumModuleName.FabricBatch).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            string sTemps = "";
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            List<FabricMachine> oFabricMachines = new List<FabricMachine>();


            _sSQL = "Select * from View_FabricLoomPlan where PlanStatus in (" + (int)EnumFabricPlanStatus.Initialize + "," + (int)EnumFabricPlanStatus.Planned + ") ORDER BY  (CONVERT(bigint, dbo.udf_GetNumeric(MachineCode))) ASC ";
            //_sSQL = "Select * from View_FabricLoomPlan";
            _oFabricLoomPlans = FabricLoomPlan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oFabricLoomPlans.Count > 0)
            {
                sTemps = string.Join(",", _oFabricLoomPlans.Select(x => x.FMID).Distinct().ToList());
            }

            if (string.IsNullOrEmpty(sTemps)) { sTemps = "0"; }

            _sSQL = "SELECT * FROM View_FabricMachine WHERE FMID not in (" + sTemps + ") and IsActive=1 AND isnull(IsBeam,0)=0 and WeavingProcess = " + (int)EnumWeavingProcess.Loom + " ORDER BY  (CONVERT(bigint, dbo.udf_GetNumeric(Code))) ASC";
            oFabricMachines = FabricMachine.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            foreach (FabricMachine oItem in oFabricMachines)
            {
                _oFabricLoomPlan = new FabricLoomPlan();
                _oFabricLoomPlan.FMID = oItem.FMID;
                _oFabricLoomPlan.MachineCode = oItem.Code;
                _oFabricLoomPlan.MachineName = oItem.Code + "-" +oItem.Name;
                _oFabricLoomPlans.Add(_oFabricLoomPlan);
            }
            FabricBatchLoom oFabricBatchLoom = new FabricBatchLoom();
            List<FabricBatchLoom> oFabricBatchLooms = new List<FabricBatchLoom>();
            oFabricBatchLooms = FabricBatchLoom.Gets("SELECT * FROM View_FabricBatchLoom WHERE FMID IN (SELECT FMID FROM FabricMachine WHERE MachineStatus = 1) AND [Status] <>" + (int)EnumFabricBatchStatus.Finish + " and ISNULL(BeamQty,0) > 2", ((User)Session[SessionInfo.CurrentUser]).UserID);

            foreach (FabricLoomPlan oItem in _oFabricLoomPlans)
            {
                oFabricBatchLoom = new FabricBatchLoom();
                oFabricBatchLoom = oFabricBatchLooms.Where(x => x.FMID == oItem.FMID).FirstOrDefault();
                if (oFabricBatchLoom != null)
                {
                    oItem.Const_CO = oFabricBatchLoom.Construction;
                    oItem.Shift_CO = oFabricBatchLoom.ShiftName;
                    oItem.ReedCount_CO = oFabricBatchLoom.ReedCount;
                }
                if(oItem.FLPID > 0)
                    oItem.FabricLoomPlanDetails = FabricLoomPlanDetail.Gets(oItem.FLPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            //_oFabricLoomPlans = _oFabricLoomPlans.OrderByDescending(x => x.PlanStatus).OrderBy(x => x.MachineCode).ToList();
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
            ViewBag.FabricProgrameList = EnumObject.jGets(typeof(EnumFabricPrograme));
            ViewBag.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oFabricLoomPlans);
        }
     
        [HttpPost]
        public JsonResult GetPS(FabricLoomPlan oFabricLoomPlan)
        {
            _sSQL = "";
            FabricMachine oFabricMachine = new FabricMachine();
             _oFabricLoomPlan = new FabricLoomPlan();
           
            if(oFabricLoomPlan.FLPID>0)
            {
              _oFabricLoomPlan = _oFabricLoomPlan.Get(oFabricLoomPlan.FLPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
              
            }
            else
            {
                 List<FabricLoomPlan> oPSs = new List<FabricLoomPlan>();
                 oFabricMachine = oFabricMachine.Get(oFabricLoomPlan.FMID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 _sSQL = "Select * from View_FabricLoomPlan Where FMID=" + oFabricLoomPlan.FMID + " and " + " EndTime = (Select MAX(EndTime) from FabricLoomPlan Where FMID=" + oFabricLoomPlan.FMID + ")";
                 oPSs = FabricLoomPlan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
                if (oPSs.Count > 0)
                {
                    _oFabricLoomPlan.StartTime=oPSs[0].EndTime.AddMinutes(1);
                }
                _oFabricLoomPlan.FMID = oFabricMachine.FMID;
                //_oFabricLoomPlan.MachineID = oMachine.MachineID;
                _oFabricLoomPlan.MachineName = oFabricMachine.Name;
                //_oFabricLoomPlan.MachineNo = oMachine.Code;
                //_oFabricLoomPlan.Capacity = oMachine.Capacity;
                //_oFabricLoomPlan.BUID = oMachine.BUID;
            }
          
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oFabricLoomPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsFabricExecutionSpe(FabricLoomPlan oFabricLoomPlan)// For Delivery Order
        {
            string sSQL = "";
            List<FabricLoomPlan> oFabricLoomPlans = new List<FabricLoomPlan>();
            List<FabricExecutionOrderSpecification> oFEOSs = new List<FabricExecutionOrderSpecification>();
            FabricLoomPlan oFsp = new FabricLoomPlan();
            try
            {
                string sReturn = "";

                 List<RPT_Dispo> oRPT_Dispos = new List<RPT_Dispo>();
                 //#region DispoNo
                 if (!string.IsNullOrEmpty(oFabricLoomPlan.ExeNo))
                 {
                     Global.TagSQL(ref sReturn);
                     //  sReturn = sReturn + "ExeNo like '%" + ExeNo + "%'";
                     sReturn = sReturn + "FSCDID in (Select dd.FabricsalesContractDetailID from FabricsalesContractDetail as dd where ExeNo like '%" + oFabricLoomPlan.ExeNo + "%')";
                 }
                 Global.TagSQL(ref sReturn);
                 //  sReturn = sReturn + "ExeNo like '%" + ExeNo + "%'";
                 //sReturn = sReturn + "FEOSID in (select FEOSID from (Select FEOS.FEOSID,FEOS.RequiredWarpLength,isnull((Select sum( FP.Qty) from FabricLoomPlan as FP where FEOSID=FEOS.FEOSID ),0) as QtyPlane from FabricExecutionOrderSpecification as FEOS  where  ProdtionType in (" + (int)EnumDispoProType.General + "," + (int)EnumDispoProType.ExcessFabric + ") and FEOS.FEOSID in (Select FEOSID from FabricExecutionOrderSpecificationDetail where FEOSDID in ( Select FEOSDID from FabricExecutionOrderYarnReceive where FEOSDID>0 and isnull(ReceiveBy,0)<>0))) as HH where HH.RequiredWarpLength>HH.QtyPlane)";
                 sReturn = sReturn + "FEOSID in (select FEOSID from (Select FEOS.FEOSID,FEOS.RequiredWarpLength,isnull((Select sum( FP.Qty) from FabricLoomPlan as FP where FEOSID=FEOS.FEOSID ),0) as QtyPlane from FabricExecutionOrderSpecification as FEOS  where  ProdtionType in (" + (int)EnumDispoProType.General + "," + (int)EnumDispoProType.ExcessFabric + ") and FEOS.FEOSID in (Select FEOSID from FabricExecutionOrderSpecificationDetail where FEOSDID >0)) as HH where HH.RequiredWarpLength>HH.QtyPlane)";


                 oRPT_Dispos = RPT_Dispo.Gets_Weaving(sReturn, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (RPT_Dispo oItem in oRPT_Dispos)
                    {
                        oFsp = new FabricLoomPlan();
                        oFsp.FLPID =0;
                        oFsp.FBID = oItem.FEOSID;
                        oFsp.FSCDID = oItem.FSCDID;
                        oFsp.FMID = oFabricLoomPlan.FMID;
                        oFsp.ExeNo = oItem.ExeNo;
                        oFsp.ContractorName = oItem.BuyerName;
                        oFsp.Construction = oItem.Construction;
                        oFsp.Composition = oItem.ProductName;
                        oFsp.BatchNo = "";
                        oFsp.Weave = oItem.FabricWeaveName;
                        oFsp.RequiredWarpLength = oItem.WarpLength;
                        oFsp.GreigeDemand = oItem.ReqGreyFabrics;
                        oFsp.OrderQty = oItem.Qty_Order;
                        oFsp.TFLength = oItem.WarpLengthRecd;
                        oFsp.Qty_FWP = oItem.Qty_FWP;
                        oFsp.Qty = oItem.WarpLength - oItem.Qty_FWP;
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
                        oFabricLoomPlans.Add(oFsp);
                    }
               

            }
            catch (Exception ex)
            {
                oFabricLoomPlans = new List<FabricLoomPlan>();
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oFabricLoomPlans);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(oFabricLoomPlans, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetFabricBatches(FabricBatch oFabricBatch)// For Delivery Order
        {
            List<FabricBatch> oFabricBatchs = new List<FabricBatch>();
            List<FabricBatchProductionBeam> oFBPBs = new List<FabricBatchProductionBeam>();
            try
            {
                //_sSQL = "SELECT * FROM View_FabricBatchProductionBeam ReadyLBeam WHERE FBID NOT IN (SELECT FBID FROM FabricLoomPlan) and ReadyLBeam.FBStatus != 14 AND ReadyLBeam.IsFinish=1 AND ReadyLBeam.WeavingProcessType=1  AND FBPBeamID NOT IN (SELECT FBPBeamID FROM FabricBatchLoom) ";
                //oFBPBs = FabricBatchProductionBeam.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string sSQL = "SELECT top(200)* FROM View_FabricBatch WHERE FBID NOT IN (SELECT FBID FROM FabricLoomPlan) and FBID in (SELECT FBID FROM View_FabricBatchProductionBeam ReadyLBeam WHERE  ReadyLBeam.FBStatus != 14 AND ReadyLBeam.IsFinish in (0,1) AND ReadyLBeam.WeavingProcessType=1  AND FBPBeamID NOT IN (SELECT FBPBeamID FROM FabricBatchLoom) )";
                string sReturn=" ";
                if (!string.IsNullOrEmpty(oFabricBatch.FEONo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FEONo like '%" + oFabricBatch.FEONo + "%'";
                }
                sSQL += sReturn;
                oFabricBatchs = FabricBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricBatchs = new List<FabricBatch>();
            }
            var jsonResult = Json(oFabricBatchs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetsOrderDetails(FabricLoomPlan oFabricLoomPlan)
        {
            List<FabricExecutionOrderSpecificationDetail> oFEOSDs = new List<FabricExecutionOrderSpecificationDetail>();
            
            List<FabricExecutionOrderYarnReceive> oFabricEOYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            try
            {
                if (oFabricLoomPlan.FBID > 0)
                {
                    oFEOSDs = FabricExecutionOrderSpecificationDetail.Gets(oFabricLoomPlan.FBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oFEOSDs.Count > 0)
                    {
                        oFabricLoomPlan.WeftColor = oFEOSDs.Where(x => x.IsWarp == true).ToList().Count;
                        oFabricLoomPlan.WarpColor = oFEOSDs.Where(x => x.IsWarp == false).ToList().Count;
                    }

                    _sSQL = "SELECT * FROM View_FabricExecutionOrderYarnReceive as FF WHERE FEOSID=" + oFabricLoomPlan.FBID + " order by WYRequisitionID";
                    oFabricEOYarnReceives = FabricExecutionOrderYarnReceive.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);

                    oFabricEOYarnReceives = oFabricEOYarnReceives.GroupBy(x => new { x.WYRequisitionID, x.TFLength }, (key, grp) =>
                                                    new FabricExecutionOrderYarnReceive
                                                    {
                                                        WYRequisitionID = key.WYRequisitionID,
                                                        TFLength = key.TFLength
                                                    }).ToList();
                    oFabricLoomPlan.TFLength = oFabricEOYarnReceives.Sum(p => p.TFLength);

                }
            }
            catch (Exception ex)
            {
                oFabricLoomPlan = new  FabricLoomPlan();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricLoomPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsMachine(Machine oMachine)// For Delivery Order
        {
            string sSQL = "";
            List<FabricMachine> oMachines = new List<FabricMachine>();
            try
            {
                sSQL = "SELECT * FROM View_FabricMachine WHERE IsBeam=0 AND IsActive = 1 AND WeavingProcess = " + (int)EnumWeavingProcess.Warping;
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
                sSQL = "SELECT * FROM View_Contractor WHERE ContractorID IN (SELECT BuyerID FROM View_FabricLoomPlan) ";//
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
            string sSQL = "Select top(1)* from View_FabricLoomPlan Where FMID=" + nFMID + "  Order by StartTime  DESC";

            List<FabricLoomPlan> oPSs = new List<FabricLoomPlan>();
            FabricLoomPlan oPS = new FabricLoomPlan();
            string sStartTime = "";
            try
            {
                oPSs = FabricLoomPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
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

        [HttpGet]
        public JsonResult GetPreviousConstruction(int nFMID, double nts)
        {
            FabricBatchLoom oFabricBatchLoom = new FabricBatchLoom();
            oFabricBatchLoom = FabricBatchLoom.Gets("SELECT TOP 1 * FROM View_FabricBatchloom WHERE FMID = " + nFMID + " ORDER BY EndTime DESC", ((User)Session[SessionInfo.CurrentUser]).UserID).FirstOrDefault();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchLoom);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Number of Schedules In Given Time Period.
        [HttpPost]
        public JsonResult ScheduleInTimePeriod(FabricLoomPlan oFabricLoomPlan, double nts)
        {

            int nScheduleCount = NumberofScheduleInTimePeriod(oFabricLoomPlan);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(nScheduleCount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public int NumberofScheduleInTimePeriod(FabricLoomPlan oFabricLoomPlan)
        {
            int nScheduleCount = 0;
            DateTime dStartTime = oFabricLoomPlan.StartTime;
            DateTime dEndTime = oFabricLoomPlan.EndTime.AddMinutes(-1);
            //int nLocationID = oFabricLoomPlan.LocationID;
            int nMachineID = oFabricLoomPlan.FMID;
            string sSQL = "Select * from View_FabricLoomPlan where EndTime >= '" + dStartTime.ToString("dd MMM yyyy HH:mm") + "' and StartTime <='" + dEndTime.ToString("dd MMM yyyy HH:mm") + "'  and FMID=" + nMachineID + " and FLPID NOT IN (" + oFabricLoomPlan.FLPID + ")";
            if (nMachineID > 0)
            {
                List<FabricLoomPlan> oPS = new List<FabricLoomPlan>();
                oPS = FabricLoomPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oPS.Count() > 0)
                {
                    if (oFabricLoomPlan.StartTime <= oPS.Min(x => x.EndTime))
                    {
                        nScheduleCount = oPS.Count();
                    }
                }
            }
            return nScheduleCount;
        }
        #endregion

        [HttpPost]
        public JsonResult Save(FabricLoomPlan oFabricLoomPlan)
        {
            try
            {
                _oFabricLoomPlan = oFabricLoomPlan.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricLoomPlan.FabricLoomPlanDetails = FabricLoomPlanDetail.Gets(_oFabricLoomPlan.FLPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricLoomPlan = new FabricLoomPlan();
                _oFabricLoomPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricLoomPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveMultiplePlan(List<FabricLoomPlan> oFabricLoomPlans)
        {
            try
            {
                _oFabricLoomPlans = new List<FabricLoomPlan>();
                for (int i=0; i<oFabricLoomPlans.Count; i++)
                {
                    if (i != 0)
                    {
                        oFabricLoomPlans[i].StartTime = oFabricLoomPlans[i - 1].EndTime.AddMinutes(1);
                        oFabricLoomPlans[i].EndTime = oFabricLoomPlans[i - 1].EndTime.AddMinutes(10);
                    }
                    int nScheduleCount = NumberofScheduleInTimePeriod(oFabricLoomPlans[i]);
                    if (nScheduleCount > 0) oFabricLoomPlans[i].IsIncreaseTime = true;
                    else oFabricLoomPlans[i].IsIncreaseTime = false;
                }
                _oFabricLoomPlans = FabricLoomPlan.SaveMultiplePlan(oFabricLoomPlans, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (FabricLoomPlan oFLP in _oFabricLoomPlans)
                {
                    oFLP.FabricLoomPlanDetails = FabricLoomPlanDetail.Gets(oFLP.FLPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oFabricLoomPlan = new FabricLoomPlan();
                _oFabricLoomPlan.ErrorMessage = ex.Message;
                _oFabricLoomPlans = new List<FabricLoomPlan>();
                _oFabricLoomPlans.Add(_oFabricLoomPlan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricLoomPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveRS(FabricLoomPlan oFabricLoomPlan)
        {
            RouteSheet oRouteSheet = new RouteSheet();
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            try
            {
                oRouteSheet = oRouteSheet.GetByPS(oFabricLoomPlan.FLPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            
            }
            catch (Exception ex)
            {
                _oFabricLoomPlan = new FabricLoomPlan();
                _oFabricLoomPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricLoomPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdatePlanStatus(FabricLoomPlan oFabricLoomPlan)
        {
            _oFabricLoomPlan = new FabricLoomPlan();
            try
            {
                _oFabricLoomPlan = oFabricLoomPlan.UpdatePlanStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricLoomPlan = new FabricLoomPlan();
                _oFabricLoomPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricLoomPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult Delete(FabricLoomPlan oFabricLoomPlan)
        {
            try
            {
                if (oFabricLoomPlan.FLPID <= 0) { throw new Exception("Please select an valid item."); }
                oFabricLoomPlan.ErrorMessage = oFabricLoomPlan.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricLoomPlan = new FabricLoomPlan();
                oFabricLoomPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricLoomPlan.ErrorMessage);
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

                FabricLoomPlan oFabricLoomPlan = new FabricLoomPlan();
                FabricLoomPlan oFabricLoomPlanOne = new FabricLoomPlan();
                FabricLoomPlan oFabricLoomPlanTwo = new FabricLoomPlan();
                oFabricLoomPlanOne = oFabricLoomPlanOne.Get(nDUpScID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricLoomPlanTwo = oFabricLoomPlanTwo.Get(nDUpScIDTwo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                /// Mapping One
                oFabricLoomPlan.FLPID = oFabricLoomPlanOne.FLPID;
                oFabricLoomPlan.FMID = oFabricLoomPlanTwo.FMID;
                //oFabricLoomPlan.LocationID = oFabricLoomPlanTwo.LocationID;
                oFabricLoomPlanOne.EndTime = oFabricLoomPlanOne.EndTime.AddMinutes(1);
                oTimeSpan = oFabricLoomPlanOne.EndTime - oFabricLoomPlanOne.StartTime;
                oFabricLoomPlan.StartTime = oFabricLoomPlanTwo.StartTime;
                oFabricLoomPlan.EndTime = oFabricLoomPlanTwo.StartTime + oTimeSpan;
                _oFabricLoomPlans.Add(oFabricLoomPlan);

                ///Mapping Two
                oFabricLoomPlan = new FabricLoomPlan();
                oFabricLoomPlan.FLPID = oFabricLoomPlanTwo.FLPID;
                oFabricLoomPlan.FMID = oFabricLoomPlanOne.FMID;
                //oFabricLoomPlan.LocationID = oFabricLoomPlanOne.LocationID;
                oFabricLoomPlanTwo.EndTime = oFabricLoomPlanTwo.EndTime.AddMinutes(1);
                oTimeSpan = oFabricLoomPlanTwo.EndTime - oFabricLoomPlanTwo.StartTime;
                oFabricLoomPlan.StartTime = oFabricLoomPlanOne.StartTime;
                oFabricLoomPlan.EndTime = oFabricLoomPlanOne.StartTime + oTimeSpan;
                _oFabricLoomPlans.Add(oFabricLoomPlan);

              //  oFabricLoomPlanDetails = FabricLoomPlanDetail.Swap(_oFabricLoomPlans, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                FabricLoomPlan oFabricLoomPlan = new FabricLoomPlan();
                oFabricLoomPlan.ErrorMessage = ex.Message;
                _oFabricLoomPlans.Add(oFabricLoomPlan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricLoomPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PastSchedule(FabricLoomPlan oFabricLoomPlan)
        {
           
            TimeSpan oTimeSpan = new TimeSpan();
            int nDUpScID = 0;
            int nDUpScIDTwo = 0;
            string sTime = "";
            string sPSIDs = oFabricLoomPlan.Params;

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

                //FabricLoomPlan oFabricLoomPlan = new FabricLoomPlan();
                FabricLoomPlan oFabricLoomPlanOne = new FabricLoomPlan();
                oFabricLoomPlanOne = oFabricLoomPlan;
                FabricLoomPlan oFabricLoomPlanTwo = new FabricLoomPlan();
                oFabricLoomPlanOne.EndTime = oDateTime;
                if (nDUpScID > 0)
                {
                    oFabricLoomPlanOne = oFabricLoomPlanOne.Get(nDUpScID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                oFabricLoomPlanTwo = oFabricLoomPlanTwo.Get(nDUpScIDTwo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                /// Mapping One
              
                ///Mapping Two
                //oFabricLoomPlan = new FabricLoomPlan();
                oFabricLoomPlanTwo.FLPID = oFabricLoomPlanTwo.FLPID;
                oFabricLoomPlanTwo.FMID = oFabricLoomPlanOne.FMID;
                //oFabricLoomPlanTwo.LocationID = oFabricLoomPlanOne.LocationID;

                oTimeSpan = oFabricLoomPlanTwo.EndTime - oFabricLoomPlanTwo.StartTime;
                oFabricLoomPlanTwo.StartTime = oFabricLoomPlanOne.EndTime.AddMinutes(1);
                oFabricLoomPlanTwo.EndTime = oFabricLoomPlanTwo.StartTime + oTimeSpan.Duration();
                _oFabricLoomPlans.Add(oFabricLoomPlanTwo);

                _oFabricLoomPlans = FabricLoomPlan.Swap(_oFabricLoomPlans, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                FabricLoomPlan oFabricLoomPlanTemp = new FabricLoomPlan();
                oFabricLoomPlanTemp.ErrorMessage = ex.Message;
                _oFabricLoomPlans.Add(oFabricLoomPlanTemp);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricLoomPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SwapTwoSchedule(FabricLoomPlan oFabricLoomPlan)
        {
            TimeSpan oTimeSpan = new TimeSpan();
            int nDUpScID = 0;
            int nDUpScIDTwo = 0;
            string sTime = "";
            string sPSIDs = oFabricLoomPlan.Params;

            DateTime oDateTime = DateTime.MinValue;
            try
            {
                nDUpScID = Convert.ToInt32(sPSIDs.Split('~')[0]);
                nDUpScIDTwo = Convert.ToInt32(sPSIDs.Split('~')[1]);

                if (nDUpScID <= 0 || nDUpScIDTwo <= 0) { throw new Exception("Please select valid items."); }

                FabricLoomPlan oFabricLoomPlanOne = new FabricLoomPlan();
                FabricLoomPlan oFabricLoomPlanTwo = new FabricLoomPlan();

                oFabricLoomPlanOne = oFabricLoomPlanOne.Get(nDUpScID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricLoomPlanTwo = oFabricLoomPlanTwo.Get(nDUpScIDTwo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                /// Mapping
                DateTime dStartTime1 = oFabricLoomPlanOne.StartTime;
                DateTime dStartTime2 = oFabricLoomPlanTwo.StartTime;
                DateTime dEndTime1 = oFabricLoomPlanOne.EndTime;
                DateTime dEndTime2 = oFabricLoomPlanTwo.EndTime;

                oFabricLoomPlanOne.StartTime = dStartTime2;
                oFabricLoomPlanTwo.StartTime = dStartTime1;

                oFabricLoomPlanOne.EndTime = dStartTime2 + (dEndTime1 - dStartTime1).Duration();
                oFabricLoomPlanTwo.EndTime = dStartTime1 + (dEndTime2 - dStartTime2).Duration();

                _oFabricLoomPlans.Add(oFabricLoomPlanOne);
                _oFabricLoomPlans.Add(oFabricLoomPlanTwo);
                _oFabricLoomPlans = FabricLoomPlan.Swap(_oFabricLoomPlans, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                FabricLoomPlan oFabricLoomPlanTemp = new FabricLoomPlan();
                oFabricLoomPlanTemp.ErrorMessage = ex.Message;
                _oFabricLoomPlans.Add(oFabricLoomPlanTemp);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricLoomPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchFBPB(FabricBatchLoom oFabricBatchLoom)
        {
            List<FabricBatchLoom> _oFabricBatchLooms = new List<FabricBatchLoom>();
            try
            {
                _oFabricBatchLooms = this.GetsFabricBatchProductionBeam(oFabricBatchLoom);
            }
            catch (Exception ex)
            {
                FabricBatchLoom oFBPB = new FabricBatchLoom();
                oFBPB.ErrorMessage = ex.Message;
                _oFabricBatchLooms.Add(oFBPB);
            }
            var jsonResult = Json(_oFabricBatchLooms, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private List<FabricBatchLoom> GetsFabricBatchProductionBeam(FabricBatchLoom oFabricBatchLoom)
        {
            List<FabricBatchLoom> _oFabricBatchLooms = new List<FabricBatchLoom>();
            string sSQL = "";
            if (oFabricBatchLoom.Status == EnumFabricBatchStatus.Running || oFabricBatchLoom.Status == EnumFabricBatchStatus.Hold)//Loom Running
            {
                sSQL = "SELECT * FROM View_FabricBatchLoom WHERE Status = " + (int)oFabricBatchLoom.Status;
                _oFabricBatchLooms = FabricBatchLoom.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                List<FabricBatchProductionBeam> oFBPBs = new List<FabricBatchProductionBeam>();
                sSQL = " SELECT * FROM View_FabricBatchProductionBeam ReadyLBeam WHERE  ReadyLBeam.FBStatus != 14  AND ReadyLBeam.IsFinish in (0,1) AND ReadyLBeam.WeavingProcessType=1 "
                + " AND FBPBeamID NOT IN (SELECT FBPBeamID FROM FabricBatchLoom) AND FBPBeamID NOT IN (SELECT FBPBeamID FROM FabricLoomPlanDetail)";
                if (!string.IsNullOrEmpty(oFabricBatchLoom.FEONo))
                    sSQL += " AND FEONo LIKE '%" + oFabricBatchLoom.FEONo + "%'";
                List<FabricLoomPlan> oFabricLoomPlans = new List<FabricLoomPlan>();
                oFabricLoomPlans = FabricLoomPlan.Gets("Select * from View_FabricLoomPlan where PlanStatus in (" + (int)EnumFabricPlanStatus.Initialize + "," + (int)EnumFabricPlanStatus.Planned + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFBPBs = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (FabricBatchProductionBeam oItem in oFBPBs)
                {
                    FabricBatchLoom _oFabricBatchLoom = new FabricBatchLoom();
                    _oFabricBatchLoom.BatchNo = oItem.BatchNo;
                    _oFabricBatchLoom.Qty = oItem.Qty;
                    _oFabricBatchLoom.FEONo = oItem.FEONo;
                    _oFabricBatchLoom.FBID = oItem.FBID;                    
                    _oFabricBatchLoom.BuyerName = oItem.BuyerName;
                    _oFabricBatchLoom.Construction = oItem.Construction;
                    _oFabricBatchLoom.BeamNo = oItem.BeamNo;
                    _oFabricBatchLoom.TotalEnds = oItem.TotalEnds;
                    _oFabricBatchLoom.Weave = oItem.Weave;
                    _oFabricBatchLoom.FabricWeave = oItem.Weave;
                    _oFabricBatchLoom.FBPBeamID = oItem.FBPBeamID;
                    _oFabricBatchLoom.BeamID = oItem.BeamID;
                    _oFabricBatchLoom.FSCDID = oItem.FabricSalesContractDetailID;
                    _oFabricBatchLoom.FEOSID = oItem.FEOSID;
                    _oFabricBatchLoom.FSpcType = oItem.FSpcType;
                    _oFabricBatchLoom.PlanType = oItem.PlanType;
                    _oFabricBatchLoom.IsDrawing = oItem.IsDrawing;
                    if (oFabricLoomPlans.FirstOrDefault() != null && oFabricLoomPlans.FirstOrDefault().FSCDID > 0 && oFabricLoomPlans.FirstOrDefault().FMID > 0 && oFabricLoomPlans.FirstOrDefault().FBID > 0 && oFabricLoomPlans.Where(b => (b.FSCDID == oItem.FabricSalesContractDetailID && b.FBID == oItem.FBID)).Count() > 0)
                    {
                        _oFabricBatchLoom.FabricMachineName = oFabricLoomPlans.Where(x => x.FBID == oItem.FBID && x.FSCDID == oItem.FabricSalesContractDetailID).FirstOrDefault().MachineName;
                        _oFabricBatchLoom.FMID = oFabricLoomPlans.Where(x => x.FBID == oItem.FBID && x.FSCDID == oItem.FabricSalesContractDetailID).FirstOrDefault().FMID;
                        _oFabricBatchLoom.StartTime = oFabricLoomPlans.Where(x => x.FBID == oItem.FBID && x.FSCDID == oItem.FabricSalesContractDetailID).FirstOrDefault().StartTime;
                    }
                    else
                    {
                        _oFabricBatchLoom.FabricMachineName = "Wait For Plane";
                        _oFabricBatchLoom.FMID = 0;
                    }
                    _oFabricBatchLooms.Add(_oFabricBatchLoom);
                }
            }

            return _oFabricBatchLooms;
        }


        //[HttpPost]
        //public JsonResult GetPSByDU(FabricLoomPlan oFabricLoomPlanDetail)
        //{
        //    _sSQL = "";
        //    Machine oMachine = new Machine();
        //    List<FabricLoomPlan> _oFabricLoomPlans = new List<FabricLoomPlan>();
        //    if (oFabricLoomPlanDetail.FSPDID > 0)
        //    {
        //        List<FabricLoomPlan> oPSs = new List<FabricLoomPlan>();
        //       // _sSQL = "SELECT * FROM FabricLoomPlan WHERE FabricLoomPlanID IN (SELECT FabricLoomPlanID FROM FabricLoomPlanDetail WHERE DODID =" + oFabricLoomPlanDetail.DODID + ")";
        //        _oFabricLoomPlans = FabricLoomPlan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
            
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oFabricLoomPlans);
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
        public JsonResult GetByLoomNo(FabricLoomPlan oFabricLoomPlan)
        {
            _oFabricLoomPlans = new List<FabricLoomPlan>();
            string sTemps = "";
            try
            {
                _sSQL = "Select * from View_FabricLoomPlan where PlanStatus in (" + (int)EnumFabricPlanStatus.Initialize + "," + (int)EnumFabricPlanStatus.Planned + ") AND MachineName LIKE '%" + oFabricLoomPlan.MachineName + "%' ORDER BY  (CONVERT(bigint, dbo.udf_GetNumeric(MachineCode))) ASC ";
                //_sSQL = "Select * from View_FabricLoomPlan where MachineName LIKE '%" + oFabricLoomPlan.MachineName + "%' ";
                _oFabricLoomPlans = FabricLoomPlan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oFabricLoomPlans.Count > 0)
                {
                    sTemps = string.Join(",", _oFabricLoomPlans.Select(x => x.FMID).Distinct().ToList());
                }

                if (string.IsNullOrEmpty(sTemps)) { sTemps = "0"; }

                _sSQL = "SELECT * FROM View_FabricMachine WHERE FMID not in (" + sTemps + ") and IsActive=1 AND isnull(IsBeam,0)=0 and WeavingProcess = " + (int)EnumWeavingProcess.Loom + " AND ISNULL(Name,'')+ISNULL(Code,'') LIKE '%" + oFabricLoomPlan.MachineName + "%' ORDER BY  (CONVERT(bigint, dbo.udf_GetNumeric(Code))) ASC";
                List<FabricMachine> oFabricMachines = FabricMachine.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (FabricMachine oItem in oFabricMachines)
                {
                    _oFabricLoomPlan = new FabricLoomPlan();
                    _oFabricLoomPlan.FMID = oItem.FMID;
                    _oFabricLoomPlan.MachineCode = oItem.Code;
                    _oFabricLoomPlan.MachineName = oItem.Code + "-" + oItem.Name;
                    _oFabricLoomPlans.Add(_oFabricLoomPlan);
                }
                FabricBatchLoom oFabricBatchLoom = new FabricBatchLoom();
                List<FabricBatchLoom> oFabricBatchLooms = new List<FabricBatchLoom>();
                oFabricBatchLooms = FabricBatchLoom.Gets("SELECT * FROM View_FabricBatchLoom WHERE FMID IN (SELECT FMID FROM FabricMachine WHERE MachineStatus = 1) AND ISNULL(RemainingQty,0) > 2 AND ISNULL(FabricMachineName,'')+ISNULL(Code,'') LIKE '%" + oFabricLoomPlan.MachineName + "%'", ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (FabricLoomPlan oItem in _oFabricLoomPlans)
                {
                    oFabricBatchLoom = new FabricBatchLoom();
                    oFabricBatchLoom = oFabricBatchLooms.Where(x => x.FMID == oItem.FMID).FirstOrDefault();
                    if (oFabricBatchLoom != null)
                    {
                        oItem.Const_CO = oFabricBatchLoom.Construction;
                        oItem.Shift_CO = oFabricBatchLoom.ShiftName;
                        oItem.ReedCount_CO = oFabricBatchLoom.ReedCount;
                    }
                    if (oItem.FLPID > 0)
                        oItem.FabricLoomPlanDetails = FabricLoomPlanDetail.Gets(oItem.FLPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }


            }
            catch (Exception ex)
            {
                _oFabricLoomPlans = new List<FabricLoomPlan>();
                oFabricLoomPlan.ErrorMessage = ex.Message;
                _oFabricLoomPlans.Add(oFabricLoomPlan);
            }
            var jsonResult = Json(_oFabricLoomPlans, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AdvSearch(FabricLoomPlan oFabricLoomPlan)
        {
            _oFabricLoomPlans = new List<FabricLoomPlan>();
            try
            {
                string sSQL = MakeSQL(oFabricLoomPlan);
                _oFabricLoomPlans = FabricLoomPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (FabricLoomPlan oItem in _oFabricLoomPlans)
                {
                    if (oItem.FLPID > 0)
                        oItem.FabricLoomPlanDetails = FabricLoomPlanDetail.Gets(oItem.FLPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oFabricLoomPlans = new List<FabricLoomPlan>();
                oFabricLoomPlan.ErrorMessage = ex.Message;
                _oFabricLoomPlans.Add(oFabricLoomPlan);
            }
            var jsonResult = Json(_oFabricLoomPlans, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(FabricLoomPlan oFabricLoomPlan)
        {
            int ncboOrderDate = 0;
            DateTime dFromDate = DateTime.Now, dToDate = DateTime.Now;
            string sReturn1 = "";
            string sReturn = "";
            sReturn1 = "Select * from View_FabricLoomPlan";
            if (!string.IsNullOrEmpty(oFabricLoomPlan.ErrorMessage))
            {
                ncboOrderDate = Convert.ToInt32(oFabricLoomPlan.ErrorMessage.Split('~')[0]);
                dFromDate = Convert.ToDateTime(oFabricLoomPlan.ErrorMessage.Split('~')[1]);
                dToDate = Convert.ToDateTime(oFabricLoomPlan.ErrorMessage.Split('~')[2]);
            }
            
            #region Start Date
            DateObject.CompareDateQuery(ref sReturn, "StartTime", ncboOrderDate, dFromDate, dToDate);
            #endregion

            #region ExeNo
            if (!string.IsNullOrEmpty(oFabricLoomPlan.ExeNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExeNo LIKE '%" + oFabricLoomPlan.ExeNo + "%' ";
            }
            #endregion

            #region Loom No
            if (!string.IsNullOrEmpty(oFabricLoomPlan.MachineName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MachineName LIKE '%" + oFabricLoomPlan.MachineName + "%' ";
            }
            #endregion

            #region Construction
            if (!string.IsNullOrEmpty(oFabricLoomPlan.Construction))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Construction LIKE '%" + oFabricLoomPlan.Construction + "%' ";
            }
            #endregion

            #region Plan Status
            if ((int)oFabricLoomPlan.PlanStatus > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PlanStatus = " + (int)oFabricLoomPlan.PlanStatus;
            }
            #endregion

            #region Fabric Programe
            if ((int)oFabricLoomPlan.FabricPrograme > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricPrograme = " + (int)oFabricLoomPlan.FabricPrograme;
            }
            #endregion

            #region Customer
            if (!string.IsNullOrEmpty(oFabricLoomPlan.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + oFabricLoomPlan.ContractorName + ") ";
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn + " ORDER BY  (CONVERT(bigint, dbo.udf_GetNumeric(MachineCode))) ASC";
            return sSQL;
        }
        //public ActionResult PrintFabricWarpingPlan_Machine(string sTemp)
        //{
        //    FabricLoomPlan oFabricLoomPlan = new FabricLoomPlan();
        //    oFabricLoomPlan.ErrorMessage = sTemp;
        //    string sSQL = MakeSQL(oFabricLoomPlan);
        //    _oFabricLoomPlans = FabricLoomPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    BusinessUnit oBusinessUnit = new BusinessUnit();
        //    //string sDateRange = "Date: " + dFromDate.ToString("dd MMM yyyy");
        //    //if (dFromDate != dToDate)
        //    //{
        //    //    sDateRange = "Date: " + dFromDate.ToString("dd MMM yyyy") + " to " + dToDate.ToString("dd MMM yyyy");
        //    //}

        //    string sTitle = "WARPING PROGRAM";
        //    rptFabricWarpingPlan oReport = new rptFabricWarpingPlan();
        //    byte[] abytes = oReport.PrepareReport_WarpingPlaneTwo(_oFabricLoomPlans, oCompany, oBusinessUnit, sTitle, sDateRange);
        //    return File(abytes, "application/pdf");
        //}

        public void ExcelFabricWarpingPlan_Machine(string sTemp)
        {
            FabricLoomPlan oFabricLoomPlan = new FabricLoomPlan();
            oFabricLoomPlan.ErrorMessage = sTemp;
            string sSQL = MakeSQL(oFabricLoomPlan);
            _oFabricLoomPlans = FabricLoomPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            string sTitle = "WARPING PROGRAM";
            if (_oFabricLoomPlans.Count > 0)
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

                    var oMachaines = _oFabricLoomPlans.GroupBy(x => new { x.FMID, x.MachineName, x.MachineCode }, (key, grp) => new
                                      {
                                          MachineName = key.MachineName,
                                          MachineCode = key.MachineCode,
                                          FMID = key.FMID,
                                      }).ToList();
                    int nSL = 0;
                    foreach (var oItem in oMachaines)
                    {
                        nStartCol = 2;
                        var oFabricWarpingPlanDetails = _oFabricLoomPlans.Where(x => x.FMID == oItem.FMID).ToList();
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
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (oItem1.Note != "" ? oItem1.Note.ToString() : ""), false, ExcelHorizontalAlignment.Left, false, false);
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
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFabricLoomPlans.Select(c => c.Qty).Sum().ToString(), true, ExcelHorizontalAlignment.Right, false, false);
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
  
        #region Print Fabric Loom plan
        [HttpPost]
        public ActionResult SetFabricLoomPlanData(FabricLoomPlan oFabricLoomPlan)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oFabricLoomPlan);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintProgramReport(int nBUID)
        {
            _oFabricLoomPlan = new FabricLoomPlan();
            string sTemps = "";
            try
            {                
                //_sSQL = "Select * from View_FabricLoomPlan where PlanStatus in (" + (int)EnumFabricPlanStatus.Initialize + "," + (int)EnumFabricPlanStatus.Planned + ") ";
                _sSQL = "Select * from View_FabricLoomPlan";
                _oFabricLoomPlan = (FabricLoomPlan)Session[SessionInfo.ParamObj];
                _sSQL = MakeSQL(_oFabricLoomPlan);
                _oFabricLoomPlans = FabricLoomPlan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                //if (_oFabricLoomPlans.Count > 0)
                //    sTemps = string.Join(",", _oFabricLoomPlans.Select(x => x.FMID).Distinct().ToList());
                //if (string.IsNullOrEmpty(sTemps)) { sTemps = "0"; }
                //_sSQL = "SELECT * FROM View_FabricMachine WHERE FMID not in (" + sTemps + ") and IsActive=1 AND isnull(IsBeam,0)=0 and WeavingProcess = " + (int)EnumWeavingProcess.Loom + " Order By Code";
                //List<FabricMachine> oFabricMachines = new List<FabricMachine>();
                //oFabricMachines = FabricMachine.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                //foreach (FabricMachine oItem in oFabricMachines)
                //{
                //    _oFabricLoomPlan = new FabricLoomPlan();
                //    _oFabricLoomPlan.FMID = oItem.FMID;
                //    _oFabricLoomPlan.MachineCode = oItem.Code;
                //    _oFabricLoomPlan.MachineName = oItem.Name;
                //    _oFabricLoomPlans.Add(_oFabricLoomPlan);
                //}

                List<FabricLoomPlanDetail> oFabricLoomPlanDetails = new List<FabricLoomPlanDetail>();
                oFabricLoomPlanDetails = FabricLoomPlanDetail.Gets("Select * from View_FabricLoomPlanDetail WHERE FLPID IN (" + string.Join(",", _oFabricLoomPlans.Select(x=>x.FLPID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                
                FabricBatchLoom oFabricBatchLoom = new FabricBatchLoom();
                List<FabricBatchLoom> oFabricBatchLooms = new List<FabricBatchLoom>();
                oFabricBatchLooms = FabricBatchLoom.Gets("SELECT * FROM View_FabricBatchLoom WHERE FMID IN (SELECT FMID FROM FabricMachine WHERE MachineStatus = 1)", ((User)Session[SessionInfo.CurrentUser]).UserID);
                List<FabricLoomPlanDetail> oTempFabricLoomPlanDetails = new List<FabricLoomPlanDetail>();
                foreach (FabricLoomPlan oItem in _oFabricLoomPlans)
                {
                    oFabricBatchLoom = new FabricBatchLoom();
                    oFabricBatchLoom = oFabricBatchLooms.Where(x => x.FMID == oItem.FMID).FirstOrDefault();
                    if (oFabricBatchLoom != null)
                    {
                        oItem.Const_CO = oFabricBatchLoom.Construction;
                        string sDuration = "", sShift = "";
                        if (!string.IsNullOrEmpty(oFabricBatchLoom.BatchDuration))
                        {
                            sDuration = oFabricBatchLoom.BatchDuration.Split('(')[1];
                            sDuration = sDuration.Substring(0, sDuration.Length - 1);
                            //sDuration = "1/12/2020 11:00:00 PM";
                            DateTime dt = DateTime.Parse(sDuration);

                            sShift = dt.ToString("dd MMM yyyy") + " (" + GetShift(dt) + ")";
                        }
                        oItem.Shift_CO = sShift;
                        oItem.ReedCount_CO = oFabricBatchLoom.ReedCount;
                        oItem.Dent_CO = oFabricBatchLoom.Dent;
                    }
                    oTempFabricLoomPlanDetails = oFabricLoomPlanDetails.Where(x => x.FLPID == oItem.FLPID).ToList();
                    if (oTempFabricLoomPlanDetails.Count > 0)
                    {
                        oItem.BeamNo = string.Join(",", oTempFabricLoomPlanDetails.Select(x => x.BeamNo));
                        oItem.BeamQty = oTempFabricLoomPlanDetails.Sum(x => x.QtyInMtr);
                    }
                }
            }
            catch (Exception ex)
            {
                _oFabricLoomPlan = new FabricLoomPlan();
                _oFabricLoomPlans = new List<FabricLoomPlan>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (nBUID > 0)//_oFabricLoomPlan.BUID 
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptPrintFabricLoomPlan oReport = new rptPrintFabricLoomPlan();
            byte[] abytes = oReport.PrepareReport(_oFabricLoomPlans, oCompany);
            return File(abytes, "application/pdf");
        }

        public string GetShift(DateTime dt)
        {
            string sShift = "";
            //dt = Convert.ToDateTime(dt.ToString("hh:mm:ss tt"));
            //if (dt.ToString("tt").ToUpper() == "AM")
            //    dt = dt.AddDays(1);
            //DateTime t1 = Convert.ToDateTime("06:00:00 AM");
            //DateTime t2 = Convert.ToDateTime("01:59:00 PM");
            //int nCom1 = DateTime.Compare(dt, t1);
            //int nCom2 = DateTime.Compare(dt, t2);
            ////if dt is less than t then result is Less than zero
            ////if dt equals t then result is Zero
            ////if dt is greater than t then result isGreater zero
            //if (nCom1 >= 0 && nCom2 <= 0)
            //    sShift = "Shift A";

            //t1 = Convert.ToDateTime("02:00:00 PM");
            //t2 = Convert.ToDateTime("09:59:00 PM");
            //nCom1 = DateTime.Compare(dt, t1);
            //nCom2 = DateTime.Compare(dt, t2);
            //if (nCom1 >= 0 && nCom2 <= 0)
            //    sShift = "Shift B";

            //t1 = Convert.ToDateTime("10:00:00 PM");
            //t2 = Convert.ToDateTime("05:59:00 AM").AddDays(1);
            //nCom1 = DateTime.Compare(dt, t1);
            //nCom2 = DateTime.Compare(dt, t2);
            //if (nCom1 >= 0 && nCom2 <= 0)
            //    sShift = "Shift C";

            if (dt.Hour >= 6 && dt.Hour <= 13)
                sShift = "Shift A";
            else if (dt.Hour >= 14 && dt.Hour <= 21)
                sShift = "Shift B";
            else
                sShift = "Shift C";
            return sShift;
        }

        #endregion
    }
}
