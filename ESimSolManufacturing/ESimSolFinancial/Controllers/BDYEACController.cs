using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;
using ReportManagement;
using System.Xml.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;


namespace ESimSolFinancial.Controllers
{
    public class BDYEACController:Controller
    {
        #region Declaration
        BDYEAC _oBDYEAC = new BDYEAC();
        BDYEACDetail _oBDYEACDetail = new BDYEACDetail();
        List<BDYEAC> _oBDYEACs = new List<BDYEAC>();
        List<BDYEACDetail> _oBDYEACDetails = new List<BDYEACDetail>();
        List<ExportBill> _oExportBills = new List<ExportBill>();
        ExportBill _oExportBill = new ExportBill();
        #endregion

        #region BDYEAC
        public ActionResult ViewBDYEACs(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oBDYEACs = new List<BDYEAC>();
            //_oBDYEACs = BDYEAC.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            return View(_oBDYEACs);
        }
        public ActionResult ViewBDYEAC(int id, int buid)
        {
            _oBDYEAC = new BDYEAC();
            BusinessUnit oBusinessUnit =  new BusinessUnit();
            List<PISizerBreakDown> oPISizerBreakDowns = new List<PISizerBreakDown>();
            if (id > 0)
            {
                _oBDYEAC = _oBDYEAC.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oBDYEAC.BDYEACDetails = BDYEACDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BU = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oBDYEAC);
        }
        [HttpPost]
        public JsonResult GetbyLCNo(ExportLC oExportLC)
        {

            List<ExportLC> oExportLCs = new List<ExportLC>();
            string sReturn1 = "SELECT * FROM View_ExportLC ";
            string sReturn = "";

            #region Export LC NO
            if (!string.IsNullOrEmpty(oExportLC.ExportLCNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExportLCNo LIKE '%" + oExportLC.ExportLCNo + "'";
            }
            else
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "CurrentStatus in(" + (int)EnumExportLCStatus.None + "," + (int)EnumExportLCStatus.FreshLC + "," + (int)EnumExportLCStatus.Approved + "," + (int)EnumExportLCStatus.OutstandingLC + "," + (int)EnumExportLCStatus.RequestForAmendment + "," + (int)EnumExportLCStatus.AmendmentReceive + ") ";
            }
            #endregion

            #region BUID
            if (oExportLC.BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + oExportLC.BUID;
            }
            #endregion
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + "  ExportLCID in (SELECT ExportLCID FROM ExportBill where ExportLCID!=" + oExportLC.ExportLCID + " and  ExportBillID not in (SELECT ExportBillID FROM BDYEAC))";


            string sSQL = sReturn1 + sReturn;
            oExportLCs = ExportLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult Save(BDYEAC oBDYEAC)
        {
            _oBDYEAC = new BDYEAC();
            try
            {
               
                _oBDYEAC = oBDYEAC.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oBDYEAC = new BDYEAC();
                _oBDYEAC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBDYEAC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBDYEAC(BDYEAC oBDYEAC)
        {
            _oBDYEAC = new BDYEAC();
            try
            {
                _oBDYEAC = _oBDYEAC.Get(oBDYEAC.BDYEACID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oBDYEAC.BDYEACDetails = BDYEACDetail.Gets(oBDYEAC.BDYEACID, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
            }
            catch (Exception ex)
            {
                _oBDYEAC = new BDYEAC();
                _oBDYEAC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBDYEAC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    
        [HttpPost]
        public JsonResult DeleteBDYEAC(BDYEAC oBDYEAC)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oBDYEAC.Delete(oBDYEAC.BDYEACID,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public Image GetCompanyTitle(Company oCompany)
        {
            if (oCompany.OrganizationTitle != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyImageTitle.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationTitle);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        public Image GetSignature(UserImage oUserImage)
        {
            if (oUserImage.ImageFile != null)
            {

                string fileDirectory = Server.MapPath("~/Content/SignatureImage.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oUserImage.ImageFile);
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

        #region Other
        [HttpPost]
        public JsonResult GetsBillDetails(ExportBill oExportBill)
        {
            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            List<BDYEACDetail> oBDYEACDetails = new List<BDYEACDetail>();
            if (oExportBill.ExportBillID > 0)
            {
                oExportBillDetails = ExportBillDetail.Gets(oExportBill.ExportBillID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (ExportBillDetail oItem in oExportBillDetails)
                {
                    BDYEACDetail oBDYEACDetail = new BDYEACDetail();
                    oBDYEACDetail.ProductName = oItem.ProductName;
                    oBDYEACDetail.Qty = oItem.Qty;
                    oBDYEACDetails.Add(oBDYEACDetail);
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBDYEACDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByLC(ExportBill oExportBill)
        {

            _oExportBills = new List<ExportBill>();
            try
            {
                if (oExportBill.ExportLCID <= 0) { throw new Exception("Please select a valid Export LC."); }
                _oExportBills = ExportBill.Gets("SELECT * FROM View_ExportBill where   ExportLCID=" + oExportBill.ExportLCID + " and ExportBillID not in (SELECT ExportBillID FROM BDYEAC where ExportBillID!=" + oExportBill.ExportBillID +" and ExportLCID=" + oExportBill.ExportLCID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBills);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsByImportLC(ImportLC oImportLC)
        {
            List<ImportLC> oImportLCs = new List<ImportLC>();
             string sSQL = "SELECT * FROM View_ImportLC";
            string sReturn = "";
            if (!String.IsNullOrEmpty(oImportLC.ImportLCNo))
            {
               oImportLC.ImportLCNo = oImportLC.ImportLCNo.Trim();
               Global.TagSQL(ref sReturn);
               sReturn =sReturn+ "ImportLCNo like '%" + oImportLC.ImportLCNo + "%'";
            }
            if (oImportLC.ContractorID>0)
            {
               Global.TagSQL(ref sReturn);
               sReturn =sReturn+ "ContractorID =" + oImportLC.ContractorID + "";
            }
              if (oImportLC.BUID>0)
            {
               Global.TagSQL(ref sReturn);
               sReturn =sReturn+ "BUID =" + oImportLC.BUID + "";
            }
            //if (!String.IsNullOrEmpty(oImportLC.ErrorMessage))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "ImportLCID in (Select ImportInvoice.ImportLCID from ImportInvoice where Invoicetype in ("+(int)EnumImportPIType.LC_Foreign+","+(int)EnumImportPIType.LC_Local+") and ImportInvoiceid in (Select ImportInvoiceid from ImportInvoiceDetail where ProductID in ("+oImportLC.ErrorMessage+")))";
            //}
       
            try
            {
                sSQL=sSQL+""+sReturn;
                //if (oExportBill.ExportLCID <= 0) { throw new Exception("Please select a valid Export LC."); }
                oImportLCs = ImportLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oImportLCs = new List<ImportLC>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreatePrint(BDYEAC oBDYEAC)
        {
            _oBDYEAC = new BDYEAC();
            try
            {
                _oBDYEAC = oBDYEAC.CreatePrint(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oBDYEAC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBDYEAC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchLCNoOrBillNo(BDYEAC oBDYEAC)
        {
            _oBDYEACs = new List<BDYEAC>();
            try
            {
                string sSQL = "SELEct * FROM View_BDYEAC ";
                string sSReturn = "";
                if (oBDYEAC.ExportLCNo != "" && oBDYEAC.ExportLCNo != null)
                {
                    Global.TagSQL(ref sSReturn);
                    sSReturn += " ExportLCNo LIKE '%"+oBDYEAC.ExportLCNo+"%'";
                }
                if (oBDYEAC.ExportBillNo != "" && oBDYEAC.ExportBillNo != null)
                {
                    Global.TagSQL(ref sSReturn);
                    sSReturn += " ExportBillNo LIKE '%" + oBDYEAC.ExportBillNo + "%'";
                }
                sSQL += sSReturn;
                _oBDYEACs = BDYEAC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oBDYEAC = new BDYEAC(); 
                _oBDYEAC.ErrorMessage = ex.Message;
                _oBDYEACs.Add(_oBDYEAC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBDYEACs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult WaitForPrint(BDYEAC oBDYEAC)
        {
            _oBDYEACs = new List<BDYEAC>();
            try
            {
                string sSQL = "SELEct * FROM View_BDYEAC WHERE ISNULL(IsPrint,0) = 0";
                _oBDYEACs = BDYEAC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oBDYEAC = new BDYEAC();
                _oBDYEAC.ErrorMessage = ex.Message;
                _oBDYEACs.Add(_oBDYEAC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBDYEACs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        public ActionResult PrintBDYEACERTIFICATE(int id, double ts)
        {
            BDYEAC oBDYEAC = new BDYEAC();
            oBDYEAC = oBDYEAC.Get(id,(int)Session[SessionInfo.currentUserID]);
            oBDYEAC.BDYEACDetails = BDYEACDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            rptBDYEACERTIFICATE oReport = new rptBDYEACERTIFICATE();
            byte[] abytes = oReport.PrepareReport(oBDYEAC);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintCERTIFICATEOfOrigin(int id, double ts)
        {
            BDYEAC oBDYEAC = new BDYEAC();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBDYEAC = oBDYEAC.Get(id, (int)Session[SessionInfo.currentUserID]);
            oBDYEAC.BDYEACDetails = BDYEACDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            oBDYEAC.BusinessUnit = oBusinessUnit.Get(oBDYEAC.BUID, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            rptCertificateOfOrigin oReport = new rptCertificateOfOrigin();
            byte[] abytes = oReport.PrepareReport(oBDYEAC, oCompany);
            return File(abytes, "application/pdf");
        }

        #endregion
    }
}