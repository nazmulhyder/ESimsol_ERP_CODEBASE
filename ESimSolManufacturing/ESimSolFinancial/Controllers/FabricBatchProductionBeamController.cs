using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSolFinancial.Models;
using System.Web.Script.Serialization;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class FabricBatchProductionBeamController : Controller
    {
        #region Declaration
        FabricBatchProductionBeam _oFBPB = new FabricBatchProductionBeam();
        List<FabricBatchProductionBeam> _oFBPBs = new List<FabricBatchProductionBeam>();
        #endregion

        [HttpPost]
        public JsonResult Save(FabricBatchProductionBeam oFBPB)
        {
            _oFBPB = new FabricBatchProductionBeam();
            try
            {
                _oFBPB = oFBPB.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFBPB = new FabricBatchProductionBeam();
                _oFBPB.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFBPB);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Finish(FabricBatchProductionBeam oFBPB)
        {
            _oFBPB = new FabricBatchProductionBeam();
            try
            {
                _oFBPB = oFBPB.Finish(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFBPB = new FabricBatchProductionBeam();
                _oFBPB.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFBPB);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult TransferFinishBeam(FabricBatchProductionBeam oFBPB)
        {
            _oFBPB = new FabricBatchProductionBeam();
            try
            {
                _oFBPB = oFBPB.TransferFinishBeam(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFBPB = new FabricBatchProductionBeam();
                _oFBPB.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFBPB);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(FabricBatchProductionBeam oFBPB)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFBPB.Delete(oFBPB.FBPBeamID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}