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
    public class ReceivedChequeService : MarshalByRefObject, IReceivedChequeService
    {
        #region Private functions and declaration
        private ReceivedCheque MapObject(NullHandler oReader)
        {
            ReceivedCheque oReceivedCheque = new ReceivedCheque();
            oReceivedCheque.ReceivedChequeID = oReader.GetInt32("ReceivedChequeID");
            oReceivedCheque.ContractorID = oReader.GetInt32("ContractorID");
            oReceivedCheque.IssueDate = oReader.GetDateTime("IssueDate");
            oReceivedCheque.ChequeStatus = (EnumReceivedChequeStatus)oReader.GetInt16("ChequeStatus");
            oReceivedCheque.ChequeNo = oReader.GetString("ChequeNo");
            oReceivedCheque.TransactionType = (EnumTransactionType)oReader.GetInt16("TransactionType");
            oReceivedCheque.ChequeDate = oReader.GetDateTime("ChequeDate");
            oReceivedCheque.Amount = oReader.GetDouble("Amount");
            oReceivedCheque.BankName = oReader.GetString("BankName");
            oReceivedCheque.BranchName = oReader.GetString("BranchName");
            oReceivedCheque.AccountNo = oReader.GetString("AccountNo");
            oReceivedCheque.Remarks = oReader.GetString("Remarks");
            oReceivedCheque.ReceivedAccontID = oReader.GetInt32("ReceivedAccontID");
            oReceivedCheque.AuthorizedBy = oReader.GetInt32("AuthorizedBy");
            oReceivedCheque.ReceivedAccontNo = oReader.GetString("ReceivedAccontNo");
            oReceivedCheque.AuthorizedByName = oReader.GetString("AuthorizedByName");
            oReceivedCheque.ContractorName = oReader.GetString("ContractorName");
            oReceivedCheque.SubLedgerID = oReader.GetInt32("SubLedgerID");
            oReceivedCheque.SubLedgerName = oReader.GetString("SubLedgerName");
            oReceivedCheque.SubLedgerCode = oReader.GetString("SubLedgerCode");
            oReceivedCheque.SubLedgerNameCode = oReader.GetString("SubLedgerNameCode");
            oReceivedCheque.MoneyReceiptNo = oReader.GetString("MoneyReceiptNo");
            oReceivedCheque.MoneyReceiptDate = oReader.GetDateTime("MoneyReceiptDate");
            oReceivedCheque.BillNumber = oReader.GetString("BillNumber");
            oReceivedCheque.ProductDetails = oReader.GetString("ProductDetails");
            return oReceivedCheque;
        }

        private ReceivedCheque CreateObject(NullHandler oReader)
        {
            ReceivedCheque oReceivedCheque = new ReceivedCheque();
            oReceivedCheque = MapObject(oReader);
            return oReceivedCheque;
        }

        private List<ReceivedCheque> CreateObjects(IDataReader oReader)
        {
            List<ReceivedCheque> oReceivedCheque = new List<ReceivedCheque>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ReceivedCheque oItem = CreateObject(oHandler);
                oReceivedCheque.Add(oItem);
            }
            return oReceivedCheque;
        }

        #endregion

        #region Interface implementation
        public ReceivedChequeService() { }

        public ReceivedCheque Save(ReceivedCheque oReceivedCheque, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oReceivedCheque.ReceivedChequeID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ReceivedCheque, EnumRoleOperationType.Add);
                    reader = ReceivedChequeDA.InsertUpdate(tc, oReceivedCheque, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ReceivedCheque, EnumRoleOperationType.Edit);
                    reader = ReceivedChequeDA.InsertUpdate(tc, oReceivedCheque, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oReceivedCheque = new ReceivedCheque();
                    oReceivedCheque = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ReceivedCheque. Because of " + e.Message, e);
                #endregion
            }
            return oReceivedCheque;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ReceivedCheque oReceivedCheque = new ReceivedCheque();
                oReceivedCheque.ReceivedChequeID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ReceivedCheque, EnumRoleOperationType.Delete);
                ReceivedChequeDA.Delete(tc, oReceivedCheque, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ReceivedCheque. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public ReceivedCheque Get(int id, int nUserId)
        {
            ReceivedCheque oAccountHead = new ReceivedCheque();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ReceivedChequeDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ReceivedCheque", e);
                #endregion
            }

            return oAccountHead;
        }

       

        public List<ReceivedCheque> Gets(int nUserID)
        {
            List<ReceivedCheque> oReceivedCheque = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ReceivedChequeDA.Gets(tc);
                oReceivedCheque = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ReceivedCheque", e);
                #endregion
            }

            return oReceivedCheque;
        }

        public List<ReceivedCheque> Gets(string sSQL, int nUserID)
        {
            List<ReceivedCheque> oReceivedCheque = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ReceivedChequeDA.Gets(tc,sSQL);
                oReceivedCheque = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ReceivedCheque", e);
                #endregion
            }

            return oReceivedCheque;
        }
        public List<ReceivedCheque> GetsForSuggestion(ReceivedCheque oReceivedCheque, int nUserID)
        {
            List<ReceivedCheque> oReceivedCheques = new List<ReceivedCheque>();
            oReceivedCheque.BankName = oReceivedCheque.BankName == null ? "" : oReceivedCheque.BankName;
            oReceivedCheque.AccountNo = oReceivedCheque.AccountNo == null ? "" : oReceivedCheque.AccountNo;
            oReceivedCheque.BranchName = oReceivedCheque.BranchName == null ? "" : oReceivedCheque.BranchName;

            string sSQL = oReceivedCheque.BankName != "" ? "SELECT  DISTINCT RC.BankName FROM View_ReceivedCheque AS RC WHERE RC.BankName LIKE '%" + oReceivedCheque.BankName + "%'" :
                oReceivedCheque.BranchName != "" ? "SELECT  DISTINCT RC.BranchName FROM View_ReceivedCheque AS RC WHERE RC.BranchName LIKE '%" + oReceivedCheque.BranchName + "%'" :
                oReceivedCheque.AccountNo != "" ? "SELECT  DISTINCT RC.AccountNo FROM View_ReceivedCheque AS RC WHERE RC.AccountNo LIKE '%" + oReceivedCheque.AccountNo + "%'" : 
                "SELECT * FROM View_ReceivedCheque";
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                
                reader = ReceivedChequeDA.Gets(tc, sSQL);
                 NullHandler oReader = new NullHandler(reader);
                //oReceivedCheque = CreateObjects(reader);
                while (reader.Read())
                {
                    ReceivedCheque oRC = new ReceivedCheque();
                    if (oReceivedCheque.BankName != "") { oRC.BankName = oReader.GetString("BankName"); oReceivedCheques.Add(oRC); }
                    if (oReceivedCheque.BranchName != "") { oRC.BranchName = oReader.GetString("BranchName"); oReceivedCheques.Add(oRC); }
                    if (oReceivedCheque.AccountNo != "") { oRC.AccountNo = oReader.GetString("AccountNo"); oReceivedCheques.Add(oRC); }
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
                throw new ServiceException("Failed to Get ReceivedCheque", e);
                #endregion
            }

            return oReceivedCheques;
        }

        public ReceivedCheque UpdateReceivedChequeStatus(ReceivedCheque oReceivedCheque, ReceivedChequeHistory oReceivedChequeHistory, int nCurrentUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader;
                reader = ReceivedChequeDA.UpdateReceivedChequeStatus(tc, oReceivedCheque, oReceivedChequeHistory, nCurrentUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oReceivedCheque = CreateObject(oReader);
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
                throw new ServiceException("Failed to Update Cheque Status. Because of " + e.Message, e);
                #endregion
            }
            return oReceivedCheque;
        }
        #endregion
    }   
}