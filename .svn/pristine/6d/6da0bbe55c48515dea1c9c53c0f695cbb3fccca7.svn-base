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
using ReportManagement;


namespace ESimSolFinancial.Controllers
{
    public class ImportClaimController : PdfViewController
    {
        #region Declaration
        ImportClaim _oImportClaim = new ImportClaim();
        List<ImportClaim> _oImportClaims = new List<ImportClaim>();
        #endregion

        #region Action/JSon Result
        public ActionResult ViewImportClaims(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<ImportClaim> oImportClaims = new List<ImportClaim>();
            oImportClaims = ImportClaim.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.ClaimTypes = ClaimReason.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(oImportClaims);
        }
        public ActionResult ViewImportClaim(int nId, int buid, double ts)
        {
            ImportClaim oImportClaim = new ImportClaim();
            if (nId > 0)
            {
                oImportClaim = oImportClaim.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportClaim.ImportClaimDetails = ImportClaimDetail.Gets(oImportClaim.ImportClaimID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportClaim.ImportClaimDetails.ForEach(o => o.CurrencySymbol = oImportClaim.Currency);
            }
            oImportClaim.BUID = buid;
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUs = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ClaimTypes = ClaimReason.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ImportClaimSettleType = EnumObject.jGets(typeof(EnumImportClaimSettleType));
            return View(oImportClaim);
        }
        public ActionResult AdvSearchImportClaim()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult Save(ImportClaim oImportClaim)
        {
            oImportClaim.RemoveNulls();
            _oImportClaim = new ImportClaim();
            try
            {
                if (oImportClaim.Amount <= 0)
                {
                    oImportClaim.Amount = oImportClaim.ImportClaimDetails.Sum(x => (x.Qty*x.UnitPrice));
                }
                _oImportClaim = oImportClaim.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportClaim = new ImportClaim();
                _oImportClaim.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportClaim);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(ImportClaim oImportClaim)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oImportClaim.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult RequestImportClaim(ImportClaim oImportClaim)
        {
            _oImportClaim = new ImportClaim();
            oImportClaim.RequestBy = ((User)Session[SessionInfo.CurrentUser]).UserID;
            _oImportClaim = oImportClaim.Request(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportClaim);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ApproveImportClaim(ImportClaim oImportClaim)
        {
            _oImportClaim = new ImportClaim();
            oImportClaim.ApproveBy = ((User)Session[SessionInfo.CurrentUser]).UserID;
            _oImportClaim = oImportClaim.Approve(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportClaim);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Advance Search
       
        [HttpGet]
        public JsonResult AdvSearch(string sTemp)
        {
            List<ImportClaim> oImportClaims = new List<ImportClaim>();
            ImportClaim oImportClaim = new ImportClaim();
            try
            {
                string sSQL = GetSQL(sTemp);
                oImportClaims = ImportClaim.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {

                oImportClaim.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportClaims);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {

            //Invoice Date
            int nImportClaimDateCompare = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dtImportClaimDateStart = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dtImportClaimEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sImportInvoiceNo = sTemp.Split('~')[3];
            string sImportLCNo = sTemp.Split('~')[4];
            string sContractors = sTemp.Split('~')[5];
            int nBUID = Convert.ToInt32(sTemp.Split('~')[6]);
            int nClaimType = Convert.ToInt32(sTemp.Split('~')[7]);

            string sReturn1 = "SELECT * FROM View_ImportClaim";
            string sReturn = "";
            
            //#region BUID
            //if (nBUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "BUID = " + nBUID;
            //}
            //#endregion

            #region Invoice No

            if (sImportInvoiceNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportInvoiceNo LIKE '%" + sImportInvoiceNo + "%'";

            }
            #endregion
            #region LC No
            if (sImportLCNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportLCNo LIKE '%" + sImportLCNo + "%'";
            }
            #endregion
            #region ContractorID wise
            if (sContractors != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + sContractors + ")";
            }
            #endregion
            #region Claim Date Wise
            if (nImportClaimDateCompare > 0)
            {
                if (nImportClaimDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtImportClaimDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nImportClaimDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtImportClaimDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nImportClaimDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtImportClaimDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nImportClaimDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtImportClaimDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nImportClaimDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtImportClaimDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtImportClaimEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nImportClaimDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtImportClaimDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtImportClaimEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region Claim Type
            if (nClaimType != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ClaimReasonID = " + nClaimType;
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        #endregion

        #region GetsProduct
        [HttpPost]
        public JsonResult GetsInvoiceProduct(ImportInvoice oImportInvoice)
        {
            List<ImportInvoiceDetail> oImportInvoiceDetails = new List<ImportInvoiceDetail>();
            List<ImportInvoiceDetail> _oImportInvoiceDetails = new List<ImportInvoiceDetail>();
            if(oImportInvoice.ImportInvoiceID > 0)
            {
                oImportInvoiceDetails = ImportInvoiceDetail.Gets(oImportInvoice.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
            }
            foreach (ImportInvoiceDetail oItem in oImportInvoiceDetails)
            {
                if (oItem.Qty > 0)
                {
                    _oImportInvoiceDetails.Add(oItem);
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oImportInvoiceDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetsClaim
        [HttpPost]
        public JsonResult GetsInvoiceClaim(ImportInvoice oImportInvoice)
        {
            List<ImportInvoiceDetail> oImportInvoiceDetails = new List<ImportInvoiceDetail>();
            List<ImportInvoiceDetail> _oImportInvoiceDetails = new List<ImportInvoiceDetail>();
            if (oImportInvoice.ImportInvoiceID > 0)
            {
                oImportInvoice = oImportInvoice.Get(oImportInvoice.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
            }
            if (oImportInvoice.ImportInvoiceID > 0)
            {
                oImportInvoiceDetails = ImportInvoiceDetail.Gets(oImportInvoice.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
            }
            List<ImportClaimDetail> oImportClaimDetails = new List<ImportClaimDetail>();
            ImportClaimDetail oImportClaimDetail = new ImportClaimDetail();
            foreach (ImportInvoiceDetail oItem in oImportInvoiceDetails)
            {
                oImportClaimDetail = new ImportClaimDetail();
                if (oItem.Qty > 0)
                {
                    oImportClaimDetail.UnitPrice = oItem.UnitPrice;
                    oImportClaimDetail.ProductID = oItem.ProductID;
                    oImportClaimDetail.ProductName = oItem.ProductName;
                    oImportClaimDetail.CurrencySymbol = oItem.CurrencySymbol;
                    oImportClaimDetail.MUnit = oItem.MUName;
                    oImportClaimDetail.Qty = oItem.Qty;
                    //oImportClaimDetail.Amount = oItem.Amount;
                    oImportClaimDetails.Add(oImportClaimDetail);
                }
            }
            oImportClaimDetails.ForEach(o => o.CurrencySymbol = oImportInvoice.Currency);
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oImportClaimDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        public ActionResult LetterPrint(int nImportClaimID, int nBUID)
        {
            string sMasterLCNo = "";
            ImportClaim oImportClaim = new ImportClaim();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            ImportLetterSetup oImportLetterSetup = new ImportLetterSetup();
            Contractor oContractor = new Contractor();
            List<ImportPI> oImportPIs = new List<ImportPI>();
            List<ImportMasterLC> oImportMasterLCs = new List<ImportMasterLC>();

            #region Print Setup
            if (nImportClaimID > 0)
            {
                _oImportClaim = oImportClaim.Get(nImportClaimID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
                //if (!String.IsNullOrEmpty(_oImportClaim.ContractorName))
                //{
                //    _oImportClaim.ImportClaimNo = _oImportClaim.ImportClaimNo + "(Bangladesh Bank DC No: " + _oImportClaim.BBankRefNo + ")";
                //}

                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                _oImportClaim.ImportClaimDetails = ImportClaimDetail.Gets(nImportClaimID,((User)Session[SessionInfo.CurrentUser]).UserID);
             
                oImportLetterSetup = oImportLetterSetup.GetBy((int)EnumImportLetterType.Import_Claim_Report, (int)EnumImportLetterIssueTo.Supplier, nBUID, nImportClaimID, "", ((User)Session[SessionInfo.CurrentUser]).UserID); //sSQL=""
                oImportLetterSetup.ContractorAddress = oContractor.Address;
                oImportLetterSetup.MasterLCs = sMasterLCNo;
            }
            #endregion

            //_oImportClaim.PINos = sPINoWithDate+".";
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
            rptImportClaimLetterIssue oReport = new rptImportClaimLetterIssue();
            byte[] abytes = oReport.PreparePrintLetter(_oImportClaim, oImportLetterSetup, oImportPIs, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region Get Company Logo
        public System.Drawing.Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion
        public JsonResult Gets()
        {
            List<ImportClaim> oImportClaims = new List<ImportClaim>();
            oImportClaims = ImportClaim.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oImportClaims);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
