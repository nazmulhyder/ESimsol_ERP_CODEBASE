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
    public class PropertyController : Controller
    {
        #region Declaration
        Property _oProperty = new Property();
        List<Property> _oPropertys = new List<Property>();
        string _sErrorMessage = "";
        #endregion
        public ActionResult ViewPropertys(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser("'Property','PropertyValue'", (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID));            
            _oPropertys = new List<Property>();
            _oPropertys = Property.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oPropertys);
        }
        public ActionResult ViewPropertysFromItem(int id)
        {
           
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser("'Property','PropertyValue'", (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID));
            _oPropertys = new List<Property>();
            _oPropertys = Property.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oPropertys);
        }
        public ActionResult ViewProperty(int id)
        {
            _oProperty = new Property();
            if (id > 0)
            {
                _oProperty = _oProperty.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }            
            return View(_oProperty);
        }

        [HttpPost]
        public JsonResult Save(Property oProperty)
        {
            try
            {
                //oProperty.CompanyID = 1;
                oProperty = oProperty.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oProperty.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProperty);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(Property oProperty)
        {
            string sErrorMease = "";
            try
            {
                _oProperty = new Property();
                sErrorMease = _oProperty.Delete(oProperty.PropertyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Searching()
        {
            Property oProperty = new Property();
            return PartialView(oProperty);
        }
        [HttpPost]
        public JsonResult GetsByName(Property oProperty)
        {
            List<Property> oPropertys = new List<Property>();
            try
            {
                string sSQL = "SELECT * FROM View_Property AS P WHERE P.PropertyName LIKE '%"+oProperty.PropertyName+"%'";
                oPropertys = Property.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPropertys = new List<Property>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPropertys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Gets(Property oProperty)
        {
            List<Property> oPropertys = new List<Property>();
            try
            {
                string sSQL = GetSQL(oProperty);
                oPropertys = Property.Gets(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPropertys = new List<Property>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPropertys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(Property oProperty)
        {
            
            string sReturn1 = "select * from View_Property";
            string sReturn = "";
            if (oProperty.PropertyName != null)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PropertyName like '%" + oProperty.PropertyName + "%'";
            }
            
            
            return sReturn;
        }
    }
}
