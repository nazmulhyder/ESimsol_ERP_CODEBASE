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

namespace ESimSolFinancial.Controllers
{
    public class CostSetupController : Controller
    {
        #region Declaration
        CostSetup _oCostSetup = new CostSetup();
        List<CostSetup> _oCostSetups = new List<CostSetup>();
        #endregion

        #region Functions

        #endregion

        public ActionResult ViewCostSetup(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oCostSetup = _oCostSetup.Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.CostSetupTypes = EnumObject.jGets(typeof(EnumColorType));
            return View(_oCostSetup);
        }

        [HttpPost]
        public JsonResult Save(CostSetup oCostSetup)
        {
            _oCostSetup = new CostSetup();
            try
            {
                _oCostSetup = oCostSetup;
                _oCostSetup = _oCostSetup.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCostSetup = new CostSetup();
                _oCostSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCostSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                CostSetup oCostSetup = new CostSetup();
                sFeedBackMessage = oCostSetup.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Searching

        [HttpPost]
        public JsonResult Gets()
        {
            List<CostSetup> oCostSetups = new List<CostSetup>();
            oCostSetups = CostSetup.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oCostSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }

}

