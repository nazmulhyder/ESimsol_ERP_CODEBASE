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
using System.Security;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class SalesStatementController : PdfViewController
    {
        string _sErrorMesage = ""; int _nReportLayout;
        ExportBillReport _oExportBillReport = new ExportBillReport();
        List<ExportBillReport> _oExportBillReports = new List<ExportBillReport>();
        List<ExportLCRegister> _oExportLCRegisters = new List<ExportLCRegister>();
        public ActionResult View_SalesStatements(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<SalesStatement> oSalesStatements = new List<SalesStatement>();

            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            ViewBag.Buid = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));

            return View(oSalesStatements);
        }
        public ActionResult View_SalesStatementSummary(int buid, string sParam)
        {
            SalesStatement oSalesStatement = new SalesStatement();
            List<SalesStatement> oSalesStatements = new List<SalesStatement>();
            try
            {
                string sSQL = MakeSQL_Summary(buid, sParam);
                oSalesStatements = SalesStatement.Gets(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception e)
            {
                
            }
            ViewBag.Buid = buid;
            return View(oSalesStatements);
        }
        
        #region Report
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
        public ActionResult PrintSalesStatementSummery(String sTemp)
        {
            List<SalesStatement> oSalesStatements = new List<SalesStatement>();
            List<SalesStatement> oSalesStatements_ExportBill = new List<SalesStatement>();

            int nDateCompare = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dtStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dtEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            int BUID = Convert.ToInt32(sTemp.Split('~')[3]);

            if (string.IsNullOrEmpty(sTemp))
            {
                throw new Exception("Nothing  to Print");
            }
            else
            {

                //oSalesStatements = SalesStatement.GetsSalesStatement(dtStartDate, dtEndDate.AddDays(1),((User)Session[SessionInfo.CurrentUser]).UserID);

                if (nDateCompare > 0)
                {
                    if (nDateCompare == 1)
                    {
                        oSalesStatements = SalesStatement.GetsSalesStatement(BUID, dtStartDate, dtStartDate, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }

                    if (nDateCompare == 5)
                    {
                        oSalesStatements = SalesStatement.GetsSalesStatement(BUID, dtStartDate, dtEndDate.AddDays(1), ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }

            }

            oSalesStatements_ExportBill = SalesStatement.Gets_Summary(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptSalesStatement oReport = new rptSalesStatement();
            byte[] abytes = oReport.PrepareReport(oSalesStatements_ExportBill, oSalesStatements, oCompany, sTemp, oBusinessUnit);
            return File(abytes, "application/pdf");


        }
        #endregion

        #region Statement Details
        private string MakeSQL_Summary(int buid, string SearchStr)
        {
            DateTime dStratDate = DateTime.Today;
            DateTime dEndDate = DateTime.Today;
            int nPartID = 0;
            int nDateCompare = 0;
            int nDivisionID = 0;
            int nRefID = 0;
            if (!string.IsNullOrEmpty(SearchStr))
            {
                nDateCompare = Convert.ToInt32(SearchStr.Split('~')[0]); 
                dStratDate = Convert.ToDateTime(SearchStr.Split('~')[1]);
                dEndDate = Convert.ToDateTime(SearchStr.Split('~')[2]);
                nPartID = Convert.ToInt32(SearchStr.Split('~')[3]);
                nDivisionID = Convert.ToInt32(SearchStr.Split('~')[4]);
                nRefID = Convert.ToInt32(SearchStr.Split('~')[5]);
            }
            string sReturn1 = "";
          
            if (nPartID == 1)
            {
                switch (nDivisionID)
                {
                    case 1: sReturn1 = "  SELECT DD.ID, DD.Name, Count(*) AS [Count], Sum(DD.Qty) AS Qty, SUM(DD.Amount) AS Amount FROM ( SELECT DyeingOrder.ContractorID AS ID, DyeingOrder.ContractorName AS Name, DyeingOrder.Qty, DyeingOrder.Amount FROM View_DyeingOrder AS DyeingOrder "
                                     + "  WHERE DyeingOrder.[Status] NOT IN (9) " + GetDateSearch(nDateCompare, "DyeingOrder.OrderDate", dStratDate, dEndDate) + " ) AS DD ";
                            break;
                    case 2: sReturn1 = "  SELECT DD.ID, DD.Name, Count(*) AS [Count], Sum(DD.Qty) AS Qty, SUM(DD.Amount) AS Amount FROM ( SELECT DyeingOrder.ContractorID AS ID, DyeingOrder.ContractorName AS Name, DyeingOrder.Qty, DyeingOrder.Amount FROM View_DyeingOrder AS DyeingOrder "
                                     + "  WHERE DyeingOrder.[Status] NOT IN (9) AND DyeingOrder.SampleInvoiceID=0 " + GetDateSearch(nDateCompare, "DyeingOrder.OrderDate", dStratDate, dEndDate) + " ) AS DD ";
                            break;
                    case 3: sReturn1 = "  SELECT DD.ID, DD.Name, Count(*) AS [Count], Sum(DD.Qty) AS Qty, SUM(DD.Amount) AS Amount FROM ( SELECT DyeingOrder.ContractorID AS ID, DyeingOrder.ContractorName AS Name, DyeingOrder.Qty, DyeingOrder.Amount FROM View_DyeingOrder AS DyeingOrder "
                                      +"  WHERE DyeingOrder.[Status] NOT IN (9) AND DyeingOrder.SampleInvoiceID<>0 " + GetDateSearch(nDateCompare, "DyeingOrder.OrderDate", dStratDate, dEndDate) + " ) AS DD ";
                            break;
                }
            }
            else if (nPartID == 2)
            {

                switch (nDivisionID)
                {
                    case 1: sReturn1 = "  SELECT DD.ID, DD.Name, Count(*) AS [Count], Sum(DD.Qty) AS Qty, SUM(DD.Amount) AS Amount FROM (Select SI.SampleInvoiceID AS ID,  SI.SampleInvoiceNo AS Name,	(Select SUM(Qty) from SampleInvoiceDetail where SampleInvoiceID=SI.SampleInvoiceID) as Qty, SI.Amount,PaymentType from SampleInvoice as SI where PaymentType=1 "  + GetDateSearch(nDateCompare, "SI.SampleInvoiceDate", dStratDate, dEndDate) +  ") AS DD";
                            break;
                    case 2: sReturn1 = "  SELECT DD.ID, DD.Name, Count(*) AS [Count], Sum(DD.Qty) AS Qty, SUM(DD.Amount) AS Amount FROM ( (Select PaymentDetail.ReferenceID  as ID, 'Payment' as Name, 0 as Qty, PaymentAmount AS Amount from PaymentDetail where  PaymentDetail.ReferenceID in (select  SI.SampleInvoiceID from SampleInvoice as SI where PaymentType in (1)  and isnull(SI.ExportPIID,0) = 0 " + GetDateSearch(nDateCompare, "SI.SampleInvoiceDate", dStratDate, dEndDate) + "))) AS DD";
                            break;
                    case 3: sReturn1 =  "  SELECT DD.ID, DD.Name, Count(*) AS [Count], Sum(DD.Qty) AS Qty, SUM(DD.Amount) AS Amount FROM ( (Select PaymentDetail.ReferenceID  as ID, 'Payment' as Name, 0 as Qty, PaymentAmount AS Amount from PaymentDetail where  PaymentDetail.ReferenceID in (select  SI.SampleInvoiceID from SampleInvoice as SI where PaymentType in (1)  and isnull(SI.ExportPIID,0) <> 0 " + GetDateSearch(nDateCompare, "SI.SampleInvoiceDate", dStratDate, dEndDate) + "))) AS DD";
                            break;
                }
            }
            else if (nPartID == 3)
            {
                switch (nDivisionID)
                {
                    
                    case 1: sReturn1 = "  SELECT DD.ID, DD.Name, Count(*) AS [Count], Sum(DD.Qty) AS Qty, SUM(DD.Amount) AS Amount FROM (Select SI.SampleInvoiceID as ID, SI.SampleInvoiceNo as Name,	(Select SUM(Qty) from SampleInvoiceDetail where SampleInvoiceID=SI.SampleInvoiceID) as Qty, SI.Amount,PaymentType from SampleInvoice as SI where SI.PaymentType in (2,3)" + GetDateSearch(nDateCompare, "SI.SampleInvoiceDate", dStratDate, dEndDate) + ") AS DD";
                            break;
                    case 2: sReturn1 = "  SELECT DD.ID, DD.Name, Count(*) AS [Count], Sum(DD.Qty) AS Qty, SUM(DD.Amount) AS Amount FROM (Select PaymentDetail.ReferenceID  as ID, 'Payment' as Name, 0 as Qty, PaymentAmount AS Amount from PaymentDetail where  PaymentDetail.ReferenceID in (select  SI.SampleInvoiceID from SampleInvoice as SI where PaymentType in (2,3)  and isnull(SI.ExportPIID,0)= 0 " + GetDateSearch(nDateCompare, "SI.SampleInvoiceDate", dStratDate, dEndDate) + ")) AS DD";
                            break;
                    case 3: sReturn1 = "  SELECT DD.ID, DD.Name, Count(*) AS [Count], Sum(DD.Qty) AS Qty, SUM(DD.Amount) AS Amount FROM (Select PaymentDetail.ReferenceID  as ID, 'Payment' as Name, 0 as Qty, PaymentAmount AS Amount from PaymentDetail where  PaymentDetail.ReferenceID in (select  SI.SampleInvoiceID from SampleInvoice as SI where PaymentType in (2,3)  and isnull(SI.ExportPIID,0)<> 0 " + GetDateSearch(nDateCompare, "SI.SampleInvoiceDate", dStratDate, dEndDate) + ")) AS DD";
                            break;
                }
            }
            else if (nPartID == 4)
            {
                switch (nDivisionID)
                {
                    /*
                       SELECT DD.ID, DD.Name, Count(*) AS [Count], Sum(DD.Qty) AS Qty, SUM(DD.Amount) AS Amount FROM 
                        
                       GROUP BY DD.ID, DD.Name
                     */
                    case 1: sReturn1 = "  SELECT DD.ID, DD.Name, Count(*) AS [Count], Sum(DD.Qty) AS Qty, SUM(DD.Amount) AS Amount FROM (SELECT ExportPI.Qty, ExportPI.Amount, ExportPI.ExportPIID AS ID  , ExportPI.PINo AS Name FROM ExportPI WHERE ExportPI.BUID=" + buid + " and ExportPI.PIStatus in (2,3,4,5)  and ExportPI.PaymentType in (0,1) " + GetDateSearch(nDateCompare, "ExportPI.IssueDate", dStratDate, dEndDate) + ") AS DD";
                        break;
                    case 2: sReturn1 = "  SELECT DD.ID, DD.Name, Count(*) AS [Count], Sum(DD.Qty) AS Qty, SUM(DD.Amount) AS Amount FROM (SELECT ExportPI.Qty, ExportPI.Amount, ExportPI.ExportPIID as ID, ExportPI.PINo as Name FROM ExportPI WHERE ExportPI.BUID=" + buid + " and isnull(ExportPI.LCID,0)=0 and ExportPI.PIStatus in (2,3,4,5)  and ExportPI.PaymentType in (0,1) " + GetDateSearch(nDateCompare, "ExportPI.IssueDate", dStratDate, dEndDate) + ") AS DD";
                        break;
                    case 3: sReturn1 = "  SELECT DD.ID, DD.Name, Count(*) AS [Count], Sum(DD.Qty) AS Qty, SUM(DD.Amount) AS Amount FROM (SELECT ExportPI.Qty, ExportPI.Amount, ExportPI.ExportPIID as ID, ExportPI.PINo as Name FROM ExportPI WHERE ExportPI.BUID=" + buid + " and isnull(ExportPI.LCID,0)<>0 and ExportPI.PIStatus in (2,3,4,5)  and ExportPI.PaymentType in (0,1) " + GetDateSearch(nDateCompare, "ExportPI.IssueDate", dStratDate, dEndDate) + ") AS DD";
                        break;
                }
            }
            else
            {
                sReturn1 = "";
            }

            string sSQL = sReturn1 +  " GROUP BY DD.ID, DD.Name";
            return sSQL;
        }
        private string GetDateSearch(int nDateCompare, string sDateType, DateTime dDateStart, DateTime dDateEnd) 
        {
            string sReturn = "";
            #region Date Criteria
            if (nDateCompare > 0)
            {
                if (nDateCompare == 1)
                {
                    dDateEnd = dDateStart;
                }
                sReturn =" AND "+ sDateType + " BETWEEN '" + dDateStart.ToString("dd MMM yyy") + "' AND '" + dDateEnd.ToString("dd MMM yyy") + "' ";
            }
            #endregion
            return sReturn;
        }
        #endregion

        #region Export Bill Report
        [HttpPost]
        public JsonResult GetsGridData(SalesStatement oSalesStatement)
        {
            List<SalesStatement> oSalesStatements = new List<SalesStatement>();
            List<SalesStatement> oSalesStatements_Temp = new List<SalesStatement>();

            int tabId = Convert.ToInt32(oSalesStatement.nReportType);
            int nDateCompare = Convert.ToInt32(oSalesStatement.Params.Split('~')[0]);
            DateTime dtStartDate = Convert.ToDateTime(oSalesStatement.Params.Split('~')[1]);
            DateTime dtEndDate = Convert.ToDateTime(oSalesStatement.Params.Split('~')[2]);
            int BUID = Convert.ToInt32(oSalesStatement.Params.Split('~')[3]);

            try
            {
                if (tabId == 1)//UnitInfo
                {
                    if (nDateCompare > 0)
                    {
                        if (nDateCompare == 1)
                        {
                            oSalesStatements = SalesStatement.GetsSalesStatement(BUID, dtStartDate, dtStartDate, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                        else if (nDateCompare == 5)
                        {
                            oSalesStatements = SalesStatement.GetsSalesStatement(BUID, dtStartDate, dtEndDate.AddDays(1), ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                    }
                }
                if (tabId == 2)
                {
                    oSalesStatements = SalesStatement.Gets_Summary(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oSalesStatements.Count > 1) 
                    {
                        SalesStatement oSalesStatement_Temp = new SalesStatement();
                        oSalesStatement_Temp.BUName = "";
                        oSalesStatement_Temp.BankName = "GRAND TOTAL:";
                        oSalesStatement_Temp.Amount_LC = oSalesStatements.Sum(x => x.Amount_LC);
                        oSalesStatement_Temp.BOinHand = oSalesStatements.Sum(x => x.BOinHand); ;
                        oSalesStatement_Temp.BOInCusHand = oSalesStatements.Sum(x => x.BOInCusHand); ;
                        oSalesStatement_Temp.AcceptadBill = oSalesStatements.Sum(x => x.AcceptadBill); ;
                        oSalesStatement_Temp.NegoTransit = oSalesStatements.Sum(x => x.NegoTransit); ;
                        oSalesStatement_Temp.NegotiatedBill = oSalesStatements.Sum(x => x.NegotiatedBill); ;
                        oSalesStatement_Temp.Amount_Due = oSalesStatements.Sum(x => x.Amount_Due); ;
                        oSalesStatement_Temp.Amount_ODue = oSalesStatements.Sum(x => x.Amount_ODue); ;
                        oSalesStatement_Temp.Discounted = oSalesStatements.Sum(x => x.Discounted); ;
                        oSalesStatement_Temp.PaymentDone = oSalesStatements.Sum(x => x.PaymentDone); ;
                        oSalesStatement_Temp.BFDDRecd = oSalesStatements.Sum(x => x.BFDDRecd);
                        oSalesStatements.Add(oSalesStatement_Temp);
                    }
                }
                if (tabId == 3)
                {
                    if (nDateCompare > 0)
                    {
                        if (nDateCompare == 1)
                        {
                            oSalesStatements = SalesStatement.Gets_BillRealize(BUID, dtStartDate, dtStartDate, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }

                        if (nDateCompare == 5)
                        {
                            oSalesStatements = SalesStatement.Gets_BillRealize(BUID, dtStartDate, dtEndDate.AddDays(1), ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                    }
                }
                if (tabId == 4)//Maturity
                {
                    if (nDateCompare > 0)
                    {
                        if (nDateCompare == 1)
                        {
                            oSalesStatements = SalesStatement.Gets_BillMaturity(BUID, dtStartDate, dtEndDate, 0);
                        }

                        if (nDateCompare == 5)
                        {
                            oSalesStatements = SalesStatement.Gets_BillMaturity(BUID, dtStartDate, dtEndDate.AddDays(1), ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }

                        if (oSalesStatements.Count > 1)
                        {
                            foreach (SalesStatement oItem in oSalesStatements)
                            {
                                oItem.SDMStr = oItem.StartDateMonthStr;
                            }
                            SalesStatement oSalesStatement_Temp = new SalesStatement();
                            oSalesStatement_Temp.SDMStr = "GRAND TOTAL:";
                            oSalesStatement_Temp.Currency = oSalesStatements[0].Currency;
                            oSalesStatement_Temp.Amount = oSalesStatements.Sum(x => x.Amount);
                            oSalesStatements.Add(oSalesStatement_Temp);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                oSalesStatements = new List<SalesStatement>();
                oSalesStatement.ErrorMessage = ex.Message;
                oSalesStatements.Add(oSalesStatement);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalesStatements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Print_ExportBillReport(int BUID, string sParam, string sPHeader, string sExportBillFieldST)
        {
            _oExportLCRegisters = new List<ExportLCRegister>();
            string sHeader="Export Bill List";
            string sState = "";
            string sCurrentStatus = Convert.ToString(sParam.Split('~')[0]);
            if (Convert.ToInt32(sParam.Split('~')[7]) == 2)//Tab_ID
            {
                 sState=(sParam.Split('~')[0]);
                    sHeader = EnumObject.jGet((EnumLCBillEvent)Convert.ToInt16(sState.Split(',')[0]));
            }
            else if (Convert.ToInt32(sParam.Split('~')[7]) == 3)//Tab_ID
            {
                
                sHeader = "Payment Receive";
            }
            else if (Convert.ToInt32(sParam.Split('~')[7]) == 4)//Tab_ID
            {
                sHeader = "Maturity Report";
            }    

            ExportBillReport oExportBillReport = new ExportBillReport();
            oExportBillReport.Params = sParam;
            string sSQL = MakeSQL(oExportBillReport, BUID);

            if (sCurrentStatus.Contains("0,1,2"))
            {
                var sSQL_T = sParam.Split('~');
                string sSQL_LC = GetSQL(sSQL_T[1] + "~" + sSQL_T[3] + "~" + sSQL_T[4] + "~" + sSQL_T[5] + "~" + sSQL_T[6]+"~"+4, true);
                _oExportLCRegisters = ExportLCRegister.Gets(sSQL_LC, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sHeader = "DOC Status";
            }

            if (sSQL == "Error")
            {
                _oExportBillReport = new ExportBillReport();
                _oExportBillReport.ErrorMessage = "Please select a searching critaria.";
                _oExportBillReports = new List<ExportBillReport>();
            }
            else
            {
                _oExportBillReports = new List<ExportBillReport>();
                if (sState=="4")
                {
                    sSQL = sSQL + " order by LDBCDate";
                }
                if (sState == "5,1")
                {
                    sSQL = sSQL + " order by MaturityDate";
                }
                if (sState == "5,2")
                {
                    sSQL = sSQL + " order by MaturityDate";
                }
                _oExportBillReports = ExportBillReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oExportBillReports.Count == 0)
                {
                    _oExportBillReports = new List<ExportBillReport>();
                }

                if (!string.IsNullOrEmpty(sPHeader)) 
                {
                    sHeader += " (";
                    if (!string.IsNullOrEmpty(sPHeader.Split('~')[0]))
                    {
                        sHeader += sPHeader.Split('~')[0];
                    }
                    if (!string.IsNullOrEmpty(sPHeader.Split('~')[1]))
                    {
                        sHeader += sPHeader.Split('~')[1];
                    }
                    if (!string.IsNullOrEmpty(sPHeader.Split('~')[2]))
                    {
                        sHeader += sPHeader.Split('~')[2];
                    }

                    sHeader += " )";
                }
            }
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            oExportBillReport.ErrorMessage = sExportBillFieldST;

            if (_oExportBillReports.Count == 0)
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }
            else 
            {
                if (sCurrentStatus.Contains("0,1,2"))
                {
                    rptSalesStatement_Bill oReport = new rptSalesStatement_Bill();
                    byte[] abytes = oReport.PrepareReportWithLC(_oExportBillReports,_oExportLCRegisters, oBusinessUnit, oCompany, sHeader, sExportBillFieldST);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    rptSalesStatement_Bill oReport = new rptSalesStatement_Bill();
                    byte[] abytes = oReport.PrepareReport(_oExportBillReports, oBusinessUnit, oCompany, sHeader, sExportBillFieldST);
                    return File(abytes, "application/pdf");
                }
            }
        }
        private string MakeSQL(ExportBillReport oExportBillReport, int BUID)
        {
            string sParams = oExportBillReport.Params;

            string sCurrentStatus = "";
            int nBankBranchID_Nego = 0;

            int ntab= 0;
            string sDateType = "";
            int nDateSearchCriteria = 0;
            DateTime dStartDateCritaria = DateTime.Today;
            DateTime dEndDateCritaria = DateTime.Today;
           
            if (!string.IsNullOrEmpty(sParams))
            {
                sCurrentStatus = Convert.ToString(sParams.Split('~')[0]);

                if (sCurrentStatus == "51")
                {
                    sCurrentStatus = "5,1";
                }
                else if (sCurrentStatus == "52")
                {
                    sCurrentStatus = "5,2";
                }
                nBankBranchID_Nego = Convert.ToInt32(sParams.Split('~')[1]);
               
                sDateType = Convert.ToString(sParams.Split('~')[2]);
                nDateSearchCriteria = Convert.ToInt32(sParams.Split('~')[3]);
                if (nDateSearchCriteria > 0)
                {
                    dStartDateCritaria = (sParams.Split('~')[4] == "") ? DateTime.Now : Convert.ToDateTime(sParams.Split('~')[4]);
                    dEndDateCritaria = (sParams.Split('~')[5] == "") ? DateTime.Now : Convert.ToDateTime(sParams.Split('~')[5]);
                }
                ntab = Convert.ToInt32(sParams.Split('~')[7]);
            }
            else
            {
                sCurrentStatus = "0";
            }
            string sReturn1 = "SELECT * FROM View_ExportBillReport AS EB";
            string sReturn = "";

            #region Current State
            if (!String.IsNullOrEmpty(sCurrentStatus))
            {
                Global.TagSQL(ref sReturn);

                if(ntab==3)
                    sReturn = sReturn + " EB.State NOT IN (" + sCurrentStatus + ") ";
                else
                    sReturn = sReturn + " EB.State IN (" + sCurrentStatus + ") ";

                if (ntab ==2 && sCurrentStatus.Split(',').Count() > 1)
                {
                    if (!sCurrentStatus.Contains("0,1,2"))
                    { 
                        if (sCurrentStatus.Split(',')[1].Equals("1"))
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + "  Convert(Date,( MaturityDate))>=Convert(Date,(GetDate())) ";
                        }else if (sCurrentStatus.Split(',')[1].Equals("2"))
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + "  Convert(Date,( MaturityDate))<Convert(Date,(GetDate()))";
                        }
                        if (sCurrentStatus.Split(',')[0].Equals("5"))
                            sCurrentStatus = "5";
                    }
                } 
            }
            #endregion

            #region _Nego Bank
            if (nBankBranchID_Nego > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.BankBranchID_Negotiation = " + nBankBranchID_Nego;
            }
            #endregion

            #region Date Criteria
            if (sDateType != "None")
            {
                if (nDateSearchCriteria > 0)
                {
                    if (ntab == 3 || ntab == 4) 
                    {
                        //dStartDateCritaria = dStartDateCritaria.AddDays(-(dStartDateCritaria.Day-1));
                        dEndDateCritaria = dStartDateCritaria.AddMonths(1).AddDays(-1);
                    }

                    Global.TagSQL(ref sReturn);
                    if (nDateSearchCriteria == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " EB." + sDateType + " = '" + dStartDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                    else if (nDateSearchCriteria == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " EB." + sDateType + " != '" + dStartDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                    else if (nDateSearchCriteria == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " EB." + sDateType + " > '" + dStartDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                    else if (nDateSearchCriteria == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " EB." + sDateType + " < '" + dStartDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                    else if (nDateSearchCriteria == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " EB." + sDateType + " BETWEEN '" + dStartDateCritaria.ToString("dd MMM yyy") + "' AND '" + dEndDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                    else if (nDateSearchCriteria == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " EB." + sDateType + " NOT BETWEEN '" + dStartDateCritaria.ToString("dd MMM yyy") + "' AND '" + dEndDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                }
            }
            #endregion

            #region BUID
            if (BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.BUID=" + BUID;
            }
            #endregion

            sReturn = sReturn1 + sReturn;

            return sReturn;
        }
        #endregion

        #region Excel
        public List<TableHeader> GetHeader()
        {
            #region Header
            List<TableHeader> table_headers = new List<TableHeader>();
            table_headers.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
          
            table_headers.Add(new TableHeader { Header = "Bill No", Width = 25f, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "LC No", Width = 25f, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Party Name", Width = 40f, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "PI No", Width = 22, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Issue Bank", Width = 50, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "LDBC No", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "LDBC Date", Width = 20, IsRotate = false });

            table_headers.Add(new TableHeader { Header = "Qty", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Amount", Width = 20, IsRotate = false });
           

            table_headers.Add(new TableHeader { Header = "Maturity Rcv Date", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Maturity Date", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Due", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Invoice Date", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Due (P)", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Doc Prepare Date", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Send To Party", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Expiry Date", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Send To Bank", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Relization Date", Width = 20, IsRotate = false });

            table_headers.Add(new TableHeader { Header = "FDD Rec Date", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Recd From Party", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "MKT Person", Width = 20, IsRotate = false });
            #endregion

            return table_headers;
        }
        public void Excel_ExportBillReport(int BUID, string sParam, string sPHeader)
        {
            #region Get Data From DB
            _oExportLCRegisters = new List<ExportLCRegister>();
            string sHeader = "Export Bill List";

            string sCurrentStatus = Convert.ToString(sParam.Split('~')[0]);
            if (Convert.ToInt32(sParam.Split('~')[7]) == 2)//Tab_ID
            {
                string sState = (sParam.Split('~')[0]);
                if (sState == "51")
                {
                    sHeader = "Amount";
                }
                else if(sState == "52")
                {
                    sHeader = "Amount O Deu";
                }
                else
                {
                    sHeader = EnumObject.jGet((EnumLCBillEvent)Convert.ToInt16(sState.Split(',')[0]));
                }
            }
            else if (Convert.ToInt32(sParam.Split('~')[7]) == 3)//Tab_ID
            {

                sHeader = "Payment Receive";
            }
            else if (Convert.ToInt32(sParam.Split('~')[7]) == 4)//Tab_ID
            {
                sHeader = "Maturity Report";
            }

            ExportBillReport oExportBillReport = new ExportBillReport();
            oExportBillReport.Params = sParam;
            string sSQL = MakeSQL(oExportBillReport, BUID);

            if (sCurrentStatus.Contains("0,1,2,3,4,5,6,7,15,25"))
            {
                var sSQL_T = sParam.Split('~');
                string sSQL_LC = GetSQL(sSQL_T[1] + "~" + sSQL_T[3] + "~" + sSQL_T[4] + "~" + sSQL_T[5] + "~" + sSQL_T[6] + "~" + 4, true);
                _oExportLCRegisters = ExportLCRegister.Gets(sSQL_LC, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sHeader = "DOC Status";
            }

            if (sSQL == "Error")
            {
                _oExportBillReport = new ExportBillReport();
                _oExportBillReport.ErrorMessage = "Please select a searching critaria.";
                _oExportBillReports = new List<ExportBillReport>();
            }
            else
            {
                _oExportBillReports = new List<ExportBillReport>();
                _oExportBillReports = ExportBillReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oExportBillReports.Count == 0)
                {
                    _oExportBillReports = new List<ExportBillReport>();
                }

                if (!string.IsNullOrEmpty(sPHeader))
                {
                    sHeader += " (";
                    if (!string.IsNullOrEmpty(sPHeader.Split('~')[0]))
                    {
                        sHeader += sPHeader.Split('~')[0];
                    }
                    if (!string.IsNullOrEmpty(sPHeader.Split('~')[1]))
                    {
                        sHeader += sPHeader.Split('~')[1];
                    }
                    if (!string.IsNullOrEmpty(sPHeader.Split('~')[2]))
                    {
                        sHeader += sPHeader.Split('~')[2];
                    }

                    sHeader += " )";
                }
            }

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            #endregion

            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header = GetHeader();
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Export L/C Status");
                    sheet.Name = sHeader;

                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 12]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 13, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = sHeader ; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 12]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 13, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    ExcelTool.Formatter = " #,##0.00;(#,##0.00)";

                    #region Data
                    
                    nRowIndex++;

                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    nRowIndex++; int nCount = 0, nRowSpan = 0;
                    foreach (var obj in _oExportBillReports)
                    {
                        nStartCol = 2; ExcelTool.Formatter = " #,##0.00;(#,##0.00)";
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ExportBillNo, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ExportLCNo, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ApplicantName, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.PINo, false);

                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.BankName_Issue + "[" + obj.BBranchName_Issue+"]", false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LDBCNo, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LDBCDateSt, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty_Bill.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Amount.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MaturityReceivedDateSt, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MaturityDateSt, false);

                        ExcelTool.Formatter = " #,##0;(#,##0)";
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.DueDay.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.StartDateSt, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.DueDay_Party.ToString(), true);

                        ExcelTool.Formatter = " #,##0.00;(#,##0.00)";
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.DocPrepareDateSt, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.SendToPartySt, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ExpiryDateSt, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.SendToBankDateSt, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.RelizationDateSt, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.BankFDDRecDateSt, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.RecdFromPartySt, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MKTPName, false);
                        nRowIndex++;
                    }
                    #region Grand Total
                    nStartCol = 2;
                    ExcelTool.Formatter = " #,##0.00;(#,##0.00)";
                    ExcelTool.FillCellMerge(ref sheet, " Grand Total: ", nRowIndex, nRowIndex, nStartCol, 9, true, ExcelHorizontalAlignment.Right);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol += 8, Global.MillionFormat(_oExportBillReports.Sum(x => x.Qty_Bill)), true, true);
                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, Global.MillionFormat(_oExportBillReports.Sum(x => x.Amount)), true, true);

                    for(int i=0;i<=12;i++)
                        ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);

                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ExportLCRegister(" + sHeader + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }

        #endregion

        #region Export LC Register
        public ActionResult PrintExportLCRegister(string sParam)
        {
            ExportLCRegister oExportLCRegister = new ExportLCRegister();
            try
            {
                _sErrorMesage = "";
                _oExportLCRegisters = new List<ExportLCRegister>();
                try
                {
                    string sSQL = GetSQL(sParam, true);
                    _oExportLCRegisters = ExportLCRegister.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                catch (Exception ex)
                {
                    _sErrorMesage = ex.Message;
                }

                if (_oExportLCRegisters.Count <= 0 && string.IsNullOrEmpty(_sErrorMesage))
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oExportLCRegisters = new List<ExportLCRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();

                int nBUID = Convert.ToInt32(sParam.Split('~')[4]);
                oBU = oBU.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptSalesStatement_LC oReport = new rptSalesStatement_LC();
                byte[] abytes = oReport.PrepareReport(_oExportLCRegisters, oCompany, Convert.ToInt32(sParam.Split('~')[5]), "  ");
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }
        private string GetSQL(string sTemp, bool isPDF)
        {
            string sReturn1 = "SELECT ExportPIID,PINo,PIDate,Round(LCValue,2) AS LCValue,BuyerID,MKTEmpID,PIStatus,VersionNo,LCOpenDate,AmendmentDate,LCReceiveDate,UDRecDate,UDRcvType,ExportLCID,LCNo,ApplicantID,LCStatus,NegoBankBranchID,IssueBankBranchID,NoteQuery,NoteUD,HaveQuery,GetOriginalCopy,Currency,ApplicantName,BuyerName,MKTPersonName,NegoBankName,IssueBankName,(Value_DO) AS Value_DO,Value_DC AS Value_DC,ProductName,Qty ,Qty_Invoice,Round(UnitPrice,4) AS UnitPrice,Acc_Bank,Acc_Party,ShipmentDate,[ExpiryDate]"
                               + "FROM View_ExportLCRegister ";
            string sReturn = "";

            if (!string.IsNullOrEmpty(sTemp))
            {
                #region Set Values
                int nBankBranch_Nego = Convert.ToInt32(sTemp.Split('~')[0]);
                int nCboLCOpenDate = Convert.ToInt32(sTemp.Split('~')[1]);
                DateTime dFromLCOpenDate = DateTime.Now;
                DateTime dToLCOpenDate = DateTime.Now;
                if (nCboLCOpenDate > 0)
                {
                    dFromLCOpenDate = Convert.ToDateTime(sTemp.Split('~')[2]);
                    dToLCOpenDate = Convert.ToDateTime(sTemp.Split('~')[3]);
                }
                int nBUID = Convert.ToInt32(sTemp.Split('~')[4]);
                int nReportLayout = Convert.ToInt32(sTemp.Split('~')[5]);
                _nReportLayout = nReportLayout;

                #endregion

                #region Make Query

                #region BankBranch_Nego
                if (nBankBranch_Nego > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NegoBankBranchID =" + nBankBranch_Nego;
                }
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "(Qty-Qty_Invoice)>0.2";
                #endregion
                #region LC Open Date
                if (nCboLCOpenDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboLCOpenDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboLCOpenDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboLCOpenDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboLCOpenDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboLCOpenDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboLCOpenDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion
                #region Business Unit
                if (nBUID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BUID = " + nBUID + " ";
                }
                #endregion
                #endregion
             
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LCStatus IN (0,1,2,3,4,5) ";

            }
            sReturn = sReturn1 + sReturn;
          //  sReturn = sReturn1 + sReturn + " GROUP BY ProductName,ExportPIID,PINo,BuyerID,PIDate,BuyerID,MKTEmpID,UDRcvType,ExportLCID,ApplicantID,NegoBankBranchID,IssueBankBranchID,NoteQuery,NoteUD,HaveQuery,GetOriginalCopy,Currency,ApplicantName,BuyerName,MKTPersonName,NegoBankName,IssueBankName,PIStatus,VersionNo,LCOpenDate,AmendmentDate,LCReceiveDate,UDRecDate,LCNo,LCStatus,Acc_Bank,Acc_Party,ShipmentDate,[ExpiryDate],UnitPrice";
          
            string sOrderBy = "";

            if (_nReportLayout == (int)EnumReportLayout.PartyWise)
                sOrderBy = " Order By ApplicantName,ApplicantID,ExportLCID,ProductName,NegoBankName";
            else if (_nReportLayout == (int)EnumReportLayout.BankWise)
                sOrderBy = " Order By NegoBankName,NegoBankBranchID,ExportLCID,ProductName,ApplicantName";
            else if (_nReportLayout == (int)EnumReportLayout.ProductWise)
                sOrderBy = " Order By ProductName,ExportLCID,ApplicantName,NegoBankName";

            if (isPDF)
                sReturn += sOrderBy;
            else
                sReturn += " Order By ExportLCID,ProductName,ApplicantName,NegoBankName";

            return sReturn;
        }
        #endregion
    }
}