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
using System.Drawing.Printing;
using System.Reflection;


namespace ESimSolFinancial.Controllers
{
    public class FabricDispoController : Controller
    {
        #region Declaration
        FabricDispo _oFabricDispo = new FabricDispo();
        List<FabricDispo> _oFabricDispos = new List<FabricDispo>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
        public ActionResult ViewFabricDispos(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oFabricDispos = FabricDispo.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.FabricOrderTypes = EnumObject.jGets(typeof(EnumFabricRequestType));
            ViewBag.BusinessUnitTypes = EnumObject.jGets(typeof(EnumBusinessUnitType));

            //ViewBag.FabricOrderTypes = Enum.GetValues(typeof(EnumFabricRequestType)).Cast<EnumFabricRequestType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            //ViewBag.BusinessUnitTypes = Enum.GetValues(typeof(EnumBusinessUnitType)).Cast<EnumBusinessUnitType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            
            return View(_oFabricDispos);
        }
        [HttpPost]
        public JsonResult Save(FabricDispo oFabricDispo)
        {
            _oFabricDispo = new FabricDispo();
            try
            {
                _oFabricDispo = oFabricDispo;
                _oFabricDispo = _oFabricDispo.Save(oFabricDispo, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricDispo = new FabricDispo();
                _oFabricDispo.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricDispo);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = _oFabricDispo.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Get(FabricDispo oFabricDispo)
        {
            _oFabricDispo = new FabricDispo();
            try
            {
                if (oFabricDispo.FabricDispoID > 0)
                {
                    _oFabricDispo = _oFabricDispo.Get(oFabricDispo.FabricDispoID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oFabricDispo = new FabricDispo();
                _oFabricDispo.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricDispo);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Gets(FabricDispo oFabricDispo)
        {
            string sSQL = "";
            _oFabricDispos = new List<FabricDispo>();
            _oFabricDispos = FabricDispo.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricDispos);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
