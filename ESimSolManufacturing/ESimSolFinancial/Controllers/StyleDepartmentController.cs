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
    public class StyleDepartmentController : Controller
    {
        #region Declaration
        StyleDepartment _oStyleDepartment = new StyleDepartment();
        List<StyleDepartment> _oStyleDepartments = new List<StyleDepartment>();
        string _sErrorMessage = "";
        #endregion

        public ActionResult ViewStyleDepartments(int menuid) // id  is Factory ID
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            _oStyleDepartments = new List<StyleDepartment>();
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.StyleDepartment).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oStyleDepartments = StyleDepartment.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oStyleDepartments);
        }

        public ActionResult ViewStyleDepartment(int id, double ts)
        {
            _oStyleDepartment = new StyleDepartment();
            if (id > 0)
            {
                _oStyleDepartment = _oStyleDepartment.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oStyleDepartment);
        }

        [HttpPost]
        public JsonResult Save(StyleDepartment oStyleDepartment)
        {
            _oStyleDepartment = new StyleDepartment();
            try
            {
                _oStyleDepartment = oStyleDepartment.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oStyleDepartment = new StyleDepartment();
                _oStyleDepartment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oStyleDepartment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                StyleDepartment oStyleDepartment = new StyleDepartment();
                sFeedBackMessage = oStyleDepartment.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetStyleDepartments(StyleDepartment oStyleDepartment)
        {
            _oStyleDepartments = new List<StyleDepartment>();
            try
            {
                string sSQL = "SELECT * FROM StyleDepartment";
                if(!string.IsNullOrEmpty(oStyleDepartment.Name))
                {
                    sSQL += " WHERE Name LIKE '%"+oStyleDepartment.Name+"%'";
                }
                _oStyleDepartments = StyleDepartment.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oStyleDepartment = new StyleDepartment();
                _oStyleDepartment.ErrorMessage = ex.Message;
                _oStyleDepartments.Add(_oStyleDepartment);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oStyleDepartments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

    }
  
}