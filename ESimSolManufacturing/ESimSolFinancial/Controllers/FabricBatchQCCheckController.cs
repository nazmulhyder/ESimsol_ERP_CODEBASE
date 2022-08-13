using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.Reports;
using ESimSolFinancial.Models;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace ESimSolFinancial.Controllers
{
    public class FabricBatchQCCheckController : Controller
    {
        #region Declaration
        FabricBatchQCCheck _oFabricBatchQCCheck = new FabricBatchQCCheck();
        List<FabricBatchQCCheck> _oFabricBatchQCChecks = new List<FabricBatchQCCheck>();
        #endregion
        [HttpGet]
        public ActionResult ViewFabricBatchQCChecks(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string ssSQL = "SELECT * FROM View_FabricBatchQCCheck  AS HH ORDER BY HH.FabricBatchQCCheckID ASC";
            _oFabricBatchQCChecks = FabricBatchQCCheck.Gets(ssSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oFabricBatchQCChecks);
        }

        [HttpPost]
        public JsonResult Save(FabricBatchQCCheck oFabricBatchQCCheck)
        {
            _oFabricBatchQCCheck = new FabricBatchQCCheck();
            try
            {
                _oFabricBatchQCCheck = oFabricBatchQCCheck;
                _oFabricBatchQCCheck = _oFabricBatchQCCheck.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricBatchQCCheck = new FabricBatchQCCheck();
                _oFabricBatchQCCheck.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBatchQCCheck);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                FabricBatchQCCheck oFabricBatchQCCheck = new FabricBatchQCCheck();
                sFeedBackMessage = oFabricBatchQCCheck.Delete(id, (int)Session[SessionInfo.currentUserID]);

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