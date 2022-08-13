using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Metadata.Edm;
using System.Globalization;
using System.Linq;
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
    public class ImportLCClauseSetupController : Controller
    {
        #region declaration

        ImportLCClauseSetup _oImportLCClause = new ImportLCClauseSetup();
        List<ImportLCClauseSetup> _oImportLCClauses = new List<ImportLCClauseSetup>();

        #endregion

        #region View 

        public ActionResult ViewImportLCClauseSetups(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oImportLCClauses = new List<ImportLCClauseSetup>();
            _oImportLCClauses = ImportLCClauseSetup.Gets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.LCPaymentTypes = EnumObject.jGets(typeof(EnumLCPaymentType));
            ViewBag.LCAppTypes = EnumObject.jGets(typeof(EnumLCAppType));
            ViewBag.ProductNatures = EnumObject.jGets(typeof(EnumProductNature));
            return View(_oImportLCClauses);

        }

        #endregion

        #region Http
        
        [HttpPost]
        public JsonResult Save(ImportLCClauseSetup oImportLCClause)
        {
            _oImportLCClause = new ImportLCClauseSetup();
            _oImportLCClause = oImportLCClause.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLCClause);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

       
        [HttpPost]
        public JsonResult Delete(ImportLCClauseSetup oImportLCClause)
        {
            try
            {
                if (oImportLCClause.ImportLCClauseSetupID <= 0) { throw new Exception("Please select an valid item."); }
                oImportLCClause = oImportLCClause.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oImportLCClause = new ImportLCClauseSetup();
                oImportLCClause.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportLCClause.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

       
        [HttpPost]
        public JsonResult ActivityChange(ImportLCClauseSetup oImportLCClauseSetup)
        {
            try
            {
                if (oImportLCClauseSetup.ImportLCClauseSetupID <= 0) { throw new Exception("Please select an valid item."); }
                bool val = !oImportLCClauseSetup.Activity;
                oImportLCClauseSetup.Activity = val;
                oImportLCClauseSetup = oImportLCClauseSetup.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oImportLCClauseSetup = new ImportLCClauseSetup();
                oImportLCClauseSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportLCClauseSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Advanced Search
       
        [HttpPost]
        public JsonResult AdvSearch(ImportLCClauseSetup oImportLCClauseSetup)
        {
            List<ImportLCClauseSetup> oImportLCClauseSetups = new List<ImportLCClauseSetup>();
            _oImportLCClause = new ImportLCClauseSetup();
            try
            {
                string sSQL = GetSQL(oImportLCClauseSetup);
                oImportLCClauseSetups = ImportLCClauseSetup.GetsWithSQL(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {

                oImportLCClauseSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportLCClauseSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(ImportLCClauseSetup oImportLCClauseSetup)
        {
            string sClause = oImportLCClauseSetup.Clause;
            int PaymentType = (int)oImportLCClauseSetup.LCPaymentType;
            int LCAppType =(int) oImportLCClauseSetup.LCAppType;
            int productType =(int) oImportLCClauseSetup.ProductType;
            bool isActive = oImportLCClauseSetup.Activity;
            bool isMandatority = oImportLCClauseSetup.IsMandatory;

            string sReturn1 = "SELECT * FROM ImportLCClauseSetup";
            string sReturn = "";

            #region Clause

            if (!string.IsNullOrEmpty(sClause))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Clause LIKE '%" + sClause + "%'";

            }
            #endregion

            #region PaymentType
            if (PaymentType>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PaymentType IN '(" + PaymentType + ")'";
            }
            #endregion

            #region LCAppType

            if (LCAppType >0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LCAppType IN (" + LCAppType + ")";
            }
            #endregion
            #region productType

            if (productType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " productType IN (" + productType + ")";
            }
            #endregion

            #region Activity

            if (isActive)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Activity =" + Convert.ToInt16(isActive);
            }
            #endregion

            #region IsMandatory

            if (isMandatority)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " IsMandatory =" + Convert.ToInt16(isMandatority);
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        #endregion

    }
}

