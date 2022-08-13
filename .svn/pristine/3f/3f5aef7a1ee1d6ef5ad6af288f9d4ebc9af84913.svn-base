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
    public class DevelopmentTypeController : Controller
    {
        #region Declaration
        DevelopmentType _oDevelopmentType = new DevelopmentType();
        List<DevelopmentType> _oDevelopmentTypes = new List<DevelopmentType>();
        string _sErrorMessage = "";
        #endregion

        public ActionResult ViewDevelopmentTypes(int menuid) // id  is Factory ID
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            _oDevelopmentTypes = new List<DevelopmentType>();
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DevelopmentType).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oDevelopmentTypes = DevelopmentType.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oDevelopmentTypes);
        }

        public ActionResult ViewDevelopmentType(int id, double ts)
        {
            _oDevelopmentType = new DevelopmentType();
            if (id > 0)
            {
                _oDevelopmentType = _oDevelopmentType.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oDevelopmentType);
        }

        [HttpPost]
        public JsonResult Save(DevelopmentType oDevelopmentType)
        {
            _oDevelopmentType = new DevelopmentType();
            try
            {
                _oDevelopmentType = oDevelopmentType.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDevelopmentType = new DevelopmentType();
                _oDevelopmentType.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDevelopmentType);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                DevelopmentType oDevelopmentType = new DevelopmentType();
                sFeedBackMessage = oDevelopmentType.Delete(id, (int)Session[SessionInfo.currentUserID]);
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