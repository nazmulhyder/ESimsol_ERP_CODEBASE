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
    public class AccountingRatioSetupDetailService : MarshalByRefObject, IAccountingRatioSetupDetailService
    {
        #region Private functions and declaration
        private AccountingRatioSetupDetail MapObject(NullHandler oReader)
        {
            AccountingRatioSetupDetail oAccountingRatioSetupDetail = new AccountingRatioSetupDetail();
            oAccountingRatioSetupDetail.AccountingRatioSetupDetailID = oReader.GetInt32("AccountingRatioSetupDetailID");
            oAccountingRatioSetupDetail.AccountingRatioSetupID = oReader.GetInt32("AccountingRatioSetupID");
            oAccountingRatioSetupDetail.SubGroupID = oReader.GetInt32("SubGroupID");
            oAccountingRatioSetupDetail.IsDivisible = oReader.GetBoolean("IsDivisible");
            oAccountingRatioSetupDetail.RatioName = oReader.GetString("RatioName");
            oAccountingRatioSetupDetail.DivisibleName = oReader.GetString("DivisibleName");
            oAccountingRatioSetupDetail.DividerName = oReader.GetString("DividerName");
            oAccountingRatioSetupDetail.SubGroupCode = oReader.GetString("SubGroupCode");
            oAccountingRatioSetupDetail.SubGroupName = oReader.GetString("SubGroupName");
            oAccountingRatioSetupDetail.AccountType = (EnumAccountType)oReader.GetInt16("AccountType");
            oAccountingRatioSetupDetail.ComponentID = oReader.GetInt32("ComponentID");
            oAccountingRatioSetupDetail.RatioComponent = (EnumRatioComponent)oReader.GetInt32("RatioComponent");
            oAccountingRatioSetupDetail.RatioComponentInt = oReader.GetInt32("RatioComponent");
                        
            return oAccountingRatioSetupDetail;
        }

        private AccountingRatioSetupDetail CreateObject(NullHandler oReader)
        {
            AccountingRatioSetupDetail oAccountingRatioSetupDetail = new AccountingRatioSetupDetail();
            oAccountingRatioSetupDetail = MapObject(oReader);
            return oAccountingRatioSetupDetail;
        }

        private List<AccountingRatioSetupDetail> CreateObjects(IDataReader oReader)
        {
            List<AccountingRatioSetupDetail> oAccountingRatioSetupDetail = new List<AccountingRatioSetupDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AccountingRatioSetupDetail oItem = CreateObject(oHandler);
                oAccountingRatioSetupDetail.Add(oItem);
            }
            return oAccountingRatioSetupDetail;
        }

        #endregion

        #region Interface implementation
        public AccountingRatioSetupDetailService() { }

        public AccountingRatioSetupDetail Save(AccountingRatioSetupDetail oAccountingRatioSetupDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                if (oAccountingRatioSetupDetail.AccountingRatioSetupDetailID <= 0)
                {
                    reader = AccountingRatioSetupDetailDA.InsertUpdate(tc, oAccountingRatioSetupDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = AccountingRatioSetupDetailDA.InsertUpdate(tc, oAccountingRatioSetupDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountingRatioSetupDetail = new AccountingRatioSetupDetail();
                    oAccountingRatioSetupDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save AccountingRatioSetupDetail. Because of " + e.Message, e);
                #endregion
            }
            return oAccountingRatioSetupDetail;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AccountingRatioSetupDetail oAccountingRatioSetupDetail = new AccountingRatioSetupDetail();
                oAccountingRatioSetupDetail.AccountingRatioSetupDetailID = id;
                AccountingRatioSetupDetailDA.Delete(tc, oAccountingRatioSetupDetail, EnumDBOperation.Delete, nUserId,"");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete AccountingRatioSetupDetail. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public AccountingRatioSetupDetail Get(int id, int nUserId)
        {
            AccountingRatioSetupDetail oAccountHead = new AccountingRatioSetupDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AccountingRatioSetupDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get AccountingRatioSetupDetail", e);
                #endregion
            }

            return oAccountHead;
        }

       

        public List<AccountingRatioSetupDetail> Gets(int nUserID)
        {
            List<AccountingRatioSetupDetail> oAccountingRatioSetupDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AccountingRatioSetupDetailDA.Gets(tc);
                oAccountingRatioSetupDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountingRatioSetupDetail", e);
                #endregion
            }

            return oAccountingRatioSetupDetail;
        }
        public List<AccountingRatioSetupDetail> GetsForARS(int nARSID, bool bIsDivisible, int nUserID)
        {
            List<AccountingRatioSetupDetail> oAccountingRatioSetupDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AccountingRatioSetupDetailDA.GetsForARS(tc,nARSID,bIsDivisible);
                oAccountingRatioSetupDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountingRatioSetupDetail", e);
                #endregion
            }

            return oAccountingRatioSetupDetail;
        }
       
        public List<AccountingRatioSetupDetail> Gets(string sSQL,int nUserID)
        {
            List<AccountingRatioSetupDetail> oAccountingRatioSetupDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                
                reader = AccountingRatioSetupDetailDA.Gets(tc, sSQL);
                oAccountingRatioSetupDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountingRatioSetupDetail", e);
                #endregion
            }

            return oAccountingRatioSetupDetail;
        }

       
        #endregion
    }   
}