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
    public class FabricQCParNameController : Controller
    {
        #region Declaration
        FabricQCParName _oFabricQCParName = new FabricQCParName();
        List<FabricQCParName> _oFabricQCParNames = new List<FabricQCParName>();
        #endregion
        [HttpGet]
        public ActionResult ViewFabricQCParNames(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string ssSQL = "SELECT * FROM View_FabricQCParName  AS HH ORDER BY HH.FabricQCParNameID ASC";
            _oFabricQCParNames = FabricQCParName.Gets(ssSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oFabricQCParNames);
        }

        [HttpPost]
        public JsonResult Save(FabricQCParName oFabricQCParName)
        {
            _oFabricQCParName = new FabricQCParName();
            try
            {
                _oFabricQCParName = oFabricQCParName;
                _oFabricQCParName = _oFabricQCParName.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricQCParName = new FabricQCParName();
                _oFabricQCParName.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricQCParName);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                FabricQCParName oFabricQCParName = new FabricQCParName();
                sFeedBackMessage = oFabricQCParName.Delete(id, (int)Session[SessionInfo.currentUserID]);

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