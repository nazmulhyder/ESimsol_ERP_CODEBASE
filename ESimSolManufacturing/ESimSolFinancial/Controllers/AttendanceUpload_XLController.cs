using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using ICS.Core.Utility;
using System.Data;
using System.Data.OleDb;

namespace ESimSolFinancial.Controllers
{
    public class AttendanceUpload_XLController : Controller
    {
        #region Declaration
        AttendanceDaily _oAttendanceDaily = new AttendanceDaily();
        List<AttendanceDaily> _oAttendanceDailys = new List<AttendanceDaily>();
        #endregion

        #region Import From Excel
        public ActionResult ImportAttendanceFromExcel(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oAttendanceDailys = new List<AttendanceDaily>();
            return View(_oAttendanceDailys);
        }

        private List<AttendanceDaily> GetAttendanceFromExcel(HttpPostedFileBase PostedFile, DateTime dtStartDate, DateTime dtEndDate)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();
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

                    string sAttendance="";
                    DateTime dTempStartDate;
                    //DateTime dStartDate = Convert.ToDateTime("26 Dec 2015");  //Convert.ToDateTime(oRows[0][2] == DBNull.Value ? DateTime.Today : oRows[0][2]);
                    //DateTime dEndDate = Convert.ToDateTime("25 Jan 2016");  //Convert.ToDateTime(oRows[0][3] == DBNull.Value ? DateTime.Today : oRows[0][3]);

                    //DateTime dStartDate = Convert.ToDateTime(oRows[oRows.Count-1][2] == DBNull.Value ? DateTime.Today : oRows[oRows.Count-1][2]);
                    //DateTime dEndDate = dtEndDate == DBNull.Value ? DateTime.Today : dtEndDate;

                    for (int i = 0; i < oRows.Count; i++)
                    {
                        dTempStartDate = dtStartDate;
                        oAttendanceDaily = new AttendanceDaily();
                        oAttendanceDaily.EmployeeCode =  Convert.ToString(oRows[i][0] == DBNull.Value ? "" : oRows[i][0]);
                        int j = 1;
                        sAttendance = "";
                        while (dTempStartDate <= dtEndDate)
                        {
                            sAttendance = sAttendance + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + ",";
                            dTempStartDate = dTempStartDate.AddDays(1);
                            j = j + 1;
                        }

                        if (sAttendance.Length > 0)
                        {
                            sAttendance = sAttendance.Remove(sAttendance.Length - 1, 1);
                        }
                        oAttendanceDaily.ErrorMessage = sAttendance;
                        oAttendanceDaily.AttendanceDate = dtStartDate;
                        oAttendanceDailys.Add(oAttendanceDaily);                        
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
            return oAttendanceDailys;
        }

        [HttpPost]
        public ActionResult ImportAttendanceFromExcel(HttpPostedFileBase fileAttendances, DateTime dtStartDate, DateTime dtEndDate)
        {
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            List<AttendanceDaily> oAttDs = new List<AttendanceDaily>();
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();
            try
            {
                if (fileAttendances == null) { throw new Exception("File not Found"); }
                oAttendanceDailys = this.GetAttendanceFromExcel(fileAttendances,dtStartDate,dtEndDate);
                oAttDs = AttendanceDaily.UploadAttendanceXL(oAttendanceDailys, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oAttDs.Count <= 0)
                {
                    throw new Exception("Nothing to upload. Please check the file formation.");
                }
                if(oAttDs.Count>0 && oAttDs[0].ErrorMessage!="")
                {
                    throw new Exception(oAttDs[0].ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ViewBag.FeedBack = ex.Message;
                oAttDs = new List<AttendanceDaily>();
                return View(oAttDs);
            }
            
            return View(oAttDs);
            //return RedirectToAction("ImportAttendanceFromExcel", "AttendanceUpload_XL", new { menuid = (int)Session[SessionInfo.MenuID] });
        }

        //public ActionResult DownloadFormat(int ift)
        //{
        //    ImportFormat oImportFormat = new ImportFormat();
        //    try
        //    {
        //        oImportFormat = ImportFormat.GetByType((EnumImportFormatType)ift, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        if (oImportFormat.AttatchFile != null)
        //        {
        //            var file = File(oImportFormat.AttatchFile, oImportFormat.FileType);
        //            file.FileDownloadName = oImportFormat.AttatchmentName;
        //            return file;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch
        //    {
        //        throw new HttpException(404, "Couldn't find " + oImportFormat.AttatchmentName);
        //    }
        //}
        #endregion
    }
}
