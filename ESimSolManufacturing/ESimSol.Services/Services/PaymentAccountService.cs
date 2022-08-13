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
    public class PaymentAccountService : MarshalByRefObject, IPaymentAccountService
    {
        #region Private functions and declaration
        private PaymentAccount MapObject(NullHandler oReader)
        {
            PaymentAccount oPaymentAccount = new PaymentAccount();
            oPaymentAccount.PaymentAccountID = oReader.GetInt32("PaymentAccountID");
            oPaymentAccount.BUID = oReader.GetInt32("BUID");
            oPaymentAccount.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oPaymentAccount.IsDefault = oReader.GetBoolean("IsDefault");
            oPaymentAccount.AccountCode = oReader.GetString("AccountCode");
            oPaymentAccount.AccountHeadName = oReader.GetString("AccountHeadName");
            return oPaymentAccount;
        }

        private PaymentAccount CreateObject(NullHandler oReader)
        {
            PaymentAccount oPaymentAccount = new PaymentAccount();
            oPaymentAccount = MapObject(oReader);
            return oPaymentAccount;
        }

        private List<PaymentAccount> CreateObjects(IDataReader oReader)
        {
            List<PaymentAccount> oPaymentAccount = new List<PaymentAccount>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PaymentAccount oItem = CreateObject(oHandler);
                oPaymentAccount.Add(oItem);
            }
            return oPaymentAccount;
        }

        #endregion

        #region Interface implementation
        public PaymentAccountService() { }

        public PaymentAccount Save(PaymentAccount oPaymentAccount, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPaymentAccount.PaymentAccountID <= 0)
                {
                    reader = PaymentAccountDA.InsertUpdate(tc, oPaymentAccount, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = PaymentAccountDA.InsertUpdate(tc, oPaymentAccount, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPaymentAccount = new PaymentAccount();
                    oPaymentAccount = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPaymentAccount = new PaymentAccount();
                oPaymentAccount.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save PaymentAccount. Because of " + e.Message, e);
                #endregion
            }
            return oPaymentAccount;
        }
        public PaymentAccount SetDefault(PaymentAccount oPaymentAccount, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                PaymentAccountDA.SetDefault(tc, oPaymentAccount);
                reader = PaymentAccountDA.Get(tc, oPaymentAccount.PaymentAccountID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPaymentAccount = new PaymentAccount();
                    oPaymentAccount = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPaymentAccount = new PaymentAccount();
                oPaymentAccount.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save PaymentAccount. Because of " + e.Message, e);
                #endregion
            }
            return oPaymentAccount;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PaymentAccount oPaymentAccount = new PaymentAccount();
                oPaymentAccount.PaymentAccountID = id;
                DBTableReferenceDA.HasReference(tc, "PaymentAccount", id);
                PaymentAccountDA.Delete(tc, oPaymentAccount, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete PaymentAccount. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public PaymentAccount Get(int id, int nUserId)
        {
            PaymentAccount oAccountHead = new PaymentAccount();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PaymentAccountDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get PaymentAccount", e);
                #endregion
            }

            return oAccountHead;
        }



        public List<PaymentAccount> Gets(int nUserID)
        {
            List<PaymentAccount> oPaymentAccount = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PaymentAccountDA.Gets(tc);
                oPaymentAccount = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PaymentAccount", e);
                #endregion
            }

            return oPaymentAccount;
        }
        public List<PaymentAccount> Gets(string sSQL, int nUserID)
        {
            List<PaymentAccount> oPaymentAccount = new List<PaymentAccount>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if (sSQL == "")
                {
                    sSQL = "Select * from View_PaymentAccount where PaymentAccountID in (1,2,80,272,347,370,60,45)";
                }
                reader = PaymentAccountDA.Gets(tc, sSQL);
                oPaymentAccount = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PaymentAccount", e);
                #endregion
            }

            return oPaymentAccount;
        }

        public List<PaymentAccount> GetsByBU(int nBUID, int nUserID)
        {
            List<PaymentAccount> oPaymentAccounts = new List<PaymentAccount>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PaymentAccountDA.GetsByBU(tc, nBUID);
                oPaymentAccounts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPaymentAccounts = new List<PaymentAccount>();
                PaymentAccount oPaymentAccount = new PaymentAccount();
                oPaymentAccount.ErrorMessage = e.Message;
                oPaymentAccounts.Add(oPaymentAccount);
                #endregion
            }
            return oPaymentAccounts;
        }
        #endregion
    }
}
