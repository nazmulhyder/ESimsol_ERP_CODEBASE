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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;

namespace ESimSolFinancial.Controllers
{
    public class ImportSetupController : Controller
    {
        #region Declaration
        ImportSetup _oImportSetup = new ImportSetup();
        List<ImportSetup> _oImportSetups = new List<ImportSetup>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
       
        #endregion

        public ActionResult ViewImportSetups(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oImportSetup = new ImportSetup();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
                _oImportSetups = ImportSetup.Gets(buid,((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
                _oImportSetups = ImportSetup.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
           
            return View(_oImportSetups);
        }
        public ActionResult ViewImportSetup(int id)
        {
            _oImportSetup = _oImportSetup.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oImportSetup = new ImportSetup();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            _oImportSetup = _oImportSetup.Get(id,((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ImportFileTyps = EnumObject.jGets(typeof(EnumImportFileType));
            ViewBag.ImportDateCalBy = EnumObject.jGets(typeof(EnumImportDateCalBy));

            return View(_oImportSetup);
        }

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(ImportSetup oImportSetup)
        {
            _oImportSetup = new ImportSetup();
            try
            {
                _oImportSetup = oImportSetup;
                _oImportSetup = _oImportSetup.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oImportSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
        [HttpPost]
        public JsonResult Activate_Setup(ImportSetup oImportSetup)
        {
            _oImportSetup = new ImportSetup();
            string sMsg = "";
            _oImportSetup = oImportSetup.Activate(((User)Session[SessionInfo.CurrentUser]).UserID);

            //_oImportLetterSetup.ErrorMessage = sMsg;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region HTTP Delete

        [HttpPost]
        public JsonResult Delete(ImportSetup oImportSetup)
        {
            string sErrorMease = "";
            try
            {

                sErrorMease = oImportSetup.Delete((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
     
        #endregion

     


    }
}
