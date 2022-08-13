using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
 

namespace ESimSolFinancial.Controllers
{
    public class KnittingController : Controller
    {
        #region Declaration
        Knitting _oKnitting = new Knitting();
        List<Knitting> _oKnittings = new List<Knitting>();
        string _sErrorMessage = "";
        #endregion

        public ActionResult ViewKnittings(int menuid) // id  is Factory ID
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            _oKnittings = new List<Knitting>();
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Knitting).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oKnittings = Knitting.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oKnittings);
        }

        public ActionResult ViewKnitting(int id, double ts)
        {
            _oKnitting = new Knitting();
            if (id > 0)
            {
                _oKnitting = _oKnitting.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oKnitting);
        }

        [HttpPost]
        public JsonResult Save(Knitting oKnitting)
        {
            _oKnitting = new Knitting();
            try
            {
                _oKnitting = oKnitting.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnitting = new Knitting();
                _oKnitting.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitting);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                Knitting oKnitting = new Knitting();
                sFeedBackMessage = oKnitting.Delete(id, (int)Session[SessionInfo.currentUserID]);
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