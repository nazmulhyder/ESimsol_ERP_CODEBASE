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
    public class FabricPOSetupController : PdfViewController
    {
        #region Declaration
        FabricPOSetup _oFabricPOSetup = new FabricPOSetup();
        List<FabricPOSetup> _oFabricPOSetups = new List<FabricPOSetup>();
        #endregion

        public ActionResult ViewFabricPOSetups(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<FabricPOSetup> oFabricPOSetups = new List<FabricPOSetup>();
            oFabricPOSetups = FabricPOSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
           
            ViewBag.BUID = buid;
            ViewBag.BU = oBusinessUnit;
          
            return View(oFabricPOSetups);
        }
        public ActionResult ViewFabricPOSetup(int nId, int buid, double ts)
        {
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            if (nId > 0)
            {
                oFabricPOSetup = oFabricPOSetup.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
             
            }
            ViewBag.BUID = buid;
            ViewBag.PrintNos = EnumObject.jGets(typeof(EnumExcellColumn));
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oFabricPOSetup);
        }

        [HttpPost]
        public JsonResult Save(FabricPOSetup oFabricPOSetup)
        {
            oFabricPOSetup.RemoveNulls();
            _oFabricPOSetup = new FabricPOSetup();
            try
            {
                _oFabricPOSetup = oFabricPOSetup.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricPOSetup = new FabricPOSetup();
                _oFabricPOSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricPOSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult Delete(FabricPOSetup oFabricPOSetup)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabricPOSetup.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
            List<FabricPOSetup> oFabricPOSetups = new List<FabricPOSetup>();
            oFabricPOSetups = FabricPOSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oFabricPOSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        #region Search  by Press Enter
        //[HttpGet]
        //public JsonResult SearchByChallanNo(string sTempData,double ts)
        //{
        //    _oFabricPOSetups = new List<FabricPOSetup>();
        //    string sSQL = "";
      
        //    sSQL = "SELECT * FROM View_FabricPOSetup WHERE ChallanNo LIKE'%" + sTempData + "%'";
           
        //    try
        //    {
        //        _oFabricPOSetups = FabricPOSetup.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oFabricPOSetup = new FabricPOSetup();
        //        _oFabricPOSetup.ErrorMessage = ex.Message;
        //        _oFabricPOSetups.Add(_oFabricPOSetup);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oFabricPOSetups);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public JsonResult SearchByInvoiceNo(string sTempData, double ts)
        //{
        //    _oFabricPOSetups = new List<FabricPOSetup>();
        //    string sSQL = "";

        //    sSQL = "SELECT * FROM View_FabricPOSetup where FabricPOSetupID in (Select FabricPOSetupID from View_FabricPOSetupDetail where View_FabricPOSetupDetail.ImportInvoiceNo  LIKE'%" + sTempData + "%')";

        //    try
        //    {
        //        _oFabricPOSetups = FabricPOSetup.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oFabricPOSetup = new FabricPOSetup();
        //        _oFabricPOSetup.ErrorMessage = ex.Message;
        //        _oFabricPOSetups.Add(_oFabricPOSetup);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oFabricPOSetups);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult PrintFabricPOSetupPreview(int id)
        //{
        //    _oFabricPOSetup = new FabricPOSetup();
        //    _oFabricPOSetup = _oFabricPOSetup.Get(id, (int)Session[SessionInfo.currentUserID]);
        //    _oFabricPOSetup.FabricPOSetupDetails = FabricPOSetupDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //    BusinessUnit oBusinessUnit = new BusinessUnit();
        //    oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    rptFabricPOSetup oReport = new rptFabricPOSetup();
        //    byte[] abytes = oReport.PrepareReport(_oFabricPOSetup, oCompany, oBusinessUnit);
        //    return File(abytes, "application/pdf");
        //}
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
            List<FabricPOSetup> oFabricPOSetups = new List<FabricPOSetup>();
            try
            {
                string sSQL = MakeSQL(sTemp);
                oFabricPOSetups = FabricPOSetup.Gets( (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFabricPOSetups = new List<FabricPOSetup>();

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricPOSetups);
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

            string sReturn1 = "SELECT * FROM View_FabricPOSetup ";
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
                sReturn = sReturn + "FabricPOSetupID in (Select FabricPOSetupID from View_FabricPOSetupDetail where ImportInvoiceNo  LIKE '%" + sImportInvoiceNo + "%')";
            }
            #endregion

            //#region PI No
            //if (!string.IsNullOrEmpty(oFDOrder.PINo))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " FDOID IN (SELECT FDOID FROM FabricPOSetupDetail WHERE ExportPIID IN (SELECT ExportPIID FROM ExportPI WHERE PINo LIKE '%" + oFDOrder.PINo + "%')) ";
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
