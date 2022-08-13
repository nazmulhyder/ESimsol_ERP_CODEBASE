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
    public class ServiceBookLeaveService : MarshalByRefObject, IServiceBookLeaveService
    {
        #region Private functions and declaration
        private ServiceBookLeave MapObject(NullHandler oReader)
        {
            ServiceBookLeave oServiceBookLeave = new ServiceBookLeave();

            oServiceBookLeave.StartDate = oReader.GetDateTime("StartDate");
            oServiceBookLeave.EndDate = oReader.GetDateTime("EndDate");
            oServiceBookLeave.Leave = oReader.GetString("Leave");
            oServiceBookLeave.LeaveTaken = oReader.GetInt32("LeaveTaken");
            return oServiceBookLeave;

        }

        private ServiceBookLeave CreateObject(NullHandler oReader)
        {
            ServiceBookLeave oServiceBookLeave = MapObject(oReader);
            return oServiceBookLeave;
        }

        private List<ServiceBookLeave> CreateObjects(IDataReader oReader)
        {
            List<ServiceBookLeave> oServiceBookLeaves = new List<ServiceBookLeave>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ServiceBookLeave oItem = CreateObject(oHandler);
                oServiceBookLeaves.Add(oItem);
            }
            return oServiceBookLeaves;
        }

        #endregion

        #region Interface implementation
        public ServiceBookLeaveService() { }
        public List<ServiceBookLeave> Gets(int nEmployeeID, Int64 nUserID)
        {
            List<ServiceBookLeave> oServiceBookLeaves = new List<ServiceBookLeave>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ServiceBookLeaveDA.Gets(nEmployeeID, tc);
                oServiceBookLeaves = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ServiceBookLeave oServiceBookLeave = new ServiceBookLeave();
                oServiceBookLeave.ErrorMessage = e.Message;
                oServiceBookLeaves.Add(oServiceBookLeave);
                #endregion
            }
            return oServiceBookLeaves;
        }

        #endregion
    }
}
