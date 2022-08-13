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
    public class AttendanceSummery_ListService : MarshalByRefObject, IAttendanceSummery_ListService
    {
        #region Private functions and declaration
        private AttendanceSummery_List MapObject(NullHandler oReader)
        {
            AttendanceSummery_List oAttendanceSummery = new AttendanceSummery_List();

            oAttendanceSummery.EmployeeID = oReader.GetInt32("EmployeeID");
            oAttendanceSummery.EmployeeCode = oReader.GetString("EmployeeCode");
            oAttendanceSummery.EmployeeName = oReader.GetString("EmployeeName");
            oAttendanceSummery.DepartmentName = oReader.GetString("DepartmentName");
            oAttendanceSummery.DesignationName = oReader.GetString("DesignationName");
            oAttendanceSummery.JoiningDate = oReader.GetDateTime("JoiningDate");
            oAttendanceSummery.WorkingTill = oReader.GetString("WorkingTill");
            oAttendanceSummery.TotalPresent = oReader.GetInt32("TotalPresent");
            oAttendanceSummery.TotalAbsent = oReader.GetInt32("TotalAbsent");
            oAttendanceSummery.TotalLate = oReader.GetInt32("TotalLate");
            oAttendanceSummery.TotalEarlyLeave = oReader.GetInt32("TotalEarlyLeave");
            oAttendanceSummery.TotalLeave = oReader.GetInt32("TotalLeave");
            oAttendanceSummery.LeaveBalance = oReader.GetString("LeaveBalance");
            oAttendanceSummery.Performance = oReader.GetString("Performance");
            
            return oAttendanceSummery;
        }

        private AttendanceSummery_List CreateObject(NullHandler oReader)
        {
            AttendanceSummery_List oAttendanceSummery = MapObject(oReader);
            return oAttendanceSummery;
        }

        private List<AttendanceSummery_List> CreateObjects(IDataReader oReader)
        {
            List<AttendanceSummery_List> oAttendanceSummery = new List<AttendanceSummery_List>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceSummery_List oItem = CreateObject(oHandler);
                oAttendanceSummery.Add(oItem);
            }
            return oAttendanceSummery;
        }

        #endregion

        #region Interface implementation
        public AttendanceSummery_ListService() { }

        public List<AttendanceSummery_List> Gets(string sParams, Int64 nUserID)
        {
            List<AttendanceSummery_List>  oAttendanceSummerys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceSummery_ListDA.Gets(sParams, tc);
                oAttendanceSummerys = CreateObjects(reader);

                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AttendanceSummery", e);
                #endregion
            }
            return oAttendanceSummerys;
        }


        #endregion

    }
}
