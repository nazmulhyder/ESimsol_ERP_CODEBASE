using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.OleDb;
using System.Data;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;

using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Data.SqlClient;
using System.Dynamic;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class PunchLogController : Controller
    {
        #region Declaration
        PunchLog _oPunchLog;
        private List<PunchLog> _oPunchLogs;
        #endregion

        #region Views
        public ActionResult View_PunchLogs(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oPunchLogs = new List<PunchLog>();

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
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

            return View(_oPunchLogs);
        }

        public ActionResult View_PunchLog(double ts)//nId=PunchLogID
        {
            return PartialView(_oPunchLog);
        }


        #endregion

        #region IUD
        [HttpPost]
        public JsonResult PunchLog_IU(string sEmployeeIds, DateTime PunchDate, DateTime PunchTime)
        {
            DateTime dtPunchTime = PunchDate.Add(TimeSpan.Parse(PunchTime.ToString("HH:mm")));
            _oPunchLog = new PunchLog();
            try
            {
                    _oPunchLog = _oPunchLog.IUD(sEmployeeIds,dtPunchTime,(int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oPunchLog = new PunchLog();
                _oPunchLog.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPunchLog);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult PunchLog_Delete(PunchLog oPunchLog)
        //{
        //    string sMsg = "";
        //    try
        //    {
        //        sMsg = oPunchLog.Delete(oPunchLog.PunchLogID, oPunchLog.PunchDateTime, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        sMsg = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(sMsg);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult PunchLog_Delete(PunchLog oPunchLog)
        {
            string sIDs = oPunchLog.ErrorMessage;
            bool bSuccess = false;
            try
            {
                bSuccess = oPunchLog.Delete(sIDs, oPunchLog.sParam, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                bSuccess = false;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(bSuccess);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Search
        [HttpPost]
        public JsonResult SearchWithPagination(DateTime dtDateFrom, DateTime dtDateTo,string sEmployeeIds,string sBusinessUnitIds,string sLocationIDs, string sDepartmentIds, string sDesignationIDs, int nLoadRecords, int nRowLength, double ts)
        {
            try
            {
                string sSql = "";
                sSql = "SELECT * FROM (SELECT top(" + nLoadRecords + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY PunchLogID) Row,* FROM View_PunchLog WHERE CONVERT(DATE, PunchDateTime) BETWEEN'" + dtDateFrom.ToString("dd MMM yyyy") + "' AND '" + dtDateTo.ToString("dd MMM yyyy") + "'";
                if (sEmployeeIds != "")
                {
                    sSql = sSql + " AND EmployeeID IN(" + sEmployeeIds + ")";
                }
                if (sBusinessUnitIds != "")
                {
                    sSql = sSql + " AND BusinessUnitID IN(" + sBusinessUnitIds + ")";
                }
                if (sLocationIDs != "")
                {
                    sSql = sSql + " AND LocationID IN(" + sLocationIDs + ")";
                }
                if (sDepartmentIds != "")
                {
                    sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
                }
                if (sDesignationIDs != "")
                {
                    sSql = sSql + " AND DesignationID IN(" + sDesignationIDs + ")";
                }
              
                sSql = sSql + ") aa WHERE Row >" + nRowLength + ") aaa ORDER BY EmployeeCode,PunchDateTime";
                
                _oPunchLogs = new List<PunchLog>();
                _oPunchLogs = PunchLog.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oPunchLogs.Count == 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oPunchLogs = new List<PunchLog>();
                _oPunchLog = new PunchLog();
                _oPunchLog.ErrorMessage = ex.Message;
                _oPunchLogs.Add(_oPunchLog);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPunchLogs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Upload Punch
        public ActionResult View_ImportPunchs(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oPunchLogs = new List<PunchLog>();

            List<PunchLogImportFormat> oPunchLogImportFormats = new List<PunchLogImportFormat>();
            oPunchLogImportFormats= PunchLogImportFormat.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            if (oPunchLogImportFormats.Count > 0) { ViewBag.PunchLogImportFormats = oPunchLogImportFormats[0]; }
            ViewBag.EnumPunchFormats = Enum.GetValues(typeof(EnumPunchFormat)).Cast<EnumPunchFormat>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            return View(_oPunchLogs);
        }

        [HttpPost]
        public JsonResult PunchLogImportFormat_IU(PunchLogImportFormat oPunchLogImportFormat)
        {
            //PunchLogImportFormat oPLImportFormat = new PunchLogImportFormat();
            try
            {
                oPunchLogImportFormat = oPunchLogImportFormat.IUD(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oPunchLogImportFormat = new PunchLogImportFormat();
                oPunchLogImportFormat.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPunchLogImportFormat);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Import & Export
        private List<PunchLog> GetPunchLogFromExcel(HttpPostedFileBase PostedFile, int nPunchFormat)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            List<PunchLog> oPunchLogXLs = new List<PunchLog>();
            PunchLog oPunchLogXL = new PunchLog();
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
                    //excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                    ////excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";

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

                    for (int i = 0; i < oRows.Count; i++)
                    {
                        oPunchLogXL = new PunchLog();

                        oPunchLogXL.CardNo = Convert.ToString(oRows[i][0] == DBNull.Value ? "" : oRows[i][0]);
                        if (oPunchLogXL.CardNo != "" && !string.IsNullOrEmpty(oPunchLogXL.CardNo))
                        {
                            string sTempDate = Convert.ToString(oRows[i][1] == DBNull.Value ? "" : oRows[i][1]);
                            string pdt = "";
                            if (nPunchFormat == (int)(EnumPunchFormat.DD_MM_YY))
                            {
                                string sDay = sTempDate.Split('/')[0];
                                string sMonth = sTempDate.Split('/')[1];
                                string sYearAndTime = sTempDate.Split('/')[2];
                                DateTime sDate = Convert.ToDateTime(sDay + "/" + sMonth + "/" + sYearAndTime);

                                pdt = sDate.ToString("dd MMM yyyy HH:mm:ss");
                            }

                            else if (nPunchFormat == (int)(EnumPunchFormat.MM_DD_YY))
                            {
                                string sMonth = sTempDate.Split('/')[0];
                                string sDay = sTempDate.Split('/')[1];
                                string sYearAndTime = sTempDate.Split('/')[2];
                                //string[] aMonths = new string["","Jan","Feb", "Mar"];
                                DateTime sDate = Convert.ToDateTime(sMonth + "/" + sDay + "/" + sYearAndTime);
                                //DateTime sDate1 = Convert.ToDateTime("01 " + aMonths[Convert.ToInt16(sMonth)] + " 2016 05.00 AM");

                                //DateTime PDate=Convert.ToDateTime(oRows[i][1] == DBNull.Value ? "" : oRows[i][1]);
                                pdt = sDate.ToString("dd MMM yyyy HH:mm:ss");
                            }

                            else if (nPunchFormat == (int)(EnumPunchFormat.YY_MM_DD))
                            {
                                string sYearAndTime = sTempDate.Split('/')[0];
                                string sMonth = sTempDate.Split('/')[1];
                                string sDay = sTempDate.Split('/')[2];
                                DateTime sDate = Convert.ToDateTime(sYearAndTime + "/" + sMonth + "/" + sDay);
                                pdt = sDate.ToString("dd MMM yyyy HH:mm:ss");
                            }

                            oPunchLogXL.PunchDateTime_ST = sTempDate;// pdt;
                            if (Convert.ToDateTime(sTempDate).ToString("HH:mm:ss") != "00:00:00") { oPunchLogXLs.Add(oPunchLogXL); }
                        }
                    }
                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                }
                else
                {
                    throw new Exception("File not supported");
                }
            }
            return oPunchLogXLs;
        }

        [HttpPost]
        public ActionResult View_ImportPunchs(HttpPostedFileBase filePunchLogs, int txtEnumPunchFormat)
        {
            List<PunchLog> oPunchLogXLs = new List<PunchLog>();
            PunchLog oPunchLogXL = new PunchLog();

            try
            {
                if (filePunchLogs == null) { throw new Exception("File not Found"); }
                oPunchLogXLs = this.GetPunchLogFromExcel(filePunchLogs, txtEnumPunchFormat);
                oPunchLogXLs = PunchLog.UploadXL(oPunchLogXLs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //if (oPunchLogXLs.Count > 0)
                //{
                //    oPunchLogXLs[0].ErrorMessage = "Uploaded Successfully!";
                //}
                //else
                //{
                //    oPunchLogXLs = new List<PunchLog>();
                //    oPunchLogXL = new PunchLog();
                //    oPunchLogXL.ErrorMessage = "nothing to upload or these employees alraedy uploaded!";
                //    oPunchLogXLs.Add(oPunchLogXL);
                //}
                ViewBag.FeedBack = "Uploaded Successfully!";//oPunchLogXLs[0].ErrorMessage;

                List<PunchLogImportFormat> oPunchLogImportFormats = new List<PunchLogImportFormat>();
                oPunchLogImportFormats = PunchLogImportFormat.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oPunchLogImportFormats.Count > 0) { ViewBag.PunchLogImportFormats = oPunchLogImportFormats[0]; }
                ViewBag.EnumPunchFormats = Enum.GetValues(typeof(EnumPunchFormat)).Cast<EnumPunchFormat>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            }
            catch (Exception ex)
            {
                ViewBag.FeedBack = ex.Message;
                _oPunchLogs = new List<PunchLog>();
                return View(_oPunchLogs);
            }
            _oPunchLogs = new List<PunchLog>();
            return View(_oPunchLogs);
        }


        #endregion Import & Export

        #endregion Upload Punch


        #region Punch_XL



        public void Print_PunchXL(string date, string BUIDS, string LIDS,string Dept,string Desg, string type, string id, string DateTo, string format)
        {
            if (type == "1")
            {
                Print_PunchXLDaily(date, BUIDS, LIDS,Dept,Desg, format);
            }
            else if (type == "2")
            {
                Print_PunchXLIndividual(date, BUIDS, LIDS, Dept,Desg,id, DateTo, format);
            }
        }


        private List<dynamic> generateDynamicList(string date, string BUIDS, string LIDS,string Dept,string Desg, string format)
        {

            DateTime dt = Convert.ToDateTime(date);
            string ssql = "";

            ssql = "SELECT * FROM View_PunchLog WHERE Convert(date,PunchDateTime)='" + dt.ToString("dd MMM yyyy") + "'";
            if (!string.IsNullOrEmpty(BUIDS))
                ssql += " AND BusinessUnitID IN("+BUIDS+") ";
            if (!string.IsNullOrEmpty(LIDS))
                ssql += " AND LocationID IN(" + LIDS + ") ";
            if (!string.IsNullOrEmpty(Dept))
                ssql += " AND DepartmentID IN(" + Dept + ") ";
            if (!string.IsNullOrEmpty(Desg))
                ssql += " AND DesignationID IN(" + Desg + ") ";
            ssql += " ORDER BY EmployeeCode,PunchDateTime";

            //string sSql = "SELECT * FROM View_PunchLog WHERE Convert(date,PunchDateTime)='" + dt.ToString("dd MMM yyyy") + "'";
            List<PunchLog> _oPunchLogs = new List<PunchLog>();
            _oPunchLogs = PunchLog.Gets(ssql, (int)Session[SessionInfo.currentUserID]);


            var grpEmpPunch = _oPunchLogs.GroupBy(x => new { x.EmployeeCode, x.EmployeeName, x.EmployeeID, x.BUName, x.LocationID, x.DepartmentID }, (key, grp) => new
            {
                EmployeeID = key.EmployeeID,
                EmployeeCode = grp.First().EmployeeCode,
                EmployeeName = grp.First().EmployeeName,
                punchList = grp,
                PunchCount = grp.Count(),
                BusinessUnitName = key.BUName,
                LocationID = key.LocationID,
                LocationName = grp.First().LocationName,
                DepartmentID=key.DepartmentID,
                DepartmentName = grp.First().DepartmentName

            }).OrderBy(x => x.BusinessUnitName).ThenBy(x => x.LocationID).ThenBy(x => x.DepartmentID).ThenBy(x => x.EmployeeID).ToList();

            List<dynamic> oDynamics = new List<dynamic>();
            const string colkey = "Time";
            int maxPunch = 0;
            if (grpEmpPunch.Count > 0)
            {
                maxPunch = grpEmpPunch.Select(x => x.PunchCount).Max();
            }

            int index = 0;
            grpEmpPunch.ForEach(x =>
            {
                index = 0;
                dynamic obj = new ExpandoObject();
                var expobj = obj as IDictionary<string, object>;
                expobj.Add("EmployeeCode", x.EmployeeCode);
                expobj.Add("EmployeeName", x.EmployeeName);
                expobj.Add("BusinessUnitName", x.BusinessUnitName);
                expobj.Add("LocationName", x.LocationName);
                expobj.Add("DepartmentName", x.DepartmentName);


                foreach (var oItem in x.punchList)
                {
                    expobj.Add(colkey + (++index).ToString(), (format == "format24" ? oItem.PunchTimeInString : oItem.PunchTimeInStringAMPM));
                }

                while (index < maxPunch)
                {
                    expobj.Add(colkey + (++index).ToString(), "");
                }
                oDynamics.Add(expobj);

            });
            return oDynamics;
        }

        public void Print_PunchXLDaily(string date, string BUIDS, string LIDS,string Dept,string Desg, string format)
        {
            int nRowIndex = 2, nEndRow = 0, nStartCol = 1, nEndCol = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            List<dynamic> oDynamics = new List<dynamic>();
            oDynamics = generateDynamicList(date, BUIDS, LIDS,Dept,Desg, format);


            using (var excelPackage = new ExcelPackage())
            {


                if (oDynamics.Any())
                {
                    var obj = (IDictionary<string, object>)oDynamics.First();


                    List<string> skipColumns = new List<string>(new string[] { "BusinessUnitName", "LocationName", "DepartmentName" });
                    List<string> columns = obj.Keys.Where(x => !skipColumns.Contains(x)).ToList();
                    nEndCol = columns.Count();


                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "PUNCH LOG";
                    var sheet = excelPackage.Workbook.Worksheets.Add("PUNCH LOG");
                    sheet.Name = "PUNCH LOG";

                    Company oCompany = new Company();
                    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.PringReportHead; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 2;

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "Date : " + date; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    nRowIndex = nRowIndex + 2;

                    bool hasInitial = true;
                    int colIn = 1;
                    #region if
                    Dictionary<string, string> odictionary = new Dictionary<string, string>();
                    oDynamics.ForEach(x =>
                    {
                        var instances = ((IDictionary<string, object>)x).Where(p => !skipColumns.Contains(p.Key));
                        var tempObj = (((IDictionary<string, object>)x).Where(p => skipColumns.Contains(p.Key)));

                        if (string.Join("", tempObj.Select(p => (string)p.Value)) != string.Join("", odictionary.Select(p => (string)p.Value)))
                        {

                            if (!hasInitial)
                                nRowIndex += 2;
                            else
                                hasInitial = false;

                            foreach (KeyValuePair<string, object> kvp in tempObj)
                            {
                                if (odictionary.ContainsKey(kvp.Key))
                                    odictionary[kvp.Key] = (string)kvp.Value;
                                else
                                    odictionary.Add(kvp.Key, (string)kvp.Value);

                                cell = sheet.Cells[nRowIndex, 1, nRowIndex, nEndCol]; cell.Merge = true; cell.Value = Global.CapitalSpilitor(kvp.Key) + ": " + kvp.Value; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nRowIndex++;
                            }
                            colIn = 1;

                            foreach (string columnName in columns)
                            {
                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = columnName; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            nRowIndex++;
                        }

                        colIn = 1;
                        foreach (KeyValuePair<string, object> kvp in instances)
                        {

                            cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "" + kvp.Value; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        nRowIndex++;

                    });

                    //cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    //fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=PunchLog.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                    #endregion
                }
                else
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "PUNCH LOG";
                    var sheet = excelPackage.Workbook.Worksheets.Add("PUNCH LOG");
                    sheet.Name = "PUNCH LOG";

                    Company oCompany = new Company();
                    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 15]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;


                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 15]; cell.Merge = true;
                    cell.Value = oCompany.PringReportHead; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;


                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 15]; cell.Merge = true;
                    cell.Value = "Nothing to print"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=PunchLog.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }


            }



        }


        private List<dynamic> generateDynamicListIndividual(string date, string BUIDS, string LIDS,string Dept,string Desg, string id, string DateTo, string format)
        {
            
            DateTime dt = Convert.ToDateTime(date);
            DateTime dtTo = Convert.ToDateTime(DateTo);
            string ssql = "";
            
            ssql = "SELECT * FROM View_PunchLog WHERE CONVERT(DATE, PunchDateTime) BETWEEN'" + dt.ToString("dd MMM yyyy") + "' AND '" + dtTo.ToString("dd MMM yyyy") + "'";
            if (!string.IsNullOrEmpty(BUIDS))
                ssql += " AND BusinessUnitID IN(" + BUIDS + ") ";
            if (!string.IsNullOrEmpty(LIDS))
                ssql += " AND LocationID IN(" + LIDS + ") ";
            if (!string.IsNullOrEmpty(Dept))
                ssql += " AND DepartmentID IN(" + Dept + ") ";
            if (!string.IsNullOrEmpty(Desg))
                ssql += " AND DesignationID IN(" + Desg + ") ";
            if (!string.IsNullOrEmpty(id))
                ssql += " AND EmployeeID='" + id + "'";
            ssql += " ORDER BY EmployeeCode,PunchDateTime";


            //string sSql = "SELECT * FROM View_PunchLog WHERE Convert(date,PunchDateTime)='" + dt.ToString("dd MMM yyyy") + "'";
            List<PunchLog> _oPunchLogs = new List<PunchLog>();
            _oPunchLogs = PunchLog.Gets(ssql, (int)Session[SessionInfo.currentUserID]);
            _oPunchLogs = _oPunchLogs.OrderBy(x => x.PunchLogID).ToList();

            var grpEmpPunch = _oPunchLogs.GroupBy(x => new { x.PunchDateInString }, (key, grp) => new
            {
                PunchDateInString = key.PunchDateInString,
                PunchCount = grp.Count(),
                punchList = grp,
                //EmployeeID = key.EmployeeID,
                EmployeeCode = grp.First().EmployeeCode,
                EmployeeName = grp.First().EmployeeName,
                //punchList = grp,
                //PunchCount = grp.Count(),
                //BusinessUnitName = key.BUName,
                //LocationID = key.LocationID,
                LocationName = grp.First().LocationName,
                BusinessUnitName = grp.First().BUName,
                //DepartmentID = key.DepartmentID,
                DepartmentName = grp.First().DepartmentName,
                DesignationName = grp.First().DesignationName,
                PunchDateTime = grp.First().PunchDateTime

            }).OrderBy(x => x.PunchDateTime).ToList();

            List<dynamic> oDynamics = new List<dynamic>();
            const string colkey = "Time";
            int maxPunch = 0;
            if (grpEmpPunch.Count > 0)
            {
                maxPunch = grpEmpPunch.Select(x => x.PunchCount).Max();
            }

            int index = 0;
            grpEmpPunch.ForEach(x =>
            {
                index = 0;
                dynamic obj = new ExpandoObject();
                var expobj = obj as IDictionary<string, object>;
                expobj.Add("Punch Date", x.PunchDateInString);
                expobj.Add("BusinessUnit", x.BusinessUnitName);
                expobj.Add("Location", x.LocationName);
                expobj.Add("EmployeeCode", x.EmployeeCode);
                expobj.Add("EmployeeName", x.EmployeeName);
                expobj.Add("DepartmentName", x.DepartmentName);
                expobj.Add("DesignationName", x.DesignationName);

                foreach (var oItem in x.punchList)
                {
                    expobj.Add(colkey + (++index).ToString(), (format == "format24" ? oItem.PunchTimeInString : oItem.PunchTimeInStringAMPM));
                }

                while (index < maxPunch)
                {
                    expobj.Add(colkey + (++index).ToString(), "");
                }
                oDynamics.Add(expobj);

            });
            return oDynamics;
        }

        public void Print_PunchXLIndividual(string date, string BUIDS, string LIDS,string Dept,string Desg, string Code, string DateTo, string format)
        {
            int nRowIndex = 2, nEndRow = 0, nStartCol = 1, nEndCol = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            List<dynamic> oDynamics = new List<dynamic>();
            oDynamics = generateDynamicListIndividual(date, BUIDS, LIDS,Dept,Desg, Code, DateTo, format);


            using (var excelPackage = new ExcelPackage())
            {
                if (oDynamics.Any())
                {
                    var obj = (IDictionary<string, object>)oDynamics.First();


                    List<string> skipColumns = new List<string>(new string[] { "BusinessUnit", "Location", "EmployeeCode", "EmployeeName", "DepartmentName", "DesignationName" });
                    List<string> columns = obj.Keys.Where(x => !skipColumns.Contains(x)).ToList();
                    nEndCol = columns.Count();


                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "PUNCH LOG";
                    var sheet = excelPackage.Workbook.Worksheets.Add("PUNCH LOG");
                    sheet.Name = "PUNCH LOG";

                    Company oCompany = new Company();
                    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.PringReportHead; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 2;

                    //cell = sheet.Cells[nRowIndex, 1]; cell.Value = "Date : " + date; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //nRowIndex = nRowIndex + 2;

                    bool hasInitial = true;
                    int colIn = 1;
                    

                        Dictionary<string, string> odictionary = new Dictionary<string, string>();
                        oDynamics.ForEach(x =>
                        {
                            var instances = ((IDictionary<string, object>)x).Where(p => !skipColumns.Contains(p.Key));
                            var tempObj = (((IDictionary<string, object>)x).Where(p => skipColumns.Contains(p.Key)));

                            if (string.Join("", tempObj.Select(p => (string)p.Value)) != string.Join("", odictionary.Select(p => (string)p.Value)))
                            {

                                if (!hasInitial)
                                    nRowIndex += 2;
                                else
                                    hasInitial = false;

                                foreach (KeyValuePair<string, object> kvp in tempObj)
                                {
                                    if (odictionary.ContainsKey(kvp.Key))
                                        odictionary[kvp.Key] = (string)kvp.Value;
                                    else
                                        odictionary.Add(kvp.Key, (string)kvp.Value);

                                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, nEndCol]; cell.Merge = true; cell.Value = Global.CapitalSpilitor(kvp.Key) + ": " + kvp.Value; cell.Style.Font.Bold = false;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    nRowIndex++;
                                }
                                colIn = 1;

                                foreach (string columnName in columns)
                                {
                                    cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = columnName; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                }
                                nRowIndex++;
                            }

                            colIn = 1;
                            foreach (KeyValuePair<string, object> kvp in instances)
                            {

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "" + kvp.Value; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            nRowIndex++;

                        });

                        //cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                        //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                        //fill.BackgroundColor.SetColor(Color.White);

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=PunchLog.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                else
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "PUNCH LOG";
                    var sheet = excelPackage.Workbook.Worksheets.Add("PUNCH LOG");
                    sheet.Name = "PUNCH LOG";

                    Company oCompany = new Company();
                    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 15]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;


                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 15]; cell.Merge = true;
                    cell.Value = oCompany.PringReportHead; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;


                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 15]; cell.Merge = true;
                    cell.Value = "Nothing to print"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=PunchLog.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
            }
            /*int nRowIndex = 2, nEndRow = 0, nStartCol = 1, nEndCol = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            List<dynamic> oDynamics = new List<dynamic>();
            oDynamics = generateDynamicListIndividual(date, BUIDS, LIDS, Code, DateTo);


            using (var excelPackage = new ExcelPackage())
            {
                var obj = (IDictionary<string, object>)oDynamics.First();

                nEndCol = obj.Keys.Count();


                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "PUNCH LOG";
                var sheet = excelPackage.Workbook.Worksheets.Add("PUNCH LOG");
                sheet.Name = "PUNCH LOG";

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;


                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + obj["EmployeeCode"]; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex++;


                int colIn = 1;
                if (oDynamics.Any())
                {
                    foreach (string columnName in obj.Keys)
                    {
                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = columnName; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    nRowIndex++;
                    oDynamics.ForEach(x =>
                    {
                        colIn = 1;
                        foreach (KeyValuePair<string, object> kvp in x)
                        {
                            cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "" + kvp.Value; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        nRowIndex++;

                    });

                    //cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    //fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=PF_Distribution.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }


            }*/
        }

        #endregion
        public System.Drawing.Image GetCompanyLogo(Company oCompany)
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

    }
}
