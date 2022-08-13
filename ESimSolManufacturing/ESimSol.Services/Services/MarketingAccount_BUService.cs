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
    public class MarketingAccount_BUService : MarshalByRefObject, IMarketingAccount_BUService
    {
        #region Private functions and declaration

        private MarketingAccount_BU MapObject(NullHandler oReader)
        {
            MarketingAccount_BU oMarketingAccount_BU = new MarketingAccount_BU();
            oMarketingAccount_BU.MarketingAccount_BUID = oReader.GetInt32("MarketingAccount_BUID");
            oMarketingAccount_BU.BUID = oReader.GetInt32("BUID");
            oMarketingAccount_BU.MarketingAccountID = oReader.GetInt32("MarketingAccountID");
            oMarketingAccount_BU.BUName = oReader.GetString("BUName");
           oMarketingAccount_BU.Name = oReader.GetString("MarketingAccountName");
            return oMarketingAccount_BU;
        }

        private MarketingAccount_BU CreateObject(NullHandler oReader)
        {
            MarketingAccount_BU oMarketingAccount_BU = new MarketingAccount_BU();
            oMarketingAccount_BU = MapObject(oReader);
            return oMarketingAccount_BU;
        }

        private List<MarketingAccount_BU> CreateObjects(IDataReader oReader)
        {
            List<MarketingAccount_BU> oMarketingAccount_BU = new List<MarketingAccount_BU>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MarketingAccount_BU oItem = CreateObject(oHandler);
                oMarketingAccount_BU.Add(oItem);
            }
            return oMarketingAccount_BU;
        }

        #endregion

        #region Interface implementation
        public MarketingAccount_BU Save(MarketingAccount_BU oMarketingAccount_BU, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oMarketingAccount_BU.MarketingAccount_BUID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "MarketingAccount_BU", EnumRoleOperationType.Add);
                    reader = MarketingAccount_BUDA.InsertUpdate(tc, oMarketingAccount_BU, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "MarketingAccount_BU", EnumRoleOperationType.Edit);
                    reader = MarketingAccount_BUDA.InsertUpdate(tc, oMarketingAccount_BU, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMarketingAccount_BU = new MarketingAccount_BU();
                    oMarketingAccount_BU = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oMarketingAccount_BU = new MarketingAccount_BU();
                    oMarketingAccount_BU.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oMarketingAccount_BU;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MarketingAccount_BU oMarketingAccount_BU = new MarketingAccount_BU();
                oMarketingAccount_BU.MarketingAccount_BUID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "MarketingAccount_BU", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "MarketingAccount_BU", id);
                MarketingAccount_BUDA.Delete(tc, oMarketingAccount_BU, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }

        public MarketingAccount_BU Get(int id, Int64 nUserId)
        {
            MarketingAccount_BU oMarketingAccount_BU = new MarketingAccount_BU();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = MarketingAccount_BUDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMarketingAccount_BU = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get MarketingAccount_BU", e);
                #endregion
            }
            return oMarketingAccount_BU;
        }

        public List<MarketingAccount_BU> Gets(int nID, Int64 nUserID)
        {
            List<MarketingAccount_BU> oMarketingAccount_BUs = new List<MarketingAccount_BU>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MarketingAccount_BUDA.Gets(tc, nID);
                oMarketingAccount_BUs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                MarketingAccount_BU oMarketingAccount_BU = new MarketingAccount_BU();
                oMarketingAccount_BU.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oMarketingAccount_BUs;
        }

        public List<MarketingAccount_BU> Gets(string sSQL, Int64 nUserID)
        {
            List<MarketingAccount_BU> oMarketingAccount_BUs = new List<MarketingAccount_BU>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MarketingAccount_BUDA.Gets(tc, sSQL);
                oMarketingAccount_BUs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MarketingAccount_BU", e);
                #endregion
            }
            return oMarketingAccount_BUs;
        }

        #endregion
    }

}
