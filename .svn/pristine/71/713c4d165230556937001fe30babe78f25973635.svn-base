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
    public class ELProcessEditHistoryService : MarshalByRefObject, IELProcessEditHistoryService
    {
        #region Private functions and declaration
        private ELProcessEditHistory MapObject(NullHandler oReader)
        {
            ELProcessEditHistory oELProcessEditHistory = new ELProcessEditHistory();
            oELProcessEditHistory.ELPEHID = oReader.GetInt32("ELPEHID");
            oELProcessEditHistory.ELPID = oReader.GetInt32("ELPID");
            oELProcessEditHistory.PreviousPresentBalance = oReader.GetInt32("PreviousPresentBalance");
            oELProcessEditHistory.CurrentpresentBalance = oReader.GetInt32("CurrentpresentBalance");
            oELProcessEditHistory.LastProcessDate = oReader.GetDateTime("LastProcessDate");
            oELProcessEditHistory.Description = oReader.GetString("Description");
            oELProcessEditHistory.UserName = oReader.GetString("EmployeeNameCode");
            return oELProcessEditHistory;

        }

        private ELProcessEditHistory CreateObject(NullHandler oReader)
        {
            ELProcessEditHistory oELProcessEditHistory = MapObject(oReader);
            return oELProcessEditHistory;
        }

        private List<ELProcessEditHistory> CreateObjects(IDataReader oReader)
        {
            List<ELProcessEditHistory> oELProcessEditHistory = new List<ELProcessEditHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ELProcessEditHistory oItem = CreateObject(oHandler);
                oELProcessEditHistory.Add(oItem);
            }
            return oELProcessEditHistory;
        }

        #endregion

        #region Interface implementation
        public ELProcessEditHistoryService() { }

        public ELProcessEditHistory IUD(ELProcessEditHistory oELProcessEditHistory, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ELProcessEditHistoryDA.IUD(tc, oELProcessEditHistory, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oELProcessEditHistory = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oELProcessEditHistory.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oELProcessEditHistory;
        }

        public List<ELProcessEditHistory> Gets(string sSQL, Int64 nUserID)
        {
            List<ELProcessEditHistory> oELProcessEditHistorys = new List<ELProcessEditHistory>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ELProcessEditHistoryDA.Gets(sSQL,tc);
                oELProcessEditHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ELProcessEditHistory", e);
                #endregion
            }
            return oELProcessEditHistorys;
        }

        #endregion
    }
}
