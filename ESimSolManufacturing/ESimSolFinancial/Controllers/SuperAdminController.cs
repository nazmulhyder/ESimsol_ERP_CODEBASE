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
using ICS.Core.Utility;
using System.Threading;
using ESimSolFinancial.Hubs;
using System.Threading.Tasks;

namespace ESimSolFinancial.Controllers
{
    public class SuperAdminController : Controller
    {
        SuperAdmin _oSuperAdmin;

        public ActionResult View_SuperAdmin(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);

            _oSuperAdmin = new SuperAdmin();
            return View(_oSuperAdmin);
        }

        [HttpPost]
        public ActionResult MakeDayoffHoliday(string sTemp)
        {
            //DateTime sDate, DateTime eDate, bool isComp
            _oSuperAdmin = new SuperAdmin();
            try
            {
                DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[0]);
                DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[1]);
                bool isComp= Convert.ToBoolean(sTemp.Split('~')[2]);

                _oSuperAdmin = SuperAdmin.MakeDayoffHoliday(Startdate.ToString("dd MMM yyyy"), EndDate.ToString("dd MMM yyyy"), isComp, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                
            }
            catch (Exception ex)
            {
                _oSuperAdmin = new SuperAdmin();
                _oSuperAdmin.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSuperAdmin);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

    }
}
