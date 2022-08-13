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
using System.Reflection;


namespace ESimSolFinancial.Controllers
{
    public class ConsumptionForecastController : Controller
    {
        #region Declaration
        ConsumptionForecast _oConsumptionForecast = new ConsumptionForecast();
        List<ConsumptionForecast> _oConsumptionForecasts = new List<ConsumptionForecast>();
        #endregion

        #region Actions
        public ActionResult ViewConsumptionForecast(int ProductNature, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ConsumptionForecast).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oConsumptionForecast = new ConsumptionForecast();
            _oConsumptionForecast.BUID = buid;
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            //ViewBag.ProductCategorys = ProductCategory.GetsBUWiseLastLayer(buid, (int)Session[SessionInfo.currentUserID]);    
            ViewBag.BUID = buid;
            ViewBag.ProductNature = ProductNature;
            return View(_oConsumptionForecast);
        }
        #endregion

        #region Post Method

        [HttpPost]
        public JsonResult PrepareConsumptionForecast(ConsumptionForecast oConsumptionForecast)
        {
            List<ConsumptionForecast> oConsumptionForecasts = new List<ConsumptionForecast>();
            oConsumptionForecasts = ConsumptionForecast.PrepareConsumptionForecast(oConsumptionForecast, (int)Session[SessionInfo.currentUserID]);
            _oConsumptionForecast.ConsumptionProduct = oConsumptionForecasts.Where(x => x.IsBooking == true).ToList();
            _oConsumptionForecast.ConsumptionRM = oConsumptionForecasts.Where(x => x.IsBooking == false).ToList();
            var jsonResult = Json(_oConsumptionForecast, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Functions
      
        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Print

        [HttpPost]
        public ActionResult SetPrintSessionData(ConsumptionForecast oConsumptionForecast)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oConsumptionForecast);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintConsumptionForecast()
        {
            Company oCompany = new Company();
            ConsumptionForecast  oConsumptionForecast = new ConsumptionForecast();

            try
            {
                oConsumptionForecast = (ConsumptionForecast)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oConsumptionForecast = null;
            }

            if (oConsumptionForecast == null)
            {
                oConsumptionForecast = new ConsumptionForecast();
            }


            List<ConsumptionForecast> oConsumptionForecasts = new List<ConsumptionForecast>();
            oConsumptionForecasts = ConsumptionForecast.PrepareConsumptionForecast(oConsumptionForecast, (int)Session[SessionInfo.currentUserID]);
            _oConsumptionForecast.ConsumptionProduct = oConsumptionForecasts.Where(x => x.IsBooking == true).ToList();
            _oConsumptionForecast.ConsumptionRM = oConsumptionForecasts.Where(x => x.IsBooking == false).ToList();
            _oConsumptionForecast.StartDate = oConsumptionForecast.StartDate;
            _oConsumptionForecast.EndDate = oConsumptionForecast.EndDate;
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            rptConsumptionForecast orptConsumptionForecast = new rptConsumptionForecast();
            byte[] abytes = orptConsumptionForecast.PrepareReport(oCompany, _oConsumptionForecast);
            return File(abytes, "application/pdf");
        }
        #endregion
    }
}
