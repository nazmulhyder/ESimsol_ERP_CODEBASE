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
    public class BodyPartController :  Controller
    {
        #region Declaration
        BodyPart _oBodyPart = new BodyPart();
        List<BodyPart> _oBodyParts = new List<BodyPart>();
        string _sErrorMessage = "";
        #endregion

        #region Functions

        #endregion

        public ActionResult ViewBodyParts(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.BodyPart).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oBodyParts = new List<BodyPart>();
            _oBodyParts = BodyPart.Gets((int)Session[SessionInfo.currentUserID]);            
            return View(_oBodyParts);
        }

        public ActionResult ViewBodyPart(int id)
        {
            _oBodyPart = new BodyPart();
            if (id > 0)
            {
                _oBodyPart = _oBodyPart.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oBodyPart);
        }

        [HttpPost]
        public JsonResult Save(BodyPart oBodyPart)
        {
            _oBodyPart = new BodyPart();
            try
            {
                _oBodyPart = oBodyPart;
                _oBodyPart = _oBodyPart.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oBodyPart = new BodyPart();
                _oBodyPart.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBodyPart);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(BodyPart oBodyPart)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oBodyPart.Delete(oBodyPart.BodyPartID, (int)Session[SessionInfo.currentUserID]);
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
            List<BodyPart> oBodyParts = new List<BodyPart>();
            BodyPart oBodyPart = new BodyPart();
            oBodyPart.BodyPartName = "-- Select Product Type --";
            oBodyParts.Add(oBodyPart);
            oBodyParts.AddRange(BodyPart.Gets((int)Session[SessionInfo.currentUserID]));

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oBodyParts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

    }
}
