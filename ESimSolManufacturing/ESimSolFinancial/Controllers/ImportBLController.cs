using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using System.Web.Script.Serialization;

namespace ESimSolFinancial.Controllers
{
    public class ImportBLController : Controller
    {
        ImportBL _oImportBL = new ImportBL();
        public ActionResult ViewImportBL(int nPiId, decimal ts)
        {
            ImportBL oImportBL = new ImportBL();
            ImportInvoice oImportInvoice=new ImportInvoice();
            oImportBL = oImportBL.GetByInvoice(nPiId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oImportBL.RouteLocations = RouteLocation.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            oImportBL.PIM = oImportInvoice.Get(nPiId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oImportBL.ImportInvoiceID = oImportBL.PIM.ImportInvoiceID;
            return View(oImportBL);
        }

        [HttpPost]
        public JsonResult Save(ImportBL oImportBL)
        {
            _oImportBL= new ImportBL();
            try
            {
                _oImportBL= oImportBL;
                _oImportBL = _oImportBL.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportBL= new ImportBL();
                _oImportBL.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportBL);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
