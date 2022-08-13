using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;


namespace ESimSolFinancial.Controllers
{
    public class Export_LDBPController : Controller
    {
        #region Declaration
        Export_LDBP _oExport_LDBP = new Export_LDBP();
        Export_LDBPDetail _oExport_LDBPDetail = new Export_LDBPDetail();
        List<Export_LDBP> _oExport_LDBPs = new List<Export_LDBP>();

        List<ExportBill> _oExportBills = new List<ExportBill>();
        string _sErrorMessage = "";
        #endregion

        #region Actions
        #region 
        public ActionResult ViewExport_LDBPs(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oExport_LDBPs = new List<Export_LDBP>();
            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'InvoicePurchase'", (int)Session[SessionInfo.currentUserID], (Guid)Session[SessionInfo.wcfSessionID]));
            _oExport_LDBPs = Export_LDBP.WaitForApproval(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            int nBUID = 0;
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.BUID = buid;
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);
                oBusinessUnits.Add(oBusinessUnit);
                nBUID = buid;
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnits.Count > 0)
                {
                    nBUID = oBusinessUnits[0].BusinessUnitID;
                }
            }
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), nBUID, "", ((User)Session[SessionInfo.CurrentUser]).UserID);// BankBranch.GetsOwnBranchs(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BankAccounts = BankAccount.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);// BankAccount.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            return View(_oExport_LDBPs);
        }
        public ActionResult ViewAddLDBP(int nExport_LDBPDetailID, double ts)
        {
            Export_LDBPDetail oExport_LDBPDetail = new Export_LDBPDetail();
            ExportBill oExportBill = new ExportBill();
            if (nExport_LDBPDetailID > 0)
            {
                oExport_LDBPDetail = oExport_LDBPDetail.Get(nExport_LDBPDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oExportBill = oExportBill.Get(oExport_LDBPDetail.ExportBillID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oExport_LDBPDetail.ExportBill = oExportBill;
            }
            return PartialView(oExport_LDBPDetail);
        }
        #endregion

        #region Accessories
        [HttpPost]
        public JsonResult GetExport_LDBP(Export_LDBP oExport_LDBP)
        {
            _oExport_LDBP = new Export_LDBP();
            try
            {
                _oExport_LDBP = _oExport_LDBP.Get(oExport_LDBP.Export_LDBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExport_LDBP.Export_LDBPDetails = Export_LDBPDetail.Gets(oExport_LDBP.Export_LDBPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
             
            }
            catch (Exception ex)
            {
                _oExport_LDBP = new Export_LDBP();
                _oExport_LDBP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExport_LDBP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetExport_LDBPDetail(Export_LDBPDetail oExport_LDBPDetail)
        {
            _oExport_LDBPDetail = new Export_LDBPDetail();
            try
            {
                _oExport_LDBPDetail = _oExport_LDBPDetail.Get(oExport_LDBPDetail.Export_LDBPDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExport_LDBP = new Export_LDBP();
                _oExport_LDBP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExport_LDBPDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsExportBills(Export_LDBPDetail oExport_LDBPDetailTemp)
        {
            List<ExportBill> oExportBills = new List<ExportBill>();
            List<Export_LDBPDetail> oExport_LDBPDetails = new List<Export_LDBPDetail>();
            Export_LDBPDetail oExport_LDBPDetail = new Export_LDBPDetail();
            try
            {
                string sSQL = "SELECT * FROM View_ExportBill as dd where [State]=" + (int)EnumLCBillEvent.BankAcceptedBill;
                if (oExport_LDBPDetailTemp.BankBranchID > 0) sSQL = sSQL + " And BankBranchID_Bill = " + oExport_LDBPDetailTemp.BankBranchID + "";
                if (!string.IsNullOrEmpty(oExport_LDBPDetailTemp.LDBCNo)) sSQL = sSQL + " And LDBCNo like '%" + oExport_LDBPDetailTemp.LDBCNo + "%'";
                if (!string.IsNullOrEmpty(oExport_LDBPDetailTemp.ExportLCNo)) sSQL = sSQL + " And ExportLCNo like '%" + oExport_LDBPDetailTemp.ExportLCNo + "%'";
                if (!string.IsNullOrEmpty(oExport_LDBPDetailTemp.ExportBillNo)) sSQL = sSQL + " And ExportBillNo like '%" + oExport_LDBPDetailTemp.ExportBillNo + "%'";
                if (!string.IsNullOrEmpty(oExport_LDBPDetailTemp.ErrorMessage)) sSQL = sSQL + " And ExportBillID not in (" + oExport_LDBPDetailTemp.ErrorMessage + ")";
                if (oExport_LDBPDetailTemp.BUID > 0) sSQL = sSQL + " And BUID = " + oExport_LDBPDetailTemp.BUID + "";
                
                oExportBills = ExportBill.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (ExportBill oItem in oExportBills)
                {
                    oExport_LDBPDetail = new Export_LDBPDetail();
                    oExport_LDBPDetail.ExportBillID = oItem.ExportBillID;
                    oExport_LDBPDetail.LDBCNo = oItem.LDBCNo;
                    oExport_LDBPDetail.CurrencyID = oItem.CurrencyID;
                    oExport_LDBPDetail.Amount = oItem.Amount;
                    oExport_LDBPDetail.BankBranchID = oItem.BankBranchID_Nego;
                    oExport_LDBPDetail.MaturityDate = oItem.MaturityDate;
                    oExport_LDBPDetail.ExportBillNo = oItem.ExportBillNo;
                    oExport_LDBPDetail.ExportBillDate = oItem.StartDate;
                    oExport_LDBPDetail.LDBPAmount = oItem.LDBPAmount;
                    oExport_LDBPDetail.BankName_Issue = oItem.BankName_Issue;
                    oExport_LDBPDetail.ExportLCNo = oItem.ExportLCNo;
                    oExport_LDBPDetail.Status =oItem.State;
                    oExport_LDBPDetail.LDBPDate = oItem.LDBCDate;
                    oExport_LDBPDetail.NegoBank = oItem.BankName_Nego;
                    oExport_LDBPDetail.LCOpeningDate = oItem.LCOpeningDate;
                    
                    oExport_LDBPDetails.Add(oExport_LDBPDetail);
                }

            }
            catch (Exception ex)
            {
                oExport_LDBPDetail = new Export_LDBPDetail();
                oExport_LDBPDetail.ErrorMessage = ex.Message;
                oExport_LDBPDetails.Add(oExport_LDBPDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExport_LDBPDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        #endregion
        #region Functions
        private bool ValidateInput(Export_LDBP oExport_LDBP)
        {

            if (oExport_LDBP.BankAccountID <= 0)
            {
                _sErrorMessage = "Please enter, Bank account No.";
                return false;
            }
          

            return true;
        }
        private bool ValidateInput_LDBP(Export_LDBPDetail oExport_LDBPDetail)
        {

            if (oExport_LDBPDetail.CurrencyID <= 0)
            {
                _sErrorMessage = "Please select currency.";
                return false;
            }
            if (oExport_LDBPDetail.CCRate <= 0)
            {
                _sErrorMessage = "Please select currency.";
                return false;
            }

            return true;
        }
        #endregion
        [HttpPost]
        public JsonResult Save(Export_LDBP oExport_LDBP)
        {
            _oExport_LDBP = new Export_LDBP();
            try
            {
                _oExport_LDBP = oExport_LDBP;

                if (this.ValidateInput(_oExport_LDBP))
                {
                    _oExport_LDBP = _oExport_LDBP.Save(((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                else
                {
                    _oExport_LDBP.ErrorMessage = _sErrorMessage;
                }

               
            }
            catch (Exception ex)
            {
                _oExport_LDBP = new Export_LDBP();
                _oExport_LDBP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExport_LDBP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveExport_LDBPDetail(Export_LDBPDetail oExport_LDBPDetail)
        {
            _oExport_LDBPDetail = new Export_LDBPDetail();
            try
            {
                _oExport_LDBPDetail = oExport_LDBPDetail;

                if (this.ValidateInput(_oExport_LDBPDetail.Export_LDBP))
                {
                    _oExport_LDBPDetail = _oExport_LDBPDetail.Save(((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                else
                {
                    _oExport_LDBPDetail.ErrorMessage = _sErrorMessage;
                }


            }
            catch (Exception ex)
            {
                _oExport_LDBP = new Export_LDBP();
                _oExport_LDBP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExport_LDBPDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(Export_LDBP oExport_LDBP)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExport_LDBP.Delete( ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Cancel_Request(Export_LDBP oExport_LDBP)
        {

            _oExport_LDBP = new Export_LDBP();
            try
            {
                _oExport_LDBP = oExport_LDBP;

                if (this.ValidateInput(_oExport_LDBP))
                {
                    _oExport_LDBP = _oExport_LDBP.Cancel_Request(((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                else
                {
                    _oExport_LDBP.ErrorMessage = _sErrorMessage;
                }


            }
            catch (Exception ex)
            {
                _oExport_LDBP = new Export_LDBP();
                _oExport_LDBP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExport_LDBP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Cancel_LDBP_Request(Export_LDBPDetail oExport_LDBPDetail)
        {

            _oExport_LDBPDetail = new Export_LDBPDetail();
            try
            {
                _oExport_LDBPDetail = oExport_LDBPDetail;
                _oExport_LDBPDetail = _oExport_LDBPDetail.Cancel_Request(((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oExport_LDBP = new Export_LDBP();
                _oExport_LDBP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExport_LDBPDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save_LDBP(Export_LDBPDetail oExport_LDBPDetail)
        {
            _oExport_LDBPDetail = new Export_LDBPDetail();
            try
            {
                _oExport_LDBPDetail = oExport_LDBPDetail;

                if (this.ValidateInput_LDBP(oExport_LDBPDetail))
                {
                    _oExport_LDBPDetail = _oExport_LDBPDetail.Save_LDBP(((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                else
                {
                    _oExport_LDBPDetail.ErrorMessage = _sErrorMessage;
                }


            }
            catch (Exception ex)
            {
                _oExport_LDBP = new Export_LDBP();
                _oExport_LDBP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExport_LDBPDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Approved(Export_LDBP oExport_LDBP)
        {            
            try
            {
                oExport_LDBP = oExport_LDBP.Approved(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExport_LDBP = new Export_LDBP();
                oExport_LDBP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExport_LDBP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintPriviewExport_LDBP(int id)
        {
            ImportLetterSetup oImportLetterSetup = new ImportLetterSetup();

            if (id > 0)
            {
                _oExport_LDBP = _oExport_LDBP.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExport_LDBP.Export_LDBPDetails = Export_LDBPDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oExport_LDBP.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oImportLetterSetup = oImportLetterSetup.GetBy((int)EnumImportLetterType.ExportBillDiscount, (int)EnumImportLetterIssueTo.Bank,_oExport_LDBP.BUID, _oExport_LDBP.Export_LDBPID, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptExport_LDBC oReport = new rptExport_LDBC();
            byte[] abytes = oReport.PrepareReportFromSetup(_oExport_LDBP, oCompany, oBusinessUnit, oImportLetterSetup);
            return File(abytes, "application/pdf");


        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(this.ControllerContext.HttpContext.Server.MapPath("~/Content/") + "CompanyLogo.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }


        [HttpPost]
        public JsonResult WaitForApproval(Export_LDBP oExport_LDBP)
        {
            _oExport_LDBPs = new List<Export_LDBP>();
            try
            {
                _oExport_LDBPs = Export_LDBP.WaitForApproval(oExport_LDBP.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExport_LDBPs = new List<Export_LDBP>();
                oExport_LDBP = new Export_LDBP();
                oExport_LDBP.ErrorMessage = ex.Message;
                _oExport_LDBPs.Add(oExport_LDBP);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExport_LDBPs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InvoicePurchaseSearch()
        {
            List<Export_LDBP> oInvoicePurchase = new List<Export_LDBP>();
            oInvoicePurchase = Export_LDBP.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(oInvoicePurchase);
        }        
        #endregion

        #region Searching
        [HttpPost]
        public JsonResult GetbyLDBCNo(ExportBill oExportBill)
        {
            List<Export_LDBP> oExport_LDBPs = new List<Export_LDBP>();
            string sSQL = "SELECT * FROM View_Export_LDBP as ELP where ELP.Export_LDBPID in (Select Export_LDBPID from  View_Export_LDBPDetail where View_Export_LDBPDetail.LDBCNo like '%" + oExportBill.LDBCNo + "%' )";
            oExport_LDBPs = Export_LDBP.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExport_LDBPs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewAdvanceSearch()
        {
            Export_LDBP oExport_LDBP = new Export_LDBP();
            //oExport_LDBP.Users = ESimSol.BusinessObjects.User.Gets((Guid)Session[SessionInfo.wcfSessionID]);
            return PartialView(oExport_LDBP);
        }

        private string GetSQL(Export_LDBP oExport_LDBP)
        {


            string sReturn1 = "SELECT * FROM View_Export_LDBP";
            string sReturn = "";

            #region Issue Date Wise
            if (oExport_LDBP.SelectedOption > 0)
            {
                if (oExport_LDBP.SelectedOption == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LetterIssueDate,106))  = CONVERT(DATE,CONVERT(VARCHAR(12),'" + oExport_LDBP.LetterIssueDate .ToString("dd MMM yyyy") + "',106))";
                }
                if (oExport_LDBP.SelectedOption == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LetterIssueDate,106))  != CONVERT(DATE,CONVERT(VARCHAR(12),'" + oExport_LDBP.LetterIssueDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (oExport_LDBP.SelectedOption == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LetterIssueDate,106))  > CONVERT(DATE,CONVERT(VARCHAR(12),'" + oExport_LDBP.LetterIssueDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (oExport_LDBP.SelectedOption == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LetterIssueDate,106))  < CONVERT(DATE,CONVERT(VARCHAR(12),'" + oExport_LDBP.LetterIssueDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (oExport_LDBP.SelectedOption == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LetterIssueDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oExport_LDBP.LetterIssueDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oExport_LDBP.LetterIssueDate_end.ToString("dd MMM yyyy") + "',106))";
                }
                if (oExport_LDBP.SelectedOption == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LetterIssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oExport_LDBP.LetterIssueDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oExport_LDBP.LetterIssueDate_end.ToString("dd MMM yyyy") + "',106))";
                }
            }

            #endregion

            #region RequestBy
            if (oExport_LDBP.RequestBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RequestBy = " + oExport_LDBP.RequestBy;
            }
            #endregion

            #region Approved By 
            if (oExport_LDBP.ApprovedBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ApprovedBy = " + oExport_LDBP.ApprovedBy;
            }
            #endregion

            #region AccountNo By
            if (!String.IsNullOrEmpty(oExport_LDBP.AccountNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BankAccountID in (" + oExport_LDBP.AccountNo+" )";
            }
            #endregion

            #region BankBranch By
            if (oExport_LDBP.BankBranchID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BankBranchID = " + oExport_LDBP.BankBranchID;
            }
            #endregion

            #region BankID By
            if (oExport_LDBP.BankID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BankID = " + oExport_LDBP.BankID;
            }
            #endregion

            #region CurrencyType
            if (oExport_LDBP.CurrencyType)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " CurrencyType = 1";
            }
          
            #endregion

            #region BUID
            if (oExport_LDBP.BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + oExport_LDBP.BUID;
            }
            #endregion

            sReturn = sReturn1 + sReturn + " ";
            return sReturn;
        }


        [HttpPost]
        public JsonResult GetsSearchedData(Export_LDBP oExport_LDBP)
        {
            List<Export_LDBP> oExport_LDBPs = new List<Export_LDBP>();
            try
            {
                string sSQL = GetSQL(oExport_LDBP);
                oExport_LDBPs = Export_LDBP.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExport_LDBPs = new List<Export_LDBP>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExport_LDBPs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
