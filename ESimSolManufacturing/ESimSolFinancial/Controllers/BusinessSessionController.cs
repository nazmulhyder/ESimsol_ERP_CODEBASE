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
    public class BusinessSessionController : Controller
    {
        #region Declaration
        BusinessSession _oBusinessSession = new BusinessSession();
        List<BusinessSession> _oBusinessSessions = new List<BusinessSession>();
        string _sErrorMessage = "";
        #endregion

        public ActionResult ViewBusinessSessions(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.BusinessSession).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oBusinessSessions = new List<BusinessSession>();
            _oBusinessSessions = BusinessSession.Gets((int)Session[SessionInfo.currentUserID]);            
            return View(_oBusinessSessions);
        }

        public ActionResult ViewBusinessSession(int id)
        {
            _oBusinessSession = new BusinessSession();
            if (id > 0)
            {
                _oBusinessSession = _oBusinessSession.Get(id, (int)Session[SessionInfo.currentUserID]);
            }            
            return View(_oBusinessSession);
        }

        [HttpPost]
        public JsonResult Save(BusinessSession oBusinessSession)
        {
            _oBusinessSession = new BusinessSession();
            try
            {                
                _oBusinessSession = oBusinessSession.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oBusinessSession = new BusinessSession();
                _oBusinessSession.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBusinessSession);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                BusinessSession oBusinessSession = new BusinessSession();
                sFeedBackMessage = oBusinessSession.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
