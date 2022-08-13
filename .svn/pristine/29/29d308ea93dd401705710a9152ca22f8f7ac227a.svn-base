using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using iTextSharp.text;
using ESimSol.Reports;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ReportManagement;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using CrystalDecisions.CrystalReports.Engine;


namespace ESimSolFinancial.Controllers
{
    public class LetterSetupController : PdfViewController
    {
        #region Declaration
        LetterSetup _oLetterSetup;
        List<LetterSetup> _oLetterSetups;
        LetterSetupEmployee _oLetterSetupEmployee;
        List<LetterSetupEmployee> _oLetterSetupEmployees;
        int AHID;
        
        #endregion

        #region Views
        public ActionResult ViewLetterSetups(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oLetterSetups = new List<LetterSetup>();
            _oLetterSetups = LetterSetup.Gets("SELECT * FROM LetterSetup", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            return View(_oLetterSetups);
        }
        public ActionResult ViewLetterSetup(int id)
        {
            _oLetterSetup = new LetterSetup();
            if (id > 0)
            {
                _oLetterSetup = _oLetterSetup.Get(id, (int)Session[SessionInfo.currentUserID]);
            }

            return View(_oLetterSetup);
        }


        public ActionResult ViewLetterSetupEmployees(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oLetterSetupEmployees = new List<LetterSetupEmployee>();
            _oLetterSetupEmployees = LetterSetupEmployee.Gets("SELECT * FROM View_LetterSetupEmployee WHERE (ApproveBy=0 OR ApproveBy IS NULL)", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oLetterSetupEmployees.ForEach(x =>
            {
                x.Body = "";
            });

            _oLetterSetups = new List<LetterSetup>();
            _oLetterSetups = LetterSetup.Gets("SELECT LSID, Name FROM LetterSetup", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.LetterSetups = _oLetterSetups;

            return View(_oLetterSetupEmployees);
        }
        public ActionResult ViewLetterSetupEmployee(int id)
        {
            _oLetterSetupEmployee = new LetterSetupEmployee();
            if (id > 0)
            {
                _oLetterSetupEmployee = _oLetterSetupEmployee.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oLetterSetups = new List<LetterSetup>();
            _oLetterSetups = LetterSetup.Gets("SELECT LSID, Name, IsEnglish FROM LetterSetup", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.LetterSetups = _oLetterSetups;

            ViewBag.LeaveHeads = LeaveHead.Gets("SELECT * FROM LeaveHead WHERE IsActive=1 AND ShortName IS NOT NULL", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            string sSql = "";
            sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSql = sSql + ")";

            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));


            return View(_oLetterSetupEmployee);
        }



        #endregion

        #region IUD LetterSetup
        [HttpPost]
        public JsonResult LetterSetup_IU(LetterSetup oLetterSetup)
        {
            _oLetterSetup = new LetterSetup();
            try
            {
                _oLetterSetup = oLetterSetup;
                if (_oLetterSetup.LSID > 0)
                {
                    _oLetterSetup = _oLetterSetup.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oLetterSetup = _oLetterSetup.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oLetterSetup = new LetterSetup();
                _oLetterSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLetterSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
                [HttpPost]
        public JsonResult LetterSetup_Copy(LetterSetup oLetterSetup)
        {
            _oLetterSetup = new LetterSetup();
            try
            {
                _oLetterSetup = oLetterSetup;
                if (_oLetterSetup.LSID > 0)
                {
                    _oLetterSetup.LSID = 0;
                    _oLetterSetup.Name = _oLetterSetup.Name + "-copy";
                    _oLetterSetup = _oLetterSetup.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oLetterSetup = new LetterSetup();
                _oLetterSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLetterSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LetterSetup_Delete(LetterSetup oLetterSetup)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oLetterSetup.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Get
        
        [HttpPost]
        public JsonResult GetEmployeeLetter(LetterSetupEmployee oLetterSetupEmployee)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oLetterSetupEmployee.GetEmpLetter(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Get(string sSql, double ts)
        {
            LetterSetup oLetterSetup = new LetterSetup();
            try
            {
                oLetterSetup = LetterSetup.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oLetterSetup = new LetterSetup();
                oLetterSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLetterSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region IUD LetterSetupEmployee
        [HttpPost]
        public JsonResult LetterSetupEmployee_IU(LetterSetupEmployee oLetterSetupEmployee)
        {
            _oLetterSetupEmployee = new LetterSetupEmployee();
            try
            {
                _oLetterSetupEmployee = oLetterSetupEmployee;
                if (_oLetterSetupEmployee.LSEID > 0)
                {
                    _oLetterSetupEmployee = _oLetterSetupEmployee.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oLetterSetupEmployee = _oLetterSetupEmployee.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oLetterSetupEmployee = new LetterSetupEmployee();
                _oLetterSetupEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLetterSetupEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult LetterSetupEmployee_Delete(LetterSetupEmployee oLetterSetupEmployee)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oLetterSetupEmployee.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion
        [HttpPost]
        public JsonResult ApproveLSE(LetterSetupEmployee oLetterSetupEmployee)
        {
            _oLetterSetupEmployee = new LetterSetupEmployee();
            try
            {
                _oLetterSetupEmployee = oLetterSetupEmployee;
                _oLetterSetupEmployee = _oLetterSetupEmployee.IUD((int)EnumDBOperation.Approval, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oLetterSetupEmployee = new LetterSetupEmployee();
                _oLetterSetupEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLetterSetupEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult SearchEmployeeLetter(bool nIsCheckDate, string sDate, int nEmployeeID, int nLetterType, bool bIsApproved)
        {
            List<LetterSetupEmployee> oLetterSetupEmployees = new List<LetterSetupEmployee>();
            try
            {
                string sSQL = "SELECT * FROM View_LetterSetupEmployee WHERE LSEID<>0";
                if (nIsCheckDate)
                {
                    sSQL += " AND CONVERT(DATE, DBServerDateTime)=" + sDate;
                }
                if (nEmployeeID > 0)
                {
                    sSQL += " AND EmployeeID=" + nEmployeeID;
                }
                if (nLetterType > 0)
                {
                    sSQL += " AND LSID=" + nLetterType;
                }
                if (bIsApproved)
                {
                    sSQL += " AND ApproveBy > 0";
                }
                oLetterSetupEmployees = LetterSetupEmployee.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oLetterSetupEmployees = new List<LetterSetupEmployee>();
                oLetterSetupEmployees[0].ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLetterSetupEmployees);
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
        public System.Drawing.Image GetImage(byte[] Image, string sImageName = "Image.jpg")
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
        public ActionResult PrintLetter(int nLetterSetupID, int nLetterSetupEmployeeID, bool isBangla, double ts)
        {
            
            LetterSetup oLetterSetup = new LetterSetup();
            List<LetterSetup> oLetterSetups = new List<LetterSetup>();
            LetterSetupEmployee oLetterSetupEmployee = new LetterSetupEmployee();
            if (nLetterSetupEmployeeID > 0 && nLetterSetupID==0)
            {
                oLetterSetupEmployee = oLetterSetupEmployee.Get(nLetterSetupEmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oLetterSetup = oLetterSetup.Get(oLetterSetupEmployee.LSID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                
                string[] aPageContents = oLetterSetupEmployee.Body.Split(new[] { "@999" }, StringSplitOptions.None);
                foreach (string sPageContent in aPageContents)
                {
                    LetterSetup oLetterSetupObj = new LetterSetup();
                    oLetterSetupObj = new LetterSetup();
                    oLetterSetupObj.Body = sPageContent;
                    oLetterSetupObj.Name = oLetterSetupEmployee.LetterName;
                    oLetterSetupObj.IsEnglish = oLetterSetupEmployee.IsEnglish;
                    oLetterSetupObj.HeaderName = oLetterSetup.HeaderName;
                    oLetterSetupObj.HeaderFontSize = oLetterSetup.HeaderFontSize;
                    oLetterSetupObj.HeaderTextAlign = oLetterSetup.HeaderTextAlign;
                    oLetterSetups.Add(oLetterSetupObj);
                }
            }
            else if (nLetterSetupID>0)
            {
                oLetterSetup = oLetterSetup.Get(nLetterSetupID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                Employee oEmployee = new Employee();
                oEmployee = oEmployee.Get(nLetterSetupEmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oLetterSetupEmployee.EmployeeID = oEmployee.EmployeeID;
                oLetterSetupEmployee.LSID = nLetterSetupID;
                string sBody = oLetterSetupEmployee.GetEmpLetter(((User)(Session[SessionInfo.CurrentUser])).UserID);
                string[] aPageContents = sBody.Split(new[] { "@999" }, StringSplitOptions.None);
                foreach (string sPageContent in aPageContents)
                {
                    LetterSetup oLetterSetupObj = new LetterSetup();
                    oLetterSetupObj = new LetterSetup();
                    oLetterSetupObj.Body = sPageContent;
                    oLetterSetupObj.Name = oLetterSetupEmployee.LetterName;
                    oLetterSetupObj.IsEnglish = oLetterSetup.IsEnglish;
                    oLetterSetupObj.HeaderName = oLetterSetup.HeaderName;
                    oLetterSetupObj.HeaderFontSize = oLetterSetup.HeaderFontSize;
                    oLetterSetupObj.HeaderTextAlign = oLetterSetup.HeaderTextAlign;
                    oLetterSetups.Add(oLetterSetupObj);
                }
            }
            Company oCompany = new Company();
            List<Company> oCompanys = new List<Company>();
            oCompany = oCompany.GetCompanyLogo(1, (int)(Session[SessionInfo.currentUserID]));
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oCompanys.Add(oCompany);

            ReportDocument rd = new ReportDocument();
            if(oLetterSetups[0].IsEnglish == true)
            {
                rd.Load(Path.Combine(Server.MapPath("~/Reports"), "LetterEnglish.rpt"));
            }
            else
            {
                rd.Load(Path.Combine(Server.MapPath("~/Reports"), "LetterBangla.rpt"));
            }
            rd.Database.Tables["LetterSetup"].SetDataSource(oLetterSetups);
            rd.Database.Tables["Company"].SetDataSource(oCompanys);

            //#region Margin(for dynamic page margin)
            //CrystalDecisions.Shared.PageMargins margins;
            //margins = rd.PrintOptions.PageMargins;
            //margins.bottomMargin = Convert.ToInt32(oLetterSetup.MarginBottom) * 1440; //1440 twips per inch
            //margins.leftMargin = Convert.ToInt32(oLetterSetup.MarginLeft) * 1440; //1440 twips per inch
            //margins.rightMargin = Convert.ToInt32(oLetterSetup.MarginRight) * 1440; //1440 twips per inch
            //margins.topMargin = Convert.ToInt32(oLetterSetup.MarginTop) * 1440; //1440 twips per inch
            //rd.PrintOptions.ApplyPageMargins(margins);   // for margin;
            //#endregion

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf");
            }
            catch { throw; }
        }
        public ActionResult EmployeeLetterPad(int nLSEID)
        {
            _oLetterSetupEmployee = new LetterSetupEmployee();
            _oLetterSetupEmployee = LetterSetupEmployee.Get("SELECT * FROM View_LetterSetupEmployee WHERE LSEID=" + nLSEID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            LetterSetup oLS = new LetterSetup();
            oLS = LetterSetup.Get("SELECT * FROM LetterSetup WHERE LSID="+_oLetterSetupEmployee.LSID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            rptPrintPad oReport = new rptPrintPad();
            byte[] abytes = oReport.PrepareReport(_oLetterSetupEmployee, oLS);
            return File(abytes, "application/pdf");
        }


        public void EmployeeLetterExcel(int nLSEID, bool bIsCompany, bool bIsTitle)
        {
            _oLetterSetupEmployee = new LetterSetupEmployee();
            _oLetterSetupEmployee = LetterSetupEmployee.Get("SELECT * FROM View_LetterSetupEmployee WHERE LSEID="+nLSEID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            bool isEng = _oLetterSetupEmployee.IsEnglish;

            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            int rowIndex = 1, nEndCol = 1;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Employee Letter");
                sheet.PrinterSettings.TopMargin = 0.2M;
                sheet.PrinterSettings.LeftMargin = 0.6M;
                sheet.PrinterSettings.BottomMargin = 0.2M;
                sheet.PrinterSettings.RightMargin = 0.6M;
                //sheet.PrinterSettings.TopMargin = 0;
                //sheet.PrinterSettings.LeftMargin = 0;
                //sheet.PrinterSettings.BottomMargin = 0;
                //sheet.PrinterSettings.RightMargin = 0;
                sheet.PrinterSettings.Orientation = eOrientation.Portrait;
                sheet.PrinterSettings.PaperSize = ePaperSize.A4;
                sheet.Name = "Employee Letter";


                sheet.Column(1).Width = 15;
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 25;
                sheet.Column(4).Width = 25;
                sheet.Column(5).Width = 25;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                

                #region Report Header
                //cell = sheet.Cells[rowIndex, 1, rowIndex, 5]; cell.Value = _oLetterSetupEmployee.BUName; cell.Style.Font.Bold = true; cell.Merge = true;
                //cell.Style.Font.Size = 14; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //rowIndex = rowIndex + 1;
                //cell = sheet.Cells[rowIndex, 1, rowIndex, 5]; cell.Value = _oLetterSetupEmployee.BUAddress; cell.Style.Font.Bold = false; cell.Merge = true;
                //cell.Style.Font.Size = 11; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //rowIndex = rowIndex + 1;

                if (bIsCompany)
                {

                    cell = sheet.Cells[rowIndex, 1, rowIndex, 5]; cell.Value = (isEng == false) ? oCompany.NameInBangla : oCompany.Name; cell.Style.Font.Bold = true; cell.Merge = true;
                    cell.Style.Font.Size = 14; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    rowIndex = rowIndex + 1;
                    cell = sheet.Cells[rowIndex, 1, rowIndex, 5]; cell.Value = (isEng == false) ? oCompany.AddressInBangla : oCompany.Address; cell.Style.Font.Bold = false; cell.Merge = true;
                    cell.Style.Font.Size = 11; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    rowIndex = rowIndex + 1;
                }

                if (bIsTitle)
                {

                    cell = sheet.Cells[rowIndex, 1, rowIndex, 5]; cell.Value = _oLetterSetupEmployee.LetterName; cell.Style.Font.Bold = true; cell.Merge = true;
                    cell.Style.Font.Size = 10; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    rowIndex = rowIndex + 1;
                }
                #endregion
                List<string> partsOfLetter = new List<string>();
                bool isContailtau = _oLetterSetupEmployee.Body.Contains("~");
                int nCount = 0;
                if (isContailtau)
                {
                    nCount = _oLetterSetupEmployee.Body.Split('~').Length;
                    for (int i = 0; i < nCount; i++)
                    {
                        string sPart = _oLetterSetupEmployee.Body.Split('~')[i];
                        partsOfLetter.Add(sPart);
                    }
                }

                var oLines = _oLetterSetupEmployee.Body.Split('\n');
                int lineCount = oLines.Count();
                bool bFlag = true;
                foreach (var oItem in oLines)
                {
                    if (bFlag && oItem.Contains("~"))
                    {
                        cell = sheet.Cells[rowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;  //cell.Style.WrapText = true;
                        cell.Style.Font.Size = 9; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = (isEng==false)?"ক্রমিক নং":"SL"; cell.Style.Font.Bold = true;  //cell.Style.WrapText = true;
                        cell.Style.Font.Size = 9; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = (isEng==false)?"বেতন খাতসমূহ":"SalaryHead"; cell.Style.Font.Bold = true;  //cell.Style.WrapText = true;
                        cell.Style.Font.Size = 9; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = (isEng==false)?"টাকা":"Amount"; cell.Style.Font.Bold = true;  //cell.Style.WrapText = true;
                        cell.Style.Font.Size = 9; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = false;  //cell.Style.WrapText = true;
                        cell.Style.Font.Size = 9; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        rowIndex += 1;
                        int nSL = 0;
                        double nTotal = 0.0;
                        for (int i = 1; i < partsOfLetter.Count - 1; i += 2)
                        {
                            nSL++;


                            cell = sheet.Cells[rowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;  //cell.Style.WrapText = true;
                            cell.Style.Font.Size = 9; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            cell = sheet.Cells[rowIndex, 2]; cell.Value = (isEng==false)?NumberFormatBan(nSL.ToString()):nSL.ToString(); cell.Style.Font.Bold = false;  //cell.Style.WrapText = true;
                            cell.Style.Font.Size = 9; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 3]; cell.Value = partsOfLetter[i]; cell.Style.Font.Bold = false;  //cell.Style.WrapText = true;
                            cell.Style.Font.Size = 9; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            string sAmt = "";
                            string sAmtBan = "";
                            
                            if (isEng == false)
                            {
                                string sNum = NumberFormat(partsOfLetter[i + 1]);
                                nTotal += Convert.ToDouble(sNum);
                                sAmt = NumberFormat(partsOfLetter[i + 1]);
                                sAmtBan = NumberFormatBan(Math.Round(Convert.ToDouble(sNum)).ToString());
                            }
                            else
                            {
                                nTotal += Convert.ToDouble(partsOfLetter[i + 1]);
                                sAmt = partsOfLetter[i + 1];
                            }
                            cell = sheet.Cells[rowIndex, 4]; cell.Value = (isEng) ? Math.Round(Convert.ToDouble(sAmt)).ToString() : sAmtBan; cell.Style.Font.Bold = false;  //cell.Style.WrapText = true;
                            cell.Style.Font.Size = 9; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = false;  //cell.Style.WrapText = true;
                            cell.Style.Font.Size = 9; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                            
                            rowIndex += 1;
                        }

                        
                        cell = sheet.Cells[rowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;  //cell.Style.WrapText = true;
                        cell.Style.Font.Size = 9; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = false;  //cell.Style.WrapText = true;
                        cell.Style.Font.Size = 9; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = (isEng==false)?"মোট":"Total"; cell.Style.Font.Bold = true; //cell.Style.WrapText = true;
                        cell.Style.Font.Size = 9; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = (isEng) ? Math.Round(nTotal).ToString() : NumberFormatBan(Math.Round(nTotal).ToString()); cell.Style.Font.Bold = true;  //cell.Style.WrapText = true;
                        cell.Style.Font.Size = 9; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = false;  //cell.Style.WrapText = true;
                        cell.Style.Font.Size = 9; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                            
                        rowIndex += 1;
                        bFlag = false;
                    }
                    else if (!oItem.Contains("~"))
                    {
                        int letterCount = oItem.Length;
                        int line = (letterCount > 0) ? (letterCount / 120 + ((letterCount % 120) != 0 ? 1 : 0)) - 1 : 0;

                        cell = sheet.Cells[rowIndex, 1, rowIndex+line, 5]; cell.Value = oItem; cell.Style.Font.Bold = false; cell.Merge = true; cell.Style.WrapText = true;
                        cell.Style.Font.Size = 9; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        rowIndex = rowIndex + 1 + line;
                    }
                }
                rowIndex += 1;

                cell = sheet.Cells[1, 5, lineCount, 5];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Employee Letter.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

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
                    if (ch == NumbersInBangla[i])
                    {
                        sNum = sNum.Replace(ch, NumbersInEnglish[i]);
                        break;
                    }
                    i++;
                }
            }
            return sNum;
        }
        public string NumberFormatBan(string sNum)
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

        #region Crystal Report EmployeeLetter
        public ActionResult EmployeeLetterExcelTest(int nLSEID)
        {
            List<LetterSetupEmployee> oLetterSetupEmployees = new List<LetterSetupEmployee>();
            oLetterSetupEmployees = LetterSetupEmployee.Gets("SELECT * FROM View_LetterSetupEmployee WHERE LSEID=" + nLSEID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "EmployeeLetter.rpt"));
            rd.SetDataSource(oLetterSetupEmployees);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                //string[] months = new string[] { "জানুয়ারী", "ফেব্রুয়ারি", "মার্চ", "এপ্রিল", "মে", "জুন", "জুলাই", "অগাস্ট", "সেপ্টেম্বর", "অক্টোবর", "নভেম্বর", "ডিসেম্বর" };
                //string[] months = new string[] { "Rvbyqvix", "‡deªyqvwi", "gvP©", "GwcÖj", "‡g", "Ryb", "RyjvB", "AMvó", "‡m‡Þ¤^i", "A‡±vei", "b‡f¤^i", "wW‡m¤^i" };
                //string sMonthName = months[nMonthID - 1] + " " + NumberFormatWithBijoy(nYear.ToString());
                TextObject txtMonthWithYear = (TextObject)rd.ReportDefinition.Sections["Section3"].ReportObjects["txtRemark"];
                txtMonthWithYear.Text = "ASAD";

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf");
            }
            catch { throw; }

        }

        #endregion
    }
}

