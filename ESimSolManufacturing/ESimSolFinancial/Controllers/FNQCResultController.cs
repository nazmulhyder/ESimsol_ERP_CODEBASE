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
    public class FNQCResultController : Controller
    {
        #region Declaration
        FNQCResult _oFNQCResult = new FNQCResult();
        List<FNQCResult> _oFNQCResults = new List<FNQCResult>();
        #endregion
        #region Functions

        #endregion
        #region Actions
        public ActionResult ViewFNQCResults(int id)
        {
            FNProductionBatch oFNProductionBatch = new FNProductionBatch();
            FNTreatmentProcess oFNTreatmentProcess = new FNTreatmentProcess();
            List<FNQCResult> oFNQCResults = new List<FNQCResult>();
            List<FNQCResultSetup> oFNQCResultSetups = new List<FNQCResultSetup>();
            List<FNQCParameter> oFNQCParameters = new List<FNQCParameter>();
            oFNQCParameters = FNQCParameter.Gets((int)Session[SessionInfo.currentUserID]);

            if (id > 0)
            {
                oFNProductionBatch = oFNProductionBatch.Get(id, (int)Session[SessionInfo.currentUserID]);
                oFNTreatmentProcess = oFNTreatmentProcess.Get(oFNProductionBatch.FNTPID, (int)Session[SessionInfo.currentUserID]);
                if (oFNProductionBatch.FNPBatchID > 0)
                {
                    oFNQCResults = FNQCResult.Gets("SELECT * FROM View_FNQCResult WHERE FNPBatchID = '" + oFNProductionBatch.FNPBatchID + "' AND FNTPID = " + oFNTreatmentProcess.FNTPID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            //oFNQCResultSetups = FNQCResultSetup.Gets("SELECT * FROM FNQCResultSetup WHERE FNTPID = " + oFNProductionBatch.FNTPID, (int)Session[SessionInfo.currentUserID]);



            //if (oFNQCResultSetups.Count > 0)
            //{
            //    foreach (FNQCResultSetup oTemp in oFNQCResultSetups)
            //    {
            //        for (int i = 0; i < oFNQCParameters.Count; i++)
            //        {
            //            if (oTemp.FNQCParameterID == oFNQCResults[i].FNQCParameterID && oFNQCResults[i].Value == 0)
            //            {
            //                oFNQCResults[i].Value = oTemp.Value;
            //            }
            //        }
            //    }
            //}
            //string sString = "";
            //foreach (FNQCResult oItem in oFNQCResults)
            //{
            //    sString = sString + oItem.FNQCParameterID + ",";
            //}
            //if (!string.IsNullOrEmpty(sString))
            //{
            //    sString = sString.Remove(sString.Length - 1, 1);
            //    oFNQCParameters = FNQCParameter.Gets("SELECT * FROM View_FNQCParameter WHERE FNQCParameterID NOT IN (" + sString + ")", (int)Session[SessionInfo.currentUserID]);
            //}
            //foreach (FNQCParameter oItem in oFNQCParameters)
            //{
            //    FNQCResult objFNQCResult = new FNQCResult();
            //    objFNQCResult.FNQCParameterID = oItem.FNQCParameterID;
            //    objFNQCResult.FNTPID = oFNProductionBatch.FNTPID;
            //    objFNQCResult.FNPBatchID = oFNProductionBatch.FNPBatchID;
            //    objFNQCResult.Name = oItem.Name;
            //    objFNQCResult.Code = oItem.Code;
            //    oFNQCResults.Add(objFNQCResult);
            //}

            ViewBag.FNQCResults = oFNQCResults;
            return View(oFNProductionBatch);
        }
        [HttpPost]
        public JsonResult Save(List<FNQCResult> oFNQCResults)
        {
            try
            {
                _oFNQCResults = _oFNQCResult.SaveAll(oFNQCResults, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNQCResult = new FNQCResult();
                _oFNQCResult.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNQCResults);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveFromGrid(FNQCResult oFNQCResult)
        {
            try
            {
                _oFNQCResult = _oFNQCResult.Save(oFNQCResult, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNQCResult = new FNQCResult();
                _oFNQCResult.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNQCResult);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(FNQCResult oFNQCResult)
        {
            string sFeedBackMessage = "";
            try
            {
                if (oFNQCResult.FNQCResultID > 0)
                {
                    sFeedBackMessage = oFNQCResult.Delete(oFNQCResult.FNQCResultID, (int)Session[SessionInfo.currentUserID]);
                }
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
        public JsonResult GetsBuyerLastResult(FNQCResult oFNQCResult)
        {
            string stemp = "";
            string sFNQCParameterID = "";
            FNProductionBatch oFNProductionBatch = new FNProductionBatch();
            List<FabricSCReport> oFabricSCReports = new List<FabricSCReport>();
            FNTreatmentProcess oFNTreatmentProcess = new FNTreatmentProcess();
            List<FNQCResult> oFNQCResults = new List<FNQCResult>();
            List<FNQCResult> oFNQCResultsLast = new List<FNQCResult>();
            List<FNProductionBatch> oFNProductionBatchs = new List<FNProductionBatch>();
            try
            {
                if (oFNQCResult.FNPBatchID > 0)
                {
                    oFNProductionBatch = oFNProductionBatch.Get(oFNQCResult.FNPBatchID, (int)Session[SessionInfo.currentUserID]);
                    oFNTreatmentProcess = oFNTreatmentProcess.Get(oFNProductionBatch.FNTPID, (int)Session[SessionInfo.currentUserID]);
                    oFNQCResults = FNQCResult.Gets("SELECT * FROM View_FNQCResult WHERE FNPBatchID = '" + oFNProductionBatch.FNPBatchID + "' AND FNTPID = " + oFNTreatmentProcess.FNTPID, (int)Session[SessionInfo.currentUserID]);
                    if (oFNQCResults.Count >0)
                    {
                        sFNQCParameterID = string.Join(",", oFNQCResults.Select(x => x.FNQCParameterID).Distinct().ToList());
                    }
                    //if (string.IsNullOrEmpty(sFNQCParameterID)) {sFNQCParameterID }
                        oFabricSCReports = FabricSCReport.Gets("Select top(1)* from View_FabricSalesContractReport as HH where FabricSalesContractDetailID in (Select FNExOID from FNBatch where FNBatchID=" + oFNProductionBatch.FNBatchID + ")", (int)Session[SessionInfo.currentUserID]);
                        stemp = string.Join(",", oFabricSCReports.Select(x => x.BuyerID).ToList());

                        oFNQCResultsLast = FNQCResult.Gets("SELECT top(1)* FROM FNQCResult where FNTPID ="+ oFNTreatmentProcess.FNTPID+" and FNPBatchID in (Select FNPBatchID from view_FNProductionBatch where FNPBatchID<>" + oFNProductionBatch.FNPBatchID + " and FNTPID= " + oFNTreatmentProcess.FNTPID + " and FNBatchID in (Select FNBatchID  from FNBatch where FNExOID in (Select FabricSalesContractDetailID from FabricSalesContractDetail where FabricSalesContractID in (Select FabricSalesContractID from FabricSalesContract where BuyerID in (" + stemp + "))))) order by DBServerDateTime DESC", (int)Session[SessionInfo.currentUserID]);

                      //  oFNProductionBatchs = FNProductionBatch.Gets("Select top(1)* from view_FNProductionBatch where FNPBatchID<>" + oFNProductionBatch.FNPBatchID + " and FNTPID >0 and FNPBatchID in (SELECT FNPBatchID FROM FNQCResult )and FNBatchID in (Select FNBatchID  from FNBatch where FNExOID in (Select FabricSalesContractDetailID from FabricSalesContractDetail where FabricSalesContractID in (Select FabricSalesContractID from FabricSalesContract where BuyerID in (" + stemp + ")))) order by FNPBatchID DESC", (int)Session[SessionInfo.currentUserID]);
                        stemp = string.Join(",", oFNQCResultsLast.Select(x => x.FNPBatchID).Distinct().ToList());
                        if (!string.IsNullOrEmpty(stemp))
                        {
                            oFNQCResults = FNQCResult.Gets("SELECT * FROM View_FNQCResult WHERE FNQCParameterID not in (" + (String.IsNullOrEmpty(sFNQCParameterID)?"0":sFNQCParameterID)+ ") and  FNPBatchID in (" + stemp + ") AND FNTPID = " + oFNTreatmentProcess.FNTPID, (int)Session[SessionInfo.currentUserID]);
                        }

                        oFNQCResults.ForEach(o => o.FNQCResultID = 0);
                        oFNQCResults.ForEach(o => o.FNPBatchID = oFNQCResult.FNPBatchID);
                        if (oFNQCResults.Count > 0)
                        {
                            _oFNQCResults = _oFNQCResult.SaveAll(oFNQCResults, (int)Session[SessionInfo.currentUserID]);
                        }
                  //  }
                }
            }
            catch (Exception ex)
            {
                _oFNQCResult = new FNQCResult();
                _oFNQCResult.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNQCResults);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }

}
