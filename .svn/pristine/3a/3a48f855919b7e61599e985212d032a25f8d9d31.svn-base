using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class ManagementDashboardService : MarshalByRefObject, IManagementDashboardService
    {
        #region Private functions and declaration
        private ManagementDashboard MapObject(NullHandler oReader)
        {
            ManagementDashboard oManagementDashboard = new ManagementDashboard();
            oManagementDashboard.MerchandiserID = oReader.GetInt32("MerchandiserID");
            oManagementDashboard.MerchandiserName = oReader.GetString("MerchandiserName");
            oManagementDashboard.LogInID = oReader.GetString("LogInID");
            oManagementDashboard.NumberOfStyle = oReader.GetInt32("NumberOfStyle");
            oManagementDashboard.NumberOfDevelopment = oReader.GetInt32("NumberOfDevelopment");
            oManagementDashboard.NumberOfOrder = oReader.GetInt32("NumberOfOrder");
            oManagementDashboard.NumberOfTAP = oReader.GetInt32("NumberOfTAP");
            oManagementDashboard.TotalOrderQty = oReader.GetInt32("TotalOrderQty");
            oManagementDashboard.NumberOfBuyer = oReader.GetInt32("NumberOfBuyer");
            oManagementDashboard.NumberOfFactory = oReader.GetInt32("NumberOfFactory");
            oManagementDashboard.TaskPending = oReader.GetInt32("TaskPending");
            oManagementDashboard.RegularTask = oReader.GetInt32("RegularTask");
            return oManagementDashboard;
        }

        private ManagementDashboard CreateObject(NullHandler oReader)
        {
            ManagementDashboard oManagementDashboard = new ManagementDashboard();
            oManagementDashboard = MapObject(oReader);
            return oManagementDashboard;
        }

        private List<ManagementDashboard> CreateObjects(IDataReader oReader)
        {
            List<ManagementDashboard> oManagementDashboard = new List<ManagementDashboard>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ManagementDashboard oItem = CreateObject(oHandler);
                oManagementDashboard.Add(oItem);
            }
            return oManagementDashboard;
        }

        #endregion



        #region Interface implementation
        public List<ManagementDashboard> Gets(string sSQL, int BusinessSessionID, Int64 nUserID)
        {
            List<ManagementDashboard> oManagementDashboards = new List<ManagementDashboard>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ManagementDashboardDA.Gets(tc, sSQL, BusinessSessionID);
                oManagementDashboards = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Management Dashboard", e);
                #endregion
            }

            return oManagementDashboards;
        }
        #endregion
    }
}
