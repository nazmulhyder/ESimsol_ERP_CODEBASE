using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class TrailBalanceController : Controller
    {
        #region Declaration
        TrailBalance _oTrailBalance = new TrailBalance();
        List<TrailBalance> _oTrailBalances = new List<TrailBalance>();
        TTrailBalance _oTTrailBalance = new TTrailBalance();
        List<TTrailBalance> _oTTrailBalances = new List<TTrailBalance>();

        List<SP_GeneralLedger> _oSP_GeneralLedgers = new List<SP_GeneralLedger>();
        SP_GeneralLedger _oSP_GeneralLedger = new SP_GeneralLedger();
        string _sFormatter = "";
        string _sErrorMesage = "";
        #endregion

        public ActionResult ViewTrailBalance(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TrailBalance).ToString(), (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID));
            _oTrailBalance = new TrailBalance();
            _oTrailBalance.AccountingYears = AccountingSession.GetsAccountingYears(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oTrailBalance);
        }
        public ActionResult ViewTrailBalanceDateRange(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            Company oCompany = new Company();
            _oTTrailBalance = new TTrailBalance();
            _oTTrailBalance.Company = oCompany.Get((int)Session[SessionInfo.CurrentCompanyID], ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oTTrailBalance.AccountTypeObjs = EnumObject.jGets(typeof(EnumAccountType));

            #region Business Unit
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            #endregion

            return View(_oTTrailBalance);
        }
        #region PrintTrialBalanceInXL
        private List<TTrailBalance> GetChild(int nAccountHeadID)
        {
            List<TTrailBalance> oTTrailBalances = new List<TTrailBalance>();
            foreach (TTrailBalance oItem in _oTTrailBalances)
            {
                if (oItem.ParentHeadID == nAccountHeadID)
                {
                    oTTrailBalances.Add(oItem);
                }
            }
            return oTTrailBalances;
        }
        private List<TTrailBalance> GetRoot()
        {
            List<TTrailBalance> oTTrailBalances = new List<TTrailBalance>();
            foreach (TTrailBalance oItem in _oTTrailBalances)
            {
                if (oItem.ParentHeadID == 1)
                {
                    oTTrailBalances.Add(oItem);
                }
            }            
            return oTTrailBalances;
        }

        private void AddTreeNodes(ref TTrailBalance oTTrailBalance)
        {
            List<TTrailBalance> oChildNodes;
            oChildNodes = GetChild(oTTrailBalance.AccountHeadID);
            oTTrailBalance.children = oChildNodes;

            foreach (TTrailBalance oItem in oChildNodes)
            {
                oTTrailBalance.state = "closed";
                TTrailBalance oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }
        #endregion

        private void DeconstructTree(List<TTrailBalance> oTTrailBalances)
        {
            foreach (TTrailBalance oItem in oTTrailBalances)
            {
                _oTrailBalance = new TrailBalance();
                _oTrailBalance.AccountHeadID = oItem.AccountHeadID;
                _oTrailBalance.ParentAccountHeadID = oItem.ParentHeadID;
                _oTrailBalance.AccountHeadName = oItem.AccountHeadName;
                _oTrailBalance.AccountCode = oItem.AccountCode;
                _oTrailBalance.OpenningBalance = oItem.OpenningBalanceDbl;
                _oTrailBalance.ClosingBalance = oItem.ClosingBalanceDbl;
                _oTrailBalance.DebitAmount = oItem.DebitAmountDbl;
                _oTrailBalance.CreditAmount = oItem.CreditAmountDbl;
                _oTrailBalance.AccountType = oItem.AccountType;
                _oTrailBalance.ComponentType = oItem.ComponentType;
                _oTrailBalances.Add(_oTrailBalance);

                if (oItem.children != null && oItem.children.Count > 0)
                {
                    DeconstructTree(oItem.children);
                }
            }
        }

        [HttpPost]
        public JsonResult RefreshTrailBalByDateRange(TrailBalance oTB)
        {
            _oTrailBalances = new List<TrailBalance>();
            _oTrailBalance = new TrailBalance();
            _oTTrailBalance = new TTrailBalance();
            _oTTrailBalances = new List<TTrailBalance>();
            List<TTrailBalance> oTempTTrailBalances = new List<TTrailBalance>();
            List<TTrailBalance> oTTrailBalances = new List<TTrailBalance>();

            try
            {
                if (oTB.DateType == "EqualTo")
                {
                    oTB.EndDate = oTB.StartDate;
                }
                _oTrailBalances = TrailBalance.Gets(0, oTB.AccountTypeInInt, oTB.StartDate, oTB.EndDate, oTB.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (TrailBalance oItem in _oTrailBalances)
                {
                    _oTTrailBalance = new TTrailBalance();
                    _oTTrailBalance.AccountHeadID = oItem.AccountHeadID;
                    _oTTrailBalance.ParentHeadID = oItem.ParentAccountHeadID;
                    _oTTrailBalance.AccountHeadName = oItem.AccountHeadName;
                    _oTTrailBalance.AccountCode = oItem.AccountCode;
                    _oTTrailBalance.OpenningBalance = oItem.OpeningBalanceSt;
                    _oTTrailBalance.ClosingBalance = oItem.ClosingBalanceSt;
                    _oTTrailBalance.DebitAmount = oItem.DebitAmountInString;
                    _oTTrailBalance.CreditAmount = oItem.CreditAmountInString;
                    _oTTrailBalance.AccountTypeInString = oItem.AccountType.ToString();
                    _oTTrailBalances.Add(_oTTrailBalance);
                }
                _oTTrailBalance = new TTrailBalance();                
                oTTrailBalances = new List<TTrailBalance>();
                oTempTTrailBalances = new List<TTrailBalance>();

                oTempTTrailBalances = GetRoot();
                foreach (TTrailBalance oItem in oTempTTrailBalances)
                {
                    TTrailBalance oTTrailBalance = new TTrailBalance();
                    oTTrailBalance = oItem;
                    oTTrailBalance.state = "closed";
                    this.AddTreeNodes(ref  oTTrailBalance);
                    oTTrailBalances.Add(oTTrailBalance);
                }
            }
            catch (Exception ex)
            {
                oTTrailBalances = new List<TTrailBalance>();
                _oTTrailBalance=new TTrailBalance();
                _oTTrailBalance.ErrorMessage = ex.Message;
                oTTrailBalances.Add(_oTTrailBalance);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTTrailBalances);
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

        #region PrintTrialBalanceInXL
        public ActionResult PrintTrialBalanceInXL(string sTempString)
        {
            _oTrailBalances = new List<TrailBalance>();

            DateTime dStartDate = Convert.ToDateTime(sTempString.Split('~')[0]);
            DateTime dEndDate = Convert.ToDateTime(sTempString.Split('~')[1]);
            int nAccountType = Convert.ToInt32(sTempString.Split('~')[2]);
            int nAccountHeadID = Convert.ToInt32(sTempString.Split('~')[3]);
            int nBusinessUnitID = Convert.ToInt32(sTempString.Split('~')[4]);


            _oTrailBalances = TrailBalance.Gets(nAccountHeadID, nAccountType, dStartDate, dEndDate, nBusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<TrailBalanceXL>));

            //We load the data

            int nCount = 0; double nTotalDebit = 0; double nTotalCredit = 0; double nTotalOpeningBalance = 0; double nTotalClosingBalance = 0;
            TrailBalanceXL oTrailBalanceXL = new TrailBalanceXL();
            List<TrailBalanceXL> oTrailBalanceXLs = new List<TrailBalanceXL>();
            foreach (TrailBalance oItem in _oTrailBalances)
            {
                nCount++;
                oTrailBalanceXL = new TrailBalanceXL();
                oTrailBalanceXL.SLNo = nCount.ToString();
                oTrailBalanceXL.AccountCode = oItem.AccountCode;
                oTrailBalanceXL.AccountHeadName = oItem.AccountHeadName;
                oTrailBalanceXL.ComponentType = oItem.ComponentType.ToString();
                oTrailBalanceXL.OpeningBalance = oItem.OpenningBalance;
                oTrailBalanceXL.DebitAmount = oItem.DebitAmount;
                oTrailBalanceXL.CreditAmount = oItem.CreditAmount;
                oTrailBalanceXL.ClosingBalance = oItem.ClosingBalance;
                oTrailBalanceXLs.Add(oTrailBalanceXL);
                nTotalOpeningBalance = nTotalOpeningBalance + oItem.OpenningBalance;
                nTotalDebit = nTotalDebit + oItem.DebitAmount;
                nTotalCredit = nTotalCredit + oItem.CreditAmount;
                nTotalClosingBalance = nTotalClosingBalance + oItem.ClosingBalance;

            }

            #region Total
            oTrailBalanceXL = new TrailBalanceXL();
            oTrailBalanceXL.SLNo = "";
            oTrailBalanceXL.AccountCode = "";
            oTrailBalanceXL.AccountHeadName = "";
            oTrailBalanceXL.ComponentType = "Total";
            oTrailBalanceXL.OpeningBalance = nTotalOpeningBalance;
            oTrailBalanceXL.DebitAmount = nTotalDebit;
            oTrailBalanceXL.CreditAmount = nTotalCredit;
            oTrailBalanceXL.ClosingBalance = nTotalClosingBalance;
            oTrailBalanceXLs.Add(oTrailBalanceXL);

            #endregion
            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oTrailBalanceXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "Trial Balance.xls");
        }
        #endregion
        public ActionResult PrintTrialBal_DateRange(string sTempString)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();
            
            string sDateRange = "";
            int nDateType = Convert.ToInt32(sTempString.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(sTempString.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sTempString.Split('~')[2]);
            int nAccountType = Convert.ToInt32(sTempString.Split('~')[3]);
            int nAccountHead = Convert.ToInt32(sTempString.Split('~')[4]);
            int nBusinessUnitID = Convert.ToInt32(sTempString.Split('~')[5]);

            if (nDateType == 1)
            {
                dEndDate = dStartDate.AddDays(1);
                sDateRange = "Date on " + dStartDate.ToString("dd MMM yyyy");
            }
            else
            {
                sDateRange = dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy");                
            }
            Company oCompany = new Company();
            _oTrailBalances = new List<TrailBalance>();
            _oTrailBalance = new TrailBalance();
            _oTrailBalance.StartDate = dStartDate;
            _oTrailBalance.EndDate = dEndDate;
            _oTrailBalance.AccountTypeInInt = nAccountType;
            _oTrailBalance.BusinessUnitID = nBusinessUnitID;

            try
            {
                _oTrailBalances = TrailBalance.Gets(nAccountHead, _oTrailBalance.AccountTypeInInt, Convert.ToDateTime(_oTrailBalance.StartDate.ToString("dd MMM yyyy")), Convert.ToDateTime(_oTrailBalance.EndDate), _oTrailBalance.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                oBusinessUnit = oBusinessUnit.Get(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]);

                foreach (TrailBalance oItem in _oTrailBalances)
                {
                    _oTTrailBalance = new TTrailBalance();
                    _oTTrailBalance.AccountHeadID = oItem.AccountHeadID;
                    _oTTrailBalance.ParentHeadID = oItem.ParentAccountHeadID;
                    _oTTrailBalance.AccountHeadName = oItem.AccountHeadName;
                    _oTTrailBalance.AccountCode = oItem.AccountCode;
                    _oTTrailBalance.OpenningBalanceDbl = oItem.OpenningBalance;
                    _oTTrailBalance.ClosingBalanceDbl = oItem.ClosingBalance;
                    _oTTrailBalance.DebitAmountDbl = oItem.DebitAmount;
                    _oTTrailBalance.CreditAmountDbl = oItem.CreditAmount;
                    _oTTrailBalance.AccountType = oItem.AccountType;
                    _oTTrailBalance.ComponentType = oItem.ComponentType;
                    _oTTrailBalances.Add(_oTTrailBalance);
                }
                List<TTrailBalance> oTTrailBalances = new List<TTrailBalance>();
                List<TTrailBalance> oTempTTrailBalances = new List<TTrailBalance>();

                oTempTTrailBalances = GetRoot();
                _oTrailBalances = new List<TrailBalance>();
                foreach (TTrailBalance oItem in oTempTTrailBalances)
                {
                    TTrailBalance oTTrailBalance = new TTrailBalance();
                    oTTrailBalance = oItem;
                    this.AddTreeNodes(ref  oTTrailBalance);
                    oTTrailBalances.Add(oTTrailBalance);
                }

                DeconstructTree(oTTrailBalances);
            }
            catch (Exception ex)
            {
                _oTTrailBalance.ErrorMessage = ex.Message;
            }
            rptTrailBalances_DateRange oReport = new rptTrailBalances_DateRange();
            byte[] abytes = oReport.PrepareReport(_oTrailBalances, oCompany, sDateRange, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintShortTrialBal_DateRange(string sTempString)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();

            string sDateRange = "";
            int nDateType = Convert.ToInt32(sTempString.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(sTempString.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sTempString.Split('~')[2]);
            int nAccountType = Convert.ToInt32(sTempString.Split('~')[3]);
            int nAccountHead = Convert.ToInt32(sTempString.Split('~')[4]);
            int nBusinessUnitID = Convert.ToInt32(sTempString.Split('~')[5]);

            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            if (nDateType == 1)
            {
                dEndDate = dStartDate.AddDays(1);
                sDateRange = "Date on " + dStartDate.ToString("dd MMM yyyy");
            }
            else
            {
                sDateRange = dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy");
            }
            Company oCompany = new Company();
            _oTrailBalances = new List<TrailBalance>();
            _oTrailBalance = new TrailBalance();
            _oTrailBalance.StartDate = dStartDate;
            _oTrailBalance.EndDate = dEndDate;
            _oTrailBalance.AccountTypeInInt = nAccountType;
            _oTrailBalance.BusinessUnitID = nBusinessUnitID;

            try
            {
                _oTrailBalances = TrailBalance.Gets(nAccountHead, _oTrailBalance.AccountTypeInInt, Convert.ToDateTime(_oTrailBalance.StartDate.ToString("dd MMM yyyy")), Convert.ToDateTime(_oTrailBalance.EndDate), _oTrailBalance.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                oBusinessUnit = oBusinessUnit.Get(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]);


                foreach (TrailBalance oItem in _oTrailBalances)
                {
                    _oTTrailBalance = new TTrailBalance();
                    _oTTrailBalance.AccountHeadID = oItem.AccountHeadID;
                    _oTTrailBalance.ParentHeadID = oItem.ParentAccountHeadID;
                    _oTTrailBalance.AccountHeadName = oItem.AccountHeadName;
                    _oTTrailBalance.AccountCode = oItem.AccountCode;
                    _oTTrailBalance.OpenningBalanceDbl = oItem.OpenningBalance;
                    _oTTrailBalance.ClosingBalanceDbl = oItem.ClosingBalance;
                    _oTTrailBalance.DebitAmountDbl = oItem.DebitAmount;
                    _oTTrailBalance.CreditAmountDbl = oItem.CreditAmount;
                    _oTTrailBalance.AccountType = oItem.AccountType;
                    _oTTrailBalance.ComponentType = oItem.ComponentType;
                    _oTTrailBalances.Add(_oTTrailBalance);
                }
                List<TTrailBalance> oTTrailBalances = new List<TTrailBalance>();
                List<TTrailBalance> oTempTTrailBalances = new List<TTrailBalance>();

                oTempTTrailBalances = GetRoot();
                _oTrailBalances = new List<TrailBalance>();
                foreach (TTrailBalance oItem in oTempTTrailBalances)
                {
                    TTrailBalance oTTrailBalance = new TTrailBalance();
                    oTTrailBalance = oItem;
                    this.AddTreeNodes(ref  oTTrailBalance);
                    oTTrailBalances.Add(oTTrailBalance);
                }

                DeconstructTree(oTTrailBalances);

            }
            catch (Exception ex)
            {
                _oTTrailBalance.ErrorMessage = ex.Message;
            }
            rptTrailBalances_DateRange oReport = new rptTrailBalances_DateRange();
            byte[] abytes = oReport.PrepareReportShort(_oTrailBalances, oCompany, sDateRange, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintTrialBal_Ledger(string sTempString)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();

            string sDateRange = "";
            int nDateType = Convert.ToInt32(sTempString.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(sTempString.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sTempString.Split('~')[2]);
            int nAccountType = Convert.ToInt32(sTempString.Split('~')[3]);
            int nAccountHead = Convert.ToInt32(sTempString.Split('~')[4]);
            int nBusinessUnitID = Convert.ToInt32(sTempString.Split('~')[5]);

            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            if (nDateType == 1)
            {
                dEndDate = dStartDate.AddDays(1);
                sDateRange = "Date on " + dStartDate.ToString("dd MMM yyyy");
            }
            else
            {
                sDateRange = dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy");
            }
            Company oCompany = new Company();
            _oTrailBalances = new List<TrailBalance>();
            _oTrailBalance = new TrailBalance();
            _oTrailBalance.StartDate = dStartDate;
            _oTrailBalance.EndDate = dEndDate;
            _oTrailBalance.AccountTypeInInt = nAccountType;
            _oTrailBalance.BusinessUnitID = nBusinessUnitID;

            try
            {
                _oTrailBalances = TrailBalance.Gets(nAccountHead, _oTrailBalance.AccountTypeInInt, Convert.ToDateTime(_oTrailBalance.StartDate.ToString("dd MMM yyyy")), Convert.ToDateTime(_oTrailBalance.EndDate), _oTrailBalance.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                oBusinessUnit = oBusinessUnit.Get(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]);


                foreach (TrailBalance oItem in _oTrailBalances)
                {
                    _oTTrailBalance = new TTrailBalance();
                    _oTTrailBalance.AccountHeadID = oItem.AccountHeadID;
                    _oTTrailBalance.ParentHeadID = oItem.ParentAccountHeadID;
                    _oTTrailBalance.AccountHeadName = oItem.AccountHeadName;
                    _oTTrailBalance.AccountCode = oItem.AccountCode;
                    _oTTrailBalance.OpenningBalanceDbl = oItem.OpenningBalance;
                    _oTTrailBalance.ClosingBalanceDbl = oItem.ClosingBalance;
                    _oTTrailBalance.DebitAmountDbl = oItem.DebitAmount;
                    _oTTrailBalance.CreditAmountDbl = oItem.CreditAmount;
                    _oTTrailBalance.AccountType = oItem.AccountType;
                    _oTTrailBalance.ComponentType = oItem.ComponentType;
                    _oTTrailBalances.Add(_oTTrailBalance);
                }
                List<TTrailBalance> oTTrailBalances = new List<TTrailBalance>();
                List<TTrailBalance> oTempTTrailBalances = new List<TTrailBalance>();

                oTempTTrailBalances = GetRoot();
                _oTrailBalances = new List<TrailBalance>();
                foreach (TTrailBalance oItem in oTempTTrailBalances)
                {
                    TTrailBalance oTTrailBalance = new TTrailBalance();
                    oTTrailBalance = oItem;
                    this.AddTreeNodes(ref  oTTrailBalance);
                    oTTrailBalances.Add(oTTrailBalance);
                }

                DeconstructTree(oTTrailBalances);

            }
            catch (Exception ex)
            {
                _oTTrailBalance.ErrorMessage = ex.Message;
            }
            rptTrailBalances_DateRange oReport = new rptTrailBalances_DateRange();
            byte[] abytes = oReport.PrepareReport_Ledger(_oTrailBalances, oCompany, sDateRange, oBusinessUnit);
            return File(abytes, "application/pdf");
        }

        #region Excel Helper
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber)
        {
            return FillCell(sheet, nRowIndex, nStartCol, sVal, IsNumber, false);
        }
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber, bool IsBold)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[nRowIndex, nStartCol];
            if (IsNumber)
            {
                if (Convert.ToDouble(sVal) == 0)
                    cell.Value = "";
                else
                    cell.Value = Convert.ToDouble(sVal);
            }
            else
                cell.Value = sVal;
            cell.Style.Font.Bold = IsBold;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            if (IsNumber)
            {
                cell.Style.Numberformat.Format = _sFormatter;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            return cell;
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, string sVal)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            cell.Merge = true;
            cell.Value = sVal;
            cell.Style.Font.Bold = false;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex)
        {
            FillCellMerge(ref sheet, sVal, startRowIndex, endRowIndex, startColIndex, endColIndex, false, ExcelHorizontalAlignment.Left);
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool isBold, ExcelHorizontalAlignment HoriAlign)
        {
            FillCellMerge(ref sheet, sVal, startRowIndex, endRowIndex, startColIndex, endColIndex, isBold, HoriAlign, true);
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool isBold, ExcelHorizontalAlignment HoriAlign, bool isLeftBorder)
        {
            FillCellMerge(ref sheet, sVal, startRowIndex, endRowIndex, startColIndex, endColIndex, isBold, HoriAlign, isLeftBorder, true);
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool isBold, ExcelHorizontalAlignment HoriAlign, bool isLeftBorder, bool isRightBorder)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            cell.Merge = true;
            cell.Value = sVal;
            cell.Style.Font.Bold = isBold;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = HoriAlign;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            
            if (!isLeftBorder)border.Left.Style = ExcelBorderStyle.None;
            if (!isRightBorder)border.Right.Style = ExcelBorderStyle.None;
        }

        #endregion
        public void ExportToExcel_TrailBal(string sTempString)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();

            #region Get Data
            string sDateRange = ""; _sFormatter = " #,##0.00;(#,##0.00)";
            int nDateType = Convert.ToInt32(sTempString.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(sTempString.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sTempString.Split('~')[2]);
            int nAccountType = Convert.ToInt32(sTempString.Split('~')[3]);
            int nAccountHead = Convert.ToInt32(sTempString.Split('~')[4]);
            int nBusinessUnitID = Convert.ToInt32(sTempString.Split('~')[5]);

            if (nDateType == 1)
            {
                dEndDate = dStartDate.AddDays(1);
                sDateRange = "Date on " + dStartDate.ToString("dd MMM yyyy");
            }
            else
            {
                sDateRange = dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy");
            }
            Company oCompany = new Company();
            _oTrailBalances = new List<TrailBalance>();
            _oTrailBalance = new TrailBalance();
            _oTrailBalance.StartDate = dStartDate;
            _oTrailBalance.EndDate = dEndDate;
            _oTrailBalance.AccountTypeInInt = nAccountType;
            _oTrailBalance.BusinessUnitID = nBusinessUnitID;

            try
            {
                _oTrailBalances = TrailBalance.Gets(nAccountHead, _oTrailBalance.AccountTypeInInt, Convert.ToDateTime(_oTrailBalance.StartDate.ToString("dd MMM yyyy")), Convert.ToDateTime(_oTrailBalance.EndDate), _oTrailBalance.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                oBusinessUnit = oBusinessUnit.Get(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]);

                foreach (TrailBalance oItem in _oTrailBalances)
                {
                    _oTTrailBalance = new TTrailBalance();
                    _oTTrailBalance.AccountHeadID = oItem.AccountHeadID;
                    _oTTrailBalance.ParentHeadID = oItem.ParentAccountHeadID;
                    _oTTrailBalance.AccountHeadName = oItem.AccountHeadName;
                    _oTTrailBalance.AccountCode = oItem.AccountCode;
                    _oTTrailBalance.OpenningBalanceDbl = oItem.OpenningBalance;
                    _oTTrailBalance.ClosingBalanceDbl = oItem.ClosingBalance;
                    _oTTrailBalance.DebitAmountDbl = oItem.DebitAmount;
                    _oTTrailBalance.CreditAmountDbl = oItem.CreditAmount;
                    _oTTrailBalance.AccountType = oItem.AccountType;
                    _oTTrailBalance.ComponentType = oItem.ComponentType;
                    _oTTrailBalances.Add(_oTTrailBalance);
                }
                List<TTrailBalance> oTTrailBalances = new List<TTrailBalance>();
                List<TTrailBalance> oTempTTrailBalances = new List<TTrailBalance>();

                oTempTTrailBalances = GetRoot();
                _oTrailBalances = new List<TrailBalance>();
                foreach (TTrailBalance oItem in oTempTTrailBalances)
                {
                    TTrailBalance oTTrailBalance = new TTrailBalance();
                    oTTrailBalance = oItem;
                    this.AddTreeNodes(ref  oTTrailBalance);
                    oTTrailBalances.Add(oTTrailBalance);
                }

                DeconstructTree(oTTrailBalances);
            }
            catch (Exception ex)
            {
                _sErrorMesage = ex.Message;
            }
            #endregion

            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();

                table_header.Add(new TableHeader { Header = "Code", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "", Width = 3f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "", Width = 3f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "", Width = 3f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "", Width = 3f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Particulars", Width = 35f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Type", Width = 13f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Opening Balance", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Debit Activity", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Credit Activity", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Closing Balance", Width = 20f, IsRotate = false });

                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Trial Balance");
                    sheet.Name = "Trial Balance";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Trial Balance"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = sDateRange; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    nStartCol = 1;
                    foreach (TableHeader listItem in table_header)
                    {
                        if (string.IsNullOrEmpty(listItem.Header)) continue;

                        if (listItem.Header.Equals("Particulars"))
                            cell = sheet.Cells[nRowIndex, ++nStartCol, nRowIndex, nStartCol += 4];
                        else cell = sheet.Cells[nRowIndex, ++nStartCol];

                        cell.Value = listItem.Header; cell.Style.Font.Bold = true; cell.Merge = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nRowIndex++;

                    string sCurrencySymbol = ""; int nCount = 0;
                    #region Data

                    foreach (TrailBalance oItem in _oTrailBalances)
                    {
                        nStartCol = 1;
                        if (oItem.OpenningBalance != 0 || oItem.DebitAmount != 0 || oItem.CreditAmount != 0 || oItem.ClosingBalance != 0)
                        {
                            nCount++;


                            if (oItem.AccountType == EnumAccountType.Component)
                            {
                                if (oItem.AccountHeadID != 1)
                                {
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountCode, false, true);
                                    FillCellMerge(ref sheet, oItem.AccountHeadName, nRowIndex, nRowIndex, ++nStartCol, nStartCol += 4, true, ExcelHorizontalAlignment.Left);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountTypeSt, false, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.OpenningBalance.ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.DebitAmount.ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.CreditAmount.ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.ClosingBalance.ToString(), true, true);
                                }
                            }
                            else
                            {
                                if (oItem.AccountType == EnumAccountType.Segment)
                                {
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountCode, false, true);
                                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, ++nStartCol, nStartCol, false, ExcelHorizontalAlignment.Left,true,false);
                                    FillCellMerge(ref sheet, oItem.AccountHeadName, nRowIndex, nRowIndex, ++nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Left,false);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountTypeSt, false, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.OpenningBalance.ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.DebitAmount.ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.CreditAmount.ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.ClosingBalance.ToString(), true, true);
                                }
                                if (oItem.AccountType == EnumAccountType.Group)
                                {
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountCode, false, true);

                                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, ++nStartCol, nStartCol += 1, false, ExcelHorizontalAlignment.Left,true, false);
                                    FillCellMerge(ref sheet, oItem.AccountHeadName, nRowIndex, nRowIndex, ++nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Left,false);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountTypeSt, false, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.OpenningBalance.ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.DebitAmount.ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.CreditAmount.ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.ClosingBalance.ToString(), true, true);
                                }
                                if (oItem.AccountType == EnumAccountType.SubGroup)
                                {
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountCode, false, false);
                                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, ++nStartCol, nStartCol += 2, false, ExcelHorizontalAlignment.Left, true, false);
                                    FillCellMerge(ref sheet, oItem.AccountHeadName, nRowIndex, nRowIndex, ++nStartCol, nStartCol += 1, false, ExcelHorizontalAlignment.Left,false);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountTypeSt, false, false);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.OpenningBalance.ToString(), true, false);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.DebitAmount.ToString(), true, false);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.CreditAmount.ToString(), true, false);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.ClosingBalance.ToString(), true, false);
                                }
                                if (oItem.AccountType == EnumAccountType.Ledger)
                                {
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountCode, false, false);

                                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, ++nStartCol, nStartCol += 3, false, ExcelHorizontalAlignment.Left, true, false);
                                    FillCellMerge(ref sheet, oItem.AccountHeadName, nRowIndex, nRowIndex, ++nStartCol, nStartCol, false, ExcelHorizontalAlignment.Left,false);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountTypeSt, false, false);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.OpenningBalance.ToString(), true, false);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.DebitAmount.ToString(), true, false);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.CreditAmount.ToString(), true, false);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.ClosingBalance.ToString(), true, false);
                                }
                            }
                            nRowIndex++;
                        }
                    }
                    #region Grand Total
                    nStartCol = 2;
                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol, nStartCol += 7, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oTrailBalances.Where(x=>x.AccountType == EnumAccountType.Ledger).Sum(x => x.DebitAmount).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oTrailBalances.Where(x => x.AccountType == EnumAccountType.Ledger).Sum(x => x.CreditAmount).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);

                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=TrailBalance.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        public void ExportToExcel_ShortTrialBal(string sTempString)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();

            #region Get Data
            string sDateRange = ""; _sFormatter = " #,##0.00;(#,##0.00)";
            int nDateType = Convert.ToInt32(sTempString.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(sTempString.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sTempString.Split('~')[2]);
            int nAccountType = Convert.ToInt32(sTempString.Split('~')[3]);
            int nAccountHead = Convert.ToInt32(sTempString.Split('~')[4]);
            int nBusinessUnitID = Convert.ToInt32(sTempString.Split('~')[5]);

            if (nDateType == 1)
            {
                dEndDate = dStartDate.AddDays(1);
                sDateRange = "Date on " + dStartDate.ToString("dd MMM yyyy");
            }
            else
            {
                sDateRange = dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy");
            }
            Company oCompany = new Company();
            _oTrailBalances = new List<TrailBalance>();
            _oTrailBalance = new TrailBalance();
            _oTrailBalance.StartDate = dStartDate;
            _oTrailBalance.EndDate = dEndDate;
            _oTrailBalance.AccountTypeInInt = nAccountType;
            _oTrailBalance.BusinessUnitID = nBusinessUnitID;

            try
            {
                _oTrailBalances = TrailBalance.Gets(nAccountHead, _oTrailBalance.AccountTypeInInt, Convert.ToDateTime(_oTrailBalance.StartDate.ToString("dd MMM yyyy")), Convert.ToDateTime(_oTrailBalance.EndDate), _oTrailBalance.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                oBusinessUnit = oBusinessUnit.Get(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]);

                foreach (TrailBalance oItem in _oTrailBalances)
                {
                    _oTTrailBalance = new TTrailBalance();
                    _oTTrailBalance.AccountHeadID = oItem.AccountHeadID;
                    _oTTrailBalance.ParentHeadID = oItem.ParentAccountHeadID;
                    _oTTrailBalance.AccountHeadName = oItem.AccountHeadName;
                    _oTTrailBalance.AccountCode = oItem.AccountCode;
                    _oTTrailBalance.OpenningBalanceDbl = oItem.OpenningBalance;
                    _oTTrailBalance.ClosingBalanceDbl = oItem.ClosingBalance;
                    _oTTrailBalance.DebitAmountDbl = oItem.DebitAmount;
                    _oTTrailBalance.CreditAmountDbl = oItem.CreditAmount;
                    _oTTrailBalance.AccountType = oItem.AccountType;
                    _oTTrailBalance.ComponentType = oItem.ComponentType;
                    _oTTrailBalances.Add(_oTTrailBalance);
                }
                List<TTrailBalance> oTTrailBalances = new List<TTrailBalance>();
                List<TTrailBalance> oTempTTrailBalances = new List<TTrailBalance>();

                oTempTTrailBalances = GetRoot();
                _oTrailBalances = new List<TrailBalance>();
                foreach (TTrailBalance oItem in oTempTTrailBalances)
                {
                    TTrailBalance oTTrailBalance = new TTrailBalance();
                    oTTrailBalance = oItem;
                    this.AddTreeNodes(ref  oTTrailBalance);
                    oTTrailBalances.Add(oTTrailBalance);
                }

                DeconstructTree(oTTrailBalances);
            }
            catch (Exception ex)
            {
                _sErrorMesage = ex.Message;
            }
            #endregion

            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();

                table_header.Add(new TableHeader { Header = "Code", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "", Width = 3f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "", Width = 3f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "", Width = 3f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "", Width = 3f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Particulars", Width = 38f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Type", Width = 15f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Debit Balance", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Credit Balance", Width = 25f, IsRotate = false });

                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Trial Balance");
                    sheet.Name = "Trial Balance";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Trial Balance"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = sDateRange; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    nStartCol = 1;
                    foreach (TableHeader listItem in table_header)
                    {
                        if (string.IsNullOrEmpty(listItem.Header)) continue;

                        if (listItem.Header.Equals("Particulars"))
                            cell = sheet.Cells[nRowIndex, ++nStartCol, nRowIndex, nStartCol += 4];
                        else cell = sheet.Cells[nRowIndex, ++nStartCol];

                        cell.Value = listItem.Header; cell.Style.Font.Bold = true; cell.Merge = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nRowIndex++;

                    string sCurrencySymbol = ""; int nCount = 0;
                    #region Data
                    double nTotalDebitAmount = 0;
                    double nTotalCreditAmount = 0;
                    foreach (TrailBalance oItem in _oTrailBalances)
                    {
                        nStartCol = 1;
                        if (oItem.OpenningBalance != 0 || oItem.DebitAmount != 0 || oItem.CreditAmount != 0 || oItem.ClosingBalance != 0)
                        {
                            nCount++;

                            if (oItem.AccountType == EnumAccountType.Component)
                            {
                                if (oItem.AccountHeadID != 1)
                                {
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountCode, false, true);
                                    FillCellMerge(ref sheet, oItem.AccountHeadName, nRowIndex, nRowIndex, ++nStartCol, nStartCol += 4, true, ExcelHorizontalAlignment.Left);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountTypeSt, false, true);

                                    if (oItem.ComponentType == EnumComponentType.Asset || oItem.ComponentType == EnumComponentType.Expenditure)
                                    {
                                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ClosingBalance.ToString(), true, true);
                                        FillCell(sheet, nRowIndex, ++nStartCol, (0).ToString(), true, true);
                                    }
                                    else
                                    {
                                        FillCell(sheet, nRowIndex, ++nStartCol, (0).ToString(), true, true);
                                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ClosingBalance.ToString(), true, true);
                                    }                                    
                                }
                            }
                            else
                            {
                                if (oItem.AccountType == EnumAccountType.Segment)
                                {
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountCode, false, true);
                                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, ++nStartCol, nStartCol, false, ExcelHorizontalAlignment.Left, true, false);
                                    FillCellMerge(ref sheet, oItem.AccountHeadName, nRowIndex, nRowIndex, ++nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Left, false);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountTypeSt, false, true);
                                    if (oItem.ComponentType == EnumComponentType.Asset || oItem.ComponentType == EnumComponentType.Expenditure)
                                    {
                                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ClosingBalance.ToString(), true, true);
                                        FillCell(sheet, nRowIndex, ++nStartCol, (0).ToString(), true, true);
                                    }
                                    else
                                    {
                                        FillCell(sheet, nRowIndex, ++nStartCol, (0).ToString(), true, true);
                                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ClosingBalance.ToString(), true, true);
                                    }
                                }
                                if (oItem.AccountType == EnumAccountType.Group)
                                {
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountCode, false, true);

                                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, ++nStartCol, nStartCol += 1, false, ExcelHorizontalAlignment.Left, true, false);
                                    FillCellMerge(ref sheet, oItem.AccountHeadName, nRowIndex, nRowIndex, ++nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Left, false);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountTypeSt, false, true);
                                    if (oItem.ComponentType == EnumComponentType.Asset || oItem.ComponentType == EnumComponentType.Expenditure)
                                    {
                                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ClosingBalance.ToString(), true, true);
                                        FillCell(sheet, nRowIndex, ++nStartCol, (0).ToString(), true, true);
                                    }
                                    else
                                    {
                                        FillCell(sheet, nRowIndex, ++nStartCol, (0).ToString(), true, true);
                                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ClosingBalance.ToString(), true, true);
                                    }
                                }
                                if (oItem.AccountType == EnumAccountType.SubGroup)
                                {
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountCode, false, false);

                                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, ++nStartCol, nStartCol += 2, false, ExcelHorizontalAlignment.Left, true, false);
                                    FillCellMerge(ref sheet, oItem.AccountHeadName, nRowIndex, nRowIndex, ++nStartCol, nStartCol += 1, false, ExcelHorizontalAlignment.Left, false);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountTypeSt, false, false);
                                    if (oItem.ComponentType == EnumComponentType.Asset || oItem.ComponentType == EnumComponentType.Expenditure)
                                    {
                                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ClosingBalance.ToString(), true, true);
                                        FillCell(sheet, nRowIndex, ++nStartCol, (0).ToString(), true, true);
                                    }
                                    else
                                    {
                                        FillCell(sheet, nRowIndex, ++nStartCol, (0).ToString(), true, true);
                                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ClosingBalance.ToString(), true, true);
                                    }
                                }
                                if (oItem.AccountType == EnumAccountType.Ledger)
                                {
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountCode, false, false);

                                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, ++nStartCol, nStartCol += 3, false, ExcelHorizontalAlignment.Left, true, false);
                                    FillCellMerge(ref sheet, oItem.AccountHeadName, nRowIndex, nRowIndex, ++nStartCol, nStartCol, false, ExcelHorizontalAlignment.Left, false);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountTypeSt, false, false);
                                    if (oItem.ComponentType == EnumComponentType.Asset || oItem.ComponentType == EnumComponentType.Expenditure)
                                    {
                                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ClosingBalance.ToString(), true, true);
                                        FillCell(sheet, nRowIndex, ++nStartCol, (0).ToString(), true, true);
                                        nTotalDebitAmount = nTotalDebitAmount + oItem.ClosingBalance;
                                    }
                                    else
                                    {
                                        FillCell(sheet, nRowIndex, ++nStartCol, (0).ToString(), true, true);
                                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ClosingBalance.ToString(), true, true);
                                        nTotalCreditAmount = nTotalCreditAmount + oItem.ClosingBalance;
                                    }
                                }
                            }
                            nRowIndex++;
                        }
                    }
                    #region Grand Total
                    nStartCol = 2;
                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol, nStartCol += 6, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, nTotalDebitAmount.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nTotalCreditAmount.ToString(), true, true);

                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=TrailBalance.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        public void ExportToExcel_Ledger(string sTempString)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();

            #region Get Data
            string sDateRange = ""; _sFormatter = " #,##0.00;(#,##0.00)";
            int nDateType = Convert.ToInt32(sTempString.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(sTempString.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sTempString.Split('~')[2]);
            int nAccountType = Convert.ToInt32(sTempString.Split('~')[3]);
            int nAccountHead = Convert.ToInt32(sTempString.Split('~')[4]);
            int nBusinessUnitID = Convert.ToInt32(sTempString.Split('~')[5]);

            if (nDateType == 1)
            {
                dEndDate = dStartDate.AddDays(1);
                sDateRange = "Date on " + dStartDate.ToString("dd MMM yyyy");
            }
            else
            {
                sDateRange = dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy");
            }
            Company oCompany = new Company();
            _oTrailBalances = new List<TrailBalance>();
            _oTrailBalance = new TrailBalance();
            _oTrailBalance.StartDate = dStartDate;
            _oTrailBalance.EndDate = dEndDate;
            _oTrailBalance.AccountTypeInInt = nAccountType;
            _oTrailBalance.BusinessUnitID = nBusinessUnitID;

            try
            {
                _oTrailBalances = TrailBalance.Gets(nAccountHead, _oTrailBalance.AccountTypeInInt, Convert.ToDateTime(_oTrailBalance.StartDate.ToString("dd MMM yyyy")), Convert.ToDateTime(_oTrailBalance.EndDate), _oTrailBalance.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                oBusinessUnit = oBusinessUnit.Get(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]);

                foreach (TrailBalance oItem in _oTrailBalances)
                {
                    _oTTrailBalance = new TTrailBalance();
                    _oTTrailBalance.AccountHeadID = oItem.AccountHeadID;
                    _oTTrailBalance.ParentHeadID = oItem.ParentAccountHeadID;
                    _oTTrailBalance.AccountHeadName = oItem.AccountHeadName;
                    _oTTrailBalance.AccountCode = oItem.AccountCode;
                    _oTTrailBalance.OpenningBalanceDbl = oItem.OpenningBalance;
                    _oTTrailBalance.ClosingBalanceDbl = oItem.ClosingBalance;
                    _oTTrailBalance.DebitAmountDbl = oItem.DebitAmount;
                    _oTTrailBalance.CreditAmountDbl = oItem.CreditAmount;
                    _oTTrailBalance.AccountType = oItem.AccountType;
                    _oTTrailBalance.ComponentType = oItem.ComponentType;
                    _oTTrailBalances.Add(_oTTrailBalance);
                }
                List<TTrailBalance> oTTrailBalances = new List<TTrailBalance>();
                List<TTrailBalance> oTempTTrailBalances = new List<TTrailBalance>();

                oTempTTrailBalances = GetRoot();
                _oTrailBalances = new List<TrailBalance>();
                foreach (TTrailBalance oItem in oTempTTrailBalances)
                {
                    TTrailBalance oTTrailBalance = new TTrailBalance();
                    oTTrailBalance = oItem;
                    this.AddTreeNodes(ref  oTTrailBalance);
                    oTTrailBalances.Add(oTTrailBalance);
                }

                DeconstructTree(oTTrailBalances);
            }
            catch (Exception ex)
            {
                _sErrorMesage = ex.Message;
            }
            #endregion

            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();

                table_header.Add(new TableHeader { Header = "Code", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "", Width = 3f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "", Width = 3f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "", Width = 3f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "", Width = 3f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Particulars", Width = 38f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Type", Width = 15f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Debit Balance", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Credit Balance", Width = 25f, IsRotate = false });

                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Trial Balance");
                    sheet.Name = "Trial Balance";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Trial Balance"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = sDateRange; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    nStartCol = 1;
                    foreach (TableHeader listItem in table_header)
                    {
                        if (string.IsNullOrEmpty(listItem.Header)) continue;

                        if (listItem.Header.Equals("Particulars"))
                            cell = sheet.Cells[nRowIndex, ++nStartCol, nRowIndex, nStartCol += 4];
                        else cell = sheet.Cells[nRowIndex, ++nStartCol];

                        cell.Value = listItem.Header; cell.Style.Font.Bold = true; cell.Merge = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nRowIndex++;

                    string sCurrencySymbol = ""; int nCount = 0;
                    #region Data
                    double nTotalDebitAmount = 0;
                    double nTotalCreditAmount = 0;
                    foreach (TrailBalance oItem in _oTrailBalances)
                    {
                        nStartCol = 1;
                        if (oItem.OpenningBalance != 0 || oItem.DebitAmount != 0 || oItem.CreditAmount != 0 || oItem.ClosingBalance != 0)
                        {
                            nCount++;

                            if (oItem.AccountType == EnumAccountType.Ledger)
                            {
                                FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountCode, false, false);

                                FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, ++nStartCol, nStartCol += 3, false, ExcelHorizontalAlignment.Left, true, false);
                                FillCellMerge(ref sheet, oItem.AccountHeadName, nRowIndex, nRowIndex, ++nStartCol, nStartCol, false, ExcelHorizontalAlignment.Left, false);
                                FillCell(sheet, nRowIndex, ++nStartCol, oItem.AccountTypeSt, false, false);
                                if (oItem.ComponentType == EnumComponentType.Asset || oItem.ComponentType == EnumComponentType.Expenditure)
                                {
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.ClosingBalance.ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, (0).ToString(), true, true);
                                    nTotalDebitAmount = nTotalDebitAmount + oItem.ClosingBalance;
                                }
                                else
                                {
                                    FillCell(sheet, nRowIndex, ++nStartCol, (0).ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.ClosingBalance.ToString(), true, true);
                                    nTotalCreditAmount = nTotalCreditAmount + oItem.ClosingBalance;
                                }
                                nRowIndex++;
                            }
                        }
                    }
                    #region Grand Total
                    nStartCol = 2;
                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol, nStartCol += 6, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, nTotalDebitAmount.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nTotalCreditAmount.ToString(), true, true);

                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=TrailBalance.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
       
        public ActionResult ViewVoucherDetail(string sTempString)
        {
            string sDateRange = "";
            int nDateType = Convert.ToInt32(sTempString.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(sTempString.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sTempString.Split('~')[2]);
            int nAccountType = Convert.ToInt32(sTempString.Split('~')[3]);
            int nAccountHead = Convert.ToInt32(sTempString.Split('~')[4]);
            int nBusinessUnitID = Convert.ToInt32(sTempString.Split('~')[5]);
            bool bIsApproved = false;// Convert.ToBoolean(sTempString.Split('~')[5]);
            int nCurrencyID = 1;//You can change 
            if (nDateType == 1)
            {
                dEndDate = dStartDate;
                sDateRange = "Date on " + dStartDate.ToString("dd MMM yyyy");
            }
            else
            {
                sDateRange = dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy");
            }
            
            string sStatementInfo = "";
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            oSP_GeneralLedger.AccountHeadID = nAccountHead;
            oSP_GeneralLedger.StartDate = dStartDate;
            oSP_GeneralLedger.EndDate = dEndDate;
            oSP_GeneralLedger.CurrencyID = nCurrencyID;
            oSP_GeneralLedger.BusinessUnitID = nBusinessUnitID;
            _oSP_GeneralLedgers = new List<SP_GeneralLedger>();

            oChartsOfAccount = oChartsOfAccount.Get(oSP_GeneralLedger.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            sStatementInfo = "Account Head : " + oChartsOfAccount.AccountHeadName + "[" + oChartsOfAccount.AccountCode + "], @ " + dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy");
            
            string sSQL = GetSQL(oSP_GeneralLedger);
            _oSP_GeneralLedgers = SP_GeneralLedger.GetsGeneralLedger(oSP_GeneralLedger.AccountHeadID, oSP_GeneralLedger.StartDate, oSP_GeneralLedger.EndDate, nCurrencyID, bIsApproved, oSP_GeneralLedger.BusinessUnitIDs, (int)Session[SessionInfo.currentUserID]);

            ViewBag.StatementInfo = sStatementInfo;
            return PartialView(_oSP_GeneralLedgers);
        }
        #region Make SQL
        private string GetSQL(SP_GeneralLedger oGeneralLedger)
        {
            string sVoucherNo = "";
            double nFromVoucherAmount = 0;
            double nToVoucherAmount = 0;
            string sAccountHeadName = "";
            int nVoucherType = 0;
            string sHeadWiseNarration = "";
            string sVoucherNarration = "";
            string sVoucherBillNo = "";
            double nFromBillAmount = 0;
            double nToBillAmount = 0;
            string sCostCenterName = "";
            double nFromCostCenterAmount = 0;
            double nToCostCenterAmount = 0;
            int nVoucherAmountCompareOperatorType = 0;
            int nBillAmountCompareOperatorType = 0;
            int nCostCenterAmountCompareOperatorType = 0;
            string sParams = oGeneralLedger.Narration;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (sParams != null)
            {
                if (sParams != "")
                {
                    sVoucherNo = Convert.ToString(sParams.Split('~')[0]);
                    nFromVoucherAmount = Convert.ToDouble(sParams.Split('~')[1]);
                    nToVoucherAmount = Convert.ToDouble(sParams.Split('~')[2]);
                    sAccountHeadName = Convert.ToString(sParams.Split('~')[3]);
                    nVoucherType = Convert.ToInt32(sParams.Split('~')[4]);
                    sHeadWiseNarration = Convert.ToString(sParams.Split('~')[5]);
                    sVoucherNarration = Convert.ToString(sParams.Split('~')[6]);
                    sVoucherBillNo = Convert.ToString(sParams.Split('~')[7]);
                    nFromBillAmount = Convert.ToDouble(sParams.Split('~')[8]);
                    nToBillAmount = Convert.ToDouble(sParams.Split('~')[9]);
                    sCostCenterName = Convert.ToString(sParams.Split('~')[10]);
                    nFromCostCenterAmount = Convert.ToDouble(sParams.Split('~')[11]);
                    nToCostCenterAmount = Convert.ToDouble(sParams.Split('~')[12]);

                    nVoucherAmountCompareOperatorType = Convert.ToInt32(sParams.Split('~')[13]);
                    nBillAmountCompareOperatorType = Convert.ToInt32(sParams.Split('~')[14]);
                    nCostCenterAmountCompareOperatorType = Convert.ToInt32(sParams.Split('~')[15]);
                }
            }
                        
            string sReturn1 = "SELECT VD.VoucherID, VD.AccountHeadID FROM View_VoucherDetail AS VD ";
            string sReturn = "";

            #region BusinessUnit
            if (oGeneralLedger.BusinessUnitID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " VD.BUID =" + oGeneralLedger.BusinessUnitID.ToString();
            }
            #endregion
                        
            if (oGeneralLedger.IsApproved)
            {
                Global.TagSQL(ref sReturn);
                if (oCompany.BaseCurrencyID == oGeneralLedger.CurrencyID)
                {
                    sReturn = sReturn + "  VD.AccountHeadID = " + oGeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))) ";
                }
                else
                {
                    sReturn = sReturn + "  VD.CurrencyID= " + oGeneralLedger.CurrencyID + " AND VD.AccountHeadID=" + oGeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                }
            }
            else
            {
                Global.TagSQL(ref sReturn);
                if (oCompany.BaseCurrencyID == oGeneralLedger.CurrencyID)
                {
                    sReturn = sReturn + "  VD.AccountHeadID = " + oGeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))) ";
                }
                else
                {
                    sReturn = sReturn + "  VD.CurrencyID = " + oGeneralLedger.CurrencyID + " AND VD.AccountHeadID=" + oGeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                }
            }
                       
            if (sParams != null)
            {
                if (sParams != "")
                {
                    #region Voucher No
                    if (sVoucherNo != null && sVoucherNo != "")
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " VD.VoucherID IN (SELECT VoucherID FROM Voucher WHERE VoucherNo LIKE '%" + sVoucherNo + "%')  ";
                    }
                    #endregion

                    #region Voucher Amount
                    if (nVoucherAmountCompareOperatorType != (int)EnumCompareOperator.None)
                    {
                        Global.TagSQL(ref sReturn);
                        if (nVoucherAmountCompareOperatorType == (int)EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " VD.Amount = " + nFromVoucherAmount;
                        }
                        else if (nVoucherAmountCompareOperatorType == (int)EnumCompareOperator.NotEqualTo)
                        {
                            sReturn = sReturn + " VD.Amount != " + nFromVoucherAmount;
                        }
                        else if (nVoucherAmountCompareOperatorType == (int)EnumCompareOperator.GreaterThan)
                        {
                            sReturn = sReturn + " VD.Amount > " + nFromVoucherAmount;
                        }
                        else if (nVoucherAmountCompareOperatorType == (int)EnumCompareOperator.SmallerThan)
                        {
                            sReturn = sReturn + " VD.Amount < " + nFromVoucherAmount;
                        }
                        else if (nVoucherAmountCompareOperatorType == (int)EnumCompareOperator.Between)
                        {
                            sReturn = sReturn + " VD.Amount BETWEEN " + nFromVoucherAmount + " AND " + nToVoucherAmount + " ";
                        }
                        else if (nVoucherAmountCompareOperatorType == (int)EnumCompareOperator.NotBetween)
                        {
                            sReturn = sReturn + " VD.Amount NOT BETWEEN " + nFromVoucherAmount + " AND " + nToVoucherAmount + " ";
                        }
                    }
                    #endregion

                    #region Voucher Type
                    if (nVoucherType > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " VD.VoucherID IN (SELECT VoucherID FROM Voucher WHERE VoucherTypeID = " + nVoucherType + " ) ";
                    }
                    #endregion
                    #region Revarse AccountHeadName
                    if (sAccountHeadName != null && sAccountHeadName != "")
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " AccountHeadID IN (SELECT AccountHeadID FROM COA_ChartsOfAccount WHERE AccountHeadName LIKE '%" + sAccountHeadName + "%') AND AccountHeadID != " + oGeneralLedger.AccountHeadID + "  ";
                    }
                    #endregion

                    #region Head Wise Narration
                    if (sHeadWiseNarration != null && sHeadWiseNarration != "")
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " VD.Narration LIKE '%" + sHeadWiseNarration + "%' ";
                    }
                    #endregion

                    #region Voucher Narration
                    if (sVoucherNarration != null && sVoucherNarration != "")
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " VD.VoucherID IN (SELECT VoucherID FROM Voucher AS V WHERE V.Narration LIKE '%" + sVoucherNarration + "%') ";
                    }
                    #endregion

                    #region BillNo
                    if (sVoucherBillNo != null && sVoucherBillNo != "")
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " VD.VoucherDetailID IN (SELECT VBT.VoucherDetailID FROM View_VoucherBillTransaction AS VBT WHERE VBT.BillNo LIKE '%" + sVoucherBillNo + "%' ) ";
                    }
                    #endregion

                    #region Bill Amount
                    if (nBillAmountCompareOperatorType != (int)EnumCompareOperator.None)
                    {
                        Global.TagSQL(ref sReturn);
                        if (nBillAmountCompareOperatorType == (int)EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT VBT.VoucherDetailID FROM View_VoucherBillTransaction AS VBT WHERE VBT.Amount = " + nFromBillAmount + " ) ";
                        }
                        else if (nBillAmountCompareOperatorType == (int)EnumCompareOperator.NotEqualTo)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT VBT.VoucherDetailID FROM View_VoucherBillTransaction AS VBT WHERE VBT.Amount != " + nFromBillAmount + " ) ";
                        }
                        else if (nBillAmountCompareOperatorType == (int)EnumCompareOperator.GreaterThan)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT VBT.VoucherDetailID FROM View_VoucherBillTransaction AS VBT WHERE VBT.Amount > " + nFromBillAmount + " ) ";
                        }
                        else if (nBillAmountCompareOperatorType == (int)EnumCompareOperator.SmallerThan)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT VBT.VoucherDetailID FROM View_VoucherBillTransaction AS VBT WHERE VBT.Amount < " + nFromBillAmount + " ) ";
                        }
                        else if (nBillAmountCompareOperatorType == (int)EnumCompareOperator.Between)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT VBT.VoucherDetailID FROM View_VoucherBillTransaction AS VBT WHERE VBT.Amount BETWEEN " + nFromBillAmount + " AND " + nToBillAmount + " ) ";
                        }
                        else if (nBillAmountCompareOperatorType == (int)EnumCompareOperator.NotBetween)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT VBT.VoucherDetailID FROM View_VoucherBillTransaction AS VBT WHERE VBT.Amount NOT BETWEEN " + nFromBillAmount + " AND " + nToBillAmount + ") ";
                        }
                    }
                    #endregion

                    #region CostCenterName
                    if (sCostCenterName != null && sCostCenterName != "")
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " VD.VoucherDetailID IN (SELECT CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE CCT.CostCenterName LIKE '%" + sCostCenterName + "%' ) ";
                    }
                    #endregion

                    #region Cost Center Amount
                    if (nCostCenterAmountCompareOperatorType != (int)EnumCompareOperator.None)
                    {
                        Global.TagSQL(ref sReturn);
                        if (nCostCenterAmountCompareOperatorType == (int)EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE CCT.Amount = " + nFromCostCenterAmount + " ) ";
                        }
                        else if (nCostCenterAmountCompareOperatorType == (int)EnumCompareOperator.NotEqualTo)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE CCT.Amount != " + nFromCostCenterAmount + " ) ";
                        }
                        else if (nCostCenterAmountCompareOperatorType == (int)EnumCompareOperator.GreaterThan)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE CCT.Amount > " + nFromCostCenterAmount + " ) ";
                        }
                        else if (nCostCenterAmountCompareOperatorType == (int)EnumCompareOperator.SmallerThan)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE CCT.Amount < " + nFromCostCenterAmount + " ) ";
                        }
                        else if (nCostCenterAmountCompareOperatorType == (int)EnumCompareOperator.Between)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE CCT.Amount BETWEEN " + nFromCostCenterAmount + " AND " + nToCostCenterAmount + " ) ";
                        }
                        else if (nCostCenterAmountCompareOperatorType == (int)EnumCompareOperator.NotBetween)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE CCT.Amount NOT BETWEEN " + nFromCostCenterAmount + " AND " + nToCostCenterAmount + ") ";
                        }
                    }
                    #endregion
                }
            }
            sReturn = sReturn1 + sReturn + "  GROUP BY VoucherID, AccountHeadID ";
            return sReturn;
        }
        #endregion

    }
}
