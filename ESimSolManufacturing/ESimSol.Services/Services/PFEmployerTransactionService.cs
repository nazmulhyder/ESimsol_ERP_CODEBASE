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
    public class PFEmployerTransactionService : MarshalByRefObject, IPFEmployerTransactionService
    {
        #region Private functions and declaration
        private PFEmployerTransaction MapObject(NullHandler oReader)
        {
            PFEmployerTransaction oPFEmployerTransaction = new PFEmployerTransaction();
            oPFEmployerTransaction.PETID = oReader.GetInt32("PETID");
            oPFEmployerTransaction.Year = oReader.GetInt32("Year");
            oPFEmployerTransaction.Month = oReader.GetInt32("Month");
            oPFEmployerTransaction.PETType = (EnumPETType)oReader.GetInt16("PETType");
            oPFEmployerTransaction.Amount = oReader.GetDouble("Amount");
            oPFEmployerTransaction.Note = oReader.GetString("Note");
            oPFEmployerTransaction.OperatorBy = oReader.GetInt32("OperatorBy");
            oPFEmployerTransaction.OperateDate = oReader.GetDateTime("OperateDate");
            oPFEmployerTransaction.ApproveBy = oReader.GetInt32("ApproveBy");
            oPFEmployerTransaction.ApproveByDate = oReader.GetDateTime("ApproveByDate");
            oPFEmployerTransaction.DistributedBy = oReader.GetInt32("DistributedBy");
            oPFEmployerTransaction.DistributedDate = oReader.GetDateTime("DistributedDate");
            oPFEmployerTransaction.OperateByName = oReader.GetString("OperateByName");
            oPFEmployerTransaction.ApproveByName = oReader.GetString("ApproveByName");
            oPFEmployerTransaction.DistributedByName = oReader.GetString("DistributedByName");
            return oPFEmployerTransaction;
        }

        public static PFEmployerTransaction CreateObject(NullHandler oReader)
        {
            PFEmployerTransactionService oPFEmployerTransactionService = new PFEmployerTransactionService();
            PFEmployerTransaction oPFEmployerTransaction = oPFEmployerTransactionService.MapObject(oReader);
            return oPFEmployerTransaction;
        }

        private List<PFEmployerTransaction> CreateObjects(IDataReader oReader)
        {
            List<PFEmployerTransaction> oPFEmployerTransaction = new List<PFEmployerTransaction>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PFEmployerTransaction oItem = CreateObject(oHandler);
                oPFEmployerTransaction.Add(oItem);
            }
            return oPFEmployerTransaction;
        }

        #endregion

        #region Interface implementation
        public PFEmployerTransactionService() { }

        public PFEmployerTransaction IUD(PFEmployerTransaction oPFEmployerTransaction, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = PFEmployerTransactionDA.IUD(tc, oPFEmployerTransaction, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPFEmployerTransaction = new PFEmployerTransaction();
                    oPFEmployerTransaction = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oPFEmployerTransaction = new PFEmployerTransaction();
                    oPFEmployerTransaction.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oPFEmployerTransaction = new PFEmployerTransaction();
                oPFEmployerTransaction.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oPFEmployerTransaction;
        }

        public PFEmployerTransaction Distribute(int EnumDBOperation, int nPETID, Int64 nUserId)
        {
            PFEmployerTransaction oPFDistribution = new PFEmployerTransaction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PFEmployerTransactionDA.Distribute(tc, EnumDBOperation, nPETID, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPFDistribution = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPFDistribution = new PFEmployerTransaction();
                oPFDistribution.ErrorMessage = "Failed to distribute.";
                #endregion
            }

            return oPFDistribution;
        }

        public PFEmployerTransaction Get(int nPETID, Int64 nUserId)
        {
            PFEmployerTransaction oPFEmployerTransaction = new PFEmployerTransaction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PFEmployerTransactionDA.Get(tc, nPETID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPFEmployerTransaction = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPFEmployerTransaction = new PFEmployerTransaction();
                oPFEmployerTransaction.ErrorMessage = "Failed to get employer transaction.";
                #endregion
            }

            return oPFEmployerTransaction;
        }

        //distribute
       


        public List<PFEmployerTransaction> Gets(string sSQL, Int64 nUserID)
        {
            List<PFEmployerTransaction> oPFEmployerTransactions = new List<PFEmployerTransaction>(); ;
            PFEmployerTransaction oPFEmployerTransaction = new PFEmployerTransaction();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PFEmployerTransactionDA.Gets(tc, sSQL);
                oPFEmployerTransactions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPFEmployerTransaction = new PFEmployerTransaction();
                oPFEmployerTransaction.ErrorMessage = "Failed to get employer transaction.";
                oPFEmployerTransactions.Add(oPFEmployerTransaction);
                #endregion
            }

            return oPFEmployerTransactions;
        }
        #endregion
    }
}