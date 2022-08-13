using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class CompanyRuleController : Controller
    {
        #region Declaration
        CompanyRuleName _oCompanyRuleName;
        List<CompanyRuleName> _oCompanyRuleNames;
        #endregion

        #region Views
        public ActionResult View_CompanyRules(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oCompanyRuleNames = new List<CompanyRuleName>();
            string sSql = "SELECT * FROM CompanyRuleName ";
            _oCompanyRuleNames = CompanyRuleName.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oCompanyRuleNames);
        }
        
        public ActionResult View_CompanyRule(string sid, string sMsg)//CRNID
        {
            int nId = Convert.ToInt32(sid != "0" ? Global.Decrypt(sid) : "0");  
            _oCompanyRuleName = new CompanyRuleName();
            if (nId > 0)
            {
                _oCompanyRuleName = CompanyRuleName.Get(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oCompanyRuleName.CompanyRuleDescriptions = CompanyRuleDescription.Gets("SELECT * FROM View_CompanyRuleDescription WHERE CRNID=" + nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            if (sMsg != "N/A")
            {
                _oCompanyRuleName.ErrorMessage = sMsg;
            }
            return View(_oCompanyRuleName);
        }

        #endregion

        #region IUD
        [HttpPost]
        public JsonResult CompanyRuleName_IU(CompanyRuleName oCompanyRuleName)
        {
            try
            {
                if (oCompanyRuleName.CRNID > 0)
                {
                    oCompanyRuleName = oCompanyRuleName.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oCompanyRuleName = oCompanyRuleName.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oCompanyRuleName = new CompanyRuleName();
                oCompanyRuleName.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCompanyRuleName);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CompanyRuleDescription_IU(CompanyRuleDescription oCompanyRuleDescription)
        {
            try
            {
                if (oCompanyRuleDescription.CRDID > 0)
                {
                    oCompanyRuleDescription = oCompanyRuleDescription.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oCompanyRuleDescription = oCompanyRuleDescription.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oCompanyRuleDescription = new CompanyRuleDescription();
                oCompanyRuleDescription.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCompanyRuleDescription);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCompanyRuleDescription(CompanyRuleDescription oCompanyRuleDescription)
        {
            if (oCompanyRuleDescription.CRDID> 0)
            {
                oCompanyRuleDescription = CompanyRuleDescription.Get("SELECT * FROM View_CompanyRuleDescription WHERE CRDID=" + oCompanyRuleDescription.CRDID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCompanyRuleDescription);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CompanyRuleName_Delete(CompanyRuleName oCompanyRuleName)
        {
            try
            {
                oCompanyRuleName = oCompanyRuleName.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oCompanyRuleName = new CompanyRuleName();
                oCompanyRuleName.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCompanyRuleName.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CompanyRuleDescription_Delete(CompanyRuleDescription oCompanyRuleDescription)
        {
            try
            {
                oCompanyRuleDescription = oCompanyRuleDescription.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oCompanyRuleDescription = new CompanyRuleDescription();
                oCompanyRuleDescription.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCompanyRuleDescription.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion oCompanyRuleName_IUD

        #region  Activity
        [HttpPost]
        public JsonResult CompanyRuleName_Activity(CompanyRuleName oCompanyRuleName)
        {
            _oCompanyRuleName = new CompanyRuleName();
            try
            {

                _oCompanyRuleName = oCompanyRuleName;
                _oCompanyRuleName = CompanyRuleName.Activite(_oCompanyRuleName.CRNID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oCompanyRuleName = new CompanyRuleName();
                _oCompanyRuleName.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCompanyRuleName);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CompanyRuleDescription_Activity(CompanyRuleDescription oCompanyRuleDescription)
        {
            CompanyRuleDescription oCRD = new CompanyRuleDescription();
            try
            {

                oCRD = CompanyRuleDescription.Activite(oCompanyRuleDescription.CRDID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oCRD = new CompanyRuleDescription();
                oCRD.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCRD);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Print
        public ActionResult View_PrintCompanyRules(string sCRNIDs, double ts)
        {
            CompanyRuleName oCompanyRuleName = new CompanyRuleName();
            List<CompanyRuleDescription> oCompanyRuleDescriptions = new List<CompanyRuleDescription>();
            oCompanyRuleName.CompanyRuleNames = CompanyRuleName.Gets("SELECT * FROM CompanyRuleName WHERE IsActive=1 AND CRNID IN(" + sCRNIDs+")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oCompanyRuleName.CompanyRuleDescriptions = CompanyRuleDescription.Gets("SELECT * FROM View_CompanyRuleDescription WHERE IsActive=1 AND CRNID IN(" + sCRNIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oCompanyRuleName.Employee = new Employee();
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oCompanyRuleName.Company = oCompanys.First();
            return PartialView(oCompanyRuleName);
        }
        #endregion Print

        [HttpPost]
        public JsonResult GetsCompanyRuleName(CompanyRuleName oCompanyRuleName)
        {
            List<CompanyRuleName> oCompanyRuleNames = new List<CompanyRuleName>();
            oCompanyRuleNames = CompanyRuleName.Gets("SELECT * FROM CompanyRuleName WHERE IsActive=1" , ((User)(Session[SessionInfo.CurrentUser])).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCompanyRuleNames);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
