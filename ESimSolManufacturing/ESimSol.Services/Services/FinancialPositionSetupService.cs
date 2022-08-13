using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.Services
{

    public class FinancialPositionSetupService : MarshalByRefObject, IFinancialPositionSetupService
    {
        #region Private functions and declaration
        private FinancialPositionSetup MapObject(NullHandler oReader)
        {
            FinancialPositionSetup oFinancialPositionSetup = new FinancialPositionSetup();
            oFinancialPositionSetup.FinancialPositionSetupID = oReader.GetInt32("FinancialPositionSetupID");
            oFinancialPositionSetup.Sequence = oReader.GetInt32("Sequence");
            oFinancialPositionSetup.AccountHeadName = oReader.GetString("AccountHeadName");
            oFinancialPositionSetup.AccountCode = oReader.GetString("AccountCode");
            oFinancialPositionSetup.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oFinancialPositionSetup.AccountType = (EnumAccountType)oReader.GetInt32("AccountType");
            oFinancialPositionSetup.ComponentID = oReader.GetInt32("ComponentID");
            return oFinancialPositionSetup;
        }

        private FinancialPositionSetup CreateObject(NullHandler oReader)
        {
            FinancialPositionSetup oFinancialPositionSetup = new FinancialPositionSetup();
            oFinancialPositionSetup = MapObject(oReader);
            return oFinancialPositionSetup;
        }

        private List<FinancialPositionSetup> CreateObjects(IDataReader oReader)
        {
            List<FinancialPositionSetup> oFinancialPositionSetup = new List<FinancialPositionSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FinancialPositionSetup oItem = CreateObject(oHandler);
                oFinancialPositionSetup.Add(oItem);
            }
            return oFinancialPositionSetup;
        }

        #endregion

        #region Interface implementation
        public FinancialPositionSetupService() { }

        public List<FinancialPositionSetup> Save(FinancialPositionSetup oFinancialPositionSetup, Int64 nUserID)
        {
            List<FinancialPositionSetup> oFinancialPositionSetups = new List<FinancialPositionSetup>();
            List<FinancialPositionSetup> oTempFinancialPositionSetups = new List<FinancialPositionSetup>();
            oTempFinancialPositionSetups.AddRange(oFinancialPositionSetup.AssetSetups);//set assets
            oTempFinancialPositionSetups.AddRange(oFinancialPositionSetup.LiabilityWithOwnersEquitySetups);//liabilityey
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (FinancialPositionSetup oItem in oTempFinancialPositionSetups)
                {
                    if(oItem.FinancialPositionSetupID<=0)
                    {
                        reader = FinancialPositionSetupDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = FinancialPositionSetupDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFinancialPositionSetup = new FinancialPositionSetup();
                        oFinancialPositionSetup = CreateObject(oReader);
                    }
                    reader.Close();
                }
                reader = null;
                reader = FinancialPositionSetupDA.Gets(tc);
                oFinancialPositionSetups = CreateObjects(reader);
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save FinancialPositionSetup. Because of " + e.Message, e);
                #endregion
            }
            return oFinancialPositionSetups;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FinancialPositionSetup oFinancialPositionSetup = new FinancialPositionSetup();
                oFinancialPositionSetup.FinancialPositionSetupID = id;
                FinancialPositionSetupDA.Delete(tc, oFinancialPositionSetup, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public FinancialPositionSetup Get(int id, Int64 nUserId)
        {
            FinancialPositionSetup oFinancialPositionSetup = new FinancialPositionSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FinancialPositionSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFinancialPositionSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FinancialPositionSetup", e);
                #endregion
            }
            return oFinancialPositionSetup;
        }



        public List<FinancialPositionSetup> Gets(Int64 nUserID)
        {
            List<FinancialPositionSetup> oFinancialPositionSetups = new List<FinancialPositionSetup>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FinancialPositionSetupDA.Gets(tc);
                oFinancialPositionSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FinancialPositionSetup", e);
                #endregion
            }
            return oFinancialPositionSetups;
        }
        public List<FinancialPositionSetup> Gets(string sSQL, Int64 nUserID)
        {
            List<FinancialPositionSetup> oFinancialPositionSetups = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FinancialPositionSetupDA.Gets(tc, sSQL);
                oFinancialPositionSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FinancialPositionSetup", e);
                #endregion
            }
            return oFinancialPositionSetups;
        }

        #endregion
    }   
   
}
