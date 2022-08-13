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
    public class MaterialTypeController : Controller
    {
        #region Declaration
        MaterialType _oMaterialType = new MaterialType();
        List<MaterialType> _oMaterialTypes = new List<MaterialType>();
        string _sErrorMessage = "";
        #endregion

        public ActionResult ViewMaterialTypes(int menuid) // id  is Factory ID
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            _oMaterialTypes = new List<MaterialType>();
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.MaterialType).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oMaterialTypes = MaterialType.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oMaterialTypes);
        }

        public ActionResult ViewMaterialType(int id, double ts)
        {
            _oMaterialType = new MaterialType();
            if (id > 0)
            {
                _oMaterialType = _oMaterialType.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oMaterialType);
        }

        [HttpPost]
        public JsonResult Save(MaterialType oMaterialType)
        {
            _oMaterialType = new MaterialType();
            try
            {
                _oMaterialType = oMaterialType.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMaterialType = new MaterialType();
                _oMaterialType.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMaterialType);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                MaterialType oMaterialType = new MaterialType();
                sFeedBackMessage = oMaterialType.Delete(id, (int)Session[SessionInfo.currentUserID]);
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