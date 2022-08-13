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
    public class AttendanceDaily_ZNService : MarshalByRefObject, IAttendanceDaily_ZNService
    {
        #region Private functions and declaration
        private AttendanceDaily_ZN MapObject(NullHandler oReader)
        {
            AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();
            oAttendanceDaily_ZN.AttendanceID = oReader.GetInt32("AttendanceID");
            oAttendanceDaily_ZN.GroupByID = oReader.GetInt32("GroupByID");
            oAttendanceDaily_ZN.EmployeeID = oReader.GetInt32("EmployeeID");
            oAttendanceDaily_ZN.AttendanceSchemeID = oReader.GetInt32("AttendanceSchemeID");
            oAttendanceDaily_ZN.LocationID = oReader.GetInt32("LocationID");
            oAttendanceDaily_ZN.DepartmentID = oReader.GetInt32("DepartmentID");
            oAttendanceDaily_ZN.DesignationID = oReader.GetInt32("DesignationID");
            oAttendanceDaily_ZN.RosterPlanID = oReader.GetInt32("RosterPlanID");
            oAttendanceDaily_ZN.ShiftID = oReader.GetInt32("ShiftID");
            //oAttendanceDaily_ZN.EmployeeTypeID = oReader.GetInt32("EmployeeTypeID");
            oAttendanceDaily_ZN.AttendanceDate = oReader.GetDateTime("AttendanceDate");
            oAttendanceDaily_ZN.InTime = oReader.GetDateTime("InTime");
            oAttendanceDaily_ZN.OutTime = oReader.GetDateTime("OutTime");
            oAttendanceDaily_ZN.CompInTime = oReader.GetDateTime("CompInTime");
            oAttendanceDaily_ZN.CompOutTime = oReader.GetDateTime("CompOutTime");
            oAttendanceDaily_ZN.LateArrivalMinute = oReader.GetInt32("LateArrivalMinute");
            oAttendanceDaily_ZN.EarlyDepartureMinute = oReader.GetInt32("EarlyDepartureMinute");
            oAttendanceDaily_ZN.TotalWorkingHourInMinute = oReader.GetInt32("TotalWorkingHourInMinute");
            oAttendanceDaily_ZN.OverTimeInMinute = oReader.GetInt32("OverTimeInMinute");

            oAttendanceDaily_ZN.CompLateArrivalMinute = oReader.GetInt32("CompLateArrivalMinute");
            oAttendanceDaily_ZN.CompEarlyDepartureMinute = oReader.GetInt32("CompEarlyDepartureMinute");
            oAttendanceDaily_ZN.CompTotalWorkingHourInMinute = oReader.GetInt32("CompTotalWorkingHourInMinute");
            oAttendanceDaily_ZN.CompOverTimeInMinute = oReader.GetInt32("CompOverTimeInMinute");

            oAttendanceDaily_ZN.IsDayOff = oReader.GetBoolean("IsDayOff");
            oAttendanceDaily_ZN.IsLeave = oReader.GetBoolean("IsLeave");
            oAttendanceDaily_ZN.IsUnPaid = oReader.GetBoolean("IsUnPaid");

            oAttendanceDaily_ZN.IsHoliday = oReader.GetBoolean("IsHoliday");
            oAttendanceDaily_ZN.IsCompDayOff = oReader.GetBoolean("IsCompDayOff");
            oAttendanceDaily_ZN.IsCompLeave = oReader.GetBoolean("IsCompLeave");
            oAttendanceDaily_ZN.IsCompHoliday = oReader.GetBoolean("IsCompHoliday");
            oAttendanceDaily_ZN.IsPromoted = oReader.GetBoolean("IsPromoted");

            oAttendanceDaily_ZN.WorkingStatus = (EnumEmployeeWorkigStatus)oReader.GetInt16("WorkingStatus");
            oAttendanceDaily_ZN.WorkingStatusInt = (int)(EnumEmployeeWorkigStatus)oReader.GetInt16("WorkingStatus");
            oAttendanceDaily_ZN.Note = oReader.GetString("Note");
            oAttendanceDaily_ZN.APMID = oReader.GetInt32("APMID");
            oAttendanceDaily_ZN.IsLock = oReader.GetBoolean("IsLock");
            oAttendanceDaily_ZN.IsNoWork = oReader.GetBoolean("IsNoWork");
            oAttendanceDaily_ZN.IsManual = oReader.GetBoolean("IsManual");
            oAttendanceDaily_ZN.LastUpdatedBY = oReader.GetInt32("LastUpdatedBY");
            oAttendanceDaily_ZN.LastUpdatedDate = oReader.GetDateTime("LastUpdatedDate");
            //derive
            oAttendanceDaily_ZN.EmployeeName = oReader.GetString("EmployeeName");
            oAttendanceDaily_ZN.EmployeeCode = oReader.GetString("Code");
            oAttendanceDaily_ZN.AttendanceSchemeName = oReader.GetString("AttendanceScheme");
            oAttendanceDaily_ZN.DepartmentName = oReader.GetString("Department");
            oAttendanceDaily_ZN.DesignationName = oReader.GetString("Designation");
            oAttendanceDaily_ZN.HRM_ShiftName = oReader.GetString("HRM_Shift");
            //oAttendanceDaily_ZN.EmployeeTypeName = oReader.GetString("EmployeeType");
            oAttendanceDaily_ZN.RosterPlanName = oReader.GetString("RosterPlanName");
            oAttendanceDaily_ZN.LocationName = oReader.GetString("LocationName");
            oAttendanceDaily_ZN.IsProductionBase = oReader.GetBoolean("IsProductionBase");
            oAttendanceDaily_ZN.DateOfBirth = oReader.GetDateTime("DateOfBirth");
            oAttendanceDaily_ZN.JoiningDate = oReader.GetDateTime("JoiningDate");
            oAttendanceDaily_ZN.LeaveHeadID = oReader.GetInt32("LeaveHeadID");
            oAttendanceDaily_ZN.IsOSD = oReader.GetBoolean("IsOSD");
            oAttendanceDaily_ZN.BUName = oReader.GetString("BUName");
            
            return oAttendanceDaily_ZN;

        }

        private AttendanceDaily_ZN MapObjectForReport(NullHandler oReader)
        {
            AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();
            oAttendanceDaily_ZN.EmployeeName = oReader.GetString("EmployeeName");
            oAttendanceDaily_ZN.EmployeeCode = oReader.GetString("Code");
            oAttendanceDaily_ZN.DesignationName = oReader.GetString("Designation");
            oAttendanceDaily_ZN.EmployeeTypeName = oReader.GetString("EmployeeType");

            oAttendanceDaily_ZN.TotalWorkingDay = oReader.GetInt32("TotalWorkingDay");
            oAttendanceDaily_ZN.TotalShift = oReader.GetInt32("TotalShift");
            oAttendanceDaily_ZN.TotalWorkingHour = oReader.GetDouble("TotalWorkingHour");
            oAttendanceDaily_ZN.PresentShift = oReader.GetInt32("PresentShift");
            oAttendanceDaily_ZN.AbsentShift = oReader.GetInt32("AbsentShift");
            oAttendanceDaily_ZN.IsDayOFFs = oReader.GetInt32("IsDayOFF");
            oAttendanceDaily_ZN.Leave = oReader.GetInt32("Leave");
            oAttendanceDaily_ZN.Paid = oReader.GetInt32("Paid");
            oAttendanceDaily_ZN.InWorkPlace = oReader.GetInt32("InWorkPlace");
            oAttendanceDaily_ZN.OSD = oReader.GetInt32("OSD");
            oAttendanceDaily_ZN.Suspended = oReader.GetInt32("Suspended");
            oAttendanceDaily_ZN.Holiday = oReader.GetInt32("Holiday");
            oAttendanceDaily_ZN.Late = oReader.GetInt32("Late");
            oAttendanceDaily_ZN.EarlyLeaving = oReader.GetInt32("EarlyLeaving");
            oAttendanceDaily_ZN.OvertimeInhour = oReader.GetDouble("OvertimeInhour");
            return oAttendanceDaily_ZN;
        }

        private AttendanceDaily_ZN CreateObject(NullHandler oReader)
        {
            AttendanceDaily_ZN oAttendanceDaily_ZN = MapObject(oReader);
            return oAttendanceDaily_ZN;
        }

        private List<AttendanceDaily_ZN> CreateObjects(IDataReader oReader)
        {
            List<AttendanceDaily_ZN> oAttendanceDaily_ZN = new List<AttendanceDaily_ZN>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceDaily_ZN oItem = CreateObject(oHandler);
                oAttendanceDaily_ZN.Add(oItem);
            }
            return oAttendanceDaily_ZN;
        }

        private AttendanceDaily_ZN CreateObjectForReport(NullHandler oReader)
        {
            AttendanceDaily_ZN oAttendanceDaily_ZN = MapObjectForReport(oReader);
            return oAttendanceDaily_ZN;
        }

        private List<AttendanceDaily_ZN> CreateObjectsForReport(IDataReader oReader)
        {
            List<AttendanceDaily_ZN> oAttendanceDaily_ZN = new List<AttendanceDaily_ZN>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceDaily_ZN oItem = CreateObjectForReport(oHandler);
                oAttendanceDaily_ZN.Add(oItem);
            }
            return oAttendanceDaily_ZN;
        }


        #endregion

        #region Interface implementation
        public AttendanceDaily_ZNService() { }

        public AttendanceDaily_ZN IUD(AttendanceDaily_ZN oAttendanceDaily_ZN, int nDBOperation, Int64 nUserID)
        {
            string[] sEmpIDs;
            sEmpIDs = oAttendanceDaily_ZN.ErrorMessage.Split(',');
            oAttendanceDaily_ZN.ErrorMessage = "";
            DateTime InTimeRandomStart = DateTime.Now;
            DateTime InTimeRandomEnd = DateTime.Now;
            DateTime OutTimeRandomStart = DateTime.Now;
            DateTime OutTimeRandomEnd = DateTime.Now;

            bool isinTimeRandom = Convert.ToBoolean(oAttendanceDaily_ZN.sRandom.Split('~')[0]);
            if (isinTimeRandom)
            {
                InTimeRandomStart = Convert.ToDateTime(oAttendanceDaily_ZN.sRandom.Split('~')[1]);
                InTimeRandomEnd = Convert.ToDateTime(oAttendanceDaily_ZN.sRandom.Split('~')[2]);
            }
            bool isoutTimeRandom = Convert.ToBoolean(oAttendanceDaily_ZN.sRandom.Split('~')[3]);
            if (isoutTimeRandom)
            {
                OutTimeRandomStart = Convert.ToDateTime(oAttendanceDaily_ZN.sRandom.Split('~')[4]);
                OutTimeRandomEnd = Convert.ToDateTime(oAttendanceDaily_ZN.sRandom.Split('~')[5]);
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
                        oAttendanceDaily_ZN.InTime = InTimeRandomStart.AddMinutes(nRandomMinute);
                    }
                    if (isoutTimeRandom)
                    {
                        TimeSpan ts = OutTimeRandomStart - OutTimeRandomEnd;
                        int nRandomMinute = rand.Next(Math.Abs(ts.Minutes));
                        oAttendanceDaily_ZN.OutTime = OutTimeRandomStart.AddMinutes(nRandomMinute);
                    }

                    int EmployeeID = Convert.ToInt32(sEmpID);
                    oAttendanceDaily_ZN.EmployeeID = EmployeeID;
                    reader = AttendanceDaily_ZNDA.IUD(tc, oAttendanceDaily_ZN, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {

                        oAttendanceDaily_ZN = CreateObject(oReader);
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
                oAttendanceDaily_ZN.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oAttendanceDaily_ZN;
        }

        public AttendanceDaily_ZN Get(int nEmployeeID, DateTime dAttendanceDate, Int64 nUserId)
        {
            AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AttendanceDaily_ZNDA.Get(tc, nEmployeeID, dAttendanceDate);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceDaily_ZN = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get AttendanceDaily_ZN", e);
                oAttendanceDaily_ZN.ErrorMessage = e.Message;
                #endregion
            }

            return oAttendanceDaily_ZN;
        }

        public int ProcessCompAsPerEdit(string EmployeeID, DateTime Startdate, DateTime EndDate, int MOCID, int nIndex, string sLocationIDs, string sBUIDs, Int64 nUserId)
        {
            int nNewIndex = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nNewIndex = AttendanceProcessManagementDA.ProcessCompAsPerEdit(tc, EmployeeID, Startdate, EndDate, MOCID, nIndex, sLocationIDs, sBUIDs, nUserId);
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
            return nNewIndex;
        }

        public List<AttendanceDaily_ZN> EmployeeWiseReprocessComp(int EmployeeID, DateTime Startdate, DateTime EndDate, Int64 nUserID)
        {
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            List<AttendanceDaily_ZN> oAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceProcessManagementDA.EmployeeWiseReprocess(EmployeeID, Startdate, EndDate, tc, nUserID);
                NullHandler oReader = new NullHandler(reader);
                while (reader.Read())
                {
                    AttendanceDaily_ZN oItem = new AttendanceDaily_ZN();
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
                    oAttendanceDaily_ZNs.Add(oItem);
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
            return oAttendanceDaily_ZNs;
        }
        public AttendanceDaily_ZN Get(string sSQL, Int64 nUserId)
        {
            AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AttendanceDaily_ZNDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceDaily_ZN = CreateObject(oReader);
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
                //oAttendanceDaily_ZN.ErrorMessage = e.Message;
                #endregion
            }

            return oAttendanceDaily_ZN;
        }

        public List<AttendanceDaily_ZN> Gets(string sSQL, Int64 nUserID)
        {
            List<AttendanceDaily_ZN> oAttendanceDaily_ZN = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceDaily_ZNDA.Gets(sSQL, tc);
                oAttendanceDaily_ZN = CreateObjects(reader);
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
            return oAttendanceDaily_ZN;
        }

        public List<AttendanceDaily_ZN> GetsForReport(string sSQL, Int64 nUserID)
        {
            List<AttendanceDaily_ZN> oAttendanceDaily_ZN = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceDaily_ZNDA.Gets(sSQL, tc);
                oAttendanceDaily_ZN = CreateObjectsForReport(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceDaily_ZN", e);
                #endregion
            }
            return oAttendanceDaily_ZN;
        }

        #region No Work
        public List<AttendanceDaily_ZN> NoWorkExecution(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                string sAttIDs = "";
                foreach (AttendanceDaily_ZN oItem in oAttendanceDaily_ZNs)
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
                string sSql = "UPDATE AttendanceDaily_ZN SET IsNoWork=1 WHERE AttendanceID IN(" + sAttIDs + "); SELECT * FROM View_AttendanceDaily_ZN WHERE AttendanceID IN(" + sAttIDs + ")";

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AttendanceDaily_ZNDA.Gets(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                oAttendanceDaily_ZNs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttendanceDaily_ZNs[0].ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oAttendanceDaily_ZNs;
        }

        public List<AttendanceDaily_ZN> CancelNoWorkExecution(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                string sAttIDs = "";
                foreach (AttendanceDaily_ZN oItem in oAttendanceDaily_ZNs)
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
                string sSql = "UPDATE AttendanceDaily_ZN SET IsNoWork=0 WHERE AttendanceID IN(" + sAttIDs + "); SELECT * FROM View_AttendanceDaily_ZN WHERE AttendanceID IN(" + sAttIDs + ")";

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AttendanceDaily_ZNDA.Gets(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                oAttendanceDaily_ZNs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttendanceDaily_ZNs[0].ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oAttendanceDaily_ZNs;
        }

        #endregion No Work

        public AttendanceDaily_ZN ManualAttendance_Update(AttendanceDaily_ZN oAttendanceDaily_ZN, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                DateTime InTimeRandomStart_Comp = DateTime.Now;
                DateTime InTimeRandomEnd_Comp = DateTime.Now;
                DateTime OutTimeRandomStart_Comp = DateTime.Now;
                DateTime OutTimeRandomEnd_Comp = DateTime.Now;

                bool isinTimeRandom_Comp = Convert.ToBoolean(oAttendanceDaily_ZN.sRandom.Split('~')[0]);
                if (isinTimeRandom_Comp)
                {
                    InTimeRandomStart_Comp = Convert.ToDateTime(oAttendanceDaily_ZN.sRandom.Split('~')[1]);
                    InTimeRandomEnd_Comp = Convert.ToDateTime(oAttendanceDaily_ZN.sRandom.Split('~')[2]);
                }
                bool isoutTimeRandom_Comp = Convert.ToBoolean(oAttendanceDaily_ZN.sRandom.Split('~')[3]);
                if (isoutTimeRandom_Comp)
                {
                    OutTimeRandomStart_Comp = Convert.ToDateTime(oAttendanceDaily_ZN.sRandom.Split('~')[4]);
                    OutTimeRandomEnd_Comp = Convert.ToDateTime(oAttendanceDaily_ZN.sRandom.Split('~')[5]);
                }

                DateTime InTimeRandomStart = DateTime.Now;
                DateTime InTimeRandomEnd = DateTime.Now;
                DateTime OutTimeRandomStart = DateTime.Now;
                DateTime OutTimeRandomEnd = DateTime.Now;

                bool isinTimeRandom = Convert.ToBoolean(oAttendanceDaily_ZN.sRandom.Split('~')[6]);
                if (isinTimeRandom)
                {
                    InTimeRandomStart = Convert.ToDateTime(oAttendanceDaily_ZN.sRandom.Split('~')[7]);
                    InTimeRandomEnd = Convert.ToDateTime(oAttendanceDaily_ZN.sRandom.Split('~')[8]);
                }
                bool isoutTimeRandom = Convert.ToBoolean(oAttendanceDaily_ZN.sRandom.Split('~')[9]);
                if (isoutTimeRandom)
                {
                    OutTimeRandomStart = Convert.ToDateTime(oAttendanceDaily_ZN.sRandom.Split('~')[10]);
                    OutTimeRandomEnd = Convert.ToDateTime(oAttendanceDaily_ZN.sRandom.Split('~')[11]);
                }

                Random rand = new Random();

                if (isinTimeRandom_Comp)
                {
                    TimeSpan ts = InTimeRandomStart_Comp - InTimeRandomEnd_Comp;
                    int nRandomMinute_Comp = rand.Next(Math.Abs(ts.Minutes));
                    oAttendanceDaily_ZN.CompInTime = InTimeRandomStart_Comp.AddMinutes(nRandomMinute_Comp);
                }
                if (isoutTimeRandom_Comp)
                {
                    TimeSpan ts = OutTimeRandomStart_Comp - OutTimeRandomEnd_Comp;
                    int nRandomMinute_Comp = rand.Next(Math.Abs(ts.Minutes));
                    oAttendanceDaily_ZN.CompOutTime = OutTimeRandomStart.AddMinutes(nRandomMinute_Comp);
                }

                if (isinTimeRandom)
                {
                    TimeSpan ts = InTimeRandomStart - InTimeRandomEnd;
                    int nRandomMinute = rand.Next(Math.Abs(ts.Minutes));
                    oAttendanceDaily_ZN.InTime = InTimeRandomStart.AddMinutes(nRandomMinute);
                }
                if (isoutTimeRandom)
                {
                    TimeSpan ts = OutTimeRandomStart - OutTimeRandomEnd;
                    int nRandomMinute = rand.Next(Math.Abs(ts.Minutes));
                    oAttendanceDaily_ZN.OutTime = OutTimeRandomStart.AddMinutes(nRandomMinute);
                }

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AttendanceDaily_ZNDA.ManualAttendance_Update(tc, oAttendanceDaily_ZN, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceDaily_ZN = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttendanceDaily_ZN.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oAttendanceDaily_ZN;
        }

        #region UploadXL
        public List<AttendanceDaily_ZN> UploadAttendanceXL(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs, Int64 nUserID)
        {
            List<AttendanceDaily_ZN> oTempAttDs = new List<AttendanceDaily_ZN>();
            List<AttendanceDaily_ZN> oAttDs = new List<AttendanceDaily_ZN>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (AttendanceDaily_ZN oItem in oAttendanceDaily_ZNs)
                {
                    oTempAttDs = new List<AttendanceDaily_ZN>();
                    reader = AttendanceDaily_ZNDA.UploadAttendanceXL(tc, oItem, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    oTempAttDs = CreateObjects(reader);
                    oAttDs.AddRange(oTempAttDs);
                    reader.Close();
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                AttendanceDaily_ZN oAttD = new AttendanceDaily_ZN();
                oAttDs.Add(oAttD);
                oAttDs[0].ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oAttDs;
        }
        #endregion UploadXl

        public List<AttendanceDaily_ZN> GetsDayWiseAbsent(int nDays, DateTime dDate, Int64 nUserID)
        {
            List<AttendanceDaily_ZN> oAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendanceDaily_ZNDA.GetsDayWiseAbsent(nDays, dDate, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    AttendanceDaily_ZN oItem = new AttendanceDaily_ZN();
                    oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                    oItem.EmployeeCode = oreader.GetString("Code");
                    oItem.EmployeeName = oreader.GetString("Name");
                    oItem.DesignationName = oreader.GetString("DesignationName");
                    oItem.DepartmentName = oreader.GetString("DepartmentName");
                    oItem.JoiningDate = oreader.GetDateTime("JoiningDate");
                    oItem.LastAttendanceDate = oreader.GetDateTime("LastAttendanceDate");

                    oAttendanceDaily_ZNs.Add(oItem);
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
            return oAttendanceDaily_ZNs;
        }

        public List<AttendanceDaily_ZN> GetsTimeCard(string sEmployeeIDs, DateTime Startdate, DateTime EndDate, string sLocationID, string sDepartmentIds, string sType, string sBUnitIDs, double nStartSalaryRange, double nEndSalaryRange, string sBlockIDs, string sGroupIDs, Int64 nUserID)
        {
            List<AttendanceDaily_ZN> oAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendanceDaily_ZNDA.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sType, sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBlockIDs, sGroupIDs,nUserID, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    AttendanceDaily_ZN oItem = new AttendanceDaily_ZN();
                    oItem.AttendanceID = oreader.GetInt32("AttendanceID");
                    oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                    oItem.EmployeeCode = oreader.GetString("EmployeeCode");
                    oItem.EmployeeName = oreader.GetString("EmployeeName");
                    oItem.AttendanceDate = oreader.GetDateTime("AttendanceDate");
                    oItem.InTime = oreader.GetDateTime("InTime");
                    oItem.OutTime = oreader.GetDateTime("OutTime");
                    oItem.OT_NowWork_First = oreader.GetInt32("OT_NowWork_First");
                    oItem.OT_NowWork_2nd = oreader.GetInt32("OT_NowWork_2nd");
                    oItem.OT_NowWork_Rest = oreader.GetInt32("OT_NowWork_Rest");
                    oItem.TotalWorkingHourInMinute = oreader.GetInt32("TotalWorkingHourInMinute");
                    oItem.LocationName = oreader.GetString("LocationName");
                    oItem.DepartmentName = oreader.GetString("DepartmentName");
                    oItem.DesignationName = oreader.GetString("DesignationName");
                    oItem.LateArrivalMinute = oreader.GetInt32("LateArrivalMinute");
                    oItem.EarlyDepartureMinute = oreader.GetInt32("EarlyDepartureMinute");
                    oItem.IsDayOff = oreader.GetBoolean("IsDayOff");
                    oItem.IsOSD = oreader.GetBoolean("IsOSD");
                    oItem.IsLeave = oreader.GetBoolean("IsLeave");
                    oItem.IsUnPaid = oreader.GetBoolean("IsUnPaid");
                    oItem.IsNoWork = oreader.GetBoolean("IsNoWork");
                    oItem.OverTimeInMinute = oreader.GetInt32("OverTimeInMinute");

                    oItem.LateArrivalMinute = oreader.GetInt32("LateArrivalMinute");
                    oItem.EarlyDepartureMinute = oreader.GetInt32("EarlyDepartureMinute");

                    oItem.LeaveName = oreader.GetString("LeaveName");
                    oItem.LeaveType = (EnumLeaveType)oreader.GetInt16("LeaveType");
                    oItem.IsHoliday = oreader.GetBoolean("IsHoliday");
                    oItem.ShiftID = oreader.GetInt32("ShiftID");
                    oItem.ShiftName = oreader.GetString("ShiftName");
                    oItem.ShiftStartTime = oreader.GetDateTime("ShiftStartTime");
                    oItem.ShiftEndTime = oreader.GetDateTime("ShiftEndTime");
                    oItem.OT_NHR = oreader.GetDouble("OT_NHR");
                    oItem.OT_HHR = oreader.GetDouble("OT_HHR");
                    oItem.LeaveDuration = oreader.GetInt32("LeaveDuration");
                    oItem.JoiningDate = oreader.GetDateTime("JoiningDate");
                    oItem.BUName = oreader.GetString("BUName");
                    oItem.BUAddress = oreader.GetString("BUAddress");
                    oItem.Remark = oreader.GetString("Remark");
                    oItem.IsManualOT = oreader.GetBoolean("IsManualOT");
                    oItem.IsPromoted = oreader.GetBoolean("IsPromoted");
                    oAttendanceDaily_ZNs.Add(oItem);
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
            return oAttendanceDaily_ZNs;
        }
        public List<AttendanceDaily_ZN> GetsTimeCardMaxOTConfSearch(string sEmployeeIDs, DateTime Startdate, DateTime EndDate, string sLocationID, string sDepartmentIds, string sBUnitIDs, double nStartSalaryRange, double nEndSalaryRange, string sBlockIDs, string sGroupIDs, int nMOCID, Int64 nUserID)
        {
            List<AttendanceDaily_ZN> oAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendanceDaily_ZNDA.GetsTimeCardMaxOTConfSearch(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBlockIDs, sGroupIDs, nMOCID, nUserID, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    AttendanceDaily_ZN oItem = new AttendanceDaily_ZN();
                    oItem.AttendanceID = oreader.GetInt32("AttendanceID");
                    oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                    oItem.AttendanceDate = oreader.GetDateTime("AttendanceDate");

                    oItem.InTime = oreader.GetDateTime("InTime");
                    oItem.OutTime = oreader.GetDateTime("OutTime");
                    oItem.TotalWorkingHourInMinute = oreader.GetInt32("TotalWorkingHourInMinute");


                    oItem.OverTimeInMinute = oreader.GetInt32("OverTimeInMinute");
                    oItem.ShiftID = oreader.GetInt32("ShiftID");
                    oItem.ShiftName = oreader.GetString("ShiftName");


                    oItem.LeaveHeadID = oreader.GetInt32("LeaveHeadID");
                    oItem.LeaveName = oreader.GetString("LeaveName");
                    oItem.IsLeave = oreader.GetBoolean("IsLeave");


                    oItem.IsDayOff = oreader.GetBoolean("IsDayOff");
                    oItem.IsOSD = oreader.GetBoolean("IsOSD");
                    oItem.IsHoliday = oreader.GetBoolean("IsHoliday");
                    oItem.IsLeave = oreader.GetBoolean("IsLeave");

                    oItem.EmployeeCode = oreader.GetString("EmployeeCode");
                    oItem.EmployeeName = oreader.GetString("EmployeeName");
                    oItem.JoiningDate = oreader.GetDateTime("JoiningDate");

                    oItem.DepartmentName = oreader.GetString("DepartmentName");
                    oItem.DesignationName = oreader.GetString("DesignationName");
                    oItem.LocationName = oreader.GetString("LocationName");
                    oItem.BUName = oreader.GetString("BUName");
                    oItem.IsManual = oreader.GetBoolean("IsManual");


                    //oItem.OT_NowWork_First = oreader.GetInt32("OT_NowWork_First");
                    //oItem.OT_NowWork_2nd = oreader.GetInt32("OT_NowWork_2nd");
                    //oItem.OT_NowWork_Rest = oreader.GetInt32("OT_NowWork_Rest");
                    //oItem.DesignationName = oreader.GetString("DesignationName");
                    //oItem.LateArrivalMinute = oreader.GetInt32("LateArrivalMinute");
                    //oItem.EarlyDepartureMinute = oreader.GetInt32("EarlyDepartureMinute");
                    //oItem.IsLeave = oreader.GetBoolean("IsLeave");
                    //oItem.IsUnPaid = oreader.GetBoolean("IsUnPaid");
                    //oItem.IsNoWork = oreader.GetBoolean("IsNoWork");
                    //oItem.LeaveName = oreader.GetString("LeaveName");
                    //oItem.LeaveType = (EnumLeaveType)oreader.GetInt16("LeaveType");
                    //oItem.ShiftName = oreader.GetString("ShiftName");
                    //oItem.OT_NHR = oreader.GetDouble("OT_NHR");
                    //oItem.OT_HHR = oreader.GetDouble("OT_HHR");
                    //oItem.LeaveDuration = oreader.GetInt32("LeaveDuration");
                    //oItem.BUAddress = oreader.GetString("BUAddress");
                    //oItem.Remark = oreader.GetString("Remark");
                    //oItem.IsManualOT = oreader.GetBoolean("IsManualOT");
                    //oItem.IsPromoted = oreader.GetBoolean("IsPromoted");
                    oAttendanceDaily_ZNs.Add(oItem);
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
            return oAttendanceDaily_ZNs;
        }

        public List<AttendanceDaily_ZN> GetsTimeCardMaxOTConf(string sEmployeeIDs, DateTime Startdate, DateTime EndDate, string sLocationID, string sDepartmentIds, string sBUnitIDs, double nStartSalaryRange, double nEndSalaryRange, string sBlockIDs, string sGroupIDs, int nMOCID, Int64 nUserID)
        {
            List<AttendanceDaily_ZN> oAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            TransactionContext tc = null;
            try
            {

                #region SQL 
                string sSQL = "SELECT * FROM View_MaxOTConfigurationAttendance AS MOCA WHERE MOCA.MOCID = " + nMOCID.ToString() + " AND MOCA.AttendanceDate BETWEEN '" + Startdate.ToString("dd MMM yyyy") + "' AND '" + EndDate.ToString("dd MMM yyyy") + "'";
                if (sEmployeeIDs != "")
                {
                    sSQL = sSQL + " AND MOCA.EmployeeID IN (SELECT Emp.items FROM dbo.SplitInToDataSet('" + sEmployeeIDs + "', ',') AS Emp)";
                }
                if (sLocationID != "")
                {
                    sSQL = sSQL + " AND MOCA.LocationID IN (SELECT LOC.items FROM dbo.SplitInToDataSet('"+sLocationID+"', ',') AS LOC)";
                }
                if (sDepartmentIds != "")
                {
                    sSQL = sSQL + " AND MOCA.DepartmentID IN (SELECT DEPT.items FROM dbo.SplitInToDataSet('"+sDepartmentIds+"', ',') AS DEPT)";
                }
                if (sBUnitIDs != "")
                {
                    sSQL = sSQL + " AND MOCA.BusinessUnitID IN (SELECT BU.items FROM dbo.SplitInToDataSet('"+sBUnitIDs+"', ',') AS BU)";
                }
                if (nStartSalaryRange != 0 && nEndSalaryRange != 0)
                {
                    sSQL = sSQL + " AND MOCA.EmployeeID IN (SELECT ESS.EmployeeID FROM EmployeeSalaryStructure AS ESS WHERE ESS.GrossAmount BETWEEN " + nStartSalaryRange.ToString() + " AND " + nEndSalaryRange.ToString() + ")";
                }
                #endregion




                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendanceDaily_ZNDA.Gets(sSQL, tc);
                //reader = AttendanceDaily_ZNDA.GetsTimeCardMaxOTConf(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBlockIDs, sGroupIDs, nMOCID, nUserID, tc);

                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    AttendanceDaily_ZN oItem = new AttendanceDaily_ZN();
                    oItem.AttendanceID = oreader.GetInt32("AttendanceID");
                    oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                    oItem.AttendanceDate = oreader.GetDateTime("AttendanceDate");

                    oItem.InTime = oreader.GetDateTime("InTime");
                    oItem.OutTime = oreader.GetDateTime("OutTime");
                    oItem.TotalWorkingHourInMinute = oreader.GetInt32("TotalWorkingHourInMinute");


                    oItem.OverTimeInMinute = oreader.GetInt32("OverTimeInMin");
                    oItem.ShiftID = oreader.GetInt32("ShiftID");
                    oItem.ShiftName = oreader.GetString("ShiftName");


                    oItem.LeaveHeadID = oreader.GetInt32("LeaveHeadID");
                    oItem.LeaveName = oreader.GetString("LeaveName");
                    oItem.IsLeave = oreader.GetBoolean("IsLeave");


                    oItem.IsDayOff = oreader.GetBoolean("IsDayOff");
                    oItem.IsOSD = oreader.GetBoolean("IsOSD");
                    oItem.IsHoliday = oreader.GetBoolean("IsHoliday");
                    oItem.IsLeave = oreader.GetBoolean("IsLeave");

                    oItem.EmployeeCode = oreader.GetString("EmployeeCode");
                    oItem.EmployeeName = oreader.GetString("EmployeeName");
                    oItem.JoiningDate = oreader.GetDateTime("JoiningDate");

                    oItem.DepartmentName = oreader.GetString("DepartmentName");
                    oItem.DesignationName = oreader.GetString("DesignationName");
                    oItem.LocationName = oreader.GetString("LocationName");
                    oItem.BUName = oreader.GetString("BUName");
                    oItem.IsManual = oreader.GetBoolean("IsManual");
                    oAttendanceDaily_ZNs.Add(oItem);
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
            return oAttendanceDaily_ZNs;
        }

        public List<AttendanceDaily_ZN> GetsTimeCardComp(string sEmployeeIDs, DateTime Startdate, DateTime EndDate, string sLocationID, string sDepartmentIds, string sType, string sBUnitIDs, double nStartSalaryRange, double nEndSalaryRange, string sBlockIDs, string sGroupIDs, Int64 nUserID)
        {
            List<AttendanceDaily_ZN> oAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendanceDaily_ZNDA.GetsTimeCardComp(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sType, sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBlockIDs, sGroupIDs, nUserID, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    AttendanceDaily_ZN oItem = new AttendanceDaily_ZN();
                    oItem.AttendanceID = oreader.GetInt32("AttendanceID");
                    oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                    oItem.EmployeeCode = oreader.GetString("EmployeeCode");
                    oItem.EmployeeName = oreader.GetString("EmployeeName");
                    oItem.AttendanceDate = oreader.GetDateTime("AttendanceDate");
                    oItem.InTime = oreader.GetDateTime("InTime");
                    oItem.OutTime = oreader.GetDateTime("OutTime");
                    oItem.OT_NowWork_First = oreader.GetInt32("OT_NowWork_First");
                    oItem.OT_NowWork_2nd = oreader.GetInt32("OT_NowWork_2nd");
                    oItem.OT_NowWork_Rest = oreader.GetInt32("OT_NowWork_Rest");
                    oItem.TotalWorkingHourInMinute = oreader.GetInt32("TotalWorkingHourInMinute");
                    oItem.LocationName = oreader.GetString("LocationName");
                    oItem.DepartmentName = oreader.GetString("DepartmentName");
                    oItem.DesignationName = oreader.GetString("DesignationName");
                    oItem.LateArrivalMinute = oreader.GetInt32("LateArrivalMinute");
                    oItem.EarlyDepartureMinute = oreader.GetInt32("EarlyDepartureMinute");
                    oItem.IsDayOff = oreader.GetBoolean("IsDayOff");
                    oItem.IsOSD = oreader.GetBoolean("IsOSD");
                    oItem.IsLeave = oreader.GetBoolean("IsLeave");
                    oItem.IsUnPaid = oreader.GetBoolean("IsUnPaid");
                    oItem.IsNoWork = oreader.GetBoolean("IsNoWork");
                    oItem.OverTimeInMinute = oreader.GetInt32("OverTimeInMinute");
                    oItem.LeaveName = oreader.GetString("LeaveName");
                    oItem.LeaveType = (EnumLeaveType)oreader.GetInt16("LeaveType");
                    oItem.IsHoliday = oreader.GetBoolean("IsHoliday");
                    oItem.ShiftID = oreader.GetInt32("ShiftID");
                    oItem.LeaveHeadID = oreader.GetInt32("LeaveHeadID");
                    oItem.ShiftName = oreader.GetString("ShiftName");
                    oItem.OT_NHR = oreader.GetDouble("OT_NHR");
                    oItem.OT_HHR = oreader.GetDouble("OT_HHR");
                    oItem.LeaveDuration = oreader.GetInt32("LeaveDuration");
                    oItem.JoiningDate = oreader.GetDateTime("JoiningDate");
                    oItem.BUName = oreader.GetString("BUName");
                    oItem.BUAddress = oreader.GetString("BUAddress");
                    oItem.Remark = oreader.GetString("Remark");
                    oItem.IsManualOT = oreader.GetBoolean("IsManualOT");
                    oItem.IsPromoted = oreader.GetBoolean("IsPromoted");
                    oItem.IsManual = oreader.GetBoolean("IsManual");
                    oAttendanceDaily_ZNs.Add(oItem);
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
            return oAttendanceDaily_ZNs;
        }

        #endregion
    }
}
