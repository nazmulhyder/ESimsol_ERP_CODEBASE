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
using System.Web;
using ICS.Core.Utility;


namespace ESimSolFinancial.Controllers
{
    public class HeadDisplayConfigureController : Controller
    {
        #region Declaration
        HeadDisplayConfigure _oHeadDisplayConfigure = new HeadDisplayConfigure();
        List<HeadDisplayConfigure> _oHeadDisplayConfigures = new List<HeadDisplayConfigure>();
        List<ChartsOfAccount> _oChartsOfAccounts = new List<ChartsOfAccount>();
        ChartsOfAccount _oChartsOfAccount = new ChartsOfAccount();
        #endregion
        public ActionResult ViewLedgerConfigure(int id)
        {
            _oHeadDisplayConfigures = new List<HeadDisplayConfigure>();
            VoucherType oVoucherType = new VoucherType();
            oVoucherType = oVoucherType.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oHeadDisplayConfigures = HeadDisplayConfigure.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.VoucherType = oVoucherType;
            return View(_oHeadDisplayConfigures);
        }
       [HttpPost]
       public JsonResult Save(HeadDisplayConfigure oHeadDisplayConfigure)
       {
           _oHeadDisplayConfigure = new HeadDisplayConfigure();
           try
           {
               _oHeadDisplayConfigure = oHeadDisplayConfigure.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
           }
           catch (Exception ex)
           {
               _oHeadDisplayConfigure = new HeadDisplayConfigure();
               _oHeadDisplayConfigure.ErrorMessage = ex.Message;
           }
           JavaScriptSerializer serializer = new JavaScriptSerializer();
           string sjson = serializer.Serialize(_oHeadDisplayConfigure);
           return Json(sjson, JsonRequestBehavior.AllowGet);
       }
       [HttpPost]
       public JsonResult Delete(HeadDisplayConfigure oHeadDisplayConfigure)
       {
           string sFeedBackMessage = "";
           try
           {
               sFeedBackMessage = oHeadDisplayConfigure.Delete(oHeadDisplayConfigure.HeadDisplayConfigureID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public ActionResult GetChartOfAccounts(ChartsOfAccount oChartsOfAccount)
        {
            if (oChartsOfAccount.AccountHeadName == null) { oChartsOfAccount.AccountHeadName = ""; }
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oChartsOfAccounts = ChartsOfAccount.GetsbyAccountTypeOrName((int)EnumAccountType.SubGroup, oChartsOfAccount.AccountHeadName, ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
