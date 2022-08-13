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
    public class AccountingRatioSetupService : MarshalByRefObject, IAccountingRatioSetupService
    {
        #region Private functions and declaration
        private AccountingRatioSetup MapObject(NullHandler oReader)
        {
            AccountingRatioSetup oAccountingRatioSetup = new AccountingRatioSetup();
            oAccountingRatioSetup.AccountingRatioSetupID = oReader.GetInt32("AccountingRatioSetupID");
            oAccountingRatioSetup.Name = oReader.GetString("Name");
            oAccountingRatioSetup.RatioFormat = (EnumRatioFormat)oReader.GetInt16("RatioFormat");
            oAccountingRatioSetup.DivisibleName = oReader.GetString("DivisibleName");
            oAccountingRatioSetup.DividerName = oReader.GetString("DividerName");
            oAccountingRatioSetup.Remarks = oReader.GetString("Remarks");
            oAccountingRatioSetup.RatioSetupType = (EnumRatioSetupType)oReader.GetInt32("RatioSetupType");
            oAccountingRatioSetup.RatioSetupTypeInt = oReader.GetInt32("RatioSetupType");
            return oAccountingRatioSetup;
        }

        private AccountingRatioSetup CreateObject(NullHandler oReader)
        {
            AccountingRatioSetup oAccountingRatioSetup = new AccountingRatioSetup();
            oAccountingRatioSetup = MapObject(oReader);
            return oAccountingRatioSetup;
        }

        private List<AccountingRatioSetup> CreateObjects(IDataReader oReader)
        {
            List<AccountingRatioSetup> oAccountingRatioSetup = new List<AccountingRatioSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AccountingRatioSetup oItem = CreateObject(oHandler);
                oAccountingRatioSetup.Add(oItem);
            }
            return oAccountingRatioSetup;
        }

        #endregion

        #region Interface implementation
        public AccountingRatioSetupService() { }

        public AccountingRatioSetup Save(AccountingRatioSetup oAccountingRatioSetup, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                List<AccountingRatioSetupDetail> oAccountingRatioSetupDetails = new List<AccountingRatioSetupDetail>();
                oAccountingRatioSetupDetails = oAccountingRatioSetup.AccountingRatioSetupDetails;

                tc = TransactionContext.Begin(true);
                IDataReader reader;

                if (oAccountingRatioSetup.AccountingRatioSetupID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.AccountingRatioSetup, EnumRoleOperationType.Add);
                    reader = AccountingRatioSetupDA.InsertUpdate(tc, oAccountingRatioSetup, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.AccountingRatioSetup, EnumRoleOperationType.Edit);
                    reader = AccountingRatioSetupDA.InsertUpdate(tc, oAccountingRatioSetup, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountingRatioSetup = new AccountingRatioSetup();
                    oAccountingRatioSetup = CreateObject(oReader);
                }
                reader.Close();

                #region Details
                

                if (oAccountingRatioSetupDetails != null)
                {
                    string sAccountingRatioSetupDetailIDIDs = "";
                    foreach (AccountingRatioSetupDetail oItem in oAccountingRatioSetupDetails)
                    {
                        IDataReader readertnc;
                        
                        if (oItem.AccountingRatioSetupDetailID <= 0)
                        {
                            oItem.AccountingRatioSetupID = oAccountingRatioSetup.AccountingRatioSetupID;
                            readertnc = AccountingRatioSetupDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            oItem.AccountingRatioSetupID = oAccountingRatioSetup.AccountingRatioSetupID;
                            readertnc = AccountingRatioSetupDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);
                        if (readertnc.Read())
                        {
                            sAccountingRatioSetupDetailIDIDs = sAccountingRatioSetupDetailIDIDs + oReaderTNC.GetString("AccountingRatioSetupDetailID") + ",";
                        }
                        readertnc.Close();
                    }

                    if (sAccountingRatioSetupDetailIDIDs.Length > 0)
                    {
                        sAccountingRatioSetupDetailIDIDs = sAccountingRatioSetupDetailIDIDs.Remove(sAccountingRatioSetupDetailIDIDs.Length - 1, 1);
                    }
                    AccountingRatioSetupDetail oTempAccountingRatioSetupDetail = new AccountingRatioSetupDetail();
                    oTempAccountingRatioSetupDetail.AccountingRatioSetupID = oAccountingRatioSetup.AccountingRatioSetupID;
                    AccountingRatioSetupDetailDA.Delete(tc, oTempAccountingRatioSetupDetail, EnumDBOperation.Delete, nUserID, sAccountingRatioSetupDetailIDIDs);
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save AccountingRatioSetup. Because of " + e.Message, e);
                #endregion
            }
            return oAccountingRatioSetup;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AccountingRatioSetup oAccountingRatioSetup = new AccountingRatioSetup();
                oAccountingRatioSetup.AccountingRatioSetupID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.AccountingRatioSetup, EnumRoleOperationType.Delete);
                AccountingRatioSetupDA.Delete(tc, oAccountingRatioSetup, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete AccountingRatioSetup. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public AccountingRatioSetup Get(int id, int nUserId)
        {
            AccountingRatioSetup oAccountHead = new AccountingRatioSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AccountingRatioSetupDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get AccountingRatioSetup", e);
                #endregion
            }

            return oAccountHead;
        }

       

        public List<AccountingRatioSetup> Gets(int nUserID)
        {
            List<AccountingRatioSetup> oAccountingRatioSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AccountingRatioSetupDA.Gets(tc);
                oAccountingRatioSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountingRatioSetup", e);
                #endregion
            }

            return oAccountingRatioSetup;
        }

       
        public List<AccountingRatioSetup> Gets(string sSQL,int nUserID)
        {
            List<AccountingRatioSetup> oAccountingRatioSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                
                reader = AccountingRatioSetupDA.Gets(tc, sSQL);
                oAccountingRatioSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountingRatioSetup", e);
                #endregion
            }

            return oAccountingRatioSetup;
        }

       
        #endregion
    }   
}