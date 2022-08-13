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
    public class ImportInvChallanController : PdfViewController
    {
        #region Declaration
        ImportInvChallan _oImportInvChallan = new ImportInvChallan();
        List<ImportInvChallan> _oImportInvChallans = new List<ImportInvChallan>();
        #endregion

        public ActionResult ViewImportInvChallans(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<ImportInvChallan> oImportInvChallans = new List<ImportInvChallan>();
            oImportInvChallans = ImportInvChallan.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
           
            ViewBag.BUID = buid;
            ViewBag.BU = oBusinessUnit;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ImportInvoiceChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            return View(oImportInvChallans);
        }
        public ActionResult ViewImportInvChallan(int nId, int buid, double ts)
        {
            ImportInvChallan oImportInvChallan = new ImportInvChallan();
            List<ImportInvChallanDetail> oImportInvChallanDetails = new List<ImportInvChallanDetail>();
            if (nId > 0)
            {
                oImportInvChallan = oImportInvChallan.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportInvChallanDetails = ImportInvChallanDetail.Gets(oImportInvChallan.ImportInvChallanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.ImportInvChallanDetails = oImportInvChallanDetails;
            return View(oImportInvChallan);
        }
     

        [HttpPost]
        public JsonResult Save(ImportInvChallan oImportInvChallan)
        {
            oImportInvChallan.RemoveNulls();
            _oImportInvChallan = new ImportInvChallan();
            try
            {
                _oImportInvChallan = oImportInvChallan.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportInvChallan = new ImportInvChallan();
                _oImportInvChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult Approved(ImportInvChallan oImportInvChallan)
        {
            oImportInvChallan.RemoveNulls();
            _oImportInvChallan = new ImportInvChallan();
            try
            {
                _oImportInvChallan = oImportInvChallan.Approved(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportInvChallan = new ImportInvChallan();
                _oImportInvChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Received(ImportInvChallan oImportInvChallan)
        {
            oImportInvChallan.RemoveNulls();
            _oImportInvChallan = new ImportInvChallan();
            try
            {
                _oImportInvChallan = oImportInvChallan.Received(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportInvChallan = new ImportInvChallan();
                _oImportInvChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public JsonResult Delete(ImportInvChallan oImportInvChallan)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oImportInvChallan.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets()
        {
            List<ImportInvChallan> oImportInvChallans = new List<ImportInvChallan>();
            oImportInvChallans = ImportInvChallan.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oImportInvChallans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsPackingList(ImportInvChallanDetail oImportInvChallanDetail)
        {
            string sSQL = "";
            List<ImportInvChallanDetail> oImportInvChallanDetails = new List<ImportInvChallanDetail>();
            try
            {


                sSQL = "Select top(100)* from View_ImportPackDetailForChallan";
                    string sReturn = "";
                    if (!String.IsNullOrEmpty(oImportInvChallanDetail.ImportInvoiceNo))
                    {
                        oImportInvChallanDetail.ImportInvoiceNo = oImportInvChallanDetail.ImportInvoiceNo.Trim();
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "ImportInvoiceNo Like'%" + oImportInvChallanDetail.ImportInvoiceNo + "%'";
                    }
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "Qty>Isnull(Qty_IPL,0) order by ImportInvoiceID,ProductID,LotNo";

                    sSQL = sSQL + "" + sReturn;
                    oImportInvChallanDetails = ImportInvChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                      foreach(ImportInvChallanDetail oItem in  oImportInvChallanDetails)
                      {
                          oItem.Qty = oItem.Qty - oItem.Qty_GRN - oItem.Qty_IPL;
                          oItem.QtyPerPack = oItem.QtyPerPack;
                          oItem.NumberOfPack = oItem.NumberOfPack - oItem.NumberOfPack_CD;
                      }


            }
            catch (Exception ex)
            {
                oImportInvChallanDetails = new List<ImportInvChallanDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportInvChallanDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region Search  by Press Enter
        [HttpGet]
        public JsonResult SearchByChallanNo(string sTempData,double ts)
        {
            _oImportInvChallans = new List<ImportInvChallan>();
            string sSQL = "";
      
            sSQL = "SELECT * FROM View_ImportInvChallan WHERE ChallanNo LIKE'%" + sTempData + "%'";
           
            try
            {
                _oImportInvChallans = ImportInvChallan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportInvChallan = new ImportInvChallan();
                _oImportInvChallan.ErrorMessage = ex.Message;
                _oImportInvChallans.Add(_oImportInvChallan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvChallans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByInvoiceNo(string sTempData, double ts)
        {
            _oImportInvChallans = new List<ImportInvChallan>();
            string sSQL = "";

            sSQL = "SELECT * FROM View_ImportInvChallan where ImportInvChallanID in (Select ImportInvChallanID from View_ImportInvChallanDetail where View_ImportInvChallanDetail.ImportInvoiceNo  LIKE'%" + sTempData + "%')";

            try
            {
                _oImportInvChallans = ImportInvChallan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportInvChallan = new ImportInvChallan();
                _oImportInvChallan.ErrorMessage = ex.Message;
                _oImportInvChallans.Add(_oImportInvChallan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvChallans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintImportInvChallanPreview(int id)
        {
            _oImportInvChallan = new ImportInvChallan();
            _oImportInvChallan = _oImportInvChallan.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oImportInvChallan.ImportInvChallanDetails = ImportInvChallanDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptImportInvChallan oReport = new rptImportInvChallan();
            byte[] abytes = oReport.PrepareReport(_oImportInvChallan, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
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


        #region Adv Search
        public JsonResult AdvSearch(string sTemp)
        {
            List<ImportInvChallan> oImportInvChallans = new List<ImportInvChallan>();
            try
            {
                string sSQL = MakeSQL(sTemp);
                oImportInvChallans = ImportInvChallan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oImportInvChallans = new List<ImportInvChallan>();

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportInvChallans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL(string sTemp)
        {

            int ncboChallanDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime txtChallanDateFrom = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime txtChallanDateTo = Convert.ToDateTime(sTemp.Split('~')[2]);
            //
            string sChallanNo = Convert.ToString(sTemp.Split('~')[3]);
            string sImportInvoiceNo = Convert.ToString(sTemp.Split('~')[4]);

            string sReturn1 = "SELECT * FROM View_ImportInvChallan ";
            string sReturn = "";



            #region Challan No
            if (!string.IsNullOrEmpty(sChallanNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ChallanNo LIKE '%" + sChallanNo + "%' ";
            }
            #endregion

            #region SC No
            if (!string.IsNullOrEmpty(sImportInvoiceNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ImportInvChallanID in (Select ImportInvChallanID from View_ImportInvChallanDetail where ImportInvoiceNo  LIKE '%" + sImportInvoiceNo + "%')";
            }
            #endregion

            //#region PI No
            //if (!string.IsNullOrEmpty(oFDOrder.PINo))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " FDOID IN (SELECT FDOID FROM ImportInvChallanDetail WHERE ExportPIID IN (SELECT ExportPIID FROM ExportPI WHERE PINo LIKE '%" + oFDOrder.PINo + "%')) ";
            //}
            //#endregion

            //#region Delivery To
            //if (!string.IsNullOrEmpty(sApplicantIDs))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " ContractorID in( " + sApplicantIDs + ")";
            //}
            //#endregion

            #region Issue Date Wise
            if (ncboChallanDate > 0)
            {
                if (ncboChallanDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtChallanDateFrom.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboChallanDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtChallanDateFrom.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboChallanDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtChallanDateFrom.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboChallanDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtChallanDateFrom.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboChallanDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtChallanDateFrom.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtChallanDateTo.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboChallanDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtChallanDateFrom.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtChallanDateTo.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn + " ORDER BY ChallanDate DESC";
            return sSQL;
        }
        public ActionResult AdvanceSearch()
        {
            return PartialView();
        }
        #endregion

    }
}
