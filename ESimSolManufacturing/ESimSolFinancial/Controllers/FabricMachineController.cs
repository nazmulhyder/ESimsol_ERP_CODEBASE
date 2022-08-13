using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
 

namespace ESimSolFinancial.Controllers
{
    public class FabricMachineController : Controller
    {
        #region Declaration
        FabricMachine _oFabricMachine = new FabricMachine();
        List<FabricMachine> _oFabricMachines = new List<FabricMachine>();
        FabricBreakage _oFabricBreakage = new FabricBreakage();
        List<FabricBreakage> _oFabricBreakages = new List<FabricBreakage>();
        #endregion

        public ActionResult ViewFabricMachines(int menuid) 
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFabricMachines = new List<FabricMachine>();
            _oFabricMachines = FabricMachine.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.WeavingProcess = Enum.GetValues(typeof(EnumWeavingProcess)).Cast<EnumWeavingProcess>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MachineStatus = Enum.GetValues(typeof(EnumMachineStatus)).Cast<EnumMachineStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            Dictionary<string, bool> hasPermisson = new Dictionary<string, bool>();
            hasPermisson.Add("MakeFree", MakeMahineFree());
          
            string sSQL = "Select * from View_TextileSubUnit Where ISNULL(InactiveBy,0)=0";
            List<TextileSubUnit> oTextileSubUnits = new List<TextileSubUnit>();
            oTextileSubUnits = TextileSubUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.TextileSubUnits = oTextileSubUnits;
            //List<AuthorizationUserOEDO> oAUOEDOs = new List<AuthorizationUserOEDO>();
            //sSQL = "SELECT * FROM View_AuthorizationUserOEDO Where DBObjectName='FabricMachine' And UserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "";
            //oAUOEDOs = AuthorizationUserOEDO.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //hasPermisson.Add("Add", ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Add, "FabricMachine", oAUOEDOs));
            //hasPermisson.Add("Edit", ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Edit, "FabricMachine", oAUOEDOs));
            ViewBag.Permisson = hasPermisson;
            ViewBag.FabricMachineTypes = FabricMachineType.Gets("SELECT HH.*, (SELECT Name FROM FabricMachineType AS FF WHERE FF.FabricMachineTypeID = HH.ParentID) AS ParentName FROM FabricMachineType AS HH WHERE HH.ParentID NOT IN (0,1)", ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oFabricMachines);
        }

