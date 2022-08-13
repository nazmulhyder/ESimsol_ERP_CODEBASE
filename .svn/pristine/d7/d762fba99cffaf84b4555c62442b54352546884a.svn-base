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

namespace ESimSolFinancial.Controllers
{
    public class ACConfigController : Controller
    {
        #region Declaration
        List<ACConfig> _oACConfigs = new List<ACConfig>();
        ACConfig _oACConfig = new ACConfig();
        #endregion
        public ActionResult ViewACConfig(string sConfigType)
        {
            _oACConfig = new ACConfig();
            _oACConfig.ConfigureTypeObjs = EnumObject.jGets(typeof(EnumConfigureType));
            string sSQL = "";
            if (sConfigType == "GL")
            {
                sSQL = "SELECT * FROM ACConfig WHERE ConfigureType >= " + (int)EnumConfigureType.GLAccHeadWiseNarration + " AND ConfigureType <= " + (int)EnumConfigureType.GLVC;
            }
            else if (sConfigType == "GJ")
            {
                sSQL = "SELECT * FROM ACConfig WHERE ConfigureType >= " + (int)EnumConfigureType.GJAccHeadWiseNarration + " AND ConfigureType <= " + (int)EnumConfigureType.GJVC;
            }
            _oACConfig.ACConfigs = ACConfig.Gets(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(_oACConfig);
        }

        [HttpPost]
        public JsonResult Save(ACConfig oACConfig)
        {
            try
            {                
                _oACConfig = oACConfig.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oACConfig = new ACConfig();
                _oACConfig.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oACConfig);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}