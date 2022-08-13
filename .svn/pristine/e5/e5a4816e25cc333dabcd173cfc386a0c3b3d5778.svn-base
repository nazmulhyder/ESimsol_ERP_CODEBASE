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
    public class DUOrderSetupController : PdfViewController
    {
        #region Declaration
        DUOrderSetup _oDUOrderSetup = new DUOrderSetup();
        List<DUOrderSetup> _oDUOrderSetups = new List<DUOrderSetup>();
        DUStepWiseSetup _oDUStepWiseSetup = new DUStepWiseSetup();
        #endregion

        public ActionResult ViewDUOrderSetups(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
           
            ViewBag.BUID = buid;
            ViewBag.BU = oBusinessUnit;
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumOrderType));
           
            return View(oDUOrderSetups);
        }
        public ActionResult ViewDUOrderSetup(int nId, int buid, double ts)
        {

            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            if (nId > 0)
            {
                oDUOrderSetup = oDUOrderSetup.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else oDUOrderSetup.BUID = buid;

           
            oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BusinessUnits = oBusinessUnits;
 
            ViewBag.BUID = buid;
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumOrderType));
            ViewBag.PrintNos = EnumObject.jGets(typeof(EnumExcellColumn));
            ViewBag.DeliveryValidations = EnumObject.jGets(typeof(EnumDeliveryValidation));
            ViewBag.MeasurementUnits = MeasurementUnit.Gets(EnumUniteType.Weight,((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oDUOrderSetup);
        }
      
        [HttpPost]
        public JsonResult Save(DUOrderSetup oDUOrderSetup)
        {
            oDUOrderSetup.RemoveNulls();
            _oDUOrderSetup = new DUOrderSetup();
            try
            {
                _oDUOrderSetup = oDUOrderSetup.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUOrderSetup = new DUOrderSetup();
                _oDUOrderSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUOrderSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult Delete(DUOrderSetup oDUOrderSetup)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oDUOrderSetup.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDUOrderSetups);
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
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            try
            {
                string sSQL = MakeSQL(sTemp);
                oDUOrderSetups = DUOrderSetup.Gets( (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDUOrderSetups = new List<DUOrderSetup>();

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUOrderSetups);
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

            string sReturn1 = "SELECT * FROM View_DUOrderSetup ";
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
                sReturn = sReturn + "DUOrderSetupID in (Select DUOrderSetupID from View_DUOrderSetupDetail where ImportInvoiceNo  LIKE '%" + sImportInvoiceNo + "%')";
            }
            #endregion

            //#region PI No
            //if (!string.IsNullOrEmpty(oFDOrder.PINo))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " FDOID IN (SELECT FDOID FROM DUOrderSetupDetail WHERE ExportPIID IN (SELECT ExportPIID FROM ExportPI WHERE PINo LIKE '%" + oFDOrder.PINo + "%')) ";
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


        #region Step Wise Setup
        public ActionResult ViewDUStepWiseSetups(int nId)
        {
            List<DUStepWiseSetup> oDUStepWiseSetups = new List<DUStepWiseSetup>();
            string sSQL = "SELECT * FROM DUStepWiseSetup WHERE DUOrderSetupID=" + nId;
            oDUStepWiseSetups = DUStepWiseSetup.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
           
            ViewBag.DUDyeingSteps = DUDyeingStep.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            return View(oDUStepWiseSetups);
        }

        [HttpPost]
        public JsonResult SaveStepSetup(DUStepWiseSetup oDUStepWiseSetup)
        {
            oDUStepWiseSetup.RemoveNulls();
            _oDUStepWiseSetup = new DUStepWiseSetup();
            try
            {
                _oDUStepWiseSetup = oDUStepWiseSetup.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUStepWiseSetup = new DUStepWiseSetup();
                _oDUStepWiseSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUStepWiseSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult DeleteStepSetup(DUStepWiseSetup oDUStepWiseSetup)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oDUStepWiseSetup.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
