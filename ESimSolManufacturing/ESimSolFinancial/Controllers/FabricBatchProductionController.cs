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
using System.Collections;
using ESimSol.BusinessObjects.ReportingObject;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using System.Data.OleDb;
using System.Globalization;

namespace ESimSolFinancial.Controllers
{
    public class FabricBatchProductionController : Controller
    {
        #region Declaration
        FabricBatchProduction _oFabricBatchProduction = new FabricBatchProduction();
        List<FabricBatchProduction> _oFabricBatchProductions = new List<FabricBatchProduction>();
        FabricBatchProductionBatchMan _oFBPBatchMan = new FabricBatchProductionBatchMan();
        List<FabricBatchProductionBatchMan> _oFBPBatchMans = new List<FabricBatchProductionBatchMan>();
        FabricBatchProductionColor _oFBPColor = new FabricBatchProductionColor();
        List<FabricBatchProductionColor> _oFBPColors = new List<FabricBatchProductionColor>();
        FabricBatchProductionBreakage _oFBPBreakage = new FabricBatchProductionBreakage();
        List<FabricBatchProductionBreakage> _oFBPBreakages = new List<FabricBatchProductionBreakage>();
        #endregion

        public ActionResult ViewFabricBatchProductions(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFabricBatchProductions = new List<FabricBatchProduction>();
            string sSQL = "";
            sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=1 AND WeavingProcess = " + (int)EnumWeavingProcess.Warping + " AND MachineStatus = " + (int)EnumMachineStatus.Free + " Order By Code";
            _oFabricBatchProductions = FabricBatchProduction.Gets("Select top(500)* from view_FabricBatchProduction as TT  order by FBPID DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.MachineTypes = EnumObject.jGets(typeof(EnumWeavingProcess)).Where(x => x.id != 2 && x.id != 3).ToList();
            ViewBag.WeavingProcess = EnumObject.jGets(typeof(EnumWeavingProcess)).Where(x => x.id != 2 && x.id != 3 && x.id != 4).ToList();
            ViewBag.FabricBatchStates = EnumObject.jGets(typeof(EnumFabricBatchState));
            return View(_oFabricBatchProductions);
        }

        #region Warping
        public ActionResult ViewWarping(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFabricBatchProduction = new FabricBatchProduction();
            ViewBag.Stores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricBatch, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);

            string sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=0 AND WeavingProcess = " + (int)EnumWeavingProcess.Warping + " AND MachineStatus = " + (int)EnumMachineStatus.Free + " Order By Code";
            _oFabricBatchProduction.FabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oFabricBatchProduction.BatchMans = Employee.Gets(EnumEmployeeDesignationType.WarpingOperator, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oFabricBatchProduction.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oFabricBatchProduction.FabricBreakages = FabricBreakage.Gets(EnumWeavingProcess.Warping, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=1 AND WeavingProcess = " + (int)EnumWeavingProcess.Warping + " AND MachineStatus = " + (int)EnumMachineStatus.Free + " Order By Code";
            //ViewBag.Beams = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            sSQL = "SELECT * FROM View_FabricMachineType WHERE FabricMachineTypeID IN (SELECT DISTINCT ChildMachineTypeID FROM FabricMachine WHERE IsBeam=1 AND IsActive=1 AND WeavingProcess=" + (int)EnumWeavingProcess.Warping + ")";
            ViewBag.FabricMachineTypes = FabricMachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.BUID = buid;
            return View(_oFabricBatchProduction);
        }
        public ActionResult ViewWarpingExecution(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricBatchProduction).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oFabricBatchProductions = new List<FabricBatchProduction>();
            _oFabricBatchProductions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE  WeavingProcess = " + (int)EnumWeavingProcess.Warping + " and ProductionStatus=" + (int)EnumProductionStatus.Run, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;

            //ViewBag.Stores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricBatch, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);

            string sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=0 AND WeavingProcess = " + (int)EnumWeavingProcess.Warping + " AND MachineStatus = " + (int)EnumMachineStatus.Free + " Order By Code";
            ViewBag.FabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BatchMans = Employee.Gets(EnumEmployeeDesignationType.WarpingOperator, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricBreakages = FabricBreakage.Gets(EnumWeavingProcess.Warping, ((User)Session[SessionInfo.CurrentUser]).UserID);
            sSQL = "SELECT * FROM View_FabricMachineType WHERE FabricMachineTypeID IN (SELECT DISTINCT ChildMachineTypeID FROM FabricMachine WHERE IsBeam=1 AND IsActive=1 AND WeavingProcess=" + (int)EnumWeavingProcess.Warping + ")";
            ViewBag.FabricMachineTypes = FabricMachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperatorTwo));
            ViewBag.ProductionStatusList = EnumObject.jGets(typeof(EnumProductionStatus));
            ViewBag.FabricMachinesWarping = FabricMachine.Gets("SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=0 AND WeavingProcess = " + (int)EnumWeavingProcess.Warping + " Order By Code", ((User)Session[SessionInfo.CurrentUser]).UserID);

            return View(_oFabricBatchProductions);
        }
        public ActionResult ViewBatchProductionWarping(int nFBID, int buid)
        {
            FabricBatch oFabricBatch = new FabricBatch();
            _oFabricBatchProduction = new FabricBatchProduction();
            ViewBag.Stores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricBatch, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);

            string sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=0 AND WeavingProcess = " + (int)EnumWeavingProcess.Warping + " AND MachineStatus = " + (int)EnumMachineStatus.Free + " Order By Code";


            sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=1 AND WeavingProcess = " + (int)EnumWeavingProcess.Warping + " AND MachineStatus = " + (int)EnumMachineStatus.Free + " Order By Code";
            ViewBag.Beams = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;

