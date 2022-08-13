using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
 
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using iTextSharp;
using ESimSol.Reports;

namespace ESimSolFinancial.Controllers
{
    public class BodyMeasureController : Controller
    {
        #region Declaration       
        BodyMeasure _oBodyMeasure = new BodyMeasure();
        #endregion

        public ViewResult ViewBodyMeasure(int csid, double ts)
        {
            CostSheet oCostSheet =new CostSheet();
            BodyMeasure oBodyMeasure = new BodyMeasure();            
            List<BodyMeasure> oBodyMeasures = new List<BodyMeasure>();
            oCostSheet = oCostSheet.Get(csid, (int)Session[SessionInfo.currentUserID]);
            oBodyMeasures = BodyMeasure.Gets(csid, (int)Session[SessionInfo.currentUserID]);
            oBodyMeasure.CostSheetID = csid;            
            oBodyMeasure.BodyMeasures = oBodyMeasures;
            ViewBag.BodyParts = BodyPart.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.HeaderTitle = "Body Measurement for Style No : " + oCostSheet.StyleNo + ", Buyer Name : " + oCostSheet.BuyerName;
            return View(oBodyMeasure);
        }

        [HttpPost]
        public JsonResult SaveBodyMeasurement(BodyMeasure oBodyMeasure)
        {
            try
            {
                oBodyMeasure = oBodyMeasure.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oBodyMeasure = new BodyMeasure();
                oBodyMeasure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBodyMeasure);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteBodyMeasure(BodyMeasure oBodyMeasure)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oBodyMeasure.Delete(oBodyMeasure.BodyMeasureID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
