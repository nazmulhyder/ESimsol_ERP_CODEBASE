using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;

namespace ESimSolFinancial.Controllers
{
    public class RSShiftController : Controller
    {
        #region Declaration
        RSShift _oRSShift = new RSShift();
        List<RSShift> _oRSShifts = new List<RSShift>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
       
        #endregion
        public ActionResult ViewRSShifts(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oRSShifts = new List<RSShift>();
            _oRSShifts = RSShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.BUID = buid;
            return View(_oRSShifts);
        }

        public ActionResult ViewRSShift(int id, int buid)
        {
            _oRSShift = new RSShift();

            if (id > 0)
                _oRSShift = _oRSShift.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.Modules = EnumObject.jGets(typeof(EnumModuleName)).OrderBy(x => x.Value).ToList(); 
            return View(_oRSShift);
        }

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(RSShift oRSShift)
        {
            _oRSShift = new RSShift();
            try
            {
                _oRSShift = oRSShift;
                _oRSShift = _oRSShift.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oRSShift.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRSShift);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ToggleActivity(RSShift oRSShift)
        {
            _oRSShift = new RSShift();
            try
            {
                _oRSShift = oRSShift;
                _oRSShift = _oRSShift.ToggleActivity(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oRSShift.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRSShift);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HTTP GET Delete
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string smessage = "";
            try
            {
                RSShift oRSShift = new RSShift();
                smessage = oRSShift.Delete(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                smessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(smessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
