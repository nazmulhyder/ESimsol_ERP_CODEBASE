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
    public class ExportPIPrintSetupController : Controller
    {
        #region Declaration
        ExportPIPrintSetup _oExportPIPrintSetup = new ExportPIPrintSetup();
        List<ExportPIPrintSetup> _oExportPIPrintSetups = new List<ExportPIPrintSetup>();
        string _sErrorMessage = "";
        #endregion

        #region Actions
        public ActionResult ViewExportPIPrintSetups(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oExportPIPrintSetups = new List<ExportPIPrintSetup>();
           
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                _oExportPIPrintSetups = ExportPIPrintSetup.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                _oExportPIPrintSetups = ExportPIPrintSetup.Gets( ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PrintNoList = EnumObject.jGets(typeof(EnumExcellColumn));
            ViewBag.BUID = buid;
            return View(_oExportPIPrintSetups);
        }

        [HttpPost]
        public JsonResult Save(ExportPIPrintSetup oExportPIPrintSetup)
        {
            _oExportPIPrintSetup = new ExportPIPrintSetup();
            try
            {
                _oExportPIPrintSetup = oExportPIPrintSetup;
                _oExportPIPrintSetup = _oExportPIPrintSetup.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPIPrintSetup = new ExportPIPrintSetup();
                _oExportPIPrintSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIPrintSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get(ExportPIPrintSetup oExportPIPrintSetup)
        {
            _oExportPIPrintSetup = new ExportPIPrintSetup();
            try
            {
                if (oExportPIPrintSetup.ExportPIPrintSetupID <= 0) { throw new Exception("Please select a valid contractor."); }
                _oExportPIPrintSetup = _oExportPIPrintSetup.Get(oExportPIPrintSetup.ExportPIPrintSetupID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPIPrintSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIPrintSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(ExportPIPrintSetup oExportPIPrintSetup)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportPIPrintSetup.Delete(oExportPIPrintSetup.ExportPIPrintSetupID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActiveInactive(ExportPIPrintSetup oExportPIPrintSetup)
        {
            _oExportPIPrintSetup = new ExportPIPrintSetup();
            try
            {

                _oExportPIPrintSetup = oExportPIPrintSetup.ActivatePIPrintSetup(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPIPrintSetup = new ExportPIPrintSetup();
                _oExportPIPrintSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIPrintSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetActivePrintSetup(ExportPIPrintSetup oExportPIPrintSetup)
        {
            _oExportPIPrintSetup = new ExportPIPrintSetup();
            try
            {
                _oExportPIPrintSetup = _oExportPIPrintSetup.Get(true, oExportPIPrintSetup.BUID,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportPIPrintSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIPrintSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetActivePrintSetupBYPI(ExportPI oExportPI)
        {
            _oExportPIPrintSetup = new ExportPIPrintSetup();
            try
            {
                ExportPIShipment _oExportPIShipment = new ExportPIShipment();
                if (oExportPI.ExportPIID > 0)
                {
                    _oExportPIShipment = _oExportPIShipment.GetByExportPIID(oExportPI.ExportPIID, (int)Session[SessionInfo.currentUserID]);
                }
                _oExportPIPrintSetup = _oExportPIPrintSetup.Get(true, oExportPI.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oExportPIShipment.ExportPIShipmentID > 0)
                {
                    _oExportPIPrintSetup.ShipmentBy = _oExportPIShipment.ShipmentBy;
                    _oExportPIPrintSetup.PlaceOfDelivery = _oExportPIShipment.DestinationPort;
                }

            }
            catch (Exception ex)
            {
                _oExportPIPrintSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIPrintSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}