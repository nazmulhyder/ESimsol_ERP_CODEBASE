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
using System.Reflection;
using System.Net;
using OfficeOpenXml.Style;
using OfficeOpenXml;
namespace ESimSolFinancial.Controllers
{
    public class FabricBatchController : Controller
    {
        #region Declaration
        FabricBatch _oFabricBatch = new FabricBatch();
        List<FabricBatch> _oFabricBatchs = new List<FabricBatch>();
        FabricBatchQC _oFabricBatchQC = new FabricBatchQC();
        List<FabricBatchQC> _oFabricBatchQCs = new List<FabricBatchQC>();
        List<FabricBatchQCDetail> _oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
        FabricBatchQCDetail _oFabricBatchQCDetail = new FabricBatchQCDetail();
        FabricBatchQCCheck oFabricBatchQCCheck = new FabricBatchQCCheck();
        List<FabricBatchQCCheck> _oFabricBatchQCChecks = new List<FabricBatchQCCheck>();
        string _sDateRangeText = "";
        #endregion

        #region Fabric Batch
        public ActionResult ViewFabricBatchs(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string sSQL = "SELECT top(200)* FROM View_FabricBatch WHERE Status = "+(int)EnumFabricBatchState.Initialize + " ";
            _oFabricBatchs = FabricBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FBStatus = FabricBatchStateObj.Gets();
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperatorTwo));
            return View(_oFabricBatchs);
        }
        public ActionResult ViewFabricBatch(string id)
        {
            _oFabricBatch = new FabricBatch();

            int nFabricSalesContractDetailID = 0;
            int nTotalSplitedItem = id.Split('~').Length;

            #region This part used when We want to create fabric batch from fabric warping plan

            if (nTotalSplitedItem > 1) 
            {
                nFabricSalesContractDetailID = Convert.ToInt32(id.Split('~')[1]);
                int nFWPDID = Convert.ToInt32(id.Split('~')[2]); //nFWPDID = Fabric Warping Plan Detail Id
                if (nFabricSalesContractDetailID > 0)
                {
                  
                    ViewBag.FabricBatchs = FabricBatch.GetsByFabricSalesContractDetailID(nFabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
               
            }

            #endregion

            #region This part used when we want to create fabric batch directly from fabric batch menu.

            else
            {
                int nId = Convert.ToInt32(id);
                if (nId > 0)
                {
                    _oFabricBatch = _oFabricBatch.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                ViewBag.FabricBatchs = new List<FabricBatch>();
            }

            #endregion

            return View(_oFabricBatch);
        }

        public ActionResult ViewYarnOut(int id, int buid)
        {
            _oFabricBatch = new FabricBatch();
            if (id > 0)
            {
                _oFabricBatch = _oFabricBatch.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricBatch.FabricBatchRawMaterials = FabricBatchRawMaterial.Gets(id, EnumWeavingProcess.Warping, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            ViewBag.BUID = buid;
            ViewBag.Stores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricBatch, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            return View(_oFabricBatch);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = _oFabricBatch.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult SearchFB(FabricBatch oFabricBatch)
        {
            _oFabricBatchs = new List<FabricBatch>();
            try
            {
                string sFEONo = Convert.ToString(oFabricBatch.Params.Split('~')[0]);
                string sBatchNo = Convert.ToString(oFabricBatch.Params.Split('~')[1]);
                int nDateType = Convert.ToInt16(oFabricBatch.Params.Split('~')[2]);
                DateTime dIssueStartDate = Convert.ToDateTime(oFabricBatch.Params.Split('~')[3]);
                DateTime dIssueEndDate = Convert.ToDateTime(oFabricBatch.Params.Split('~')[4]);
                string sFPBStatus = oFabricBatch.Params.Split('~')[5];
                string sMachines = oFabricBatch.Params.Split('~')[6];

                string sSQL = "SELECT * FROM View_FabricBatch WHERE FBID>0";

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
                    DateObject.CompareDateQuery(ref sSQL, "IssueDate", nDateType, dIssueStartDate, dIssueEndDate);
                }
                //if (!string.IsNullOrEmpty(sFPBStatus))
                if ( !string.IsNullOrEmpty(sFPBStatus) && sFPBStatus != "-1")
                {

                    sSQL = sSQL + " AND ISNULL(Status,0) IN (" + sFPBStatus + ")";
                }
                if (!string.IsNullOrEmpty(sMachines))
                {
                    sSQL = sSQL + " AND FMID IN (" + sMachines + ")";
                }
                sSQL = sSQL + " ORDER BY BatchNo";
                _oFabricBatchs = FabricBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatch = new FabricBatch();
                _oFabricBatch.ErrorMessage = ex.Message;
                _oFabricBatchs.Add(_oFabricBatch);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateBatchNo(FabricBatch oFabricBatch)
        {
            _oFabricBatch = new FabricBatch();
            try
            {
                _oFabricBatch = oFabricBatch.UpdateBatchNo(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatch = new FabricBatch();
                _oFabricBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchByBatchNo(FabricBatch oFabricBatch)
        {
            try
            {
                int nFBPID = 0;
                bool bIsFromQcMenu = oFabricBatch.IsFromQcMenu;
                int nWeavingProcessTypeTemp = oFabricBatch.WeavingProcessTypeTemp;
                int.TryParse(oFabricBatch.Params, out nFBPID);
                if (oFabricBatch.FBID > 0)
                { oFabricBatch = oFabricBatch.Get(oFabricBatch.FBID, ((User)Session[SessionInfo.CurrentUser]).UserID); }
                else
                {
                    oFabricBatch = oFabricBatch.GetByBatchNo(oFabricBatch.BatchNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                //if (bIsFromQcMenu && !this.CheckFabricBatch(oFabricBatch))
                //{
                //    oFabricBatch = new FabricBatch();
                //}
                if (oFabricBatch.FBID > 0)
                {
                    
                    List<FabricBatchProduction> oFabricBatchProductions = new List<FabricBatchProduction>();
                    string sSQL = "SELECT * FROM View_FabricBatchProduction WHERE FBID = " + oFabricBatch.FBID + " AND WeavingProcess =" + nWeavingProcessTypeTemp;
                    oFabricBatchProductions = FabricBatchProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


                    //if(nFBPID>0)
                    //    oFabricBatch.ReedCountWithDent = (oFabricBatchProductions.Where(x => x.FBPID == nFBPID).Any()) ? oFabricBatchProductions.Where(x => x.FBPID == nFBPID).FirstOrDefault().ReedNo : "";

                    List<FabricMachine> oFabricMachines = new List<FabricMachine>();
                    oFabricMachines = FabricMachine.Gets(true, EnumWeavingProcess.Loom, EnumMachineStatus.Free, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFabricMachines = oFabricMachines.Where(x=>x.IsActive).OrderBy(x => x.Code).ToList();

                    if (oFabricMachines.Count > 0)
                    {
                        List<FabricMachine> oTempFMs = new List<FabricMachine>();
                        foreach (FabricMachine oItem in oFabricMachines)
                        {
                            oTempFMs = new List<FabricMachine>();
                            oTempFMs = oFabricBatch.Beams.Where(o => o.FMID == oItem.FMID).ToList();
                            if (oTempFMs.Count == 0)
                            {
                                oFabricBatch.Beams.Add(oItem);
                            }
                        }
                    }
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
        public JsonResult SearchBatchNoByFBP(FabricBatch oFabricBatch)
        {
            try
            {
                int nFBPID = 0, nFabricBatchLoomID = 0;
                bool bIsFromQcMenu = oFabricBatch.IsFromQcMenu;
                int nWeavingProcessTypeTemp = oFabricBatch.WeavingProcessTypeTemp;
                //int.TryParse(oFabricBatch.Params, out nFBPID);
                int.TryParse(oFabricBatch.Params, out nFabricBatchLoomID);
                string sSQL = string.Empty;
                //oFabricBatch = oFabricBatch.GetByBatchNo(oFabricBatch.BatchNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //FabricBatchProduction oFabricBatchProduction = new FabricBatchProduction();                
                //if (nFBPID>0)
                //    oFabricBatchProduction = oFabricBatchProduction.Get(nFBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //if (oFabricBatchProduction.FBID>0)
                //{
                //    oFabricBatch = oFabricBatch.Get(oFabricBatchProduction.FBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //}
                FabricBatchLoom oFabricBatchLoom = new FabricBatchLoom();
                if (nFabricBatchLoomID > 0)
                    oFabricBatchLoom = oFabricBatchLoom.Get(nFabricBatchLoomID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFabricBatch.FBID > 0)
                {
                    oFabricBatch = oFabricBatch.Get(oFabricBatch.FBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                oFabricBatch.ReedCountWithDent = oFabricBatch.ReedCountWithDent;
                if (oFabricBatch.FBID > 0)
                {
                    List<FabricMachine> oFabricMachines = new List<FabricMachine>();
                   // oFabricMachines = FabricMachine.Gets(true, EnumWeavingProcess.Loom, EnumMachineStatus.Free, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFabricMachines = oFabricMachines.Where(x => x.IsActive).OrderBy(x => x.Code).ToList();
                    oFabricBatch.Beams = new List<FabricMachine>();
                    if (oFabricMachines.Count > 0)
                    {
                        List<FabricMachine> oTempFMs = new List<FabricMachine>();
                        foreach (FabricMachine oItem in oFabricMachines)
                        {
                            oTempFMs = new List<FabricMachine>();
                            oTempFMs = oFabricBatch.Beams.Where(o => o.FMID == oItem.FMID).ToList();
                            if (oTempFMs.Count == 0)
                            {
                                oFabricBatch.Beams.Add(oItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oFabricBatch = new FabricBatch();
                oFabricBatch.ErrorMessage = ex.Message;
            }
            //var jsonResult = Json(oFabricBatch, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetsByBatchNo(FabricBatch oFabricBatch)
        {
            try
            {
                _oFabricBatchs = new List<FabricBatch>();
                string sReturn1 = "SELECT * FROM View_FabricBatch ";
                string sReturn = "";
                if (!string.IsNullOrEmpty(oFabricBatch.BatchNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BatchNo Like '%" + oFabricBatch.BatchNo + "%'";
                }

                if (oFabricBatch.WeavingProcessType == (int)EnumWeavingProcess.Sizing)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " [Status] IN (" + (int)EnumFabricBatchState.warping_Finish + ", " + (int)EnumFabricBatchState.Sizing + ") ";
                }
                else if (oFabricBatch.WeavingProcessType == (int)EnumWeavingProcess.Drawing_IN)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " [Status] IN (" + (int)EnumFabricBatchState.Sizing_Finish + ", " + (int)EnumFabricBatchState.DrawingIn + ") ";
                }
                else if (oFabricBatch.WeavingProcessType == (int)EnumWeavingProcess.Loom)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " [Status] IN (" + (int)EnumFabricBatchState.Sizing_Finish + "," + (int)EnumFabricBatchState.DrawingIn_Finish + ", " + (int)EnumFabricBatchState.Weaving + ") ";
                }
                else if (oFabricBatch.WeavingProcessType == -1) // -1 = FabricBatchInQC
                {
                    
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " [Status] IN (" + (int)EnumFabricBatchState.Sizing_Finish + "," + (int)EnumFabricBatchState.Weaving_Finish + ", " + (int)EnumFabricBatchState.In_QC + ") ";
                    sReturn = sReturn + " [Status] IN  (" + (int)EnumFabricBatchState.Sizing_Finish + "," + (int)EnumFabricBatchState.DrawingIn_Finish + "," + (int)EnumFabricBatchState.Weaving_Finish + "," + (int)EnumFabricBatchState.In_QC + "," + (int)EnumFabricBatchState.InStore + ")";
                }

                string sSQL = sReturn1 + " " + sReturn + " ORDER BY BatchNo";
                _oFabricBatchs = FabricBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatch = new FabricBatch();
                _oFabricBatch.ErrorMessage = ex.Message;
                _oFabricBatchs.Add(_oFabricBatch);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save(FabricBatch oFabricBatch)
        {
            try
            {
                oFabricBatch = oFabricBatch.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Finish(FabricBatch oFabricBatch)
        {
            try
            {
                oFabricBatch = oFabricBatch.Finish(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult BatchFinish(FabricBatch oFabricBatch)
        {
            try
            {
                oFabricBatch = oFabricBatch.BatchFinish(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetsByFabricSalesContractDetailID(FabricBatch oFabricBatch)
        {
            _oFabricBatchs = new List<FabricBatch>();
            _oFabricBatch = new FabricBatch();
            try
            {
                string sSQL = "SELECT * FROM View_FabricBatch WHERE FabricSalesContractDetailID = " + oFabricBatch.FabricSalesContractDetailID + " ORDER BY BatchNo";
                _oFabricBatchs = FabricBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFabricBatch.FBID <= 0)
                {
                    _oFabricBatch.Qty = _oFabricBatchs.Sum(t => t.Qty);
                }
                else
                {
                    _oFabricBatch.Qty = _oFabricBatchs.Where(x => x.FBID != oFabricBatch.FBID).ToList().Sum(t => t.Qty);
                }
            }
            catch (Exception ex)
            {
                _oFabricBatch = new FabricBatch();
                _oFabricBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFabricBatch(FabricBatch oFabricBatch)
        {
            _oFabricBatch = new FabricBatch();
            try
            {
                _oFabricBatch = _oFabricBatch.Get(oFabricBatch.FBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //_oFabricBatch.FabricSalesContractDetail = FabricSalesContractDetail.Get(oFabricBatch.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatch = new FabricBatch();
                _oFabricBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsFabricBatches(FabricBatch oFabricBatch)
        {
            _oFabricBatchs = new List<FabricBatch>();
            try
            {
                string sSQL = "SELECT * FROM View_FabricBatch WHERE FBID <> 0";
                if (oFabricBatch.BatchNo != null || oFabricBatch.BatchNo != "")
                {
                    sSQL += " And BatchNo Like '%" + oFabricBatch.BatchNo+"%'";
                }
                if (oFabricBatch.FabricSalesContractDetailID > 0)
                {
                    sSQL += " And FabricSalesContractDetailID =" + oFabricBatch.FabricSalesContractDetailID + "";
                }
                sSQL = sSQL + " ORDER BY BatchNo";
                _oFabricBatchs = FabricBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatch = new FabricBatch();
                _oFabricBatch.ErrorMessage = ex.Message;
                _oFabricBatchs.Add(_oFabricBatch);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
        
        #region Fabric Batch Row Material
        //[HttpPost]
        //public JsonResult GetsFabricSalesContractDetailYarnReceive(FabricBatch oFabricBatch)
        //{
        //    List<FabricSalesContractDetailYarnReceive> oFEOYRs = new List<FabricSalesContractDetailYarnReceive>();
        //    try
        //    {
        //        if (oFabricBatch.FabricSalesContractDetailID > 0)
        //        {
        //            List<FabricSalesContractDetailYarnReceive> oTempFEOYRs = new List<FabricSalesContractDetailYarnReceive>();
        //            string sSQL = "SELECT * FROM View_FabricSalesContractDetailYarnReceive WHERE FabricSalesContractDetailID=" + oFabricBatch.FabricSalesContractDetailID;
        //            oTempFEOYRs = FabricSalesContractDetailYarnReceive.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //            if (oTempFEOYRs.Count > 0)
        //            {
        //                string[] splitLotIDs = string.Join(",", oTempFEOYRs.Select(o => o.LotID).Distinct()).Split(',');
        //                List<FabricSalesContractDetailYarnReceive> oTemps = new List<FabricSalesContractDetailYarnReceive>();
        //                FabricSalesContractDetailYarnReceive oFEOYR = new FabricSalesContractDetailYarnReceive();
        //                foreach (string sLotID in splitLotIDs)
        //                {
        //                    oTemps = new List<FabricSalesContractDetailYarnReceive>();

        //                    int nLotID = Convert.ToInt32(sLotID);
        //                    oTemps = oTempFEOYRs.Where(o => o.LotID == nLotID).ToList();

        //                    if (oTemps.Count > 0)
        //                    {
        //                        oFEOYR = new FabricSalesContractDetailYarnReceive();
        //                        oFEOYR = this.SetFEOYR(oTemps);
        //                        oFEOYRs.Add(oFEOYR);
        //                    }
        //                }
        //                oFEOYRs.ForEach(o => o.Qty = Math.Round(o.Qty, 2));
        //                oFEOYRs.ForEach(o => o.LotBalance = Math.Round(o.LotBalance, 2));
        //            }
 
        //            sSQL = "Select * from View_FabricYarnOrderAllocate Where FYOID In (Select FYOID from FabricYarnOrder Where FabricSalesContractDetailID=" + oFabricBatch.FabricSalesContractDetailID + ")";
        //            List<FabricYarnOrderAllocate> oFYOAs = new List<FabricYarnOrderAllocate>();
        //            oFYOAs = FabricYarnOrderAllocate.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //            if(oFYOAs.Any() && oFYOAs.First().FYOAID>0)
        //            {
        //                FabricSalesContractDetailYarnReceive oFEOYR = new FabricSalesContractDetailYarnReceive();

        //                List<FabricBatchRawMaterial> oFabricBatchRawMaterials = new List<FabricBatchRawMaterial>();

        //                sSQL = "Select * from View_FabricBatchRawMaterial Where LotID In (" + string.Join(",", oFYOAs.Select(x => x.LotID).Distinct().ToList()) + ")";
        //                oFabricBatchRawMaterials = FabricBatchRawMaterial.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //                var oFBRMByLot = oFabricBatchRawMaterials.GroupBy(x => x.LotID).Select(grp => new
        //                {
        //                    LotID = grp.Key,
        //                    Qty = grp.Sum(x=>x.Qty)
        //                });

        //                var oFYOAByLots = oFYOAs.GroupBy(x => x.LotID).Select(grp => new FabricSalesContractDetailYarnReceive
        //                {
        //                    LotID = grp.Key,
        //                    LotNo = grp.FirstOrDefault().LotNo,
        //                    Qty = grp.Sum(x => x.Qty) - ((oFBRMByLot.Where(x => x.LotID == grp.Key).Any())? oFBRMByLot.Where(x => x.LotID == grp.Key).First().Qty : 0),
        //                    LotBalance = grp.FirstOrDefault().Balance - grp.Sum(x => x.Qty),
        //                    WUID = grp.FirstOrDefault().WUID,
        //                    UnitName= grp.FirstOrDefault().OperationUnitName,
        //                    LocationName = grp.FirstOrDefault().LocationName,
        //                    ProductID = grp.FirstOrDefault().ProductID,
        //                    ProductName = grp.FirstOrDefault().ProductName,
        //                    ProductCode = grp.FirstOrDefault().ProductCode,
        //                    IsFabricYarnOrderAllocation=true
        //                }).Where(x=>x.Qty>0).ToList();

        //                oFEOYRs.AddRange(oFYOAByLots);
                       
        //            }


        //        }
        //        else
        //        {
        //            FabricSalesContractDetailYarnReceive oFEOYR = new FabricSalesContractDetailYarnReceive();
        //            oFEOYR.ErrorMessage = "Invalid FEO.";
        //            oFEOYRs.Add(oFEOYR);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        FabricSalesContractDetailYarnReceive oFEOYR = new FabricSalesContractDetailYarnReceive();
        //        oFEOYR.ErrorMessage = ex.Message;
        //        oFEOYRs.Add(oFEOYR);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oFEOYRs);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        //private FabricSalesContractDetailYarnReceive SetFEOYR(List<FabricSalesContractDetailYarnReceive> oFEPYRs)
        //{
        //    FabricSalesContractDetailYarnReceive oFEOYR = new FabricSalesContractDetailYarnReceive();
        //    if (oFEPYRs.Count > 0)
        //    {
        //        oFEOYR.FEOYID = 0;
        //        oFEOYR.FabricSalesContractDetailID = oFEPYRs[0].FabricSalesContractDetailID;
        //        oFEOYR.LotID = oFEPYRs[0].LotID;
        //        oFEOYR.Qty = oFEPYRs.Sum(o => o.Qty);
        //        oFEOYR.ReceiveBy = oFEPYRs[0].ReceiveBy;
        //        oFEOYR.ReceiveDate = oFEPYRs[0].ReceiveDate;
        //        oFEOYR.ChallanDetailID = oFEPYRs[0].ChallanDetailID;
        //        oFEOYR.WUID = oFEPYRs[0].WUID;
        //        oFEOYR.SUDeliveryChallanDetailID = oFEPYRs[0].SUDeliveryChallanDetailID;
        //        oFEOYR.ProductID = oFEPYRs[0].ProductID;
        //        oFEOYR.ProductName = oFEPYRs[0].ProductName;
        //        oFEOYR.ProductCode = oFEPYRs[0].ProductCode;
        //        oFEOYR.LotNo = oFEPYRs[0].LotNo;
        //        oFEOYR.LotBalance = oFEPYRs[0].LotBalance;
        //        oFEOYR.UnitPrice = oFEPYRs[0].UnitPrice;
        //        oFEOYR.UnitName = oFEPYRs[0].UnitName;
        //        oFEOYR.LocationName = oFEPYRs[0].LocationName;
        //        oFEOYR.BuyerID = oFEPYRs[0].BuyerID;
        //        oFEOYR.FEONo = oFEPYRs[0].FEONo;

        //        oFEOYR.OrderType = oFEPYRs[0].OrderType;
        //        oFEOYR.IsInHouse = oFEPYRs[0].IsInHouse;
        //        oFEOYR.BuyerName = oFEPYRs[0].BuyerName;
        //        oFEOYR.ColorName = oFEPYRs[0].ColorName;
        //        oFEOYR.Warp = oFEPYRs[0].Warp;
        //        oFEOYR.Length = oFEPYRs[0].Length;
        //        oFEOYR.HanksCone = oFEPYRs[0].HanksCone;
        //        oFEOYR.BagQty = oFEPYRs[0].BagQty;
        //    }
        //    return oFEOYR;
        //}


        //[HttpPost]
        //public JsonResult GetYarnByFEO(FabricBatch oFabricBatch)
        //{
        //    List<FEOReReceiveYarn> oFEOReReceiveYarns = new List<FEOReReceiveYarn>();
        //    try
        //    {
        //        oFEOReReceiveYarns = FEOReReceiveYarn.Gets(oFabricBatch.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oFabricBatch = new FabricBatch();
        //        _oFabricBatch.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oFEOReReceiveYarns);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        

        [HttpGet]
        public JsonResult DeleteRowMaterial(int id)//RowMaterial ID
        {
            string sFeedBackMessage = "";
            FabricBatchRawMaterial oFabricBatchRawMaterial = new FabricBatchRawMaterial();
            try
            {
                sFeedBackMessage = oFabricBatchRawMaterial.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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

        #region Fabric Batch QC Declaration
        public ActionResult ViewFabricBatchInQC(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFabricBatchQC = new FabricBatchQC();
            ViewBag.Employees = Employee.Gets(EnumEmployeeDesignationType.Incharge, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.Grades = EnumObject.jGets(typeof (EnumFBQCGrade));//  FBQCGradeObj.Gets();
            ViewBag.FabricQCGrades = FabricQCGrade.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FaultTypes = FabricFaultTypeObj.Gets();

            string sSQL = "SELECT * FROM FabricProductionFault WHERE IsActive=1 AND ISNULL(BUType,0) = " + (int)EnumBusinessUnitType.Weaving + " Order By Sequence";
            ViewBag.Faults = FabricProductionFault.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            string _SSQL = "SELECT * FROM FabricQCParName";
            ViewBag.FabricQCParNames = FabricQCParName.Gets(_SSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            return View(_oFabricBatchQC);
        }
        public ActionResult ViewFabricBatchInQCV2(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricBatchQC).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            List<FabricBatchQC> oFabricBatchQCs = new List<FabricBatchQC>();
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.Employees = Employee.Gets(EnumEmployeeDesignationType.Incharge, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricQCGrades = FabricQCGrade.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FaultTypes = FabricFaultTypeObj.Gets();
            string sSQL = "SELECT * FROM FabricProductionFault WHERE IsActive=1 AND ISNULL(BUType,0) = " + (int)EnumBusinessUnitType.Weaving + " Order By Sequence";
            ViewBag.Faults = FabricProductionFault.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string _SSQL = "SELECT * FROM FabricQCParName";
            ViewBag.FabricQCParNames = FabricQCParName.Gets(_SSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            return View(oFabricBatchQCs);
        }

        #region Fabric Batch QC Report
        public ActionResult ViewFabricBatchQCReport(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<FabricBatchQCDetail> FabricBatchQCDetails = new List<FabricBatchQCDetail>();
            _oFabricBatchQC.FabricBatchQCDetails = FabricBatchQCDetails;
            ViewBag.Shifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)).Where(x => x.id == (int)EnumCompareOperator.EqualTo || x.id == (int)EnumCompareOperator.Between);
            return View(_oFabricBatchQC);
        }

        [HttpPost]
        public JsonResult GetsQCGrades(FabricQCGrade oFabricQCGrade)
        {
            List<FabricQCGrade> oFabricQCGrades = new List<FabricQCGrade>();
            try
            {
                string sParamName = "";              
                sParamName = oFabricQCGrade.Name;             
                string sSQL = "SELECT * FROM View_FabricQCGrade WHERE FabricQCGradeID >0";
                if (sParamName != null && sParamName != "")
                {
                    sSQL += "AND Name LIKE'%" + sParamName + "%'";
                }

                oFabricQCGrades = FabricQCGrade.Gets(sSQL + " ORDER BY Name", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricQCGrades = new List<FabricQCGrade>();
                oFabricQCGrades.Add(new FabricQCGrade() { ErrorMessage = ex.Message });
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricQCGrades);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllCheckingParameter(int FabricBatchQCID)
        {
            List<FabricBatchQCCheck> oFabricBatchQCChecks = new List<FabricBatchQCCheck>();
            List<FabricQCParName> oFabricQCParNames = new List<FabricQCParName>();

            string sSQL = "";
            try
            {
                if (FabricBatchQCID > 0)
                {
                    sSQL = "SELECT * FROM View_FabricBatchQCCheck WHERE FabricBatchQCID = " + FabricBatchQCID;
                    oFabricBatchQCChecks = FabricBatchQCCheck.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (oFabricBatchQCChecks.Count == 0)
                {
                    sSQL = "SELECT * FROM View_FabricQCParName";
                    oFabricQCParNames = FabricQCParName.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);                   
                    foreach(FabricQCParName oItem in oFabricQCParNames){
                        oFabricBatchQCCheck = new FabricBatchQCCheck();
                        oFabricBatchQCCheck.Name = oItem.Name;
                        oFabricBatchQCCheck.Note = oItem.Note;
                        oFabricBatchQCCheck.FabricQCParNameID = oItem.FabricQCParNameID;
                        oFabricBatchQCChecks.Add(oFabricBatchQCCheck);
                    }
                   
                }
                
               
            }
            catch (Exception ex)
            {
                oFabricBatchQCChecks = new List<FabricBatchQCCheck>();
                oFabricBatchQCChecks.Add(new FabricBatchQCCheck() { ErrorMessage = ex.Message });
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchQCChecks);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckingParameterSave(List<FabricBatchQCCheck> oFabricBatchQCChecks)
        {
            FabricBatchQCCheck oFabricBatchQCCheck = new FabricBatchQCCheck();
            List<FabricBatchQCCheck> _CheckingParameters = new List<FabricBatchQCCheck>();
            try
            {
                _CheckingParameters = oFabricBatchQCCheck.SaveList(oFabricBatchQCChecks, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _CheckingParameters = new List<FabricBatchQCCheck>();
                oFabricBatchQCCheck = new FabricBatchQCCheck();
                oFabricBatchQCCheck.ErrorMessage = ex.Message;
                _CheckingParameters.Add(oFabricBatchQCCheck);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_CheckingParameters);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteFabricBatchQCCheck(FabricBatchQCCheck oFabricBatchQCCheck)
        {
           
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabricBatchQCCheck.Delete(oFabricBatchQCCheck.FabricBatchQCCheckID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult SearchFabricBatchQCDetail(FabricBatchQCDetail oFabricBatchQCDetail)
        {
            _oFabricBatchQCDetails = new List<FabricBatchQCDetail>();

            DateTime dProductionStartDate= DateTime.Now;
            DateTime dProductionEndDate = DateTime.Now;
            DateTime dDeliveryStartDate = DateTime.Now;
            DateTime dDeliveryEndDate = DateTime.Now;
            DateTime dStoreRcvStartDate = DateTime.Now;
            DateTime dStoreRcvEndDate = DateTime.Now;
            DateTime dIssueRcvStartDate = DateTime.Now;
            DateTime dIssueRcvEndDate  = DateTime.Now;

            string sFabricQCGradeIDs = oFabricBatchQCDetail.ErrorMessage.Split('~')[0];

            EnumCompareOperator eProductionDate = (EnumCompareOperator)Convert.ToInt32(oFabricBatchQCDetail.ErrorMessage.Split('~')[1]);
            if (eProductionDate != EnumCompareOperator.None)
            {
                 dProductionStartDate = Convert.ToDateTime(oFabricBatchQCDetail.ErrorMessage.Split('~')[2]);
                 dProductionEndDate = Convert.ToDateTime(oFabricBatchQCDetail.ErrorMessage.Split('~')[3]);
            }
            EnumCompareOperator eDeliveryDate = (EnumCompareOperator)Convert.ToInt32(oFabricBatchQCDetail.ErrorMessage.Split('~')[4]);
            if (eDeliveryDate != EnumCompareOperator.None)
            {
                 dDeliveryStartDate = Convert.ToDateTime(oFabricBatchQCDetail.ErrorMessage.Split('~')[5]);
                 dDeliveryEndDate = Convert.ToDateTime(oFabricBatchQCDetail.ErrorMessage.Split('~')[6]);
            }

            EnumCompareOperator eStoreRcvDate = (EnumCompareOperator)Convert.ToInt32(oFabricBatchQCDetail.ErrorMessage.Split('~')[7]);
            if (eStoreRcvDate != EnumCompareOperator.None)
            {
                 dStoreRcvStartDate = Convert.ToDateTime(oFabricBatchQCDetail.ErrorMessage.Split('~')[8]);
                 dStoreRcvEndDate = Convert.ToDateTime(oFabricBatchQCDetail.ErrorMessage.Split('~')[9]);
            }

            EnumCompareOperator eIssueRcvDate = (EnumCompareOperator)Convert.ToInt32(oFabricBatchQCDetail.ErrorMessage.Split('~')[10]);
            if (eIssueRcvDate != EnumCompareOperator.None)
            {
                 dIssueRcvStartDate = Convert.ToDateTime(oFabricBatchQCDetail.ErrorMessage.Split('~')[11]);
                 dIssueRcvEndDate = Convert.ToDateTime(oFabricBatchQCDetail.ErrorMessage.Split('~')[12]);
                
            }
            oFabricBatchQCDetail.ShiftID = Convert.ToInt32(oFabricBatchQCDetail.ErrorMessage.Split('~')[13]);
            try
            {              
                string sReturn1 = "SELECT * FROM View_FabricBatchQCDetail AS HH";
                string sReturn = "";

                #region FABRIC QC
                if (!String.IsNullOrEmpty(sFabricQCGradeIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " HH.FabricQCGradeID IN(" + sFabricQCGradeIDs + ")";

                }
                #endregion
                #region SHIFT
                if (oFabricBatchQCDetail.ShiftID >0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " HH.ShiftID = "+oFabricBatchQCDetail.ShiftID;

                }
                #endregion

                #region ProductionDate
                if (eProductionDate != EnumCompareOperator.None)
                {
                    if (eProductionDate == EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.ProDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dProductionStartDate.ToString("dd MMM yyyy h:mm tt") + "',106))";
                    }
                    else if (eProductionDate == EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.ProDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dProductionStartDate.ToString("dd MMM yyyy h:mm tt") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dProductionEndDate.ToString("dd MMM yyyy h:mm tt") + "',106))";
                        
                    }                 
                }
                #endregion
                #region Delivery Date
                if (eDeliveryDate != EnumCompareOperator.None)
                {
                    if (eDeliveryDate == EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.DeliveryDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dDeliveryStartDate.ToString("dd MMM yyyy h:mm tt") + "',106))";
                    }
                    else if (eDeliveryDate == EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.DeliveryDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dDeliveryStartDate.ToString("dd MMM yyyy h:mm tt") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dDeliveryEndDate.ToString("dd MMM yyyy h:mm tt") + "',106))";
                    }
                }
                #endregion
                #region Store Rcv Date
                if (eStoreRcvDate != EnumCompareOperator.None)
                {
                    if (eStoreRcvDate == EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.StoreRcvDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStoreRcvStartDate.ToString("dd MMM yyyy h:mm tt") + "',106))";
                    }
                    else if (eStoreRcvDate == EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.StoreRcvDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStoreRcvStartDate.ToString("dd MMM yyyy h:mm tt") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStoreRcvEndDate.ToString("dd MMM yyyy h:mm tt") + "',106))";
                    }
                }
                #endregion                
                #region Issue Date
                if (eIssueRcvDate != EnumCompareOperator.None)
                {
                    if (eIssueRcvDate == EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.DBServerDateTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueRcvStartDate.ToString("dd MMM yyyy h:mm tt") + "',106))";
                    }
                    else if (eIssueRcvDate == EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.DBServerDateTime,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueRcvStartDate.ToString("dd MMM yyyy h:mm tt") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueRcvEndDate.ToString("dd MMM yyyy h:mm tt") + "',106))";
                    }
                }
                #endregion                
                sReturn = sReturn1 + sReturn + " AND ISNULL(GradeSL,0) != 0 ORDER BY FBQCDetailID ASC";
                //for testing
                //sReturn = "SELECT * FROM View_FabricBatchQCDetail AS HH";
                _oFabricBatchQCDetails = FabricBatchQCDetail.Gets(sReturn, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
                _oFabricBatchQCDetail = new FabricBatchQCDetail();
                _oFabricBatchQCDetail.ErrorMessage = ex.Message;
                _oFabricBatchQCDetails.Add(_oFabricBatchQCDetail);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchQCDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetSessionSearchCriterias(FabricBatchQCDetail oFabricBatchQCDetail)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oFabricBatchQCDetail);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintLists()
        {
            _oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
            _oFabricBatchQCDetail = new FabricBatchQCDetail();
            string sSQL = ""; string sReportHeader = "";
            try
            {
                _oFabricBatchQCDetail = (FabricBatchQCDetail)Session[SessionInfo.ParamObj];
                sSQL = "SELECT * FROM View_FabricBatchQCDetail WHERE FBQCDetailID IN (" + _oFabricBatchQCDetail.ErrorMessage + ") ORDER BY FBQCDetailID";
                _oFabricBatchQCDetails = FabricBatchQCDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch(Exception ex){
                _oFabricBatchQCDetail = new FabricBatchQCDetail();
                _oFabricBatchQCDetail.ErrorMessage = ex.Message;
                _oFabricBatchQCDetails.Add(_oFabricBatchQCDetail);
            }
            if (_oFabricBatchQCDetails.Count > 0)
            {

                if (_oFabricBatchQCDetail.sSearchParams != "")
                {
                    string sString = _oFabricBatchQCDetail.sSearchParams;
                    int nIssueDate = Convert.ToInt32(sString.Split('~')[0]);
                    int nProductionDate = Convert.ToInt32(sString.Split('~')[3]);
                    int nDeliveryDate = Convert.ToInt32(sString.Split('~')[6]);
                    int nStoreDate = Convert.ToInt32(sString.Split('~')[9]);

                    #region print Header
                    if (nIssueDate > 0)
                    {
                         DateTime sIssueStartDate = Convert.ToDateTime(sString.Split('~')[1]);
                         DateTime sIssueEndDate = Convert.ToDateTime(sString.Split('~')[2]);
                        if (nIssueDate == 1)
                        {
                            if (sReportHeader == "")
                            {
                                sReportHeader = "Issue Date " + sIssueStartDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader = sReportHeader+", Issue Date " + sIssueStartDate.ToString("dd MMM yyyy");
                            }
                           
                        }

                        if (nIssueDate == 5)
                        {

                            if (sReportHeader == "")
                            {
                                sReportHeader = "Issue Date from "+sIssueStartDate.ToString("dd MMM yyyy")+" to "+sIssueEndDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader = sReportHeader+", Issue Date from"+sIssueStartDate.ToString("dd MMM yyyy")+" to "+sIssueEndDate.ToString("dd MMM yyyy");
                            }
                         
                        }
                    }
                    if (nProductionDate > 0)
                    {
                        DateTime sProductionStartDate = Convert.ToDateTime(sString.Split('~')[4]);
                        DateTime sProductionEndDate = Convert.ToDateTime(sString.Split('~')[5]);
                        if (nProductionDate == 1)
                        {
                            if (sReportHeader == "")
                            {
                                sReportHeader = "Production Date " + sProductionStartDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader = sReportHeader + ", Issue Date " + sProductionStartDate.ToString("dd MMM yyyy");
                            }

                        }

                        if (nProductionDate == 5)
                        {

                            if (sReportHeader == "")
                            {
                                sReportHeader = "Production Date  from " + sProductionStartDate.ToString("dd MMM yyyy") + " to " + sProductionEndDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader = sReportHeader + ", Production Date  from" + sProductionStartDate.ToString("dd MMM yyyy") + " to " + sProductionEndDate.ToString("dd MMM yyyy");
                            }

                        }
                    }
                    if (nDeliveryDate > 0)
                    {
                        DateTime sDeliveryStartDate = Convert.ToDateTime(sString.Split('~')[7]);
                        DateTime sDeliveryEndDate = Convert.ToDateTime(sString.Split('~')[8]);
                        if (nIssueDate == 1)
                        {
                            if (sReportHeader == "")
                            {
                                sReportHeader = "Delivery Date " + sDeliveryStartDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader = sReportHeader + ", Delivery Date " + sDeliveryStartDate.ToString("dd MMM yyyy");
                            }

                        }

                        if (nIssueDate == 5)
                        {

                            if (sReportHeader == "")
                            {
                                sReportHeader = "Delivery Date from " + sDeliveryStartDate.ToString("dd MMM yyyy") + " to " + sDeliveryEndDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader = sReportHeader + ", Delivery Date from" + sDeliveryStartDate.ToString("dd MMM yyyy") + " to " + sDeliveryEndDate.ToString("dd MMM yyyy");
                            }

                        }
                    }
                    if (nStoreDate > 0)
                    {
                        DateTime sStoreStartDate = Convert.ToDateTime(sString.Split('~')[10]);
                        DateTime sStoreEndDate = Convert.ToDateTime(sString.Split('~')[11]);
                        if (nIssueDate == 1)
                        {
                            if (sReportHeader == "")
                            {
                                sReportHeader = "Store Rcv Date " + sStoreStartDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader = sReportHeader + ", Store Rcv Date " + sStoreStartDate.ToString("dd MMM yyyy");
                            }

                        }

                        if (nIssueDate == 5)
                        {

                            if (sReportHeader == "")
                            {
                                sReportHeader = "Store Rcv Date " + sStoreStartDate.ToString("dd MMM yyyy") + " to " + sStoreEndDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader = sReportHeader + ", Store Rcv Date" + sStoreStartDate.ToString("dd MMM yyyy") + " to " + sStoreEndDate.ToString("dd MMM yyyy");
                            }

                        }
                    }
                    #endregion

                }

                Company oCompany = new Company();
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptFabricBatchQCReport oReport = new rptFabricBatchQCReport();
                byte[] abytes = oReport.PrepareReport(_oFabricBatchQCDetails, oCompany, sReportHeader);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            }         
        }
        public void FPBatchByWeavingProcessExcel()
        {
            _oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
            _oFabricBatchQCDetail = new FabricBatchQCDetail();
            string sSQL = ""; string sReportHeader = "";
            try
            {
                _oFabricBatchQCDetail = (FabricBatchQCDetail)Session[SessionInfo.ParamObj];
                sSQL = "SELECT * FROM View_FabricBatchQCDetail WHERE FBQCDetailID IN (" + _oFabricBatchQCDetail.ErrorMessage + ") ORDER BY FBQCDetailID";
                _oFabricBatchQCDetails = FabricBatchQCDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricBatchQCDetail = new FabricBatchQCDetail();
                _oFabricBatchQCDetail.ErrorMessage = ex.Message;
                _oFabricBatchQCDetails.Add(_oFabricBatchQCDetail);
            }
            if (_oFabricBatchQCDetails.Count > 0)
            {

                if (_oFabricBatchQCDetail.sSearchParams != "")
                {
                    string sString = _oFabricBatchQCDetail.sSearchParams;
                    int nIssueDate = Convert.ToInt32(sString.Split('~')[0]);
                    int nProductionDate = Convert.ToInt32(sString.Split('~')[3]);
                    int nDeliveryDate = Convert.ToInt32(sString.Split('~')[6]);
                    int nStoreDate = Convert.ToInt32(sString.Split('~')[9]);

                    #region print Header
                    if (nIssueDate > 0)
                    {
                        DateTime sIssueStartDate = Convert.ToDateTime(sString.Split('~')[1]);
                        DateTime sIssueEndDate = Convert.ToDateTime(sString.Split('~')[2]);
                        if (nIssueDate == 1)
                        {
                            if (sReportHeader == "")
                            {
                                sReportHeader = "Issue Date " + sIssueStartDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader = sReportHeader + ", Issue Date " + sIssueStartDate.ToString("dd MMM yyyy");
                            }

                        }

                        if (nIssueDate == 5)
                        {

                            if (sReportHeader == "")
                            {
                                sReportHeader = "Issue Date from " + sIssueStartDate.ToString("dd MMM yyyy") + " to " + sIssueEndDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader = sReportHeader + ", Issue Date from" + sIssueStartDate.ToString("dd MMM yyyy") + " to " + sIssueEndDate.ToString("dd MMM yyyy");
                            }

                        }
                    }
                    if (nProductionDate > 0)
                    {
                        DateTime sProductionStartDate = Convert.ToDateTime(sString.Split('~')[4]);
                        DateTime sProductionEndDate = Convert.ToDateTime(sString.Split('~')[5]);
                        if (nProductionDate == 1)
                        {
                            if (sReportHeader == "")
                            {
                                sReportHeader = "Production Date " + sProductionStartDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader = sReportHeader + ", Issue Date " + sProductionStartDate.ToString("dd MMM yyyy");
                            }

                        }

                        if (nProductionDate == 5)
                        {

                            if (sReportHeader == "")
                            {
                                sReportHeader = "Production Date  from " + sProductionStartDate.ToString("dd MMM yyyy") + " to " + sProductionEndDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader = sReportHeader + ", Production Date  from" + sProductionStartDate.ToString("dd MMM yyyy") + " to " + sProductionEndDate.ToString("dd MMM yyyy");
                            }

                        }
                    }
                    if (nDeliveryDate > 0)
                    {
                        DateTime sDeliveryStartDate = Convert.ToDateTime(sString.Split('~')[7]);
                        DateTime sDeliveryEndDate = Convert.ToDateTime(sString.Split('~')[8]);
                        if (nIssueDate == 1)
                        {
                            if (sReportHeader == "")
                            {
                                sReportHeader = "Delivery Date " + sDeliveryStartDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader = sReportHeader + ", Delivery Date " + sDeliveryStartDate.ToString("dd MMM yyyy");
                            }

                        }

                        if (nIssueDate == 5)
                        {

                            if (sReportHeader == "")
                            {
                                sReportHeader = "Delivery Date from " + sDeliveryStartDate.ToString("dd MMM yyyy") + " to " + sDeliveryEndDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader = sReportHeader + ", Delivery Date from" + sDeliveryStartDate.ToString("dd MMM yyyy") + " to " + sDeliveryEndDate.ToString("dd MMM yyyy");
                            }

                        }
                    }
                    if (nStoreDate > 0)
                    {
                        DateTime sStoreStartDate = Convert.ToDateTime(sString.Split('~')[10]);
                        DateTime sStoreEndDate = Convert.ToDateTime(sString.Split('~')[11]);
                        if (nIssueDate == 1)
                        {
                            if (sReportHeader == "")
                            {
                                sReportHeader = "Store Rcv Date " + sStoreStartDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader = sReportHeader + ", Store Rcv Date " + sStoreStartDate.ToString("dd MMM yyyy");
                            }

                        }

                        if (nIssueDate == 5)
                        {

                            if (sReportHeader == "")
                            {
                                sReportHeader = "Store Rcv Date " + sStoreStartDate.ToString("dd MMM yyyy") + " to " + sStoreEndDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                sReportHeader = sReportHeader + ", Store Rcv Date" + sStoreStartDate.ToString("dd MMM yyyy") + " to " + sStoreEndDate.ToString("dd MMM yyyy");
                            }

                        }
                    }
                    #endregion

                }

                #region EXCEL START
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
                    var sheet = excelPackage.Workbook.Worksheets.Add("Fabric Batch QC Report");
                    sheet.Name = "Fabric Batch QC Report";
                    sheet.Column(colIndex++).Width = 10; //SL
                    sheet.Column(colIndex++).Width = 15; //Loom No
                    sheet.Column(colIndex++).Width = 15; //Batch No
                    sheet.Column(colIndex++).Width = 15; //Dispo No
                    sheet.Column(colIndex++).Width = 35; //customer
                    sheet.Column(colIndex++).Width = 35; //construction
                    sheet.Column(colIndex++).Width = 15; //shift
                    sheet.Column(colIndex++).Width = 20; //date
                    sheet.Column(colIndex++).Width = 20; //qty
                    sheet.Column(colIndex++).Width = 20; //remarks

                    #region Report Header
                    sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Fabric Batch QC Report"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;
                    sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = sReportHeader; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rowIndex += 2;
                    #endregion
                    #region Report Body
                    var FabricBatchQCDetails = _oFabricBatchQCDetails.GroupBy(x => new { x.QCGrade })
                                            .OrderBy(x => x.Key.QCGrade)
                                            .Select(x => new
                                            {
                                                _QCGrade = x.Key.QCGrade,
                                                _FabricBatchQCDetailList = x.OrderBy(c => c.FBQCDetailID),
                                                SubTotalQty = x.Sum(y => y.Qty),

                                            });

                    foreach (var oData in FabricBatchQCDetails)
                    {
                        int nSL = 1;
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "QCGrade: " +oData._QCGrade; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        rowIndex++;

                        #region Header 1
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Loom No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Batch No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Dispo No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Customer"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Construction"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Shift"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Date"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                        #endregion
                        //data
                        foreach (var oItem in oData._FabricBatchQCDetailList)
                        {
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LoomNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BatchNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DispoNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BuyerName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Construction; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ShiftName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DeliveryDateSt; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Remark; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;                          
                            nSL++;
                            rowIndex++;
                        
                        }

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = oData.SubTotalQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value =""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rowIndex++;

                    }

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Merge = true; cell.Value = "Grand Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = _oFabricBatchQCDetails.Sum(x=>x.Qty); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rowIndex++;
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=RPT_FabricBatchQCReport.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
               
                #endregion

            }
            else
            {
                throw new Exception("Data Not Found");
            }


        }





        #endregion
        [HttpPost]
        public JsonResult SearchByLoomNoDispoNo(FabricBatchQCDetail oFabricBatchQCDetail)
        {
            _oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
            try
            {
                string sSQL = "SELECT * FROM View_FabricBatchQCDetail ";
                string sSReturn = "";
                if (oFabricBatchQCDetail.LoomNo != "" && oFabricBatchQCDetail.LoomNo != null)
                {
                    Global.TagSQL(ref sSReturn);
                    sSReturn += " LoomNo LIKE '%" + oFabricBatchQCDetail.LoomNo + "%'";
                }
                if (oFabricBatchQCDetail.DispoNo != "" && oFabricBatchQCDetail.DispoNo != null)
                {
                    Global.TagSQL(ref sSReturn);
                    sSReturn += " DispoNo LIKE '%" + oFabricBatchQCDetail.DispoNo + "%'";
                }
                sSQL += sSReturn;
                _oFabricBatchQCDetails = FabricBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatchQCDetail = new FabricBatchQCDetail();
                _oFabricBatchQCDetail.ErrorMessage = ex.Message;
                _oFabricBatchQCDetails.Add(_oFabricBatchQCDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchQCDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFabricBatchInQC(int id)//FBID
        
        {
            FabricBatchQC oFabricBatchQC = new FabricBatchQC();
            try
            {
                if (id > 0)
                {
                    oFabricBatchQC = oFabricBatchQC.GetByBatch(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oFabricBatchQC.FBQCID > 0)
                    {
                        oFabricBatchQC.FabricBatchQCDetails = FabricBatchQCDetail.Gets(oFabricBatchQC.FBQCID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get QC Details
                        //if (oFabricBatchQC.FabricBatchQCDetails.Any())
                        //{
                        //    var TSDistinctList = oFabricBatchQC.FabricBatchQCDetails.Select(a => new { FBQCTransferID = a.FBQCTransferID }).Distinct().Where(p => p.FBQCTransferID != 0).ToList();
                        //    if (TSDistinctList.Any())
                        //    {
                        //        oFabricBatchQC.FabricBatchQCTransfers = FabricBatchQCTransfer.Gets("SELECT * FROM View_FabricBatchQCTransfer WHERE FBQCTransferID IN ( " + string.Join(" , ", TSDistinctList.Select(x => x.FBQCTransferID)) + " ) ORDER BY IssueDate", ((User)Session[SessionInfo.CurrentUser]).UserID);
                              
                        //    }
                        //}
                        
                        if(oFabricBatchQC.FabricBatchQCDetails.Any() && oFabricBatchQC.FabricBatchQCDetails.FirstOrDefault().FBQCDetailID>0)
                        {
                            string sSQL = "SELECT * FROM View_FabricBatchQCFault WHERE FBQCDetailID In ("+ string.Join(",",oFabricBatchQC.FabricBatchQCDetails.Select(x=>x.FBQCDetailID).ToList()) +")";
                            oFabricBatchQC.FabricBatchQCFaults = FabricBatchQCFault.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                oFabricBatchQC = new FabricBatchQC();
                oFabricBatchQC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchQC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFabricBatchQCFalults(FabricBatchQCDetail oFabricBatchQCDetail)
        {
            FabricBatchQCFault oFabricBatchQCFault = new FabricBatchQCFault();
            List<FabricBatchQCFault> oFabricBatchQCFaults = new List<FabricBatchQCFault>();
            try
            {
                if (oFabricBatchQCDetail.FBQCID > 0)
                {
                    string sSQL = "SELECT * FROM View_FabricBatchQCFault AS HH WHERE HH.FBQCDetailID = " + oFabricBatchQCDetail.FBQCDetailID;
                    oFabricBatchQCFaults = FabricBatchQCFault.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);//get QC Details
                }
                else
                {
                    throw new Exception("Please select a valid FN Batch QC");
                }
            }
            catch (Exception ex)
            {
                oFabricBatchQCFaults = new List<FabricBatchQCFault>();
                oFabricBatchQCFault = new FabricBatchQCFault();
                oFabricBatchQCFault.ErrorMessage = ex.Message;
                oFabricBatchQCFaults.Add(oFabricBatchQCFault);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchQCFaults);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
         [HttpGet]
        public JsonResult GetQCByProduction(int id, int nFBQCID)//BatchID, BatchQCID
        {
            FabricBatchQC oFabricBatchQC = new FabricBatchQC();
            try
            {
                if (nFBQCID > 0)
                {
                    //oFabricBatchQC = oFabricBatchQC.GetByBatch(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFabricBatchQC = oFabricBatchQC.Get(nFBQCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFabricBatchQC.FabricBatchQCDetails = FabricBatchQCDetail.Gets(nFBQCID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get QC Details
                    if (oFabricBatchQC.FabricBatchQCDetails.Any() && oFabricBatchQC.FabricBatchQCDetails.FirstOrDefault().FBQCDetailID > 0)
                    {
                        string sSQL = "SELECT * FROM View_FabricBatchQCFault WHERE FBQCDetailID In (" + string.Join(",", oFabricBatchQC.FabricBatchQCDetails.Select(x => x.FBQCDetailID).ToList()) + ")";
                        oFabricBatchQC.FabricBatchQCFaults = FabricBatchQCFault.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
            }
            catch (Exception ex)
            {
                oFabricBatchQC = new FabricBatchQC();
                oFabricBatchQC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchQC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        //public JsonResult GetFabricBatchWiseTransferSlip(int id)//FBID
        //{
        //    FabricBatchQC oFabricBatchQC = new FabricBatchQC();
        //    try
        //    {
        //        if (id > 0)
        //        {
        //            oFabricBatchQC = oFabricBatchQC.GetByBatch(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //            if (oFabricBatchQC.FBQCID > 0)
        //            {
        //                oFabricBatchQC.FabricBatchQCDetails = FabricBatchQCDetail.Gets(oFabricBatchQC.FBQCID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get QC Details
        //                if (oFabricBatchQC.FabricBatchQCDetails.Any())
        //                {
        //                    var TSDistinctList = oFabricBatchQC.FabricBatchQCDetails.Select(a => new { FBQCTransferID = a.FBQCTransferID }).Distinct().Where(p => p.FBQCTransferID != 0).ToList();
        //                    if (TSDistinctList.Any())
        //                    {
        //                        oFabricBatchQC.FabricBatchQCTransfers = FabricBatchQCTransfer.Gets("SELECT * FROM View_FabricBatchQCTransfer WHERE FBQCTransferID IN ( " + string.Join(" , ", TSDistinctList.Select(x => x.FBQCTransferID)) + " ) ORDER BY IssueDate", ((User)Session[SessionInfo.CurrentUser]).UserID);

        //                    }
        //                }


        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        oFabricBatchQC = new FabricBatchQC();
        //        oFabricBatchQC.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oFabricBatchQC);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetFabricBatchWiseTransferSlip(int id , bool isBatch)//FBID
        {
            List<FabricBatchQC> oFabricBatchQCs = new List<FabricBatchQC>();
            FabricBatchQC oFabricBatchQC = new FabricBatchQC();
            string sSql = "";
            try
            {
                if (id > 0)
                {
                    if (isBatch)
                        sSql = "SELECT * FROM View_FabricBatchQC  WHERE FBID=" + id;
                    else
                        sSql = "SELECT * FROM View_FabricBatchQC  WHERE FBPID=" + id;

                    oFabricBatchQCs = FabricBatchQC.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //if (oFabricBatchQCs.Any())
                    //{
                    //    oFabricBatchQC.FabricBatchQCDetails = FabricBatchQCDetail.Gets("SELECT * FROM View_FabricBatchQCDetail WHERE FBQCID IN (" + string.Join(",", oFabricBatchQCs.Select(x=>x.FBQCID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);//get QC Details
                    //    if (oFabricBatchQC.FabricBatchQCDetails.Any())
                    //    {
                    //        var TSDistinctList = oFabricBatchQC.FabricBatchQCDetails.Select(a => new { FBQCTransferID = a.FBQCTransferID }).Distinct().Where(p => p.FBQCTransferID != 0).ToList();
                    //        if (TSDistinctList.Any())
                    //        {
                    //            oFabricBatchQC.FabricBatchQCTransfers = FabricBatchQCTransfer.Gets("SELECT * FROM View_FabricBatchQCTransfer WHERE FBQCTransferID IN ( " + string.Join(" , ", TSDistinctList.Select(x => x.FBQCTransferID)) + " ) ORDER BY IssueDate", ((User)Session[SessionInfo.CurrentUser]).UserID);

                    //        }
                    //    }


                    //}
                }
            }
            catch (Exception ex)
            {
                oFabricBatchQC = new FabricBatchQC();
                oFabricBatchQC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchQC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetOrderWiseTransferSlip(int id)//FabricSalesContractDetailID
        //{
        //    FabricBatchQC oFabricBatchQC = new FabricBatchQC();
        //    try
        //    {
        //        if (id > 0)
        //        {
        //            string sql = "SELECT * FROM View_FabricBatchQCTransfer WHERE FBQCTransferID IN (SELECT FBQCTransferID FROM FabricBatchQCDetail WHERE FBQCID IN (SELECT FBQCID FROM FabricBatchQC WHERE FBID IN (SELECT FBID FROM FabricBatch WHERE FabricSalesContractDetailID=" + id + ")))  ORDER BY IssueDate";
        //            oFabricBatchQC.FabricBatchQCTransfers = FabricBatchQCTransfer.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        oFabricBatchQC = new FabricBatchQC();
        //        oFabricBatchQC.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oFabricBatchQC);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}




        [HttpPost]
        public JsonResult GetsFabricBatchQCDetails(FabricBatchQC oFabricBatchQC)
        {
            List<FabricBatchQCDetail> oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
            try
            {
                if (oFabricBatchQC.FBID > 0)
                {
                    oFabricBatchQC = oFabricBatchQC.GetByBatch(oFabricBatchQC.FBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oFabricBatchQC.FBQCID > 0)
                    {
                        oFabricBatchQCDetails = FabricBatchQCDetail.Gets(oFabricBatchQC.FBQCID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get QC Details
                    }
                }
            }
            catch (Exception ex)
            {
                FabricBatchQCDetail oFabricBatchQCDetail = new FabricBatchQCDetail();
                oFabricBatchQCDetail.ErrorMessage = ex.Message;
                oFabricBatchQCDetails.Add(oFabricBatchQCDetail);
            }
            var jsonResult = Json(oFabricBatchQCDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult SaveQCDetail(FabricBatchQCDetail oFabricBatchQCDetail)
        {
            try
            {
                oFabricBatchQCDetail = oFabricBatchQCDetail.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricBatchQCDetail = new FabricBatchQCDetail();
                oFabricBatchQCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchQCDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DeleteQCDetail(int id)//FBQCDetailID
        {
            string sFeedBackMessage = "";
            FabricBatchQCDetail oFabricBatchQCDetail = new FabricBatchQCDetail();
            try
            {
                sFeedBackMessage = oFabricBatchQCDetail.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Lock(FabricBatchQCDetail oFabricBatchQCDetail)
        {
            try
            {
                oFabricBatchQCDetail = oFabricBatchQCDetail.Lock(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricBatchQCDetail = new FabricBatchQCDetail();
                oFabricBatchQCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchQCDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LockFabricBatchQCDetail(FabricBatchQCDetail oFabricBatchQCDetail)
        {
            try
            {
                if (!string.IsNullOrEmpty(oFabricBatchQCDetail.FBQCDetailIDs))
                {

                    oFabricBatchQCDetail.ProDate = Convert.ToDateTime(oFabricBatchQCDetail.ProDate.ToString("dd MMM yyyy"));
                    oFabricBatchQCDetail.ProDate = oFabricBatchQCDetail.ProDate.AddHours(DateTime.Now.Hour);
                    oFabricBatchQCDetail.ProDate = oFabricBatchQCDetail.ProDate.AddMinutes(DateTime.Now.Minute);
                    oFabricBatchQCDetail.ProDate = oFabricBatchQCDetail.ProDate.AddSeconds(DateTime.Now.Second);

                    oFabricBatchQCDetail.DeliveryDate = Convert.ToDateTime(oFabricBatchQCDetail.DeliveryDate.ToString("dd MMM yyyy"));
                    oFabricBatchQCDetail.DeliveryDate = oFabricBatchQCDetail.DeliveryDate.AddHours(DateTime.Now.Hour);
                    oFabricBatchQCDetail.DeliveryDate = oFabricBatchQCDetail.DeliveryDate.AddMinutes(DateTime.Now.Minute);
                    oFabricBatchQCDetail.DeliveryDate = oFabricBatchQCDetail.DeliveryDate.AddSeconds(DateTime.Now.Second);

                    oFabricBatchQCDetail = oFabricBatchQCDetail.LockFabricBatchQCDetail(((User)Session[SessionInfo.CurrentUser]).UserID);

                  
                }
                else
                {
                    oFabricBatchQCDetail = new FabricBatchQCDetail();
                    oFabricBatchQCDetail.ErrorMessage = "Select item(s) from list.";
                }
            }
            catch (Exception ex)
            {
                oFabricBatchQCDetail = new FabricBatchQCDetail();
                oFabricBatchQCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchQCDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult QCDone(FabricBatchQC oFabricBatchQC)
        {
            try
            {
                oFabricBatchQC = oFabricBatchQC.QCDone(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricBatchQC = new FabricBatchQC();
                oFabricBatchQC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchQC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult PrintFinalInspectionSticker(int nFBQCDetailID, double nts)
        //{
        //    _oFabricBatchQCDetail = new FabricBatchQCDetail();
        //    _oFabricBatchQCDetail = _oFabricBatchQCDetail.Get(nFBQCDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    _oFabricBatchQC = new FabricBatchQC();
        //    _oFabricBatchQCDetail.FabricBatchQC = _oFabricBatchQC.Get(_oFabricBatchQCDetail.FBQCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    rptFBFinalInspectionSticker oReport = new rptFBFinalInspectionSticker();
        //    byte[] abytes = oReport.PrepareReport(_oFabricBatchQCDetail, oCompany);
        //    return File(abytes, "application/pdf");
        //}
        //public ActionResult PrintTransferSlip(string sFBQCDetailIDs, int nFBID, int nFBPID, double nts ,int TransferSlipID)
        //{
        //    /* Modified By Tonmoy */
        //    _oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
        //    FabricBatchQCTransfer oFabricBatchQCTransfer = new FabricBatchQCTransfer();
        //    string sSQL = string.Empty;
        //    if (TransferSlipID > 0)
        //    {
        //        oFabricBatchQCTransfer = FabricBatchQCTransfer.Get(TransferSlipID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        sSQL = "SELECT * FROM View_FabricBatchQCDetail WHERE FBQCTransferID =" + TransferSlipID;
        //    }
        //    else
        //    {
        //        sSQL = "SELECT * FROM View_FabricBatchQCDetail WHERE FBQCDetailID IN (" + sFBQCDetailIDs + ")";
        //    }
          
        //    _oFabricBatchQCDetails = FabricBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    FabricBatch oFB = new FabricBatch();
        //    oFB = oFB.Get(nFBID, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    FabricBatchProduction oFBP = new FabricBatchProduction();
        //    if (nFBPID > 0)
        //    {
        //        oFBP = oFBP.Get(nFBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }

        //    FabricBatchQC oFabricBatchQC=new FabricBatchQC();

        //    if(_oFabricBatchQCDetails.Count()>0 && _oFabricBatchQCDetails.FirstOrDefault().FBQCID>0)
        //    {
        //        oFabricBatchQC = oFabricBatchQC.Get(_oFabricBatchQCDetails.FirstOrDefault().FBQCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    oFabricBatchQC.FabricBatchQCTransfers.Add(oFabricBatchQCTransfer);
        //    oFabricBatchQC.FabricBatchQCs.Add(oFabricBatchQC);
        //    rptTransferSlip oReport = new rptTransferSlip();
        //    byte[] abytes = oReport.PrepareReport(oFabricBatchQC, _oFabricBatchQCDetails, oFB, oFBP, oCompany);
        //    return File(abytes, "application/pdf");
        //}
        public ActionResult PrintBatchCard(int nFBID)
        {
            FabricBatchProduction oWarping = new FabricBatchProduction();
            FabricBatchProduction oSizing = new FabricBatchProduction();
            FabricBatchProduction oWeaving = new FabricBatchProduction();

            oWarping = oWarping.GetByBatchAndWeavingType(nFBID, EnumWeavingProcess.Warping, ((User)Session[SessionInfo.CurrentUser]).UserID);//get warping
            if (oWarping.FBID == 0)
            {
                oWarping = this.GetFabricBatchDefaultValue(nFBID);
            }
            oWarping.FabricBatchProductionBatchMans = FabricBatchProductionBatchMan.Gets(oWarping.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Batchmans
            oWarping.FabricBatchRawMaterials = FabricBatchRawMaterial.Gets(nFBID, EnumWeavingProcess.Warping, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oWarping.FBPBs = FabricBatchProductionBeam.GetsByFabricBatchProduction(oWarping.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get beam no Added By Tonmoy

            List<FabricSalesContractDetail> oFEODs = new List<FabricSalesContractDetail>();
            oFEODs = FabricSalesContractDetail.Gets(oWarping.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oSizing = oSizing.GetByBatchAndWeavingType(nFBID, EnumWeavingProcess.Sizing, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Sizing
            if (oSizing.FBID == 0)
            {
                oSizing = this.GetFabricBatchDefaultValue(nFBID);
            }
            oSizing.FabricBatchRawMaterials = FabricBatchRawMaterial.Gets(nFBID, EnumWeavingProcess.Sizing, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Chemical for sizing

            oWeaving = oWeaving.GetByBatchAndWeavingType(nFBID, EnumWeavingProcess.Loom, ((User)Session[SessionInfo.CurrentUser]).UserID);//get oWeaving
            if (oWeaving.FBID == 0)
            {
                oWeaving = this.GetFabricBatchDefaultValue(nFBID);
            }
            oWeaving.FabricBatchProductionBatchMans = FabricBatchProductionBatchMan.Gets(oWeaving.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Batchmans for weaving
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            DocPrintEngine oDocPrintEngine = new DocPrintEngine();
            oDocPrintEngine = oDocPrintEngine.GetActiveByType((int)EnumDocumentPrintType.SIZING_SECTION, (int)Session[SessionInfo.currentUserID]);
            oDocPrintEngine.DocPrintEngineDetails = DocPrintEngineDetail.Gets("SELECT * FROM DocPrintEngineDetail WHERE DocPrintEngineID =" + oDocPrintEngine.DocPrintEngineID + "  ORDER BY CONVERT(INT, SLNo)", (int)Session[SessionInfo.currentUserID]);

            rptFabricBatchCard oReport = new rptFabricBatchCard();
            byte[] abytes = oReport.PrepareReport(oWarping, oSizing, oWeaving, oCompany);
            //byte[] abytes = oReport.PrepareReport_Dynamic_Sizing(oWarping, oSizing, oWeaving, oCompany, oDocPrintEngine);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintBatchCard_Dynamic(int nFBID, int nProcess)
        {
            FabricBatchProduction oWarping = new FabricBatchProduction();
            FabricBatchProduction oSizing = new FabricBatchProduction();
            FabricBatchProduction oWeaving = new FabricBatchProduction();

            oWarping = oWarping.GetByBatchAndWeavingType(nFBID, EnumWeavingProcess.Warping, ((User)Session[SessionInfo.CurrentUser]).UserID);//get warping
            if (oWarping.FBID == 0)
            {
                oWarping = this.GetFabricBatchDefaultValue(nFBID);
            }
            oWarping.FabricBatchProductionBatchMans = FabricBatchProductionBatchMan.Gets(oWarping.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Batchmans
            oWarping.FabricBatchRawMaterials = FabricBatchRawMaterial.Gets(nFBID, EnumWeavingProcess.Warping, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oWarping.FBPBs = FabricBatchProductionBeam.GetsByFabricBatchProduction(oWarping.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get beam no Added By Tonmoy

            List<FabricSalesContractDetail> oFEODs = new List<FabricSalesContractDetail>();
            oFEODs = FabricSalesContractDetail.Gets(oWarping.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oSizing = oSizing.GetByBatchAndWeavingType(nFBID, EnumWeavingProcess.Sizing, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Sizing
            if (oSizing.FBID == 0)
            {
                oSizing = this.GetFabricBatchDefaultValue(nFBID);
            }
            oSizing.FabricBatchRawMaterials = FabricBatchRawMaterial.Gets(nFBID, EnumWeavingProcess.Sizing, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Chemical for sizing

            oWeaving = oWeaving.GetByBatchAndWeavingType(nFBID, EnumWeavingProcess.Loom, ((User)Session[SessionInfo.CurrentUser]).UserID);//get oWeaving
            if (oWeaving.FBID == 0)
            {
                oWeaving = this.GetFabricBatchDefaultValue(nFBID);
            }
            oWeaving.FabricBatchProductionBatchMans = FabricBatchProductionBatchMan.Gets(oWeaving.FBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);//get Batchmans for weaving
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            DocPrintEngine oDocPrintEngine = new DocPrintEngine();
           
            rptFabricBatchCard oReport = new rptFabricBatchCard();
            byte[] abytes = null;

            if (nProcess == (int)EnumWeavingProcess.Sizing)
            {
                oDocPrintEngine = oDocPrintEngine.GetActiveByType((int)EnumDocumentPrintType.SIZING_SECTION, (int)Session[SessionInfo.currentUserID]);
                oDocPrintEngine.DocPrintEngineDetails = DocPrintEngineDetail.Gets("SELECT * FROM DocPrintEngineDetail WHERE DocPrintEngineID =" + oDocPrintEngine.DocPrintEngineID + "  ORDER BY CONVERT(INT, SLNo)", (int)Session[SessionInfo.currentUserID]);

                abytes = oReport.PrepareReport_Dynamic_Sizing(oWarping, oSizing, oWeaving, oCompany, oDocPrintEngine);
            }
            else if (nProcess == (int)EnumWeavingProcess.Warping)
            {
                oDocPrintEngine = oDocPrintEngine.GetActiveByType((int)EnumDocumentPrintType.WARPING_SECTION, (int)Session[SessionInfo.currentUserID]);
                oDocPrintEngine.DocPrintEngineDetails = DocPrintEngineDetail.Gets("SELECT * FROM DocPrintEngineDetail WHERE DocPrintEngineID =" + oDocPrintEngine.DocPrintEngineID + "  ORDER BY CONVERT(INT, SLNo)", (int)Session[SessionInfo.currentUserID]);

                abytes = oReport.PrepareReport_Dynamic_Warping(oWarping, oSizing, oWeaving, oCompany, oDocPrintEngine);
            }
            else 
            {
                abytes = oReport.PrepareReport(oWarping, oSizing, oWeaving, oCompany);
            }

            return File(abytes, "application/pdf");
        }

        //public ActionResult PrintTransferSlipFromFEOUpdateStatus(int nFabricSalesContractDetailID, bool hasDate, string dtFrom, string dtTo)
        //{

        //    List<FabricBatchQCDetail>  oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
        //    string sSQL = "Select * from View_FabricBatchQCDetail Where IsLock=1 And FBQCID In (Select FBQCID from FabricBatchQC Where FBID IN (Select FBID from FabricBatch Where FabricSalesContractDetailID = " + nFabricSalesContractDetailID + "))";
        //    if (hasDate)
        //        sSQL += " And Convert(date,TransferDate) Between '" + dtFrom + "' And '" + dtTo + "'";

        //    oFabricBatchQCDetails = FabricBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


        //    if (oFabricBatchQCDetails.Any() && oFabricBatchQCDetails.FirstOrDefault().FBQCDetailID > 0)
        //        sSQL = "Select top(1)* from View_FabricBatch Where FBID In (Select FBID from FabricBatchQC Where FBQCID In (" + string.Join(",", oFabricBatchQCDetails.Select(x => x.FBQCID).Distinct().ToList()) + "))";


        //    List<FabricBatch> oFBs = new List<FabricBatch>();
        //    oFBs = FabricBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    List<FabricBatchQC> oFabricBatchQCs = new List<FabricBatchQC>();

        //    if (oFabricBatchQCDetails.Count() > 0 && oFabricBatchQCDetails.FirstOrDefault().FBQCID > 0)
        //    {
        //        sSQL = "Select * from View_FabricBatchQC Where FBQCID In (" + string.Join(",", oFabricBatchQCDetails.Select(x => x.FBQCID).Distinct().ToList()) + ")";
        //        oFabricBatchQCs = FabricBatchQC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //    FabricBatchQC oFabricBatchQC = new FabricBatchQC();
        //    oFabricBatchQC = (oFabricBatchQCs.Any())? oFabricBatchQCs.FirstOrDefault():new FabricBatchQC();
        //    //oFabricBatchQC.GreyWidth = (oFabricBatchQCs.Any()) ? oFabricBatchQCs.Sum(x=>x.GreyWidth) :0;
        //    oFabricBatchQC.FabricBatchQCs = oFabricBatchQCs;

        //    FabricBatch oFB = new FabricBatch();
        //    oFB = (oFBs.Any()) ? oFBs.FirstOrDefault() : new FabricBatch();

        //    rptTransferSlip oReport = new rptTransferSlip();
        //    byte[] abytes = oReport.PrepareReport(oFabricBatchQC, oFabricBatchQCDetails, oFB, new FabricBatchProduction(), oCompany);
        //    return File(abytes, "application/pdf");
        //}
        #endregion

        #region Fabric Receive
        public ActionResult ViewFabricReceive(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFabricBatchQCs = new List<FabricBatchQC>();
            string sSQL = "SELECT * FROM View_FabricBatchQC WHERE CountNotRecDetail > 0 AND FBQCID IN (SELECT FBQCID FROM FabricBatchQCDetail WHERE ISNULL(DeliveryBy,0) <> 0 AND ISNULL(ReceiveBy,0)=0)  AND FabricBatchStatus<" + (int)EnumFabricBatchState.Finish + " ORDER By BatchNo";//AND FabricBatchStatus>=" + (int)EnumFabricBatchState.Weaving_Finish + "
            _oFabricBatchQCs = FabricBatchQC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Stores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricBatchQC, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            return View(_oFabricBatchQCs);
        }

        public JsonResult GetFabricBatchQCDetails(int id)//FBQCID
        {
            _oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
            try
            {
                if (id > 0)  
                {
                    string sSQL = "SELECT * FROM view_FabricBatchQCDetail WHERE FBQCID  = " + id + " AND  ISNULL(DeliveryBy,0) <> 0 ORDER BY DeliveryDate DESC ";
                    _oFabricBatchQCDetails = FabricBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);//get QC Details
                }
            }
            catch (Exception ex)
            {
                FabricBatchQCDetail oFabricBatchQCDetail = new FabricBatchQCDetail();
                oFabricBatchQCDetail.ErrorMessage = ex.Message;
                _oFabricBatchQCDetails.Add(oFabricBatchQCDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchQCDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ReceiveFabric(FabricBatchQCDetail oFabricBatchQCDetail)
        {
            try
            {
                if (!string.IsNullOrEmpty(oFabricBatchQCDetail.FBQCDetailIDs))
                {
                //    string sSQL = "SELECT * FROM View_FabricBatchQCDetail WHERE FBQCDetailID IN (" + oFabricBatchQCDetail.FBQCDetailIDs + ")";
                //    oFabricBatchQCDetail.FabricBatchQCDetails = new List<FabricBatchQCDetail>();
                //    oFabricBatchQCDetail.FabricBatchQCDetails = FabricBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oFabricBatchQCDetail.StoreRcvDate = Convert.ToDateTime(oFabricBatchQCDetail.StoreRcvDate.ToString("dd MMM yyyy"));
                    oFabricBatchQCDetail.StoreRcvDate = oFabricBatchQCDetail.StoreRcvDate.AddHours(DateTime.Now.Hour);
                    oFabricBatchQCDetail.StoreRcvDate = oFabricBatchQCDetail.StoreRcvDate.AddMinutes(DateTime.Now.Minute);
                    oFabricBatchQCDetail.StoreRcvDate = oFabricBatchQCDetail.StoreRcvDate.AddSeconds(DateTime.Now.Second);

                    oFabricBatchQCDetail.FabricBatchQCDetails = FabricBatchQCDetail.ReceiveInDelivery(oFabricBatchQCDetail,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                //oFabricBatchQCDetail = oFabricBatchQCDetail.ReceiveInDelivery(((User)Session[SessionInfo.CurrentUser]).UserID);
                //if (string.IsNullOrEmpty(oFabricBatchQCDetail.ErrorMessage))
                //{
                //    if (oFabricBatchQCDetail.FabricBatchQC.FBQCID > 0)
                //    {
                //        oFabricBatchQCDetail.FabricBatchQCDetails = new List<FabricBatchQCDetail>();
                //        string sSQL = "SELECT * FROM View_FabricBatchQCDetail WHERE FBQCID  = " + oFabricBatchQCDetail.FabricBatchQC.FBQCID + " AND isnull(DeliveryBy,0)=0";
                //        oFabricBatchQCDetail.FabricBatchQCDetails = FabricBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);//get QC Details
                //    }
                //}
            }
            catch (Exception ex)
            {
                oFabricBatchQCDetail = new FabricBatchQCDetail();
                oFabricBatchQCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchQCDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetReceiveFabricByFEO(FabricBatchQC oFabricBatchQC)
        {
            try
            {
                _oFabricBatchQCs = new List<FabricBatchQC>();
                string sSQL = "SELECT * FROM View_FabricBatchQC WHERE CountNotRecDetail > 0 AND FBQCID IN (SELECT FBQCID FROM FabricBatchQCDetail WHERE ISNULL(ReceiveBy,0)<=0)  AND FabricBatchStatus<" + (int)EnumFabricBatchState.Finish + "AND FEONo Like'%" + oFabricBatchQC.FEONo + "%' ORDER By BatchNo";//AND FabricBatchStatus>" + (int)EnumFabricBatchState.Weaving_Finish + "
                _oFabricBatchQCs = FabricBatchQC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatchQCs = new List<FabricBatchQC>();
                oFabricBatchQC.ErrorMessage = ex.Message;
            }

            var jsonResult = Json(_oFabricBatchQCs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        #endregion
        private FabricBatchProduction GetFabricBatchDefaultValue(int nFBID)
        {
            FabricBatchProduction oFBP = new FabricBatchProduction();
            FabricBatch oFB = new FabricBatch();
            oFB = oFB.Get(nFBID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFBP.FBID = oFB.FBID;
            oFBP.StartTime = oFB.IssueDate;
            oFBP.BatchNo = oFB.BatchNo;
            oFBP.FabricSalesContractDetailID = oFB.FabricSalesContractDetailID;
            oFBP.FabricBatchQty = oFB.Qty;
            oFBP.FEONo = oFB.FEONo;
            oFBP.IsInHouse = oFB.IsInHouse;
            oFBP.OrderType = oFB.OrderType;
            oFBP.BuyerName = oFB.BuyerName;
            //oFBP.WarpDoneQty = oFB.WarpDoneQty;
            oFBP.TotalEnds = oFB.TotalEnds;
            oFBP.NoOfSection = oFB.NoOfSection;
            oFBP.WarpCount = oFB.WarpCount;
            oFBP.FabricBatchStatus = oFB.Status;
            oFBP.Construction = oFB.Construction;
            return oFBP;
        }

        //[HttpPost]
        //public JsonResult DeleteFabricBatchQCFault(FabricBatchQCFault oFabricBatchQCFault)
        //{
        //    string sMsg = "";
        //    try
        //    {
        //        if (oFabricBatchQCFault.FBQCFaultID <= 0) { throw new Exception("Please select an valid item."); }
        //        sMsg = oFabricBatchQCFault.Delete(oFabricBatchQCFault.FBQCFaultID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        sMsg = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(sMsg);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult PrintFabricBatchQC(int nFBQCID)
        {
            _oFabricBatchQC = new FabricBatchQC();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            FabricExecutionOrderSpecification oFabricExecutionOrderSpecification = new FabricExecutionOrderSpecification();
            Company oCompany = new Company();
            List<FabricBatchQCCheck> oFabricBatchQCChecks = new List<FabricBatchQCCheck>();
            if (nFBQCID > 0)
            {
                
                string sSQL = "";
                sSQL = "SELECT * FROM View_FabricBatchQCCheck WHERE FabricBatchQCID = " + nFBQCID;
                oFabricBatchQCChecks = FabricBatchQCCheck.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
                _oFabricBatchQC = _oFabricBatchQC.Get(nFBQCID, (int)Session[SessionInfo.currentUserID]);
                _oFabricBatchQC.FabricBatchQCDetails = FabricBatchQCDetail.Gets(_oFabricBatchQC.FBQCID, (int)Session[SessionInfo.currentUserID]);
                oFabricSCReport = oFabricSCReport.Get(_oFabricBatchQC.FEOID, (int)Session[SessionInfo.currentUserID]);
                oFabricExecutionOrderSpecification = FabricExecutionOrderSpecification.Get(_oFabricBatchQC.FEOSID, (int)Session[SessionInfo.currentUserID]);
            }

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            string sql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.FabricBatchQC + "  Order By Sequence";
            oApprovalHeads = ApprovalHead.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            byte[] abytes;
            rptPrintFabricBatchQC oReport = new rptPrintFabricBatchQC();
            abytes = oReport.PrepareReport(_oFabricBatchQC,oFabricBatchQCChecks, oCompany, oApprovalHeads, oFabricSCReport, oFabricExecutionOrderSpecification);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintFabricBatchQC2(int nFBQCID)
        {
            _oFabricBatchQC = new FabricBatchQC();
            Company oCompany = new Company();
            if (nFBQCID > 0)
            {
                _oFabricBatchQC = FabricBatchQC.Gets("SELECT * FROM View_FabricBatchQC WHERE FBQCID  = " + nFBQCID, (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
                _oFabricBatchQC.FabricBatchQCDetails = FabricBatchQCDetail.Gets(_oFabricBatchQC.FBQCID, (int)Session[SessionInfo.currentUserID]);
                _oFabricBatchQC.FabricBatchQCFaults = FabricBatchQCFault.Gets("SELECT * FROM View_FabricBatchQCFault WHERE FBQCDetailID IN (SELECT FBQCDetailID FROM FabricBatchQCDetail WHERE FBQCID = " + nFBQCID + ")", (int)Session[SessionInfo.currentUserID]); 
            }

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            string sql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.FabricBatchQC + "  Order By Sequence";
            oApprovalHeads = ApprovalHead.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            byte[] abytes;
            rptPreviewFabricBatchQC2 oReport = new rptPreviewFabricBatchQC2();
            abytes = oReport.PrepareReport(_oFabricBatchQC, oCompany, oApprovalHeads);
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

        [HttpPost]
        public JsonResult AdvSearch(FabricBatchQC oFabricBatchQC)
        {
            _oFabricBatchQCs = new List<FabricBatchQC>();
            try
            {
                string sSQL = MakeSQL(oFabricBatchQC);
                _oFabricBatchQCs = FabricBatchQC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatchQCs = new List<FabricBatchQC>();
                oFabricBatchQC = new FabricBatchQC();
                oFabricBatchQC.ErrorMessage = ex.Message;
                _oFabricBatchQCs.Add(oFabricBatchQC);
            }
            var jsonResult = Json(_oFabricBatchQCs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string MakeSQL(FabricBatchQC oFabricBatchQC)
        {
            int nCboStartDateAdv = 0, nCboGradeAdv = 0;
            DateTime dFromStartDateAdv = DateTime.Today, dToStartDateAdv = DateTime.Today;
            string sDispoNoAdv = "";
            string sReturn1 = "", sReturn = "";
            string sBatchNo = "";
            sReturn1 = "Select * from View_FabricBatchQC";
            if (!string.IsNullOrEmpty(oFabricBatchQC.ErrorMessage))
            {
                nCboStartDateAdv = Convert.ToInt32(oFabricBatchQC.ErrorMessage.Split('~')[0]);
                dFromStartDateAdv = Convert.ToDateTime(oFabricBatchQC.ErrorMessage.Split('~')[1]);
                dToStartDateAdv = Convert.ToDateTime(oFabricBatchQC.ErrorMessage.Split('~')[2]);

                sDispoNoAdv = Convert.ToString(oFabricBatchQC.ErrorMessage.Split('~')[3]);
                sBatchNo = Convert.ToString(oFabricBatchQC.ErrorMessage.Split('~')[4]);
                nCboGradeAdv = Convert.ToInt32(oFabricBatchQC.ErrorMessage.Split('~')[5]);
            }

            #region Start Date
            if (nCboStartDateAdv != (int)EnumCompareOperator.None)
            {
                DateObject.CompareDateQuery(ref sReturn, "QCStartDateTime", nCboStartDateAdv, dFromStartDateAdv, dToStartDateAdv);
                if (nCboStartDateAdv == (int)EnumCompareOperator.EqualTo)
                    _sDateRangeText = "Start Date: " + dFromStartDateAdv.ToString("dd MMM yyyy");
                else if (nCboStartDateAdv == (int)EnumCompareOperator.NotEqualTo)
                    _sDateRangeText = "Start Date Not Equal: " + dFromStartDateAdv.ToString("dd MMM yyyy");
                else if (nCboStartDateAdv == (int)EnumCompareOperator.GreaterThan)
                    _sDateRangeText = "Start Date Greater Than: " + dFromStartDateAdv.ToString("dd MMM yyyy");
                else if (nCboStartDateAdv == (int)EnumCompareOperator.SmallerThan)
                    _sDateRangeText = "Start Date Smaller Than: " + dFromStartDateAdv.ToString("dd MMM yyyy");
                else if (nCboStartDateAdv == (int)EnumCompareOperator.Between)
                    _sDateRangeText = "Start Date Between: " + dFromStartDateAdv.ToString("dd MMM yyyy") + " AND " + dToStartDateAdv.ToString("dd MMM yyyy");
                else if (nCboStartDateAdv == (int)EnumCompareOperator.NotBetween)
                    _sDateRangeText = "Start Date Not Between: " + dFromStartDateAdv.ToString("dd MMM yyyy") + " AND " + dToStartDateAdv.ToString("dd MMM yyyy");

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
            #region Grade
            if (nCboGradeAdv > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FBQCID IN (SELECT FBQCID FROM FabricBatchQCDetail WHERE Grade = " + nCboGradeAdv + ")";
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn;
            return sSQL;
        }

        [HttpPost]
        public JsonResult GetFabricBatchQCs(FabricBatch oFabricBatch)
        {
            List<FabricBatch> oFabricBatchs = new List<FabricBatch>();
            List<FabricBatchQC> oFabricBatchQCs = new List<FabricBatchQC>();
            List<FabricBatchLoom> oFabricBatchLooms = new List<FabricBatchLoom>();
            try
            {
                if (oFabricBatch.NoOfSection == 2)//Waiting
                {
                    string sSQL = "select * from (SELECT *,isnull((SELECT TotalLength FROM FabricBatchQC where FabricBatchLoomID=TT.FabricBatchLoomID ),0) as QtyQC FROM View_FabricBatchLoom as TT where [Status] in (1,2,3) AND (SELECT COUNT(*) FROM FabricBatchLoomDetail WHERE FabricBatchLoomID=TT.FabricBatchLoomID) > 0 AND FBID not in (SELECT FBID FROM FabricBatchQC WHERE FabricBatchLoomID <= 0) AND FabricBatchLoomID not in (SELECT FabricBatchLoomID FROM FabricBatchQC WHERE FabricBatchLoomID > 0) and FBID in (Select FBID from FabricBatch where  [Status]>=5 )) as TT where TT.QtyPro>isnull(TT.QtyQC,0)+0.5 ";
                    oFabricBatchLooms = FabricBatchLoom.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (FabricBatchLoom oItem in oFabricBatchLooms)
                    {
                        FabricBatchQC oFBQC = new FabricBatchQC();
                        oFBQC.FBQCID = 0;
                        oFBQC.FBID = oItem.FBID;
                        oFBQC.FabricBatchLoomID = oItem.FabricBatchLoomID;
                        oFBQC.BatchNo = oItem.BatchNo;
                        oFBQC.FEOSID = oItem.FEOSID;
                        oFBQC.FEONo = oItem.FEONo;
                        oFBQC.FabricBatchQty = oItem.Qty;
                        oFBQC.LoomQty = oItem.Qty;
                        oFBQC.FabricMachineName = oItem.FabricMachineName;
                        oFBQC.BuyerName = oItem.BuyerName;
                        oFBQC.Construction = oItem.Construction;
                        oFBQC.IsInHouse = oItem.IsInHouse;
                        oFBQC.OrderType = oItem.OrderType;
                        oFBQC.FEOSID = oItem.FEOSID;
                        oFabricBatchQCs.Add(oFBQC);
                    }
                }
                else if (oFabricBatch.NoOfSection == 1)//Running
                {
                    oFabricBatchQCs = new List<FabricBatchQC>();
                    #region Fabric Batch Qc
                    oFabricBatchQCs = FabricBatchQC.Gets("Select * from View_FabricBatchQC WHERE FabricBatchStatus = " + (int)EnumFabricBatchState.In_QC + " ORDER BY FBQCID DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    #endregion
                }

            }
            catch (Exception ex)
            {
                FabricBatchQC oFBQC = new FabricBatchQC();
                oFBQC.ErrorMessage = ex.Message;
                oFabricBatchQCs = new List<FabricBatchQC>();
                oFabricBatchQCs.Add(oFBQC);
            }
            var jsonResult = Json(oFabricBatchQCs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetFabricBatchAndQCs(FabricBatchQC oFabricBatchQC)
        {
            List<FabricBatchLoom> oFabricBatchLooms = new List<FabricBatchLoom>();
            List<FabricBatchLoomDetail> oFabricBatchLoomDetails = new List<FabricBatchLoomDetail>();
            _oFabricBatchQCs = new List<FabricBatchQC>();
            try
            {
                string sReturn1 = "", sReturn = "";
                string sDispoNo = Convert.ToString(oFabricBatchQC.ErrorMessage.Split('~')[0]);
                string sBatchNo = Convert.ToString(oFabricBatchQC.ErrorMessage.Split('~')[1]);

                #region Fabric Batch Qc
                sReturn1 = "Select * from View_FabricBatchQC";
                #region Dispo No
                if (!string.IsNullOrEmpty(sDispoNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FEONo LIKE '%" + sDispoNo + "%' ";
                }
                #endregion
                #region Batch No
                if (!string.IsNullOrEmpty(sBatchNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BatchNo LIKE '%" + sBatchNo + "%' ";
                }
                #endregion
                sReturn1 += sReturn;
                _oFabricBatchQCs = FabricBatchQC.Gets(sReturn1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                #endregion

                #region Fabric Batch

                sReturn1 = string.Join(",", _oFabricBatchQCs.Where(p=>p.FabricBatchLoomID>0).Select(x => x.FabricBatchLoomID));
                sReturn = string.Join(",", _oFabricBatchQCs.Where(p => p.FabricBatchLoomID<=0).Select(x => x.FBID));

                sReturn1 = "select * from (SELECT * ,isnull((SELECT TotalLength FROM FabricBatchQC where FabricBatchLoomID=TT.FabricBatchLoomID ),0) as QtyQC FROM View_FabricBatchLoom as TT where [Status] in (1,2,3) AND (SELECT COUNT(*) FROM FabricBatchLoomDetail WHERE FabricBatchLoomID=TT.FabricBatchLoomID) > 0  " + (!string.IsNullOrEmpty(sReturn) ? " AND FBID not in (" + sReturn + ")" : "") + "" + (!string.IsNullOrEmpty(sReturn1) ? " AND FabricBatchLoomID not in (" + sReturn1 + ")" : "") + "" + (!string.IsNullOrEmpty(sDispoNo) ? " AND FEONo LIKE '%" + sDispoNo + "%'" : "") + "" + (!string.IsNullOrEmpty(sBatchNo) ? " AND BatchNo LIKE '%" + sBatchNo + "%'" : "") + "   and FBID in (Select FBID from FabricBatch where  [Status]>=5 )) as TT where TT.QtyPro>isnull(TT.QtyQC,0)+0.5 ";
                oFabricBatchLooms = FabricBatchLoom.Gets(sReturn1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricBatchLoomDetails = FabricBatchLoomDetail.GetsBySql("SELECT * FROM View_FabricBatchLoomDetail WHERE FabricBatchLoomID IN (" + string.Join(",",oFabricBatchLooms.Select(x=>x.FabricBatchLoomID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (FabricBatchLoom oItem in oFabricBatchLooms)
                {
                    FabricBatchQC oFBQC = new FabricBatchQC();
                    oFBQC.FBQCID = 0;
                    oFBQC.FBID = oItem.FBID;
                    oFBQC.FabricBatchLoomID = oItem.FabricBatchLoomID;
                   // oFBQC.FabricBatchLoomID = oItem.Loo;
                    oFBQC.BatchNo = oItem.BatchNo;
                    oFBQC.FEOSID = oItem.FEOSID;
                    oFBQC.FEONo = oItem.FEONo;
                    oFBQC.FabricBatchQty = oItem.Qty;
                    oFBQC.LoomQty = oFabricBatchLoomDetails.Where(x => x.FabricBatchLoomID == oItem.FabricBatchLoomID).Sum(x => x.Qty);
                    //oFBQC.FabricBatchStatus = oItem.Status;
                    oFBQC.FabricMachineName = oItem.FabricMachineName;
                    oFBQC.BuyerName = oItem.BuyerName;
                    oFBQC.Construction = oItem.Construction;
                    oFBQC.IsInHouse = oItem.IsInHouse;
                    oFBQC.OrderType = oItem.OrderType;
                    oFBQC.FEOSID = oItem.FEOSID;
                    _oFabricBatchQCs.Add(oFBQC);
                }
                #endregion
            }
            catch (Exception ex)
            {
                _oFabricBatchQCs = new List<FabricBatchQC>();
                oFabricBatchQC = new FabricBatchQC();
                oFabricBatchQC.ErrorMessage = ex.Message;
                _oFabricBatchQCs.Add(oFabricBatchQC);
            }
            var jsonResult = Json(_oFabricBatchQCs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetsFabricBatchByFWPD(FabricBatch oFabricBatch)
        {
            _oFabricBatchs = new List<FabricBatch>();
            try
            {
                if (oFabricBatch.FWPDID > 0)
                {
                    string sSQL = "SELECT * FROM View_FabricBatch WHERE FWPDID = " + oFabricBatch.FWPDID + " ORDER BY BatchNo";
                    _oFabricBatchs = FabricBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oFabricBatch = new FabricBatch();
                _oFabricBatch.ErrorMessage = ex.Message;
                _oFabricBatchs.Add(_oFabricBatch);
            }
            var jsonResult = Json(_oFabricBatchs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SearchForQC(FabricBatch oFabricBatch)
        {
            List<FabricBatch> oFabricBatchs = new List<FabricBatch>();
            List<FabricBatchLoom> _oFabricBatchLooms = new List<FabricBatchLoom>();
            try
            {
                //string sSQL = "Select * from (Select *,(Select SUM(Qty) from FabricBatchLoom where FabricBatchLoom.FabricBatchLoomID=FBL.FabricBatchLoomID) as QtyFinish,(Select SUM(Qty) from FabricBatchQCDetail where FabricBatchQCDetail.FBQCID=(SELECT TOP 1 FBQCID FROM FabricBatchQC WHERE FabricBatchLoomID = FBL.FabricBatchLoomID)) as QtyQC  from View_FabricBatchLoom as FBL where FBL.FEONo LIKE '%" + oFabricBatch.FEONo + "%' AND FBL.[Status] in (1,3) and FBL.FBID in (Select FBID from FabricBatch where FabricBatch.[Status]<>14 ) ) as HH where HH.QtyFinish>=isnull(HH.QtyQC,0)"; // AND FabricBatchLoomID NOT IN (SELECT FabricBatchLoomID FROM FabricBatchLoom)
                //_oFabricBatchLooms = FabricBatchLoom.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                string sSQL = " Select top(10)*   from View_FabricBatch as FB where FB.Status >=3 and FEONo LIKE '%" + oFabricBatch.FEONo + "%' "; // AND FabricBatchLoomID NOT IN (SELECT FabricBatchLoomID FROM FabricBatchLoom)
                oFabricBatchs = FabricBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                FabricBatch oFB = new FabricBatch();
                oFB.ErrorMessage = ex.Message;
                oFabricBatchs.Add(oFB);
            }
            var jsonResult = Json(oFabricBatchs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult Search(FabricBatch oFabricBatch)
        {
            _oFabricBatchs = new List<FabricBatch>();
            try
            {
                string sSQL = this.MakeSQL(oFabricBatch);
                _oFabricBatchs = FabricBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricBatch = new FabricBatch();
                oFabricBatch.ErrorMessage = ex.Message;
                _oFabricBatchs.Add(oFabricBatch);
            }
            var jsonResult = Json(_oFabricBatchs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string MakeSQL(FabricBatch oFabricBatch)
        {
            string sReturn1 = "SELECT * FROM View_FabricBatch ";
            string sReturn = "";

            #region FEO No
            if (!string.IsNullOrEmpty(oFabricBatch.FEONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FEONo LIKE '%" + oFabricBatch.FEONo + "%' ";
            }
            #endregion

            #region Not Warp Done Execution Order Load
            if (oFabricBatch.IsNotWarpDone)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " [Status] <  " + (int)EnumFabricBatchState.warping_Finish;
            }
            #endregion

            #region Weaving Process Type Wise
            if (oFabricBatch.WeavingProcessType == (int)EnumWeavingProcess.Sizing)
            {
                 Global.TagSQL(ref sReturn);
                 sReturn = sReturn + " [Status]>=3 And ( (FBID IN (SELECT FBID FROM FabricBatchProduction WHERE WeavingProcess=1 AND ENDTime IS NULL)"
                                    + " OR FBID NOT IN (SELECT FBID FROM FabricBatchProduction WHERE WeavingProcess=1)))";
            }
            else if (oFabricBatch.WeavingProcessType == (int)EnumWeavingProcess.Drawing_IN)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " [Status] IN  (" + (int)EnumFabricBatchState.Sizing_Finish + "," + (int)EnumFabricBatchState.DrawingIn + ")";
            }
            else if (oFabricBatch.WeavingProcessType == (int)EnumWeavingProcess.Loom)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " [Status] IN  (" + (int)EnumFabricBatchState.Sizing_Finish + "," + (int)EnumFabricBatchState.DrawingIn_Finish + "," + (int)EnumFabricBatchState.Weaving + ")";
            }
            //else if (oFabricBatch.WeavingProcessType == -1) // -1 means In Fabric QC
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " [Status] IN  (" + (int)EnumFabricBatchState.Sizing_Finish + "," + (int)EnumFabricBatchState.DrawingIn_Finish + "," + (int)EnumFabricBatchState.Weaving_Finish + "," + (int)EnumFabricBatchState.In_QC + "," + (int)EnumFabricBatchState.InStore + ")";
            //    //sReturn = sReturn + " [Status] IN  (" + (int)EnumFabricBatchState.DrawingIn_Finish + "," + (int)EnumFabricBatchState.Weaving_Finish + "," + (int)EnumFabricBatchState.In_QC + "," + (int)EnumFabricBatchState.InStore + ")";
            //}
            #endregion

            #region Not finished batch
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " [Status] != " + (int)EnumFabricBatchState.Finish;
            #endregion

            string sSQL = sReturn1 + " " + sReturn + " ORDER BY BatchNo";
            return sSQL;
        }

        #region FEYR

        [HttpPost]
        public JsonResult GetProducts(Lot oLot)
        {
            List<Product> oProducts = new List<Product>();
       
            try
            {
                string sSQL = "SELECT * FROM View_Product";
                string sSQL1 = "";

                //#region BUID
                //if (oLot.BUID > 0)//if apply style configuration business unit
                //{
                //    Global.TagSQL(ref sSQL1);
                //    sSQL1 = sSQL1 + " ProductCategoryID IN (SELECT ProductCategoryID FROM  BUWiseProductCategory WHERE BUID =" + oLot.BUID.ToString() + ")";
                //}
                //#endregion

                #region ProductName

                if (!string.IsNullOrEmpty(oLot.ProductName))
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " ProductName LIKE '%" + oLot.ProductName + "%'";
                }
                #endregion

                #region Deafult
                Global.TagSQL(ref sSQL1);
                sSQL1 = sSQL1 + " Activity=1";
                #endregion

                #region Style Wise Suggested Product
                if (oLot.WorkingUnitID > 0) 
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " ProductID in (Select ProductID from Lot where Balance>0.1 and WorkingUnitID=" + oLot.WorkingUnitID+ ")";
                }
                #endregion

                sSQL = sSQL + sSQL1 + " Order By ProductName ASC";

                oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oProducts.Count() <= 0) throw new Exception("No product found.");

            

            }
            catch (Exception ex)
            {
                Product oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

       
        #endregion

        #region Fabric Batch QC Fault
        [HttpPost]
        public JsonResult SaveFabricBatchQCFault(FabricBatchQCFault oFabricBatchQCFault)
        {
            try
            {
                oFabricBatchQCFault = oFabricBatchQCFault.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricBatchQCFault = new FabricBatchQCFault();
                oFabricBatchQCFault.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchQCFault);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteFabricBatchQCFault(FabricBatchQCFault oFabricBatchQCFault)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabricBatchQCFault.Delete(oFabricBatchQCFault.FBQCFaultID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult SaveMultipleFBQCFault(List<FabricBatchQCFault> oFabricBatchQCFaults)
        {
            List<FabricBatchQCFault> _oFabricBatchQCFaults = new List<FabricBatchQCFault>();
            try
            {
                if (oFabricBatchQCFaults.Count > 0)
                {
                    _oFabricBatchQCFaults = FabricBatchQCFault.SaveMultipleFNBatchQCFault(oFabricBatchQCFaults, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                FabricBatchQCFault oFabricBatchQCFault = new FabricBatchQCFault();
                oFabricBatchQCFault.ErrorMessage = ex.Message;
                _oFabricBatchQCFaults.Add(oFabricBatchQCFault);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchQCFaults);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsFabricBatchQCFault(FabricBatchQCFault oFabricBatchQCFault)
        {
            List<FabricBatchQCFault> oFabricBatchQCFaults = new List<FabricBatchQCFault>();
            try
            {
                string sSQL = "SELECT * FROM View_FabricBatchQCFault WHERE FBQCDetailID = " + oFabricBatchQCFault.FBQCDetailID;
                oFabricBatchQCFaults = FabricBatchQCFault.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricBatchQCFault = new FabricBatchQCFault();
                oFabricBatchQCFault.ErrorMessage = ex.Message;
                oFabricBatchQCFaults.Add(oFabricBatchQCFault);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricBatchQCFaults);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsByDBServerDate(FabricBatchQC oFabricBatchQC)
        {
            _oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
            try
            {
                string sSQL = "SELECT * FROM View_FabricBatchQCDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oFabricBatchQC.QCStartDateTime.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oFabricBatchQC.QCStartDateTime.ToString("dd MMM yyyy") + "',106)) AND FBQCID IN (SELECT FBQCID FROM FabricBatchQC WHERE FBID=" + oFabricBatchQC.FBID + ")";
                _oFabricBatchQCDetails = FabricBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatchQCDetail = new FabricBatchQCDetail();
                _oFabricBatchQCDetail.ErrorMessage = ex.Message;
                _oFabricBatchQCDetails.Add(_oFabricBatchQCDetail);
            }
            var jsonResult = Json(_oFabricBatchQCDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion
        private bool CheckFabricBatch(FabricBatch oFabricBatch)
        {
            if (oFabricBatch.Status != EnumFabricBatchState.DrawingIn_Finish)
            {
                if (oFabricBatch.Status != EnumFabricBatchState.Weaving_Finish)
                {
                    if (oFabricBatch.Status != EnumFabricBatchState.In_QC)
                    {
                        if (oFabricBatch.Status != EnumFabricBatchState.InStore)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        #region RTN
        [HttpPost]
        public JsonResult OutRowMaterials(FabricBatch oFabricBatch)
        {
            try
            {
                oFabricBatch = oFabricBatch.RowMatarialOut(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        #endregion

        #region Create Fabric Batch From Warping Plan

      
        [HttpPost]
        public JsonResult GetLastFabricBatchByWarpPlane(FabricBatch oFabricBatch)
        {
            try
            {
                List<FabricBatch> oFBs = new List<FabricBatch>();
                if (oFabricBatch.FWPDID <= 0)
                    throw new Exception("Select a valid fabric warping plan .");

                oFBs = FabricBatch.Gets(" SELECT top(1)* FROM View_FabricBatch WHERE FWPDID =" + oFabricBatch.FWPDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFBs.Any())
                    oFabricBatch = oFBs.First();
                else
                    oFabricBatch = new FabricBatch();
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
        public JsonResult SaveFabricBatchFromWarpPlan(FabricBatch oFabricBatch)
        {
            try
            {
                if (oFabricBatch.FWPDID <= 0)
                    throw new Exception("Invalid fabric warping plan detail.");
                oFabricBatch = oFabricBatch.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        #endregion

        #region Fabric Batch QC Register
        public ActionResult ViewFabricBatchRegister(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oFabricBatchQC = new FabricBatchQC();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            string _sSQL = "SELECT * FROM View_FabricQCGrade Order By FabricQCGradeID";
            ViewBag.FabricQCGrades = FabricQCGrade.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;

            return View(_oFabricBatchQC);
        }

        public ActionResult SetSessionSearchCriteria(FabricBatchQC oFabricBatchQC)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oFabricBatchQC);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintStockReport(double ts)
        {
            FabricBatchQC oFabricBatchQC = new FabricBatchQC();
            _oFabricBatchQCs = new List<FabricBatchQC>();
            _oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
            List<FabricExecutionOrderSpecification> _oEOSs = new List<FabricExecutionOrderSpecification>();
            List<FabricQCGrade> oFabricQCGrades = new List<FabricQCGrade>();
            string _sErrorMesage = ""; string sPrintHeader = ""; string strParam = ""; string _sSQL = "";
            try
            {                
                oFabricBatchQC = (FabricBatchQC)Session[SessionInfo.ParamObj];
                strParam = oFabricBatchQC.ErrorMessage;
                string sSQL = this.GetSQL(oFabricBatchQC);

                _oFabricBatchQCDetails = FabricBatchQCDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oFabricBatchQCDetails.Count > 0)
                {
                    _oFabricBatchQCs = FabricBatchQC.Gets("SELECT * FROM View_FabricBatchQC where FBQCID in (" + string.Join(",", _oFabricBatchQCDetails.Select(x => x.FBQCID)) + ")", (int)Session[SessionInfo.currentUserID]);
                    _sSQL = "SELECT * FROM View_FabricExecutionOrderSpecification WHERE FEOSID IN (" + string.Join(",", _oFabricBatchQCDetails.Select(x => x.FEOSID)) + ")";
                    _oEOSs = FabricExecutionOrderSpecification.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);
                    oFabricQCGrades = FabricQCGrade.Gets("SELECT * FROM View_FabricQCGrade ORDER BY QCGradeType, SLNo", (int)Session[SessionInfo.currentUserID]);
                }
                if (_oFabricBatchQCDetails.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oFabricBatchQCs = new List<FabricBatchQC>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);               
                //if (oFabricBatchQC.BUID > 0)
                //{
                //    BusinessUnit oBU = new BusinessUnit();
                //    oBU = oBU.Get(oFabricBatchQC.BUID, (int)Session[SessionInfo.currentUserID]);
                //    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                //}

                int nProdDate = Convert.ToInt32(strParam.Split('~')[0]);
                int nDeliveryDate = Convert.ToInt32(strParam.Split('~')[3]);
                int nRcvDate = Convert.ToInt32(strParam.Split('~')[6]);
                #region Production Date
                if (nProdDate > 0)
                {
                    DateTime nProdStartDate = Convert.ToDateTime(strParam.Split('~')[1]);
                    DateTime nProdEndDate = Convert.ToDateTime(strParam.Split('~')[2]);
                    if (nProdDate == 1)
                    {
                        if (sPrintHeader == "")
                        {
                            sPrintHeader += "Production Date " + nProdStartDate.ToString("dd MMM yyyy"); 
                        }
                        else
                        {
                            sPrintHeader += ", Production Date " + nProdStartDate.ToString("dd MMM yyyy"); 
                        }
                    }
                    if (nProdDate == 5)
                    {
                        if (sPrintHeader == "")
                        {
                            sPrintHeader += "Production Date Between " + nProdStartDate.ToString("dd MMM yyyy") + " To " + nProdEndDate.ToString("dd MMM yyyy");
                        }
                        else
                        {
                            sPrintHeader += ",Production Date Between " + nProdStartDate.ToString("dd MMM yyyy") + " To " + nProdEndDate.ToString("dd MMM yyyy");
                        }
                    }
                }
                #endregion
                #region delivery Date
                if (nDeliveryDate > 0)
                {
                    DateTime nDeliveryStartDate = Convert.ToDateTime(strParam.Split('~')[4]);
                    DateTime nDeliveryEndDate = Convert.ToDateTime(strParam.Split('~')[5]);
                    if (nDeliveryDate == 1)
                    {
                        if (sPrintHeader == "")
                        {
                            sPrintHeader += "Delivery Date " + nDeliveryStartDate.ToString("dd MMM yyyy");
                        }
                        else
                        {
                            sPrintHeader += ", Delivery Date " + nDeliveryStartDate.ToString("dd MMM yyyy");
                        }
                    }
                    if (nDeliveryDate == 5)
                    {
                        if (sPrintHeader == "")
                        {
                            sPrintHeader += "Delivery Date Between " + nDeliveryStartDate.ToString("dd MMM yyyy") + " To " + nDeliveryEndDate.ToString("dd MMM yyyy");
                        }
                        else
                        {
                            sPrintHeader += ", Delivery Date Between " + nDeliveryStartDate.ToString("dd MMM yyyy") + " To " + nDeliveryEndDate.ToString("dd MMM yyyy");
                        }
                    }
                }
                #endregion
                #region Receive Date
                if (nRcvDate > 0)
                {
                    DateTime nRcvStartDate = Convert.ToDateTime(strParam.Split('~')[7]);
                    DateTime nRcvEndDate = Convert.ToDateTime(strParam.Split('~')[8]);
                    if (nRcvDate == 1)
                    {
                        if (sPrintHeader == "")
                        {
                            sPrintHeader += "Receive Date " + nRcvStartDate;
                        }
                        else
                        {
                            sPrintHeader += ", Receive Date " + nRcvStartDate;
                        }
                    }
                    if (nRcvDate == 5)
                    {
                        if (sPrintHeader == "")
                        {
                            sPrintHeader += "Receive Date Between " + nRcvStartDate + " To " + nRcvEndDate;
                        }
                        else
                        {
                            sPrintHeader += ", Receive Date Between " + nRcvStartDate + " To " + nRcvEndDate;
                        }
                    }
                }
                #endregion



                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptFabricBatchQCStockReport oReport = new rptFabricBatchQCStockReport();
                byte[] abytes = oReport.PrepareReport(_oFabricBatchQCs, _oFabricBatchQCDetails, _oEOSs, oCompany, sPrintHeader, oFabricQCGrades);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            }
        }
        public ActionResult PrintGreigeInspectionReport(double ts)
        {
            FabricBatchQC oFabricBatchQC = new FabricBatchQC();
            _oFabricBatchQCs = new List<FabricBatchQC>();
            _oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
            List<FabricExecutionOrderSpecification> _oEOSs = new List<FabricExecutionOrderSpecification>();
            List<FabricQCGrade> oFabricQCGrades = new List<FabricQCGrade>();
            List<FabricBatchQCFault> _oFabricBatchQCFault = new List<FabricBatchQCFault>();
            string _sErrorMesage = ""; string sPrintHeader = ""; string strParam = ""; string _sSQL = "";
            try
            {
                oFabricBatchQC = (FabricBatchQC)Session[SessionInfo.ParamObj];
                strParam = oFabricBatchQC.ErrorMessage;
                string sSQL = this.GetSQL(oFabricBatchQC);

                _oFabricBatchQCDetails = FabricBatchQCDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oFabricBatchQCDetails.Count > 0)
                {
                    _oFabricBatchQCs = FabricBatchQC.Gets("SELECT * FROM View_FabricBatchQC where FBQCID in (" + string.Join(",", _oFabricBatchQCDetails.Select(x => x.FBQCID)) + ")", (int)Session[SessionInfo.currentUserID]);
                    _sSQL = "SELECT * FROM View_FabricExecutionOrderSpecification WHERE FEOSID IN (" + string.Join(",", _oFabricBatchQCDetails.Select(x => x.FEOSID)) + ")";
                    _oEOSs = FabricExecutionOrderSpecification.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);
                    oFabricQCGrades = FabricQCGrade.Gets("SELECT * FROM View_FabricQCGrade ORDER BY QCGradeType, SLNo", (int)Session[SessionInfo.currentUserID]);
                    _oFabricBatchQCFault = FabricBatchQCFault.Gets("SELECT * FROM View_FabricBatchQCFault where FBQCDetailID in (" + string.Join(",", _oFabricBatchQCDetails.Select(x => x.FBQCDetailID)) + ")", (int)Session[SessionInfo.currentUserID]);
                }
                if (_oFabricBatchQCDetails.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oFabricBatchQCs = new List<FabricBatchQC>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                int nProdDate = Convert.ToInt32(strParam.Split('~')[0]);
                int nDeliveryDate = Convert.ToInt32(strParam.Split('~')[3]);
                int nRcvDate = Convert.ToInt32(strParam.Split('~')[6]);
                #region Production Date
                if (nProdDate > 0)
                {
                    DateTime nProdStartDate = Convert.ToDateTime(strParam.Split('~')[1]);
                    DateTime nProdEndDate = Convert.ToDateTime(strParam.Split('~')[2]);
                    if (nProdDate == 1)
                    {
                        if (sPrintHeader == "")
                        {
                            sPrintHeader += "Production Date " + nProdStartDate.ToString("dd MMM yyyy");
                        }
                        else
                        {
                            sPrintHeader += ", Production Date " + nProdStartDate.ToString("dd MMM yyyy");
                        }
                    }
                    if (nProdDate == 5)
                    {
                        if (sPrintHeader == "")
                        {
                            sPrintHeader += "Production Date Between " + nProdStartDate.ToString("dd MMM yyyy") + " To " + nProdEndDate.ToString("dd MMM yyyy");
                        }
                        else
                        {
                            sPrintHeader += ",Production Date Between " + nProdStartDate.ToString("dd MMM yyyy") + " To " + nProdEndDate.ToString("dd MMM yyyy");
                        }
                    }
                }
                #endregion
                #region delivery Date
                if (nDeliveryDate > 0)
                {
                    DateTime nDeliveryStartDate = Convert.ToDateTime(strParam.Split('~')[4]);
                    DateTime nDeliveryEndDate = Convert.ToDateTime(strParam.Split('~')[5]);
                    if (nDeliveryDate == 1)
                    {
                        if (sPrintHeader == "")
                        {
                            sPrintHeader += "Delivery Date " + nDeliveryStartDate.ToString("dd MMM yyyy");
                        }
                        else
                        {
                            sPrintHeader += ", Delivery Date " + nDeliveryStartDate.ToString("dd MMM yyyy");
                        }
                    }
                    if (nDeliveryDate == 5)
                    {
                        if (sPrintHeader == "")
                        {
                            sPrintHeader += "Delivery Date Between " + nDeliveryStartDate.ToString("dd MMM yyyy") + " To " + nDeliveryEndDate.ToString("dd MMM yyyy");
                        }
                        else
                        {
                            sPrintHeader += ", Delivery Date Between " + nDeliveryStartDate.ToString("dd MMM yyyy") + " To " + nDeliveryEndDate.ToString("dd MMM yyyy");
                        }
                    }
                }
                #endregion
                #region Receive Date
                if (nRcvDate > 0)
                {
                    DateTime nRcvStartDate = Convert.ToDateTime(strParam.Split('~')[7]);
                    DateTime nRcvEndDate = Convert.ToDateTime(strParam.Split('~')[8]);
                    if (nRcvDate == 1)
                    {
                        if (sPrintHeader == "")
                        {
                            sPrintHeader += "Receive Date " + nRcvStartDate;
                        }
                        else
                        {
                            sPrintHeader += ", Receive Date " + nRcvStartDate;
                        }
                    }
                    if (nRcvDate == 5)
                    {
                        if (sPrintHeader == "")
                        {
                            sPrintHeader += "Receive Date Between " + nRcvStartDate + " To " + nRcvEndDate;
                        }
                        else
                        {
                            sPrintHeader += ", Receive Date Between " + nRcvStartDate + " To " + nRcvEndDate;
                        }
                    }
                }
                #endregion


                List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
                oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.GreigeInspectionReport, (int)Session[SessionInfo.currentUserID]);  
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                sPrintHeader = "Daily Greige Inspection Loom Wise " + _oFabricBatchQCDetails[0].QCGrade + " & Rejection Report"; 
                rptGreigeInspectionReport oReport = new rptGreigeInspectionReport();
                byte[] abytes = oReport.PrepareReport(_oFabricBatchQCs, _oFabricBatchQCDetails, _oEOSs, oCompany, sPrintHeader, oFabricQCGrades, _oFabricBatchQCFault, oSignatureSetups);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        private string GetSQL(FabricBatchQC oFabricBatchQC)
        {
            //string _sDateRange = "";
            string sSearchingData = oFabricBatchQC.ErrorMessage;
            EnumCompareOperator eProductionDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dProductionDateStart = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dProductionDateEnd = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            EnumCompareOperator eDeliveryDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dDeliveryDateStart = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dDeliveryDateEnd = Convert.ToDateTime(sSearchingData.Split('~')[5]);
            EnumCompareOperator eReceiveDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            DateTime dReceiveDateStart = Convert.ToDateTime(sSearchingData.Split('~')[7]);
            DateTime dReceiveDateEnd = Convert.ToDateTime(sSearchingData.Split('~')[8]);
            int nType = Convert.ToInt32(sSearchingData.Split('~')[9]);

            string sSQLQuery = "", sReturn = "";

            sSQLQuery = "SELECT * FROM View_FabricBatchQCDetail";

            if (string.IsNullOrEmpty(oFabricBatchQC.DispoNo ))
            {
                Global.TagSQL(ref sReturn);
                sReturn += "  DispoNo LIKE '%" + oFabricBatchQC.DispoNo + "%'";
            }
            if (string.IsNullOrEmpty(oFabricBatchQC.BatchNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn += "  BatchNo LIKE '%" + oFabricBatchQC.BatchNo + "%'";
            }

            #region Production Date
            if (eProductionDate != EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (eProductionDate == EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ProDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dProductionDateStart.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (eProductionDate == EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ProDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dProductionDateStart.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (eProductionDate == EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ProDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dProductionDateStart.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (eProductionDate == EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ProDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dProductionDateStart.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (eProductionDate == EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ProDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dProductionDateStart.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dProductionDateEnd.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (eProductionDate == EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ProDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dProductionDateStart.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dProductionDateEnd.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region Delivery Date
            if (eDeliveryDate != EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (eDeliveryDate == EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dDeliveryDateStart.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (eDeliveryDate == EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dDeliveryDateStart.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (eDeliveryDate == EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dDeliveryDateStart.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (eDeliveryDate == EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dDeliveryDateStart.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (eDeliveryDate == EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dDeliveryDateStart.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dDeliveryDateEnd.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (eDeliveryDate == EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dDeliveryDateStart.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dDeliveryDateEnd.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region Receive Date
            if (eReceiveDate != EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (eReceiveDate == EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dReceiveDateStart.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (eReceiveDate == EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dReceiveDateStart.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (eReceiveDate == EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dReceiveDateStart.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (eReceiveDate == EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dReceiveDateStart.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (eReceiveDate == EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dReceiveDateStart.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dReceiveDateEnd.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (eReceiveDate == EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dReceiveDateStart.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dReceiveDateEnd.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region Type
            if (nType > 0)
            {
                if (nType == 1)  // Stock In Hand
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LotID IN (SELECT LotID FROM Lot WHERE ISNULL(Balance,0) > 0)";
                }
                
            }
            #endregion

            #region QCGrade
            if (oFabricBatchQC.FBGradeID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn += "  FabricQCGradeID =" + oFabricBatchQC.FBGradeID;
            }
            #endregion
            sSQLQuery = sSQLQuery + sReturn;
            return sSQLQuery;
        }

        #endregion

        public ActionResult WeavingStatement(int nFBID)
        {
            _oFabricBatch = new FabricBatch();
            List<FabricBatch> oFabricBatchs = new List<FabricBatch>();
            FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
            FabricExecutionOrderSpecification oFEOS = new FabricExecutionOrderSpecification();
            List<FabricWarpPlan> oFabricWarpPlans = new List<FabricWarpPlan>();
            List<FabricBatchProduction> oWarpingExecutions = new List<FabricBatchProduction>();
            List<FabricSizingPlan> oFabricSizingPlans = new List<FabricSizingPlan>();
            List<FabricBatchProduction> oSizingExecutions = new List<FabricBatchProduction>();
            List<FabricBatchProduction> oDrawingExecutions = new List<FabricBatchProduction>();
            List<FabricBatchProduction> oLeasingExecutions = new List<FabricBatchProduction>();
            List<FabricLoomPlan> oFabricLoomPlans = new List<FabricLoomPlan>();
            List<FabricBatchLoom> oFabricBatchLooms = new List<FabricBatchLoom>();
            List<FabricBatchQC> oFabricBatchQCs = new List<FabricBatchQC>();
            Company oCompany = new Company();
            if (nFBID > 0)
            {
                _oFabricBatch = _oFabricBatch.Get(nFBID, (int)Session[SessionInfo.currentUserID]);
                oFabricBatchs.Add(_oFabricBatch);
                oFabricSalesContractDetail = oFabricSalesContractDetail.Get(_oFabricBatch.FEOID, (int)Session[SessionInfo.currentUserID]);
                oFEOS = FabricExecutionOrderSpecification.Gets("SELECT TOP 1 * FROM View_FabricExecutionOrderSpecification WHERE FSCDID = " + oFabricSalesContractDetail.FabricSalesContractDetailID + " AND ProdtionType = " + (int)EnumDispoProType.General, (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
                oFabricWarpPlans = FabricWarpPlan.Gets("SELECT * FROM View_FabricWarpPlan WHERE  FSCDID = " + oFabricSalesContractDetail.FabricSalesContractDetailID, (int)Session[SessionInfo.currentUserID]);
                oWarpingExecutions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE FEOID = " + oFabricSalesContractDetail.FabricSalesContractDetailID + " AND WeavingProcess = 0", (int)Session[SessionInfo.currentUserID]);
                oFabricSizingPlans = FabricSizingPlan.Gets("SELECT * FROM View_FabricSizingPlan WHERE FSCDID = " + oFabricSalesContractDetail.FabricSalesContractDetailID, (int)Session[SessionInfo.currentUserID]);
                oSizingExecutions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE FEOID = " + oFabricSalesContractDetail.FabricSalesContractDetailID + " AND WeavingProcess = 1", (int)Session[SessionInfo.currentUserID]);
                oDrawingExecutions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE FEOID = " + oFabricSalesContractDetail.FabricSalesContractDetailID + " AND WeavingProcess = 2", (int)Session[SessionInfo.currentUserID]);
                oLeasingExecutions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE FEOID = " + oFabricSalesContractDetail.FabricSalesContractDetailID + " AND WeavingProcess = 4", (int)Session[SessionInfo.currentUserID]);
                oFabricLoomPlans = FabricLoomPlan.Gets("SELECT * FROM View_FabricLoomPlan WHERE FSCDID = " + oFabricSalesContractDetail.FabricSalesContractDetailID, (int)Session[SessionInfo.currentUserID]);
                oFabricBatchLooms = FabricBatchLoom.Gets("SELECT * FROM View_FabricBatchLoom WHERE FLPID IN (" + string.Join(",", oFabricLoomPlans.Select(x => x.FLPID)) + ")", (int)Session[SessionInfo.currentUserID]);
                oFabricBatchQCs = FabricBatchQC.Gets("SELECT * FROM View_FabricBatchQC WHERE FEOID = " + oFabricSalesContractDetail.FabricSalesContractDetailID, (int)Session[SessionInfo.currentUserID]);
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            rptWeavingStatement oReport = new rptWeavingStatement();
            byte[] abytes = oReport.PrepareReport(oFabricBatchs, oFabricSalesContractDetail, oFEOS, oCompany, oFabricWarpPlans, oWarpingExecutions, oFabricSizingPlans, oSizingExecutions, oDrawingExecutions, oLeasingExecutions, oFabricLoomPlans, oFabricBatchLooms, oFabricBatchQCs);
            return File(abytes, "application/pdf");
        }

        public ActionResult WeavingStatementByFSCDID(int nFSCDID)
        {
            _oFabricBatch = new FabricBatch();
            List<FabricBatch> oFabricBatchs = new List<FabricBatch>();
            FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
            FabricExecutionOrderSpecification oFEOS = new FabricExecutionOrderSpecification();
            List<FabricWarpPlan> oFabricWarpPlans = new List<FabricWarpPlan>();
            List<FabricBatchProduction> oWarpingExecutions = new List<FabricBatchProduction>();
            List<FabricSizingPlan> oFabricSizingPlans = new List<FabricSizingPlan>();
            List<FabricBatchProduction> oSizingExecutions = new List<FabricBatchProduction>();
            List<FabricBatchProduction> oDrawingExecutions = new List<FabricBatchProduction>();
            List<FabricBatchProduction> oLeasingExecutions = new List<FabricBatchProduction>();
            List<FabricLoomPlan> oFabricLoomPlans = new List<FabricLoomPlan>();
            List<FabricBatchLoom> oFabricBatchLooms = new List<FabricBatchLoom>();
            List<FabricBatchQC> oFabricBatchQCs = new List<FabricBatchQC>();
            Company oCompany = new Company();
            if (nFSCDID > 0)
            {
                oFabricBatchs = FabricBatch.Gets("SELECT * FROM View_FabricBatch WHERE FEOID = " + nFSCDID, (int)Session[SessionInfo.currentUserID]);
                oFabricSalesContractDetail = oFabricSalesContractDetail.Get(nFSCDID, (int)Session[SessionInfo.currentUserID]);
                oFEOS = FabricExecutionOrderSpecification.Gets("SELECT TOP 1 * FROM View_FabricExecutionOrderSpecification WHERE FSCDID = " + oFabricSalesContractDetail.FabricSalesContractDetailID + " AND ProdtionType = " + (int)EnumDispoProType.General, (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
                oFabricWarpPlans = FabricWarpPlan.Gets("SELECT * FROM View_FabricWarpPlan WHERE  FSCDID = " + oFabricSalesContractDetail.FabricSalesContractDetailID, (int)Session[SessionInfo.currentUserID]);
                oWarpingExecutions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE FEOID = " + oFabricSalesContractDetail.FabricSalesContractDetailID + " AND WeavingProcess = 0", (int)Session[SessionInfo.currentUserID]);
                oFabricSizingPlans = FabricSizingPlan.Gets("SELECT * FROM View_FabricSizingPlan WHERE FSCDID = " + oFabricSalesContractDetail.FabricSalesContractDetailID, (int)Session[SessionInfo.currentUserID]);
                oSizingExecutions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE FEOID = " + oFabricSalesContractDetail.FabricSalesContractDetailID + " AND WeavingProcess = 1", (int)Session[SessionInfo.currentUserID]);
                oDrawingExecutions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE FEOID = " + oFabricSalesContractDetail.FabricSalesContractDetailID + " AND WeavingProcess = 2", (int)Session[SessionInfo.currentUserID]);
                oLeasingExecutions = FabricBatchProduction.Gets("SELECT * FROM View_FabricBatchProduction WHERE FEOID = " + oFabricSalesContractDetail.FabricSalesContractDetailID + " AND WeavingProcess = 4", (int)Session[SessionInfo.currentUserID]);
                oFabricLoomPlans = FabricLoomPlan.Gets("SELECT * FROM View_FabricLoomPlan WHERE FSCDID = " + oFabricSalesContractDetail.FabricSalesContractDetailID, (int)Session[SessionInfo.currentUserID]);
                oFabricBatchLooms = FabricBatchLoom.Gets("SELECT * FROM View_FabricBatchLoom WHERE FLPID IN (" + string.Join(",", oFabricLoomPlans.Select(x => x.FLPID)) + ")", (int)Session[SessionInfo.currentUserID]);
                oFabricBatchQCs = FabricBatchQC.Gets("SELECT * FROM View_FabricBatchQC WHERE FEOID = " + oFabricSalesContractDetail.FabricSalesContractDetailID, (int)Session[SessionInfo.currentUserID]);
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            rptWeavingStatement oReport = new rptWeavingStatement();
            byte[] abytes = oReport.PrepareReport(oFabricBatchs, oFabricSalesContractDetail, oFEOS, oCompany, oFabricWarpPlans, oWarpingExecutions, oFabricSizingPlans, oSizingExecutions, oDrawingExecutions, oLeasingExecutions, oFabricLoomPlans, oFabricBatchLooms, oFabricBatchQCs);
            return File(abytes, "application/pdf");
        }

        private string MakeSQLFromReport(FabricBatchQCDetail oFabricBatchQCDetail)
        {
            DateTime dProductionStartDate = DateTime.Now;
            DateTime dProductionEndDate = DateTime.Now;
            DateTime dDeliveryStartDate = DateTime.Now;
            DateTime dDeliveryEndDate = DateTime.Now;
            DateTime dStoreRcvStartDate = DateTime.Now;
            DateTime dStoreRcvEndDate = DateTime.Now;
            DateTime dIssueRcvStartDate = DateTime.Now;
            DateTime dIssueRcvEndDate = DateTime.Now;

            string sFabricQCGradeIDs = oFabricBatchQCDetail.ErrorMessage.Split('~')[0];

            EnumCompareOperator eProductionDate = (EnumCompareOperator)Convert.ToInt32(oFabricBatchQCDetail.ErrorMessage.Split('~')[1]);
            if (eProductionDate != EnumCompareOperator.None)
            {
                dProductionStartDate = Convert.ToDateTime(oFabricBatchQCDetail.ErrorMessage.Split('~')[2]);
                dProductionEndDate = Convert.ToDateTime(oFabricBatchQCDetail.ErrorMessage.Split('~')[3]);
            }
            EnumCompareOperator eDeliveryDate = (EnumCompareOperator)Convert.ToInt32(oFabricBatchQCDetail.ErrorMessage.Split('~')[4]);
            if (eDeliveryDate != EnumCompareOperator.None)
            {
                dDeliveryStartDate = Convert.ToDateTime(oFabricBatchQCDetail.ErrorMessage.Split('~')[5]);
                dDeliveryEndDate = Convert.ToDateTime(oFabricBatchQCDetail.ErrorMessage.Split('~')[6]);
            }

            EnumCompareOperator eStoreRcvDate = (EnumCompareOperator)Convert.ToInt32(oFabricBatchQCDetail.ErrorMessage.Split('~')[7]);
            if (eStoreRcvDate != EnumCompareOperator.None)
            {
                dStoreRcvStartDate = Convert.ToDateTime(oFabricBatchQCDetail.ErrorMessage.Split('~')[8]);
                dStoreRcvEndDate = Convert.ToDateTime(oFabricBatchQCDetail.ErrorMessage.Split('~')[9]);
            }

            EnumCompareOperator eIssueRcvDate = (EnumCompareOperator)Convert.ToInt32(oFabricBatchQCDetail.ErrorMessage.Split('~')[10]);
            if (eIssueRcvDate != EnumCompareOperator.None)
            {
                dIssueRcvStartDate = Convert.ToDateTime(oFabricBatchQCDetail.ErrorMessage.Split('~')[11]);
                dIssueRcvEndDate = Convert.ToDateTime(oFabricBatchQCDetail.ErrorMessage.Split('~')[12]);

            }
            oFabricBatchQCDetail.ShiftID = Convert.ToInt32(oFabricBatchQCDetail.ErrorMessage.Split('~')[13]);

            string sReturn1 = "SELECT * FROM View_FabricBatchQCDetail AS HH";
            string sReturn = "";

            #region FABRIC QC
            if (!String.IsNullOrEmpty(sFabricQCGradeIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.FabricQCGradeID IN(" + sFabricQCGradeIDs + ")";

            }
            #endregion
            #region SHIFT
            if (oFabricBatchQCDetail.ShiftID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.ShiftID = " + oFabricBatchQCDetail.ShiftID;

            }
            #endregion

            #region ProductionDate
            if (eProductionDate != EnumCompareOperator.None)
            {
                if (eProductionDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.ProDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dProductionStartDate.ToString("dd MMM yyyy h:mm tt") + "',106))";
                    _sDateRangeText = "Production Date: " + dProductionStartDate.ToString("dd MMM yyyy");
                }
                else if (eProductionDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.ProDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dProductionStartDate.ToString("dd MMM yyyy h:mm tt") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dProductionEndDate.ToString("dd MMM yyyy h:mm tt") + "',106))";
                    _sDateRangeText = "Production Date Between: " + dProductionStartDate.ToString("dd MMM yyyy") + " AND " + dProductionEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion
            #region Delivery Date
            if (eDeliveryDate != EnumCompareOperator.None)
            {
                if (eDeliveryDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.DeliveryDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dDeliveryStartDate.ToString("dd MMM yyyy h:mm tt") + "',106))";
                    _sDateRangeText = "Delivery Date: " + dDeliveryStartDate.ToString("dd MMM yyyy");
                }
                else if (eDeliveryDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.DeliveryDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dDeliveryStartDate.ToString("dd MMM yyyy h:mm tt") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dDeliveryEndDate.ToString("dd MMM yyyy h:mm tt") + "',106))";
                    _sDateRangeText = "Delivery Date Between: " + dDeliveryStartDate.ToString("dd MMM yyyy") + " AND " + dDeliveryEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion
            #region Store Rcv Date
            if (eStoreRcvDate != EnumCompareOperator.None)
            {
                if (eStoreRcvDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.StoreRcvDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStoreRcvStartDate.ToString("dd MMM yyyy h:mm tt") + "',106))";
                    _sDateRangeText = "Store Rcv. Date: " + dStoreRcvStartDate.ToString("dd MMM yyyy");
                }
                else if (eStoreRcvDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.StoreRcvDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStoreRcvStartDate.ToString("dd MMM yyyy h:mm tt") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStoreRcvEndDate.ToString("dd MMM yyyy h:mm tt") + "',106))";
                    _sDateRangeText = "Store Rcv. Date Between: " + dStoreRcvStartDate.ToString("dd MMM yyyy") + " AND " + dStoreRcvEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion
            #region Issue Date
            if (eIssueRcvDate != EnumCompareOperator.None)
            {
                if (eIssueRcvDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.DBServerDateTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueRcvStartDate.ToString("dd MMM yyyy h:mm tt") + "',106))";
                    _sDateRangeText = "Issue Rcv. Date: " + dIssueRcvStartDate.ToString("dd MMM yyyy");
                }
                else if (eIssueRcvDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.DBServerDateTime,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueRcvStartDate.ToString("dd MMM yyyy h:mm tt") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueRcvEndDate.ToString("dd MMM yyyy h:mm tt") + "',106))";
                    _sDateRangeText = "Issue Rcv. Date Between: " + dIssueRcvStartDate.ToString("dd MMM yyyy") + " AND " + dIssueRcvEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion
            sReturn = sReturn1 + sReturn + " AND ISNULL(GradeSL,0) != 0 ORDER BY FBQCDetailID ASC";

            return sReturn;
        }

        public ActionResult GreigeReport(string sParams, bool bIsFromReport)
        {
            _oFabricBatchQC = new FabricBatchQC();
            _oFabricBatchQCs = new List<FabricBatchQC>();
            _oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
            List<FabricQCGrade> oFabricQCGrades = new List<FabricQCGrade>();
            Company oCompany = new Company();
            try
            {
                string sSQL = "";
                if (!bIsFromReport)
                {
                    _oFabricBatchQC.ErrorMessage = sParams;
                    sSQL = MakeSQL(_oFabricBatchQC);
                    _oFabricBatchQCs = FabricBatchQC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oFabricBatchQCDetails = FabricBatchQCDetail.Gets("SELECT * FROM View_FabricBatchQCDetail WHERE ISNULL(GradeSL,0) != 0 AND FBQCID IN (" + string.Join(",", _oFabricBatchQCs.Select(x => x.FBQCID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    FabricBatchQCDetail oFabricBatchQCDetail = new FabricBatchQCDetail();
                    oFabricBatchQCDetail.ErrorMessage = sParams;
                    sSQL = MakeSQLFromReport(oFabricBatchQCDetail);
                    _oFabricBatchQCDetails = FabricBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oFabricBatchQCs = FabricBatchQC.Gets("SELECT * FROM View_FabricBatchQC WHERE FBQCID IN (" + string.Join(",", _oFabricBatchQCDetails.Select(x => x.FBQCID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                oFabricQCGrades = FabricQCGrade.Gets("SELECT * FROM View_FabricQCGrade ORDER BY QCGradeType", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBatchQC = new FabricBatchQC();
                _oFabricBatchQCs = new List<FabricBatchQC>();
            }
            if (_oFabricBatchQCs.Count > 0)
            {
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                byte[] abytes;
                rptPrintGreigeReport oReport = new rptPrintGreigeReport();
                abytes = oReport.PrepareReport(_oFabricBatchQCs, _oFabricBatchQCDetails, oFabricQCGrades, oCompany, _sDateRangeText);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }
            
        }

    }
}