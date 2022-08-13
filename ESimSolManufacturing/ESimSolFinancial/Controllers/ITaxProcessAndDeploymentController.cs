using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;

using ESimSol.Reports;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ReportManagement;
using System.Reflection;

namespace ESimSolFinancial.Controllers
{
    public class ITaxProcessAndDeploymentController : Controller
    {
        #region Declaration
        ITaxAdvancePayment _oITaxAdvancePayment;
        List<ITaxAdvancePayment> _oITaxAdvancePayments;
        ITaxRebatePayment _oITaxRebatePayment;
        List<ITaxRebatePayment> _oITaxRebatePayments;
        List<ITaxLedger> _oITaxLedgers;
        ITaxLedger _oITaxLedger;
        ITaxRateScheme _oITaxRateScheme;
        static List<ITaxLedger> oITaxLedgersForError = new List<ITaxLedger>();

        #endregion

        #region Income Tax Advance Payment
        public ActionResult View_ITaxAdvancePayments(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oITaxAdvancePayments = new List<ITaxAdvancePayment>();
            string sSQL = "SELECT * FROM View_ITaxAdvancePayment ";
            _oITaxAdvancePayments = ITaxAdvancePayment.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSQL = "SELECT * FROM ITaxAssessmentYear";
            List<ITaxAssessmentYear> oITaxAssessmentYears = new List<ITaxAssessmentYear>();
            oITaxAssessmentYears = ITaxAssessmentYear.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID).OrderByDescending(x => x.ITaxAssessmentYearID).ToList();
            ViewBag.ITaxAssessmentYears_Active = oITaxAssessmentYears.Where(x=>x.IsActive);
            ViewBag.ITaxAssessmentYears = oITaxAssessmentYears;
            return View(_oITaxAdvancePayments);
        }

