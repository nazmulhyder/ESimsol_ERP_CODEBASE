using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using System.Net.Mail;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing.Imaging;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class EmployeeAdvanceSalaryController : Controller
    {
        #region Declaration
        bool _bIsCompliance = false;
        EmployeeAdvanceSalary _oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
        List<EmployeeAdvanceSalary> _oEmployeeAdvanceSalarys = new List<EmployeeAdvanceSalary>();


        EmployeeAdvanceSalaryProcess _oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
        List<EmployeeAdvanceSalaryProcess> _oEmployeeAdvanceSalaryProcesss = new List<EmployeeAdvanceSalaryProcess>();
        #endregion

        #region Actions
        public ActionResult View_EmployeeAdvanceSalary(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.AccountsBookSetup).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oEmployeeAdvanceSalarys = new List<EmployeeAdvanceSalary>();
            _oEmployeeAdvanceSalaryProcesss = new List<EmployeeAdvanceSalaryProcess>();
            _oEmployeeAdvanceSalaryProcesss = EmployeeAdvanceSalaryProcess.Gets((int)Session[SessionInfo.currentUserID]);
            
            ViewBag.EmployeeGroups = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.StaffWorker, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeBlocks = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.Block, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                
            ViewBag.oEmployeeAdvanceSalaryProcess = _oEmployeeAdvanceSalaryProcesss;
            ViewBag.BusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oEmployeeAdvanceSalarys);
        }
        public ActionResult View_EmployeeAdvanceSalaryProcess(int menuid)
        {
            _oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
            if (menuid > 0)
            {
                _oEmployeeAdvanceSalaryProcess = _oEmployeeAdvanceSalaryProcess.Get(menuid, (int)Session[SessionInfo.currentUserID]);
            }
            string sSQL = "Select * from View_EmployeeAdvanceSalaryProcess";
            var oEASPs = new List<EmployeeAdvanceSalaryProcess>();
            oEASPs = EmployeeAdvanceSalaryProcess.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            List<SalaryHead> oSalaryHeads = new List<SalaryHead>();
            oSalaryHeads = SalaryHead.Gets("SELECT * FROM SalaryHead WHERE SalaryHeadType=" + (int)EnumSalaryHeadType.Deduction, (int)Session[SessionInfo.currentUserID]);


            ViewBag.EASPs = oEASPs;
            ViewBag.SalaryHeads = oSalaryHeads;

            return View(_oEmployeeAdvanceSalaryProcess);
        }

        [HttpPost]
        public JsonResult Save(EmployeeAdvanceSalary oEmployeeAdvanceSalary)
        {
            _oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
            try
            {
                _oEmployeeAdvanceSalary = oEmployeeAdvanceSalary;
                _oEmployeeAdvanceSalary = _oEmployeeAdvanceSalary.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployeeAdvanceSalary.ErrorMessage = ex.Message;

                _oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
                _oEmployeeAdvanceSalary.ErrorMessage = ex.Message.Split('~')[0];
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeAdvanceSalary);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(EmployeeAdvanceSalary oEmployeeAdvanceSalary)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oEmployeeAdvanceSalary.Delete(oEmployeeAdvanceSalary.EASID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Search(EmployeeAdvanceSalary oEmployeeAdvanceSalary)
        {

            List<EmployeeAdvanceSalary> oEmployeeAdvanceSalarys = new List<EmployeeAdvanceSalary>();
            try
            {
                DateTime dtStart = Convert.ToDateTime(oEmployeeAdvanceSalary.Params.Split('~')[0]);
                DateTime dtEnd = Convert.ToDateTime(oEmployeeAdvanceSalary.Params.Split('~')[1]);
                int BusinessunitID = Convert.ToInt32(oEmployeeAdvanceSalary.Params.Split('~')[2]);
                string LocationIDs = Convert.ToString(oEmployeeAdvanceSalary.Params.Split('~')[3]);
                string DepartmentIDs = Convert.ToString(oEmployeeAdvanceSalary.Params.Split('~')[4]);
                int  sSalary = Convert.ToInt32(oEmployeeAdvanceSalary.Params.Split('~')[5]);
                int eSalary = Convert.ToInt32(oEmployeeAdvanceSalary.Params.Split('~')[6]);
                string GroupIDs = Convert.ToString(oEmployeeAdvanceSalary.Params.Split('~')[7]);
                string BlockIDs = Convert.ToString(oEmployeeAdvanceSalary.Params.Split('~')[8]);
                string DesignationIDs = Convert.ToString(oEmployeeAdvanceSalary.Params.Split('~')[9]);
                bool bDeclaration = Convert.ToBoolean(oEmployeeAdvanceSalary.Params.Split('~')[10]);
                string sDeclarationDate = oEmployeeAdvanceSalary.Params.Split('~')[11];
                bool bDateBetween = Convert.ToBoolean(oEmployeeAdvanceSalary.Params.Split('~')[12]);


                string sSQL = "SELECT * FROM View_EmployeeAdvanceSalary WHERE EASPID IN(SELECT EASPID FROM EmployeeAdvanceSalaryProcess  WITH (NOLOCK) WHERE EASPID <> 0";
                if (bDateBetween)
                {
                    sSQL += "  StartDate='" + dtStart.ToString("dd MMM yyyy") + "' AND EndDate='" + dtEnd.ToString("dd MMM yyyy") + "'";
                }
                if (bDeclaration)
                {
                    sSQL += " AND DeclarationDate='" + sDeclarationDate+"'";
                }
                if (BusinessunitID != 0)
                {
                    sSQL += " AND BUID=" + BusinessunitID;
                }
                if (!string.IsNullOrEmpty(LocationIDs))
                {
                    sSQL += " AND LocationID IN(" + LocationIDs + ")";
                }
                sSQL = sSQL + ")";
                if (!string.IsNullOrEmpty(DepartmentIDs))
                {
                    sSQL += " AND DepartmentID IN(" + DepartmentIDs + ")";
                }
                if (!string.IsNullOrEmpty(DesignationIDs))
                {
                    sSQL += " AND DesignationID IN(" + DesignationIDs + ")";
                }
                if ((sSalary > 0 && eSalary > 0) && (sSalary <= eSalary))
                {
                    sSQL += " AND GrossSalary BETWEEN '" +sSalary +"' AND '" + eSalary + "'" ;
                }
                if (BlockIDs != "")
                {
                    sSQL += "AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + BlockIDs + "))";
                }
                if (GroupIDs != "")
                {
                    sSQL += " AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + GroupIDs + "))";
                }

                oEmployeeAdvanceSalarys = EmployeeAdvanceSalary.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oEmployeeAdvanceSalarys = new List<EmployeeAdvanceSalary>();
                oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
                oEmployeeAdvanceSalary.ErrorMessage = ex.Message;
                oEmployeeAdvanceSalarys.Add(oEmployeeAdvanceSalary);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeAdvanceSalarys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //For EmployeeAdvanceSalaryProcess

        [HttpPost]
        public JsonResult SaveProcess(EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess)
        {
            _oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
            try
            {
                _oEmployeeAdvanceSalaryProcess = oEmployeeAdvanceSalaryProcess;
                _oEmployeeAdvanceSalaryProcess = _oEmployeeAdvanceSalaryProcess.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployeeAdvanceSalaryProcess.ErrorMessage = ex.Message;

                _oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
                _oEmployeeAdvanceSalaryProcess.ErrorMessage = ex.Message.Split('~')[0];
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeAdvanceSalaryProcess);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteProcess(EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oEmployeeAdvanceSalaryProcess.Delete(oEmployeeAdvanceSalaryProcess.EASPID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveProcess(EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess)
        {
            _oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
            try
            {
                _oEmployeeAdvanceSalaryProcess = oEmployeeAdvanceSalaryProcess.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
                _oEmployeeAdvanceSalaryProcess.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeAdvanceSalaryProcess);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UndoApproveProcess(EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess)
        {
            _oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
            try
            {
                _oEmployeeAdvanceSalaryProcess = oEmployeeAdvanceSalaryProcess.UndoApprove((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
                _oEmployeeAdvanceSalaryProcess.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeAdvanceSalaryProcess);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public void ExcelAdvanceSalaryPayment(string sParam, double nts)
        {

            string sMsg = "";
            string sSQL = "";

            _oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
            _oEmployeeAdvanceSalary = makeSQLQuery(sParam, false);


            string sDate = _oEmployeeAdvanceSalary.Params.Split('~')[0];
            string eDate = _oEmployeeAdvanceSalary.Params.Split('~')[1];

            DateTime startDate = Convert.ToDateTime(sDate);
            DateTime endDate = Convert.ToDateTime(eDate);
            string BlockIDs = _oEmployeeAdvanceSalary.BlockName;

            _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.Code).ToList();

            List<EmployeeBankAccount> oEmployeeBankAccounts = new List<EmployeeBankAccount>();
            if (_oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.Count > 0)
            {
                string sEmployeeID = string.Join(",", _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.Select(x => x.EmployeeID));
                oEmployeeBankAccounts = EmployeeBankAccount.Gets("SELECT * FROM VIEW_EmployeeBankAccount WHERE EmployeeID IN(" + sEmployeeID + ") AND IsActive=1", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeAdvanceSalary.Company = oCompanys.First();
            _oEmployeeAdvanceSalary.ErrorMessage = sMsg;


            int nMaxColumn = 0;
            int nStartCol = 2, nEndCol = (_bIsCompliance ? 17 : 19);
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
                var sheet = excelPackage.Workbook.Worksheets.Add("Advance Salary Payment");
                sheet.Name = "Advance Salary Payment";

                colIndex = 1;
                sheet.Column(colIndex++).Width = 6; //Blank
                sheet.Column(colIndex++).Width = 6; //Sl
                sheet.Column(colIndex++).Width = 15; //ID
                sheet.Column(colIndex++).Width = 25; //Name                
                sheet.Column(colIndex++).Width = 25; //Designation
                sheet.Column(colIndex++).Width = 20; //DOJ
                if (!_bIsCompliance)
                {
                    sheet.Column(colIndex++).Width = 20; //DaysOfDuration
                }
                sheet.Column(colIndex++).Width = 10; //Att Days
                sheet.Column(colIndex++).Width = 10; //Early Min
                sheet.Column(colIndex++).Width = 10; //Late days
                sheet.Column(colIndex++).Width = 10; //Present Gross
                sheet.Column(colIndex++).Width = 15; //Gross Earning
                if (!_bIsCompliance)
                {
                    sheet.Column(colIndex++).Width = 15; //TotalDeduction
                }                
                sheet.Column(colIndex++).Width = 15; //Net Pay
                sheet.Column(colIndex++).Width = 15; //cash
                sheet.Column(colIndex++).Width = 15; //bank
                sheet.Column(colIndex++).Width = 20; //account no
                sheet.Column(colIndex++).Width = 20; //Bank
                sheet.Column(colIndex).Width = 15; //Signature
                nMaxColumn = colIndex;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.Bold = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Font.Size = 15;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "Advance Salary Payment"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;
               

                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ID"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DOJ"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if (!_bIsCompliance)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DaysOfDuration"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }  
                
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Att Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Early Min"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Late days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Present Gross"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Earning"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if (!_bIsCompliance)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "TotalDeduction"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }  
                
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Pay"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bank"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Cash"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Account No."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bank"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Signature"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion

                #region Table Body
                int nSL = 0;

                var EASPayment = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.GroupBy(x => new { x.LocationName, x.DepartmentName }
                    , (key, group)
               => new
               {
                   LocationName = key.LocationName,
                   DepartmentName = key.DepartmentName,
                   Result = group
               }).ToList();


                var EASPaymentBlock = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.GroupBy(x => new { x.LocationName, x.DepartmentName, x.BlockName }
                    , (key, group)
               => new
               {
                   LocationName = key.LocationName,
                   DepartmentName = key.DepartmentName,
                   BlockName = key.BlockName,
                   Result = group
               }).ToList();


                if (EASPayment.Count == 0)
                {
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nEndCol]; cell.Value = "No data to print"; cell.Style.Font.Bold = true;
                    cell.Merge = true; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }
                else
                {
                    Dictionary<string, string> aGrandTotals = new Dictionary<string, string>();
                    string sStartCell = "", sEndCell = "", sFormula = "";
                    int nStartRow = 0, nEndRow = 0;
                    if (!string.IsNullOrEmpty(BlockIDs))
                    {
                        #region withBlock
                        foreach (var data in EASPaymentBlock)
                        {
                            colIndex = 1;
                            cell = sheet.Cells[rowIndex, nStartCol]; cell.Value = "Location-" + data.LocationName + ",Department-" + data.DepartmentName + ", BlockName - " + data.BlockName; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, nMaxColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex += 1;

                            nStartRow = rowIndex; nEndRow = 0;
                            foreach (var oItem in data.Result)
                            {
                                nSL++;
                                colIndex = nStartCol;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Code.ToString(); cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.JoiningDateInString; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                if (!_bIsCompliance)
                                {
                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.DateDiff("D", startDate, endDate) + 1; cell.Style.Font.Bold = false;
                                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                }  

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PaymentDays; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalEarlyInMin; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalLateInDays; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossSalary; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossEarnings; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                if (!_bIsCompliance)
                                {
                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalDeductions; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                }  

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NetAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                List<EmployeeBankAccount> oBanks = new List<EmployeeBankAccount>();
                                oBanks = oEmployeeBankAccounts.Where(x => x.EmployeeID == oItem.EmployeeID).ToList();

                                //bank
                                double dValue = 0;
                                dValue = (oBanks.Count > 0 ? oItem.NetAmount : 0);
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = dValue; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                //cash
                                dValue = 0;
                                dValue = (oBanks.Count <= 0 ? oItem.NetAmount : 0);
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = dValue; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                //account no
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oBanks.Count > 0 ? oBanks[0].AccountNo : ""; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                //Bank
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oBanks.Count > 0 ? oBanks[0].BankBranchName : ""; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                //Signature
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                nEndRow = rowIndex;
                                rowIndex++;
                            }
                            #region Sub Total
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            if (!_bIsCompliance)
                            {
                                cell = sheet.Cells[rowIndex, colIndex++];
                                cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                            }  
                            
                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex];
                            cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Gross Amount
                            colIndex = colIndex + 1;
                            aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + nEndRow.ToString()));
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Gross Earnings
                            colIndex = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            if (!_bIsCompliance)
                            {
                                //Total Deductions
                                colIndex = colIndex + 1;
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                                cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }  

                            //Net Amount
                            colIndex = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Bank Amount
                            colIndex = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Cash Amount
                            colIndex = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Account No
                            colIndex = colIndex + 1;
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Bank
                            colIndex = colIndex + 1;
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Signature
                            colIndex = colIndex + 1;
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex++;
                            #endregion
                        }
                        #region Grand Total
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        if (!_bIsCompliance)
                        {
                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                        }  
                        
                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex];
                        cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Gross Amount
                        colIndex = colIndex + 1;
                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
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
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //GrossEarnings
                        colIndex = colIndex + 1;
                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
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
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (!_bIsCompliance)
                        {
                            //TotalDeductions
                            colIndex = colIndex + 1;
                            #region Formula
                            sFormula = "";
                            if (aGrandTotals.Count > 0)
                            {
                                sFormula = "SUM(";
                                for (int i = 1; i <= aGrandTotals.Count; i++)
                                {
                                    nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                    nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                    sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                    sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
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
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                                sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                            }
                            #endregion
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }  
                                                
                        //Net Amount
                        colIndex = colIndex + 1;
                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
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
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Bank Amount
                        colIndex = colIndex + 1;
                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
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
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        //Cash Amount
                        colIndex = colIndex + 1;
                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
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
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Account No
                        colIndex = colIndex + 1;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Bank
                        colIndex = colIndex + 1;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Signature
                        colIndex = colIndex + 1;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        #endregion
                        #endregion
                    }
                    else
                    {
                        #region withouBlock
                        foreach (var data in EASPayment)
                        {
                            colIndex = nStartCol;
                            cell = sheet.Cells[rowIndex, nStartCol]; cell.Value = "Location-" + data.LocationName + ",Department-" + data.DepartmentName; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, nMaxColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex += 1;

                            nStartRow = rowIndex; nEndRow = 0;
                            foreach (var oItem in data.Result)
                            {
                                nSL++;
                                colIndex = nStartCol;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Code.ToString(); cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.JoiningDateInString; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                if (!_bIsCompliance)
                                {
                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.DateDiff("D", startDate, endDate) + 1; cell.Style.Font.Bold = false;
                                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                }  

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PaymentDays; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalEarlyInMin; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalLateInDays; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossSalary; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossEarnings; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                if (!_bIsCompliance)
                                {
                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalDeductions; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                }  

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NetAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                List<EmployeeBankAccount> oBanks = new List<EmployeeBankAccount>();
                                oBanks = oEmployeeBankAccounts.Where(x => x.EmployeeID == oItem.EmployeeID).ToList();

                                //bank
                                double dValue = 0;
                                dValue = (oBanks.Count > 0 ? oItem.NetAmount : 0);
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = dValue; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                //cash
                                dValue = 0;
                                dValue = (oBanks.Count <= 0 ? oItem.NetAmount : 0);
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = dValue; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                //account no
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oBanks.Count > 0 ? oBanks[0].AccountNo : ""; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                //Bank
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oBanks.Count > 0 ? oBanks[0].BankBranchName : ""; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                //Signature
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nEndRow = rowIndex;
                                rowIndex++;
                            }

                            #region Sub Total
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            if (!_bIsCompliance)
                            {
                                cell = sheet.Cells[rowIndex, colIndex++];
                                cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                            }  
                                                        
                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex];
                            cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Gross Amount
                            colIndex = colIndex + 1;
                            aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + nEndRow.ToString()));
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Gross Earnings
                            colIndex = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            if (!_bIsCompliance)
                            {
                                //Total Deductions
                                colIndex = colIndex + 1;
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                                cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }  
                                                        
                            //Net Amount
                            colIndex = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Bank Amount
                            colIndex = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Cash Amount
                            colIndex = colIndex + 1;
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Account No
                            colIndex = colIndex + 1;
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Bank
                            colIndex = colIndex + 1;
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Signature
                            colIndex = colIndex + 1;
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex++;
                            #endregion
                        }
                        #region Grand Total
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        if (!_bIsCompliance)
                        {
                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                        }  

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex];
                        cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Gross Amount
                        colIndex = colIndex + 1;
                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
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
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //GrossEarnings
                        colIndex = colIndex + 1;
                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
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
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (!_bIsCompliance)
                        {
                            //TotalDeductions
                            colIndex = colIndex + 1;
                            #region Formula
                            sFormula = "";
                            if (aGrandTotals.Count > 0)
                            {
                                sFormula = "SUM(";
                                for (int i = 1; i <= aGrandTotals.Count; i++)
                                {
                                    nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                    nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                    sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                    sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
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
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                                sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                            }
                            #endregion
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }  

                        //Net Amount
                        colIndex = colIndex + 1;
                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
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
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Bank Amount
                        colIndex = colIndex + 1;
                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
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
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        //Cash Amount
                        colIndex = colIndex + 1;
                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
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
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Account No
                        colIndex = colIndex + 1;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Bank
                        colIndex = colIndex + 1;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Signature
                        colIndex = colIndex + 1;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        #endregion
                        #endregion
                    }
                }
                #endregion

                cell = sheet.Cells[1, 1, rowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=AdvanceSalaryPayment.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void ExcelAdvanceSalaryPaymentF2(string sParam, double nts)
        {
            string sMsg = "";
            string sSQL = "";

            _oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
            _oEmployeeAdvanceSalary = makeSQLQuery(sParam, false);


            string sDate = _oEmployeeAdvanceSalary.Params.Split('~')[0];
            string eDate = _oEmployeeAdvanceSalary.Params.Split('~')[1];

            DateTime startDate = Convert.ToDateTime(sDate);
            DateTime endDate = Convert.ToDateTime(eDate);
            string BlockIDs = _oEmployeeAdvanceSalary.BlockName;

            _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.Code).ToList();

            List<EmployeeBankAccount> oEmployeeBankAccounts = new List<EmployeeBankAccount>();
            if (_oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.Count > 0)
            {
                string sEmployeeID = string.Join(",", _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.Select(x => x.EmployeeID));
                oEmployeeBankAccounts = EmployeeBankAccount.Gets("SELECT * FROM VIEW_EmployeeBankAccount WHERE EmployeeID IN(" + sEmployeeID + ") AND IsActive=1", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeAdvanceSalary.Company = oCompanys.First();
            _oEmployeeAdvanceSalary.ErrorMessage = sMsg;


            int nMaxColumn = 0;
            int nStartCol = 2, nEndCol = 9;
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
                var sheet = excelPackage.Workbook.Worksheets.Add("Advance Salary Payment");
                sheet.Name = "Advance Salary Payment";

                sheet.Column(1).Width = 6; //Blank
                sheet.Column(2).Width = 6; //Sl
                sheet.Column(3).Width = 15; //ID
                sheet.Column(4).Width = 25; //Name                
                sheet.Column(5).Width = 25; //Designation
                sheet.Column(6).Width = 10; //Att Days
                sheet.Column(7).Width = 15; //Gross Amount
                sheet.Column(8).Width = 15; //Net Amount
                sheet.Column(9).Width = 15; //Signature
                nMaxColumn = 9;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.Bold = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Font.Size = 15;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "Advance Salary Payment"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;
                #endregion


                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ID"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Att Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Signature"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;

                #region Table Body
                int nSL = 0;

                var EASPayment = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.GroupBy(x => new { x.LocationName, x.DepartmentName }
                    , (key, group)
               => new
               {
                   LocationName = key.LocationName,
                   DepartmentName = key.DepartmentName,
                   Result = group
               }).ToList();


                var EASPaymentBlock = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.GroupBy(x => new { x.LocationName, x.DepartmentName, x.BlockName }
                    , (key, group)
               => new
               {
                   LocationName = key.LocationName,
                   DepartmentName = key.DepartmentName,
                   BlockName = key.BlockName,
                   Result = group
               }).ToList();


                if (EASPayment.Count == 0)
                {
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nEndCol]; cell.Value = "No data to print"; cell.Style.Font.Bold = true;
                    cell.Merge = true; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }
                else
                {
                    Dictionary<string, string> aGrandTotals = new Dictionary<string, string>();
                    string sStartCell = "", sEndCell = "", sFormula = "";
                    int nStartRow = 0, nEndRow = 0;
                    if (!string.IsNullOrEmpty(BlockIDs))
                    {
                        #region withBlock
                        foreach (var data in EASPaymentBlock)
                        {
                            colIndex = 1;
                            cell = sheet.Cells[rowIndex, nStartCol]; cell.Value = "Location-" + data.LocationName + ",Department-" + data.DepartmentName + ", BlockName - " + data.BlockName; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, nMaxColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex += 1;

                            nStartRow = rowIndex; nEndRow = 0;
                            foreach (var oItem in data.Result)
                            {                                
                                nSL++;
                                colIndex = nStartCol;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Code.ToString(); cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PaymentDays; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossSalary; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NetAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nEndRow = rowIndex;
                                rowIndex++;
                            }
                            #region Sub Total
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex];
                            cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Gross Amount
                            colIndex = colIndex + 1;
                            aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + nEndRow.ToString()));
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Net Amount
                            colIndex = colIndex + 1;                            
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Signature
                            colIndex = colIndex + 1;
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex++;
                            #endregion
                        }
                        #region Grand Total
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex];
                        cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Gross Amount
                        colIndex = colIndex + 1;
                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
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
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Net Amount
                        colIndex = colIndex + 1;
                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
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
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Signature
                        colIndex = colIndex + 1;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        #endregion
                        #endregion
                    }
                    else
                    {
                        #region withouBlock
                        foreach (var data in EASPayment)
                        {
                            colIndex = nStartCol;
                            cell = sheet.Cells[rowIndex, nStartCol]; cell.Value = "Location-" + data.LocationName + ",Department-" + data.DepartmentName; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, nMaxColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex += 1;

                            nStartRow = rowIndex; nEndRow = 0;
                            foreach (var oItem in data.Result)
                            {                                
                                nSL++;
                                colIndex = nStartCol;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Code.ToString(); cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                                                
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PaymentDays; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                                                
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossSalary; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NetAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nEndRow = rowIndex;
                                rowIndex++;
                            }

                            #region Sub Total
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++];
                            cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex];
                            cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Gross Amount
                            colIndex = colIndex + 1;
                            aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + nEndRow.ToString()));
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Net Amount
                            colIndex = colIndex + 1;                            
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Signature
                            colIndex = colIndex + 1;
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex++;
                            #endregion
                        }
                        #region Grand Total
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex];
                        cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Gross Amount
                        colIndex = colIndex + 1;
                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {                            
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
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
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Net Amount
                        colIndex = colIndex + 1;
                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
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
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion                        
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Signature
                        colIndex = colIndex + 1;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                        #endregion
                        #endregion
                    }
                }
                #endregion

                cell = sheet.Cells[1, 1, rowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=AdvanceSalaryPayment.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void ExcelAdvanceSalaryPaymentProcessTab(string sParam, double nts)
        {
            #region Commnet code
            //string sDate = sParam.Split('~')[0];
            //string eDate = sParam.Split('~')[1];
            //string sEmployeeIDs = sParam.Split('~')[2];
            //int BusinessUnitID = Convert.ToInt32(sParam.Split('~')[3]);
            //string LocationIDs = sParam.Split('~')[4];
            //string DepartmentIDs = sParam.Split('~')[5];
            //int sSalary = Convert.ToInt32(sParam.Split('~')[6]);
            //int eSalary = Convert.ToInt32(sParam.Split('~')[7]);
            //string GroupIDs = sParam.Split('~')[8];
            //string BlockIDs = sParam.Split('~')[9];
            //string DesignationIDs = sParam.Split('~')[10];

            //DateTime startDate = Convert.ToDateTime(sDate);
            //DateTime endDate = Convert.ToDateTime(eDate);

            //string sMsg = "";

            //string sSQL = "SELECT * FROM View_EmployeeAdvanceSalary WHERE EASPID IN(SELECT EASPID FROM EmployeeAdvanceSalaryProcess WHERE StartDate='" + sDate + "' AND EndDate='" + eDate + "'";

            //if (BusinessUnitID > 0)
            //{
            //    sSQL += " AND BUID =" + BusinessUnitID;
            //}

            //if (!string.IsNullOrEmpty(LocationIDs))
            //{
            //    sSQL += " AND LocationID IN(" + LocationIDs + ")";
            //}
            //sSQL = sSQL + ")";

            //if (!string.IsNullOrEmpty(DepartmentIDs))
            //{
            //    sSQL += " AND DepartmentID IN(" + DepartmentIDs + ")";
            //}
            //if (!string.IsNullOrEmpty(sEmployeeIDs))
            //{
            //    sSQL += " AND EmployeeID IN(" + sEmployeeIDs + ")";
            //}
            //if (sSalary > 0 && eSalary > 0)
            //{
            //    sSQL += " AND GrossSalary BETWEEN '" + sSalary + "' AND '" + eSalary + "'";
            //}
            //if (!string.IsNullOrEmpty(DesignationIDs))
            //{
            //    sSQL += " AND DesignationID IN(" + DesignationIDs + ")";
            //}
            //if (BlockIDs != "")
            //{
            //    sSQL += "AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + BlockIDs + "))";
            //}
            //if (GroupIDs != "")
            //{
            //    sSQL += " AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + GroupIDs + "))";
            //}

            //sSQL += " ORDER BY EmployeeID";

            //_oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
            //_oEmployeeAdvanceSalary.EmployeeAdvanceSalarys = EmployeeAdvanceSalary.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            #endregion

            string sMsg = "";
            string sSQL = "";
            _oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
            _oEmployeeAdvanceSalary = makeSQLQuery(sParam, true);


            string sDate = _oEmployeeAdvanceSalary.Params.Split('~')[0];
            string eDate = _oEmployeeAdvanceSalary.Params.Split('~')[1];

            DateTime startDate = Convert.ToDateTime(sDate);
            DateTime endDate = Convert.ToDateTime(eDate);
            string BlockIDs = _oEmployeeAdvanceSalary.BlockName;

            _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.Code).ToList();

            List<EmployeeBankAccount> oEmployeeBankAccounts = new List<EmployeeBankAccount>();
            if (_oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.Count > 0)
            {
                string sEmployeeID = string.Join(",", _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.Select(x => x.EmployeeID));
                oEmployeeBankAccounts = EmployeeBankAccount.Gets("SELECT * FROM VIEW_EmployeeBankAccount WHERE EmployeeID IN(" + sEmployeeID + ") AND IsActive=1", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeAdvanceSalary.Company = oCompanys.First();
            _oEmployeeAdvanceSalary.ErrorMessage = sMsg;


            int nMaxColumn = 0;
            int nStartCol = 1, nEndCol = 15;
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
                var sheet = excelPackage.Workbook.Worksheets.Add("Advance Salary Payment");
                sheet.Name = "Advance Salary Payment";

                sheet.Column(1).Width = 6; //Sl
                sheet.Column(2).Width = 15; //Code
                sheet.Column(3).Width = 25; //Name
                //sheet.Column(4).Width = 25; //Dept Name
                sheet.Column(4).Width = 25; //Designation
                sheet.Column(5).Width = 20; //DOJ
                sheet.Column(6).Width = 20; //DaysOfDuration
                sheet.Column(7).Width = 10; //Att Days
                sheet.Column(8).Width = 10; //Early Min
                sheet.Column(9).Width = 10; //Late days
                sheet.Column(10).Width = 10; //Present Gross
                sheet.Column(11).Width = 10; // Gross Earning
                sheet.Column(12).Width = 15; //TotalDeduction
                sheet.Column(13).Width = 10; //Net Pay
                sheet.Column(14).Width = 15; //cash
                sheet.Column(15).Width = 15; //bank
                sheet.Column(16).Width = 20; //account no
                sheet.Column(17).Width = 10; //Signature

                nMaxColumn = 15;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Report Header
                //sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                //cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, 2]; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Font.Bold = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Font.Size = 15;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;

                //sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                //cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date : " + Date; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Advance Salary Payment"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;



                //sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                //cell = sheet.Cells[rowIndex, 2]; cell.Value = "Location : " + _oAttendanceDaily.LocationName; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                //rowIndex = rowIndex + 2;
                #endregion


                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ID"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DOJ"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DaysOfDuration"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Att Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Early Min"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Late days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Present Gross"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Earning"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "TotalDeduction"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Pay"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bank"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Cash"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Account No."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Signature"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowIndex++;

                #region Table Body
                int nSL = 0;

                var EASPayment = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.GroupBy(x => new { x.LocationName, x.DepartmentName }
                    , (key, group)
               => new
               {
                   LocationName = key.LocationName,
                   DepartmentName = key.DepartmentName,
                   Result = group
               }).ToList();


                var EASPaymentBlock = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.GroupBy(x => new { x.LocationName, x.DepartmentName, x.BlockName }
                    , (key, group)
               => new
               {
                   LocationName = key.LocationName,
                   DepartmentName = key.DepartmentName,
                   BlockName = key.BlockName,
                   Result = group
               }).ToList();


                if (EASPayment.Count == 0)
                {
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nEndCol]; cell.Value = "No data to print"; cell.Style.Font.Bold = true;
                    cell.Merge = true; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }
                else
                {
                    if (!string.IsNullOrEmpty(BlockIDs))
                    {
                        #region withBlock
                        foreach (var data in EASPaymentBlock)
                        {
                            colIndex = 1;

                            cell = sheet.Cells[rowIndex, 2]; cell.Value = "Location-" + data.LocationName + ",Department-" + data.DepartmentName + ", BlockName - " + data.BlockName; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex += 1;

                            foreach (var oItem in data.Result)
                            {
                                //foreach (AttendanceDaily oItem in _oAttendanceDaily.AttendanceDailys)
                                //{
                                nSL++;
                                colIndex = 1;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Code.ToString(); cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.JoiningDateInString; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.DateDiff("D", startDate, endDate) + 1; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PaymentDays; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalEarlyInMin; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalLateInDays; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossSalary; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossEarnings; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalDeductions; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NetAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                List<EmployeeBankAccount> oBanks = new List<EmployeeBankAccount>();
                                oBanks = oEmployeeBankAccounts.Where(x => x.EmployeeID == oItem.EmployeeID).ToList();


                                //bank
                                double dValue = 0;
                                dValue = (oBanks.Count > 0 ? oItem.NetAmount : 0);
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = dValue; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                //cash
                                dValue = 0;
                                dValue = (oBanks.Count <= 0 ? oItem.NetAmount : 0);
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = dValue; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                //account no
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oBanks.Count > 0 ? oBanks[0].AccountNo : ""; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                rowIndex++;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region withouBlock
                        foreach (var data in EASPayment)
                        {
                            colIndex = 1;

                            cell = sheet.Cells[rowIndex, 2]; cell.Value = "Location-" + data.LocationName + ",Department-" + data.DepartmentName; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex += 1;

                            foreach (var oItem in data.Result)
                            {
                                //foreach (AttendanceDaily oItem in _oAttendanceDaily.AttendanceDailys)
                                //{
                                nSL++;
                                colIndex = 1;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Code.ToString(); cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.JoiningDateInString; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.DateDiff("D", startDate, endDate) + 1; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PaymentDays; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalEarlyInMin; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalLateInDays; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossSalary; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossEarnings; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalDeductions; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NetAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                List<EmployeeBankAccount> oBanks = new List<EmployeeBankAccount>();
                                oBanks = oEmployeeBankAccounts.Where(x => x.EmployeeID == oItem.EmployeeID).ToList();


                                //bank
                                double dValue = 0;
                                dValue = (oBanks.Count > 0 ? oItem.NetAmount : 0);
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = dValue; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                //cash
                                dValue = 0;
                                dValue = (oBanks.Count <= 0 ? oItem.NetAmount : 0);
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = dValue; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                //account no
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oBanks.Count > 0 ? oBanks[0].AccountNo : ""; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                rowIndex++;
                            }
                        }
                        #endregion
                    }

                }

                #endregion


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=AdvanceSalaryPayment.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void ExcelAdvanceSalaryPaymentProcessTab_F2(string sParam, double nts)
        {
            double deptWiseTotal;
            double GrandTotal=0.0;
            string sMsg = "";
            string sSQL = "";
            _oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
            _oEmployeeAdvanceSalary = makeSQLQuery(sParam, true);

            string sDate = _oEmployeeAdvanceSalary.Params.Split('~')[0];
            string eDate = _oEmployeeAdvanceSalary.Params.Split('~')[1];

            DateTime startDate = Convert.ToDateTime(sDate);
            DateTime endDate = Convert.ToDateTime(eDate);
            string BlockIDs = _oEmployeeAdvanceSalary.BlockName;

            _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.Code).ToList();

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeAdvanceSalary.Company = oCompanys.First();
            _oEmployeeAdvanceSalary.ErrorMessage = sMsg;


            int nMaxColumn = 0;
            int nStartCol = 1, nEndCol = 15;
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
                var sheet = excelPackage.Workbook.Worksheets.Add("Advance Salary Payment F2");
                sheet.Name = "Advance Salary Payment F2";

                sheet.Column(1).Width = 6; //Sl
                sheet.Column(2).Width = 15; //Code
                sheet.Column(3).Width = 25; //Name
                sheet.Column(4).Width = 25; //Designation
                sheet.Column(5).Width = 20; //Attendance Days
                sheet.Column(6).Width = 20; //Gross Amount
                sheet.Column(7).Width = 15; //Net Amount
                sheet.Column(8).Width = 10; //Signature

                nMaxColumn = 15;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Report Header

                cell = sheet.Cells[rowIndex, 2]; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Font.Bold = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Font.Size = 15;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Advance Salary Payment F2"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;

                #endregion

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ID"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Attendance "; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Signature"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;


                #region Table Body
                int nSL = 0;
                int counter = 0;
                if (_oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.Count <= 0)
                {
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nEndCol]; cell.Value = "No data to print"; cell.Style.Font.Bold = true;
                    cell.Merge = true; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }
                else
                {
                    _oEmployeeAdvanceSalarys = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.OrderBy(x => x.DepartmentName).ToList();
                    while (_oEmployeeAdvanceSalarys.Count > 0)
                    {
                        counter++;
                        List<EmployeeAdvanceSalary> oTempEmployeeAdvanceSalarys = new List<EmployeeAdvanceSalary>();
                        oTempEmployeeAdvanceSalarys = _oEmployeeAdvanceSalarys.Where(x => x.DepartmentID == _oEmployeeAdvanceSalarys[0].DepartmentID).OrderBy(x => x.Code).ToList();


                        colIndex = 1;

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = oTempEmployeeAdvanceSalarys[0].BUName + ", " + oTempEmployeeAdvanceSalarys[0].LocationName + ", " + oTempEmployeeAdvanceSalarys[0].DepartmentName; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex += 1;

                        deptWiseTotal = 0.00;
                       
                        //int nStartRow = rowIndex;
                        foreach (EmployeeAdvanceSalary oItem in oTempEmployeeAdvanceSalarys)
                        {                            
                            nSL++;
                            colIndex = 1;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Code.ToString(); cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PaymentDays; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossSalary; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NetAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            rowIndex += 1;

                            deptWiseTotal += oItem.NetAmount;

                        }
                      
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true; cell.Value = "Total :"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;
                        colIndex += 6;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = deptWiseTotal; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0";
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        rowIndex += 1;
                        GrandTotal += deptWiseTotal;
                        //ExcelAttendanceDaily(oTempEmployeeAdvanceSalarys);
                        _oEmployeeAdvanceSalarys.RemoveAll(x => x.DepartmentID == oTempEmployeeAdvanceSalarys[0].DepartmentID);
                    }

                     //grand total

                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true; cell.Value = "Grand Total :"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;
                    colIndex += 6;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =GrandTotal; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Numberformat.Format = "#,##0";
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                   
                }
                #endregion



                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=AdvanceSalaryPaymentF2.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }


        }

        public EmployeeAdvanceSalary makeSQLQuery(string sParam, bool bFromProcess)
        {
            EmployeeAdvanceSalary oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
            EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess = new EmployeeAdvanceSalaryProcess();
            if (bFromProcess)
            {
                int EASPID = 0; int nPrintType = 0;
                EASPID = Convert.ToInt32(sParam.Split('~')[0]);
                if (sParam.Split('~').Count() > 1)
                {
                    nPrintType = Convert.ToInt32(sParam.Split('~')[1]);
                }
                string sSQL = "SELECT * FROM View_EmployeeAdvanceSalary WHERE EASPID=" + EASPID;
                if (nPrintType == 4 || nPrintType == 5)//Advance Salary Sheet (Com)
                {
                    sSQL = "SELECT * FROM View_EmployeeAdvanceSalaryCompliance WHERE EASPID=" + EASPID;
                    _bIsCompliance = true;
                }
                sSQL += " ORDER BY EmployeeID";

                oEmployeeAdvanceSalary.EmployeeAdvanceSalarys = EmployeeAdvanceSalary.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                string sQuery = "SELECT * FROM EmployeeAdvanceSalaryProcess WHERE EASPID=" + EASPID;
                oEmployeeAdvanceSalaryProcess = EmployeeAdvanceSalaryProcess.Get(sQuery, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                oEmployeeAdvanceSalary.Params = oEmployeeAdvanceSalaryProcess.StartDate.ToString("dd MMM yyyy") + "~" + oEmployeeAdvanceSalaryProcess.EndDate.ToString("dd MMM yyyy");
                oEmployeeAdvanceSalary.BlockName = "";
            }
            else
            {
                int nPrintType = 0;
                string sDate = sParam.Split('~')[0];
                string eDate = sParam.Split('~')[1];
                string sEmployeeIDs = sParam.Split('~')[2];
                int BusinessUnitID = Convert.ToInt32(sParam.Split('~')[3]);
                string LocationIDs = sParam.Split('~')[4];
                string DepartmentIDs = sParam.Split('~')[5];
                int sSalary = Convert.ToInt32(sParam.Split('~')[6]);
                int eSalary = Convert.ToInt32(sParam.Split('~')[7]);
                string GroupIDs = sParam.Split('~')[8];
                string BlockIDs = sParam.Split('~')[9];
                string DesignationIDs = sParam.Split('~')[10];
                bool bDeclaration = Convert.ToBoolean(sParam.Split('~')[11]);
                string sDeclarationDate = sParam.Split('~')[12];
                bool bDateBetween = Convert.ToBoolean(sParam.Split('~')[13]);
                if (sParam.Split('~').Count() > 14)
                {
                    nPrintType = Convert.ToInt32(sParam.Split('~')[14]);
                }

                DateTime startDate = Convert.ToDateTime(sDate);
                DateTime endDate = Convert.ToDateTime(eDate);

                oEmployeeAdvanceSalary.Params = sDate + "~" + eDate;
                oEmployeeAdvanceSalary.BlockName = BlockIDs;
                string sMsg = "";

                //<option value="5">Advance Salary Summery(Com)</option>
                //<option value="4">Advance Salary(Com)</option>
                string sSQL = "SELECT * FROM View_EmployeeAdvanceSalary WHERE EASPID IN(SELECT EASPID FROM EmployeeAdvanceSalaryProcess WITH (NoLock) WHERE EASPID <> 0";
                if (nPrintType == 4 || nPrintType == 5)
                {
                    sSQL = "SELECT * FROM View_EmployeeAdvanceSalaryCompliance WHERE EASPID IN(SELECT EASPID FROM EmployeeAdvanceSalaryProcess WITH (NoLock) WHERE EASPID <> 0";
                    _bIsCompliance = true;
                }

                if (bDateBetween)
                {
                    sSQL += " AND StartDate='" + sDate + "' AND EndDate='" + eDate + "'";
                }
                if (bDeclaration)
                {
                    sSQL += " AND DeclarationDate='" + sDeclarationDate+"'";
                }
                if (BusinessUnitID > 0)
                {
                    sSQL += " AND BUID =" + BusinessUnitID;
                }
                if (!string.IsNullOrEmpty(LocationIDs))
                {
                    sSQL += " AND LocationID IN(" + LocationIDs + ")";
                }
                sSQL = sSQL + ")";
                if (!string.IsNullOrEmpty(sEmployeeIDs))
                {
                    sSQL += " AND EmployeeID IN(" + sEmployeeIDs + ")";
                }
                if (!string.IsNullOrEmpty(DepartmentIDs))
                {
                    sSQL += " AND DepartmentID IN(" + DepartmentIDs + ")";
                }
                if (!string.IsNullOrEmpty(DesignationIDs))
                {
                    sSQL += " AND DesignationID IN(" + DesignationIDs + ")";
                }
                if (sSalary > 0 && eSalary > 0)
                {
                    sSQL += " AND GrossSalary BETWEEN '" + sSalary + "' AND '" + eSalary + "'";
                }
                if (BlockIDs != "")
                {
                    sSQL += "AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + BlockIDs + "))";
                }
                if (GroupIDs != "")
                {
                    sSQL += " AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + GroupIDs + "))";
                }

                sSQL += " ORDER BY EmployeeID";
                oEmployeeAdvanceSalary.EmployeeAdvanceSalarys = EmployeeAdvanceSalary.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            return oEmployeeAdvanceSalary;
        }

        public ActionResult PDFAdvanceSalaryPaymentF1(string sParam, double nts)
        {           
            string sMsg = "";
            string sSQL = "";

            _oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
            _oEmployeeAdvanceSalary = makeSQLQuery(sParam, false);


            string sDate = _oEmployeeAdvanceSalary.Params.Split('~')[0];
            string eDate = _oEmployeeAdvanceSalary.Params.Split('~')[1];

            DateTime startDate = Convert.ToDateTime(sDate);
            DateTime endDate = Convert.ToDateTime(eDate);
            string BlockIDs = _oEmployeeAdvanceSalary.BlockName;

            _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.Code).ToList();



            List<EmployeeBankAccount> oEmployeeBankAccounts = new List<EmployeeBankAccount>();
            if (_oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.Count > 0)
            {
                string sEmployeeID = string.Join(",", _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.Select(x => x.EmployeeID));
                oEmployeeBankAccounts = EmployeeBankAccount.Gets("SELECT * FROM VIEW_EmployeeBankAccount WHERE EmployeeID IN(" + sEmployeeID + ") AND IsActive=1", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeAdvanceSalary.Company = oCompanys.First();
            _oEmployeeAdvanceSalary.ErrorMessage = sMsg;

            sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_bIsCompliance)
            {
                rptAdvanceSalarySheet_F1 oReport = new rptAdvanceSalarySheet_F1();
                byte[] abytes = oReport.PrepareReportCom(_oEmployeeAdvanceSalary, oSalarySheetSignature, startDate, endDate, oEmployeeBankAccounts, BlockIDs);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptAdvanceSalarySheet_F1 oReport = new rptAdvanceSalarySheet_F1();
                byte[] abytes = oReport.PrepareReport(_oEmployeeAdvanceSalary, oSalarySheetSignature, startDate, endDate, oEmployeeBankAccounts, BlockIDs);
                return File(abytes, "application/pdf");
            }
        }

        public ActionResult PDFAdvanceSalaryPaymentF1ProcessTab(string sParam, double nts)
        {
            string sMsg = "";
            string sSQL = "";
            _oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
            _oEmployeeAdvanceSalary = makeSQLQuery(sParam, true);


            string sDate = _oEmployeeAdvanceSalary.Params.Split('~')[0];
            string eDate = _oEmployeeAdvanceSalary.Params.Split('~')[1];

            DateTime startDate = Convert.ToDateTime(sDate);
            DateTime endDate = Convert.ToDateTime(eDate);
            string BlockIDs = _oEmployeeAdvanceSalary.BlockName;

            _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.Code).ToList();



            List<EmployeeBankAccount> oEmployeeBankAccounts = new List<EmployeeBankAccount>();
            if (_oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.Count > 0)
            {
                string sEmployeeID = string.Join(",", _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.Select(x => x.EmployeeID));
                oEmployeeBankAccounts = EmployeeBankAccount.Gets("SELECT * FROM VIEW_EmployeeBankAccount WHERE EmployeeID IN(" + sEmployeeID + ") AND IsActive=1", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeAdvanceSalary.Company = oCompanys.First();
            _oEmployeeAdvanceSalary.ErrorMessage = sMsg;

            sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


            rptAdvanceSalarySheet_F1 oReport = new rptAdvanceSalarySheet_F1();
            byte[] abytes = oReport.PrepareReport(_oEmployeeAdvanceSalary, oSalarySheetSignature, startDate, endDate, oEmployeeBankAccounts, BlockIDs);
            return File(abytes, "application/pdf");
        }

        public void ExcelAdvanceSalarySummery(string sParam, double nts)
        {            
            string sMsg = "";
            string sSQL = "";

            _oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
            _oEmployeeAdvanceSalary = makeSQLQuery(sParam, false);


            string sDate = _oEmployeeAdvanceSalary.Params.Split('~')[0];
            string eDate = _oEmployeeAdvanceSalary.Params.Split('~')[1];

            DateTime startDate = Convert.ToDateTime(sDate);
            DateTime endDate = Convert.ToDateTime(eDate);
            string BlockIDs = _oEmployeeAdvanceSalary.BlockName;

            _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.Code).ToList();

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeAdvanceSalary.Company = oCompanys.First();
            _oEmployeeAdvanceSalary.ErrorMessage = sMsg;


            int nMaxColumn = 0;
            int nStartCol = 1, nEndCol = _bIsCompliance ? 7 : 8;
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
                var sheet = excelPackage.Workbook.Worksheets.Add("Advance Salary Summery");
                sheet.Name = "Advance Salary Summery";


                colIndex = 1;
                sheet.Column(colIndex++).Width = 6; //Blank
                sheet.Column(colIndex++).Width = 8; //Sl
                sheet.Column(colIndex++).Width = 25; //Department
                sheet.Column(colIndex++).Width = 20; //Man Power
                sheet.Column(colIndex++).Width = 20; //Salary
                sheet.Column(colIndex++).Width = 20; //Payable
                if (!_bIsCompliance)
                {
                    sheet.Column(colIndex++).Width = 20; //All Deduction
                }
                sheet.Column(colIndex).Width = 20; //Net payable
                nMaxColumn = colIndex;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = oCompany.Name; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.Bold = true;  cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Font.Size = 15;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true;  cell.Value = "Advance Salary Payment"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;
                #endregion

                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Man Power"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Salary"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Payable"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if (!_bIsCompliance)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "All Deduction"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net payable"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;

                #region Table Body
                int nSL = 0;
                var EASSummery = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.GroupBy(x => x.DepartmentID).Select(grp => new
                {
                    DepartmentID = grp.Key,
                    DepartmentName = grp.First().DepartmentName,
                    EmployeeID = grp.Key,
                    Result = grp
                }).ToList();

                if (EASSummery.Count == 0)
                {
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nEndCol]; cell.Value = "No data to print"; cell.Style.Font.Bold = true;
                    cell.Merge = true; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }
                else
                {
                    int nStartRow = 0, nEndRow = 0;
                    nStartRow = rowIndex; nEndRow = 0;
                    foreach (var data in EASSummery)
                    {
                        colIndex = 1;

                        int manPower = 0;
                        double totalSalary = 0.0;
                        double totalPayable = 0.0;
                        double allDeduction = 0.0;
                        double netPayable = 0.0;


                        foreach (var oItem in data.Result)
                        {

                            manPower += 1;
                            totalSalary += oItem.GrossSalary;
                            totalPayable += oItem.GrossEarnings;
                            allDeduction += oItem.TotalDeductions;
                            netPayable += oItem.NetAmount;
                        }

                        nSL++;
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.DepartmentName; cell.Style.Font.Bold = false; 
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = manPower; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = totalSalary; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = totalPayable; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (!_bIsCompliance)
                        {
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = allDeduction; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = netPayable; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nEndRow = rowIndex;
                        rowIndex++;
                    }


                    #region Total
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    string sStartCell = "", sEndCell="";
                    sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (!_bIsCompliance)
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                    #endregion
                }

                #endregion

                cell = sheet.Cells[1, 1, rowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=AdvanceSalarySummery.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public ActionResult PDFAdvanceSalarySummery(string sParam, double nts)
        {
            string sMsg = "";
            string sSQL = "";

            _oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
            _oEmployeeAdvanceSalary = makeSQLQuery(sParam, false);


            string sDate = _oEmployeeAdvanceSalary.Params.Split('~')[0];
            string eDate = _oEmployeeAdvanceSalary.Params.Split('~')[1];

            DateTime startDate = Convert.ToDateTime(sDate);
            DateTime endDate = Convert.ToDateTime(eDate);
            string BlockIDs = _oEmployeeAdvanceSalary.BlockName;

            _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.Code).ToList();

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeAdvanceSalary.Company = oCompanys.First();
            _oEmployeeAdvanceSalary.ErrorMessage = sMsg;

            sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_bIsCompliance)
            {
                rptAdvanceSalarySheet_F1 oReport = new rptAdvanceSalarySheet_F1();
                byte[] abytes = oReport.PrepareAdvanceSalarySummeryReportCom(_oEmployeeAdvanceSalary, oSalarySheetSignature, startDate, endDate);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptAdvanceSalarySheet_F1 oReport = new rptAdvanceSalarySheet_F1();
                byte[] abytes = oReport.PrepareAdvanceSalarySummeryReport(_oEmployeeAdvanceSalary, oSalarySheetSignature, startDate, endDate);
                return File(abytes, "application/pdf");
            }
            
        }

        public void ExcelAdvanceSalarySummeryProcessTab(string sParam, double nts)
        {
            string sMsg = "";
            string sSQL = "";
            _oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
            _oEmployeeAdvanceSalary = makeSQLQuery(sParam, true);


            string sDate = _oEmployeeAdvanceSalary.Params.Split('~')[0];
            string eDate = _oEmployeeAdvanceSalary.Params.Split('~')[1];

            DateTime startDate = Convert.ToDateTime(sDate);
            DateTime endDate = Convert.ToDateTime(eDate);
            string BlockIDs = _oEmployeeAdvanceSalary.BlockName;

            _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.Code).ToList();



            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeAdvanceSalary.Company = oCompanys.First();
            _oEmployeeAdvanceSalary.ErrorMessage = sMsg;


            int nMaxColumn = 0;
            int nStartCol = 1, nEndCol = 7;
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
                var sheet = excelPackage.Workbook.Worksheets.Add("Advance Salary Summery");
                sheet.Name = "Advance Salary Summery";



                sheet.Column(1).Width = 6; //Sl
                sheet.Column(2).Width = 15; //Department
                sheet.Column(3).Width = 25; //Man Power
                sheet.Column(4).Width = 20; //Salary
                sheet.Column(5).Width = 20; //Payable
                sheet.Column(6).Width = 20; //All Deduction
                sheet.Column(7).Width = 20; //Net payable

                nMaxColumn = 7;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Report Header
                //sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                //cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, 2]; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Font.Bold = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Font.Size = 15;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;

                //sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                //cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date : " + Date; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Advance Salary Payment"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;



                //sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                //cell = sheet.Cells[rowIndex, 2]; cell.Value = "Location : " + _oAttendanceDaily.LocationName; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                //rowIndex = rowIndex + 2;
                #endregion


                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Man Power"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Salary"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Payable"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "All Deduction"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net payable"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowIndex++;

                #region Table Body
                int nSL = 0;

                var EASSummery = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.GroupBy(x => x.DepartmentID).Select(grp => new
                {
                    DepartmentID = grp.Key,
                    DepartmentName = grp.First().DepartmentName,
                    EmployeeID = grp.Key,
                    Result = grp
                }).ToList();

                if (EASSummery.Count == 0)
                {
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nEndCol]; cell.Value = "No data to print"; cell.Style.Font.Bold = true;
                    cell.Merge = true; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }
                else
                {
                    foreach (var data in EASSummery)
                    {
                        colIndex = 1;

                        int manPower = 0;
                        double totalSalary = 0.0;
                        double totalPayable = 0.0;
                        double allDeduction = 0.0;
                        double netPayable = 0.0;


                        foreach (var oItem in data.Result)
                        {

                            manPower += 1;
                            totalSalary += oItem.GrossSalary;
                            totalPayable += oItem.GrossEarnings;
                            allDeduction += oItem.TotalDeductions;
                            netPayable += oItem.NetAmount;


                            //nSL++;
                            //colIndex = 1;
                            //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                            //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeID; cell.Style.Font.Bold = false;
                            //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                            //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.JoiningDateInString; cell.Style.Font.Bold = false;
                            //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.DateDiff("D", startDate, endDate); cell.Style.Font.Bold = false;
                            //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PaymentDaysInString; cell.Style.Font.Bold = false;
                            //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalEarlyInMin; cell.Style.Font.Bold = false;
                            //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalLateInDays; cell.Style.Font.Bold = false;
                            //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossSalary; cell.Style.Font.Bold = false;
                            //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossEarnings; cell.Style.Font.Bold = false;
                            //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalDeductions; cell.Style.Font.Bold = false;
                            //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NetAmount; cell.Style.Font.Bold = false;
                            //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                            //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            //rowIndex++;
                        }
                        nSL++;
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.DepartmentName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = manPower; ; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = totalSalary; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = totalPayable; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = allDeduction; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = netPayable; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }
                }

                #endregion


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=AdvanceSalarySummery.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion

        #region Print
        public ActionResult PDFAdvanceSalaryPayment(string sParam, double nts)
        {
            string sMsg = "";
            string sSQL = "";

            _oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
            _oEmployeeAdvanceSalary = makeSQLQuery(sParam, false);


            string sDate = _oEmployeeAdvanceSalary.Params.Split('~')[0];
            string eDate = _oEmployeeAdvanceSalary.Params.Split('~')[1];

            DateTime startDate = Convert.ToDateTime(sDate);
            DateTime endDate = Convert.ToDateTime(eDate);
            string BlockIDs = _oEmployeeAdvanceSalary.BlockName;

            _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.Code).ToList();

            
            sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeAdvanceSalary.Company = oCompanys.First();
            _oEmployeeAdvanceSalary.ErrorMessage = sMsg;

            rptAdvanceSalarySheet oReport = new rptAdvanceSalarySheet();
            byte[] abytes = oReport.PrepareReport(_oEmployeeAdvanceSalary, oSalarySheetSignature);
            return File(abytes, "application/pdf");
        }

        public ActionResult PDFAdvanceSalaryPaymentProcessTab(string sParam, double nts)
        {
            string sMsg = "";
            string sSQL = "";
            _oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
            _oEmployeeAdvanceSalary = makeSQLQuery(sParam, true);


            string sDate = _oEmployeeAdvanceSalary.Params.Split('~')[0];
            string eDate = _oEmployeeAdvanceSalary.Params.Split('~')[1];

            DateTime startDate = Convert.ToDateTime(sDate);
            DateTime endDate = Convert.ToDateTime(eDate);
            string BlockIDs = _oEmployeeAdvanceSalary.BlockName;

            _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys = _oEmployeeAdvanceSalary.EmployeeAdvanceSalarys.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.Code).ToList();


            sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeAdvanceSalary.Company = oCompanys.First();
            _oEmployeeAdvanceSalary.ErrorMessage = sMsg;

            rptAdvanceSalarySheet oReport = new rptAdvanceSalarySheet();
            byte[] abytes = oReport.PrepareReport(_oEmployeeAdvanceSalary, oSalarySheetSignature);
            return File(abytes, "application/pdf");
        }

        #endregion

        public System.Drawing.Image GetImage(byte[] Image)
        {
            if (Image != null)
            {
                string fileDirectory = Server.MapPath("~/Content/Image.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(Image);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;

            }
            else
            {
                return null;
            }
        }
    }
}

