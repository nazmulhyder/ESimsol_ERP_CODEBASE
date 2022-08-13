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
    public class FDOrderSetupController : PdfViewController
    {
        #region Declaration
        FDOrderSetup _oFDOrderSetup = new FDOrderSetup();
        List<FDOrderSetup> _oFDOrderSetups = new List<FDOrderSetup>();
        #endregion

        public ActionResult ViewFDOrderSetups(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<FDOrderSetup> oFDOrderSetups = new List<FDOrderSetup>();
            oFDOrderSetups = FDOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
           
            ViewBag.BUID = buid;
            ViewBag.BU = oBusinessUnit;
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumDOType)).Where(x=>x.id!=(int)EnumDOType.None);
           
            return View(oFDOrderSetups);
        }
        public ActionResult ViewFDOrderSetup(int nId, int buid, double ts)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            FDOrderSetup oFDOrderSetup = new FDOrderSetup();
            if (nId > 0)
            {
                oFDOrderSetup = oFDOrderSetup.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else oFDOrderSetup.BUID = buid;

           
            oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BusinessUnits = oBusinessUnits;
 
            ViewBag.BUID = buid;
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumDOType)).Where(x => x.id != (int)EnumDOType.None);
           
            ViewBag.PrintNos = EnumObject.jGets(typeof(EnumExcellColumn));
            ViewBag.MeasurementUnits = MeasurementUnit.Gets(EnumUniteType.Weight,((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oFDOrderSetup);
        }
      
        [HttpPost]
        public JsonResult Save(FDOrderSetup oFDOrderSetup)
        {
            oFDOrderSetup.RemoveNulls();
            _oFDOrderSetup = new FDOrderSetup();
            try
            {
                _oFDOrderSetup = oFDOrderSetup.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFDOrderSetup = new FDOrderSetup();
                _oFDOrderSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFDOrderSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult Delete(FDOrderSetup oFDOrderSetup)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFDOrderSetup.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
            List<FDOrderSetup> oFDOrderSetups = new List<FDOrderSetup>();
            oFDOrderSetups = FDOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oFDOrderSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        #region Search  by Press Enter
       
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
            List<FDOrderSetup> oFDOrderSetups = new List<FDOrderSetup>();
            try
            {
                string sSQL = MakeSQL(sTemp);
                oFDOrderSetups = FDOrderSetup.Gets( (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFDOrderSetups = new List<FDOrderSetup>();

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFDOrderSetups);
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

            string sReturn1 = "SELECT * FROM View_FDOrderSetup ";
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
                sReturn = sReturn + "FDOrderSetupID in (Select FDOrderSetupID from View_FDOrderSetupDetail where ImportInvoiceNo  LIKE '%" + sImportInvoiceNo + "%')";
            }
            #endregion

            //#region PI No
            //if (!string.IsNullOrEmpty(oFDOrder.PINo))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " FDOID IN (SELECT FDOID FROM FDOrderSetupDetail WHERE ExportPIID IN (SELECT ExportPIID FROM ExportPI WHERE PINo LIKE '%" + oFDOrder.PINo + "%')) ";
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
