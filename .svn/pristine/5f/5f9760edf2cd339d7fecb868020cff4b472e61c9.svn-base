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
    public class EmployeeActivityController : Controller
    {
        #region Declaration
        EmployeeActivityCategory _oEmployeeActivityCategory = new EmployeeActivityCategory();
        List<EmployeeActivityCategory> _oEmployeeActivityCategories = new List<EmployeeActivityCategory>();

        EmployeeActivityNote _oEmployeeActivityNote = new EmployeeActivityNote();
        List<EmployeeActivityNote> _oEmployeeActivityNotes = new List<EmployeeActivityNote>();
        byte[] abytes;
        
        #endregion

        #region Actions
        public ActionResult View_EmployeeActivities(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.AccountsBookSetup).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oEmployeeActivityCategories = new List<EmployeeActivityCategory>();
            _oEmployeeActivityCategories = EmployeeActivityCategory.Gets((int)Session[SessionInfo.currentUserID]);

            string sSql = "SELECT * FROM View_EmployeeActivityNote WHERE ApproveBy <= 0";
            _oEmployeeActivityNotes = new List<EmployeeActivityNote>();
            _oEmployeeActivityNotes = EmployeeActivityNote.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            ViewBag.EmployeeNote = _oEmployeeActivityNotes;


            return View(_oEmployeeActivityCategories);
        }

        public ActionResult View_EmployeeActivity(int id)
        {
            _oEmployeeActivityNote = new EmployeeActivityNote();
            if (id > 0)
            {
                _oEmployeeActivityNote = _oEmployeeActivityNote.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            string sSQL="Select * from EmployeeActivityCategory";
            var oEACs = new List<EmployeeActivityCategory>();
            oEACs = EmployeeActivityCategory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            ViewBag.EACs = oEACs;

            return View(_oEmployeeActivityNote);
        }

        [HttpPost]
        public JsonResult Save(EmployeeActivityCategory oEmployeeActivityCategory)
        {
            _oEmployeeActivityCategory = new EmployeeActivityCategory();
            try
            {
                _oEmployeeActivityCategory = oEmployeeActivityCategory;
                _oEmployeeActivityCategory = _oEmployeeActivityCategory.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oEmployeeActivityCategory = new EmployeeActivityCategory();
                _oEmployeeActivityCategory.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeActivityCategory);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Delete(EmployeeActivityCategory oEmployeeActivityCategory)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oEmployeeActivityCategory.Delete(oEmployeeActivityCategory.EACID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult SaveNote(EmployeeActivityNote oEmployeeActivityNote)
        {
            _oEmployeeActivityNote = new EmployeeActivityNote();
            try
            {
                _oEmployeeActivityNote = oEmployeeActivityNote;
                _oEmployeeActivityNote = _oEmployeeActivityNote.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oEmployeeActivityNote = new EmployeeActivityNote();
                _oEmployeeActivityNote.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeActivityNote);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteNote(EmployeeActivityNote oEmployeeActivityNote)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oEmployeeActivityNote.Delete(oEmployeeActivityNote.EANID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApproveNote(EmployeeActivityNote oEmployeeActivityNote)
        {
            _oEmployeeActivityNote = new EmployeeActivityNote();
            try
            {
                _oEmployeeActivityNote = oEmployeeActivityNote.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployeeActivityNote = new EmployeeActivityNote();
                _oEmployeeActivityNote.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeActivityNote);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsEmployeeActivityNote(EmployeeActivityNote oEmployeeActivityNote)
        {

            List<EmployeeActivityNote> oEmployeeActivityNotes = new List<EmployeeActivityNote>();
            try
            {
                bool IsDateSearch = Convert.ToBoolean(oEmployeeActivityNote.Params.Split('~')[0]);
                DateTime dtIssueFrom = Convert.ToDateTime(oEmployeeActivityNote.Params.Split('~')[1]);
                DateTime dtIssueTo = Convert.ToDateTime(oEmployeeActivityNote.Params.Split('~')[2]);
                int EACID =Convert.ToInt32(oEmployeeActivityNote.Params.Split('~')[3]);
                string Name = oEmployeeActivityNote.Params.Split('~')[4];
                String sSQL = "";

                if ((IsDateSearch == false))
                {
                    sSQL = "SELECT * FROM View_EmployeeActivityNote WHERE EANID <> 0";
                    if (EACID != 0 && string.IsNullOrEmpty(Name))
                    {
                        sSQL += " AND EACID=" + EACID;
                    }
                    if (!string.IsNullOrEmpty(Name) && EACID == 0)
                    {
                        sSQL += " AND NAME='" + Name + "'";
                    }
                    if (!string.IsNullOrEmpty(Name) && EACID != 0)
                    {
                        sSQL += " AND EACID=" + EACID + " AND NAME= '" + Name + "'";
                    }

                }


                else
                {

                    sSQL = "SELECT * FROM View_EmployeeActivityNote WHERE EANID <> 0";
                    if (EACID != 0 && string.IsNullOrEmpty(Name))
                    {
                        sSQL += " AND EACID=" + EACID;
                    }
                    if (!string.IsNullOrEmpty(Name) && EACID == 0)
                    {
                        sSQL += " AND NAME='" + Name + "'";
                    }
                    if (!string.IsNullOrEmpty(Name) && EACID != 0)
                    {
                        sSQL += " AND EACID=" + EACID + " AND NAME= '" + Name + "'";
                    }
                    if (IsDateSearch)
                        sSQL += " And ActivityDate Between '" + dtIssueFrom.ToString("dd MMM yyyy") + "' And '" + dtIssueTo.ToString("dd MMM yyyy") + "'";

                }

                oEmployeeActivityNotes = EmployeeActivityNote.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oEmployeeActivityNotes = new List<EmployeeActivityNote>();
                oEmployeeActivityNote = new EmployeeActivityNote();
                oEmployeeActivityNote.ErrorMessage = ex.Message;
                oEmployeeActivityNotes.Add(oEmployeeActivityNote);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeActivityNotes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PDF
        public ActionResult Print_PDF(string sParams)
        {
            List<EmployeeActivityNote> oEmployeeActivityNotes = new List<EmployeeActivityNote>();

            bool IsDateSearch = Convert.ToBoolean(sParams.Split('~')[0]);
            DateTime dtIssueFrom = Convert.ToDateTime(sParams.Split('~')[1]);
            DateTime dtIssueTo = Convert.ToDateTime(sParams.Split('~')[2]);
            int EACID = Convert.ToInt32(sParams.Split('~')[3]);
            string Name = sParams.Split('~')[4];
            String sSQL = "";

            if ((IsDateSearch == false))
            {
                sSQL = "SELECT * FROM View_EmployeeActivityNote WHERE EANID <> 0";
                if (EACID != 0 && string.IsNullOrEmpty(Name))
                {
                    sSQL += " AND EACID=" + EACID;
                }
                if (!string.IsNullOrEmpty(Name) && EACID == 0)
                {
                    sSQL += " AND NAME='" + Name + "'";
                }
                if (!string.IsNullOrEmpty(Name) && EACID != 0)
                {
                    sSQL += " AND EACID=" + EACID + " AND NAME= '" + Name + "'";
                }

            }
            else
            {

                sSQL = "SELECT * FROM View_EmployeeActivityNote WHERE EANID <> 0";
                if (EACID != 0 && string.IsNullOrEmpty(Name))
                {
                    sSQL += " AND EACID=" + EACID;
                }
                if (!string.IsNullOrEmpty(Name) && EACID == 0)
                {
                    sSQL += " AND NAME='" + Name + "'";
                }
                if (!string.IsNullOrEmpty(Name) && EACID != 0)
                {
                    sSQL += " AND EACID=" + EACID + " AND NAME= '" + Name + "'";
                }
                if (IsDateSearch)
                    sSQL += " And ActivityDate Between '" + dtIssueFrom.ToString("dd MMM yyyy") + "' And '" + dtIssueTo.ToString("dd MMM yyyy") + "'";

            }
            oEmployeeActivityNotes = EmployeeActivityNote.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptEmployeeActivity oReport = new rptEmployeeActivity();
            abytes = oReport.PrepareReport(oEmployeeActivityNotes, oCompany);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region Excel

        public void Print_Excel(string sParams)
        {
            List<EmployeeActivityNote> oEmployeeActivityNotes = new List<EmployeeActivityNote>();

            bool IsDateSearch = Convert.ToBoolean(sParams.Split('~')[0]);
            DateTime dtIssueFrom = Convert.ToDateTime(sParams.Split('~')[1]);
            DateTime dtIssueTo = Convert.ToDateTime(sParams.Split('~')[2]);
            int EACID = Convert.ToInt32(sParams.Split('~')[3]);
            string Name = sParams.Split('~')[4];
            String sSQL = "";

            if ((IsDateSearch == false))
            {
                sSQL = "SELECT * FROM View_EmployeeActivityNote WHERE EANID <> 0";
                if (EACID != 0 && string.IsNullOrEmpty(Name))
                {
                    sSQL += " AND EACID=" + EACID;
                }
                if (!string.IsNullOrEmpty(Name) && EACID == 0)
                {
                    sSQL += " AND NAME='" + Name + "'";
                }
                if (!string.IsNullOrEmpty(Name) && EACID != 0)
                {
                    sSQL += " AND EACID=" + EACID + " AND NAME= '" + Name + "'";
                }

            }
            else
            {

                sSQL = "SELECT * FROM View_EmployeeActivityNote WHERE EANID <> 0";
                if (EACID != 0 && string.IsNullOrEmpty(Name))
                {
                    sSQL += " AND EACID=" + EACID;
                }
                if (!string.IsNullOrEmpty(Name) && EACID == 0)
                {
                    sSQL += " AND NAME='" + Name + "'";
                }
                if (!string.IsNullOrEmpty(Name) && EACID != 0)
                {
                    sSQL += " AND EACID=" + EACID + " AND NAME= '" + Name + "'";
                }
                if (IsDateSearch)
                    sSQL += " And ActivityDate Between '" + dtIssueFrom.ToString("dd MMM yyyy") + "' And '" + dtIssueTo.ToString("dd MMM yyyy") + "'";

            }
            oEmployeeActivityNotes = EmployeeActivityNote.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            int nSL = 0;
            #region Export Excel
            int nRowIndex = 2, nEndRow = 0, nStartCol = 1, nEndCol = 8;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Production Status");
                sheet.Name = "Production Status";

                sheet.Column(1).Width = 8;
                sheet.Column(2).Width = 35;
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 30;
                sheet.Column(5).Width = 30;
                sheet.Column(6).Width = 30;
                sheet.Column(7).Width = 30;
                sheet.Column(8).Width = 40;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Employee Activity List"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Header Row
                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "EmployeeID"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Location"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Activity Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Category"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex += 1;
                #endregion

                #region DATA
                foreach (EmployeeActivityNote oItem in oEmployeeActivityNotes)
                {
                    ++nSL;

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = oItem.Code; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.LocationName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ActivityDateInStr; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.ActivityCategory; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex += 1;
                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Production Status (Order View).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

        }
        #endregion
        #endregion
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
    }
}
