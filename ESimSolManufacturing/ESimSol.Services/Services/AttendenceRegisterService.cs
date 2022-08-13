using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
namespace ESimSol.Services.Services
{
    public class AttendenceRegisterService : MarshalByRefObject, IAttendenceRegisterService
    {
        #region Private functions and declaration

        private AttendenceRegister MapObject(NullHandler oReader)
        {
            AttendenceRegister oAttendenceRegister = new AttendenceRegister();
            oAttendenceRegister.EmployeeID = oReader.GetInt32("EmployeeID");
            oAttendenceRegister.MonthID = oReader.GetInt32("MonthID");
            oAttendenceRegister.TotalWorkingDays = oReader.GetInt32("TotalWorkingDays");
            oAttendenceRegister.YearID = oReader.GetInt32("YearID");
            oAttendenceRegister.EarlyLeaveCount = oReader.GetInt32("EarlyLeaveCount");
            oAttendenceRegister.LateAttendanceCount = oReader.GetInt32("LateAttendanceCount");
            oAttendenceRegister.EmployeeName = oReader.GetString("EmployeeName");
            oAttendenceRegister.Code = oReader.GetString("Code");
            oAttendenceRegister.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oAttendenceRegister.LocationID = oReader.GetInt32("LocationID");
            oAttendenceRegister.DepartmentID = oReader.GetInt32("DepartmentID");
            oAttendenceRegister.BUName = oReader.GetString("BUName");
            oAttendenceRegister.DesignationID = oReader.GetInt32("DesignationID");
            oAttendenceRegister.LocationName = oReader.GetString("LocationName");
            oAttendenceRegister.Department = oReader.GetString("Department");
            oAttendenceRegister.Designation = oReader.GetString("Designation");
            oAttendenceRegister.Gender = oReader.GetString("Gender");
            oAttendenceRegister.ShiftID = oReader.GetInt32("ShiftID");

            oAttendenceRegister.Fullfiled = oReader.GetInt32("Fullfiled");
            oAttendenceRegister.LessThan11Hrs = oReader.GetInt32("LessThan11Hrs");
            oAttendenceRegister.LessThan9Hrs = oReader.GetInt32("LessThan9Hrs");
            oAttendenceRegister.LessThan6Hrs = oReader.GetInt32("LessThan6Hrs");
            oAttendenceRegister.TotalAbsent = oReader.GetInt32("TotalAbsent");
            oAttendenceRegister.TotalLeave = oReader.GetInt32("TotalLeave");
            oAttendenceRegister.DutyHour = oReader.GetInt32("DutyHour");
            oAttendenceRegister.TotalDayOff = oReader.GetInt32("TotalDayOff");
            oAttendenceRegister.EmployeeTypeName = oReader.GetString("EmployeeTypeName");
            
            return oAttendenceRegister;
        }

        private AttendenceRegister CreateObject(NullHandler oReader)
        {
            AttendenceRegister oAttendenceRegister = new AttendenceRegister();
            oAttendenceRegister = MapObject(oReader);
            return oAttendenceRegister;
        }

        private List<AttendenceRegister> CreateObjects(IDataReader oReader)
        {
            List<AttendenceRegister> oAttendenceRegister = new List<AttendenceRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendenceRegister oItem = CreateObject(oHandler);
                oAttendenceRegister.Add(oItem);
            }
            return oAttendenceRegister;
        }

        #endregion

        #region Interface implementation


        public AttendenceRegister Get(int id, Int64 nUserId)
        {
            AttendenceRegister oAttendenceRegister = new AttendenceRegister();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = AttendenceRegisterDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendenceRegister = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get AttendenceRegister", e);
                #endregion
            }
            return oAttendenceRegister;
        }

        public List<AttendenceRegister> Gets(int nAttendenceID, Int64 nUserID)
        {
            List<AttendenceRegister> oAttendenceRegisters = new List<AttendenceRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendenceRegisterDA.Gets(tc, nAttendenceID);
                oAttendenceRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                AttendenceRegister oAttendenceRegister = new AttendenceRegister();
                oAttendenceRegister.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oAttendenceRegisters;
        }

        public List<AttendenceRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<AttendenceRegister> oAttendenceRegisters = new List<AttendenceRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendenceRegisterDA.Gets(tc, sSQL);
                oAttendenceRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendenceRegister", e);
                #endregion
            }
            return oAttendenceRegisters;
        }
        
        public List<AttendenceRegister> Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, Int64 nUserID)
        {
            List<AttendenceRegister> oAttendenceRegister = new List<AttendenceRegister>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendenceRegisterDA.Gets(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds, sRemarks, nUserID, tc);
                oAttendenceRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendenceRegister", e);
                #endregion
            }
            return oAttendenceRegister;
        }
        public List<AttendenceRegister> GetsLateEarly(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, bool bIsMultipleMonth, string sMonthIDs, string sYearIDs, Int64 nUserID)
        {
            List<AttendenceRegister> oAttendenceRegister = new List<AttendenceRegister>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendenceRegisterDA.GetsLateEarly(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds, sRemarks, bIsMultipleMonth, sMonthIDs, sYearIDs, nUserID, tc);
                oAttendenceRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendenceRegister", e);
                #endregion
            }
            return oAttendenceRegister;
        }

        #endregion
    }

}
