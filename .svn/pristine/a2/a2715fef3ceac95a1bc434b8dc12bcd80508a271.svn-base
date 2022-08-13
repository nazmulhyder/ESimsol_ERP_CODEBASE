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
using System.Data;
using System.Data.OleDb;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;
using CrystalDecisions.CrystalReports.Engine;


namespace ESimSolFinancial.Controllers
{
    public class EmployeeIDCardController : PdfViewController
    {
        #region Declaration
        Employee _oEmployee = new Employee();
        List<Employee> _oEmployees = new List<Employee>();
        EmployeeGroup _oEmployeeGroup = new EmployeeGroup();
        List<EmployeeGroup> _oEmployeeGroups = new List<EmployeeGroup>();
        record rec = new record();
        recordInactive recInactive = new recordInactive();
        List<record> showSummery = new List<record>();
        List<recordInactive> showSummeryInactive = new List<recordInactive>();
        private string _sSQL = "";
        EmployeeSalaryStructure _oEmployeeSalaryStructure;

        #endregion
        public ActionResult View_EmployeeIDCard(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oEmployees = new List<Employee>();

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalary).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            ViewBag.EmployeeTypes = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x => x.Value != 0.ToString()).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            string sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSql = sSql + ")";

            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
            //ViewBag.IDCardFormats = EnumObject.jGets(typeof(EnumIDCardFormat));
            ViewBag.IDCardFormats = GetPermissionList();
            return View(_oEmployees);
        }
        public List<EnumObject> GetPermissionList()
        {
            List<EnumObject> oEnumObjects = new List<EnumObject>();
            List<EnumObject> objEnumObjects = new List<EnumObject>();

            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.IDCardFormat).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            foreach (AuthorizationRoleMapping oItem in oAuthorizationRoleMappings)
            {
                if (oItem.OperationType == EnumRoleOperationType.IDCardProtrait)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumIDCardFormat.Protrait;
                    oEnumObject.Value = EnumObject.jGet(EnumIDCardFormat.Protrait);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.IDCardLandscape)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumIDCardFormat.Landscape;
                    oEnumObject.Value = EnumObject.jGet(EnumIDCardFormat.Landscape);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.IDCardBothSideProtrait)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumIDCardFormat.Both_Side_Protrait;
                    oEnumObject.Value = EnumObject.jGet(EnumIDCardFormat.Both_Side_Protrait);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.IDCardBothSideBangla)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumIDCardFormat.Both_Side_Bangla;
                    oEnumObject.Value = EnumObject.jGet(EnumIDCardFormat.Both_Side_Bangla);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.IDCardBanglaF1)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumIDCardFormat.ID_Card_Bangla_F1;
                    oEnumObject.Value = EnumObject.jGet(EnumIDCardFormat.ID_Card_Bangla_F1);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.IDCardBanglaF2)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumIDCardFormat.ID_Card_Bangla_F2;
                    oEnumObject.Value = EnumObject.jGet(EnumIDCardFormat.ID_Card_Bangla_F2);
                    objEnumObjects.Add(oEnumObject);
                }
            }
            return objEnumObjects;
        }
        #region bothSideBangla
        public void PrintIDCardWithSearchingCriteriaTest(string sEmpIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, DateTime expDate, int nRowLength, int nLoadRecords, string sGroupIDs, string sBlockIDs)
        {
            List<EmployeeForIDCard> _oEmployeeForIDCards = new List<EmployeeForIDCard>();
            string sSql = "SELECT top(" + nLoadRecords + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY Code) Row,* FROM View_Employee_For_IDCard WHERE EmployeeID <>0";

            //string sSql = "select * from View_Employee_For_IDCard WHERE EmployeeID<>0";
            if (!string.IsNullOrEmpty(sEmpIDs))
            {
                sSql += " AND EmployeeID IN(" + sEmpIDs + ")";
            }
            if (!string.IsNullOrEmpty(sBusinessUnitIds))
            {
                sSql += " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
            }
            if (!string.IsNullOrEmpty(sLocationID))
            {
                sSql += " AND LocationID IN(" + sLocationID + ")";
            }
            if (!string.IsNullOrEmpty(sDepartmentIds))
            {
                sSql += " AND DepartmentID IN(" + sDepartmentIds + ")";
            }
            if (!string.IsNullOrEmpty(sDesignationIds))
            {
                sSql += " AND DesignationID IN(" + sDesignationIds + ")";
            }
            if (!string.IsNullOrEmpty(sGroupIDs))
            {
                sSql += " AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(" + sGroupIDs + "))";
            }
            if (!string.IsNullOrEmpty(sBlockIDs))
            {
                sSql += " AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(" + sBlockIDs + "))";
            }
            sSql = sSql + ") aa WHERE Row >" + nRowLength + " Order By Code";

            _oEmployeeForIDCards = EmployeeForIDCard.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            foreach (EmployeeForIDCard item in _oEmployeeForIDCards)
            {
                string sQuery = "select TOP(1)* from HRResponsibility where HRRID IN(SELECT HRResponsibilityID FROM DesignationResponsibility WHERE DRPID=" + item.DRPID + " AND DesignationID =" + item.DesignationID + ") ";
                item.HRResponsibility = HRResponsibility.Gets(sQuery, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            Company oCompany = new Company();
            _oEmployee.Company = oCompany.GetCompanyLogo(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);
            _oEmployee.Company.AuthSig = GetImage(_oEmployee.Company.AuthorizedSignature);

            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            int nRowIndex = 0, nEndCol = 16, nColumnIndex = 2;
            double nRowHeight = 11;
            if (_oEmployeeForIDCards.Count <= 0)
            {
                nRowIndex = 1;
            }

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("ID Card");
                sheet.PrinterSettings.TopMargin = 0.2M;
                sheet.PrinterSettings.LeftMargin = 0.2M;
                sheet.PrinterSettings.BottomMargin = 0.2M;
                sheet.PrinterSettings.RightMargin = 0.2M;
                //sheet.PrinterSettings.TopMargin = 0;
                //sheet.PrinterSettings.LeftMargin = 0;
                //sheet.PrinterSettings.BottomMargin = 0;
                //sheet.PrinterSettings.RightMargin = 0;
                sheet.PrinterSettings.Orientation = eOrientation.Portrait;
                sheet.PrinterSettings.PaperSize = ePaperSize.A4;
                sheet.Name = "ID Card";

                #region Column Declare
                sheet.Column(1).Width = 0;//Extra
                sheet.Column(2).Width = 3; //Blank
                sheet.Column(3).Width = 8;//Image
                sheet.Column(4).Width = 3;//Blank
                sheet.Column(5).Width = 10;//caption
                sheet.Column(6).Width = 2; //:
                sheet.Column(7).Width = 16;//value
                sheet.Column(8).Width = 1;//Blank

                sheet.Column(9).Width = 2; //Middle Blank

                sheet.Column(10).Width = 7;//Caption
                sheet.Column(11).Width = 1; //:
                sheet.Column(12).Width = 12;//Value
                sheet.Column(13).Width = 11;//Caption
                sheet.Column(14).Width = 1;//:
                sheet.Column(15).Width = 10;//Value
                sheet.Column(16).Width = 1;//Blank
                #endregion

                int nCount = 0;
                int comCounter = 0;
                int empCounter = 0;
                int comSigCount = 0;
                if (_oEmployeeForIDCards.Count > 0)
                {
                    int nStartEmpIndex = 0;
                    for (int i = 0; i < _oEmployeeForIDCards.Count; i++)
                    {
                        comCounter++;
                        empCounter++;
                        comSigCount++;

                        nStartEmpIndex = nRowIndex;
                        nColumnIndex = 2;
                        nRowIndex = nRowIndex + 1;
                        #region Blank with Top/Left/Right Border
                        //if (i > 0)
                        //{
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;
                        //}
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = border.Top.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region front side
                        //1st row
                        #region Company image

                        sheet.Row(nRowIndex).Height = 30;
                        //sheet.Row(nRowIndex).Height = nRowHeight;
                        //sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 4)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = 30;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)];
                        cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;

                        ExcelPicture excelImage = null;
                        excelImage = sheet.Drawings.AddPicture((++comCounter).ToString(), _oEmployee.Company.CompanyLogo);
                        excelImage.From.Column = nColumnIndex;
                        excelImage.From.Row = nRowIndex - 2;
                        excelImage.SetSize(250, 25);
                        excelImage.From.ColumnOff = this.Pixel2MTU(2);
                        excelImage.From.RowOff = this.Pixel2MTU(2);

                        sheet.Row(nRowIndex).Height = 30;
                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        //2nd row
                        #region শ্রমিকের পরিচয়পত্র
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)]; cell.Value = "\"শ্রমিকের পরিচয়পত্র\"";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;
                        #endregion


                        //3rd row
                        #region image


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, (nColumnIndex), (nRowIndex + 7), (nColumnIndex)].Merge = true;
                        cell = sheet.Cells[nRowIndex, (nColumnIndex), (nRowIndex + 7), (nColumnIndex)]; cell.Value = "ইস্যুর তারিখঃ " + this.StringFormat(DateTime.Now.ToString("dd/MM/yyyy"));
                        cell.Style.Font.Size = 5; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.TextRotation = 90;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;



                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "প্রতিষ্ঠানের নাম";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].BusinessUnitNameInBangla)) ? _oEmployeeForIDCards[i].BusinessUnitNameInBangla : _oEmployeeForIDCards[i].BusinessUnitName;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;


                        //4th row
                        if (_oEmployeeForIDCards[i].EmployeePhoto != null)
                        {
                            sheet.Row(nRowIndex).Height = nRowHeight;
                            sheet.Cells[nRowIndex, (nColumnIndex + 1), (nRowIndex + 3), (nColumnIndex + 1)].Merge = true;
                            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1), nRowIndex + 3, (nColumnIndex + 1)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                            excelImage = null;
                            excelImage = sheet.Drawings.AddPicture(_oEmployeeForIDCards[i].EmployeeID.ToString(), _oEmployeeForIDCards[i].EmployeePhoto);
                            excelImage.From.Column = nColumnIndex;
                            excelImage.From.Row = nRowIndex - 1;
                            excelImage.SetSize(60, 60);
                            excelImage.From.ColumnOff = this.Pixel2MTU(2);
                            excelImage.From.RowOff = this.Pixel2MTU(2);
                        }
                        else
                        {
                            sheet.Row(nRowIndex).Height = nRowHeight;
                            sheet.Cells[nRowIndex, (nColumnIndex + 1), (nRowIndex + 3), (nColumnIndex + 1)].Merge = true;
                            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1), nRowIndex + 3, (nColumnIndex + 1)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        }

                        sheet.Cells[nRowIndex, (nColumnIndex + 2), (nRowIndex + 6), (nColumnIndex + 2)].Merge = true;
                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 2), (nRowIndex + 6), (nColumnIndex + 2)]; cell.Value = "";
                        cell.Style.Font.Size = 5; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.TextRotation = 90;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.None; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;



                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "শ্রমিকের নাম";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].NameInBangla)) ? _oEmployeeForIDCards[i].NameInBangla : _oEmployeeForIDCards[i].Name;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;

                        //5th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "আই ডি কার্ড নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = _oEmployeeForIDCards[i].Code;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;


                        //6th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "পদবী";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].DesignationNameInBangla)) ? _oEmployeeForIDCards[i].DesignationNameInBangla : _oEmployeeForIDCards[i].DesignationName;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;


                        //7th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "বিভাগ/শাখা";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].DepartmentNameInBangla)) ? _oEmployeeForIDCards[i].DepartmentNameInBangla : _oEmployeeForIDCards[i].DepartmentName;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;


                        //8th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "যোগদানের তারিখ";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = this.DateFormat(_oEmployeeForIDCards[i].JoiningDate.ToString("dd MMM yyyy"));
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;

                        //9th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "কাজের ধরন";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        string sStringWorkType = "";

                        if (_oEmployeeForIDCards[i].HRResponsibility.Count > 0)
                        {
                            sStringWorkType = _oEmployeeForIDCards[i].HRResponsibility[0].DescriptionInBangla;
                        }

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = sStringWorkType;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;


                        //10th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "ফ্লোর";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].LocationNameInBangla)) ? _oEmployeeForIDCards[i].LocationNameInBangla : _oEmployeeForIDCards[i].LocationName;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;

                        //11th row
                        sheet.Row(nRowIndex).Height = 20;
                        cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
                        cell.Style.Font.Size = 9; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        if (_oEmployeeForIDCards[i].EmployeeSiganture != null)
                        {
                            sheet.Row(nRowIndex).Height = 20;
                            sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 3)].Merge = true;
                            cell = sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


                            excelImage = null;
                            excelImage = sheet.Drawings.AddPicture((empCounter += 2).ToString(), _oEmployeeForIDCards[i].EmployeeSiganture);
                            excelImage.From.Column = nColumnIndex;
                            excelImage.From.Row = nRowIndex - 1;
                            excelImage.SetSize(125, 20);
                            excelImage.From.ColumnOff = this.Pixel2MTU(2);
                            excelImage.From.RowOff = this.Pixel2MTU(2);
                        }
                        else
                        {
                            sheet.Row(nRowIndex).Height = 20;
                            sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 3)].Merge = true;
                            cell = sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        }

                        //sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 2)].Merge = true;
                        //cell = sheet.Cells[nRowIndex, nColumnIndex + 1, nRowIndex, (nColumnIndex + 2)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;


                        if (_oEmployee.Company.AuthSig != null)
                        {
                            sheet.Row(nRowIndex).Height = 20;
                            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


                            excelImage = null;
                            excelImage = sheet.Drawings.AddPicture(_oEmployeeForIDCards[i].NameCode, _oEmployee.Company.AuthSig);
                            excelImage.From.Column = nColumnIndex + 3;
                            excelImage.From.Row = nRowIndex - 1;
                            excelImage.SetSize(125, 20);
                            excelImage.From.ColumnOff = this.Pixel2MTU(1);
                            excelImage.From.RowOff = this.Pixel2MTU(1);
                        }
                        else
                        {
                            sheet.Row(nRowIndex).Height = 20;
                            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                            cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        }

                        //cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Right.Style = border.Top.Style = border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = 20;
                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;



                        //12th row

                        cell = sheet.Cells[nRowIndex, (nColumnIndex), nRowIndex, (nColumnIndex)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 1), nRowIndex, nColumnIndex + 4]; cell.Value = "শ্রমিকের সাক্ষর"; cell.Merge = true;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.Thin;

                        //cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        //border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.Thin;

                        //cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = "মালিক/ব্যবস্থাপক";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = ExcelBorderStyle.None; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        //nRowIndex = nRowIndex + 1;

                        //sheet.Row(nRowIndex).Height = nRowHeight;
                        //sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)].Merge = true;
                        //cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        //cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Right.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        //nRowIndex = nRowIndex + 1;

                        #endregion
                        #endregion

                        nRowIndex = nStartEmpIndex;
                        nColumnIndex = 10;
                        nRowIndex = nRowIndex + 1;

                        #region back side
                        //if(i > 0)
                        //{
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;
                        // }


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = ExcelBorderStyle.None;

                        nRowIndex = nRowIndex + 1;

                        #region প্রতিষ্ঠানের ঠিকানা
                        sheet.Row(nRowIndex).Height = 13;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 2)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 2)]; cell.Value = "প্রতিষ্ঠানের ঠিকানা : ";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = 13;
                        sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex, (nColumnIndex + 6)]; cell.Value = "শ্রমিকের স্থায়ী ঠিকানা : ";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region Address
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 2, (nColumnIndex + 2)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 2, (nColumnIndex + 2)]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].BUAddressInBangla)) ? _oEmployeeForIDCards[i].BUAddressInBangla : _oEmployeeForIDCards[i].BUAddress;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3]; cell.Value = "গ্রাম";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].Village)) ? _oEmployeeForIDCards[i].Village : "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;



                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3]; cell.Value = "পোস্ট";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].PostOffice)) ? _oEmployeeForIDCards[i].PostOffice : "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3]; cell.Value = "থানা";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].Thana)) ? _oEmployeeForIDCards[i].Thana : "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;


                        //

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "টেলিফোন নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, nColumnIndex + 1]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, nColumnIndex + 2]; cell.Value = this.StringFormat(_oEmployeeForIDCards[i].BUPhone);
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3]; cell.Value = "জেলা";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].District)) ? _oEmployeeForIDCards[i].District : "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 1;

                        //
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex]; cell.Value = "ফ্যাক্স নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, nColumnIndex + 1]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2]; cell.Value = this.StringFormat(_oEmployeeForIDCards[i].BUFaxNo);
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3]; cell.Value = "জরুরী যোগাযোগের ফোন নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = (!string.IsNullOrEmpty(_oEmployeeForIDCards[i].OtherPhoneNo)) ? this.StringFormat(_oEmployeeForIDCards[i].OtherPhoneNo) : "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 2;


                        //
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex]; cell.Value = "রক্তের গ্রুপ";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, nColumnIndex + 1]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2]; cell.Value = _oEmployeeForIDCards[i].BloodGroupST;
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3]; cell.Value = "জাতীয় পরিচয় পত্র নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = this.StringFormat(_oEmployeeForIDCards[i].NationalID);
                        cell.Style.Font.Size = 6; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 2;

                        //
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex]; cell.Value = "কার্ডের মেয়াদ";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, nColumnIndex + 1]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;


                        sheet.Row(nRowIndex).Height = 9;
                        sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 2, nRowIndex + 1, nColumnIndex + 2]; cell.Value = this.DateFormat(expDate.ToString("dd MMM yyyy"));
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 3, nRowIndex + 1, nColumnIndex + 3]; cell.Value = "প্রক্সিমিটি নং";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 4, nRowIndex + 1, nColumnIndex + 4].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 4]; cell.Value = ":";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex + 5, nRowIndex + 1, nColumnIndex + 5].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 5]; cell.Value = this.StringFormat(_oEmployeeForIDCards[i].CardNo);
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;

                        sheet.Row(nRowIndex).Height = 9;
                        sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex + 6, nRowIndex + 1, nColumnIndex + 6]; cell.Value = "";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                        nRowIndex = nRowIndex + 2;

                        //

                        //
                        sheet.Row(nRowIndex).Height = nRowHeight;
                        sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "উক্ত পরিচয়পত্র হারিয়ে গেলে তাৎক্ষনিক ব্যবস্থাপনা কর্তৃপক্ষকে জানাতে হইবে";
                        cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Bottom.Style = border.Top.Style = ExcelBorderStyle.Thin;
                        //nRowIndex = nRowIndex + 1;

                        #endregion
                        //sheet.Row(nRowIndex).Height = nRowHeight;
                        //sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
                        //cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Bottom.Style = ExcelBorderStyle.None;


                        //sheet.Row(nRowIndex).Height = nRowHeight;
                        //sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)].Merge = true;
                        //cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 5)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;

                        //cell = sheet.Cells[nRowIndex, (nColumnIndex + 5)]; cell.Value = "";
                        //cell.Style.Font.Size = 7; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //border = cell.Style.Border; border.Right.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        //nRowIndex = nRowIndex + 1;

                        //nRowIndex = nRowIndex + 1;

                        //if (nRowIndex % 75 == 0)
                        //{
                        //    PageBreak(ref sheet, nRowIndex, nEndCol);
                        //}

                        if (nRowIndex % 56 == 0)
                        {
                            PageBreak(ref sheet, nRowIndex, nEndCol);
                        }
                        #endregion
                    }
                }
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=IDCard.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void PageBreak(ref ExcelWorksheet sheet, int nRowIndex, int nEndCol)
        {
            sheet.Row(nRowIndex).PageBreak = true;
            sheet.Row(nEndCol).PageBreak = true;
        }

        public string StringFormat(string sNum)
        {
            char[] NumbersInBangla = { '০', '১', '২', '৩', '৪', '৫', '৬', '৭', '৮', '৯', '-', '/', '+', '(', ')' };
            char[] NumbersInEnglish = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '/', '+', '(', ')' };

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

        public string DateFormat(string sDate)
        {
            string banglaDate = "";
            string[] DateInEnglish = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            string[] DateInBangla = { "জানু.", "ফেব্রু.", "মার্চ", "এপ্রিল", "মে", "জুন", "জুলাই", "অগাস্ট", "সেপ্টেম্বর", "অক্টোবর", "নভে.", "ডিসে." };

            string[] arr = sDate.Split(' ');

            banglaDate = this.NumberFormat(arr[0]);

            foreach (string st in arr)
            {
                int i = 0;
                while (i != 12)
                {
                    if (st == DateInEnglish[i])
                    {
                        banglaDate = banglaDate + " " + DateInBangla[i];
                        break;
                    }
                    i++;
                }
            }
            return banglaDate + " " + this.NumberFormat(arr[2]);
        }

        public int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }
        public string NumberFormat(string sNum)
        {
            char[] NumbersInBangla = { '০', '১', '২', '৩', '৪', '৫', '৬', '৭', '৮', '৯' };
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

        #endregion

        #region potrait
        public ActionResult PrintEmployeeCard_Potrait(string sEmpIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, DateTime expDate, int nRowLength, int nLoadRecords, string sGroupIDs, string sBlockIDs, string itemIDs)
        {
            _oEmployee = new Employee();
            //string sSql = "select * from View_Employee_WithImage WHERE EmployeeID IN(" + sIDs + ")";
            string sSql = "SELECT top(" + nLoadRecords + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY Code) Row,* FROM View_Employee_For_IDCard_WithImage WHERE EmployeeID <>0";

            if (!string.IsNullOrEmpty(sEmpIDs))
            {
                sSql += " AND EmployeeID IN(" + sEmpIDs + ")";
            }
            if (!string.IsNullOrEmpty(sBusinessUnitIds))
            {
                sSql += " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
            }
            if (!string.IsNullOrEmpty(sLocationID))
            {
                sSql += " AND LocationID IN(" + sLocationID + ")";
            }
            if (!string.IsNullOrEmpty(sDepartmentIds))
            {
                sSql += " AND DepartmentID IN(" + sDepartmentIds + ")";
            }
            if (!string.IsNullOrEmpty(sDesignationIds))
            {
                sSql += " AND DesignationID IN(" + sDesignationIds + ")";
            }
            if (!string.IsNullOrEmpty(sGroupIDs))
            {
                sSql += " AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(" + sGroupIDs + "))";
            }
            if (!string.IsNullOrEmpty(sBlockIDs))
            {
                sSql += " AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(" + sBlockIDs + "))";
            }
            sSql = sSql + ") aa WHERE Row >" + nRowLength + " AND IsActive = 1 Order By Code";
            _oEmployee.EmployeeHrms = Employee.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            Company oCompany = new Company();
            _oEmployee.Company = oCompany.GetCompanyLogo(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);
            _oEmployee.Company.AuthSig = GetImage(_oEmployee.Company.AuthorizedSignature);
            foreach (Employee oItem in _oEmployee.EmployeeHrms)
            {
                oItem.EmployeePhoto = GetImage(oItem.Photo);
                oItem.EmployeeSiganture = GetImage(oItem.Signature);
            }
            _oEmployee.ErrorMessage = itemIDs;
            rptEmployeeCard_Potrait oReport = new rptEmployeeCard_Potrait();
            byte[] abytes = oReport.PrepareReport(_oEmployee);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region landscape
        public ActionResult PrintEmployeeCard_Landscape(string sEmpIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, DateTime expDate, int nRowLength, int nLoadRecords, string sGroupIDs, string sBlockIDs)
        {
            _oEmployee = new Employee();
            string sSql = "SELECT top(" + nLoadRecords + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY Code) Row,* FROM View_Employee_For_IDCard_WithImage WHERE EmployeeID <>0";

            if (!string.IsNullOrEmpty(sEmpIDs))
            {
                sSql += " AND EmployeeID IN(" + sEmpIDs + ")";
            }
            if (!string.IsNullOrEmpty(sBusinessUnitIds))
            {
                sSql += " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
            }
            if (!string.IsNullOrEmpty(sLocationID))
            {
                sSql += " AND LocationID IN(" + sLocationID + ")";
            }
            if (!string.IsNullOrEmpty(sDepartmentIds))
            {
                sSql += " AND DepartmentID IN(" + sDepartmentIds + ")";
            }
            if (!string.IsNullOrEmpty(sDesignationIds))
            {
                sSql += " AND DesignationID IN(" + sDesignationIds + ")";
            }
            if (!string.IsNullOrEmpty(sGroupIDs))
            {
                sSql += " AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(" + sGroupIDs + "))";
            }
            if (!string.IsNullOrEmpty(sBlockIDs))
            {
                sSql += " AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(" + sBlockIDs + "))";
            }
            sSql = sSql + ") aa WHERE Row >" + nRowLength + " AND IsActive = 1 Order By Code";
            _oEmployee.EmployeeHrms = Employee.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                        
            Company oCompany = new Company();
            _oEmployee.Company = oCompany.GetCompanyLogo(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployee.Company.AuthSig = GetImage(_oEmployee.Company.AuthorizedSignature);
            //_oEmployee.Company.CompanyLogo = null; //GetImage(_oEmployee.Company.OrganizationLogo); ;
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);
            //_oEmployee.Company.AuthSig = GetImage(_oEmployee.Company.AuthorizedSignature);
            foreach (Employee oItem in _oEmployee.EmployeeHrms)
            {
                oItem.EmployeePhoto = GetImage(oItem.Photo);
                oItem.EmployeeSiganture = GetImage(oItem.Signature);
            }

            rptEmployeeCard oReport = new rptEmployeeCard();
            byte[] abytes = oReport.PrepareReport(_oEmployee, DateTime.Today, expDate);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region bothsidepotrait
        public ActionResult PrintEmployeeCard_Potrait_BothSide(string sEmpIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, DateTime expDate, int nRowLength, int nLoadRecords, string sGroupIDs, string sBlockIDs, string itemIDs)
        {
            _oEmployee = new Employee();
            string sSql = "SELECT top(" + nLoadRecords + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY Code) Row,* FROM View_Employee_For_IDCard_WithImage WHERE EmployeeID <>0";

            if (!string.IsNullOrEmpty(sEmpIDs))
            {
                sSql += " AND EmployeeID IN(" + sEmpIDs + ")";
            }
            if (!string.IsNullOrEmpty(sBusinessUnitIds))
            {
                sSql += " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
            }
            if (!string.IsNullOrEmpty(sLocationID))
            {
                sSql += " AND LocationID IN(" + sLocationID + ")";
            }
            if (!string.IsNullOrEmpty(sDepartmentIds))
            {
                sSql += " AND DepartmentID IN(" + sDepartmentIds + ")";
            }
            if (!string.IsNullOrEmpty(sDesignationIds))
            {
                sSql += " AND DesignationID IN(" + sDesignationIds + ")";
            }
            if (!string.IsNullOrEmpty(sGroupIDs))
            {
                sSql += " AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(" + sGroupIDs + "))";
            }
            if (!string.IsNullOrEmpty(sBlockIDs))
            {
                sSql += " AND EmployeeID IN( SELECT EmployeeID From View_EmployeeGroup WHERE EmployeeTypeID IN(" + sBlockIDs + "))";
            }
            sSql = sSql + ") aa WHERE Row >" + nRowLength + " AND IsActive = 1 Order By Code";
            _oEmployee.EmployeeHrms = Employee.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            Company oCompany = new Company();
            _oEmployee.Company = oCompany.GetCompanyLogo(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployee.Company.CompanyLogo = GetImage(_oEmployee.Company.OrganizationLogo);
            _oEmployee.Company.AuthSig = GetImage(_oEmployee.Company.AuthorizedSignature);
            foreach (Employee oItem in _oEmployee.EmployeeHrms)
            {
                oItem.EmployeePhoto = GetImage(oItem.Photo);
                oItem.EmployeeSiganture = GetImage(oItem.Signature);
            }
            _oEmployee.ErrorMessage = itemIDs;
            rptEmployeeCard_Potrait_BothSide oReport = new rptEmployeeCard_Potrait_BothSide();
            byte[] abytes = oReport.PrepareReport(_oEmployee, DateTime.Today, expDate);
            return File(abytes, "application/pdf");
        }
        #endregion

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(EmployeeForIDCard oEmployeeForIDCard)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oEmployeeForIDCard);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Crystal Report Bangla ID Card
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
        public ActionResult IDCardBanglaBothSide(string sString)
        {
            List<EmployeeForIDCard> _oEmployeeForIDCards = new List<EmployeeForIDCard>();
            EmployeeForIDCard _oEmployeeForIDCard = new EmployeeForIDCard();
            _oEmployeeForIDCard = (EmployeeForIDCard)Session[SessionInfo.ParamObj];

            string sSql = "SELECT * FROM View_Employee_For_IDCard_WithImage WHERE EmployeeID IN (" + _oEmployeeForIDCard.Name + ")";
            _oEmployeeForIDCards = EmployeeForIDCard.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            Company oCompany = new Company();
            List<Company> oCompanys = new List<Company>();
            oCompany = oCompany.GetCompanyLogo(1, (int)(Session[SessionInfo.currentUserID]));            
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oCompany.AuthSig = GetImage(oCompany.AuthorizedSignature, "AuthSig.jpg");            
            oCompanys.Add(oCompany);

            var folderPath = Server.MapPath("~/Content/Images/Employee");
            Directory.CreateDirectory(folderPath);

            foreach (EmployeeForIDCard oEmployeeForIDCard in _oEmployeeForIDCards)
            {
                if (oCompany.AuthSig != null)
                {
                    oEmployeeForIDCard.AuthSigPath = Server.MapPath("~/Content/Images/AuthSig.jpg");
                }
                oEmployeeForIDCard.CompanyLogoPath = Server.MapPath("~/Content/CompanyLogo.jpg");

                oEmployeeForIDCard.EmployeePhoto = GetImage(oEmployeeForIDCard.Photo, "Employee/" + oEmployeeForIDCard.Code + ".jpg");
                oEmployeeForIDCard.EmployeePhotoPath = Server.MapPath("~/Content/Images/Employee/" + oEmployeeForIDCard.Code + ".jpg");

                oEmployeeForIDCard.EmployeeSiganture = GetImage(oEmployeeForIDCard.Signature, "Employee/" + oEmployeeForIDCard.Code + "-sig.jpg");
                oEmployeeForIDCard.EmployeeSiganturePath = Server.MapPath("~/Content/Images/Employee/" + oEmployeeForIDCard.Code + "-sig.jpg");

            }
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "BanglaIDCardBothSide.rpt"));
            rd.Database.Tables["EmployeeForIDCard"].SetDataSource(_oEmployeeForIDCards);
            rd.Database.Tables["Company"].SetDataSource(oCompanys);
            //rd.SetDataSource(_oEmployeeForIDCards);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                //string[] months = new string[] { "জানুয়ারী", "ফেব্রুয়ারি", "মার্চ", "এপ্রিল", "মে", "জুন", "জুলাই", "অগাস্ট", "সেপ্টেম্বর", "অক্টোবর", "নভেম্বর", "ডিসেম্বর" };
                //string[] months = new string[] { "Rvbyqvix", "‡deªyqvwi", "gvP©", "GwcÖj", "‡g", "Ryb", "RyjvB", "AMvó", "‡m‡Þ¤^i", "A‡±vei", "b‡f¤^i", "wW‡m¤^i" };
                //string sMonthName = months[nMonthID - 1] + " " + NumberFormatWithBijoy(nYear.ToString());

                //TextObject txtExpDate = (TextObject)rd.ReportDefinition.Sections["Section3"].ReportObjects["txtExpDate"];
                //txtExpDate.Text = expDate.ToString("dd/MM/yyyy");

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                Directory.Delete(folderPath, true);
                return File(stream, "application/pdf");
            }
            catch { throw; }
        }

        public ActionResult IDCardBanglaF1(string sIDs, string itemIDs, string sIssueDate, string sExpireDate, double ts)
        {
            DateTime dIssueDate = Convert.ToDateTime(sIssueDate);
            DateTime dExpireDate = Convert.ToDateTime(sExpireDate);
            List<EmployeeForIDCard> _oEmployeeForIDCards = new List<EmployeeForIDCard>();
            EmployeeForIDCard _oEmployeeForIDCard = new EmployeeForIDCard();

            string sSql = "SELECT * FROM View_Employee_For_IDCard_WithImage WHERE EmployeeID IN(" + sIDs + ")";
            _oEmployeeForIDCards = EmployeeForIDCard.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            Company oCompany = new Company();
            List<Company> oCompanys = new List<Company>();
            oCompany = oCompany.GetCompanyLogo(1, (int)(Session[SessionInfo.currentUserID]));
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oCompany.AuthSig = GetImage(oCompany.AuthorizedSignature, "AuthSig.jpg");
            oCompanys.Add(oCompany);

            var folderPath = Server.MapPath("~/Content/Images/Employee");
            Directory.CreateDirectory(folderPath);

            foreach (EmployeeForIDCard oEmployeeForIDCard in _oEmployeeForIDCards)
            {
                if (oCompany.AuthSig!=null)
                {
                    oEmployeeForIDCard.AuthSigPath = Server.MapPath("~/Content/Images/AuthSig.jpg");
                }
                oEmployeeForIDCard.CompanyLogoPath = Server.MapPath("~/Content/CompanyLogo.jpg");

                oEmployeeForIDCard.EmployeePhoto = GetImage(oEmployeeForIDCard.Photo, "Employee/" + oEmployeeForIDCard.Code +".jpg");
                oEmployeeForIDCard.EmployeePhotoPath = Server.MapPath("~/Content/Images/Employee/" + oEmployeeForIDCard.Code + ".jpg");

                oEmployeeForIDCard.EmployeeSiganture = GetImage(oEmployeeForIDCard.Signature, "Employee/" + oEmployeeForIDCard.Code + "-sig.jpg");
                oEmployeeForIDCard.EmployeeSiganturePath = Server.MapPath("~/Content/Images/Employee/" + oEmployeeForIDCard.Code + "-sig.jpg");
            }
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "IDCardBanglaF1.rpt"));            
            rd.Database.Tables["EmployeeForIDCard"].SetDataSource(_oEmployeeForIDCards);
            rd.Database.Tables["Company"].SetDataSource(oCompanys);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                //string[] months = new string[] { "জানুয়ারী", "ফেব্রুয়ারি", "মার্চ", "এপ্রিল", "মে", "জুন", "জুলাই", "অগাস্ট", "সেপ্টেম্বর", "অক্টোবর", "নভেম্বর", "ডিসেম্বর" };
                //string[] months = new string[] { "Rvbyqvix", "‡deªyqvwi", "gvP©", "GwcÖj", "‡g", "Ryb", "RyjvB", "AMvó", "‡m‡Þ¤^i", "A‡±vei", "b‡f¤^i", "wW‡m¤^i" };
                //string sMonthName = months[nMonthID - 1] + " " + NumberFormatWithBijoy(nYear.ToString());
                //TextObject txtExpDate = (TextObject)rd.ReportDefinition.Sections["Section3"].ReportObjects["txtExpDate"];
                //txtExpDate.Text = expDate.ToString("dd/MM/yyyy");
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                Directory.Delete(folderPath, true);
                return File(stream, "application/pdf");
            }
            catch { throw; }
        }
        public ActionResult IDCardBanglaF2(string sIDs, string itemIDs, string sIssueDate, string sExpireDate, double ts)
        {
            DateTime dIssueDate = Convert.ToDateTime(sIssueDate);
            DateTime dExpireDate = Convert.ToDateTime(sExpireDate);
            List<EmployeeForIDCard> _oEmployeeForIDCards = new List<EmployeeForIDCard>();
            EmployeeForIDCard _oEmployeeForIDCard = new EmployeeForIDCard();

            string sSql = "SELECT * FROM View_Employee_For_IDCard_WithImage WHERE EmployeeID IN(" + sIDs + ")";
            _oEmployeeForIDCards = EmployeeForIDCard.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            Company oCompany = new Company();
            List<Company> oCompanys = new List<Company>();
            oCompany = oCompany.GetCompanyLogo(1, (int)(Session[SessionInfo.currentUserID]));
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oCompany.AuthSig = GetImage(oCompany.AuthorizedSignature, "AuthSig.jpg");
            oCompanys.Add(oCompany);

            var folderPath = Server.MapPath("~/Content/Images/Employee");
            Directory.CreateDirectory(folderPath);

            foreach (EmployeeForIDCard oEmployeeForIDCard in _oEmployeeForIDCards)
            {
                if (oCompany.AuthSig != null)
                {
                    oEmployeeForIDCard.AuthSigPath = Server.MapPath("~/Content/Images/AuthSig.jpg");
                }
                oEmployeeForIDCard.AuthSigPath = Server.MapPath("~/Content/Images/AuthSig.jpg");
                oEmployeeForIDCard.CompanyLogoPath = Server.MapPath("~/Content/CompanyLogo.jpg");

                oEmployeeForIDCard.EmployeePhoto = GetImage(oEmployeeForIDCard.Photo, "Employee/" + oEmployeeForIDCard.Code + ".jpg");
                oEmployeeForIDCard.EmployeePhotoPath = Server.MapPath("~/Content/Images/Employee/" + oEmployeeForIDCard.Code + ".jpg");

                oEmployeeForIDCard.EmployeeSiganture = GetImage(oEmployeeForIDCard.Signature, "Employee/" + oEmployeeForIDCard.Code + "-sig.jpg");
                oEmployeeForIDCard.EmployeeSiganturePath = Server.MapPath("~/Content/Images/Employee/" + oEmployeeForIDCard.Code + "-sig.jpg");
            }
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "IDCardBanglaF2.rpt"));
            rd.Database.Tables["EmployeeForIDCard"].SetDataSource(_oEmployeeForIDCards);
            rd.Database.Tables["Company"].SetDataSource(oCompanys);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                //string[] months = new string[] { "জানুয়ারী", "ফেব্রুয়ারি", "মার্চ", "এপ্রিল", "মে", "জুন", "জুলাই", "অগাস্ট", "সেপ্টেম্বর", "অক্টোবর", "নভেম্বর", "ডিসেম্বর" };
                //string[] months = new string[] { "Rvbyqvix", "‡deªyqvwi", "gvP©", "GwcÖj", "‡g", "Ryb", "RyjvB", "AMvó", "‡m‡Þ¤^i", "A‡±vei", "b‡f¤^i", "wW‡m¤^i" };
                //string sMonthName = months[nMonthID - 1] + " " + NumberFormatWithBijoy(nYear.ToString());
                //TextObject txtExpDate = (TextObject)rd.ReportDefinition.Sections["Section3"].ReportObjects["txtExpDate"];
                //txtExpDate.Text = expDate.ToString("dd/MM/yyyy");
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf");
            }
            catch { throw; }
        }
        public void GetCompanyAuthSig(Company oCompany)
        {
            if (oCompany.AuthorizedSignature != null)
            {
                MemoryStream m = new MemoryStream(oCompany.AuthorizedSignature);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(Server.MapPath("~/Content/Auth") + "CompanyAuthSig.jpg", ImageFormat.Jpeg);
                
            }
        }
        public Image GetImage(byte[] Image, string sImageName = "Image.jpg")
        {
            if (Image != null)
            {
                string fileDirectory = Server.MapPath("~/Content/Images/" + sImageName);
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
        #endregion
    }
}