        public ActionResult ViewFabricMachine(int id, double ts)
        {
            _oFabricMachine = new FabricMachine();
            if (id > 0)
            {
                _oFabricMachine = _oFabricMachine.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.FabricMachineTypes = FabricMachineType.Gets("SELECT HH.*, (SELECT Name FROM FabricMachineType AS FF WHERE FF.FabricMachineTypeID = HH.ParentID) AS ParentName FROM FabricMachineType AS HH WHERE HH.ParentID NOT IN (0,1)", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricMachineGroups = FabricMachineGroup.Gets("SELECT * FROM View_FabricMachineGroup", ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oFabricMachine);
        }

        [HttpPost]
        public JsonResult Save(FabricMachine oFabricMachine)
        {
            _oFabricMachine = new FabricMachine();
            try
            {
                _oFabricMachine = oFabricMachine.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricMachine = new FabricMachine();
                _oFabricMachine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricMachine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                FabricMachine oFabricMachine = new FabricMachine();
                sFeedBackMessage = oFabricMachine.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult ActiveInActive(int id, bool IsActive)
        {
            FabricMachine oFabricMachine = new FabricMachine();
            try
            {
                
                if(IsActive)
                {
                    oFabricMachine = oFabricMachine.ActiveInActive(id, false);
                }
                else
                {
                    oFabricMachine = oFabricMachine.ActiveInActive(id, true);
                }
                
            }
            catch (Exception ex)
            {
                oFabricMachine = new FabricMachine();
                oFabricMachine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricMachine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult MachineSearch(string Params)
        {
            _oFabricMachines = new List<FabricMachine>();
            try
            {
                bool bIsNameWise = Convert.ToBoolean( Params.Split('~')[0]);
                string sNameCode = Params.Split('~')[1];
                string sSQL = "";
                if (bIsNameWise)
                {
                    if (!string.IsNullOrEmpty(sNameCode))
                    {
                        sSQL = "SELECT * FROM View_FabricMachine WHERE Name Like '%" + sNameCode + "%'  OR Code Like '%" + sNameCode + "%'";
                        _oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                        
                }
                else
                {
                    sSQL = "SELECT * FROM View_FabricMachine WHERE WeavingProcess = " + Convert.ToInt32(Params.Split('~')[1]);
                    _oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                

            }
            catch (Exception ex)
            {
                _oFabricMachine = new FabricMachine();
                _oFabricMachine.ErrorMessage = ex.Message;
                _oFabricMachines.Add(_oFabricMachine);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult LoomMachineGetLastProductionAndBeamStatus(int FMID)
        //{
        //    List<FabricBatchProduction> oFBPs= new List<FabricBatchProduction>();
          
        //     _oFabricMachines = new List<FabricMachine>();
        //     string sSQL = string.Empty;
            
        //    try
        //    {
        //         if(FMID>0)
        //         {
        //             sSQL = "SELECT top(1)* FROM View_FabricBatchProduction WHERE FMID= " + FMID + "ORDER BY FBPID DESC";
        //             oFBPs = FabricBatchProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //             if (oFBPs.Any())
        //             {
                        
        //                     sSQL = "SELECT * FROM View_FabricMachine WHERE IsBeam =1 AND FMID IN (  SELECT top(1) BeamID FROM FabricBatchProductionBeam WHERE FBPID IN ( " + oFBPs.FirstOrDefault().FBPID + ") ORDER BY FBPID DESC)";
        //                     _oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //             }


        //         }



        //    }
        //    catch (Exception ex)
        //    {
        //        _oFabricMachine = new FabricMachine();
        //        _oFabricMachine.ErrorMessage = ex.Message;
        //        _oFabricMachines.Add(_oFabricMachine);
        //    }
        //    return Json(new { FabricBatchProduction = oFBPs,  FabricMachine = _oFabricMachines }, JsonRequestBehavior.AllowGet);
        //    //JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    //string sjson = serializer.Serialize(_oFabricMachines);
        //    //return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult LoomMachineRestore(FabricMachine oFabricMachine)
        {
            _oFabricMachine = new FabricMachine();
            try
            {
                _oFabricMachine = oFabricMachine.LoomMachineRestore(oFabricMachine.FMID);
            }
            catch (Exception ex)
            {
                _oFabricMachine = new FabricMachine();
                _oFabricMachine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricMachine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult HoldBeamFinishForLoomProcess(FabricMachine oFabricMachine)
        {
            _oFabricMachine = new FabricMachine();
            try
            {
                _oFabricMachine = oFabricMachine.HoldBeamFinishForLoomProcess(oFabricMachine.FMID);
            }
            catch (Exception ex)
            {
                _oFabricMachine = new FabricMachine();
                _oFabricMachine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricMachine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMachines(int nWeavingProcess, int nMachineStatus)
        {
            _oFabricMachines = new List<FabricMachine>();
            try
            {
                _oFabricMachines = FabricMachine.Gets(false, (EnumWeavingProcess)nWeavingProcess, (EnumMachineStatus)nMachineStatus, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricMachine = new FabricMachine();
                _oFabricMachine.ErrorMessage = ex.Message;
                _oFabricMachines.Add(_oFabricMachine);
                
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMachineByID(int nMachine)
        {
            FabricMachine oFabricMachine = new FabricMachine();
            try
            {
                oFabricMachine = oFabricMachine.Get(nMachine, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricMachine = new FabricMachine();
                oFabricMachine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricMachine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsMachine(FabricMachine oFabricMachine)
        {
            _oFabricMachines = new List<FabricMachine>();
            try
            {
                _oFabricMachines = FabricMachine.Gets(true, oFabricMachine.WeavingProcess, oFabricMachine.MachineStatus, ((User)Session[SessionInfo.CurrentUser]).UserID);
             
            }
            catch (Exception ex)
            {
                _oFabricMachine = new FabricMachine();
                _oFabricMachine.ErrorMessage = ex.Message;
                _oFabricMachines.Add(_oFabricMachine);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsOnluMachine(FabricMachine oFabricMachine)
        {
            _oFabricMachines = new List<FabricMachine>();
            string sSQL = "";
            try
            {
                //AND WeavingProcess = " + (int)EnumWeavingProcess.Warping
                sSQL = "SELECT * FROM View_FabricMachine where  IsBeam=0 AND IsActive = 1";
                string sReturn = " ";
                #region MachineName
                if (oFabricMachine.Name != null && oFabricMachine.Name != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Name LIKE '%" + oFabricMachine.Name + "%'";
                }
                #endregion
                #region Type
                if (!string.IsNullOrEmpty(oFabricMachine.ErrorMessage))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WeavingProcess in ( " + oFabricMachine.ErrorMessage+")";
                }
                #endregion

                sSQL = sSQL + "" + sReturn + "  Order By Code";

                _oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricMachine = new FabricMachine();
                _oFabricMachine.ErrorMessage = ex.Message;
                _oFabricMachines.Add(_oFabricMachine);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SearchMachine(FabricMachine oFabricMachine)
        {
            _oFabricMachines = new List<FabricMachine>();
            try
            {
                int nProcessType = Convert.ToInt32(oFabricMachine.Params.Split('~')[0]);
                int nMachineStatus = Convert.ToInt32(oFabricMachine.Params.Split('~')[1]);
                bool bIsBeam = Convert.ToBoolean(oFabricMachine.Params.Split('~')[2]);
                bool bIsActive = Convert.ToBoolean(oFabricMachine.Params.Split('~')[3]);
                double nRPMFrom = Convert.ToDouble(oFabricMachine.Params.Split('~')[4]);
                double nRPMTo = Convert.ToDouble(oFabricMachine.Params.Split('~')[5]);
                int nTSUID = Convert.ToInt32(oFabricMachine.Params.Split('~')[6]);

                string sSQL = "Select * from View_FabricMachine Where FMID<>0 AND IsActive = "+((bIsActive)?1:0)+" AND IsBeam = "+((bIsBeam)?1:0)+"";

                if (nProcessType > -1)
                    sSQL += "AND WeavingProcess="+nProcessType+"";

                if(nMachineStatus>0)
                    sSQL += "AND MachineStatus="+nMachineStatus+"";

                if (nRPMFrom != 0 && nRPMTo != 0 && nRPMFrom < nRPMTo)
                    sSQL += " AND RPM BETWEEN " + nRPMFrom + " AND " + nRPMTo + "";

                if (nTSUID > 0)
                    sSQL += " AND TSUID=" + nTSUID + "";

                //if (oFabricMachine.ParentMachineTypeID > 0)
                //    sSQL += " AND ParentMachineTypeID =" + oFabricMachine.ParentMachineTypeID + " ";

                if (oFabricMachine.ChildMachineTypeID > 0)
                    sSQL += " AND ChildMachineTypeID =" + oFabricMachine.ChildMachineTypeID + " ";

                _oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricMachine = new FabricMachine();
                _oFabricMachine.ErrorMessage = ex.Message;
                _oFabricMachines.Add(_oFabricMachine);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMachineByNameCode(FabricMachine oFabricMachine)
        {
            _oFabricMachines = new List<FabricMachine>();
            try
            {
                string sSQL = "Select * from View_FabricMachine Where FMID<>0";

                if (!string.IsNullOrEmpty(oFabricMachine.Name) && oFabricMachine.Name.Trim() != "")
                    sSQL += " And Name Like '%" + oFabricMachine.Name.Trim() + "%'" + " OR Code Like '%" + oFabricMachine.Name.Trim() + "%'";
                _oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricMachine = new FabricMachine();
                _oFabricMachine.ErrorMessage = ex.Message;
                _oFabricMachines.Add(_oFabricMachine);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBeamByNameCode(FabricMachine oFabricMachine)
        {
            _oFabricMachines = new List<FabricMachine>();
            try
            {
                string sSQL = "Select * from View_FabricMachine Where FMID<>0 AND IsBeam=0 AND IsActive = 1 ";
                if (!string.IsNullOrEmpty(oFabricMachine.Name) && oFabricMachine.Name.Trim() != "")
                    sSQL += " And Name Like '%" + oFabricMachine.Name.Trim() + "%'" + " OR Code Like '%" + oFabricMachine.Name.Trim() + "%'";
                _oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricMachine = new FabricMachine();
                _oFabricMachine.ErrorMessage = ex.Message;
                _oFabricMachines.Add(_oFabricMachine);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Fabric Breakage
        public ActionResult ViewFabricBreakages(int menuid) 
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFabricBreakages = new List<FabricBreakage>();
            _oFabricBreakages = FabricBreakage.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oFabricBreakages);
        }

        [HttpPost]
        public JsonResult SaveFabricBreakage(FabricBreakage oFabricBreakage)
        {
            _oFabricBreakage = new FabricBreakage();
            try
            {
                _oFabricBreakage = oFabricBreakage.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBreakage = new FabricBreakage();
                _oFabricBreakage.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBreakage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeleteFabricBreakage(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                FabricBreakage oFabricBreakage = new FabricBreakage();
                sFeedBackMessage = oFabricBreakage.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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

        //#region Machine DashBoard
        //public ActionResult View_FabricMachineDashboard(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);

        //    string sSQL = "SELECT * FROM View_FabricMachine WHERE IsBeam = 0 ORDER BY FMID";
        //    _oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    _oFabricMachines = this.GetsMachinesWithProductionInfo(_oFabricMachines);
        //    FabricMachineDashboard oFMD = new FabricMachineDashboard();

        //    if (_oFabricMachines.Count > 0)
        //    {
        //        #region Warping
        //        oFMD.WarpingTotal = _oFabricMachines.Where(x => x.WeavingProcess == EnumWeavingProcess.Warping).Count();
        //        oFMD.WarpingPlanning = _oFabricMachines.Where(x => x.WeavingProcess == EnumWeavingProcess.Warping && x.MachineStatus == EnumMachineStatus.Planning).Count();
        //        oFMD.WarpingRunning = _oFabricMachines.Where(x => x.WeavingProcess == EnumWeavingProcess.Warping && x.MachineStatus == EnumMachineStatus.Running).Count();
        //        oFMD.WarpingBreak = _oFabricMachines.Where(x => x.WeavingProcess == EnumWeavingProcess.Warping && x.MachineStatus == EnumMachineStatus.Break).Count();
        //        oFMD.WarpingFree = _oFabricMachines.Where(x => x.WeavingProcess == EnumWeavingProcess.Warping && x.MachineStatus == EnumMachineStatus.Free).Count();
        //        #endregion

        //        #region Sizing
        //        oFMD.SizingTotal = _oFabricMachines.Where(x => x.WeavingProcess == EnumWeavingProcess.Sizing).Count();
        //        oFMD.SizingPlanning = _oFabricMachines.Where(x => x.WeavingProcess == EnumWeavingProcess.Sizing && x.MachineStatus == EnumMachineStatus.Planning).Count();
        //        oFMD.SizingRunning = _oFabricMachines.Where(x => x.WeavingProcess == EnumWeavingProcess.Sizing && x.MachineStatus == EnumMachineStatus.Running).Count();
        //        oFMD.SizingBreak = _oFabricMachines.Where(x => x.WeavingProcess == EnumWeavingProcess.Sizing && x.MachineStatus == EnumMachineStatus.Break).Count();
        //        oFMD.SizingFree = _oFabricMachines.Where(x => x.WeavingProcess == EnumWeavingProcess.Sizing && x.MachineStatus == EnumMachineStatus.Free).Count();
        //        #endregion

        //        #region Loom
        //        oFMD.LoomTotal = _oFabricMachines.Where(x => x.WeavingProcess == EnumWeavingProcess.Loom).Count();
        //        oFMD.LoomPlanning = _oFabricMachines.Where(x => x.WeavingProcess == EnumWeavingProcess.Loom && x.MachineStatus == EnumMachineStatus.Planning).Count();
        //        oFMD.LoomRunning = _oFabricMachines.Where(x => x.WeavingProcess == EnumWeavingProcess.Loom && x.MachineStatus == EnumMachineStatus.Running).Count();
        //        oFMD.LoomBreak = _oFabricMachines.Where(x => x.WeavingProcess == EnumWeavingProcess.Loom && x.MachineStatus == EnumMachineStatus.Break).Count();
        //        oFMD.LoomFree = _oFabricMachines.Where(x => x.WeavingProcess == EnumWeavingProcess.Loom && x.MachineStatus == EnumMachineStatus.Free).Count();
        //        #endregion
        //    }
         
        //    ViewBag.FMD = oFMD;
        //    return View(_oFabricMachines);
        //}
        //[HttpPost]
        //public JsonResult GetsStatusWise(FabricMachine oFabricMachine)
        //{
        //    _oFabricMachines = new List<FabricMachine>();
        //    try
        //    {
        //        string sSQL = "SELECT * FROM View_FabricMachine WHERE IsBeam = 0 AND WeavingProcess = " + (int)oFabricMachine.WeavingProcess;
        //        if (oFabricMachine.RPM != -1) //When not click on "Total"
        //        {
        //            sSQL = sSQL + " AND MachineStatus = " + (int)oFabricMachine.MachineStatus + " ";
        //        }
        //        _oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        _oFabricMachines = this.GetsMachinesWithProductionInfo(_oFabricMachines);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oFabricMachine = new FabricMachine();
        //        _oFabricMachine.ErrorMessage = ex.Message;
        //        _oFabricMachines.Add(_oFabricMachine);

        //    }
        //    var jsonResult = Json(_oFabricMachines, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}
        ////private List<FabricMachine> GetsMachinesWithProductionInfo(List<FabricMachine> oFabricMachines)
        ////{
        ////    if (oFabricMachines.Count > 0)
        ////    {
        ////        string sFMIDs = string.Join(",", oFabricMachines.Select(o => o.FMID).Distinct());
        ////        if (!string.IsNullOrEmpty(sFMIDs))
        ////        {
        ////            string sSQL = "SELECT * FROM View_FabricBatchProduction WHERE FMID IN (" + sFMIDs + ") AND FMID > 0 AND FBPID IN (SELECT MAX(FBPID) FROM FabricBatchProduction GROUP BY FMID)";
        ////            List<FabricBatchProduction> oFBPs = new List<FabricBatchProduction>();
        ////            oFBPs = FabricBatchProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

        ////            if (oFBPs.Count > 0)
        ////            {
        ////                List<FabricBatchProduction> oTempFBPs = new List<FabricBatchProduction>();
        ////                foreach (FabricMachine oItem in oFabricMachines)
        ////                {
        ////                    oTempFBPs = new List<FabricBatchProduction>();
        ////                    oTempFBPs = oFBPs.Where(o => o.FMID == oItem.FMID).ToList();
        ////                    if (oTempFBPs.Count > 0)
        ////                    {
        ////                        oItem.BatchNo = (!string.IsNullOrEmpty(oTempFBPs[0].BatchNo) ? oTempFBPs[0].BatchNo : "N/A");
        ////                        oItem.OrderNo = (!string.IsNullOrEmpty(oTempFBPs[0].OrderNo) ? oTempFBPs[0].OrderNo : "N/A");
        ////                        oItem.Duration = (!string.IsNullOrEmpty(oTempFBPs[0].BatchDuration) ? oTempFBPs[0].BatchDuration : "N/A");
        ////                        oItem.Duration.Replace("01 Jan 0001 00:00", " Running");
        ////                    }
        ////                    else
        ////                    {
        ////                        oItem.BatchNo = "N/A";
        ////                        oItem.OrderNo = "N/A";
        ////                        oItem.Duration = "N/A";
        ////                    }
        ////                }
        ////            }
        ////            else
        ////            {
        ////                oFabricMachines.ForEach(o =>
        ////                {
        ////                    o.BatchNo = "N/A";
        ////                    o.OrderNo = "N/A";
        ////                    o.Duration = "N/A";
        ////                });
        ////            }
        ////        }
        ////    }
        ////    oFabricMachines.Where(o => o.MachineStatus == EnumMachineStatus.Free).ToList().ForEach(o =>
        ////    {
        ////        o.BatchNo = "N/A";
        ////        o.OrderNo = "N/A";
        ////        o.Duration = "N/A";
        ////    });
        ////    return oFabricMachines;
        ////}
        //#endregion

        #region Make Machine Free

        bool MakeMahineFree()
        {
            //List<AuthorizationUserOEDO> oAUOEDOs = new List<AuthorizationUserOEDO>();

            //string sSQL = "SELECT * FROM View_AuthorizationUserOEDO Where DBObjectName='FabricMachine' And OEValue=" + (int)EnumOperationFunctionality._View + " And UserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "";
            //oAUOEDOs = AuthorizationUserOEDO.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //return (oAUOEDOs.Count() > 0 && oAUOEDOs.FirstOrDefault().AUOEDOID > 0) ? true : false ;

            return true;
        }

        [HttpPost]
        public JsonResult MakeFree(FabricMachine oFabricMachine)
        {
            try
            {
                if (oFabricMachine.FMID <= 0)
                    throw new Exception("Invalid machine");

                if (!MakeMahineFree())
                    throw new Exception("You are not permitted user to make it free.");

                oFabricMachine.MachineStatus = EnumMachineStatus.Free;
                oFabricMachine = oFabricMachine.MakeFree(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricMachine = new FabricMachine();
                oFabricMachine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricMachine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        
        #endregion

        #region Gets Loom Machine By Textile SU Unit

        [HttpPost]
        public JsonResult GetsLoomMachineByTextileSubUnit(TextileSubUnit oTSU)
        {
            _oFabricMachines = new List<FabricMachine>();
            try
            {

                string sSQL = "SELECT * FROM View_FabricMachine WHERE IsBeam=0 AND WeavingProcess = " + (int)EnumWeavingProcess.Loom + " AND MachineStatus = " + (int)EnumMachineStatus.Free + ""
                             + " And FMID In (Select FMID from TextileSubUnitMachine Where TSUID = " + oTSU.TSUID + ")";

                _oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricMachines = new List<FabricMachine>();
                _oFabricMachines.Add(new FabricMachine { ErrorMessage=ex.Message });

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        [HttpPost]
        public ActionResult SetFabricMachineData(FabricMachine oFabricMachine)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oFabricMachine);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintList()
        {
            _oFabricMachine = new FabricMachine();
            _oFabricMachines = new List<FabricMachine>();
            int nProcessType = -1;
            try
            {
                _oFabricMachine = (FabricMachine)Session[SessionInfo.ParamObj];
                nProcessType = Convert.ToInt32(_oFabricMachine.Params.Split('~')[0]);
                int nMachineStatus = Convert.ToInt32(_oFabricMachine.Params.Split('~')[1]);
                bool bIsBeam = Convert.ToBoolean(_oFabricMachine.Params.Split('~')[2]);
                bool bIsActive = Convert.ToBoolean(_oFabricMachine.Params.Split('~')[3]);
                double nRPMFrom = Convert.ToDouble(_oFabricMachine.Params.Split('~')[4]);
                double nRPMTo = Convert.ToDouble(_oFabricMachine.Params.Split('~')[5]);
                int nTSUID = Convert.ToInt32(_oFabricMachine.Params.Split('~')[6]);

                string sSQL = "Select WeavingProcess, TSUID, TextileSubUnitName, ParentMachineTypeID, ParentMachineTypeName, ChildMachineTypeID, ChildMachineTypeName, FabricMachineGroupID, FabricMachineGroupName, Capacity, COUNT(*) AS RPM from View_FabricMachine Where FMID<>0 AND IsActive = " + ((bIsActive) ? 1 : 0) + " AND IsBeam = " + ((bIsBeam) ? 1 : 0) + "";

                if (nProcessType > -1)
                    sSQL += "AND WeavingProcess=" + nProcessType + "";

                if (nMachineStatus > 0)
                    sSQL += "AND MachineStatus=" + nMachineStatus + "";

                if (nRPMFrom != 0 && nRPMTo != 0 && nRPMFrom < nRPMTo)
                    sSQL += " AND RPM BETWEEN " + nRPMFrom + " AND " + nRPMTo + "";

                if (nTSUID > 0)
                    sSQL += " AND TSUID=" + nTSUID + "";

                //if (_oFabricMachine.ParentMachineTypeID > 0)
                //    sSQL += " AND ParentMachineTypeID =" + _oFabricMachine.ParentMachineTypeID + " ";

                if (_oFabricMachine.ChildMachineTypeID > 0)
                    sSQL += " AND ChildMachineTypeID =" + _oFabricMachine.ChildMachineTypeID + " ";

                sSQL += " GROUP BY WeavingProcess, TSUID, TextileSubUnitName, ParentMachineTypeID, ParentMachineTypeName, ChildMachineTypeID, ChildMachineTypeName, FabricMachineGroupID, FabricMachineGroupName, Capacity ORDER BY WeavingProcess, TSUID, TextileSubUnitName, ParentMachineTypeID, ParentMachineTypeName, ChildMachineTypeID, ChildMachineTypeName, FabricMachineGroupID, FabricMachineGroupName, Capacity ";

                _oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricMachine = new FabricMachine();
                _oFabricMachines = new List<FabricMachine>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            //_oFabricMachine.Company = oCompany;

            string SQL = "Select * from View_TextileSubUnit Where ISNULL(InactiveBy,0)=0";
            List<TextileSubUnit> oTextileSubUnits = new List<TextileSubUnit>();
            oTextileSubUnits = TextileSubUnit.Gets(SQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptFabricMachines oReport = new rptFabricMachines();
            byte[] abytes = oReport.PrepareReport(_oFabricMachines, oTextileSubUnits, oCompany, nProcessType);
            return File(abytes, "application/pdf");
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

        #endregion
    }
  
}