        [HttpPost]
        public JsonResult ITaxAdvancePayment_IU(ITaxAdvancePayment oITaxAdvancePayment)
        {
            try
            {
                if (oITaxAdvancePayment.ITaxAdvancePaymentID > 0)
                {
                    oITaxAdvancePayment = oITaxAdvancePayment.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oITaxAdvancePayment = oITaxAdvancePayment.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oITaxAdvancePayment = new ITaxAdvancePayment();
                oITaxAdvancePayment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxAdvancePayment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ITaxAdvancePayment_Delete(ITaxAdvancePayment oITaxAdvancePayment)
        {
            try
            {
                if (oITaxAdvancePayment.ITaxAdvancePaymentID <= 0)
                    throw new Exception("Please select a valid item from list.");
                oITaxAdvancePayment = oITaxAdvancePayment.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oITaxAdvancePayment = new ITaxAdvancePayment();
                oITaxAdvancePayment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxAdvancePayment.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetITaxAdvancePayment(ITaxAdvancePayment oITaxAdvancePayment)
        {
            try
            {
                oITaxAdvancePayment = ITaxAdvancePayment.Get(oITaxAdvancePayment.ITaxAdvancePaymentID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oITaxAdvancePayment = new ITaxAdvancePayment();
                oITaxAdvancePayment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxAdvancePayment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ITaxAdvancePayment_Search(ITaxAdvancePayment oITaxAdvancePayment)
        {
            _oITaxAdvancePayments = new List<ITaxAdvancePayment>();
            try
            {
                string sSql = "SELECT * FROM View_ITaxAdvancePayment  WHERE ITaxAdvancePaymentID>0 ";
                if (oITaxAdvancePayment.EmployeeID>0)
                {
                    sSql += " AND  EmployeeID=" + oITaxAdvancePayment.EmployeeID;
                }
                if (oITaxAdvancePayment.ITaxAssessmentYearID > 0)
                {
                    sSql += " AND  ITaxAssessmentYearID=" + oITaxAdvancePayment.ITaxAssessmentYearID;
                }
                _oITaxAdvancePayments = ITaxAdvancePayment.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oITaxAdvancePayments = new List<ITaxAdvancePayment>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oITaxAdvancePayments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Income Tax Rebate Payments

        public ActionResult View_ITaxRebatePayments(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oITaxRebatePayments = new List<ITaxRebatePayment>();
            string sSQL = "SELECT * FROM View_ITaxRebatePayment ";
            _oITaxRebatePayments = ITaxRebatePayment.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);


            sSQL = "SELECT * FROM ITaxAssessmentYear WHERE IsActive =1";
            List<ITaxAssessmentYear> oITaxAssessmentYears = new List<ITaxAssessmentYear>();
            oITaxAssessmentYears = ITaxAssessmentYear.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID).OrderByDescending(x => x.ITaxAssessmentYearID).ToList();
            ViewBag.ITaxAssessmentYears = oITaxAssessmentYears;

            List<ITaxRebateItem> oITaxRebateItems = new List<ITaxRebateItem>();
            sSQL = "SELECT *  FROM ITaxRebateItem";
            oITaxRebateItems = ITaxRebateItem.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.ITaxRebateItems = oITaxRebateItems;
          
            return View(_oITaxRebatePayments);
        }

        [HttpPost]
        public JsonResult ITaxRebatePayment_IU(ITaxRebatePayment oITaxRebatePayment)
        {
            try
            {
                if (oITaxRebatePayment.ITaxRebatePaymentID > 0)
                {
                    oITaxRebatePayment = oITaxRebatePayment.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oITaxRebatePayment = oITaxRebatePayment.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oITaxRebatePayment = new ITaxRebatePayment();
                oITaxRebatePayment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxRebatePayment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ITaxRebatePayment_Delete(ITaxRebatePayment oITaxRebatePayment)
        {
            try
            {
                if (oITaxRebatePayment.ITaxRebatePaymentID <= 0)
                    throw new Exception("Please select a valid item from list.");
                oITaxRebatePayment = oITaxRebatePayment.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oITaxRebatePayment = new ITaxRebatePayment();
                oITaxRebatePayment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxRebatePayment.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetITaxRebatePayment(ITaxRebatePayment oITaxRebatePayment)
        {
            try
            {
                oITaxRebatePayment = ITaxRebatePayment.Get(oITaxRebatePayment.ITaxRebatePaymentID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oITaxRebatePayment = new ITaxRebatePayment();
                oITaxRebatePayment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxRebatePayment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ITaxRebatePayment_Search(ITaxRebatePayment oITaxRebatePayment)
        {
            _oITaxRebatePayments = new List<ITaxRebatePayment>();
            try
            {
                string sSql = "SELECT * FROM View_ITaxRebatePayment  Where EmployeeNameCode Like '%" + oITaxRebatePayment.EmployeeNameCode + "%'";
                _oITaxRebatePayments = ITaxRebatePayment.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oITaxRebatePayments = new List<ITaxRebatePayment>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oITaxRebatePayments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Process
        [HttpPost]
        public JsonResult ITaxProcess(int nEmployeeID, int nITaxAssessmentYearID, bool IsAllEmployee, bool IsConsiderMaxRebate, double MinGross)
        {
            _oITaxLedgers = new List<ITaxLedger>();
            _oITaxLedger = new ITaxLedger();
            try
            {
                _oITaxLedgers = _oITaxLedger.ITaxProcess(nEmployeeID, nITaxAssessmentYearID, IsAllEmployee,IsConsiderMaxRebate, MinGross, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oITaxLedgers.Count <= 0)
                {
                    throw new Exception("Data not found!");
                }
            }
            catch (Exception ex)
            {
                _oITaxLedger = new ITaxLedger();
                _oITaxLedgers = new List<ITaxLedger>();
                _oITaxLedger.ErrorMessage = ex.Message;
                _oITaxLedgers.Add(_oITaxLedger);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oITaxLedgers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Process


        #region ReProcess
        [HttpPost]
        public JsonResult ITaxReprocess(int nEmployeeID, int nITaxAssessmentYearID, bool IsAllEmployee, bool IsConsiderMaxRebate, double MinGross, DateTime dtDate)
        {
            _oITaxLedgers = new List<ITaxLedger>();
            _oITaxLedger = new ITaxLedger();
            try
            {
                _oITaxLedgers = _oITaxLedger.ITaxReprocess(nEmployeeID, nITaxAssessmentYearID, IsAllEmployee, IsConsiderMaxRebate, MinGross, dtDate, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                if (_oITaxLedgers.Count > 0 && (_oITaxLedgers[0].ErrorMessage != "" || _oITaxLedgers[0].ErrorMessage != null))
                {
                    oITaxLedgersForError = _oITaxLedgers;
                }
            }
            catch (Exception ex)
            {
                _oITaxLedger = new ITaxLedger();
                _oITaxLedgers = new List<ITaxLedger>();
                _oITaxLedger.ErrorMessage = ex.Message;
                _oITaxLedgers.Add(_oITaxLedger);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oITaxLedgers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public void printErrorList(double ts)
        {
            if (oITaxLedgersForError.Count > 0)
            {
                string sEmpID = "";
                sEmpID = string.Join(",", oITaxLedgersForError.Select(x => x.EmployeeID));
                List<Employee> oEmps = new List<Employee>();
                oEmps = Employee.Gets("SELECT * FROM Employee WHERE EmployeeID IN(" + sEmpID + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID);

                ExcelRange cell;
                OfficeOpenXml.Style.Border border;
                ExcelFill fill;
                int colIndex = 1;
                int rowIndex = 2;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Error List");
                    sheet.Name = "Error List";

                    int n = 1;
                    sheet.Column(n++).Width = 13;
                    sheet.Column(n++).Width = 50;

                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EmployeeCode"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Error"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    rowIndex += 1;

                    foreach (ITaxLedger oItem in oITaxLedgersForError)
                    {
                        colIndex = 1;
                        string empCode = oEmps.Where(x => x.EmployeeID == oItem.EmployeeID).ToList().First().Code;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = empCode; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ErrorMessage; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ErrorList.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
            }
            oITaxLedgersForError = new List<ITaxLedger>();
        }
        #endregion Process

        #region ITax Ledger
        public ActionResult View_ITaxLedgers(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oITaxLedgers = new List<ITaxLedger>();
            _oITaxLedger = new ITaxLedger();
            _oITaxLedgers.Add(_oITaxLedger);

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            string sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSql = sSql + ")";

            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));

            string sAssesmentYearSql = "SELECT * FROM ITaxAssessmentYear WHERE IsActive =1";
            ViewBag.ITaxAssessmentYear = ITaxAssessmentYear.Get(sAssesmentYearSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            _oITaxLedgers[0].ITaxAssessmentYears = ITaxAssessmentYear.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oITaxLedgers);
        }

        public ActionResult View_IncomeTaxProcess(int nId, double ts)
        {
            _oITaxLedger = new ITaxLedger();
            string sAssesmentYearSql = "SELECT * FROM ITaxAssessmentYear WHERE IsActive =1";
            _oITaxLedger.ITaxAssessmentYear = ITaxAssessmentYear.Get(sAssesmentYearSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return PartialView(_oITaxLedger);
        }
        
        [HttpPost]
        public JsonResult ITaxLedger_Search(string sEmployeeIDs, int nAssesmentYearID, int nStatus)
        {
            _oITaxLedgers = new List<ITaxLedger>();
            try
            {
                string sSql = "SELECT * FROM View_ITaxLedger WHERE ITaxLedgerID<>0";
                if (sEmployeeIDs != "")
                {
                    sSql += " AND EmployeeID IN(" + sEmployeeIDs + ")";
                }
                if (nAssesmentYearID > 0)
                {
                    sSql += " AND ITaxRateSchemeID IN(SELECT ITaxRateSchemeID  FROM ITaxRateScheme WHERE ITaxAssessmentYearID=" + nAssesmentYearID + ")";
                }
                if (nStatus == 1)
                {
                    sSql += " AND IsActive = 1";
                }
                if (nStatus == 0)
                {
                    sSql += " AND IsActive = 0";
                }

                _oITaxLedgers = ITaxLedger.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oITaxLedgers.Count <= 0)
                {
                    throw new Exception("Data not found!");
                }
            }
            catch (Exception ex)
            {
                _oITaxLedger = new ITaxLedger();
                _oITaxLedgers = new List<ITaxLedger>();
                _oITaxLedger.ErrorMessage = ex.Message;
                _oITaxLedgers.Add(_oITaxLedger);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oITaxLedgers);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult ITaxLedger_Delete(string sITaxLedgerIDs)
        {
            _oITaxLedger = new ITaxLedger();
            try
            {
                _oITaxLedger = ITaxLedger.ITaxLedger_Delete(sITaxLedgerIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oITaxLedger = new ITaxLedger();
                _oITaxLedger.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oITaxLedger.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion ITaxLedger

        #region ITaxLedgerSalaryHead search
        [HttpPost]
        public JsonResult ITaxLedgerSalaryHead_Search(ITaxLedgerSalaryHead oITaxLedgerSalaryHead)
        {
            List<ITaxLedgerSalaryHead> oITaxLedgerSalaryHeads = new List<ITaxLedgerSalaryHead>();
            try
            {
                string sSql = "SELECT * FROM View_ITaxLedgerSalaryHead WHERE ITaxLedgerID=" + oITaxLedgerSalaryHead.ITaxLedgerID;

                oITaxLedgerSalaryHeads = ITaxLedgerSalaryHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oITaxLedgerSalaryHeads.Count <= 0)
                {
                    throw new Exception("Data not found!");
                }
            }
            catch (Exception ex)
            {
                oITaxLedgerSalaryHead = new ITaxLedgerSalaryHead();
                oITaxLedgerSalaryHeads = new List<ITaxLedgerSalaryHead>();
                oITaxLedgerSalaryHead.ErrorMessage = ex.Message;
                oITaxLedgerSalaryHeads.Add(oITaxLedgerSalaryHead);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxLedgerSalaryHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }

        #endregion ITaxLedgerSalaryHead search

        #region Print Salary Certificate
        public ActionResult PrintSalaryCertificate(int nITaxLedgerID, int nType, double ts)
        {
            _oITaxLedger = new ITaxLedger();
            string sSql = "";
            sSql = "SELECT * FROM View_ITaxLedger WHERE ITaxLedgerID=" + nITaxLedgerID;
            _oITaxLedger = ITaxLedger.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            sSql = "";
            sSql = "SELECT * FROM View_ITaxLedgerSalaryHead WHERE ITaxLedgerID=" + nITaxLedgerID;
            _oITaxLedger.ITaxLedgerSalaryHeads = ITaxLedgerSalaryHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "";
            sSql = "SELECT TOP(1)* FROM ITaxAssessmentYear WHERE ITaxAssessmentYearID IN(SELECT ITaxAssessmentYearID FROM ITaxRateScheme WHERE ITaxRateSchemeID IN(SELECT ITaxRateSchemeID FROM ITaxLedger WHERE ITaxLedgerID=" + nITaxLedgerID + "))";
            _oITaxLedger.ITaxAssessmentYear = ITaxAssessmentYear.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<PFScheme> oPFSchemes = new List<PFScheme>();
            sSql = "";
            sSql = "SELECT TOP(1)* FROM VIEW_PFScheme WHERE IsActive=1 AND ApproveBy>0";
            oPFSchemes = PFScheme.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            if (oPFSchemes.Count > 0) {
                sSql = "";
                sSql = "SELECT * FROM View_EmployeeSalaryDetail WHERE SalaryHeadID=" + oPFSchemes[0].RecommandedSalaryHeadID + " AND  EmployeeSalaryID IN(SELECT EmployeeSalaryID FROM EmployeeSalary WHERE (StartDate BETWEEN '" + _oITaxLedger.ITaxAssessmentYear.StartDateInString + "' AND '" + _oITaxLedger.ITaxAssessmentYear.EndDateInString + "') AND (EndDate BETWEEN '" + _oITaxLedger.ITaxAssessmentYear.StartDateInString + "' AND '" + _oITaxLedger.ITaxAssessmentYear.EndDateInString + "'))";
                _oITaxLedger.EmployeeSalaryDetails = EmployeeSalaryDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }

            List<EmployeeBankAccount> oEmployeeBankAccounts = new List<EmployeeBankAccount>();
            sSql = "";
            sSql = "SELECT * FROM VIEW_EmployeeBankAccount  WHERE EmployeeID =" + _oITaxLedger.EmployeeID;
            _oITaxLedger.EmployeeBankAccounts = EmployeeBankAccount.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oITaxLedger.Company = oCompanys.First();
            _oITaxLedger.Company.CompanyLogo = GetCompanyLogo(_oITaxLedger.Company);

            _oITaxLedger.ErrorMessage = nType.ToString();

            rptSalaryCertificate_Tax oReport = new rptSalaryCertificate_Tax();
            byte[] abytes = oReport.PrepareReport(_oITaxLedger);
            return File(abytes, "application/pdf");
        }
        #endregion Print Salary Certificate
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

        #region Print Tax Detail
        public ActionResult PrintTaxDetail(int nITaxLedgerID, double ts)
        {
            _oITaxLedger = new ITaxLedger();
            string sSql = "";
            sSql = "SELECT * FROM View_ITaxLedger WHERE ITaxLedgerID=" + nITaxLedgerID;
            _oITaxLedger = ITaxLedger.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            sSql = "";
            sSql = "SELECT * FROM View_ITaxLedgerSalaryHead WHERE ITaxLedgerID=" + nITaxLedgerID;
            _oITaxLedger.ITaxLedgerSalaryHeads = ITaxLedgerSalaryHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "";
            sSql = "SELECT TOP(1)* FROM ITaxAssessmentYear WHERE ITaxAssessmentYearID IN(SELECT ITaxAssessmentYearID FROM ITaxRateScheme WHERE ITaxRateSchemeID IN(SELECT ITaxRateSchemeID FROM ITaxLedger WHERE ITaxLedgerID=" + nITaxLedgerID + "))";
            _oITaxLedger.ITaxAssessmentYear = ITaxAssessmentYear.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oITaxLedger.Company = oCompanys.First();
            _oITaxLedger.Company.CompanyLogo = GetCompanyLogo(_oITaxLedger.Company);

            rptTaxDetail oReport = new rptTaxDetail();
            byte[] abytes = oReport.PrepareReport(_oITaxLedger);
            return File(abytes, "application/pdf");
        }
        #endregion Print Tax Detail

        #region Print Tax Detail
        public ActionResult PrintTaxDetail_F1(int nITaxLedgerID, double ts)
        {
            _oITaxLedger = new ITaxLedger();

            string sSql = "";
            sSql = "SELECT * FROM View_ITaxLedger WHERE ITaxLedgerID=" + nITaxLedgerID;
            _oITaxLedger = ITaxLedger.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            sSql = "";


            sSql = "SELECT * FROM ITaxRateScheme WHERE ITaxRateSchemeID IN (SELECT ITaxRateSchemeID FROM ITaxLedger WHERE ITaxLedgerID=" + nITaxLedgerID + ")";
            _oITaxLedger.ITaxRateScheme = ITaxRateScheme.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            sSql = "";


            sSql = "SELECT * FROM View_ITaxLedgerSalaryHead WHERE SalaryHeadID IN (SELECT SalaryHeadID FROM VIEW_EmployeeSalaryStructureDetail) AND ITaxLedgerID=" + nITaxLedgerID;
            _oITaxLedger.ITaxLedgerSalaryHeads = ITaxLedgerSalaryHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "";
            sSql = "SELECT TOP(1)* FROM ITaxAssessmentYear WHERE ITaxAssessmentYearID IN(SELECT ITaxAssessmentYearID FROM ITaxRateScheme WHERE ITaxRateSchemeID IN(SELECT ITaxRateSchemeID FROM ITaxLedger WHERE ITaxLedgerID=" + nITaxLedgerID + "))";
            _oITaxLedger.ITaxAssessmentYear = ITaxAssessmentYear.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "";
            sSql = "SELECT * FROM View_ITaxHeadConfiguration ";
            _oITaxLedger.ITaxHeadConfigurations = ITaxHeadConfiguration.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "";
            sSql = "SELECT * FROM ITaxRebateScheme WHERE ITaxRateSchemeID IN(SELECT ITaxRateSchemeID FROM ITaxLedger WHERE ITaxLedgerID="+nITaxLedgerID+")";
            _oITaxLedger.ITaxRebateSchemes = ITaxRebateScheme.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "";
            sSql = "SELECT * FROM View_EmployeeSalaryStructuredetail WHERE ESSID IN(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID IN(SELECT  EmployeeID  FROM ITaxLedger WHERE ITaxLedgerID="+nITaxLedgerID + ")) ORDER BY SalaryHeadID" ;
            _oITaxLedger.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oITaxLedger.Company = oCompanys.First();
            _oITaxLedger.Company.CompanyLogo = GetCompanyLogo(_oITaxLedger.Company);

            rptTaxDetail_F1 oReport = new rptTaxDetail_F1();
            byte[] abytes = oReport.PrepareReport(_oITaxLedger);
            return File(abytes, "application/pdf");
        }
        #endregion Print Tax Detail

        #region ITax Ledger List In XL
        public void PrintITaxLedgerListInXL(string sITaxLedgerIDs, int nAssesmentYearID, int nStatus)
        {
            List<ITaxLedger> ITaxLedgers = new List<ITaxLedger>();

            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            string sSql = "SELECT * FROM View_ITaxLedger WHERE ITaxLedgerID<>0";
            if (!string.IsNullOrEmpty(sITaxLedgerIDs))
            {
                sSql += " AND ITaxLedgerID IN(" + sITaxLedgerIDs + ")";
            }
            if (nAssesmentYearID > 0)
            {
                sSql += " AND ITaxRateSchemeID IN(SELECT ITaxRateSchemeID  FROM ITaxRateScheme WHERE ITaxAssessmentYearID=" + nAssesmentYearID + ")";
            }
            if (nStatus == 1)
            {
                sSql += " AND IsActive = 1";
            }
            if (nStatus == 0)
            {
                sSql += " AND IsActive = 0";
            }
            ITaxLedgers = ITaxLedger.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("ITax Ledger List");
                sheet.Name = "ITax Ledger List";

                sheet.Column(2).Width = 6; //SL
                sheet.Column(3).Width = 30; //EmployeeNameCode
                sheet.Column(4).Width = 20; //AssessmentYear
                sheet.Column(5).Width = 20; //TaxableAmount
                sheet.Column(6).Width = 20; //TaxAmount
                sheet.Column(7).Width = 20; //RebateAmount
                sheet.Column(8).Width = 25; //AdvancePaidAmount
                sheet.Column(9).Width = 25; //PaidByPreviousLedger
                sheet.Column(10).Width = 20; //TotalTax
                sheet.Column(11).Width = 25; //InstallmentAmount
                
                nMaxColumn = 11;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "ITax Ledger List"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Assessment Year"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Taxable Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Tax Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Rebate Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Advance Paid Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Paid By Previous Ledger"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Tax"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Installment Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowIndex++;
                #endregion

                #region Table Body

                int nSL = 0;
                double nTotalTaxableAmount = 0;
                double nTotalTaxAmount = 0;
                double nTotalRebateAmount = 0;
                double nTotalAdvancePaidAmount = 0;
                double nTotalPaidByPreviousLedger = 0;
                double nTotalTax = 0;
                double nTotalInstallmentAmount = 0;

                foreach (ITaxLedger oItem in ITaxLedgers)
                {
                    nSL++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeNameCode; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.AssessmentYear; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem.TaxableAmount); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem.TaxAmount); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem.RebateAmount); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem.AdvancePaidAmount); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem.PaidByPreviousLedger); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem.TotalTax); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem.InstallmentAmount); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StatusInStr; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    rowIndex++;

                    nTotalTaxableAmount+=oItem.TaxableAmount;
                    nTotalTaxAmount+=oItem.TaxAmount;
                    nTotalRebateAmount+=oItem.RebateAmount;
                    nTotalAdvancePaidAmount+=oItem.AdvancePaidAmount;
                    nTotalPaidByPreviousLedger+=oItem.PaidByPreviousLedger;
                    nTotalTax+=oItem.TotalTax;
                    nTotalInstallmentAmount+=oItem.InstallmentAmount;

                }
                #endregion

                #region Total

                sheet.Cells[rowIndex, 2, rowIndex, 4].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Total:"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                colIndex = 5;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalTaxableAmount); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalTaxAmount); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalRebateAmount); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalAdvancePaidAmount); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalPaidByPreviousLedger); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalTax); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalInstallmentAmount); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //signature

                rowIndex = rowIndex + 8;
                colIndex = 0;

                cell = sheet.Cells[rowIndex, colIndex+1, rowIndex, colIndex + 3]; cell.Merge = true; cell.Value = "_________________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, colIndex+4, rowIndex, colIndex + 5]; cell.Merge = true; cell.Value = "_______________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, colIndex+7, rowIndex, colIndex + 8]; cell.Merge = true; cell.Value = "__________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, colIndex+10, rowIndex,  colIndex + 11]; cell.Merge = true; cell.Value = "_____________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                rowIndex = rowIndex + 1;
                colIndex = 0;

                cell = sheet.Cells[rowIndex, colIndex+1, rowIndex, colIndex + 3]; cell.Merge = true; cell.Value = "Prepared By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, colIndex+4, rowIndex, colIndex + 5]; cell.Merge = true; cell.Value = "Checked By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, colIndex+7, rowIndex, colIndex + 8]; cell.Merge = true; cell.Value = "Reviewed By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, colIndex+10, rowIndex, colIndex + 11]; cell.Merge = true; cell.Value = "Approved By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ITaxLedgerList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        #endregion ITax Ledger List In XL


        public void PrintSalaryCertificateF3Excel(int nEmployeeID, int AssesmentYearID, int Status, double ts)
        {


            ITaxAssessmentYear oITaxAssessmentYear = new ITaxAssessmentYear();
            oITaxAssessmentYear = ITaxAssessmentYear.Get("SELECT * FROM ITaxAssessmentYear WHERE ITaxAssessmentYearID=" + AssesmentYearID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            DateTime sDate = oITaxAssessmentYear.StartDate;
            DateTime eDate = oITaxAssessmentYear.EndDate;

            Employee oEmployee = new Employee();
            oEmployee = Employee.Get("SELECT * FROM View_Employee WHERE EmployeeID=" + nEmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            int TaxPayerTypeID = 0;
            if (oEmployee.Gender == EnumTaxPayerType.Male.ToString())
            {
                TaxPayerTypeID = (int)EnumTaxPayerType.Male;
            }
            if (oEmployee.Gender == EnumTaxPayerType.Female.ToString())
            {
                TaxPayerTypeID = (int)EnumTaxPayerType.Female;
            }

            ITaxRateScheme oITaxRateScheme = new ITaxRateScheme();
            oITaxRateScheme = ITaxRateScheme.Get("SELECT * FROM ITaxRateScheme WHERE ITaxAssessmentYearID=" + AssesmentYearID + " AND TaxPayerType=" + TaxPayerTypeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<PFScheme> oPFSchemes = new List<PFScheme>();
            oPFSchemes = PFScheme.Gets("SELECT * FROM PFScheme", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            string sSql = "SELECT * FROM View_EmployeeSalaryDetail WHERE EmployeeSalaryID IN (SELECT EmployeeSalaryID FROM  EmployeeSalary WHERE EmployeeID=" + nEmployeeID + " AND EndDate BETWEEN '" + oITaxAssessmentYear.StartDateInString + "' AND '" + oITaxAssessmentYear.EndDateInString + "')";
            List<EmployeeSalaryDetail> oEmployeeSalaryDetails = new List<EmployeeSalaryDetail>();
            oEmployeeSalaryDetails = EmployeeSalaryDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            //var vEmployeeSalaryDetails = oEmployeeSalaryDetails.GroupBy(x=>x.SalaryHeadType).ToList();

            var vAddEmployeeSalaryDetails = oEmployeeSalaryDetails.Where(x => x.SalaryHeadType == 1 || x.SalaryHeadType == 1).ToList();
            var vDedEmployeeSalaryDetails = oEmployeeSalaryDetails.Where(x => x.SalaryHeadType == 3).ToList();

            var vAddEmployeeSalaryDetailsLinq = vAddEmployeeSalaryDetails
                .GroupBy(l => l.SalaryHeadName)
                .Select(cl => new EmployeeSalaryDetail
                {
                    SalaryHeadName = cl.First().SalaryHeadName,
                    Amount = cl.Sum(c => c.Amount)
                }).ToList();

            var vDedEmployeeSalaryDetailsLinq = vDedEmployeeSalaryDetails
                .GroupBy(l => l.SalaryHeadName)
                .Select(cl => new EmployeeSalaryDetail
                {
                    SalaryHeadName = cl.First().SalaryHeadName,
                    Amount = cl.Sum(c => c.Amount)
                }).ToList();


            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            int nMaxColumn = 0;
            int nStartCol = 1, nEndCol = 4;
            int colIndex = 1;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            string makeFormat = "";


            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Salary Certificate");
                sheet.Name = "Salary Certificate";

                int n = 1;
                sheet.Column(n++).Width = 20;
                sheet.Column(n++).Width = 20;
                sheet.Column(n++).Width = 20;
                sheet.Column(n++).Width = 20;

                cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nEndCol]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nEndCol]; cell.Merge = true; cell.Value = oCompany.Address;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;

                cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nEndCol]; cell.Merge = true; cell.Value = "REF.: "; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 1;
                cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nEndCol]; cell.Merge = true; cell.Value = "Date: " + DateTime.Now.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 2;

                cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nEndCol]; cell.Merge = true; cell.Value = "SALARY CERTIFICAT"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rowIndex = rowIndex + 2;

                cell = sheet.Cells[rowIndex, nStartCol, rowIndex + 3, nEndCol]; cell.Merge = true;
                cell.Value = "This is to certify that " + oEmployee.Name + " Code No. " + oEmployee.Code + " is working in our company as " + oEmployee.DesignationName + " on " + oEmployee.DepartmentName + " Department. He has drawn salary for the financial year " + oITaxAssessmentYear.AssessmentYear + " is as under:"; cell.Style.WrapText = true;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rowIndex = rowIndex + 7;

                cell = sheet.Cells[rowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "PARTICULARS"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell = sheet.Cells[rowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell = sheet.Cells[rowIndex, 4]; cell.Value = "AMOUNT IN BDT."; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rowIndex = rowIndex + 2;

                cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nEndCol]; cell.Value = "Income"; cell.Merge = true; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 1;

                int countAdd = (vAddEmployeeSalaryDetailsLinq.Count > 0 ? vAddEmployeeSalaryDetailsLinq.Count : 0);

                cell = sheet.Cells[rowIndex, 1, rowIndex + countAdd, 1]; cell.Value = ""; cell.Merge = true; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, 2, rowIndex + countAdd, 2]; cell.Value = ""; cell.Merge = true; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                double addTotal = 0.0;
                double dedTotal = 0.0;
                foreach (EmployeeSalaryDetail oItem in vAddEmployeeSalaryDetailsLinq)
                {

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.SalaryHeadName;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.Amount;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    addTotal += oItem.Amount;

                    rowIndex = rowIndex + 1;
                }

                rowIndex = rowIndex + 1;
                cell = sheet.Cells[rowIndex, 1]; cell.Value = "Gross Total"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = addTotal; cell.Style.Font.Bold = true; border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nEndCol]; cell.Value = "Less Deductions"; cell.Merge = true; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 1;

                int countDed = (vDedEmployeeSalaryDetailsLinq.Count > 0 ? vDedEmployeeSalaryDetailsLinq.Count : 0);

                cell = sheet.Cells[rowIndex, 1, rowIndex + countDed, 1]; cell.Value = ""; cell.Merge = true; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, 2, rowIndex + countDed, 2]; cell.Value = ""; cell.Merge = true; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                foreach (EmployeeSalaryDetail oItem in vDedEmployeeSalaryDetailsLinq)
                {

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.SalaryHeadName;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.Amount;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    dedTotal += oItem.Amount;

                    rowIndex = rowIndex + 1;
                }

                rowIndex = rowIndex + 1;
                cell = sheet.Cells[rowIndex, 1]; cell.Value = "Net Salary Pay"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = addTotal - dedTotal; cell.Style.Font.Bold = true; border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                rowIndex = rowIndex + 2;

                cell = sheet.Cells[rowIndex, 1, rowIndex, 4]; cell.Value = "This certificate is issued on the specific request of the employee"; cell.Merge = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                rowIndex = rowIndex + 3;
                cell = sheet.Cells[rowIndex, 1, rowIndex, 4]; cell.Value = "For " + oCompany.Name; cell.Merge = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 2;

                cell = sheet.Cells[1, 1, rowIndex, nEndCol];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SalaryCertificate.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public ActionResult PDFTaxAssessment(int nEmployeeID, int AssesmentYearID, int Status, int nITaxLedgerID, double ts)
        {
            _oITaxLedger = new ITaxLedger();
            string sSql = "";


            sSql = "SELECT * FROM View_ITaxLedger WHERE ITaxLedgerID=" + nITaxLedgerID;
            _oITaxLedger = ITaxLedger.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);


            sSql = "SELECT * FROM Employee WHERE EmployeeID=" + nEmployeeID;
            _oITaxLedger.Employee = Employee.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "";
            sSql = "SELECT * FROM View_ITaxLedgerSalaryHead WHERE ITaxLedgerID=" + nITaxLedgerID;
            _oITaxLedger.ITaxLedgerSalaryHeads = ITaxLedgerSalaryHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "";
            sSql = "SELECT TOP(1)* FROM ITaxAssessmentYear WHERE ITaxAssessmentYearID IN(SELECT ITaxAssessmentYearID FROM ITaxRateScheme WHERE ITaxRateSchemeID IN(SELECT ITaxRateSchemeID FROM ITaxLedger WHERE ITaxLedgerID=" + nITaxLedgerID + "))";
            _oITaxLedger.ITaxAssessmentYear = ITaxAssessmentYear.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "SELECT * FROM ITaxRateScheme WHERE ITaxRateSchemeID=" + _oITaxLedger.ITaxRateSchemeID;
            _oITaxLedger.ITaxRateScheme = ITaxRateScheme.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "SELECT * FROM EmployeeTINInformation WHERE EmployeeID=" + nEmployeeID;
            _oITaxLedger.EmployeeTINInformation = EmployeeTINInformation.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "select * from View_EmployeeSalaryStructureDetail WHERE ESSID IN(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=" + nEmployeeID + ")";
            _oITaxLedger.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "SELECT * FROM View_ITaxHeadConfiguration ";
            _oITaxLedger.ITaxHeadConfigurations = ITaxHeadConfiguration.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "SELECT * FROM ITaxRateSlab";
            _oITaxLedger.ITaxRateSlabs = ITaxRateSlab.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "SELECT * FROM ITaxRebateItem";
            _oITaxLedger.ITaxRebateItems = ITaxRebateItem.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "SELECT * FROM ITaxRebatePayment WHERE EmployeeID=" + nEmployeeID;
            _oITaxLedger.ITaxRebatePayments = ITaxRebatePayment.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);


            //select * from View_EmployeeSalaryDetail where Employeesalaryid in(select EmployeeSalaryID from EmployeeSalary where EmployeeID = 3 and EndDate between '01 Jul 2016' and '30 Jun 2017')

            sSql = "select * from EmployeeSalary where EmployeeID=" + nEmployeeID + " AND (SalaryDate BETWEEN '" + _oITaxLedger.ITaxAssessmentYear.StartDateInString + "' AND '" + _oITaxLedger.ITaxAssessmentYear.EndDateInString + "')";
            _oITaxLedger.EmployeeSalarys = EmployeeSalary.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "SELECT * FROM View_EmployeeSalaryDetail WHERE EmployeeSalaryID IN(select EmployeeSalaryID from EmployeeSalary WHERE EmployeeID = "+nEmployeeID+" AND (EndDate BETWEEN '" + _oITaxLedger.ITaxAssessmentYear.StartDateInString + "' AND '" + _oITaxLedger.ITaxAssessmentYear.EndDateInString + "'))";
            _oITaxLedger.EmployeeSalaryDetails = EmployeeSalaryDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "SELECT * FROM SalaryHead";
            _oITaxLedger.SalaryHeads = SalaryHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oITaxLedger.Company = oCompanys.First();
            _oITaxLedger.Company.CompanyLogo = GetCompanyLogo(_oITaxLedger.Company);

            rptTaxAssessment oReport = new rptTaxAssessment();
            byte[] abytes = oReport.PrepareReport(_oITaxLedger);
            return File(abytes, "application/pdf");
        }

    }
}
