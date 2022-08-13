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
    public class BudgetService : MarshalByRefObject, IBudgetService
    {
        #region Private functions and declaration
        private Budget MapObject(NullHandler oReader)
        {
            Budget oBudget = new Budget();
            oBudget.BudgetID = oReader.GetInt32("BudgetID");
            oBudget.BudgetNo = oReader.GetString("BudgetNo");
            oBudget.ReviseNo = oReader.GetString("ReviseNo");
            oBudget.IssueDate = oReader.GetDateTime("IssueDate");
            oBudget.AccountingSessionID = oReader.GetInt32("AccountingSessionID");
            oBudget.BudgetType = (EnumBudgetType)oReader.GetInt16("BudgetType");
            oBudget.BudgetStatus = (EnumBudgetStatus)oReader.GetInt16("BudgetStatus");
            oBudget.ApproveBy = oReader.GetInt32("ApproveBy");
            oBudget.Remarks = oReader.GetString("Remarks");
            oBudget.SessionName = oReader.GetString("SessionName");
            oBudget.ApproveByName = oReader.GetString("ApproveByName");
            oBudget.BUID = oReader.GetInt32("BUID");
            oBudget.BUName = oReader.GetString("BUName");
            return oBudget;
        }
        private Budget CreateObject(NullHandler oReader)
        {
            Budget oBudget = new Budget();
            oBudget = MapObject(oReader);
            return oBudget;
        }
        private List<Budget> CreateObjects(IDataReader oReader)
        {
            List<Budget> oBudget = new List<Budget>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Budget oItem = CreateObject(oHandler);
                oBudget.Add(oItem);
            }
            return oBudget;
        }

        #endregion

        #region Interface implementation
        public BudgetService() { }

        public List<Budget> Gets(string sSQL,Int64 nUserID)
        {
            List<Budget> oBudgets = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BudgetDA.Gets(tc,sSQL);
                oBudgets = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Budget", e);
                #endregion
            }
            return oBudgets;
        }
        public Budget Save(Budget oBudget, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<BudgetDetail> oBudgetDetails = new List<BudgetDetail>();
            oBudgetDetails = oBudget.BudgetDetails;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBudget.BudgetID <= 0)
                {
                    reader = BudgetDA.InsertUpdate(tc, oBudget, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = BudgetDA.InsertUpdate(tc, oBudget, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBudget = new Budget();
                    oBudget = CreateObject(oReader);
                }
                reader.Close();

                string sBudgetDetailIDs = "";
                #region Delivery Order Detail Part
                foreach (BudgetDetail oItem in oBudgetDetails)
                {
                    if (oItem.BudgetAmount > 0 && oItem.AccountType== EnumAccountType.Ledger) 
                    {
                        IDataReader readerdetail;
                        oItem.BudgetID = oBudget.BudgetID;
                        if (oItem.BudgetDetailID <= 0)
                        {
                            readerdetail = BudgetDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = BudgetDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sBudgetDetailIDs = sBudgetDetailIDs + oReaderDetail.GetString("BudgetDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                }
                if (sBudgetDetailIDs.Length > 0)
                {
                    sBudgetDetailIDs = sBudgetDetailIDs.Remove(sBudgetDetailIDs.Length - 1, 1);
                }
                BudgetDetail oBudgetDetail = new BudgetDetail();
                oBudgetDetail.BudgetID = oBudget.BudgetID;
                BudgetDetailDA.Delete(tc, oBudgetDetail, EnumDBOperation.Delete, nUserID, sBudgetDetailIDs);
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save Budget. Because of " + e.Message.Split('~')[0], e);
                #endregion
            }
            return oBudget;
        }
        public Budget Revise(Budget oBudget, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<BudgetDetail> oBudgetDetails = new List<BudgetDetail>();
            oBudgetDetails = oBudget.BudgetDetails;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = BudgetDA.Revise(tc, oBudget, EnumDBOperation.Revise, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBudget = new Budget();
                    oBudget = CreateObject(oReader);
                }
                reader.Close();

                string sBudgetDetailIDs = "";
                #region Delivery Order Detail Part
                foreach (BudgetDetail oItem in oBudgetDetails)
                {
                    if (oItem.BudgetAmount > 0 && oItem.AccountType == EnumAccountType.Ledger)
                    {
                        IDataReader readerdetail;
                        oItem.BudgetID = oBudget.BudgetID;
                        if (oItem.BudgetDetailID <= 0)
                        {
                            readerdetail = BudgetDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = BudgetDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sBudgetDetailIDs = sBudgetDetailIDs + oReaderDetail.GetString("BudgetDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                }
                if (sBudgetDetailIDs.Length > 0)
                {
                    sBudgetDetailIDs = sBudgetDetailIDs.Remove(sBudgetDetailIDs.Length - 1, 1);
                }
                BudgetDetail oBudgetDetail = new BudgetDetail();
                oBudgetDetail.BudgetID = oBudget.BudgetID;
                BudgetDetailDA.Delete(tc, oBudgetDetail, EnumDBOperation.Delete, nUserID, sBudgetDetailIDs);
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save Budget. Because of " + e.Message.Split('~')[0], e);
                #endregion
            }
            return oBudget;
        }
        public Budget BudgetStatusChange(Budget oBudget, EnumDBOperation oDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<BudgetDetail> oBudgetDetails = new List<BudgetDetail>();
            oBudgetDetails = oBudget.BudgetDetails;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = BudgetDA.InsertUpdate(tc, oBudget, oDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBudget = new Budget();
                    oBudget = CreateObject(oReader);
                }
                reader.Close();

                string sBudgetDetailIDs = "";
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed Because of " + e.Message.Split('~')[0], e);
                #endregion
            }
            return oBudget;
        }
        
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Budget oBudget = new Budget();
                oBudget.BudgetID = id;
                BudgetDA.Delete(tc, oBudget, EnumDBOperation.Delete, nUserId);
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
        public Budget Get(int id, Int64 nUserId)
        {
            Budget oBudget = new Budget();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = BudgetDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBudget = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Budget", e);
                #endregion
            }
            return oBudget;
        }
        #endregion
    }   
}
