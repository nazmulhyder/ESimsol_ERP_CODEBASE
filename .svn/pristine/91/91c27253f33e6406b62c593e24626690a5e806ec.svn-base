using System;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Drawing;
using ESimSol.Reports;
using System.Drawing.Imaging;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ReportManagement;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Globalization;

namespace ESimSolFinancial.Controllers
{
    public class EmployeeSalaryV2Controller : Controller
    {
        #region Actual Actions
        public ActionResult View_EmployeeSalarys(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<EmployeeSalaryV2> _oEmployeeSalaryV2s = new List<EmployeeSalaryV2>();

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);

            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalary).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.SalarySheetFormats = this.GetsPermittedSalarySheet(oAuthorizationRoleMappings,false);
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.SalaryFieldSetups = SalaryFieldSetup.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oEmployeeSalaryV2s);
        }

        public ActionResult PrintSalarySheet(double ts)
        {
            List<EmployeeSalaryV2> oEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailV2s = new List<EmployeeSalaryDetailV2>();
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailBasics = new List<EmployeeSalaryDetailV2>();
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailAllowances = new List<EmployeeSalaryDetailV2>();
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailDeductions = new List<EmployeeSalaryDetailV2>();
            List<SalarySheetSignature> oSalarySheetSignatures = new List<SalarySheetSignature>();
            List<LeaveStatus> oLeaveStatus = new List<LeaveStatus>();
            List<LeaveHead> oLeaveHeads = new List<LeaveHead>();
            HCMSearchObj oHCMSearchObj = new HCMSearchObj();
            Company oCompany = new Company();
            string _sErrorMesage = "";
            string sSQLEmpImg = "";
            try
            {
                oHCMSearchObj = (HCMSearchObj)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oHCMSearchObj);
                sSQLEmpImg = sSQL;
                oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                oEmployeeSalaryV2s = EmployeeSalaryV2.Gets(sSQL, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                oLeaveStatus = LeaveStatus.Gets(sSQL, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                sSQL = "SELECT * FROM VIEW_EmployeeSalaryDetail AS ESD WHERE ESD.EmployeeSalaryID IN (SELECT ES.EmployeeSalaryID FROM EmployeeSalary AS ES " + sSQL + ") ORDER BY ESD.EmployeeSalaryID, ESD.HeadSequence ASC";
                oEmployeeSalaryDetailV2s = EmployeeSalaryDetailV2.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                sSQL = "Select * from LeaveHead Order By LeaveHeadID";
                oLeaveHeads = LeaveHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oEmployeeSalaryDetailBasics = oEmployeeSalaryDetailV2s.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Basic).GroupBy(p => p.SalaryHeadID).Select(c => c.FirstOrDefault()).ToList().OrderBy(y => y.SalaryHeadSequence).ToList();
                oEmployeeSalaryDetailAllowances = oEmployeeSalaryDetailV2s.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Addition).GroupBy(p => p.SalaryHeadID).Select(c => c.FirstOrDefault()).ToList().OrderBy(y => y.SalaryHeadSequence).ToList();
                oEmployeeSalaryDetailDeductions = oEmployeeSalaryDetailV2s.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Deduction).GroupBy(p => p.SalaryHeadID).Select(c => c.FirstOrDefault()).ToList().OrderBy(y => y.SalaryHeadSequence).ToList();
                if (oEmployeeSalaryV2s.Count <= 0)
                {
                    rptErrorMessage oReport = new rptErrorMessage();
                    byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                    return File(abytes, "application/pdf");
                }
          
                    oEmployeeSalaryV2s = EmployeeSalaryV2.MapSalaryDetails(oEmployeeSalaryV2s, oEmployeeSalaryDetailV2s);
                    oEmployeeSalaryV2s = EmployeeSalaryV2.MapLeaveStatus(oEmployeeSalaryV2s, oLeaveStatus);
           
                sSQL = "Select * from SalarySheetSignature";
                oSalarySheetSignatures = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
                _sErrorMesage = ex.Message;
            }
            if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Salary_Sheet_F_01)
            {


                List<Employee> oEmployees = new List<Employee>();
                string sSql = "SELECT * FROM View_Employee_WithImage AS EWI WHERE EWI.EmployeeID IN (SELECT ES.EmployeeID FROM EmployeeSalary AS ES " + sSQLEmpImg + ")";
                oEmployees = Employee.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oEmployeeSalaryV2s = EmployeeSalaryV2.MapEmployeeImages(oEmployeeSalaryV2s, oEmployees);
                foreach (EmployeeSalaryV2 oEmployeeSalary in oEmployeeSalaryV2s)
                {
                    oEmployeeSalary.EmployeePhoto = GetEmployeePhoto(oEmployeeSalary);
                }
                rptSalarySheetF01 oReport = new rptSalarySheetF01();
                byte[] abytes = oReport.PrepareReport(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oCompany, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                return File(abytes, "application/pdf");
            }
            else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Salary_Sheet_F_02)
            {
                SalaryField oSalaryField = new SalaryField();
                SalaryFieldSetup oSalaryFieldSetup = new SalaryFieldSetup();
                List<SalaryFieldSetupDetail> oSalaryFieldSetupDetails = new List<SalaryFieldSetupDetail>();
                if (oHCMSearchObj.SalaryFieldSetupID > 0)
                {
                    oSalaryFieldSetup = oSalaryFieldSetup.Get(oHCMSearchObj.SalaryFieldSetupID, (int)Session[SessionInfo.currentUserID]);
                    oSalaryFieldSetupDetails = SalaryFieldSetupDetail.Gets(oHCMSearchObj.SalaryFieldSetupID, (int)(Session[SessionInfo.currentUserID]));
                    oSalaryField = SalaryField.MapSalaryFieldExists(oSalaryFieldSetupDetails);
                }
                rptSalarySheetF02 oReport = new rptSalarySheetF02();
                byte[] abytes = oReport.PrepareReport(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                return File(abytes, "application/pdf");
            }
          

            else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Pay_Slip_F_03)
            {
                SalaryField oSalaryField = new SalaryField();
                SalaryFieldSetup oSalaryFieldSetup = new SalaryFieldSetup();
                List<SalaryFieldSetupDetail> oSalaryFieldSetupDetails = new List<SalaryFieldSetupDetail>();
                if (oHCMSearchObj.SalaryFieldSetupID > 0)
                {
                    oSalaryFieldSetup = oSalaryFieldSetup.Get(oHCMSearchObj.SalaryFieldSetupID, (int)Session[SessionInfo.currentUserID]);
                    oSalaryFieldSetupDetails = SalaryFieldSetupDetail.Gets(oHCMSearchObj.SalaryFieldSetupID, (int)(Session[SessionInfo.currentUserID]));
                    oSalaryField = SalaryField.MapSalaryFieldExists(oSalaryFieldSetupDetails);
                }
                rptPaySlipF03 oReport = new rptPaySlipF03();
                byte[] abytes = oReport.PrepareReport(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                return File(abytes, "application/pdf");
            }
            else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Salary_Sheet_F_03)
            {
                SalaryField oSalaryField = new SalaryField();
                SalaryFieldSetup oSalaryFieldSetup = new SalaryFieldSetup();
                List<SalaryFieldSetupDetail> oSalaryFieldSetupDetails = new List<SalaryFieldSetupDetail>();
                if (oHCMSearchObj.SalaryFieldSetupID > 0)
                {
                    oSalaryFieldSetup = oSalaryFieldSetup.Get(oHCMSearchObj.SalaryFieldSetupID, (int)Session[SessionInfo.currentUserID]);
                    oSalaryFieldSetupDetails = SalaryFieldSetupDetail.Gets(oHCMSearchObj.SalaryFieldSetupID, (int)(Session[SessionInfo.currentUserID]));
                    oSalaryField = SalaryField.MapSalaryFieldExists(oSalaryFieldSetupDetails);
                }
                if (oSalaryFieldSetup.PageOrientation == EnumPageOrientation.A4_Landscape)
                {
                    rptSalarySheetF03 oReport = new rptSalarySheetF03();
                    byte[] abytes = oReport.PrepareReport(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                    return File(abytes, "application/pdf");
                }
                else if (oSalaryFieldSetup.PageOrientation == EnumPageOrientation.A4_Portrait)
                {
                    rptSalarySheetF03 oReport = new rptSalarySheetF03();
                    byte[] abytes = oReport.PrepareReportPortrait(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    _sErrorMesage = "Please Select A4_Portrat or Landscape";
                    rptErrorMessage oReport = new rptErrorMessage();
                    byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                    return File(abytes, "application/pdf");
                }
            }

            else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.BankSheet)
            {
                BankAccount oBankAccount = new BankAccount();
                if (Convert.ToInt32(oHCMSearchObj.BankAccountID) > 0)
                {
                    oBankAccount = oBankAccount.Get(Convert.ToInt32(oHCMSearchObj.BankAccountID), ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
                oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.BankPaymentAdvice, (int)Session[SessionInfo.currentUserID]);
                rptBankSheetV2 oReport = new rptBankSheetV2();
                byte[] abytes = oReport.PrepareReport(oHCMSearchObj.GroupBySerialID, oSignatureSetups, oEmployeeSalaryV2s, oHCMSearchObj.SelectedColNames, oHCMSearchObj.IsRound, oBankAccount, oHCMSearchObj.LetterInformationDate, oHCMSearchObj.PrintHeaderType, oCompany, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.HeaderHeightInch, oHCMSearchObj.FooterHeightInch, oHCMSearchObj.IsOT);
                return File(abytes, "application/pdf");

            }
            else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.CashSheet)
            {

                rptCashSheetV2 oReport = new rptCashSheetV2();
                byte[] abytes = oReport.PrepareReport(oEmployeeSalaryV2s,oCompany,oHCMSearchObj.IsOT);
                return File(abytes, "application/pdf");

            }
            return RedirectToAction("~/blank");
        }

        public void ExportToExcelSalarySheet(double ts)
        {
            List<EmployeeSalaryV2> oEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailV2s = new List<EmployeeSalaryDetailV2>();
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailBasics = new List<EmployeeSalaryDetailV2>();
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailAllowances = new List<EmployeeSalaryDetailV2>();
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailDeductions = new List<EmployeeSalaryDetailV2>();
            List<SalarySheetSignature> oSalarySheetSignatures = new List<SalarySheetSignature>();
            List<LeaveStatus> oLeaveStatus = new List<LeaveStatus>();
            List<LeaveHead> oLeaveHeads = new List<LeaveHead>();
            HCMSearchObj oHCMSearchObj = new HCMSearchObj();
            Company oCompany = new Company();
            try
            {
                oHCMSearchObj = (HCMSearchObj)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oHCMSearchObj);
                oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                oEmployeeSalaryV2s = EmployeeSalaryV2.Gets(sSQL, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                oLeaveStatus = LeaveStatus.Gets(sSQL, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                sSQL = "SELECT * FROM VIEW_EmployeeSalaryDetail AS ESD WHERE ESD.EmployeeSalaryID IN (SELECT ES.EmployeeSalaryID FROM EmployeeSalary AS ES " + sSQL + ") ORDER BY ESD.EmployeeSalaryID, ESD.HeadSequence ASC";
                oEmployeeSalaryDetailV2s = EmployeeSalaryDetailV2.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                sSQL = "Select * from LeaveHead Order By LeaveHeadID";
                oLeaveHeads = LeaveHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oEmployeeSalaryDetailBasics = oEmployeeSalaryDetailV2s.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Basic).GroupBy(p => p.SalaryHeadID).Select(c => c.FirstOrDefault()).ToList().OrderBy(y => y.SalaryHeadSequence).ToList();
                oEmployeeSalaryDetailAllowances = oEmployeeSalaryDetailV2s.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Addition).GroupBy(p => p.SalaryHeadID).Select(c => c.FirstOrDefault()).ToList().OrderBy(y => y.SalaryHeadSequence).ToList();
                oEmployeeSalaryDetailDeductions = oEmployeeSalaryDetailV2s.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Deduction).GroupBy(p => p.SalaryHeadID).Select(c => c.FirstOrDefault()).ToList().OrderBy(y => y.SalaryHeadSequence).ToList();
                if (oEmployeeSalaryV2s.Count <= 0)
                {
                    throw new Exception("Invalid Searching Criteria!");
                }
     
                    oEmployeeSalaryV2s = EmployeeSalaryV2.MapSalaryDetails(oEmployeeSalaryV2s, oEmployeeSalaryDetailV2s);
                    oEmployeeSalaryV2s = EmployeeSalaryV2.MapLeaveStatus(oEmployeeSalaryV2s, oLeaveStatus);
             
              
                sSQL = "Select * from SalarySheetSignature";
                oSalarySheetSignatures = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


                SalaryField oSalaryField = new SalaryField();
                SalaryFieldSetup oSalaryFieldSetup = new SalaryFieldSetup();
                List<SalaryFieldSetupDetail> oSalaryFieldSetupDetails = new List<SalaryFieldSetupDetail>();
                if (oHCMSearchObj.SalaryFieldSetupID > 0)
                {
                    oSalaryFieldSetup = oSalaryFieldSetup.Get(oHCMSearchObj.SalaryFieldSetupID, (int)Session[SessionInfo.currentUserID]);
                    oSalaryFieldSetupDetails = SalaryFieldSetupDetail.Gets(oHCMSearchObj.SalaryFieldSetupID, (int)(Session[SessionInfo.currentUserID]));
                    oSalaryField = SalaryField.MapSalaryFieldExists(oSalaryFieldSetupDetails);
                }

                if(oHCMSearchObj.PrintFormatInt==(int)EnumSalarySheetFormat.Salary_Sheet_F_01)
                {
                    this.CommonSalarySheetXL(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                }
                if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Salary_Sheet_F_02)
                {
                    this.SalarySheetXL(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                }
                else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Salary_Sheet_F_03)
                {

                    if (oSalaryFieldSetup.PageOrientation == EnumPageOrientation.A4_Landscape)
                    {
                        this.SalarySheetXLF3(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                    }

                    else if (oSalaryFieldSetup.PageOrientation == EnumPageOrientation.A4_Portrait)
                    {
                        this.SalarySheetXLF3Portrait(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                    }

                    else
                    {
                        throw new Exception("Select A4 Landscape Or Portrait");
                    }
                }

              
                else if(oHCMSearchObj.PrintFormatInt==(int)EnumSalarySheetFormat.Pay_Slip_F_01)
                {
                    this.PrintPaySlipXLF1(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oEmployeeSalaryDetailV2s, oCompany, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                }

                else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Pay_Slip_F_02)
                {
                    this.PrintPaySlipXLF2(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oEmployeeSalaryDetailV2s, oCompany, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                }
                else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Pay_Slip_F_03)
                {
                    this.PrintPaySlipXLF3(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                }

                else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.BankSheet)
                {
                    BankAccount oBankAccount = new BankAccount();
                    if (Convert.ToInt32(oHCMSearchObj.BankAccountID) > 0)
                    {
                        oBankAccount = oBankAccount.Get(Convert.ToInt32(oHCMSearchObj.BankAccountID), ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
                    oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.BankPaymentAdvice, (int)Session[SessionInfo.currentUserID]);
                    this.PrintBankSheet_XL(oHCMSearchObj.GroupBySerialID, oSignatureSetups, oEmployeeSalaryV2s, oHCMSearchObj.SelectedColNames, oHCMSearchObj.IsRound, oBankAccount, oHCMSearchObj.LetterInformationDate, oHCMSearchObj.PrintHeaderType, oCompany, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.HeaderHeightInch, oHCMSearchObj.FooterHeightInch, oHCMSearchObj.IsOT);
                }

                else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.CashSheet)
                {

                    this.CashSheet_XL(oEmployeeSalaryV2s, oCompany, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                }



            }
            catch (Exception ex)
            {
                #region Errormessage
                ExcelRange cell;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Salary Sheet");
                    sheet.Name = "Salary Sheet";

                    cell = sheet.Cells[2, 2, 2, 5]; cell.Merge = true; cell.Value = ex.Message; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=SalarySheet.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion

        #region Comp Actions
        public ActionResult View_CompEmployeeSalarys(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<EmployeeSalaryV2> _oEmployeeSalaryV2s = new List<EmployeeSalaryV2>();

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);

            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalary).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            List<MaxOTConfiguration> oMaxOTConfiguration = new List<MaxOTConfiguration>();
            oMaxOTConfiguration = MaxOTConfiguration.GetsByUser((int)(Session[SessionInfo.currentUserID]));
            ViewBag.SalarySheetFormats = this.GetsPermittedSalarySheet(oAuthorizationRoleMappings,true);
            ViewBag.TimeCards = oMaxOTConfiguration;
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.SalaryFieldSetups = SalaryFieldSetup.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oEmployeeSalaryV2s);
        }
        public ActionResult PrintCompSalarySheet(double ts)
        {
            List<EmployeeSalaryV2> oEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailV2s = new List<EmployeeSalaryDetailV2>();
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailBasics = new List<EmployeeSalaryDetailV2>();
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailAllowances = new List<EmployeeSalaryDetailV2>();
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailDeductions = new List<EmployeeSalaryDetailV2>();
            List<SalarySheetSignature> oSalarySheetSignatures = new List<SalarySheetSignature>();
            List<LeaveStatus> oLeaveStatus = new List<LeaveStatus>();
            List<LeaveHead> oLeaveHeads = new List<LeaveHead>();
            HCMSearchObj oHCMSearchObj = new HCMSearchObj();
            MaxOTConfiguration oMaxOTConfiguration = new MaxOTConfiguration();
            Company oCompany = new Company();
            string _sErrorMesage = "";
            string sSQLEmpImg = "";
            try
            {
                oHCMSearchObj = (HCMSearchObj)Session[SessionInfo.ParamObj];
                oMaxOTConfiguration = oMaxOTConfiguration.Get(oHCMSearchObj.MOCID, (int)Session[SessionInfo.currentUserID]);
                string sSQL = "";
                if (oMaxOTConfiguration.IsActive == false && oMaxOTConfiguration.SourceTimeCardID > 0)
                {
                    oHCMSearchObj.MOCID = oMaxOTConfiguration.SourceTimeCardID;
                    sSQL = this.CompGetSQL(oHCMSearchObj);
                    sSQLEmpImg = sSQL;
                    oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                    oEmployeeSalaryV2s = EmployeeSalaryV2.CompGets(sSQL, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                    oLeaveStatus = LeaveStatus.CompGets(sSQL, oHCMSearchObj.MOCID, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                    sSQL = "SELECT * FROM View_ComplianceEmployeeSalaryDetail AS ESD WHERE ESD.EmployeeSalaryID IN (SELECT ES.EmployeeSalaryID FROM ComplianceEmployeeSalary AS ES " + sSQL + ") ORDER BY ESD.EmployeeSalaryID, ESD.HeadSequence ASC";
                    oEmployeeSalaryDetailV2s = EmployeeSalaryDetailV2.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                    sSQL = "Select * from LeaveHead Order By LeaveHeadID";
                    oLeaveHeads = LeaveHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                else if(oMaxOTConfiguration.IsActive==false && oMaxOTConfiguration.SourceTimeCardID==0)
                {
                    sSQL = this.GetSQL(oHCMSearchObj);
                    sSQLEmpImg = sSQL;
                    oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                    oEmployeeSalaryV2s = EmployeeSalaryV2.Gets(sSQL, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                    oLeaveStatus = LeaveStatus.Gets(sSQL, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                    sSQL = "SELECT * FROM View_EmployeeSalaryDetail AS ESD WHERE ESD.EmployeeSalaryID IN (SELECT ES.EmployeeSalaryID FROM EmployeeSalary AS ES " + sSQL + ") ORDER BY ESD.EmployeeSalaryID, ESD.HeadSequence ASC";
                    oEmployeeSalaryDetailV2s = EmployeeSalaryDetailV2.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                    sSQL = "Select * from LeaveHead Order By LeaveHeadID";
                    oLeaveHeads = LeaveHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (EmployeeSalaryV2 oItem in oEmployeeSalaryV2s)
                    {
                        oItem.ExtraOTHour = oItem.ExtraOTHour + oItem.DayOfRegularOTHour;
                    }
                }

                else
                {
                    sSQL = this.CompGetSQL(oHCMSearchObj);
                    sSQLEmpImg = sSQL;
                    oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                    oEmployeeSalaryV2s = EmployeeSalaryV2.CompGets(sSQL, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                    oLeaveStatus = LeaveStatus.CompGets(sSQL, oHCMSearchObj.MOCID, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                    sSQL = "SELECT * FROM View_ComplianceEmployeeSalaryDetail AS ESD WHERE ESD.EmployeeSalaryID IN (SELECT ES.EmployeeSalaryID FROM ComplianceEmployeeSalary AS ES " + sSQL + ") ORDER BY ESD.EmployeeSalaryID, ESD.HeadSequence ASC";
                    oEmployeeSalaryDetailV2s = EmployeeSalaryDetailV2.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                    sSQL = "Select * from LeaveHead Order By LeaveHeadID";
                    oLeaveHeads = LeaveHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
               

                oEmployeeSalaryDetailBasics = oEmployeeSalaryDetailV2s.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Basic).GroupBy(p => p.SalaryHeadID).Select(c => c.FirstOrDefault()).ToList().OrderBy(y => y.SalaryHeadSequence).ToList();
                oEmployeeSalaryDetailAllowances = oEmployeeSalaryDetailV2s.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Addition).GroupBy(p => p.SalaryHeadID).Select(c => c.FirstOrDefault()).ToList().OrderBy(y => y.SalaryHeadSequence).ToList();
                oEmployeeSalaryDetailDeductions = oEmployeeSalaryDetailV2s.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Deduction).GroupBy(p => p.SalaryHeadID).Select(c => c.FirstOrDefault()).ToList().OrderBy(y => y.SalaryHeadSequence).ToList();
                if (oEmployeeSalaryV2s.Count <= 0)
                {
                    rptErrorMessage oReport = new rptErrorMessage();
                    byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                    return File(abytes, "application/pdf");
                }

                if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Salary_Sheet_F_04 || oHCMSearchObj.PrintFormatInt==(int)EnumSalarySheetFormat.Salary_Sheet_F_05)
                {
                    oEmployeeSalaryV2s = EmployeeSalaryV2.MapAMGSalaryDetails(oEmployeeSalaryV2s, oEmployeeSalaryDetailV2s);
                    oEmployeeSalaryV2s = EmployeeSalaryV2.MapAMGLeaveStatus(oEmployeeSalaryV2s, oLeaveStatus);
                }
                else
                {
                    oEmployeeSalaryV2s = EmployeeSalaryV2.MapSalaryDetails(oEmployeeSalaryV2s, oEmployeeSalaryDetailV2s);
                    oEmployeeSalaryV2s = EmployeeSalaryV2.MapLeaveStatus(oEmployeeSalaryV2s, oLeaveStatus);
                }
                sSQL = "Select * from SalarySheetSignature";
                oSalarySheetSignatures = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                //oEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
                _sErrorMesage = ex.Message;
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
            if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Salary_Sheet_F_01)
            {


                List<Employee> oEmployees = new List<Employee>();
                string sSql = "SELECT * FROM View_Employee_WithImage AS EWI WHERE EWI.EmployeeID IN (SELECT ES.EmployeeID FROM ComplianceEmployeeSalary AS ES " + sSQLEmpImg + ")";
                oEmployees = Employee.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oEmployeeSalaryV2s = EmployeeSalaryV2.MapEmployeeImages(oEmployeeSalaryV2s, oEmployees);
                foreach (EmployeeSalaryV2 oEmployeeSalary in oEmployeeSalaryV2s)
                {
                    oEmployeeSalary.EmployeePhoto = GetEmployeePhoto(oEmployeeSalary);
                }
                rptSalarySheetF01 oReport = new rptSalarySheetF01();
                byte[] abytes = oReport.PrepareReport(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oCompany, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                return File(abytes, "application/pdf");
            }
            else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Salary_Sheet_F_02)
            {
                SalaryField oSalaryField = new SalaryField();
                SalaryFieldSetup oSalaryFieldSetup = new SalaryFieldSetup();
                List<SalaryFieldSetupDetail> oSalaryFieldSetupDetails = new List<SalaryFieldSetupDetail>();
                if (oHCMSearchObj.SalaryFieldSetupID > 0)
                {
                    oSalaryFieldSetup = oSalaryFieldSetup.Get(oHCMSearchObj.SalaryFieldSetupID, (int)Session[SessionInfo.currentUserID]);
                    oSalaryFieldSetupDetails = SalaryFieldSetupDetail.Gets(oHCMSearchObj.SalaryFieldSetupID, (int)(Session[SessionInfo.currentUserID]));
                    oSalaryField = SalaryField.MapSalaryFieldExists(oSalaryFieldSetupDetails);
                }
                rptSalarySheetF02 oReport = new rptSalarySheetF02();
                byte[] abytes = oReport.PrepareReport(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                return File(abytes, "application/pdf");
            }

            else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Salary_Sheet_F_04)
            {
                rptSalarySheetF04 oReport = new rptSalarySheetF04();
                byte[] abytes = oReport.PrepareReport(oEmployeeSalaryV2s, oCompany, oHCMSearchObj.SalaryStartDate.ToString("MMMM yyyy"), oHCMSearchObj.SalaryStartDate);
                return File(abytes, "application/pdf");
            }

            else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Salary_Sheet_F_05)
            {
                rptSalarySheetF05 oReport = new rptSalarySheetF05();
                byte[] abytes = oReport.PrepareReport(oEmployeeSalaryV2s, oCompany, oHCMSearchObj.SalaryStartDate.ToString("MMMM yyyy"), oHCMSearchObj.SalaryStartDate);
                return File(abytes, "application/pdf");
            }
            else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Pay_Slip_F_03)
            {
                SalaryField oSalaryField = new SalaryField();
                SalaryFieldSetup oSalaryFieldSetup = new SalaryFieldSetup();
                List<SalaryFieldSetupDetail> oSalaryFieldSetupDetails = new List<SalaryFieldSetupDetail>();
                if (oHCMSearchObj.SalaryFieldSetupID > 0)
                {
                    oSalaryFieldSetup = oSalaryFieldSetup.Get(oHCMSearchObj.SalaryFieldSetupID, (int)Session[SessionInfo.currentUserID]);
                    oSalaryFieldSetupDetails = SalaryFieldSetupDetail.Gets(oHCMSearchObj.SalaryFieldSetupID, (int)(Session[SessionInfo.currentUserID]));
                    oSalaryField = SalaryField.MapSalaryFieldExists(oSalaryFieldSetupDetails);
                }
                rptPaySlipF03 oReport = new rptPaySlipF03();
                byte[] abytes = oReport.PrepareReport(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                return File(abytes, "application/pdf");
            }
            else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Salary_Sheet_F_03)
            {
                SalaryField oSalaryField = new SalaryField();
                SalaryFieldSetup oSalaryFieldSetup = new SalaryFieldSetup();
                List<SalaryFieldSetupDetail> oSalaryFieldSetupDetails = new List<SalaryFieldSetupDetail>();
                if (oHCMSearchObj.SalaryFieldSetupID > 0)
                {
                    oSalaryFieldSetup = oSalaryFieldSetup.Get(oHCMSearchObj.SalaryFieldSetupID, (int)Session[SessionInfo.currentUserID]);
                    oSalaryFieldSetupDetails = SalaryFieldSetupDetail.Gets(oHCMSearchObj.SalaryFieldSetupID, (int)(Session[SessionInfo.currentUserID]));
                    oSalaryField = SalaryField.MapSalaryFieldExists(oSalaryFieldSetupDetails);
                }
                if(oSalaryFieldSetup.PageOrientation==EnumPageOrientation.A4_Landscape)
                {
                    rptSalarySheetF03 oReport = new rptSalarySheetF03();
                    byte[] abytes = oReport.PrepareReport(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                    return File(abytes, "application/pdf");
                }
                else if(oSalaryFieldSetup.PageOrientation==EnumPageOrientation.A4_Portrait)
                {
                    rptSalarySheetF03 oReport = new rptSalarySheetF03();
                    byte[] abytes = oReport.PrepareReportPortrait(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    _sErrorMesage ="Please Select A4_Portrat or Landscape";
                    rptErrorMessage oReport = new rptErrorMessage();
                    byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                    return File(abytes, "application/pdf");
                }
              
            }

            else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.BankSheet)
            {
                BankAccount oBankAccount = new BankAccount();
                if (Convert.ToInt32(oHCMSearchObj.BankAccountID) > 0)
                {
                    oBankAccount = oBankAccount.Get(Convert.ToInt32(oHCMSearchObj.BankAccountID), ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
                oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.BankPaymentAdvice, (int)Session[SessionInfo.currentUserID]);
                rptBankSheetV2 oReport = new rptBankSheetV2();
                byte[] abytes = oReport.PrepareReport(oHCMSearchObj.GroupBySerialID, oSignatureSetups, oEmployeeSalaryV2s, oHCMSearchObj.SelectedColNames, oHCMSearchObj.IsRound, oBankAccount, oHCMSearchObj.LetterInformationDate, oHCMSearchObj.PrintHeaderType, oCompany, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.HeaderHeightInch, oHCMSearchObj.FooterHeightInch, oHCMSearchObj.IsOT);
                return File(abytes, "application/pdf");

            }

            else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.CashSheet)
            {

                rptCashSheetV2 oReport = new rptCashSheetV2();
                byte[] abytes = oReport.PrepareReport(oEmployeeSalaryV2s, oCompany, oHCMSearchObj.IsOT);
                return File(abytes, "application/pdf");

            }
            return RedirectToAction("~/blank");
        }
        public void ExportToExcelCompSalarySheet(double ts)
        {
            List<EmployeeSalaryV2> oEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailV2s = new List<EmployeeSalaryDetailV2>();
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailBasics = new List<EmployeeSalaryDetailV2>();
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailAllowances = new List<EmployeeSalaryDetailV2>();
            List<EmployeeSalaryDetailV2> oEmployeeSalaryDetailDeductions = new List<EmployeeSalaryDetailV2>();
            List<SalarySheetSignature> oSalarySheetSignatures = new List<SalarySheetSignature>();
            List<LeaveStatus> oLeaveStatus = new List<LeaveStatus>();
            List<LeaveHead> oLeaveHeads = new List<LeaveHead>();
            HCMSearchObj oHCMSearchObj = new HCMSearchObj();
            MaxOTConfiguration oMaxOTConfiguration = new MaxOTConfiguration();
            Company oCompany = new Company();
            try
            {
                oHCMSearchObj = (HCMSearchObj)Session[SessionInfo.ParamObj];
                oMaxOTConfiguration = oMaxOTConfiguration.Get(oHCMSearchObj.MOCID, (int)Session[SessionInfo.currentUserID]);
                string sSQL = "";
                if (oMaxOTConfiguration.IsActive == false && oMaxOTConfiguration.SourceTimeCardID > 0)
                {
                    oHCMSearchObj.MOCID = oMaxOTConfiguration.SourceTimeCardID;
                    sSQL = this.CompGetSQL(oHCMSearchObj);
                    oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                    oEmployeeSalaryV2s = EmployeeSalaryV2.CompGets(sSQL, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                    oLeaveStatus = LeaveStatus.CompGets(sSQL, oHCMSearchObj.MOCID, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                    sSQL = "SELECT * FROM View_ComplianceEmployeeSalaryDetail AS ESD WHERE ESD.EmployeeSalaryID IN (SELECT ES.EmployeeSalaryID FROM ComplianceEmployeeSalary AS ES " + sSQL + ") ORDER BY ESD.EmployeeSalaryID, ESD.HeadSequence ASC";
                    oEmployeeSalaryDetailV2s = EmployeeSalaryDetailV2.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                    sSQL = "Select * from LeaveHead Order By LeaveHeadID";
                    oLeaveHeads = LeaveHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                else if (oMaxOTConfiguration.IsActive == false && oMaxOTConfiguration.SourceTimeCardID == 0)
                {
                    sSQL = this.GetSQL(oHCMSearchObj);
                    oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                    oEmployeeSalaryV2s = EmployeeSalaryV2.Gets(sSQL, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                    oLeaveStatus = LeaveStatus.Gets(sSQL, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                    sSQL = "SELECT * FROM View_EmployeeSalaryDetail AS ESD WHERE ESD.EmployeeSalaryID IN (SELECT ES.EmployeeSalaryID FROM EmployeeSalary AS ES " + sSQL + ") ORDER BY ESD.EmployeeSalaryID, ESD.HeadSequence ASC";
                    oEmployeeSalaryDetailV2s = EmployeeSalaryDetailV2.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                    sSQL = "Select * from LeaveHead Order By LeaveHeadID";
                    oLeaveHeads = LeaveHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    foreach (EmployeeSalaryV2 oItem in oEmployeeSalaryV2s)
                    {
                        oItem.ExtraOTHour = oItem.ExtraOTHour + oItem.DayOfRegularOTHour;
                    }
                }

                else
                {
                    sSQL = this.CompGetSQL(oHCMSearchObj);
                    oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                    oEmployeeSalaryV2s = EmployeeSalaryV2.CompGets(sSQL, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                    oLeaveStatus = LeaveStatus.CompGets(sSQL, oHCMSearchObj.MOCID, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                    sSQL = "SELECT * FROM View_ComplianceEmployeeSalaryDetail AS ESD WHERE ESD.EmployeeSalaryID IN (SELECT ES.EmployeeSalaryID FROM ComplianceEmployeeSalary AS ES " + sSQL + ") ORDER BY ESD.EmployeeSalaryID, ESD.HeadSequence ASC";
                    oEmployeeSalaryDetailV2s = EmployeeSalaryDetailV2.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                    sSQL = "Select * from LeaveHead Order By LeaveHeadID";
                    oLeaveHeads = LeaveHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                oEmployeeSalaryDetailBasics = oEmployeeSalaryDetailV2s.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Basic).GroupBy(p => p.SalaryHeadID).Select(c => c.FirstOrDefault()).ToList().OrderBy(y => y.SalaryHeadSequence).ToList();
                oEmployeeSalaryDetailAllowances = oEmployeeSalaryDetailV2s.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Addition).GroupBy(p => p.SalaryHeadID).Select(c => c.FirstOrDefault()).ToList().OrderBy(y => y.SalaryHeadSequence).ToList();
                oEmployeeSalaryDetailDeductions = oEmployeeSalaryDetailV2s.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Deduction).GroupBy(p => p.SalaryHeadID).Select(c => c.FirstOrDefault()).ToList().OrderBy(y => y.SalaryHeadSequence).ToList();
                if (oEmployeeSalaryV2s.Count <= 0)
                {
                    throw new Exception("Invalid Searching Criteria!");
                }
                if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Salary_Sheet_F_04 || oHCMSearchObj.PrintFormatInt==(int)EnumSalarySheetFormat.Salary_Sheet_F_05)
                {
                    oEmployeeSalaryV2s = EmployeeSalaryV2.MapAMGSalaryDetails(oEmployeeSalaryV2s, oEmployeeSalaryDetailV2s);
                    oEmployeeSalaryV2s = EmployeeSalaryV2.MapAMGLeaveStatus(oEmployeeSalaryV2s, oLeaveStatus);
                }
                else
                {
                    oEmployeeSalaryV2s = EmployeeSalaryV2.MapSalaryDetails(oEmployeeSalaryV2s, oEmployeeSalaryDetailV2s);
                    oEmployeeSalaryV2s = EmployeeSalaryV2.MapLeaveStatus(oEmployeeSalaryV2s, oLeaveStatus);
                }
                sSQL = "Select * from SalarySheetSignature";
                oSalarySheetSignatures = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                SalaryField oSalaryField = new SalaryField();
                SalaryFieldSetup oSalaryFieldSetup = new SalaryFieldSetup();
                List<SalaryFieldSetupDetail> oSalaryFieldSetupDetails = new List<SalaryFieldSetupDetail>();
                if (oHCMSearchObj.SalaryFieldSetupID > 0)
                {
                    oSalaryFieldSetup = oSalaryFieldSetup.Get(oHCMSearchObj.SalaryFieldSetupID, (int)Session[SessionInfo.currentUserID]);
                    oSalaryFieldSetupDetails = SalaryFieldSetupDetail.Gets(oHCMSearchObj.SalaryFieldSetupID, (int)(Session[SessionInfo.currentUserID]));
                    oSalaryField = SalaryField.MapSalaryFieldExists(oSalaryFieldSetupDetails);
                }
                if(oHCMSearchObj.PrintFormatInt==(int)EnumSalarySheetFormat.Salary_Sheet_F_01)
                {
                    this.CommonSalarySheetXL(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                }
                if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Salary_Sheet_F_02)
                {
                    this.SalarySheetXL(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                }
                else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Salary_Sheet_F_03)
                {
                    if(oSalaryFieldSetup.PageOrientation==EnumPageOrientation.A4_Landscape)
                    {
                        this.SalarySheetXLF3(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                    }

                    else if(oSalaryFieldSetup.PageOrientation==EnumPageOrientation.A4_Portrait)
                    {
                        this.SalarySheetXLF3Portrait(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                    }

                    else
                    {
                        throw new Exception("Select A4 Landscape Or Portrait");
                    }
                }
                else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Salary_Sheet_F_04)
                {
                    this.ExportIntoExcelSalarySheet_F4(oEmployeeSalaryV2s, oCompany, oHCMSearchObj.SalaryStartDate.ToString("MMMM yyyy"), oHCMSearchObj.SalaryStartDate);
                }
                else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Salary_Sheet_F_05)
                {
                    this.ExportIntoExcelSalarySheet_F5(oEmployeeSalaryV2s, oCompany, oHCMSearchObj.SalaryStartDate.ToString("MMMM yyyy"), oHCMSearchObj.SalaryStartDate);
                }

                else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Pay_Slip_F_01)
                {
                    this.PrintPaySlipXLF1(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oEmployeeSalaryDetailV2s, oCompany, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                }

                else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Pay_Slip_F_02)
                {
                    this.PrintPaySlipXLF2(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oEmployeeSalaryDetailV2s, oCompany, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                }
                else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.Pay_Slip_F_03)
                {
                    this.PrintPaySlipXLF3(oHCMSearchObj.GroupBySerialID, oEmployeeSalaryV2s, oLeaveHeads, oEmployeeSalaryDetailBasics, oEmployeeSalaryDetailAllowances, oEmployeeSalaryDetailDeductions, oCompany, oSalaryField, oSalaryFieldSetup.PageOrientation, oSalarySheetSignatures, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                }

                else if(oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.BankSheet)
                {
                    BankAccount oBankAccount = new BankAccount();
                    if (Convert.ToInt32(oHCMSearchObj.BankAccountID) > 0)
                    {
                        oBankAccount = oBankAccount.Get(Convert.ToInt32(oHCMSearchObj.BankAccountID), ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
                    oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.BankPaymentAdvice, (int)Session[SessionInfo.currentUserID]);
                    this.PrintBankSheet_XL(oHCMSearchObj.GroupBySerialID, oSignatureSetups, oEmployeeSalaryV2s, oHCMSearchObj.SelectedColNames, oHCMSearchObj.IsRound, oBankAccount, oHCMSearchObj.LetterInformationDate, oHCMSearchObj.PrintHeaderType, oCompany, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.HeaderHeightInch, oHCMSearchObj.FooterHeightInch, oHCMSearchObj.IsOT);
                }

                else if (oHCMSearchObj.PrintFormatInt == (int)EnumSalarySheetFormat.CashSheet)
                {

                    this.CashSheet_XL(oEmployeeSalaryV2s, oCompany, oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy"), oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy"), oHCMSearchObj.IsOT);
                }

            }
            catch (Exception ex)
            {
                #region Errormessage
                ExcelRange cell;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Salary Sheet");
                    sheet.Name = "Salary Sheet";

                    cell = sheet.Cells[2, 2, 2, 5]; cell.Merge = true; cell.Value = ex.Message; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=SalarySheet.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion
        

        #region Utility Functions
        private List<EnumObject> GetsPermittedSalarySheet(List<AuthorizationRoleMapping> oAuthorizationRoleMappings,bool IsCompliance)
        {
            EnumObject oSalarySheetFormat = new EnumObject();
            List<EnumObject> oSalarySheetFormats = new List<EnumObject>();


            foreach (AuthorizationRoleMapping oItem in oAuthorizationRoleMappings)
            {
                if (oItem.OperationType == EnumRoleOperationType.Salary_Sheet_F_01)
                {
                    oSalarySheetFormat = new EnumObject();
                    oSalarySheetFormat.id = (int)EnumSalarySheetFormat.Salary_Sheet_F_01;
                    oSalarySheetFormat.Value = EnumObject.jGet(EnumSalarySheetFormat.Salary_Sheet_F_01);
                    oSalarySheetFormats.Add(oSalarySheetFormat);
                }
                else if (oItem.OperationType == EnumRoleOperationType.Salary_Sheet_F_02)
                {
                    oSalarySheetFormat = new EnumObject();
                    oSalarySheetFormat.id = (int)EnumSalarySheetFormat.Salary_Sheet_F_02;
                    oSalarySheetFormat.Value = EnumObject.jGet(EnumSalarySheetFormat.Salary_Sheet_F_02);
                    oSalarySheetFormats.Add(oSalarySheetFormat);
                }
                else if (oItem.OperationType == EnumRoleOperationType.Salary_Sheet_F_03)
                {
                    oSalarySheetFormat = new EnumObject();
                    oSalarySheetFormat.id = (int)EnumSalarySheetFormat.Salary_Sheet_F_03;
                    oSalarySheetFormat.Value = EnumObject.jGet(EnumSalarySheetFormat.Salary_Sheet_F_03);
                    oSalarySheetFormats.Add(oSalarySheetFormat);
                }
                else if (oItem.OperationType == EnumRoleOperationType.Pay_Slip_F_01)
                {
                    oSalarySheetFormat = new EnumObject();
                    oSalarySheetFormat.id = (int)EnumSalarySheetFormat.Pay_Slip_F_01;
                    oSalarySheetFormat.Value = EnumObject.jGet(EnumSalarySheetFormat.Pay_Slip_F_01);
                    oSalarySheetFormats.Add(oSalarySheetFormat);
                }
                else if (oItem.OperationType == EnumRoleOperationType.Pay_Slip_F_02)
                {
                    oSalarySheetFormat = new EnumObject();
                    oSalarySheetFormat.id = (int)EnumSalarySheetFormat.Pay_Slip_F_02;
                    oSalarySheetFormat.Value = EnumObject.jGet(EnumSalarySheetFormat.Pay_Slip_F_02);
                    oSalarySheetFormats.Add(oSalarySheetFormat);
                }
                else if (oItem.OperationType == EnumRoleOperationType.Pay_Slip_F_03)
                {
                    oSalarySheetFormat = new EnumObject();
                    oSalarySheetFormat.id = (int)EnumSalarySheetFormat.Pay_Slip_F_03;
                    oSalarySheetFormat.Value = EnumObject.jGet(EnumSalarySheetFormat.Pay_Slip_F_03);
                    oSalarySheetFormats.Add(oSalarySheetFormat);
                }
                else if (oItem.OperationType == EnumRoleOperationType.BankSheet)
                {
                    oSalarySheetFormat = new EnumObject();
                    oSalarySheetFormat.id = (int)EnumSalarySheetFormat.BankSheet;
                    oSalarySheetFormat.Value = EnumObject.jGet(EnumSalarySheetFormat.BankSheet);
                    oSalarySheetFormats.Add(oSalarySheetFormat);
                }
                else if (oItem.OperationType == EnumRoleOperationType.CashSheet)
                {
                    oSalarySheetFormat = new EnumObject();
                    oSalarySheetFormat.id = (int)EnumSalarySheetFormat.CashSheet;
                    oSalarySheetFormat.Value = EnumObject.jGet(EnumSalarySheetFormat.CashSheet);
                    oSalarySheetFormats.Add(oSalarySheetFormat);
                }
                   
                else if (oItem.OperationType == EnumRoleOperationType.Salary_Sheet_F_04)
                {
                    if (IsCompliance)
                    {
                        oSalarySheetFormat = new EnumObject();
                        oSalarySheetFormat.id = (int)EnumSalarySheetFormat.Salary_Sheet_F_04;
                        oSalarySheetFormat.Value = EnumObject.jGet(EnumSalarySheetFormat.Salary_Sheet_F_04);
                        oSalarySheetFormats.Add(oSalarySheetFormat);
                    }
                 
                }
                else if (oItem.OperationType == EnumRoleOperationType.Salary_Sheet_F_05)
                {
                    if (IsCompliance)
                    {
                        oSalarySheetFormat = new EnumObject();
                        oSalarySheetFormat.id = (int)EnumSalarySheetFormat.Salary_Sheet_F_05;
                        oSalarySheetFormat.Value = EnumObject.jGet(EnumSalarySheetFormat.Salary_Sheet_F_05);
                        oSalarySheetFormats.Add(oSalarySheetFormat);
                    }

                }
            }

            return oSalarySheetFormats.OrderBy(x => x.Value).ToList();
        }

        private string GetSQL(HCMSearchObj oHCMSearchObj)
        {

            string sSQL = " WHERE ES.StartDate <='" + oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy") + "' AND ES.EndDate>='" + oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy") + "'";
            if (oHCMSearchObj.EmployeeIDs != "" && oHCMSearchObj.EmployeeIDs != null)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT Emp.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.EmployeeIDs + "', ',') AS Emp)";
            }
            if (oHCMSearchObj.LocationIDs != "" && oHCMSearchObj.LocationIDs != null)
            {
                sSQL = sSQL + " AND ES.LocationID IN (SELECT LOC.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.LocationIDs + "', ',') AS LOC)";
            }
            if (oHCMSearchObj.DepartmentIDs != "" && oHCMSearchObj.DepartmentIDs != null)
            {
                sSQL = sSQL + " AND ES.DepartmentID IN (SELECT DEPT.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.DepartmentIDs + "', ',') AS DEPT)";
            }
            if (oHCMSearchObj.DesignationIDs != "" && oHCMSearchObj.DesignationIDs != null)
            {
                sSQL = sSQL + " AND ES.DesignationID IN (SELECT Desg.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.DesignationIDs + "', ',') AS Desg)";
            }
            if (oHCMSearchObj.BUIDs != "" && oHCMSearchObj.BUIDs != null)
            {
                sSQL = sSQL + " AND ES.BUID IN (SELECT BU.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.BUIDs + "', ',') AS BU)";
            }
            if (oHCMSearchObj.BlockIDs != "" && oHCMSearchObj.BlockIDs != null)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.BlockIDs + "))";
            }
            if (oHCMSearchObj.GroupIDs != "" && oHCMSearchObj.GroupIDs != null)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.GroupIDs + "))";
            }
            if (oHCMSearchObj.EmployeeTypeIDs != "" && oHCMSearchObj.EmployeeTypeIDs != null)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.EmployeeTypeIDs + "))";
            }
            if (oHCMSearchObj.ShiftIDs != "" && oHCMSearchObj.ShiftIDs != null)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.CurrentShiftID IN (" + oHCMSearchObj.ShiftIDs + "))";
            }
            if (oHCMSearchObj.SalarySchemeIDs != "" && oHCMSearchObj.SalarySchemeIDs != null)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT ESS.EmployeeID FROM EmployeeSalaryStructure AS ESS WHERE ESS.SalarySchemeID IN (" + oHCMSearchObj.SalarySchemeIDs + "))";
            }
            if (oHCMSearchObj.AttendanceSchemeIDs != "" && oHCMSearchObj.AttendanceSchemeIDs != null)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.AttendanceSchemeID IN (" + oHCMSearchObj.AttendanceSchemeIDs + "))";
            }
            if (oHCMSearchObj.CategoryID != 0)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EC.EmployeeID FROM EmployeeConfirmation AS EC WHERE EO.EmployeeCategory=" + oHCMSearchObj.CategoryID + ")";
            }
            if (oHCMSearchObj.AuthenticationNo != "" && oHCMSearchObj.AuthenticationNo != null)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EA.EmployeeID FROM EmployeeAuthentication AS EA WHERE EA.IsActive =1 AND EA.CardNo LIKE '%" + oHCMSearchObj.AuthenticationNo + "%')";
            }
            if (oHCMSearchObj.StartSalaryRange != 0 && oHCMSearchObj.EndSalaryRange != 0)
            {
                sSQL = sSQL + " AND ES.GrossAmount BETWEEN " + oHCMSearchObj.StartSalaryRange.ToString() + " AND " + oHCMSearchObj.EndSalaryRange.ToString();
            }
            if (oHCMSearchObj.BankAccountID > 0 && oHCMSearchObj.IsSearchByBank==true)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN ( SELECT EBA.EmployeeID FROM EmployeeBankAccount AS EBA WHERE EBA.BankBranchID IN (SELECT BA.BankBranchID FROM BankAccount AS BA WHERE BA.BankID=(SELECT BAA.BankID FROM BankAccount AS BAA WHERE BAA.BankAccountID="+oHCMSearchObj.BankAccountID+")))";
            }
            if (oHCMSearchObj.PaymentTypeID > 0)
            {
                if (oHCMSearchObj.PaymentTypeID == 1)
                {
                    sSQL = sSQL + " AND ES.EmployeeID IN ( SELECT ESS.EmployeeID FROM EmployeeSalaryStructure AS ESS WHERE ESS.IsAllowBankAccount=1)";
                }
                if (oHCMSearchObj.PaymentTypeID == 2)
                {
                    sSQL = sSQL + " AND ES.EmployeeID IN ( SELECT ESS.EmployeeID FROM EmployeeSalaryStructure AS ESS WHERE ESS.IsAllowBankAccount=0)";
                }
            }

            if (oHCMSearchObj.IsNewJoin)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.JoiningDate BETWEEN ES.StartDate AND ES.EndDate)";
            }

            if (oHCMSearchObj.IsJoiningDate == true)
            {

                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.DateOfJoin BETWEEN '" + oHCMSearchObj.JoiningStartDate.ToString("dd MMM yyyy") + "' AND '" + oHCMSearchObj.JoiningEndDate.ToString("dd MMM yyyy") + "')";
            }

            return sSQL;
        }
        private string CompGetSQL(HCMSearchObj oHCMSearchObj)
        {

            string sSQL = " WHERE ES.MOCID=" + oHCMSearchObj.MOCID + " AND ES.StartDate <='" + oHCMSearchObj.SalaryStartDate.ToString("dd MMM yyyy") + "' AND ES.EndDate>='" + oHCMSearchObj.SalaryEndDate.ToString("dd MMM yyyy") + "'";
            if (oHCMSearchObj.EmployeeIDs != "" && oHCMSearchObj.EmployeeIDs != null)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT Emp.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.EmployeeIDs + "', ',') AS Emp)";
            }
            if (oHCMSearchObj.LocationIDs != "" && oHCMSearchObj.LocationIDs != null)
            {
                sSQL = sSQL + " AND ES.LocationID IN (SELECT LOC.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.LocationIDs + "', ',') AS LOC)";
            }
            if (oHCMSearchObj.DepartmentIDs != "" && oHCMSearchObj.DepartmentIDs != null)
            {
                sSQL = sSQL + " AND ES.DepartmentID IN (SELECT DEPT.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.DepartmentIDs + "', ',') AS DEPT)";
            }
            if (oHCMSearchObj.DesignationIDs != "" && oHCMSearchObj.DesignationIDs != null)
            {
                sSQL = sSQL + " AND ES.DesignationID IN (SELECT Desg.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.DesignationIDs + "', ',') AS Desg)";
            }
            if (oHCMSearchObj.BUIDs != "" && oHCMSearchObj.BUIDs != null)
            {
                sSQL = sSQL + " AND ES.BUID IN (SELECT BU.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.BUIDs + "', ',') AS BU)";
            }
            if (oHCMSearchObj.BlockIDs != "" && oHCMSearchObj.BlockIDs != null)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.BlockIDs + "))";
            }
            if (oHCMSearchObj.GroupIDs != "" && oHCMSearchObj.GroupIDs != null)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.GroupIDs + "))";
            }
            if (oHCMSearchObj.EmployeeTypeIDs != "" && oHCMSearchObj.EmployeeTypeIDs != null)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.EmployeeTypeIDs + "))";
            }
            if (oHCMSearchObj.ShiftIDs != "" && oHCMSearchObj.ShiftIDs != null)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.CurrentShiftID IN (" + oHCMSearchObj.ShiftIDs + "))";
            }
            if (oHCMSearchObj.SalarySchemeIDs != "" && oHCMSearchObj.SalarySchemeIDs != null)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT ESS.EmployeeID FROM EmployeeSalaryStructure AS ESS WHERE ESS.SalarySchemeID IN (" + oHCMSearchObj.SalarySchemeIDs + "))";
            }
            if (oHCMSearchObj.AttendanceSchemeIDs != "" && oHCMSearchObj.AttendanceSchemeIDs != null)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.AttendanceSchemeID IN (" + oHCMSearchObj.AttendanceSchemeIDs + "))";
            }
            if (oHCMSearchObj.CategoryID != 0)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EC.EmployeeID FROM EmployeeConfirmation AS EC WHERE EO.EmployeeCategory=" + oHCMSearchObj.CategoryID + ")";
            }
            if (oHCMSearchObj.AuthenticationNo != "" && oHCMSearchObj.AuthenticationNo != null)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EA.EmployeeID FROM EmployeeAuthentication AS EA WHERE EA.IsActive =1 AND EA.CardNo LIKE '%" + oHCMSearchObj.AuthenticationNo + "%')";
            }
            if (oHCMSearchObj.StartSalaryRange != 0 && oHCMSearchObj.EndSalaryRange != 0)
            {
                sSQL = sSQL + " AND ES.GrossAmount BETWEEN " + oHCMSearchObj.StartSalaryRange.ToString() + " AND " + oHCMSearchObj.EndSalaryRange.ToString();
            }

            if (oHCMSearchObj.BankAccountID > 0 && oHCMSearchObj.IsSearchByBank==true)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN ( SELECT EBA.EmployeeID FROM EmployeeBankAccount AS EBA WHERE EBA.BankBranchID IN (SELECT BA.BankBranchID FROM BankAccount AS BA WHERE BA.BankID=(SELECT BAA.BankID FROM BankAccount AS BAA WHERE BAA.BankAccountID=" + oHCMSearchObj.BankAccountID + ")))";
            }
            if (oHCMSearchObj.PaymentTypeID > 0)
            {
                if (oHCMSearchObj.PaymentTypeID == 1)
                {
                    sSQL = sSQL + " AND ES.EmployeeID IN ( SELECT ESS.EmployeeID FROM EmployeeSalaryStructure AS ESS WHERE ESS.IsAllowBankAccount=1)";
                }
                if (oHCMSearchObj.PaymentTypeID == 2)
                {
                    sSQL = sSQL + " AND ES.EmployeeID IN ( SELECT ESS.EmployeeID FROM EmployeeSalaryStructure AS ESS WHERE ESS.IsAllowBankAccount=0)";
                }
            }

            if (oHCMSearchObj.IsNewJoin)
            {
                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.JoiningDate BETWEEN ES.StartDate AND ES.EndDate)";
            }

            if (oHCMSearchObj.IsJoiningDate == true)
            {

                sSQL = sSQL + " AND ES.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.DateOfJoin BETWEEN '" + oHCMSearchObj.JoiningStartDate.ToString("dd MMM yyyy") + "' AND '" + oHCMSearchObj.JoiningEndDate.ToString("dd MMM yyyy") + "')";
            }

            return sSQL;
        }
        private void ResetSubTotal(List<EmployeeSalaryDetailV2> oEmployeeSalaryBasics, List<EmployeeSalaryDetailV2> oEmployeeSalaryAllowances, List<EmployeeSalaryDetailV2> oEmployeeSalaryDeductions)
        {
            foreach (EmployeeSalaryDetailV2 oEmpBasics in oEmployeeSalaryBasics)
            {
                oEmpBasics.SubTotalAmount = 0;
            }
            foreach (EmployeeSalaryDetailV2 oEmpAllowances in oEmployeeSalaryAllowances)
            {
                oEmpAllowances.SubTotalAmount = 0;
            }
            foreach (EmployeeSalaryDetailV2 oEmpDeductions in oEmployeeSalaryDeductions)
            {
                oEmpDeductions.SubTotalAmount = 0;
            }
        }
        private bool IsFieldExists(EnumSalaryField eEnumSalaryField, List<SalaryFieldSetupDetail> oSalaryFieldSetupDetails)
        {
            foreach (SalaryFieldSetupDetail oItem in oSalaryFieldSetupDetails)
            {
                if (oItem.SalaryField == eEnumSalaryField)
                {
                    return true;
                }
            }
            return false;
        }

        public Image GetEmployeePhoto(EmployeeSalaryV2 oEmployeeSalary)
        {
            if (oEmployeeSalary.Photo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/Employeeimage.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oEmployeeSalary.Photo);
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


        #region Post Method
        [HttpPost]
        public ActionResult SetSessionSearchCriteria(HCMSearchObj oHCMSearchObj)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oHCMSearchObj);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsEmployeeType(EmployeeType oEmployeeType)
        {
            List<EmployeeType> _oEmployeeTypes = new List<EmployeeType>();
            try
            {
                string sSql = "";
                if (!string.IsNullOrEmpty(oEmployeeType.Name))
                {
                    sSql = "select * from EmployeeType where IsActive=1 AND Name LIKE'%" + oEmployeeType.Name + "%'";
                }
                else
                    sSql = "select * from EmployeeType where IsActive=1";
                _oEmployeeTypes = EmployeeType.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
                if (_oEmployeeTypes.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                EmployeeType _oEmployeeGroup = new EmployeeType();
                _oEmployeeGroup.ErrorMessage = ex.Message;
                _oEmployeeTypes.Add(_oEmployeeGroup);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeTypes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsHRMShiftByName(HRMShift oHRMShift)
        {
            List<HRMShift> _oHRMShifts = new List<HRMShift>();
            try
            {
                string sSql = "";
                if (!string.IsNullOrEmpty(oHRMShift.Name))
                {
                    sSql = "select * from HRM_Shift where  IsActive=1 AND Name LIKE'%" + oHRMShift.Name + "%'";
                }
                else
                    sSql = "select * from HRM_Shift WHERE IsActive=1";
                _oHRMShifts = HRMShift.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
                if (_oHRMShifts.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                HRMShift _oHRMShift = new HRMShift();
                _oHRMShift.ErrorMessage = ex.Message;
                _oHRMShifts.Add(_oHRMShift);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHRMShifts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public JsonResult AdvanceSearch(HCMSearchObj oHCMSearchObj)
        {
            List<EmployeeSalaryV2> oEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            try
            {
                string sSQL = this.GetSQL(oHCMSearchObj);
                oEmployeeSalaryV2s = EmployeeSalaryV2.Gets(sSQL, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                if (oEmployeeSalaryV2s.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                EmployeeSalaryV2 oEmployeeSalaryV2 = new EmployeeSalaryV2();
                oEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
                oEmployeeSalaryV2.ErrorMessage = ex.Message;
                oEmployeeSalaryV2s.Add(oEmployeeSalaryV2);
            }
            var jsonResult = Json(oEmployeeSalaryV2s, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult CompAdvanceSearch(HCMSearchObj oHCMSearchObj)
        {
            List<EmployeeSalaryV2> oEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            MaxOTConfiguration oMaxOTConfiguration = new MaxOTConfiguration();
            try
            {
                oMaxOTConfiguration = oMaxOTConfiguration.Get(oHCMSearchObj.MOCID, (int)Session[SessionInfo.currentUserID]);
                if (oMaxOTConfiguration.IsActive == false && oMaxOTConfiguration.SourceTimeCardID > 0)
                {
                    oHCMSearchObj.MOCID = oMaxOTConfiguration.SourceTimeCardID;
                    string sSQL = this.CompGetSQL(oHCMSearchObj);
                    oEmployeeSalaryV2s = EmployeeSalaryV2.CompGets(sSQL, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                }

                else if (oMaxOTConfiguration.IsActive == false && oMaxOTConfiguration.SourceTimeCardID == 0)
                {
                    string sSQL = this.GetSQL(oHCMSearchObj);
                    oEmployeeSalaryV2s = EmployeeSalaryV2.Gets(sSQL, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                    foreach (EmployeeSalaryV2 oItem in oEmployeeSalaryV2s)
                    {
                        oItem.ExtraOTHour = oItem.ExtraOTHour + oItem.DayOfRegularOTHour;
                    }
                }

                else
                {
                    string sSQL = this.CompGetSQL(oHCMSearchObj);
                    oEmployeeSalaryV2s = EmployeeSalaryV2.CompGets(sSQL, oHCMSearchObj.SalaryStartDate, oHCMSearchObj.SalaryEndDate, (int)(Session[SessionInfo.currentUserID]));
                }
               
                if (oEmployeeSalaryV2s.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                EmployeeSalaryV2 oEmployeeSalaryV2 = new EmployeeSalaryV2();
                oEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
                oEmployeeSalaryV2.ErrorMessage = ex.Message;
                oEmployeeSalaryV2s.Add(oEmployeeSalaryV2);
            }
            var jsonResult = Json(oEmployeeSalaryV2s, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion


        #region SalarySheetXL

        public void PrintBankSheet_XL(int nGroupBySerial, List<SignatureSetup> oSignatureSetups, List<EmployeeSalaryV2> oEmployeeSalaryV2s, string SelectedColNames, bool bRound, BankAccount oBankAccount, DateTime dApplyDate, string sPrintHeader, Company oCompany, string StartDate, string EndDate, float HeaderHeightInch, float FooterHeightInch, bool IsOT)
        {
            string[] _selectedColNames = new String[20];
            DateTime sEndDate = Convert.ToDateTime(EndDate);
            int _nMonthID = Convert.ToInt32(sEndDate.ToString("MM"));
            int _nYearID = sEndDate.Year;
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", oEmployeeSalaryV2s.Select(p => p.BUID).Distinct().ToList());
            if (BUIDs != "") { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }


            int nMaxColumn = 0;
            int colIndex = 1;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("BANK ADVICE");
                sheet.Name = "BANK ADVICE";
                if (SelectedColNames != "")
                {
                    _selectedColNames = SelectedColNames.Split(',');
                }

                var matchString = _selectedColNames.FirstOrDefault(x => x.Contains("EmployeeCode"));
                sheet.Column(1).Width = 6; //SL
                int nCnt = 1;
                nMaxColumn = 1;
                for (int i = 1; i <= _selectedColNames.Length; i++)
                {
                    if (matchString != null && i == 1)
                    {
                        sheet.Column(nCnt+i).Width = 15; //CODE
                    }
                    else if (i == 2)
                    {
                        sheet.Column(nCnt + i).Width = 13; //NAME
                    }
                    else if (i == _selectedColNames.Length)
                    {
                        sheet.Column(nCnt + i).Width = 12; //AMOUNT
                    }
                    else
                    {
                        sheet.Column(nCnt + i).Width = 15;
                    }
                    nMaxColumn++;
                }
             

                nMaxColumn +=1;

                


                #region Report Header
                cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn-1]; cell.Merge = true; cell.Value = oBusinessUnits.Count == 1 ? oBusinessUnits[0].Name : oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn-1]; cell.Merge = true; cell.Value = "BANK ADVICE"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;


                cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn-1]; cell.Merge = true; cell.Value = (oEmployeeSalaryV2s.Count > 0 ? ("From " + StartDate + " To " + EndDate) : ""); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;

                cell = sheet.Cells[rowIndex, 1, rowIndex, 3]; cell.Merge = true; cell.Value = "Date: "+dApplyDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                rowIndex++;
                cell = sheet.Cells[rowIndex, 1, rowIndex, 2]; cell.Merge = true; cell.Value = "To"; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 9; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 1, rowIndex, 3]; cell.Merge = true; cell.Value = "The Branch Manager"; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 9; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex+=2;
                string strMonthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_nMonthID);
                cell = sheet.Cells[rowIndex, 1, rowIndex, 5]; cell.Merge = true; cell.Value = "Subject: Request to disburse salary for the month of " + strMonthName + ", " + _nYearID + "."; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 9; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style  = ExcelBorderStyle.Thin;
                rowIndex+=2;
                cell = sheet.Cells[rowIndex, 1, rowIndex, 2]; cell.Merge = true; cell.Value = "Dear Sir,"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 9; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                double nAmtWithOT = oEmployeeSalaryV2s.Sum(x => x.BankAmount);
                rowIndex++;
                if (IsOT)
                {
                    nAmtWithOT += oEmployeeSalaryV2s.Sum(x => x.TotalOT);
                }

                rowIndex++;


                cell = sheet.Cells[rowIndex, 1, rowIndex, 7]; cell.Merge = true; cell.Value = "We are requesting you to disburse the net payable salary BDT " + Global.MillionFormat(nAmtWithOT,0) + " (In Words: "+Global.TakaWords(Math.Round(nAmtWithOT,0))+") from our Company Account (" + oBankAccount.AccountNo + ") to our employee’s individual accounts as mentioned here below :"; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Row(rowIndex).Height = MeasureTextHeight("We are requesting you to disburse the net payable salary BDT " + Global.MillionFormat(nAmtWithOT) + " (In Words: " + Global.TakaWords(Math.Round(nAmtWithOT, 0)) + ") from our Company Account (" + oBankAccount.AccountNo + ") to our employee’s individual accounts as mentioned here below :", cell.Style.Font, 77);
                rowIndex += 2;
                #endregion

                #region Table Header 02
                if (oEmployeeSalaryV2s.Count <= 0)
                {
                    sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn-1].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "NOTHING TO PRINT"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                else
                {
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    matchString = _selectedColNames.FirstOrDefault(x => x.Contains("EmployeeCode"));
                    if (matchString != null)
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "CODE"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    matchString = _selectedColNames.FirstOrDefault(x => x.Contains("EmployeeName"));
                    if(matchString!=null)
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "NAME"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    matchString = _selectedColNames.FirstOrDefault(x => x.Contains("DepartmentName"));
                      if (matchString != null)
                      {
                          cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DEPARTMENT"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                          fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                          border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                      }

                      matchString = _selectedColNames.FirstOrDefault(x => x.Contains("DesignationName"));
                      if (matchString != null)
                      {
                          cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DESIGNATION"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                          fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                          border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                      }

                      matchString = _selectedColNames.FirstOrDefault(x => x.Contains("JoiningDate"));
                    if (matchString != null)
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "JOINING"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                 
                     matchString = _selectedColNames.FirstOrDefault(x => x.Contains("ContactNo"));
                     if (matchString != null)
                     {
                         cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "CONTACT NO"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                         fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                     }
                   
                     matchString = _selectedColNames.FirstOrDefault(x => x.Contains("AccountNo"));
                     if (matchString != null)
                     {
                         cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BANK ACCOUNT"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                         fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                     }
                   matchString = _selectedColNames.FirstOrDefault(x => x.Contains("Amount"));
                   if (matchString != null)
                   {
                       cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "AMOUNT PAYBALE"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                       fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                       border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                   }
                   

                    rowIndex++;
                #endregion

                    #region Table Body

                    int nCount = 0;
                    double nTotalAmount = 0;

                    foreach (EmployeeSalaryV2 oItem in oEmployeeSalaryV2s)
                    {
                        if (oItem.BankAmount > 0)
                        {
                            nCount++;
                            colIndex = 1;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                             matchString = _selectedColNames.FirstOrDefault(x => x.Contains("EmployeeCode"));
                             if (matchString != null)
                             {
                                 cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                 border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                             }

                             matchString = _selectedColNames.FirstOrDefault(x => x.Contains("EmployeeName"));
                             if (matchString != null)
                             {
                                 cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                 border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                             }

                             matchString = _selectedColNames.FirstOrDefault(x => x.Contains("DepartmentName"));
                             if (matchString != null)
                             {
                                 cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                 border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                             }

                             matchString = _selectedColNames.FirstOrDefault(x => x.Contains("DesignationName"));
                             if (matchString != null)
                             {
                                 cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                 border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                             }

                             matchString = _selectedColNames.FirstOrDefault(x => x.Contains("JoiningDate"));
                             if (matchString != null)
                             {
                                 cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.JoiningDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                 border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                             }

                             matchString = _selectedColNames.FirstOrDefault(x => x.Contains("ContactNo"));
                             if (matchString != null)
                             {
                                 cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ContactNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                 border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                             }

                             matchString = _selectedColNames.FirstOrDefault(x => x.Contains("AccountNo"));
                              if (matchString != null)
                              {
                                  cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.AccountNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                  fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                  border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                              }
                           

                            double nOTAllowance = Math.Round(oItem.TotalOT);
                            double nEmpAmount = 0;

                            if (IsOT)
                            {
                                oItem.BankAmount += nOTAllowance;
                            }

                            nEmpAmount = oItem.BankAmount;
                            nTotalAmount += nEmpAmount;


                            matchString = _selectedColNames.FirstOrDefault(x => x.Contains("Amount"));
                              if (matchString != null)
                              {
                                  cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (bRound == true) ? Math.Round(nEmpAmount, 0) : Math.Round(nEmpAmount, 0); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                  fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                                  cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                  border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                              }
                           

                            rowIndex++;
                        }
                    }
                    colIndex = 1;
                    sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn - 2].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "GRAND TOTAL"; cell.Style.Font.Bold = true; border = cell.Style.Border;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nMaxColumn - 1]; cell.Value = (bRound == true) ? Math.Round(nTotalAmount, 0) : Math.Round(nTotalAmount, 0); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    rowIndex++;
                    sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn - 1].Merge = true;
                    cell = sheet.Cells[rowIndex, 1]; cell.Value = Global.TakaWords(Math.Round(nTotalAmount, 0)); cell.Style.Font.Bold = true; border = cell.Style.Border;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    #endregion
                }

                cell = sheet.Cells[1, 1, rowIndex + 3, 14];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=BANK_ADVICE.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public double MeasureTextHeight(string text, ExcelFont font, int width)
        {
            if (string.IsNullOrEmpty(text)) return 0.0;
            var bitmap = new Bitmap(1, 1);
            var graphics = Graphics.FromImage(bitmap);

            var pixelWidth = Convert.ToInt32(width * 6);
            var drawingFont = new Font(font.Name, font.Size);
            var size = graphics.MeasureString(text, drawingFont, pixelWidth);

            return Math.Min(Convert.ToDouble(size.Height) * 72 / 96, 409);
        }
        public void CashSheet_XL(List<EmployeeSalaryV2> oEmployeeSalaryV2s, Company oCompany, string StartDate, string EndDate, bool IsOT)
        {

           

            int nMaxColumn = 0;
            int colIndex = 1;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Cash Sheet");
                sheet.Name = "Cash Sheet";

                sheet.Column(1).Width = 6; //SL
                sheet.Column(2).Width = 14; //CODE
                sheet.Column(3).Width = 16; //NAME
                sheet.Column(4).Width = 13; //DEPARTMENT
                sheet.Column(5).Width = 15; //DESIGNATION
                sheet.Column(6).Width = 13; //JOINING
                sheet.Column(7).Width = 13; //AMOUNT

                nMaxColumn = 8;




                #region Report Header
                sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn-1].Merge = true;
                cell = sheet.Cells[rowIndex, 1]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn-1].Merge = true;
                cell = sheet.Cells[rowIndex, 1]; cell.Value = "Cash Sheet"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn-1].Merge = true;
                cell = sheet.Cells[rowIndex, 1]; cell.Value = (oEmployeeSalaryV2s.Count > 0 ? ("From " + StartDate + " To " + EndDate) : ""); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;

                #endregion

                #region Table Header 02
                if (oEmployeeSalaryV2s.Count <= 0)
                {
                    cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn - 1]; cell.Merge = true; cell.Value = "NOTHING TO PRINT"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                else
                {
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "CODE"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "NAME"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DEPARTMENT"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DESIGNATION"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "JOINING"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "AMOUNT PAYBALE"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                #endregion

                    #region Table Body

                    int nCount = 0;
                    double nTotalAmount = 0;

                    foreach (EmployeeSalaryV2 oItem in oEmployeeSalaryV2s)
                    {
                        if (oItem.CashAmount > 0)
                        {
                            nCount++;
                            colIndex = 1;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.JoiningDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                          


                            double nOTAllowance = Math.Round(oItem.TotalOT);
                            double nEmpAmount = 0;

                            if (IsOT)
                            {
                                oItem.CashAmount += nOTAllowance;
                            }

                            nEmpAmount = oItem.CashAmount;
                            nTotalAmount += nEmpAmount;



                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nEmpAmount, 0); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            rowIndex++;
                        }
                    }
                    colIndex = 1;
                    sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn - 2].Merge = true;
                    cell = sheet.Cells[rowIndex, 1]; cell.Value = "GRAND TOTAL"; cell.Style.Font.Bold = true; border = cell.Style.Border;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nMaxColumn - 1]; cell.Value = Math.Round(nTotalAmount, 0); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rowIndex++;

                    cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn - 1]; cell.Merge = true; cell.Value = Global.TakaWords(Math.Round(nTotalAmount, 0)); cell.Style.WrapText = true; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    #endregion
                }
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=CashSheet.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        private void CommonSalarySheetXL(int nGroupBySerial, List<EmployeeSalaryV2> oEmployeeSalaryV2s, List<LeaveHead> oLeaveHeads, List<EmployeeSalaryDetailV2> oEmployeeSalaryBasics, List<EmployeeSalaryDetailV2> oEmployeeSalaryAllowances, List<EmployeeSalaryDetailV2> oEmployeeSalaryDeductions, Company oCompany, SalaryField oSalaryField, EnumPageOrientation ePageOrientation, List<SalarySheetSignature> oSalarySheetSignatures, string StartDate, string EndDate, bool IsOT)
        {


            bool _IsOT = false;
            double _SubTOTAllowance = 0, _GrandTOTAllowance = 0;
            List<EmployeeSalaryV2> oTempEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            DateTime sStartDate = Convert.ToDateTime(StartDate);
            DateTime sEndDate = Convert.ToDateTime(EndDate);
            _IsOT = IsOT;
            int rowIndex = 2;
            int _rowIndex = 0;
            int nMaxColumn = 0;
            int colIndex = 1, nTotalLeaveCol = 0;
            ExcelRange cell;
            Border border;
            ExcelFill fill;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                #region Detail Report
                var sheet = excelPackage.Workbook.Worksheets.Add("Salary Sheet F-01");
                sheet.Name = "Salary Sheet F-01";

                sheet.View.FreezePanes(9, 1);



                Dictionary<string, string> aGrandTotals = new Dictionary<string, string>();
                string sStartCell = "", sEndCell = "";
                int nStartRow = 0, nStartCol = 0;
                int nCount = 0;
                #region Declare Column
                colIndex = 0;
                sheet.Column(++colIndex).Width = 8;//SL
                #region Emp Info

                nTotalLeaveCol = oLeaveHeads.Count;

                sheet.Column(++colIndex).Width = 10;//EmployeeCode
                sheet.Column(++colIndex).Width = 10;//Employee Name
                sheet.Column(++colIndex).Width = 10;//Department
                sheet.Column(++colIndex).Width = 10;//Designation
                sheet.Column(++colIndex).Width = 10;//Joining date
                sheet.Column(++colIndex).Width = 10;//Employee Type
                sheet.Column(++colIndex).Width = 10;//Employee Group
                sheet.Column(++colIndex).Width = 10;//Payment Type
                #endregion
                #region Att.Detail

                sheet.Column(++colIndex).Width = 5;//Total Days
                sheet.Column(++colIndex).Width = 5;//Present Day
                sheet.Column(++colIndex).Width = 5;//Dayoff Holiday
                sheet.Column(++colIndex).Width = 5;//Absent Days

                for (int i = 0; i < oLeaveHeads.Count(); i++)
                {
                    sheet.Column(++colIndex).Width = 3;
                }


                sheet.Column(++colIndex).Width = 5;//Leave Days
                sheet.Column(++colIndex).Width = 5;//Emp Working Days
                sheet.Column(++colIndex).Width = 5;//Early Out Days
                sheet.Column(++colIndex).Width = 5;//Early Out Mins
                sheet.Column(++colIndex).Width = 5;//Late Days
                sheet.Column(++colIndex).Width = 8;//Late Hours
                if (_IsOT)
                {
                    sheet.Column(++colIndex).Width = 8;//OT Hours
                    sheet.Column(++colIndex).Width = 8;//OT Rate
                }

                sheet.Column(++colIndex).Width = 8;//Present Salary

                #endregion
                #region IncrementDetail

                #endregion

                for (int i = 0; i < oEmployeeSalaryBasics.Count(); i++)
                {
                    sheet.Column(++colIndex).Width = 8;
                }

                for (int i = 0; i < oEmployeeSalaryAllowances.Count(); i++)
                {
                    sheet.Column(++colIndex).Width = 8;
                }


                sheet.Column(++colIndex).Width = 8;//Gross Earnings

                for (int i = 0; i < oEmployeeSalaryDeductions.Count(); i++)
                {
                    sheet.Column(++colIndex).Width = 8;
                }

                sheet.Column(++colIndex).Width = 8;//Gross Deductions
                sheet.Column(++colIndex).Width = 8;//Net Amount

                sheet.Column(++colIndex).Width = 8;//Bank
                sheet.Column(++colIndex).Width = 8;//Cash
                sheet.Column(++colIndex).Width = 8;//Account No
                sheet.Column(++colIndex).Width = 8;//Bank Name
                sheet.Column(++colIndex).Width = 7;//Signature
                nMaxColumn = colIndex;
                #endregion
                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 12]; cell.Merge = true; cell.Value = oEmployeeSalaryV2s[0].BUName; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 12]; cell.Merge = true; cell.Value = "Salary Sheet"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 12]; cell.Merge = true; cell.Value = "From " + sStartDate.ToString("dd MMM yyyy") + " To " + sEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;
                #endregion
                int nCheckColSetup = 0;
                oEmployeeSalaryV2s.ForEach(x => oTempEmployeeSalaryV2s.Add(x));
                string PrevBUName = "", PrevLocationName = "";
                while (oEmployeeSalaryV2s.Count > 0)
                {
                    var oResults = oEmployeeSalaryV2s.Where(x => x.BUID == oEmployeeSalaryV2s[0].BUID && x.LocationID == oEmployeeSalaryV2s[0].LocationID).OrderBy(x=>x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();


                    #region Report Body
                    if (PrevBUName != oResults[0].BUShortName || PrevLocationName != oResults[0].LocationName)
                    {
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 4]; cell.Merge = true; cell.Value = "Unit : " + oResults[0].BUShortName + ": Location(" + oResults[0].LocationName + ")"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    }



                    if (PrevBUName != oResults[0].BUShortName || PrevLocationName != oResults[0].LocationName)
                    {
                        rowIndex++;
                        PrevBUName = oResults[0].BUShortName;
                        PrevLocationName = oResults[0].LocationName;
                        nStartRow = rowIndex;
                    }



                    #endregion
                    if (nCheckColSetup == 0)
                    {
                        #region Column SetUp
                        colIndex = 0;
                        int nRowspan = 2;
                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "SL#"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

           
                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + 7]; cell.Merge = true; cell.Value = "Employee Information"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        int nTCSpan = _IsOT == true ? 12 : 10;
                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + nTCSpan + nTotalLeaveCol - 1]; cell.Merge = true; cell.Value = "Att. Detail"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Present Salary"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (nTCSpan==12)
                        {
                            nTCSpan = 1;
                        }
                        else
                        {
                            nTCSpan = 0;
                        }
                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + oEmployeeSalaryBasics.Count() + oEmployeeSalaryAllowances.Count() + nTCSpan - 1]; cell.Merge = true; cell.Value = "Allowance"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Gross Earnings"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + oEmployeeSalaryDeductions.Count() - 1]; cell.Merge = true; cell.Value = "Deduction"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Gross Deduction"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Net Amount"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Bank"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Cash"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Account No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Bank name"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Signature"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                        colIndex = 1;


                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Employee Code"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Employee Name"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Department"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Joining date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Employee Type"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Emp Group"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Payment Type"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Total Days"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Present Day"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Day Off Holidays"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Absent Days"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        int nStartCollastRowHeader = colIndex;

                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + nTotalLeaveCol - 1]; cell.Merge = true; cell.Value = "Leave Head"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Leave Days"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "EWD"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Early Out Days"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Early Out Mins"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Late Days"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Late Hrs"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        if (_IsOT)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "OT Hours"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "OT Rate"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }


                        colIndex++;


                        foreach (EmployeeSalaryDetailV2 oItem in oEmployeeSalaryBasics)
                        {

                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = oItem.SalaryHeadName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        foreach (EmployeeSalaryDetailV2 oItem in oEmployeeSalaryAllowances)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = oItem.SalaryHeadName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (_IsOT)
                        {


                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "O T Allowance"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }


                        colIndex += 1;
                        foreach (EmployeeSalaryDetailV2 oItem in oEmployeeSalaryDeductions)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = oItem.SalaryHeadName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        rowIndex++;
                        colIndex = nStartCollastRowHeader;

                        foreach (LeaveHead oItem in oLeaveHeads)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.ShortName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        rowIndex++;




                        #endregion
                        nCheckColSetup = 1;
                        nStartRow = rowIndex;
                    }


                    #region SalarySheet Data

           

                    int nTotalRowPage = 0;



                    foreach (EmployeeSalaryV2 oEmpSalaryItem in oResults)
                    {
                        nTotalRowPage++;
                        nCount++;
                        colIndex = 0;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nCount.ToString();
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.EmployeeCode; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.EmployeeName; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.DepartmentName; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.DesignationName; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.JoiningDate.ToString("dd MMM yyyy"); cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.EmployeeTypeName; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.Grade; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.PaymentType; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        int days = DateTime.DaysInMonth(sStartDate.Year, sStartDate.Month);
                        if(oEmpSalaryItem.JoiningDate>sStartDate)
                        {
                            days = days - Convert.ToInt32(oEmpSalaryItem.JoiningDate.ToString("dd"));
                        }

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = days; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        int NTotalLeave = oEmpSalaryItem.EmployeeWiseLeaveStatus.Sum(x => x.LeaveDays);
                        int nTPresentDays = days - (oEmpSalaryItem.TotalDayOff + oEmpSalaryItem.TotalHoliday + NTotalLeave + oEmpSalaryItem.TotalAbsent);

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nTPresentDays; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        int nDayOffHoliday = oEmpSalaryItem.TotalDayOff + oEmpSalaryItem.TotalHoliday;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nDayOffHoliday; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.TotalAbsent; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        int nTotalLeave = 0;
                        int nLeave = 0, nLWP = 0;
                        foreach (LeaveHead oEmpLeave in oLeaveHeads)
                        {

                            nLeave = oEmpSalaryItem.EmployeeWiseLeaveStatus.Where(x => x.LeaveHeadID == oEmpLeave.LeaveHeadID).Select(x => x.LeaveDays).FirstOrDefault();
                            if (oEmpLeave.IsLWP)
                            {
                                nLWP += nLeave;
                            }

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nLeave; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            nTotalLeave += nLeave;
                        }


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nTotalLeave; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        int nTWorkingDay = nTPresentDays + nDayOffHoliday + nTotalLeave - nLWP;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nTWorkingDay; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.TotalEarlyLeaving; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.EarlyOutInMin; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.TotalLate; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.MinInHourMin(oEmpSalaryItem.LateInMin); cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        if (_IsOT)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.OTHour; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.OTRatePerHour; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }





                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.PresentSalary; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        foreach (EmployeeSalaryDetailV2 oEmpBasics in oEmployeeSalaryBasics)
                        {
                            double nBasic = oEmpSalaryItem.EmployeeSalaryBasics.Where(x => x.SalaryHeadID == oEmpBasics.SalaryHeadID).Select(x => x.Amount).FirstOrDefault();
                            oEmpBasics.SubTotalAmount = oEmpBasics.SubTotalAmount + nBasic;
                            oEmpBasics.GrandTotalAmount = oEmpBasics.GrandTotalAmount + nBasic;
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nBasic; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        foreach (EmployeeSalaryDetailV2 oEmpAllowances in oEmployeeSalaryAllowances)
                        {
                            double nAllowance = oEmpSalaryItem.EmployeeSalaryAllowances.Where(x => x.SalaryHeadID == oEmpAllowances.SalaryHeadID).Select(x => x.Amount).FirstOrDefault();
                            oEmpAllowances.SubTotalAmount = oEmpAllowances.SubTotalAmount + nAllowance;
                            oEmpAllowances.GrandTotalAmount = oEmpAllowances.GrandTotalAmount + nAllowance;
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nAllowance; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        double TotalOT=0;
                        if (_IsOT)
                        {
                             TotalOT = oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour;


                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = TotalOT; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.WrapText = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        double nTotalEarnings = oEmpSalaryItem.EmployeeSalaryBasics.Sum(x => x.Amount) + oEmpSalaryItem.EmployeeSalaryAllowances.Sum(x => x.Amount) + TotalOT;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nTotalEarnings; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        foreach (EmployeeSalaryDetailV2 oEmpDeductions in oEmployeeSalaryDeductions)
                        {
                            double nDeduction = oEmpSalaryItem.EmployeeSalaryDeductions.Where(x => x.SalaryHeadID == oEmpDeductions.SalaryHeadID).Select(x => x.Amount).FirstOrDefault();
                            oEmpDeductions.SubTotalAmount = oEmpDeductions.SubTotalAmount + nDeduction;
                            oEmpDeductions.GrandTotalAmount = oEmpDeductions.GrandTotalAmount + nDeduction;
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nDeduction; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                        double TotalDeduction = oEmpSalaryItem.EmployeeSalaryDeductions.Sum(x => x.Amount);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = TotalDeduction; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.NetAmount + TotalOT; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.BankAmount; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.CashAmount; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.AccountNo;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.BankName; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "--";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }
                    #endregion


                    oEmployeeSalaryV2s.RemoveAll(x => x.BUID == oResults[0].BUID && x.LocationID == oResults[0].LocationID);
                    if (((oEmployeeSalaryV2s.Count <= 0) || (oEmployeeSalaryV2s[0].BUShortName != PrevBUName || oEmployeeSalaryV2s[0].LocationName != PrevLocationName)))
                    {
                        #region SubTotal
                        colIndex = 0;
                        aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));
                        int nTColspan = (_IsOT == true) ? 20 : 18;
                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + nTColspan + nTotalLeaveCol]; cell.Merge = true; cell.Value = "Location Wise Sub Total";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                        foreach (EmployeeSalaryDetailV2 oEmpBasics in oEmployeeSalaryBasics)
                        {
                            nStartCol = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        foreach (EmployeeSalaryDetailV2 oEmpAllowances in oEmployeeSalaryAllowances)
                        {
                            nStartCol = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                        if (_IsOT)
                        {


                            nStartCol = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            _GrandTOTAllowance += _SubTOTAllowance;


                        }

                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        foreach (EmployeeSalaryDetailV2 oEmpDeductions in oEmployeeSalaryDeductions)
                        {

                            nStartCol = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        }

                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        _SubTOTAllowance = 0;
                        ResetSubTotal(oEmployeeSalaryBasics, oEmployeeSalaryAllowances, oEmployeeSalaryDeductions);
                        rowIndex += 2;
                        #endregion
                    }

                }
                #region GrandTotal
                string sFormula = ""; int nTempStartRow = 0, nTempEndRow = 0;
                int nTotalIndex = 0, nEndRow = rowIndex - 1;
                colIndex = 0;
                int nTGColspan = (_IsOT == true) ? 20 : 18;
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + nTGColspan + nTotalLeaveCol]; cell.Merge = true; cell.Value = "Grand Total";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nTotalIndex = colIndex;
       




                    #region Formula
                    sFormula = "";
                    if (aGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= aGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion


                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    foreach (EmployeeSalaryDetailV2 oEmpBasics in oEmployeeSalaryBasics)
                    {
                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            nTotalIndex = nTotalIndex + 1;
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                                sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                                sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                            }
                            if (sFormula.Length > 0)
                            {
                                sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                            }
                            sFormula = sFormula + ")";
                        }
                        else
                        {
                            sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    }

                    foreach (EmployeeSalaryDetailV2 oEmpAllowances in oEmployeeSalaryAllowances)
                    {

                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            nTotalIndex = nTotalIndex + 1;
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                                sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                                sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                            }
                            if (sFormula.Length > 0)
                            {
                                sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                            }
                            sFormula = sFormula + ")";
                        }
                        else
                        {
                            sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    }
                    if (_IsOT)
                    {

                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            nTotalIndex = nTotalIndex + 1;
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                                sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                                sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                            }
                            if (sFormula.Length > 0)
                            {
                                sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                            }
                            sFormula = sFormula + ")";
                        }
                        else
                        {
                            sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    }



                    #region Formula
                    sFormula = "";
                    if (aGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= aGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    foreach (EmployeeSalaryDetailV2 oEmpDeductions in oEmployeeSalaryDeductions)
                    {
                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            nTotalIndex = nTotalIndex + 1;
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                                sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                                sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                            }
                            if (sFormula.Length > 0)
                            {
                                sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                            }
                            sFormula = sFormula + ")";
                        }
                        else
                        {
                            sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    }
                    #region Formula
                    sFormula = "";
                    if (aGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= aGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion


                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    #region Formula
                    sFormula = "";
                    if (aGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= aGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion


                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    // double nTotalNetAmount =Convert.ToDouble(sheet.Cells[rowIndex, colIndex].Text);




                    #region Formula
                    sFormula = "";
                    if (aGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= aGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion


                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                    #region Formula
                    sFormula = "";
                    if (aGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= aGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                    _rowIndex = rowIndex;

                    #endregion
                #endregion

                    #region Summary Report
                    Dictionary<string, string> abGrandTotals = new Dictionary<string, string>();
                    var Summarysheet = excelPackage.Workbook.Worksheets.Add("Salary Summary");
                    Summarysheet.Name = "Salary Summary";
                    colIndex = 2;
                    Summarysheet.View.FreezePanes(6, 1);
                    Summarysheet.Column(++colIndex).Width = 8; //SL
                    Summarysheet.Column(++colIndex).Width = 20; //Department
                    Summarysheet.Column(++colIndex).Width = 10; //Man Power
                    Summarysheet.Column(++colIndex).Width = 13; //Gross
                    Summarysheet.Column(++colIndex).Width = 13; //Addition
                    Summarysheet.Column(++colIndex).Width = 13; //Deduction
                if(_IsOT==true)
                {
                    Summarysheet.Column(++colIndex).Width = 15; //Total OT Hour
                    Summarysheet.Column(++colIndex).Width = 15; //Total OT Amount
                }
                  
                    Summarysheet.Column(++colIndex).Width = 13; //Net
                    Summarysheet.Column(++colIndex).Width = 13; //Bank
                    Summarysheet.Column(++colIndex).Width = 13; //Cash


                    colIndex = 2;
                    rowIndex = 2;


                    while (oTempEmployeeSalaryV2s.Count > 0)
                    {

                        string BUName = oTempEmployeeSalaryV2s[0].BUName;
                        #region Report Header
                        cell = Summarysheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = BUName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;
                        cell = Summarysheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "Salary Sheet"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        cell = Summarysheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "From " + sStartDate.ToString("dd MMM yyyy") + " To " + sEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        #endregion
                        oEmployeeSalaryV2s = oTempEmployeeSalaryV2s.Where(x => x.BUName == BUName).OrderBy(x=>x.LocationName).ThenBy(x=>x.DepartmentName).ToList();



                        var oDBMonthlyAttendanceReports = oEmployeeSalaryV2s.GroupBy(x => new { x.DepartmentName }, (key, grp) => new
                        {
                            DepartmentName = key.DepartmentName,
                            EmpCount = grp.ToList().Count(),
                            GrossAmount = grp.ToList().Sum(y => y.GrossAmount),
                            TwoHourOT = grp.ToList().Sum(y => y.DefineOTHour),
                            OTHour = grp.ToList().Sum(y => y.OTHour),
                            ExtraOtHour = grp.ToList().Sum(y => y.ExtraOTHour),
                            TotalOT = grp.ToList().Sum(y => y.OTHour * y.OTRatePerHour),
                            TotalExtraOT = grp.ToList().Sum(y => y.ExtraOTHour * y.OTRatePerHour),
                            TotalTwoHourOT = grp.ToList().Sum(y => y.DefineOTHour * y.OTRatePerHour),
                            NetAmount = grp.ToList().Sum(y => y.NetAmount),
                            BankAmount = grp.ToList().Sum(y => y.BankAmount),
                            CashAmount = grp.ToList().Sum(y => y.CashAmount),
                            Results = grp.ToList(),
                            AdditionAmount = grp.ToList().Sum(x => x.EmployeeSalaryAllowances.Sum(y => y.Amount)),
                            DeductionAmount = grp.ToList().Sum(x => x.EmployeeSalaryDeductions.Sum(y => y.Amount))
                        });
                        colIndex = 2;
                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "SL#"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "ManPower"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Gross"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Addition"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Deduction"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if(_IsOT==true)
                        {
                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Total OT Hour"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Total OT Amount"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                       


                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Net Amount"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Bank Amount"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Cash Amount"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex++;
                        colIndex = 2;
                        nCount = 0;
                        nStartRow = rowIndex;
                        foreach (var oBUEmployeeSalary in oDBMonthlyAttendanceReports)
                        {
                            nCount++;

                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = nCount; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.DepartmentName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.EmpCount; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.GrossAmount; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.AdditionAmount; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.DeductionAmount; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            if(_IsOT==true)
                            {
                                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.OTHour; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.TotalOT; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            }
                          

                            if(_IsOT==true)
                            {
                                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.NetAmount + oBUEmployeeSalary.TotalOT; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.BankAmount + oBUEmployeeSalary.TotalOT; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.CashAmount + oBUEmployeeSalary.TotalOT; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            else
                            {
                                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.NetAmount; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.BankAmount; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.CashAmount; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                          


                            rowIndex++;
                            colIndex = 2;
                        }
                        #region SubTotal
                        abGrandTotals.Add((abGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));

                        cell = Summarysheet.Cells[rowIndex, ++colIndex, rowIndex, ++colIndex]; cell.Merge = true; cell.Value = "SubTotal:"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        if(_IsOT==true)
                        {
                            nStartCol = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                            nStartCol = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                       




                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        #endregion
                        oTempEmployeeSalaryV2s.RemoveAll(x => x.BUName == BUName);
                        rowIndex++;
                    }

                    #region GrandTotal
                    colIndex = 2;
                    cell = Summarysheet.Cells[rowIndex, ++colIndex, rowIndex, ++colIndex]; cell.Merge = true; cell.Value = "GrandTotal:"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nEndRow = rowIndex - 2;
                    nTotalIndex = colIndex;
                    #region Formula
                    sFormula = "";
                    if (abGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= abGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    #region Formula
                    sFormula = "";
                    if (abGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= abGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    #region Formula
                    sFormula = "";
                    if (abGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= abGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    #region Formula
                    sFormula = "";
                    if (abGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= abGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if(_IsOT==true)
                {
                    #region Formula
                    sFormula = "";
                    if (abGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= abGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    #region Formula
                    sFormula = "";
                    if (abGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= abGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }
                   

                    #region Formula
                    sFormula = "";
                    if (abGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= abGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    #region Formula
                    sFormula = "";
                    if (abGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= abGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    #region Formula
                    sFormula = "";
                    if (abGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= abGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    #endregion
                    #endregion

                    cell = sheet.Cells[1, 1, _rowIndex + 3, nMaxColumn + 3];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=SalarySheet F-01.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
            }
      

        private void SalarySheetXL(int nGroupBySerial, List<EmployeeSalaryV2> oEmployeeSalaryV2s, List<LeaveHead> oLeaveHeads, List<EmployeeSalaryDetailV2> oEmployeeSalaryBasics, List<EmployeeSalaryDetailV2> oEmployeeSalaryAllowances, List<EmployeeSalaryDetailV2> oEmployeeSalaryDeductions, Company oCompany, SalaryField oSalaryField, EnumPageOrientation ePageOrientation, List<SalarySheetSignature> oSalarySheetSignatures, string StartDate, string EndDate, bool IsOT)
        {


            bool _IsOT = false;
            double _SubTOTAllowance = 0, _GrandTOTAllowance = 0;
            List<EmployeeSalaryV2> oTempEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            DateTime sStartDate = Convert.ToDateTime(StartDate);
            DateTime sEndDate = Convert.ToDateTime(EndDate);
            _IsOT = IsOT;
            int rowIndex = 2;
            int _rowIndex = 0;
            int nMaxColumn = 0;
            int colIndex = 1, nTotalLeaveCol = 0 ;
            ExcelRange cell;
            Border border;
            ExcelFill fill;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                #region Detail Report
                var sheet = excelPackage.Workbook.Worksheets.Add("Salary Sheet");
                sheet.Name = "Salary Sheet";
                if(oSalaryField.LeaveDetail)
                {
                    sheet.View.FreezePanes(9, 1);
                }
                else
                {
                    sheet.View.FreezePanes(8, 1);
                }
               
                Dictionary<string, string> aGrandTotals = new Dictionary<string, string>();
                string sStartCell = "", sEndCell = "";
                int nStartRow = 0, nStartCol = 0;
                int nCount = 0;
                #region Declare Column
                colIndex = 0;
                sheet.Column(++colIndex).Width = 8;//SL
                #region Emp Info
                if(oSalaryField.LeaveDetail)
                {
                    nTotalLeaveCol = oLeaveHeads.Count;
                }
                if (oSalaryField.EmployeeCode)
                {
                    sheet.Column(++colIndex).Width = 10;
                }

                if (oSalaryField.EmployeeName)
                {
                    sheet.Column(++colIndex).Width = 10;
                }
                if (oSalaryField.Designation)
                {
                    sheet.Column(++colIndex).Width = 10;
                }
                if (oSalaryField.Grade)
                {
                    sheet.Column(++colIndex).Width = 10;
                }
                if (oSalaryField.JoiningDate)
                {
                    sheet.Column(++colIndex).Width = 10;
                }

                if (oSalaryField.EmployeeType)
                {
                    sheet.Column(++colIndex).Width = 10;
                }

                if (oSalaryField.PaymentType)
                {
                    sheet.Column(++colIndex).Width = 10;
                }
                #endregion
                #region Att.Detail
                if (oSalaryField.TotalDays)
                {
                    sheet.Column(++colIndex).Width = 5;
                }
                if (oSalaryField.PresentDay)
                {
                    sheet.Column(++colIndex).Width = 5;
                }
                if (oSalaryField.Day_off_Holidays)
                {
                    sheet.Column(++colIndex).Width = 5;
                }

                if (oSalaryField.AbsentDays)
                {
                    sheet.Column(++colIndex).Width = 5;
                }
                if(oSalaryField.LeaveDetail)
                {
                    for (int i = 0; i < oLeaveHeads.Count(); i++)
                    {
                        sheet.Column(++colIndex).Width = 3;
                    }
                }
               

                if (oSalaryField.LeaveDays)
                {
                    sheet.Column(++colIndex).Width = 5;
                }

                if (oSalaryField.Employee_Working_Days)
                {
                    sheet.Column(++colIndex).Width = 5;
                }

                if (oSalaryField.Early_Out_Days)
                {
                    sheet.Column(++colIndex).Width = 5;
                }

                if (oSalaryField.Early_Out_Mins)
                {
                    sheet.Column(++colIndex).Width = 5;
                }

                if (oSalaryField.LateDays)
                {
                    sheet.Column(++colIndex).Width = 5;
                }

                if (oSalaryField.LateHrs)
                {
                    sheet.Column(++colIndex).Width = 8;
                }



                #endregion
                #region IncrementDetail
                if (oSalaryField.LastGross)
                {
                    sheet.Column(++colIndex).Width = 7;

                }

                if (oSalaryField.LastIncrement)
                {
                    sheet.Column(++colIndex).Width = 7;
                }

                if (oSalaryField.Increment_Effect_Date)
                {
                    sheet.Column(++colIndex).Width = 8;
                }
                #endregion
                if (oSalaryField.PresentSalary)
                {
                    sheet.Column(++colIndex).Width = 8;
                }

                for (int i = 0; i < oEmployeeSalaryBasics.Count(); i++)
                {
                    sheet.Column(++colIndex).Width = 8;
                }

                for (int i = 0; i < oEmployeeSalaryAllowances.Count(); i++)
                {
                    sheet.Column(++colIndex).Width = 8;
                }
                if (_IsOT)
                {
                    for (int i = 0; i < oSalaryField.OTAllowanceCount; i++)
                    {
                        sheet.Column(++colIndex).Width = 8;
                    }
                }


                sheet.Column(++colIndex).Width = 8;//Gross Earnings

                for (int i = 0; i < oEmployeeSalaryDeductions.Count(); i++)
                {
                    sheet.Column(++colIndex).Width = 8;
                }

                sheet.Column(++colIndex).Width = 8;//Gross Deductions
                sheet.Column(++colIndex).Width = 8;//Net Amount

                if (oSalaryField.BankAmount)
                {
                    sheet.Column(++colIndex).Width = 8;
                }

                if (oSalaryField.CashAmount)
                {
                    sheet.Column(++colIndex).Width = 8;
                }

                if (oSalaryField.AccountNo)
                {
                    sheet.Column(++colIndex).Width = 8;
                }

                if (oSalaryField.BankName)
                {
                    sheet.Column(++colIndex).Width = 8;
                }

                sheet.Column(++colIndex).Width = 7;//Signature
                nMaxColumn = colIndex;
                #endregion
                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 12]; cell.Merge = true; cell.Value = oEmployeeSalaryV2s[0].BUName; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 12]; cell.Merge = true; cell.Value = "Salary Sheet"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 12]; cell.Merge = true; cell.Value = "From " + sStartDate.ToString("dd MMM yyyy") + " To " + sEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;
                #endregion
                int nCheckColSetup = 0;
                oEmployeeSalaryV2s.ForEach(x => oTempEmployeeSalaryV2s.Add(x));
                string PrevBUName = "";
                while (oEmployeeSalaryV2s.Count > 0)
                {
                    var oResults = oEmployeeSalaryV2s.Where(x => x.BUID == oEmployeeSalaryV2s[0].BUID && x.LocationID == oEmployeeSalaryV2s[0].LocationID && x.DepartmentID == oEmployeeSalaryV2s[0].DepartmentID).OrderBy(x=>x.LocationName).ThenBy(x=>x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();


                    #region Report Body
                    if ((nGroupBySerial!=2) || PrevBUName != oResults[0].BUShortName)//1 for Group By Dept or Default and 2 fro Serial
                    {
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 4]; cell.Merge = true; cell.Value = "Unit : " + oResults[0].BUShortName+": Location("+oResults[0].LocationName+")"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    }


                    if (nGroupBySerial!=2)
                    {
                        cell = sheet.Cells[rowIndex, 5, rowIndex, 9]; cell.Merge = true; cell.Value = "Department : " + oResults.FirstOrDefault().DepartmentName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rowIndex++;
                    }
                    else
                    {
                        if (PrevBUName != oResults[0].BUShortName)
                        {
                            rowIndex++;
                            PrevBUName = oResults[0].BUShortName;
                            nStartRow = rowIndex;
                        }
                    }


                    #endregion
                    if (nCheckColSetup == 0)
                    {
                        #region Column SetUp
                        colIndex = 0;
                        int nRowspan = 1;
                        if(oSalaryField.LeaveDetail)
                        {
                            nRowspan = 2;
                        }
                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "SL#"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (oSalaryField.TotalEmpInfoCol > 0)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + oSalaryField.TotalEmpInfoCol - 1]; cell.Merge = true; cell.Value = "Employee Information"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        if (oSalaryField.TotalAttDetailColPrev + oSalaryField.TotalAttDetailColPost + nTotalLeaveCol>0)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + oSalaryField.TotalAttDetailColPrev + oSalaryField.TotalAttDetailColPost + nTotalLeaveCol - 1]; cell.Merge = true; cell.Value = "Att. Detail"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        

                        if (oSalaryField.TotalIncrementDetailCol > 0)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + oSalaryField.TotalIncrementDetailCol - 1]; cell.Merge = true; cell.Value = "Increment Detail"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (oSalaryField.PresentSalary)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Present Salary"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (oEmployeeSalaryBasics.Count()>0)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + oEmployeeSalaryBasics.Count() - 1]; cell.Merge = true; cell.Value = "Basic"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                       
                        if (_IsOT)
                        {
                            if (oEmployeeSalaryAllowances.Count() + oSalaryField.OTAllowanceCount>0)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + oEmployeeSalaryAllowances.Count() + oSalaryField.OTAllowanceCount - 1]; cell.Merge = true; cell.Value = "Allowance"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            }
                           
                        }
                        else
                        {
                            if (oEmployeeSalaryAllowances.Count()>0)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + oEmployeeSalaryAllowances.Count() - 1]; cell.Merge = true; cell.Value = "Allowance"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                          
                        }
                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Gross Earnings"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        if (oEmployeeSalaryDeductions.Count()>0)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + oEmployeeSalaryDeductions.Count() - 1]; cell.Merge = true; cell.Value = "Deduction"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                       
                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Gross Deduction"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Net Amount"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        if (oSalaryField.BankAmount)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Bank"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (oSalaryField.CashAmount)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Cash"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.AccountNo)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Account No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (oSalaryField.BankName)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Bank name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan, colIndex]; cell.Merge = true; cell.Value = "Signature"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                        colIndex = 1;
                        if (oSalaryField.EmployeeCode)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan-1, colIndex]; cell.Merge = true; cell.Value = "Employee Code"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.EmployeeName)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan-1, colIndex]; cell.Merge = true; cell.Value = "Employee Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.Designation)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan-1, colIndex]; cell.Merge = true; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (oSalaryField.Grade)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan-1, colIndex]; cell.Merge = true; cell.Value = "Grade"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (oSalaryField.JoiningDate)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan-1, colIndex]; cell.Merge = true; cell.Value = "Joining date"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.EmployeeType)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan-1, colIndex]; cell.Merge = true; cell.Value = "Employee Type"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.PaymentType)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan-1, colIndex]; cell.Merge = true; cell.Value = "Payment Type"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (oSalaryField.TotalDays)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan-1, colIndex]; cell.Merge = true; cell.Value = "Total Days"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.PresentDay)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Present Day"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.Day_off_Holidays)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Day Off Holidays"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.AbsentDays)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Absent Days"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        int nStartCollastRowHeader = colIndex;
                        if(oSalaryField.LeaveDetail)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + nTotalLeaveCol - 1]; cell.Merge = true; cell.Value = "Leave Head"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                       
                        if (oSalaryField.LeaveDays)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Leave Days"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.Employee_Working_Days)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "EWD"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.Early_Out_Days)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Early Out Days"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.Early_Out_Mins)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Early Out Mins"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.LateDays)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Late Days"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.LateHrs)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Late Hrs"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.LastGross)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Last Gross"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (oSalaryField.LastIncrement)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Last Increment"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (oSalaryField.Increment_Effect_Date)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Increment Effect Date"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (oSalaryField.PresentSalary)
                        {
                            colIndex += 1;
                        }
                        foreach (EmployeeSalaryDetailV2 oItem in oEmployeeSalaryBasics)
                        {

                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = oItem.SalaryHeadName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        foreach (EmployeeSalaryDetailV2 oItem in oEmployeeSalaryAllowances)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = oItem.SalaryHeadName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (_IsOT)
                        {
                            string sOTHour = "OT Hour";
                            if (oSalaryField.DefineOTHour)
                            {
                                sOTHour = "Total OT Hour";
                                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "OT Hour"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            if (oSalaryField.ExtraOTHour)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "Extra OT Hour"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            if (oSalaryField.OTHours)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = sOTHour; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            if (oSalaryField.OTRate)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "OT Rate"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            if (oSalaryField.OTAllowance)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = "O T Allowance"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                        }


                        colIndex += 1;
                        foreach (EmployeeSalaryDetailV2 oItem in oEmployeeSalaryDeductions)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + nRowspan - 1, colIndex]; cell.Merge = true; cell.Value = oItem.SalaryHeadName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        rowIndex++;
                        colIndex = nStartCollastRowHeader;
                        if(oSalaryField.LeaveDetail)
                        {
                            foreach (LeaveHead oItem in oLeaveHeads)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.ShortName; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            rowIndex++;
                        }
                        

                       
                        #endregion
                        nCheckColSetup = 1;
                        nStartRow = rowIndex;
                    }


                    #region SalarySheet Data

                    if (nGroupBySerial <= 1)
                    {
                        nCount = 0;
                        nStartRow = rowIndex;
                    }


                    int nTotalRowPage = 0;



                    foreach (EmployeeSalaryV2 oEmpSalaryItem in oResults)
                    {
                        nTotalRowPage++;
                        nCount++;
                        colIndex = 0;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nCount.ToString();
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (oSalaryField.EmployeeCode)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.EmployeeCode; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        if (oSalaryField.EmployeeName)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.EmployeeName; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                        if (oSalaryField.Designation)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.DesignationName; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                        if (oSalaryField.Grade)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.Grade; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        if (oSalaryField.JoiningDate)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.JoiningDate.ToString("dd MMM yyyy"); cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        if (oSalaryField.EmployeeType)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.EmployeeTypeName; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        if (oSalaryField.PaymentType)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.PaymentType; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                        int days = DateTime.DaysInMonth(sStartDate.Year, sStartDate.Month);
                        if(oEmpSalaryItem.JoiningDate>sStartDate)
                        {
                            days = days - Convert.ToInt32(oEmpSalaryItem.JoiningDate.ToString("dd"));
                        }
                        if (oSalaryField.TotalDays)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = days; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        int NTotalLeave = oEmpSalaryItem.EmployeeWiseLeaveStatus.Sum(x => x.LeaveDays);
                        int nTPresentDays = days - (oEmpSalaryItem.TotalDayOff + oEmpSalaryItem.TotalHoliday + NTotalLeave + oEmpSalaryItem.TotalAbsent);
                        if (oSalaryField.PresentDay)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nTPresentDays; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        int nDayOffHoliday = oEmpSalaryItem.TotalDayOff + oEmpSalaryItem.TotalHoliday;
                        if (oSalaryField.Day_off_Holidays)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nDayOffHoliday; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.AbsentDays)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.TotalAbsent; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }


                        int nTotalLeave = 0;
                        int nLeave = 0, nLWP = 0; 
                        foreach (LeaveHead oEmpLeave in oLeaveHeads)
                        {

                            nLeave = oEmpSalaryItem.EmployeeWiseLeaveStatus.Where(x => x.LeaveHeadID == oEmpLeave.LeaveHeadID).Select(x => x.LeaveDays).FirstOrDefault();
                            if (oEmpLeave.IsLWP)
                            {
                                nLWP += nLeave;
                            }
                            if(oSalaryField.LeaveDetail)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nLeave; cell.Style.WrapText = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                           
                            nTotalLeave += nLeave;
                        }

                        if (oSalaryField.LeaveDays)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nTotalLeave; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        int nTWorkingDay = nTPresentDays + nDayOffHoliday + nTotalLeave - nLWP;
                        if (oSalaryField.Employee_Working_Days)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nTWorkingDay; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        if (oSalaryField.Early_Out_Days)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.TotalEarlyLeaving; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        }

                        if (oSalaryField.Early_Out_Mins)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.EarlyOutInMin; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        if (oSalaryField.LateDays)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.TotalLate; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        if (oSalaryField.LateHrs)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.MinInHourMin(oEmpSalaryItem.LateInMin); cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        }


                        if (oSalaryField.LastGross)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.LastGross; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }


                        if (oSalaryField.LastIncrement)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.LastIncrement; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        if (oSalaryField.Increment_Effect_Date)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value =oEmpSalaryItem.IncrementEffectDate!=DateTime.MinValue?oEmpSalaryItem.IncrementEffectDate.ToString("dd MMM yyyy"):""; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        if (oSalaryField.PresentSalary)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.PresentSalary; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        }


                        foreach (EmployeeSalaryDetailV2 oEmpBasics in oEmployeeSalaryBasics)
                        {
                            double nBasic = oEmpSalaryItem.EmployeeSalaryBasics.Where(x => x.SalaryHeadID == oEmpBasics.SalaryHeadID).Select(x => x.Amount).FirstOrDefault();
                            oEmpBasics.SubTotalAmount = oEmpBasics.SubTotalAmount + nBasic;
                            oEmpBasics.GrandTotalAmount = oEmpBasics.GrandTotalAmount + nBasic;
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nBasic; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        foreach (EmployeeSalaryDetailV2 oEmpAllowances in oEmployeeSalaryAllowances)
                        {
                            double nAllowance = oEmpSalaryItem.EmployeeSalaryAllowances.Where(x => x.SalaryHeadID == oEmpAllowances.SalaryHeadID).Select(x => x.Amount).FirstOrDefault();
                            oEmpAllowances.SubTotalAmount = oEmpAllowances.SubTotalAmount + nAllowance;
                            oEmpAllowances.GrandTotalAmount = oEmpAllowances.GrandTotalAmount + nAllowance;
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nAllowance; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        double TotalEarnings = oEmpSalaryItem.EmployeeSalaryAllowances.Sum(x => x.Amount) + oEmpSalaryItem.EmployeeSalaryBasics.Sum(x => x.Amount);
                        double ToT = 0;
                        if (_IsOT)
                        {
                            double TotalOT = oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour;
                            ToT = TotalOT;
                            if (oSalaryField.DefineOTHour)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.DefineOTHour; cell.Style.WrapText = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            if (oSalaryField.ExtraOTHour)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.ExtraOTHour; cell.Style.WrapText = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            if (oSalaryField.OTHours)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.OTHour; cell.Style.WrapText = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            }

                            if (oSalaryField.OTRate)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.OTRatePerHour; cell.Style.WrapText = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            }
                            if (oSalaryField.OTAllowance)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = TotalOT; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.WrapText = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            }

                            oEmpSalaryItem.GrossAmount += TotalOT;
                        }


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.GrossAmount; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        foreach (EmployeeSalaryDetailV2 oEmpDeductions in oEmployeeSalaryDeductions)
                        {
                            double nDeduction = oEmpSalaryItem.EmployeeSalaryDeductions.Where(x => x.SalaryHeadID == oEmpDeductions.SalaryHeadID).Select(x => x.Amount).FirstOrDefault();
                            oEmpDeductions.SubTotalAmount = oEmpDeductions.SubTotalAmount + nDeduction;
                            oEmpDeductions.GrandTotalAmount = oEmpDeductions.GrandTotalAmount + nDeduction;
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nDeduction; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                        double TotalDeduction = oEmpSalaryItem.EmployeeSalaryDeductions.Sum(x => x.Amount);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = TotalDeduction; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.NetAmount + ToT; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (oSalaryField.BankAmount)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.BankAmount; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.CashAmount)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.CashAmount; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.AccountNo)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.AccountNo;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        if (oSalaryField.BankName)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.BankName; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "--";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }
                    #endregion


                    oEmployeeSalaryV2s.RemoveAll(x => x.BUID == oResults[0].BUID && x.LocationID == oResults[0].LocationID && x.DepartmentID == oResults[0].DepartmentID);
                    if (((oEmployeeSalaryV2s.Count <= 0) || (nGroupBySerial == 2 && oEmployeeSalaryV2s[0].BUShortName != PrevBUName)) || nGroupBySerial !=2)
                    {
                        #region SubTotal
                        colIndex = 0;
                        aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));
                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + oSalaryField.TotalEmpInfoCol + oSalaryField.TotalAttDetailColPrev + oSalaryField.TotalAttDetailColPost + nTotalLeaveCol]; cell.Merge = true; cell.Value = "Sub Total";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        if (oSalaryField.LastGross)
                        {
                            nStartCol = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.LastIncrement)
                        {
                            nStartCol = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.Increment_Effect_Date)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        if (oSalaryField.PresentSalary)
                        {
                            nStartCol = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        foreach (EmployeeSalaryDetailV2 oEmpBasics in oEmployeeSalaryBasics)
                        {
                            nStartCol = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        foreach (EmployeeSalaryDetailV2 oEmpAllowances in oEmployeeSalaryAllowances)
                        {
                            nStartCol = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                        if (_IsOT)
                        {
                            if (oSalaryField.DefineOTHour)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.WrapText = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            if (oSalaryField.ExtraOTHour)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.WrapText = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            if (oSalaryField.OTHours)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.WrapText = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            }

                            if (oSalaryField.OTRate)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.WrapText = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            }

                            if (oSalaryField.OTAllowance)
                            {

                                nStartCol = colIndex + 1;
                                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                _GrandTOTAllowance += _SubTOTAllowance;
                            }

                            //nStartCol = colIndex + 1;
                            //sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            //sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            //cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                            //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        foreach (EmployeeSalaryDetailV2 oEmpDeductions in oEmployeeSalaryDeductions)
                        {

                            nStartCol = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        }

                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (oSalaryField.BankAmount)
                        {

                            nStartCol = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }


                        if (oSalaryField.CashAmount)
                        {

                            nStartCol = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (oSalaryField.AccountNo)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        if (oSalaryField.BankName)
                        {
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        _SubTOTAllowance = 0;
                        ResetSubTotal(oEmployeeSalaryBasics, oEmployeeSalaryAllowances, oEmployeeSalaryDeductions);
                        rowIndex += 2;
                        #endregion
                    }

                }
                #region GrandTotal
                string sFormula = ""; int nTempStartRow = 0, nTempEndRow = 0;
                int nTotalIndex = 0, nEndRow = rowIndex - 1;
                colIndex = 0;
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + oSalaryField.TotalEmpInfoCol + oSalaryField.TotalAttDetailColPrev + oSalaryField.TotalAttDetailColPost + nTotalLeaveCol]; cell.Merge = true; cell.Value = "Grand Total";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nTotalIndex = colIndex;
                if (oSalaryField.LastGross)
                {

                    #region Formula
                    sFormula = "";
                    if (aGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= aGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion


                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }

                if (oSalaryField.LastIncrement)
                {
                    #region Formula
                    sFormula = "";
                    if (aGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= aGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                if (oSalaryField.Increment_Effect_Date)
                {
                    nTotalIndex = nTotalIndex + 1;
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }

                if (oSalaryField.PresentSalary)
                {

                    #region Formula
                    sFormula = "";
                    if (aGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= aGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion


                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                foreach (EmployeeSalaryDetailV2 oEmpBasics in oEmployeeSalaryBasics)
                {
                    #region Formula
                    sFormula = "";
                    if (aGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= aGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }

                foreach (EmployeeSalaryDetailV2 oEmpAllowances in oEmployeeSalaryAllowances)
                {

                    #region Formula
                    sFormula = "";
                    if (aGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= aGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }
                if (_IsOT)
                {

                    if (oSalaryField.DefineOTHour)
                    {
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (oSalaryField.ExtraOTHour)
                    {
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if (oSalaryField.OTHours)
                    {
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    }

                    if (oSalaryField.OTRate)
                    {
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    }
                    if (oSalaryField.OTAllowance)
                    {


                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            nTotalIndex = nTotalIndex + 1;
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                                sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                                sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                            }
                            if (sFormula.Length > 0)
                            {
                                sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                            }
                            sFormula = sFormula + ")";
                        }
                        else
                        {
                            sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    }
                }


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                foreach (EmployeeSalaryDetailV2 oEmpDeductions in oEmployeeSalaryDeductions)
                {
                    #region Formula
                    sFormula = "";
                    if (aGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= aGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion


                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                }
                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                // double nTotalNetAmount =Convert.ToDouble(sheet.Cells[rowIndex, colIndex].Text);


                if (oSalaryField.BankAmount)
                {

                    #region Formula
                    sFormula = "";
                    if (aGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= aGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion


                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }


                if (oSalaryField.CashAmount)
                {
                    #region Formula
                    sFormula = "";
                    if (aGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= aGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }

                if (oSalaryField.AccountNo)
                {
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }

                if (oSalaryField.BankName)
                {
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                _rowIndex = rowIndex;

                #endregion
                #endregion

                #region Summary Report
                Dictionary<string, string> abGrandTotals = new Dictionary<string, string>();
                var Summarysheet = excelPackage.Workbook.Worksheets.Add("Salary Summary");
                Summarysheet.Name = "Salary Summary";
                colIndex = 2;
                Summarysheet.View.FreezePanes(6, 1);
                Summarysheet.Column(++colIndex).Width = 8; //SL
                Summarysheet.Column(++colIndex).Width = 20; //Department
                Summarysheet.Column(++colIndex).Width = 10; //Man Power
                Summarysheet.Column(++colIndex).Width = 13; //Gross
                Summarysheet.Column(++colIndex).Width = 13; //Addition
                Summarysheet.Column(++colIndex).Width = 13; //Deduction
                if(_IsOT==true)
                {
                    Summarysheet.Column(++colIndex).Width = 15; //OT two Hour
                    Summarysheet.Column(++colIndex).Width = 15; //Extra OT hour
                    Summarysheet.Column(++colIndex).Width = 15; //Total OT Hour
                    Summarysheet.Column(++colIndex).Width = 15; //Two Hour OT Amount
                    Summarysheet.Column(++colIndex).Width = 15; //Extra OT Amount
                    Summarysheet.Column(++colIndex).Width = 15; //Total OT Amount
                }
               
                Summarysheet.Column(++colIndex).Width = 13; //Net
                Summarysheet.Column(++colIndex).Width = 13; //Bank
                Summarysheet.Column(++colIndex).Width = 13; //Cash


                colIndex = 2;
                rowIndex = 2;

                oTempEmployeeSalaryV2s = oTempEmployeeSalaryV2s.OrderBy(x => x.BUName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ToList();
                while (oTempEmployeeSalaryV2s.Count > 0)
                {

                    string BUName = oTempEmployeeSalaryV2s[0].BUName;
                    #region Report Header
                    cell = Summarysheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = BUName; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;
                    cell = Summarysheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "Salary Summary"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;

                    cell = Summarysheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "From " + sStartDate.ToString("dd MMM yyyy") + " To " + sEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;

                    #endregion
                    oEmployeeSalaryV2s = oTempEmployeeSalaryV2s.Where(x => x.BUName == BUName).ToList();



                    var oDBMonthlyAttendanceReports = oEmployeeSalaryV2s.GroupBy(x => new { x.DepartmentName }, (key, grp) => new
                    {
                        DepartmentName = key.DepartmentName,
                        EmpCount = grp.ToList().Count(),
                        GrossAmount = grp.ToList().Sum(y => y.GrossAmount),
                        TwoHourOT = grp.ToList().Sum(y => y.DefineOTHour),
                        OTHour = grp.ToList().Sum(y => y.OTHour),
                        ExtraOtHour = grp.ToList().Sum(y => y.ExtraOTHour),
                        TotalOT = grp.ToList().Sum(y => y.OTHour * y.OTRatePerHour),
                        TotalExtraOT = grp.ToList().Sum(y => y.ExtraOTHour * y.OTRatePerHour),
                        TotalTwoHourOT = grp.ToList().Sum(y => y.DefineOTHour * y.OTRatePerHour),
                        NetAmount = grp.ToList().Sum(y => y.NetAmount),
                        BankAmount = grp.ToList().Sum(y => y.BankAmount),
                        CashAmount = grp.ToList().Sum(y => y.CashAmount),
                        Results = grp.ToList(),
                        AdditionAmount = grp.ToList().Sum(x => x.EmployeeSalaryAllowances.Sum(y => y.Amount)),
                        DeductionAmount = grp.ToList().Sum(x => x.EmployeeSalaryDeductions.Sum(y => y.Amount))
                    });
                    colIndex = 2;
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "SL#"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "ManPower"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Gross"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Addition"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Deduction"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if(_IsOT==true)
                    {
                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "OT Two Hour"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Extra OT Hour"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Total OT Hour"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Two Hour OT Amount"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Extra OT Amount"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Total OT Amount"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                   

                 
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Net Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Bank Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Cash Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                    colIndex = 2;
                    nCount = 0;
                    nStartRow = rowIndex;
                    foreach (var oBUEmployeeSalary in oDBMonthlyAttendanceReports)
                    {
                        nCount++;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = nCount; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.DepartmentName; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.EmpCount; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.GrossAmount; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.AdditionAmount; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.DeductionAmount; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        
                        if(_IsOT==true)
                        {
                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.TwoHourOT; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.ExtraOtHour; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.OTHour; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.TotalTwoHourOT; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.TotalExtraOT; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.TotalOT; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                      
                        }
                       

                        if(_IsOT)
                        {
                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.NetAmount + oBUEmployeeSalary.TotalOT; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.BankAmount + oBUEmployeeSalary.TotalOT; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.CashAmount + oBUEmployeeSalary.TotalOT; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        
                        }
                        else
                        {
                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.NetAmount; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.BankAmount; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.CashAmount; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                       

                        rowIndex++;
                        colIndex = 2;
                    }
                    #region SubTotal
                    abGrandTotals.Add((abGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));

                    cell = Summarysheet.Cells[rowIndex, ++colIndex, rowIndex, ++colIndex]; cell.Merge = true; cell.Value = "SubTotal:"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nStartCol = colIndex + 1;
                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                   

                    nStartCol = colIndex + 1;
                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                   

                    nStartCol = colIndex + 1;
                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                   


                    nStartCol = colIndex + 1;
                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    


                    if(_IsOT==true)
                    {
                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                   

                    }
                   

                    nStartCol = colIndex + 1;
                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nStartCol = colIndex + 1;
                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nStartCol = colIndex + 1;
                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    

                    #endregion
                    oTempEmployeeSalaryV2s.RemoveAll(x => x.BUName == BUName);
                    rowIndex++;
                }

                #region GrandTotal
                colIndex = 2;
                cell = Summarysheet.Cells[rowIndex, ++colIndex, rowIndex, ++colIndex]; cell.Merge = true; cell.Value = "GrandTotal:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nEndRow = rowIndex - 2;
                nTotalIndex = colIndex;
                #region Formula
                sFormula = "";
                if (abGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= abGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                #region Formula
                sFormula = "";
                if (abGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= abGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                #region Formula
                sFormula = "";
                if (abGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= abGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                #region Formula
                sFormula = "";
                if (abGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= abGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if(_IsOT==true)
                {
                    #region Formula
                    sFormula = "";
                    if (abGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= abGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    #region Formula
                    sFormula = "";
                    if (abGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= abGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    #region Formula
                    sFormula = "";
                    if (abGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= abGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    #region Formula
                    sFormula = "";
                    if (abGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= abGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    #region Formula
                    sFormula = "";
                    if (abGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= abGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    #region Formula
                    sFormula = "";
                    if (abGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= abGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                }
            
                #region Formula
                sFormula = "";
                if (abGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= abGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                #region Formula
                sFormula = "";
                if (abGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= abGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                #region Formula
                sFormula = "";
                if (abGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= abGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(abGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                #endregion
                #endregion

                cell = sheet.Cells[1, 1, _rowIndex + 3, nMaxColumn + 3];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SalarySheet.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        private void SalarySheetXLF3(int nGroupBySerial, List<EmployeeSalaryV2> oEmployeeSalaryV2s, List<LeaveHead> oLeaveHeads, List<EmployeeSalaryDetailV2> oEmployeeSalaryBasics, List<EmployeeSalaryDetailV2> oEmployeeSalaryAllowances, List<EmployeeSalaryDetailV2> oEmployeeSalaryDeductions, Company oCompany, SalaryField oSalaryField, EnumPageOrientation ePageOrientation, List<SalarySheetSignature> oSalarySheetSignatures, string StartDate, string EndDate, bool IsOT)
        {

            bool _IsOT = false;
            List<EmployeeSalaryV2> oTempEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            List<EmployeeSalaryV2> _oSummaryEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            DateTime sStartDate = Convert.ToDateTime(StartDate);
            DateTime sEndDate = Convert.ToDateTime(EndDate);
            _IsOT = IsOT;
            int rowIndex = 2;
            int _rowIndex = 0;
            int nMaxColumn = 0;
            int colIndex = 1;
            ExcelRange cell;
            Border border;
            ExcelFill fill;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                #region Details Report
                var sheet = excelPackage.Workbook.Worksheets.Add("Salary Sheet");
                sheet.Name = "OT Sheet";
                sheet.View.FreezePanes(8, 1);
                Dictionary<string, string> aGrandTotals = new Dictionary<string, string>();
                string sStartCell = "", sEndCell = "";
                int nStartRow = 0, nStartCol = 0;
                int nCount = 0;
                #region Declare Column
                colIndex = 0;
                sheet.Column(++colIndex).Width = 8;//SL
                #region Emp Info
               
                    sheet.Column(++colIndex).Width = 10;//Code
                    sheet.Column(++colIndex).Width = 10;//Name
                    sheet.Column(++colIndex).Width = 10;//Designation
                    sheet.Column(++colIndex).Width = 10;//Grade
                    sheet.Column(++colIndex).Width = 10;//Joining Date
                    sheet.Column(++colIndex).Width = 10;//Employee Type
                    sheet.Column(++colIndex).Width = 10;//Payment Type
                #endregion



                for (int i = 0; i < oEmployeeSalaryBasics.Count(); i++)
                {
                    sheet.Column(++colIndex).Width = 8;
                }



                sheet.Column(++colIndex).Width = 8;//Gross Salary
                sheet.Column(++colIndex).Width = 8;//OT Rate
                sheet.Column(++colIndex).Width = 8;//Extra OT Hour
                sheet.Column(++colIndex).Width = 8;//OT Amount
                sheet.Column(++colIndex).Width = 9;//Signature
                nMaxColumn = colIndex;
                #endregion
                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 6]; cell.Merge = true; cell.Value = oEmployeeSalaryV2s[0].BUName; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 6]; cell.Merge = true; cell.Value = "OT Sheet"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 6]; cell.Merge = true; cell.Value = "From " + sStartDate.ToString("dd MMM yyyy") + " To " + sEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;
                #endregion
                int nCheckColSetup = 0;
                oEmployeeSalaryV2s.ForEach(x => oTempEmployeeSalaryV2s.Add(x));
                oEmployeeSalaryV2s.ForEach(x => _oSummaryEmployeeSalaryV2s.Add(x));
                string PrevBUName = "";
                while (oEmployeeSalaryV2s.Count > 0)
                {
                    var oResults = oEmployeeSalaryV2s.Where(x => x.BUID == oEmployeeSalaryV2s[0].BUID && x.LocationID == oEmployeeSalaryV2s[0].LocationID && x.DepartmentID == oEmployeeSalaryV2s[0].DepartmentID).OrderBy(x => x.EmployeeCode).ToList();


                    #region Report Body
                    if (nGroupBySerial !=2 || PrevBUName != oResults[0].BUShortName)//1 for Group By Dept or Default and 2 fro Serial
                    {
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 4]; cell.Merge = true; cell.Value = "Unit : " + oResults[0].BUShortName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    }


                    if (nGroupBySerial !=2)
                    {
                        cell = sheet.Cells[rowIndex, 5, rowIndex, 9]; cell.Merge = true; cell.Value = "Department : " + oResults.FirstOrDefault().DepartmentName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rowIndex++;
                    }
                    else
                    {
                        if (PrevBUName != oResults[0].BUShortName)
                        {
                            rowIndex++;
                            PrevBUName = oResults[0].BUShortName;
                            nStartRow = rowIndex;
                        }
                    }


                    #endregion
                    if (nCheckColSetup == 0)
                    {
                        #region Column SetUp
                        colIndex = 0;
                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 1, colIndex]; cell.Merge = true; cell.Value = "SL#"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    
                            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + 6]; cell.Merge = true; cell.Value = "Employee Information"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        

                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + oEmployeeSalaryBasics.Count + 3]; cell.Merge = true; cell.Value = "Total Gross"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 1, colIndex]; cell.Merge = true; cell.Value = "Signature"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                        colIndex = 1;
                      
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Employee Code"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                 
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Employee Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        
                    
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Grade"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        
                    
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Joining date"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        

                     
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Employee Type"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        

                       
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Payment Type"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        


                        foreach (EmployeeSalaryDetailV2 oItem in oEmployeeSalaryBasics)
                        {

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.SalaryHeadName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Gross Salary"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "OT Rate"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Extra OT Hour"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "OT Amount"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                        #endregion
                        nCheckColSetup = 1;
                        nStartRow = rowIndex;
                    }


                    #region SalarySheet Data

                    if (nGroupBySerial <= 1)
                    {
                        nCount = 0;
                        nStartRow = rowIndex;
                    }


                    int nTotalRowPage = 0;



                    foreach (EmployeeSalaryV2 oEmpSalaryItem in oResults)
                    {
                        nTotalRowPage++;
                        nCount++;
                        colIndex = 0;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nCount.ToString();
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                     
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.EmployeeCode; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        

                   
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.EmployeeName; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        
                       
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.DesignationName; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        
                      
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.Grade; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.JoiningDate.ToString("dd MMM yyyy"); cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.EmployeeTypeName; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.PaymentType; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        foreach (EmployeeSalaryDetailV2 oEmpBasics in oEmployeeSalaryBasics)
                        {
                            double nBasic = oEmpSalaryItem.EmployeeSalaryBasics.Where(x => x.SalaryHeadID == oEmpBasics.SalaryHeadID).Select(x => x.Amount).FirstOrDefault();
                            oEmpBasics.SubTotalAmount = oEmpBasics.SubTotalAmount + nBasic;
                            oEmpBasics.GrandTotalAmount = oEmpBasics.GrandTotalAmount + nBasic;
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nBasic; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.GrossAmount; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.OTRatePerHour; cell.Style.Numberformat.Format = "#,##,##0.00;(#,##,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.ExtraOTHour; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.OTRatePerHour * oEmpSalaryItem.ExtraOTHour; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "--";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }
                    #endregion


                    oEmployeeSalaryV2s.RemoveAll(x => x.BUID == oResults[0].BUID && x.LocationID == oResults[0].LocationID && x.DepartmentID == oResults[0].DepartmentID);
                    if (((oEmployeeSalaryV2s.Count <= 0) || (nGroupBySerial == 2 && oEmployeeSalaryV2s[0].BUShortName != PrevBUName)) || nGroupBySerial != 2)
                    {
                        #region SubTotal
                        colIndex = 0;
                        aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));
                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + 7]; cell.Merge = true; cell.Value = "Sub Total";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        foreach (EmployeeSalaryDetailV2 oEmpBasics in oEmployeeSalaryBasics)
                        {
                            nStartCol = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }



                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "--"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        ResetSubTotal(oEmployeeSalaryBasics, oEmployeeSalaryAllowances, oEmployeeSalaryDeductions);
                        rowIndex += 2;
                        #endregion
                    }

                }
                #region GrandTotal
                string sFormula = ""; int nTempStartRow = 0, nTempEndRow = 0;
                int nTotalIndex = 0, nEndRow = rowIndex - 1;
                colIndex = 0;
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + 7]; cell.Merge = true; cell.Value = "Grand Total";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nTotalIndex = colIndex;


                foreach (EmployeeSalaryDetailV2 oEmpBasics in oEmployeeSalaryBasics)
                {
                    #region Formula
                    sFormula = "";
                    if (aGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= aGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }




                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 2;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "--"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                _rowIndex = rowIndex;

                #endregion
                #endregion
                #region Summary Report
                aGrandTotals = new Dictionary<string, string>();
                var Summarysheet = excelPackage.Workbook.Worksheets.Add("Salary Summary");
                Summarysheet.Name = "Salary Summary";
                colIndex = 2;
                Summarysheet.View.FreezePanes(6, 1);
                Summarysheet.Column(++colIndex).Width = 8; //SL
                Summarysheet.Column(++colIndex).Width = 20; //Department
                Summarysheet.Column(++colIndex).Width = 10; //Man Power
                Summarysheet.Column(++colIndex).Width = 10; //OT Hour
                Summarysheet.Column(++colIndex).Width = 15; //Total OT
                Summarysheet.Column(++colIndex).Width = 13; //Gross


                colIndex = 2;
                rowIndex = 2;


                while (_oSummaryEmployeeSalaryV2s.Count > 0)
                {

                    string BUName = _oSummaryEmployeeSalaryV2s[0].BUName;
                    #region Report Header
                    cell = Summarysheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 6]; cell.Merge = true; cell.Value = BUName; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;
                    cell = Summarysheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 6]; cell.Merge = true; cell.Value = "Salary Sheet"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;

                    cell = Summarysheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 6]; cell.Merge = true; cell.Value = "From " + sStartDate.ToString("dd MMM yyyy") + " To " + sEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;

                    #endregion
                    oEmployeeSalaryV2s = _oSummaryEmployeeSalaryV2s.Where(x => x.BUName == BUName).ToList();



                    var oDBMonthlyAttendanceReports = oEmployeeSalaryV2s.GroupBy(x => new { x.DepartmentName }, (key, grp) => new
                    {
                        DepartmentName = key.DepartmentName,
                        EmpCount = grp.ToList().Count(),
                        GrossAmount = grp.ToList().Sum(y => y.GrossAmount),
                        OTHour = grp.ToList().Sum(y => y.ExtraOTHour),
                        TotalOT = grp.ToList().Sum(y => y.ExtraOTHour * y.OTRatePerHour),
                        Results = grp.ToList(),
                        TotalGross = grp.ToList().Sum(x => x.EmployeeSalaryBasics.Sum(y => y.Amount))
                    });
                    colIndex = 2;
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "SL#"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "ManPower"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Extra OT Hour"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Total OT Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Gross"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    rowIndex++;
                    colIndex = 2;
                    nCount = 0;
                    nStartRow = rowIndex;
                    foreach (var oBUEmployeeSalary in oDBMonthlyAttendanceReports)
                    {
                        nCount++;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = nCount; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.DepartmentName; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.EmpCount; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                       

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.OTHour; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    


                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.TotalOT; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                      

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.GrossAmount; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                      

                        rowIndex++;
                        colIndex = 2;
                    }
                    #region SubTotal
                    aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));
                    cell = Summarysheet.Cells[rowIndex, ++colIndex, rowIndex, ++colIndex]; cell.Merge = true; cell.Value = "SubTotal:"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nStartCol = colIndex + 1;
                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                   

                    nStartCol = colIndex + 1;
                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    


                    nStartCol = colIndex + 1;
                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                   


                    nStartCol = colIndex + 1;
                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    

                    #endregion
                    _oSummaryEmployeeSalaryV2s.RemoveAll(x => x.BUName == BUName);
                    rowIndex++;
                }

                #region GrandTotal
            
                colIndex = 2;

                cell = Summarysheet.Cells[rowIndex, ++colIndex, rowIndex, ++colIndex]; cell.Merge = true; cell.Value = "GrandTotal:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nTotalIndex = colIndex;
                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion

                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                #endregion
                #endregion

                cell = sheet.Cells[1, 1, _rowIndex + 3, nMaxColumn + 3];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SalarySheet.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        private void SalarySheetXLF3Portrait(int nGroupBySerial, List<EmployeeSalaryV2> oEmployeeSalaryV2s, List<LeaveHead> oLeaveHeads, List<EmployeeSalaryDetailV2> oEmployeeSalaryBasics, List<EmployeeSalaryDetailV2> oEmployeeSalaryAllowances, List<EmployeeSalaryDetailV2> oEmployeeSalaryDeductions, Company oCompany, SalaryField oSalaryField, EnumPageOrientation ePageOrientation, List<SalarySheetSignature> oSalarySheetSignatures, string StartDate, string EndDate, bool IsOT)
        {

            bool _IsOT = false;
            List<EmployeeSalaryV2> oTempEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            List<EmployeeSalaryV2> _oSummaryEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            DateTime sStartDate = Convert.ToDateTime(StartDate);
            DateTime sEndDate = Convert.ToDateTime(EndDate);
            _IsOT = IsOT;
            int rowIndex = 2;
            int _rowIndex = 0;
            int nMaxColumn = 0;
            int colIndex = 1;
            ExcelRange cell;
            Border border;
            ExcelFill fill;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                #region Details Report
                var sheet = excelPackage.Workbook.Worksheets.Add("OT Sheet");
                sheet.Name = "OT Sheet";
                sheet.View.FreezePanes(7, 1);
                Dictionary<string, string> aGrandTotals = new Dictionary<string, string>();
                string sStartCell = "", sEndCell = "";
                int nStartRow = 0, nStartCol = 0;
                int nCount = 0;
                #region Declare Column
                colIndex = 0;
                sheet.Column(++colIndex).Width = 8;//SL
                #region Emp Info

                sheet.Column(++colIndex).Width = 10;//Code
                sheet.Column(++colIndex).Width = 10;//Name
                sheet.Column(++colIndex).Width = 10;//Designation
                sheet.Column(++colIndex).Width = 10;//Joining
                #endregion

                sheet.Column(++colIndex).Width = 8;//Gross
                sheet.Column(++colIndex).Width = 8;//Basic
                sheet.Column(++colIndex).Width = 8;//OT Rate
                sheet.Column(++colIndex).Width = 8;//OT Hour
                sheet.Column(++colIndex).Width = 8;//OT
                sheet.Column(++colIndex).Width = 9;//Signature
                nMaxColumn = colIndex;
                #endregion
                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 6]; cell.Merge = true; cell.Value = oEmployeeSalaryV2s[0].BUName; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 6]; cell.Merge = true; cell.Value = "OT Sheet"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 6]; cell.Merge = true; cell.Value = "From " + sStartDate.ToString("dd MMM yyyy") + " To " + sEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;
                #endregion
                int nCheckColSetup = 0;
                oEmployeeSalaryV2s.ForEach(x => oTempEmployeeSalaryV2s.Add(x));
                oEmployeeSalaryV2s.ForEach(x => _oSummaryEmployeeSalaryV2s.Add(x));
                string PrevBUName = "";
                while (oEmployeeSalaryV2s.Count > 0)
                {
                    var oResults = oEmployeeSalaryV2s.Where(x => x.BUID == oEmployeeSalaryV2s[0].BUID && x.LocationID == oEmployeeSalaryV2s[0].LocationID && x.DepartmentID == oEmployeeSalaryV2s[0].DepartmentID).OrderBy(x => x.EmployeeCode).ToList();


                    #region Report Body
                    if (nGroupBySerial !=2 || PrevBUName != oResults[0].BUShortName)//1 for Group By Dept or Default and 2 fro Serial
                    {
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 4]; cell.Merge = true; cell.Value = "Unit : " + oResults[0].BUShortName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    }


                    if (nGroupBySerial !=2)
                    {
                        cell = sheet.Cells[rowIndex, 5, rowIndex, 9]; cell.Merge = true; cell.Value = "Department : " + oResults.FirstOrDefault().DepartmentName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rowIndex++;
                    }
                    else
                    {
                        if (PrevBUName != oResults[0].BUShortName)
                        {
                            rowIndex++;
                            PrevBUName = oResults[0].BUShortName;
                            nStartRow = rowIndex;
                        }
                    }


                    #endregion
                    if (nCheckColSetup == 0)
                    {
                        #region Column SetUp
                        colIndex = 0;
                        cell = sheet.Cells[rowIndex, ++colIndex];cell.Value = "#SL"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

               


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Joining"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Gross"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Basic"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "OT Rate"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "OT Hour"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "OT"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex];  cell.Value = "Signature"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                        #endregion
                        nCheckColSetup = 1;
                        nStartRow = rowIndex;
                    }


                    #region SalarySheet Data

                    if (nGroupBySerial <= 1)
                    {
                        nCount = 0;
                        nStartRow = rowIndex;
                    }


                    int nTotalRowPage = 0;



                    foreach (EmployeeSalaryV2 oEmpSalaryItem in oResults)
                    {
                        nTotalRowPage++;
                        nCount++;
                        colIndex = 0;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nCount.ToString();
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.EmployeeCode; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.EmployeeName; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.DesignationName; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.JoiningDate.ToString("dd MMM yyyy"); cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.GrossAmount; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        double nBasic = oEmpSalaryItem.EmployeeSalaryBasics.Where(x => x.SalaryHeadName == "Basic").Select(x => x.Amount).FirstOrDefault();

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nBasic; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.OTRatePerHour; cell.Style.Numberformat.Format = "#,##,##0.00;(#,##,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.ExtraOTHour; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpSalaryItem.OTRatePerHour * oEmpSalaryItem.ExtraOTHour; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "--";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }
                    #endregion


                    oEmployeeSalaryV2s.RemoveAll(x => x.BUID == oResults[0].BUID && x.LocationID == oResults[0].LocationID && x.DepartmentID == oResults[0].DepartmentID);
                    if (((oEmployeeSalaryV2s.Count <= 0) || (nGroupBySerial == 2 && oEmployeeSalaryV2s[0].BUShortName != PrevBUName)) || nGroupBySerial !=2)
                    {
                        #region SubTotal
                        colIndex = 0;
                        aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));
                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + 4]; cell.Merge = true; cell.Value = "Sub Total";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nStartCol = colIndex + 1;
                        sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "--"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        ResetSubTotal(oEmployeeSalaryBasics, oEmployeeSalaryAllowances, oEmployeeSalaryDeductions);
                        rowIndex += 2;
                        #endregion
                    }

                }
                #region GrandTotal
                string sFormula = ""; int nTempStartRow = 0, nTempEndRow = 0;
                int nTotalIndex = 0, nEndRow = rowIndex - 1;
                colIndex = 0;
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + 4]; cell.Merge = true; cell.Value = "Grand Total";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nTotalIndex = colIndex;


                    #region Formula
                    sFormula = "";
                    if (aGrandTotals.Count > 0)
                    {
                        nTotalIndex = nTotalIndex + 1;
                        sFormula = "SUM(";
                        for (int i = 1; i <= aGrandTotals.Count; i++)
                        {
                            nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                            nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                            sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                            sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                            sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                        }
                        if (sFormula.Length > 0)
                        {
                            sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                        }
                        sFormula = sFormula + ")";
                    }
                    else
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                        sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    }
                    #endregion

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 2;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "--"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                _rowIndex = rowIndex;

                #endregion
                #endregion
                #region Summary Report
                aGrandTotals = new Dictionary<string, string>();
                var Summarysheet = excelPackage.Workbook.Worksheets.Add("Salary Summary");
                Summarysheet.Name = "Salary Summary";
                colIndex = 0;
                Summarysheet.View.FreezePanes(6, 1);
                Summarysheet.Column(++colIndex).Width = 8; //SL
                Summarysheet.Column(++colIndex).Width = 20; //Department
                Summarysheet.Column(++colIndex).Width = 10; //Man Power
                Summarysheet.Column(++colIndex).Width = 10; //OT Hour
                Summarysheet.Column(++colIndex).Width = 15; //Total OT
                Summarysheet.Column(++colIndex).Width = 13; //Gross


                colIndex = 2;
                rowIndex = 2;


                while (_oSummaryEmployeeSalaryV2s.Count > 0)
                {

                    string BUName = _oSummaryEmployeeSalaryV2s[0].BUName;
                    #region Report Header
                    cell = Summarysheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 6]; cell.Merge = true; cell.Value = BUName; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;
                    cell = Summarysheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 6]; cell.Merge = true; cell.Value = "Salary Sheet"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;

                    cell = Summarysheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 6]; cell.Merge = true; cell.Value = "From " + sStartDate.ToString("dd MMM yyyy") + " To " + sEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;

                    #endregion
                    oEmployeeSalaryV2s = _oSummaryEmployeeSalaryV2s.Where(x => x.BUName == BUName).ToList();



                    var oDBMonthlyAttendanceReports = oEmployeeSalaryV2s.GroupBy(x => new { x.DepartmentName }, (key, grp) => new
                    {
                        DepartmentName = key.DepartmentName,
                        EmpCount = grp.ToList().Count(),
                        GrossAmount = grp.ToList().Sum(y => y.GrossAmount),
                        OTHour = grp.ToList().Sum(y => y.ExtraOTHour),
                        TotalOT = grp.ToList().Sum(y => y.ExtraOTHour * y.OTRatePerHour),
                        Results = grp.ToList(),
                        TotalGross = grp.ToList().Sum(x => x.EmployeeSalaryBasics.Sum(y => y.Amount))
                    });
                    colIndex = 0;
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "SL#"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "ManPower"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Extra OT Hour"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Total OT Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Gross"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    rowIndex++;
                    colIndex = 0;
                    nCount = 0;
                    nStartRow = rowIndex;
                    foreach (var oBUEmployeeSalary in oDBMonthlyAttendanceReports)
                    {
                        nCount++;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = nCount; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.DepartmentName; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.EmpCount; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.OTHour; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.TotalOT; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oBUEmployeeSalary.GrossAmount; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        rowIndex++;
                        colIndex = 0;
                    }
                    #region SubTotal
                    aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));
                    cell = Summarysheet.Cells[rowIndex, ++colIndex, rowIndex, ++colIndex]; cell.Merge = true; cell.Value = "SubTotal:"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nStartCol = colIndex + 1;
                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nStartCol = colIndex + 1;
                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    nStartCol = colIndex + 1;
                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    nStartCol = colIndex + 1;
                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    #endregion
                    _oSummaryEmployeeSalaryV2s.RemoveAll(x => x.BUName == BUName);
                    rowIndex++;
                }

                #region GrandTotal

                colIndex = 0;

                cell = Summarysheet.Cells[rowIndex, ++colIndex, rowIndex, ++colIndex]; cell.Merge = true; cell.Value = "GrandTotal:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nTotalIndex = colIndex;
                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion

                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                #endregion
                #endregion

                cell = sheet.Cells[1, 1, _rowIndex + 3, nMaxColumn + 3];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SalarySheet.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        private void ExportIntoExcelSalarySheet_F4(List<EmployeeSalaryV2> oAMGSalarySheets, Company oCompany, string salaryMonth, DateTime StartDate)
        {

            string BlockIds = string.Empty;
         
            string sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignatures = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


            #region Header
            ArrayList table_header = new ArrayList();
            table_header.Add(new TableHeader { Header = "#SL", Width = 5f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Employee Code", Width = 10f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "EMployee Name", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Designation", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Join Date", Width = 10f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Total Days", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Present Day", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Day Off Holiday", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Absent Days", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "CL", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "SL", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "EL", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "LWP", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Leave Days", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "EWD", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Gross Salary", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Att Bonous", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Gross Earnings", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Basic", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "House Rent", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Conveyance", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Food", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Medical", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Absent Amt", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Stamp", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Total Deduct", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Net Amount", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Bank", Width = 10f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Cash", Width = 10f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Account No", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Bank Name", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Signature", Width = 15f, IsRotate = false });
            #endregion


            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Salary Sheet F4");
                sheet.Name = "Salary Sheet F4";

                foreach (TableHeader listItem in table_header)
                {
                    sheet.Column(nStartCol++).Width = listItem.Width;
                }

                #region Report Header
                //int nmiddlecol = Convert.ToInt32(nEndCol / 2);
                cell = sheet.Cells[nRowIndex, 4, nRowIndex, 15]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 4, nRowIndex, 15]; cell.Merge = true;
                cell.Value = "Salary Sheet"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.Font.Color.SetColor(Color.Black); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;


                nStartCol = 2;
                foreach (TableHeader listItem in table_header)
                {
                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                #endregion


                #region Body
                int nCount = 0;
                oAMGSalarySheets = oAMGSalarySheets.OrderBy(x => x.BUName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x=>x.EmployeeCode).ToList();
                var data = oAMGSalarySheets.GroupBy(x => new { x.BUName, x.LocationName, x.DepartmentName }, (key, grp) => new
                {
                    BUName = key.BUName,
                    LocName = key.LocationName,
                    DptName = key.DepartmentName,
                    Results = grp.ToList()
                });

                foreach (var oItem in data)
                {
                    nRowIndex--;
                    nStartCol = 2;
                    cell = sheet.Cells[nRowIndex, nStartCol++, nRowIndex, nStartCol + 7]; cell.Merge = true; cell.Value = "Unit Name: "+oItem.LocName+" Dept. Name: "+oItem.DptName; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    nRowIndex+=2;

                    foreach (var obj in oItem.Results)
                    {
                        nStartCol = 2;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = (++nCount).ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.EmployeeCode; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.EmployeeName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.DesignationName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.JoiningDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        int days = DateTime.DaysInMonth(StartDate.Year, StartDate.Month);
                        if(obj.JoiningDate>StartDate)
                        {
                            days = days - Convert.ToInt32(obj.JoiningDate.ToString("dd"));
                        }

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = days; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        int NTotalLeave = (obj.CL + obj.ML + obj.EL + obj.LWP);

                        int nTPresentDays = days - (obj.TotalDayOff + obj.TotalHoliday + NTotalLeave + obj.TotalAbsent);

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = nTPresentDays; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = (obj.TotalDayOff + obj.TotalHoliday); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.TotalAbsent; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.CL; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.ML; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.EL; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.LWP; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = (obj.CL + obj.ML + obj.EL + obj.LWP); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        int nTWorkingDay = nTPresentDays + obj.TotalDayOff + obj.TotalHoliday + NTotalLeave - obj.LWP;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = nTWorkingDay; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.GrossAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.AttendanceBonus; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        double GrossEarnings = obj.BasicAmount + obj.HouseRentAmount + obj.ConveyanceAmount + obj.FoodAmount + obj.MedicalAmount;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = GrossEarnings; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.BasicAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.HouseRentAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.ConveyanceAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.FoodAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.MedicalAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.AbsentAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.StampAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        double nTotalDeductionAmt = obj.StampAmount + obj.AbsentAmount;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = nTotalDeductionAmt; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.NetAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = (string.IsNullOrEmpty(obj.AccountNo)) ? 0 : obj.NetAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,###;(#,###)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        double nNet= (string.IsNullOrEmpty(obj.AccountNo)) ?obj.NetAmount:0;
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value =nNet; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,###;(#,###)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.AccountNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.BankName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "-"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }
                    nRowIndex+=2;
                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SalarySheet-F4.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        private void ExportIntoExcelSalarySheet_F5(List<EmployeeSalaryV2> oAMGSalarySheets, Company oCompany, string salaryMonth, DateTime StartDate)
        {

            string BlockIds = string.Empty;

            string sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignatures = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


            #region Header
            ArrayList table_header = new ArrayList();
            table_header.Add(new TableHeader { Header = "#SL", Width = 5f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Employee Code", Width = 10f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "EMployee Name", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Designation", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Join Date", Width = 10f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Total Days", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Present Day", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Day Off Holiday", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Absent Days", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "CL", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "SL", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "EL", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "LWP", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Leave Days", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "EWD", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Gross Salary", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Att Bonous", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Gross Earnings", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Basic", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "House Rent", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Conveyance", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Food", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Medical", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Absent Amt", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Stamp", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Total Deduct", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Net Amount", Width = 10f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Bank", Width = 10f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Cash", Width = 10f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Account No", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Bank Name", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Signature", Width = 15f, IsRotate = false });
            #endregion


            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Salary Sheet F5");
                sheet.Name = "Salary Sheet F5";

                foreach (TableHeader listItem in table_header)
                {
                    sheet.Column(nStartCol++).Width = listItem.Width;
                }

                #region Report Header
                //int nmiddlecol = Convert.ToInt32(nEndCol / 2);
                cell = sheet.Cells[nRowIndex, 4, nRowIndex, 15]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 4, nRowIndex, 15]; cell.Merge = true;
                cell.Value = "Salary Sheet"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.Font.Color.SetColor(Color.Black); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;


                nStartCol = 2;
                foreach (TableHeader listItem in table_header)
                {
                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                #endregion


                #region Body
                int nCount = 0;
                oAMGSalarySheets = oAMGSalarySheets.OrderBy(x => x.BUName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
                var data = oAMGSalarySheets.GroupBy(x => new { x.BUName, x.LocationName, x.DepartmentName }, (key, grp) => new
                {
                    BUName = key.BUName,
                    LocName = key.LocationName,
                    DptName = key.DepartmentName,
                    Results = grp.ToList()
                });

                foreach (var oItem in data)
                {
                    nRowIndex--;
                    nStartCol = 2;
                    cell = sheet.Cells[nRowIndex, nStartCol++, nRowIndex, nStartCol + 7]; cell.Merge = true; cell.Value = "Unit Name: " + oItem.LocName + " Dept. Name: " + oItem.DptName; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    nRowIndex += 2;

                    foreach (var obj in oItem.Results)
                    {
                        nStartCol = 2;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = (++nCount).ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.EmployeeCode; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.EmployeeName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.DesignationName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.JoiningDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        int days = DateTime.DaysInMonth(StartDate.Year, StartDate.Month);
                        if (obj.JoiningDate > StartDate)
                        {
                            days = days - Convert.ToInt32(obj.JoiningDate.ToString("dd"));
                        }

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = days; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        int NTotalLeave = (obj.CL + obj.ML + obj.EL + obj.LWP);

                        int nTPresentDays = days - (obj.TotalDayOff + obj.TotalHoliday + NTotalLeave + obj.TotalAbsent);

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = nTPresentDays; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = (obj.TotalDayOff + obj.TotalHoliday); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.TotalAbsent; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.CL; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.ML; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.EL; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.LWP; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = (obj.CL + obj.ML + obj.EL + obj.LWP); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        int nTWorkingDay = nTPresentDays + obj.TotalDayOff + obj.TotalHoliday + NTotalLeave - obj.LWP;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = nTWorkingDay; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.GrossAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.AttendanceBonus; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        double GrossEarnings = obj.BasicAmount + obj.HouseRentAmount + obj.ConveyanceAmount + obj.FoodAmount + obj.MedicalAmount;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = GrossEarnings; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.BasicAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.HouseRentAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.ConveyanceAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.FoodAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.MedicalAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.AbsentAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.StampAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        double nTotalDeductionAmt = obj.StampAmount + obj.AbsentAmount;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = nTotalDeductionAmt; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.NetAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value =""; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = obj.NetAmount; cell.Style.Font.Bold = false; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "-"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }
                    nRowIndex += 2;
                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SalarySheet-F5.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion

        #region Pay Slip XL
        public string NumberFormat(string sNum)
        {
            char[] NumbersInBangla = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char[] NumbersInEnglish = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            char[] arr = sNum.ToCharArray();

            foreach (char ch in arr)
            {
                int i = 0;
                while (i != 10)
                {
                    if (ch == NumbersInEnglish[i])
                    {
                        sNum = sNum.Replace(ch, NumbersInBangla[i]);
                        break;
                    }
                    i++;
                }
            }
            return sNum;
        }

        private string GetAmountInStr(double amount, bool bIsround, bool bWithPrecision)
        {
            amount = (bIsround) ? Math.Round(amount, 0) : Math.Round(amount, 2);
            return amount.ToString();
        }

        private string getDeducHead(int nStartIndex, List<EmployeeSalaryDetailV2> EmployeeSalaryDeductions)
        {
            if (nStartIndex <= EmployeeSalaryDeductions.Count() - 1)
            {

                return EmployeeSalaryDeductions[nStartIndex].SalaryHeadName;
            }
            else
            {
                return "";
            }

        }

        private double getDeducAmount(int nStartIndex, List<EmployeeSalaryDetailV2> EmployeeSalaryDeductions)
        {
            if (nStartIndex <= EmployeeSalaryDeductions.Count() - 1)
            {

                return EmployeeSalaryDeductions[nStartIndex].Amount;
            }
            else
            {
                return 0f;
            }

        }

        private void PrintPaySlipXLF1(int nGroupBySerial, List<EmployeeSalaryV2> oEmployeeSalaryV2s, List<EmployeeSalaryDetailV2> oEmployeeSalaryDetails, Company oCompany, string StartDate, string EndDate, bool IsOT)
        {           
            List<EmployeeSalaryV2> oTempEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            List<EmployeeSalaryV2> _oSummaryEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            DateTime sStartDate = Convert.ToDateTime(StartDate);
            DateTime sEndDate = Convert.ToDateTime(EndDate);          
            int rowIndex = 2;          
            int nMaxColumn = 0, nMaxColFirst = 0;
            int colIndex = 1;
            ExcelRange cell;
            Border border;
            ExcelFill fill;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                #region Details Report
                var sheet = excelPackage.Workbook.Worksheets.Add("Pay Slip");
                sheet.PrinterSettings.TopMargin = 0.2M;
                sheet.PrinterSettings.LeftMargin = 0.2M;
                sheet.PrinterSettings.BottomMargin = 0.2M;
                sheet.PrinterSettings.RightMargin = 0.2M;
                sheet.PrinterSettings.Orientation = eOrientation.Landscape;
                sheet.PrinterSettings.PaperSize = ePaperSize.A4;
                sheet.Name = "Pay Slip";

                #region Column Declare
                sheet.Column(1).Width = 3;//Extra
                sheet.Column(2).Width = 2; //Blank
                sheet.Column(3).Width = 10;//Caption
                sheet.Column(4).Width = 2; //:
                sheet.Column(5).Width = 10;//Basic Salary
                sheet.Column(6).Width = 10;//Other Salary
                sheet.Column(7).Width = 10;//Net Salary
                sheet.Column(8).Width = 2;//Blank

                sheet.Column(9).Width = 4; //Middle Blank

                sheet.Column(10).Width = 2; //Blank
                sheet.Column(11).Width = 10;//Caption
                sheet.Column(12).Width = 2; //:
                sheet.Column(13).Width = 10;//Basic Salary
                sheet.Column(14).Width = 10;//Other Salary
                sheet.Column(15).Width = 10;//Net Salary
                sheet.Column(16).Width = 2;//Blank
                sheet.Column(17).Width = 3;//Extra
                #endregion

                int nPrintCount = 4;
                if (oEmployeeSalaryV2s != null)
                {
                    int nPaySlipCount = 0;
                    oEmployeeSalaryV2s = oEmployeeSalaryV2s.OrderBy(x => x.EmployeeCode).ToList();
                    int nLeftSideRowIndex = 0, nRightSideRowIndex = 0, nTempPrintCount = 0;
                    List<EmployeeSalaryDetailV2> oSalaryHeads = oEmployeeSalaryDetails.GroupBy(p => p.SalaryHeadID).Select(c => c.FirstOrDefault()).ToList().OrderBy(y => y.SalaryHeadType).ToList();                    
                    for (int i = 0; i < oEmployeeSalaryV2s.Count; i = i + 1)
                    {
                        nTempPrintCount = nTempPrintCount + 2;
                        if (i <= (oEmployeeSalaryV2s.Count - 1))
                       {
                           nLeftSideRowIndex = this.FillSalaraySlipComp(sheet, rowIndex, oEmployeeSalaryV2s[i], oEmployeeSalaryDetails, oSalaryHeads, oCompany, 2, IsOT, "Office Copy", ++nPaySlipCount);
                        }

                        if (i <= (oEmployeeSalaryV2s.Count - 1))
                        {
                            nRightSideRowIndex = this.FillSalaraySlipComp(sheet, rowIndex, oEmployeeSalaryV2s[i], oEmployeeSalaryDetails, oSalaryHeads, oCompany, 10, IsOT, "Employee Copy", nPaySlipCount);
                        }

                        rowIndex = nLeftSideRowIndex;
                        if (nRightSideRowIndex > nLeftSideRowIndex)
                        {
                            rowIndex = nRightSideRowIndex;
                        }

                        #region Blank
                        sheet.Row(rowIndex).Height = 20;
                        sheet.Cells[rowIndex, 2, rowIndex, 16].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "";
                        cell.Style.Font.Size = 8; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        if (nTempPrintCount >= nPrintCount)
                        {
                            sheet.Row(rowIndex).PageBreak = true;
                            nTempPrintCount = 0;
                        }
                        rowIndex = rowIndex + 1;
                        #endregion
                    }
                }

                #endregion

                cell = sheet.Cells[1, 1, rowIndex + 3, 20];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Payslip.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        private void PrintPaySlipXLF2(int nGroupBySerial, List<EmployeeSalaryV2> oEmployeeSalaryV2s, List<EmployeeSalaryDetailV2> oEmployeeSalaryDetails, Company oCompany, string StartDate, string EndDate, bool IsOT)
        {

            List<EmployeeSalaryV2> oTempEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            List<EmployeeSalaryV2> _oSummaryEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            DateTime sStartDate = Convert.ToDateTime(StartDate);
            DateTime sEndDate = Convert.ToDateTime(EndDate);
            int rowIndex = 2;
            int nMaxColumn = 0, nMaxColFirst = 0;
            int colIndex = 1;
            ExcelRange cell;
            Border border;
            ExcelFill fill;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                #region Details Report
                var sheet = excelPackage.Workbook.Worksheets.Add("Pay Slip2");
                sheet.PrinterSettings.TopMargin = 0.2M;
                sheet.PrinterSettings.LeftMargin = 0.2M;
                sheet.PrinterSettings.BottomMargin = 0.2M;
                sheet.PrinterSettings.RightMargin = 0.2M;
                sheet.PrinterSettings.Orientation = eOrientation.Landscape;
                sheet.PrinterSettings.PaperSize = ePaperSize.A4;
                sheet.Name = "Pay Slip";

                #region Column Declare
                sheet.Column(1).Width = 3;//Extra
                sheet.Column(2).Width = 2; //Blank
                sheet.Column(3).Width = 10;//Caption
                sheet.Column(4).Width = 2; //:
                sheet.Column(5).Width = 10;//Basic Salary
                sheet.Column(6).Width = 10;//Other Salary
                sheet.Column(7).Width = 10;//Net Salary
                sheet.Column(8).Width = 2;//Blank

                sheet.Column(9).Width = 4; //Middle Blank

                sheet.Column(10).Width = 2; //Blank
                sheet.Column(11).Width = 10;//Caption
                sheet.Column(12).Width = 2; //:
                sheet.Column(13).Width = 10;//Basic Salary
                sheet.Column(14).Width = 10;//Other Salary
                sheet.Column(15).Width = 10;//Net Salary
                sheet.Column(16).Width = 2;//Blank
                sheet.Column(17).Width = 3;//Extra
                #endregion

                int nPrintCount = 4;
                if (oEmployeeSalaryV2s != null)
                {
                    int nPaySlipCount = 0;
                    oEmployeeSalaryV2s = oEmployeeSalaryV2s.OrderBy(x => x.EmployeeCode).ToList();
                    int nLeftSideRowIndex = 0, nRightSideRowIndex = 0, nTempPrintCount = 0;
                    List<EmployeeSalaryDetailV2> oSalaryHeads = oEmployeeSalaryDetails.GroupBy(p => p.SalaryHeadID).Select(c => c.FirstOrDefault()).ToList().OrderBy(y => y.SalaryHeadType).ToList();
                    for (int i = 0; i < oEmployeeSalaryV2s.Count; i = i + 2)
                    {
                        nTempPrintCount = nTempPrintCount + 2;
                        if (i <= (oEmployeeSalaryV2s.Count - 1))
                        {
                            nLeftSideRowIndex = this.FillSalaraySlipComp(sheet, rowIndex, oEmployeeSalaryV2s[i], oEmployeeSalaryDetails, oSalaryHeads, oCompany, 2, IsOT, "", ++nPaySlipCount);
                        }

                        if ((i + 1) <= (oEmployeeSalaryV2s.Count - 1))
                        {
                            nRightSideRowIndex = this.FillSalaraySlipComp(sheet, rowIndex, oEmployeeSalaryV2s[i + 1], oEmployeeSalaryDetails, oSalaryHeads, oCompany, 10, IsOT, "", ++nPaySlipCount);
                        }

                        rowIndex = nLeftSideRowIndex;
                        if (nRightSideRowIndex > nLeftSideRowIndex)
                        {
                            rowIndex = nRightSideRowIndex;
                        }

                        #region Blank
                        sheet.Row(rowIndex).Height = 20;
                        sheet.Cells[rowIndex, 2, rowIndex, 16].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "";
                        cell.Style.Font.Size = 8; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        if (nTempPrintCount >= nPrintCount)
                        {
                            sheet.Row(rowIndex).PageBreak = true;
                            nTempPrintCount = 0;
                        }
                        rowIndex = rowIndex + 1;
                        #endregion
                    }
                }

                #endregion

                cell = sheet.Cells[1, 1, rowIndex + 3, 20];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Payslip2.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        private void PrintPaySlipXLF3(int nGroupBySerial, List<EmployeeSalaryV2> oEmployeeSalaryV2s, List<LeaveHead> oLeaveHeads, List<EmployeeSalaryDetailV2> oEmployeeSalaryBasics, List<EmployeeSalaryDetailV2> oEmployeeSalaryAllowances, List<EmployeeSalaryDetailV2> oEmployeeSalaryDeductions, Company oCompany, SalaryField oSalaryField, EnumPageOrientation ePageOrientation, List<SalarySheetSignature> oSalarySheetSignatures, string StartDate, string EndDate, bool IsOT)
        {
            bool _IsOT = false;
            List<EmployeeSalaryV2> oTempEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            List<EmployeeSalaryV2> _oSummaryEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            DateTime sStartDate = Convert.ToDateTime(StartDate);
            DateTime sEndDate = Convert.ToDateTime(EndDate);
            _IsOT = IsOT;
            int rowIndex = 2;
            int nMaxColumn = 0;
            int colIndex = 1;
            ExcelRange cell;
            Border border;
            ExcelFill fill;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                #region Details Report
                var sheet = excelPackage.Workbook.Worksheets.Add("Pay Slip F3");
                sheet.PrinterSettings.TopMargin = 0.2M;
                sheet.PrinterSettings.LeftMargin = 0.2M;
                sheet.PrinterSettings.BottomMargin = 0.2M;
                sheet.PrinterSettings.RightMargin = 0.2M;
                sheet.PrinterSettings.Orientation = eOrientation.Landscape;
                sheet.PrinterSettings.PaperSize = ePaperSize.A4;

                sheet.Name = "Pay Slip";

                #region Declare Column
                oEmployeeSalaryV2s = oEmployeeSalaryV2s.OrderBy(x => x.EmployeeCode).ToList();
                colIndex = 0;
                sheet.Column(++colIndex).Width = 23;//Earnings Head
                sheet.Column(++colIndex).Width = 20;//Earnings Amount(BDT)
                sheet.Column(++colIndex).Width = 23;//Deductions Head
                sheet.Column(++colIndex).Width = 20;//Deductions Amount(BDT)
                nMaxColumn = colIndex;
                #endregion

                foreach (EmployeeSalaryV2 oItem in oEmployeeSalaryV2s)
                {
                    #region Report Header
                    cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn - 1]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;
                    cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn - 1]; cell.Merge = true; cell.Value = oCompany.Address;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Pay Slip"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;
                    rowIndex++;

                    cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn - 1]; cell.Merge = true; cell.Value = "For The Month of " + sStartDate.ToString("MMMM") + ", " + sStartDate.Year; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex += 3;
                    #endregion

                    #region CONFIDENTIAL
                    cell = sheet.Cells[rowIndex, 1]; cell.Value = "CONFIDENTIAL"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                    colIndex = 0;
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Employee Code"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = ": " + oItem.EmployeeCode; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    rowIndex++;

                    colIndex = 0;
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Employee Name"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = ": " + oItem.EmployeeName;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    rowIndex++;

                    colIndex = 0;
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = ": " + oItem.DesignationName;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    rowIndex++;

                    colIndex = 0;
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Date of Joining"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = ": " + oItem.JoiningDate.ToString("dd MMM yyyy");
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    rowIndex++;

                    colIndex = 0;
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Location"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = ": " + oItem.LocationName;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    rowIndex++;

                    colIndex = 0;
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "ETIN"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = ": " + oItem.ETIN;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    rowIndex += 3;

                    colIndex = 0;
                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, ++colIndex]; cell.Merge = true; cell.Value = "Earnings"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, ++colIndex]; cell.Merge = true; cell.Value = "Deductions"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    rowIndex++;

                    colIndex = 0;
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Heads"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Amount(BDT)"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Heads"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Amount(BDT)"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    rowIndex++;

                    int nDeductionRow = 0;

                    foreach (EmployeeSalaryDetailV2 oEmpBasics in oEmployeeSalaryBasics)
                    {

                        double nBasic = oItem.EmployeeSalaryBasics.Where(x => x.SalaryHeadID == oEmpBasics.SalaryHeadID).Select(x => x.Amount).FirstOrDefault();
                        colIndex = 0;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpBasics.SalaryHeadName;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nBasic; cell.Style.Numberformat.Format = "#,##,##0.00;(#,##,##0.00)";
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = getDeducHead(nDeductionRow, oItem.EmployeeSalaryDeductions);
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        double nDeduction = getDeducAmount(nDeductionRow, oItem.EmployeeSalaryDeductions);
                        string sNumberFormat = "";
                        if (nDeduction > 0)
                        {
                            sNumberFormat = "#,##,##0.00;(#,##,##0.00)";
                        }
                        else
                        {
                            sNumberFormat = "#,##,###;(#,##,###)";
                        }
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nDeduction; cell.Style.Numberformat.Format = sNumberFormat;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                        rowIndex++;
                        nDeductionRow++;
                    }

                    foreach (EmployeeSalaryDetailV2 oEmpBasics in oEmployeeSalaryAllowances)
                    {

                        double nBasic = oItem.EmployeeSalaryAllowances.Where(x => x.SalaryHeadID == oEmpBasics.SalaryHeadID).Select(x => x.Amount).FirstOrDefault();
                        colIndex = 0;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oEmpBasics.SalaryHeadName;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nBasic; cell.Style.Numberformat.Format = "#,##,##0.00;(#,##,##0.00)";
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = getDeducHead(nDeductionRow, oItem.EmployeeSalaryDeductions);
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        double nDeduction = getDeducAmount(nDeductionRow, oItem.EmployeeSalaryDeductions);
                        string sNumberFormat = "";
                        if (nDeduction > 0)
                        {
                            sNumberFormat = "#,##,##0.00;(#,##,##0.00)";
                        }
                        else
                        {
                            sNumberFormat = "#,##,###;(#,##,###)";
                        }
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nDeduction; cell.Style.Numberformat.Format = sNumberFormat;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                        rowIndex++;
                        nDeductionRow++;
                    }

                    #region Total Earnings
                    double nTotalEarnings = oItem.EmployeeSalaryBasics.Sum(x => x.Amount) + oItem.EmployeeSalaryAllowances.Sum(x => x.Amount);

                    colIndex = 0;
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Total Earnings"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nTotalEarnings; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##,##0.00;(#,##,##0.00)";
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    double nTotalDeductions = oItem.EmployeeSalaryDeductions.Sum(x => x.Amount);
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Total Deductions"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nTotalDeductions; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##,##0.00;(#,##,##0.00)";
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    rowIndex += 2;
                    #endregion

                    #region Net Payable
                    colIndex = 0;

                    double nNetPayable = nTotalEarnings - nTotalDeductions;
                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, ++colIndex]; cell.Merge = true; cell.Value = "Net Payable"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, ++colIndex]; cell.Merge = true; cell.Value = nNetPayable; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##,##0.00;(#,##,##0.00)";
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    rowIndex += 2;
                    #endregion

                    #region Mode Of Payment
                    colIndex = 0;
                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "MODE OF PAYMENT"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                    colIndex = 0;
                    string sModeMsg = "";
                    if (oItem.AccountNo != "")
                    {
                        sModeMsg = "Through Bank Account:(" + oItem.AccountNo + ")";
                    }
                    else
                    {
                        sModeMsg = "Through Cash";
                    }
                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, ++colIndex]; cell.Merge = true; cell.Value = sModeMsg;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, ++colIndex]; cell.Merge = true; cell.Value = nNetPayable; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##,##0.00;(#,##,##0.00)";
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    #endregion

                    rowIndex += 5;
                    #endregion
                }



                #endregion

                cell = sheet.Cells[1, 1, rowIndex + 3, nMaxColumn + 3];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PaySlipF3.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        private int FillSalaraySlipComp(OfficeOpenXml.ExcelWorksheet sheet, int nRowIndex, EmployeeSalaryV2 oEmployeeSalary, List<EmployeeSalaryDetailV2> oEmployeeSalaryDetails, List<EmployeeSalaryDetailV2>  oSalaryHeads, Company oCompany, int nColumnIndex, bool bIsOT, string sSlipCopy,int nPaySlipCount)
        {
            double nRowHeight = 12;
            float nFontSize = 9.5f;
            string sFontName = "SutonnyMj";

            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            #region Blank with Top/Left/Right Border
            sheet.Row(nRowIndex).Height = 5;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Company Name


            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, nColumnIndex + 1]; cell.Merge = true; cell.Value = nPaySlipCount; cell.Style.Numberformat.Format = "#,##0";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Row(nRowIndex).Height = nRowHeight;
            sheet.Cells[nRowIndex, nColumnIndex+2, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex, (nColumnIndex + 6)]; cell.Value = (oCompany.NameInBangla != "") ? oCompany.NameInBangla : oCompany.Name ;
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; 
            border = cell.Style.Border;  border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region বেতন প্রদান রশিদ
            sheet.Row(nRowIndex).Height = nRowHeight;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "‡eZb c«`vb iwk`";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Salary Date
            sheet.Row(nRowIndex).Height = nRowHeight;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = oEmployeeSalary.StartDate.ToString("dd MMM yyyy") + "-" + oEmployeeSalary.EndDate.ToString("dd MMM yyyy");
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Slip Copy
            sheet.Row(nRowIndex).Height = nRowHeight;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = sSlipCopy;
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Left Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region ইউনিট
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "BDwbU";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = oEmployeeSalary.LocationName;
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region কার্ড নং
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "KvW© bs";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = oEmployeeSalary.EmployeeCode;
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = "c`ex:";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 5), nRowIndex, (nColumnIndex + 6)]; cell.Merge = true;
            cell.Value = oEmployeeSalary.DesignationName;
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region নাম
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "bvg";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = oEmployeeSalary.EmployeeName;
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

         

            #region সেকশন
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "‡mKkb";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = oEmployeeSalary.DepartmentName;
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)];
            cell.Value = "‡M«W:";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 5), nRowIndex, (nColumnIndex + 6)]; cell.Merge = true;
            cell.Value = oEmployeeSalary.EmployeeTypeName;
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;  border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Bottom/Left/Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Left Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region কর্মদিবস
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "Kg©w`em";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = oEmployeeSalary.TotalWorkingDay;
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0";
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            TimeSpan diff = oEmployeeSalary.EndDate - oEmployeeSalary.StartDate;
            int nDays = diff.Days + 1;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; 
            cell.Value = "‡gvU w`b :";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Right.Style=border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)];  cell.Value = nDays;
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0";
            border = cell.Style.Border; border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region উপস্থিত দিন
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "Dcw¯’Z w`b";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            double nAttTotal = 0;
            int days = DateTime.DaysInMonth(oEmployeeSalary.StartDate.Year, oEmployeeSalary.StartDate.Month);
            int pDay = days - (oEmployeeSalary.TotalAbsent + oEmployeeSalary.TotalDayOff + oEmployeeSalary.TotalHoliday + oEmployeeSalary.TotalPLeave + oEmployeeSalary.TotalUpLeave);
            nAttTotal = pDay + oEmployeeSalary.TotalAbsent + oEmployeeSalary.TotalDayOff + oEmployeeSalary.TotalHoliday + oEmployeeSalary.TotalPLeave + oEmployeeSalary.TotalUpLeave;


            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)];
            cell.Value = pDay;
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0";
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                       
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)];
            cell.Value = "‡gvU eÜ :";//মোট বন্ধ :
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border;  border.Right.Style = border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)];  cell.Value = oEmployeeSalary.TotalDayOff;
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0";
            border = cell.Style.Border; border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region ছুটি
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "QywU";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)];
            cell.Value =oEmployeeSalary.TotalUpLeave + oEmployeeSalary.TotalPLeave;
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0";
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 4) ];
            cell.Value = "‡gvU QywU :";//মোট ছুটি :
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = oEmployeeSalary.TotalHoliday;
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0";
            border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region অনুপস্থিত দিন
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "Abycw¯’Z w`b";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = oEmployeeSalary.TotalAbsent;
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0";
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 4), nRowIndex, (nColumnIndex + 5)]; cell.Merge = true; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border;  border.Right.Style= border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region মোট বেতন
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "‡gvU ‡eZb";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = oEmployeeSalary.GrossAmount > 0 ? oEmployeeSalary.GrossAmount : 0;
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Bottom/Left/Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Left Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            double nAmount = 0; string sHeadName = "";
            List<EmployeeSalaryDetailV2> oTempEmployeeSalaryDetails = oEmployeeSalaryDetails.Where(x => x.EmployeeSalaryID == oEmployeeSalary.EmployeeSalaryID).ToList();

            #region Salary Head
            foreach (EmployeeSalaryDetailV2 oItem in oSalaryHeads)
            {
                nAmount = (oTempEmployeeSalaryDetails.Where(x => x.SalaryHeadID == oItem.SalaryHeadID).Count() > 0) ? oTempEmployeeSalaryDetails.Where(x => x.SalaryHeadID == oItem.SalaryHeadID).FirstOrDefault().Amount : 0;
                if (!string.Equals(@oItem.SalaryHeadName.ToLower(), "n/a") && !string.Equals(@oItem.SalaryHeadName.Trim(), ""))
                {
                    sHeadName = ((!string.Equals(@oItem.SalaryHeadNameInBangla.ToLower(), "n/a") && !string.Equals(@oItem.SalaryHeadNameInBangla.Trim(), "")) ? @oItem.SalaryHeadNameInBangla : @oItem.SalaryHeadName);
                }

                sheet.Row(nRowIndex).Height = nRowHeight;
                cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
                cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = sHeadName;
                cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
                cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                if (oItem.SalaryHeadType == EnumSalaryHeadType.Basic)
                {
                    cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value =Math.Round(nAmount); //ICS.Core.Utility.Global.MillionFormat(nAmount).Split('.')[0];
                    cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)];
                    cell.Value = "";
                    cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value ="";
                    cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; 
                    border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;


                }
                else
                {
                    cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                    cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = Math.Round(nAmount);// ICS.Core.Utility.Global.MillionFormat(nAmount).Split('.')[0];
                    cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = "";
                    cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

                }


                cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                nRowIndex = nRowIndex + 1;
            }
            #endregion

            #region সর্বমোট বেতন
            nAmount = oTempEmployeeSalaryDetails.Where(x => x.SalaryHeadType == ESimSol.BusinessObjects.EnumSalaryHeadType.Basic).Sum(x => x.Amount) +
                           oTempEmployeeSalaryDetails.Where(x => x.SalaryHeadType == ESimSol.BusinessObjects.EnumSalaryHeadType.Addition).Sum(x => x.Amount) -
                           oTempEmployeeSalaryDetails.Where(x => x.SalaryHeadType == ESimSol.BusinessObjects.EnumSalaryHeadType.Deduction).Sum(x => x.Amount);

            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "me©‡gvU ‡eZb";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = Math.Round(nAmount);
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Bottom/Left/Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Left Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region ওভারটাইম হার
            if (bIsOT == true )
            {
                sheet.Row(nRowIndex).Height = nRowHeight;
                cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
                cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "IfviUvBg nvi";
                cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
                cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

               
                cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = oEmployeeSalary.OTRatePerHour;
                cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, (nColumnIndex + 4), nRowIndex, (nColumnIndex + 5)]; cell.Merge = true; cell.Value = "";
                cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; 
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                nRowIndex = nRowIndex + 1;
            }
            #endregion

            #region ওভারটাইম ঘন্টা
            if (bIsOT == true)
            {
                sheet.Row(nRowIndex).Height = nRowHeight;
                cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
                cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "IfviUvBg N›Uv";
                cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
                cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                

                cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = Convert.ToInt32(oEmployeeSalary.OTHour);
                cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = "IfviUvBg PvR©";
                cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = Math.Round(oEmployeeSalary.OTHour * oEmployeeSalary.OTRatePerHour);
                cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                nRowIndex = nRowIndex + 1;
            }
            #endregion


            #region Blank with Bottom/Left/Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Left Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region প্রদেয় বেতন
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "c«‡`q ‡eZb";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
            if (bIsOT == true && (oEmployeeSalary.OTHour * oEmployeeSalary.OTHour) > 0)
            {
                nAmount = nAmount + (oEmployeeSalary.OTHour * oEmployeeSalary.OTRatePerHour);
            }
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = Math.Round(nAmount);
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Left Right Border
            sheet.Row(nRowIndex).Height = 20;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Under Line
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 1), nRowIndex, (nColumnIndex + 3)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "_________________";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


            sheet.Cells[nRowIndex, (nColumnIndex + 4), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = "_________________";
            cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region স্বাক্ষর
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 1), nRowIndex, (nColumnIndex + 3)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "k«wg‡Ki ¯^v¶i";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


            sheet.Cells[nRowIndex, (nColumnIndex + 4), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = "KZ©…c‡¶i ¯^v¶i";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Left Right Border
            sheet.Row(nRowIndex).Height = 5;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Bottom/Left/Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Name = sFontName; cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            return nRowIndex;
        }
        #endregion

    }
}