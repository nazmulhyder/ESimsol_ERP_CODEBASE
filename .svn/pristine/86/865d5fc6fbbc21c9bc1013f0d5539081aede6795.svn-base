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
using OfficeOpenXml.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class EmployeeBonusController : Controller
    {
        #region Declaration
        EmployeeBonus _oEmployeeBonus;
        List<EmployeeBonus> _oEmployeeBonuss;
        EmployeeBonusProcess _oEmployeeBonusProcess;
        private List<EmployeeBonusProcess> _oEmployeeBonusProcesss;
        EmployeeBonusProcessObject _oEmployeeBonusProcessObject;
        private List<EmployeeBonusProcessObject> _oEmployeeBonusProcessObjects;
        bool _bPercent;
        #endregion

        #region Employee Bonouss
        public ActionResult View_EmployeeBonuss(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oEmployeeBonuss = new List<EmployeeBonus>();

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

            ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x=>x.Value!=0.ToString()).ToList();
            ViewBag.SalaryHeads = SalaryHead.Gets("SELECT * FROM SalaryHead WHERE IsActive=1 AND SalaryHeadType=2", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            

            return View(_oEmployeeBonuss);
        }

        public ActionResult View_EmployeeBonussComp(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oEmployeeBonuss = new List<EmployeeBonus>();

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

            ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x => x.Value != 0.ToString()).ToList();
            ViewBag.SalaryHeads = SalaryHead.Gets("SELECT * FROM SalaryHead WHERE IsActive=1 AND SalaryHeadType=2", ((User)(Session[SessionInfo.CurrentUser])).UserID);


            return View(_oEmployeeBonuss);
        }

        public ActionResult ViewEmployeeBonusProcessManagements(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oEmployeeBonusProcesss = new List<EmployeeBonusProcess>();
            _oEmployeeBonusProcessObjects = new List<EmployeeBonusProcessObject>();
            List<EmployeeBonusProcessObject> _oEmployeeBonusProcessObjectTemps = new List<EmployeeBonusProcessObject>();
            string sSQL = "select * from View_EmployeeBonusProcess";
            _oEmployeeBonusProcesss = EmployeeBonusProcess.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeBonusProcesss = _oEmployeeBonusProcesss.OrderByDescending(x => x.ProcessDate).ToList();
            ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x => x.Value != 0.ToString()).ToList();


            return View(_oEmployeeBonusProcesss);
        }
        [HttpPost]
        public JsonResult SearchBonus(string sBU, string sLocationID, int nMonthID, int nYear)
        {
            List<EmployeeBonusProcess> oEmployeeBonuss = new List<EmployeeBonusProcess>();
            try
            {
                string sSql = "SELECT * FROM View_EmployeeBonusProcess WHERE MonthID=" + nMonthID + " AND DATEPART(YYYY,BonusDeclarationDate)=" + nYear;
                if (sBU.Trim() != "" && sBU.Trim() != "0")
                {
                    sSql = sSql + " AND BUID=" + sBU;
                }
                if (sLocationID.Trim() != "")
                {
                    sSql = sSql + " AND LocationID=" + sLocationID;
                }
                //if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                //{
                //    sSql = sSql + "AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
                //}
                oEmployeeBonuss = EmployeeBonusProcess.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oEmployeeBonuss.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                oEmployeeBonuss = new List<EmployeeBonusProcess>();
                EmployeeBonusProcess oEmployeeBonus = new EmployeeBonusProcess();
                oEmployeeBonuss.Add(oEmployeeBonus);
                oEmployeeBonuss[0].ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeBonuss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewEmployeeBonusProcessManagement(string sid, string sMsg)
        {
            int nEBPID = Convert.ToInt32(sid);
            _oEmployeeBonusProcess = new EmployeeBonusProcess();
            if (nEBPID > 0)
            {
                _oEmployeeBonusProcess = EmployeeBonusProcess.Get(nEBPID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }

            string sSQL_SS = "SELECT * FROM SalaryScheme WHERE IsActive=1";
            _oEmployeeBonusProcess.SalarySchemes = SalaryScheme.Gets(sSQL_SS, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            string sSQL_SH = "SELECT * FROM SalaryHead WHERE IsActive=1 AND IsProcessDependent=1";
            _oEmployeeBonusProcess.SalaryHeads = SalaryHead.Gets(sSQL_SH, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            string sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSql = sSql + ")";
            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeBonusProcess.EmployeeGroups = EmployeeGroup.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.StaffWorker, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            //_oPayrollProcessManagement.Locations = Location.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeTypes = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            //ViewBag.BusinessUnits = BusinessUnit.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            List<DepartmentRequirementPolicy> oDRPs = new List<DepartmentRequirementPolicy>();
            oDRPs = DepartmentRequirementPolicy.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oDRPs = oDRPs.GroupBy(x => x.LocationID).Select(x => x.First()).ToList();
            ViewBag.Locations = oDRPs;
            ViewBag.SalaryHeads = SalaryHead.Gets("SELECT * FROM SalaryHead WHERE IsActive=1 AND SalaryHeadType=2", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.EmployeeGroups = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.StaffWorker, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.BankAccounts = BankAccount.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<EmployeeBonusProcessObject> oPPMOs = new List<EmployeeBonusProcessObject>();
            oPPMOs = EmployeeBonusProcessObject.Gets("SELECT * FROM EmployeeBonusProcessObject WHERE EBPID=" + nEBPID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.PPMobjects = oPPMOs;

            return View(_oEmployeeBonusProcess);
        }

        #endregion

        #region Process
        [HttpPost]
        public JsonResult BonousProcess(int SalaryHeadID, int ProcessMonth, int ProcessYear, string Purpose, DateTime UptoDate)
        {
            _oEmployeeBonuss = new List<EmployeeBonus>();
            _oEmployeeBonus = new EmployeeBonus();
            try
            {
                _oEmployeeBonuss = EmployeeBonus.Process(SalaryHeadID,ProcessMonth,ProcessYear,Purpose,UptoDate, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oEmployeeBonuss.Count <= 0)
                {
                    throw new Exception("Nothing to process!");
                }
            }
            catch (Exception ex)
            {
                _oEmployeeBonus = new EmployeeBonus();
                _oEmployeeBonuss = new List<EmployeeBonus>();
                _oEmployeeBonus.ErrorMessage = ex.Message;
                _oEmployeeBonuss.Add(_oEmployeeBonus);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeBonuss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Process

        [HttpGet]
        public JsonResult EmployeeBonus_Delete(string sEmployeeBonusIDs)
        {
            string sMsg = "";
            try
            {
                sMsg = EmployeeBonus.Delete(sEmployeeBonusIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                sMsg = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMsg);
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

        #region Search
        [HttpPost]
        public JsonResult SearchWithPasignation(int ProcessMonth, int ProcessYear, string sEmployeeIDs, string nLocationID, string sDepartmentIds, string sDesignationIds, int nCategory, double nStartSalaryRange, double nEndSalaryRange, int nLoadRecords, int nRowLength, string sBusinessUnitIDs, string sBlockIDs, string sGroupIDs, double ts)
        {
            try
            {
                string sSql = "";
                sSql = "SELECT * FROM (SELECT top(" + nLoadRecords + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY EmployeeCode) Row,* FROM View_EmployeeBonus WHERE EBID<>0";
                if (ProcessMonth>0)
                {
                    sSql += " AND Month =" + ProcessMonth;
                }
                if (ProcessYear > 0)
                {
                    sSql += " AND Year =" + ProcessYear;
                }
                if (sEmployeeIDs!="")
                {
                    sSql += " AND EmployeeID IN(" + sEmployeeIDs+")";
                }
                if (sBusinessUnitIDs != "")
                {
                    sSql += " AND BusinessUnitID IN("+sBusinessUnitIDs+")";
                }
                if (nLocationID!="")
                {
                    sSql += " AND LocationID IN(" + nLocationID+")";
                }
                if (sDepartmentIds != "")
                {
                    sSql += " AND DepartmentID IN(" + sDepartmentIds + ")";
                }
                if (sDesignationIds != "")
                {
                    sSql += " AND DesignationID IN(" + sDesignationIds + ")";
                }
                if (sBlockIDs != "")
                {
                    sSql += "AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + sBlockIDs + "))";
                }
                if (sGroupIDs != "")
                {
                    sSql += " AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + sGroupIDs + "))";
                }
                if (nCategory>0)
                {
                    sSql += " AND EmployeeCategory=" + nCategory;
                }
                if (nStartSalaryRange > 0 && nEndSalaryRange > 0)
                {
                    sSql += " AND GrossAmount BETWEEN " + nStartSalaryRange + " AND " + nEndSalaryRange;
                }
                if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                {
                    sSql = sSql + " AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID "
                                + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
                }

                sSql = sSql + ") aa WHERE Row >" + nRowLength + ") aaa ORDER BY EmployeeCode";
                _oEmployeeBonuss = new List<EmployeeBonus>();
                _oEmployeeBonuss = EmployeeBonus.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oEmployeeBonuss.Count == 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oEmployeeBonuss = new List<EmployeeBonus>();
                _oEmployeeBonus = new EmployeeBonus();
                _oEmployeeBonus.ErrorMessage = ex.Message;
                _oEmployeeBonuss.Add(_oEmployeeBonus);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeBonuss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Search

        #region EB Approve 
        [HttpPost]
        public JsonResult EmployeeBonus_Approve(string sParams)
        {
            _oEmployeeBonus = new EmployeeBonus();
            _oEmployeeBonuss = new List<EmployeeBonus>();
            try
            {
                _oEmployeeBonuss = EmployeeBonus.Approve(sParams, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oEmployeeBonuss.Count > 0 && _oEmployeeBonuss[0].ErrorMessage != "")
                {
                    _oEmployeeBonus.ErrorMessage = _oEmployeeBonuss[0].ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _oEmployeeBonus = new EmployeeBonus();
                _oEmployeeBonuss = new List<EmployeeBonus>();
                _oEmployeeBonus.ErrorMessage = ex.Message;
                _oEmployeeBonuss.Add(_oEmployeeBonus);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeBonuss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion DA Approve 

        #region XL
        public void PrintBEListInXL(string sParams, double ts)
        {
            _oEmployeeBonuss = new List<EmployeeBonus>();
            _oEmployeeBonuss = EBGets(sParams);
            string sESSIDs = string.Join(",", _oEmployeeBonuss.Select(x=>x.ESSID));
            List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
            if(!string.IsNullOrEmpty(sESSIDs))
            {
                oEmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets("SELECT * FROM VIEW_EmployeeSalaryStructureDetail WHERE ESSID IN(" + sESSIDs + ") AND SalaryHeadType=1", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            List<EmployeeSalaryStructureDetail> oDistinctSalaryHeads = new List<EmployeeSalaryStructureDetail>();
            oDistinctSalaryHeads = oEmployeeSalaryStructureDetails.GroupBy(x => x.SalaryHeadName).Select(g => g.First()).OrderBy(x=>x.SalaryHeadID).ToList();


            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("BONUS List");
                sheet.Name = "BONUS LIST";

                sheet.Column(2).Width = 6; //SL
                sheet.Column(3).Width = 20; //EmployeeCode
                sheet.Column(4).Width = 20; //EmployeeName
                sheet.Column(5).Width = 20; //LocationName
                sheet.Column(6).Width = 20; //DepartmentName
                sheet.Column(7).Width = 20; //DesignationName
                sheet.Column(8).Width = 20; //JOINING
                sheet.Column(9).Width = 20; //CONFIRMATION
                sheet.Column(10).Width = 20; //CONFIRMATION
                sheet.Column(11).Width = 20; //CATEGORY
                sheet.Column(12).Width = 20; //SalaryHeadName
                sheet.Column(13).Width = 20; //Note
                sheet.Column(14).Width = 20; //Month
                sheet.Column(15).Width = 20; //Year
                sheet.Column(16).Width = 20; //Declaration
                sheet.Column(17).Width = 20; //GROSS
                int nCol = 18;
                foreach (EmployeeSalaryStructureDetail oItem in oDistinctSalaryHeads)
                {
                    sheet.Column(nCol++).Width = 20; //SS
                }
                sheet.Column(nCol++).Width = 20; //others
                sheet.Column(nCol++).Width = 20; //BonusAmount
                sheet.Column(nCol++).Width = 20; //stamp
                sheet.Column(nCol++).Width = 20; //EBGrossAmount
                sheet.Column(nCol++).Width = 20; //Account no 
                sheet.Column(nCol++).Width = 20; //BAnk name

                nMaxColumn = nCol;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2]; cell.Value = (_oEmployeeBonuss[0].Note); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
               
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Location"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "joining"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Confirmation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Service Year"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Category"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Salary Head"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Note"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Month"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Year"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Declaration"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                foreach (EmployeeSalaryStructureDetail oItem in oDistinctSalaryHeads)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SalaryHeadName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Other Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bonus Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Stamp"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Bonus Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Account No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bank Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowIndex++;
                #endregion

                #region Table Body
                int nSL = 0;
                double nGrossAmount = 0;
                double nBonusAmount = 0;
                double nEBGrossAmount = 0;
                double nOthersAmount = 0;
                double nStamp = 0;


                foreach (EmployeeBonus EB in _oEmployeeBonuss)
                {
                    nSL++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.EmployeeCode; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.EmployeeName; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.LocationName; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.DepartmentName; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.DesignationName; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.JoiningDateInString; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.ConfirmationDateInString; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Service Year
                    DateDifference dateDifference = new DateDifference(EB.BonusDeclarationDate, EB.JoiningDate);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = dateDifference.ToString(); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.EmployeeCategoryInString; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.SalaryHeadName; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.Note; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.MonthInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.Year; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.BonusDeclarationDateInString; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.GrossAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nGrossAmount = nGrossAmount + EB.GrossAmount;

                    foreach (EmployeeSalaryStructureDetail oItem in oDistinctSalaryHeads)
                    {
                        double nHeadAmount = oEmployeeSalaryStructureDetails.Where(x => x.SalaryHeadID == oItem.SalaryHeadID && x.ESSID == EB.ESSID).Sum(x=>x.Amount);
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nHeadAmount > 0 ? nHeadAmount : 0; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.OthersAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nOthersAmount = nOthersAmount + EB.OthersAmount;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.BonusAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nBonusAmount = nBonusAmount + EB.BonusAmount;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.Stamp; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nStamp = nStamp + EB.Stamp;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.EBGrossAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nEBGrossAmount = nEBGrossAmount + EB.EBGrossAmount;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.BankAccountNo ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.BankBranchName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                }

                #endregion

                #region Total

                cell = sheet.Cells[rowIndex, 16]; 
                cell.Value = "Total:"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 17]; cell.Value = nGrossAmount; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                int n = 18;
                foreach (EmployeeSalaryStructureDetail oItem in oDistinctSalaryHeads)
                {
                    double nHeadAmount = oEmployeeSalaryStructureDetails.Where(x => x.SalaryHeadID == oItem.SalaryHeadID).Sum(x => x.Amount);
                    cell = sheet.Cells[rowIndex, n++]; cell.Value = nHeadAmount; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                cell = sheet.Cells[rowIndex, n++]; cell.Value = nOthersAmount; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, n++]; cell.Value = nBonusAmount; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, n++]; cell.Value = nStamp; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, n++]; cell.Value = nEBGrossAmount; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, n++];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                
                cell = sheet.Cells[rowIndex, n++];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //signature

                rowIndex = rowIndex + 8;
                colIndex = 1;

                int Colspan = 0;

                Colspan = (nMaxColumn - nMaxColumn % 4) / 4-1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "_________________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "_______________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "__________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "_____________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                rowIndex = rowIndex + 1;
                colIndex = 0;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Prepared By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Checked By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Reviewed By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Approved By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EBList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void PrintBEListInXL_F2(string sParams, double ts)
        {
            _oEmployeeBonuss = new List<EmployeeBonus>();
            _oEmployeeBonuss = EBGets(sParams);

            int nCount = sParams.Split('~').Length;

            string _sBlockIDs = sParams.Split('~')[10];

            List<EmployeeBonus> oEmployeeBonuss = new List<EmployeeBonus>();
            _oEmployeeBonuss.ForEach(x => oEmployeeBonuss.Add(x));


            List<Company> oCompanys = new List<Company>();
            Company oCompany = new Company();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oCompany = oCompanys.First();
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 1;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("BONUS List");
                sheet.Name = "BONUS LIST";

                sheet.Column(2).Width = 6; //SL
                sheet.Column(3).Width = 20; //EmployeeCode
                sheet.Column(4).Width = 20; //EmployeeName
                sheet.Column(5).Width = 20; //DesignationName
                sheet.Column(6).Width = 20; //EmployeeType
                sheet.Column(7).Width = 20; //JOINING
                sheet.Column(8).Width = 20; //GROSS
                sheet.Column(9).Width = 20; //Declaration
                sheet.Column(10).Width = 20; //SERVICE YEAR
                sheet.Column(11).Width = 20; //PERCENT
                sheet.Column(12).Width = 20; //PAYABLE
                sheet.Column(13).Width = 20; //STAMP
                sheet.Column(14).Width = 20; //NET
                sheet.Column(15).Width = 20; //SIGN

                nMaxColumn = 15;

                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string BUIDs = string.Join(",", _oEmployeeBonuss.Where(x => x.EBID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
                if (BUIDs != "")
                { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }


                cell = sheet.Cells[rowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;

                EBReportHeaderXL(ref  sheet, ref  cell, ref  rowIndex, ref  colIndex, nMaxColumn, oBusinessUnits[0], _oEmployeeBonuss[0]);
                EBColumnSetup(ref  sheet, ref  cell, ref fill, ref border, ref rowIndex, ref colIndex, nMaxColumn, _oEmployeeBonuss[0], _sBlockIDs);

                _oEmployeeBonuss = _oEmployeeBonuss.OrderBy(x => x.BusinessUnitName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.BlockName).ToList();
                while (_oEmployeeBonuss.Count > 0)
                {
                    //var oResults = _oEmployeeBonuss.Where(x => x.BusinessUnitName == _oEmployeeBonuss[0].BusinessUnitName && x.LocationName == _oEmployeeBonuss[0].LocationName && x.DepartmentName == _oEmployeeBonuss[0].DepartmentName).ToList();

                    var oResults = new List<EmployeeBonus>();
                    if (!string.IsNullOrEmpty(_sBlockIDs))
                    {
                        oResults = _oEmployeeBonuss.Where(x => x.BusinessUnitName == _oEmployeeBonuss[0].BusinessUnitName && x.LocationName == _oEmployeeBonuss[0].LocationName && x.DepartmentName == _oEmployeeBonuss[0].DepartmentName && x.BlockName == _oEmployeeBonuss[0].BlockName).ToList();
                    }
                    else
                    {
                        oResults = _oEmployeeBonuss.Where(x => x.BusinessUnitName == _oEmployeeBonuss[0].BusinessUnitName && x.LocationName == _oEmployeeBonuss[0].LocationName && x.DepartmentName == _oEmployeeBonuss[0].DepartmentName).ToList();
                    }

                    List<BusinessUnit> oTempBusinessUnits = new List<BusinessUnit>();
                    BusinessUnit oBusinessUnit = new BusinessUnit();
                    oTempBusinessUnits = oBusinessUnits.Where(x => x.BusinessUnitID == _oEmployeeBonuss[0].BusinessUnitID).ToList();
                    oBusinessUnit = oTempBusinessUnits.Count > 0 ? oTempBusinessUnits[0] : new BusinessUnit();

                    //EBReportHeaderXL(ref  sheet, ref  cell, ref  rowIndex, ref  colIndex, nMaxColumn, oBusinessUnit, _oEmployeeBonuss[0]);
                    //EBColumnSetup(ref  sheet, ref  cell, ref fill, ref border, ref rowIndex, ref colIndex, nMaxColumn, _oEmployeeBonuss[0], _sBlockIDs);

                    colIndex = 2;


                    string sDeptBlock = "Department- " + _oEmployeeBonuss[0].DepartmentName;
                    if (!string.IsNullOrEmpty(_sBlockIDs))
                    {
                        if (!string.IsNullOrEmpty(_oEmployeeBonuss[0].BlockName))
                        {
                            sDeptBlock += ", Block- " + _oEmployeeBonuss[0].BlockName;
                        }
                    }

                    cell = sheet.Cells[rowIndex, 2];
                    cell.Value = "Unit Name : " + _oEmployeeBonuss[0].LocationName + ", Department Name : " + sDeptBlock; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell = sheet.Cells[rowIndex, nMaxColumn];
                    //cell.Value = sDeptBlock; cell.Style.Font.Bold = true;
                    //cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    rowIndex += 1;
                    colIndex = 2;

                    EBBodySetup(ref  sheet, ref  cell, ref fill, ref border, ref rowIndex, ref colIndex, nMaxColumn, oResults, oResults[0].DepartmentID, oCompany, _sBlockIDs, oResults[0].BlockName);

                    //_oEmployeeBonuss.RemoveAll(x => x.BusinessUnitName == oResults[0].BusinessUnitName && x.LocationName == oResults[0].LocationName && x.DepartmentName == oResults[0].DepartmentName);
                    if (!string.IsNullOrEmpty(_sBlockIDs))
                    {
                        _oEmployeeBonuss.RemoveAll(x => x.BusinessUnitName == oResults[0].BusinessUnitName && x.LocationName == oResults[0].LocationName && x.DepartmentName == oResults[0].DepartmentName && x.BlockName == oResults[0].BlockName);
                    }
                    else
                    {
                        _oEmployeeBonuss.RemoveAll(x => x.BusinessUnitName == oResults[0].BusinessUnitName && x.LocationName == oResults[0].LocationName && x.DepartmentName == oResults[0].DepartmentName);
                    }
                }

                #region Total

                cell = sheet.Cells[rowIndex, 2];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 3];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 5];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 6];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 7]; 
                cell.Value = "Grand Total:"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                double nGrossAmount = oEmployeeBonuss.Sum(x => x.GrossAmount);
                cell = sheet.Cells[rowIndex, 8]; cell.Value = nGrossAmount; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 9];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 10];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 11];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                double nBonusAmount = oEmployeeBonuss.Sum(x => x.BonusAmount);
                cell = sheet.Cells[rowIndex, 12]; cell.Value = nBonusAmount; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                double nStamp = oEmployeeBonuss.Count * 10;
                //For Golden
                int EmpCountForBelow500 = oEmployeeBonuss.Where(x => (x.BonusAmount <= 500)).ToList().Count();
                double TotalStampForGolden = EmpCountForBelow500 * 10;

               
                cell = sheet.Cells[rowIndex, 13]; cell.Value = (oCompany.BaseAddress == "golden") ? (nStamp - TotalStampForGolden) : nStamp; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0;(#,##0)"; 
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 14]; cell.Value = (oCompany.BaseAddress == "golden") ? (nBonusAmount - (nStamp - TotalStampForGolden)) : (nBonusAmount - nStamp); cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0;(#,##0)"; 
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //signature

                rowIndex = rowIndex + 8;
                colIndex = 1;

                //int Colspan = 0;

                //Colspan = (nMaxColumn - nMaxColumn % 4) / 4 - 1;

                //cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "_________________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "_______________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "__________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "_____________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //rowIndex = rowIndex + 1;
                //colIndex = 0;

                //cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Prepared By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Checked By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Reviewed By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Approved By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EBList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }


        public void PrintEBExcel_F3(string sParams, double ts)
        {
            _oEmployeeBonus = new EmployeeBonus();
            _oEmployeeBonus.EmployeeBonuss = EBGets(sParams);

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployeeBonus.EmployeeBonuss.Where(x => x.EBID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "")
            { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }
            _oEmployeeBonus.BusinessUnits = oBusinessUnits;

            string sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


            List<Company> oCompanys = new List<Company>();
            Company oCompany = new Company();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oCompany = oCompanys.First();
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 1;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("BONUS List");
                sheet.Name = "BONUS LIST";

                sheet.Column(2).Width = 6; //
                sheet.Column(3).Width = 20; //
                sheet.Column(4).Width = 30; //
                sheet.Column(5).Width = 30; //
                sheet.Column(6).Width = 20; //
                sheet.Column(7).Width = 20; //
                sheet.Column(8).Width = 20; //
                sheet.Column(9).Width = 20; //
                sheet.Column(10).Width = 20; //
                int i = 10;
                if (_bPercent)
                {
                    sheet.Column(i++).Width = 20; //
                }
                sheet.Column(i++).Width = 20; //
                sheet.Column(i++).Width = 20; //

                nMaxColumn = i;

                //List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                //string BUIDs = string.Join(",", _oEmployeeBonuss.Where(x => x.EBID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
                //if (BUIDs != "")
                //{ oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }



                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2]; cell.Value = _oEmployeeBonus.BusinessUnits[0].Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2]; cell.Value = _oEmployeeBonus.BusinessUnits[0].Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2]; cell.Value = _oEmployeeBonus.EmployeeBonuss[0].Note; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rowIndex = rowIndex + 1;

                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "CODE"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "NAME"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DESIGNATION"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "JOINING DATE"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SERVICE YEAR"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "GROSS"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BASIC"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                if (_bPercent)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "%"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BONUS AMOUNT"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SIGNATURE"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                _oEmployeeBonuss = _oEmployeeBonus.EmployeeBonuss.OrderBy(x=>x.DepartmentName).ThenBy(x=>x.EmployeeCode).ToList();
                if (_oEmployeeBonuss.Count <= 0 )
                {
                    string masg = "Nothing to print";
                    cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn];cell.Merge = true; cell.Value = masg; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                else
                {
                    var grpEmpBonus = _oEmployeeBonuss.GroupBy(x => new { x.DepartmentID, x.DepartmentName }, (key, grp) => new
                    {
                        DepartmentID = key.DepartmentID,
                        DepartmentName = key.DepartmentName,
                        EmpBonusCount = grp.Count(),
                        EmpBonusList = grp,
                    

                    }).ToList();
                    foreach (var oItem in grpEmpBonus)
                    {
                        double TotalBonus = 0.0;
                        //Print Department

                        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn];cell.Merge = true; cell.Value = "Department : " + oItem.DepartmentName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        rowIndex += 1;
                    
                        int nCount = 0;
                        foreach (EmployeeBonus data in oItem.EmpBonusList.OrderBy(x=>x.DepartmentName).ThenBy(x => x.EmployeeCode))
                        {
                            nCount++;
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCount; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.EmployeeCode; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.EmployeeName; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.DesignationName; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.JoiningDateInString; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            DateTime DeclarationDate = data.BonusDeclarationDate.AddDays(1);
                            DateDifference oService = new DateDifference(data.JoiningDate, DeclarationDate);

                            double nTotalDayCount = (DeclarationDate - data.JoiningDate).TotalDays;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (nTotalDayCount < 365) ? oService.ToString() + "\n(" + nTotalDayCount.ToString() + "d)" : oService.ToString(); cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; 

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.GrossAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.BasicAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            if (_bPercent)
                            {
                                double Percent = data.BonusAmount / data.GrossAmount * 100;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Percent; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.BonusAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            TotalBonus += data.BonusAmount;
                            rowIndex++;
                        }
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Value = "TOTAL"; cell.Style.Font.Bold = true; cell.Merge = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, (_bPercent) ? 11 : 10]; cell.Value = TotalBonus; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, (_bPercent) ? 12 : 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        rowIndex++;
                    }
                }


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EBList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void PrintEBExcel_F5(string sParams, double ts)
        {
            _oEmployeeBonus = new EmployeeBonus();
            _oEmployeeBonus.EmployeeBonuss = EBGets(sParams);

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployeeBonus.EmployeeBonuss.Where(x => x.EBID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "")
            { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }
            _oEmployeeBonus.BusinessUnits = oBusinessUnits;

            string sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


            List<Company> oCompanys = new List<Company>();
            Company oCompany = new Company();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oCompany = oCompanys.First();
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 1;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("BONUS List");
                sheet.Name = "BONUS LIST";

                sheet.Column(2).Width = 6; //
                sheet.Column(3).Width = 20; //
                sheet.Column(4).Width = 30; //
                sheet.Column(5).Width = 30; //
                sheet.Column(6).Width = 20; //
                sheet.Column(7).Width = 20; //
                sheet.Column(8).Width = 20; //
                sheet.Column(9).Width = 20; //
                sheet.Column(10).Width = 20; //
                int i = 10;
                if (_bPercent)
                {
                    sheet.Column(i++).Width = 20; //
                }
                sheet.Column(i++).Width = 20; //
                sheet.Column(i++).Width = 20; //
                sheet.Column(i++).Width = 20; //
                sheet.Column(i++).Width = 20; //

                nMaxColumn = i;

                
                rowIndex = rowIndex + 1;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = _oEmployeeBonus.BusinessUnits[0].Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2]; cell.Value = _oEmployeeBonus.BusinessUnits[0].Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2]; cell.Value = _oEmployeeBonus.EmployeeBonuss[0].Note; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rowIndex = rowIndex + 1;

                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "CODE"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "NAME"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DESIGNATION"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "JOINING DATE"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SERVICE YEAR"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "GROSS"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BASIC"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                if (_bPercent)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "%"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BONUS AMOUNT"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BANK AMOUNT"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "CASH AMOUNT"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SIGNATURE"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                _oEmployeeBonuss = _oEmployeeBonus.EmployeeBonuss.OrderBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
                if (_oEmployeeBonuss.Count <= 0)
                {
                    string masg = "Nothing to print";
                    cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = masg; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                else
                {
                    var grpEmpBonus = _oEmployeeBonuss.GroupBy(x => new { x.DepartmentID, x.DepartmentName }, (key, grp) => new
                    {
                        DepartmentID = key.DepartmentID,
                        DepartmentName = key.DepartmentName,
                        EmpBonusCount = grp.Count(),
                        EmpBonusList = grp,


                    }).ToList();
                    double GrandTotalBonus = 0.0;
                    double GrandTotalBonusBankAmount = 0.0;
                    double GrandTotalBonusCashAmount = 0.0;
                    foreach (var oItem in grpEmpBonus)
                    {
                        double TotalBonus = 0.0;
                        double TotalBonusBankAmount = 0.0;
                        double TotalBonusCashAmount = 0.0;
                        //Print Department

                        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "Department : " + oItem.DepartmentName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        rowIndex += 1;

                        int nCount = 0;
                        foreach (EmployeeBonus data in oItem.EmpBonusList.OrderBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode))
                        {
                            nCount++;
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCount; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.EmployeeCode; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.EmployeeName; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.DesignationName; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.JoiningDateInString; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            DateTime DeclarationDate = data.BonusDeclarationDate.AddDays(1);
                            DateDifference oService = new DateDifference(data.JoiningDate, DeclarationDate);

                            double nTotalDayCount = (DeclarationDate - data.JoiningDate).TotalDays;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (nTotalDayCount < 365) ? oService.ToString() + "\n(" + nTotalDayCount.ToString() + "d)" : oService.ToString(); cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.GrossAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.BasicAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            if (_bPercent)
                            {
                                double Percent = data.BonusAmount / data.GrossAmount * 100;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Percent; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.BonusAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            if (data.BankAccountNo != "")
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.BonusBankAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.BonusCashAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                TotalBonusBankAmount += data.BonusBankAmount;
                                TotalBonusCashAmount += data.BonusCashAmount;

                                GrandTotalBonusBankAmount += data.BonusBankAmount;
                                GrandTotalBonusCashAmount += data.BonusCashAmount;
                            }
                            else
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0.00; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (data.BonusCashAmount + data.BonusBankAmount); cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                TotalBonusBankAmount += 0;
                                TotalBonusCashAmount += (data.BonusCashAmount + data.BonusBankAmount);

                                GrandTotalBonusBankAmount += 0;
                                GrandTotalBonusCashAmount += (data.BonusCashAmount + data.BonusBankAmount);
                            }
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            TotalBonus += data.BonusAmount;
                            GrandTotalBonus += data.BonusAmount;               
                            rowIndex++;
                        }

                        #region Sub Total
                        colIndex = (_bPercent) ? 10 : 9;
                        cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex++]; cell.Value = "SUB TOTAL"; cell.Style.Font.Bold = true; cell.Merge = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = TotalBonus; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = TotalBonusBankAmount; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = TotalBonusCashAmount; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rowIndex++;
                        #endregion
                    }

                    #region Grand Total
                    colIndex = (_bPercent) ? 10 : 9;
                    cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex++]; cell.Value = "GRAND TOTAL"; cell.Style.Font.Bold = true; cell.Merge = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = GrandTotalBonus; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = GrandTotalBonusBankAmount; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = GrandTotalBonusCashAmount; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rowIndex++;
                    #endregion
                }


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EBList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void EBReportHeaderXL(ref ExcelWorksheet sheet, ref ExcelRange cell, ref int rowIndex, ref int colIndex, int nMaxColumn, BusinessUnit oBusinessUnit, EmployeeBonus oEmployeeBonus)
        {
            rowIndex = rowIndex + 1;
         
            cell = sheet.Cells[rowIndex, 2]; cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rowIndex = rowIndex + 1;

            cell = sheet.Cells[rowIndex, 2]; cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rowIndex = rowIndex + 1;

            cell = sheet.Cells[rowIndex, 2]; cell.Value = oEmployeeBonus.Note; cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            rowIndex = rowIndex + 1;
        }

        public void EBColumnSetup(ref ExcelWorksheet sheet, ref ExcelRange cell, ref ExcelFill fill, ref OfficeOpenXml.Style.Border border, ref int rowIndex, ref int colIndex, int nMaxColumn, EmployeeBonus oEmployeeBonus, string _sBlockIDs)
        {
            //colIndex = 2;

            //cell = sheet.Cells[rowIndex, 2];
            //cell.Value = "Unit-" + oEmployeeBonus.LocationName; cell.Style.Font.Bold = true;
            //cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            //string sDeptBlock = "Department- " + oEmployeeBonus.DepartmentName;
            //if (!string.IsNullOrEmpty(_sBlockIDs))
            //{
            //    if (!string.IsNullOrEmpty(oEmployeeBonus.BlockName))
            //    {
            //        sDeptBlock += ", Block- " + oEmployeeBonus.BlockName;
            //    }
            //}

            //cell = sheet.Cells[rowIndex, nMaxColumn];
            //cell.Value = sDeptBlock; cell.Style.Font.Bold = true;
            //cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            rowIndex++;

            colIndex = 2;
            cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Code"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Name"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Designation"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Employee Type"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Joining"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Salary"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Declaration Date"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex = colIndex + 3]; cell.Merge = true; cell.Value = oEmployeeBonus.SalaryHeadName + " " + oEmployeeBonus.Note +" "+ oEmployeeBonus.Year; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Net Payable"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Recv Sign"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            rowIndex++;

            colIndex = 10;
            cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true;  cell.Value = "Service Year"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Value = "%"; cell.Merge = true;  cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true;  cell.Value = "Payable"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Value = "Stamp"; cell.Merge = true; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            rowIndex++;
        }

        public void EBBodySetup(ref ExcelWorksheet sheet, ref ExcelRange cell, ref ExcelFill fill, ref OfficeOpenXml.Style.Border border, ref int rowIndex, ref int colIndex, int nMaxColumn, List<EmployeeBonus> oEmployeeBonuss, int nDepartmentID, Company oCompany, string _sBlockIDs, string blockName)
        {
            int nSL = 0;

            foreach (EmployeeBonus EB in oEmployeeBonuss)
            {
                nSL++;
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.EmployeeCode; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.EmployeeName; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.DesignationName; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.EmployeeTypeName; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.JoiningDateInString; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.GrossAmount; cell.Style.Numberformat.Format = "#,##0"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.BonusDeclarationDateInString; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                DateTime DeclarationDate = EB.BonusDeclarationDate.AddDays(1);
                DateDifference ServiceYear = new DateDifference(EB.JoiningDate, DeclarationDate);

                double nTotalDayCount = (DeclarationDate - EB.JoiningDate).TotalDays;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (nTotalDayCount < 365) ? nTotalDayCount.ToString() + "d" : ServiceYear.ToString(); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                double Percent = EB.BonusAmount / EB.GrossAmount * 100;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Percent; cell.Style.Numberformat.Format = "#,##0"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.BonusAmount; cell.Style.Numberformat.Format = "#,##0"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //For BaseAddress Golden<=500
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oCompany.BaseAddress == "golden" && (EB.BonusAmount <= 500)) ? 0.00 : 10.00; cell.Style.Numberformat.Format = "#,##0"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oCompany.BaseAddress == "golden" && (EB.BonusAmount <= 500)) ? EB.BonusAmount : EB.BonusAmount - 10; cell.Style.Numberformat.Format = "#,##0"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
            }
            cell = sheet.Cells[rowIndex, 2];
            cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 3];
            cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 4];
            cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 5];
            cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 6];
            cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 7];
            cell.Value = "Total:"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            //double nGrossAmount = oEmployeeBonuss.Sum(x => x.GrossAmount);
            double nGrossAmount;
            if (!string.IsNullOrEmpty(_sBlockIDs))
            {
                nGrossAmount = oEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID && (x.BlockName == blockName)).Sum(x => x.GrossAmount);
            }
            else
            {
                nGrossAmount = oEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID).Sum(x => x.GrossAmount);
            }

            cell = sheet.Cells[rowIndex, 8]; cell.Value = nGrossAmount; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 9];
            cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 10];
            cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 11]; 
            cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            //double nBonusAmount = oEmployeeBonuss.Sum(x => x.BonusAmount);
            double nBonusAmount;
            if (!string.IsNullOrEmpty(_sBlockIDs))
            {
                nBonusAmount = oEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID && (x.BlockName == blockName)).Sum(x => x.BonusAmount);
            }
            else
            {
                nBonusAmount = oEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID).Sum(x => x.BonusAmount);
            }



            cell = sheet.Cells[rowIndex, 12]; cell.Value = nBonusAmount; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            //double nStamp = oEmployeeBonuss.Count * 10;
            double nStamp;
            if (!string.IsNullOrEmpty(_sBlockIDs))
            {
                nStamp = oEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID && (x.BlockName == blockName)).ToList().Count * 10;
            }
            else
            {
                nStamp = oEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID).ToList().Count * 10;
            }


            //For Golden
            //int EmpCountForBelow500 = oEmployeeBonuss.Where(x => (x.BonusAmount <= 500)).ToList().Count();
            int EmpCountForBelow500;
            if (!string.IsNullOrEmpty(_sBlockIDs))
            {
                EmpCountForBelow500 = oEmployeeBonuss.Where(x => (x.DepartmentID == nDepartmentID && (x.BlockName == blockName)) && (x.BonusAmount <= 500)).ToList().Count();
            }
            else
            {
                EmpCountForBelow500 = oEmployeeBonuss.Where(x => (x.DepartmentID == nDepartmentID) && (x.BonusAmount <= 500)).ToList().Count();
            }


            double TotalStampForGolden = EmpCountForBelow500 * 10;


            cell = sheet.Cells[rowIndex, 13]; cell.Value = (oCompany.BaseAddress == "golden") ? (nStamp - TotalStampForGolden) : nStamp; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 14]; cell.Value = (oCompany.BaseAddress == "golden") ? (nBonusAmount - (nStamp - TotalStampForGolden)) : (nBonusAmount - nStamp); cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            rowIndex++;
            colIndex = 2;

        }

        public void PrintEBPaySlipExcel(string sParams, double ts)
        {
            List<EmployeeBonus> oEmployeeBonuss = new List<EmployeeBonus>();
            oEmployeeBonuss = EBGets(sParams);
            PrintPaySlipXL(oEmployeeBonuss);
        }


        public void PrintEBSummaryExcel(string sParams, double ts)
        {
            List<EmployeeBonus> oEmployeeBonuss = new List<EmployeeBonus>();
            oEmployeeBonuss = EBGets(sParams);
            PrintSummaryExcel(oEmployeeBonuss);
        }
        private void PrintSummaryExcel(List<EmployeeBonus> oEmployeeBonuss)
        {
            Company oCompany = new Company();
            int nMaxColumn = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Employee Bonus Summary");
                sheet.Name = "Employee Bonus Summary";

                sheet.Column(1).Width = 25;//SL
                sheet.Column(2).Width = 25;//Dept Name
                sheet.Column(3).Width = 20;//ManPower
                sheet.Column(4).Width = 20;//Gross
                sheet.Column(5).Width = 20;//Payable
                sheet.Column(6).Width = 20;//Stamp
                sheet.Column(7).Width = 20;//NetPayable

                nMaxColumn = 5;

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Table Body

                int rowIndex = 1;
                int nPS = 0;
                int colIndex = 1;


                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string BUIDs = string.Join(",", oEmployeeBonuss.Select(p => p.BusinessUnitID).Distinct().ToList());
                if (BUIDs != "") { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }


                cell = sheet.Cells[rowIndex, colIndex]; cell.Merge = true;
                cell.Value = oBusinessUnits.Count == 1 ? oBusinessUnits[0].Name : oCompany.Name; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 30; cell.Style.Font.SetFromFont(new Font("Century Gothic", 22));
                //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 22; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                rowIndex = rowIndex + 1;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex]; cell.Merge = true;
                cell.Value = oBusinessUnits.Count == 1 ? oBusinessUnits[0].Address : oCompany.Address; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 16;
                //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colIndex = 1;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, colIndex]; cell.Merge = true;
                cell.Value = oEmployeeBonuss[0].Note; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 16;
                //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colIndex = 1;
                rowIndex = rowIndex + 1;


                var grpEBSummary = oEmployeeBonuss.GroupBy(x => new { x.BusinessUnitID, x.BusinessUnitName, x.LocationID }, (key, grp) => new
                {
                    BusinessUnitID = key.BusinessUnitID,
                    BusinessUnitName = key.BusinessUnitName,
                    LocationID = key.LocationID,
                    LocationName = grp.First().LocationName,
                    EBSCount = grp.Count(),
                    EBSList = grp,
                    DepartmentName = grp.First().DepartmentName,

                }).OrderBy(x => x.BusinessUnitName).ToList();


                foreach (var oItem in grpEBSummary)
                {
                    colIndex = 1;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BU"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BusinessUnitName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    rowIndex += 1;
                    colIndex = 1;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Location"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LocationName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    
                    rowIndex += 2;
                    colIndex = 1;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Dept Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ManPower"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Salary"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Payable"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Stamp"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Payable"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                    rowIndex += 1;
                    colIndex = 1;

                    var List = oItem.EBSList.GroupBy(x => new { x.DepartmentID }, (key, grp) => new EmployeeBonus
                    {
                        DepartmentName = grp.First().DepartmentName,
                        BonusAmount = grp.Sum(x => x.BonusAmount),
                        GrossAmount = grp.Sum(x => x.GrossAmount),

                        ManPower = grp.Count()

                    }).OrderBy(x => x.DepartmentName).ToList();
                    nPS = 1;
                    double total = 0.0;
                    double nManPower = 0.0;
                    double nGross = 0.0;
                    int totalStamp = 0;
                    double totalNetPayable = 0.0;
                    foreach (var data in List)
                    {
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nPS++;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = data.DepartmentName;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = data.ManPower;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = data.GrossAmount; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = data.BonusAmount; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        int nStamp = data.ManPower * 10;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nStamp; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        double nNetPayable = (data.BonusAmount - nStamp);
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nNetPayable; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        rowIndex += 1;
                        nManPower += data.ManPower;
                        nGross += data.GrossAmount;
                        total += data.BonusAmount;
                        totalStamp += nStamp;
                        totalNetPayable += nNetPayable;
                    }
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nManPower; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nGross; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = total; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = totalStamp; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = totalNetPayable; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    rowIndex += 3;

                }

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "Group Summary Total"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                
                rowIndex += 1;
                colIndex = 1;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BUName"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ManPower"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Salary"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Payable"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Stamp"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Payable"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;
                colIndex = 1;

                var grpEBGrandSummary = oEmployeeBonuss.GroupBy(x => new { x.BusinessUnitID, x.BusinessUnitName }, (key, grp) => new
                {
                    BusinessUnitID = key.BusinessUnitID,
                    BusinessUnitName = key.BusinessUnitName,
                    LocationName = grp.First().LocationName,
                    EBSCount = grp.Count(),
                    EBSList = grp,
                    DepartmentName = grp.First().DepartmentName,
                    BonusAmount = grp.Sum(x => x.BonusAmount),
                    GrossAmount = grp.Sum(x => x.GrossAmount),
                    ManPower = grp.Count()


                }).OrderBy(x => x.BusinessUnitName).ToList();

                foreach (var oItem in grpEBGrandSummary)
                {
                    //var List = oItem.EBSList.GroupBy(x => new { x.LocationName }, (key, grp) => new EmployeeBonus
                    //{
                    //    LocationName = grp.First().LocationName,
                    //    BonusAmount = grp.Sum(x => x.BonusAmount),
                    //    GrossAmount = grp.Sum(x => x.GrossAmount),

                    //    ManPower = grp.Count()

                    //}).OrderBy(x => x.LocationName).ToList();

                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.BusinessUnitName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;


                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.ManPower; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.GrossAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.BonusAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    int nStamp = oItem.ManPower * 10;
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nStamp; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    double nNetPayable = (oItem.BonusAmount - nStamp);
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nNetPayable; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;


                    rowIndex += 1;
                }


                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EmployeeBonusSummary.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }


        private void PrintPaySlipXL(List<EmployeeBonus> oEmployeeBonuss)
        {
            Company oCompany = new Company();
            int nMaxColumn = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Pay Slip");
                sheet.Name = "Pay Slip";

                sheet.Column(1).Width = 28;//Caption
                sheet.Column(2).Width = 20;//Value
                sheet.Column(3).Width = 32;//Caption
                sheet.Column(4).Width = 20;//Value

                nMaxColumn = 4;

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Table Body

                int rowIndex = 1;
                int nPS = 0;
                foreach (EmployeeBonus oItem in oEmployeeBonuss)
                {
                    #region Report Header
                    int colIndex = 1;
                    if (oCompany.CompanyLogo != null)
                    {
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        sheet.Row(rowIndex).Height = 30;
                        //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style= border.Right.Style = ExcelBorderStyle.None;

                        ExcelPicture excelImage = null;
                        excelImage = sheet.Drawings.AddPicture(oItem.EBID.ToString() + "A", oCompany.CompanyLogo);
                        excelImage.From.Column = 0;
                        excelImage.From.Row = (rowIndex - 1);
                        excelImage.SetSize(85, 40);
                        excelImage.From.ColumnOff = this.Pixel2MTU(2);
                        excelImage.From.RowOff = this.Pixel2MTU(2);
                        excelImage.SetPosition(rowIndex - 1, 1, colIndex - 1, 118);
                    }
                    else
                    {
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //border = cell.Style.Border; border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;

                    }
                    colIndex++;

                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 30; cell.Style.Font.SetFromFont(new Font("Century Gothic", 22));
                    //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 22; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    rowIndex = rowIndex + 1;
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 16;
                    //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    colIndex = 1;
                    rowIndex = rowIndex + 1;

                    //cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true;
                    //cell.Value = ""; cell.Style.Font.Bold = true;
                    //border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.Thin;
                    //border = cell.Style.Border; border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.Thin;
                    //cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //rowIndex = rowIndex + 1;

                    #endregion

                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                    cell.Value = "Pay Slip No - " + oItem.EBID.ToString(); cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 13;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border;
                    border.Left.Style = border.Top.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                    cell.Value = oItem.Note; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 12;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border;
                    border.Left.Style = border.Top.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    int nfontSize = 10;
                    double nRH = 13;
                    
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name(নাম):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code No.(কোড নং.):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Div(বিভাগ):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation(পদবী):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Sec(সেকশন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Joining Date(যোগদানের তাং.):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.JoiningDateInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Confirmation Date (স্থায়ীকরনের তাং):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ConfirmationDateInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bonus Annou. Date (বোনাস ঘোষণার তাং):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BonusDeclarationDateInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Service Year(চাকুরীর বয়স):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    DateDifference dateDifference = new DateDifference(oItem.BonusDeclarationDate, oItem.JoiningDate);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = dateDifference.ToString(); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Account no.(অ্যাকাউন্ট নং.):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BankAccountNo; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Wages(মোট বেতন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(ICS.Core.Utility.Global.MillionFormat(oItem.GrossAmount)); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Basic(মূল বেতন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =  NumberInBan(ICS.Core.Utility.Global.MillionFormat(oItem.BasicAmount)); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Earning(মোট উপার্জন):"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(ICS.Core.Utility.Global.MillionFormat(oItem.BonusAmount)); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Deduction	(মোট কর্তন)"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(ICS.Core.Utility.Global.MillionFormat(oItem.Stamp)); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Payable	(নেট প্রদেয়):"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(ICS.Core.Utility.Global.MillionFormat(oItem.EBGrossAmount)); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "In Word:" + @ICS.Core.Utility.Global.TakaWords(oItem.EBGrossAmount); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    if (GetImage(oCompany.AuthorizedSignature) != null)
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        sheet.Row(rowIndex).Height = 27; border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;
                        border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        ExcelPicture excelImage = null;
                        excelImage = sheet.Drawings.AddPicture(oItem.EBID.ToString() + "B", GetImage(oCompany.AuthorizedSignature));
                        excelImage.From.Column = 0;
                        excelImage.From.Row = (rowIndex - 1);
                        excelImage.SetSize(48, 42);
                        excelImage.From.ColumnOff = this.Pixel2MTU(2);
                        excelImage.From.RowOff = this.Pixel2MTU(2);
                    }
                    else
                    {
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;
                        border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;
                    }

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Top.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = 18;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "কর্তৃপক্ষের স্বাক্ষর  "; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Right.Style = border.Top.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Left.Style = border.Bottom.Style = border.Right.Style = border.Top.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Left.Style = border.Bottom.Style = border.Right.Style = border.Top.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "কর্মীর স্বাক্ষর "; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.None;

                    rowIndex = rowIndex + 1;
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++, rowIndex, nMaxColumn]; cell.Merge = true; cell.Style.Font.Size = 8; sheet.Row(rowIndex).Height = 10;
                    cell.Value = "বিঃ দ্রঃ- পে-স্লিপের কোন তথ্য ভুল হলে, পে-স্লিপ প্রপ্তির পরবর্তী ১০ (দশ) কর্ম দিবসের মধ্যে কতৃপক্ষকে অবগত করার জন্য অনুরোধ করা গেল।অন্যথায় উল্লেখিত সময়ের পরে কোন প্রকার অভিযোগ গ্রহণযোগ্য হবেনা।"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.None;

                    //nPS++;
                    //if (nPS % 3 != 0)
                    //{
                        rowIndex = rowIndex + 2;
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++, ++rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = false;
                        //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); 
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    //}
                    //else if (nPS % 3 == 0)
                    //{
                    //    rowIndex = rowIndex + 1;
                    //    colIndex = 1;
                    //    cell = sheet.Cells[rowIndex, colIndex++, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = false;
                    //    sheet.Row(rowIndex).Height = 30;
                    //    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); 
                    //    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    //}

                    rowIndex = rowIndex + 1;

                }

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PAYSLIP.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }



        public void PrintEBExcelF4(string sParams, double ts)
        {
            int BusinessUnitID = 0;
            _oEmployeeBonus = new EmployeeBonus();
            _oEmployeeBonus.EmployeeBonuss = EBGets(sParams);

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployeeBonus.EmployeeBonuss.Where(x => x.EBID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "")
            { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }
            _oEmployeeBonus.BusinessUnits = oBusinessUnits;

            string sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<EmployeeBonus> _oEmployeeBonuss = new List<EmployeeBonus>();
            _oEmployeeBonuss = _oEmployeeBonus.EmployeeBonuss;



            List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
            _oBusinessUnits = _oEmployeeBonus.BusinessUnits;

            if (_oEmployeeBonuss.Count() > 0)
            {
                BusinessUnitID = _oEmployeeBonuss[0].BusinessUnitID;
            }
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnits = _oBusinessUnits.Where(x => x.BusinessUnitID == BusinessUnitID).ToList();
            if (oBusinessUnits.Count > 0) { oBusinessUnit = oBusinessUnits[0]; }


            Company oCompany = new Company();
            int nMaxColumn = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            int rowIndex = 1;
            int colIndex = 1;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Employee Bonus");
                sheet.Name = "Employee Bonus";

                sheet.Column(1).Width = 10;
                sheet.Column(2).Width = 20;
                sheet.Column(3).Width = 25;
                sheet.Column(4).Width = 22;
                sheet.Column(5).Width = 18;
                sheet.Column(6).Width = 18;
                sheet.Column(7).Width = 18;
                sheet.Column(8).Width = 18;
                sheet.Column(9).Width = 20;
                sheet.Column(10).Width = 17;
                sheet.Column(11).Width = 17;
                sheet.Column(12).Width = 17;
                sheet.Column(13).Width = 22;

                nMaxColumn = 13;

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);


                cell = sheet.Cells[rowIndex, nMaxColumn / 2]; cell.Merge = true; cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;
                cell = sheet.Cells[rowIndex, nMaxColumn / 2]; cell.Merge = true; cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;

                cell = sheet.Cells[rowIndex, nMaxColumn / 2]; cell.Merge = true; cell.Value = _oEmployeeBonuss[0].Note; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;


                if (_oEmployeeBonuss.Count <= 0)
                {
                    string masg = "Nothing to print";
                    //if (_oEmployeeBonuss[0].ErrorMessage != "") { masg = ""; masg = _oEmployeeBonuss[0].ErrorMessage; }

                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = masg; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    rowIndex += 1;
                }
                else
                {
                    var grpEmpBonus = _oEmployeeBonuss.GroupBy(x => new { x.DepartmentID, x.DepartmentName }, (key, grp) => new
                    {
                        DepartmentID = key.DepartmentID,
                        DepartmentName = key.DepartmentName,
                        EmpBonusCount = grp.Count(),
                        EmpBonusList = grp,


                    }).ToList();


                    foreach (var oItem in grpEmpBonus)
                    {
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Joining Date"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Grade"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Salary"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Basic Salary"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Length of Service"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bonus TK."; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Stamp"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Payable"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        rowIndex += 1;


                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 1]; cell.Merge = true; cell.Value = "Department : " + oItem.DepartmentName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        rowIndex = rowIndex + 1;

                        double TotalBonus = 0.0;
                        int nCount = 0;
                        int nEmployee = 0;
                        double nGrossSalary = 0.0;
                        double nBasicSalary = 0.0;
                        double nBonus = 0.0;
                        double nStamp = 0.0;
                        double nNetPayable = 0.0;

                        foreach (EmployeeBonus data in oItem.EmpBonusList.OrderBy(x => x.EmployeeCode))
                        {
                            nCount++;
                            colIndex = 1;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.EmployeeCode; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.EmployeeName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.DesignationName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.JoiningDateInString; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.EmployeeTypeName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;
                            
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.CompGrossAmount; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;
                           
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.CompBasicAmount; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                            DateTime DeclarationDate = data.BonusDeclarationDate.AddDays(1);
                            DateDifference oService = new DateDifference(data.JoiningDate, DeclarationDate);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oService.ToString(); cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.CompBonusAmount; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.Stamp; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.CompBonusAmount - data.Stamp; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;


                            rowIndex += 1;


                            TotalBonus += data.BonusAmount;
                            nEmployee++;
                            nGrossSalary += data.CompGrossAmount;
                            nBasicSalary += data.CompBasicAmount;
                            nBonus += data.CompBonusAmount;
                            nStamp += data.Stamp;
                            nNetPayable += (data.CompBonusAmount - data.Stamp);
                        }
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 5]; cell.Merge = true; cell.Value = "Total Employee : " + nEmployee; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;
                        colIndex += 6;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nGrossSalary; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBasicSalary; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (Math.Round(nBonus)); cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (Math.Round(nStamp)); cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (Math.Round(nNetPayable)); cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        rowIndex += 2;


                    }
                }

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EmployeeBonus.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

        }


        #endregion XL

        #region PDF 
        public ActionResult PrintEBListInPDF_F2(string sParams, double ts)
        {
            _oEmployeeBonus = new EmployeeBonus();
            _oEmployeeBonus.EmployeeBonuss = EBGets(sParams);

            int nCount = sParams.Split('~').Length;

            string sBlockIDs = sParams.Split('~')[10];


            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployeeBonus.EmployeeBonuss.Where(x => x.EBID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "")
            { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }
            _oEmployeeBonus.BusinessUnits = oBusinessUnits;

            string sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


            List<Company> oCompanys = new List<Company>();
            Company oCompany = new Company();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oCompany = oCompanys.First();
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptEmployeeBonus_F2 oReport = new rptEmployeeBonus_F2();
            byte[] abytes = oReport.PrepareReport(_oEmployeeBonus, oSalarySheetSignature, oCompany, sBlockIDs);
            return File(abytes, "application/pdf");

        }



        public ActionResult PrintPDFF3(string sParams, double ts)
        {
            _oEmployeeBonus = new EmployeeBonus();
            _oEmployeeBonus.EmployeeBonuss = EBGets(sParams);

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployeeBonus.EmployeeBonuss.Where(x => x.EBID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "")
            { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }
            _oEmployeeBonus.BusinessUnits = oBusinessUnits;

            string sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptEmployeeBonus_F3 oReport = new rptEmployeeBonus_F3();
            byte[] abytes = oReport.PrepareReport(_oEmployeeBonus, oSalarySheetSignature, _bPercent);
            return File(abytes, "application/pdf");

        }

        public ActionResult PrintPDFF5(string sParams, double ts)
        {
            _oEmployeeBonus = new EmployeeBonus();
            _oEmployeeBonus.EmployeeBonuss = EBGets(sParams);

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployeeBonus.EmployeeBonuss.Where(x => x.EBID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "")
            { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }
            _oEmployeeBonus.BusinessUnits = oBusinessUnits;

            string sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptEmployeeBonus_F3 oReport = new rptEmployeeBonus_F3();
            byte[] abytes = oReport.PrepareReportF5(_oEmployeeBonus, oSalarySheetSignature, _bPercent);
            return File(abytes, "application/pdf");
        }


        public ActionResult PrintPDFF4(string sParams, double ts)
        {
            _oEmployeeBonus = new EmployeeBonus();
            _oEmployeeBonus.EmployeeBonuss = EBGets(sParams);

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployeeBonus.EmployeeBonuss.Where(x => x.EBID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "")
            { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }
            _oEmployeeBonus.BusinessUnits = oBusinessUnits;

            string sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptEmployeeBonus_F4 oReport = new rptEmployeeBonus_F4();
            byte[] abytes = oReport.PrepareReport(_oEmployeeBonus, oSalarySheetSignature);
            return File(abytes, "application/pdf");

        }

        public List<EmployeeBonus> EBGets(string sParams)
        {
            List<EmployeeBonus> oEmployeeBonuss = new List<EmployeeBonus>();

            int nCount = sParams.Split('~').Length;

            string sEmployeeBonusIDs = sParams.Split('~')[0];
            int ProcessMonth = Convert.ToInt32(sParams.Split('~')[1]);
            int ProcessYear = Convert.ToInt32(sParams.Split('~')[2]);
            string sEmployeeIDs = sParams.Split('~')[3];
            int nLocationID = Convert.ToInt32(sParams.Split('~')[4]);
            string sDepartmentIds = sParams.Split('~')[5];
            string sDesignationIds = sParams.Split('~')[6];
            int nCategory = Convert.ToInt32(sParams.Split('~')[7]);
            double nStartSalaryRange = Convert.ToDouble(sParams.Split('~')[8]);
            double nEndSalaryRange = Convert.ToDouble(sParams.Split('~')[9]);
            string sBlockIDs = sParams.Split('~')[10];
            string sGroupIDs = sParams.Split('~')[11];
            string sBusinessUnitIDs = sParams.Split('~')[12];

            bool bPercent = false;
            if (nCount >= 14) 
            { 
                bPercent = Convert.ToBoolean(sParams.Split('~')[13]);
                _bPercent = bPercent;
            }

            string sSql = "";
            sSql = "SELECT * FROM View_EmployeeBonus WHERE EBID<>0 ";
            if (sEmployeeBonusIDs != "")
            {
                sSql += " AND EBID IN (" + sEmployeeBonusIDs+")";
            }
            else
            {
                if (ProcessMonth > 0)
                {
                    sSql += " AND Month =" + ProcessMonth;
                }
                if (ProcessYear > 0)
                {
                    sSql += " AND Year =" + ProcessYear;
                }
                if (sEmployeeIDs != "")
                {
                    sSql += " AND EmployeeID IN(" + sEmployeeIDs + ")";
                }
                if (!string.IsNullOrEmpty(sBusinessUnitIDs))
                {
                    sSql += " AND BusinessUnitID IN(" + sBusinessUnitIDs + ")";
                }
                if (nLocationID > 0)
                {
                    sSql += " AND LocationID =" + nLocationID;
                }
                if (sDepartmentIds != "")
                {
                    sSql += " AND DepartmentID IN(" + sDepartmentIds + ")";
                }
                if (sDesignationIds != "")
                {
                    sSql += " AND DesignationID IN(" + sDesignationIds + ")";
                }
                if (nCategory > 0)
                {
                    sSql += " AND EmployeeCategory=" + nCategory;
                }
                if (sBlockIDs != "")
                {
                    sSql += "AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + sBlockIDs + "))";
                }
                if (sGroupIDs != "")
                {
                    sSql += " AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + sGroupIDs + "))";
                }
                if (nStartSalaryRange > 0 && nEndSalaryRange > 0)
                {
                    sSql += " AND GrossAmount BETWEEN " + nStartSalaryRange + " AND " + nEndSalaryRange;
                }
            }
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
            }
            sSql = sSql + " ORDER BY EmployeeCode";

            oEmployeeBonuss = EmployeeBonus.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            return oEmployeeBonuss;
        }
        #endregion PDF
        public string NumberInBan(string num)
        {
            string[] Numbers = { "০", "১", "২", "৩", "৪", "৫", "৬", "৭", "৮", "৯", ",", "." };
            var nlength1 = 0;
            var nlength2 = 1;
            var NumberInBangla = "";
            var number = "";
            number = num;

            for (var i = 0; i < number.Length; i++)
            {
                if (number.Substring(nlength1, nlength2) == "0")
                {
                    NumberInBangla += Numbers[0];
                }
                else if (number.Substring(nlength1, nlength2) == "1")
                {
                    NumberInBangla += Numbers[1];
                }
                else if (number.Substring(nlength1, nlength2) == "2")
                {
                    NumberInBangla += Numbers[2];
                }
                else if (number.Substring(nlength1, nlength2) == "3")
                {
                    NumberInBangla += Numbers[3];
                }
                else if (number.Substring(nlength1, nlength2) == "4")
                {
                    NumberInBangla += Numbers[4];
                }
                else if (number.Substring(nlength1, nlength2) == "5")
                {
                    NumberInBangla += Numbers[5];
                }
                else if (number.Substring(nlength1, nlength2) == "6")
                {
                    NumberInBangla += Numbers[6];
                }
                else if (number.Substring(nlength1, nlength2) == "7")
                {
                    NumberInBangla += Numbers[7];
                }
                else if (number.Substring(nlength1, nlength2) == "8")
                {
                    NumberInBangla += Numbers[8];
                }
                else if (number.Substring(nlength1, nlength2) == "9")
                {
                    NumberInBangla += Numbers[9];
                }
                else if (number.Substring(nlength1, nlength2) == ",")
                {
                    NumberInBangla += Numbers[10];
                }
                else if (number.Substring(nlength1, nlength2) == "/" || number.Substring(nlength1, nlength2) == ".")
                {
                    NumberInBangla += Numbers[11];
                }

                nlength1++;
                //nlength2++;
            }
            return NumberInBangla;
        }

        public int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }
        public Image GetImage(byte[] Image)
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


        #region Bonus Process
        [HttpPost]
        public JsonResult ProcessEmployeeBonus(EmployeeBonusProcess oEmployeeBonusProcess)
        {
            try
            {
                int nNewIndex = 1;
                int nIndex = 0;
                string sEmployeeIDs = "";

                sEmployeeIDs = oEmployeeBonusProcess.EmployeeIDs;
                oEmployeeBonusProcess.MonthID = (EnumMonth)oEmployeeBonusProcess.MonthIDInt;
                oEmployeeBonusProcess.ProcessDate = DateTime.Now;
                oEmployeeBonusProcess = oEmployeeBonusProcess.Process(((User)(Session[SessionInfo.CurrentUser])).UserID);


                if (oEmployeeBonusProcess.EBPID > 0)
                {
                    if (sEmployeeIDs == null) { sEmployeeIDs = ""; }
                    
                    while (nNewIndex != 0)
                    {
                        nNewIndex = oEmployeeBonusProcess.ProcessEmpBonus(nIndex, oEmployeeBonusProcess.EBPID, sEmployeeIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                        nIndex = nNewIndex;
                    }
                        
                }

            }
            catch (Exception ex)
            {
                oEmployeeBonusProcess = new EmployeeBonusProcess();
                oEmployeeBonusProcess.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeBonusProcess);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult BonusDelete(int nEBPId, double nts)
        {
            _oEmployeeBonusProcess = new EmployeeBonusProcess();
            try
            {
                _oEmployeeBonusProcess.EBPID = nEBPId;
                _oEmployeeBonusProcess = _oEmployeeBonusProcess.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                
            }
            catch (Exception ex)
            {
                _oEmployeeBonusProcess = new EmployeeBonusProcess();
                _oEmployeeBonusProcess.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeBonusProcess.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Approved(EmployeeBonusProcess oEmployeeBonusProcess)
        {
            _oEmployeeBonusProcess = new EmployeeBonusProcess();
            try
            {
                _oEmployeeBonusProcess = oEmployeeBonusProcess;
                _oEmployeeBonusProcess = _oEmployeeBonusProcess.Approved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployeeBonusProcess = new EmployeeBonusProcess();
                _oEmployeeBonusProcess.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeBonusProcess);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UndoApproved(EmployeeBonusProcess oEmployeeBonusProcess)
        {
            _oEmployeeBonusProcess = new EmployeeBonusProcess();
            try
            {
                _oEmployeeBonusProcess = oEmployeeBonusProcess;
                _oEmployeeBonusProcess = _oEmployeeBonusProcess.UndoApproved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployeeBonusProcess = new EmployeeBonusProcess();
                _oEmployeeBonusProcess.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeBonusProcess);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        #region Compliance Bonus


        public void CompPrintBEListInXL(string sParams, double ts)
        {
            _oEmployeeBonuss = new List<EmployeeBonus>();
            _oEmployeeBonuss = EBGets(sParams);
            string sESSIDs = string.Join(",", _oEmployeeBonuss.Select(x => x.ESSID));
            List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
            if (!string.IsNullOrEmpty(sESSIDs))
            {
                oEmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets("SELECT * FROM VIEW_EmployeeSalaryStructureDetail WHERE ESSID IN(" + sESSIDs + ") AND SalaryHeadType=1", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            List<EmployeeSalaryStructureDetail> oDistinctSalaryHeads = new List<EmployeeSalaryStructureDetail>();
            oDistinctSalaryHeads = oEmployeeSalaryStructureDetails.GroupBy(x => x.SalaryHeadName).Select(g => g.First()).OrderBy(x => x.SalaryHeadID).ToList();


            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("BONUS List");
                sheet.Name = "BONUS LIST";

                sheet.Column(2).Width = 6; //SL
                sheet.Column(3).Width = 20; //EmployeeCode
                sheet.Column(4).Width = 20; //EmployeeName
                sheet.Column(5).Width = 20; //LocationName
                sheet.Column(6).Width = 20; //DepartmentName
                sheet.Column(7).Width = 20; //DesignationName
                sheet.Column(8).Width = 20; //JOINING
                sheet.Column(9).Width = 20; //CONFIRMATION
                sheet.Column(10).Width = 20; //CONFIRMATION
                sheet.Column(11).Width = 20; //CATEGORY
                sheet.Column(12).Width = 20; //SalaryHeadName
                sheet.Column(13).Width = 20; //Note
                sheet.Column(14).Width = 20; //Month
                sheet.Column(15).Width = 20; //Year
                sheet.Column(16).Width = 20; //Declaration
                sheet.Column(17).Width = 20; //GROSS
                int nCol = 18;
                foreach (EmployeeSalaryStructureDetail oItem in oDistinctSalaryHeads)
                {
                    sheet.Column(nCol++).Width = 20; //SS
                }
                sheet.Column(nCol++).Width = 20; //others
                sheet.Column(nCol++).Width = 20; //BonusAmount
                sheet.Column(nCol++).Width = 20; //stamp
                sheet.Column(nCol++).Width = 20; //EBGrossAmount
                sheet.Column(nCol++).Width = 20; //Account no 
                sheet.Column(nCol++).Width = 20; //BAnk name

                nMaxColumn = nCol;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2]; cell.Value = (_oEmployeeBonuss[0].Note); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Location"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "joining"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Confirmation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Service Year"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Category"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Salary Head"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Note"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Month"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Year"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Declaration"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                foreach (EmployeeSalaryStructureDetail oItem in oDistinctSalaryHeads)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SalaryHeadName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Other Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bonus Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Stamp"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Bonus Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Account No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bank Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowIndex++;
                #endregion

                #region Table Body
                int nSL = 0;
                double nGrossAmount = 0;
                double nBonusAmount = 0;
                double nEBGrossAmount = 0;
                double nOthersAmount = 0;
                double nStamp = 0;


                foreach (EmployeeBonus EB in _oEmployeeBonuss)
                {
                    nSL++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.EmployeeCode; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.EmployeeName; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.LocationName; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.DepartmentName; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.DesignationName; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.JoiningDateInString; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.ConfirmationDateInString; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Service Year
                    DateDifference dateDifference = new DateDifference(EB.BonusDeclarationDate, EB.JoiningDate);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = dateDifference.ToString(); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.EmployeeCategoryInString; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.SalaryHeadName; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.Note; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.MonthInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.Year; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.BonusDeclarationDateInString; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.CompGrossAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nGrossAmount = nGrossAmount + EB.CompGrossAmount;

                    foreach (EmployeeSalaryStructureDetail oItem in oDistinctSalaryHeads)
                    {
                        double nHeadAmount = oEmployeeSalaryStructureDetails.Where(x => x.SalaryHeadID == oItem.SalaryHeadID && x.ESSID == EB.ESSID).Sum(x => x.CompAmount);
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nHeadAmount > 0 ? nHeadAmount : 0; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.OthersAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nOthersAmount = nOthersAmount + EB.OthersAmount;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.CompBonusAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nBonusAmount = nBonusAmount + EB.CompBonusAmount;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.Stamp; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nStamp = nStamp + EB.Stamp;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.CompEBGrossAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nEBGrossAmount = nEBGrossAmount + EB.CompEBGrossAmount;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.BankAccountNo; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.BankBranchName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                }

                #endregion

                #region Total

                cell = sheet.Cells[rowIndex, 16];
                cell.Value = "Total:"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 17]; cell.Value = Global.MillionFormat(nGrossAmount); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                int n = 18;
                foreach (EmployeeSalaryStructureDetail oItem in oDistinctSalaryHeads)
                {
                    double nHeadAmount = oEmployeeSalaryStructureDetails.Where(x => x.SalaryHeadID == oItem.SalaryHeadID).Sum(x => x.CompAmount);
                    cell = sheet.Cells[rowIndex, n++]; cell.Value = Global.MillionFormat(nHeadAmount); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                cell = sheet.Cells[rowIndex, n++]; cell.Value = Global.MillionFormat(nOthersAmount); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, n++]; cell.Value = Global.MillionFormat(nBonusAmount); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, n++]; cell.Value = Global.MillionFormat(nStamp); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, n++]; cell.Value = Global.MillionFormat(nEBGrossAmount); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, n++];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, n++];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //signature

                rowIndex = rowIndex + 8;
                colIndex = 1;

                int Colspan = 0;

                Colspan = (nMaxColumn - nMaxColumn % 4) / 4 - 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "_________________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "_______________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "__________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "_____________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                rowIndex = rowIndex + 1;
                colIndex = 0;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Prepared By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Checked By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Reviewed By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Approved By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EBList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void CompPrintBEListInXL_F2(string sParams, double ts)
        {
            _oEmployeeBonuss = new List<EmployeeBonus>();
            _oEmployeeBonuss = EBGets(sParams);

            int nCount = sParams.Split('~').Length;

            string _sBlockIDs = sParams.Split('~')[10];

            List<EmployeeBonus> oEmployeeBonuss = new List<EmployeeBonus>();
            _oEmployeeBonuss.ForEach(x => oEmployeeBonuss.Add(x));


            List<Company> oCompanys = new List<Company>();
            Company oCompany = new Company();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oCompany = oCompanys.First();
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 1;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("BONUS List");
                sheet.Name = "BONUS LIST";

                sheet.Column(2).Width = 6; //SL
                sheet.Column(3).Width = 20; //EmployeeCode
                sheet.Column(4).Width = 20; //EmployeeName
                sheet.Column(5).Width = 20; //DesignationName
                sheet.Column(6).Width = 20; //EmployeeType
                sheet.Column(7).Width = 20; //JOINING
                sheet.Column(8).Width = 20; //GROSS
                sheet.Column(9).Width = 20; //Declaration
                sheet.Column(10).Width = 20; //SERVICE YEAR
                sheet.Column(11).Width = 20; //PERCENT
                sheet.Column(12).Width = 20; //PAYABLE
                sheet.Column(13).Width = 20; //STAMP
                sheet.Column(14).Width = 20; //NET
                sheet.Column(15).Width = 20; //SIGN

                nMaxColumn = 15;

                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string BUIDs = string.Join(",", _oEmployeeBonuss.Where(x => x.EBID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
                if (BUIDs != "")
                { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }


                cell = sheet.Cells[rowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;

                EBReportHeaderXL(ref  sheet, ref  cell, ref  rowIndex, ref  colIndex, nMaxColumn, oBusinessUnits[0], _oEmployeeBonuss[0]);
                EBColumnSetup(ref  sheet, ref  cell, ref fill, ref border, ref rowIndex, ref colIndex, nMaxColumn, _oEmployeeBonuss[0], _sBlockIDs);

                _oEmployeeBonuss = _oEmployeeBonuss.OrderBy(x => x.BusinessUnitName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.BlockName).ToList();
                while (_oEmployeeBonuss.Count > 0)
                {
                    //var oResults = _oEmployeeBonuss.Where(x => x.BusinessUnitName == _oEmployeeBonuss[0].BusinessUnitName && x.LocationName == _oEmployeeBonuss[0].LocationName && x.DepartmentName == _oEmployeeBonuss[0].DepartmentName).ToList();

                    var oResults = new List<EmployeeBonus>();
                    if (!string.IsNullOrEmpty(_sBlockIDs))
                    {
                        oResults = _oEmployeeBonuss.Where(x => x.BusinessUnitName == _oEmployeeBonuss[0].BusinessUnitName && x.LocationName == _oEmployeeBonuss[0].LocationName && x.DepartmentName == _oEmployeeBonuss[0].DepartmentName && x.BlockName == _oEmployeeBonuss[0].BlockName).ToList();
                    }
                    else
                    {
                        oResults = _oEmployeeBonuss.Where(x => x.BusinessUnitName == _oEmployeeBonuss[0].BusinessUnitName && x.LocationName == _oEmployeeBonuss[0].LocationName && x.DepartmentName == _oEmployeeBonuss[0].DepartmentName).ToList();
                    }

                    List<BusinessUnit> oTempBusinessUnits = new List<BusinessUnit>();
                    BusinessUnit oBusinessUnit = new BusinessUnit();
                    oTempBusinessUnits = oBusinessUnits.Where(x => x.BusinessUnitID == _oEmployeeBonuss[0].BusinessUnitID).ToList();
                    oBusinessUnit = oTempBusinessUnits.Count > 0 ? oTempBusinessUnits[0] : new BusinessUnit();

                    //EBReportHeaderXL(ref  sheet, ref  cell, ref  rowIndex, ref  colIndex, nMaxColumn, oBusinessUnit, _oEmployeeBonuss[0]);
                    //EBColumnSetup(ref  sheet, ref  cell, ref fill, ref border, ref rowIndex, ref colIndex, nMaxColumn, _oEmployeeBonuss[0], _sBlockIDs);

                    colIndex = 2;


                    string sDeptBlock = "Department- " + _oEmployeeBonuss[0].DepartmentName;
                    if (!string.IsNullOrEmpty(_sBlockIDs))
                    {
                        if (!string.IsNullOrEmpty(_oEmployeeBonuss[0].BlockName))
                        {
                            sDeptBlock += ", Block- " + _oEmployeeBonuss[0].BlockName;
                        }
                    }

                    cell = sheet.Cells[rowIndex, 2];
                    cell.Value = "Unit Name : " + _oEmployeeBonuss[0].LocationName + ", Department Name : " + sDeptBlock; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell = sheet.Cells[rowIndex, nMaxColumn];
                    //cell.Value = sDeptBlock; cell.Style.Font.Bold = true;
                    //cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    rowIndex += 1;
                    colIndex = 2;

                    EBBodySetupComp(ref  sheet, ref  cell, ref fill, ref border, ref rowIndex, ref colIndex, nMaxColumn, oResults, oResults[0].DepartmentID, oCompany, _sBlockIDs, oResults[0].BlockName);

                    //_oEmployeeBonuss.RemoveAll(x => x.BusinessUnitName == oResults[0].BusinessUnitName && x.LocationName == oResults[0].LocationName && x.DepartmentName == oResults[0].DepartmentName);
                    if (!string.IsNullOrEmpty(_sBlockIDs))
                    {
                        _oEmployeeBonuss.RemoveAll(x => x.BusinessUnitName == oResults[0].BusinessUnitName && x.LocationName == oResults[0].LocationName && x.DepartmentName == oResults[0].DepartmentName && x.BlockName == oResults[0].BlockName);
                    }
                    else
                    {
                        _oEmployeeBonuss.RemoveAll(x => x.BusinessUnitName == oResults[0].BusinessUnitName && x.LocationName == oResults[0].LocationName && x.DepartmentName == oResults[0].DepartmentName);
                    }
                }

                #region Total

                cell = sheet.Cells[rowIndex, 2];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 3];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 5];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 6];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 7];
                cell.Value = "Grand Total:"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                double nGrossAmount = oEmployeeBonuss.Sum(x => x.CompGrossAmount);
                cell = sheet.Cells[rowIndex, 8]; cell.Value = Global.MillionFormat(nGrossAmount); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 9];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 10];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 11];
                cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                double nBonusAmount = oEmployeeBonuss.Sum(x => x.CompBonusAmount);
                cell = sheet.Cells[rowIndex, 12]; cell.Value = Global.MillionFormat(nBonusAmount); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                double nStamp = oEmployeeBonuss.Count * 10;
                //For Golden
                int EmpCountForBelow500 = oEmployeeBonuss.Where(x => (x.CompBonusAmount <= 500)).ToList().Count();
                double TotalStampForGolden = EmpCountForBelow500 * 10;

                cell = sheet.Cells[rowIndex, 13]; cell.Value = (oCompany.BaseAddress == "golden") ? (nStamp - TotalStampForGolden) : nStamp; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 14]; cell.Value = (oCompany.BaseAddress == "golden") ? (nBonusAmount - (nStamp - TotalStampForGolden)) : nBonusAmount - nStamp; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //signature

                rowIndex = rowIndex + 8;
                colIndex = 1;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EBList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void EBBodySetupComp(ref ExcelWorksheet sheet, ref ExcelRange cell, ref ExcelFill fill, ref OfficeOpenXml.Style.Border border, ref int rowIndex, ref int colIndex, int nMaxColumn, List<EmployeeBonus> oEmployeeBonuss, int nDepartmentID, Company oCompany, string _sBlockIDs, string blockName)
        {
            int nSL = 0;

            foreach (EmployeeBonus EB in oEmployeeBonuss)
            {
                nSL++;
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.EmployeeCode; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.EmployeeName; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.DesignationName; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.EmployeeTypeName; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.JoiningDateInString; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.CompGrossAmount; cell.Style.Numberformat.Format = "#,##0.00"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.BonusDeclarationDateInString; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                DateTime DeclarationDate = EB.BonusDeclarationDate.AddDays(1);
                DateDifference ServiceYear = new DateDifference(EB.JoiningDate, DeclarationDate);

                double nTotalDayCount = (DeclarationDate - EB.JoiningDate).TotalDays;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (nTotalDayCount < 365) ? nTotalDayCount.ToString() + "d" : ServiceYear.ToString(); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                double Percent = EB.CompBonusAmount / EB.CompGrossAmount * 100;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Percent; cell.Style.Numberformat.Format = "#,##0.00"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.CompBonusAmount; cell.Style.Numberformat.Format = "#,##0.00"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //For BaseAddress Golden<=500
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oCompany.BaseAddress == "golden" && (EB.CompBonusAmount <= 500)) ? 0.00 : 10.00; cell.Style.Numberformat.Format = "#,##0.00"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oCompany.BaseAddress == "golden" && (EB.CompBonusAmount <= 500)) ? EB.CompBonusAmount : EB.CompBonusAmount - 10; cell.Style.Numberformat.Format = "#,##0.00"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
            }
            cell = sheet.Cells[rowIndex, 2];
            cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 3];
            cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 4];
            cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 5];
            cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 6];
            cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 7];
            cell.Value = "Total:"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            //double nGrossAmount = oEmployeeBonuss.Sum(x => x.GrossAmount);
            double nGrossAmount;
            if (!string.IsNullOrEmpty(_sBlockIDs))
            {
                nGrossAmount = oEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID && (x.BlockName == blockName)).Sum(x => x.CompGrossAmount);
            }
            else
            {
                nGrossAmount = oEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID).Sum(x => x.CompGrossAmount);
            }

            cell = sheet.Cells[rowIndex, 8]; cell.Value = Global.MillionFormat(nGrossAmount); cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 9];
            cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 10];
            cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 11];
            cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            //double nBonusAmount = oEmployeeBonuss.Sum(x => x.BonusAmount);
            double nBonusAmount;
            if (!string.IsNullOrEmpty(_sBlockIDs))
            {
                nBonusAmount = oEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID && (x.BlockName == blockName)).Sum(x => x.CompBonusAmount);
            }
            else
            {
                nBonusAmount = oEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID).Sum(x => x.CompBonusAmount);
            }



            cell = sheet.Cells[rowIndex, 12]; cell.Value = Global.MillionFormat(nBonusAmount); cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            //double nStamp = oEmployeeBonuss.Count * 10;
            double nStamp;
            if (!string.IsNullOrEmpty(_sBlockIDs))
            {
                nStamp = oEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID && (x.BlockName == blockName)).ToList().Count * 10;
            }
            else
            {
                nStamp = oEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID).ToList().Count * 10;
            }


            //For Golden
            //int EmpCountForBelow500 = oEmployeeBonuss.Where(x => (x.BonusAmount <= 500)).ToList().Count();
            int EmpCountForBelow500;
            if (!string.IsNullOrEmpty(_sBlockIDs))
            {
                EmpCountForBelow500 = oEmployeeBonuss.Where(x => (x.DepartmentID == nDepartmentID && (x.BlockName == blockName)) && (x.CompBonusAmount <= 500)).ToList().Count();
            }
            else
            {
                EmpCountForBelow500 = oEmployeeBonuss.Where(x => (x.DepartmentID == nDepartmentID) && (x.CompBonusAmount <= 500)).ToList().Count();
            }


            double TotalStampForGolden = EmpCountForBelow500 * 10;


            cell = sheet.Cells[rowIndex, 13]; cell.Value = (oCompany.BaseAddress == "golden") ? (nStamp - TotalStampForGolden) : nStamp; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 14]; cell.Value = (oCompany.BaseAddress == "golden") ? (nBonusAmount - (nStamp - TotalStampForGolden)) : (nBonusAmount - nStamp); cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            rowIndex++;
            colIndex = 2;

        }

        public void CompPrintEBExcel_F3(string sParams, double ts)
        {
            _oEmployeeBonus = new EmployeeBonus();
            _oEmployeeBonus.EmployeeBonuss = EBGets(sParams);

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployeeBonus.EmployeeBonuss.Where(x => x.EBID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "")
            { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }
            _oEmployeeBonus.BusinessUnits = oBusinessUnits;

            string sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


            List<Company> oCompanys = new List<Company>();
            Company oCompany = new Company();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oCompany = oCompanys.First();
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 1;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("BONUS List");
                sheet.Name = "BONUS LIST";

                sheet.Column(2).Width = 6; //
                sheet.Column(3).Width = 20; //
                sheet.Column(4).Width = 30; //
                sheet.Column(5).Width = 30; //
                sheet.Column(6).Width = 20; //
                sheet.Column(7).Width = 20; //
                sheet.Column(8).Width = 20; //
                sheet.Column(9).Width = 20; //
                sheet.Column(10).Width = 20; //
                int i = 10;
                if (_bPercent)
                {
                    sheet.Column(i++).Width = 20; //
                }
                sheet.Column(i++).Width = 20; //
                sheet.Column(i++).Width = 20; //

                nMaxColumn = i;

                //List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                //string BUIDs = string.Join(",", _oEmployeeBonuss.Where(x => x.EBID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
                //if (BUIDs != "")
                //{ oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }



                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2]; cell.Value = _oEmployeeBonus.BusinessUnits[0].Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2]; cell.Value = _oEmployeeBonus.BusinessUnits[0].Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2]; cell.Value = _oEmployeeBonus.EmployeeBonuss[0].Note; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rowIndex = rowIndex + 1;

                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "CODE"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "NAME"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DESIGNATION"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "JOINING DATE"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SERVICE YEAR"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "GROSS"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BASIC"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                if (_bPercent)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "%"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BONUS AMOUNT"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SIGNATURE"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                _oEmployeeBonuss = _oEmployeeBonus.EmployeeBonuss.OrderBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
                if (_oEmployeeBonuss.Count <= 0)
                {
                    string masg = "Nothing to print";
                    cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = masg; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                else
                {
                    var grpEmpBonus = _oEmployeeBonuss.GroupBy(x => new { x.DepartmentID, x.DepartmentName }, (key, grp) => new
                    {
                        DepartmentID = key.DepartmentID,
                        DepartmentName = key.DepartmentName,
                        EmpBonusCount = grp.Count(),
                        EmpBonusList = grp,


                    }).ToList();
                    foreach (var oItem in grpEmpBonus)
                    {
                        double TotalBonus = 0.0;
                        //Print Department

                        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "Department : " + oItem.DepartmentName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        rowIndex += 1;

                        int nCount = 0;
                        foreach (EmployeeBonus data in oItem.EmpBonusList.OrderBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode))
                        {
                            nCount++;
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCount; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.EmployeeCode; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.EmployeeName; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.DesignationName; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.JoiningDateInString; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            DateTime DeclarationDate = data.BonusDeclarationDate.AddDays(1);
                            DateDifference oService = new DateDifference(data.JoiningDate, DeclarationDate);

                            double nTotalDayCount = (DeclarationDate - data.JoiningDate).TotalDays;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (nTotalDayCount < 365) ? oService.ToString() + "\n(" + nTotalDayCount.ToString() + "d)" : oService.ToString(); cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(data.CompGrossAmount, 2); cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(data.CompBasicAmount, 2); cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            if (_bPercent)
                            {
                                double Percent = data.CompBonusAmount / data.CompGrossAmount * 100;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(Percent, 2); cell.Style.Font.Bold = false;
                                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(data.CompBonusAmount, 2); cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            TotalBonus += data.CompBonusAmount;
                            rowIndex++;
                        }
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Value = "TOTAL"; cell.Style.Font.Bold = true; cell.Merge = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, (_bPercent) ? 11 : 10]; cell.Value = Math.Round(TotalBonus, 2); cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, (_bPercent) ? 12 : 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        rowIndex++;
                    }
                }


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EBList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public ActionResult CompPrintEBListInPDF_F2(string sParams, double ts)
        {
            _oEmployeeBonus = new EmployeeBonus();
            _oEmployeeBonus.EmployeeBonuss = EBGets(sParams);

            _oEmployeeBonus.EmployeeBonuss.ForEach(x =>
            {
                x.GrossAmount= x.CompGrossAmount;
                x.EBGrossAmount = x.CompEBGrossAmount;
                x.BasicAmount = x.CompBasicAmount;
                x.BonusAmount = x.CompBonusAmount;
            });

            int nCount = sParams.Split('~').Length;

            string sBlockIDs = sParams.Split('~')[10];


            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployeeBonus.EmployeeBonuss.Where(x => x.EBID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "")
            { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }
            _oEmployeeBonus.BusinessUnits = oBusinessUnits;

            string sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


            List<Company> oCompanys = new List<Company>();
            Company oCompany = new Company();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oCompany = oCompanys.First();
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptEmployeeBonus_F2 oReport = new rptEmployeeBonus_F2();
            byte[] abytes = oReport.PrepareReport(_oEmployeeBonus, oSalarySheetSignature, oCompany, sBlockIDs);
            return File(abytes, "application/pdf");

        }

        public ActionResult CompPrintPDFF3(string sParams, double ts)
        {
            _oEmployeeBonus = new EmployeeBonus();
            _oEmployeeBonus.EmployeeBonuss = EBGets(sParams);


            _oEmployeeBonus.EmployeeBonuss.ForEach(x =>
            {
                x.GrossAmount = x.CompGrossAmount;
                x.EBGrossAmount = x.CompEBGrossAmount;
                x.BasicAmount = x.CompBasicAmount;
                x.BonusAmount = x.CompBonusAmount;
            });



            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployeeBonus.EmployeeBonuss.Where(x => x.EBID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "")
            { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }
            _oEmployeeBonus.BusinessUnits = oBusinessUnits;

            string sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptEmployeeBonus_F3 oReport = new rptEmployeeBonus_F3();
            byte[] abytes = oReport.PrepareReport(_oEmployeeBonus, oSalarySheetSignature, _bPercent);
            return File(abytes, "application/pdf");

        }

        public void CompPrintEBExcelF4(string sParams, double ts)
        {
            int BusinessUnitID = 0;
            _oEmployeeBonus = new EmployeeBonus();
            _oEmployeeBonus.EmployeeBonuss = EBGets(sParams);

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployeeBonus.EmployeeBonuss.Where(x => x.EBID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "")
            { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }
            _oEmployeeBonus.BusinessUnits = oBusinessUnits;

            string sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<EmployeeBonus> _oEmployeeBonuss = new List<EmployeeBonus>();
            _oEmployeeBonus.EmployeeBonuss.ForEach(x =>
            {
                x.Stamp = 10;
            });
            _oEmployeeBonuss = _oEmployeeBonus.EmployeeBonuss;


            List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
            _oBusinessUnits = _oEmployeeBonus.BusinessUnits;

            if (_oEmployeeBonuss.Count() > 0)
            {
                BusinessUnitID = _oEmployeeBonuss[0].BusinessUnitID;
            }
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnits = _oBusinessUnits.Where(x => x.BusinessUnitID == BusinessUnitID).ToList();
            if (oBusinessUnits.Count > 0) { oBusinessUnit = oBusinessUnits[0]; }


            Company oCompany = new Company();
            int nMaxColumn = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            int rowIndex = 1;
            int colIndex = 1;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Employee Bonus");
                sheet.Name = "Employee Bonus";

                sheet.Column(1).Width = 10;
                sheet.Column(2).Width = 20;
                sheet.Column(3).Width = 25;
                sheet.Column(4).Width = 22;
                sheet.Column(5).Width = 18;
                //sheet.Column(6).Width = 18;
                sheet.Column(6).Width = 18;
                sheet.Column(7).Width = 18;
                sheet.Column(8).Width = 20; 
                sheet.Column(9).Width = 18;
                sheet.Column(10).Width = 17;
                sheet.Column(11).Width = 17;
                sheet.Column(12).Width = 17;
                sheet.Column(13).Width = 22;

                nMaxColumn = 13;

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);


                cell = sheet.Cells[rowIndex, nMaxColumn / 2]; cell.Merge = true; cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;
                cell = sheet.Cells[rowIndex, nMaxColumn / 2]; cell.Merge = true; cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;

                cell = sheet.Cells[rowIndex, nMaxColumn / 2]; cell.Merge = true; cell.Value = _oEmployeeBonuss[0].Note; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;


                if (_oEmployeeBonuss.Count <= 0)
                {
                    string masg = "Nothing to print";
                    //if (_oEmployeeBonuss[0].ErrorMessage != "") { masg = ""; masg = _oEmployeeBonuss[0].ErrorMessage; }

                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = masg; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    rowIndex += 1;
                }
                else
                {
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Joining Date"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Salary"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Basic Salary"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Length of Service"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "%"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bonus TK."; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Stamp"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Payable"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    rowIndex += 1;

                    var grpEmpBonus = _oEmployeeBonuss.GroupBy(x => new {x.LocationID, x.LocationName, x.DepartmentID, x.DepartmentName }, (key, grp) => new
                    {
                        LocationID = key.LocationID,
                        LocationName = key.LocationName,
                        DepartmentID = key.DepartmentID,
                        DepartmentName = key.DepartmentName,
                        EmpBonusCount = grp.Count(),
                        EmpBonusList = grp,


                    }).ToList();



                    double gTotalBonus = 0.0;
                    int gEmployee = 0;
                    double gGrossSalary = 0.0;
                    double gBasicSalary = 0.0;
                    double gBonus = 0.0;
                    double gStamp = 0.0;
                    double gNetPayable = 0.0;
                    foreach (var oItem in grpEmpBonus)
                    {
                        
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Location:" + oItem.LocationName +", Department : " + oItem.DepartmentName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        rowIndex = rowIndex + 1;

                        double TotalBonus = 0.0;
                        int nCount = 0;
                        int nEmployee = 0;
                        double nGrossSalary = 0.0;
                        double nBasicSalary = 0.0;
                        double nBonus = 0.0;
                        double nStamp = 0.0;
                        double nNetPayable = 0.0;


                        foreach (EmployeeBonus data in oItem.EmpBonusList.OrderBy(x => x.EmployeeCode))
                        {
                            if (data.CompBonusAmount > 0)
                            {
                                nCount++;
                                colIndex = 1;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.EmployeeCode; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.EmployeeName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.DesignationName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.JoiningDateInString; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(data.CompGrossAmount); cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(data.CompBasicAmount); cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                                DateTime DeclarationDate = data.BonusDeclarationDate.AddDays(1);
                                DateDifference oService = new DateDifference(data.JoiningDate, DeclarationDate);
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oService.ToString(); cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                                double Percent = data.CompBonusAmount / data.CompBasicAmount * 100;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(Percent); cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(data.CompBonusAmount); cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(data.Stamp); cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(data.CompBonusAmount - data.Stamp); cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;


                                rowIndex += 1;


                                TotalBonus += data.CompBonusAmount;
                                nEmployee++;
                                nGrossSalary += data.CompGrossAmount;
                                nBasicSalary += data.CompBasicAmount;
                                nBonus += data.CompBonusAmount;
                                nStamp += data.Stamp;
                                nNetPayable += (data.CompBonusAmount - data.Stamp);


                                gTotalBonus += data.CompBonusAmount;
                                gEmployee++;
                                gGrossSalary += data.CompGrossAmount;
                                gBasicSalary += data.CompBasicAmount;
                                gBonus += data.CompBonusAmount;
                                gStamp += data.Stamp;
                                gNetPayable += (data.CompBonusAmount - data.Stamp);
                            }
                        }
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++];  cell.Value = "Total Employee : " + nEmployee; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "" ; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];  cell.Value = "" ; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];  cell.Value = "" ; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "" ; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++];  cell.Value = "" ; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;
                        

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nGrossSalary); cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nBasicSalary); cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); 
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nBonus); cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); 
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nStamp); cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); 
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nNetPayable); cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); 
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        rowIndex += 2;


                    }

                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Employee : " + gEmployee; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = gGrossSalary; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = gBasicSalary; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = gBonus; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = gStamp; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = gNetPayable; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                    rowIndex += 2;
                }

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EmployeeBonus.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

        }

        public void CompPrintEBSummaryExcel(string sParams, double ts)
        {
            List<EmployeeBonus> oEmployeeBonuss = new List<EmployeeBonus>();
            oEmployeeBonuss = EBGets(sParams);
            PrintSummaryExcelComp(oEmployeeBonuss);
        }

        private void PrintSummaryExcelComp(List<EmployeeBonus> oEmployeeBonuss)
        {
            Company oCompany = new Company();
            int nMaxColumn = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Employee Bonus Summary");
                sheet.Name = "Employee Bonus Summary";

                sheet.Column(1).Width = 25;//SL
                sheet.Column(2).Width = 25;//Dept Name
                sheet.Column(3).Width = 20;//ManPower
                sheet.Column(4).Width = 20;//Gross
                sheet.Column(5).Width = 20;//Payable
                sheet.Column(6).Width = 20;//Stamp
                sheet.Column(7).Width = 20;//NetPayable

                nMaxColumn = 5;

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Table Body

                int rowIndex = 1;
                int nPS = 0;
                int colIndex = 1;


                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string BUIDs = string.Join(",", oEmployeeBonuss.Select(p => p.BusinessUnitID).Distinct().ToList());
                if (BUIDs != "") { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }


                cell = sheet.Cells[rowIndex, colIndex]; cell.Merge = true;
                cell.Value = oBusinessUnits.Count == 1 ? oBusinessUnits[0].Name : oCompany.Name; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 30; cell.Style.Font.SetFromFont(new Font("Century Gothic", 22));
                //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 22; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                rowIndex = rowIndex + 1;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex]; cell.Merge = true;
                cell.Value = oBusinessUnits.Count == 1 ? oBusinessUnits[0].Address : oCompany.Address; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 16;
                //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colIndex = 1;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, colIndex]; cell.Merge = true;
                cell.Value = oEmployeeBonuss[0].Note; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 16;
                //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colIndex = 1;
                rowIndex = rowIndex + 1;


                var grpEBSummary = oEmployeeBonuss.GroupBy(x => new { x.BusinessUnitID, x.BusinessUnitName, x.LocationID }, (key, grp) => new
                {
                    BusinessUnitID = key.BusinessUnitID,
                    BusinessUnitName = key.BusinessUnitName,
                    LocationID = key.LocationID,
                    LocationName = grp.First().LocationName,
                    EBSCount = grp.Count(),
                    EBSList = grp,
                    DepartmentName = grp.First().DepartmentName,

                }).OrderBy(x => x.BusinessUnitName).ToList();


                foreach (var oItem in grpEBSummary)
                {
                    colIndex = 1;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BU"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BusinessUnitName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    rowIndex += 1;
                    colIndex = 1;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Location"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LocationName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    rowIndex += 2;
                    colIndex = 1;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Dept Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ManPower"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Salary"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Payable"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Stamp"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Payable"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex += 1;
                    colIndex = 1;

                    var List = oItem.EBSList.GroupBy(x => new { x.DepartmentID }, (key, grp) => new EmployeeBonus
                    {
                        DepartmentName = grp.First().DepartmentName,
                        CompBonusAmount = grp.Sum(x => x.CompBonusAmount),
                        CompGrossAmount = grp.Sum(x => x.CompGrossAmount),

                        ManPower = grp.Count()

                    }).OrderBy(x => x.DepartmentName).ToList();
                    nPS = 1;
                    double total = 0.0;
                    double nManPower = 0.0;
                    double nGross = 0.0;
                    int totalStamp = 0;
                    double totalNetPayable = 0.0;
                    foreach (var data in List)
                    {
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nPS++;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = data.DepartmentName;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = data.ManPower;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = data.CompGrossAmount;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = data.CompBonusAmount;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        int nStamp = data.ManPower * 10;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nStamp;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        double nNetPayable = (data.CompBonusAmount - nStamp);
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nNetPayable;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        rowIndex += 1;
                        nManPower += data.ManPower;
                        nGross += data.CompGrossAmount;
                        total += data.CompBonusAmount;
                        totalStamp += nStamp;
                        totalNetPayable += nNetPayable;
                    }
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nManPower; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nGross; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = total; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = totalStamp; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = totalNetPayable; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    rowIndex += 3;

                }

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "Group Summary Total"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;
                colIndex = 1;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BUName"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ManPower"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Salary"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Payable"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Stamp"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Payable"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;
                colIndex = 1;

                var grpEBGrandSummary = oEmployeeBonuss.GroupBy(x => new { x.BusinessUnitID, x.BusinessUnitName }, (key, grp) => new
                {
                    BusinessUnitID = key.BusinessUnitID,
                    BusinessUnitName = key.BusinessUnitName,
                    LocationName = grp.First().LocationName,
                    EBSCount = grp.Count(),
                    EBSList = grp,
                    DepartmentName = grp.First().DepartmentName,
                    CompBonusAmount = grp.Sum(x => x.CompBonusAmount),
                    CompGrossAmount = grp.Sum(x => x.CompGrossAmount),
                    ManPower = grp.Count()


                }).OrderBy(x => x.BusinessUnitName).ToList();

                foreach (var oItem in grpEBGrandSummary)
                {
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.BusinessUnitName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;


                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.ManPower; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.CompGrossAmount; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.CompBonusAmount; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    int nStamp = oItem.ManPower * 10;
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nStamp; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    double nNetPayable = (oItem.CompBonusAmount - nStamp);
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nNetPayable; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;


                    rowIndex += 1;
                }


                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EmployeeBonusSummary.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public ActionResult CompPrintPDFF4(string sParams, double ts)
        {
            _oEmployeeBonus = new EmployeeBonus();
            _oEmployeeBonus.EmployeeBonuss = EBGets(sParams);

            _oEmployeeBonus.EmployeeBonuss.ForEach(x =>
            {
                x.GrossAmount = x.CompGrossAmount;
                x.EBGrossAmount = x.CompEBGrossAmount;
                x.BasicAmount = x.CompBasicAmount;
                x.BonusAmount = x.CompBonusAmount;
                x.Stamp = 10;
            });

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oEmployeeBonus.EmployeeBonuss.Where(x => x.EBID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "")
            { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }
            _oEmployeeBonus.BusinessUnits = oBusinessUnits;

            string sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptEmployeeBonus_F4 oReport = new rptEmployeeBonus_F4();
            byte[] abytes = oReport.PrepareReport(_oEmployeeBonus, oSalarySheetSignature);
            return File(abytes, "application/pdf");

        }


        public void CompPrintEBPaySlipExcel(string sParams, double ts)
        {
            List<EmployeeBonus> oEmployeeBonuss = new List<EmployeeBonus>();
            oEmployeeBonuss = EBGets(sParams);
            PrintPaySlipXLComp(oEmployeeBonuss);
        }

        private void PrintPaySlipXLComp(List<EmployeeBonus> oEmployeeBonuss)
        {
            Company oCompany = new Company();
            int nMaxColumn = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Pay Slip");
                sheet.Name = "Pay Slip";

                sheet.Column(1).Width = 28;//Caption
                sheet.Column(2).Width = 20;//Value
                sheet.Column(3).Width = 32;//Caption
                sheet.Column(4).Width = 20;//Value

                nMaxColumn = 4;

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Table Body

                int rowIndex = 1;
                int nPS = 0;
                foreach (EmployeeBonus oItem in oEmployeeBonuss)
                {
                    #region Report Header
                    int colIndex = 1;
                    if (oCompany.CompanyLogo != null)
                    {
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        sheet.Row(rowIndex).Height = 30;
                        //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style= border.Right.Style = ExcelBorderStyle.None;

                        ExcelPicture excelImage = null;
                        excelImage = sheet.Drawings.AddPicture(oItem.EBID.ToString() + "A", oCompany.CompanyLogo);
                        excelImage.From.Column = 0;
                        excelImage.From.Row = (rowIndex - 1);
                        excelImage.SetSize(85, 40);
                        excelImage.From.ColumnOff = this.Pixel2MTU(2);
                        excelImage.From.RowOff = this.Pixel2MTU(2);
                        excelImage.SetPosition(rowIndex - 1, 1, colIndex - 1, 118);
                    }
                    else
                    {
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //border = cell.Style.Border; border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;

                    }
                    colIndex++;

                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 30; cell.Style.Font.SetFromFont(new Font("Century Gothic", 22));
                    //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 22; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    rowIndex = rowIndex + 1;
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 16;
                    //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    colIndex = 1;
                    rowIndex = rowIndex + 1;

                    //cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true;
                    //cell.Value = ""; cell.Style.Font.Bold = true;
                    //border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.Thin;
                    //border = cell.Style.Border; border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.Thin;
                    //cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //rowIndex = rowIndex + 1;

                    #endregion

                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                    cell.Value = "Pay Slip No - " + oItem.EBID.ToString(); cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 13;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border;
                    border.Left.Style = border.Top.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                    cell.Value = oItem.Note; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 12;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border;
                    border.Left.Style = border.Top.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;

                    int nfontSize = 10;
                    double nRH = 13;

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name(নাম):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code No.(কোড নং.):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Div(বিভাগ):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation(পদবী):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Sec(সেকশন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Joining Date(যোগদানের তাং.):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.JoiningDateInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Confirmation Date (স্থায়ীকরনের তাং):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ConfirmationDateInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bonus Annou. Date (বোনাস ঘোষণার তাং):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BonusDeclarationDateInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Service Year(চাকুরীর বয়স):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    DateDifference dateDifference = new DateDifference(oItem.BonusDeclarationDate, oItem.JoiningDate);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = dateDifference.ToString(); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Account no.(অ্যাকাউন্ট নং.):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BankAccountNo; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Wages(মোট বেতন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(ICS.Core.Utility.Global.MillionFormat(oItem.CompGrossAmount)); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Basic(মূল বেতন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(ICS.Core.Utility.Global.MillionFormat(oItem.CompBasicAmount)); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Earning(মোট উপার্জন):"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(ICS.Core.Utility.Global.MillionFormat(oItem.CompBonusAmount)); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Deduction	(মোট কর্তন)"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(ICS.Core.Utility.Global.MillionFormat(oItem.Stamp)); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Payable	(নেট প্রদেয়):"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(ICS.Core.Utility.Global.MillionFormat(oItem.CompEBGrossAmount)); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "In Word:" + @ICS.Core.Utility.Global.TakaWords(oItem.EBGrossAmount); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    if (GetImage(oCompany.AuthorizedSignature) != null)
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        sheet.Row(rowIndex).Height = 27; border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;
                        border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        ExcelPicture excelImage = null;
                        excelImage = sheet.Drawings.AddPicture(oItem.EBID.ToString() + "B", GetImage(oCompany.AuthorizedSignature));
                        excelImage.From.Column = 0;
                        excelImage.From.Row = (rowIndex - 1);
                        excelImage.SetSize(48, 42);
                        excelImage.From.ColumnOff = this.Pixel2MTU(2);
                        excelImage.From.RowOff = this.Pixel2MTU(2);
                    }
                    else
                    {
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;
                        border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;
                    }

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Top.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = 18;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "কর্তৃপক্ষের স্বাক্ষর  "; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Right.Style = border.Top.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Left.Style = border.Bottom.Style = border.Right.Style = border.Top.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Left.Style = border.Bottom.Style = border.Right.Style = border.Top.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "কর্মীর স্বাক্ষর "; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.None;

                    rowIndex = rowIndex + 1;
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++, rowIndex, nMaxColumn]; cell.Merge = true; cell.Style.Font.Size = 8; sheet.Row(rowIndex).Height = 10;
                    cell.Value = "বিঃ দ্রঃ- পে-স্লিপের কোন তথ্য ভুল হলে, পে-স্লিপ প্রপ্তির পরবর্তী ১০ (দশ) কর্ম দিবসের মধ্যে কতৃপক্ষকে অবগত করার জন্য অনুরোধ করা গেল।অন্যথায় উল্লেখিত সময়ের পরে কোন প্রকার অভিযোগ গ্রহণযোগ্য হবেনা।"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.None;

                    //nPS++;
                    //if (nPS % 3 != 0)
                    //{
                    rowIndex = rowIndex + 2;
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++, ++rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = false;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); 
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    //}
                    //else if (nPS % 3 == 0)
                    //{
                    //    rowIndex = rowIndex + 1;
                    //    colIndex = 1;
                    //    cell = sheet.Cells[rowIndex, colIndex++, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = false;
                    //    sheet.Row(rowIndex).Height = 30;
                    //    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); 
                    //    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    //}

                    rowIndex = rowIndex + 1;

                }

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PAYSLIP.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion


        public ActionResult SetSessionSearchCriteria(EmployeeBonus oEmployeeBonus)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oEmployeeBonus);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintEmployeeBonusPaySlip(double ts)
        {
            EmployeeBonus oEmployeeBonus = new EmployeeBonus();
            oEmployeeBonus = (EmployeeBonus)Session[SessionInfo.ParamObj];
            List<EmployeeBonus> _oEmployeeBonusList = new List<EmployeeBonus>();
            _oEmployeeBonusList = EmployeeBonus.Gets("SELECT * FROM  View_EmployeeBonus WHERE EBID IN ("+oEmployeeBonus.sIDs+")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeBonus.EmployeeBonuss = _oEmployeeBonusList;

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeBonus.Company = oCompanys.First();
            oEmployeeBonus.Company.CompanyLogo = GetCompanyLogo(oEmployeeBonus.Company);



            rptMultiMonthPaySlipForBonus oReport = new rptMultiMonthPaySlipForBonus();
            byte[] abytes = oReport.PrepareReport(oEmployeeBonus);
            return File(abytes, "application/pdf");
        }
    }
}
