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
    public class ExportBankForwardingController : Controller
    {
        #region Declaration
        ExportBankForwarding _oExportBankForwarding = new ExportBankForwarding();
        List<ExportBankForwarding> _oExportBankForwardings = new List<ExportBankForwarding>();
        string _sErrorMessage = "";
        #endregion

        [HttpPost]
        public JsonResult GetbyExportBill(ExportBill oExportBill)
        {
            ExportBankForwarding oExportBankForwarding = new ExportBankForwarding();
            _oExportBankForwardings = new List<ExportBankForwarding>();

            List<ExportDocSetup> oExportDocSetups = new List<ExportDocSetup>();


            if (oExportBill.ExportBillID > 0)
            {
                _oExportBankForwardings = ExportBankForwarding.Gets(oExportBill.ExportBillID,  ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (_oExportBankForwardings.Count <= 0)
            {
                oExportDocSetups = ExportDocSetup.Gets(true, oExportBill.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (ExportDocSetup oItem in oExportDocSetups)
                {
                    oExportBankForwarding = new ExportBankForwarding();
                    oExportBankForwarding.ExportBillID = oExportBill.ExportBillID;
                    //oExportBankForwarding.ExportBankID = oItem.ExportDocSetupID;
                    oExportBankForwarding.Name_Print = oItem.DocName;
                    //oExportBankForwarding.Name_Bank = oItem.DocName;
                    _oExportBankForwardings.Add(oExportBankForwarding);
                }
                oExportBankForwarding = new ExportBankForwarding();
                oExportBankForwarding.ExportBillID = oExportBill.ExportBillID;
                //oExportBankForwarding.ExportBankID = 0;
                oExportBankForwarding.Name_Print = "MUSOK-11"; //Ratin
                _oExportBankForwardings.Add(oExportBankForwarding);
                oExportBankForwarding = new ExportBankForwarding();
                oExportBankForwarding.ExportBillID = oExportBill.ExportBillID;
                //oExportBankForwarding.ExportBankID = 0;
                oExportBankForwarding.Name_Print = "BACK TO BACK L/C(ORIGINAL)";
                _oExportBankForwardings.Add(oExportBankForwarding);
                oExportBankForwarding = new ExportBankForwarding();
                oExportBankForwarding.ExportBillID = oExportBill.ExportBillID;
                //oExportBankForwarding.ExportBankID = 0;
                oExportBankForwarding.Name_Print = "BTMA CERTIFICATE";
                _oExportBankForwardings.Add(oExportBankForwarding);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBankForwardings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetbyExportBill_Reload(ExportBill oExportBill)
        {
            ExportBankForwarding oExportBankForwarding = new ExportBankForwarding();
            _oExportBankForwardings = new List<ExportBankForwarding>();

            List<ExportDocSetup> oExportDocSetups = new List<ExportDocSetup>();
            string sErrormessage="";

            if (oExportBill.ExportBillID > 0)
            {
                 oExportBankForwarding = oExportBankForwarding.DeleteBYExportBillID(oExportBill.ExportBillID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            if (oExportBankForwarding.ErrorMessage == "" || oExportBankForwarding.ErrorMessage == null)
            {
                if (_oExportBankForwardings.Count <= 0)
                {
                    oExportDocSetups = ExportDocSetup.Gets(true, oExportBill.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (ExportDocSetup oItem in oExportDocSetups)
                    {
                        oExportBankForwarding = new ExportBankForwarding();
                        oExportBankForwarding.ExportBillID = oExportBill.ExportBillID;
                        //oExportBankForwarding.ExportBankID = oItem.ExportDocSetupID;
                        oExportBankForwarding.Name_Print = oItem.DocName;
                        //oExportBankForwarding.Name_Bank = oItem.BankName;
                        _oExportBankForwardings.Add(oExportBankForwarding);
                    }
                    oExportBankForwarding = new ExportBankForwarding();
                    oExportBankForwarding.ExportBillID = oExportBill.ExportBillID;
                    oExportBankForwarding.Copies = 0;
                    oExportBankForwarding.Name_Print = "MUSOK-11";
                    _oExportBankForwardings.Add(oExportBankForwarding);
                    oExportBankForwarding = new ExportBankForwarding();
                    oExportBankForwarding.ExportBillID = oExportBill.ExportBillID;
                    oExportBankForwarding.Copies = 0;
                    oExportBankForwarding.Name_Print = "BACK TO BACK L/C(ORIGINAL)";
                    _oExportBankForwardings.Add(oExportBankForwarding);
                    oExportBankForwarding = new ExportBankForwarding();
                    oExportBankForwarding.ExportBillID = oExportBill.ExportBillID;
                    oExportBankForwarding.Copies = 0;
                    oExportBankForwarding.Name_Print = "BTMA CERTIFICATE";
                    _oExportBankForwardings.Add(oExportBankForwarding);
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBankForwardings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveExportBankForwarding(ExportBankForwarding oExportBankForwarding)
        {
            _oExportBankForwarding = new ExportBankForwarding();
            try
            {

                _oExportBankForwarding = oExportBankForwarding;
                if (_oExportBankForwarding.ExportBankForwardings.Count > 0)
                {
                    _oExportBankForwarding = _oExportBankForwarding.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oExportBankForwarding = new ExportBankForwarding();
                _oExportBankForwarding.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBankForwarding);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteExportBankForwarding(ExportBankForwarding oExportBankForwarding)
        {
            string sFeedBackMessage = "";
            try
            {
                _oExportBankForwarding = new ExportBankForwarding();

                sFeedBackMessage = _oExportBankForwarding.Delete(oExportBankForwarding.ExportBankForwardingID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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