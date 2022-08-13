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
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class ExportBillRegisterController : Controller
    {
        string _sDateRange = "";
        string _sErrorMesage = "";
        public string sExportLCType = "";
        ExportBillReport _oExportBillReport = new ExportBillReport();
        List<ExportBillReport> _oExportBillReports = new List<ExportBillReport>();

        #region Actions
        public ActionResult ViewExportBillRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ExportBillReport oExportBillReport = new ExportBillReport();

            List<BankBranch> oBankBranchs_Nego = new List<BankBranch>();
            oBankBranchs_Nego = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BankBranchs_Nego = oBankBranchs_Nego;

            ViewBag.CompareOperators = Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.LCBillEventObj = EnumObject.jGets(typeof(EnumLCBillEvent));


            List<Currency> oCurrencys = new List<Currency>();
            oCurrencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = oCurrencys;
            ViewBag.BUID = buid;
            ViewBag.LCTypes = EnumObject.jGets(typeof(EnumExportLCType));
            return View(oExportBillReport);
        }
        
        [HttpPost]
        public JsonResult SetSessionSearchCriteria(ExportBillReport oExportBillReport)
        {
            List<ExportBillReport> oExportBillReports = new List<ExportBillReport>();
            string sSQL = MakeSQL(oExportBillReport);
            if (sSQL == "Error")
            {
                _oExportBillReport = new ExportBillReport();
                _oExportBillReport.ErrorMessage = "Please select a searching critaria.";
                oExportBillReports = new List<ExportBillReport>();
            }
            else
            {
                oExportBillReports = new List<ExportBillReport>();
                oExportBillReports = ExportBillReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oExportBillReports.Count == 0)
                    oExportBillReports = new List<ExportBillReport>();
                else
                    oExportBillReports[0].ErrorMessage = _sDateRange;
            }
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oExportBillReports);

            var jsonResult = Json(oExportBillReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string MakeSQL(ExportBillReport oExportBillReport)
        {
            string sParams = oExportBillReport.Params;

            string sContractorIds = "";
            string sCurrentStatus = "";
            int nBankBranchID_Nego = 0;
            string sBankBranch_IssueIds = "";
            int nSearchAmountType = 0;
            double nFromAmount = 0;
            double nToAmount = 0;

            string sDateType = "";
            int nDateSearchCriteria = 0;
            DateTime dStartDateCritaria = DateTime.Today;
            DateTime dEndDateCritaria = DateTime.Today;

            int nStateDateType = 0;
            int nDateSearchState = 0;
            DateTime dStartDateState = DateTime.Today;
            DateTime dEndDateState = DateTime.Today;
            string sExportLCNo = "";
            string sExportBillNo = "";
            string sExportLDBCNo = "";
            string sExportPINo = "";
            int nBUID = 0;
            int nPrintType = 0;
            int nPrintLayout = 0;
            bool IsYetToEUP = false;
            bool IsEUPDone = false;
            int nExportLC = 0;

            if (!string.IsNullOrEmpty(sParams))
            {
                sContractorIds = Convert.ToString(sParams.Split('~')[0]);
                sCurrentStatus = Convert.ToString(sParams.Split('~')[1]);
                nBankBranchID_Nego = Convert.ToInt32(sParams.Split('~')[2]);
                sBankBranch_IssueIds = Convert.ToString(sParams.Split('~')[3]);
                nSearchAmountType = Convert.ToInt32(sParams.Split('~')[4]);
                nFromAmount = Convert.ToDouble(sParams.Split('~')[5]);
                nToAmount = Convert.ToDouble(sParams.Split('~')[6]);

                sDateType = Convert.ToString(sParams.Split('~')[7]);
                nDateSearchCriteria = Convert.ToInt32(sParams.Split('~')[8]);
                if (nDateSearchCriteria > 0)
                {
                    dStartDateCritaria = (sParams.Split('~')[9] == "") ? DateTime.Now : Convert.ToDateTime(sParams.Split('~')[9]);
                    dEndDateCritaria = (sParams.Split('~')[10] == "") ? DateTime.Now : Convert.ToDateTime(sParams.Split('~')[10]);
                }
                nStateDateType = Convert.ToInt32(sParams.Split('~')[11]);
                nDateSearchState = Convert.ToInt32(sParams.Split('~')[12]);
                if (nDateSearchState > 0)
                {
                    dStartDateState = (sParams.Split('~')[13] == "") ? DateTime.Now : Convert.ToDateTime(sParams.Split('~')[13]);
                    dEndDateState = (sParams.Split('~')[14] == "") ? DateTime.Now : Convert.ToDateTime(sParams.Split('~')[14]);
                }
                sExportLCNo = Convert.ToString(sParams.Split('~')[15]);
                sExportBillNo = Convert.ToString(sParams.Split('~')[16]);
                sExportLDBCNo = Convert.ToString(sParams.Split('~')[17]);
                sExportPINo = Convert.ToString(sParams.Split('~')[18]);
                nBUID = Convert.ToInt32(sParams.Split('~')[19]);
                IsYetToEUP = Convert.ToBoolean(oExportBillReport.Params.Split('~')[20]);
                IsEUPDone = Convert.ToBoolean(oExportBillReport.Params.Split('~')[21]);
                nPrintType = Convert.ToInt32(sParams.Split('~')[22]);
                nPrintLayout = Convert.ToInt32(sParams.Split('~')[23]);
                nExportLC = Convert.ToInt32(sParams.Split('~')[24]);

                if (nDateSearchCriteria == (int)EnumCompareOperator.EqualTo)
                    _sDateRange = "Equal: " + dStartDateCritaria.ToString("dd MMM yyy");
                else if (nDateSearchCriteria == (int)EnumCompareOperator.NotEqualTo)
                    _sDateRange = "Not Equal: " + dStartDateCritaria.ToString("dd MMM yyy");
                else if (nDateSearchCriteria == (int)EnumCompareOperator.GreaterThan)
                    _sDateRange = "Greater Than: " + dStartDateCritaria.ToString("dd MMM yyy");
                else if (nDateSearchCriteria == (int)EnumCompareOperator.SmallerThan)
                    _sDateRange = "Smaller Than: " + dStartDateCritaria.ToString("dd MMM yyy");
                else if (nDateSearchCriteria == (int)EnumCompareOperator.Between)
                    _sDateRange = "Between: " + dStartDateCritaria.ToString("dd MMM yyy") + "  To  " + dEndDateCritaria.ToString("dd MMM yyy");
                else if (nDateSearchCriteria == (int)EnumCompareOperator.Between)
                    _sDateRange = "Not Between: " + dStartDateCritaria.ToString("dd MMM yyy") + "  To  " + dEndDateCritaria.ToString("dd MMM yyy");
                
            }
            else
            {
                sCurrentStatus = "0";
            }

            string sStateForBTBLC = (int)EnumLCBillEvent.BOEinHand + "," + (int)EnumLCBillEvent.BOEInCustomerHand + "," + (int)EnumLCBillEvent.BuyerAcceptedBill + "," + (int)EnumLCBillEvent.NegoTransit;
            string sStateForSTBLC = (int)EnumLCBillEvent.NegotiatedBill + "";
            string sStateForRFBLC = (int)EnumLCBillEvent.BankAcceptedBill + "," + (int)EnumLCBillEvent.Discounted + "," + (int)EnumLCBillEvent.ReqForDiscounted;

            string sReturn1 = "SELECT * FROM View_ExportBillRegister AS EB";
            string sReturn = "";

            #region PartyName
            if (!String.IsNullOrEmpty(sContractorIds))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ApplicantID IN (" + sContractorIds + ")";
            }
            #endregion

            #region Current State
            if (!String.IsNullOrEmpty(sCurrentStatus))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.State IN (" + sCurrentStatus + ") ";

            }
            #endregion

            #region Export LC Type
            if (nExportLC >0)
            {             
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ExportLCType = " + nExportLC.ToString();
            }
            #endregion

            #region _Nego Bank
            if (nBankBranchID_Nego > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.BankBranchID_Negotiation = " + nBankBranchID_Nego;
            }
            #endregion

            #region Issue Bank
            if (!string.IsNullOrEmpty(sBankBranch_IssueIds))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.BankBranchID_Issue = " + sBankBranch_IssueIds;
            }
            #endregion

            #region Bill Amount
            if (nSearchAmountType > 0)
            {
                Global.TagSQL(ref sReturn);
                if (nSearchAmountType == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " EB.Amount = " + nFromAmount;
                }
                else if (nSearchAmountType == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " EB.Amount != " + nFromAmount;
                }
                else if (nSearchAmountType == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " EB.Amount > " + nFromAmount;
                }
                else if (nSearchAmountType == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " EB.Amount < " + nFromAmount;
                }
                else if (nSearchAmountType == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " EB.Amount BETWEEN " + nFromAmount + " AND " + nToAmount + "";
                }
                else if (nSearchAmountType == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " EB.Amount NOT BETWEEN " + nFromAmount + " AND " + nToAmount + "";
                }
            }
            #endregion

            #region Date Criteria
            if (sDateType != "None")
            {

                if (nDateSearchCriteria > 0)
                {
                    Global.TagSQL(ref sReturn);
                    if (nDateSearchCriteria == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " EB." + sDateType + " = '" + dStartDateCritaria.ToString("dd MMM yyy") + "' ";
                        _sDateRange = "Equal: " + dStartDateCritaria.ToString("dd MMM yyy");
                    }
                    else if (nDateSearchCriteria == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " EB." + sDateType + " != '" + dStartDateCritaria.ToString("dd MMM yyy") + "' ";
                        _sDateRange = "Not Equal: " + dStartDateCritaria.ToString("dd MMM yyy");
                    }
                    else if (nDateSearchCriteria == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " EB." + sDateType + " > '" + dStartDateCritaria.ToString("dd MMM yyy") + "' ";
                        _sDateRange = "Greater Than: " + dStartDateCritaria.ToString("dd MMM yyy");
                    }
                    else if (nDateSearchCriteria == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " EB." + sDateType + " < '" + dStartDateCritaria.ToString("dd MMM yyy") + "' ";
                        _sDateRange = "Smaller Than: " + dStartDateCritaria.ToString("dd MMM yyy");
                    }
                    else if (nDateSearchCriteria == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " EB." + sDateType + " BETWEEN '" + dStartDateCritaria.ToString("dd MMM yyy") + "' AND '" + dEndDateCritaria.ToString("dd MMM yyy") + "' ";
                        _sDateRange = "Between: " + dStartDateCritaria.ToString("dd MMM yyy") + "  To  " + dEndDateCritaria.ToString("dd MMM yyy");
                    }
                    else if (nDateSearchCriteria == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " EB." + sDateType + " NOT BETWEEN '" + dStartDateCritaria.ToString("dd MMM yyy") + "' AND '" + dEndDateCritaria.ToString("dd MMM yyy") + "' ";
                        _sDateRange = "Not Between: " + dStartDateCritaria.ToString("dd MMM yyy") + "  To  " + dEndDateCritaria.ToString("dd MMM yyy");
                    }
                }
            }
            #endregion

            #region State on Date
            if (nStateDateType > -1)
            {

                if (nDateSearchState > 0)
                {
                    Global.TagSQL(ref sReturn);
                    if (nDateSearchState == (int)EnumCompareOperator.EqualTo)
                    {
                        // sReturn = sReturn + " EB.ExportBillReportID IN (SELECT EBH.ExportBillReportID FROM View_ExportBillReportHistory AS EBH WHERE EBH.State= " + oExportBillReport.StateDateType + " AND EBH.DBServerDateTime = '" + oExportBillReport.StartDateState.ToString("dd MMM yyyy") + "') ";
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + nStateDateType + " AND EBH.DBServerDateTime >= '" + dStartDateState.ToString("dd MMM yyyy") + "' AND EBH.DBServerDateTime<'" + dStartDateState.AddDays(1).ToString("dd MMM yyyy") + "') ";

                    }
                    else if (nDateSearchState == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + nStateDateType + " AND EBH.DBServerDateTime != '" + dStartDateState.ToString("dd MMM yyyy") + "') ";
                    }
                    else if (nDateSearchState == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + nStateDateType + " AND EBH.DBServerDateTime > '" + dStartDateState.ToString("dd MMM yyyy") + "') ";
                    }
                    else if (nDateSearchState == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + nStateDateType + " AND EBH.DBServerDateTime < '" + dStartDateState.ToString("dd MMM yyyy") + "') ";
                    }
                    else if (nDateSearchState == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + nStateDateType + " AND EBH.DBServerDateTime >= '" + dStartDateState.ToString("dd MMM yyyy") + "' AND  EBH.DBServerDateTime<'" + dEndDateState.ToString("dd MMM yyyy") + "') ";
                    }
                    else if (nDateSearchState == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + nStateDateType + " AND EBH.DBServerDateTime NOT BETWEEN '" + dStartDateState.ToString("dd MMM yyyy") + "' AND '" + dEndDateState.ToString("dd MMM yyyy") + "') ";
                    }
                }
            }
            #endregion
            #region L/C No
            if (!String.IsNullOrEmpty(sExportLCNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ExportLCNo like '%" + sExportLCNo + "%' ";
            }
            #endregion
            #region L/C No
            if (!String.IsNullOrEmpty(sExportBillNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ExportBillNo like '%" + sExportBillNo + "%' ";
            }
            #endregion
            #region LDBCNo
            if (!String.IsNullOrEmpty(sExportLDBCNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.LDBCNo like '%" + sExportLDBCNo + "%' ";
            }
            #endregion
            #region PICNo
            if (!String.IsNullOrEmpty(sExportPINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "EB.ExportBillID in (Select ExportBillID from ExportBillDetail where ExportPIDetailID in (Select ExportPIDetail.ExportPIDetailID from ExportPIDetail where ExportPIID in (Select ExportPI.ExportPIID from ExportPI where PINo like '%" + sExportPINo + "%')))";
            }
            #endregion
            #region BUID
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.BUID=" + nBUID;
            }
            #endregion
            #region Export UP

            if (IsYetToEUP)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ExportBillID Not In (Select  ExportBillID From ExportUPDetail)";
            }
            if (IsEUPDone)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ExportBillID In (Select  ExportBillID From ExportUPDetail)";
            }

            #endregion

            #region PRINT TYPE
            if (nPrintType ==1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.State IN (" + sStateForBTBLC+")";
            }
            else if (nPrintType == 2)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.State IN (" + sStateForSTBLC + ")";
            }
            else if (nPrintType == 3)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.State IN (" + sStateForRFBLC + ")";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        public ActionResult PrintExportBillRegister(int nType, int nLCType)
        {
            ExportBillReport oExportBillReport = new ExportBillReport();
            try
            {
                _sErrorMesage = "";
                _oExportBillReports = new List<ExportBillReport>();
                _oExportBillReports = (List<ExportBillReport>)Session[SessionInfo.ParamObj];
                if (nLCType != null)
                {
                    if (nLCType == 0)
                    {
                        sExportLCType = "";
                    }
                    if (nLCType == 1)
                    {
                        sExportLCType = "LC";
                    }
                    if (nLCType == 2)
                    {
                        sExportLCType = "FDD";
                    }
                    if (nLCType == 3)
                    {
                        sExportLCType = "TT";
                    } 
                }
                if (_oExportBillReports.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oExportBillReports = new List<ExportBillReport>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oExportBillReports[0].BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                _sDateRange = _oExportBillReports[0].ErrorMessage;
                //int nReportType=1, nReportLayout = 0;
                rptExportBillRegister oReport = new rptExportBillRegister();
                byte[] abytes = oReport.PrepareReport(_oExportBillReports, oCompany, oBU, 0, nType, _sDateRange, sExportLCType);//nType=0
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        public void ExportToExcelExportBillRegister(int nType, int nLCType)
        {
            ExportBillReport oExportBillReport = new ExportBillReport();
            try
            {
                if (nLCType != null)
                {
                    if (nLCType == 0)
                    {
                        sExportLCType = "";
                    }
                    if (nLCType == 1)
                    {
                        sExportLCType = "LC";
                    }
                    if (nLCType == 2)
                    {
                        sExportLCType = "FDD";
                    }
                    if (nLCType == 3)
                    {
                        sExportLCType = "TT";
                    }
                }
                _sErrorMesage = "";
                _oExportBillReports = new List<ExportBillReport>();
                _oExportBillReports = (List<ExportBillReport>)Session[SessionInfo.ParamObj];
                _sDateRange = _oExportBillReports[0].ErrorMessage;

                _oExportBillReports = _oExportBillReports.OrderBy(x => x.ExportBillID).ThenBy(x => x.ExportLCNo).ThenBy(x => x.PINo).ThenBy(x => x.ProductName).ToList();
                if (_oExportBillReports.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oExportBillReports = new List<ExportBillReport>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oExportBillReports[0].BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);                
               
                if (nType == 1)//Back2Back
                {
                    #region Print XL

                    int nRowIndex = 2, nStartRow = 2, nStartCol = 2, nEndCol = 15, nColumn = 1, nCount = 0, nImportLCID = 0;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("ExportBillRegister");
                        sheet.Name = "Export Bill Register";
                        sheet.Column(++nColumn).Width = 10; //SL No
                        sheet.Column(++nColumn).Width = 15; //LC No
                        sheet.Column(++nColumn).Width = 25; //Issue Bank
                        sheet.Column(++nColumn).Width = 20; //NgoBank
                        sheet.Column(++nColumn).Width = 30; //PartyName
                        sheet.Column(++nColumn).Width = 15; //PINO
                        sheet.Column(++nColumn).Width = 30; //Commodity
                        sheet.Column(++nColumn).Width = 15; //Qty
                        sheet.Column(++nColumn).Width = 15; //Amount
                        sheet.Column(++nColumn).Width = 15; //Amount
                        sheet.Column(++nColumn).Width = 15; //Shipmentdate
                        sheet.Column(++nColumn).Width = 15; //ExpireDate
                        sheet.Column(++nColumn).Width = 15; //State
                        sheet.Column(++nColumn).Width = 25; //CP
                        //   nEndCol = 13;

                        #region Report Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 7]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nStartCol + 8, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Export Bill Register (BTB)"; cell.Style.Font.Bold = true;
                        cell.Style.WrapText = true;
                        cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;


                        #endregion

                        #region Address & Date
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 7]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nStartCol + 8, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = _sDateRange; cell.Style.Font.Bold = false;
                        cell.Style.WrapText = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        if (sExportLCType != "")
                        {
                            cell = sheet.Cells[nRowIndex, nStartCol + 8, nRowIndex, nEndCol]; cell.Merge = true;
                            cell.Value = "L/C Type :" + sExportLCType; cell.Style.Font.Bold = true;
                            cell.Style.WrapText = true;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            nRowIndex = nRowIndex + 1;

                        }

                        #region Report Data

                        #region Blank
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "BTB LC No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Issue Bank"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "NEGO. Bank"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Party Name"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "P/I No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Commodity"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Bill Amount"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Expire Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "C/P"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex++;
                        #endregion

                        #region Data
                        int nExportBillID = 0;
                        //_oExportBillReports = _oExportBillReports.OrderBy(x => x.ImportLCNo).ThenBy(x => x.ExportBillNo).ToList();
                        double nTotalAmount = 0;
                        foreach (ExportBillReport oItem in _oExportBillReports)
                        {
                            nColumn = 1;
                            if (nExportBillID != oItem.ExportBillID)
                            {
                                nCount++;
                                int nEndRow = (_oExportBillReports.Where(PIR => PIR.ExportBillID == oItem.ExportBillID).ToList().Count) + nRowIndex - 1;
                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                                cell.Merge = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.ExportLCNo.ToString(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.BankName_Issue.ToString(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.BankName_Nego.ToString(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.ApplicantName.ToString(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nTotalAmount = nTotalAmount + oItem.Amount;
                            }
                            nColumn = 6;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "# #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = (oItem.Qty * oItem.UnitPrice); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "$ #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            if (nExportBillID != oItem.ExportBillID)
                            {
                                int nEndRow = (_oExportBillReports.Where(PIR => PIR.ExportBillID == oItem.ExportBillID).ToList().Count) + nRowIndex - 1;
                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "$ #,##0.00";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.ShipmentDateSt; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.ExpiryDateSt; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.StateSt; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.MKTPName; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            nExportBillID = oItem.ExportBillID;
                            nRowIndex++;
                        }
                        #endregion

                        #region Total

                        int nMergeCol = 7;
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - nMergeCol--]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        double nTotalQty = _oExportBillReports.Select(c => c.Qty).Sum();
                        cell = sheet.Cells[nRowIndex, nEndCol - nMergeCol--]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nTotalAmount = _oExportBillReports.Select(c => c.Qty*c.UnitPrice).Sum();
                        cell = sheet.Cells[nRowIndex, nEndCol - nMergeCol--]; cell.Value = nTotalAmount; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nTotalAmount = _oExportBillReports.GroupBy(p => p.ExportBillID).Select(grp => new ExportBillReport 
                        {
                            Amount=grp.First().Amount
                        }).Sum(x=>x.Amount);
                        cell = sheet.Cells[nRowIndex, nEndCol - nMergeCol--]; cell.Value = nTotalAmount; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        while (nMergeCol>=0)
                        {
                            cell = sheet.Cells[nRowIndex, nEndCol - nMergeCol--]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        nRowIndex = nRowIndex + 1;
                        #endregion
                        #endregion

                        cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=ExportBillRegister_BackToBack.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
                if (nType == 2)//SendToBank
                {
                    #region Print XL

                    int nRowIndex = 2, nStartCol = 2, nEndCol = 15, nColumn = 1, nCount = 0;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("ExportBillRegister");
                        sheet.Name = "Export Bill Register";
                        sheet.Column(++nColumn).Width = 10; //SL No
                        sheet.Column(++nColumn).Width = 15; //LC No
                        sheet.Column(++nColumn).Width = 45; //Issue Bank
                        sheet.Column(++nColumn).Width = 20; //NgoBank
                        sheet.Column(++nColumn).Width = 15; //LDBCDate
                        sheet.Column(++nColumn).Width = 30; //Bank Rcv Date
                        sheet.Column(++nColumn).Width = 30; //Applicant
                        sheet.Column(++nColumn).Width = 15; //PINO
                        sheet.Column(++nColumn).Width = 30; //Commodity
                        sheet.Column(++nColumn).Width = 15; //Qty
                        sheet.Column(++nColumn).Width = 15; //Amount
                        sheet.Column(++nColumn).Width = 15; //Amount
                        sheet.Column(++nColumn).Width = 15; //State
                        sheet.Column(++nColumn).Width = 25; //CP
                        //   nEndCol = 12;

                        #region Report Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 7]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nStartCol + 8, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Export Bill Register (Send To Bank)"; cell.Style.Font.Bold = true;
                        cell.Style.WrapText = true;
                        cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region Address & Date
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 7]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nStartCol + 8, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = _sDateRange; cell.Style.Font.Bold = false;
                        cell.Style.WrapText = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion


                        #region Report Data

                        #region Blank
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "BTB LC No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Issue Bank & Branch"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LDBC No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LDBC Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Bank Rcv Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Party Name"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "P/I No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Commodity"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Bill Amount"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "C/P"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex++;
                        #endregion

                        #region Data
                        int nExportBillID = 0;
                        //_oExportBillReports = _oExportBillReports.OrderBy(x => x.ImportLCNo).ThenBy(x => x.ExportBillNo).ToList();
                        double nTotalAmount = 0;
                        foreach (ExportBillReport oItem in _oExportBillReports)
                        {
                            nColumn = 1;

                            if (nExportBillID != oItem.ExportBillID)
                            {
                                nCount++;
                                int nEndRow = (_oExportBillReports.Where(PIR => PIR.ExportBillID == oItem.ExportBillID).ToList().Count) + nRowIndex - 1;
                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false; cell.Merge = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.ExportLCNo.ToString(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.BankName_Issue+" ["+oItem.BBranchName_Issue+"]"; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.LDBCNo.ToString(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.LDBCDateSt.ToString(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.RecedFromBankDateSt.ToString(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.ApplicantName.ToString(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nTotalAmount = nTotalAmount + oItem.Amount;
                            }
                            nColumn = 8;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "# #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty*oItem.UnitPrice; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "$ #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            if (nExportBillID != oItem.ExportBillID)
                            {
                                int nEndRow = (_oExportBillReports.Where(PIR => PIR.ExportBillID == oItem.ExportBillID).ToList().Count) + nRowIndex - 1;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "$ #,##0.00";
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.StateSt; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.MKTPName; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            nExportBillID = oItem.ExportBillID;
                            nRowIndex++;
                        }
                        #endregion

                        #region Total
                        int nMergeCol = 5;
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - nMergeCol--]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        double nTotalQty = _oExportBillReports.Select(c => c.Qty).Sum();
                        cell = sheet.Cells[nRowIndex, nEndCol - nMergeCol--]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nTotalAmount = _oExportBillReports.Select(c => c.Qty*c.UnitPrice).Sum();
                        cell = sheet.Cells[nRowIndex, nEndCol - nMergeCol--]; cell.Value = nTotalAmount; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        double nGrandTotal = _oExportBillReports.GroupBy(p => p.ExportBillID).Select(grp => new ExportBillReport
                        {
                            Amount = grp.First().Amount
                        }).Sum(x => x.Amount);
                        cell = sheet.Cells[nRowIndex, nEndCol - nMergeCol--]; cell.Value = nGrandTotal; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        while (nMergeCol >= 0)
                        {
                            cell = sheet.Cells[nRowIndex, nEndCol - nMergeCol--]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        nRowIndex = nRowIndex + 1;
                        #endregion
                        #endregion

                        cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=ExportBillRegister_SendToBank.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
                if (nType == 3)//RcvFromBank
                {
                    #region Print XL

                    int nRowIndex = 2, nStartCol = 2, nEndCol = 15, nColumn = 1, nCount = 0;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("ExportBillRegister");
                        sheet.Name = "Export Bill Register";
                        sheet.Column(++nColumn).Width = 10; //SL No
                        sheet.Column(++nColumn).Width = 15; //LC No
                        sheet.Column(++nColumn).Width = 25; //Issue Bank
                        sheet.Column(++nColumn).Width = 20; //NgoBank
                        sheet.Column(++nColumn).Width = 30; //PartyName
                        sheet.Column(++nColumn).Width = 15; //PINO
                        sheet.Column(++nColumn).Width = 30; //Commodity
                        sheet.Column(++nColumn).Width = 15; //Qty
                        sheet.Column(++nColumn).Width = 15; //Amount
                        sheet.Column(++nColumn).Width = 15; //Amount
                        sheet.Column(++nColumn).Width = 15; //MaturityRcv
                        sheet.Column(++nColumn).Width = 15; //Maturity
                        sheet.Column(++nColumn).Width = 15; //State
                        sheet.Column(++nColumn).Width = 25; //CP
                        //   nEndCol = 13;

                        #region Report Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 7]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nStartCol + 8, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Export Bill Register (Recceived From Bank)"; cell.Style.Font.Bold = true;
                        cell.Style.WrapText = true;
                        cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region Address & Date
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 7]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nStartCol + 8, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = _sDateRange; cell.Style.Font.Bold = false;
                        cell.Style.WrapText = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion


                        #region Report Data

                        #region Blank
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "BTB LC No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Issue Bank"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "NEGO. Bank"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Party Name"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "P/I No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Commodity"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Bill Amount"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Maturity Rcv"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Maturity DT"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "C/P"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex++;
                        #endregion

                        #region Data
                        int nExportBillID = 0;
                        double nTotalAmount = 0;
                        foreach (ExportBillReport oItem in _oExportBillReports)
                        {
                            nColumn = 1;

                            if (nExportBillID != oItem.ExportBillID)
                            {
                                nCount++;
                                int nEndRow = (_oExportBillReports.Where(PIR => PIR.ExportBillID == oItem.ExportBillID).ToList().Count) + nRowIndex - 1;
                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                                cell.Merge = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.ExportLCNo.ToString(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.BankName_Issue.ToString(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.LDBCNo.ToString(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                try { cell.Merge = true; }
                                catch (Exception ex) { }
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.ApplicantName.ToString(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nTotalAmount = nTotalAmount + oItem.Amount;
                            }
                            nColumn = 6;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "# #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty*oItem.UnitPrice; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "$ #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            if (nExportBillID != oItem.ExportBillID)
                            {
                                int nEndRow = (_oExportBillReports.Where(PIR => PIR.ExportBillID == oItem.ExportBillID).ToList().Count) + nRowIndex - 1;
                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.Numberformat.Format = "$ #,##0.00";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.MaturityReceivedDateSt; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.MaturityDateSt; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.StateSt; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.MKTPName; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            nExportBillID = oItem.ExportBillID;
                            nRowIndex++;
                        }
                        #endregion

                        #region Total
                        int nMergeCol = 7;
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - nMergeCol--]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        double nTotalQty = _oExportBillReports.Select(c => c.Qty).Sum();
                        cell = sheet.Cells[nRowIndex, nEndCol - nMergeCol--]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nTotalQty = _oExportBillReports.Select(c => c.Qty*c.UnitPrice).Sum();
                        cell = sheet.Cells[nRowIndex, nEndCol - nMergeCol--]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nTotalAmount = _oExportBillReports.GroupBy(p => p.ExportBillID).Select(grp => new ExportBillReport
                        {
                            Amount = grp.First().Amount
                        }).Sum(x => x.Amount);
                        cell = sheet.Cells[nRowIndex, nEndCol - nMergeCol--]; cell.Value = nTotalAmount; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        while (nMergeCol >= 0)
                        {
                            cell = sheet.Cells[nRowIndex, nEndCol - nMergeCol--]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        nRowIndex = nRowIndex + 1;
                        #endregion
                        #endregion

                        cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=ExportBillRegister_RcvFromBank.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
            }
        }
        #endregion

        #region Support Functions
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
    }
}

