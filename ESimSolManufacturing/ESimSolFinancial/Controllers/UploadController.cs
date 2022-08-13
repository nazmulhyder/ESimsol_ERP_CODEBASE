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

    public class UploadController : PdfViewController
    {
        #region Declaration
        bool IsComp;
        AttendanceDaily_ZN _oAttendanceDaily_ZN;
        private List<AttendanceDaily_ZN> _oAttendanceDaily_ZNs;
        private List<MaxOTConfiguration> _oMaxOTConfiguration;
        private List<EmployeeSalary> _oEmployeeSalarys;
        EmployeeSalary _oEmployeeSalary;
        List<TransferPromotionIncrement> _oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
        List<EmployeeBankAccount> _oEmployeeBankAccounts = new List<EmployeeBankAccount>();
        List<EmployeeSalaryDetail> _oEmployeeSalaryDetails = new List<EmployeeSalaryDetail>();
        List<SalarySummary_F2> oSalarySummerys = new List<SalarySummary_F2>();
        List<SalarySummaryDetail_F2> oSalarySummaryDetail_F2s = new List<SalarySummaryDetail_F2>();
        List<SalarySummary_F2> _oSalarySummary_F2s = new List<SalarySummary_F2>();
        List<SalarySummary_F2> _oTempSalarySummary_F2s = new List<SalarySummary_F2>();
        List<SalarySummaryDetail_F2> _oSalarySummaryDetail_F2s = new List<SalarySummaryDetail_F2>();
        List<SalarySummaryDetail_F2> _oAdditionSalaryHeads = new List<SalarySummaryDetail_F2>();
        List<SalarySummaryDetail_F2> _oDeductionSalaryHeads = new List<SalarySummaryDetail_F2>();
        List<SalarySummary_F2> _oSalarySummary_F2s_Location = new List<SalarySummary_F2>();
        EmployeeSettlement _oEmployeeSettlement;
        List<EmployeeSettlement> _oEmployeeSettlements;
        bool IsNUInDate, IsNUInTime, IsNUOutDate, IsNUOutTime, IsNULate, IsNUEarly, IsNUOT, IsNURemark;
        AttendanceDaily _oAttendanceDaily;
        private List<AttendanceDaily> _oAttendanceDailys;
        string sFormat = "";
        List<CalculateManpower> showSummery = new List<CalculateManpower>();
        int Present, Absent, Late, EarlyLeaving, OT, NoWork, Leave, NoOutTime;
        UploadConfigure _oUploadConfigure = new UploadConfigure();

        #endregion

        #region view

        public ActionResult View_Upload(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();

            return View(oAttendanceDaily);
        }


        #endregion view

        #region UploadManualAttendance
        private List<AttendanceDaily> GetAttFromExcel(HttpPostedFileBase PostedFile)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();
            AttendanceDaily oAttendanceDailyT = new AttendanceDaily();
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
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=No;IMEX=1\"";
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

                    //IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);

                    string sAtt = "";
                    DateTime tempDT = new DateTime(1950, 01, 01);
                    DateTime tempTime = new DateTime(1950, 01, 01);
                    DateTime tempTimeFinal = new DateTime(1950, 01, 01);
                    oAttendanceDailyT.AttendanceDate = Convert.ToDateTime(oRows[0][1]);
                    for (int i = 2; i < oRows.Count; i++)
                    {
                        oAttendanceDaily = new AttendanceDaily();
                        oAttendanceDaily.EmployeeCode = Convert.ToString(oRows[i][0] == DBNull.Value ? "NU" : oRows[i][0]);
                        int j = 1;
                        sAtt = "";
                        sAtt = Convert.ToString(oRows[i][j] == DBNull.Value ? "NU" : oRows[i][j]);
                        j++;

                        //InDate
                        if (Convert.ToString(oRows[i][j]) == "NU")
                        {
                            IsNUInDate = true;
                        }
                        else
                        {
                            string sDate = Convert.ToString(oRows[i][j]);
                            //DateTime.TryParse(Convert.ToString(oRows[i][j]), out tempDT);
                            //tempDT = DateTime.ParseExact(sDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            //tempDT = DateTime.Parse(sDate);
                            tempDT = DateTime.ParseExact(sDate, "dd/MM/yyyy", null);
                        }
                        j++;

                        if (Convert.ToString(oRows[i][j]) == "NU")
                        {
                            IsNUInTime = true;
                            //oAttendanceDaily.CompInTime = new DateTime(1950, 01, 01);
                        }
                        else
                        {
                            string sDate = Convert.ToString(oRows[i][j]);
                            TimeSpan ts = new TimeSpan(Convert.ToInt32(sDate.Split(':')[0]), Convert.ToInt32(sDate.Split(':')[1]), 0);
                            tempTime = tempTime.Date + ts;
                            tempTimeFinal = new DateTime(tempDT.Year, tempDT.Month, tempDT.Day, tempTime.Hour, tempTime.Minute, tempTime.Second);


                        }
                        if (IsNUInDate == true && IsNUInTime == true)
                        {
                            oAttendanceDaily.CompInTime = new DateTime(1950, 01, 01);
                        }
                        else
                        {
                            oAttendanceDaily.CompInTime = new DateTime(tempDT.Year, tempDT.Month, tempDT.Day, tempTimeFinal.Hour, tempTimeFinal.Minute, tempTimeFinal.Second);
                        }
                        j++;

                        //OutDate
                        if (Convert.ToString(oRows[i][j]) == "NU")
                        {
                            IsNUOutDate = true;
                        }
                        else
                        {
                            string sDate = Convert.ToString(oRows[i][j]);
                            //DateTime.TryParse(Convert.ToString(oRows[i][j]), out tempDT);
                            //tempDT = DateTime.ParseExact(sDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            //tempDT = DateTime.Parse(sDate);
                            tempDT = DateTime.ParseExact(sDate, "dd/MM/yyyy", null);
                            //tempDT = DateTime.ParseExact(Convert.ToString(oRows[i][j]), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        j++;
                        if (Convert.ToString(oRows[i][j]) == "NU")
                        {
                            IsNUOutTime = true;
                            //oAttendanceDaily.CompOutTime = new DateTime(1950, 01, 01);
                        }
                        else
                        {
                            string sDate = Convert.ToString(oRows[i][j]);
                            TimeSpan ts = new TimeSpan(Convert.ToInt32(sDate.Split(':')[0]), Convert.ToInt32(sDate.Split(':')[1]), 0);
                            tempTime = tempTime.Date + ts;
                            tempTimeFinal = new DateTime(tempDT.Year, tempDT.Month, tempDT.Day, tempTime.Hour, tempTime.Minute, tempTime.Second);
                        }
                        if (IsNUOutDate == true && IsNUOutTime == true)
                        {
                            oAttendanceDaily.CompOutTime = new DateTime(1950, 01, 01);
                        }
                        else
                        {
                            oAttendanceDaily.CompOutTime = new DateTime(tempDT.Year, tempDT.Month, tempDT.Day, tempTimeFinal.Hour, tempTimeFinal.Minute, tempTimeFinal.Second);
                        }
                        j++;

                        if (Convert.ToString(oRows[i][j]) == "NU")
                        {
                            IsNULate = true;
                        }
                        else
                        {
                            oAttendanceDaily.CompLateArrivalMinute = Convert.ToInt32(oRows[i][j]);
                        }
                        j++;
                        if (Convert.ToString(oRows[i][j]) == "NU")
                        {
                            IsNUEarly = true;
                        }
                        else
                        {
                            oAttendanceDaily.CompEarlyDepartureMinute = Convert.ToInt32(oRows[i][j]);
                        }
                        j++;
                        if (Convert.ToString(oRows[i][j]) == "NU")
                        {
                            IsNUOT = true;
                        }
                        else
                        {
                            oAttendanceDaily.CompOverTimeInMinute = Convert.ToInt32(oRows[i][j]);
                        }
                        j++;
                        if (Convert.ToString(oRows[i][j]) == "NU")
                        {
                            IsNURemark = true;
                        }
                        else
                        {
                            oAttendanceDaily.Remark = Convert.ToString(oRows[i][0] == DBNull.Value ? "NU" : oRows[i][j]);
                        }
                        j++;

                        oAttendanceDaily.ErrorMessage = sAtt;
                        oAttendanceDailys.Add(oAttendanceDaily);
                    }
                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                    foreach (AttendanceDaily ADs in oAttendanceDailys)
                    {
                        ADs.AttendanceDate = oAttendanceDailyT.AttendanceDate;
                    }
                }
                else
                {
                    throw new Exception("File not supported");
                }
            }
            return oAttendanceDailys;
        }


        private List<Employee_UploadXL> GetEmployeeFromExcel(HttpPostedFileBase PostedFile)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            List<Employee_UploadXL> oEXLs = new List<Employee_UploadXL>();
            Employee_UploadXL oEXL = new Employee_UploadXL();
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
                    DateTime dtDate = DateTime.Now;
                    Int16 nUser = 0;
                    for (int i = 0; i < oRows.Count; i++)
                    {
                        oEXL = new Employee_UploadXL();
                        oEXL.Code = Convert.ToString(oRows[i][0] == DBNull.Value ? "" : oRows[i][0]);
                        if (oEXL.Code != "")
                        {
                            oEXL.NameInBangla = Convert.ToString(oRows[i][1] == DBNull.Value ? "" : oRows[i][1]);
                            oEXL.ContactNo = Convert.ToString(oRows[i][2] == DBNull.Value ? "" : oRows[i][2]);
                            oEXL.Email = Convert.ToString(oRows[i][3] == DBNull.Value ? "" : oRows[i][3]);
                            oEXL.FathersName = Convert.ToString(oRows[i][4] == DBNull.Value ? "" : oRows[i][4]);
                            oEXL.HusbandName = Convert.ToString(oRows[i][5] == DBNull.Value ? "" : oRows[i][5]);
                            oEXL.MothersName = Convert.ToString(oRows[i][6] == DBNull.Value ? "" : oRows[i][6]);
                            oEXL.BloodGroup = Convert.ToString(oRows[i][7] == DBNull.Value ? "" : oRows[i][7]);
                            oEXL.NationalID = Convert.ToString(oRows[i][8] == DBNull.Value ? "" : oRows[i][8]);
                            oEXL.PresentAdd = Convert.ToString(oRows[i][9] == DBNull.Value ? "" : oRows[i][9]);
                            oEXL.PermanentAdd = Convert.ToString(oRows[i][10] == DBNull.Value ? "" : oRows[i][10]);

                            oRows[i][11] = oRows[i][11] ?? "";
                            oEXL.DateOfBirth = (DateTime.TryParse(oRows[i][11].ToString(), out dtDate) ? dtDate : DateTime.Now);
                            oEXL.Village = Convert.ToString(oRows[i][12] == DBNull.Value ? "" : oRows[i][12]);
                            oEXL.PostOffice = Convert.ToString(oRows[i][13] == DBNull.Value ? "" : oRows[i][13]);
                            oEXL.Thana = Convert.ToString(oRows[i][14] == DBNull.Value ? "" : oRows[i][14]);
                            oEXL.District = Convert.ToString(oRows[i][15] == DBNull.Value ? "" : oRows[i][15]);
                            oEXL.PostCode = Convert.ToString(oRows[i][16] == DBNull.Value ? "" : oRows[i][16]);
                            oEXL.Grade = Convert.ToString(oRows[i][17] == DBNull.Value ? "" : oRows[i][17]);
                            oEXL.ProximityCardNo = Convert.ToString(oRows[i][18] == DBNull.Value ? "" : oRows[i][18]);
                            oEXLs.Add(oEXL);
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
            return oEXLs;
        }

        private List<Employee> GetEmployeeFromExcelWithConfigure(HttpPostedFileBase PostedFile, UploadConfigure oUploadConfigure)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            string sQueryEmployee = "";
            string sQueryEmployeeOfficial = "";
            string sQueryEmployeeBankAccount = "";
            string sQueryEmployeeAuthentication = "";
            string sQueryEmployeeSalaryStructure = "";

            string sFinalQuery = "";
            
            string sSelectEmployeeID = "";
            string sSelectBank = "";
            string sSelectEmployeeType = "";
            bool isEmpID = false;
            bool isBank = false;
            bool isEmpType = false, isEmpBlock = false, isEmpGroup = false; ;

            List<Employee_UploadXL> oEXLs = new List<Employee_UploadXL>();
            Employee_UploadXL oEXL = new Employee_UploadXL();
            Employee oEmployee = new Employee();
            List<Employee> oEmployees = new List<Employee>();
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
                    DateTime dtDate = DateTime.Now;
                    Int16 nUser = 0;
                    List<string> FieldNames =new List<string>();
                    List<string> ColumnNames =new List<string>();
                  //  string[] FieldNames, ColumnNames;
                    ColumnNames = _oUploadConfigure.CaptionNames.Split(',').ToList();
                    bool bFlag = false;

                    int nEmpLength = 0;
                    int nEmpOffLength = 0;
                    int nEmpBankAccLength = 0;
                    int nEmpSalaryStructureLength = 0;
                    int nEmpAutLength = 0;
                    string sEmpGroupName="",sEmpBlockName="",sTIN="",sETIN="";
                    //oEXL.NameInBangla = Convert.ToString(oRows[i][1] == DBNull.Value ? "" : oRows[i][1]);
                    for (int i = 0; i < oRows.Count; i++)
                    {
                        sEmpGroupName="";
                        sEmpBlockName="";
                        sTIN = "";
                        sETIN = "";
                        FieldNames = _oUploadConfigure.FieldNames.Split(',').ToList();
                        int nColmnCount = FieldNames.Count();
                        string EmpCode = Convert.ToString(oRows[i][0] == DBNull.Value ? "" : oRows[i][0]);
                        if (!string.IsNullOrEmpty(EmpCode))
                        {
                            sQueryEmployee = "UPDATE Employee SET";
                            sQueryEmployeeOfficial = "UPDATE EmployeeOfficial SET ";
                            sQueryEmployeeBankAccount = "UPDATE EmployeeBankAccount SET ";
                            sQueryEmployeeSalaryStructure = "UPDATE EmployeeSalaryStructure SET ";
                            sQueryEmployeeAuthentication = "UPDATE EmployeeAuthentication SET ";

                            nEmpLength = sQueryEmployee.Length;
                            nEmpOffLength = sQueryEmployeeOfficial.Length;
                            nEmpBankAccLength = sQueryEmployeeBankAccount.Length;
                            nEmpSalaryStructureLength = sQueryEmployeeSalaryStructure.Length;
                            nEmpAutLength = sQueryEmployeeAuthentication.Length;

                            bFlag = true;
                            sSelectEmployeeID = "SELECT(SELECT EmployeeID FROM Employee WHERE EmployeeID=(SELECT EmployeeID FROM Employee WHERE Code='" + EmpCode + "') AND IsActive=1) AS EmployeeID";
                            isEmpID = true;

                            for (int j = 1; j < nColmnCount; j++)
                            {
                                #region Employee Table
                                //Name
                                if (FieldNames.Contains("Name"))
                                {
                                    sQueryEmployee += " Name='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Name");
                                    continue;
                                }
                                ////NameInBangla
                                //if (FieldNames.Contains("NameInBangla"))
                                //{
                                //    sQueryEmployee += " NameInBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                //    FieldNames.RemoveAll(x => x == ("NameInBangla"));
                                //    continue;
                                //}
                                //FatherName
                                if (FieldNames.Contains("FatherName"))
                                {
                                    sQueryEmployee += " FatherName='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "', IsFather=1" + ",";
                                    FieldNames.RemoveAll(x => x == "FatherName");
                                    if (FieldNames.Contains("HusbandName"))
                                    {
                                        FieldNames.RemoveAll(x => x == "HusbandName");
                                        j = j + 1;
                                    }
                                    continue;
                                }
                                //HusbandName
                                if (FieldNames.Contains("HusbandName"))
                                {
                                    sQueryEmployee += " FatherName='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "', IsFather=0" + ",";
                                    FieldNames.RemoveAll(x => x == "HusbandName");
                                    continue;
                                }
                                //MotherName
                                if (FieldNames.Contains("MotherName"))
                                {
                                    sQueryEmployee += " MotherName='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "MotherName");
                                    continue;
                                }
                                //DateOfBirth
                                if (FieldNames.Contains("DateOfBirth"))
                                {
                                    sQueryEmployee += " DateOfBirth='" + Convert.ToDateTime(Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j])) + "',";
                                    FieldNames.RemoveAll(x => x == "DateOfBirth");
                                    continue;
                                }
                                //Gender
                                if (FieldNames.Contains("Gender"))
                                {
                                    sQueryEmployee += " Gender='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Gender");
                                    continue;
                                }
                                //ContactNo
                                if (FieldNames.Contains("ContactNo"))
                                {
                                    sQueryEmployee += " ContactNo='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "ContactNo");
                                    continue;
                                }
                                //Email
                                if (FieldNames.Contains("Email"))
                                {
                                    sQueryEmployee += " Email='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Email");
                                    continue;
                                }
                                //BloodGroup
                                if (FieldNames.Contains("BloodGroup"))
                                {
                                    sQueryEmployee += " BloodGroup='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "BloodGroup");
                                    continue;
                                }
                                //NationalID
                                if (FieldNames.Contains("NationalID"))
                                {
                                    sQueryEmployee += " NationalID='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "NationalID");
                                    continue;
                                }
                                //PresentAddress
                                if (FieldNames.Contains("PresentAddress"))
                                {
                                    sQueryEmployee += " PresentAddress='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "PresentAddress");
                                    continue;
                                }
                                //PermanentAddress
                                if (FieldNames.Contains("PermanentAddress"))
                                {
                                    sQueryEmployee += " PermanentAddress='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "PermanentAddress");
                                    continue;
                                }
                                //Village
                                if (FieldNames.Contains("Village"))
                                {
                                    sQueryEmployee += " Village='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Village");
                                    continue;
                                }
                                //PostOffice
                                if (FieldNames.Contains("PostOffice"))
                                {
                                    sQueryEmployee += " PostOffice='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "PostOffice");
                                    continue;
                                }
                                //Thana
                                if (FieldNames.Contains("Thana"))
                                {
                                    sQueryEmployee += " Thana='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Thana");
                                    continue;
                                }
                                //District
                                if (FieldNames.Contains("District"))
                                {
                                    sQueryEmployee += " District='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "District");
                                    continue;
                                }
                                //PostCode
                                if (FieldNames.Contains("PostCode"))
                                {
                                    sQueryEmployee += " PostCode='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "PostCode");
                                    continue;
                                }
                                //MaritalStatus
                                if (FieldNames.Contains("MaritalStatus"))
                                {
                                    sQueryEmployee += " MaritalStatus='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "MaritalStatus");
                                    continue;
                                }
                                
                                #endregion

                                #region EmployeeOfficial Table

                                //DateOfJoin
                                if (FieldNames.Contains("DateOfJoin"))
                                {
                                    sQueryEmployeeOfficial += " DateOfJoin='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "DateOfJoin");
                                    continue;
                                }
                                //DateOfConfirmation
                                if (FieldNames.Contains("DateOfConfirmation"))
                                {
                                    sQueryEmployeeOfficial += " DateOfConfirmation='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "DateOfConfirmation");
                                    continue;
                                }
                                //EmployeeType
                                if (FieldNames.Contains("EmployeeType"))
                                {
                                    isEmpType = true;
                                    sQueryEmployeeOfficial += " EmployeeTypeID = ISNULL((select EmployeeTypeID from EmployeeType where name='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "'),0),";
                                    FieldNames.RemoveAll(x => x == ("EmployeeType"));

                                    sSelectEmployeeType = ",ISNULL((SELECT EmployeeTypeID FROM EmployeeType WHERE Name='" + oRows[i][j].ToString() + "'),0) AS EmployeeTypeID";
                                    continue;
                                }

                                //Employee Block
                                if (FieldNames.Contains("Block"))
                                {
                                    sEmpBlockName = oRows[i][j].ToString();
                                    FieldNames.RemoveAll(x => x == ("Block"));
                                    continue;
                                }

                                //Employee Group
                                if (FieldNames.Contains("Group"))
                                {
                                   sEmpGroupName=oRows[i][j].ToString();
                                   FieldNames.RemoveAll(x => x == ("Group"));
                                    continue;
                                }
                               
                                #endregion

                                #region EmployeeBankAccount Table

                                //BankCode
                                if (FieldNames.Contains("BankCode"))
                                {
                                    isBank = true;
                                    sQueryEmployeeBankAccount += " BankBranchID=ISNULL((SELECT top(1)BankBranchID FROM BankBranch WHERE BankID IN (SELECT BankID FROM Bank WHERE Code='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "')),0),";
                                    FieldNames.RemoveAll(x => x == "BankCode");

                                    sSelectBank = ",ISNULL((SELECT BankID FROM Bank WHERE Code='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "'),0) AS BankID";

                                    continue;
                                }
                                //AccountNo
                                if (FieldNames.Contains("AccountNo"))
                                {
                                    sQueryEmployeeBankAccount += " AccountNo='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "AccountNo");
                                    continue;
                                }
                                #endregion

                                #region EmployeeSalaryStructure Table

                                //BankAmount
                                if (FieldNames.Contains("BankAmount"))
                                {
                                    sQueryEmployeeSalaryStructure += " CashAmount=" + Convert.ToDouble(oRows[i][j] == DBNull.Value ? 0 : oRows[i][j]) + ", IsCashFixed=0,";
                                    FieldNames.RemoveAll(x => x == "BankAmount");
                                    continue;
                                }
                                //CashAmount
                                if (FieldNames.Contains("CashAmount"))
                                {
                                    sQueryEmployeeSalaryStructure += " CashAmount=" + Convert.ToDouble(oRows[i][j] == DBNull.Value ? 0 : oRows[i][j]) + ", IsCashFixed=1,";
                                    FieldNames.RemoveAll(x => x == "CashAmount");
                                    continue;
                                }
                                #endregion

                                #region EmployeeAuthentication Table
                                //CardNo
                                if (FieldNames.Contains("CardNo"))
                                {
                                    sQueryEmployeeAuthentication += " CardNo='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "CardNo");
                                    continue;
                                }
                                #endregion

                                #region EmployeeTINInformation
                                if (FieldNames.Contains("TIN"))
                                {
                                    sTIN = oRows[i][j].ToString();
                                    FieldNames.RemoveAll(x => x == "TIN");
                                    continue;
                                }

                                if (FieldNames.Contains("ETIN"))
                                {
                                    sETIN = oRows[i][j].ToString();
                                    FieldNames.RemoveAll(x => x == "ETIN");
                                    continue;
                                }
                                #endregion

                                #region Bangla Field
                                //CodeBangla
                                if (FieldNames.Contains("Code_Bangla"))
                                {
                                    sQueryEmployee += " CodeBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Code_Bangla");
                                    continue;
                                }
                                //NameInBangla
                                if (FieldNames.Contains("Name_InBangla"))
                                {
                                    sQueryEmployee += " NameInBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Name_InBangla");
                                    continue;
                                }
                                //FatherNameBangla
                                if (FieldNames.Contains("Father_NameBangla"))
                                {
                                    sQueryEmployee += " FatherNameBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Father_NameBangla");
                                    continue;
                                }
                                //MotherNameBangla
                                if (FieldNames.Contains("Mother_NameBangla"))
                                {
                                    sQueryEmployee += " MotherNameBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Mother_NameBangla");
                                    continue;
                                }
                                //NationalityBangla
                                if (FieldNames.Contains("Nationality_Bangla"))
                                {
                                    sQueryEmployee += " NationalityBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Nationality_Bangla");
                                    continue;
                                }
                                //NationalIDBangla
                                if (FieldNames.Contains("National_IDBangla"))
                                {
                                    sQueryEmployee += " NationalIDBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "National_IDBangla");
                                    continue;
                                }
                                //BloodGroupBangla
                                if (FieldNames.Contains("Blood_GroupBangla"))
                                {
                                    string sBG = "";
                                    string asd = oRows[i][j].ToString();
                                    int nBG = Convert.ToInt32(oRows[i][j].ToString().Trim());
                                    if (nBG == 1) sBG = "G c‡RwUf";
                                    if (nBG == 2) sBG = "G †b‡MwUf";
                                    if (nBG == 3) sBG = "we c‡RwUf";
                                    if (nBG == 4) sBG = "we †b‡MwUf";
                                    if (nBG == 5) sBG = "I c‡RwUf";
                                    if (nBG == 6) sBG = "I †b‡MwUf";
                                    if (nBG == 7) sBG = "Gwe c‡RwUf";
                                    if (nBG == 8) sBG = "Gwe †b‡MwUf";

                                    sQueryEmployee += " BloodGroupBangla='" + sBG + "',";
                                    FieldNames.RemoveAll(x => x == "Blood_GroupBangla");
                                    continue;
                                }
                                //HeightBangla
                                if (FieldNames.Contains("Height_Bangla"))
                                {
                                    sQueryEmployee += " HeightBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Height_Bangla");
                                    continue;
                                }
                                //WeightBangla
                                if (FieldNames.Contains("Weight_Bangla"))
                                {
                                    sQueryEmployee += " WeightBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Weight_Bangla");
                                    continue;
                                }
                                //DistrictBangla
                                if (FieldNames.Contains("District_BanglaName"))
                                {
                                    sQueryEmployee += " DistrictBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "District_BanglaName");
                                    continue;
                                }
                                //ThanaBangla
                                if (FieldNames.Contains("Thana_BanglaName"))
                                {
                                    sQueryEmployee += " ThanaBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Thana_BanglaName");
                                    continue;
                                }
                                //PostOfficeBangla
                                if (FieldNames.Contains("Post_OfficeBangla"))
                                {
                                    sQueryEmployee += " PostOfficeBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Post_OfficeBangla");
                                    continue;
                                }
                                //VillageBangla
                                if (FieldNames.Contains("Village_Name_Bangla"))
                                {
                                    sQueryEmployee += " VillageBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Village_Name_Bangla");
                                    continue;
                                }
                                //PresentAddressBangla
                                if (FieldNames.Contains("Present_AddressBangla"))
                                {
                                    sQueryEmployee += " PresentAddressBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Present_AddressBangla");
                                    continue;
                                }
                                //PermDistrictBangla
                                if (FieldNames.Contains("Perm_District_Bangla"))
                                {
                                    sQueryEmployee += " PermDistrictBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Perm_District_Bangla");
                                    continue;
                                }
                                //PermThanaBangla
                                if (FieldNames.Contains("Perm_Thana_Bangla"))
                                {
                                    sQueryEmployee += " PermThanaBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Perm_Thana_Bangla");
                                    continue;
                                }
                                //PermPostOfficeBangla
                                if (FieldNames.Contains("Perm_Post_Office_Bangla"))
                                {
                                    sQueryEmployee += " PermPostOfficeBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Perm_Post_Office_Bangla");
                                    continue;
                                }
                                //PermVillageBangla
                                if (FieldNames.Contains("Perm_Village_Bangla"))
                                {
                                    sQueryEmployee += " PermVillageBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Perm_Village_Bangla");
                                    continue;
                                }
                                //PermanentAddressBangla
                                if (FieldNames.Contains("Permanent_AddressBangla"))
                                {
                                    sQueryEmployee += " PermanentAddressBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Permanent_AddressBangla");
                                    continue;
                                }
                                //ReligionBangla
                                if (FieldNames.Contains("Religion_Bangla"))
                                {
                                    string sReligion = "";
                                    string asd = oRows[i][j].ToString();
                                    int nReligion = Convert.ToInt16(oRows[i][j].ToString().Trim());
                                    if (nReligion == 1) sReligion = "Bmjvg";
                                    if (nReligion == 2) sReligion = "wn›`y";
                                    if (nReligion == 3) sReligion = "wL«÷vb";
                                    if (nReligion == 4) sReligion = "‡eŠ× ";

                                    sQueryEmployee += " ReligionBangla='" + sReligion + "',";
                                    FieldNames.RemoveAll(x => x == "Religion_Bangla");
                                    continue;
                                }
                                //MaritalStatusBangla
                                if (FieldNames.Contains("Marital_Status_Bangla"))
                                {
                                    string sMS = "";
                                    int nMS = Convert.ToInt16(oRows[i][j].ToString().Trim());
                                    if (nMS == 1) sMS = "weevwnZ";
                                    if (nMS == 2) sMS = "AweevwnZ";

                                    sQueryEmployee += " MaritalStatusBangla='" + sMS + "',";
                                    FieldNames.RemoveAll(x => x == "Marital_Status_Bangla");
                                    continue;
                                }
                                //NomineeBangla
                                if (FieldNames.Contains("Nominee_Bangla"))
                                {
                                    sQueryEmployee += " NomineeBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Nominee_Bangla");
                                    continue;
                                }
                                //AuthenticationBangla
                                if (FieldNames.Contains("Authentication_Bangla"))
                                {
                                    sQueryEmployee += " AuthenticationBangla='" + Convert.ToString(oRows[i][j] == DBNull.Value ? "" : oRows[i][j]) + "',";
                                    FieldNames.RemoveAll(x => x == "Authentication_Bangla");
                                    continue;
                                }
                                #endregion
                            }

                            if (bFlag == true)
                            {
                                if (nEmpLength != sQueryEmployee.Length)
                                {
                                    sQueryEmployee = sQueryEmployee.Remove(sQueryEmployee.Length - 1, 1);
                                    sQueryEmployee += " WHERE EmployeeID=(SELECT EmployeeID FROM Employee WHERE Code='" + EmpCode + "') AND IsActive=1";

                                }
                                if (nEmpOffLength != sQueryEmployeeOfficial.Length)
                                {
                                    sQueryEmployeeOfficial = sQueryEmployeeOfficial.Remove(sQueryEmployeeOfficial.Length - 1, 1);
                                    sQueryEmployeeOfficial += " WHERE EmployeeID=(SELECT EmployeeID FROM Employee WHERE Code='" + EmpCode + "') AND IsActive=1";
                                }
                                if (nEmpBankAccLength != sQueryEmployeeBankAccount.Length)
                                {
                                    sQueryEmployeeBankAccount = sQueryEmployeeBankAccount.Remove(sQueryEmployeeBankAccount.Length - 1, 1);
                                    sQueryEmployeeBankAccount += " WHERE EmployeeID=(SELECT EmployeeID FROM Employee WHERE Code='" + EmpCode + "') AND IsActive=1";
                                }
                                if (nEmpSalaryStructureLength!= sQueryEmployeeSalaryStructure.Length)
                                {
                                    sQueryEmployeeSalaryStructure = sQueryEmployeeSalaryStructure.Remove(sQueryEmployeeSalaryStructure.Length - 1, 1);
                                    sQueryEmployeeSalaryStructure += " WHERE EmployeeID=(SELECT EmployeeID FROM Employee WHERE Code='" + EmpCode + "') AND IsActive=1";
                                }
                                if (nEmpAutLength != sQueryEmployeeAuthentication.Length)
                                {
                                    sQueryEmployeeAuthentication = sQueryEmployeeAuthentication.Remove(sQueryEmployeeAuthentication.Length - 1, 1);
                                    sQueryEmployeeAuthentication += " WHERE EmployeeID=(SELECT EmployeeID FROM Employee WHERE Code='" + EmpCode + "') AND IsActive=1";
                                }

                              
                                sFinalQuery += (nEmpLength == sQueryEmployee.Length) ? "" : (sQueryEmployee + ";");
                                sFinalQuery += (nEmpOffLength == sQueryEmployeeOfficial.Length) ? "" : (sQueryEmployeeOfficial + ";");
                                sFinalQuery += (nEmpBankAccLength == sQueryEmployeeBankAccount.Length) ? "" : (sQueryEmployeeBankAccount + ";");
                                sFinalQuery += (nEmpAutLength == sQueryEmployeeAuthentication.Length) ? "" : (sQueryEmployeeAuthentication + ";");
                                sFinalQuery += (nEmpSalaryStructureLength == sQueryEmployeeSalaryStructure.Length) ? "" : (sQueryEmployeeSalaryStructure + ";");
                                sFinalQuery += sSelectEmployeeID + sSelectBank + sSelectEmployeeType;

                                oEmployee = Employee.UploadEmpBasicXLWithConfig(sFinalQuery, (int)Session[SessionInfo.currentUserID]);
                                string sFeedbackMessage = "";
                                if(oEmployee.EmployeeID>0&& sEmpBlockName!="")
                                {
                                    EmployeeGroup oEmployeeGroup = new EmployeeGroup();
                                    oEmployeeGroup.EGID = 0;
                                    oEmployeeGroup.EmployeeID =oEmployee.EmployeeID;
                                    oEmployeeGroup.EmployeeTypeID = 0;
                                    oEmployeeGroup.IsBlock = true;
                                    oEmployeeGroup.Name = sEmpBlockName;
                                    sFeedbackMessage = oEmployeeGroup.Upload((int)Session[SessionInfo.currentUserID]);

                                }
                               
                                if (oEmployee.EmployeeID > 0 && sEmpGroupName != "")
                                {
                                    EmployeeGroup oEmployeeGroup = new EmployeeGroup();
                                    oEmployeeGroup.EGID = 0;
                                    oEmployeeGroup.EmployeeID = oEmployee.EmployeeID;
                                    oEmployeeGroup.EmployeeTypeID = 0;
                                    oEmployeeGroup.IsBlock = false;
                                    oEmployeeGroup.Name = sEmpGroupName;
                                    sFeedbackMessage += oEmployeeGroup.Upload((int)Session[SessionInfo.currentUserID]);

                                }

                                string sFeedBackMessageTIN = "";
                                if(oEmployee.EmployeeID>0 && (sTIN!=""|| sETIN!=""))
                                {
                                    EmployeeTINInformation oEmployeeTINInformation = new EmployeeTINInformation();
                                    oEmployeeTINInformation.ETINID = 0;
                                    oEmployeeTINInformation.EmployeeID = oEmployee.EmployeeID;
                                    oEmployeeTINInformation.TIN = sTIN;
                                    oEmployeeTINInformation.ETIN = sETIN;
                                    oEmployeeTINInformation.TaxArea = 0;
                                    oEmployeeTINInformation.Circle = "";
                                    oEmployeeTINInformation.Zone = "";
                                    oEmployeeTINInformation.IsNonResident = false;
                                    sFeedBackMessageTIN = oEmployeeTINInformation.Upload((int)Session[SessionInfo.currentUserID]);

                                }
                                

                                if (isEmpID && oEmployee.EmployeeID == 0)
                                {
                                    oEmployee.ErrorMessage = "No Active Employee Found by this Code";
                                }
                                if (!string.IsNullOrEmpty(sFeedbackMessage))
                                {
                                    oEmployee.ErrorMessage += sFeedbackMessage;
                                }
                                if (!string.IsNullOrEmpty(sFeedBackMessageTIN))
                                {
                                    oEmployee.ErrorMessage += sFeedBackMessageTIN;
                                }

                                if (isEmpType && oEmployee.EmployeeTypeID == 0)
                                {
                                    oEmployee.ErrorMessage += (!string.IsNullOrEmpty(oEmployee.ErrorMessage))?", ":"No Employee Type Found by this Name";
                                }
                              
                                if (isBank && oEmployee.BankID == 0)
                                {
                                    oEmployee.ErrorMessage += (!string.IsNullOrEmpty(oEmployee.ErrorMessage))?", ":"No Bank Found by this Code";
                                }
                                if (!string.IsNullOrEmpty(oEmployee.ErrorMessage))
                                {
                                    oEmployee.Code = EmpCode;
                                    oEmployees.Add(oEmployee);
                                }
                                isEmpID = false;
                                isBank = false;
                                isEmpType = false;
                                sFinalQuery = "";
                            }
                        }

                        if (System.IO.File.Exists(fileDirectory))
                        {
                            System.IO.File.Delete(fileDirectory);
                        }
                    }
                }
                else
                {
                    throw new Exception("File not supported");
                }
            }
            return oEmployees;
        }

        [HttpPost]
        public ActionResult View_Upload(HttpPostedFileBase uploadedFile, string UploadModule)
        {
            string sType = UploadModule.Split('~')[0];
            int nUploadType = Convert.ToInt32(UploadModule.Split('~')[1]);

            #region AttendanceUpload
            if (sType == "Att")
            {
                List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
                List<AttendanceDaily> oADs = new List<AttendanceDaily>();
                AttendanceDaily oAttendanceDaily = new AttendanceDaily();
                try
                {
                    if (uploadedFile == null) { throw new Exception("File not Found"); }
                    oAttendanceDailys = this.GetAttFromExcel(uploadedFile);

                    if (oAttendanceDailys.Count() <= 0)
                        throw new Exception("Nothing found to Upload");

                    oADs = AttendanceDaily.UploadAttXL(oAttendanceDailys, IsNUInTime, IsNUOutTime, IsNULate, IsNUEarly, IsNUInDate, IsNUOutDate, IsNUOT, IsNURemark, true, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oADs.Count > 0)
                    {
                        int nRowIndex = 2, nStartCol = 2, nEndCol = 2;
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
                            sheet.Column(n++).Width = 13;
                            sheet.Column(n++).Width = 13;
                            sheet.Column(n++).Width = 13;
                            sheet.Column(n++).Width = 13;
                            sheet.Column(n++).Width = 13;
                            sheet.Column(n++).Width = 13;
                            sheet.Column(n++).Width = 25;

                            colIndex = 1;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "InDateTime"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OutDateTime"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Late"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Early"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OT"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Remark"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Error"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            rowIndex += 1;

                            foreach (AttendanceDaily oItem in oADs)
                            {
                                colIndex = 1;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.CompInTimeInString; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.CompOutTimeInString; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.CompLateArrivalMinute; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.CompEarlyDepartureMinute; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.CompOverTimeInMinute; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Remark; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ErrorMessage; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            }

                            Response.ClearContent();
                            Response.BinaryWrite(excelPackage.GetAsByteArray());
                            Response.AddHeader("content-disposition", "attachment; filename=ErrorList.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.FeedBack = ex.Message;
                    ViewBag.FeedBackText = "Unsuccessful";
                }
                ViewBag.FeedBackText = "successful";
            }
            #endregion


            #region EmployeeBasicInfo
            if (sType == "Emp")
            {
                List<Employee_UploadXL> oEXLs = new List<Employee_UploadXL>();
                Employee_UploadXL oEXL = new Employee_UploadXL();
                List<Employee> oEmps = new List<Employee>();

                try
                {
                    _oUploadConfigure = new UploadConfigure();
                    _oUploadConfigure = _oUploadConfigure.Get(nUploadType, (int)Session[SessionInfo.currentUserID]);

                    if (uploadedFile == null) { throw new Exception("File not Found"); }
                    oEmps = this.GetEmployeeFromExcelWithConfigure(uploadedFile, _oUploadConfigure);
                    if (oEmps.Count <= 0)
                    {
                        ViewBag.FeedBackText = "successful";
                    }
                    if (oEmps.Count > 0)
                    {
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
                            sheet.Column(n++).Width = 35;

                            colIndex = 1;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Error"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            rowIndex += 1;

                            foreach (Employee oItem in oEmps)
                            {
                                colIndex = 1;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Code; cell.Style.Font.Bold = false;
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

                }
                catch (Exception ex)
                {
                    ViewBag.FeedBack = ex.Message;
                    ViewBag.FeedBackText = "Unsuccessful";
                    return View(oEmps);
                }
                return View(oEmps);
            }
            #endregion

            return View(new AttendanceDaily());
        }

        public void DownloadFormat()
        {
            int nRowIndex = 2, nStartCol = 2, nEndCol = 2;
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;
            ExcelFill fill;
            int colIndex = 1;
            int rowIndex = 1;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Format Downlaod");
                sheet.Name = "Format Downlaod";

                int n = 1;
                sheet.Column(n++).Width = 13;//code
                sheet.Column(n++).Width = 13;//status
                sheet.Column(n++).Width = 13;//indate
                sheet.Column(n++).Width = 13;//intime
                sheet.Column(n++).Width = 13;//outdate
                sheet.Column(n++).Width = 13;//outtime
                sheet.Column(n++).Width = 13;//late
                sheet.Column(n++).Width = 13;//early
                sheet.Column(n++).Width = 13;//OT
                sheet.Column(n++).Width = 13;//Remark

                sheet.Column(n++).Width = 13;//blank
                sheet.Column(n++).Width = 13;//blank
                sheet.Column(n++).Width = 13;//blank

                sheet.Column(n++).Width = 20;//
                sheet.Column(n++).Width = 20;//
                sheet.Column(n++).Width = 30;//

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "25-Sep-17"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                
                rowIndex++;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "InDate"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "InTime"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OutDate"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OutTime"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Late"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Early"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OT"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Remark"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, 14]; cell.Value = "Legend"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                
                cell = sheet.Cells[rowIndex, 15]; cell.Value = "If Holiday, No effect"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                
                rowIndex += 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "10001"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "P"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "25/9/2017"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "8:20"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "25/9/2017"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "16:10"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "10"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "20"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "40"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Remark"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, 14]; cell.Value = "A"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 15]; cell.Value = "Absent"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 14]; cell.Value = "P"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 15]; cell.Value = "Present"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 16]; cell.Value = "Must Enter in time and out time"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;


                cell = sheet.Cells[rowIndex, 14]; cell.Value = "OFF"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 15]; cell.Value = "Dayoff"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 14, rowIndex, 16]; cell.Value = "For leave use short name of leave head"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                cell = sheet.Cells[rowIndex, 14, rowIndex, 16]; cell.Value = "NU=Not Update"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Format.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void SampleFormatDownloadEmpBasicInfoTest()
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
                var sheet = excelPackage.Workbook.Worksheets.Add("Format Downlaod");
                sheet.Name = "Format Downlaod";

                int n = 1;
                sheet.Column(n++).Width = 13;//code
                sheet.Column(n++).Width = 13;//NameInBangla
                sheet.Column(n++).Width = 13;//ContactNo
                sheet.Column(n++).Width = 13;//Email
                sheet.Column(n++).Width = 13;//Father
                sheet.Column(n++).Width = 13;//Husband
                sheet.Column(n++).Width = 13;//Mother
                sheet.Column(n++).Width = 13;//BGroup
                sheet.Column(n++).Width = 13;//NationalID
                sheet.Column(n++).Width = 13;//PresentAdd
                sheet.Column(n++).Width = 13;//PermanentAdd
                sheet.Column(n++).Width = 13;//DOB
                sheet.Column(n++).Width = 13;//Village
                sheet.Column(n++).Width = 13;//PostOffice
                sheet.Column(n++).Width = 13;//Thana
                sheet.Column(n++).Width = 13;//District
                sheet.Column(n++).Width = 13;//PostCode
                sheet.Column(n++).Width = 13;//Grade
                sheet.Column(n++).Width = 13;//PriximityCard


                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "NameInBangla"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ContactNo"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Email"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "FathersName"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "HusbandName"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "MothersName"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BloodGroup"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "NationalID"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "PresentAddress"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "PermanentAddress"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DOB"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Village"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "PostOffice"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Thana"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "District"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "PostCode"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Grade"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ProximityCardNo"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "10001"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "নাম"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "0171000000"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "email@g.com"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Father Name (Bangla/Eng)"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Husband Name (Bangla/Eng)"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Mother Name (Bangla/Eng)"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "A+"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1000000000000000"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Present Address"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Permanent Address"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "01 Jan 2018"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Village"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Post Office"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Thana"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "District"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Post Code"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Grade Name"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Proximity Card No"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Format.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        public void SampleFormatDownloadEmpBasicInfo(int nUploadType)
        {

            _oUploadConfigure = new UploadConfigure();
            _oUploadConfigure = _oUploadConfigure.Get(nUploadType, (int)Session[SessionInfo.currentUserID]);


            ExcelRange cell;
            OfficeOpenXml.Style.Border border;
            ExcelFill fill;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Format Downlaod");
                sheet.Name = "Format Downlaod";

                if (_oUploadConfigure.UploadConfigureID > 0)
                {
                    string[] FieldNames, ColumnNames;
                    //FieldNames = _oUploadConfigure.FieldNames.Split(',');
                    ColumnNames = _oUploadConfigure.CaptionNames.Split(',');

                    int nRowIndex = 1, nEndRow = 0, nStartCol = 1, nEndCol = ColumnNames.Count(), nTempCol = 1;
                    int n = 1;
                    foreach (string items in ColumnNames)
                    {
                        sheet.Column(n++).Width = 13;//code
                    }

                    foreach (string items in ColumnNames)
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = items; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nRowIndex += 1;
                    nTempCol = 1;

                    foreach (string items in ColumnNames)
                    {
                        if (items == "Code")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "1001"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Name")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Mr. A"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "FathersName")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Mr. F"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "HusbandName")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Mr. H"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "MothersName")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Mrs. M"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Date of Birth")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "22/12/1995"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (items == "Gender")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Male"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Date of join")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "22 Dec 1995"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Date of Confirmation")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "22/12/1995"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Proximity Card No")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "1000001"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Bank Code")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "1001"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "AccNo")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "101202303"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }


                        if (items == "Employee Category")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Permanent"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Bank Amount")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "15000"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Cash Amount")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "20000"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        //if (items == "NameInBangla")
                        //{
                        //    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "aaaaa"; cell.Style.Font.Bold = false;
                        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        //}
                        if (items == "ContactNo")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "01710000000"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Email")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "a@g.com"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "BloodGroup")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "A+"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "NationalID")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "12154964612"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "PresentAddress")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "village, post, thana, district"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "PermanentAddress")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "village, post, thana, district"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Village")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Village"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "PostOffice")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "PostOffice"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Thana")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Thana"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }


                        if (items == "District")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "District"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "PostCode")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "1211"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "EmployeeType")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Non Grade"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Marital Status")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Married"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Reporting Person")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "1001"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        if (items == "Block")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Block"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Group")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Group"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "TIN")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "1234567"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "ETIN")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "1234567"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        #region Bangla
                        if (items == "Code Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Name In Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Father Name Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Mother Name Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Nationality Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "National ID Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Blood Group Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "0"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Height Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "0"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Weight Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "0"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "District Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Thana Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Post Office Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Village Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Present Address Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Perm District Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Perm Thana Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Perm Post Office Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Perm Village Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Permanent Address Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Religion Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "0"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Marital Status Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "0"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Nominee Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (items == "Authentication Bangla")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "evsjv‡`k"; cell.Style.Font.Bold = false; cell.Style.Font.Name = "SutonnyMJ";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        #endregion
                    }
                }

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EmployeeBasicUploadFormat.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }


        #endregion

        
    }
 
}


