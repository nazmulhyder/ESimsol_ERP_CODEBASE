using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;

using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using System.Drawing.Printing;
//using Microsoft.Office.Interop.Word;
using System.Reflection;


namespace ESimSolFinancial.Controllers
{
    public class DyeingPeriodConfigController:Controller
    {
        #region Declaration
        DyeingPeriodConfig _oDyeingPeriodConfig = new DyeingPeriodConfig();
        List<DyeingPeriodConfig> _oDyeingPeriodConfigs = new List<DyeingPeriodConfig>();
        string _sErrorMessage = "";

        #endregion
        #region Action
        public ActionResult ViewDyeingPeriodConfigs(int buid, int menuid)
        {
            try
            {
                this.Session.Remove(SessionInfo.MenuID);
                this.Session.Add(SessionInfo.MenuID, menuid);
                this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DyeingPeriodConfig).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

                _oDyeingPeriodConfigs = new List<DyeingPeriodConfig>();
                _oDyeingPeriodConfigs = DyeingPeriodConfig.Gets((int)Session[SessionInfo.currentUserID]);
                ViewBag.DyeingCapacitys = DyeingCapacity.Gets((int)Session[SessionInfo.currentUserID]);
                ViewBag.BUID = buid;
                return View(_oDyeingPeriodConfigs);
            }
            catch (Exception ex)
            {
                return RedirectToAction("LogIn", "User");
            }
        }
        public ActionResult ViewDyeingPeriodConfig(int id, int buid)
        {
            _oDyeingPeriodConfig = new DyeingPeriodConfig();
            if (id > 0)
            {
                _oDyeingPeriodConfig = _oDyeingPeriodConfig.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.DyeingCapacitys = DyeingCapacity.Gets((int)Session[SessionInfo.currentUserID]);           
            ViewBag.BUID = buid;
           
            return View(_oDyeingPeriodConfig);
        }
        [HttpPost]
        public JsonResult Save(DyeingPeriodConfig oDyeingPeriodConfig)
        {
            _oDyeingPeriodConfig = new DyeingPeriodConfig();
            try
            {
                _oDyeingPeriodConfig = oDyeingPeriodConfig;
                _oDyeingPeriodConfig = _oDyeingPeriodConfig.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDyeingPeriodConfig = new DyeingPeriodConfig();
                _oDyeingPeriodConfig.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingPeriodConfig);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(DyeingPeriodConfig oDyeingPeriodConfig)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oDyeingPeriodConfig.Delete(oDyeingPeriodConfig.DyeingPeriodConfigID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}