using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;
using ESimSolFinancial.Controllers;

namespace ESimSolFinancial.Controllers
{
    public class GarmentsTypeController :  Controller
    {
        #region Declaration
        GarmentsType _oGarmentsType = new GarmentsType();
        List<GarmentsType> _oGarmentsTypes = new List<GarmentsType>();
        string _sErrorMessage = "";
        #endregion

        #region Functions

        #endregion

        public ActionResult ViewGarmentsTypes(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.GarmentsType).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oGarmentsTypes = new List<GarmentsType>();
            _oGarmentsTypes = GarmentsType.Gets((int)Session[SessionInfo.currentUserID]);            
            return View(_oGarmentsTypes);
        }

        public ActionResult ViewGarmentsType(int id)
        {
            _oGarmentsType = new GarmentsType();
            if (id > 0)
            {
                _oGarmentsType = _oGarmentsType.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oGarmentsType);
        }

        [HttpPost]
        public JsonResult Save(GarmentsType oGarmentsType)
        {
            _oGarmentsType = new GarmentsType();
            try
            {
                _oGarmentsType = oGarmentsType;
                _oGarmentsType = _oGarmentsType.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGarmentsType = new GarmentsType();
                _oGarmentsType.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGarmentsType);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                GarmentsType oGarmentsType = new GarmentsType();
                sFeedBackMessage = oGarmentsType.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets()
        {
            List<GarmentsType> oGarmentsTypes = new List<GarmentsType>();
            GarmentsType oGarmentsType = new GarmentsType();
            oGarmentsType.TypeName = "-- Select Product Type --";
            oGarmentsTypes.Add(oGarmentsType);
            oGarmentsTypes.AddRange(GarmentsType.Gets((int)Session[SessionInfo.currentUserID]));

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oGarmentsTypes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

    }
}
