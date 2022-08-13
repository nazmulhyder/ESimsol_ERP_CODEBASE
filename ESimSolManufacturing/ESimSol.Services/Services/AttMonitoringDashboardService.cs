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
    public class AttMonitoringDashboardService : MarshalByRefObject, IAttMonitoringDashboardService
    {
        #region Private functions and declaration
        private AttMonitoringDashboard MapObject(NullHandler oReader)
        {
            AttMonitoringDashboard oAttMonitoringDashboard = new AttMonitoringDashboard();
            oAttMonitoringDashboard.TotalEmployee = oReader.GetInt32("TotalEmployee");
            oAttMonitoringDashboard.PresentPercent = oReader.GetDouble("PresentPercent");
            oAttMonitoringDashboard.AbsentPercent = oReader.GetDouble("AbsentPercent");
            oAttMonitoringDashboard.LeavePercent = oReader.GetDouble("LeavePercent");
            oAttMonitoringDashboard.LatePercent = oReader.GetDouble("LatePercent");
            oAttMonitoringDashboard.EarlyPercent = oReader.GetDouble("EarlyPercent");
            
            return oAttMonitoringDashboard;

        }

        private AttMonitoringDashboard CreateObject(NullHandler oReader)
        {
            AttMonitoringDashboard oAttMonitoringDashboard = MapObject(oReader);
            return oAttMonitoringDashboard;
        }

        private List<AttMonitoringDashboard> CreateObjects(IDataReader oReader)
        {
            List<AttMonitoringDashboard> oAttMonitoringDashboard = new List<AttMonitoringDashboard>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttMonitoringDashboard oItem = CreateObject(oHandler);
                oAttMonitoringDashboard.Add(oItem);
            }
            return oAttMonitoringDashboard;
        }

        #endregion

        #region Interface implementation
        public AttMonitoringDashboardService() { }




        public AttMonitoringDashboard Get(DateTime StartDate, DateTime EndDate,Int64 nUserId)
        {
            AttMonitoringDashboard oAttMonitoringDashboard = new AttMonitoringDashboard();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttMonitoringDashboardDA.Get(tc, StartDate, EndDate);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttMonitoringDashboard = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get View_EmployeeProduction", e);
                #endregion
            }
            return oAttMonitoringDashboard;
        }


        #endregion

    }
}
