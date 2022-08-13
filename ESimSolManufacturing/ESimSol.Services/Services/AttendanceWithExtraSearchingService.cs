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
    public class AttendanceWithExtraSearchingService : MarshalByRefObject, IAttendanceWithExtraSearchingService
    {
        #region Private functions and declaration
        private AttendanceWithExtraSearching MapObject(NullHandler oReader)
        {
            AttendanceWithExtraSearching oAttendanceWithExtraSearching = new AttendanceWithExtraSearching();
            oAttendanceWithExtraSearching.EmployeeID = oReader.GetInt32("EmployeeID");
            oAttendanceWithExtraSearching.EmployeeName = oReader.GetString("EmployeeName");
            oAttendanceWithExtraSearching.EmployeeCode = oReader.GetString("EmployeeCode");
            oAttendanceWithExtraSearching.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oAttendanceWithExtraSearching.LocationName = oReader.GetString("LocationName");
            oAttendanceWithExtraSearching.DepartmentName = oReader.GetString("DepartmentName");
            oAttendanceWithExtraSearching.DesignationName = oReader.GetString("DesignationName");
            oAttendanceWithExtraSearching.JoiningDate = oReader.GetDateTime("JoiningDate");
            oAttendanceWithExtraSearching.PresentGross = oReader.GetDouble("PresentGross");
            oAttendanceWithExtraSearching.AttDates = oReader.GetString("AttDates");
            oAttendanceWithExtraSearching.AttDates = oReader.GetString("AttDates");
            oAttendanceWithExtraSearching.TotalDays = oReader.GetInt32("TotalDays");

            return oAttendanceWithExtraSearching;
        }

        private AttendanceWithExtraSearching CreateObject(NullHandler oReader)
        {
            AttendanceWithExtraSearching oAttendanceWithExtraSearching = MapObject(oReader);
            return oAttendanceWithExtraSearching;
        }

        private List<AttendanceWithExtraSearching> CreateObjects(IDataReader oReader)
        {
            List<AttendanceWithExtraSearching> oAttendanceWithExtraSearching = new List<AttendanceWithExtraSearching>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceWithExtraSearching oItem = CreateObject(oHandler);
                oAttendanceWithExtraSearching.Add(oItem);
            }
            return oAttendanceWithExtraSearching;
        }

        #endregion

        #region Interface implementation
        public AttendanceWithExtraSearchingService() { }
        public List<AttendanceWithExtraSearching> Gets(string sParams, Int64 nUserID)
        {
            List<AttendanceWithExtraSearching> oAttendanceWithExtraSearching = new List<AttendanceWithExtraSearching>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendanceWithExtraSearchingDA.Gets(sParams, nUserID, tc);
                oAttendanceWithExtraSearching = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceWithExtraSearching", e);
                #endregion
            }
            return oAttendanceWithExtraSearching;
        }
        public List<AttendanceWithExtraSearching> GetsComp(string sParams, Int64 nUserID)
        {
            List<AttendanceWithExtraSearching> oAttendanceWithExtraSearching = new List<AttendanceWithExtraSearching>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendanceWithExtraSearchingDA.GetsComp(sParams, nUserID, tc);
                oAttendanceWithExtraSearching = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceWithExtraSearching", e);
                #endregion
            }
            return oAttendanceWithExtraSearching;
        }
        #endregion
    }
}
