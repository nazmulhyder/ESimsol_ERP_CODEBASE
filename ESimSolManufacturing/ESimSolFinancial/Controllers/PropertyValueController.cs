using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using ICS.Core.Utility;


namespace ESimSolFinancial.Controllers
{
    public class PropertyValueController : Controller
    {
        #region Declaration
        PropertyValue _oPropertyValue = new PropertyValue();
        List<PropertyValue> _oPropertyValues = new List<PropertyValue>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
        private bool ValidateInput(PropertyValue oPropertyValue)
        {
            if (oPropertyValue.Remarks == null || oPropertyValue.Remarks == "")
            {
                _sErrorMessage = "Please enter Property Unit";
                return false;
            }
            if (oPropertyValue.ValueOfProperty == null || oPropertyValue.ValueOfProperty == "")
            {
                _sErrorMessage = "Please enter Value Of Property";
                return false;
            }
            return true;
        }
        #endregion

        public ActionResult ViewPropertyValues(int menuid)
        {

            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.PropertyValue).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oPropertyValues = new List<PropertyValue>();
            _oPropertyValues = PropertyValue.Gets((int)Session[SessionInfo.currentUserID]);
            
            return View(_oPropertyValues);
        }

        public ActionResult ViewPropertyValue(int id)
        {
            _oPropertyValue = new PropertyValue();
            if (id > 0)
            {
                _oPropertyValue = _oPropertyValue.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.PropertyTypes = EnumObject.jGets(typeof(EnumPropertyType));
            return View(_oPropertyValue);
        }

        [HttpPost]
        public JsonResult Save(PropertyValue oPropertyValue)
        {
            try
            {
                oPropertyValue.PropertyType = (EnumPropertyType)oPropertyValue.PropertyTypeInInt;
                oPropertyValue = oPropertyValue.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPropertyValue.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPropertyValue);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

       [HttpPost]
        public JsonResult Delete(PropertyValue oPropertyValue)
        {
            string sErrorMease = "";
            try
            {
                _oPropertyValue = new PropertyValue();
                sErrorMease = _oPropertyValue.Delete(oPropertyValue.PropertyValueID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
     

    }
}
