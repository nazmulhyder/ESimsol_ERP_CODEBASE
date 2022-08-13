using ESimSol.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ESimSolFinancial.Controllers
{
    public class EmailConfigController : Controller
    {
        #region Declaration
        EmailConfig _oEmailConfig = new EmailConfig();
        List<EmailConfig> _oEmailConfigs = new List<EmailConfig>();
        #endregion

        public ActionResult ViewEmailConfigs(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oEmailConfigs = new List<EmailConfig>();
            _oEmailConfigs = EmailConfig.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oEmailConfigs);
        }

        public ActionResult ViewEmailConfig(int id)
        {
            _oEmailConfig = new EmailConfig();
            if (id > 0)
            {
                _oEmailConfig = _oEmailConfig.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oEmailConfig);
        }

        [HttpPost]
        public JsonResult Save(EmailConfig oEmailConfig)
        {
            _oEmailConfig = new EmailConfig();
            try
            {
                _oEmailConfig = oEmailConfig;
                _oEmailConfig = _oEmailConfig.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmailConfig = new EmailConfig();
                _oEmailConfig.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmailConfig);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                EmailConfig oEmailConfig = new EmailConfig();
                sFeedBackMessage = oEmailConfig.Delete(id, (int)Session[SessionInfo.currentUserID]);
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