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
    public class AttendanceDailyManualHistoryService : MarshalByRefObject, IAttendanceDailyManualHistoryService
    {
        #region Private functions and declaration
        private AttendanceDailyManualHistory MapObject(NullHandler oReader)
        {
            AttendanceDailyManualHistory oAttendanceDailyManualHistory = new AttendanceDailyManualHistory();

            oAttendanceDailyManualHistory.ADMHID = oReader.GetInt32("ADMHID");
            oAttendanceDailyManualHistory.MOMUHID = oReader.GetInt32("MOMUHID");
            oAttendanceDailyManualHistory.MOCAID = oReader.GetInt32("MOCAID");
            oAttendanceDailyManualHistory.AttendanceID = oReader.GetInt32("AttendanceID");
            oAttendanceDailyManualHistory.Description = oReader.GetString("Description");
            oAttendanceDailyManualHistory.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oAttendanceDailyManualHistory.EmployeeName = oReader.GetString("EmployeeName");

            return oAttendanceDailyManualHistory;

        }

        private AttendanceDailyManualHistory CreateObject(NullHandler oReader)
        {
            AttendanceDailyManualHistory oAttendanceDailyManualHistory = MapObject(oReader);
            return oAttendanceDailyManualHistory;
        }

        private List<AttendanceDailyManualHistory> CreateObjects(IDataReader oReader)
        {
            List<AttendanceDailyManualHistory> oAttendanceDailyManualHistorys = new List<AttendanceDailyManualHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceDailyManualHistory oItem = CreateObject(oHandler);
                oAttendanceDailyManualHistorys.Add(oItem);
            }
            return oAttendanceDailyManualHistorys;
        }

        #endregion

        #region Interface implementation
        public AttendanceDailyManualHistoryService() { }
        public List<AttendanceDailyManualHistory> Gets(string sSQL, Int64 nUserID)
        {
            List<AttendanceDailyManualHistory> oAttendanceDailyManualHistorys = new List<AttendanceDailyManualHistory>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceDailyManualHistoryDA.Gets(sSQL, tc);
                oAttendanceDailyManualHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                AttendanceDailyManualHistory oAttendanceDailyManualHistory = new AttendanceDailyManualHistory();
                oAttendanceDailyManualHistory.ErrorMessage = e.Message;
                oAttendanceDailyManualHistorys.Add(oAttendanceDailyManualHistory);
                #endregion
            }
            return oAttendanceDailyManualHistorys;
        }

        #endregion


    }
}
