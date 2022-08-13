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
    public class FNProductionBatchTransferController : Controller
    {
        #region Declaration

        FNProductionBatchQuality _oFNProductionBatchQuality = new FNProductionBatchQuality();
        List<FNProductionBatchQuality> _oFNProductionBatchQualitys = new List<FNProductionBatchQuality>();
        List<FNProductionBatch> _oFNProductionBatchs = new List<FNProductionBatch>();
        FNProductionBatchTransfer _oFNProductionBatchTransfer = new FNProductionBatchTransfer();
        List<FNProductionBatchTransfer> _oFNProductionBatchTransfers = new List<FNProductionBatchTransfer>();
        #endregion
        #region Actions
        public ActionResult ViewFFNProductionBatchTransfers(int TreatmentProcess, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFNProductionBatchs = new List<FNProductionBatch>();
            string sSQL = string.Empty;
            sSQL = "SELECT * FROM View_FNProductionBatch WHERE FNPBatchID IN (SELECT FNPBatchID FROM FNProductionBatchQuality WHERE IsOK =1) AND FNPBatchID NOT IN ( SELECT FNPBatchID FROM FNProductionBatchTransferDetail) AND FNProductionID IN(SELECT FNProductionID FROM FNProduction WHERE FNTPID IN( SELECT FNTPID FROM FNTreatmentProcess WHERE FNTreatment= " + TreatmentProcess + "))";
            _oFNProductionBatchs = FNProductionBatch.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);//Consider Production Batch  Depens on FN Treatment
            sSQL = "SELECT * FROM View_FNProductionBatchTransfer WHERE ReceiveBy =0 AND  FNTreatment IN (" + TreatmentProcess + ")";
            _oFNProductionBatchTransfers = FNProductionBatchTransfer.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.FNProductionBatchTransfers = _oFNProductionBatchTransfers;
            ViewBag.FNTreatments=EnumObject.jGets(typeof(EnumFNTreatment));
            ViewBag.TreatmentProcess = TreatmentProcess;
            ViewBag.TransferByName = (User)Session[SessionInfo.CurrentUser];
            var d = (User)Session[SessionInfo.CurrentUser];
                 
            return View(_oFNProductionBatchs);
        }

        public ActionResult ViewFNProductionBatchQuality(int id)
        {
            _oFNProductionBatchQuality = new FNProductionBatchQuality();
            if (id > 0)
            {
                _oFNProductionBatchQuality = _oFNProductionBatchQuality.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oFNProductionBatchQuality);
        }

        [HttpPost]
        public JsonResult TransferFBPBatch(FNProductionBatchTransferDetail oFNProductionBatchTransferDetail)
        {
            FNProductionBatchTransferDetail _ooFNProductionBatchTransferDetail = new FNProductionBatchTransferDetail();
            try
            {
                _ooFNProductionBatchTransferDetail = oFNProductionBatchTransferDetail;
                _ooFNProductionBatchTransferDetail = _ooFNProductionBatchTransferDetail.IUD((int)EnumDBOperation.Insert,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _ooFNProductionBatchTransferDetail = new FNProductionBatchTransferDetail();
                _ooFNProductionBatchTransferDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_ooFNProductionBatchTransferDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFNTransfer(FNProductionBatchTransfer oFNProductionBatchTransfer)
        {
            FNProductionBatchTransfer _oFNProductionBatchTransfer = new FNProductionBatchTransfer();
            try
            {
               _oFNProductionBatchTransfer = FNProductionBatchTransfer.Get(oFNProductionBatchTransfer.FNPBTransferID, (int)Session[SessionInfo.currentUserID]);
               string sSQL = "SELECT * FROM View_FNProductionBatchTransferDetail WHERE FNPBTransferID =" + _oFNProductionBatchTransfer.FNPBTransferID;
               _oFNProductionBatchTransfer.FNProductionBatchTransferDetails = FNProductionBatchTransferDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNProductionBatchTransfer.ErrorMessage = ex.Message;
              
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNProductionBatchTransfer);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ReceiveFBPBatch(FNProductionBatchTransfer oFNProductionBatchTransfer)
        {
            FNProductionBatchTransfer _oFNProductionBatchTransfer = new FNProductionBatchTransfer();
            try
            {
                _oFNProductionBatchTransfer = oFNProductionBatchTransfer;
                _oFNProductionBatchTransfer = _oFNProductionBatchTransfer.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNProductionBatchTransfer = new FNProductionBatchTransfer();
                _oFNProductionBatchTransfer.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNProductionBatchTransfer);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsFNProductionBatchTransfer(FNProductionBatchTransfer oFNProductionBatchTransfer)
        {
            List<FNProductionBatchTransfer> _oFNProductionBatchTransfers = new List<FNProductionBatchTransfer>();
            try
            {
                string orderIDs = oFNProductionBatchTransfer.Params.Split('~')[0];
                string batchIDs = oFNProductionBatchTransfer.Params.Split('~')[1];
                bool chkDate = Convert.ToBoolean(oFNProductionBatchTransfer.Params.Split('~')[2]);
                DateTime dtFrom = Convert.ToDateTime(oFNProductionBatchTransfer.Params.Split('~')[3]);
                DateTime dtTo = Convert.ToDateTime(oFNProductionBatchTransfer.Params.Split('~')[4]);
                string sSQL = "SELECT* FROM View_FNProductionBatchTransfer WHERE FNPBTransferID<>0";

                #region Transfer Date Range
                if (chkDate)
                {
                    sSQL += "  AND TransferDate Between  '" + dtFrom.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy")+"'";

                }
                #endregion
                #region FNExeOrders
                if (!string.IsNullOrEmpty(orderIDs))
                    sSQL += "FNPBTransferID IN (SELECT FNPBTransferID FROM FNProductionBatchTransferDetail WHERE FNPBatchID IN (SELECT FNPBatchID FROM View_FNProductionBatch WHERE FNExOID IN( " + orderIDs + ")) )";
                #endregion
                #region batchIDs
                if (!string.IsNullOrEmpty(batchIDs))
                      sSQL += "FNPBTransferID IN (SELECT FNPBTransferID FROM FNProductionBatchTransferDetail WHERE FNPBatchID IN (SELECT FNPBatchID FROM View_FNProductionBatch WHERE FNBatchID IN( " + batchIDs + ")) )";
                  
                #endregion
                _oFNProductionBatchTransfers = FNProductionBatchTransfer.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNProductionBatchTransfers = new List<FNProductionBatchTransfer>();
                oFNProductionBatchTransfer = new FNProductionBatchTransfer();
                oFNProductionBatchTransfer.ErrorMessage = ex.Message;
                _oFNProductionBatchTransfers.Add(oFNProductionBatchTransfer);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNProductionBatchTransfers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}