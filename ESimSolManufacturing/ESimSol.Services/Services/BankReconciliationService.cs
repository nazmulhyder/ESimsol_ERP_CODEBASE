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
    public class BankReconciliationService : MarshalByRefObject, IBankReconciliationService
    {
        #region Private functions and declaration
        private BankReconciliation MapObject(NullHandler oReader)
        {
            BankReconciliation oBankReconciliation = new BankReconciliation();
            oBankReconciliation.BankReconciliationID = oReader.GetInt32("BankReconciliationID");
            oBankReconciliation.SubledgerID = oReader.GetInt32("SubledgerID");
            oBankReconciliation.VoucherDetailID = oReader.GetInt32("VoucherDetailID");
            oBankReconciliation.CCTID = oReader.GetInt32("CCTID");
            oBankReconciliation.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oBankReconciliation.ReconcilHeadID = oReader.GetInt32("ReconcilHeadID");
            oBankReconciliation.ReconcilDate = oReader.GetDateTime("ReconcilDate");
            oBankReconciliation.ReconcilRemarks = oReader.GetString("ReconcilRemarks");
            oBankReconciliation.IsDebit = oReader.GetBoolean("IsDebit");
            oBankReconciliation.Amount = oReader.GetDouble("Amount");
            oBankReconciliation.ReconcilStatus = (EnumReconcilStatus)oReader.GetInt32("ReconcilStatus");
            oBankReconciliation.ReconcilStatusInt = oReader.GetInt32("ReconcilStatus");
            oBankReconciliation.ReconcilBy = oReader.GetInt32("ReconcilBy");
            oBankReconciliation.SubledgerCode = oReader.GetString("SubledgerCode");
            oBankReconciliation.SubledgerName = oReader.GetString("SubledgerName");
            oBankReconciliation.AccountCode = oReader.GetString("AccountCode");
            oBankReconciliation.AccountHeadName = oReader.GetString("AccountHeadName");
            oBankReconciliation.RCAHCode = oReader.GetString("RCAHCode");
            oBankReconciliation.RCAHName = oReader.GetString("RCAHName");
            oBankReconciliation.ReverseHead = oReader.GetString("ReverseHead");
            oBankReconciliation.ReconcilByName = oReader.GetString("ReconcilByName");
            oBankReconciliation.VoucherID = oReader.GetInt32("VoucherID");
            oBankReconciliation.VoucherDate = oReader.GetDateTime("VoucherDate");
            oBankReconciliation.VoucherNo = oReader.GetString("VoucherNo");
            oBankReconciliation.DebitAmount = oReader.GetDouble("DebitAmount");
            oBankReconciliation.CreditAmount = oReader.GetDouble("CreditAmount");
            oBankReconciliation.CurrentBalance = oReader.GetDouble("CurrentBalance");
            oBankReconciliation.OpeningBalance = oReader.GetDouble("CurrentBalance");
            oBankReconciliation.ReverseHeadID = oReader.GetInt32("ReverseHeadID");
            oBankReconciliation.ReverseHeadIsLedger = oReader.GetBoolean("ReverseHeadIsLedger");
            oBankReconciliation.ReverseHeadName = oReader.GetString("ReverseHeadName");
            oBankReconciliation.DrCAmount = oReader.GetDouble("DrCAmount");
            oBankReconciliation.CrCAmount = oReader.GetDouble("CrCAmount");
            oBankReconciliation.CUSymbol = oReader.GetString("CUSymbol");
            return oBankReconciliation;
        }
        private BankReconciliation CreateObject(NullHandler oReader)
        {
            BankReconciliation oBankReconciliation = new BankReconciliation();
            oBankReconciliation = MapObject(oReader);
            return oBankReconciliation;
        }
        private List<BankReconciliation> CreateObjects(IDataReader oReader)
        {
            List<BankReconciliation> oBankReconciliation = new List<BankReconciliation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BankReconciliation oItem = CreateObject(oHandler);
                oBankReconciliation.Add(oItem);
            }
            return oBankReconciliation;
        }
        #endregion

        #region Interface implementation
        public BankReconciliationService() { }
        public BankReconciliation Save(BankReconciliation oBankReconciliation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oBankReconciliation.ReconcilHeadID = oBankReconciliation.AccountHeadID;
                if (oBankReconciliation.BankReconciliationID <= 0)
                {
                    reader = BankReconciliationDA.InsertUpdate(tc, oBankReconciliation, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = BankReconciliationDA.InsertUpdate(tc, oBankReconciliation, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBankReconciliation = new BankReconciliation();
                    oBankReconciliation = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oBankReconciliation = new BankReconciliation();
                oBankReconciliation.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save BankReconciliation. Because of " + e.Message, e);
                #endregion
            }
            return oBankReconciliation;
        }
        public BankReconciliation UpdateRemarks(BankReconciliation oBankReconciliation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                BankReconciliationDA.UpdateRemarks(tc, oBankReconciliation);
                reader = BankReconciliationDA.Get(tc, oBankReconciliation.BankReconciliationID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBankReconciliation = new BankReconciliation();
                    oBankReconciliation = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oBankReconciliation = new BankReconciliation();
                oBankReconciliation.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save BankReconciliation. Because of " + e.Message, e);
                #endregion
            }
            return oBankReconciliation;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BankReconciliation oBankReconciliation = new BankReconciliation();
                oBankReconciliation.BankReconciliationID = id;
                BankReconciliationDA.Delete(tc, oBankReconciliation, EnumDBOperation.Delete, nUserId);
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

        public string AuthorizeBankReconciliation(List<BankReconciliation> oBankReconciliations, Int64 nUserID)
        {
            TransactionContext tc = null;     
            VoucherHistory oVoucherHistory = new VoucherHistory();
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (BankReconciliation oBankReconciliation in oBankReconciliations)
                {                   
                    BankReconciliationDA.ApprovedReconciliation(tc, oBankReconciliation.BankReconciliationID, nUserID);
                }
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
            return Global.SessionParamSetMessage;
        }

        public BankReconciliation Get(int id, Int64 nUserId)
        {
            BankReconciliation oBankReconciliation = new BankReconciliation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = BankReconciliationDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBankReconciliation = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get BankReconciliation", e);
                #endregion
            }
            return oBankReconciliation;
        }
        public BankReconciliation GetLastTransaction(int nSubledgerID, Int64 nUserId)
        {
            BankReconciliation oBankReconciliation = new BankReconciliation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = BankReconciliationDA.GetLastTransaction(tc, nSubledgerID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBankReconciliation = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get BankReconciliation", e);
                #endregion
            }
            return oBankReconciliation;
        }
        public List<BankReconciliation> Gets(Int64 nUserID)
        {
            List<BankReconciliation> oBankReconciliations = new List<BankReconciliation>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankReconciliationDA.Gets(tc);
                oBankReconciliations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankReconciliation", e);
                #endregion
            }
            return oBankReconciliations;
        }
        public List<BankReconciliation> Gets(string sSQL, Int64 nUserID)
        {
            List<BankReconciliation> oBankReconciliations = new List<BankReconciliation>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BankReconciliationDA.Gets(tc, sSQL);
                oBankReconciliations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankReconciliation", e);
                #endregion
            }
            return oBankReconciliations;
        }
        public List<BankReconciliation> PrepareBankReconciliation(BankReconciliation oBankReconciliation, Int64 nUserID)
        {
            List<BankReconciliation> oBankReconciliations = new List<BankReconciliation>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BankReconciliationDA.PrepareBankReconciliation(tc, oBankReconciliation);
                oBankReconciliations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankReconciliation", e);
                #endregion
            }
            return oBankReconciliations;
        }
        public List<BankReconciliation> MultiSave(BankReconciliation oBankReconciliation, Int64 nUserID)
        {
            List<BankReconciliation> oBankReconciliations = new List<BankReconciliation>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                #region Save Bank Reconciliation
                foreach (BankReconciliation oItem in oBankReconciliation.BankReconciliations)
                {
                    oItem.ReconcilHeadID = oBankReconciliation.ReconcilHeadID;
                    IDataReader readersave;
                    if (oItem.BankReconciliationID <= 0)
                    {                        
                        readersave = BankReconciliationDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readersave = BankReconciliationDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    readersave.Close();
                }
                #endregion
                                
                IDataReader reader = null;
                oBankReconciliation.ReconcileDataType = EnumReconcileDataType.ReconcileDone;
                reader = BankReconciliationDA.PrepareBankReconciliation(tc, oBankReconciliation);
                oBankReconciliations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oBankReconciliation = new BankReconciliation();
                oBankReconciliation.ErrorMessage = e.Message;
                oBankReconciliations = new List<BankReconciliation>();
                oBankReconciliations.Add(oBankReconciliation);
                
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get BankReconciliation", e);
                #endregion
            }
            return oBankReconciliations;
        }
        #endregion
    }
}