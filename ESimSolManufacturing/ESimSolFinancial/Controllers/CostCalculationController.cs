using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class CostCalculationController : Controller
    {
        #region Declaration
        CostCalculation _oCostCalculation = new CostCalculation();
        List<CostCalculation> _oCostCalculations = new List<CostCalculation>();
        #endregion

        public ActionResult ViewCostCalculationList(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.CostCalculation).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            CostCalculation oCostCalculation = new CostCalculation();
            _oCostCalculations = new List<CostCalculation>();
            string sSQL = "SELECT * FROM View_CostCalculation";
            _oCostCalculations = oCostCalculation.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oCostCalculations);
        }

        public ActionResult ViewCostCalculationSetup(int id, double tsv)
        {
            _oCostCalculation = new CostCalculation();
            CostSetup oCostSetup = new CostSetup();
            Company oCompany = new Company();
            if (id > 0)
            {
                _oCostCalculation = _oCostCalculation.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.CostSetup = oCostSetup.Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            return View(_oCostCalculation);
        }
        public ActionResult ViewCostCalculation(int id)
        {
            _oCostCalculation = new CostCalculation();
            CostSetup oCostSetup = new CostSetup();
            Company oCompany = new Company();
            if (id > 0)
            {
                _oCostCalculation = _oCostCalculation.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.CostSetup = oCostSetup.Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            _oCostCalculation.MarginRate=(100-((_oCostCalculation.LandedCost*100)/_oCostCalculation.ExShowroomPrice));
            double Div=(100-  _oCostCalculation.MarginRate);
            double res = (_oCostCalculation.LandedCost / Div);
            _oCostCalculation.ExShowroomPrice = (res * 100);
            _oCostCalculation.ExShowroomPriceBC = _oCostCalculation.ExShowroomPrice * _oCostCalculation.CRate;
            return View(_oCostCalculation);
        }

        [HttpPost]
        public JsonResult Save(CostCalculation oCostCalculation)
        {
            _oCostCalculation = new CostCalculation();
            try
            {
                _oCostCalculation = oCostCalculation;
                _oCostCalculation = _oCostCalculation.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCostCalculation = new CostCalculation();
                _oCostCalculation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCostCalculation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(CostCalculation oCostCalculation)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oCostCalculation.Delete(oCostCalculation.CostCalculationID, (int)Session[SessionInfo.currentUserID]);
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
