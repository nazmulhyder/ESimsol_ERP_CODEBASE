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
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using iTextSharp.text.pdf;
using System.Web.Services;

namespace ESimSolFinancial.Controllers
{
    public class VehicleTypeController : Controller
    {
        #region Declaration
        VehicleType _oVehicleType = new VehicleType();
        List<VehicleType> _oVehicleTypes = new List<VehicleType>();
        string _sErrorMessage = "";
        #endregion

        public ActionResult ViewVehicleTypes(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.VehicleType).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            
            _oVehicleTypes = new List<VehicleType>();
            _oVehicleTypes = VehicleType.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oVehicleTypes);
        }

        public ActionResult ViewVehicleType(int id)
        {
            _oVehicleType = new VehicleType();
            if (id > 0)
            {
                _oVehicleType = _oVehicleType.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oVehicleType);
        }

        [HttpPost]
        public JsonResult Save(VehicleType oVehicleType)
        {
            _oVehicleType = new VehicleType();
            try
            {
                _oVehicleType = oVehicleType;
                _oVehicleType = oVehicleType.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVehicleType = new VehicleType();
                _oVehicleType.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVehicleType);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                VehicleType oVehicleType = new VehicleType();
                sFeedBackMessage = oVehicleType.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VehicleTypeSearch()
        {
            List<VehicleType> oVehicleTypes = new List<VehicleType>();
            return PartialView(oVehicleTypes);
        }
        #region Searching

        [HttpPost]
        public JsonResult Gets()
        {
            List<VehicleType> oVehicleTypes = new List<VehicleType>();
            oVehicleTypes = VehicleType.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVehicleTypes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}

