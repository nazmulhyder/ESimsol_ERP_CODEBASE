using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;
using System.Xml.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;


namespace ESimSolFinancial.Controllers
{
    public class ExportSC_CommonController : Controller
    {
        #region Declaration
        ExportSC _oExportSC = new ExportSC();
        ExportSCDetail _oExportSCDetail = new ExportSCDetail();
        List<ExportSC> _oExportSCs = new List<ExportSC>();
        List<ExportSCDetail> _oExportSCDetails = new List<ExportSCDetail>();
     
        #endregion

        #region Export SC
        public ActionResult ViewExportSCs(int ProductNature, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oExportSCs = new List<ExportSC>();
            _oExportSCs = ExportSC.GetsByBU(buid, ProductNature, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PIPaymentTypes = EnumObject.jGets(typeof(EnumPIPaymentType));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = Employee.Gets("SELECT * FROM View_Employee WHERE EmployeeDesignationType = " + (int)EnumEmployeeDesignationType.MarketPerson, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PIStatusObjs = EnumObject.jGets(typeof(EnumPIStatus));
            ViewBag.DepthOfShades = EnumObject.jGets(typeof(EnumDepthOfShade));
            ViewBag.BankBranchs = BankBranch.GetsOwnBranchs(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.ProductNature = ProductNature;//EnumProductNature{Dyeing = 0,Hanger = 1,Poly = 2,Cone = 3}
            return View(_oExportSCs);
        }

        public ActionResult ViewExportSC(int id)
        {
            _oExportSC = new ExportSC();
            List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            List<SampleInvoice> oSampleInvoices = new List<SampleInvoice>();
            if (id > 0)
            {
                _oExportSC = _oExportSC.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportSC.ExportSCDetails = ExportSCDetail.GetsByESCID(id, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oExportSC.ExportPIID > 0)
                {
                    string sSQL = "Select * from View_ExportPIDetail Where ExportPIID=" + _oExportSC.ExportPIID;
                    oExportPIDetails = ExportPIDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    sSQL = "Select * from View_DyeingOrder Where  ExportPIID = " + _oExportSC.ExportPIID + " And PaymentType in (" + ((int)EnumOrderPaymentType.AdjWithNextLC).ToString() + "," + ((int)EnumOrderPaymentType.AdjWithPI).ToString() + ")";
                    oDyeingOrders = DyeingOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                   // oSampleInvoices=SampleInvoice.Gets()
                }
            }
            ViewBag.ExportPIDetails = oExportPIDetails;
            ViewBag.PolyMeasurementTypes = EnumObject.jGets(typeof(EnumPolyMeasurementType));
            return View(_oExportSC);
        }

        [HttpPost]
        public JsonResult Save(ExportSC oExportSC)
        {
            _oExportSC = new ExportSC();
            try
            {
                _oExportSC = oExportSC.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportSC = new ExportSC();
                _oExportSC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportSC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Approved(ExportSC oExportSC)
        {
            _oExportSC = new ExportSC();
            try
            {
                _oExportSC = oExportSC.ApproveSalesContract(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportSC = new ExportSC();
                _oExportSC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportSC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateExportSC(ExportSC oExportSC)
        {
            _oExportSC = new ExportSC();
            try
            {
                _oExportSC = oExportSC.UpdateExportSC(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportSC = new ExportSC();
                _oExportSC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportSC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveLog(ExportSC oExportSC)
        {
            _oExportSC = new ExportSC();
            try
            {
                _oExportSC = oExportSC.AcceptRevise(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportSC = new ExportSC();
                _oExportSC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportSC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveExportSCDetail(ExportSCDetail oExportSCDetail)
        {
            _oExportSCDetail = new ExportSCDetail();
            try
            {
                _oExportSCDetail = oExportSCDetail.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportSCDetail = new ExportSCDetail();
                _oExportSCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportSCDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteExportSCDetail(ExportSCDetail oExportSCDetail)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportSCDetail.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult DeleteExportSC(ExportSC oExportSC)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportSC.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetsByPINoAndContractor(ExportSC oExportSC)
        {
            if (oExportSC.ContractorName == "") oExportSC.ContractorName = null;
            _oExportSCs = new List<ExportSC>();
            try
            {
                string sSQL = "SELECT * FROM View_ExportSC WHERE ContractorName Like '%" + oExportSC.ContractorName + "%' AND ContractorID = " + oExportSC.ContractorID;
                _oExportSCs = ExportSC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportSCs = new List<ExportSC>();
                _oExportSC = new ExportSC();
                _oExportSC.ErrorMessage = ex.Message;
                _oExportSCs.Add(_oExportSC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportSCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
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

        public Image GetCompanyTitle(Company oCompany)
        {
            if (oCompany.OrganizationTitle != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationTitle);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(this.ControllerContext.HttpContext.Server.MapPath("~/Content/") + "CompanyImageTitle.jpg", ImageFormat.Jpeg);
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
                MemoryStream m = new MemoryStream(oUserImage.ImageFile);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(this.ControllerContext.HttpContext.Server.MapPath("~/Content/") + "SignatureImage.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Search
        [HttpPost]
        public JsonResult AdvSearch(ExportSC oExportSC)
        {
            _oExportSCs = new List<ExportSC>();
            try
            {
                string sSQL = MakeSQL(oExportSC);
                _oExportSCs = ExportSC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportSCs = new List<ExportSC>();
                //_oExportSC.ErrorMessage = ex.Message;
                //_oExportSCs.Add(_oExportSC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportSCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL(ExportSC oExportSC)
        {
            string sParams = oExportSC.Note;


            int nCboIssueDate = 0;
            DateTime dFromIssueDate = DateTime.Today;
            DateTime dToIssueDate = DateTime.Today;
            int nCboValidityDate = 0;
            DateTime dFromValidityDate = DateTime.Today;
            DateTime dToValidityDate = DateTime.Today;
            int nCboLCDate = 0;
            DateTime dFromLCDate = DateTime.Today;
            DateTime dToLCDate = DateTime.Today;
            int nCboPIBank = 0;
            int nCboMkPerson = 0;
            string sCurrentStatus = "";
            int nPaymentType = 0;
            bool bExportLCIsntCreateYet = false;
            int nPIType = 0;
            bool bYetNotMakeFEO = false;
            string sTemp = "";
            string sConstruction = "";
            int nProcessType = 0;
            string sStyleNo = "";
            int nBUID = 0;
            int nProductNature = 0;

            int nCboStatusDate = 0;
            DateTime dFromStatusDate = DateTime.Today;
            DateTime dToStatusDate = DateTime.Today;

            if (!string.IsNullOrEmpty(sParams))
            {
                _oExportSC.ContractorName = Convert.ToString(sParams.Split('~')[0]);
                _oExportSC.BuyerName = Convert.ToString(sParams.Split('~')[1]);
                nCboIssueDate = Convert.ToInt32(sParams.Split('~')[2]);
                dFromIssueDate = Convert.ToDateTime(sParams.Split('~')[3]);
                dToIssueDate = Convert.ToDateTime(sParams.Split('~')[4]);
                nCboValidityDate = Convert.ToInt32(sParams.Split('~')[5]);
                dFromValidityDate = Convert.ToDateTime(sParams.Split('~')[6]);
                dToValidityDate = Convert.ToDateTime(sParams.Split('~')[7]);
                nCboLCDate = Convert.ToInt32(sParams.Split('~')[8]);
                dFromLCDate = Convert.ToDateTime(sParams.Split('~')[9]);
                dToLCDate = Convert.ToDateTime(sParams.Split('~')[10]);
                nCboPIBank = Convert.ToInt32(sParams.Split('~')[11]);
                nCboMkPerson = Convert.ToInt32(sParams.Split('~')[12]);
                sCurrentStatus = Convert.ToString(sParams.Split('~')[13]);
                nPaymentType = Convert.ToInt32(sParams.Split('~')[14]);

                nPIType = Convert.ToInt32(sParams.Split('~')[15]);
                sStyleNo = Convert.ToString(sParams.Split('~')[16]);

                nCboStatusDate = Convert.ToInt32(sParams.Split('~')[17]);
                sTemp = sParams.Split('~')[18];
                if (!String.IsNullOrEmpty(sTemp))
                {
                    dFromStatusDate = Convert.ToDateTime(sParams.Split('~')[18]);
                }
                sTemp = sParams.Split('~')[19];
                if (!String.IsNullOrEmpty(sTemp))
                {
                    dToStatusDate = Convert.ToDateTime(sParams.Split('~')[19]);
                }

                nBUID = Convert.ToInt32(sParams.Split('~')[20]);
                nProductNature = Convert.ToInt32(sParams.Split('~')[21]);
            }


            string sReturn1 = "SELECT * FROM View_ExportSC AS ESC ";
            string sReturn = "";

            #region Contractor
            if (!String.IsNullOrEmpty(_oExportSC.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ESC.ContractorID in(" + _oExportSC.ContractorName + ")";
            }
            #endregion

            #region Buyer Id
            if (!String.IsNullOrEmpty(_oExportSC.BuyerName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ESC.BuyerID in(" + _oExportSC.BuyerName + ")";
            }
            #endregion

            #region Issue Date
            if (nCboIssueDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboIssueDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ESC.IssueDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ESC.IssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ESC.IssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ESC.IssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ESC.IssueDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ESC.IssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region Validity Date
            if (nCboValidityDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboValidityDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ESC.ValidityDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromValidityDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboValidityDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ESC.ValidityDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromValidityDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboValidityDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ESC.ValidityDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromValidityDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboValidityDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ESC.ValidityDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromValidityDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboValidityDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ESC.ValidityDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromValidityDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToValidityDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboValidityDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ESC.ValidityDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromValidityDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToValidityDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region LC Date
            if (nCboLCDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);

                if (nCboLCDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + "ESC.ExportPIID in (Select ExportPIID from ExportPILCMapping  where Activity=1 and  CONVERT(DATE,CONVERT(VARCHAR(12),ExportPILCMapping.[Date],106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCDate.ToString("dd MMM yyyy") + "',106)) )";
                }
                else if (nCboLCDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ESC.LCOpenDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboLCDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ESC.LCOpenDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboLCDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ESC.LCOpenDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCDate.ToString("dd MMM yyyy") + "',106)) ";
                }

                else if (nCboLCDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + "ESC.ExportPIID in (Select ExportPIID from ExportPILCMapping  where Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),ExportPILCMapping.[Date],106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLCDate.ToString("dd MMM yyyy") + "',106))  )";
                }

                else if (nCboLCDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ESC.LCOpenDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLCDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region Status With Date
            if (nCboStatusDate != (int)EnumCompareOperator.None)
            {
                if (!String.IsNullOrEmpty(sCurrentStatus))
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboStatusDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " ESC.ExportPIID in (Select ExportPIID from ExportPIHistory where PIStatus in (" + sCurrentStatus + ") and CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromStatusDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    if (nCboStatusDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " ESC.ExportPIID in (Select ExportPIID from ExportPIHistory where PIStatus in (" + sCurrentStatus + ") and CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromStatusDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToStatusDate.ToString("dd MMM yyyy") + "',106)) ) ";
                    }
                }

            }
            #endregion
            #region PI Bank (BankBranchID)
            if (nCboPIBank > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ESC.BankBranchID = " + nCboPIBank;
            }
            #endregion

            #region nPayment Type
            if (nPaymentType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ESC.PaymentType = " + nPaymentType;
            }
            #endregion

            #region Mkt. Person
            if (nCboMkPerson > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ESC.MKTEmpID = " + nCboMkPerson;
            }
            #endregion

            #region Current Status
            if (!string.IsNullOrEmpty(sCurrentStatus))
            {
                if (nCboStatusDate == (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ESC.PIStatus IN (" + sCurrentStatus + ")";
                }
            }
            #endregion

            #region LC Pending
            if (bExportLCIsntCreateYet)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LCID=0 AND PIStatus>" + (int)EnumPIStatus.RequestForApproved + " ";
            }
            #endregion

            #region PI Type
            if (nPIType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ESC.PIType = " + nPIType;
            }
            #endregion

            #region Yet Not Make FEO
            if (bYetNotMakeFEO)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ESC.ExportPIID NOT IN (SELECT PIID FROM FabricExecutionOrder) ";
            }
            #endregion

            #region FEO No
            //if (!string.IsNullOrEmpty(sFEONo))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " ESC.ExportPIID IN (SELECT FEO.PIID FROM FabricExecutionOrder AS FEO WHERE FEO.FEONo LIKE '%" + sFEONo + "%') ";
            //}
            #endregion

            #region Construction
            if (!string.IsNullOrEmpty(sConstruction))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ESC.ExportPIID IN (SELECT EPD.ExportPIID FROM ExportPIDetail AS EPD WHERE EPD.FabricID IN (SELECT F.FabricID FROM Fabric AS F WHERE F.Construction LIKE '%" + sConstruction + "%')) ";
            }
            #endregion

            #region ProcessType
            if (nProcessType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ESC.ExportPIID IN (SELECT EPD.ExportPIID FROM ExportPIDetail AS EPD WHERE EPD.FabricID IN (SELECT F.FabricID FROM Fabric AS F WHERE F.ProcessType = " + nProcessType + ")) ";
            }
            #endregion

            #region Style No
            if (!string.IsNullOrEmpty(sStyleNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ESC.ExportPIID IN (SELECT EPD.ExportPIID FROM ExportPIDetail AS EPD WHERE EPD.StyleNo LIKE '%" + sStyleNo + "%') ";
            }
            #endregion

            #region Business Unit
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ESC.BUID = " + nBUID;
            }
            #endregion
            #region Product Nature
            if (nProductNature > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ESC.ProductNature,0) = " + nProductNature;
            }
            #endregion
            
            string sSQL = sReturn1 + " " + sReturn + " Order By ESC.IssueDate DESC ";
            return sSQL;
        }
        [HttpPost]
        public JsonResult GetbyNo(ExportSC oExportSC)
        {
           
            _oExportSCs = new List<ExportSC>();
            string sSQL = "SELECT * FROM View_ExportSC";
            string sReturn = "";
            if (!String.IsNullOrEmpty(oExportSC.PINo))
            {
                oExportSC.PINo = oExportSC.PINo.Trim();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "PINo like '%" + oExportSC.PINo + "%'";
            }
            #region BU
            if(oExportSC.BUID!=0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID =" + oExportSC.BUID;
            }
            #endregion
            #region BU
            if (oExportSC.ProductNatureInInt != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ProductNature,0) =" + oExportSC.ProductNatureInInt;
            }
            #endregion
            sSQL = sSQL + " " + sReturn;

            _oExportSCs = ExportSC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportSCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export SC Detail
        [HttpPost]
        public JsonResult GetsSCDetails(ExportSC oExportSC)
        {
            _oExportSCDetails = new List<ExportSCDetail>();
            try
            {
                if (oExportSC.ExportSCID > 0)
                {
                    _oExportSCDetails = ExportSCDetail.GetsByESCID(oExportSC.ExportSCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oExportSCDetail = new ExportSCDetail();
                _oExportSCDetail.ErrorMessage = ex.Message;
                _oExportSCDetails.Add(_oExportSCDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportSCDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchByExportSCBUWise(ExportSC oExportSC)
        {
            _oExportSCs = new List<ExportSC>();
            try
            {
                string sSQL = "SELECT * FROM View_ExportSC WHERE BUID=" + oExportSC.BUID + " AND ProductNature = " + oExportSC.ProductNatureInInt;
                if (oExportSC.PINo != null && oExportSC.PINo != "")
                {
                    sSQL += " And PINo Like '%" + oExportSC.PINo.Trim() + "%'";
                }
                if (oExportSC.ContractorID > 0)
                {
                    sSQL += " And ContractorID = " + oExportSC.ContractorID + "";
                }
                _oExportSCs = ExportSC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportSCs = new List<ExportSC>();
                _oExportSC = new ExportSC();
                _oExportSC.ErrorMessage = ex.Message;
                _oExportSCs.Add(_oExportSC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportSCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PrintSalesContractPriview
        public ActionResult PrintSalesContractPriview(int nExportSCID)
        {
            _oExportSC = new ExportSC();
            _oExportSC = _oExportSC.Get(nExportSCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportSC.ExportSCDetails = ExportSCDetail.GetsByESCID(nExportSCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportSC.CurrentUserId = ((User)Session[SessionInfo.CurrentUser]).UserID;
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oCompany.CompanyTitle = GetCompanyTitle(oCompany);
            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(_oExportSC.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (!String.IsNullOrEmpty(oContractor.Address3))
            {
                _oExportSC.ContractorAddress = oContractor.Address;
            }
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oExportSC.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptExportSC_Common oReport = new rptExportSC_Common();//for plastic and integrated
            byte[] abytes = oReport.PrepareReport(_oExportSC, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
            
        }
        #endregion
    }
}
