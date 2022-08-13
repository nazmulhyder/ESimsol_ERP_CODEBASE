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
    public class MeasurementUnitConController : PdfViewController
    {
        #region Declaration
        MeasurementUnitCon _oMeasurementUnitCon = new MeasurementUnitCon();
        List<MeasurementUnitCon> _oMeasurementUnitCons = new List<MeasurementUnitCon>();
        #endregion

        public ActionResult ViewMeasurementUnitCons( int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<MeasurementUnitCon> oMeasurementUnitCons = new List<MeasurementUnitCon>();
            oMeasurementUnitCons = MeasurementUnitCon.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //List<MeasurementUnit> oMeasurementUnits = new MeasurementUnit();
            //if (buid > 0)
            //{
            //    oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //}
           
            ViewBag.BUID = 0;
            return View(oMeasurementUnitCons);
        }
        public ActionResult ViewMeasurementUnitCon(int nId,  double ts)
        {
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            if (nId > 0)
            {
                oMeasurementUnitCon = oMeasurementUnitCon.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string sSQL = "select * from View_MeasurementUnitBU where MeasurementUnitConID = " + oMeasurementUnitCon.MeasurementUnitConID;
                oMeasurementUnitCon.MeasurementUnitBUs = MeasurementUnitBU.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.MeasurementUnits = MeasurementUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BussinessUnit = BusinessUnit.Gets("Select * from BusinessUnit", ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oMeasurementUnitCon);
        }
     

        [HttpPost]
        public JsonResult Save(MeasurementUnitCon oMeasurementUnitCon)
        {
            oMeasurementUnitCon.RemoveNulls();
            _oMeasurementUnitCon = new MeasurementUnitCon();
            try
            {
                _oMeasurementUnitCon = oMeasurementUnitCon.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oMeasurementUnitCon = new MeasurementUnitCon();
                _oMeasurementUnitCon.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMeasurementUnitCon);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
    
       
        [HttpPost]
        public JsonResult Delete(MeasurementUnitCon oMeasurementUnitCon)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oMeasurementUnitCon.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
            List<MeasurementUnitCon> oMeasurementUnitCons = new List<MeasurementUnitCon>();
            oMeasurementUnitCons = MeasurementUnitCon.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oMeasurementUnitCons);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        #region Search  by Press Enter
        //[HttpGet]
        //public JsonResult SearchByChallanNo(string sTempData,double ts)
        //{
        //    _oMeasurementUnitCons = new List<MeasurementUnitCon>();
        //    string sSQL = "";
      
        //    sSQL = "SELECT * FROM View_MeasurementUnitCon WHERE ChallanNo LIKE'%" + sTempData + "%'";
           
        //    try
        //    {
        //        _oMeasurementUnitCons = MeasurementUnitCon.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oMeasurementUnitCon = new MeasurementUnitCon();
        //        _oMeasurementUnitCon.ErrorMessage = ex.Message;
        //        _oMeasurementUnitCons.Add(_oMeasurementUnitCon);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oMeasurementUnitCons);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public JsonResult SearchByInvoiceNo(string sTempData, double ts)
        //{
        //    _oMeasurementUnitCons = new List<MeasurementUnitCon>();
        //    string sSQL = "";

        //    sSQL = "SELECT * FROM View_MeasurementUnitCon where MeasurementUnitConID in (Select MeasurementUnitConID from View_MeasurementUnitConDetail where View_MeasurementUnitConDetail.ImportInvoiceNo  LIKE'%" + sTempData + "%')";

        //    try
        //    {
        //        _oMeasurementUnitCons = MeasurementUnitCon.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oMeasurementUnitCon = new MeasurementUnitCon();
        //        _oMeasurementUnitCon.ErrorMessage = ex.Message;
        //        _oMeasurementUnitCons.Add(_oMeasurementUnitCon);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oMeasurementUnitCons);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult PrintMeasurementUnitConPreview(int id)
        //{
        //    _oMeasurementUnitCon = new MeasurementUnitCon();
        //    _oMeasurementUnitCon = _oMeasurementUnitCon.Get(id, (int)Session[SessionInfo.currentUserID]);
        //    _oMeasurementUnitCon.MeasurementUnitConDetails = MeasurementUnitConDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //    BusinessUnit oBusinessUnit = new BusinessUnit();
        //    oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    rptMeasurementUnitCon oReport = new rptMeasurementUnitCon();
        //    byte[] abytes = oReport.PrepareReport(_oMeasurementUnitCon, oCompany, oBusinessUnit);
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
            List<MeasurementUnitCon> oMeasurementUnitCons = new List<MeasurementUnitCon>();
            try
            {
                string sSQL = MakeSQL(sTemp);
                oMeasurementUnitCons = MeasurementUnitCon.Gets( (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oMeasurementUnitCons = new List<MeasurementUnitCon>();

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMeasurementUnitCons);
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

            string sReturn1 = "SELECT * FROM View_MeasurementUnitCon ";
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
                sReturn = sReturn + "MeasurementUnitConID in (Select MeasurementUnitConID from View_MeasurementUnitConDetail where ImportInvoiceNo  LIKE '%" + sImportInvoiceNo + "%')";
            }
            #endregion

            //#region PI No
            //if (!string.IsNullOrEmpty(oFDOrder.PINo))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " FDOID IN (SELECT FDOID FROM MeasurementUnitConDetail WHERE ExportPIID IN (SELECT ExportPIID FROM ExportPI WHERE PINo LIKE '%" + oFDOrder.PINo + "%')) ";
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
