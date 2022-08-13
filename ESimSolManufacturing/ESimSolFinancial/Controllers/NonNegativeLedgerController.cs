using ESimSol.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ESimSolFinancial.Controllers
{
    public class NonNegativeLedgerController : Controller
    {
        public ActionResult ViewNonNegativeLedgers(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<NonNegativeLedger> _oNonNegativeLedger = new List<NonNegativeLedger>();
            string sSQL = "SELECT * FROM View_NonNegativeLedger AS HH ORDER BY HH.NonNegativeLedgerID ASC";
            _oNonNegativeLedger = NonNegativeLedger.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            sSQL = "SELECT * FROM BusinessUnit AS HH ORDER BY HH.BusinessUnitID ASC";
            ViewBag.BusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oNonNegativeLedger);
        }

        [HttpPost]
        public JsonResult Save(NonNegativeLedger oNonNegativeLedger)
        {
            NonNegativeLedger _oNonNegativeLedger = new NonNegativeLedger();
            try
            {

                _oNonNegativeLedger = oNonNegativeLedger.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oNonNegativeLedger = new NonNegativeLedger();
                _oNonNegativeLedger.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNonNegativeLedger);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(NonNegativeLedger oNonNegativeLedger)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oNonNegativeLedger.Delete(oNonNegativeLedger.NonNegativeLedgerID, (int)Session[SessionInfo.currentUserID]);
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