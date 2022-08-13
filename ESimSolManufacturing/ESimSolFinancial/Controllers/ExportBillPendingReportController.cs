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
using ESimSol.BusinessObjects.ReportingObject;
using System.Xml.Serialization;


namespace ESimSolFinancial.Controllers
{
    public class ExportBillPendingReportController : Controller
    {
        #region Declaration
        List<ExportBillPendingReport> _oExportBillPendingReports = new List<ExportBillPendingReport>();
        ExportBillPendingReport _oExportBillPendingReport = new ExportBillPendingReport();
        string _sErrorMessage = "";
        #endregion

        #region View Report
        public ActionResult View_ExportBillPendingReports(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<ExportBillPendingReport> oExportBillPendingReports = new List<ExportBillPendingReport>();
          //  oExportBillPendingReports = ExportBillPendingReport.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            return View(oExportBillPendingReports);
        }
        #region HTTPGet

        [HttpPost]
        public JsonResult GetsReport(ExportBillPendingReport oExportBillPendingReport)
        {
            List<ExportBillPendingReport> oExportBillPendingReports = new List<ExportBillPendingReport>();
            try
            {
                oExportBillPendingReports = ExportBillPendingReport.Gets(oExportBillPendingReport.ReportType, oExportBillPendingReport.DiscountType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportBillPendingReports = new List<ExportBillPendingReport>();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportBillPendingReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);

            //var jsonResult = Json(oExportBillPendingReports, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;
        }
        [HttpPost]
        public JsonResult GetsSearchedData_EBillMReport(ExportBillPendingReport oExportBillPendingReport)
        {
            List<ExportBillPendingReport> oExportBillPendingReports = new List<ExportBillPendingReport>();
            try
            {
                string sSQL = GetSQL_EBMReport(oExportBillPendingReport);
                oExportBillPendingReports = ExportBillPendingReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportBillPendingReports = new List<ExportBillPendingReport>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportBillPendingReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL_EBMReport(ExportBillPendingReport oExportBillPendingReport)
        {
            string sReturn1 = "SELECT * FROM [View_ExportBillMaturityReport] ";
            string sReturn = "";
            #region String BankName
            if (oExportBillPendingReport.ReportType ==1) // Pending Maturity
            {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " [State]=4";
            }
            if (oExportBillPendingReport.ReportType == 2)// Pending Overdue Payment 
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " [State] in (5,7,12) and Convert(Date,MaturityDate) <'" + DateTime.Today.ToString("dd MMM yyyy");
            }
            if (oExportBillPendingReport.ReportType == 3) // Total Due Payment
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " [State] in (5,7,12)" + DateTime.Today.ToString("dd MMM yyyy");
            }

            if (oExportBillPendingReport.BankName_Nego != null)
            {
                if (oExportBillPendingReport.BankName_Nego != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BankName_Nego like '%" + oExportBillPendingReport.BankName_Nego + "%'";
                }
            }

            if (oExportBillPendingReport.ApplicantName != null)
            {
                if (oExportBillPendingReport.ApplicantName != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ApplicantID IN (" + oExportBillPendingReport.ApplicantName + ")";
                }
            }
            if (oExportBillPendingReport.MKTPName != null)
            {
                if (oExportBillPendingReport.MKTPName != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MKTPName like '%" + oExportBillPendingReport.MKTPName + "%'";
                }
            }
            if (oExportBillPendingReport.SelectedOption != null)
            {
                if (oExportBillPendingReport.SelectedOption != "")
                {
                    if (oExportBillPendingReport.SelectedOption != EnumCompareOperator.None.ToString())
                    {

                        if (oExportBillPendingReport.SelectedOption == EnumCompareOperator.Between.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + " MaturityDate >= '" + oExportBillPendingReport.MaturityDate.ToString("dd MMM yyyy") + "' AND MaturityDate < '" + oExportBillPendingReport.DateofMaturityEnd.AddDays(1).ToString("dd MMM yyyy") + "'";

                        }
                        else if (oExportBillPendingReport.SelectedOption == EnumCompareOperator.NotBetween.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + " MaturityDate < '" + oExportBillPendingReport.MaturityDate.ToString("dd MMM yyyy") + "' OR MaturityDate > '" + oExportBillPendingReport.DateofMaturityEnd.AddDays(1).ToString("dd MMM yyyy") + "'";

                        }
                        else if (oExportBillPendingReport.SelectedOption == EnumCompareOperator.EqualTo.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + " MaturityDate = '" + oExportBillPendingReport.MaturityDate.ToString("dd MMM yyyy") + "'";
                        }

                        else if (oExportBillPendingReport.SelectedOption == EnumCompareOperator.NotEqualTo.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + "MaturityDate != '" + oExportBillPendingReport.MaturityDate.ToString("dd MMM yyyy") + "'";
                        }
                        else if (oExportBillPendingReport.SelectedOption == EnumCompareOperator.GreaterThan.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + "MaturityDate > '" + oExportBillPendingReport.MaturityDate.ToString("dd MMM yyyy") + "'";
                        }
                        else if (oExportBillPendingReport.SelectedOption == EnumCompareOperator.SmallerThan.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + "MaturityDate < '" + oExportBillPendingReport.MaturityDate.ToString("dd MMM yyyy") + "'";
                        }

                    }
                }
            }
            #endregion
            sReturn = sReturn1 + sReturn + "  order by BankBranchID_Negotiation";
            return sReturn;
        }

            

        #endregion
        //public ActionResult Print_Report(string sTempString)
        //{
        //    List<ExportBillPendingReport> oExportBillPendingReports = new List<ExportBillPendingReport>();

        //    //_oExportBillPendingReports = new List<ExportBillPendingReport>();
        //    ExportBillPendingReport oExportBillPendingReport = new ExportBillPendingReport();

        //    oExportBillPendingReport.SearchType = Convert.ToInt32(sTempString.Split('~')[0]);
        //    oExportBillPendingReport.SelectedOption = sTempString.Split('~')[1];
        //    oExportBillPendingReport.MaturityDate = Convert.ToDateTime(sTempString.Split('~')[2]);
        //    oExportBillPendingReport.DateofMaturityEnd = Convert.ToDateTime(sTempString.Split('~')[3]);
        //    oExportBillPendingReport.BankName_Nego = sTempString.Split('~')[4];
        //    oExportBillPendingReport.ApplicantName = sTempString.Split('~')[5];
        //    oExportBillPendingReport.MKTPName = sTempString.Split('~')[6];


        //    try
        //    {
        //        string sSQL = GetSQL_EBMReport(oExportBillPendingReport);
        //        oExportBillPendingReports = ExportBillPendingReport.Gets(sSQL, (Guid)Session[SessionInfo.wcfSessionID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oExportBillPendingReports = new List<ExportBillPendingReport>();
        //    }


        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (Guid)Session[SessionInfo.wcfSessionID]);
        //    rptExportBillPendingReport oReport = new rptExportBillPendingReport();
        //    byte[] abytes = oReport.PrepareReport(oExportBillPendingReports, oCompany);
        //    return File(abytes, "application/pdf");
        //}
        #endregion

