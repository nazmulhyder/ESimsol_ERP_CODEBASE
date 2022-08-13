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
    public class AttendanceSummery_ChartService : MarshalByRefObject, IAttendanceSummery_ChartService
    {
        #region Private functions and declaration
        private AttendanceSummery_Chart MapObject(NullHandler oReader)
        {
            AttendanceSummery_Chart oAttendanceSummery = new AttendanceSummery_Chart();

            oAttendanceSummery.TotalPresent = oReader.GetInt32("TotalPresent");
            oAttendanceSummery.PresentPercent = oReader.GetDouble("PresentPercent");
            oAttendanceSummery.TotalAbsent = oReader.GetInt32("TotalAbsent");
            oAttendanceSummery.AbsentPercent = oReader.GetDouble("AbsentPercent");
            oAttendanceSummery.TotalLeave = oReader.GetInt32("TotalLeave");
            oAttendanceSummery.LeavePercent = oReader.GetDouble("LeavePercent");
            oAttendanceSummery.TotalEarlyLeave = oReader.GetInt32("TotalEarlyLeave");
            oAttendanceSummery.EarlyLeavePercent = oReader.GetDouble("EarlyLeavePercent");
            oAttendanceSummery.TotalLate = oReader.GetInt32("TotalLate");
            oAttendanceSummery.LatePercent = oReader.GetDouble("LatePercent");
          
            return oAttendanceSummery;
        }

        private AttendanceSummery_Chart CreateObject(NullHandler oReader)
        {
            AttendanceSummery_Chart oAttendanceSummery = MapObject(oReader);
            return oAttendanceSummery;
        }

        private List<AttendanceSummery_Chart> CreateObjects(IDataReader oReader)
        {
            List<AttendanceSummery_Chart> oAttendanceSummery = new List<AttendanceSummery_Chart>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceSummery_Chart oItem = CreateObject(oHandler);
                oAttendanceSummery.Add(oItem);
            }
            return oAttendanceSummery;
        }

        #endregion

        #region Interface implementation
        public AttendanceSummery_ChartService() { }

        public AttendanceSummery_Chart Get(DateTime DateFrom, DateTime DateTo, Int64 nUserID)
        {
            AttendanceSummery_Chart oAttendanceSummery = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AttendanceSummery_ChartDA.Get(DateFrom, DateTo, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceSummery = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get AttendanceSummery", e);
                #endregion
            }
            return oAttendanceSummery;
        }


        #endregion

    }
}
