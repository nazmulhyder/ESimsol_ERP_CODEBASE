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


namespace ESimSolFinancial.Controllers
{
    public class ExportDocForwardingController : Controller
    {
        #region Declaration
        ExportDocForwarding _oExportDocForwarding = new ExportDocForwarding();
        List<ExportDocForwarding> _oExportDocForwardings = new List<ExportDocForwarding>();
        string _sErrorMessage = "";
        #endregion

      
        [HttpPost]
        public JsonResult GetbyExportBill(ExportBill oExportBill)
        {
            ExportDocForwarding oExportDocForwarding = new ExportDocForwarding();
            _oExportDocForwardings = new List<ExportDocForwarding>();

            List<ExportDocSetup> oExportDocSetups = new List<ExportDocSetup>();


            if (oExportBill.ExportBillID > 0)
            {
                _oExportDocForwardings = ExportDocForwarding.Gets(oExportBill.ExportLCID,(int)EnumMasterLCType.ExportLC,  ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (_oExportDocForwardings.Count <= 0)
            {
                oExportDocSetups = ExportDocSetup.Gets(true, oExportBill.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (ExportDocSetup oItem in oExportDocSetups)
                {
                    oExportDocForwarding = new ExportDocForwarding();
                    oExportDocForwarding.ReferenceID = oExportBill.ExportBillID;
                    oExportDocForwarding.ExportDocID = oItem.ExportDocSetupID;
                    oExportDocForwarding.Name_Print = oItem.DocName;
                    oExportDocForwarding.Name_Doc = oItem.DocName;
                    _oExportDocForwardings.Add(oExportDocForwarding);
                }
                oExportDocForwarding = new ExportDocForwarding();
                oExportDocForwarding.ReferenceID = oExportBill.ExportBillID;
                oExportDocForwarding.ExportDocID = 0;
                oExportDocForwarding.Name_Print = "MUSOK-11"; //Ratin
                _oExportDocForwardings.Add(oExportDocForwarding);
                oExportDocForwarding = new ExportDocForwarding();
                oExportDocForwarding.ReferenceID = oExportBill.ExportBillID;
                oExportDocForwarding.ExportDocID = 0;
                oExportDocForwarding.Name_Print = "BACK TO BACK L/C(ORIGINAL)";
                _oExportDocForwardings.Add(oExportDocForwarding);
                oExportDocForwarding = new ExportDocForwarding();
                oExportDocForwarding.ReferenceID = oExportBill.ExportBillID;
                oExportDocForwarding.ExportDocID = 0;
                oExportDocForwarding.Name_Print = "BTMA CERTIFICATE";
                _oExportDocForwardings.Add(oExportDocForwarding);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportDocForwardings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetbyExportBill_Reload(ExportBill oExportBill)
        {
            ExportDocForwarding oExportDocForwarding = new ExportDocForwarding();
            _oExportDocForwardings = new List<ExportDocForwarding>();

            List<ExportDocSetup> oExportDocSetups = new List<ExportDocSetup>();
            string sErrormessage="";

            if (oExportBill.ExportBillID > 0)
            {
                 oExportDocForwarding = oExportDocForwarding.DeleteBYExportBillID(oExportBill.ExportBillID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            if (oExportDocForwarding.ErrorMessage == "" || oExportDocForwarding.ErrorMessage == null)
            {
                if (_oExportDocForwardings.Count <= 0)
                {
                    oExportDocSetups = ExportDocSetup.Gets(true, oExportBill.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (ExportDocSetup oItem in oExportDocSetups)
                    {
                        oExportDocForwarding = new ExportDocForwarding();
                        oExportDocForwarding.ReferenceID = oExportBill.ExportBillID;
                        oExportDocForwarding.ExportDocID = oItem.ExportDocSetupID;
                        oExportDocForwarding.Name_Print = oItem.DocName;
                        oExportDocForwarding.Name_Doc = oItem.DocName;
                        _oExportDocForwardings.Add(oExportDocForwarding);
                    }
                    oExportDocForwarding = new ExportDocForwarding();
                    oExportDocForwarding.ReferenceID = oExportBill.ExportBillID;
                    oExportDocForwarding.ExportDocID = 0;
                    oExportDocForwarding.Name_Print = "MUSOK-11";
                    _oExportDocForwardings.Add(oExportDocForwarding);
                    oExportDocForwarding = new ExportDocForwarding();
                    oExportDocForwarding.ReferenceID = oExportBill.ExportBillID;
                    oExportDocForwarding.ExportDocID = 0;
                    oExportDocForwarding.Name_Print = "BACK TO BACK L/C(ORIGINAL)";
                    _oExportDocForwardings.Add(oExportDocForwarding);
                    oExportDocForwarding = new ExportDocForwarding();
                    oExportDocForwarding.ReferenceID = oExportBill.ExportBillID;
                    oExportDocForwarding.ExportDocID = 0;
                    oExportDocForwarding.Name_Print = "BTMA CERTIFICATE";
                    _oExportDocForwardings.Add(oExportDocForwarding);
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportDocForwardings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveExportDocForwarding(ExportDocForwarding oExportDocForwarding)
        {
            _oExportDocForwarding = new ExportDocForwarding();
            try
            {

                _oExportDocForwarding = oExportDocForwarding;
                if (_oExportDocForwarding.ExportDocForwardings.Count > 0)
                {
                    _oExportDocForwarding = _oExportDocForwarding.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oExportDocForwarding = new ExportDocForwarding();
                _oExportDocForwarding.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportDocForwarding);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteExportDocForwarding(ExportDocForwarding oExportDocForwarding)
        {
            string sFeedBackMessage = "";
            try
            {
                _oExportDocForwarding = new ExportDocForwarding();

                sFeedBackMessage = _oExportDocForwarding.Delete(oExportDocForwarding.ExportDocForwardingID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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