            if (nFBID > 0)
            {
                oFabricBatch = oFabricBatch.Get(nFBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricBatchProduction = _oFabricBatchProduction.GetByBatchAndWeavingType(nFBID, EnumWeavingProcess.Warping, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricBatchProduction.FabricBatchRawMaterials = FabricBatchRawMaterial.Gets(nFBID, EnumWeavingProcess.Warping, ((User)Session[SessionInfo.CurrentUser]).UserID);//get yarns
                if (_oFabricBatchProduction.FBPID > 0)
                {
                    _oFabricBatchProduction.FabricBatchProductionBreakages = FabricBatchProductionBreakage.Gets(_oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Breakages
                    _oFabricBatchProduction.FabricBatchProductionColors = FabricBatchProductionColor.Gets(_oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Colors
                    _oFabricBatchProduction.FabricBatchProductionBatchMans = FabricBatchProductionBatchMan.Gets(_oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Batchmans
                }

                _oFabricBatchProduction.FBPBs = FabricBatchProductionBeam.GetsByFabricBatchProduction(_oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (_oFabricBatchProduction.EndTime.ToString("dd MMM yyyy") == new DateTime(0001, 01, 01).ToString("dd MMM yyyy"))
            {
                _oFabricBatchProduction.EndTime = _oFabricBatchProduction.StartTime.AddMinutes(1);
            }
            sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=0 AND WeavingProcess = " + (int)EnumWeavingProcess.Warping + " AND MachineStatus = " + (int)EnumMachineStatus.Free + " Order By Code";
            _oFabricBatchProduction.FabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oFabricBatchProduction.BatchMans = Employee.Gets(EnumEmployeeDesignationType.WarpingOperator, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oFabricBatchProduction.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oFabricBatchProduction.FabricBreakages = FabricBreakage.Gets(EnumWeavingProcess.Warping, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.FabricBatch = oFabricBatch;

            return View(_oFabricBatchProduction);
        }

        [HttpGet]
        public JsonResult GetFabricBatchProduction(int nFBID)//FBID
        {
            FabricBatchProduction oFabricBatchProduction = new FabricBatchProduction();
            try
            {
                if (nFBID > 0)
                {
                    oFabricBatchProduction = oFabricBatchProduction.GetByBatchAndWeavingType(nFBID, EnumWeavingProcess.Warping, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFabricBatchProduction.FabricBatchRawMaterials = FabricBatchRawMaterial.Gets(nFBID, EnumWeavingProcess.Warping, ((User)Session[SessionInfo.CurrentUser]).UserID);//get yarns
                    if (oFabricBatchProduction.FBPID > 0)
                    {
                        oFabricBatchProduction.FabricBatchProductionBreakages = FabricBatchProductionBreakage.Gets(oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Breakages
                        oFabricBatchProduction.FabricBatchProductionColors = FabricBatchProductionColor.Gets(oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Colors
                        oFabricBatchProduction.FabricBatchProductionBatchMans = FabricBatchProductionBatchMan.Gets(oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Batchmans
                    }

                    oFabricBatchProduction.FBPBs = FabricBatchProductionBeam.GetsByFabricBatchProduction(oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (oFabricBatchProduction.EndTime.ToString("dd MMM yyyy") == new DateTime(0001, 01, 01).ToString("dd MMM yyyy"))
                {
                    oFabricBatchProduction.EndTime = oFabricBatchProduction.StartTime.AddMinutes(1);
                }
            }
            catch (Exception ex)
            {
                oFabricBatchProduction = new FabricBatchProduction();
                oFabricBatchProduction.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchProduction);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetProductionBeams(int nFBID)//FBID
        {
            FabricBatchProduction oFabricBatchProduction = new FabricBatchProduction();
            try
            {
                oFabricBatchProduction.FBPBs = FabricBatchProductionBeam.Gets("SELECT * FROM View_FabricBatchProductionBeam WHERE FBID = " + nFBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricBatchProduction = new FabricBatchProduction();
                oFabricBatchProduction.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchProduction);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFabricBatchProductionByID(FabricBatchProduction oFBP)
        {
            _oFabricBatchProduction = new FabricBatchProduction();
            _oFabricBatchProduction.FabricBatch = new FabricBatch();
            try
            {
                if (oFBP.FBPID > 0)
                {
                    _oFabricBatchProduction = _oFabricBatchProduction.Get(oFBP.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oFBP.FBPID > 0)
                    {
                        FabricMachine oFabricMachine = new FabricMachine();
                        oFabricMachine = oFabricMachine.Get(_oFabricBatchProduction.FMID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        _oFabricBatchProduction.FabricBatchProductionBreakages = FabricBatchProductionBreakage.Gets(oFBP.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Breakages
                        _oFabricBatchProduction.FabricBatchProductionColors = FabricBatchProductionColor.Gets(oFBP.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Colors
                        //_oFabricBatchProduction.FabricBatchProductionBatchMans = FabricBatchProductionBatchMan.Gets(oFBP.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Batchmans
                        _oFabricBatchProduction.FabricBatchProductionDetails = FabricBatchProductionDetail.Gets(oFBP.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Batchmans
                        _oFabricBatchProduction.FabricMachines.Add(oFabricMachine);
                    }
                    _oFabricBatchProduction.FBPBList = FabricBatchProductionBeam.GetsByFabricBatchProduction(_oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                _oFabricBatchProductions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE WeavingProcess in (" + (int)EnumWeavingProcess.Warping + ")  and FBID=" + oFBP.FBID, (int)Session[SessionInfo.currentUserID]);
                _oFabricBatchProduction.FabricBatch = _oFabricBatchProduction.FabricBatch.Get(oFBP.FBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricBatchProduction.FabricBatch.QtyPro = _oFabricBatchProductions.Where(x => x.FBPID != oFBP.FBPID).Sum(y => y.Qty);

                if (_oFabricBatchProduction.EndTime.ToString("dd MMM yyyy") == new DateTime(0001, 01, 01).ToString("dd MMM yyyy"))
                {
                    _oFabricBatchProduction.EndTime = _oFabricBatchProduction.StartTime.AddMinutes(1);
                }
            }
            catch (Exception ex)
            {
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchProduction);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProduction(FabricBatchProduction oFBP)
        {
            _oFabricBatchProductions = new List<FabricBatchProduction>();
            _oFabricBatchProduction = new FabricBatchProduction();
            try
            {
                if (oFBP.ProductionStatus == EnumProductionStatus.Run)
                {
                    _oFabricBatchProductions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE WeavingProcess in (" + (int)EnumWeavingProcess.Warping + ")  and ProductionStatus=" + (int)EnumProductionStatus.Run, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    List<FabricBatch> oFBs = new List<FabricBatch>();
                    oFBs = FabricBatch.Gets("SELECT * FROM View_FabricBatch WHERE isnull(FinishStatus,0)<" + (int)EnumFabricBatchState.warping_Finish + " and Status = " + (int)EnumFabricBatchState.Initialize + "" + (!string.IsNullOrEmpty(oFBP.FEONo) ? " AND FEONo LIKE '%" + oFBP.FEONo + "%'" : "") + " AND FBID NOT IN (SELECT FBID FROM FabricBatchProduction) and FWPDID in (Select FabricWarpPlanID from FabricWarpPlan where PlanStatus in (" + (int)EnumFabricPlanStatus.Initialize + "," + (int)EnumFabricPlanStatus.Planned + ")) ", (int)Session[SessionInfo.currentUserID]);
                    //  oFBs = FabricBatch.Gets("Select top(500)* from (SELECT *,isnull((SELECT SUM(Qty) FROM FabricBatchProduction AS FBP where WeavingProcess in (" + (int)EnumWeavingProcess.Warping + ") and FBP.FBID =FB.FBID),0) as QtyPro FROM View_FabricBatch as FB where FBID=383 and FB.[Status] in (" + (int)EnumFabricBatchState.Initialize + "," + (int)EnumFabricBatchState.InFloor + "," + (int)EnumFabricBatchState.warping_Finish + "," + (int)EnumFabricBatchState.Sizing + "," + (int)EnumFabricBatchState.Sizing_Finish + ") ) as HH where  HH.Qty-10.0>isnull(HH.QtyPro,0) order by FBID DESC", (int)Session[SessionInfo.currentUserID]);
                    _oFabricBatchProductions = new List<FabricBatchProduction>();
                    foreach (FabricBatch oItem in oFBs)
                    {
                        _oFabricBatchProduction = new FabricBatchProduction();
                        _oFabricBatchProduction.FBID = oItem.FBID;
                        _oFabricBatchProduction.FMID = oItem.FMID;
                        _oFabricBatchProduction.WeavingProcess = oItem.WeavingProcess;
                        _oFabricBatchProduction.Qty = oItem.Qty - oItem.QtyPro;
                        _oFabricBatchProduction.FEOID = oItem.FEOID;
                        _oFabricBatchProduction.FEONo = oItem.FEONo;
                        _oFabricBatchProduction.FEOSID = oItem.FEOSID;
                        _oFabricBatchProduction.FSpcType = oItem.FSpcType;
                        _oFabricBatchProduction.PlanType = oItem.PlanType;
                        _oFabricBatchProduction.BatchNo = oItem.BatchNo;
                        //_oFabricBatchProduction.StartTime = oItem.tim;
                        //_oFabricBatchProduction.EndTime = oItem.FEONo;
                        _oFabricBatchProduction.ProductName = oItem.ProductName;
                        _oFabricBatchProduction.BuyerName = oItem.BuyerName;
                        _oFabricBatchProduction.Construction = oItem.Construction;
                        _oFabricBatchProduction.FabricMachineName = oItem.WarpingMachineCode;
                        _oFabricBatchProduction.FabricWeave = oItem.Weave;
                        _oFabricBatchProduction.ColorName = oItem.Color;
                        _oFabricBatchProduction.FabricBatchStatus = oItem.Status;

                        _oFabricBatchProduction.ProductionStatus = EnumProductionStatus.Initialize;

                        _oFabricBatchProductions.Add(_oFabricBatchProduction);
                    }
                    oFBs = new List<FabricBatch>();
                    oFBs = FabricBatch.Gets("Select * from (SELECT *,isnull((SELECT SUM(Qty) FROM FabricBatchProduction AS FBP where WeavingProcess in (" + (int)EnumWeavingProcess.Warping + ") and FBP.FBID =FB.FBID),0) as QtyPro FROM View_FabricBatch as FB where isnull(FB.FinishStatus,0)<" + (int)EnumFabricBatchState.warping_Finish + " and FB.[Status] in (" + (int)EnumFabricBatchState.Initialize + "," + (int)EnumFabricBatchState.Warping + "," + (int)EnumFabricBatchState.InFloor + "," + (int)EnumFabricBatchState.warping_Finish + "," + (int)EnumFabricBatchState.Sizing + "," + (int)EnumFabricBatchState.Sizing_Finish + ") " + (!string.IsNullOrEmpty(oFBP.FEONo) ? " AND FEONo LIKE '%" + oFBP.FEONo + "%'" : "") + " and FB.FBID not in (SELECT FBP.FBID FROM FabricBatchProduction AS FBP where  ProductionStatus in (0,1) and WeavingProcess in (0))) as HH where isnull(HH.QtyPro,0)>0 and (isnull(HH.Qty,0)-isnull(HH.QtyPro,0))>10", (int)Session[SessionInfo.currentUserID]);
                    foreach (FabricBatch oItem in oFBs)
                    {
                        _oFabricBatchProduction = new FabricBatchProduction();
                        _oFabricBatchProduction.FBID = oItem.FBID;
                        _oFabricBatchProduction.FMID = oItem.FMID;
                        _oFabricBatchProduction.WeavingProcess = oItem.WeavingProcess;
                        _oFabricBatchProduction.Qty = oItem.Qty - oItem.QtyPro;
                        _oFabricBatchProduction.FEOID = oItem.FEOID;
                        _oFabricBatchProduction.FEONo = oItem.FEONo;
                        _oFabricBatchProduction.FEOSID = oItem.FEOSID;
                        _oFabricBatchProduction.FSpcType = oItem.FSpcType;
                        _oFabricBatchProduction.PlanType = oItem.PlanType;
                        _oFabricBatchProduction.BatchNo = oItem.BatchNo;
                        //_oFabricBatchProduction.StartTime = oItem.tim;
                        //_oFabricBatchProduction.EndTime = oItem.FEONo;
                        _oFabricBatchProduction.ProductName = oItem.ProductName;
                        _oFabricBatchProduction.BuyerName = oItem.BuyerName;
                        _oFabricBatchProduction.Construction = oItem.Construction;
                        _oFabricBatchProduction.FabricMachineName = oItem.WarpingMachineCode;
                        _oFabricBatchProduction.FabricWeave = oItem.Weave;
                        _oFabricBatchProduction.ColorName = oItem.Color;
                        _oFabricBatchProduction.FabricBatchStatus = oItem.Status;
                        _oFabricBatchProduction.ProductionStatus = EnumProductionStatus.Initialize;
                        _oFabricBatchProductions.Add(_oFabricBatchProduction);
                    }
                    if (!string.IsNullOrEmpty(oFBP.FEONo))
                    {
                        _oFabricBatchProductions.AddRange(FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE WeavingProcess in (" + (int)EnumWeavingProcess.Warping + ")  and FEONo LIKE '%" + oFBP.FEONo + "%'", (int)Session[SessionInfo.currentUserID]));
                        // _oFabricBatchProductions.AddRange(Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID));
                    }

                }
            }
            catch (Exception ex)
            {
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
                _oFabricBatchProductions.Add(_oFabricBatchProduction);
            }
            var jsonResult = Json(_oFabricBatchProductions, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public JsonResult GetFabricBatch(int nFBID)//FBID
        {
            FabricBatch oFabricBatch = new FabricBatch();
            try
            {
                if (nFBID > 0)
                {
                    oFabricBatch = oFabricBatch.Get(nFBID, (int)Session[SessionInfo.currentUserID]);

                }
            }
            catch (Exception ex)
            {
                oFabricBatch = new FabricBatch();
                oFabricBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsTotalBreakageInformation(FabricBatchProductionBatchMan oFBPBatchMan)
        {
            FabricBatchProductionBreakage oFabricBatchProductionBreakage = new FabricBatchProductionBreakage();
            List<FabricBatchProductionBreakage> oFabricBatchProductionBreakages = new List<FabricBatchProductionBreakage>();
            try
            {
                if (oFBPBatchMan.FBPBID > 0)
                {
                    oFabricBatchProductionBreakages = FabricBatchProductionBreakage.Gets(oFBPBatchMan.FBPBID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Breakages
                }
                else
                {
                    oFabricBatchProductionBreakage.ErrorMessage = "Invalid Fabric Batch Production Batch Man";
                    oFabricBatchProductionBreakages.Add(oFabricBatchProductionBreakage);
                }
            }
            catch (Exception ex)
            {
                oFabricBatchProductionBreakage.ErrorMessage = ex.Message;
                oFabricBatchProductionBreakages.Add(oFabricBatchProductionBreakage);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchProductionBreakages);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsColorBreakDown(FabricBatchProductionBatchMan oFBPBatchMan)
        {
            FabricBatchProductionColor oFabricBatchProductionColor = new FabricBatchProductionColor();
            List<FabricBatchProductionColor> oFabricBatchProductionColors = new List<FabricBatchProductionColor>();
            try
            {
                if (oFBPBatchMan.FBPBID > 0)
                {
                    oFabricBatchProductionColors = FabricBatchProductionColor.Gets(oFBPBatchMan.FBPBID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Colors
                }
                else
                {
                    oFabricBatchProductionColor.ErrorMessage = "Invalid Fabric Batch Production Batch Man";
                    oFabricBatchProductionColors.Add(oFabricBatchProductionColor);
                }
            }
            catch (Exception ex)
            {
                oFabricBatchProductionColor.ErrorMessage = ex.Message;
                oFabricBatchProductionColors.Add(oFabricBatchProductionColor);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchProductionColors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AdvSearchForWarpingExecution(FabricBatchProduction oFabricBatchProduction)
        {
            _oFabricBatchProductions = new List<FabricBatchProduction>();
            try
            {
                string sSQL = MakeSQLForExecution(oFabricBatchProduction);
                _oFabricBatchProductions = FabricBatchProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatchProductions = new List<FabricBatchProduction>();
                oFabricBatchProduction = new FabricBatchProduction();
                oFabricBatchProduction.ErrorMessage = ex.Message;
                _oFabricBatchProductions.Add(oFabricBatchProduction);
            }
            var jsonResult = Json(_oFabricBatchProductions, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQLForExecution(FabricBatchProduction oFabricBatchProduction)
        {
            int nCboProductionDateAdv = 0, nCboStartDateAdv = 0, ncboEndDateAdv = 0, nCboStatusAdv = 0, nCboShiftAdv = 0, nFMID = 0;
            DateTime dFromProductionDateAdv = DateTime.Today, dToProductionDateAdv = DateTime.Today, dFromStartDateAdv = DateTime.Today, dToStartDateAdv = DateTime.Today, dFromEndDateAdv = DateTime.Today, dToEndDateAdv = DateTime.Today;
            string sDispoNoAdv = "", sBeamIDs = "";
            string sReturn1 = "", sReturn = "";
            string sWeavingProcess = "";
            string sBatchNo = "";
            sReturn1 = "Select * from View_FabricBatchProduction";
            if (!string.IsNullOrEmpty(oFabricBatchProduction.ErrorMessage))
            {
                nCboProductionDateAdv = Convert.ToInt32(oFabricBatchProduction.ErrorMessage.Split('~')[0]);
                dFromProductionDateAdv = Convert.ToDateTime(oFabricBatchProduction.ErrorMessage.Split('~')[1]);
                dToProductionDateAdv = Convert.ToDateTime(oFabricBatchProduction.ErrorMessage.Split('~')[2]);

                nCboStartDateAdv = Convert.ToInt32(oFabricBatchProduction.ErrorMessage.Split('~')[3]);
                dFromStartDateAdv = Convert.ToDateTime(oFabricBatchProduction.ErrorMessage.Split('~')[4]);
                dToStartDateAdv = Convert.ToDateTime(oFabricBatchProduction.ErrorMessage.Split('~')[5]);

                ncboEndDateAdv = Convert.ToInt32(oFabricBatchProduction.ErrorMessage.Split('~')[6]);
                dFromEndDateAdv = Convert.ToDateTime(oFabricBatchProduction.ErrorMessage.Split('~')[7]);
                dToEndDateAdv = Convert.ToDateTime(oFabricBatchProduction.ErrorMessage.Split('~')[8]);

                sDispoNoAdv = Convert.ToString(oFabricBatchProduction.ErrorMessage.Split('~')[9]);
                nCboStatusAdv = Convert.ToInt32(oFabricBatchProduction.ErrorMessage.Split('~')[10]);
                nCboShiftAdv = Convert.ToInt32(oFabricBatchProduction.ErrorMessage.Split('~')[11]);
                sWeavingProcess = oFabricBatchProduction.ErrorMessage.Split('~')[12];
                sBatchNo = Convert.ToString(oFabricBatchProduction.ErrorMessage.Split('~')[13]);
                if(oFabricBatchProduction.ErrorMessage.Split('~').ToList().Count > 14)
                    nFMID = Convert.ToInt32(oFabricBatchProduction.ErrorMessage.Split('~')[14]);
                if (oFabricBatchProduction.ErrorMessage.Split('~').ToList().Count > 15)
                    sBeamIDs = Convert.ToString(oFabricBatchProduction.ErrorMessage.Split('~')[15]);
            }

            /// Make it dynamic for Sizing , Drowaing
            #region WeavingProcess
            if (!string.IsNullOrEmpty(sWeavingProcess))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " isnull(WeavingProcess,0) in (" + sWeavingProcess + ")";
            }
            else
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " isnull(WeavingProcess,0)=0";
            }
            #endregion


            //DateObject.CompareDateQuery(ref sReturn, "FinishDate", nCboProductionDateAdv, dFromProductionDateAdv, dToProductionDateAdv);
            #region History Date
            if (nCboProductionDateAdv != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboProductionDateAdv == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " FBPID in (Select FBPID from FabricBatchProductionDetail where  ProductionDate>='" + dFromProductionDateAdv.ToString("dd MMM yyyy") + "' and ProductionDate<'" + dToProductionDateAdv.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " FBPID in (Select FBPID from FabricBatchProductionDetail where  ProductionDate>='" + dFromProductionDateAdv.ToString("dd MMM yyyy") + "' and ProductionDate<'" + dToProductionDateAdv.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " FBPID in (Select FBPID from FabricBatchProductionDetail where  ProductionDate>='" + dFromProductionDateAdv.ToString("dd MMM yyyy") + "' and ProductionDate<'" + dToProductionDateAdv.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " FBPID in (Select FBPID from FabricBatchProductionDetail where  ProductionDate>='" + dFromProductionDateAdv.ToString("dd MMM yyyy ") + "' and ProductionDate<'" + dToProductionDateAdv.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " FBPID in (Select FBPID from FabricBatchProductionDetail where  ProductionDate>='" + dFromProductionDateAdv.ToString("dd MMM yyyy") + "' and ProductionDate<'" + dToProductionDateAdv.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " FBPID in (Select FBPID from FabricBatchProductionDetail where  ProductionDate>='" + dFromProductionDateAdv.ToString("dd MMM yyyy") + "' and ProductionDate<'" + dToProductionDateAdv.AddDays(1).ToString("dd MMM yyyy") + "')";
                }

            }
            #endregion


            #region Start Date
            if (nCboStartDateAdv != (int)EnumCompareOperator.None)
            {

                DateObject.CompareDateQuery(ref sReturn, "StartTime", nCboStartDateAdv, dFromStartDateAdv, dToStartDateAdv);
            }
            #endregion

            #region End Date
            if (ncboEndDateAdv != (int)EnumCompareOperator.None)
            {
                DateObject.CompareDateQuery(ref sReturn, "EndTime", ncboEndDateAdv, dFromEndDateAdv, dToEndDateAdv);
            }
            #endregion

            #region Dispo No
            if (!string.IsNullOrEmpty(sDispoNoAdv))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FEONo LIKE '%" + sDispoNoAdv + "%' ";
            }
            #endregion

            #region Batch No
            if (!string.IsNullOrEmpty(sBatchNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BatchNo LIKE '%" + sBatchNo + "%' ";
            }
            #endregion

            #region Status
            if (nCboStatusAdv > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductionStatus = " + nCboStatusAdv;
            }
            #endregion

            #region Shift
            if (nCboShiftAdv > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FBPID in (Select FBPID from FabricBatchProductionDetail where ShiftID=" + nCboShiftAdv + ")";
            }
            #endregion

            #region Machine
            if (nFMID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FMID = " + nFMID;
            }
            #endregion

            #region Beam
            if (!string.IsNullOrEmpty(sBeamIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FBPID IN (SELECT FBPID FROM FabricBatchProductionBeam WHERE BeamID IN (" + sBeamIDs + ")) ";
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn + "  ORDER BY StartTime DESC";
            return sSQL;
        }
        #endregion

        #region Sizing New
        public ActionResult ViewSizingExecution(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricBatchProduction).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oFabricBatchProductions = new List<FabricBatchProduction>();
            _oFabricBatchProductions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE  WeavingProcess = " + (int)EnumWeavingProcess.Sizing + " and ProductionStatus=" + (int)EnumProductionStatus.Run, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            //ViewBag.Stores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricBatch, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=0 AND WeavingProcess = " + (int)EnumWeavingProcess.Sizing + " AND MachineStatus = " + (int)EnumMachineStatus.Free + " Order By Code";
            ViewBag.FabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BatchMans = Employee.Gets(EnumEmployeeDesignationType.WarpingOperator, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricBreakages = FabricBreakage.Gets(EnumWeavingProcess.Sizing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            sSQL = "SELECT * FROM View_FabricMachineType WHERE FabricMachineTypeID IN (SELECT DISTINCT ChildMachineTypeID FROM FabricMachine WHERE IsBeam=1 AND IsActive=1 AND WeavingProcess=" + (int)EnumWeavingProcess.Sizing + ")";
            ViewBag.FabricMachineTypes = FabricMachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperatorTwo));
            ViewBag.ProductionStatusList = EnumObject.jGets(typeof(EnumProductionStatus));
            ViewBag.Stores = WorkingUnit.GetsPermittedStore(0, EnumModuleName.FabricBatch_PRO_Sizing, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            ViewBag.FabricMachinesWarping = FabricMachine.Gets("SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=0 AND WeavingProcess = " + (int)EnumWeavingProcess.Sizing + " Order By Code", ((User)Session[SessionInfo.CurrentUser]).UserID);

            return View(_oFabricBatchProductions);
        }
        [HttpPost]
        public JsonResult GetFabricBatchProductionByIDSizing(FabricBatchProduction oFBP)
        {
            _oFabricBatchProduction = new FabricBatchProduction();
            _oFabricBatchProduction.FabricBatch = new FabricBatch();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            try
            {
                if (oFBP.FBPID > 0)
                {
                    _oFabricBatchProduction = _oFabricBatchProduction.Get(oFBP.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oFBP.FBPID > 0)
                    {
                        FabricMachine oFabricMachine = new FabricMachine();
                        oFabricMachine = oFabricMachine.Get(_oFabricBatchProduction.FMID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        //_oFabricBatchProduction.FabricBatchProductionBreakages = FabricBatchProductionBreakage.Gets(oFBP.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Breakages
                        //_oFabricBatchProduction.FabricBatchProductionColors = FabricBatchProductionColor.Gets(oFBP.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Colors
                        //_oFabricBatchProduction.FabricBatchProductionBatchMans = FabricBatchProductionBatchMan.Gets(oFBP.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Batchmans
                        _oFabricBatchProduction.FabricBatchProductionDetails = FabricBatchProductionDetail.Gets(oFBP.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Batchmans
                        _oFabricBatchProduction.FabricMachines.Add(oFabricMachine);
                    }
                    _oFabricBatchProduction.FBPBList = FabricBatchProductionBeam.GetsByFabricBatchProduction(_oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                _oFabricBatchProductions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE WeavingProcess in (" + (int)EnumWeavingProcess.Sizing + ")  and FBID=" + oFBP.FBID, (int)Session[SessionInfo.currentUserID]);

                _oFabricBatchProduction.FBPBs = FabricBatchProductionBeam.Gets("SELECT * FROM View_FabricBatchProductionBeam WHERE  FBPID in  (Select FBPID from FabricBatchProduction where WeavingProcess=" + (int)EnumWeavingProcess.Warping + " and FBID=" + oFBP.FBID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricBatchProduction.FabricBatchRawMaterials = FabricBatchRawMaterial.Gets("SELECT * FROM View_FabricBatchRawMaterial WHERE FBID = " + oFBP.FBID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                _oFabricBatchProduction.FabricBatch = _oFabricBatchProduction.FabricBatch.Get(oFBP.FBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricSCReport = oFabricSCReport.Get(_oFabricBatchProduction.FabricBatch.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFabricSCReport.OrderType == (int)EnumFabricRequestType.Bulk)
                {
                    _oFabricBatchProduction.GainLossPer = 0.2; ///20%
                }
                else
                {
                    _oFabricBatchProduction.GainLossPer = 0.5;//50 % for Sample
                }

                _oFabricBatchProduction.FabricBatch.QtyPro = _oFabricBatchProductions.Where(x => x.FBPID != oFBP.FBPID).Sum(y => y.Qty);

                var oQtyPres = _oFabricBatchProduction.FBPBs.GroupBy(x => new { x.Qty, x.FBPID }, (key, grp) =>
                   new 
                   {
                       Qty = grp.Max(p => p.Qty)
                   }).ToList();


                _oFabricBatchProduction.QtyPrev = oQtyPres.Sum(y => y.Qty);

                if (_oFabricBatchProduction.EndTime.ToString("dd MMM yyyy") == new DateTime(0001, 01, 01).ToString("dd MMM yyyy"))
                {
                    _oFabricBatchProduction.EndTime = _oFabricBatchProduction.StartTime.AddMinutes(1);
                }
            }
            catch (Exception ex)
            {
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchProduction);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetProductionSizing(FabricBatchProduction oFBP)
        {
            _oFabricBatchProductions = new List<FabricBatchProduction>();
            _oFabricBatchProduction = new FabricBatchProduction();
            try
            {
                if (oFBP.ProductionStatus == EnumProductionStatus.Run)
                {
                    _oFabricBatchProductions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE WeavingProcess in (" + (int)EnumWeavingProcess.Sizing + ")  and ProductionStatus=" + (int)EnumProductionStatus.Run, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    List<FabricBatch> oFBs = new List<FabricBatch>();
                    oFBs = FabricBatch.Gets("SELECT * FROM View_FabricBatch WHERE isnull(FinishStatus,0)<" + (int)EnumFabricBatchState.Sizing_Finish + " and [Status] in (" + (int)EnumFabricBatchState.Warping + "," + (int)EnumFabricBatchState.warping_Finish + "," + (int)EnumFabricBatchState.Sizing + "," + (int)EnumFabricBatchState.Sizing_Finish + ") " + (!string.IsNullOrEmpty(oFBP.FEONo) ? " AND FEONo LIKE '%" + oFBP.FEONo + "%'" : "") + "  and  FBID NOT IN (SELECT FBID FROM FabricBatchProduction WHERE WeavingProcess in (" + (int)EnumWeavingProcess.Sizing + ")) and FWPDID in (Select FabricWarpPlanID from FabricSizingPlan where isnull(FabricWarpPlanID,0)>0 and PlanStatus in (" + (int)EnumFabricPlanStatus.Initialize + "," + (int)EnumFabricPlanStatus.Planned + "))", (int)Session[SessionInfo.currentUserID]);
                    //  oFBs = FabricBatch.Gets("Select top(500)* from (SELECT *,isnull((SELECT SUM(Qty) FROM FabricBatchProduction AS FBP where WeavingProcess in (" + (int)EnumWeavingProcess.Warping + ") and FBP.FBID =FB.FBID),0) as QtyPro FROM View_FabricBatch as FB where FBID=383 and FB.[Status] in (" + (int)EnumFabricBatchState.Initialize + "," + (int)EnumFabricBatchState.InFloor + "," + (int)EnumFabricBatchState.warping_Finish + "," + (int)EnumFabricBatchState.Sizing + "," + (int)EnumFabricBatchState.Sizing_Finish + ") ) as HH where  HH.Qty-10.0>isnull(HH.QtyPro,0) order by FBID DESC", (int)Session[SessionInfo.currentUserID]);
                    _oFabricBatchProductions = new List<FabricBatchProduction>();
                    foreach (FabricBatch oItem in oFBs)
                    {
                        _oFabricBatchProduction = new FabricBatchProduction();
                        _oFabricBatchProduction.FBID = oItem.FBID;
                        _oFabricBatchProduction.FabricSalesContractDetailID = oItem.FabricSalesContractDetailID;
                        _oFabricBatchProduction.FMID = oItem.FMID;
                        _oFabricBatchProduction.WeavingProcess = EnumWeavingProcess.Sizing;
                        _oFabricBatchProduction.Qty = oItem.Qty - oItem.QtyPro;
                        _oFabricBatchProduction.FEONo = oItem.FEONo;
                        _oFabricBatchProduction.BatchNo = oItem.BatchNo;
                        _oFabricBatchProduction.PlanType = oItem.PlanType;
                        _oFabricBatchProduction.FSpcType = oItem.FSpcType;
                        _oFabricBatchProduction.ProductName = oItem.ProductName;
                        _oFabricBatchProduction.BuyerName = oItem.BuyerName;
                        _oFabricBatchProduction.Construction = oItem.Construction;
                        _oFabricBatchProduction.FabricMachineName = oItem.WarpingMachineCode;
                        _oFabricBatchProduction.FabricWeave = oItem.Weave;
                        _oFabricBatchProduction.ColorName = oItem.Color;
                        _oFabricBatchProduction.FabricBatchStatus = oItem.Status;

                        if (oItem.QtyPro > 0)
                        {
                            _oFabricBatchProduction.ProductionStatus = EnumProductionStatus.Hold;
                        }
                        else
                        {
                            _oFabricBatchProduction.ProductionStatus = EnumProductionStatus.Initialize;
                        }
                        _oFabricBatchProductions.Add(_oFabricBatchProduction);
                    }
                    oFBs = new List<FabricBatch>();
                    oFBs = FabricBatch.Gets("Select * from (SELECT *,isnull((SELECT SUM(Qty) FROM FabricBatchProduction AS FBP where  WeavingProcess in (" + (int)EnumWeavingProcess.Sizing + ") and FBP.FBID =FB.FBID),0) as QtyPro FROM View_FabricBatch as FB where  isnull(FB.FinishStatus,0)<" + (int)EnumFabricBatchState.Sizing_Finish + " and  FB.[Status] in (" + (int)EnumFabricBatchState.Warping + "," + (int)EnumFabricBatchState.warping_Finish + "," + (int)EnumFabricBatchState.Sizing + "," + (int)EnumFabricBatchState.Sizing_Finish + ") " + (!string.IsNullOrEmpty(oFBP.FEONo) ? " AND FEONo LIKE '%" + oFBP.FEONo + "%'" : "") + " and FB.FBID not in (SELECT FBP.FBID FROM FabricBatchProduction AS FBP where  ProductionStatus in (0,1) and WeavingProcess in (1))) as HH where isnull(HH.QtyPro,0)>0 and (isnull(HH.Qty,0)-isnull(HH.QtyPro,0))>0.1", (int)Session[SessionInfo.currentUserID]);
                    foreach (FabricBatch oItem in oFBs)
                    {
                        _oFabricBatchProduction = new FabricBatchProduction();
                        _oFabricBatchProduction.FBID = oItem.FBID;
                        _oFabricBatchProduction.FMID = oItem.FMID;
                        _oFabricBatchProduction.WeavingProcess = EnumWeavingProcess.Sizing;
                        _oFabricBatchProduction.Qty = oItem.Qty - oItem.QtyPro;
                        _oFabricBatchProduction.FEONo = oItem.FEONo;
                        _oFabricBatchProduction.BatchNo = oItem.BatchNo;
                        _oFabricBatchProduction.PlanType = oItem.PlanType;
                        _oFabricBatchProduction.FSpcType = oItem.FSpcType;
                        _oFabricBatchProduction.ProductName = oItem.ProductName;
                        _oFabricBatchProduction.BuyerName = oItem.BuyerName;
                        _oFabricBatchProduction.Construction = oItem.Construction;
                        _oFabricBatchProduction.FabricMachineName = oItem.WarpingMachineCode;
                        _oFabricBatchProduction.FabricWeave = oItem.Weave;
                        _oFabricBatchProduction.ColorName = oItem.Color;
                        _oFabricBatchProduction.FabricBatchStatus = oItem.Status;
                        _oFabricBatchProduction.ProductionStatus = EnumProductionStatus.Initialize;
                        _oFabricBatchProductions.Add(_oFabricBatchProduction);
                    }
                    if (!string.IsNullOrEmpty(oFBP.FEONo))
                    {
                        _oFabricBatchProductions.AddRange(FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE WeavingProcess in (" + (int)EnumWeavingProcess.Sizing + ")  and FEONo LIKE '%" + oFBP.FEONo + "%'", (int)Session[SessionInfo.currentUserID]));
                        // _oFabricBatchProductions.AddRange(Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID));
                    }

                }
            }
            catch (Exception ex)
            {
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
                _oFabricBatchProductions.Add(_oFabricBatchProduction);
            }
            var jsonResult = Json(_oFabricBatchProductions, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion
        #region Sizing
        public ActionResult ViewSizing(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFabricBatchProduction = new FabricBatchProduction();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            //ViewBag.Stores = WorkingUnit.Gets("FabricBatch", (int)EnumTriggerParentsType._FabricProduction, (int)EnumOperationFunctionality._Disburse, (int)EnumInOutType.Disburse, false, 0, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=0 AND WeavingProcess = " + (int)EnumWeavingProcess.Sizing + " AND MachineStatus = " + (int)EnumMachineStatus.Free + " Order By Code";
            _oFabricBatchProduction.FabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.Beams = FabricMachine.Gets(true, EnumWeavingProcess.Sizing, EnumMachineStatus.Free, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=1 AND WeavingProcess = " + (int)EnumWeavingProcess.Sizing + " AND MachineStatus = " + (int)EnumMachineStatus.Free + "  Order By Code";
            sSQL = "SELECT * FROM View_FabricMachineType WHERE FabricMachineTypeID IN (SELECT DISTINCT ChildMachineTypeID FROM FabricMachine WHERE IsBeam=1 AND IsActive=1 AND WeavingProcess=1)";
            ViewBag.FabricMachineTypes = FabricMachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Stores = WorkingUnit.GetsPermittedStore(0, EnumModuleName.FabricBatch_PRO_Sizing, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            _oFabricBatchProduction.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oFabricBatchProduction);
        }
        [HttpGet]
        public JsonResult GetFabricBatchProductionForSizing(int nFBID)//FBID
        {
            FabricBatchProduction oFabricBatchProduction = new FabricBatchProduction();
            try
            {
                string sSQL = string.Empty;
                if (nFBID > 0)
                {
                    oFabricBatchProduction = oFabricBatchProduction.GetByBatchAndWeavingType(nFBID, EnumWeavingProcess.Sizing, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFabricBatchProduction.FabricBatchRawMaterials = FabricBatchRawMaterial.Gets(nFBID, EnumWeavingProcess.Sizing, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Rowmaterial
                    if (oFabricBatchProduction.FBPID <= 0)
                    {
                        FabricBatchProduction oFBP = new FabricBatchProduction();
                        oFBP = oFBP.GetByBatchAndWeavingType(nFBID, EnumWeavingProcess.Warping, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        if (oFBP.FBPID > 0)
                        {
                            oFabricBatchProduction.StartTime = oFBP.EndTime.AddMinutes(1);
                            oFabricBatchProduction.EndTime = oFBP.EndTime.AddMinutes(2);
                        }
                        else
                        {
                            oFabricBatchProduction.StartTime = DateTime.Now;
                            oFabricBatchProduction.EndTime = DateTime.Now.AddMinutes(1);
                        }
                    }
                    else
                    {
                        sSQL = "SELECT * FROM View_FabricMachine WHERE  IsBeam=0 AND WeavingProcess = " + (int)EnumWeavingProcess.Sizing + " AND FMID = " + oFabricBatchProduction.FMID + " Order By Code";
                        oFabricBatchProduction.FabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        if (oFabricBatchProduction.EndTime.ToString("dd MMM yyyy") == new DateTime(0001, 01, 01).ToString("dd MMM yyyy"))
                        {
                            oFabricBatchProduction.EndTime = oFabricBatchProduction.StartTime.AddMinutes(1);
                        }

                        oFabricBatchProduction.FBPBs1 = FabricBatchProductionBeam.GetsByFabricBatchProduction(oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }

                    _oFabricBatchProductions = new List<FabricBatchProduction>();
                    sSQL = "SELECT * FROM View_FabricBatchProduction WHERE FBID=" + nFBID + " AND WeavingProcess=" + (int)EnumWeavingProcess.Warping;
                    _oFabricBatchProductions = FabricBatchProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oFabricBatchProductions.Count > 0)
                    {
                        oFabricBatchProduction.FBPBs = FabricBatchProductionBeam.GetsByFabricBatchProduction(_oFabricBatchProductions[0].FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    oFabricBatchProduction.oFBSS = FabricBatchSizingSolution.Get(nFBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFabricBatchProduction.oFBSS.SugPrevRestQty = FabricBatchSizingSolution.GetPrevQtyForSizing(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFabricBatchProduction = new FabricBatchProduction();
                oFabricBatchProduction.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchProduction);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsBeamForSizing(FabricMachine oFabricMachine)
        {
            List<FabricMachine> oFabricMachines = new List<FabricMachine>();
            try
            {
                string sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=1 AND WeavingProcess = " + (int)EnumWeavingProcess.Sizing + " AND MachineStatus = " + (int)EnumMachineStatus.Free + " Order By Code";
                oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricMachine = new FabricMachine();
                oFabricMachine.ErrorMessage = ex.Message;
                oFabricMachines.Add(oFabricMachine);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsBeamForSizingSizing(FabricMachine oFabricMachine)
        {
            List<FabricMachine> oFabricMachines = new List<FabricMachine>();
            try
            {
                string sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=1 AND WeavingProcess = " + (int)EnumWeavingProcess.Sizing + " AND MachineStatus = " + (int)EnumMachineStatus.Free + " Order By Code";
                oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricMachine = new FabricMachine();
                oFabricMachine.ErrorMessage = ex.Message;
                oFabricMachines.Add(oFabricMachine);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBeamsForSizing(FabricBatchProduction oFBP)
        {
            _oFabricBatchProduction = new FabricBatchProduction();
            _oFabricBatchProduction.FabricBatch = new FabricBatch();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            try
            {
                if (oFBP.FBPID > 0)
                {
                    _oFabricBatchProduction = _oFabricBatchProduction.Get(oFBP.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oFBP.FBPID > 0)
                    {
                        FabricMachine oFabricMachine = new FabricMachine();
                        oFabricMachine = oFabricMachine.Get(_oFabricBatchProduction.FMID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oFabricBatchProduction.FabricBatchProductionDetails = FabricBatchProductionDetail.Gets(oFBP.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Batchmans
                        _oFabricBatchProduction.FabricMachines.Add(oFabricMachine);
                    }
                    _oFabricBatchProduction.FBPBList = FabricBatchProductionBeam.GetsByFabricBatchProduction(_oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                _oFabricBatchProduction.FBPBs = FabricBatchProductionBeam.Gets("SELECT * FROM View_FabricBatchProductionBeam WHERE  FBPID in  (Select FBPID from FabricBatchProduction where WeavingProcess=" + (int)EnumWeavingProcess.Sizing + " and FBID=" + oFBP.FBID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                _oFabricBatchProduction.FabricBatch = _oFabricBatchProduction.FabricBatch.Get(oFBP.FBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricSCReport = oFabricSCReport.Get(oFBP.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFabricSCReport.OrderType == (int)EnumFabricRequestType.Bulk)
                {
                    _oFabricBatchProduction.GainLossPer = 0.2; ///20%
                }
                else
                {
                    _oFabricBatchProduction.GainLossPer = 0.5;//50 % for Sample
                }

                _oFabricBatchProduction.FabricBatch.QtyPro = _oFabricBatchProductions.Where(x => x.FBPID != oFBP.FBPID).Sum(y => y.Qty);

                if (_oFabricBatchProduction.EndTime.ToString("dd MMM yyyy") == new DateTime(0001, 01, 01).ToString("dd MMM yyyy"))
                {
                    _oFabricBatchProduction.EndTime = _oFabricBatchProduction.StartTime.AddMinutes(1);
                }
            }
            catch (Exception ex)
            {
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchProduction);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Drwaing
        public ActionResult ViewDrawingExecution(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricBatchProduction).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oFabricBatchProductions = new List<FabricBatchProduction>();
            _oFabricBatchProductions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE  WeavingProcess = " + (int)EnumWeavingProcess.Drawing_IN + " and ProductionStatus=" + (int)EnumProductionStatus.Run, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            //ViewBag.Stores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricBatch, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=0 AND WeavingProcess = " + (int)EnumWeavingProcess.Drawing_IN + " AND MachineStatus = " + (int)EnumMachineStatus.Free + " Order By Code";
            ViewBag.FabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BatchMans = Employee.Gets(EnumEmployeeDesignationType.WarpingOperator, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //   ViewBag.FabricBreakages = FabricBreakage.Gets(EnumWeavingProcess.Sizing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            // sSQL = "SELECT * FROM View_FabricMachineType WHERE FabricMachineTypeID IN (SELECT DISTINCT ChildMachineTypeID FROM FabricMachine WHERE IsBeam=1 AND IsActive=1 AND WeavingProcess=" + (int)EnumWeavingProcess.Sizing + ")";
            //  ViewBag.FabricMachineTypes = FabricMachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperatorTwo));
            ViewBag.ProductionStatusList = EnumObject.jGets(typeof(EnumProductionStatus));
            // ViewBag.Stores = WorkingUnit.GetsPermittedStore(0, EnumModuleName.FabricBatch_PRO_Sizing, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);

            return View(_oFabricBatchProductions);
        }
        [HttpPost]
        public JsonResult GetFabricBatchProductionByIDDrawing(FabricBatchProduction oFBP)
        {
            _oFabricBatchProduction = new FabricBatchProduction();
            _oFabricBatchProduction.FabricBatch = new FabricBatch();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            try
            {
                if (oFBP.FBPID > 0)
                {
                    _oFabricBatchProduction = _oFabricBatchProduction.Get(oFBP.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oFBP.FBPID > 0)
                    {
                        FabricMachine oFabricMachine = new FabricMachine();
                        oFabricMachine = oFabricMachine.Get(_oFabricBatchProduction.FMID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oFabricBatchProduction.FabricBatchProductionDetails = FabricBatchProductionDetail.Gets(oFBP.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Batchmans
                        _oFabricBatchProduction.FabricMachines.Add(oFabricMachine);
                    }
                    _oFabricBatchProduction.FBPBList = FabricBatchProductionBeam.GetsByFabricBatchProduction(_oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                _oFabricBatchProduction.FBPBs = FabricBatchProductionBeam.Gets("SELECT * FROM View_FabricBatchProductionBeam WHERE IsDrawing = " + (int)EnumFabricBatchState.DrawingIn + " AND FBPID in  (Select FBPID from FabricBatchProduction where WeavingProcess=" + (int)EnumWeavingProcess.Sizing + " and FBID=" + oFBP.FBID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                _oFabricBatchProduction.FabricBatch = _oFabricBatchProduction.FabricBatch.Get(oFBP.FBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricSCReport = oFabricSCReport.Get(oFBP.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFabricSCReport.OrderType == (int)EnumFabricRequestType.Bulk)
                {
                    _oFabricBatchProduction.GainLossPer = 0.2; ///20%
                }
                else
                {
                    _oFabricBatchProduction.GainLossPer = 0.5;//50 % for Sample
                }

                _oFabricBatchProduction.FabricBatch.QtyPro = _oFabricBatchProductions.Where(x => x.FBPID != oFBP.FBPID).Sum(y => y.Qty);

                if (_oFabricBatchProduction.EndTime.ToString("dd MMM yyyy") == new DateTime(0001, 01, 01).ToString("dd MMM yyyy"))
                {
                    _oFabricBatchProduction.EndTime = _oFabricBatchProduction.StartTime.AddMinutes(1);
                }
            }
            catch (Exception ex)
            {
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchProduction);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetProductionDrawing(FabricBatchProduction oFBP)
        {
            _oFabricBatchProductions = new List<FabricBatchProduction>();
            _oFabricBatchProduction = new FabricBatchProduction();
            try
            {
                if (oFBP.ProductionStatus == EnumProductionStatus.Run)
                {
                    _oFabricBatchProductions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE WeavingProcess in (" + (int)EnumWeavingProcess.Drawing_IN + ")  and ProductionStatus=" + (int)EnumProductionStatus.Run, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    List<FabricBatch> oFBs = new List<FabricBatch>();
                    oFBs = FabricBatch.Gets("SELECT * FROM View_FabricBatch WHERE isnull(FinishStatus,0)<" + (int)EnumFabricBatchState.DrawingIn_Finish + " and [Status] in (" + (int)EnumFabricBatchState.Warping + "," + (int)EnumFabricBatchState.warping_Finish + "," + (int)EnumFabricBatchState.Sizing + "," + (int)EnumFabricBatchState.Sizing_Finish + "," +(int)EnumFabricBatchState.LeasingIn + "," + (int)EnumFabricBatchState.LeasingIn_Finish + ") " + (!string.IsNullOrEmpty(oFBP.FEONo) ? " AND FEONo LIKE '%" + oFBP.FEONo + "%'" : "") + "  and  FBID NOT IN (SELECT FBID FROM FabricBatchProduction WHERE WeavingProcess in (" + (int)EnumWeavingProcess.Drawing_IN + ")) AND FBID IN (SELECT FBID FROM View_FabricBatchProductionBeam WHERE IsDrawing = " + (int)EnumFabricBatchState.DrawingIn + ")", (int)Session[SessionInfo.currentUserID]);
                    //oFBs = FabricBatch.Gets("SELECT * FROM View_FabricBatch WHERE isnull(FinishStatus,0)<" + (int)EnumFabricBatchState.Sizing_Finish + " and [Status] in (" + (int)EnumFabricBatchState.Warping + "," + (int)EnumFabricBatchState.warping_Finish + "," + (int)EnumFabricBatchState.Sizing + "," + (int)EnumFabricBatchState.Sizing_Finish + ") " + (!string.IsNullOrEmpty(oFBP.FEONo) ? " AND FEONo LIKE '%" + oFBP.FEONo + "%'" : "") + "  and  FBID NOT IN (SELECT FBID FROM FabricBatchProduction WHERE WeavingProcess in (" + (int)EnumWeavingProcess.Sizing + ")) and FWPDID in (Select FabricWarpPlanID from FabricSizingPlan where isnull(FabricWarpPlanID,0)>0 and PlanStatus in (" + (int)EnumFabricPlanStatus.Initialize + "," + (int)EnumFabricPlanStatus.Planned + "))", (int)Session[SessionInfo.currentUserID]);                    
                    _oFabricBatchProductions = new List<FabricBatchProduction>();
                    foreach (FabricBatch oItem in oFBs)
                    {
                        _oFabricBatchProduction = new FabricBatchProduction();
                        _oFabricBatchProduction.FBID = oItem.FBID;
                        _oFabricBatchProduction.FMID = oItem.FMID;
                        _oFabricBatchProduction.WeavingProcess = EnumWeavingProcess.Sizing;
                        _oFabricBatchProduction.Qty = oItem.Qty - oItem.QtyPro;
                        _oFabricBatchProduction.FEONo = oItem.FEONo;
                        _oFabricBatchProduction.BatchNo = oItem.BatchNo;
                        _oFabricBatchProduction.ProductName = oItem.ProductName;
                        _oFabricBatchProduction.BuyerName = oItem.BuyerName;
                        _oFabricBatchProduction.Construction = oItem.Construction;
                        _oFabricBatchProduction.FabricMachineName = oItem.WarpingMachineCode;
                        _oFabricBatchProduction.FabricWeave = oItem.Weave;
                        _oFabricBatchProduction.ColorName = oItem.Color;
                        _oFabricBatchProduction.FabricBatchStatus = oItem.Status;

                        if (oItem.QtyPro > 0)
                        {
                            _oFabricBatchProduction.ProductionStatus = EnumProductionStatus.Hold;
                        }
                        else
                        {
                            _oFabricBatchProduction.ProductionStatus = EnumProductionStatus.Initialize;
                        }
                        _oFabricBatchProductions.Add(_oFabricBatchProduction);
                    }
                    #region Partial
                    //oFBs = new List<FabricBatch>();
                    //oFBs = FabricBatch.Gets("Select * from (SELECT *,isnull((SELECT SUM(Qty) FROM FabricBatchProduction AS FBP where  WeavingProcess in (" + (int)EnumWeavingProcess.Drawing_IN + ") and FBP.FBID =FB.FBID),0) as QtyPro FROM View_FabricBatch as FB where  isnull(FB.FinishStatus,0)<" + (int)EnumFabricBatchState.DrawingIn_Finish + " and  FB.[Status] in (" + (int)EnumFabricBatchState.Sizing + "," + (int)EnumFabricBatchState.Sizing_Finish + "," + (int)EnumFabricBatchState.DrawingIn + "," + (int)EnumFabricBatchState.DrawingIn_Finish + ") " + (!string.IsNullOrEmpty(oFBP.FEONo) ? " AND FEONo LIKE '%" + oFBP.FEONo + "%'" : "") + " AND FBID IN (SELECT FBID FROM View_FabricBatchProductionBeam WHERE IsDrawing = " + (int)EnumFabricBatchState.DrawingIn + ") and FB.FBID not in (SELECT FBP.FBID FROM FabricBatchProduction AS FBP where  ProductionStatus in (0,1) and WeavingProcess in (" + (int)EnumWeavingProcess.Drawing_IN + "))) as HH where isnull(HH.QtyPro,0)>0 and (isnull(HH.Qty,0)-isnull(HH.QtyPro,0))>0.1", (int)Session[SessionInfo.currentUserID]);
                    ////oFBs = FabricBatch.Gets("Select * from (SELECT *,isnull((SELECT SUM(Qty) FROM FabricBatchProduction AS FBP where  WeavingProcess in (" + (int)EnumWeavingProcess.Sizing + ") and FBP.FBID =FB.FBID),0) as QtyPro FROM View_FabricBatch as FB where  isnull(FB.FinishStatus,0)<" + (int)EnumFabricBatchState.Sizing_Finish + " and  FB.[Status] in (" + (int)EnumFabricBatchState.Warping + "," + (int)EnumFabricBatchState.warping_Finish + "," + (int)EnumFabricBatchState.Sizing + "," + (int)EnumFabricBatchState.Sizing_Finish + ") " + (!string.IsNullOrEmpty(oFBP.FEONo) ? " AND FEONo LIKE '%" + oFBP.FEONo + "%'" : "") + " and FB.FBID in (SELECT FBP.FBID FROM FabricBatchProduction AS FBP where  ProductionStatus in (2,3) and WeavingProcess in (1))) as HH where (isnull(HH.Qty,0)-isnull(HH.QtyPro,0))>0.1", (int)Session[SessionInfo.currentUserID]);
                    //foreach (FabricBatch oItem in oFBs)
                    //{
                    //    _oFabricBatchProduction = new FabricBatchProduction();
                    //    _oFabricBatchProduction.FBID = oItem.FBID;
                    //    _oFabricBatchProduction.FMID = oItem.FMID;
                    //    _oFabricBatchProduction.WeavingProcess = EnumWeavingProcess.Sizing;
                    //    _oFabricBatchProduction.Qty = oItem.Qty - oItem.QtyPro;
                    //    _oFabricBatchProduction.FEONo = oItem.FEONo;
                    //    _oFabricBatchProduction.BatchNo = oItem.BatchNo;
                    //    _oFabricBatchProduction.ProductName = oItem.ProductName;
                    //    _oFabricBatchProduction.BuyerName = oItem.BuyerName;
                    //    _oFabricBatchProduction.Construction = oItem.Construction;
                    //    _oFabricBatchProduction.FabricMachineName = oItem.WarpingMachineCode;
                    //    _oFabricBatchProduction.FabricWeave = oItem.Weave;
                    //    _oFabricBatchProduction.ColorName = oItem.Color;
                    //    _oFabricBatchProduction.FabricBatchStatus = oItem.Status;
                    //    _oFabricBatchProduction.ProductionStatus = EnumProductionStatus.Initialize;
                    //    _oFabricBatchProductions.Add(_oFabricBatchProduction);
                    //}
                    #endregion
                    if (!string.IsNullOrEmpty(oFBP.FEONo))
                    {
                        _oFabricBatchProductions.AddRange(FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE WeavingProcess in (" + (int)EnumWeavingProcess.Drawing_IN + ")  and FEONo LIKE '%" + oFBP.FEONo + "%'", (int)Session[SessionInfo.currentUserID]));
                    }

                }
            }
            catch (Exception ex)
            {
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
                _oFabricBatchProductions.Add(_oFabricBatchProduction);
            }
            var jsonResult = Json(_oFabricBatchProductions, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Leasing
        public ActionResult ViewLeasingExecution(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricBatchProduction).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oFabricBatchProductions = new List<FabricBatchProduction>();
            _oFabricBatchProductions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE  WeavingProcess = " + (int)EnumWeavingProcess.Leasing_IN + " and ProductionStatus=" + (int)EnumProductionStatus.Run, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            //ViewBag.Stores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricBatch, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=0 AND WeavingProcess = " + (int)EnumWeavingProcess.Leasing_IN + " AND MachineStatus = " + (int)EnumMachineStatus.Free + " Order By Code";
            ViewBag.FabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BatchMans = Employee.Gets(EnumEmployeeDesignationType.WarpingOperator, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //   ViewBag.FabricBreakages = FabricBreakage.Gets(EnumWeavingProcess.Sizing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            // sSQL = "SELECT * FROM View_FabricMachineType WHERE FabricMachineTypeID IN (SELECT DISTINCT ChildMachineTypeID FROM FabricMachine WHERE IsBeam=1 AND IsActive=1 AND WeavingProcess=" + (int)EnumWeavingProcess.Sizing + ")";
            //  ViewBag.FabricMachineTypes = FabricMachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperatorTwo));
            ViewBag.ProductionStatusList = EnumObject.jGets(typeof(EnumProductionStatus));
            // ViewBag.Stores = WorkingUnit.GetsPermittedStore(0, EnumModuleName.FabricBatch_PRO_Sizing, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);

            return View(_oFabricBatchProductions);
        }
        [HttpPost]
        public JsonResult GetFabricBatchProductionByIDLeasing(FabricBatchProduction oFBP)
        {
            _oFabricBatchProduction = new FabricBatchProduction();
            _oFabricBatchProduction.FabricBatch = new FabricBatch();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            try
            {
                if (oFBP.FBPID > 0)
                {
                    _oFabricBatchProduction = _oFabricBatchProduction.Get(oFBP.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oFBP.FBPID > 0)
                    {
                        FabricMachine oFabricMachine = new FabricMachine();
                        oFabricMachine = oFabricMachine.Get(_oFabricBatchProduction.FMID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oFabricBatchProduction.FabricBatchProductionDetails = FabricBatchProductionDetail.Gets(oFBP.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Batchmans
                        _oFabricBatchProduction.FabricMachines.Add(oFabricMachine);
                    }
                    _oFabricBatchProduction.FBPBList = FabricBatchProductionBeam.GetsByFabricBatchProduction(_oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                _oFabricBatchProduction.FBPBs = FabricBatchProductionBeam.Gets("SELECT * FROM View_FabricBatchProductionBeam WHERE IsDrawing = "+(int)EnumFabricBatchState.LeasingIn+" AND  FBPID in  (Select FBPID from FabricBatchProduction where WeavingProcess=" + (int)EnumWeavingProcess.Sizing + " and FBID=" + oFBP.FBID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                _oFabricBatchProduction.FabricBatch = _oFabricBatchProduction.FabricBatch.Get(oFBP.FBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricSCReport = oFabricSCReport.Get(oFBP.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFabricSCReport.OrderType == (int)EnumFabricRequestType.Bulk)
                {
                    _oFabricBatchProduction.GainLossPer = 0.2; ///20%
                }
                else
                {
                    _oFabricBatchProduction.GainLossPer = 0.5;//50 % for Sample
                }

                _oFabricBatchProduction.FabricBatch.QtyPro = _oFabricBatchProductions.Where(x => x.FBPID != oFBP.FBPID).Sum(y => y.Qty);

                if (_oFabricBatchProduction.EndTime.ToString("dd MMM yyyy") == new DateTime(0001, 01, 01).ToString("dd MMM yyyy"))
                {
                    _oFabricBatchProduction.EndTime = _oFabricBatchProduction.StartTime.AddMinutes(1);
                }
            }
            catch (Exception ex)
            {
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchProduction);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetProductionLeasing(FabricBatchProduction oFBP)
        {
            _oFabricBatchProductions = new List<FabricBatchProduction>();
            _oFabricBatchProduction = new FabricBatchProduction();
            try
            {
                if (oFBP.ProductionStatus == EnumProductionStatus.Run)
                {
                    _oFabricBatchProductions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE WeavingProcess in (" + (int)EnumWeavingProcess.Leasing_IN + ")  and ProductionStatus=" + (int)EnumProductionStatus.Run, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    List<FabricBatch> oFBs = new List<FabricBatch>();
                    oFBs = FabricBatch.Gets("SELECT * FROM View_FabricBatch WHERE isnull(FinishStatus,0)<" + (int)EnumFabricBatchState.LeasingIn_Finish + " and [Status] in (" + (int)EnumFabricBatchState.Warping + "," + (int)EnumFabricBatchState.warping_Finish + "," + (int)EnumFabricBatchState.Sizing + "," + (int)EnumFabricBatchState.Sizing_Finish + "," + (int)EnumFabricBatchState.DrawingIn + "," + (int)EnumFabricBatchState.DrawingIn_Finish + ") " + (!string.IsNullOrEmpty(oFBP.FEONo) ? " AND FEONo LIKE '%" + oFBP.FEONo + "%'" : "") + "  and  FBID NOT IN (SELECT FBID FROM FabricBatchProduction WHERE WeavingProcess in (" + (int)EnumWeavingProcess.Leasing_IN + ")) AND FBID IN (SELECT FBID FROM View_FabricBatchProductionBeam WHERE IsDrawing = " + (int)EnumFabricBatchState.LeasingIn + ")", (int)Session[SessionInfo.currentUserID]);
                    //oFBs = FabricBatch.Gets("SELECT * FROM View_FabricBatch WHERE isnull(FinishStatus,0)<" + (int)EnumFabricBatchState.Sizing_Finish + " and [Status] in (" + (int)EnumFabricBatchState.Warping + "," + (int)EnumFabricBatchState.warping_Finish + "," + (int)EnumFabricBatchState.Sizing + "," + (int)EnumFabricBatchState.Sizing_Finish + ") " + (!string.IsNullOrEmpty(oFBP.FEONo) ? " AND FEONo LIKE '%" + oFBP.FEONo + "%'" : "") + "  and  FBID NOT IN (SELECT FBID FROM FabricBatchProduction WHERE WeavingProcess in (" + (int)EnumWeavingProcess.Sizing + ")) and FWPDID in (Select FabricWarpPlanID from FabricSizingPlan where isnull(FabricWarpPlanID,0)>0 and PlanStatus in (" + (int)EnumFabricPlanStatus.Initialize + "," + (int)EnumFabricPlanStatus.Planned + "))", (int)Session[SessionInfo.currentUserID]);                    
                    _oFabricBatchProductions = new List<FabricBatchProduction>();
                    foreach (FabricBatch oItem in oFBs)
                    {
                        _oFabricBatchProduction = new FabricBatchProduction();
                        _oFabricBatchProduction.FBID = oItem.FBID;
                        _oFabricBatchProduction.FMID = oItem.FMID;
                        _oFabricBatchProduction.WeavingProcess = EnumWeavingProcess.Sizing;
                        _oFabricBatchProduction.Qty = oItem.Qty - oItem.QtyPro;
                        _oFabricBatchProduction.FEONo = oItem.FEONo;
                        _oFabricBatchProduction.BatchNo = oItem.BatchNo;
                        _oFabricBatchProduction.ProductName = oItem.ProductName;
                        _oFabricBatchProduction.BuyerName = oItem.BuyerName;
                        _oFabricBatchProduction.Construction = oItem.Construction;
                        _oFabricBatchProduction.FabricMachineName = oItem.WarpingMachineCode;
                        _oFabricBatchProduction.FabricWeave = oItem.Weave;
                        _oFabricBatchProduction.ColorName = oItem.Color;
                        _oFabricBatchProduction.FabricBatchStatus = oItem.Status;

                        if (oItem.QtyPro > 0)
                        {
                            _oFabricBatchProduction.ProductionStatus = EnumProductionStatus.Hold;
                        }
                        else
                        {
                            _oFabricBatchProduction.ProductionStatus = EnumProductionStatus.Initialize;
                        }
                        _oFabricBatchProductions.Add(_oFabricBatchProduction);
                    }
                    #region Partial
                    //oFBs = new List<FabricBatch>();
                    //oFBs = FabricBatch.Gets("Select * from (SELECT *,isnull((SELECT SUM(Qty) FROM FabricBatchProduction AS FBP where  WeavingProcess in (" + (int)EnumWeavingProcess.Leasing_IN + ") and FBP.FBID =FB.FBID),0) as QtyPro FROM View_FabricBatch as FB where  isnull(FB.FinishStatus,0)<" + (int)EnumFabricBatchState.LeasingIn_Finish + " and  FB.[Status] in (" + (int)EnumFabricBatchState.Sizing + "," + (int)EnumFabricBatchState.Sizing_Finish + "," + (int)EnumFabricBatchState.LeasingIn + "," + (int)EnumFabricBatchState.LeasingIn_Finish + ") " + (!string.IsNullOrEmpty(oFBP.FEONo) ? " AND FEONo LIKE '%" + oFBP.FEONo + "%'" : "") + " AND FBID IN (SELECT FBID FROM View_FabricBatchProductionBeam WHERE IsDrawing = "+(int)EnumFabricBatchState.LeasingIn+") and FB.FBID not in (SELECT FBP.FBID FROM FabricBatchProduction AS FBP where  ProductionStatus in (0,1) and WeavingProcess in (" + (int)EnumWeavingProcess.Leasing_IN + "))) as HH where (isnull(HH.Qty,0)-isnull(HH.QtyPro,0))>0.1", (int)Session[SessionInfo.currentUserID]);
                    ////oFBs = FabricBatch.Gets("Select * from (SELECT *,isnull((SELECT SUM(Qty) FROM FabricBatchProduction AS FBP where  WeavingProcess in (" + (int)EnumWeavingProcess.Sizing + ") and FBP.FBID =FB.FBID),0) as QtyPro FROM View_FabricBatch as FB where  isnull(FB.FinishStatus,0)<" + (int)EnumFabricBatchState.Sizing_Finish + " and  FB.[Status] in (" + (int)EnumFabricBatchState.Warping + "," + (int)EnumFabricBatchState.warping_Finish + "," + (int)EnumFabricBatchState.Sizing + "," + (int)EnumFabricBatchState.Sizing_Finish + ") " + (!string.IsNullOrEmpty(oFBP.FEONo) ? " AND FEONo LIKE '%" + oFBP.FEONo + "%'" : "") + " and FB.FBID in (SELECT FBP.FBID FROM FabricBatchProduction AS FBP where  ProductionStatus in (2,3) and WeavingProcess in (1))) as HH where (isnull(HH.Qty,0)-isnull(HH.QtyPro,0))>0.1", (int)Session[SessionInfo.currentUserID]);
                    //foreach (FabricBatch oItem in oFBs)
                    //{
                    //    _oFabricBatchProduction = new FabricBatchProduction();
                    //    _oFabricBatchProduction.FBID = oItem.FBID;
                    //    _oFabricBatchProduction.FMID = oItem.FMID;
                    //    _oFabricBatchProduction.WeavingProcess = EnumWeavingProcess.Sizing;
                    //    _oFabricBatchProduction.Qty = oItem.Qty - oItem.QtyPro;
                    //    _oFabricBatchProduction.FEONo = oItem.FEONo;
                    //    _oFabricBatchProduction.BatchNo = oItem.BatchNo;
                    //    _oFabricBatchProduction.ProductName = oItem.ProductName;
                    //    _oFabricBatchProduction.BuyerName = oItem.BuyerName;
                    //    _oFabricBatchProduction.Construction = oItem.Construction;
                    //    _oFabricBatchProduction.FabricMachineName = oItem.WarpingMachineCode;
                    //    _oFabricBatchProduction.FabricWeave = oItem.Weave;
                    //    _oFabricBatchProduction.ColorName = oItem.Color;
                    //    _oFabricBatchProduction.FabricBatchStatus = oItem.Status;
                    //    _oFabricBatchProduction.ProductionStatus = EnumProductionStatus.Initialize;
                    //    _oFabricBatchProductions.Add(_oFabricBatchProduction);
                    //}
                    #endregion
                    if (!string.IsNullOrEmpty(oFBP.FEONo))
                    {
                        _oFabricBatchProductions.AddRange(FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE WeavingProcess in (" + (int)EnumWeavingProcess.Leasing_IN + ")  and FEONo LIKE '%" + oFBP.FEONo + "%'", (int)Session[SessionInfo.currentUserID]));
                    }
                }
            }
            catch (Exception ex)
            {
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
                _oFabricBatchProductions.Add(_oFabricBatchProduction);
            }
            var jsonResult = Json(_oFabricBatchProductions, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        //#region Weaving
        //public ActionResult ViewWeaving(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);
        //    _oFabricBatchProduction = new FabricBatchProduction();
        //    List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
        //    _oFabricBatchProduction.FabricMachines = FabricMachine.Gets(false, EnumWeavingProcess.Loom, EnumMachineStatus.Free, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    _oFabricBatchProduction.BatchMans = Employee.Gets(EnumEmployeeType.LoomOperator, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    _oFabricBatchProduction.FabricBreakages = FabricBreakage.Gets(EnumWeavingProcess.Loom, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    _oFabricBatchProduction.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

        //    string sSQL = "SELECT * FROM FabricMachine WHERE IsActive=1 And IsBeam=1 AND WeavingProcess = " + (int)EnumWeavingProcess.Loom + " AND MachineStatus = " + (int)EnumMachineStatus.Free + " Order By Code";
        //    ViewBag.Beams = FabricMachine.Gets(true, EnumWeavingProcess.Loom, EnumMachineStatus.Free, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    return View(_oFabricBatchProduction);
        //}
        //[HttpGet]
        //public JsonResult GetFabricBatchProductionForWeaving(int nFBID)//FBID
        //{
        //    FabricBatchProduction oFabricBatchProduction = new FabricBatchProduction();
        //    try
        //    {

        //        if (nFBID > 0)
        //        {
        //            oFabricBatchProduction = oFabricBatchProduction.GetByBatchAndWeavingType(nFBID, EnumWeavingProcess.Loom, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //            if (oFabricBatchProduction.FBPID > 0)
        //            {
        //                oFabricBatchProduction.FabricBatchProductionBreakages = FabricBatchProductionBreakage.Gets(oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Breakages
        //                oFabricBatchProduction.FabricBatchProductionBatchMans = FabricBatchProductionBatchMan.Gets(oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Batchmans
        //                if (oFabricBatchProduction.EndTime.ToString("dd MMM yyyy") == new DateTime(0001, 01, 01).ToString("dd MMM yyyy"))
        //                {
        //                    oFabricBatchProduction.EndTime = oFabricBatchProduction.StartTime.AddMinutes(1);
        //                }
        //                oFabricBatchProduction.FBPBs1 = FabricBatchProductionBeam.GetsByFabricBatchProduction(oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //            }
        //            else
        //            {
        //                FabricBatchProduction oFBP = new FabricBatchProduction();
        //                //If get fabric production plan from Drawing_IN then set Weaving.StartTime = Drawing_IN.EndTime
        //                oFBP = oFBP.GetByBatchAndWeavingType(nFBID, EnumWeavingProcess.Drawing_IN, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //                if (oFBP.FBPID > 0)
        //                {
        //                    oFabricBatchProduction.StartTime = oFBP.EndTime.AddMinutes(1);
        //                    oFabricBatchProduction.EndTime = oFBP.EndTime.AddMinutes(2);
        //                }
        //                else
        //                {
        //                    //If Drawing_IN not created with this fabric batch then set Weaving.StartTime = Sizing.EndTime
        //                    oFBP = oFBP.GetByBatchAndWeavingType(nFBID, EnumWeavingProcess.Sizing, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //                    if (oFBP.FBPID > 0)
        //                    {
        //                        oFabricBatchProduction.StartTime = oFBP.EndTime.AddMinutes(1);
        //                        oFabricBatchProduction.EndTime = oFBP.EndTime.AddMinutes(2);
        //                    }
        //                    else
        //                    {
        //                        oFabricBatchProduction.StartTime = DateTime.Now;
        //                        oFabricBatchProduction.EndTime = DateTime.Now.AddMinutes(1);
        //                    }
        //                }
        //            }

        //            _oFabricBatchProductions = new List<FabricBatchProduction>();
        //            string sSQL = "SELECT * FROM View_FabricBatchProduction WHERE FBID=" + nFBID + " AND WeavingProcess=" + (int)EnumWeavingProcess.Sizing;
        //            _oFabricBatchProductions = FabricBatchProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //            if (_oFabricBatchProductions.Count > 0)
        //            {
        //                oFabricBatchProduction.FBPBs = FabricBatchProductionBeam.GetsByFabricBatchProduction(_oFabricBatchProductions[0].FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        oFabricBatchProduction = new FabricBatchProduction();
        //        oFabricBatchProduction.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oFabricBatchProduction);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //#region PrintRollDoffingCard
        //public ActionResult PrintRollDoffingCard(int nFBPID, double nts)
        //{
        //    _oFabricBatchProduction = new FabricBatchProduction();
        //    _oFabricBatchProduction = _oFabricBatchProduction.Get(nFBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    _oFabricBatchProduction.FabricBatchProductionBatchMans = FabricBatchProductionBatchMan.Gets(nFBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    rptRollDoffingCard oReport = new rptRollDoffingCard();
        //    byte[] abytes = oReport.PrepareReport(_oFabricBatchProduction, oCompany);
        //    return File(abytes, "application/pdf");

        //}
        //#endregion
        //[HttpGet]
        //public JsonResult GetNextStartTime(int nFBID)//FBID
        //{
        //    _oFabricBatchProduction = new FabricBatchProduction();
        //    try
        //    {
        //        _oFabricBatchProductions = new List<FabricBatchProduction>();
        //        if (nFBID > 0)
        //        {
        //            string sSQL = "SELECT * FROM View_FabricBatchProduction WHERE WeavingProcess=" + (int)EnumWeavingProcess.Drawing_IN + " AND FBID=" + nFBID;
        //            _oFabricBatchProductions = FabricBatchProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //            if (_oFabricBatchProductions.Count == 0)
        //            {
        //                sSQL = "SELECT * FROM View_FabricBatchProduction WHERE WeavingProcess=" + (int)EnumWeavingProcess.Sizing + " AND FBID=" + nFBID;
        //                _oFabricBatchProductions = FabricBatchProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //            }

        //            if (_oFabricBatchProductions.Count > 0)
        //            {
        //                _oFabricBatchProduction = _oFabricBatchProductions[0];
        //                _oFabricBatchProduction.StartTime = _oFabricBatchProduction.EndTime.AddMinutes(1);
        //                _oFabricBatchProduction.EndTime = _oFabricBatchProduction.StartTime.AddHours(1);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _oFabricBatchProduction = new FabricBatchProduction();
        //        _oFabricBatchProduction.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oFabricBatchProduction);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //[HttpPost]
        //public JsonResult GetNextEnddateFromStartDate(FabricBatchProduction oFabricBatchProduction)
        //{
        //    try
        //    {
        //        oFabricBatchProduction.EndTime = oFabricBatchProduction.StartTime.AddHours(1);
        //    }
        //    catch (Exception ex)
        //    {
        //        oFabricBatchProduction = new FabricBatchProduction();
        //        oFabricBatchProduction.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oFabricBatchProduction);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult View_Weaving(string sFBPBeamID, string sBtnID, string sMsg)
        //{
        //    FabricBatch oFB = new FabricBatch();
        //    _oFabricBatchProduction = new FabricBatchProduction();
        //    List<FabricBatchProductionBatchMan> oFBPBMs = new List<FabricBatchProductionBatchMan>();

        //    int nFBPBeamID = Convert.ToInt32(sFBPBeamID.Split('~')[0]);
        //    int nBeamID = Convert.ToInt32(sFBPBeamID.Split('~')[1]);
        //    int nFBID = Convert.ToInt32(sFBPBeamID.Split('~')[2]);
        //    int nFabricSalesContractDetailID = Convert.ToInt32(sFBPBeamID.Split('~')[3]);
        //    int nFBPID = Convert.ToInt32(sFBPBeamID.Split('~')[4]);
        //    int nBUID = Convert.ToInt32(sFBPBeamID.Split('~')[5]);
        //    _oFabricBatchProduction.FBID = nFBID;
        //    _oFabricBatchProduction.FabricSalesContractDetailID = nFabricSalesContractDetailID;

        //    _oFabricBatchProduction.FabricMachines = FabricMachine.Gets(false, EnumWeavingProcess.Loom, EnumMachineStatus.Free, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    _oFabricBatchProduction.BatchMans = Employee.Gets(EnumEmployeeType.LoomOperator, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    _oFabricBatchProduction.FabricBreakages = FabricBreakage.Gets(EnumWeavingProcess.Loom, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    _oFabricBatchProduction.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);


        //    #region FabricBatchProduction
        //    // string sSQL = "SELECT * FROM View_FabricBatchProduction FBP WHERE FBP.FabricSalesContractDetailID=" + nFabricSalesContractDetailID + " AND FBP.WeavingProcess=" + (int)EnumWeavingProcess.Loom + " AND FBP.FBPID IN (SELECT FBPB.FBPID FROM FabricBatchProductionBeam FBPB WHERE FBPB.BeamID=" + nBeamID + ")";
        //    string sSQL = "SELECT * FROM View_FabricBatchProduction FBP WHERE FBP.FEOID=" + nFabricSalesContractDetailID + " AND FBP.WeavingProcess=" + (int)EnumWeavingProcess.Loom + " AND FBP.FBPID =" + nFBPID + " AND EndTime IS NULL";
        //    _oFabricBatchProductions = FabricBatchProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    if (_oFabricBatchProductions.Count > 0)
        //    {
        //        _oFabricBatchProduction.FBPID = _oFabricBatchProductions[0].FBPID;
        //        _oFabricBatchProduction.FBID = _oFabricBatchProductions[0].FBID;
        //        _oFabricBatchProduction.WeavingProcess = _oFabricBatchProductions[0].WeavingProcess;
        //        _oFabricBatchProduction.FMID = _oFabricBatchProductions[0].FMID;
        //        _oFabricBatchProduction.StartTime = _oFabricBatchProductions[0].StartTime;
        //        _oFabricBatchProduction.EndTime = _oFabricBatchProductions[0].EndTime;
        //        _oFabricBatchProduction.Qty = _oFabricBatchProductions[0].Qty;
        //        _oFabricBatchProduction.RPM = _oFabricBatchProductions[0].RPM;
        //        _oFabricBatchProduction.Reed = _oFabricBatchProductions[0].Reed;
        //        _oFabricBatchProduction.Dent = _oFabricBatchProductions[0].Dent;
        //        _oFabricBatchProduction.TSUID = _oFabricBatchProductions[0].TSUID;
        //        _oFabricBatchProduction.Texture = _oFabricBatchProductions[0].Texture;
        //        _oFabricBatchProduction.ShiftID = _oFabricBatchProductions[0].ShiftID;

        //        FabricMachine oFabricMachine = new FabricMachine();
        //        oFabricMachine = oFabricMachine.Get(_oFabricBatchProduction.FMID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        _oFabricBatchProduction.FabricMachines.Add(oFabricMachine);

        //        if (_oFabricBatchProduction.EndTime < new DateTime(1900, 01, 01, 1, 1, 1))
        //        {
        //            _oFabricBatchProduction.EndTime = new DateTime(1900, 01, 01, 1, 1, 1);
        //        }

        //        if (_oFabricBatchProduction.EndTime != new DateTime(1900, 01, 01, 1, 1, 1))
        //        {
        //            _oFabricBatchProduction.IsRunOut = true;
        //        }
        //        else
        //        {
        //            _oFabricBatchProduction.IsRunOut = false;
        //        }
        //        oFBPBMs = FabricBatchProductionBatchMan.Gets(_oFabricBatchProduction.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    #endregion


        //    #region Get Fabric Batch
        //    if (nFBID > 0)
        //    {
        //        oFB = oFB.Get(nFBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        if (_oFabricBatchProductions.Count == 0 && oFB.FabricSalesContractDetailID > 0)
        //        {
        //            FabricSalesContractDetail oTempFabricSalesContractDetail = new FabricSalesContractDetail();
        //            //oTempFabricSalesContractDetail = FabricSalesContractDetail.Get(oFB.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //            oFB.Texture = oTempFabricSalesContractDetail.FabricWeaveName;
        //        }
        //    }
        //    #endregion


        //    #region FabricBatchProductionBeam
        //    List<FabricBatchProductionBeam> oFBPBs = new List<FabricBatchProductionBeam>();
        //    FabricBatchProductionBeam oFBPB = new FabricBatchProductionBeam();

        //    if (nBeamID > 0 && nFBID > 0 && nFabricSalesContractDetailID > 0)
        //    {
        //        sSQL = "SELECT top(1)* FROM View_FabricBatchProductionBeam FBPB WHERE FBPB.FabricSalesContractDetailID=" + nFabricSalesContractDetailID + " AND FBPB.FBID=" + nFBID + " AND FBPB.IsFinish = 1 AND FBPB.BeamID=" + nBeamID + " Order By FBPBeamID DESC";
        //        oFBPBs = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        if (oFBPBs.Count > 0)
        //        {
        //            oFBPB.FBPBeamID = oFBPBs[0].FBPBeamID;
        //            oFBPB.FBPID = oFBPBs[0].FBPID;
        //            oFBPB.BeamID = oFBPBs[0].BeamID;
        //            oFBPB.Qty = oFBPBs[0].Qty;
        //            oFBPB.IsFinish = oFBPBs[0].IsFinish;
        //            oFBPB.FBID = oFBPBs[0].FBID;
        //            oFBPB.WeavingProcessType = oFBPBs[0].WeavingProcessType;
        //            oFBPB.BeamName = oFBPBs[0].BeamName;
        //            oFBPB.BeamCode = oFBPBs[0].BeamCode;
        //            oFBPB.FabricSalesContractDetailID = oFBPBs[0].FabricSalesContractDetailID;
        //            oFBPB.IsInHouse = oFBPBs[0].IsInHouse;
        //            oFBPB.OrderType = oFBPBs[0].OrderType;
        //            oFBPB.FEONo = oFBPBs[0].FEONo;
        //            oFBPB.Construction = oFBPBs[0].Construction;
        //            oFBPB.Weave = oFBPBs[0].Weave;
        //            oFBPB.BeamNo = oFBPBs[0].BeamNo;
        //            oFBPB.BuyerID = oFBPBs[0].BuyerID;
        //            oFBPB.BuyerName = oFBPBs[0].BuyerName;
        //            oFBPB.TotalEnds = oFBPBs[0].TotalEnds;
        //            oFBPB.ReedCount = oFBPBs[0].ReedCount;
        //            oFBPB.MachineStatus = oFBPBs[0].MachineStatus;
        //            oFBPB.FBStatus = oFBPBs[0].FBStatus;
        //            oFBPB.StartTime = oFBPBs[0].StartTime;
        //            oFBPB.EndTime = oFBPBs[0].EndTime;
        //            oFBPB.BatchQty = oFBPBs[0].BatchQty;
        //            oFBPB.FEOQty = oFBPBs[0].FEOQty;
        //            oFBPB.HoldQty = oFBPBs[0].TotalHoldQty;
        //        }
        //    }
        //    #endregion



        //    ViewBag.Beams = FabricMachine.Gets(true, EnumWeavingProcess.Loom, EnumMachineStatus.Free, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    ViewBag.BtnIDHtml = sBtnID;
        //    ViewBag.FabricBatch = oFB;
        //    ViewBag.BeamID = nBeamID;
        //    ViewBag.FBPB = oFBPB;
        //    ViewBag.FBPBMs = oFBPBMs;
        //    ViewBag.BackBtnControllerName = "FabricBatchProduction";
        //    ViewBag.BackBtnActionName = "View_ReadyBeamsForWeaving";
        //    //ViewBag.Stores = WorkingUnit.Gets("FabricBatch", (int)EnumTriggerParentsType._FabricProduction, (int)EnumOperationFunctionality.Disburse, (int)EnumInOutType.Disburse, false, 0, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    ViewBag.FabricBatchRawMaterials = FabricBatchRawMaterial.Gets(nFBID, EnumWeavingProcess.Loom, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Rowmaterial
        //   // ViewBag.Stores = WorkingUnit.Gets("FabricBatch", (int)EnumTriggerParentsType._FabricProduction, (int)EnumOperationFunctionality._Disburse, (int)EnumInOutType.Disburse, false, 0, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    ViewBag.Stores = WorkingUnit.GetsPermittedStore(nBUID, EnumModuleName.FabricBatch, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
        //    sSQL = "Select * from View_TextileSubUnit Where TextileUnit = "+(int)EnumTextileUnit.Weaving+" And ISNULL(InactiveBy,0)=0";
        //    List<TextileSubUnit> oTSUs = new List<TextileSubUnit>();
        //    oTSUs = TextileSubUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    ViewBag.TSUs = oTSUs;

        //    return View(_oFabricBatchProduction);
        //}


        //#endregion

        #region Beam Ref

        [HttpPost]
        public JsonResult GetLastBeamRef(FabricBatchProductionBeam oFBPB)
        {
            try
            {
                string sSQL = "Select top(1)* from View_FabricBatchProductionBeam Where BeamID=" + oFBPB.BeamID + "  Order By FBPBeamID DESC";
                var oFBPBs = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFBPBs.Any() && oFBPBs.First().FBPBeamID > 0)
                {
                    oFBPB = oFBPBs.First();
                }
            }
            catch (Exception ex)
            {
                oFBPB = new FabricBatchProductionBeam();
                oFBPB.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFBPB);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Ready beams for weaving
        public ActionResult View_ReadyBeamsForWeaving(int menuid, string res)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<FabricBatchProductionBeam> oFBPBs = new List<FabricBatchProductionBeam>();
            oFBPBs = GetsFabricBatchProductionBeam(new FabricBatchProductionBeam());
            ViewBag.MenuID = menuid;
            ViewBag.Message = res ?? "";

            string sSQL = "Select * from View_TextileSubUnit Where ISNULL(InactiveBy,0)=0";
            List<TextileSubUnit> oTextileSubUnits = new List<TextileSubUnit>();
            oTextileSubUnits = TextileSubUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.TextileSubUnits = oTextileSubUnits;
            ViewBag.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);


            return View(oFBPBs);
        }
        private List<FabricBatchProductionBeam> GetsFabricBatchProductionBeamForExcel(FabricBatchProductionBeam oFabricBatchProductionBeam)
        {

            List<FabricBatchProductionBeam> oFBPBs = new List<FabricBatchProductionBeam>();

            string sQuery = "";
            #region FEO No wise
            if (!string.IsNullOrEmpty(oFabricBatchProductionBeam.FEONo))
                sQuery = sQuery + " AND FEONo LIKE '%" + oFabricBatchProductionBeam.FEONo + "%' ";
            #endregion

            #region Buyer ID
            if (!string.IsNullOrEmpty(oFabricBatchProductionBeam.BuyerName)) // BuyerName used for carrying BuyerIDs
                sQuery = sQuery + " AND BuyerID IN (" + oFabricBatchProductionBeam.BuyerName + ") ";
            #endregion

            #region Beam No
            if (!string.IsNullOrEmpty(oFabricBatchProductionBeam.BeamNo))
                sQuery = sQuery + " AND BeamNo LIKE '%" + oFabricBatchProductionBeam.BeamNo + "%' ";
            #endregion

            #region Beam No
            if (!string.IsNullOrEmpty(oFabricBatchProductionBeam.MachineCode))
                sQuery = sQuery + " AND MachineCode LIKE '%" + oFabricBatchProductionBeam.MachineCode + "%' ";
            #endregion
            //#region textile subunit
            //if (oFabricBatchProductionBeam.TSUID>0)
            //    sQuery = sQuery + " AND TSUID =" + oFabricBatchProductionBeam.TSUID ;
            //#endregion


            string sSQL = "SELECT * FROM (SELECT * FROM View_FabricBatchProductionBeam ReadyLBeam WHERE  ReadyLBeam.FBStatus != 14 "//AND ReadyLBeam.MachineStatus= 1
                           + sQuery
                           + " AND ReadyLBeam.IsFinish=1 AND ReadyLBeam.WeavingProcessType=1 AND ReadyLBeam.BeamID NOT IN ("
                           + " SELECT LBeam.BeamID FROM View_FabricBatchProductionBeam LBeam WHERE LBeam.WeavingProcessType=3 AND LBeam.FBID=ReadyLBeam.FBID)"
                           + " UNION"
                           + " SELECT * FROM View_FabricBatchProductionBeam RunningBeam WHERE  RunningBeam.FBStatus != 14 "//AND  RunningBeam.MachineStatus= 1
                           + sQuery
                           + " AND RunningBeam.WeavingProcessType=3 AND RunningBeam.MachineStatus=" + (int)EnumMachineStatus.Running + " And RunningBeam.FBPID IN ("
                           + " SELECT FBPID FROM FabricBatchProduction WHERE EndTime IS NULL AND WeavingProcess=3)) AS F ORDER BY CONVERT(INT,dbo.fnGetNumberFromString(F.MachineCode)) ASC";

            oFBPBs = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);



            #region Beam Hold


            sSQL = "SELECT * FROM View_FabricBatchProductionBeam HoldBeam WHERE HoldBeam.FBStatus != 14  AND  EndTime IS NOT NULL AND WeavingProcess=" + (int)EnumWeavingProcess.Loom + " AND"
                  + " FBPBeamID In (Select MAX(FBPBeamID) from FabricBatchProductionBeam Where  BeamID IN (Select FMID from FabricMachine Where MachineStatus=" + (int)EnumMachineStatus.Hold + " AND IsBeam=1) Group By BeamID) ";
            List<FabricBatchProductionBeam> oBeamHolds = new List<FabricBatchProductionBeam>();
            oBeamHolds = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFBPBs.AddRange(oBeamHolds);
            #endregion

            if (oFabricBatchProductionBeam.Status == 1) //Loom Running
            {
                //oFBPBs = oFBPBs.Where(x => x.StatusRBFW == "Loom Running").ToList();
                oFBPBs = oFBPBs.Where(x => x.WeavingProcessType == EnumWeavingProcess.Loom && x.EndTime == DateTime.MinValue).ToList();
            }
            //oFBPBs = oFBPBs.Where(x => x.StatusRBFW != "-").ToList();
            else if (oFabricBatchProductionBeam.Status == 2) //Ready For Loom
            {
                //oFBPBs = oFBPBs.Where(x => x.StatusRBFW == "Ready For Loom").ToList();
                oFBPBs = oFBPBs.Where(x => x.WeavingProcessType == EnumWeavingProcess.Sizing).ToList();//Ready for loom (Sizing beam finish only)
            }
            else if (oFabricBatchProductionBeam.Status == 3) //Beam Hold
            {
                //oFBPBs = oFBPBs.Where(x => x.StatusRBFW == "Loom Running").ToList();
                oFBPBs = oFBPBs.Where(x => x.WeavingProcessType == EnumWeavingProcess.Loom && x.EndTime != DateTime.MinValue).ToList();
            }
            return oFBPBs;
        }
        private List<FabricBatchProductionBeam> GetsFabricBatchProductionBeam(FabricBatchProductionBeam oFabricBatchProductionBeam)
        {

            List<FabricBatchProductionBeam> oFBPBs = new List<FabricBatchProductionBeam>();

            string sQuery = "";
            #region FEO No wise
            if (!string.IsNullOrEmpty(oFabricBatchProductionBeam.FEONo))
                sQuery = sQuery + " AND FEONo LIKE '%" + oFabricBatchProductionBeam.FEONo + "%' ";
            #endregion

            #region Buyer ID
            if (!string.IsNullOrEmpty(oFabricBatchProductionBeam.BuyerName)) // BuyerName used for carrying BuyerIDs
                sQuery = sQuery + " AND BuyerID IN (" + oFabricBatchProductionBeam.BuyerName + ") ";
            #endregion

            #region Beam No
            if (!string.IsNullOrEmpty(oFabricBatchProductionBeam.BeamNo))
                sQuery = sQuery + " AND BeamNo ='" + oFabricBatchProductionBeam.BeamNo + "' ";
            #endregion

            #region Beam No
            if (!string.IsNullOrEmpty(oFabricBatchProductionBeam.MachineCode))
                //sQuery = sQuery + " AND MachineCode LIKE '%" + oFabricBatchProductionBeam.MachineCode + "%' ";
                sQuery = sQuery + " AND MachineCode ='" + oFabricBatchProductionBeam.MachineCode + "' ";
            #endregion
            //#region textile subunit
            //if (oFabricBatchProductionBeam.TSUID > 0)
            //    sQuery = sQuery + " AND TSUID =" + oFabricBatchProductionBeam.TSUID;
            //#endregion


            string sSQL = "SELECT * FROM (SELECT * FROM View_FabricBatchProductionBeam ReadyLBeam WHERE  ReadyLBeam.FBStatus != 14 "//AND ReadyLBeam.MachineStatus= 1
                           + sQuery
                           + " AND ReadyLBeam.IsFinish=1 AND ReadyLBeam.WeavingProcessType=1 AND ReadyLBeam.BeamID NOT IN ("
                           + " SELECT LBeam.BeamID FROM View_FabricBatchProductionBeam LBeam WHERE LBeam.WeavingProcessType=3 AND LBeam.FBID=ReadyLBeam.FBID)"
                           + " UNION"
                           + " SELECT * FROM View_FabricBatchProductionBeam RunningBeam WHERE  RunningBeam.FBStatus != 14 "//AND  RunningBeam.MachineStatus= 1
                           + sQuery
                           + " AND RunningBeam.WeavingProcessType=3 AND RunningBeam.MachineStatus=" + (int)EnumMachineStatus.Running + " And RunningBeam.FBPID IN ("
                           + " SELECT FBPID FROM FabricBatchProduction WHERE EndTime IS NULL AND WeavingProcess=3)) AS F ORDER BY CONVERT(INT,dbo.fnGetNumberFromString(F.MachineCode)) ASC";

            oFBPBs = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);



            #region Beam Hold
            if (string.IsNullOrEmpty(oFabricBatchProductionBeam.MachineCode))
            {
                sQuery = "";
                #region FEO No wise
                if (!string.IsNullOrEmpty(oFabricBatchProductionBeam.FEONo))
                    sQuery = sQuery + " AND FEONo LIKE '%" + oFabricBatchProductionBeam.FEONo + "%' ";
                #endregion

                #region Beam No
                if (!string.IsNullOrEmpty(oFabricBatchProductionBeam.BeamNo))
                    sQuery = sQuery + " AND BeamNo ='" + oFabricBatchProductionBeam.BeamNo + "' ";
                #endregion

                sSQL = "SELECT * FROM View_FabricBatchProductionBeam HoldBeam WHERE HoldBeam.FBStatus != 14  AND  EndTime IS NOT NULL AND WeavingProcess=" + (int)EnumWeavingProcess.Loom + " AND"
                  + " FBPBeamID In (Select MAX(FBPBeamID) from FabricBatchProductionBeam Where  BeamID IN (Select FMID from FabricMachine Where MachineStatus=" + (int)EnumMachineStatus.Hold + " AND IsBeam=1) Group By BeamID) " + sQuery;
                List<FabricBatchProductionBeam> oBeamHolds = new List<FabricBatchProductionBeam>();
                oBeamHolds = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFBPBs.AddRange(oBeamHolds);
            }
            #endregion

            if (oFabricBatchProductionBeam.Status == 1) //Loom Running
            {

                oFBPBs = oFBPBs.Where(x => x.WeavingProcessType == EnumWeavingProcess.Loom && x.EndTime == DateTime.MinValue).ToList();
            }

            else if (oFabricBatchProductionBeam.Status == 2) //Ready For Loom
            {

                oFBPBs = oFBPBs.Where(x => x.WeavingProcessType == EnumWeavingProcess.Sizing).ToList();//Ready for loom (Sizing beam finish only)
            }
            else if (oFabricBatchProductionBeam.Status == 3) //Beam Hold
            {

                oFBPBs = oFBPBs.Where(x => x.WeavingProcessType == EnumWeavingProcess.Loom && x.EndTime != DateTime.MinValue).ToList();
            }
            oFBPBs = oFBPBs.OrderBy(x => x.OrderNo).ToList();

            return oFBPBs;
        }



        [HttpPost]
        public JsonResult SearchFBPB(FabricBatchProductionBeam oFabricBatchProductionBeam)
        {
            List<FabricBatchProductionBeam> oFBPBs = new List<FabricBatchProductionBeam>();
            try
            {
                oFBPBs = this.GetsFabricBatchProductionBeam(oFabricBatchProductionBeam);
            }
            catch (Exception ex)
            {
                FabricBatchProductionBeam oFBPB = new FabricBatchProductionBeam();
                oFBPB.ErrorMessage = ex.Message;
                oFBPBs.Add(oFBPB);
            }
            var jsonResult = Json(oFBPBs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SearchByDispoNO(FabricBatchProduction oFabricBatchProduction)
        {
            _oFabricBatchProductions = new List<FabricBatchProduction>();
            try
            {
                string sSQL = "SELECT * FROM View_FabricBatchProduction ";
                string sSReturn = "";
                if (oFabricBatchProduction.FEONo != "" && oFabricBatchProduction.FEONo != null)
                {
                    Global.TagSQL(ref sSReturn);
                    sSReturn += " FEONo LIKE '%" + oFabricBatchProduction.FEONo + "%'";
                }
                if (oFabricBatchProduction.BatchNo != "" && oFabricBatchProduction.BatchNo != null)
                {
                    Global.TagSQL(ref sSReturn);
                    sSReturn += " BatchNo LIKE '%" + oFabricBatchProduction.BatchNo + "%'";
                }
                sSQL += sSReturn;
                _oFabricBatchProductions = FabricBatchProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
                _oFabricBatchProductions.Add(_oFabricBatchProduction);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchProductions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchFPB(FabricBatchProduction oFabricBatchProduction)
        {
            _oFabricBatchProductions = new List<FabricBatchProduction>();
            try
            {
                string sFEONo = Convert.ToString(oFabricBatchProduction.Params.Split('~')[0]);
                string sBatchNo = Convert.ToString(oFabricBatchProduction.Params.Split('~')[1]);
                int nDateType = Convert.ToInt16(oFabricBatchProduction.Params.Split('~')[2]);
                DateTime dIssueStartDate = Convert.ToDateTime(oFabricBatchProduction.Params.Split('~')[3]);
                DateTime dIssueEndDate = Convert.ToDateTime(oFabricBatchProduction.Params.Split('~')[4]);
                string sFPBStatus = oFabricBatchProduction.Params.Split('~')[5];
                string sMachines = oFabricBatchProduction.Params.Split('~')[6];
                string nWeavingProcess = oFabricBatchProduction.Params.Split('~')[7];

                string sSQL = "SELECT * FROM View_FabricBatchProduction WHERE FBPID>0";

                if (!string.IsNullOrEmpty(sFEONo))
                {
                    sSQL += " AND FEONo LIKE '%" + sFEONo + "%'";
                }
                if (!string.IsNullOrEmpty(sBatchNo))
                {
                    sSQL += " AND WMCode+BatchNo LIKE '%" + sBatchNo + "%'";
                }
                if (nDateType > 0)
                {
                    DateObject.CompareDateQuery(ref sSQL, "StartTime", nDateType, dIssueStartDate, dIssueEndDate);
                }
                if (!string.IsNullOrEmpty(sFPBStatus))
                //if (sFPBStatus != "-1")
                {
                    sSQL = sSQL + " AND FabricBatchStatus IN (" + sFPBStatus + ")";

                }
                if (!string.IsNullOrEmpty(nWeavingProcess))
                {
                    sSQL = sSQL + " AND WeavingProcess IN (" + nWeavingProcess + ")";
                }

                if (!string.IsNullOrEmpty(sMachines))
                {
                    sSQL = sSQL + " AND FMID IN (" + sMachines + ")";
                }
                sSQL = sSQL + " ORDER BY BatchNo";
                _oFabricBatchProductions = FabricBatchProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
                _oFabricBatchProductions.Add(_oFabricBatchProduction);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchProductions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Print
        [HttpPost]
        public ActionResult SetSessionSearchCriterias(FabricBatchProduction oFabricBatchProduction)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oFabricBatchProduction);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FPBatchByWeavingProcessPrint()
        {
            _oFabricBatchProduction = new FabricBatchProduction();
            _oFabricBatchProductions = new List<FabricBatchProduction>();
            _oFabricBatchProduction = (FabricBatchProduction)Session[SessionInfo.ParamObj];

            string sSQL = "SELECT * FROM View_FabricBatchProduction AS HH WHERE HH.FBPID IN (" + _oFabricBatchProduction.ErrorMessage + ") ORDER BY HH.FBPID ASC";
            _oFabricBatchProductions = FabricBatchProduction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            rptFabricBatchProductions oReport = new rptFabricBatchProductions();
            byte[] abytes = oReport.PrepareReport(_oFabricBatchProductions, oCompany, "");
            return File(abytes, "application/pdf");
        }

        public void FPBatchByWeavingProcessExcel()
        {
            _oFabricBatchProduction = new FabricBatchProduction();
            _oFabricBatchProductions = new List<FabricBatchProduction>();
            _oFabricBatchProduction = (FabricBatchProduction)Session[SessionInfo.ParamObj];

            string sSQL = "SELECT * FROM View_FabricBatchProduction AS HH WHERE HH.FBPID IN (" + _oFabricBatchProduction.ErrorMessage + ") ORDER BY HH.FBPID ASC";
            _oFabricBatchProductions = FabricBatchProduction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            try
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                if (_oFabricBatchProductions == null)
                {
                    throw new Exception("Invalid Searching Criteria!");
                }
                int rowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                int nTotalCol = 0;
                int nCount = 0;
                double AvgDz = 0;
                int colIndex = 2;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Fabric Batch Production");
                    sheet.Name = "Fabric Batch Production";
                    sheet.Column(colIndex++).Width = 10; //SL
                    sheet.Column(colIndex++).Width = 20; //Batch No
                    sheet.Column(colIndex++).Width = 20; //DispoNo
                    sheet.Column(colIndex++).Width = 20; //Machine Name
                    sheet.Column(colIndex++).Width = 20; //Weaving Process
                    sheet.Column(colIndex++).Width = 25; //Status
                    sheet.Column(colIndex++).Width = 20; //Construnction
                    sheet.Column(colIndex++).Width = 20; //REED No
                    sheet.Column(colIndex++).Width = 35; //Reed width
                    sheet.Column(colIndex++).Width = 20; //start time
                    sheet.Column(colIndex++).Width = 20; //endtime
                    sheet.Column(colIndex++).Width = 30; //buyer name
                    sheet.Column(colIndex++).Width = 20; //Qty(y)
                    sheet.Column(colIndex++).Width = 20; //Qty(m)

                    #region Report Header
                    sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Fabric Batch Production"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Header 1
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Batch No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Dispo No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Machine Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Weaving Process"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Status"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Construction"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "REED No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "REED Width"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Start Time"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "End Time"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty(Y)"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty(M)"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                    #endregion

                    #region Report Body
                    int nSL = 1;
                    colIndex = 2;
                    var fabricGroupProductionGroups = _oFabricBatchProductions.GroupBy(x => new { x.WeavingProcess })
                                             .OrderBy(x => x.Key.WeavingProcess)
                                             .Select(x => new
                                             {
                                                 weavingPricessName = x.Key.WeavingProcess,
                                                 _FabricBatchProductionsList = x.OrderBy(c => c.FBPID),
                                                 SubTotalQtyInYard = x.Sum(y => y.QtySt),
                                                 SubTotalQtyInMeter = x.Sum(y => y.QtyInMeterSt)
                                             });
                    foreach (var oData in fabricGroupProductionGroups)
                    {
                        nSL = 1;
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 15]; cell.Merge = true; cell.Value = "Weaving Process: " + oData.weavingPricessName; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        rowIndex++;
                        foreach (var oItem in oData._FabricBatchProductionsList)
                        {
                            colIndex = 2;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BatchNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FEONo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FabricMachineName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.WeavingProcessST; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FabricBatchStatusST; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Construction; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StartTimeSt; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EndTimeSt; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BuyerName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtySt; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyInMeterSt; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            nSL++;
                            rowIndex++;
                        }

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 13]; cell.Merge = true; cell.Value = "Total:"; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 14]; cell.Value = oData.SubTotalQtyInYard; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 15]; cell.Value = oData.SubTotalQtyInYard; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rowIndex++;

                    }

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=RPT_FabricProductionBatch.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();

                }

            }
                    #endregion
            catch (Exception ex)
            {
                _oFabricBatchProductions = new List<FabricBatchProduction>();
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
                _oFabricBatchProductions.Add(_oFabricBatchProduction);
            }

        }
        #endregion



        private List<FabricBatchProductionBeam> GetsValidList(List<FabricBatchProductionBeam> oFBPBs)
        {
            oFBPBs = oFBPBs.Where(x => x.StatusRBFW != "-").ToList();

            return oFBPBs;

        }

        private List<FabricBatchProductionBeam> GetsReadyBeamsForWeaving(List<FabricBatchProductionBeam> oParam_FBPBs)
        {
            List<FabricBatchProductionBeam> oFBPBs = new List<FabricBatchProductionBeam>();
            if (oParam_FBPBs.Count > 0)
            {
                FabricBatchProductionBeam oFBPB = new FabricBatchProductionBeam();
                List<FabricBatchProductionBeam> oTempFBPBs = new List<FabricBatchProductionBeam>();


                string[] splitBeamIDs = string.Join(",", oParam_FBPBs.Select(o => o.BeamID).Distinct()).Split(',');
                string[] splitFabricSalesContractDetailIDs = string.Join(",", oParam_FBPBs.Select(o => o.FabricSalesContractDetailID).Distinct()).Split(',');


                foreach (string sBeamID in splitBeamIDs)
                {
                    int nBeamID = Convert.ToInt32(sBeamID);

                    foreach (string sFabricSalesContractDetailID in splitFabricSalesContractDetailIDs)
                    {
                        oFBPB = new FabricBatchProductionBeam();
                        oTempFBPBs = new List<FabricBatchProductionBeam>();

                        oTempFBPBs = oParam_FBPBs.Where(o => o.BeamID == nBeamID && o.FabricSalesContractDetailID == Convert.ToInt32(sFabricSalesContractDetailID) &&
                                                      o.WeavingProcessType == EnumWeavingProcess.Loom &&
                                                      o.MachineStatus == EnumMachineStatus.Running).ToList();
                        if (oTempFBPBs.Count > 0)
                        {
                            oFBPBs.Add(oTempFBPBs[0]);
                        }
                        else
                        {
                            oTempFBPBs = oParam_FBPBs.Where(o => o.BeamID == nBeamID && o.FabricSalesContractDetailID == Convert.ToInt32(sFabricSalesContractDetailID) &&
                                                            o.IsFinish == true).ToList();
                            if (oTempFBPBs.Count > 0)
                            {
                                oFBPBs.Add(oTempFBPBs[0]);
                            }
                        }
                    }
                }

            }
            return oFBPBs;
        }

        [HttpPost]
        public ActionResult PrintFBPB(FormCollection DataCollection)
        {
            List<FabricBatchProductionBeam> oFBPBs = new List<FabricBatchProductionBeam>();
            string sFBPBeamIDs = DataCollection["txtFBPBList"];

            if (!string.IsNullOrEmpty(sFBPBeamIDs))
            {
                string sSQL = "SELECT * FROM View_FabricBatchProductionBeam WHERE FBPBeamID IN (" + sFBPBeamIDs + ")  ORDER  BY FabricSalesContractDetailID ";
                oFBPBs = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            oFBPBs = this.GetsValidList(oFBPBs);

            string Messge = "Ready beams for weaving";
            rptFabricBatchProductionBeam oReport = new rptFabricBatchProductionBeam();
            byte[] abytes = oReport.PrepareReport(oFBPBs, oCompany, Messge);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region Ready Beams For Weaving Production Upload Or Delete
        public ActionResult ViewReadyBeamsForWeavingApprove(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<FabricBatchProductionBatchMan> oFBPBs = new List<FabricBatchProductionBatchMan>();

            string sSQL = "Select * from View_TextileSubUnit Where ISNULL(InactiveBy,0)=0";
            List<TextileSubUnit> oTextileSubUnits = new List<TextileSubUnit>();
            oTextileSubUnits = TextileSubUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.TextileSubUnits = oTextileSubUnits;
            ViewBag.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);


            return View(oFBPBs);
        }
        [HttpPost]
        public JsonResult SearchFBPBM(FabricBatchProductionBatchMan oFBPBatchMan)
        {
            List<FabricBatchProductionBatchMan> oFabricBatchProductionBatchMans = new List<FabricBatchProductionBatchMan>();
            try
            {
                string sParam = oFBPBatchMan.ErrorMessage;
                if (string.IsNullOrEmpty(sParam))
                    throw new Exception("Search parameter required");


                DateTime dDate = Convert.ToDateTime(sParam.Split('~')[0]);
                int nTSUID = Convert.ToInt32(sParam.Split('~')[1]);
                int nShiftID = Convert.ToInt32(sParam.Split('~')[2]);
                DateTime dToDate = Convert.ToDateTime(sParam.Split('~')[3]);
                string sBuyerIDs = sParam.Split('~')[4];
                string sFabricSalesContractDetailIDs = sParam.Split('~')[5];
                string sMC = sParam.Split('~')[6];
                string sBeamNo = sParam.Split('~')[7];

                string subQuery = string.Empty;
                string subQuery1 = string.Empty;

                if (nTSUID > 0)
                    subQuery += " AND TSUID = " + nTSUID;

                if (!string.IsNullOrEmpty(sFabricSalesContractDetailIDs))
                    subQuery1 += " AND FabricSalesContractDetailID IN ( " + sFabricSalesContractDetailIDs + ")";
                if (!string.IsNullOrEmpty(sBuyerIDs))
                    subQuery1 += " AND FabricSalesContractDetailID IN (SELECT FabricSalesContractDetailID FROM FabricSalesContractDetail WHERE BuyerID IN (  " + sBuyerIDs + "))";

                string sSQL = "SELECT * FROM View_FabricBatchProductionBatchMan WHERE FBPID IN (SELECT FBPID FROM FabricBatchProduction WHERE WeavingProcess=3 " + subQuery + ") AND FinishDate BETWEEN '" + dDate.ToString("dd MMM yyyy") + "' AND '" + dToDate.ToString("dd MMM yyyy") + "'";
                if (nShiftID > 0)
                    sSQL += " AND ShiftID = " + nShiftID;
                if (!string.IsNullOrEmpty(sMC))
                    sSQL += " AND MachineCode = '" + sMC + "'";
                if (!string.IsNullOrEmpty(sBeamNo))
                    sSQL += " AND BeamNo = '" + sBeamNo + "'";
                sSQL += " Order By FBPBID";
                oFabricBatchProductionBatchMans = FabricBatchProductionBatchMan.GetsBySql(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oFBPBatchMan = new FabricBatchProductionBatchMan();
                _oFBPBatchMan.ErrorMessage = ex.Message;
                oFabricBatchProductionBatchMans.Add(_oFBPBatchMan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchProductionBatchMans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSqlForFBPB(string sParam)
        {

            DateTime dDate = Convert.ToDateTime(sParam.Split('~')[0]);
            int nTSUID = Convert.ToInt32(sParam.Split('~')[1]);
            int nShiftID = Convert.ToInt32(sParam.Split('~')[2]);
            DateTime dToDate = Convert.ToDateTime(sParam.Split('~')[3]);
            string sBuyerIDs = sParam.Split('~')[4];
            string sFabricSalesContractDetailIDs = sParam.Split('~')[5];


            string subQuery = string.Empty;
            string subQuery1 = string.Empty;

            if (nTSUID > 0)
                subQuery += " AND TSUID = " + nTSUID;
            //if (!string.IsNullOrEmpty(sFMIDs))
            //    subQuery += " AND FMID IN ( " + sFMIDs + ")";
            if (!string.IsNullOrEmpty(sFabricSalesContractDetailIDs))
                subQuery1 += " AND FabricSalesContractDetailID IN ( " + sFabricSalesContractDetailIDs + ")";
            if (!string.IsNullOrEmpty(sBuyerIDs))
                subQuery1 += " AND FabricSalesContractDetailID IN (SELECT FabricSalesContractDetailID FROM FabricSalesContractDetail WHERE BuyerID IN (  " + sBuyerIDs + "))";
            //if (nProcessType > 0)
            //    subQuery1 += " AND FabricSalesContractDetailID IN (SELECT FabricSalesContractDetailID FROM FabricSalesContractDetail WHERE ProcessType IN (  " + nProcessType + "))";
            //if (nWeaveType > 0)
            //    subQuery1 += " AND FabricSalesContractDetailID IN (SELECT FabricSalesContractDetailID FROM FabricSalesContractDetail WHERE FabricWeave IN (  " + nWeaveType + "))";
            //if (!string.IsNullOrEmpty(sConstruction))
            //    subQuery1 += " AND FabricSalesContractDetailID IN (SELECT FabricSalesContractDetailID FROM View_FabricSalesContractDetail WHERE Construction ='" + sConstruction + "')";


            //subQuery += "AND FBID IN (SELECT FBID FROM FabricBatch WHERE FBID<>0" + subQuery1 + ")";
            string sSQL = "SELECT * FROM View_FabricBatchProductionBatchMan WHERE FBPID IN (SELECT FBPID FROM FabricBatchProduction WHERE WeavingProcess=3 " + subQuery + ") AND FinishDate BETWEEN '" + dDate.ToString("dd MMM yyyy") + "' AND '" + dToDate.ToString("dd MMM yyyy") + "'";
            if (nShiftID > 0)
                sSQL += " AND ShiftID = " + nShiftID;
            sSQL += " Order By FBPBID";
            return sSQL;
        }
        [HttpPost]
        public JsonResult ApproveFBPB(FabricBatchProductionBatchMan oFBPBatchMan)
        {
            List<FabricBatchProductionBatchMan> oFabricBatchProductionBatchMans = new List<FabricBatchProductionBatchMan>();
            try
            {
                string Params = oFBPBatchMan.ErrorMessage;
                if (!string.IsNullOrEmpty(oFBPBatchMan.FBPBIDs))
                {

                    string res = oFBPBatchMan.MultipleApprove(oFBPBatchMan.FBPBIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (res == "Success")
                    {
                        oFabricBatchProductionBatchMans = FabricBatchProductionBatchMan.GetsBySql(GetSqlForFBPB(Params), ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }

            }
            catch (Exception ex)
            {
                _oFBPBatchMan = new FabricBatchProductionBatchMan();
                _oFBPBatchMan.ErrorMessage = ex.Message;
                oFabricBatchProductionBatchMans.Add(_oFBPBatchMan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchProductionBatchMans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteFBPB(FabricBatchProductionBatchMan oFBPBatchMan)
        {
            List<FabricBatchProductionBatchMan> oFabricBatchProductionBatchMans = new List<FabricBatchProductionBatchMan>();
            try
            {
                string Params = oFBPBatchMan.ErrorMessage;
                if (!string.IsNullOrEmpty(oFBPBatchMan.FBPBIDs))
                {

                    string res = oFBPBatchMan.MultipleDelete(oFBPBatchMan.FBPBIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (res == Global.DeleteMessage)
                    {
                        oFabricBatchProductionBatchMans = FabricBatchProductionBatchMan.GetsBySql(GetSqlForFBPB(Params), ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }

            }
            catch (Exception ex)
            {
                _oFBPBatchMan = new FabricBatchProductionBatchMan();
                _oFBPBatchMan.ErrorMessage = ex.Message;
                oFabricBatchProductionBatchMans.Add(_oFBPBatchMan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchProductionBatchMans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region RTN

        [HttpPost]
        public JsonResult Save(FabricBatchProduction oFabricBatchProduction)
        {
            try
            {
                oFabricBatchProduction = oFabricBatchProduction.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricBatchProduction = new FabricBatchProduction();
                oFabricBatchProduction.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchProduction);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(FabricBatchProduction oFabricBatchProduction)
        {
            try
            {
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = oFabricBatchProduction.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchProduction);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult UpdateWeaving(FabricBatchProduction oFabricBatchProduction)
        //{
        //    try
        //    {
        //        oFabricBatchProduction = oFabricBatchProduction.UpdateWeaving(((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        oFabricBatchProduction = new FabricBatchProduction();
        //        oFabricBatchProduction.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oFabricBatchProduction);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}



        #endregion

        #region Fabric Batch Production BatchMan
        [HttpGet]
        public JsonResult DeleteFBPBatchMan(int id)//FabricBatchProductionbatchMan ID
        {
            string sFeedBackMessage = "";
            FabricBatchProductionBatchMan oFabricBatchProductionBatchMan = new FabricBatchProductionBatchMan();
            try
            {
                sFeedBackMessage = oFabricBatchProductionBatchMan.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult DeleteFabricBatchProductionDetail(FabricBatchProductionDetail oFBPDetail)
        {
            FabricBatchProductionDetail oFabricBatchProductionDetail = new FabricBatchProductionDetail();
            try
            {
                oFabricBatchProductionDetail.ErrorMessage = oFabricBatchProductionDetail.Delete(oFBPDetail.FBPDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricBatchProductionDetail.FabricBatchProductionBeams = FabricBatchProductionBeam.Gets("SELECT * FROM [View_FabricBatchProductionBeam] WHERE FBPID = " + oFBPDetail.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricBatchProductionDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchProductionDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFBPBeam(FabricBatchProductionBeam oFBPBeam)
        {
            FabricBatchProductionBeam oFabricBatchProductionBeam = new FabricBatchProductionBeam();
            try
            {
                oFabricBatchProductionBeam.ErrorMessage = oFabricBatchProductionBeam.Delete(oFBPBeam.FBPBeamID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricBatchProductionBeam.FabricBatchProductionBeams = FabricBatchProductionBeam.Gets("SELECT * FROM [View_FabricBatchProductionBeam] WHERE FBPID = " + oFBPBeam.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricBatchProductionBeam.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchProductionBeam);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveFBPBatchMan(FabricBatchProductionBatchMan oFBPBatchMan)
        {
            _oFBPBatchMan = new FabricBatchProductionBatchMan();
            try
            {
                _oFBPBatchMan = oFBPBatchMan.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFBPBatchMan = new FabricBatchProductionBatchMan();
                _oFBPBatchMan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFBPBatchMan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveDetailWithBeam(FabricBatchProductionDetail oFabricBatchProductionDetail)
        {
            FabricBatchProductionDetail _oFabricBatchProductionDetail = new FabricBatchProductionDetail();
            try
            {
                _oFabricBatchProductionDetail = oFabricBatchProductionDetail.SaveDetailWithBeam(((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricBatchProductionDetail.FabricBatchProductionBeams = FabricBatchProductionBeam.Gets("SELECT * FROM [View_FabricBatchProductionBeam] WHERE FBPID = " + _oFabricBatchProductionDetail.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatchProductionDetail = new FabricBatchProductionDetail();
                _oFabricBatchProductionDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchProductionDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DrawingLeasingOperation(List<FabricBatchProductionBeam> oFabricBatchProductionBeams)
        {
            FabricBatchProductionBeam oFabricBatchProductionBeam = new FabricBatchProductionBeam();
            List<FabricBatchProductionBeam> oFBPBs = new List<FabricBatchProductionBeam>();
            try
            {
                oFBPBs = oFabricBatchProductionBeam.DrawingLeasingOperation(oFabricBatchProductionBeams, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFBPBs = FabricBatchProductionBeam.Gets("SELECT * FROM View_FabricBatchProductionBeam WHERE FBPID in (Select FBPID from FabricBatchProduction where WeavingProcess=" + (int)EnumWeavingProcess.Sizing + " and FBID=" + oFBPBs[0].FBID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricBatchProductionBeam = new FabricBatchProductionBeam();
                oFabricBatchProductionBeam.ErrorMessage = ex.Message;
                oFBPBs.Add(oFabricBatchProductionBeam);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFBPBs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFBPDetail(FabricBatchProductionDetail oFabricBatchProductionDetail)
        {
            FabricBatchProductionDetail _oFabricBatchProductionDetail = new FabricBatchProductionDetail();
            try
            {
                _oFabricBatchProductionDetail = oFabricBatchProductionDetail.Get(oFabricBatchProductionDetail.FBPDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricBatchProductionDetail.FabricBatchProductionBeams = FabricBatchProductionBeam.Gets("SELECT * FROM [View_FabricBatchProductionBeam] WHERE FBPDetailID = " + _oFabricBatchProductionDetail.FBPDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatchProductionDetail = new FabricBatchProductionDetail();
                _oFabricBatchProductionDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchProductionDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Fabrci Batch Production Breakage
        [HttpGet]
        public JsonResult DeleteFBPBreakage(int id)//FabricBatchProductionBreakage ID
        {
            string sFeedBackMessage = "";
            FabricBatchProductionBreakage oFabricBatchProductionBreakage = new FabricBatchProductionBreakage();
            try
            {
                sFeedBackMessage = oFabricBatchProductionBreakage.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult SaveFBPBreakage(FabricBatchProductionBreakage oFBPBreakage)
        {
            _oFBPBreakage = new FabricBatchProductionBreakage();
            try
            {
                _oFBPBreakage = oFBPBreakage.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oFBPBreakage.FBLDetailID > 0)
                {
                    _oFBPBreakage.FabricBatchProductionBreakages = FabricBatchProductionBreakage.Gets(_oFBPBreakage.FBLDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oFBPBreakage = new FabricBatchProductionBreakage();
                _oFBPBreakage.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFBPBreakage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Fabrci Batch Production Color BreakDown
        [HttpGet]
        public JsonResult DeleteFBPColor(int id)//FabricBatchProductionColor ID
        {
            string sFeedBackMessage = "";
            FabricBatchProductionColor oFabricBatchProductionColor = new FabricBatchProductionColor();
            try
            {
                sFeedBackMessage = oFabricBatchProductionColor.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult SaveFBPColor(FabricBatchProductionColor oFBPColor)
        {
            _oFBPColor = new FabricBatchProductionColor();
            try
            {
                _oFBPColor = oFBPColor.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oFBPColor.FBPBID > 0)
                {
                    _oFBPColor.FabricBatchProductionColors = FabricBatchProductionColor.Gets(_oFBPColor.FBPBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (string.IsNullOrEmpty(_oFBPColor.ErrorMessage))
                    {
                        FabricBatchProductionBatchMan oFabricBatchProductionBatchMan = new FabricBatchProductionBatchMan();
                        oFabricBatchProductionBatchMan = oFabricBatchProductionBatchMan.Get(_oFBPColor.FBPBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oFabricBatchProductionBatchMan.Qty = _oFBPColor.FabricBatchProductionColors.Select(o => o.Qty).Sum();
                        oFabricBatchProductionBatchMan = oFabricBatchProductionBatchMan.Save(((User)Session[SessionInfo.CurrentUser]).UserID);

                        if (!string.IsNullOrEmpty(oFabricBatchProductionBatchMan.ErrorMessage))
                        {
                            _oFBPColor = new FabricBatchProductionColor();
                            _oFBPColor.ErrorMessage = oFabricBatchProductionBatchMan.ErrorMessage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _oFBPColor = new FabricBatchProductionColor();
                _oFBPColor.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFBPColor);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

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



        #region Excel Repoty ReadyBeamInStock


        private void ReadyBeamStock(List<FabricBatchProductionBeam> oRunningLooms, List<FabricBatchProductionBeam> oSummary, ref int nMaxColumn, ref int rowIndex, ref int colIndex, ref ExcelWorksheet sheet, ref ExcelRange cell, int ParamTTEnds)
        {
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            string sVal = "";
            int nHorizontalAlignment = 2;
            bool IsNumberField = false;

            foreach (var oItem in oSummary)
            {
                colIndex = 2;
                for (int i = 1; i < nMaxColumn; i++)
                {
                    IsNumberField = false;
                    nHorizontalAlignment = 2;
                    switch (i)
                    {
                        case 1: sVal = oItem.Construction; break;
                        case 2: sVal = oItem.ReedCountWithDent.ToString(); nHorizontalAlignment = 0; break;
                        case 3: sVal = oItem.Weave.ToString(); break;
                        case 4: sVal = oItem.TotalEnds.ToString(); nHorizontalAlignment = 0; break;
                        case 5: sVal = oItem.BuyerName; break;
                        case 6: sVal = oItem.OrderNo; break;
                        case 7: sVal = ""; break;
                        //case 8: sVal = oItem.NoOfColor.ToString(); nHorizontalAlignment = 1; break;
                        case 8: sVal = oItem.BeamNo; break;
                        case 9: sVal = oItem.BeamCode; nHorizontalAlignment = 1; break;
                        case 10: sVal = oItem.QtyInMtr.ToString(); IsNumberField = true; nHorizontalAlignment = 0; break;
                        //case 12: sVal = string.Join(",", oRunningLooms.Where(x =>  x.TotalEnds >= oItem.TotalEnds && x.TotalEnds <= (oItem.TotalEnds + 1000) && x.Weave == oItem.Weave && x.YetWeaveQtyInMtr < 200).Where(x => x.Weave.Trim() != "").Select(x => x.MachineCode).ToList()); break;
                        case 11: sVal = string.Join(",", oRunningLooms.Where(x => x.TotalEnds >= oItem.TotalEnds && x.TotalEnds <= (oItem.TotalEnds + 1000) && x.Weave == oItem.Weave && x.YetWeaveQtyInMtr < ParamTTEnds).Where(x => x.Weave.Trim() != "").Select(x => x.MachineCode).ToList()); break;
                        case 12: sVal = oItem.WarpColor; break;
                        case 13: sVal = oItem.WeftColor; break;
                        case 14: sVal = ""; break;
                        default: sVal = ""; break;
                    }
                    cell = sheet.Cells[rowIndex, colIndex++];
                    if (IsNumberField)
                    {
                        cell.Value = Convert.ToDouble(sVal);
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    }
                    else
                    {
                        cell.Value = sVal;
                    }
                    cell.Style.Font.Bold = false;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    if (nHorizontalAlignment == 0)
                    {
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    else if (nHorizontalAlignment == 1)
                    {
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    else if (nHorizontalAlignment == 2)
                    {
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                rowIndex++;
            }
            #region Total
            colIndex = 2;
            cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "Total"; cell.Style.Font.Bold = true;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            cell = sheet.Cells[rowIndex, 12]; cell.Value = oSummary.Sum(x => x.QtyInMtr); cell.Style.Font.Bold = true;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

            cell = sheet.Cells[rowIndex, 13, rowIndex, 16]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            rowIndex++;
            #endregion
        }

        //public void ExcelRptReadyBeamInStock(int tsuid, string qs)
        //{
        //    List<FabricBatchProductionBeam> oFBPBs = new List<FabricBatchProductionBeam>();
        //    FabricBatchProductionBeam oFabricBatchProductionBeam = new FabricBatchProductionBeam();
        //    oFabricBatchProductionBeam.Status = 1; //Ready For Loom
        //    string param = qs ?? "";
        //    int ParamTTEnds = 0;
        //    if (param.Split('~').Length == 5)
        //    {
        //        oFabricBatchProductionBeam.FEONo = param.Split('~')[0];
        //        oFabricBatchProductionBeam.BuyerName = param.Split('~')[1];
        //        oFabricBatchProductionBeam.BeamNo = param.Split('~')[2];
        //        oFabricBatchProductionBeam.MachineCode = param.Split('~')[3];
        //        ParamTTEnds =Convert.ToInt32(param.Split('~')[4]);
        //    }
        //    string sSQL = "SELECT * FROM View_FabricBatchProductionBeam ReadyLBeam WHERE  ReadyLBeam.FBStatus != 14 "//AND ReadyLBeam.MachineStatus= 1                   
        //           + " AND ReadyLBeam.IsFinish=1 AND ReadyLBeam.WeavingProcessType=1 AND ReadyLBeam.BeamID NOT IN ("
        //           + " SELECT LBeam.BeamID FROM View_FabricBatchProductionBeam LBeam WHERE LBeam.WeavingProcessType=3 AND LBeam.FBID=ReadyLBeam.FBID)";

        //    if (tsuid > 0)
        //        sSQL = sSQL + " And TSUID = " + tsuid + "";


        //    oFBPBs = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


        //    #region Hold Beams used as Stock
        //    sSQL = "SELECT * FROM View_FabricBatchProductionBeam HoldBeam WHERE HoldBeam.FBStatus != 14  AND  EndTime IS NOT NULL AND WeavingProcess=" + (int)EnumWeavingProcess.Loom + " AND"
        //         + " FBPBeamID In (Select MAX(FBPBeamID) from FabricBatchProductionBeam Where  BeamID IN (Select FMID from FabricMachine Where MachineStatus=" + (int)EnumMachineStatus.Hold + " AND IsBeam=1) Group By BeamID) ";

        //    List<FabricBatchProductionBeam> oBeamHolds = new List<FabricBatchProductionBeam>();
        //    oBeamHolds = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oBeamHolds.ForEach(x => x.Qty -= x.TotalHoldQty);
        //    oFBPBs.AddRange(oBeamHolds.Where(x=>x.Qty>0).ToList());
        //    #endregion


        //    List<FabricBatchProductionBeam> oRunningLooms = new List<FabricBatchProductionBeam>();
        //    sSQL = " SELECT * FROM View_FabricBatchProductionBeam RunningBeam WHERE  RunningBeam.FBStatus != 14 "//AND  RunningBeam.MachineStatus= 1
        //         + " AND RunningBeam.WeavingProcessType=3 AND RunningBeam.FBPID IN (SELECT FBPID FROM FabricBatchProduction WHERE EndTime IS NULL AND WeavingProcess=3)";
        //    if (tsuid > 0)
        //        sSQL = sSQL + " And TSUID = " + tsuid + "";

        //    oRunningLooms = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    int nMaxColumn = 0;
        //    int colIndex = 2;
        //    int rowIndex = 2;
        //    ExcelRange cell;
        //    ExcelFill fill;
        //    OfficeOpenXml.Style.Border border;

        //    using (var excelPackage = new ExcelPackage())
        //    {
        //        excelPackage.Workbook.Properties.Author = "ESimSol";
        //        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //        var sheet = excelPackage.Workbook.Worksheets.Add("Daily Beam Stock Report");
        //        sheet.Name = "Daily Beam Stock Report";

        //        int nColumnWidth = 18;
        //        sheet.Column(2).Width = nColumnWidth; //Construction 
        //        sheet.Column(3).Width = nColumnWidth; //Reed Count
        //        sheet.Column(4).Width = nColumnWidth; //Weave
        //        sheet.Column(5).Width = nColumnWidth; //Total ends
        //        sheet.Column(6).Width = nColumnWidth; //Buyer
        //        sheet.Column(7).Width = nColumnWidth; //Order no
        //        sheet.Column(8).Width = nColumnWidth; //Option
        //        //sheet.Column(9).Width = nColumnWidth; //Weft color
        //        sheet.Column(9).Width = nColumnWidth; //Beam Stock No
        //        sheet.Column(10).Width = nColumnWidth; //No of Beam
        //        sheet.Column(11).Width = nColumnWidth; //Beam Stock Qty
        //        sheet.Column(12).Width = nColumnWidth; //Propose Loom no
        //        sheet.Column(13).Width = nColumnWidth; //Warp Color
        //        sheet.Column(14).Width = nColumnWidth; //Weft Color
        //        sheet.Column(15).Width = nColumnWidth; //Remarks
        //        nMaxColumn = 16;

        //        Company oCompany = new Company();
        //        oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //        #region Report Header
        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Beam Stock Report"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = DateTime.Now.ToString("dd MMM yyyy hh:mm tt"); cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 2;


        //        #endregion

        //        #region Table Header
        //        colIndex = 2;
        //        string sVal = "";
        //        for (int i = 1; i < nMaxColumn; i++)
        //        {
        //            switch (i)
        //            {
        //                case 1: sVal = "Construction"; break;
        //                case 2: sVal = "Reed Count"; break;
        //                case 3: sVal = "Weave"; break;
        //                case 4: sVal = "Total ends"; break;
        //                case 5: sVal = "Buyer"; break;
        //                case 6: sVal = "Order no"; break;
        //                case 7: sVal = "Option"; break;
        //                //case 8: sVal = "Weft color"; break;
        //                case 8: sVal = "Stock Beam No"; break;
        //                case 9: sVal = "No of Beam"; break;
        //                case 10: sVal = "Stock Beam Qty"; break;
        //                case 11: sVal = "Propose Loom no"; break;
        //                case 12: sVal = "Warp Color"; break;
        //                case 13: sVal = "Weft Color"; break;
        //                case 14: sVal = "Remarks"; break;
        //                default: sVal = ""; break;
        //            }

        //            cell = sheet.Cells[rowIndex, colIndex++];
        //            cell.Value = sVal;
        //            cell.Style.Font.Bold = true;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            fill = cell.Style.Fill;
        //            fill.PatternType = ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.Cyan);
        //            border = cell.Style.Border;
        //            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        }
        //        rowIndex++;
        //        #endregion

        //        #region Table body

        //        if (oFBPBs.Any())
        //        {
        //            if (tsuid>0 && oFBPBs.First().TSUID > 0)
        //            {
        //                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = oFBPBs.First().TSUName; cell.Style.Font.Bold = true;
        //                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; 
        //                rowIndex++;
        //            }

        //            var oSummary = (from oItem in oFBPBs
        //                            group oItem by oItem.FabricSalesContractDetailID into grp
        //                            select new FabricBatchProductionBeam
        //                            {
        //                                FabricSalesContractDetailID = grp.Key,
        //                                BeamCode = grp.Select(x => x.BeamCode).Distinct().Count().ToString(),
        //                                BeamNo = string.Join(",", grp.Select(x => x.BeamNo + " (" + Global.MillionFormat(x.QtyInMtr, 0) + ")").ToArray()),
        //                                Construction = grp.Select(x => x.Construction).FirstOrDefault(),
        //                                RCount = grp.Select(x => x.RCount).FirstOrDefault(),
        //                                Dent = grp.Select(x => x.Dent).FirstOrDefault(),
        //                                Weave = grp.Select(x => x.Weave).FirstOrDefault(),
        //                                TotalEnds = grp.Select(x => x.TotalEnds).FirstOrDefault(),
        //                                BuyerName = grp.Select(x => x.BuyerName).FirstOrDefault(),
        //                                IsInHouse = grp.Select(x => x.IsInHouse).FirstOrDefault(),
        //                                OrderType = grp.Select(x => x.OrderType).FirstOrDefault(),
        //                                NoOfColor = grp.Select(x => x.NoOfColor).FirstOrDefault(),
        //                                FEONo = grp.Select(x => x.FEONo).FirstOrDefault(),
        //                                IsYarnDyed = grp.Select(x => x.IsYarnDyed).FirstOrDefault(),
        //                                Qty = grp.Sum(x => x.Qty),
        //                                BatchQty = grp.Sum(x => x.BatchQty),
        //                                WarpColor = grp.FirstOrDefault().WarpColor,
        //                                WeftColor = grp.FirstOrDefault().WeftColor,
        //                            }).ToList().OrderBy(x => x.FabricSalesContractDetailID);



        //            #region Exe (In house)

        //            var oExes = oSummary.Where(x => x.IsInHouse).ToList().OrderBy(x=>x.BuyerName);

        //            if (oExes.Any())
        //            {

        //                var oExeYarnDyed = oExes.Where(x => x.IsYarnDyed).ToList();
        //                var oExeSolidDyed = oExes.Where(x => !x.IsYarnDyed).ToList();

        //                if (oExeYarnDyed.Any())
        //                {
        //                    cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "Exe-Y/D"; cell.Style.Font.Bold = true;
        //                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; rowIndex++;
        //                    ReadyBeamStock(oRunningLooms, oExeYarnDyed, ref nMaxColumn, ref rowIndex, ref colIndex, ref sheet, ref cell, ParamTTEnds);

        //                }
        //                if (oExeSolidDyed.Any())
        //                {
        //                    cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "Exe-S/D"; cell.Style.Font.Bold = true;
        //                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; rowIndex++;
        //                    ReadyBeamStock(oRunningLooms, oExeSolidDyed, ref nMaxColumn, ref rowIndex, ref colIndex, ref sheet, ref cell, ParamTTEnds);
        //                }



        //            }

        //            #endregion  

        //            #region SCE (Out Side)

        //            var oScws = oSummary.Where(x => !x.IsInHouse).ToList().OrderBy(x => x.BuyerName);

        //            if (oScws.Any())
        //            {

        //                var oScwYarnDyed = oScws.Where(x => x.IsYarnDyed).ToList();
        //                var oScwSolidDyed = oScws.Where(x => !x.IsYarnDyed).ToList();

        //                if (oScwYarnDyed.Any())
        //                {
        //                    cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "SCW-Y/D"; cell.Style.Font.Bold = true;
        //                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; rowIndex++;
        //                    ReadyBeamStock(oRunningLooms, oScwYarnDyed, ref nMaxColumn, ref rowIndex, ref colIndex, ref sheet, ref cell, ParamTTEnds);

        //                }
        //                if (oScwSolidDyed.Any())
        //                {
        //                    cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "SCW-S/D"; cell.Style.Font.Bold = true;
        //                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; rowIndex++;
        //                    ReadyBeamStock(oRunningLooms, oScwSolidDyed, ref nMaxColumn, ref rowIndex, ref colIndex, ref sheet, ref cell, ParamTTEnds);
        //                }

        //            }

        //            #endregion

        //            #region Total
        //            rowIndex++;
        //            colIndex = 2;
        //            cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "Summary Total"; cell.Style.Font.Bold = true;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

        //            cell = sheet.Cells[rowIndex, 12]; cell.Value = oSummary.Sum(x => x.QtyInMtr); cell.Style.Font.Bold = true;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

        //            cell = sheet.Cells[rowIndex, 13, rowIndex, 16]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

        //            rowIndex++;
        //            #endregion


        //        }
        //        #endregion



        //        Response.ClearContent();
        //        Response.BinaryWrite(excelPackage.GetAsByteArray());
        //        Response.AddHeader("content-disposition", "attachment; filename=DailyBeamStockReport.xlsx");
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.Flush();
        //        Response.End();
        //    }
        //}
        #endregion

        #region

        public ActionResult ViewFabricBatchProductioUpload(int menuid, FabricBatchProductionBeam oFBPB)
        {
            List<FabricBatchProduction> oFabricBatchProductions = new List<FabricBatchProduction>();
            if (Session["UploadItem"] != null)
            {
                oFabricBatchProductions = (List<FabricBatchProduction>)Session["UploadItem"];
                Session.Remove("UploadItem");
            }
            return View(oFabricBatchProductions);
        }

        private Tuple<List<FabricBatchProduction>, List<FabricBatchProductionBreakage>> GetFabricBatchProductionFromExcel(HttpPostedFileBase PostedFile, int ShiftID, DateTime RunoutTime, bool IsRunout)
        {
            List<FabricBatchProduction> oFabricBatchProductions = new List<FabricBatchProduction>();
            FabricBatchProduction oFabricBatchProduction = new FabricBatchProduction();

            List<FabricBatchProductionBatchMan> oFabricBatchProductionBatchMans = new List<FabricBatchProductionBatchMan>();
            FabricBatchProductionBatchMan oFBPBM = new FabricBatchProductionBatchMan();

            List<FabricBatchProductionBreakage> oFBPBreakages = new List<FabricBatchProductionBreakage>();

            string sSQL = "Select * from FabricBreakage Where WeavingProcess=" + (short)EnumFabricProcess.Weave + "";
            List<FabricBreakage> oFabricBreakages = new List<FabricBreakage>();
            oFabricBreakages = FabricBreakage.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<string> FBNames = oFabricBreakages.Select(x => x.Name).ToList();



            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            if (PostedFile.ContentLength > 0)
            {
                fileExtension = System.IO.Path.GetExtension(PostedFile.FileName);
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    fileDirectory = Server.MapPath("~/Content/") + PostedFile.FileName;
                    if (System.IO.File.Exists(fileDirectory))
                        System.IO.File.Delete(fileDirectory);

                    PostedFile.SaveAs(fileDirectory);
                    string excelConnectionString = string.Empty;

                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";

                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }
                    excelConnection.Close();
                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                    string query = "";


                    DateTime datetime = DateTime.MinValue;



                    #region Production
                    ds = new DataSet();
                    query = string.Format("Select * from [{0}]", excelSheets[2]); // Sheet Name => Production Format
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                    oRows = ds.Tables[0].Rows;

                    for (int i = 0; i < oRows.Count; i++)
                    {

                        if (!string.IsNullOrEmpty(Convert.ToString(oRows[i]["FEONo"] == DBNull.Value ? "" : oRows[i]["FEONo"])))
                        {
                            oFabricBatchProduction = new FabricBatchProduction();
                            oFabricBatchProduction.FabricBatchProductionBatchMans = new List<FabricBatchProductionBatchMan>();


                            // Parent Object
                            oFabricBatchProduction.TempIndex = i;
                            oFabricBatchProduction.WeavingProcess = EnumWeavingProcess.Loom;
                            oFabricBatchProduction.FabricBatchStatus = EnumFabricBatchState.Weaving;
                            oFabricBatchProduction.FEONo = Convert.ToString(oRows[i]["FEONo"] == DBNull.Value ? "" : oRows[i]["FEONo"]);
                            oFabricBatchProduction.BatchNo = Convert.ToString(oRows[i]["BatchNo"] == DBNull.Value ? "" : oRows[i]["BatchNo"]);
                            oFabricBatchProduction.MachineCode = Convert.ToString(oRows[i]["MachineCode"] == DBNull.Value ? "" : oRows[i]["MachineCode"]);
                            oFabricBatchProduction.BeamNo = Convert.ToString(oRows[i]["BeamNo"] == DBNull.Value ? 0 : oRows[i]["BeamNo"]);
                            //oFabricBatchProduction.Efficiency = Convert.ToDouble((oRows[i]["Efficiency"] == DBNull.Value || oRows[i]["Efficiency"] == "") ? 0 : oRows[i]["Efficiency"]);
                            oFabricBatchProduction.ShiftID = ShiftID;

                            datetime = DateTime.MinValue;
                            DateTime.TryParse(oRows[i]["RunTime"].ToString(), out datetime);
                            oFabricBatchProduction.StartTime = datetime;
                            if (oFabricBatchProduction.StartTime == DateTime.MinValue)
                            {
                                oFabricBatchProduction.StartTime = RunoutTime;
                            }

                            datetime = DateTime.MinValue;
                            DateTime.TryParse((string)oRows[i]["RunOutTime"].ToString(), out datetime);
                            oFabricBatchProduction.EndTime = datetime;
                            if (oFabricBatchProduction.EndTime == DateTime.MinValue && IsRunout)
                            {
                                oFabricBatchProduction.EndTime = RunoutTime;
                            }
                            //oFabricBatchProduction.RPM = Convert.ToInt32(oRows[i]["RPM"] == DBNull.Value ? 0 : oRows[i]["RPM"]);
                            //oFabricBatchProduction.Reed = Convert.ToDouble(oRows[i]["Reed"] == DBNull.Value ? 0 : oRows[i]["Reed"]);
                            //oFabricBatchProduction.Dent = Convert.ToDouble(oRows[i]["Dent"] == DBNull.Value ? "" : oRows[i]["Dent"]);

                            FabricBatchProductionBreakage oFBPB = new FabricBatchProductionBreakage();
                            List<FabricBatchProductionBreakage> oFBPBs = new List<FabricBatchProductionBreakage>();

                            for (int j = 0; j < FBNames.Count; j++)
                            {
                                oFBPB = new FabricBatchProductionBreakage();
                                oFBPB.DurationInMin = Convert.ToDouble((oRows[i][FBNames[j]] == DBNull.Value || oRows[i][FBNames[j]] == "") ? 0 : oRows[i][FBNames[j]]);
                                oFBPB.FBPBreakageID = oFabricBreakages.Where(x => x.Name == FBNames[j]).ToList().First().FBreakageID;
                                if (oFBPB.DurationInMin > 0)
                                    oFBPBs.Add(oFBPB);
                            }
                            //Detail Object
                            oFabricBatchProductionBatchMans = new List<FabricBatchProductionBatchMan>();
                            oFBPBM = new FabricBatchProductionBatchMan();
                            oFBPBM.ShiftID = ShiftID;
                            oFBPBM.FinishDate = RunoutTime;
                            oFBPBM.EmployeeID = Convert.ToInt32(((User)Session[SessionInfo.CurrentUser]).EmployeeID);

                            oFBPBM.Qty = Convert.ToDouble((oRows[i]["Production/Length"] == DBNull.Value || oRows[i]["Production/Length"] == "") ? 0 : oRows[i]["Production/Length"]);
                            oFBPBM.EmployeeID = (int)((User)Session[SessionInfo.CurrentUser]).EmployeeID;
                            oFBPBM.RPM = Convert.ToInt32(oRows[i]["RPM"] == DBNull.Value ? 0 : oRows[i]["RPM"]);
                            oFBPBM.Efficiency = Convert.ToDouble((oRows[i]["Efficiency"] == DBNull.Value || oRows[i]["Efficiency"] == "") ? 0 : oRows[i]["Efficiency"]);
                            oFBPBM.FBPBreakages = oFBPBs;
                            oFabricBatchProductionBatchMans.Add(oFBPBM);

                            oFabricBatchProductionBatchMans.RemoveAll(x => x.Qty == 0 || x.FinishDate == DateTime.MinValue);
                            oFabricBatchProduction.FabricBatchProductionBatchMans = oFabricBatchProductionBatchMans;

                            oFabricBatchProductions.Add(oFabricBatchProduction);
                        }

                    }
                    #endregion

                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                }
                else
                {
                    throw new Exception("File not supported");
                }
            }
            return new Tuple<List<FabricBatchProduction>, List<FabricBatchProductionBreakage>>(oFabricBatchProductions, oFBPBreakages); ;
        }

        [HttpPost]
        public ActionResult ViewFabricBatchProductioUpload(HttpPostedFileBase fileImport, string Params)
        {
            string sMessage = "";
            List<FabricBatchProduction> oFabricBatchProductions = new List<FabricBatchProduction>();
            List<FabricBatchProductionBreakage> oInvalidBreakages = new List<FabricBatchProductionBreakage>();

            try
            {
                if (fileImport == null) { throw new Exception("File not Found"); }
                if (string.IsNullOrEmpty(Params)) { throw new Exception("Parametrer not Set"); }
                int ShiftID = Convert.ToInt32(Params.Split('~')[0]);
                DateTime RunoutTime = Convert.ToDateTime(Params.Split('~')[1]);
                bool IsRunout = Convert.ToBoolean(Params.Split('~')[2]);




                var tuple = this.GetFabricBatchProductionFromExcel(fileImport, ShiftID, RunoutTime, IsRunout);

                oFabricBatchProductions = tuple.Item1;
                oInvalidBreakages = tuple.Item2.Where(x => x.DurationInMin <= 0 || x.NoOfBreakage <= 0 || !oFabricBatchProductions.Select(m => m.BatchNo).Contains(x.BatchNo) || !oFabricBatchProductions.Select(m => m.BeamNo).Contains(x.BeamNo)).ToList();

                List<string> FFEOs = oFabricBatchProductions.Where(x => x.FEONo.Trim() != "").Select(x => x.FEONo).Distinct().ToList();
                List<string> BatchNos = oFabricBatchProductions.Where(x => x.BatchNo.Trim() != "").Select(x => x.BatchNo).Distinct().ToList();
                List<string> MachineNos = oFabricBatchProductions.Where(x => x.MachineCode.Trim() != "").Select(x => x.MachineCode).Distinct().ToList();
                List<string> BeamNos = oFabricBatchProductions.Where(x => x.BeamNo.Trim() != "").Select(x => x.BeamNo).Distinct().ToList();
                List<string> FMs = new List<string>();

                if (MachineNos.Any())
                    FMs.AddRange(MachineNos);
                if (BeamNos.Any())
                    FMs.AddRange(BeamNos);



                string sSQL = string.Empty;
                List<FabricSalesContractDetail> oFEOs = new List<FabricSalesContractDetail>();
                List<FabricBatch> oFBs = new List<FabricBatch>();
                List<FabricMachine> oFabricMachines = new List<FabricMachine>();

                #region Get Fabric Execution Order
                if (FFEOs.Count() > 0)
                {
                    sSQL = "Select * from View_FabricSalesContractDetail Where ISNULL(ApproveBy,0)<>0 And isnull(ExeNo,'')!='' and ExeNo In (" + string.Format("'{0}'", string.Join("','", FFEOs.Select(x => x.Replace("'", "''")))) + ")";
                    oFEOs = FabricSalesContractDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                #endregion

                #region Get Fabric Batch
                if (BatchNos.Count() > 0)
                {
                    sSQL = "Select * from View_FabricBatch Where BatchNo In (" + string.Format("'{0}'", string.Join("','", BatchNos.Select(x => x.Replace("'", "''")))) + ")";
                    oFBs = FabricBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                #endregion

                #region Fabric Machine
                if (FMs.Count() > 0)
                {
                    sSQL = "Select * from View_FabricMachine Where IsActive=1 And WeavingProcess=" + (int)EnumWeavingProcess.Loom + " And MachineStatus In(" + (int)EnumMachineStatus.Free + "," + (int)EnumMachineStatus.Running + "," + (int)EnumMachineStatus.Hold + ") And Code In (" + string.Format("'{0}'", string.Join("','", FMs.Select(x => x.Replace("'", "''")))) + ")";
                    oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                #endregion


                #region Gets Running Batch Production

                List<FabricBatchProduction> oRunnigFBPs = new List<FabricBatchProduction>();
                List<FabricBatchProduction> oOtherFBPs = new List<FabricBatchProduction>();
                List<FabricBatchProductionBeam> oBeamEngagedToProductions = new List<FabricBatchProductionBeam>();
                if (oFEOs.Any() && oFBs.Any() && oFabricMachines.Where(x => x.MachineStatus == EnumMachineStatus.Running && !x.IsBeam).Any())
                {
                    int[] FMIDs = oFabricMachines.Where(x => x.MachineStatus == EnumMachineStatus.Running && !x.IsBeam).Select(x => x.FMID).ToArray();

                    sSQL = "SELECT * FROM View_FabricBatchProduction Where WeavingProcess=3 And FMID In (" + string.Join(",", FMIDs) + ") And FBID In (" + string.Join(",", oFBs.Select(x => x.FBID).ToArray()) + ") And FEOID In (" + string.Join(",", oFEOs.Select(x => x.FabricSalesContractDetailID).ToArray()) + ")";
                    oRunnigFBPs = FabricBatchProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                //else
                //{
                //    sSQL = "SELECT * FROM View_FabricBatchProduction Where WeavingProcess=3 And FBID In (" + string.Join(",", oFBs.Select(x => x.FBID).ToArray()) + ") And FabricSalesContractDetailID In (" + string.Join(",", oFEOs.Select(x => x.FabricSalesContractDetailID).ToArray()) + ")";
                //    oOtherFBPs = FabricBatchProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //}


                #endregion

                var oFBPs = oFabricBatchProductions.Where(
                x => oFEOs.Select(s => s.ExeNo).ToList().Contains(x.FEONo)
                    //&& oFBs.Select(s => s.BatchNo).ToList().Contains(x.BatchNo)
                && oFabricMachines.Where(p => !p.IsBeam && p.TSUID > 0).Select(s => s.Code).ToList().Contains(x.MachineCode)
                && oFabricMachines.Where(p => p.IsBeam).Select(s => s.Code).ToList().Contains(x.BeamNo)
                && (x.StartTime != DateTime.MinValue || (x.EndTime != DateTime.MinValue && x.EndTime > x.StartTime))
                ).ToList();



                oFBPs.ForEach(x =>
                {
                    x.FabricSalesContractDetailID = oFEOs.Where(p => p.ExeNo == x.FEONo).FirstOrDefault().FabricSalesContractDetailID;
                    x.FBID = oFBs.Where(p => p.BatchNo == x.BatchNo).FirstOrDefault().FBID;
                    x.FMID = oFabricMachines.Where(p => !p.IsBeam).Where(p => p.Code == x.MachineCode).FirstOrDefault().FMID;
                    x.MachineStatus = oFabricMachines.Where(p => !p.IsBeam).Where(p => p.Code == x.MachineCode).FirstOrDefault().MachineStatus;
                    x.BeamID = oFabricMachines.Where(p => p.IsBeam).Where(p => p.Code == x.BeamNo).FirstOrDefault().FMID;
                    x.BeamStatus = oFabricMachines.Where(p => p.IsBeam).Where(p => p.Code == x.BeamNo).FirstOrDefault().MachineStatus;

                    x.FBPID = (oRunnigFBPs.Where(p => p.FabricSalesContractDetailID == x.FabricSalesContractDetailID && p.FBID == x.FBID && p.FMID == x.FMID).Any()) ? oRunnigFBPs.Where(p => p.FabricSalesContractDetailID == x.FabricSalesContractDetailID && p.FBID == x.FBID && p.FMID == x.FMID).First().FBPID : 0;
                });
                if (oFBPs.Any())
                {

                    sSQL = "SELECT * FROM View_FabricBatchProductionBeam WHERE WeavingProcess=3 AND BeamID IN (" + string.Join(",", oFBPs.Select(x => x.BeamID)) + ")AND FBPID IN (SELECT FBPID FROM FabricBatchProduction WHERE EndTime IS null)  ORDER BY FBPID ";

                    oBeamEngagedToProductions = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }



                int[] duplicatesBeams = oFBPs.GroupBy(s => s.BeamID).SelectMany(grp => grp.Skip(1)).Select(x => x.TempIndex).ToArray();
                int[] duplicatesMachines = oFBPs.Where(x => x.FBPID == 0).ToList().GroupBy(s => s.FMID).SelectMany(grp => grp.Skip(1)).Select(x => x.TempIndex).ToArray();
                int[] invalidBeamsWithProduction = oFBPs.Where(x => x.FBPID != oBeamEngagedToProductions.Where(p => p.BeamID == x.BeamID).Select(n => n.FBPID).FirstOrDefault()).Select(x => x.TempIndex).ToArray();
                int[] InvalidMachine = oFBPs.Where(x => x.MachineStatus == EnumMachineStatus.Running && x.FBPID == 0).Select(x => x.TempIndex).ToArray();

                // int[] InvalidBeamWithMachine = oFBPs.Where(x => x.MachineStatus == EnumMachineStatus.Running && x.FBPID == 0).Select(x => x.TempIndex).ToArray();


                //int[] aryInvalidList = oOtherFBPs.Where(x => x.FBPID > 0 && x.EndTime >= x.StartTime).Select(x => x.TempIndex).ToArray();
                //var oooo = oFBPs.Where(t2 => aryInvalidList.Any(t1 => aryInvalidList.Contains(t1))).ToList();
                //int[] InvalidRunningLoom = oFBPs.Where(t2 => aryInvalidList.Any(t1 => aryInvalidList.Contains(t1)) && t2.BeamStatus != EnumMachineStatus.Hold).Select(x => x.TempIndex).ToArray();



                oFBPs.RemoveAll(x => InvalidMachine.Contains(x.TempIndex));
                //oFBPs.RemoveAll(x => InvalidRunningLoom.Contains(x.TempIndex));
                oFBPs.RemoveAll(x => invalidBeamsWithProduction.Contains(x.TempIndex));

                oFBPs.RemoveAll(x => duplicatesBeams.Contains(x.TempIndex));
                oFBPs.RemoveAll(x => duplicatesMachines.Contains(x.TempIndex));


                var oFBPs_Error = oFabricBatchProductions.Where(x => !oFBPs.Select(p => p.TempIndex).Contains(x.TempIndex)).ToList();

                //if (oFBPs.Any())
                //    oFBPs = FabricBatchProduction.ImportFabricBatchProduction(oFBPs, ((User)Session[SessionInfo.CurrentUser]).UserID);


                if (oFBPs_Error.Any())
                {
                    oFBPs_Error.ForEach(x =>
                    {
                        //if (!oFEOs.Select(s => s.FEONo).ToList().Contains(x.FEONo))
                        //    x.ErrorMessage = "Invalid Execution";

                        if (!oFBs.Select(s => s.BatchNo).ToList().Contains(x.BatchNo))
                            x.ErrorMessage += ((x.ErrorMessage != "") ? ", " : "") + "Invalid Batch";

                        if (!oFabricMachines.Where(p => !p.IsBeam && p.TSUID > 0).Select(s => s.Code).ToList().Contains(x.MachineCode))
                            x.ErrorMessage += ((x.ErrorMessage != "") ? ", " : "") + "Invalid Machine";

                        if (!oFabricMachines.Where(p => p.IsBeam).Select(s => s.Code).ToList().Contains(x.BeamNo))
                            x.ErrorMessage += ((x.ErrorMessage != "") ? ", " : "") + "Invalid Beam";

                        if (x.StartTime == DateTime.MinValue || (x.EndTime != DateTime.MinValue && x.EndTime < x.StartTime))
                            x.ErrorMessage += ((x.ErrorMessage != "") ? ", " : "") + "Check Run Time & Run Out Time";

                        //if (InvalidRunningLoom.Contains(x.TempIndex))
                        //    x.ErrorMessage += ((x.ErrorMessage != "") ? ", " : "") + "Loom Running Check Run Time & Run Out Time";

                        if (InvalidMachine.Contains(x.TempIndex) && x.EndTime == DateTime.MinValue && x.StartTime > x.EndTime)
                            x.ErrorMessage += ((x.ErrorMessage != "") ? ", " : "") + "This Machine/Loom is currently running for others loom production.";

                        if (invalidBeamsWithProduction.Contains(x.TempIndex))
                            x.ErrorMessage += ((x.ErrorMessage != "") ? ", " : "") + "Beam Running Check Run Time & Run Out Time";
                        if (duplicatesBeams.Contains(x.TempIndex))
                            x.ErrorMessage += ((x.ErrorMessage != "") ? ", " : "") + "Beam Running Check Run Time & Run Out Time";
                        if (duplicatesMachines.Contains(x.TempIndex))
                            x.ErrorMessage += ((x.ErrorMessage != "") ? ", " : "") + "Loom Running Check Run Time & Run Out Time";

                    });

                    oFBPs_Error.AddRange(oFBPs.Where(x => x.FBPID <= 0));
                    ExportWeavingProductionUploadErrorData(oFBPs_Error, oInvalidBreakages);
                }


                if (oFBPs.Count() <= 0 || oFBPs.FirstOrDefault().FBPID <= 0)
                    throw new Exception((oFBPs.Any()) ? oFBPs.FirstOrDefault().ErrorMessage : "Unable to import this data.");
                else if (oFBPs_Error.Any() && oFBPs.Any() && oFBPs.FirstOrDefault().FBPID > 0)
                    sMessage = "Partially Saved";
                else
                    sMessage = "Save successfully";

            }
            catch (Exception ex)
            {
                sMessage = ex.Message;
            }
            return RedirectToAction("View_ReadyBeamsForWeaving", "FabricBatchProduction", new { menuid = Convert.ToInt32(Session[SessionInfo.MenuID]), res = sMessage });
        }

        public void ExportWeavingProductionUploadFormat(string sParams, double nts)
        {
            List<FabricBatchProductionBeam> oFBPBs = new List<FabricBatchProductionBeam>();
            FabricBatchProductionBeam oFBPB = new FabricBatchProductionBeam();

            oFBPB.FEONo = sParams.Split('~')[0];
            oFBPB.BuyerName = sParams.Split('~')[1]; // BuyerIDs
            oFBPB.BeamNo = sParams.Split('~')[2]; // BeamIDs
            oFBPB.MachineCode = sParams.Split('~')[3]; // MachineIDs
            oFBPB.Status = Convert.ToInt16(sParams.Split('~')[4]); // Running Loom=>1, Ready For Loom=>2

            oFBPBs = GetsFabricBatchProductionBeamForExcel(oFBPB);



            int rowIndex = 1;
            int nMaxColumn = 0;
            int colIndex = 1;
            ExcelRange cell;
            ExcelFill fill;


            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";

                #region Sheet => Weaving Production Format
                var sheet = excelPackage.Workbook.Worksheets.Add("Production Batch");
                sheet.Name = "Production Format";

                #region Coloums

                string[] columnHead = new string[] { "FEONo", "BatchNo", "MachineCode", "BeamNo", "RunTime", "RunOutTime", "RPM", "Reed", "Dent", "ShiftADate", "ShiftALength", "ShiftBDate", "ShiftBLength", "ShiftCDate", "ShiftCLength" };
                int[] colWidth = new int[] { 12, 12, 12, 12, 15, 15, 8, 10, 10, 12, 12, 12, 12, 12, 12 };

                for (int i = 0; i < colWidth.Length; i++)
                {
                    sheet.Column(colIndex).Width = colWidth[i];
                    colIndex++;
                }
                nMaxColumn = colIndex;

                #endregion

                #region Column Header
                colIndex = 1;
                for (int i = 0; i < columnHead.Length; i++)
                {
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    colIndex++;
                }
                rowIndex++;
                #endregion

                #region Body


                foreach (FabricBatchProductionBeam oItem in oFBPBs)
                {
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FEONo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BatchNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.WeavingProcessType == EnumWeavingProcess.Loom && oItem.MachineStatus == EnumMachineStatus.Running) ? oItem.MachineCode : ""; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BeamNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.WeavingProcessType == EnumWeavingProcess.Loom && oItem.MachineStatus == EnumMachineStatus.Running) ? oItem.StartTime.ToString("dd MMM yyyy hh:mm") : ""; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.WeavingProcessType == EnumWeavingProcess.Loom) ? oItem.RPM : 0; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.WeavingProcessType == EnumWeavingProcess.Loom) ? oItem.RCount : 0; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "0";

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.WeavingProcessType == EnumWeavingProcess.Loom) ? oItem.Dent : ""; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "0";

                    // Shift A
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    // Shift B
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    // Shift C
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    rowIndex++;
                }

                #endregion

                #endregion

                #region Sheet => Batch Wise Breakage

                rowIndex = 1;
                colIndex = 1;
                string sSQL = "Select * from FabricBreakage Where WeavingProcess=" + (short)EnumFabricProcess.Weave + "";
                List<FabricBreakage> oFabricBreakages = new List<FabricBreakage>();
                oFabricBreakages = FabricBreakage.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);



                sheet = excelPackage.Workbook.Worksheets.Add("Production Shift Breakage");
                sheet.Name = "Production Shift Breakage";
                #region Coloums

                columnHead = new string[] { "BatchNo", "BeamNo", "Shift", "Date", "BreakName", "Duration", "NoOfBreak", "Note" };
                colWidth = new int[] { 12, 10, 10, 15, 35, 15, 15, 40 };

                for (int i = 0; i < colWidth.Length; i++)
                {
                    sheet.Column(colIndex).Width = colWidth[i];
                    colIndex++;
                }
                nMaxColumn = colIndex;

                #endregion

                #region Column Header
                colIndex = 1;
                for (int i = 0; i < columnHead.Length; i++)
                {
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    colIndex++;
                }
                rowIndex++;
                #endregion

                #region Body

                string[] shifts = new string[] { "ShiftA", "ShiftB", "ShiftC" };

                int nSpan = (oFabricBreakages.Any() && oFabricBreakages.First().FBreakageID > 0) ? oFabricBreakages.Count() - 1 : 0;
                int nSpanRowIndex = 0;
                bool IsShiftSpan = true;
                foreach (FabricBatchProductionBeam oItem in oFBPBs)
                {
                    for (int i = 0; i < shifts.Length; i++)
                    {
                        IsShiftSpan = true;
                        foreach (FabricBreakage oFBreak in oFabricBreakages)
                        {
                            colIndex = 1;

                            if (IsShiftSpan)
                            {
                                nSpanRowIndex = rowIndex + nSpan;
                                cell = sheet.Cells[rowIndex, colIndex, nSpanRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.BatchNo; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex, nSpanRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.BeamNo; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                //Shift
                                cell = sheet.Cells[rowIndex, colIndex, nSpanRowIndex, colIndex++]; cell.Merge = true; cell.Value = shifts[i]; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                IsShiftSpan = false;
                            }
                            else
                            {
                                colIndex = 4;
                            }

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            //Breakage Name, duration, no of faults, remarks
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oFBreak.Name; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            rowIndex++;
                        }
                    }
                }


                #endregion

                #endregion

                #region Sheet => Free Machine

                rowIndex = 1;
                colIndex = 1;
                sSQL = "Select * from View_FabricMachine Where WeavingProcess=" + (int)EnumWeavingProcess.Loom + " And IsBeam=0 And IsActive=1 AND MachineStatus=" + (int)EnumMachineStatus.Free + " Order BY TSUID ASC";
                List<FabricMachine> oFabricMachines = new List<FabricMachine>();
                oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);



                sheet = excelPackage.Workbook.Worksheets.Add("Free Loom Machine");
                sheet.Name = "Free Loom Machine";
                #region Coloums

                columnHead = new string[] { "#SL", "Code", "Name", "Shade" };
                colWidth = new int[] { 10, 12, 20, 15 };

                for (int i = 0; i < colWidth.Length; i++)
                {
                    sheet.Column(colIndex).Width = colWidth[i];
                    colIndex++;
                }
                nMaxColumn = colIndex;

                #endregion

                #region Column Header
                colIndex = 1;
                for (int i = 0; i < columnHead.Length; i++)
                {
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    colIndex++;
                }
                rowIndex++;
                #endregion

                #region Body
                int nCount = 0;
                foreach (FabricMachine oItem in oFabricMachines)
                {
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (++nCount).ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Code; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TextileSubUnitName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    rowIndex++;
                }


                #endregion

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=WeavingProductionFormat.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        //public void ExportWeavingProductionUploadFormatNew(string sParams, double nts)
        //{
        //    List<FabricBatchProductionBeam> oFBPBs = new List<FabricBatchProductionBeam>();
        //    FabricBatchProductionBeam oFBPB = new FabricBatchProductionBeam();

        //    oFBPB.FEONo = sParams.Split('~')[0];
        //    oFBPB.BuyerName = sParams.Split('~')[1]; // BuyerIDs
        //    oFBPB.BeamNo = sParams.Split('~')[2]; // BeamIDs
        //    oFBPB.MachineCode = sParams.Split('~')[3]; // MachineIDs
        //    oFBPB.Status = Convert.ToInt16(sParams.Split('~')[4]); // Running Loom=>1, Ready For Loom=>2
        //    oFBPB.TSUID = Convert.ToInt32(sParams.Split('~')[5]);

        //    oFBPBs = GetsFabricBatchProductionBeamForExcel(oFBPB);


        //    List<FabricBreakage> oFabricBreakages = new List<FabricBreakage>();
        //    oFabricBreakages = FabricBreakage.Gets("Select * from FabricBreakage Where WeavingProcess=" + (short)EnumFabricProcess.Weave, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    List<string> FBNames = oFabricBreakages.Select(x => x.Name).ToList();

        //    int rowIndex = 1;
        //    int nMaxColumn = 0;
        //    int colIndex = 1;
        //    ExcelRange cell;
        //    ExcelFill fill;


        //    using (var excelPackage = new ExcelPackage())
        //    {
        //        excelPackage.Workbook.Properties.Author = "ESimSol";
        //        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //        OfficeOpenXml.Style.Border border;

        //        #region Sheet => Weaving Production Format
        //        var sheet = excelPackage.Workbook.Worksheets.Add("Production Batch");
        //        sheet.Name = "Production Format";

        //        #region Coloums


        //        List<string> TempcolumnHead = new List<string> { "FEONo", "BatchNo", "MachineCode", "BeamNo", "RunTime", "RunOutTime", "RPM", "Reed", "Dent", "Production/Length", "Efficiency" };

        //        if (FBNames.Any())
        //        {

        //            foreach (var oItem in FBNames)
        //            {
        //                TempcolumnHead.Add(oItem);
        //            }

        //        }
        //        TempcolumnHead.Add("Total Run Time (Hours)");


        //        string[] columnHead = TempcolumnHead.ToArray();
        //        int[] colWidth = new int[] { };


        //        for (int i = 0; i < columnHead.Length; i++)
        //        {
        //            sheet.Column(colIndex).Width =14;
        //            colIndex++;
        //        }
        //        nMaxColumn = colIndex;

        //        #endregion

        //        #region Column Header
        //        colIndex = 1;
        //        for (int i = 0; i < columnHead.Length; i++)
        //        {
        //            cell = sheet.Cells[rowIndex, colIndex]; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //            colIndex++;
        //        }
        //        rowIndex++;
        //        #endregion

        //        #region Body

        //        int getDynamicColCount = nMaxColumn - 9; //Here  columns are fixed

        //        foreach (FabricBatchProductionBeam oItem in oFBPBs)
        //        {
        //            colIndex = 1;
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FEONo; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BatchNo; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.WeavingProcessType == EnumWeavingProcess.Loom && oItem.MachineStatus == EnumMachineStatus.Running) ? oItem.MachineCode : ""; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BeamNo; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.WeavingProcessType == EnumWeavingProcess.Loom && oItem.MachineStatus == EnumMachineStatus.Running) ? oItem.StartTime.ToString("dd MMM yyyy hh:mm") : ""; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.WeavingProcessType == EnumWeavingProcess.Loom) ? oItem.RPM : 0; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.WeavingProcessType == EnumWeavingProcess.Loom) ? oItem.RCount : 0; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "0";

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.WeavingProcessType == EnumWeavingProcess.Loom) ? oItem.Dent : ""; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "0";
        //            for (int i = getDynamicColCount; i <= nMaxColumn; i++)
        //            {
        //                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            }


        //            rowIndex++;
        //        }

        //        #endregion

        //        #endregion
        //        #region Sheet =>Print Production Format
        //        rowIndex = 1;
        //        colIndex = 1;
        //        sheet = excelPackage.Workbook.Worksheets.Add("Print Production Format");
        //        sheet.Name = "Print Production Format";

        //        Company oCompany = new Company();
        //        oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //        #region Report Header
        //        sheet.Cells[rowIndex, 2, rowIndex, 6].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 2, rowIndex, 6].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Value = "Golora,Charkhanda,Manikgonj"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 2, rowIndex, 6].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Value = " Production Format "; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 2, rowIndex, 6].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Value = "(Weaving Unit)"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 2, rowIndex, 6].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Value = " Date : " + DateTime.Now.ToString("dd MMM yyyy") +( (oFBPB.TSUID>0)?"  Shade-"+oFBPB.TSUID:""); cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;


        //        #endregion





        //        #region Coloums


        //        TempcolumnHead = new List<string> { "FEONo", "BeamNo", "MachineCode", "RPM", "Production/Length", "Efficiency" };

        //        columnHead = TempcolumnHead.ToArray();
        //        colWidth = new int[] { };


        //        for (int i = 0; i < columnHead.Length; i++)
        //        {
        //            sheet.Column(colIndex).Width = 14;
        //            colIndex++;
        //        }
        //        nMaxColumn = colIndex;

        //        #endregion

        //        #region Column Header
        //        colIndex = 1;
        //        for (int i = 0; i < columnHead.Length; i++)
        //        {
        //            cell = sheet.Cells[rowIndex, colIndex]; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            colIndex++;
        //        }
        //        rowIndex++;
        //        #endregion

        //        #region Body



        //        foreach (FabricBatchProductionBeam oItem in oFBPBs)
        //        {
        //            colIndex = 1;
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FEONo; cell.Style.Font.Bold = false;
        //            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BeamNo; cell.Style.Font.Bold = false;
        //            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.WeavingProcessType == EnumWeavingProcess.Loom && oItem.MachineStatus == EnumMachineStatus.Running) ? oItem.MachineCode : ""; cell.Style.Font.Bold = false;
        //            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =  ""; cell.Style.Font.Bold = false;
        //            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            rowIndex++;
        //        }

        //        #endregion

        //        #endregion


        //        #region Sheet => Batch Wise Breakage

        //        rowIndex = 1;
        //        colIndex = 1;
        //        string sSQL = "Select * from FabricBreakage Where WeavingProcess=" + (short)EnumFabricProcess.Weave + "";
        //        //List<FabricBreakage> oFabricBreakages = new List<FabricBreakage>();
        //        oFabricBreakages = FabricBreakage.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);



        //        sheet = excelPackage.Workbook.Worksheets.Add("Production Shift Breakage");
        //        sheet.Name = "Production Shift Breakage";
        //        #region Coloums

        //        columnHead = new string[] { "BatchNo", "BeamNo", "Shift", "Date", "BreakName", "Duration", "NoOfBreak", "Note" };
        //        colWidth = new int[] { 12, 10, 10, 15, 35, 15, 15, 40 };

        //        for (int i = 0; i < colWidth.Length; i++)
        //        {
        //            sheet.Column(colIndex).Width = colWidth[i];
        //            colIndex++;
        //        }
        //        nMaxColumn = colIndex;

        //        #endregion

        //        #region Column Header
        //        colIndex = 1;
        //        for (int i = 0; i < columnHead.Length; i++)
        //        {
        //            cell = sheet.Cells[rowIndex, colIndex]; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //            colIndex++;
        //        }
        //        rowIndex++;
        //        #endregion

        //        #region Body

        //        string[] shifts = new string[] { "ShiftA", "ShiftB", "ShiftC" };

        //        int nSpan = (oFabricBreakages.Any() && oFabricBreakages.First().FBreakageID > 0) ? oFabricBreakages.Count() - 1 : 0;
        //        int nSpanRowIndex = 0;
        //        bool IsShiftSpan = true;
        //        foreach (FabricBatchProductionBeam oItem in oFBPBs)
        //        {
        //            for (int i = 0; i < shifts.Length; i++)
        //            {
        //                IsShiftSpan = true;
        //                foreach (FabricBreakage oFBreak in oFabricBreakages)
        //                {
        //                    colIndex = 1;

        //                    if (IsShiftSpan)
        //                    {
        //                        nSpanRowIndex = rowIndex + nSpan;
        //                        cell = sheet.Cells[rowIndex, colIndex, nSpanRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.BatchNo; cell.Style.Font.Bold = false;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        cell = sheet.Cells[rowIndex, colIndex, nSpanRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.BeamNo; cell.Style.Font.Bold = false;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        //Shift
        //                        cell = sheet.Cells[rowIndex, colIndex, nSpanRowIndex, colIndex++]; cell.Merge = true; cell.Value = shifts[i]; cell.Style.Font.Bold = false;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                        IsShiftSpan = false;
        //                    }
        //                    else
        //                    {
        //                        colIndex = 4;
        //                    }

        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = false;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    //Breakage Name, duration, no of faults, remarks
        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oFBreak.Name; cell.Style.Font.Bold = false;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                    rowIndex++;
        //                }
        //            }
        //        }


        //        #endregion

        //        #endregion

        //        #region Sheet => Free Machine

        //        rowIndex = 1;
        //        colIndex = 1;
        //        sSQL = "Select * from View_FabricMachine Where WeavingProcess=" + (int)EnumWeavingProcess.Loom + " And IsBeam=0 And IsActive=1 AND MachineStatus=" + (int)EnumMachineStatus.Free + " Order BY TSUID ASC";
        //        List<FabricMachine> oFabricMachines = new List<FabricMachine>();
        //        oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);



        //        sheet = excelPackage.Workbook.Worksheets.Add("Free Loom Machine");
        //        sheet.Name = "Free Loom Machine";
        //        #region Coloums

        //        columnHead = new string[] { "#SL", "Code", "Name", "Shade" };
        //        colWidth = new int[] { 10, 12, 20, 15 };

        //        for (int i = 0; i < colWidth.Length; i++)
        //        {
        //            sheet.Column(colIndex).Width = colWidth[i];
        //            colIndex++;
        //        }
        //        nMaxColumn = colIndex;

        //        #endregion

        //        #region Column Header
        //        colIndex = 1;
        //        for (int i = 0; i < columnHead.Length; i++)
        //        {
        //            cell = sheet.Cells[rowIndex, colIndex]; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //            colIndex++;
        //        }
        //        rowIndex++;
        //        #endregion

        //        #region Body
        //        int nCount = 0;
        //        foreach (FabricMachine oItem in oFabricMachines)
        //        {
        //            colIndex = 1;
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (++nCount).ToString(); cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Code; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TextileSubUnitName; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //            rowIndex++;
        //        }


        //        #endregion

        //        #endregion

        //        Response.ClearContent();
        //        Response.BinaryWrite(excelPackage.GetAsByteArray());
        //        Response.AddHeader("content-disposition", "attachment; filename=WeavingProductionFormat.xlsx");
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.Flush();
        //        Response.End();
        //    }
        //}

        public void ExportWeavingProductionUploadErrorData(List<FabricBatchProduction> oFBPs, List<FabricBatchProductionBreakage> oInvalidBreakages)
        {

            int rowIndex = 1;
            int nMaxColumn = 0;
            int colIndex = 1;
            ExcelRange cell;
            ExcelFill fill;


            using (var excelPackage = new ExcelPackage())
            {
                // Production Shift Breakage
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";

                #region Sheet => Weaving Production Format
                var sheet = excelPackage.Workbook.Worksheets.Add("Production Batch");
                sheet.Name = "Production Batch";

                #region Coloums

                string[] columnHead = new string[] { "FEONo", "BatchNo", "MachineCode", "BeamNo", "RunTime", "RunOutTime", "RPM", "Reed", "Dent", "ShiftADate", "ShiftALength", "ShiftBDate", "ShiftBLength", "ShiftCDate", "ShiftCLength", "Error" };
                int[] colWidth = new int[] { 12, 12, 12, 12, 15, 15, 8, 10, 10, 12, 12, 12, 12, 12, 12, 35 };

                for (int i = 0; i < colWidth.Length; i++)
                {
                    sheet.Column(colIndex).Width = colWidth[i];
                    colIndex++;
                }
                nMaxColumn = colIndex;

                #endregion

                #region Column Header
                colIndex = 1;
                for (int i = 0; i < columnHead.Length; i++)
                {
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(((columnHead[i] == "Error") ? Color.Red : Color.Cyan));
                    colIndex++;
                }
                rowIndex++;
                #endregion

                #region Body

                foreach (FabricBatchProduction oItem in oFBPs)
                {
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FEONo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BatchNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.MachineCode; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BeamNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.StartTime == DateTime.MinValue) ? "" : oItem.StartTime.ToString("dd MMM yyyy hh:mm"); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.EndTime == DateTime.MinValue) ? "" : oItem.EndTime.ToString("dd MMM yyyy hh:mm"); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "".ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "0";

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "0";

                    var oFBPBM = new FabricBatchProductionBatchMan();
                    // Shift A
                    oFBPBM = (oItem.FabricBatchProductionBatchMans.Where(x => x.ShiftID == 1).Any()) ? oItem.FabricBatchProductionBatchMans.Where(x => x.ShiftID == 1).FirstOrDefault() : new FabricBatchProductionBatchMan();
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oFBPBM.FinishDate == DateTime.MinValue) ? "" : oFBPBM.FinishDateInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oFBPBM.Qty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    // Shift B
                    oFBPBM = (oItem.FabricBatchProductionBatchMans.Where(x => x.ShiftID == 2).Any()) ? oItem.FabricBatchProductionBatchMans.Where(x => x.ShiftID == 2).FirstOrDefault() : new FabricBatchProductionBatchMan();
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oFBPBM.FinishDate == DateTime.MinValue) ? "" : oFBPBM.FinishDateInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oFBPBM.Qty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    // Shift C
                    oFBPBM = (oItem.FabricBatchProductionBatchMans.Where(x => x.ShiftID == 3).Any()) ? oItem.FabricBatchProductionBatchMans.Where(x => x.ShiftID == 3).FirstOrDefault() : new FabricBatchProductionBatchMan();
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oFBPBM.FinishDate == DateTime.MinValue) ? "" : oFBPBM.FinishDateInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oFBPBM.Qty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";


                    //Set Error 
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ErrorMessage; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    rowIndex++;
                }

                #endregion
                #endregion

                #region Sheet => Batch Wise Breakage

                rowIndex = 1;
                colIndex = 1;
                sheet = excelPackage.Workbook.Worksheets.Add("Production Shift Breakage");
                sheet.Name = "Production Shift Breakage";
                #region Coloums

                columnHead = new string[] { "BatchNo", "BeamNo", "Shift", "Date", "BreakName", "Duration", "NoOfBreak", "Note" };
                colWidth = new int[] { 12, 10, 10, 15, 35, 15, 15, 40 };

                for (int i = 0; i < colWidth.Length; i++)
                {
                    sheet.Column(colIndex).Width = colWidth[i];
                    colIndex++;
                }
                nMaxColumn = colIndex;

                #endregion

                #region Column Header
                colIndex = 1;
                for (int i = 0; i < columnHead.Length; i++)
                {
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    colIndex++;
                }
                rowIndex++;
                #endregion

                #region Body

                var oSummary = oInvalidBreakages.GroupBy(x => new { x.BatchNo, x.BeamNo, x.Shift }, (key, grp) => new
                {
                    BatchNo = key.BatchNo,
                    BeamNo = key.BeamNo,
                    Shift = key.Shift,
                    InvalidBreakages = grp.ToList(),
                });

                int nSpan = 0;
                foreach (var oItem in oSummary)
                {
                    colIndex = 1;
                    nSpan = (oItem.InvalidBreakages.Count > 1) ? oItem.InvalidBreakages.Count - 1 : 0;
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex + nSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.BatchNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex, rowIndex + nSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.BeamNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //Shift
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex + nSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.Shift; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    foreach (var data in oItem.InvalidBreakages)
                    {
                        int nColIndex = colIndex;
                        cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = (data.ShiftDate == DateTime.MinValue) ? "" : data.ShiftDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        //Breakage Name, duration, no of faults, remarks
                        cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = data.FabricBreakageName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = data.DurationInMin; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = data.NoOfBreakage; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = data.Note; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        rowIndex++;
                    }

                }
                #endregion

                #endregion

                #region Sheet => Free Machine

                rowIndex = 1;
                colIndex = 1;

                string sSQL = "Select * from View_FabricMachine Where WeavingProcess=" + (int)EnumWeavingProcess.Loom + " And IsBeam=0 And IsActive=1 AND MachineStatus=" + (int)EnumMachineStatus.Free + " Order BY TSUID ASC";
                List<FabricMachine> oFabricMachines = new List<FabricMachine>();
                oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);



                sheet = excelPackage.Workbook.Worksheets.Add("Free Loom Machine");
                sheet.Name = "Free Loom Machine";
                #region Coloums

                columnHead = new string[] { "#SL", "Code", "Name", "Shade" };
                colWidth = new int[] { 10, 12, 20, 15 };

                for (int i = 0; i < colWidth.Length; i++)
                {
                    sheet.Column(colIndex).Width = colWidth[i];
                    colIndex++;
                }
                nMaxColumn = colIndex;

                #endregion

                #region Column Header
                colIndex = 1;
                for (int i = 0; i < columnHead.Length; i++)
                {
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    colIndex++;
                }
                rowIndex++;
                #endregion

                #region Body
                int nCount = 0;
                foreach (FabricMachine oItem in oFabricMachines)
                {
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (++nCount).ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Code; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TextileSubUnitName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    rowIndex++;
                }


                #endregion

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=WeavingProductionFormat.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        #endregion
        #region FabricBatchSizingSolution Added By Tonmoy
        [HttpPost]
        public JsonResult SaveFBSS(FabricBatchSizingSolution oFBSS)
        {
            try
            {

                oFBSS = oFBSS.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFBSS = new FabricBatchSizingSolution();
                oFBSS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFBSS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region For Product Get For Loom
        [HttpPost]
        public JsonResult ATMLNewSearchByProductName(Product oProduct)
        {
            string sName = oProduct.Params.Split('~')[0];
            int nSplit = oProduct.Params.Split('~').Count();
            string sSQL = "Select * from VIEW_Product as P Where P.ProductID<>0 ";
            if (sName.Trim() != "")
            {
                sSQL = sSQL + " and P.ProductName Like '%" + sName + "%'";
            }


            List<Product> _oProducts = new List<Product>();
            _oProducts = Product.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print Loom Card By Tonmoy

        [HttpPost]
        public ActionResult PrintLoomCards(FormCollection DataCollection)
        {
            FabricBatchProductionBeam oFBPB = new FabricBatchProductionBeam();
            List<FabricBatchProductionBeam> oFBPBs = new List<FabricBatchProductionBeam>();
            oFBPBs = this.GetsFabricBatchProductionBeam(oFBPB);
            List<int> FBPBeamIDs = DataCollection["txtBeams"].Split(',').Select(Int32.Parse).ToList();

            oFBPBs = oFBPBs.Where(p => FBPBeamIDs.Any(x => x == p.FBPBeamID)).ToList();
            string sSQL = "SELECT * FROM View_FabricBatchRawMaterial WHERE WeavingProcess IN (0,3) AND FBID IN (" + string.Join(",", oFBPBs.Select(x => x.FBID)) + ")";
            List<FabricBatchRawMaterial> oFBRMs = new List<FabricBatchRawMaterial>();
            oFBRMs = FabricBatchRawMaterial.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptLoomCard oReport = new rptLoomCard();
            byte[] abytes = oReport.PrepareReport(oFBPBs, oFBRMs);
            return File(abytes, "application/pdf");
        }
        #endregion

        [HttpPost]
        public JsonResult GetFabricChemicalPlans(FabricChemicalPlan oFabricChemicalPlan)
        {
            List<FabricChemicalPlan> oFabricChemicalPlans = new List<FabricChemicalPlan>();
            try
            {
                string sSQL = "SELECT * FROM View_FabricChemicalPlan WHERE FBID = " + oFabricChemicalPlan.FBID;
                oFabricChemicalPlans = FabricChemicalPlan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFabricChemicalPlan = new FabricChemicalPlan();
                oFabricChemicalPlan.ErrorMessage = ex.Message;
                oFabricChemicalPlans.Add(oFabricChemicalPlan);
            }
            var jsonResult = Json(oFabricChemicalPlans, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public JsonResult GetFabricMachineTypes(int nFBID)
        {
            string sSQL = "";
            List<FabricMachineType> oFabricMachineTypes = new List<FabricMachineType>();
            try
            {
                sSQL = "SELECT * FROM View_FabricMachineType WHERE FabricMachineTypeID IN (SELECT FabricMachineTypeID FROM FabricSizingPlanDetail WHERE FabricSizingPlanID=(SELECT TOP 1 FabricSizingPlanID FROM View_FabricSizingPlan WHERE FBID=" + nFBID + " ORDER BY FabricSizingPlanID DESC))";
                oFabricMachineTypes = FabricMachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricMachineTypes = new List<FabricMachineType>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricMachineTypes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetBeamsByType(int nType)
        {
            string sSQL = "";
            List<FabricMachine> oFabricMachines = new List<FabricMachine>();
            try
            {
                sSQL = "SELECT * FROM View_FabricMachine WHERE ChildMachineTypeID = " + nType + " AND IsActive=1 And IsBeam=1 AND WeavingProcess = " + (int)EnumWeavingProcess.Sizing + " AND MachineStatus = " + (int)EnumMachineStatus.Free + "  Order By Code";
                oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricMachines = new List<FabricMachine>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBeamsByTypeForWarping(int nType)
        {
            string sSQL = "";
            List<FabricMachine> oFabricMachines = new List<FabricMachine>();
            try
            {
                sSQL = "SELECT * FROM View_FabricMachine WHERE ChildMachineTypeID = " + nType + " AND IsActive=1 And IsBeam=1 AND WeavingProcess = " + (int)EnumWeavingProcess.Warping + " AND MachineStatus = " + (int)EnumMachineStatus.Free + "  Order By Code";
                oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricMachines = new List<FabricMachine>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBeamsByStatusForWarping(FabricMachine oFM)
        {
            string sSQL = "";
            List<FabricMachine> oFabricMachines = new List<FabricMachine>();
            try
            {
                sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=1 AND WeavingProcess = " + (int)EnumWeavingProcess.Warping;
                if (oFM.ChildMachineTypeID > 0)
                    sSQL += " AND ChildMachineTypeID = " + oFM.ChildMachineTypeID;
                if ((int)oFM.MachineStatus > 0)
                    sSQL += " AND MachineStatus = " + (int)oFM.MachineStatus;
                sSQL += " ORDER BY Code";
                oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricMachines = new List<FabricMachine>();
            }
            var jsonResult = Json(oFabricMachines, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetBeamsByStatusForSizing(FabricMachine oFM)
        {
            string sSQL = "";
            List<FabricMachine> oFabricMachines = new List<FabricMachine>();
            try
            {
                sSQL = "SELECT * FROM View_FabricMachine WHERE IsActive=1 And IsBeam=1 AND WeavingProcess = " + (int)EnumWeavingProcess.Sizing;
                if (oFM.ChildMachineTypeID > 0)
                    sSQL += " AND ChildMachineTypeID = " + oFM.ChildMachineTypeID;
                if ((int)oFM.MachineStatus > 0)
                    sSQL += " AND MachineStatus = " + (int)oFM.MachineStatus;
                sSQL += " ORDER BY Code";
                oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricMachines = new List<FabricMachine>();
            }
            var jsonResult = Json(oFabricMachines, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #region Print List
        private string MakeSQLForExecutionPro(FabricBatchProduction oFabricBatchProduction)
        {
            int nCboProductionDateAdv = 0, nCboStartDateAdv = 0, ncboEndDateAdv = 0, nCboStatusAdv = 0, nCboShiftAdv = 0;
            DateTime dFromProductionDateAdv = DateTime.Today, dToProductionDateAdv = DateTime.Today, dFromStartDateAdv = DateTime.Today, dToStartDateAdv = DateTime.Today, dFromEndDateAdv = DateTime.Today, dToEndDateAdv = DateTime.Today;
            string sDispoNoAdv = "";
            string sReturn1 = "", sReturn = "";
            string sWeavingProcess = "";
            sReturn1 = "Select * from View_FabricBatchProductionDetail";
            if (!string.IsNullOrEmpty(oFabricBatchProduction.ErrorMessage))
            {
                nCboProductionDateAdv = Convert.ToInt32(oFabricBatchProduction.ErrorMessage.Split('~')[0]);
                dFromProductionDateAdv = Convert.ToDateTime(oFabricBatchProduction.ErrorMessage.Split('~')[1]);
                dToProductionDateAdv = Convert.ToDateTime(oFabricBatchProduction.ErrorMessage.Split('~')[2]);

                nCboStartDateAdv = Convert.ToInt32(oFabricBatchProduction.ErrorMessage.Split('~')[3]);
                dFromStartDateAdv = Convert.ToDateTime(oFabricBatchProduction.ErrorMessage.Split('~')[4]);
                dToStartDateAdv = Convert.ToDateTime(oFabricBatchProduction.ErrorMessage.Split('~')[5]);

                ncboEndDateAdv = Convert.ToInt32(oFabricBatchProduction.ErrorMessage.Split('~')[6]);
                dFromEndDateAdv = Convert.ToDateTime(oFabricBatchProduction.ErrorMessage.Split('~')[7]);
                dToEndDateAdv = Convert.ToDateTime(oFabricBatchProduction.ErrorMessage.Split('~')[8]);

                sDispoNoAdv = Convert.ToString(oFabricBatchProduction.ErrorMessage.Split('~')[9]);
                nCboStatusAdv = Convert.ToInt32(oFabricBatchProduction.ErrorMessage.Split('~')[10]);
                nCboShiftAdv = Convert.ToInt32(oFabricBatchProduction.ErrorMessage.Split('~')[11]);
                sWeavingProcess = oFabricBatchProduction.ErrorMessage.Split('~')[12];
            }

            /// Make it dynamic for Sizing , Drowaing
            #region WeavingProcess
            if (!string.IsNullOrEmpty(sWeavingProcess))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " isnull(WeavingProcess,0) in (" + sWeavingProcess + ")";
            }
            else
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " isnull(WeavingProcess,0)=0";
            }
            #endregion


            //DateObject.CompareDateQuery(ref sReturn, "FinishDate", nCboProductionDateAdv, dFromProductionDateAdv, dToProductionDateAdv);
            #region History Date
            if (nCboProductionDateAdv != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboProductionDateAdv == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " ProductionDate>='" + dFromProductionDateAdv.ToString("dd MMM yyyy") + "' and ProductionDate<'" + dToProductionDateAdv.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + "   ProductionDate>='" + dFromProductionDateAdv.ToString("dd MMM yyyy") + "' and ProductionDate<'" + dToProductionDateAdv.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + "  ProductionDate>='" + dFromProductionDateAdv.ToString("dd MMM yyyy") + "' and ProductionDate<'" + dToProductionDateAdv.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + "  ProductionDate>='" + dFromProductionDateAdv.ToString("dd MMM yyyy ") + "' and ProductionDate<'" + dToProductionDateAdv.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + "   ProductionDate>='" + dFromProductionDateAdv.ToString("dd MMM yyyy") + "' and ProductionDate<'" + dToProductionDateAdv.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + "   ProductionDate>='" + dFromProductionDateAdv.ToString("dd MMM yyyy") + "' and ProductionDate<'" + dToProductionDateAdv.AddDays(1).ToString("dd MMM yyyy") + "'";
                }

            }
            #endregion


            #region Start Date
            if (nCboStartDateAdv != (int)EnumCompareOperator.None)
            {

                DateObject.CompareDateQuery(ref sReturn, "StartTime", nCboStartDateAdv, dFromStartDateAdv, dToStartDateAdv);
            }
            #endregion

            #region End Date
            if (ncboEndDateAdv != (int)EnumCompareOperator.None)
            {
                DateObject.CompareDateQuery(ref sReturn, "EndTime", ncboEndDateAdv, dFromEndDateAdv, dToEndDateAdv);
            }
            #endregion

            #region Dispo No
            if (!string.IsNullOrEmpty(sDispoNoAdv))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FEONo LIKE '%" + sDispoNoAdv + "%' ";
            }
            #endregion

            #region Status
            if (nCboStatusAdv > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FBPID in (select FBPID from FabricBatchProduction where ProductionStatus = " + nCboStatusAdv + ")";
            }
            #endregion

            #region Shift
            if (nCboShiftAdv > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ShiftID=" + nCboShiftAdv + " ";
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn + "  ORDER BY ProductionDate DESC";
            return sSQL;
        }
        public ActionResult FabricBatchProductionPrints(string sParams, double nts)
        {
            EnumWeavingProcess eWeavingProcess = EnumWeavingProcess.Warping;
            List<FabricBatchProductionDetail> _oFabricBatchProductionDetails = new List<FabricBatchProductionDetail>();
            List<FabricBatchProductionBeam> _oFabricBatchProductionBeams = new List<FabricBatchProductionBeam>();
            List<FabricBatch> oFabricBatchs = new List<FabricBatch>();
            _oFabricBatchProduction = new FabricBatchProduction();
            _oFabricBatchProduction.ErrorMessage = sParams;


            string Header = "";
            int nCboProductionDateAdv = Convert.ToInt32(sParams.Split('~')[0]);
            DateTime dFromProductionDateAdv = Convert.ToDateTime(sParams.Split('~')[1]);
            DateTime dToProductionDateAdv = Convert.ToDateTime(sParams.Split('~')[2]);
            if (nCboProductionDateAdv == (int)EnumCompareOperator.EqualTo)
            {
                Header = " Date " + dFromProductionDateAdv.ToString("dd MMM yyyy");
            }
            else
            {
                Header = " Date " + dFromProductionDateAdv.ToString("dd MMM yyyy") + " to " + dToProductionDateAdv.ToString("dd MMM yyyy");
            }

            int nCboStartDateAdv = Convert.ToInt32(sParams.Split('~')[3]);
            DateTime dFromStartDateAdv = Convert.ToDateTime(sParams.Split('~')[4]);
            DateTime dToStartDateAdv = Convert.ToDateTime(sParams.Split('~')[5]);
            if (nCboStartDateAdv == 5)
            {
                if (Header != "")
                {
                    Header += ", ";
                }
                Header = Header + "Start Date Between " + dFromStartDateAdv.ToString("dd MMM yyyy") + " to " + dToStartDateAdv.ToString("dd MMM yyyy");
            }
            int ncboEndDateAdv = Convert.ToInt32(sParams.Split('~')[6]);
            DateTime dFromEndDateAdv = Convert.ToDateTime(sParams.Split('~')[7]);
            DateTime dToEndDateAdv = Convert.ToDateTime(sParams.Split('~')[8]);
            if (ncboEndDateAdv == 5)
            {
                if (Header != "")
                {
                    Header += ", ";
                }
                Header = Header + "End Date Between " + dFromEndDateAdv.ToString("dd MMM yyyy") + " to " + dToEndDateAdv.ToString("dd MMM yyyy");
            }
            try
            {
                string sSQL = MakeSQLForExecutionPro(_oFabricBatchProduction);
                _oFabricBatchProductionDetails = FabricBatchProductionDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = string.Join(",", _oFabricBatchProductionDetails.Select(x => x.FBID).Distinct().ToList());
                sSQL = "Select * from View_FabricBatch where FBID in (" + sSQL + ")";
                oFabricBatchs = FabricBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = string.Join(",", _oFabricBatchProductionDetails.Select(x => x.FBPDetailID).Distinct().ToList());
                sSQL = "Select * from View_FabricBatchProductionBeam where FBPDetailID in (" + sSQL + ")";
                _oFabricBatchProductionBeams = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatchProductions = new List<FabricBatchProduction>();
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
                _oFabricBatchProductions.Add(_oFabricBatchProduction);
            }
            if (_oFabricBatchProductionDetails.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBusinessUnit = new BusinessUnit();
                rptFabricBatchProductionPrints oReport = new rptFabricBatchProductionPrints();

                if (oFabricBatchs.Count > 0)
                {
                    eWeavingProcess = _oFabricBatchProductionBeams[0].WeavingProcessType;
                }
                if (eWeavingProcess == EnumWeavingProcess.Warping)
                {
                    byte[] abytes = oReport.PrepareReport(oFabricBatchs, _oFabricBatchProductionDetails, _oFabricBatchProductionBeams, oCompany, oBusinessUnit, Header);
                    return File(abytes, "application/pdf");
                }
                else if (eWeavingProcess == EnumWeavingProcess.Sizing)
                {
                    byte[] abytes = oReport.PrepareReportSizing(oFabricBatchs, _oFabricBatchProductionDetails, _oFabricBatchProductionBeams, oCompany, oBusinessUnit, Header);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    byte[] abytes = oReport.PrepareReport(oFabricBatchs, _oFabricBatchProductionDetails, _oFabricBatchProductionBeams, oCompany, oBusinessUnit, Header);
                    return File(abytes, "application/pdf");
                }
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            }

        }
        public void FabricBatchProductionXL(string sParams, double nts)
        {
            EnumWeavingProcess eWeavingProcess = EnumWeavingProcess.Warping;
            List<FabricBatchProductionDetail> _oFabricBatchProductionDetails = new List<FabricBatchProductionDetail>();
            List<FabricBatchProductionBeam> _oFabricBatchProductionBeams = new List<FabricBatchProductionBeam>();
            List<FabricBatch> oFabricBatchs = new List<FabricBatch>();
            _oFabricBatchProduction = new FabricBatchProduction();
            _oFabricBatchProduction.ErrorMessage = sParams;


            string Header = "";
            int nCboProductionDateAdv = Convert.ToInt32(sParams.Split('~')[0]);
            DateTime dFromProductionDateAdv = Convert.ToDateTime(sParams.Split('~')[1]);
            DateTime dToProductionDateAdv = Convert.ToDateTime(sParams.Split('~')[2]);
            if (nCboProductionDateAdv == (int)EnumCompareOperator.EqualTo)
            {
                Header = " Date " + dFromProductionDateAdv.ToString("dd MMM yyyy");
            }
            else
            {
                Header = " Date " + dFromProductionDateAdv.ToString("dd MMM yyyy") + " to " + dToProductionDateAdv.ToString("dd MMM yyyy");
            }

            int nCboStartDateAdv = Convert.ToInt32(sParams.Split('~')[3]);
            DateTime dFromStartDateAdv = Convert.ToDateTime(sParams.Split('~')[4]);
            DateTime dToStartDateAdv = Convert.ToDateTime(sParams.Split('~')[5]);
            if (nCboStartDateAdv == 5)
            {
                if (Header != "")
                {
                    Header += ", ";
                }
                Header = Header + "Start Date Between " + dFromStartDateAdv.ToString("dd MMM yyyy") + " to " + dToStartDateAdv.ToString("dd MMM yyyy");
            }
            int ncboEndDateAdv = Convert.ToInt32(sParams.Split('~')[6]);
            DateTime dFromEndDateAdv = Convert.ToDateTime(sParams.Split('~')[7]);
            DateTime dToEndDateAdv = Convert.ToDateTime(sParams.Split('~')[8]);
            if (ncboEndDateAdv == 5)
            {
                if (Header != "")
                {
                    Header += ", ";
                }
                Header = Header + "End Date Between " + dFromEndDateAdv.ToString("dd MMM yyyy") + " to " + dToEndDateAdv.ToString("dd MMM yyyy");
            }
            try
            {
                string sSQL = MakeSQLForExecutionPro(_oFabricBatchProduction);
                _oFabricBatchProductionDetails = FabricBatchProductionDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = string.Join(",", _oFabricBatchProductionDetails.Select(x => x.FBID).Distinct().ToList());
                sSQL = "Select * from View_FabricBatch where FBID in (" + sSQL + ")";
                oFabricBatchs = FabricBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = string.Join(",", _oFabricBatchProductionDetails.Select(x => x.FBPDetailID).Distinct().ToList());
                sSQL = "Select * from View_FabricBatchProductionBeam where FBPDetailID in (" + sSQL + ")";
                _oFabricBatchProductionBeams = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatchProductions = new List<FabricBatchProduction>();
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
                _oFabricBatchProductions.Add(_oFabricBatchProduction);
            }
            if (_oFabricBatchProductionDetails.Count > 0)
            {

                if (oFabricBatchs.Count > 0)
                {
                    eWeavingProcess = _oFabricBatchProductionBeams[0].WeavingProcessType;
                }
                if (eWeavingProcess == EnumWeavingProcess.Warping)
                {

                    #region EXCEL WarpingExcel
                    try
                    {
                        Company oCompany = new Company();
                        oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                        int rowIndex = 2;
                        ExcelRange cell;
                        ExcelFill fill;
                        OfficeOpenXml.Style.Border border;
                        int nTotalCol = 0;
                        int nCount = 0;
                        int colIndex = 2;
                        using (var excelPackage = new ExcelPackage())
                        {
                            excelPackage.Workbook.Properties.Author = "ESimSol";
                            excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                            var sheet = excelPackage.Workbook.Worksheets.Add("Warping Production");
                            sheet.Name = "Warping Production";
                            sheet.Column(colIndex++).Width = 10; //SL
                            sheet.Column(colIndex++).Width = 15; //machine
                            sheet.Column(colIndex++).Width = 30; //Customer
                            sheet.Column(colIndex++).Width = 12; //Dispo No
                            sheet.Column(colIndex++).Width = 12; //batchno
                            sheet.Column(colIndex++).Width = 25; //construction
                            sheet.Column(colIndex++).Width = 15; //total ends
                            sheet.Column(colIndex++).Width = 15; //T.Beam
                            sheet.Column(colIndex++).Width = 15; //Satting Length
                            sheet.Column(colIndex++).Width = 10; //Warp beam
                            sheet.Column(colIndex++).Width = 12; //warp length
                            sheet.Column(colIndex++).Width = 12; //Breakage
                            sheet.Column(colIndex++).Width = 15; //remarks


                            #region Report Header
                            sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rowIndex = rowIndex + 1;

                            sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = "Warping Production"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rowIndex++;

                            sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = Header; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rowIndex = rowIndex + 2;
                            #endregion

                            #region Header 1
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Machine"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Customer"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Dispo No"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Batch No"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Construction"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Total Ends"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "T.Beam"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Satting Length"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Warp Beam"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Warp Length"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Breakage"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex++;
                            #endregion
                            int nSL = 1;
                            double nQty = 0;
                            double nWarpBeam = 0;
                            var oMachines = _oFabricBatchProductionDetails.GroupBy(x => new { x.FMID, x.MachineName })
                                               .OrderBy(x => x.Key.FMID)
                                               .Select(x => new
                                               {
                                                   FMID = x.Key.FMID,
                                                   MachineName = x.Key.MachineName

                                               });
                            var oFBs = _oFabricBatchProductionDetails.GroupBy(x => new { x.FBID, x.FMID, x.MachineName }, (key, grp) =>
                                 new
                                 {
                                     FBID = key.FBID,
                                     FMID = key.FMID,
                                     MachineName = key.MachineName,
                                 }).ToList();
                            int nFMID = -999;
                            foreach (var oMachine in oMachines)
                            {
                                var _oFabricBatchs = oFBs.Where(x => x.FMID == oMachine.FMID).ToList();
                                int nCountFM = _oFabricBatchs.Where(x => x.FMID == oMachine.FMID).Count() - 1;
                                if (nCountFM < 0) nCountFM = 0;
                                foreach (var oItem1 in _oFabricBatchs)
                                {
                                    var oFabricBatch = oFabricBatchs.Where(x => x.FBID == oItem1.FBID).ToList();

                                    var oFabricBatchProsDetails = _oFabricBatchProductionDetails.Where(x => x.FMID == oMachine.FMID && x.FBID == oItem1.FBID).ToList();
                                    nWarpBeam = 0;
                                    colIndex = 2;
                                    foreach (var oIem in oFabricBatchProsDetails)
                                    {
                                        nWarpBeam = _oFabricBatchProductionBeams.Where(x => x.FBPDetailID == oIem.FBPDetailID && x.FBID == oItem1.FBID).Count();
                                    }

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    if (nFMID != oItem1.FMID)
                                    {
                                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nCountFM, colIndex++]; cell.Merge = true; cell.Value = oItem1.MachineName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    }
                                    else
                                    {
                                        colIndex++;
                                    }

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oFabricBatch.FirstOrDefault().BuyerName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oFabricBatch.FirstOrDefault().FEONo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oFabricBatch.FirstOrDefault().BatchNoMCode; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oFabricBatch.FirstOrDefault().Construction; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oFabricBatch.FirstOrDefault().TotalEnds.ToString(); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (string.IsNullOrEmpty(oFabricBatch.FirstOrDefault().WarpBeam) ? "" : oFabricBatch.FirstOrDefault().WarpBeam); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ((oFabricBatch.FirstOrDefault().Qty) > 0 ? oFabricBatch.FirstOrDefault().Qty.ToString("#,##0.00;(#,##0.00)") : ""); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ((nWarpBeam) > 0 ? nWarpBeam.ToString() : ""); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    nQty = _oFabricBatchProductionDetails.Where(p => p.FBID == oItem1.FBID && p.FMID == oMachine.FMID).Sum(x => x.QtyM);

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ((nQty) > 0 ? nQty.ToString("#,##0.00;(#,##0.00)") : ""); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    nQty = _oFabricBatchProductionDetails.Where(p => p.FBID == oItem1.FBID && p.FMID == oMachine.FMID).Sum(x => x.NoOfBreakage);

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ((nQty) > 0 ? nQty.ToString("#,##0.00;(#,##0.00)") : ""); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    rowIndex++;
                                    nSL++;
                                    nFMID = oItem1.FMID;
                                }

                                cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                nQty = _oFabricBatchProductionDetails.Where(p => p.FMID == oMachine.FMID).Sum(x => x.QtyM);

                                cell = sheet.Cells[rowIndex, 12]; cell.Value = nQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, 14]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                rowIndex++;

                            }

                            Response.ClearContent();
                            Response.BinaryWrite(excelPackage.GetAsByteArray());
                            Response.AddHeader("content-disposition", "attachment; filename=RPT_WarpingProduction.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Flush();
                            Response.End();


                        }

                    }
                    catch (Exception ex)
                    {
                        _oFabricBatchProductions = new List<FabricBatchProduction>();
                        _oFabricBatchProduction = new FabricBatchProduction();
                        _oFabricBatchProduction.ErrorMessage = ex.Message;
                        _oFabricBatchProductions.Add(_oFabricBatchProduction);
                    }
                    #endregion

                }
                else if (eWeavingProcess == EnumWeavingProcess.Sizing)
                {
                    #region EXCEL SizingExcel
                    try
                    {
                        Company oCompany = new Company();
                        oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                        int rowIndex = 2;
                        ExcelRange cell;
                        ExcelFill fill;
                        OfficeOpenXml.Style.Border border;
                        int nTotalCol = 0;
                        int nCount = 0;
                        int colIndex = 2;
                        using (var excelPackage = new ExcelPackage())
                        {
                            excelPackage.Workbook.Properties.Author = "ESimSol";
                            excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                            var sheet = excelPackage.Workbook.Worksheets.Add("Sizing Production");
                            sheet.Name = "Sizing Production";
                            sheet.Column(colIndex++).Width = 10; //SL
                            sheet.Column(colIndex++).Width = 15; //machine
                            sheet.Column(colIndex++).Width = 30; //Customer
                            sheet.Column(colIndex++).Width = 12; //Dispo No
                            sheet.Column(colIndex++).Width = 12; //batchno
                            sheet.Column(colIndex++).Width = 25; //construction
                            sheet.Column(colIndex++).Width = 15; //total ends
                            sheet.Column(colIndex++).Width = 15; //T.Beam
                            sheet.Column(colIndex++).Width = 15; //Satting Length
                            sheet.Column(colIndex++).Width = 10; //Warp beam
                            sheet.Column(colIndex++).Width = 12; //warp length
                            sheet.Column(colIndex++).Width = 12; //Breakage
                            sheet.Column(colIndex++).Width = 15; //remarks


                            #region Report Header
                            sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rowIndex = rowIndex + 1;

                            sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = "Sizing Production"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rowIndex++;

                            sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = Header; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rowIndex = rowIndex + 2;
                            #endregion

                            #region Header 1
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Machine"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Customer"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Dispo No"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Batch No"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Construction"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Total Ends"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "T.Beam"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Satting Length"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Warp Beam"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Warp Length"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Breakage"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex++;
                            #endregion
                            int nSL = 1;
                            double nQty = 0;
                            double nWarpBeam = 0;
                            var oMachines = _oFabricBatchProductionDetails.GroupBy(x => new { x.FMID, x.MachineName })
                                               .OrderBy(x => x.Key.FMID)
                                               .Select(x => new
                                               {
                                                   FMID = x.Key.FMID,
                                                   MachineName = x.Key.MachineName

                                               });
                            var oFBs = _oFabricBatchProductionDetails.GroupBy(x => new { x.FBID, x.FMID, x.MachineName }, (key, grp) =>
                                 new
                                 {
                                     FBID = key.FBID,
                                     FMID = key.FMID,
                                     MachineName = key.MachineName,
                                 }).ToList();
                            int nFMID = -999;
                            foreach (var oMachine in oMachines)
                            {
                                var _oFabricBatchs = oFBs.Where(x => x.FMID == oMachine.FMID).ToList();
                                int nCountFM = _oFabricBatchs.Where(x => x.FMID == oMachine.FMID).Count() - 1;
                                if (nCountFM < 0) nCountFM = 0;
                                foreach (var oItem1 in _oFabricBatchs)
                                {
                                    var oFabricBatch = oFabricBatchs.Where(x => x.FBID == oItem1.FBID).ToList();

                                    var oFabricBatchProsDetails = _oFabricBatchProductionDetails.Where(x => x.FMID == oMachine.FMID && x.FBID == oItem1.FBID).ToList();
                                    nWarpBeam = 0;
                                    colIndex = 2;
                                    foreach (var oIem in oFabricBatchProsDetails)
                                    {
                                        nWarpBeam = _oFabricBatchProductionBeams.Where(x => x.FBPDetailID == oIem.FBPDetailID && x.FBID == oItem1.FBID).Count();
                                    }

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    if (nFMID != oItem1.FMID)
                                    {
                                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nCountFM, colIndex++]; cell.Merge = true; cell.Value = oItem1.MachineName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    }
                                    else
                                    {
                                        colIndex++;
                                    }

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oFabricBatch.FirstOrDefault().BuyerName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oFabricBatch.FirstOrDefault().FEONo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oFabricBatch.FirstOrDefault().BatchNoMCode; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oFabricBatch.FirstOrDefault().Construction; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oFabricBatch.FirstOrDefault().TotalEnds.ToString(); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (string.IsNullOrEmpty(oFabricBatch.FirstOrDefault().WarpBeam) ? "" : oFabricBatch.FirstOrDefault().WarpBeam); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ((oFabricBatch.FirstOrDefault().Qty) > 0 ? oFabricBatch.FirstOrDefault().Qty.ToString("#,##0.00;(#,##0.00)") : ""); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ((nWarpBeam) > 0 ? nWarpBeam.ToString() : ""); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    nQty = _oFabricBatchProductionDetails.Where(p => p.FBID == oItem1.FBID && p.FMID == oMachine.FMID).Sum(x => x.QtyM);

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ((nQty) > 0 ? nQty.ToString("#,##0.00;(#,##0.00)") : ""); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    nQty = _oFabricBatchProductionDetails.Where(p => p.FBID == oItem1.FBID && p.FMID == oMachine.FMID).Sum(x => x.NoOfBreakage);

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ((nQty) > 0 ? nQty.ToString("#,##0.00;(#,##0.00)") : ""); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    rowIndex++;
                                    nSL++;
                                    nFMID = oItem1.FMID;
                                }

                                cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                nQty = _oFabricBatchProductionDetails.Where(p => p.FMID == oMachine.FMID).Sum(x => x.QtyM);

                                cell = sheet.Cells[rowIndex, 12]; cell.Value = nQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, 14]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                rowIndex++;

                            }

                            Response.ClearContent();
                            Response.BinaryWrite(excelPackage.GetAsByteArray());
                            Response.AddHeader("content-disposition", "attachment; filename=RPT_SizingProduction.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Flush();
                            Response.End();


                        }

                    }
                    catch (Exception ex)
                    {
                        _oFabricBatchProductions = new List<FabricBatchProduction>();
                        _oFabricBatchProduction = new FabricBatchProduction();
                        _oFabricBatchProduction.ErrorMessage = ex.Message;
                        _oFabricBatchProductions.Add(_oFabricBatchProduction);
                    }
                    #endregion

                }
                else
                {
                    //byte[] abytes = oReport.PrepareReport(oFabricBatchs, _oFabricBatchProductionDetails, _oFabricBatchProductionBeams, oCompany, oBusinessUnit, Header);
                    //return File(abytes, "application/pdf");
                }
            }
            else
            {
                throw new Exception("Failed to Print");
            }

        }


        public ActionResult SizingExecutionPrints(string sParams, double nts)
        {
            _oFabricBatchProductions = new List<FabricBatchProduction>();
            _oFabricBatchProduction = new FabricBatchProduction();
            _oFabricBatchProduction.ErrorMessage = sParams;
            string Header = "";
            int nCboProductionDateAdv = Convert.ToInt32(sParams.Split('~')[0]);
            DateTime dFromProductionDateAdv = Convert.ToDateTime(sParams.Split('~')[1]);
            DateTime dToProductionDateAdv = Convert.ToDateTime(sParams.Split('~')[2]);
            if (nCboProductionDateAdv == 5)
            {
                Header = "Production Date Between " + dFromProductionDateAdv.ToString("dd MMM yyyy") + " to " + dToProductionDateAdv.ToString("dd MMM yyyy");
            }

            int nCboStartDateAdv = Convert.ToInt32(sParams.Split('~')[3]);
            DateTime dFromStartDateAdv = Convert.ToDateTime(sParams.Split('~')[4]);
            DateTime dToStartDateAdv = Convert.ToDateTime(sParams.Split('~')[5]);
            if (nCboStartDateAdv == 5)
            {
                if (Header != "")
                {
                    Header += ", ";
                }
                Header = Header + "Start Date Between " + dFromStartDateAdv.ToString("dd MMM yyyy") + " to " + dToStartDateAdv.ToString("dd MMM yyyy");
            }
            int ncboEndDateAdv = Convert.ToInt32(sParams.Split('~')[6]);
            DateTime dFromEndDateAdv = Convert.ToDateTime(sParams.Split('~')[7]);
            DateTime dToEndDateAdv = Convert.ToDateTime(sParams.Split('~')[8]);
            if (ncboEndDateAdv == 5)
            {
                if (Header != "")
                {
                    Header += ", ";
                }
                Header = Header + "End Date Between " + dFromEndDateAdv.ToString("dd MMM yyyy") + " to " + dToEndDateAdv.ToString("dd MMM yyyy");
            }
            try
            {
                string sSQL = MakeSQLForExecution(_oFabricBatchProduction);
                _oFabricBatchProductions = FabricBatchProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatchProductions = new List<FabricBatchProduction>();
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
                _oFabricBatchProductions.Add(_oFabricBatchProduction);
            }

            if (_oFabricBatchProductions.Count > 1)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBusinessUnit = new BusinessUnit();
                rptSizingExecutionPrints oReport = new rptSizingExecutionPrints();
                byte[] abytes = oReport.PrepareReport(_oFabricBatchProductions, oCompany, oBusinessUnit, Header, "");
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            }

        }
        #endregion
        #region ReadyBeam Prints
        public ActionResult ReadyBeamPrints(string sParams, double nts)
        {
            List<FabricBatchProductionBeam> oFBPBeams = new List<FabricBatchProductionBeam>();
            FabricBatchProductionBeam oFBPBeam = new FabricBatchProductionBeam();
            List<FabricMachine> oFabricMachines = new List<FabricMachine>();
            EnumWeavingProcess eWeavingProcess = EnumWeavingProcess.Warping;
            string sWeavingProcess = "";
            try
            {
                int nWeavingProcessType = Convert.ToInt32(sParams.Split('~')[0]);
                string sSQL = "SELECT * FROM View_FabricBatchProductionBeam WHERE  IsFinish = 0 AND WeavingProcessType = " + nWeavingProcessType + " Order By FBPBeamID";
                oFBPBeams = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                string BeamIDs = string.Join(",", oFBPBeams.Select(s => s.BeamID).Distinct());

                sSQL = "SELECT * FROM View_FabricMachine WHERE FMID NOT IN (" + BeamIDs + ") AND IsActive = 1 AND WeavingProcess =" + nWeavingProcessType + " Order By Name";
                oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                eWeavingProcess = oFBPBeams[0].WeavingProcessType;

            }
            catch (Exception ex)
            {
                oFBPBeams = new List<FabricBatchProductionBeam>();
                oFBPBeam = new FabricBatchProductionBeam();
                oFBPBeam.ErrorMessage = ex.Message;
                oFBPBeams.Add(oFBPBeam);
            }
            if (oFBPBeams.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBusinessUnit = new BusinessUnit();
                rptReadyBeamPrints oReport = new rptReadyBeamPrints();

                if (eWeavingProcess == EnumWeavingProcess.Warping)
                {
                    sWeavingProcess = "Warping Execution Process";
                    byte[] abytes = oReport.PrepareReport(oFBPBeams, oFabricMachines, oCompany, oBusinessUnit, sWeavingProcess);
                    return File(abytes, "application/pdf");
                }

                if (eWeavingProcess == EnumWeavingProcess.Sizing)
                {
                    sWeavingProcess = "Sizing Execution Process";
                    byte[] abytes = oReport.PrepareReport(oFBPBeams, oFabricMachines, oCompany, oBusinessUnit, sWeavingProcess);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    rptErrorMessage _oReport = new rptErrorMessage();
                    byte[] abytes = _oReport.PrepareReport("No Print Setup Found!!");
                    return File(abytes, "application/pdf");

                }
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            }
        }
        #endregion

        #region Drawing Print
        public ActionResult DrawingPrints(string sParams, double nts)
        {
            EnumWeavingProcess eWeavingProcess = EnumWeavingProcess.Warping;
            List<FabricBatchProductionDetail> _oFabricBatchProductionDetails = new List<FabricBatchProductionDetail>();
            List<FabricBatchProductionBeam> _oFabricBatchProductionBeams = new List<FabricBatchProductionBeam>();
            List<FabricBatch> oFabricBatchs = new List<FabricBatch>();
            _oFabricBatchProduction = new FabricBatchProduction();
            _oFabricBatchProduction.ErrorMessage = sParams;


            string Header = "";
            int nCboProductionDateAdv = Convert.ToInt32(sParams.Split('~')[0]);
            DateTime dFromProductionDateAdv = Convert.ToDateTime(sParams.Split('~')[1]);
            DateTime dToProductionDateAdv = Convert.ToDateTime(sParams.Split('~')[2]);
            if (nCboProductionDateAdv == (int)EnumCompareOperator.EqualTo)
            {
                Header = " Date " + dFromProductionDateAdv.ToString("dd MMM yyyy");
            }
            else
            {
                Header = " Date " + dFromProductionDateAdv.ToString("dd MMM yyyy") + " to " + dToProductionDateAdv.ToString("dd MMM yyyy");
            }

            int nCboStartDateAdv = Convert.ToInt32(sParams.Split('~')[3]);
            DateTime dFromStartDateAdv = Convert.ToDateTime(sParams.Split('~')[4]);
            DateTime dToStartDateAdv = Convert.ToDateTime(sParams.Split('~')[5]);
            if (nCboStartDateAdv == 5)
            {
                if (Header != "")
                {
                    Header += ", ";
                }
                Header = Header + "Start Date Between " + dFromStartDateAdv.ToString("dd MMM yyyy") + " to " + dToStartDateAdv.ToString("dd MMM yyyy");
            }
            int ncboEndDateAdv = Convert.ToInt32(sParams.Split('~')[6]);
            DateTime dFromEndDateAdv = Convert.ToDateTime(sParams.Split('~')[7]);
            DateTime dToEndDateAdv = Convert.ToDateTime(sParams.Split('~')[8]);
            if (ncboEndDateAdv == 5)
            {
                if (Header != "")
                {
                    Header += ", ";
                }
                Header = Header + "End Date Between " + dFromEndDateAdv.ToString("dd MMM yyyy") + " to " + dToEndDateAdv.ToString("dd MMM yyyy");
            }
            try
            {
                string sSQL = MakeSQLForExecutionPro(_oFabricBatchProduction);
                _oFabricBatchProductionDetails = FabricBatchProductionDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = string.Join(",", _oFabricBatchProductionDetails.Select(x => x.FBID).Distinct().ToList());
                sSQL = "Select * from View_FabricBatch where FBID in (" + sSQL + ")";
                oFabricBatchs = FabricBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //sSQL = string.Join(",", _oFabricBatchProductionDetails.Select(x => x.FBPDetailID).Distinct().ToList());
                //sSQL = "Select * from View_FabricBatchProductionBeam where FBPDetailID in (" + sSQL + ")";
                //_oFabricBatchProductionBeams = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricBatchProductionBeams = FabricBatchProductionBeam.Gets("SELECT * FROM View_FabricBatchProductionBeam WHERE  FBPID in  (Select FBPID from FabricBatchProduction where WeavingProcess=" + (int)EnumWeavingProcess.Sizing + " and FBID IN (" + string.Join(",", oFabricBatchs.Select(x => x.FBID)) + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatchProductions = new List<FabricBatchProduction>();
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
                _oFabricBatchProductions.Add(_oFabricBatchProduction);
            }
            if (_oFabricBatchProductionDetails.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptDrawingPrintDoc oReport = new rptDrawingPrintDoc();
                byte[] abytes = oReport.PrepareReport(oFabricBatchs, _oFabricBatchProductionDetails, _oFabricBatchProductionBeams, oCompany, Header);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            }

        }

        public ActionResult LeasingPrints(string sParams, double nts)
        {
            List<FabricBatchProductionDetail> _oFabricBatchProductionDetails = new List<FabricBatchProductionDetail>();
            List<FabricBatchProductionBeam> _oFabricBatchProductionBeams = new List<FabricBatchProductionBeam>();
            List<FabricBatch> oFabricBatchs = new List<FabricBatch>();
            _oFabricBatchProduction = new FabricBatchProduction();
            _oFabricBatchProduction.ErrorMessage = sParams;

            string Header = "";
            int nCboProductionDateAdv = Convert.ToInt32(sParams.Split('~')[0]);
            DateTime dFromProductionDateAdv = Convert.ToDateTime(sParams.Split('~')[1]);
            DateTime dToProductionDateAdv = Convert.ToDateTime(sParams.Split('~')[2]);
            if (nCboProductionDateAdv == (int)EnumCompareOperator.EqualTo)
            {
                Header = " Date " + dFromProductionDateAdv.ToString("dd MMM yyyy");
            }
            else
            {
                Header = " Date " + dFromProductionDateAdv.ToString("dd MMM yyyy") + " to " + dToProductionDateAdv.ToString("dd MMM yyyy");
            }

            int nCboStartDateAdv = Convert.ToInt32(sParams.Split('~')[3]);
            DateTime dFromStartDateAdv = Convert.ToDateTime(sParams.Split('~')[4]);
            DateTime dToStartDateAdv = Convert.ToDateTime(sParams.Split('~')[5]);
            if (nCboStartDateAdv == 5)
            {
                if (Header != "")
                {
                    Header += ", ";
                }
                Header = Header + "Start Date Between " + dFromStartDateAdv.ToString("dd MMM yyyy") + " to " + dToStartDateAdv.ToString("dd MMM yyyy");
            }
            int ncboEndDateAdv = Convert.ToInt32(sParams.Split('~')[6]);
            DateTime dFromEndDateAdv = Convert.ToDateTime(sParams.Split('~')[7]);
            DateTime dToEndDateAdv = Convert.ToDateTime(sParams.Split('~')[8]);
            if (ncboEndDateAdv == 5)
            {
                if (Header != "")
                {
                    Header += ", ";
                }
                Header = Header + "End Date Between " + dFromEndDateAdv.ToString("dd MMM yyyy") + " to " + dToEndDateAdv.ToString("dd MMM yyyy");
            }
            try
            {
                string sSQL = MakeSQLForExecutionPro(_oFabricBatchProduction);
                _oFabricBatchProductionDetails = FabricBatchProductionDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = string.Join(",", _oFabricBatchProductionDetails.Select(x => x.FBID).Distinct().ToList());
                sSQL = "Select * from View_FabricBatch where FBID in (" + sSQL + ")";
                oFabricBatchs = FabricBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //sSQL = string.Join(",", _oFabricBatchProductionDetails.Select(x => x.FBPDetailID).Distinct().ToList());
                //sSQL = "Select * from View_FabricBatchProductionBeam where FBPDetailID in (" + sSQL + ")";
                //_oFabricBatchProductionBeams = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricBatchProductionBeams = FabricBatchProductionBeam.Gets("SELECT * FROM View_FabricBatchProductionBeam WHERE  FBPID in  (Select FBPID from FabricBatchProduction where WeavingProcess=" + (int)EnumWeavingProcess.Sizing + " and FBID IN (" + string.Join(",", oFabricBatchs.Select(x => x.FBID)) + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatchProductions = new List<FabricBatchProduction>();
                _oFabricBatchProduction = new FabricBatchProduction();
                _oFabricBatchProduction.ErrorMessage = ex.Message;
                _oFabricBatchProductions.Add(_oFabricBatchProduction);
            }
            if (_oFabricBatchProductionDetails.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptLeasingPrintDoc oReport = new rptLeasingPrintDoc();
                byte[] abytes = oReport.PrepareReport(oFabricBatchs, _oFabricBatchProductionDetails, _oFabricBatchProductionBeams, oCompany, Header);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            }

        }
        #endregion
    }
}