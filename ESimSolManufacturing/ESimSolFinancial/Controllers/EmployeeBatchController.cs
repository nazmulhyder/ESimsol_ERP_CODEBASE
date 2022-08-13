using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class EmployeeBatchController : Controller
    {
        public ActionResult ViewEmployeeBatchs(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeBatch).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            List<EmployeeBatch> _oEmployeeBatch = new List<EmployeeBatch>();
            string sSQL = "SELECT * FROM View_EmployeeBatch  AS HH ORDER BY EmployeeBatchID ASC";
            _oEmployeeBatch = EmployeeBatch.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oEmployeeBatch);

        }
        public ActionResult ViewEmployeeBatch(int id)
        {

            EmployeeBatch _oEmployeeBatch = new EmployeeBatch();
            if (id > 0)
            {
                _oEmployeeBatch = _oEmployeeBatch.Get(id, (int)Session[SessionInfo.currentUserID]);
                List<EmployeeBatchDetail> oEmployeeBatchDetails = new List<EmployeeBatchDetail>();
                oEmployeeBatchDetails = EmployeeBatchDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

                _oEmployeeBatch.EmployeeBatchDetails = oEmployeeBatchDetails;
            }

            return View(_oEmployeeBatch);

        }
        [HttpPost]
        public JsonResult Save(EmployeeBatch oEmployeeBatch)
        {
            EmployeeBatch _oEmployeeBatch = new EmployeeBatch();
            try
            {

                _oEmployeeBatch = oEmployeeBatch.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployeeBatch = new EmployeeBatch();
                _oEmployeeBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(EmployeeBatch oEmployeeBatch)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oEmployeeBatch.Delete(oEmployeeBatch.EmployeeBatchID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Approve(EmployeeBatch oEmployeeBatch)
        {
            EmployeeBatch _oEmployeeBatch = new EmployeeBatch();
            try
            {
                _oEmployeeBatch = oEmployeeBatch.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployeeBatch = new EmployeeBatch();
                _oEmployeeBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UndoApprove(EmployeeBatch oEmployeeBatch)
        {
            EmployeeBatch _oEmployeeBatch = new EmployeeBatch();
            try
            {
                _oEmployeeBatch = oEmployeeBatch.UndoApprove((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oEmployeeBatch = new EmployeeBatch();
                _oEmployeeBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsEmployeeByNameCode(Employee oEmployee)
        {
            List<Employee> _oEmployees = new List<Employee>();

            string sSQL = "Select * from VIEW_Employee as P Where P.EmployeeID<>0 ";
            if (!String.IsNullOrEmpty(oEmployee.Name))
            {
                sSQL = sSQL + " AND P.Name Like '%" + oEmployee.Name + "%'";
                sSQL = sSQL + " OR P.Code Like '%" + oEmployee.Name + "%'";
            }


            _oEmployees = new List<Employee>();
            _oEmployees = Employee.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            var jsonResult = Json(_oEmployees, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oEmployees);
            //return Json(sjson, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult UploadExcelData()
        {
            var PostedFile = Request.Files[0];            
            string strGuid = Convert.ToString(Guid.NewGuid());
            string strTempPath = Path.GetTempPath();
            //PostedFile.SaveAs(Path.Combine(strTempPath, strGuid + Path.GetFileName(PostedFile.FileName)));
            //return Json(new { success = true });

            EmployeeBatch oEmployeeBatch = new EmployeeBatch();
            EmployeeBatchDetail oEmployeeBatchDetail = new EmployeeBatchDetail();
            List<EmployeeBatchDetail> oEmployeeBatchDetails = new List<EmployeeBatchDetail>();
            try
            {

                DataSet ds = new DataSet();
                DataRowCollection oRows = null;
                string fileExtension = "";
                string fileDirectory = "";
                List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
                if (PostedFile.ContentLength > 0)
                {
                    fileExtension = System.IO.Path.GetExtension(PostedFile.FileName);
                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        fileDirectory = Server.MapPath("~/Content/") + PostedFile.FileName;
                        if (System.IO.File.Exists(fileDirectory))
                        {
                            System.IO.File.Delete(fileDirectory);
                        }
                        PostedFile.SaveAs(fileDirectory);
                        string excelConnectionString = string.Empty;
                        //connection String for xls file format.                    
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=No;IMEX=1\"";

                        //Create Connection to Excel work book and add oledb namespace
                        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                        excelConnection.Open();
                        DataTable dt = new DataTable();

                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return null;
                        }
                        excelConnection.Close();
                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        //excel data saves in temp file here.
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }
                        OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                        string query = string.Format("Select * from [{0}]", excelSheets[0]);
                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                        {
                            dataAdapter.Fill(ds);
                        }
                        oRows = ds.Tables[0].Rows;

                        string sEmployeeCode = "";                       
                        string sSalaryScheme = "";
                        double nGrossAmount = 0;
                        double nComplianceGross = 0;
                        Employee oEmployee = new Employee();
                        EmployeeOfficial oEmployeeOfficial = new EmployeeOfficial();
                        EmployeeSalaryStructure oEmployeeSalaryStructure = new EmployeeSalaryStructure();
                        for (int i = 1; i < oRows.Count; i++)
                        {
                            sEmployeeCode = Convert.ToString(oRows[i][0] == DBNull.Value ? "" : oRows[i][0]);
                            if (sEmployeeCode == "") { continue; }
                            oEmployee = new Employee();
                            oEmployee = oEmployee.GetByCode(sEmployeeCode, (int)Session[SessionInfo.currentUserID]);
                            if (oEmployee.EmployeeID <= 0) { continue; }

                            oEmployeeOfficial = new EmployeeOfficial();
                            oEmployeeOfficial = oEmployeeOfficial.GetByEmployee(oEmployee.EmployeeID, (int)Session[SessionInfo.currentUserID]);

                            oEmployeeSalaryStructure = new EmployeeSalaryStructure();
                            oEmployeeSalaryStructure = EmployeeSalaryStructure.GetByEmp(oEmployee.EmployeeID, (int)Session[SessionInfo.currentUserID]);

                            
                            sSalaryScheme = "";
                            nGrossAmount = 0;
                            nComplianceGross = 0;

                            sSalaryScheme = Convert.ToString(oRows[i][1] == DBNull.Value ? "" : oRows[i][1]);
                            nGrossAmount = Convert.ToDouble(oRows[i][2] == DBNull.Value ? "0" : oRows[i][2]);
                            nComplianceGross = Convert.ToDouble(oRows[i][3] == DBNull.Value ? "0" : oRows[i][3]);
                            

                            oEmployeeBatchDetail = new EmployeeBatchDetail();
                            oEmployeeBatchDetail.EmployeeBatchDetailID = 0;
                            oEmployeeBatchDetail.EmployeeBatchID = 0;
                            oEmployeeBatchDetail.EmployeeID = oEmployee.EmployeeID;
                            oEmployeeBatchDetail.Location = oEmployee.LocationName;
                            oEmployeeBatchDetail.Department = oEmployee.DepartmentName;
                            oEmployeeBatchDetail.Designation = oEmployee.DesignationName;
                            oEmployeeBatchDetail.ShiftName = oEmployeeOfficial.CurrentShiftName;
                            oEmployeeBatchDetail.AttendanceScheme = oEmployeeOfficial.AttendanceSchemeName;
                            oEmployeeBatchDetail.SalaryScheme = (sSalaryScheme != "" ? sSalaryScheme : oEmployeeSalaryStructure.SalarySchemeName);
                            oEmployeeBatchDetail.DateOfJoin = oEmployee.DateOfJoin;
                            oEmployeeBatchDetail.GrossAmount = (nGrossAmount != 0 ? nGrossAmount : oEmployeeSalaryStructure.GrossAmount);
                            oEmployeeBatchDetail.ComplianceGross = (nComplianceGross != 0 ? nComplianceGross : oEmployeeSalaryStructure.CompGrossAmount);
                            oEmployeeBatchDetail.EmployeeName = oEmployee.Name;
                            oEmployeeBatchDetail.EmployeeCode = oEmployee.Code;
                            oEmployeeBatchDetails.Add(oEmployeeBatchDetail);
                        }
                        oEmployeeBatch.EmployeeBatchDetails = oEmployeeBatchDetails;
                    }
                    else
                    {
                        throw new Exception("File not supported");
                    }
                }
            }
            catch (Exception ex)
            { 
                oEmployeeBatch = new EmployeeBatch();
                oEmployeeBatch.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void DownloadEmployeeBatchUploadFormat(double ts)
        {
            
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;
            ExcelFill fill;
            int colIndex = 1;
            int rowIndex = 1;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Employee Batch Upload Format");
                sheet.Name = "Employee List";

                int n = 1;
                sheet.Column(n++).Width = 20; //Employee Code
                sheet.Column(n++).Width = 30; //SalaryScheme
                sheet.Column(n++).Width = 25; //Gross Amount
                sheet.Column(n++).Width = 25; //Comp Gross Amount

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Employee Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Salary Scheme"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Comp Gross Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex += 1;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EmployeeBatchUploadFormat.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

        }

        
    }
}