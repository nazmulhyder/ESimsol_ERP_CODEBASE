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
    public class FabricBatchLoomController : Controller
    {
        #region Declaration
        FabricBatchLoom _oFabricBatchLoom = new FabricBatchLoom();
        List<FabricBatchLoom> _oFabricBatchLooms = new List<FabricBatchLoom>();
        string _sDateMsg = "";
        #endregion

      
        #region Weaving
        public ActionResult ViewFabricBatchLooms(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricBatchLoom).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            
            ViewBag.MenuID = menuid;
            string sSQL = "Select * from View_TextileSubUnit Where ISNULL(InactiveBy,0)=0";
            List<TextileSubUnit> oTextileSubUnits = new List<TextileSubUnit>();
            oTextileSubUnits = TextileSubUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.TextileSubUnits = oTextileSubUnits;
            ViewBag.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RSShifts = RSShift.Gets("SELECT * FROM RSShift WHERE ModuleType = " + (int)EnumModuleName.LoomExecution, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oFabricBatchLooms = new List<FabricBatchLoom>();
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.FabricBreakages = FabricBreakage.Gets(EnumWeavingProcess.Loom, ((User)Session[SessionInfo.CurrentUser]).UserID);

            return View(_oFabricBatchLooms);
        }

        [HttpGet]
        public JsonResult CreateWeaving(string sIDs)
        {
            FabricBatch oFB = new FabricBatch();
            _oFabricBatchLoom = new FabricBatchLoom();
            List<FabricBatchLoomDetail> oFabricBatchLoomDetails = new List<FabricBatchLoomDetail>();
            FabricBatchProductionBeam oFBPB = new FabricBatchProductionBeam();
            FabricLoomPlan oFabricLoomPlan = new FabricLoomPlan();
            List<FabricLoomPlanDetail> oFabricLoomPlanDetails = new List<FabricLoomPlanDetail>();
            int nFBPBeamID = Convert.ToInt32(sIDs.Split('~')[0]);
            int nBeamID = Convert.ToInt32(sIDs.Split('~')[1]);
            int nFBID = Convert.ToInt32(sIDs.Split('~')[2]);
            int nFabricSalesContractDetailID = Convert.ToInt32(sIDs.Split('~')[3]);
            int nFabricBatchLoomID = Convert.ToInt32(sIDs.Split('~')[4]);
            int nBUID = Convert.ToInt32(sIDs.Split('~')[5]);
            _oFabricBatchLoom.FMID = Convert.ToInt32(sIDs.Split('~')[6]);
            int nFLPID = Convert.ToInt32(sIDs.Split('~')[7]);

            oFBPB = FabricBatchProductionBeam.Gets("SELECT * FROM View_FabricBatchProductionBeam WHERE FBPBeamID=" + nFBPBeamID, ((User)Session[SessionInfo.CurrentUser]).UserID).FirstOrDefault();
            FabricMachine oFabricMachine = new FabricMachine();
            if (nFabricBatchLoomID > 0)
            {
                _oFabricBatchLoom = _oFabricBatchLoom.Get(nFabricBatchLoomID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricBatchLoomDetails = FabricBatchLoomDetail.Gets(_oFabricBatchLoom.FabricBatchLoomID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricMachine = oFabricMachine.Get(_oFabricBatchLoom.FMID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricBatchLoom.FabricMachines.Add(oFabricMachine);
            }
            else
            {                
                oFabricLoomPlan = oFabricLoomPlan.Get(nFLPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricLoomPlanDetails = FabricLoomPlanDetail.Gets("SELECT * FROM View_FabricLoomPlanDetail WHERE FLPID = " + nFLPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFabricLoomPlan.FLPID > 0)
                {
                    _oFabricBatchLoom.FabricBatchLoomID = 0;
                    _oFabricBatchLoom.FBID = oFabricLoomPlan.FBID;
                    _oFabricBatchLoom.FLPID = oFabricLoomPlan.FLPID;
                    _oFabricBatchLoom.WeavingProcess = EnumWeavingProcess.Loom;
                    
                    _oFabricBatchLoom.FabricBatchStatus = oFabricLoomPlan.Status;
                    _oFabricBatchLoom.RPM = 0;
                    _oFabricBatchLoom.ReedCount = oFBPB.ReedCount;
                    _oFabricBatchLoom.Dent = oFBPB.Dent;
                    _oFabricBatchLoom.Texture = "";
                    _oFabricBatchLoom.ShiftID = 0;
                    //_oFabricBatchLoom.Qty = oFBPB.Qty;
                    //_oFabricBatchLoom.FBPBeamID = oFBPB.FBPBeamID;
                    if (oFabricLoomPlanDetails.Count > 0)
                    {
                        _oFabricBatchLoom.FBPBeamID = oFabricLoomPlanDetails[0].FBPBeamID;
                        _oFabricBatchLoom.BeamID = oFabricLoomPlanDetails[0].BeamID;
                        _oFabricBatchLoom.BeamNo = string.Join(",", oFabricLoomPlanDetails.Select(x => x.BeamNo));
                        _oFabricBatchLoom.Qty = oFabricLoomPlanDetails.Sum(x => x.Qty);
                        _oFabricBatchLoom.IsDrawing = oFabricLoomPlanDetails[0].IsDrawing;
                    }

                    if (_oFabricBatchLoom.FMID > 0)
                    {
                        oFabricMachine = new FabricMachine();
                        oFabricMachine = oFabricMachine.Get(_oFabricBatchLoom.FMID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oFabricBatchLoom.FabricMachines.Add(oFabricMachine);
                        _oFabricBatchLoom.TSUID = oFabricMachine.TSUID;
                    }
                    if (_oFabricBatchLoom.EndTime < new DateTime(1900, 01, 01, 1, 1, 1))
                    {
                        _oFabricBatchLoom.EndTime = new DateTime(1900, 01, 01, 1, 1, 1);
                    }

                    if (_oFabricBatchLoom.EndTime != new DateTime(1900, 01, 01, 1, 1, 1))
                    {
                        _oFabricBatchLoom.IsRunOut = true;
                    }
                    else
                    {
                        _oFabricBatchLoom.IsRunOut = false;
                    }
                    oFabricBatchLoomDetails = new List<FabricBatchLoomDetail>();
                }
            }

            _oFabricBatchLoom.FBID = nFBID;
            _oFabricBatchLoom.FSCDID = nFabricSalesContractDetailID;

            //_oFabricBatchLoom.FabricMachines = FabricMachine.Gets(false, EnumWeavingProcess.Loom, EnumMachineStatus.Free, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //if (oFabricMachine.FMID > 0) { _oFabricBatchLoom.FabricMachines.Add(oFabricMachine); }
            //else {_oFabricBatchLoom.FabricMachines = FabricMachine.Gets(false, EnumWeavingProcess.Loom, EnumMachineStatus.Free, ((User)Session[SessionInfo.CurrentUser]).UserID);}

            _oFabricBatchLoom.BatchMans = Employee.Gets(EnumEmployeeDesignationType.LoomOperator, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oFabricBatchLoom.FabricBreakages = FabricBreakage.Gets(EnumWeavingProcess.Loom, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oFabricBatchLoom.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oFabricBatchLoom.RSShifts = RSShift.Gets("SELECT * FROM RSShift WHERE ModuleType = " + (int)EnumModuleName.LoomExecution, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Get Fabric Batch
            if (nFBID > 0)
            {
                oFB = oFB.Get(nFBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oFabricBatchLooms.Count == 0 && oFB.FabricSalesContractDetailID > 0)
                {
                    FabricSalesContractDetail oTempFabricSalesContractDetail = new FabricSalesContractDetail();
                    oFB.Texture = oTempFabricSalesContractDetail.FabricWeaveName;
                }
            }
            #endregion


            _oFabricBatchLoom.FabricBatch = oFB;
            _oFabricBatchLoom.FabricBatchProductionBeam = oFBPB;
            _oFabricBatchLoom.FabricBatchLoomDetails = oFabricBatchLoomDetails;
            _oFabricBatchLoom.FEOS = FabricExecutionOrderSpecification.Get(oFB.FEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchLoom);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult SearchByDispoNo(FabricBatchLoom oFabricBatchLoom)
        {
            _oFabricBatchLooms = new List<FabricBatchLoom>();
            try
            {
                string sSQL = "", sSQLLoom = "", sSQLPro = " ";

                if (!string.IsNullOrEmpty(oFabricBatchLoom.FEONo))
                {
                    Global.TagSQL(ref sSQLLoom);
                    //Global.TagSQL(ref sSQLPro);
                    sSQLLoom += " FEONo LIKE '%" + oFabricBatchLoom.FEONo + "%'";
                    //sSQLPro += " FEONo LIKE '%" + oFabricBatchLoom.FEONo + "%'";
                }
                if (!string.IsNullOrEmpty(oFabricBatchLoom.BeamNo))
                {
                    Global.TagSQL(ref sSQLLoom);
                    //Global.TagSQL(ref sSQLPro);
                    sSQLLoom += " BeamNo LIKE '%" + oFabricBatchLoom.BeamNo + "%'";
                    //sSQLPro += " BeamNo LIKE '%" + oFabricBatchLoom.BeamNo + "%'";
                }
                if (!string.IsNullOrEmpty(oFabricBatchLoom.MachineCode))
                {
                    Global.TagSQL(ref sSQLLoom);
                    //Global.TagSQL(ref sSQLPro);
                    sSQLLoom += " FabricMachineName LIKE '%" + oFabricBatchLoom.MachineCode + "%'";
                    //sSQLPro += " ISNULL(MachineName,'')+ISNULL(MachineCode,'') LIKE '%" + oFabricBatchLoom.MachineCode + "%'";
                }


                sSQL = "SELECT * FROM View_FabricBatchLoom ";
                _oFabricBatchLooms = FabricBatchLoom.Gets(sSQL + sSQLLoom + " ORDER BY  (CONVERT(bigint, dbo.udf_GetNumeric(Code))) ASC", ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = (!string.IsNullOrEmpty(oFabricBatchLoom.BeamNo)) ? " AND BeamNo LIKE '%" + oFabricBatchLoom.BeamNo + "%'" : "";
                List<FabricLoomPlanDetail> oFabricLoomPlanDetails = new List<FabricLoomPlanDetail>();
                List<FabricLoomPlanDetail> oTempFabricLoomPlanDetails = new List<FabricLoomPlanDetail>();
                oFabricLoomPlanDetails = FabricLoomPlanDetail.Gets("SELECT * FROM View_FabricLoomPlanDetail WHERE FLPID IN (SELECT FLPID FROM FabricLoomPlan WHERE FLPID NOT IN (SELECT ISNULL(FLPID,0) FROM FabricBatchLoom))" + sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQLPro = "SELECT * FROM View_FabricLoomPlan WHERE PlanStatus = " + (int)EnumFabricPlanStatus.Planned + " AND FLPID NOT IN (SELECT ISNULL(FLPID,0) FROM FabricBatchLoom) AND FLPID IN (SELECT DISTINCT FLPID FROM View_FabricLoomPlanDetail WHERE FLPID > 0 " + sSQL + ")";
                sSQLPro += (!string.IsNullOrEmpty(oFabricBatchLoom.FEONo)) ? " AND ExeNo LIKE '%" + oFabricBatchLoom.FEONo + "%'" : "";
                sSQLPro += (!string.IsNullOrEmpty(oFabricBatchLoom.MachineCode)) ? " AND ISNULL(MachineName,'')+ISNULL(MachineCode,'') LIKE '%" + oFabricBatchLoom.MachineCode + "%'" : "";
                List<FabricLoomPlan> oFabricLoomPlans = new List<FabricLoomPlan>();
                oFabricLoomPlans = FabricLoomPlan.Gets(sSQLPro + " ORDER BY  (CONVERT(bigint, dbo.udf_GetNumeric(MachineCode))) ASC", ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (FabricLoomPlan oItem in oFabricLoomPlans)
                {
                    _oFabricBatchLoom = new FabricBatchLoom();
                    _oFabricBatchLoom.BatchNo = oItem.BatchNo;
                    //_oFabricBatchLoom.Qty = oItem.Qty;
                    _oFabricBatchLoom.FEONo = oItem.ExeNo;
                    _oFabricBatchLoom.FBID = oItem.FBID;
                    _oFabricBatchLoom.FMID = oItem.FMID;
                    _oFabricBatchLoom.FLPID = oItem.FLPID;
                    _oFabricBatchLoom.FabricBatchStatus = oItem.Status;
                    _oFabricBatchLoom.BuyerName = oItem.BuyerName;
                    _oFabricBatchLoom.Construction = oItem.Construction;
                    _oFabricBatchLoom.TotalEnds = oItem.TotalEnds;
                    _oFabricBatchLoom.Weave = oItem.Weave;
                    _oFabricBatchLoom.FabricWeave = oItem.Weave;
                    _oFabricBatchLoom.FSCDID = oItem.FSCDID;
                    _oFabricBatchLoom.FabricMachineName = oItem.MachineName;
                    _oFabricBatchLoom.PlanType = oItem.PlanType;
                    _oFabricBatchLoom.FSpcType = oItem.FSpcType;
                    oTempFabricLoomPlanDetails = oFabricLoomPlanDetails.Where(x => x.FLPID == oItem.FLPID).ToList();
                    if (oTempFabricLoomPlanDetails.Count > 0)
                    {
                        _oFabricBatchLoom.FBPBeamID = oTempFabricLoomPlanDetails[0].FBPBeamID;
                        _oFabricBatchLoom.BeamID = oTempFabricLoomPlanDetails[0].BeamID;
                        _oFabricBatchLoom.BeamNo = string.Join(",", oTempFabricLoomPlanDetails.Select(x => x.BeamNo));
                        _oFabricBatchLoom.Qty = oTempFabricLoomPlanDetails.Sum(x => x.Qty);
                        _oFabricBatchLoom.IsDrawing = oTempFabricLoomPlanDetails[0].IsDrawing;
                    }
                    _oFabricBatchLooms.Add(_oFabricBatchLoom);
                }

            }
            catch (Exception ex)
            {
                _oFabricBatchLooms = new List<FabricBatchLoom>();
                oFabricBatchLoom.ErrorMessage = ex.Message;
                _oFabricBatchLooms.Add(oFabricBatchLoom);
            }
            var jsonResult = Json(_oFabricBatchLooms, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SearchFBPB(FabricBatchLoom oFabricBatchLoom)
        {
         
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

        [HttpPost]
        public JsonResult AdvSearchFBPB(FabricBatchLoom oFabricBatchLoom)
        {
            try
            {
                _oFabricBatchLooms = this.GetsFabricBatchProductionBeamAdv(oFabricBatchLoom);
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

        private List<FabricBatchLoom> GetsFabricBatchProductionBeamAdv(FabricBatchLoom oFabricBatchLoom)
        {
            _oFabricBatchLooms = new List<FabricBatchLoom>();
            string sSQL = "";
            string SQL = "SELECT * FROM View_FabricBatchLoom ";

            int nStartDate = Convert.ToInt32(oFabricBatchLoom.ErrorMessage.Split('~')[0]);
            DateTime dStartDateStart = Convert.ToDateTime(oFabricBatchLoom.ErrorMessage.Split('~')[1]);
            DateTime dStartDateEnd = Convert.ToDateTime(oFabricBatchLoom.ErrorMessage.Split('~')[2]);
            int nEndDate = Convert.ToInt32(oFabricBatchLoom.ErrorMessage.Split('~')[3]);
            DateTime dEndDateStart = Convert.ToDateTime(oFabricBatchLoom.ErrorMessage.Split('~')[4]);
            DateTime dEndDateEnd = Convert.ToDateTime(oFabricBatchLoom.ErrorMessage.Split('~')[5]);
            int nDropingStatus = Convert.ToInt32(oFabricBatchLoom.ErrorMessage.Split('~')[6]);
            int nLastDropingStatus = Convert.ToInt32(oFabricBatchLoom.ErrorMessage.Split('~')[7]);
            int nCboProductionDateAdv = Convert.ToInt32(oFabricBatchLoom.ErrorMessage.Split('~')[8]);
            DateTime dFromProductionDateAdv = Convert.ToDateTime(oFabricBatchLoom.ErrorMessage.Split('~')[9]);
            DateTime dToProductionDateAdv = Convert.ToDateTime(oFabricBatchLoom.ErrorMessage.Split('~')[10]);

            if (!string.IsNullOrEmpty(oFabricBatchLoom.FEONo))
            {
                Global.TagSQL(ref sSQL);
                sSQL += " FEONo LIKE '%" + oFabricBatchLoom.FEONo + "%'";
            }
            if (!string.IsNullOrEmpty(oFabricBatchLoom.BeamNo))
            {
                Global.TagSQL(ref sSQL);
                sSQL += " BeamNo LIKE '%" + oFabricBatchLoom.BeamNo + "%'";
            }
            if (!string.IsNullOrEmpty(oFabricBatchLoom.MachineCode))
            {
                Global.TagSQL(ref sSQL);
                sSQL += " FabricMachineName LIKE '%" + oFabricBatchLoom.MachineCode + "%'";
            }
            if (oFabricBatchLoom.TSUID > 0)
            {
                Global.TagSQL(ref sSQL);
                sSQL += " TSUID =" + oFabricBatchLoom.TSUID;
            }
            if (oFabricBatchLoom.ReedCount > 0)
            {
                Global.TagSQL(ref sSQL);
                sSQL += " ReedCount =" + oFabricBatchLoom.ReedCount;
            }
            if (!string.IsNullOrEmpty(oFabricBatchLoom.BuyerName))
            {
                Global.TagSQL(ref sSQL);
                sSQL += " BuyerID IN (" + oFabricBatchLoom.BuyerName + ") ";
            }
            if (nCboProductionDateAdv > 0)
            {
                //DateObject.CompareDateQuery(ref sSQL, " FabricBatchLoomID IN FinishDate", nCboProductionDateAdv, dFromProductionDateAdv, dToProductionDateAdv);
                Global.TagSQL(ref sSQL);
                if (nCboProductionDateAdv == (int)EnumCompareOperator.EqualTo)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoomDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FinishDate,106)) = " + DateObject.ConvertToVarchar(dFromProductionDateAdv) + ")";
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.NotEqualTo)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoomDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FinishDate,106)) != " + DateObject.ConvertToVarchar(dFromProductionDateAdv) + ")";
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.GreaterThan)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoomDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FinishDate,106)) > " + DateObject.ConvertToVarchar(dFromProductionDateAdv) + ")";
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.SmallerThan)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoomDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FinishDate,106)) < " + DateObject.ConvertToVarchar(dFromProductionDateAdv) + ")";
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.Between)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoomDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FinishDate,106)) BETWEEN " + DateObject.ConvertToVarchar(dFromProductionDateAdv) + " AND " + DateObject.ConvertToVarchar(dToProductionDateAdv) + ")";
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.NotBetween)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoomDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FinishDate,106)) NOT BETWEEN " + DateObject.ConvertToVarchar(dFromProductionDateAdv) + " AND " + DateObject.ConvertToVarchar(dToProductionDateAdv) + ")";
                }
            }
            if (nStartDate > 0)
            {
                DateObject.CompareDateQuery(ref sSQL, "StartTime", nStartDate, dStartDateStart, dStartDateEnd);
            }
            if (nEndDate > 0)
            {
                DateObject.CompareDateQuery(ref sSQL, "EndTime", nEndDate, dEndDateStart, dEndDateEnd);
            }
            if (nDropingStatus>0)
            {
                Global.TagSQL(ref sSQL);
                if (nDropingStatus == 1) /// No Dropping
                {
                    sSQL += " isnull(QtyPro,0)<=0";
                }
                else if (nDropingStatus == 2) /// Dropping (01-20)M
                {
                    sSQL += " [Status] <" + (int)EnumFabricBatchStatus.Finish + " and isnull(QtyPro,0)>0 and  (isnull(QtyPro,0)/ 1.0936132983)<=20";
                }
                else if (nDropingStatus == 3) /// Dropping (20-50)M
                {
                    sSQL += " [Status] <" + (int)EnumFabricBatchStatus.Finish + " and isnull(QtyPro,0)>20 and  (isnull(QtyPro,0)/ 1.0936132983)<=50";
                }
                else if (nDropingStatus == 4) /// ropping (50-100)M
                {
                    sSQL += " [Status] <" + (int)EnumFabricBatchStatus.Finish + " and isnull(QtyPro,0)>50 and  (isnull(QtyPro,0)/ 1.0936132983)<=100";
                }
                else if (nDropingStatus == 5) /// Dropping (100-500)M
                {
                    sSQL += " [Status] <" + (int)EnumFabricBatchStatus.Finish + " and isnull(QtyPro,0)>100 and  (isnull(QtyPro,0)/ 1.0936132983)<=500";
                }
                else if (nDropingStatus == 6) /// Dropping (500 More)M
                {
                    sSQL += " [Status] <" + (int)EnumFabricBatchStatus.Finish + " and isnull(QtyPro,0)>500";
                }
            }
            if (nLastDropingStatus > 0)
            {
                Global.TagSQL(ref sSQL);
                if (nLastDropingStatus == 1) /// No Dropping
                {
                    sSQL += " (isnull(Qty,0)-isnull(QtyPro,0))<=0";
                }
                else if (nLastDropingStatus == 2) /// Dropping (01-20)M
                {
                    sSQL += " [Status] <" + (int)EnumFabricBatchStatus.Finish + " and (isnull(Qty,0)-isnull(QtyPro,0))>0 and  ((isnull(Qty,0)-isnull(QtyPro,0))/ 1.0936132983)<=20";
                }
                else if (nLastDropingStatus == 3) /// Dropping (20-50)M
                {
                    sSQL += " [Status] <" + (int)EnumFabricBatchStatus.Finish + " and (isnull(Qty,0)-isnull(QtyPro,0))>20 and  ((isnull(Qty,0)-isnull(QtyPro,0))/ 1.0936132983)<=50";
                }
                else if (nLastDropingStatus == 4) /// ropping (50-100)M
                {
                    sSQL += " [Status] <" + (int)EnumFabricBatchStatus.Finish + " and (isnull(Qty,0)-isnull(QtyPro,0))>50 and  ((isnull(Qty,0)-isnull(QtyPro,0))/ 1.0936132983)<=100";
                }
                else if (nLastDropingStatus == 5) /// Dropping (100-500)M
                {
                    sSQL += " [Status] <" + (int)EnumFabricBatchStatus.Finish + " and (isnull(Qty,0)-isnull(QtyPro,0))>100 and  ((isnull(Qty,0)-isnull(QtyPro,0))/ 1.0936132983)<=500";
                }
                else if (nLastDropingStatus == 6) /// Dropping (500 More)M
                {
                    sSQL += " [Status] <" + (int)EnumFabricBatchStatus.Finish + " and (isnull(Qty,0)-isnull(QtyPro,0))>500";
                }
            }

            SQL += sSQL + " ORDER BY  (CONVERT(bigint, dbo.udf_GetNumeric(Code))) ASC";
            _oFabricBatchLooms = FabricBatchLoom.Gets(SQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            return _oFabricBatchLooms;
        }
        private List<FabricBatchLoom> GetsFabricBatchProductionBeam(FabricBatchLoom oFabricBatchLoom)
        {
            _oFabricBatchLooms = new List<FabricBatchLoom>();
            string sSQL = "";
            if (oFabricBatchLoom.Status == EnumFabricBatchStatus.Running || oFabricBatchLoom.Status == EnumFabricBatchStatus.Hold)//Loom Running
            {
                sSQL = "SELECT * FROM View_FabricBatchLoom WHERE Status = " + (int)oFabricBatchLoom.Status + " ORDER BY  (CONVERT(bigint, dbo.udf_GetNumeric(Code))) ASC";
                _oFabricBatchLooms = FabricBatchLoom.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                //List<FabricBatchProductionBeam> oFBPBs = new List<FabricBatchProductionBeam>();
                //sSQL = " SELECT * FROM View_FabricBatchProductionBeam ReadyLBeam WHERE  ReadyLBeam.FBStatus != 14  AND ReadyLBeam.IsFinish in (0,1) AND ReadyLBeam.WeavingProcessType=1 "
                //+ " AND FBPBeamID NOT IN (SELECT FBPBeamID FROM FabricBatchLoom)";
                //if (!string.IsNullOrEmpty(oFabricBatchLoom.FEONo))
                //    sSQL += " AND FEONo LIKE '%" + oFabricBatchLoom.FEONo + "%'";
                //List<FabricLoomPlan> oFabricLoomPlans = new List<FabricLoomPlan>();
                //oFabricLoomPlans = FabricLoomPlan.Gets("Select * from View_FabricLoomPlan where PlanStatus in (" + (int)EnumFabricPlanStatus.Initialize + "," + (int)EnumFabricPlanStatus.Planned + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                //oFBPBs = FabricBatchProductionBeam.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //foreach (FabricBatchProductionBeam oItem in oFBPBs)
                //{
                //    _oFabricBatchLoom = new FabricBatchLoom();
                //    _oFabricBatchLoom.BatchNo = oItem.BatchNo;
                //    _oFabricBatchLoom.Qty = oItem.Qty;
                //    _oFabricBatchLoom.FEONo = oItem.FEONo;
                //    _oFabricBatchLoom.FBID = oItem.FBID;
                //    //_oFabricBatchLoom.FLPID = oItem.FLp;
                //    _oFabricBatchLoom.BuyerName = oItem.BuyerName;
                //    _oFabricBatchLoom.Construction = oItem.Construction;
                //    _oFabricBatchLoom.BeamNo = oItem.BeamNo;
                //    _oFabricBatchLoom.TotalEnds = oItem.TotalEnds;
                //    _oFabricBatchLoom.Weave = oItem.Weave;
                //    _oFabricBatchLoom.FabricWeave = oItem.Weave;
                //    _oFabricBatchLoom.FBPBeamID = oItem.FBPBeamID;
                //    _oFabricBatchLoom.BeamID = oItem.BeamID;
                //    _oFabricBatchLoom.FSCDID = oItem.FabricSalesContractDetailID;
                //    _oFabricBatchLoom.IsDrawing = oItem.IsDrawing;
                //    if (oFabricLoomPlans.FirstOrDefault() != null && oFabricLoomPlans.FirstOrDefault().FSCDID > 0 && oFabricLoomPlans.FirstOrDefault().FMID > 0 && oFabricLoomPlans.FirstOrDefault().FBID > 0 && oFabricLoomPlans.Where(b => (b.FSCDID == oItem.FabricSalesContractDetailID && b.FBID == oItem.FBID)).Count() > 0)
                //    {
                //        _oFabricBatchLoom.FabricMachineName = oFabricLoomPlans.Where(x => x.FBID == oItem.FBID && x.FSCDID == oItem.FabricSalesContractDetailID).FirstOrDefault().MachineName;
                //        _oFabricBatchLoom.FMID = oFabricLoomPlans.Where(x => x.FBID == oItem.FBID && x.FSCDID == oItem.FabricSalesContractDetailID).FirstOrDefault().FMID;
                //        _oFabricBatchLoom.StartTime = oFabricLoomPlans.Where(x => x.FBID == oItem.FBID && x.FSCDID == oItem.FabricSalesContractDetailID).FirstOrDefault().StartTime;
                //    }
                //    else
                //    {
                //        _oFabricBatchLoom.FabricMachineName = "Wait For Plane";
                //        _oFabricBatchLoom.FMID = 0;
                //    }
                //    _oFabricBatchLooms.Add(_oFabricBatchLoom);
                //}


                List<FabricLoomPlanDetail> oFabricLoomPlanDetails = new List<FabricLoomPlanDetail>();
                List<FabricLoomPlanDetail> oTempFabricLoomPlanDetails = new List<FabricLoomPlanDetail>();
                oFabricLoomPlanDetails = FabricLoomPlanDetail.Gets("SELECT * FROM View_FabricLoomPlanDetail WHERE FLPID IN (SELECT FLPID FROM FabricLoomPlan WHERE FLPID NOT IN (SELECT ISNULL(FLPID,0) FROM FabricBatchLoom))", ((User)Session[SessionInfo.CurrentUser]).UserID);

                List<FabricLoomPlan> oFabricLoomPlans = new List<FabricLoomPlan>();
                oFabricLoomPlans = FabricLoomPlan.Gets("SELECT * FROM View_FabricLoomPlan WHERE PlanStatus = " + (int)EnumFabricPlanStatus.Planned + " AND FLPID NOT IN (SELECT ISNULL(FLPID,0) FROM FabricBatchLoom) AND FLPID IN (SELECT DISTINCT FLPID FROM FabricLoomPlanDetail) ORDER BY  (CONVERT(bigint, dbo.udf_GetNumeric(MachineCode))) ASC", ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (FabricLoomPlan oItem in oFabricLoomPlans)
                {
                    _oFabricBatchLoom = new FabricBatchLoom();
                    _oFabricBatchLoom.BatchNo = oItem.BatchNo;
                    //_oFabricBatchLoom.Qty = oItem.Qty;
                    _oFabricBatchLoom.FEONo = oItem.ExeNo;
                    _oFabricBatchLoom.FBID = oItem.FBID;
                    _oFabricBatchLoom.FMID = oItem.FMID;
                    _oFabricBatchLoom.FLPID = oItem.FLPID;
                    _oFabricBatchLoom.FabricBatchStatus = oItem.Status;
                    _oFabricBatchLoom.BuyerName = oItem.BuyerName;
                    _oFabricBatchLoom.Construction = oItem.Construction;
                    _oFabricBatchLoom.TotalEnds = oItem.TotalEnds;
                    _oFabricBatchLoom.Weave = oItem.Weave;
                    _oFabricBatchLoom.FabricWeave = oItem.Weave;
                    _oFabricBatchLoom.FSCDID = oItem.FSCDID;
                    _oFabricBatchLoom.FabricMachineName = oItem.MachineName;
                    _oFabricBatchLoom.PlanType = oItem.PlanType;
                    _oFabricBatchLoom.FSpcType = oItem.FSpcType;
                    oTempFabricLoomPlanDetails = oFabricLoomPlanDetails.Where(x => x.FLPID == oItem.FLPID).ToList();
                    if (oTempFabricLoomPlanDetails.Count > 0)
                    {
                        _oFabricBatchLoom.FBPBeamID = oTempFabricLoomPlanDetails[0].FBPBeamID;
                        _oFabricBatchLoom.BeamID = oTempFabricLoomPlanDetails[0].BeamID;
                        _oFabricBatchLoom.BeamNo = string.Join(",", oTempFabricLoomPlanDetails.Select(x => x.BeamNo));
                        _oFabricBatchLoom.Qty = oTempFabricLoomPlanDetails.Sum(x => x.Qty);
                        _oFabricBatchLoom.IsDrawing = oTempFabricLoomPlanDetails[0].IsDrawing;
                    }
                    _oFabricBatchLooms.Add(_oFabricBatchLoom);
                }
            }

            return _oFabricBatchLooms;
        }

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

        [HttpPost]
        public JsonResult Save(FabricBatchLoom oFabricBatchLoom)
        {
            try
            {
                oFabricBatchLoom = oFabricBatchLoom.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                FabricBatch oFB = new FabricBatch();
                oFabricBatchLoom.FabricBatch = oFB.Get(oFabricBatchLoom.FBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                FabricBatchProductionBeam oFabricBatchProductionBeam = new FabricBatchProductionBeam();
                oFabricBatchLoom.FabricBatchProductionBeam = oFabricBatchProductionBeam.Get(oFabricBatchLoom.FBPBeamID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricBatchLoom.FabricBatchLoomDetails = FabricBatchLoomDetail.Gets(oFabricBatchLoom.FabricBatchLoomID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricBatchLoom = new FabricBatchLoom();
                oFabricBatchLoom.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchLoom);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFabricBatchLoom(FabricBatchLoom oFBL)
        {
            string sFeedBackMessage = "";
            try
            {
                FabricBatchLoom oFabricBatchLoom = new FabricBatchLoom();
                sFeedBackMessage = oFabricBatchLoom.Delete(oFBL.FabricBatchLoomID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult SaveBatchMan(FabricBatchLoomDetail oFBPBatchMan)
        {
            FabricBatchLoomDetail _oFBPBatchMan = new FabricBatchLoomDetail();
            try
            {
                _oFBPBatchMan = oFBPBatchMan.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFBPBatchMan = new FabricBatchLoomDetail();
                _oFBPBatchMan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFBPBatchMan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeleteFBLDetail(int id)
        {
            string sFeedBackMessage = "";
            FabricBatchLoomDetail oFabricBatchLoomDetail = new FabricBatchLoomDetail();
            try
            {
                sFeedBackMessage = oFabricBatchLoomDetail.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult UpdateWeaving(FabricBatchLoom oFabricBatchLoom)
        {
            try
            {
                oFabricBatchLoom = oFabricBatchLoom.UpdateWeaving(((User)Session[SessionInfo.CurrentUser]).UserID);
                FabricBatch oFB = new FabricBatch();
                oFabricBatchLoom.FabricBatch = oFB.Get(oFabricBatchLoom.FBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                FabricBatchProductionBeam oFabricBatchProductionBeam = new FabricBatchProductionBeam();
                oFabricBatchLoom.FabricBatchProductionBeam = oFabricBatchProductionBeam.Get(oFabricBatchLoom.FBPBeamID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricBatchLoom.FabricBatchLoomDetails = FabricBatchLoomDetail.Gets(oFabricBatchLoom.FabricBatchLoomID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricBatchLoom = new FabricBatchLoom();
                oFabricBatchLoom.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchLoom);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintRptReadyBeamInStock(double TotalEnds, int nBUID)
        {
            _oFabricBatchLooms = new List<FabricBatchLoom>();
            List<FabricBatchLoom> oFBLs = new List<FabricBatchLoom>();
            string sSQL = "SELECT * FROM View_FabricBatchLoom WHERE Status = 1  ORDER BY ContractorID, FSCDID, FEONo, FMID, FabricMachineName"; //AND (ISNULL(BeamQty,0) - ISNULL(QtyPro,0)) > 0
            _oFabricBatchLooms = FabricBatchLoom.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<TextileSubUnit> oTextileSubUnits = new List<TextileSubUnit>();
            sSQL = "Select * from View_TextileSubUnit Where ISNULL(InactiveBy,0)=0";
            oTextileSubUnits = TextileSubUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (nBUID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }
            if (_oFabricBatchLooms.Any())
            {
                //oFBLs = FabricBatchLoom.Gets("SELECT * FROM View_FabricBatchLoom WHERE Status = 3", ((User)Session[SessionInfo.CurrentUser]).UserID);
                //int nStopLooms = oFBLs.Count();
                List<FabricMachine> oFabricMachines = new List<FabricMachine>();
                oFabricMachines = FabricMachine.Gets("Select * from View_FabricMachine Where FMID<>0 AND IsBeam = 0 AND WeavingProcess=3 AND TSUID<>0", ((User)Session[SessionInfo.CurrentUser]).UserID);
                byte[] abytes;
                rptReadyBeamInStockBatchLoom oReport = new rptReadyBeamInStockBatchLoom();
                abytes = oReport.PrepareReport(_oFabricBatchLooms, TotalEnds, oCompany, oTextileSubUnits, oFabricMachines);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }            
        }

        [HttpPost]
        public ActionResult SetFabricBatchLoomData(FabricBatchLoom oFabricBatchLoom)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oFabricBatchLoom);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        private string GetString(FabricBatchLoom oFabricBatchLoom)
        {
            _oFabricBatchLooms = new List<FabricBatchLoom>();
            string sSQL = "";
            string SQL = "SELECT * FROM View_FabricBatchLoomDetail ";
            int nStartDate = Convert.ToInt32(oFabricBatchLoom.ErrorMessage.Split('~')[0]);
            DateTime dStartDateStart = Convert.ToDateTime(oFabricBatchLoom.ErrorMessage.Split('~')[1]);
            DateTime dStartDateEnd = Convert.ToDateTime(oFabricBatchLoom.ErrorMessage.Split('~')[2]);
            int nEndDate = Convert.ToInt32(oFabricBatchLoom.ErrorMessage.Split('~')[3]);
            DateTime dEndDateStart = Convert.ToDateTime(oFabricBatchLoom.ErrorMessage.Split('~')[4]);
            DateTime dEndDateEnd = Convert.ToDateTime(oFabricBatchLoom.ErrorMessage.Split('~')[5]);
            int nDropingStatus = Convert.ToInt32(oFabricBatchLoom.ErrorMessage.Split('~')[6]);
            int nLastDropingStatus = Convert.ToInt32(oFabricBatchLoom.ErrorMessage.Split('~')[7]);
            int nCboProductionDateAdv = Convert.ToInt32(oFabricBatchLoom.ErrorMessage.Split('~')[8]);
            DateTime dFromProductionDateAdv = Convert.ToDateTime(oFabricBatchLoom.ErrorMessage.Split('~')[9]);
            DateTime dToProductionDateAdv = Convert.ToDateTime(oFabricBatchLoom.ErrorMessage.Split('~')[10]);

            if (!string.IsNullOrEmpty(oFabricBatchLoom.FEONo))
            {
                Global.TagSQL(ref sSQL);
                sSQL += " FEONo LIKE '%" + oFabricBatchLoom.FEONo + "%'";
            }
            if (!string.IsNullOrEmpty(oFabricBatchLoom.BeamNo))
            {
                Global.TagSQL(ref sSQL);
                sSQL += " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM View_FabricBatchLoom WHERE BeamNo LIKE '%" + oFabricBatchLoom.BeamNo + "%') ";
            }
            if (!string.IsNullOrEmpty(oFabricBatchLoom.MachineCode))
            {
                Global.TagSQL(ref sSQL);
                sSQL += " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM View_FabricBatchLoom WHERE MachineCode LIKE '%" + oFabricBatchLoom.MachineCode + "%') ";
            }
            if (oFabricBatchLoom.TSUID > 0)
            {
                Global.TagSQL(ref sSQL);
                sSQL += " TSUID =" + oFabricBatchLoom.TSUID;
            }
            if (oFabricBatchLoom.ReedCount > 0)
            {
                Global.TagSQL(ref sSQL);
                sSQL += " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM View_FabricBatchLoom WHERE ReedCount = " + oFabricBatchLoom.ReedCount + ") ";
            }
            if (!string.IsNullOrEmpty(oFabricBatchLoom.BuyerName))
            {
                Global.TagSQL(ref sSQL);
                sSQL += " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM View_FabricBatchLoom WHERE BuyerName LIKE '%" + oFabricBatchLoom.BuyerName + "%') ";
            }
            if (nCboProductionDateAdv > 0)
            {
                //DateObject.CompareDateQuery(ref sSQL, " FabricBatchLoomID IN FinishDate", nCboProductionDateAdv, dFromProductionDateAdv, dToProductionDateAdv);
                Global.TagSQL(ref sSQL);
                if (nCboProductionDateAdv == (int)EnumCompareOperator.EqualTo)
                {
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),FinishDate,106)) = " + DateObject.ConvertToVarchar(dFromProductionDateAdv);
                    _sDateMsg = "Equal: " + dFromProductionDateAdv.ToString("dd MMM yyyy");
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.NotEqualTo)
                {
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),FinishDate,106)) != " + DateObject.ConvertToVarchar(dFromProductionDateAdv);
                    _sDateMsg = "Not Equal: " + dFromProductionDateAdv.ToString("dd MMM yyyy");
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.GreaterThan)
                {
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),FinishDate,106)) > " + DateObject.ConvertToVarchar(dFromProductionDateAdv);
                    _sDateMsg = "Greater Than: " + dFromProductionDateAdv.ToString("dd MMM yyyy");
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.SmallerThan)
                {
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),FinishDate,106)) < " + DateObject.ConvertToVarchar(dFromProductionDateAdv);
                    _sDateMsg = "Smaller Than: " + dFromProductionDateAdv.ToString("dd MMM yyyy");
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.Between)
                {
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),FinishDate,106)) BETWEEN " + DateObject.ConvertToVarchar(dFromProductionDateAdv) + " AND " + DateObject.ConvertToVarchar(dToProductionDateAdv);
                    _sDateMsg = "Between: " + dFromProductionDateAdv.ToString("dd MMM yyyy") + "  To  " + dToProductionDateAdv.ToString("dd MMM yyyy");
                }
                else if (nCboProductionDateAdv == (int)EnumCompareOperator.NotBetween)
                {
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),FinishDate,106)) NOT BETWEEN " + DateObject.ConvertToVarchar(dFromProductionDateAdv) + " AND " + DateObject.ConvertToVarchar(dToProductionDateAdv);
                    _sDateMsg = "Not Between: " + dFromProductionDateAdv.ToString("dd MMM yyyy") + "  To  " + dToProductionDateAdv.ToString("dd MMM yyyy");
                }
            }
            if (nStartDate > 0)
            {
                //DateObject.CompareDateQuery(ref sSQL, "StartTime", nStartDate, dStartDateStart, dStartDateEnd);
                Global.TagSQL(ref sSQL);
                if (nStartDate == (int)EnumCompareOperator.EqualTo)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoom WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartTime,106)) = " + DateObject.ConvertToVarchar(dStartDateStart) + " )";
                }
                else if (nStartDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoom WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartTime,106)) != " + DateObject.ConvertToVarchar(dStartDateStart) + " )";
                }
                else if (nStartDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoom WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartTime,106)) > " + DateObject.ConvertToVarchar(dStartDateStart) + " )";
                }
                else if (nStartDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoom WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartTime,106)) < " + DateObject.ConvertToVarchar(dStartDateStart) + " )";
                }
                else if (nStartDate == (int)EnumCompareOperator.Between)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoom WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartTime,106)) BETWEEN " + DateObject.ConvertToVarchar(dStartDateStart) + " AND " + DateObject.ConvertToVarchar(dStartDateEnd) + " )";
                }
                else if (nStartDate == (int)EnumCompareOperator.NotBetween)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoom WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartTime,106)) NOT BETWEEN " + DateObject.ConvertToVarchar(dStartDateStart) + " AND " + DateObject.ConvertToVarchar(dStartDateEnd) + " )";
                }
            }
            if (nEndDate > 0)
            {
                //DateObject.CompareDateQuery(ref sSQL, "EndTime", nEndDate, dEndDateStart, dEndDateEnd);
                Global.TagSQL(ref sSQL);
                if (nEndDate == (int)EnumCompareOperator.EqualTo)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoom WHERE CONVERT(DATE,CONVERT(VARCHAR(12),EndTime,106)) = " + DateObject.ConvertToVarchar(dEndDateStart) + " )";
                }
                else if (nEndDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoom WHERE CONVERT(DATE,CONVERT(VARCHAR(12),EndTime,106)) != " + DateObject.ConvertToVarchar(dEndDateStart) + " )";
                }
                else if (nEndDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoom WHERE CONVERT(DATE,CONVERT(VARCHAR(12),EndTime,106)) > " + DateObject.ConvertToVarchar(dEndDateStart) + " )";
                }
                else if (nEndDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoom WHERE CONVERT(DATE,CONVERT(VARCHAR(12),EndTime,106)) < " + DateObject.ConvertToVarchar(dEndDateStart) + " )";
                }
                else if (nEndDate == (int)EnumCompareOperator.Between)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoom WHERE CONVERT(DATE,CONVERT(VARCHAR(12),EndTime,106)) BETWEEN " + DateObject.ConvertToVarchar(dEndDateStart) + " AND " + DateObject.ConvertToVarchar(dEndDateEnd) + " )";
                }
                else if (nEndDate == (int)EnumCompareOperator.NotBetween)
                {
                    sSQL = sSQL + " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM FabricBatchLoom WHERE CONVERT(DATE,CONVERT(VARCHAR(12),EndTime,106)) NOT BETWEEN " + DateObject.ConvertToVarchar(dEndDateStart) + " AND " + DateObject.ConvertToVarchar(dEndDateEnd) + " )";
                }
            }
            if (nDropingStatus > 0)
            {
                Global.TagSQL(ref sSQL);
                if (nDropingStatus == 1) /// No Dropping
                {
                    sSQL += " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM View_FabricBatchLoom WHERE isnull(QtyPro,0)<=0)";
                }
                else if (nDropingStatus == 2) /// Dropping (01-20)M
                {
                    sSQL += " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM View_FabricBatchLoom WHERE [Status] <" + (int)EnumFabricBatchStatus.Finish + " and isnull(QtyPro,0)>0 and  (isnull(QtyPro,0)/ 1.0936132983)<=20)";
                }
                else if (nDropingStatus == 3) /// Dropping (20-50)M
                {
                    sSQL += " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM View_FabricBatchLoom WHERE [Status] <" + (int)EnumFabricBatchStatus.Finish + " and isnull(QtyPro,0)>20 and  (isnull(QtyPro,0)/ 1.0936132983)<=50)";
                }
                else if (nDropingStatus == 4) /// ropping (50-100)M
                {
                    sSQL += " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM View_FabricBatchLoom WHERE [Status] <" + (int)EnumFabricBatchStatus.Finish + " and isnull(QtyPro,0)>50 and  (isnull(QtyPro,0)/ 1.0936132983)<=100)";
                }
                else if (nDropingStatus == 5) /// Dropping (100-500)M
                {
                    sSQL += " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM View_FabricBatchLoom WHERE [Status] <" + (int)EnumFabricBatchStatus.Finish + " and isnull(QtyPro,0)>100 and  (isnull(QtyPro,0)/ 1.0936132983)<=500)";
                }
                else if (nDropingStatus == 6) /// Dropping (500 More)M
                {
                    sSQL += " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM View_FabricBatchLoom WHERE [Status] <" + (int)EnumFabricBatchStatus.Finish + " and isnull(QtyPro,0)>500)";
                }
            }
            if (nLastDropingStatus > 0)
            {
                Global.TagSQL(ref sSQL);
                if (nLastDropingStatus == 1) /// No Dropping
                {
                    sSQL += " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM View_FabricBatchLoom WHERE (isnull(Qty,0)-isnull(QtyPro,0))<=0)";
                }
                else if (nLastDropingStatus == 2) /// Dropping (01-20)M
                {
                    sSQL += " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM View_FabricBatchLoom WHERE [Status] <" + (int)EnumFabricBatchStatus.Finish + " and (isnull(Qty,0)-isnull(QtyPro,0))>0 and  ((isnull(Qty,0)-isnull(QtyPro,0))/ 1.0936132983)<=20)";
                }
                else if (nLastDropingStatus == 3) /// Dropping (20-50)M
                {
                    sSQL += " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM View_FabricBatchLoom WHERE [Status] <" + (int)EnumFabricBatchStatus.Finish + " and (isnull(Qty,0)-isnull(QtyPro,0))>20 and  ((isnull(Qty,0)-isnull(QtyPro,0))/ 1.0936132983)<=50)";
                }
                else if (nLastDropingStatus == 4) /// ropping (50-100)M
                {
                    sSQL += " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM View_FabricBatchLoom WHERE [Status] <" + (int)EnumFabricBatchStatus.Finish + " and (isnull(Qty,0)-isnull(QtyPro,0))>50 and  ((isnull(Qty,0)-isnull(QtyPro,0))/ 1.0936132983)<=100)";
                }
                else if (nLastDropingStatus == 5) /// Dropping (100-500)M
                {
                    sSQL += " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM View_FabricBatchLoom WHERE [Status] <" + (int)EnumFabricBatchStatus.Finish + " and (isnull(Qty,0)-isnull(QtyPro,0))>100 and  ((isnull(Qty,0)-isnull(QtyPro,0))/ 1.0936132983)<=500)";
                }
                else if (nLastDropingStatus == 6) /// Dropping (500 More)M
                {
                    sSQL += " FabricBatchLoomID IN (SELECT FabricBatchLoomID FROM View_FabricBatchLoom WHERE [Status] <" + (int)EnumFabricBatchStatus.Finish + " and (isnull(Qty,0)-isnull(QtyPro,0))>500)";
                }
            }

            SQL += sSQL;

            return SQL;
        }
        public ActionResult PrintShiftWiseReport(int nBUID)
        {
            _oFabricBatchLooms = new List<FabricBatchLoom>();
            List<FabricBatchLoomDetail> oFabricBatchLoomDetails = new List<FabricBatchLoomDetail>();
            FabricBatchLoom oFBL = new FabricBatchLoom();
            oFBL = (FabricBatchLoom)Session[SessionInfo.ParamObj];
            //string sSQL = "SELECT * FROM View_FabricBatchLoom WHERE CONVERT(Date,Convert(Varchar(12), DBServerDateTime,106)) = CONVERT(Date,Convert(Varchar(12), GETDATE(),106)) ORDER BY FMID, FabricMachineName";
            //_oFabricBatchLooms = FabricBatchLoom.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sql = this.GetString(oFBL);
            //oFabricBatchLoomDetails = FabricBatchLoomDetail.GetsBySql("SELECT * FROM View_FabricBatchLoomDetail WHERE FabricBatchLoomID IN ("+sql+") AND ShiftID IN (SELECT RSShiftID FROM RSShift WHERE ModuleType=" + (int)EnumModuleName.LoomExecution + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oFabricBatchLooms = FabricBatchLoom.Gets("SELECT * FROM View_FabricBatchLoom WHERE FabricBatchLoomID IN (" + string.Join(",", oFabricBatchLoomDetails.Select(y=>y.FabricBatchLoomID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            oFabricBatchLoomDetails = FabricBatchLoomDetail.GetsBySql(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oFabricBatchLooms = FabricBatchLoom.Gets("SELECT * FROM View_FabricBatchLoom WHERE FabricBatchLoomID IN (" + string.Join(",", oFabricBatchLoomDetails.Select(y => y.FabricBatchLoomID)) + ") ORDER BY  (CONVERT(bigint, dbo.udf_GetNumeric(Code))), FMID ASC", ((User)Session[SessionInfo.CurrentUser]).UserID);

            List <RSShift> oRSShifts = RSShift.Gets("SELECT * FROM RSShift WHERE ModuleType = " + (int)EnumModuleName.LoomExecution, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (nBUID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (_oFabricBatchLooms.Any() && oRSShifts.Any())
            {
                byte[] abytes;
                rptPrintShiftWiseReport oReport = new rptPrintShiftWiseReport();
                abytes = oReport.PrepareReport(_oFabricBatchLooms, oFabricBatchLoomDetails,oRSShifts, oCompany, _sDateMsg);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }
        }

        public void PrintShiftWiseReportXL(int nBUID)
        {
            _oFabricBatchLooms = new List<FabricBatchLoom>();
            List<FabricBatchLoomDetail> oFabricBatchLoomDetails = new List<FabricBatchLoomDetail>();
            FabricBatchLoom oFBL = new FabricBatchLoom();
            List<RSShift> oRSShifts = new List<RSShift>();
            Company oCompany = new Company();
            try
            {
                oFBL = (FabricBatchLoom)Session[SessionInfo.ParamObj];
                string sql = this.GetString(oFBL);
                oFabricBatchLoomDetails = FabricBatchLoomDetail.GetsBySql(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricBatchLooms = FabricBatchLoom.Gets("SELECT * FROM View_FabricBatchLoom WHERE FabricBatchLoomID IN (" + string.Join(",", oFabricBatchLoomDetails.Select(y => y.FabricBatchLoomID)) + ") ORDER BY  (CONVERT(bigint, dbo.udf_GetNumeric(Code))), FMID ASC", ((User)Session[SessionInfo.CurrentUser]).UserID);
                oRSShifts = RSShift.Gets("SELECT * FROM RSShift WHERE ModuleType = " + (int)EnumModuleName.LoomExecution, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                if (nBUID > 0)
                {
                    BusinessUnit oBusinessUnit = new BusinessUnit();
                    oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                }
            }
            catch
            {
                _oFabricBatchLooms = new List<FabricBatchLoom>();
                oFabricBatchLoomDetails = new List<FabricBatchLoomDetail>();
            }

            if (_oFabricBatchLooms.Count > 0)
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();

                table_header.Add(new TableHeader { Header = "Date", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Shift", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Loom No", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Order No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer", Width = 22f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Yarn Type", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Yarn Ratio", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Fab. Type", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Design", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Construction", Width = 18f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "RPM", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Eff%", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Warp BK", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Weft BK", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Production (M)", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Loom Production (M)", Width = 18f, IsRotate = false });
                #endregion


                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Shift Wise Production Report");
                    sheet.Name = "Shift_Wise_Production_Report";

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
                    cell.Value = "Shift Wise Production Report"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 1;
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = _sDateMsg; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 2;

                    #region Column Header
                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    #endregion
                    nRowIndex++;

                    #region Data
                    double nGrandTotalProduction = 0;
                    var data = _oFabricBatchLooms.GroupBy(x => new { x.FMID, x.MachineCode }, (key, grp) => new     //.OrderBy(x => x.MachineCode).ThenBy(y => y.FMID)
                    {
                        FMID = key.FMID,
                        MachineCode = key.MachineCode,
                        Results = grp.ToList()
                    });
                    List<FabricBatchLoomDetail> oTempFabricBatchLoomDetails = new List<FabricBatchLoomDetail>();
                    FabricBatchLoomDetail oTempFabricBatchLoomDetail = new FabricBatchLoomDetail();
                    foreach (var oData in data)
                    {
                        ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                        foreach (var oItem in oData.Results)
                        {
                            double nTotalProduction = 0;
                            oTempFabricBatchLoomDetails = oFabricBatchLoomDetails.Where(x => x.FabricBatchLoomID == oItem.FabricBatchLoomID).OrderBy(y => y.FabricBatchLoomID).ToList();
                            if (oTempFabricBatchLoomDetails.Count > 0)
                            {
                                foreach (RSShift oShift in oRSShifts)
                                {
                                    oTempFabricBatchLoomDetail = oTempFabricBatchLoomDetails.Where(x => x.ShiftID == oShift.RSShiftID && x.FabricBatchLoomID == oItem.FabricBatchLoomID).FirstOrDefault();
                                    if (oTempFabricBatchLoomDetail != null)
                                        nTotalProduction += oTempFabricBatchLoomDetail.QtyInM;
                                }
                                nGrandTotalProduction += nTotalProduction;
                                for (int k = 0; k < oRSShifts.Count; k++)
                                {
                                    oTempFabricBatchLoomDetail = oTempFabricBatchLoomDetails.Where(x => x.ShiftID == oRSShifts[k].RSShiftID && x.FabricBatchLoomID == oItem.FabricBatchLoomID).FirstOrDefault();
                                    nStartCol = 2;
                                    if (oTempFabricBatchLoomDetail != null)
                                    {
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oTempFabricBatchLoomDetail.FinishDateInString, false, ExcelHorizontalAlignment.Center, false, false);
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRSShifts[k].Name, false, ExcelHorizontalAlignment.Left, false, false);
                                        if (k == 0)
                                            ExcelTool.FillCellMerge(ref sheet, oItem.MachineCode, nRowIndex, nRowIndex + (oRSShifts.Count()-1), nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, false);
                                        else nStartCol++;
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.FEONo, false, ExcelHorizontalAlignment.Left, false, false);
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.BuyerName, false, ExcelHorizontalAlignment.Left, false, false);
                                        if (k == 0)
                                            ExcelTool.FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex + (oRSShifts.Count()-1), nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, false);
                                        else nStartCol++;
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oTempFabricBatchLoomDetail.FabricType, false, ExcelHorizontalAlignment.Left, false, false);
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.FabricWeave, false, ExcelHorizontalAlignment.Left, false, false);
                                        if (k == 0)
                                            ExcelTool.FillCellMerge(ref sheet, oItem.Construction, nRowIndex, nRowIndex + (oRSShifts.Count() - 1), nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, false);
                                        else nStartCol++;
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oTempFabricBatchLoomDetail.RPM.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oTempFabricBatchLoomDetail.Efficiency.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oTempFabricBatchLoomDetail.Warp.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oTempFabricBatchLoomDetail.Weft.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oTempFabricBatchLoomDetail.QtyInM.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                                        if (k == 0)
                                            ExcelTool.FillCellMergeForNumber(ref sheet, nTotalProduction, nRowIndex, nRowIndex + (oRSShifts.Count() - 1), nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                        else nStartCol++;
                                    }
                                    else
                                    {
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Center, false, false);
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRSShifts[k].Name, false, ExcelHorizontalAlignment.Left, false, false);
                                        if (k == 0)
                                            ExcelTool.FillCellMerge(ref sheet, oItem.MachineCode, nRowIndex, nRowIndex + (oRSShifts.Count() - 1), nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, false);
                                        else
                                            nStartCol++;
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Center, false, false);
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Center, false, false);
                                        if (k == 0)
                                            ExcelTool.FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex + (oRSShifts.Count() - 1), nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, false);
                                        else
                                            nStartCol++;
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Center, false, false);
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Center, false, false);
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Center, false, false);
                                        if (k == 0)
                                            ExcelTool.FillCellMerge(ref sheet, oItem.Construction, nRowIndex, nRowIndex + (oRSShifts.Count() - 1), nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, false);
                                        else nStartCol++;
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Center, false, false);
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Center, false, false);
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Center, false, false);
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Center, false, false);
                                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Center, false, false);
                                        if (k == 0)
                                            ExcelTool.FillCellMergeForNumber(ref sheet, nTotalProduction, nRowIndex, nRowIndex + (oRSShifts.Count() - 1), nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                        else nStartCol++;
                                    }
                                    
                                    nRowIndex++;
                                }
                            }
                        }
                    }
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Total  ", nRowIndex, nRowIndex, nStartCol, nStartCol+=14, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, nGrandTotalProduction.ToString(), true, ExcelHorizontalAlignment.Right, true, false);

                    cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion



                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Shift_Wise_Production_Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }

        }

        #region Breakage
        [HttpPost]
        public JsonResult GetsTotalBreakageInformation(FabricBatchLoomDetail oFBLD)
        {
            FabricBatchProductionBreakage oFabricBatchProductionBreakage = new FabricBatchProductionBreakage();
            List<FabricBatchProductionBreakage> oFabricBatchProductionBreakages = new List<FabricBatchProductionBreakage>();
            try
            {
                if (oFBLD.FBLDetailID > 0)
                {
                    oFabricBatchProductionBreakages = FabricBatchProductionBreakage.Gets(oFBLD.FBLDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult SaveFBLBreakage(FabricBatchProductionBreakage oFBPBreakage)
        {
            FabricBatchProductionBreakage _oFBPBreakage = new FabricBatchProductionBreakage();
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
        #endregion

    }
}