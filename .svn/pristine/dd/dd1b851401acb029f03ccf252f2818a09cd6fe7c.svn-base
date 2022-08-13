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
using System.Text;

namespace ESimSolFinancial.Controllers
{
    public class MaxOTConfigurationController : Controller
    {
        #region Declaration
        MaxOTConfiguration _oMaxOTConfiguration = new MaxOTConfiguration();
        List<MaxOTConfiguration> _oMaxOTConfigurations = new List<MaxOTConfiguration>();
        string _sErrorMessage = "";
        #endregion

        #region Functions

        #endregion

        public ActionResult View_MaxOTConfiguration(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.AccountsBookSetup).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oMaxOTConfigurations = new List<MaxOTConfiguration>();
            _oMaxOTConfigurations = MaxOTConfiguration.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oMaxOTConfigurations);
        }
        public ActionResult ViewMaxOTConfiguration(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.AccountsBookSetup).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oMaxOTConfigurations = new List<MaxOTConfiguration>();
            _oMaxOTConfigurations = MaxOTConfiguration.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oMaxOTConfigurations);
        }

        [HttpPost]
        public JsonResult GetSourceTimeCard(MaxOTConfiguration oMaxOTConfiguration)
        {
            List<MaxOTConfiguration> oMaxOTConfigurations = new List<MaxOTConfiguration>();

            try
            {
                string sSQL = "SELECT * FROM MaxOTConfiguration AS MOC WHERE MOC.MOCID!=" + oMaxOTConfiguration.MOCID + " AND MOC.SourceTimeCardID=0 ORDER BY MOC.TimeCardName ASC";
                oMaxOTConfigurations = MaxOTConfiguration.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMaxOTConfiguration = new MaxOTConfiguration();
                _oMaxOTConfiguration.ErrorMessage = ex.Message;
                oMaxOTConfigurations.Add(_oMaxOTConfiguration);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMaxOTConfigurations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save(MaxOTConfiguration oMaxOTConfiguration)
        {
            _oMaxOTConfiguration = new MaxOTConfiguration();
            try
            {
                _oMaxOTConfiguration = oMaxOTConfiguration;
                _oMaxOTConfiguration = _oMaxOTConfiguration.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMaxOTConfiguration = new MaxOTConfiguration();
                _oMaxOTConfiguration.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMaxOTConfiguration);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(MaxOTConfiguration oMaxOTConfiguration)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oMaxOTConfiguration.Delete((int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Gets()
        {
            List<MaxOTConfiguration> oMaxOTConfigurations = new List<MaxOTConfiguration>();
            oMaxOTConfigurations = MaxOTConfiguration.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oMaxOTConfigurations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsMaxOTCEmployeeType(MaxOTConfiguration oMaxOTConfiguration)
        {
            MaxOTConfiguration _oMaxOTConfiguration = new MaxOTConfiguration();
            List<MaxOTCEmployeeType> oMaxOTCEmployeeTypes = new List<MaxOTCEmployeeType>();
            oMaxOTCEmployeeTypes = MaxOTCEmployeeType.Gets(oMaxOTConfiguration.MOCID, (int)Session[SessionInfo.currentUserID]);
            _oMaxOTConfiguration.MaxOTCEmployeeTypes = oMaxOTCEmployeeTypes;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMaxOTConfiguration);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Activity(MaxOTConfiguration oMaxOTConfiguration)
        {
            MaxOTConfiguration _oMaxOTConfiguration = new MaxOTConfiguration();
            try
            {
                _oMaxOTConfiguration = oMaxOTConfiguration.Activity((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMaxOTConfiguration = new MaxOTConfiguration();
                _oMaxOTConfiguration.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMaxOTConfiguration);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsEmployeeType()
        {
            List<EmployeeType> _oEmployeeTypes = new List<EmployeeType>();
            try
            {
                StringBuilder sSQL = new StringBuilder("SELECT * FROM EmployeeType WHERE EmployeeGrouping=" + (int)EnumEmployeeGrouping.StaffWorker);
                _oEmployeeTypes = EmployeeType.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                EmployeeType _oEmployeeType = new EmployeeType();
                _oEmployeeType.ErrorMessage = ex.Message;
                _oEmployeeTypes.Add(_oEmployeeType);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeTypes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

    }

}


