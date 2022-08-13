using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    
    public class LabdipHistoryService : MarshalByRefObject, ILabdipHistoryService
    {
        #region Private functions and declaration
        private LabdipHistory MapObject(NullHandler oReader)
        {
            LabdipHistory oLabdipHistory = new LabdipHistory();
            oLabdipHistory.LabdipHistoryID = oReader.GetInt32("LabdipHistoryID");
            oLabdipHistory.LabDipID = oReader.GetInt32("LabDipID");
            oLabdipHistory.Currentstatus = (EnumLabdipOrderStatus)oReader.GetInt16("Currentstatus");
            oLabdipHistory.Previousstatus = (EnumLabdipOrderStatus)oReader.GetInt16("Previousstatus");
            oLabdipHistory.DateTime = oReader.GetDateTime("DBServerDateTime");
            oLabdipHistory.Note = oReader.GetString("Note");
            //oLabdipHistory.DBUserID = oReader.GetInt32("DBUserID");
            oLabdipHistory.UserName = oReader.GetString("UserName");       
            //oLabdipHistory.CntDBUserID = oReader.GetInt32("CntDBUserID");
            //oLabdipHistory.CntDBDateTime = oReader.GetDateTime("CntDBDateTime");
            return oLabdipHistory;
        }

        private LabdipHistory CreateObject(NullHandler oReader)
        {
            LabdipHistory oLabdipHistory = new LabdipHistory();
            oLabdipHistory=MapObject(oReader);
            return oLabdipHistory;
        }

        private List<LabdipHistory> CreateObjects(IDataReader oReader)
        {
            List<LabdipHistory> oLabdipHistorys = new List<LabdipHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LabdipHistory oItem = CreateObject(oHandler);
                oLabdipHistorys.Add(oItem);
            }
            return oLabdipHistorys;
        }
        #endregion

        #region Interface implementation
        public LabdipHistoryService() { }
     
    

        public LabdipHistory Get(int id, Int64 nUserId)
        {
            LabdipHistory oLabdipHistory = new LabdipHistory();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LabdipHistoryDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabdipHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LabdipHistory", e);
                #endregion
            }

            return oLabdipHistory;
        }
        public LabdipHistory Getby(int nLabdipID, int nOrderStatus, Int64 nUserId)
        {
            LabdipHistory oLabdipHistory = new LabdipHistory();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LabdipHistoryDA.Getby(tc, nLabdipID, nOrderStatus);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabdipHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LabdipHistory", e);
                #endregion
            }

            return oLabdipHistory;
        }

        public List<LabdipHistory> Gets(int nLabdipID, Int64 nUserId)
        {
            List<LabdipHistory> oLabdipHistorys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LabdipHistoryDA.Gets(tc, nLabdipID);
                oLabdipHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LabdipHistorys", e);
                #endregion
            }

            return oLabdipHistorys;
        } 
        #endregion
    }
}