        #region Bank Wise LC In Hand
        public ActionResult PrintBankWise(int nReportType, int nDiscountType, double nts)
        {
            _oExportBillPendingReport = new ExportBillPendingReport();

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oExportBillPendingReport.Company = oCompany;
            _oExportBillPendingReport.ExportBillPendingReports = ExportBillPendingReport.Gets(nReportType, nDiscountType, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptBankWiseExportBillPendingReports oReport = new rptBankWiseExportBillPendingReports();
            byte[] abytes = oReport.PrepareReport(_oExportBillPendingReport, nReportType);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region Party Wise LC In Hand
        public ActionResult PrintPartyWise(int nReportType, int nDiscountType, double nts)
        {
            _oExportBillPendingReport = new ExportBillPendingReport();

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oExportBillPendingReport.Company = oCompany;
            _oExportBillPendingReport.ExportBillPendingReports = ExportBillPendingReport.Gets(nReportType, nDiscountType, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptPartyWiseExportBillPendingReports oReport = new rptPartyWiseExportBillPendingReports();
            byte[] abytes = oReport.PrepareReport(_oExportBillPendingReport, nReportType);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region PrintXL
        public ActionResult PrintXL(int nReportType, int nDiscountType, double nts)
        {
            string sXLName = "";
            int nSL = 0;

            _oExportBillPendingReports = new List<ExportBillPendingReport>();
            _oExportBillPendingReports = ExportBillPendingReport.Gets(nReportType, nDiscountType, ((User)Session[SessionInfo.CurrentUser]).UserID);

            var stream = new MemoryStream();

            #region LC In Hand
            if (nReportType == 1) //LC In Hand
            {
                var serializerLCInHand = new XmlSerializer(typeof(List<EBPRLcInHandXL>));
                EBPRLcInHandXL oEBPRLcInHandXL = new EBPRLcInHandXL();
                List<EBPRLcInHandXL> oEBPRLcInHandXLs = new List<EBPRLcInHandXL>();
                
                double nTotalLCValue = 0;
                double nTotalBillValue = 0;
                double nTotalLCInHandValue = 0;
                double nDeliveryValue = 0;

                foreach (ExportBillPendingReport oItem in _oExportBillPendingReports)
                {
                    nSL++;
                    oEBPRLcInHandXL = new EBPRLcInHandXL();
                    oEBPRLcInHandXL.SLNo = nSL.ToString();
                    oEBPRLcInHandXL.ApplicantName = oItem.ApplicantName;
                    oEBPRLcInHandXL.BankName_Nego = oItem.BankName_Nego;
                    oEBPRLcInHandXL.ExportLCNo = oItem.ExportLCNo;
                    oEBPRLcInHandXL.Amount_LCSt = oItem.Amount_LCSt;
                    oEBPRLcInHandXL.AmountSt = oItem.AmountSt;
                    oEBPRLcInHandXL.LCinHandSt = oItem.LCinHandSt;
                    oEBPRLcInHandXL.DeliveryValueSt = oItem.DeliveryValueSt;
                    oEBPRLcInHandXL.LCStatusSt = oItem.LCStatusSt;
                    oEBPRLcInHandXLs.Add(oEBPRLcInHandXL);


                    nTotalLCValue += oItem.Amount_LC;
                    nTotalBillValue += oItem.Amount;
                    nTotalLCInHandValue = nTotalLCInHandValue + (oItem.Amount_LC - oItem.Amount);
                    nDeliveryValue += oItem.DeliveryValue;
                }

                #region Total
                oEBPRLcInHandXL = new EBPRLcInHandXL();
                oEBPRLcInHandXL.ExportLCNo = "Total : ";
                oEBPRLcInHandXL.Amount_LCSt = Global.MillionFormat(nTotalLCValue);
                oEBPRLcInHandXL.AmountSt = Global.MillionFormat(nTotalBillValue);
                oEBPRLcInHandXL.LCinHandSt = Global.MillionFormat(nTotalLCInHandValue);
                oEBPRLcInHandXL.DeliveryValueSt = Global.MillionFormat(nDeliveryValue);
                oEBPRLcInHandXLs.Add(oEBPRLcInHandXL);
                #endregion

                serializerLCInHand.Serialize(stream, oEBPRLcInHandXLs);
                stream.Position = 0;
                return File(stream, "application/vnd.ms-excel", "LC In Hand.xls");

            }
            #endregion

            #region Pending Party Acceptance

            else if (nReportType == 3) //Pending Party Acceptance
            {
                var serializerPendingPartyAcceptance = new XmlSerializer(typeof(List<EBPRPendingPartyAcceptanceXL>));
                EBPRPendingPartyAcceptanceXL oEBPRPendingPartyAcceptanceXL = new EBPRPendingPartyAcceptanceXL();
                List<EBPRPendingPartyAcceptanceXL> oEBPRPendingPartyAcceptanceXLs = new List<EBPRPendingPartyAcceptanceXL>();
                
                double nTotalValue = 0;

                foreach (ExportBillPendingReport oItem in _oExportBillPendingReports)
                {
                    nSL++;
                    oEBPRPendingPartyAcceptanceXL = new EBPRPendingPartyAcceptanceXL();
                    oEBPRPendingPartyAcceptanceXL.SLNo = nSL.ToString();
                    oEBPRPendingPartyAcceptanceXL.ExportBillNo = oItem.ExportBillNo;
                    oEBPRPendingPartyAcceptanceXL.ApplicantName = oItem.ApplicantName;
                    oEBPRPendingPartyAcceptanceXL.ExportLCNo = oItem.ExportLCNo;
                    oEBPRPendingPartyAcceptanceXL.PINo = oItem.PINo;
                    oEBPRPendingPartyAcceptanceXL.BankName_Nego = oItem.BankName_Nego;
                    oEBPRPendingPartyAcceptanceXL.AmountSt = oItem.AmountSt;
                    oEBPRPendingPartyAcceptanceXL.MKTPName = oItem.MKTPName;
                    oEBPRPendingPartyAcceptanceXL.SendToPartySt = oItem.SendToPartySt;
                    oEBPRPendingPartyAcceptanceXL.DueDays = oItem.TimeLag3.ToString();
                    oEBPRPendingPartyAcceptanceXL.DeliveryDateSt = oItem.DeliveryDateSt;
                    oEBPRPendingPartyAcceptanceXL.DueDate = "";
                    oEBPRPendingPartyAcceptanceXLs.Add(oEBPRPendingPartyAcceptanceXL);

                    nTotalValue += oItem.Amount;
                }

                #region Total
                oEBPRPendingPartyAcceptanceXL = new EBPRPendingPartyAcceptanceXL();
                oEBPRPendingPartyAcceptanceXL.BankName_Nego = "Total : ";
                oEBPRPendingPartyAcceptanceXL.AmountSt = Global.MillionFormat(nTotalValue);
                oEBPRPendingPartyAcceptanceXLs.Add(oEBPRPendingPartyAcceptanceXL);
                #endregion

                serializerPendingPartyAcceptance.Serialize(stream, oEBPRPendingPartyAcceptanceXLs);
                stream.Position = 0;
                return File(stream, "application/vnd.ms-excel", "Pending Party Acceptance.xls");

            }
            #endregion

            #region Pending Submit To Bank

            else if (nReportType == 4) //Pending Submit To Bank
            {
                var serializerPendingPartyAcceptance = new XmlSerializer(typeof(List<EBPRPendingSubmitToBankXL>));
                EBPRPendingSubmitToBankXL oEBPRPendingSubmitToBankXL = new EBPRPendingSubmitToBankXL();
                List<EBPRPendingSubmitToBankXL> oEBPRPendingSubmitToBankXLs = new List<EBPRPendingSubmitToBankXL>();

                double nTotalValue = 0;

                foreach (ExportBillPendingReport oItem in _oExportBillPendingReports)
                {
                    nSL++;
                    oEBPRPendingSubmitToBankXL = new EBPRPendingSubmitToBankXL();
                    oEBPRPendingSubmitToBankXL.SLNo = nSL.ToString();
                    oEBPRPendingSubmitToBankXL.ExportBillNo = oItem.ExportBillNo;
                    oEBPRPendingSubmitToBankXL.ApplicantName = oItem.ApplicantName;
                    oEBPRPendingSubmitToBankXL.ExportLCNo = oItem.ExportLCNo;
                    oEBPRPendingSubmitToBankXL.PINo = oItem.PINo;
                    oEBPRPendingSubmitToBankXL.BankName_Nego = oItem.BankName_Nego;
                    oEBPRPendingSubmitToBankXL.AmountSt = oItem.AmountSt;
                    oEBPRPendingSubmitToBankXL.BankName_Issue = oItem.BankName_Issue;
                    oEBPRPendingSubmitToBankXL.PrepareDate = "";
                    oEBPRPendingSubmitToBankXL.SendToPartySt = oItem.SendToPartySt;
                    oEBPRPendingSubmitToBankXL.ReceiveFromParty = "";
                    oEBPRPendingSubmitToBankXL.DueDays = oItem.TimeLag3.ToString();
                    oEBPRPendingSubmitToBankXLs.Add(oEBPRPendingSubmitToBankXL);

                    nTotalValue += oItem.Amount;
                }

                #region Total
                oEBPRPendingSubmitToBankXL = new EBPRPendingSubmitToBankXL();
                oEBPRPendingSubmitToBankXL.BankName_Nego = "Total : ";
                oEBPRPendingSubmitToBankXL.AmountSt = Global.MillionFormat(nTotalValue);
                oEBPRPendingSubmitToBankXLs.Add(oEBPRPendingSubmitToBankXL);
                #endregion

                serializerPendingPartyAcceptance.Serialize(stream, oEBPRPendingSubmitToBankXLs);
                stream.Position = 0;
                return File(stream, "application/vnd.ms-excel", "Pending Submit To Bank.xls");

            }
            #endregion

            #region Pending LDBC

            else if (nReportType == 5) //Pending LDBC
            {
                var serializerPendingPartyAcceptance = new XmlSerializer(typeof(List<EBPRPendingLDBCXL>));
                EBPRPendingLDBCXL oEBPRPendingLDBCXL = new EBPRPendingLDBCXL();
                List<EBPRPendingLDBCXL> oEBPRPendingLDBCXLs = new List<EBPRPendingLDBCXL>();

                double nTotalValue = 0;

                foreach (ExportBillPendingReport oItem in _oExportBillPendingReports)
                {
                    nSL++;
                    oEBPRPendingLDBCXL = new EBPRPendingLDBCXL();
                    oEBPRPendingLDBCXL.SLNo = nSL.ToString();
                    oEBPRPendingLDBCXL.ExportBillNo = oItem.ExportBillNo;
                    oEBPRPendingLDBCXL.ApplicantName = oItem.ApplicantName;
                    oEBPRPendingLDBCXL.ExportLCNo = oItem.ExportLCNo;
                    oEBPRPendingLDBCXL.PINo = oItem.PINo;
                    oEBPRPendingLDBCXL.BankName_Nego = oItem.BankName_Nego;
                    oEBPRPendingLDBCXL.AmountSt = oItem.AmountSt;
                    oEBPRPendingLDBCXL.BankName_Issue = oItem.BankName_Issue;
                    oEBPRPendingLDBCXL.PrepareDate = "";
                    oEBPRPendingLDBCXL.SendToPartySt = oItem.SendToPartySt;
                    oEBPRPendingLDBCXL.ReceiveFromParty = "";
                    oEBPRPendingLDBCXL.SendToBankDateSt = oItem.SendToBankDateSt;
                    oEBPRPendingLDBCXL.DueDays = oItem.TimeLag3.ToString();
                    oEBPRPendingLDBCXLs.Add(oEBPRPendingLDBCXL);

                    nTotalValue += oItem.Amount;
                }
                #region Total
                oEBPRPendingLDBCXL = new EBPRPendingLDBCXL();
                oEBPRPendingLDBCXL.BankName_Nego = "Total : ";
                oEBPRPendingLDBCXL.AmountSt = Global.MillionFormat(nTotalValue);
                oEBPRPendingLDBCXLs.Add(oEBPRPendingLDBCXL);
                #endregion

                serializerPendingPartyAcceptance.Serialize(stream, oEBPRPendingLDBCXLs);
                stream.Position = 0;
                return File(stream, "application/vnd.ms-excel", "Pending LDBC.xls");
            }
            #endregion

            #region Pending Maturity

            else if (nReportType == 6) //Pending Maturity
            {
                var serializerPendingPartyAcceptance = new XmlSerializer(typeof(List<EBPRPendingMaturityXL>));
                EBPRPendingMaturityXL oEBPRPendingMaturityXL = new EBPRPendingMaturityXL();
                List<EBPRPendingMaturityXL> oEBPRPendingMaturityXLs = new List<EBPRPendingMaturityXL>();

                double nTotalValue = 0;

                foreach (ExportBillPendingReport oItem in _oExportBillPendingReports)
                {
                    nSL++;
                    oEBPRPendingMaturityXL = new EBPRPendingMaturityXL();
                    oEBPRPendingMaturityXL.SLNo = nSL.ToString();
                    oEBPRPendingMaturityXL.ExportBillNo = oItem.ExportBillNo;
                    oEBPRPendingMaturityXL.ApplicantName = oItem.ApplicantName;
                    oEBPRPendingMaturityXL.ExportLCNo = oItem.ExportLCNo;
                    oEBPRPendingMaturityXL.PINo = oItem.PINo;
                    oEBPRPendingMaturityXL.BankName_Nego = oItem.BankName_Nego;
                    oEBPRPendingMaturityXL.AmountSt = oItem.AmountSt;
                    oEBPRPendingMaturityXL.BankName_Issue = oItem.BankName_Issue;
                    oEBPRPendingMaturityXL.LDBCNo = oItem.LDBCNo;
                    oEBPRPendingMaturityXL.LDBCDateSt = oItem.LDBCDateSt;
                    oEBPRPendingMaturityXL.MaturityRecDate = "";
                    oEBPRPendingMaturityXL.MaturityDate = "";
                    oEBPRPendingMaturityXL.ORate = "";
                    oEBPRPendingMaturityXL.ODue = "";
                    oEBPRPendingMaturityXL.LastDeliveryDate = oItem.DeliveryDateSt;
                    oEBPRPendingMaturityXL.DueDays = oItem.TimeLag3.ToString();
                    oEBPRPendingMaturityXLs.Add(oEBPRPendingMaturityXL);

                    nTotalValue += oItem.Amount;
                }
                #region Total
                oEBPRPendingMaturityXL = new EBPRPendingMaturityXL();
                oEBPRPendingMaturityXL.BankName_Nego = "Total : ";
                oEBPRPendingMaturityXL.AmountSt = Global.MillionFormat(nTotalValue);
                oEBPRPendingMaturityXLs.Add(oEBPRPendingMaturityXL);
                #endregion

                serializerPendingPartyAcceptance.Serialize(stream, oEBPRPendingMaturityXLs);
                stream.Position = 0;
                return File(stream, "application/vnd.ms-excel", "Pending Maturity.xls");
            }
            #endregion

            #region Over due Maturity

            else if (nReportType == 7) //Over due Maturity
            {
                var serializerPendingPartyAcceptance = new XmlSerializer(typeof(List<EBPROverdueMaturityXL>));
                EBPROverdueMaturityXL oEBPROverdueMaturityXL = new EBPROverdueMaturityXL();
                List<EBPROverdueMaturityXL> oEBPROverdueMaturityXLs = new List<EBPROverdueMaturityXL>();

                double nTotalValue = 0;

                foreach (ExportBillPendingReport oItem in _oExportBillPendingReports)
                {
                    nSL++;
                    oEBPROverdueMaturityXL = new EBPROverdueMaturityXL();
                    oEBPROverdueMaturityXL.SLNo = nSL.ToString();
                    oEBPROverdueMaturityXL.ExportBillNo = oItem.ExportBillNo;
                    oEBPROverdueMaturityXL.ApplicantName = oItem.ApplicantName;
                    oEBPROverdueMaturityXL.ExportLCNo = oItem.ExportLCNo;
                    oEBPROverdueMaturityXL.PINo = oItem.PINo;
                    oEBPROverdueMaturityXL.BankName_Nego = oItem.BankName_Nego;
                    oEBPROverdueMaturityXL.AmountSt = oItem.AmountSt;
                    oEBPROverdueMaturityXL.BankName_Issue = oItem.BankName_Issue;
                    oEBPROverdueMaturityXL.Status = "";
                    oEBPROverdueMaturityXL.LDBCNo = oItem.LDBCNo;
                    oEBPROverdueMaturityXL.LDBCDateSt = oItem.LDBCDateSt;
                    oEBPROverdueMaturityXL.MaturityRecDate = "";
                    oEBPROverdueMaturityXL.MaturityDate = "";
                    oEBPROverdueMaturityXL.ORate = "";
                    oEBPROverdueMaturityXL.ODue = "";
                    oEBPROverdueMaturityXL.LastDeliveryDate = oItem.DeliveryDateSt;
                    oEBPROverdueMaturityXL.DueDays = oItem.TimeLag3.ToString();
                    oEBPROverdueMaturityXLs.Add(oEBPROverdueMaturityXL);

                    nTotalValue += oItem.Amount;
                }
                #region Total
                oEBPROverdueMaturityXL = new EBPROverdueMaturityXL();
                oEBPROverdueMaturityXL.BankName_Nego = "Total : ";
                oEBPROverdueMaturityXL.AmountSt = Global.MillionFormat(nTotalValue);
                oEBPROverdueMaturityXLs.Add(oEBPROverdueMaturityXL);
                #endregion

                serializerPendingPartyAcceptance.Serialize(stream, oEBPROverdueMaturityXLs);
                stream.Position = 0;
                return File(stream, "application/vnd.ms-excel", "Over due Maturity.xls");
            }
            #endregion

            #region Pending Payment
            else //if(nReportType == 8) //Pending Payment
            {
                var serializerPendingPartyAcceptance = new XmlSerializer(typeof(List<EBPRPendingPaymentXL>));
                EBPRPendingPaymentXL oEBPRPendingPaymentXL = new EBPRPendingPaymentXL();
                List<EBPRPendingPaymentXL> oEBPRPendingPaymentXLs = new List<EBPRPendingPaymentXL>();

                double nTotalValue = 0;

                foreach (ExportBillPendingReport oItem in _oExportBillPendingReports)
                {
                    nSL++;
                    oEBPRPendingPaymentXL = new EBPRPendingPaymentXL();
                    oEBPRPendingPaymentXL.SLNo = nSL.ToString();
                    oEBPRPendingPaymentXL.ExportBillNo = oItem.ExportBillNo;
                    oEBPRPendingPaymentXL.ApplicantName = oItem.ApplicantName;
                    oEBPRPendingPaymentXL.ExportLCNo = oItem.ExportLCNo;
                    oEBPRPendingPaymentXL.PINo = oItem.PINo;
                    oEBPRPendingPaymentXL.BankName_Nego = oItem.BankName_Nego;
                    oEBPRPendingPaymentXL.AmountSt = oItem.AmountSt;
                    oEBPRPendingPaymentXL.BankName_Issue = oItem.BankName_Issue;
                    oEBPRPendingPaymentXL.Status = "";
                    oEBPRPendingPaymentXL.LDBCNo = oItem.LDBCNo;
                    oEBPRPendingPaymentXL.LDBCDateSt = oItem.LDBCDateSt;
                    oEBPRPendingPaymentXL.MaturityRecDate = "";
                    oEBPRPendingPaymentXL.MaturityDate = "";
                    oEBPRPendingPaymentXL.ORate = "";
                    oEBPRPendingPaymentXL.ODue = "";
                    oEBPRPendingPaymentXL.LastDeliveryDate = oItem.DeliveryDateSt;
                    oEBPRPendingPaymentXL.DueDays = oItem.TimeLag3.ToString();
                    oEBPRPendingPaymentXLs.Add(oEBPRPendingPaymentXL);

                    nTotalValue += oItem.Amount;
                }
                #region Total
                oEBPRPendingPaymentXL = new EBPRPendingPaymentXL();
                oEBPRPendingPaymentXL.BankName_Nego = "Total : ";
                oEBPRPendingPaymentXL.AmountSt = Global.MillionFormat(nTotalValue);
                oEBPRPendingPaymentXLs.Add(oEBPRPendingPaymentXL);
                #endregion

                serializerPendingPartyAcceptance.Serialize(stream, oEBPRPendingPaymentXLs);
                stream.Position = 0;
                return File(stream, "application/vnd.ms-excel", "Pending Payment.xls");
            }
            #endregion

        }
        #endregion
        
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



        public ActionResult ViewExportBillPending(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<ExportBillPending> oExportBillPendings = new List<ExportBillPending>();
            oExportBillPendings = ExportBillPending.Gets("", EnumReportLayout.LCWise, (int)Session[SessionInfo.currentUserID]);
            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.PIWise || (EnumReportLayout)oItem.id == EnumReportLayout.ProductWise || (EnumReportLayout)oItem.id == EnumReportLayout.LCWise )
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion
            ViewBag.ReportLayouts = oReportLayouts;
            return View(oExportBillPendings);
        }

        //
        [HttpPost]
        public JsonResult SearchExportBillPending(ExportBillPending oExportBillPending)
        {
            List<ExportBillPending> oExportBillPendings = new List<ExportBillPending>();
            try
            {
                oExportBillPendings = ExportBillPending.Gets("", (EnumReportLayout)oExportBillPending.ReportLayoutInt, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oExportBillPending = new ExportBillPending();
                oExportBillPending.ErrorMessage = ex.Message;
                oExportBillPendings.Add(oExportBillPending);
            }
            var jsonResult = Json(oExportBillPendings, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult PrintExportBill(int nLayout, double nts)
        {
            _oExportBillPendingReport = new ExportBillPendingReport();

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oExportBillPendingReport.Company = oCompany;
            _oExportBillPendingReport.ExportBillPendings = ExportBillPending.Gets("", (EnumReportLayout)nLayout, (int)Session[SessionInfo.currentUserID]);

            rptExportBillReports oReport = new rptExportBillReports();
            byte[] abytes = oReport.PrepareReport(_oExportBillPendingReport, nLayout);
            return File(abytes, "application/pdf");
        }
    }
}
