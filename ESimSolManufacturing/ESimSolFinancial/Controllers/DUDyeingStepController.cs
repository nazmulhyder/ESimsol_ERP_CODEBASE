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
    public class DUDyeingStepController : PdfViewController
    {
        #region Declaration
        DUDyeingStep _oDUDyeingStep = new DUDyeingStep();
        List<DUDyeingStep> _oDUDyeingSteps = new List<DUDyeingStep>();
        #endregion

        public ActionResult ViewDUDyeingSteps(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            oDUDyeingSteps = DUDyeingStep.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
           
            ViewBag.BUID = buid;
            ViewBag.BU = oBusinessUnit;
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumOrderType));
          
            return View(oDUDyeingSteps);
        }
        public ActionResult ViewDUDyeingStep(int nId, int buid, double ts)
        {
            DUDyeingStep oDUDyeingStep = new DUDyeingStep();
            if (nId > 0)
            {
                oDUDyeingStep = oDUDyeingStep.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BUID = buid;
            ViewBag.DyeingStepTypes = EnumObject.jGets(typeof(EnumDyeingStepType));
        
            ViewBag.MeasurementUnits = MeasurementUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oDUDyeingStep);
        }
     

        [HttpPost]
        public JsonResult Save(DUDyeingStep oDUDyeingStep)
        {
            oDUDyeingStep.RemoveNulls();
            _oDUDyeingStep = new DUDyeingStep();
            try
            {
                _oDUDyeingStep = oDUDyeingStep.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUDyeingStep = new DUDyeingStep();
                _oDUDyeingStep.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDyeingStep);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
    
       
        [HttpPost]
        public JsonResult Delete(DUDyeingStep oDUDyeingStep)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oDUDyeingStep.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            oDUDyeingSteps = DUDyeingStep.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDUDyeingSteps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        #region Search  by Press Enter
        //[HttpGet]
        //public JsonResult SearchByChallanNo(string sTempData,double ts)
        //{
        //    _oDUDyeingSteps = new List<DUDyeingStep>();
        //    string sSQL = "";
      
        //    sSQL = "SELECT * FROM View_DUDyeingStep WHERE ChallanNo LIKE'%" + sTempData + "%'";
           
        //    try
        //    {
        //        _oDUDyeingSteps = DUDyeingStep.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oDUDyeingStep = new DUDyeingStep();
        //        _oDUDyeingStep.ErrorMessage = ex.Message;
        //        _oDUDyeingSteps.Add(_oDUDyeingStep);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oDUDyeingSteps);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public JsonResult SearchByInvoiceNo(string sTempData, double ts)
        //{
        //    _oDUDyeingSteps = new List<DUDyeingStep>();
        //    string sSQL = "";

        //    sSQL = "SELECT * FROM View_DUDyeingStep where DUDyeingStepID in (Select DUDyeingStepID from View_DUDyeingStepDetail where View_DUDyeingStepDetail.ImportInvoiceNo  LIKE'%" + sTempData + "%')";

        //    try
        //    {
        //        _oDUDyeingSteps = DUDyeingStep.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oDUDyeingStep = new DUDyeingStep();
        //        _oDUDyeingStep.ErrorMessage = ex.Message;
        //        _oDUDyeingSteps.Add(_oDUDyeingStep);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oDUDyeingSteps);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult PrintDUDyeingStepPreview(int id)
        //{
        //    _oDUDyeingStep = new DUDyeingStep();
        //    _oDUDyeingStep = _oDUDyeingStep.Get(id, (int)Session[SessionInfo.currentUserID]);
        //    _oDUDyeingStep.DUDyeingStepDetails = DUDyeingStepDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //    BusinessUnit oBusinessUnit = new BusinessUnit();
        //    oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    rptDUDyeingStep oReport = new rptDUDyeingStep();
        //    byte[] abytes = oReport.PrepareReport(_oDUDyeingStep, oCompany, oBusinessUnit);
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
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            try
            {
                string sSQL = MakeSQL(sTemp);
                oDUDyeingSteps = DUDyeingStep.Gets( (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDUDyeingSteps = new List<DUDyeingStep>();

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUDyeingSteps);
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

            string sReturn1 = "SELECT * FROM View_DUDyeingStep ";
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
                sReturn = sReturn + "DUDyeingStepID in (Select DUDyeingStepID from View_DUDyeingStepDetail where ImportInvoiceNo  LIKE '%" + sImportInvoiceNo + "%')";
            }
            #endregion

            //#region PI No
            //if (!string.IsNullOrEmpty(oFDOrder.PINo))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " FDOID IN (SELECT FDOID FROM DUDyeingStepDetail WHERE ExportPIID IN (SELECT ExportPIID FROM ExportPI WHERE PINo LIKE '%" + oFDOrder.PINo + "%')) ";
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
