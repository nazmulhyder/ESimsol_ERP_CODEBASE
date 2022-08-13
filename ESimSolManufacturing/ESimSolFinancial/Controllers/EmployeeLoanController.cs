using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSol.Controllers
{
    public class EmployeeLoanController : Controller
    {
        #region Declaration
        EmployeeLoan _oEmployeeLoan = new EmployeeLoan();
        List<EmployeeLoan> _oEmployeeLoans = new List<EmployeeLoan>();
        #endregion

        #region EmployeeLoan
        public ActionResult ViewEmployeeLoans(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oEmployeeLoans = new List<EmployeeLoan>();
            string sSQL = "Select * from View_EmployeeLoan Where EmployeeLoanID<>0";
            _oEmployeeLoans = EmployeeLoan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oEmployeeLoans);
        }

        public ActionResult ViewEmployeeLoan(int nId, double ts)
        {
            
            _oEmployeeLoan = new EmployeeLoan();
            if (nId > 0)
            {
                _oEmployeeLoan = EmployeeLoan.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                List<EmployeeLoanAmount> oELAs = new List<EmployeeLoanAmount>();
                List<EmployeeLoanInstallment> oELIs = new List<EmployeeLoanInstallment>();
                if (_oEmployeeLoan.EmployeeLoanID > 0)
                {
                    string sSQL = "SELECT * FROM View_EmployeeOfficial Where EmployeeID=" + _oEmployeeLoan.EmployeeID;
                    var EmpOfficial = EmployeeOfficial.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                   
                    sSQL = "SELECT * FROM View_EmployeeSalaryStructure Where EmployeeID=" + _oEmployeeLoan.EmployeeID;
                    var EmpSalary = EmployeeSalaryStructure.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    sSQL = "SELECT * FROM View_PFmember Where EmployeeID=" + _oEmployeeLoan.EmployeeID;
                    var PFMem = PFmember.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    _oEmployeeLoan.OfficialInfo = (EmpOfficial.FirstOrDefault() != null) ? EmpOfficial.FirstOrDefault().DesignationName + " at " + EmpOfficial.FirstOrDefault().DepartmentName + " of " + EmpOfficial.FirstOrDefault().LocationName + ".  Joining Date: " + EmpOfficial.FirstOrDefault().DateOfJoinInString + ", Confirmation Date: " + EmpOfficial.FirstOrDefault().DateOfConfirmationInString + ", Year of Service: " + Global.GetDuration(EmpOfficial.FirstOrDefault().DateOfJoin, DateTime.Now) : "";
                    _oEmployeeLoan.SalaryInfo = (EmpSalary.FirstOrDefault() != null) ? EmpSalary.FirstOrDefault().PaymentCycleInString + " - " + Global.MillionFormat(EmpSalary.FirstOrDefault().GrossAmount) : "";
                    _oEmployeeLoan.PFInfo = this.GetPFInfo(_oEmployeeLoan.EmployeeID, _oEmployeeLoan.IsPFLoan);
                    _oEmployeeLoan.LastLoanInfo = this.GetLastLoanInfo(_oEmployeeLoan.EmployeeID, _oEmployeeLoan.EmployeeLoanID);

                    sSQL = "Select * from View_EmployeeLoanAmount Where EmployeeLoanID=" + _oEmployeeLoan.EmployeeLoanID;
                    oELAs = EmployeeLoanAmount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    sSQL = "Select * from EmployeeLoanInstallment Where EmployeeLoanID=" + _oEmployeeLoan.EmployeeLoanID;
                    oELIs = EmployeeLoanInstallment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                ViewBag.LoanInstallments = this.GenerateInstallment(oELAs, oELIs);

            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(((User)Session[SessionInfo.CurrentUser]).CompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CurrencySymbol = (oCompany != null && oCompany.CompanyID > 0) ? oCompany.CurrencySymbol : "tk";
            return View(_oEmployeeLoan);
        }


        [HttpPost]
        public JsonResult Gets(EmployeeLoan oEmployeeLoan)
        {
            _oEmployeeLoans = new List<EmployeeLoan>();
            try
            {
                string sEmployeeLoanNo = (string.IsNullOrEmpty(oEmployeeLoan.Params.Split('~')[0])) ? "" : oEmployeeLoan.Params.Split('~')[0].Trim();
                string sEmpNameCode = (string.IsNullOrEmpty(oEmployeeLoan.Params.Split('~')[1])) ? "" : oEmployeeLoan.Params.Split('~')[1].Trim();
                string sSQL = "Select * from View_EmployeeLoan Where EmployeeLoanID<>0 ";
                if (sEmployeeLoanNo != "") 
                    sSQL += " And Code Like '%" + sEmployeeLoanNo + "%'";
                if (sEmpNameCode != "")
                    sSQL += " And EmployeeID In (Select EmployeeID from Employee Where Code Like '%" + sEmpNameCode + "%' Or Name Like '%" + sEmpNameCode + "%')";
               
                _oEmployeeLoans = EmployeeLoan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oEmployeeLoans = new List<EmployeeLoan>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeLoans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        private List<EmployeeLoanInstallment> GenerateInstallment(List<EmployeeLoanAmount> oELAs, List<EmployeeLoanInstallment> oELIs)
        {
            List<EmployeeLoanInstallment> oTempList = new List<EmployeeLoanInstallment>();
            try
            {
                if (oELAs.Count() > 0 && oELIs.Count() > 0)
                {
                    EmployeeLoanInstallment oELI = new EmployeeLoanInstallment();
                    oELI.Type = 0;
                    oELI.InstallmentDate = oELAs.FirstOrDefault().LoanDisburseDate;
                    oELI.InstallmentAmount = oELAs.Sum(x=>x.Amount);
                    oELI.InterestPerInstallment = 0;
                    oELI.TotalAmount = 0;
                    oELI.Balance = oELAs.Sum(x => x.Amount);
                    oTempList.Add(oELI);

                    foreach(EmployeeLoanInstallment oItem in oELIs)
                    {
                        oItem.Type = 1;
                        oItem.TotalAmount = oItem.InstallmentAmount + oItem.InterestPerInstallment;
                        oItem.Balance = oTempList.FirstOrDefault().Balance - (oELIs.Where(x => x.Type != 0 && x.Balance > 0).Sum(x => x.InstallmentAmount) + oItem.InstallmentAmount);
                        oTempList.Add(oItem);
                    }
                }
            }
            catch (Exception ex)
            {
                oTempList = new List<EmployeeLoanInstallment>();
            }
            return oTempList;
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


        public ActionResult PrintEmployeeLoan(int nId, double nts)
        {
            _oEmployeeLoan = new EmployeeLoan();
            List<EmployeeLoanInstallment> oELIs = new List<EmployeeLoanInstallment>();
            List<EmployeeLoanAmount> oELAs = new List<EmployeeLoanAmount>();
            List<EmployeeLoanRefund> oEmployeeLoanRefunds = new List<EmployeeLoanRefund>();
            if (nId > 0)
            {
                _oEmployeeLoan = EmployeeLoan.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
                if (_oEmployeeLoan.EmployeeLoanID > 0)
                {
                    string sSQL = "SELECT * FROM View_EmployeeOfficial Where EmployeeID=" + _oEmployeeLoan.EmployeeID;
                    var EmpOfficial = EmployeeOfficial.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    sSQL = "SELECT * FROM View_EmployeeSalaryStructure Where EmployeeID=" + _oEmployeeLoan.EmployeeID;
                    var EmpSalary = EmployeeSalaryStructure.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                   

                    _oEmployeeLoan.OfficialInfo = (EmpOfficial.FirstOrDefault() != null) ? EmpOfficial.FirstOrDefault().DesignationName + " at " + EmpOfficial.FirstOrDefault().DepartmentName + " of " + EmpOfficial.FirstOrDefault().LocationName + ".  Joining Date: " + EmpOfficial.FirstOrDefault().DateOfJoinInString + ", Confirmation Date: " + EmpOfficial.FirstOrDefault().DateOfConfirmationInString + ", Year of Service: " + Global.GetDuration(EmpOfficial.FirstOrDefault().DateOfJoin, DateTime.Now) : "";
                    _oEmployeeLoan.SalaryInfo = (EmpSalary.FirstOrDefault() != null) ?  EmpSalary.FirstOrDefault().PaymentCycleInString + " - " + Global.MillionFormat(EmpSalary.FirstOrDefault().GrossAmount) : "";
                    _oEmployeeLoan.PFInfo = this.GetPFInfo(_oEmployeeLoan.EmployeeID, _oEmployeeLoan.IsPFLoan);
                    _oEmployeeLoan.LastLoanInfo = this.GetLastLoanInfo(_oEmployeeLoan.EmployeeID, _oEmployeeLoan.EmployeeLoanID);

                    sSQL = "Select * from View_EmployeeLoanAmount Where EmployeeLoanID=" + _oEmployeeLoan.EmployeeLoanID;
                    oELAs = EmployeeLoanAmount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    sSQL = "Select * from EmployeeLoanInstallment Where EmployeeLoanID=" + _oEmployeeLoan.EmployeeLoanID;
                    oELIs = EmployeeLoanInstallment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oELIs = this.GenerateInstallment(oELAs, oELIs);


                    sSQL = "Select * from View_EmployeeLoanRefund Where ApproveBy>0 And EmployeeLoanID=" + _oEmployeeLoan.EmployeeLoanID + "";
                    oEmployeeLoanRefunds = EmployeeLoanRefund.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptLoanRequest oReport = new rptLoanRequest();
            byte[] abytes = oReport.PrepareReport(new LoanRequest(), _oEmployeeLoan, oELIs, oELAs, oEmployeeLoanRefunds, oCompany);
            return File(abytes, "application/pdf");

        }


        private string GetLastLoanInfo(int nEmpId, int nEmployeeLoanID)
        {
            string sLastLoanInfo = "N/A";

            string sSQL = "Select * from View_EmployeeLoan Where EmployeeLoanID = (Select ISNULL(MAX(EmployeeLoanID),0) from EmployeeLoan Where EmployeeID = " + nEmpId +" "+ ((nEmployeeLoanID>0)? " And EmployeeLoanID != " + nEmployeeLoanID  : "") +" )";
            var EmpLoans = EmployeeLoan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            var EmpLoan = ((EmpLoans != null && EmpLoans.Count() > 0) ? EmpLoans.FirstOrDefault() : new EmployeeLoan());

            if (EmpLoan.EmployeeLoanID > 0)
            {
                sSQL = "Select * from View_EmployeeLoanAmount Where EmployeeLoanID In(" + EmpLoan.EmployeeLoanID + ")";
                var EmpLoanAmounts = EmployeeLoanAmount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "Select * from EmployeeLoanInstallment Where EmployeeLoanID=" + EmpLoan.EmployeeLoanID;
                var EmpLoanInsts = EmployeeLoanInstallment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                double nInterestRate = EmpLoan.InterestRate;
                DateTime dtNewEncash = (EmpLoanInsts.Where(x => x.ESDetailID > 0).Count() > 0) ? EmpLoanInsts.Where(x => x.ESDetailID > 0).LastOrDefault().InstallmentDate.AddMonths(1) : (EmpLoanInsts.Count() > 0) ? EmpLoanInsts.FirstOrDefault().InstallmentDate.AddMonths(-1) : DateTime.Now;


                sLastLoanInfo = "Disburse Date: " + EmpLoanAmounts.FirstOrDefault().LoanDisburseDateStr + 
                    "; Amount:" + Global.MillionFormat(EmpLoanAmounts.Sum(x => x.Amount)) + 
                    "; No of Inst.: " + EmpLoanInsts.Where(x => x.ELInstallmentID > 0).Count() + 
                    "; Encash Inst.: " + EmpLoanInsts.Where(x => x.ELInstallmentID > 0 && x.ESDetailID > 0).Count() + 
                    "; Pending Inst.: " + EmpLoanInsts.Where(x => x.ELInstallmentID > 0 && x.ESDetailID == 0).Count() +
                    "; Interest Rate: " + Global.MillionFormat(EmpLoan.InterestRate) +
                    "; Last Inst.: " + Global.MillionFormat((EmpLoanInsts.Where(x => x.ELInstallmentID > 0 && x.ESDetailID > 0).Count()>0)? EmpLoanInsts.Where(x => x.ELInstallmentID > 0 && x.ESDetailID > 0).LastOrDefault().InstallmentAmount:0) + 
                    "; Balance: " + Global.MillionFormat(EmpLoanInsts.Where(x => x.ELInstallmentID > 0 && x.ESDetailID == 0).Sum(x => x.InstallmentAmount))+
                    "~" + (Int16)EmpLoan.LoanType + "~" + nInterestRate + "~" + dtNewEncash.ToString("dd MMM yyyy") + "~" + ((EmpLoan.IsPFLoan) ? 1 : 0);

            }
            return sLastLoanInfo;
        }

        private string GetPFInfo(int nEmployeeId, bool IsPFLoan)
        {
            string sPFInfo = "N/A";

            try
            {
                if (IsPFLoan)
                {
                    string sSQL = "SELECT * FROM View_PFmember Where EmployeeID=" + nEmployeeId;
                    var PFMems = PFmember.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    var PFMem = (PFMems.FirstOrDefault() != null) ? PFMems.FirstOrDefault() : new PFmember();

                    if (PFMem.PFMID > 0)
                    {
                        sPFInfo = "PF Balance: " + Global.MillionFormat(PFMem.PFBalance);

                        sSQL = "Select * from EmployeeLoanSetup Where ApproveBy<>0 And InactiveBy=0";
                        var EmpLoanSetups = EmployeeLoanSetup.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        var EmpLoanSetup = (EmpLoanSetups.FirstOrDefault() != null) ? EmpLoanSetups.FirstOrDefault() : new EmployeeLoanSetup();


                        sSQL = "Select * from EmployeeLoanAmount Where EmployeeLoanID In (Select EmployeeLoanID from EmployeeLoan Where EmployeeID=" + nEmployeeId + " And IsPFLoan=1)";
                        var ELAs = EmployeeLoanAmount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        sSQL = "Select * from EmployeeLoanInstallment Where ESDetailID>0 And EmployeeLoanID In (Select EmployeeLoanID from EmployeeLoan Where EmployeeID=" + nEmployeeId + " And IsPFLoan=1)";
                        var ELIs = EmployeeLoanInstallment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        double nLoanAmount = 0;
                        if (EmpLoanSetup.ELSID > 0)
                        {
                            nLoanAmount = (PFMem.PFBalance * EmpLoanSetup.LimitInPercentOfPF) / 100;
                        }
                        else
                        {
                            nLoanAmount = PFMem.PFBalance;
                        }
                        nLoanAmount = nLoanAmount - (ELAs.Sum(x => x.Amount) - ELIs.Sum(x => x.InstallmentAmount));
                        sPFInfo += "; Max. Loan Amount: " + Global.MillionFormat(nLoanAmount);

                    }
                }
            }
            catch (Exception e)
            {
                sPFInfo = "N/A";
            }

            return sPFInfo;
        }
        #endregion

        #region Employee Loan Info Report

        public ActionResult ViewEmployeeLoanInfos(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<EmployeeLoanInfo> oEmployeeLoanInfos = new List<EmployeeLoanInfo>();
            return View(oEmployeeLoanInfos);
        }


        private List<EmployeeLoanInfo> GetsEmployeeLoanInfo(string sParams)
        {
            EmployeeLoanInfo oEmployeeLoanInfo = new EmployeeLoanInfo();
            List<EmployeeLoanInfo> oEmployeeLoanInfos = new List<EmployeeLoanInfo>();
            try
            {
                DateTime dtFrom = Convert.ToDateTime(sParams.Split('~')[0]);
                DateTime dtTo = Convert.ToDateTime(sParams.Split('~')[1]);
                oEmployeeLoanInfos = EmployeeLoanInfo.Gets(dtFrom, dtTo, ((User)Session[SessionInfo.CurrentUser]).UserID);

                //oEmployeeLoanInfos.ForEach(x =>
                //     {
                //         x.LoanAmount = Math.Ceiling(x.LoanAmount);
                //         x.InstallmentAmount = Math.Ceiling(x.InstallmentAmount);
                //     }
                // );
            }
            catch (Exception ex)
            {
                oEmployeeLoanInfo = new EmployeeLoanInfo();
                oEmployeeLoanInfo.ErrorMessage = ex.Message;
                _oEmployeeLoans = new List<EmployeeLoan>();
                oEmployeeLoanInfos.Add(oEmployeeLoanInfo);
            }
            return oEmployeeLoanInfos;
        }

        [HttpPost]
        public JsonResult GetsEmployeeLoanInfo(EmployeeLoanInfo oEmployeeLoanInfo)
        {
            var oEmployeeLoanInfos = this.GetsEmployeeLoanInfo(oEmployeeLoanInfo.Params);
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLoanInfos);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public void ExcelEmployeeLoanInfo(string sParams, double nts)
        {
            var oEmployeeLoanInfos = this.GetsEmployeeLoanInfo(sParams);

            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Employee Loan Info");
                sheet.Name = "Employee Loan Info";
                sheet.Column(2).Width = 10; //SL
                sheet.Column(3).Width = 18; //Code
                sheet.Column(4).Width = 25; //Name
                sheet.Column(5).Width = 30; //DepartmentName
                sheet.Column(6).Width = 20; //DesignationName
                sheet.Column(7).Width = 20; //Working Status
                sheet.Column(8).Width = 15; //Disburse Date
                sheet.Column(9).Width = 15; //Installment Start Date
                sheet.Column(10).Width = 15; //LoanAmount
                sheet.Column(11).Width = 15; //No of Installment
                sheet.Column(12).Width = 18; //Installment Amount
                sheet.Column(13).Width = 16; //Interest Rate
                sheet.Column(14).Width = 15; //Refund Amount
                sheet.Column(15).Width = 16; //Installment Paid Amount
                sheet.Column(16).Width = 18; //Interest Paid Amount
                sheet.Column(17).Width = 15; //Installment Payable
                sheet.Column(18).Width = 15; //Interest Payable
                sheet.Column(19).Width = 15; //Loan Balance

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, 18].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, 18].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Employee Loan Info"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Column Header

                cell = sheet.Cells[rowIndex, 2]; cell.Value = "SL"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 3]; cell.Value = "Employee Code"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "Employee Name"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 5]; cell.Value = "Department Name"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 6]; cell.Value = "Designation Name"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 7]; cell.Value = "Working Status"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 8]; cell.Value = "Disburse Date"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 9]; cell.Value = "Inst. Start Date"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 10]; cell.Value = "Loan Amount"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 11]; cell.Value = "No of Installment"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 12]; cell.Value = "Inst. Amount "; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 13]; cell.Value = "Interest Rate"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 14]; cell.Value = "Refund Amount"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 15]; cell.Value = "Inst. Paid Amount"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 16]; cell.Value = "Interest Paid Amount"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 17]; cell.Value = "Inst. Payable"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 18]; cell.Value = "Interest Payable"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 19]; cell.Value = "Loan Balance"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;
                #endregion


                #region Table Body

                if (oEmployeeLoanInfos.Count() > 0)
                {

                    #region Table Part

                    int nCount = 0;
                    foreach (EmployeeLoanInfo oItem in oEmployeeLoanInfos)
                    {
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = (++nCount).ToString(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.Code; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.IsActiveInString; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.DisburseDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.InstallmentStartDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value =oItem.LoanAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = oItem.NoOfInstallment; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 12]; cell.Value = oItem.InstallmentAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 13]; cell.Value = oItem.InterestRate; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 14]; cell.Value = oItem.RefundAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 15]; cell.Value = oItem.InstallmentPaid; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 16]; cell.Value = oItem.InterestPaid; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 17]; cell.Value = oItem.InstallmentPayable; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 18]; cell.Value = oItem.InterestPayable; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                      
                        cell = sheet.Cells[rowIndex, 19]; cell.Value = oItem.LoanBalance; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex++;
                    }
                    #endregion

                    #region Total
                    sheet.Cells[rowIndex, 2, rowIndex, 8].Merge = true;
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Value = "Total"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = oEmployeeLoanInfos.Sum(x=>x.LoanAmount); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                   
                    cell = sheet.Cells[rowIndex, 12]; cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 14]; cell.Value = oEmployeeLoanInfos.Sum(x => x.RefundAmount); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 15]; cell.Value = oEmployeeLoanInfos.Sum(x => x.InstallmentPaid); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 16]; cell.Value = oEmployeeLoanInfos.Sum(x => x.InterestPaid); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 17]; cell.Value = oEmployeeLoanInfos.Sum(x => x.InstallmentPayable); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 18]; cell.Value = oEmployeeLoanInfos.Sum(x => x.InterestPayable); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 19]; cell.Value = oEmployeeLoanInfos.Sum(x => x.LoanBalance); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                    #endregion
                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LoanInfo.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }


        #endregion

        #region Employee Loan Setup

        public ActionResult ViewEmployeeLoanSetups(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<EmployeeLoanSetup> oEmployeeLoanSetups = new List<EmployeeLoanSetup>();
            ViewBag.EnumRecruitmentEvents = Enum.GetValues(typeof(EnumRecruitmentEvent)).Cast<EnumRecruitmentEvent>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            string sSQL = "Select * from View_EmployeeLoanSetup";
            oEmployeeLoanSetups = EmployeeLoanSetup.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<SalaryHead> oSalaryHeads = new List<SalaryHead>();
            sSQL = "SELECT * FROM SalaryHead Where SalaryHeadType=" + (int)EnumSalaryHeadType.Deduction + " And IsActive=1";
            oSalaryHeads = SalaryHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.SalaryHeads = oSalaryHeads;

            return View(oEmployeeLoanSetups);
        }


        [HttpPost]
        public JsonResult SaveLoanSetup(EmployeeLoanSetup oEmployeeLoanSetup)
        {
            try
            {
                if (oEmployeeLoanSetup.ELSID <= 0)
                {
                    oEmployeeLoanSetup = oEmployeeLoanSetup.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oEmployeeLoanSetup = oEmployeeLoanSetup.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oEmployeeLoanSetup = new EmployeeLoanSetup();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLoanSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteLoanSetup(EmployeeLoanSetup oEmployeeLoanSetup)
        {
            try
            {
                if (oEmployeeLoanSetup.ELSID <= 0) { throw new Exception("Please select a valid item."); }
                oEmployeeLoanSetup = oEmployeeLoanSetup.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oEmployeeLoanSetup = new EmployeeLoanSetup();
                oEmployeeLoanSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLoanSetup.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveLoanSetup(EmployeeLoanSetup oEmployeeLoanSetup)
        {
            try
            {
                if (oEmployeeLoanSetup.ELSID <= 0)
                    throw new Exception("Please select a valid loan request.");
               
                oEmployeeLoanSetup = oEmployeeLoanSetup.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oEmployeeLoanSetup = new EmployeeLoanSetup();
                oEmployeeLoanSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLoanSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ActiveLoanSetup(EmployeeLoanSetup oEmployeeLoanSetup)
        {
            try
            {
                if (oEmployeeLoanSetup.ELSID <= 0)
                    throw new Exception("Please select a valid loan request.");

                oEmployeeLoanSetup = oEmployeeLoanSetup.IUD((int)EnumDBOperation.Active, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oEmployeeLoanSetup = new EmployeeLoanSetup();
                oEmployeeLoanSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLoanSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactiveLoanSetup(EmployeeLoanSetup oEmployeeLoanSetup)
        {
            try
            {
                if (oEmployeeLoanSetup.ELSID <= 0)
                    throw new Exception("Please select a valid loan request.");

                oEmployeeLoanSetup = oEmployeeLoanSetup.IUD((int)EnumDBOperation.InActive, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oEmployeeLoanSetup = new EmployeeLoanSetup();
                oEmployeeLoanSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLoanSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GetLoanSetup(EmployeeLoanSetup oEmployeeLoanSetup)
        {
            try
            {
                oEmployeeLoanSetup = EmployeeLoanSetup.Get(oEmployeeLoanSetup.ELSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oEmployeeLoanSetup = new EmployeeLoanSetup();
                oEmployeeLoanSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLoanSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Employee Loan Refund

        public ActionResult ViewEmployeeLoanRefunds(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<EmployeeLoanRefund> oEmployeeLoanRefunds = new List<EmployeeLoanRefund>();
            //string sSQL = "Select * from View_EmployeeLoanRefund Where ApproveBy<=0";
            string sSQL = "Select * from View_EmployeeLoanRefund";
            oEmployeeLoanRefunds = EmployeeLoanRefund.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oEmployeeLoanRefunds);
        }

        [HttpPost]
        public JsonResult SaveLoanRefund(EmployeeLoanRefund oEmployeeLoanRefund)
        {
            try
            {
                if (oEmployeeLoanRefund.ELRID <= 0)
                {
                    oEmployeeLoanRefund = oEmployeeLoanRefund.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oEmployeeLoanRefund = oEmployeeLoanRefund.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oEmployeeLoanRefund = new EmployeeLoanRefund();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLoanRefund);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteLoanRefund(EmployeeLoanRefund oEmployeeLoanRefund)
        {
            try
            {
                if (oEmployeeLoanRefund.ELRID <= 0) { throw new Exception("Please select a valid item."); }
                oEmployeeLoanRefund = oEmployeeLoanRefund.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oEmployeeLoanRefund = new EmployeeLoanRefund();
                oEmployeeLoanRefund.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLoanRefund.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveLoanRefund(EmployeeLoanRefund oEmployeeLoanRefund)
        {
            try
            {
                if (oEmployeeLoanRefund.ELRID <= 0)
                    throw new Exception("Please select a valid loan refund.");

                oEmployeeLoanRefund = oEmployeeLoanRefund.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oEmployeeLoanRefund = new EmployeeLoanRefund();
                oEmployeeLoanRefund.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLoanRefund);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsEmployeeLoanRefund(EmployeeLoanRefund oEmployeeLoanRefund)
        {
            List<EmployeeLoanRefund> oEmployeeLoanRefunds = new List<EmployeeLoanRefund>();
            try
            {


                string sRefundNo = oEmployeeLoanRefund.Params.Split('~')[0].Trim(); ;
                string sEmployeeNameCode = oEmployeeLoanRefund.Params.Split('~')[1].Trim();
                bool IsDateSearch = Convert.ToBoolean(oEmployeeLoanRefund.Params.Split('~')[2]);
                DateTime dtFrom = Convert.ToDateTime(oEmployeeLoanRefund.Params.Split('~')[3]);
                DateTime dtTo = Convert.ToDateTime(oEmployeeLoanRefund.Params.Split('~')[4]);

                string sSQL = "Select * from View_EmployeeLoanRefund Where ELRID<>0 ";

                if (!string.IsNullOrEmpty(sRefundNo))
                    sSQL += " And RefundNo Like '%" + sRefundNo + "%'";
                if (!string.IsNullOrEmpty(sEmployeeNameCode))
                    sSQL += " And (EmployeeName Like '%" + sEmployeeNameCode + "%' OR EmployeeCode Like '%" + sEmployeeNameCode + "%')";

                if (IsDateSearch)
                    sSQL += " And RefundDate Between '" + dtFrom.ToString("dd MMM yyyy") + "' And '" + dtTo.ToString("dd MMM yyyy") + "'";

                oEmployeeLoanRefunds = EmployeeLoanRefund.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oEmployeeLoanRefunds.Count() > 0 && oEmployeeLoanRefunds.FirstOrDefault().ELRID <= 0)
                    throw new Exception("No refund found.");

            }
            catch (Exception ex)
            {
                oEmployeeLoanRefund = new EmployeeLoanRefund();
                oEmployeeLoanRefund.ErrorMessage = ex.Message;

                oEmployeeLoanRefunds = new List<EmployeeLoanRefund>();
                oEmployeeLoanRefunds.Add(oEmployeeLoanRefund);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLoanRefunds);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetLoanRefund(EmployeeLoanRefund oEmployeeLoanRefund)
        {
            try
            {
                oEmployeeLoanRefund = EmployeeLoanRefund.Get(oEmployeeLoanRefund.ELRID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oEmployeeLoanRefund = new EmployeeLoanRefund();
                oEmployeeLoanRefund.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLoanRefund);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRefundableLoan(EmployeeLoan oEmployeeLoan)
        {
            List<EmployeeLoanInstallment> oELIs = new List<EmployeeLoanInstallment>();
            try
            {
                oEmployeeLoan = EmployeeLoan.Get(oEmployeeLoan.EmployeeLoanID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oEmployeeLoan.EmployeeLoanID > 0)
                {
                    string sSQL = "Select * from EmployeeLoanInstallment Where ESDetailID=0 And EmployeeLoanID =" + oEmployeeLoan.EmployeeLoanID + " Order By ELInstallmentID Desc";
                    oELIs = EmployeeLoanInstallment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oEmployeeLoan.RefundableAmount = (oELIs.Select(p => p.InstallmentAmount).Sum());
                    oEmployeeLoan.RemainingInstallment = (oELIs.Count());
                    //if (oELIs.Count() <= 0)
                    //    throw new Exception("No active loan found for refundable.");
                    //else
                    //{
                    //    oEmployeeLoan.RefundableAmount = (oELIs.Select(p => p.InstallmentAmount).Sum());
                    //    oEmployeeLoan.RemainingInstallment = (oELIs.Count());
                    //}
                }
                
            }
            catch (Exception ex)
            {
                oEmployeeLoan = new EmployeeLoan();
                oEmployeeLoan.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLoan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsRefundableLoan(EmployeeLoan oEmployeeLoan)
        {
            List<EmployeeLoan> oEmployeeLoans = new List<EmployeeLoan>();
            List<EmployeeLoanInstallment> oELIs = new List<EmployeeLoanInstallment>();
            try
            {
                if (oEmployeeLoan.EmployeeID <= 0)
                    throw new Exception("Please select an employee.");

                string sSQL = "Select * from View_EmployeeLoan Where EmployeeID=" + oEmployeeLoan.EmployeeID;
                oEmployeeLoans = EmployeeLoan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oEmployeeLoans.Count() > 0 && oEmployeeLoans.FirstOrDefault().EmployeeLoanID > 0)
                {
                    sSQL = "Select * from EmployeeLoanInstallment Where ESDetailID=0 And EmployeeLoanID In (" + string.Join(",", oEmployeeLoans.Select(x => x.EmployeeLoanID).ToList()) + ") Order By ELInstallmentID Desc";
                    oELIs = EmployeeLoanInstallment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oEmployeeLoans = oEmployeeLoans.Where(x => oELIs.Select(p => p.EmployeeLoanID).Distinct().ToList().Contains(x.EmployeeLoanID)).ToList();

                    if (oEmployeeLoans.Count() <= 0)
                        throw new Exception("No active loan found for refundable.");
                    else
                    {
                        oEmployeeLoans.ForEach(x => x.RefundableAmount = (oELIs.Where(o => o.EmployeeLoanID == x.EmployeeLoanID).Select(p => p.InstallmentAmount).Sum()));
                        oEmployeeLoans.ForEach(x => x.RemainingInstallment = (oELIs.Where(o => o.EmployeeLoanID == x.EmployeeLoanID).Count()));
                    }
                }

            }
            catch (Exception ex)
            {
                oEmployeeLoan = new EmployeeLoan();
                oEmployeeLoan.ErrorMessage = ex.Message;

                oEmployeeLoans = new List<EmployeeLoan>();
                oEmployeeLoans.Add(oEmployeeLoan);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeLoans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRefundableInstallment(EmployeeLoan oEmployeeLoan)
        {
            List<EmployeeLoanInstallment> oELIs = new List<EmployeeLoanInstallment>();
            try
            {
                string sSQL = "Select * from EmployeeLoanInstallment Where ESDetailID=0 And EmployeeLoanID =" + oEmployeeLoan.EmployeeLoanID + " Order By ELInstallmentID Desc";
                oELIs = EmployeeLoanInstallment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oELIs = new List<EmployeeLoanInstallment>();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oELIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintLoanRefund(int nId, double nts)
        {
            EmployeeLoanRefund oEmployeeLoanRefund = new EmployeeLoanRefund();
            string sLoanDetails = "";
            string sEmpInfo = "";
            try
            {
                if (nId > 0)
                {
                    oEmployeeLoanRefund = EmployeeLoanRefund.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oEmployeeLoanRefund.ELRID > 0)
                    {
                        string sSQL = "Select * from View_EmployeeLoanAmount Where EmployeeLoanID="+oEmployeeLoanRefund.EmployeeLoanID+"";
                        var oELAs= EmployeeLoanAmount.Gets(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);

                        sSQL = "Select * from EmployeeLoanInstallment Where EmployeeLoanID=" + oEmployeeLoanRefund.EmployeeLoanID + " And ESDetailID<=0";
                        var oELIs= EmployeeLoanInstallment.Gets(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);

                        sLoanDetails = "Loan Disbursed - " + Global.MillionFormat(oELAs.Sum(x => x.Amount)) + ",  Balance Amount - " +Global.MillionFormat(oELIs.Sum(x=>x.InstallmentAmount));

                        sSQL = "Select * from View_EmployeeOfficialALL Where EmployeeID=(Select EmployeeID from EmployeeLoan Where EmployeeLoanID=" + oEmployeeLoanRefund.EmployeeLoanID + ")";
                        var oEmpOffs = EmployeeOfficial.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        if (oEmpOffs.Any())
                        {
                            var oEmpOff = oEmpOffs.FirstOrDefault();
                            sEmpInfo = oEmpOff.EmployeeName + "(" + oEmpOff.Code + "), " + oEmpOff.DesignationName + ", " + oEmpOff.DepartmentName + ".";
                        }

                    }
                }
            }
            catch(Exception e)
            {
                oEmployeeLoanRefund.ErrorMessage = e.Message;
            }
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptLoanRefund oReport = new rptLoanRefund();
            byte[] abytes = oReport.PrepareReport(oEmployeeLoanRefund, oCompany,sLoanDetails, sEmpInfo);
            return File(abytes, "application/pdf");

        }


        #endregion

        #region Loan Summary

        public ActionResult ViewLoanSummary(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<LoanSummary> oLoanSummarys = new List<LoanSummary>();
            return View(oLoanSummarys);
        }

        private List<LoanSummary> GetsLoanSummary(string sParams)
        {
            List<LoanSummary> oLoanSummarys = new List<LoanSummary>();
            try
            {
                DateTime dtFrom = Convert.ToDateTime(sParams.Split('~')[0].Trim());
                DateTime dtTo = Convert.ToDateTime(sParams.Split('~')[1].Trim());
                string sDepartmentIDs = (string.IsNullOrEmpty(sParams.Split('~')[2])) ? "" : sParams.Split('~')[2].Trim();
                int nSalaryMonth = Convert.ToInt32(sParams.Split('~')[3].Trim());

                oLoanSummarys = LoanSummary.Gets( dtFrom, dtTo, sDepartmentIDs, nSalaryMonth, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLoanSummarys = new List<LoanSummary>();
                LoanSummary oLoanSummary = new LoanSummary();
                oLoanSummary.ErrorMessage = ex.Message;
                oLoanSummarys.Add(oLoanSummary);
            }
            return oLoanSummarys;
        }

        [HttpPost]
        public JsonResult GetsLoanSummary(LoanSummary oLoanSummary)
        {
            List<LoanSummary> oLoanSummarys = new List<LoanSummary>();
            oLoanSummarys = this.GetsLoanSummary(oLoanSummary.Params);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLoanSummarys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public void ExcelLoanSummary(string sParams, double nts)
        {
            var oLoanSummarys = this.GetsLoanSummary(sParams);

            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Loan Summary");
                sheet.Name = "Employee Loan Info";
                sheet.Column(2).Width = 10; //SL
                sheet.Column(3).Width = 30; //DepartmentName
                sheet.Column(4).Width = 15; //LoanAmount
                sheet.Column(5).Width = 15; //InstallmentDeduction
                sheet.Column(6).Width = 15; //InterestDeduction
                sheet.Column(7).Width = 15; //RefundAmount
                sheet.Column(8).Width = 15; //RefundCharge
                sheet.Column(9).Width = 15; //TotalExtra
                sheet.Column(10).Width = 15; //LoanBalance
                

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, 10].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, 10].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Loan Summary"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Column Header

                cell = sheet.Cells[rowIndex, 2]; cell.Value = "SL"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 3]; cell.Value = "Department Name"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "Loan Amount"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 5]; cell.Value = "Inst. Deduction"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 6]; cell.Value = "Interest Deduction"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 7]; cell.Value = "Refund Amount"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 8]; cell.Value = "Refund Interest"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 9]; cell.Value = "Total Interest"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 10]; cell.Value = "Loan Balance"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

       
                rowIndex = rowIndex + 1;
                #endregion


                #region Table Body

                if (oLoanSummarys.Count() > 0)
                {

                    #region Table Part

                    int nCount = 0;
                    foreach (LoanSummary oItem in oLoanSummarys)
                    {
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = (++nCount).ToString(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.LoanAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.InstallmentDeduction; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.InterestDeduction; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.RefundAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.RefundCharge; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.TotalExtra; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem.LoanBalance; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                     
                        rowIndex++;
                    }
                    #endregion

                    #region Total

                    sheet.Cells[rowIndex, 2, rowIndex, 3].Merge = true;
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 3]; cell.Value = "Total"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = oLoanSummarys.Sum(x => x.LoanAmount); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = oLoanSummarys.Sum(x => x.InstallmentDeduction); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = oLoanSummarys.Sum(x => x.InterestDeduction); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = oLoanSummarys.Sum(x => x.RefundAmount); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = oLoanSummarys.Sum(x => x.RefundCharge); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = oLoanSummarys.Sum(x => x.TotalExtra); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = oLoanSummarys.Sum(x => x.LoanBalance); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

               
                    rowIndex++;
                    #endregion

                    #region Signature

                    rowIndex += 4;

                    sheet.Cells[rowIndex, 2, rowIndex, 3].Merge = true;
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 3]; cell.Value = "Prepared By"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    sheet.Cells[rowIndex, 4, rowIndex, 7].Merge = true;
                    cell = sheet.Cells[rowIndex, 4, rowIndex, 8]; cell.Value = "Checked By"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                    sheet.Cells[rowIndex, 8, rowIndex, 10].Merge = true;
                    cell = sheet.Cells[rowIndex, 8, rowIndex, 10]; cell.Value = "Approved By"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    rowIndex++;
                    #endregion
                }
                #endregion
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LoanSummary.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }



        #endregion
    }
}