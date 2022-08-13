using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class AttendanceDailyService : MarshalByRefObject, IAttendanceDailyService
    {
        #region Private functions and declaration
        private AttendanceDaily MapObject(NullHandler oReader)
        {
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();

            oAttendanceDaily.LastPunchDate = oReader.GetDateTime("LastPunchDate");
            oAttendanceDaily.AbsentFrom = oReader.GetDateTime("AbsentFrom");
            oAttendanceDaily.AbsentDayCount = oReader.GetInt32("AbsentDayCount");

            //oAttendanceDaily.TotalAttendanceCount = oReader.GetInt32("TotalAttendanceCount");

            oAttendanceDaily.AttendanceID = oReader.GetInt32("AttendanceID");
            oAttendanceDaily.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oAttendanceDaily.EmployeeID = oReader.GetInt32("EmployeeID");
            oAttendanceDaily.AttendanceSchemeID = oReader.GetInt32("AttendanceSchemeID");
            oAttendanceDaily.LocationID = oReader.GetInt32("LocationID");
            oAttendanceDaily.DepartmentID = oReader.GetInt32("DepartmentID");
            oAttendanceDaily.DesignationID = oReader.GetInt32("DesignationID");
            oAttendanceDaily.RosterPlanID = oReader.GetInt32("RosterPlanID");
            oAttendanceDaily.ShiftID = oReader.GetInt32("ShiftID");
            oAttendanceDaily.AttendanceDate = oReader.GetDateTime("AttendanceDate");
            oAttendanceDaily.InTime = oReader.GetDateTime("InTime");
            oAttendanceDaily.OutTime = oReader.GetDateTime("OutTime");
            oAttendanceDaily.CompInTime = oReader.GetDateTime("CompInTime");
            oAttendanceDaily.CompOutTime = oReader.GetDateTime("CompOutTime");
            oAttendanceDaily.LateArrivalMinute = oReader.GetInt32("LateArrivalMinute");
            oAttendanceDaily.CompLateArrivalMinute = oReader.GetInt32("CompLateArrivalMinute");
            oAttendanceDaily.EarlyDepartureMinute = oReader.GetInt32("EarlyDepartureMinute");
            oAttendanceDaily.CompEarlyDepartureMinute = oReader.GetInt32("CompEarlyDepartureMinute");
            oAttendanceDaily.TotalWorkingHourInMinute = oReader.GetInt32("TotalWorkingHourInMinute");
            oAttendanceDaily.CompTotalWorkingHourInMinute = oReader.GetInt32("CompTotalWorkingHourInMinute");
            oAttendanceDaily.OverTimeInMinute = oReader.GetInt32("OverTimeInMinute");

            oAttendanceDaily.CompLateArrivalMinute = oReader.GetInt32("CompLateArrivalMinute");
            oAttendanceDaily.CompEarlyDepartureMinute = oReader.GetInt32("CompEarlyDepartureMinute");
            oAttendanceDaily.CompTotalWorkingHourInMinute = oReader.GetInt32("CompTotalWorkingHourInMinute");
            oAttendanceDaily.CompOverTimeInMinute = oReader.GetInt32("CompOverTimeInMinute");

            oAttendanceDaily.IsDayOff = oReader.GetBoolean("IsDayOff");
            oAttendanceDaily.IsLeave = oReader.GetBoolean("IsLeave");
            oAttendanceDaily.IsUnPaid = oReader.GetBoolean("IsUnPaid");

            oAttendanceDaily.IsHoliday = oReader.GetBoolean("IsHoliday");
            oAttendanceDaily.IsCompDayOff = oReader.GetBoolean("IsCompDayOff");
            oAttendanceDaily.IsCompLeave = oReader.GetBoolean("IsCompLeave");
            oAttendanceDaily.IsCompHoliday = oReader.GetBoolean("IsCompHoliday");
            

            oAttendanceDaily.WorkingStatus = (EnumEmployeeWorkigStatus)oReader.GetInt16("WorkingStatus");
            oAttendanceDaily.WorkingStatusInt =(int)(EnumEmployeeWorkigStatus)oReader.GetInt16("WorkingStatus");
            oAttendanceDaily.Note = oReader.GetString("Note");
            oAttendanceDaily.APMID = oReader.GetInt32("APMID");
            oAttendanceDaily.IsLock = oReader.GetBoolean("IsLock");
            oAttendanceDaily.IsNoWork = oReader.GetBoolean("IsNoWork");
            oAttendanceDaily.IsManual = oReader.GetBoolean("IsManual");
            oAttendanceDaily.LastUpdatedBY = oReader.GetInt32("LastUpdatedBY");
            oAttendanceDaily.LastUpdatedDate = oReader.GetDateTime("LastUpdatedDate");
            //derive
            oAttendanceDaily.EmployeeName = oReader.GetString("EmployeeName");
            oAttendanceDaily.EmployeeCode = oReader.GetString("Code");
            oAttendanceDaily.EmployeeCode = oReader.GetString("Code");
            oAttendanceDaily.AttendanceSchemeName = oReader.GetString("AttendanceScheme");
            oAttendanceDaily.DepartmentName = oReader.GetString("Department");
            oAttendanceDaily.DesignationName = oReader.GetString("Designation");
            oAttendanceDaily.HRM_ShiftName = oReader.GetString("HRM_Shift");
            //oAttendanceDaily.EmployeeTypeName = oReader.GetString("EmployeeType");
            oAttendanceDaily.RosterPlanName = oReader.GetString("RosterPlanName");
            oAttendanceDaily.LocationName = oReader.GetString("LocationName");
            oAttendanceDaily.IsProductionBase = oReader.GetBoolean("IsProductionBase");
            oAttendanceDaily.DateOfBirth = oReader.GetDateTime("DateOfBirth");
            oAttendanceDaily.JoiningDate = oReader.GetDateTime("JoiningDate");
            oAttendanceDaily.LeaveHeadID = oReader.GetInt32("LeaveHeadID");
            oAttendanceDaily.CompLeaveHeadID = oReader.GetInt32("CompLeaveHeadID");
            oAttendanceDaily.LeaveStatus = oReader.GetString("LeaveStatus");
            oAttendanceDaily.LeaveType =  (EnumLeaveType) oReader.GetInt16("LeaveType");
            oAttendanceDaily.IsOSD = oReader.GetBoolean("IsOSD");
            oAttendanceDaily.BUName = oReader.GetString("BUName");
            oAttendanceDaily.Remark = oReader.GetString("Remark");
            oAttendanceDaily.Gender = oReader.GetString("Gender");

            oAttendanceDaily.IsManualOT = oReader.GetBoolean("IsManualOT");
            oAttendanceDaily.MOCID = oReader.GetInt32("MOCID");
            oAttendanceDaily.MOCAID = oReader.GetInt32("MOCAID");

            return oAttendanceDaily;

        }

        private AttendanceDaily MapObjectForReport(NullHandler oReader)
        {
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();
            oAttendanceDaily.EmployeeName = oReader.GetString("EmployeeName");
            oAttendanceDaily.EmployeeCode = oReader.GetString("Code");
            oAttendanceDaily.DesignationName = oReader.GetString("Designation");
            oAttendanceDaily.EmployeeTypeName = oReader.GetString("EmployeeType");

            oAttendanceDaily.TotalWorkingDay = oReader.GetInt32("TotalWorkingDay");
            oAttendanceDaily.TotalShift = oReader.GetInt32("TotalShift");
            oAttendanceDaily.TotalWorkingHour = oReader.GetDouble("TotalWorkingHour");
            oAttendanceDaily.PresentShift = oReader.GetInt32("PresentShift");
            oAttendanceDaily.AbsentShift = oReader.GetInt32("AbsentShift");
            oAttendanceDaily.IsDayOFFs = oReader.GetInt32("IsDayOFF");
            oAttendanceDaily.Leave = oReader.GetInt32("Leave");
            oAttendanceDaily.Paid = oReader.GetInt32("Paid");
            oAttendanceDaily.InWorkPlace = oReader.GetInt32("InWorkPlace");
            oAttendanceDaily.OSD = oReader.GetInt32("OSD");
            oAttendanceDaily.Suspended = oReader.GetInt32("Suspended");
            oAttendanceDaily.Holiday = oReader.GetInt32("Holiday");
            oAttendanceDaily.Late = oReader.GetInt32("Late");
            oAttendanceDaily.EarlyLeaving = oReader.GetInt32("EarlyLeaving");
            oAttendanceDaily.OvertimeInhour = oReader.GetDouble("OvertimeInhour");
            return oAttendanceDaily;
        }

        private AttendanceDaily CreateObject(NullHandler oReader)
        {
            AttendanceDaily oAttendanceDaily = MapObject(oReader);
            return oAttendanceDaily;
        }

        private List<AttendanceDaily> CreateObjects(IDataReader oReader)
        {
            List<AttendanceDaily> oAttendanceDaily = new List<AttendanceDaily>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceDaily oItem = CreateObject(oHandler);
                oAttendanceDaily.Add(oItem);
            }
            return oAttendanceDaily;
        }

        private AttendanceDaily CreateObjectForReport(NullHandler oReader)
        {
            AttendanceDaily oAttendanceDaily = MapObjectForReport(oReader);
            return oAttendanceDaily;
        }

        private List<AttendanceDaily> CreateObjectsForReport(IDataReader oReader)
        {
            List<AttendanceDaily> oAttendanceDaily = new List<AttendanceDaily>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceDaily oItem = CreateObjectForReport(oHandler);
                oAttendanceDaily.Add(oItem);
            }
            return oAttendanceDaily;
        }


        #endregion

        #region Interface implementation
        public AttendanceDailyService() { }

        public AttendanceDaily IUD(AttendanceDaily oAttendanceDaily, int nDBOperation, Int64 nUserID)
        {
            string[] sEmpIDs;
            sEmpIDs = oAttendanceDaily.ErrorMessage.Split(',');
            oAttendanceDaily.ErrorMessage = "";
            DateTime InTimeRandomStart = DateTime.Now;
            DateTime InTimeRandomEnd = DateTime.Now;
            DateTime OutTimeRandomStart = DateTime.Now;
            DateTime OutTimeRandomEnd = DateTime.Now;

            bool isinTimeRandom = Convert.ToBoolean(oAttendanceDaily.sRandom.Split('~')[0]);
            if (isinTimeRandom)
            {
                InTimeRandomStart = Convert.ToDateTime(oAttendanceDaily.sRandom.Split('~')[1]);
                InTimeRandomEnd = Convert.ToDateTime(oAttendanceDaily.sRandom.Split('~')[2]);
            }
            bool isoutTimeRandom = Convert.ToBoolean(oAttendanceDaily.sRandom.Split('~')[3]);
            if (isoutTimeRandom)
            {
                OutTimeRandomStart = Convert.ToDateTime(oAttendanceDaily.sRandom.Split('~')[4]);
                OutTimeRandomEnd = Convert.ToDateTime(oAttendanceDaily.sRandom.Split('~')[5]);
            }
            Random rand = new Random();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (string sEmpID in sEmpIDs)
                {
                    if (isinTimeRandom)
                    {
                        TimeSpan ts = InTimeRandomStart - InTimeRandomEnd;
                        int nRandomMinute = rand.Next(Math.Abs(ts.Minutes));
                        oAttendanceDaily.InTime = InTimeRandomStart.AddMinutes(nRandomMinute);
                    }
                    if (isoutTimeRandom)
                    {
                        TimeSpan ts = OutTimeRandomStart - OutTimeRandomEnd;
                        int nRandomMinute = rand.Next(Math.Abs(ts.Minutes));
                        oAttendanceDaily.OutTime = OutTimeRandomStart.AddMinutes(nRandomMinute);
                    }

                    int EmployeeID = Convert.ToInt32(sEmpID);
                    oAttendanceDaily.EmployeeID = EmployeeID;
                    reader = AttendanceDailyDA.IUD(tc, oAttendanceDaily, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {

                        oAttendanceDaily = CreateObject(oReader);
                    }
                    reader.Close();
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttendanceDaily.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oAttendanceDaily;
        }
        public DataSet GetsDataSet(string sSQL, Int64 nUserID)
        {
            TransactionContext tc = null;
            DataSet oDataSet = new DataSet();
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttachDocumentDA.GetsDataSet(tc, sSQL);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[2]);
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DataSet", e);
                #endregion
            }
            return oDataSet;
        }
        public AttendanceDaily Get(int nEmployeeID, DateTime dAttendanceDate, Int64 nUserId)
        {
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AttendanceDailyDA.Get(tc, nEmployeeID, dAttendanceDate);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceDaily = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get AttendanceDaily", e);
                oAttendanceDaily.ErrorMessage = e.Message;
                #endregion
            }

            return oAttendanceDaily;
        }

        public AttendanceDaily Get(string sSQL, Int64 nUserId)
        {
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AttendanceDailyDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceDaily = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                //oAttendanceDaily.ErrorMessage = e.Message;
                #endregion
            }

            return oAttendanceDaily;
        }

        public List<AttendanceDaily> GetsContinuousAbsent(DateTime DateFrom, DateTime DateTo, string BUIDs, string LocIDs, string DepartmentIDs, string DesignationIDs, int DayCount, Int64 nUserID)
        {
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceDailyDA.GetsContinuousAbsent(DateFrom, DateTo, BUIDs, LocIDs, DepartmentIDs, DesignationIDs, DayCount, nUserID, tc);
                oAttendanceDailys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oAttendanceDailys;
        }
        public List<AttendanceDaily> GetsRecord(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, Int64 nUserID)
        {
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceDailyDA.GetsRecord(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds, nUserID, tc);
                //oAttendanceDailys = CreateObjects(reader);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    AttendanceDaily oItem = new AttendanceDaily();
                    oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                    oItem.AttendanceDate = oreader.GetDateTime("AttendanceDate");
                    oItem.InTime = oreader.GetDateTime("InTime");
                    oItem.OutTime = oreader.GetDateTime("OutTime");
                    oItem.OverTimeInMinute = oreader.GetInt32("OverTimeInMinute");
                    oItem.IsDayOff = oreader.GetBoolean("IsDayOff");
                    oItem.IsHoliday = oreader.GetBoolean("IsHoliday");
                    oItem.IsLeave = oreader.GetBoolean("IsLeave");
                    oItem.LeaveHeadID = oreader.GetInt32("LeaveHeadID");
                    oItem.LeaveType = (EnumLeaveType) oreader.GetInt32("LeaveType");

                    oAttendanceDailys.Add(oItem);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oAttendanceDailys;
        }
        public List<AttendanceDaily> Gets(string sSQL, Int64 nUserID)
        {
            List<AttendanceDaily> oAttendanceDaily = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceDailyDA.Gets(sSQL, tc);
                oAttendanceDaily = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oAttendanceDaily;
        }

        public List<AttendanceDaily> GetsForReport(string sSQL, Int64 nUserID)
        {
            List<AttendanceDaily> oAttendanceDaily = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceDailyDA.Gets(sSQL, tc);
                oAttendanceDaily = CreateObjectsForReport(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceDaily", e);
                #endregion
            }
            return oAttendanceDaily;
        }

        #region No Work
        public List<AttendanceDaily> NoWorkExecution(List<AttendanceDaily> oAttendanceDailys, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                string sAttIDs = "";
                foreach (AttendanceDaily oItem in oAttendanceDailys)
                {
                    if (oItem.IsNoWork == true)
                    {
                        throw new Exception("Some of attendance is already no work!");
                    }
                    if (oItem.IsLock == false)
                    {
                        throw new Exception("Some of attendance is not Locked! Plese select items which are locked !");
                    }
                    //if (oItem.InTimeInString == "00:00" && oItem.OutTimeInString == "00:00")
                    //{
                    //    throw new Exception("Some employee was absent in this date!");

                    //}
                    //if (oItem.IsDayOff == true)
                    //{
                    //    throw new Exception("Some employee had off day in this date!");

                    //}
                    //if (oItem.IsLeave == true)
                    //{
                    //    throw new Exception("Some employee took leave in this date!");

                    //}
                    //if (oItem.IsProductionBase == false)
                    //{
                    //    throw new Exception("Some employee are not production Base!");

                    //}

                    string sSalarySql = "SELECT ISNULL( COUNT(*),0) as TotalCount FROM View_EmployeeSalary WHERE EmployeeID=" + oItem.EmployeeID + " AND StartDate <= '" + oItem.AttendanceDate + "' AND EndDate >= '" + oItem.AttendanceDate + "' ";
                    tc = TransactionContext.Begin(true);
                    bool IsSalaryProcessed = EmployeeSalaryDA.GetSalary(sSalarySql, tc);
                    tc.End();

                    if (IsSalaryProcessed == true)
                    {
                        throw new Exception("Salary is processed for some employee. So execution is not possible !");
                    }

                    sAttIDs += oItem.AttendanceID + ",";

                }
                sAttIDs = sAttIDs.Substring(0, sAttIDs.Length - 1);
                string sSql = "UPDATE AttendanceDaily SET IsNoWork=1 WHERE AttendanceID IN(" + sAttIDs + "); SELECT * FROM View_AttendanceDaily WHERE AttendanceID IN(" + sAttIDs + ")";

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AttendanceDailyDA.Gets(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                oAttendanceDailys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttendanceDailys[0].ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oAttendanceDailys;
        }

        public List<AttendanceDaily> CancelNoWorkExecution(List<AttendanceDaily> oAttendanceDailys, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                string sAttIDs = "";
                foreach (AttendanceDaily oItem in oAttendanceDailys)
                {
                    if (oItem.IsNoWork == false)
                    {
                        throw new Exception("This item is not in no work!");
                    }

                    string sSalarySql = "SELECT ISNULL( COUNT(*),0) as TotalCount FROM View_EmployeeSalary WHERE EmployeeID=" + oItem.EmployeeID + " AND StartDate <= '" + oItem.AttendanceDate + "' AND EndDate >= '" + oItem.AttendanceDate + "' ";
                    tc = TransactionContext.Begin(true);
                    bool IsSalaryProcessed = EmployeeSalaryDA.GetSalary(sSalarySql, tc);
                    tc.End();

                    if (IsSalaryProcessed == true)
                    {
                        throw new Exception("Salary is processed for some employee. So execution is not possible !");
                    }

                    sAttIDs += oItem.AttendanceID + ",";

                }
                sAttIDs = sAttIDs.Substring(0, sAttIDs.Length - 1);
                string sSql = "UPDATE AttendanceDaily SET IsNoWork=0 WHERE AttendanceID IN(" + sAttIDs + "); SELECT * FROM View_AttendanceDaily WHERE AttendanceID IN(" + sAttIDs + ")";

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AttendanceDailyDA.Gets(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                oAttendanceDailys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttendanceDailys[0].ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oAttendanceDailys;
        }

        #endregion No Work

        public AttendanceDaily ManualAttendance_Update(AttendanceDaily oAttendanceDaily, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                DateTime InTimeRandomStart_Comp = DateTime.Now;
                DateTime InTimeRandomEnd_Comp = DateTime.Now;
                DateTime OutTimeRandomStart_Comp = DateTime.Now;
                DateTime OutTimeRandomEnd_Comp = DateTime.Now;

                bool isinTimeRandom_Comp = Convert.ToBoolean(oAttendanceDaily.sRandom.Split('~')[0]);
                if (isinTimeRandom_Comp)
                {
                    InTimeRandomStart_Comp = Convert.ToDateTime(oAttendanceDaily.sRandom.Split('~')[1]);
                    InTimeRandomEnd_Comp = Convert.ToDateTime(oAttendanceDaily.sRandom.Split('~')[2]);
                }
                bool isoutTimeRandom_Comp = Convert.ToBoolean(oAttendanceDaily.sRandom.Split('~')[3]);
                if (isoutTimeRandom_Comp)
                {
                    OutTimeRandomStart_Comp = Convert.ToDateTime(oAttendanceDaily.sRandom.Split('~')[4]);
                    OutTimeRandomEnd_Comp = Convert.ToDateTime(oAttendanceDaily.sRandom.Split('~')[5]);
                }

                DateTime InTimeRandomStart = DateTime.Now;
                DateTime InTimeRandomEnd = DateTime.Now;
                DateTime OutTimeRandomStart = DateTime.Now;
                DateTime OutTimeRandomEnd = DateTime.Now;

                bool isinTimeRandom = Convert.ToBoolean(oAttendanceDaily.sRandom.Split('~')[6]);
                if (isinTimeRandom)
                {
                    InTimeRandomStart = Convert.ToDateTime(oAttendanceDaily.sRandom.Split('~')[7]);
                    InTimeRandomEnd = Convert.ToDateTime(oAttendanceDaily.sRandom.Split('~')[8]);
                }
                bool isoutTimeRandom = Convert.ToBoolean(oAttendanceDaily.sRandom.Split('~')[9]);
                if (isoutTimeRandom)
                {
                    OutTimeRandomStart = Convert.ToDateTime(oAttendanceDaily.sRandom.Split('~')[10]);
                    OutTimeRandomEnd = Convert.ToDateTime(oAttendanceDaily.sRandom.Split('~')[11]);
                }

                Random rand = new Random();

                if (isinTimeRandom_Comp)
                {
                    TimeSpan ts = InTimeRandomStart_Comp - InTimeRandomEnd_Comp;
                    int nRandomMinute_Comp = rand.Next(Math.Abs(ts.Minutes));
                    oAttendanceDaily.CompInTime = InTimeRandomStart_Comp.AddMinutes(nRandomMinute_Comp);
                }
                if (isoutTimeRandom_Comp)
                {
                    TimeSpan ts = OutTimeRandomStart_Comp - OutTimeRandomEnd_Comp;
                    int nRandomMinute_Comp = rand.Next(Math.Abs(ts.Minutes));
                    oAttendanceDaily.CompOutTime = OutTimeRandomStart.AddMinutes(nRandomMinute_Comp);
                }

                if (isinTimeRandom)
                {
                    TimeSpan ts = InTimeRandomStart - InTimeRandomEnd;
                    int nRandomMinute = rand.Next(Math.Abs(ts.Minutes));
                    oAttendanceDaily.InTime = InTimeRandomStart.AddMinutes(nRandomMinute);
                }
                if (isoutTimeRandom)
                {
                    TimeSpan ts = OutTimeRandomStart - OutTimeRandomEnd;
                    int nRandomMinute = rand.Next(Math.Abs(ts.Minutes));
                    oAttendanceDaily.OutTime = OutTimeRandomStart.AddMinutes(nRandomMinute);
                }

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AttendanceDailyDA.ManualAttendance_Update(tc, oAttendanceDaily, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceDaily = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttendanceDaily.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oAttendanceDaily;
        }

        #region UploadXL
        public List<AttendanceDaily> UploadAttendanceXL(List<AttendanceDaily> oAttendanceDailys, Int64 nUserID)
        {
            List<AttendanceDaily> oTempAttDs = new List<AttendanceDaily>();
            List<AttendanceDaily> oAttDs = new List<AttendanceDaily>();
            TransactionContext tc = null;
            try
            {
                bool IsAdd = true;
                foreach (AttendanceDaily oItem in oAttendanceDailys)
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;

                    oTempAttDs = new List<AttendanceDaily>();
                    reader = AttendanceDailyDA.UploadAttendanceXL(tc, oItem, nUserID);
                    if (IsAdd)
                    {
                        NullHandler oReader = new NullHandler(reader);
                        oTempAttDs = CreateObjects(reader);
                        oAttDs.AddRange(oTempAttDs);
                    }
                    reader.Close();
                    IsAdd = false;
                    tc.End();
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                AttendanceDaily oAttD = new AttendanceDaily();
                oAttDs = new List<AttendanceDaily>();
                oAttDs.Add(oAttD);
                oAttDs[0].ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oAttDs;
        }
        #endregion UploadXl

        public List<AttendanceDaily> GetsDayWiseAbsent(int nDays, DateTime dDate, Int64 nUserID)
        {
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendanceDailyDA.GetsDayWiseAbsent(nDays, dDate, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    AttendanceDaily oItem = new AttendanceDaily();
                    oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                    oItem.EmployeeCode = oreader.GetString("Code");
                    oItem.EmployeeName = oreader.GetString("Name");
                    oItem.DesignationName = oreader.GetString("DesignationName");
                    oItem.DepartmentName = oreader.GetString("DepartmentName");
                    oItem.JoiningDate = oreader.GetDateTime("JoiningDate");
                    oItem.LastAttendanceDate = oreader.GetDateTime("LastAttendanceDate");

                    oAttendanceDailys.Add(oItem);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oAttendanceDailys;
        }

        public List<AttendanceDaily> EmployeeWiseReprocess(int EmployeeID, DateTime Startdate, DateTime EndDate, Int64 nUserID)
        {
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceProcessManagementDA.EmployeeWiseReprocess(EmployeeID, Startdate, EndDate, tc, nUserID);
                oAttendanceDailys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttendanceDaily = new AttendanceDaily();
                oAttendanceDaily.ErrorMessage = e.Message.Split('!')[0];
                oAttendanceDailys.Add(oAttendanceDaily);
                #endregion
            }
            return oAttendanceDailys;
        }
        
        #region AttendanceDaily_Manual_Single
        public AttendanceDaily AttendanceDaily_Manual_Single(AttendanceDaily oAttendanceDaily, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AttendanceDailyDA.AttendanceDaily_Manual_Single(tc, oAttendanceDaily, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceDaily = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oAttendanceDaily;
        }
        public AttendanceDaily Update_AttendanceDaily_Manual_Single(DateTime dtStartDate, DateTime dtEndDate, int nEmployeeID, int nBufferTime, bool bIsOverTime, Int64 nUserID)
        {
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();
            List<AttendanceDaily> oTempRPEs = new List<AttendanceDaily>();
            List<AttendanceDaily> oRPEs = new List<AttendanceDaily>();
            List<AttendanceDaily> oTempList = new List<AttendanceDaily>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);

                oTempRPEs = new List<AttendanceDaily>();
                IDataReader reader = null;
                reader = AttendanceDailyDA.Update_AttendanceDaily_Manual_Single(tc, dtStartDate, dtEndDate, nEmployeeID, nBufferTime, bIsOverTime, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceDaily = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                oAttendanceDaily = new AttendanceDaily();
                oAttendanceDaily.ErrorMessage = e.Message;
            }
            return oAttendanceDaily;
            
        }

        public List<AttendanceDaily> Update_AttendanceDaily_Manual_All(AttendanceDaily oAttendanceDaily, Int64 nUserID)
        {

            List<AttendanceDaily> oTempRPEs = new List<AttendanceDaily>();
            List<AttendanceDaily> oRPEs = new List<AttendanceDaily>();
            List<AttendanceDaily> oTempList = new List<AttendanceDaily>();
            TransactionContext tc = null;

            DateTime dtStartDate = Convert.ToDateTime(oAttendanceDaily.Remark.Split('~')[0]);
            DateTime dtEndDate = Convert.ToDateTime(oAttendanceDaily.Remark.Split('~')[1]);
            string sEmployeeIDs = oAttendanceDaily.Remark.Split('~')[2];
            int nBufferTime = Convert.ToInt32(oAttendanceDaily.Remark.Split('~')[3]);
            bool bIsOverTime = Convert.ToBoolean(oAttendanceDaily.Remark.Split('~')[4]);

            string[] oEmployees = sEmployeeIDs.Split(',');
            int nEmployeeID = 0;
            foreach (string oItem in oEmployees)
            {
                try
                {
                    tc = TransactionContext.Begin(true);
                    nEmployeeID = Convert.ToInt32(oItem);
                    oTempRPEs = new List<AttendanceDaily>();
                    IDataReader reader = null;
                    reader = AttendanceDailyDA.Update_AttendanceDaily_Manual_All(tc, dtStartDate, dtEndDate, nEmployeeID, nBufferTime, bIsOverTime, nUserID);
                    if (oRPEs.Count <= 0)
                    {
                        oTempRPEs = CreateObjects(reader);
                        oRPEs.AddRange(oTempRPEs);
                    }
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    if (tc != null)
                        tc.HandleError();
                    AttendanceDaily oAtt = new AttendanceDaily();
                    oAtt.ErrorMessage = e.Message.Contains("!") ? e.Message.Split('!')[0] : e.Message;
                    oTempList.Add(oAtt);
                }
            }
            return oTempList;
        }


        public AttendanceDaily AttendanceDaily_Manual_Single_Comp(AttendanceDaily oAttendanceDaily, Int64 nUserID)
        {
            AttendanceDaily oItem = new AttendanceDaily();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AttendanceDailyDA.AttendanceDaily_Manual_Single_Comp(tc, oAttendanceDaily, nUserID);
                NullHandler oReader = new NullHandler(reader);
                //if (reader.Read())
                //{
                //    oAttendanceDaily = CreateObject(oReader);
                //}
                if (reader.Read())
                {
                    oItem.AttendanceID = oReader.GetInt32("AttendanceID");
                    oItem.EmployeeID = oReader.GetInt32("EmployeeID");
                    oItem.EmployeeCode = oReader.GetString("EmployeeCode");
                    oItem.EmployeeName = oReader.GetString("EmployeeName");
                    oItem.AttendanceDate = oReader.GetDateTime("AttendanceDate");
                    oItem.InTime = oReader.GetDateTime("CompInTime");
                    oItem.OutTime = oReader.GetDateTime("CompOutTime");
                    oItem.TotalWorkingHourInMinute = oReader.GetInt32("CompTotalWorkingHourInMinute");
                    oItem.LocationName = oReader.GetString("LocationName");
                    oItem.DepartmentName = oReader.GetString("DepartmentName");
                    oItem.DesignationName = oReader.GetString("DesignationName");
                    oItem.LateArrivalMinute = oReader.GetInt32("CompLateArrivalMinute");
                    oItem.EarlyDepartureMinute = oReader.GetInt32("CompEarlyDepartureMinute");
                    oItem.IsDayOff = oReader.GetBoolean("IsCompDayOff");
                    oItem.IsOSD = oReader.GetBoolean("IsOSD");
                    oItem.IsLeave = oReader.GetBoolean("IsCompLeave");
                    oItem.IsUnPaid = oReader.GetBoolean("IsUnPaid");
                    oItem.IsNoWork = oReader.GetBoolean("IsNoWork");
                    oItem.OverTimeInMinute = oReader.GetInt32("CompOverTimeInMinute");
                    oItem.LeaveType = (EnumLeaveType)oReader.GetInt16("LeaveType");
                    oItem.IsHoliday = oReader.GetBoolean("IsCompHoliday");
                    oItem.ShiftID = oReader.GetInt32("ShiftID");
                    oItem.LeaveHeadID = oReader.GetInt32("CompLeaveHeadID");
                    oItem.IsManualOT = oReader.GetBoolean("IsManualOT");
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oItem;
        }

        public AttendanceDaily AttendanceDaily_Manual_Single_Comp_Conf(AttendanceDaily oAttendanceDaily, Int64 nUserID)
        {
            AttendanceDaily oItem = new AttendanceDaily();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AttendanceDailyDA.AttendanceDaily_Manual_Single_Comp_Conf(tc, oAttendanceDaily, nUserID);
                NullHandler oReader = new NullHandler(reader);
                //if (reader.Read())
                //{
                //    oAttendanceDaily = CreateObject(oReader);
                //}
                if (reader.Read())
                {
                    oItem.MOCID = oReader.GetInt32("MOCID");
                    oItem.MOCAID = oReader.GetInt32("MOCAID");
                    oItem.EmployeeID = oReader.GetInt32("EmployeeID");
                    oItem.EmployeeCode = oReader.GetString("EmployeeCode");
                    oItem.EmployeeName = oReader.GetString("EmployeeName");
                    oItem.AttendanceDate = oReader.GetDateTime("AttendanceDate");
                    oItem.InTime = oReader.GetDateTime("InTime");
                    oItem.OutTime = oReader.GetDateTime("OutTime");
                    oItem.LocationName = oReader.GetString("LocationName");
                    oItem.DepartmentName = oReader.GetString("DepartmentName");
                    oItem.DesignationName = oReader.GetString("DesignationName");
                    oItem.IsDayOff = oReader.GetBoolean("IsDayOff");
                    oItem.IsLeave = oReader.GetBoolean("IsLeave");
                    oItem.OverTimeInMinute = oReader.GetInt32("OverTimeInMin");
                    oItem.IsHoliday = oReader.GetBoolean("IsHoliday");
                    oItem.LeaveHeadID = oReader.GetInt32("LeaveHeadID");
                    oItem.ShiftID = oReader.GetInt32("ShiftID");
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oItem;
        }


        public AttendanceDaily AttendanceDaily_Manual_Single_Comp_ForDailyAttendance(AttendanceDaily oAttendanceDaily, Int64 nUserID)
        {
            AttendanceDaily oItem = new AttendanceDaily();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AttendanceDailyDA.AttendanceDaily_Manual_Single_Comp_ForDailyAttendance(tc, oAttendanceDaily, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceDaily = CreateObject(oReader);
                }
                
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oAttendanceDaily;
        }

        

        #endregion AttendanceDaily_Manual_Single

        public List<AttendanceDaily> LeaveReportGets(string sSQL, Int64 nUserID)
        {
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendanceDailyDA.Gets(sSQL, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    AttendanceDaily oItem = new AttendanceDaily();
                    oItem.EmployeeCode = oreader.GetString("Code");
                    oItem.EmployeeName = oreader.GetString("EmployeeName");
                    oItem.DepartmentName = oreader.GetString("Department");
                    oItem.DesignationName = oreader.GetString("Designation");
                    oItem.AttendanceDate = oreader.GetDateTime("AttendanceDate");
                    oItem.HRM_ShiftName = oreader.GetString("HRM_Shift");
                    oItem.LeaveStatus = oreader.GetString("LeaveStatus");
                    oItem.WorkingStatus = (EnumEmployeeWorkigStatus)oreader.GetInt16("WorkingStatus");
                    oAttendanceDailys.Add(oItem);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oAttendanceDailys;
        }

        public List<AttendanceDaily> Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, Int64 nUserID)
        {
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceDailyDA.Gets(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds, sRemarks, nUserID, tc);
                oAttendanceDailys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceDaily", e);
                #endregion
            }
            return oAttendanceDailys;
        }

        public AttendanceDaily GetTotalCount(string ssql, long nUserId)
        {
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();
            int nTotal = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                nTotal = AttendanceDailyDA.GetTotalCount(tc, ssql, nUserId);
                oAttendanceDaily.TotalAttendanceCount = nTotal;
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttendanceDaily = new AttendanceDaily();
                oAttendanceDaily.ErrorMessage = e.Message;
                #endregion
            }
            return oAttendanceDaily;
        }


        public List<AttendanceDaily> UploadAttXL(List<AttendanceDaily> oAttendanceDailys, bool IsNUInTime, bool IsNUOutTime, bool IsNULate, bool IsNUEarly, bool IsNUInDate, bool IsNUOutDate, bool IsNUOT, bool IsNURemark, bool IsComp, long nUserID)
        {
            List<AttendanceDaily> oTempRPEs = new List<AttendanceDaily>();
            List<AttendanceDaily> oRPEs = new List<AttendanceDaily>();
            List<AttendanceDaily> oTempList = new List<AttendanceDaily>();
            TransactionContext tc = null;
            foreach (AttendanceDaily oItem in oAttendanceDailys)
            {
                try
                {
                    tc = TransactionContext.Begin(true);

                    oTempRPEs = new List<AttendanceDaily>();
                    IDataReader reader = null;
                    reader = AttendanceDailyDA.UploadAttXL(tc, oItem, IsNUInTime, IsNUOutTime, IsNULate, IsNUEarly, IsNUInDate, IsNUOutDate, IsNUOT, IsNURemark, IsComp, nUserID);
                    if (oRPEs.Count <= 0)
                    {
                        oTempRPEs = CreateObjects(reader);
                        oRPEs.AddRange(oTempRPEs);
                    }
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    if (tc != null)
                        tc.HandleError();

                    oItem.ErrorMessage = e.Message.Contains("!") ? e.Message.Split('!')[0] : e.Message;
                    oTempList.Add(oItem);
                }
            }
            return oTempList;
        }

        public List<AttendanceDaily> DayoffListExcel(string sAttendanceDate, bool bIsDayoffThisDay, string BUIDs, string LocIDs, string DepartmentIDs, string DesignationIDs, bool IsComp, int nType, long nUserID)
        {
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceDailyDA.DayoffListExcel(tc, sAttendanceDate, bIsDayoffThisDay, BUIDs, LocIDs, DepartmentIDs, DesignationIDs, IsComp, nType, nUserID);
                oAttendanceDailys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttendanceDaily = new AttendanceDaily();
                oAttendanceDaily.ErrorMessage = e.Message.Split('!')[0];
                oAttendanceDailys.Add(oAttendanceDaily);
                #endregion
            }
            return oAttendanceDailys;
        }
        public List<AttendanceDaily> MakeLeave(string sAttendanceDate, bool bIsDayoffThisDay, string BUIDs, string LocIDs, string DepartmentIDs, string DesignationIDs, bool IsComp, int nType, int nLeaveHeadID, DateTime sStartDate, DateTime sEndDate, long nUserID)
        {
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceDailyDA.MakeLeave(tc, sAttendanceDate, bIsDayoffThisDay, BUIDs, LocIDs, DepartmentIDs, DesignationIDs, IsComp, nType, nLeaveHeadID, sStartDate, sEndDate, nUserID);
                oAttendanceDailys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttendanceDaily = new AttendanceDaily();
                oAttendanceDaily.ErrorMessage = e.Message.Split('!')[0];
                oAttendanceDailys.Add(oAttendanceDaily);
                #endregion
            }
            return oAttendanceDailys;
        }
        public List<AttendanceDaily> MakeAbsent(string sAttendanceDate, bool bOperation, bool bIsLeaveBefore, bool bIsLeaveAfter, bool bIsAbsentBefore, bool bIsAbsentAfter, bool bIsHolidayBefore, bool bIsHolidayAfter, bool bIsDayOffBefore, bool bIsDayOffAfter, string BUIDs, string LocIDs, string DepartmentIDs, string DesignationIDs, bool IsComp, long nUserID)
        {
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceDailyDA.MakeAbsent(tc, sAttendanceDate, bOperation, bIsLeaveBefore, bIsLeaveAfter, bIsAbsentBefore, bIsAbsentAfter, bIsHolidayBefore, bIsHolidayAfter, bIsDayOffBefore, bIsDayOffAfter, BUIDs, LocIDs, DepartmentIDs, DesignationIDs, IsComp, nUserID);
                oAttendanceDailys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttendanceDaily = new AttendanceDaily();
                oAttendanceDaily.ErrorMessage = e.Message.Split('!')[0];
                oAttendanceDailys.Add(oAttendanceDaily);
                #endregion
            }
            return oAttendanceDailys;
        }

        #endregion
    }
}
