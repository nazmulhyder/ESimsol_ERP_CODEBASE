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
    public class IncomeTaxConfigureController : Controller
    {
        #region Declaration
        ITaxHeadConfiguration _oITaxHeadConfiguration;
        List<ITaxHeadConfiguration> _oITaxHeadConfigurations;
        
        #endregion

        #region Income Tax Head Configuration
        public ActionResult View_IncomeTaxHeadConfigurations(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oITaxHeadConfigurations = new List<ITaxHeadConfiguration>();
            string sSql = "SELECT * FROM View_ITaxHeadConfiguration ";
            _oITaxHeadConfigurations = ITaxHeadConfiguration.Gets(sSql,((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<SalaryHead> oSalaryHeads = new List<SalaryHead>();
            oSalaryHeads = SalaryHead.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.SalaryHeads = oSalaryHeads;
            ViewBag.BasicHeads = oSalaryHeads.Where(x=>x.SalaryHeadType==EnumSalaryHeadType.Basic).ToList();


            ViewBag.EnumSalaryCalculationOns = Enum.GetValues(typeof(EnumSalaryCalculationOn)).Cast<EnumSalaryCalculationOn>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            return View(_oITaxHeadConfigurations);
        }

        [HttpPost]
        public JsonResult ITaxHeadConfiguration_IU(ITaxHeadConfiguration oITaxHeadConfiguration)
        {
            try
            {
                if (oITaxHeadConfiguration.ITaxHeadConfigurationID > 0)
                {
                    oITaxHeadConfiguration = oITaxHeadConfiguration.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oITaxHeadConfiguration = oITaxHeadConfiguration.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oITaxHeadConfiguration = new ITaxHeadConfiguration();
                oITaxHeadConfiguration.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxHeadConfiguration);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ITaxHeadConfiguration_Delete(ITaxHeadConfiguration oITaxHeadConfiguration)
        {
            try
            {
                if (oITaxHeadConfiguration.ITaxHeadConfigurationID <= 0)
                    throw new Exception("Please select a valid item from list.");
                oITaxHeadConfiguration = oITaxHeadConfiguration.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oITaxHeadConfiguration = new ITaxHeadConfiguration();
                oITaxHeadConfiguration.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxHeadConfiguration.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ITaxHeadConfiguration_Activity(ITaxHeadConfiguration oITaxHeadConfiguration)
        {
            try
            {
                if (oITaxHeadConfiguration.ITaxHeadConfigurationID <= 0)
                    throw new Exception("Please select a valid item from list.");
                oITaxHeadConfiguration = oITaxHeadConfiguration.IUD((int)EnumDBOperation.Approval, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oITaxHeadConfiguration = new ITaxHeadConfiguration();
                oITaxHeadConfiguration.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxHeadConfiguration);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetITaxHeadConfiguration(ITaxHeadConfiguration oITaxHeadConfiguration)
        {
            try
            {
                oITaxHeadConfiguration = ITaxHeadConfiguration.Get(oITaxHeadConfiguration.ITaxHeadConfigurationID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oITaxHeadConfiguration = new ITaxHeadConfiguration();
                oITaxHeadConfiguration.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxHeadConfiguration);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Gets_ITaxHeadConfiguration_By_Activity

        [HttpPost]
        public JsonResult Gets_ITaxHeadConfiguration_By_Activity(bool bActivity)
        {
            _oITaxHeadConfigurations = new List<ITaxHeadConfiguration>();
            try
            {
                string sSql = "";
                if (bActivity)
                { sSql = "SELECT * FROM View_ITaxHeadConfiguration WHERE InactiveDate IS  NULL"; }
                else { sSql = "SELECT * FROM View_ITaxHeadConfiguration WHERE InactiveDate IS NOT NULL"; }

                _oITaxHeadConfigurations = ITaxHeadConfiguration.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oITaxHeadConfiguration = new ITaxHeadConfiguration();
                _oITaxHeadConfiguration.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oITaxHeadConfigurations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Gets_ITaxHeadConfiguration_By_Activity

    }
}
