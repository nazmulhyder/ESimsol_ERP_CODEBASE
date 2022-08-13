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

    public class StatementSetupService : MarshalByRefObject, IStatementSetupService
    {
        #region Private functions and declaration
        private StatementSetup MapObject(NullHandler oReader)
        {
            StatementSetup oStatementSetup = new StatementSetup();
            oStatementSetup.StatementSetupID = oReader.GetInt32("StatementSetupID");
            oStatementSetup.StatementSetupName = oReader.GetString("StatementSetupName");
            oStatementSetup.AccountsHeadDefineNature = (EnumAccountsHeadDefineNature)oReader.GetInt16("AccountsHeadDefineNature");
            oStatementSetup.Note = oReader.GetString("Note");            
            return oStatementSetup;
        }
        private StatementSetup CreateObject(NullHandler oReader)
        {
            StatementSetup oStatementSetup = new StatementSetup();
            oStatementSetup = MapObject(oReader);
            return oStatementSetup;
        }

        private List<StatementSetup> CreateObjects(IDataReader oReader)
        {
            List<StatementSetup> oStatementSetups = new List<StatementSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                StatementSetup oItem = CreateObject(oHandler);
                oStatementSetups.Add(oItem);
            }
            return oStatementSetups;
        }

        #endregion

        #region Interface implementation
        public StatementSetupService() { }

        public StatementSetup Save(StatementSetup oStatementSetup, int nUserID)
        {
            TransactionContext tc = null;
            List<OperationCategorySetup> oOperationCategorySetups = new List<OperationCategorySetup>();
            List<LedgerBreakDown> oLedgerBreakDowns = new List<LedgerBreakDown>();
            LedgerBreakDown oLedgerBreakDown = new LedgerBreakDown();
            OperationCategorySetup oOperationCategorySetup = new OperationCategorySetup();
            LedgerGroupSetup oLedgerGroupSetup = new LedgerGroupSetup();
            oOperationCategorySetups = oStatementSetup.OperationCategorySetups;
            oLedgerBreakDowns = oStatementSetup.LedgerBreakDowns;

            string sOperationCategorySetupIDs = "";
            string sLedgerGroupSetupIDs = "";
            string sLedgerBreakDownIDs = "";
            try
            {

                tc = TransactionContext.Begin(true);
                #region Statement Setup
                IDataReader reader;
                if (oStatementSetup.StatementSetupID <= 0)
                {
                    reader = StatementSetupDA.InsertUpdate(tc, oStatementSetup, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = StatementSetupDA.InsertUpdate(tc, oStatementSetup, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oStatementSetup = new StatementSetup();
                    oStatementSetup = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Ledger Break Down for Statement SetUp
                if (oLedgerBreakDowns != null)
                {
                    foreach (LedgerBreakDown oItem in oLedgerBreakDowns)
                    {
                        IDataReader readerSetupBreakDown;
                        oItem.ReferenceID = oStatementSetup.StatementSetupID;                        
                        if (oItem.LedgerBreakDownID <= 0)
                        {
                            readerSetupBreakDown = LedgerBreakDownDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "", true);
                        }
                        else
                        {
                            readerSetupBreakDown = LedgerBreakDownDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "", true);
                        }
                        NullHandler oReaderSetupBreakDown = new NullHandler(readerSetupBreakDown);
                        if (readerSetupBreakDown.Read())
                        {
                            sLedgerBreakDownIDs = sLedgerBreakDownIDs + oReaderSetupBreakDown.GetString("LedgerBreakDownID") + ",";
                        }
                        readerSetupBreakDown.Close();
                    }
                    if (sLedgerBreakDownIDs.Length > 0)
                    {
                        sLedgerBreakDownIDs = sLedgerBreakDownIDs.Remove(sLedgerBreakDownIDs.Length - 1, 1);
                    }
                    oLedgerBreakDown = new LedgerBreakDown();
                    oLedgerBreakDown.ReferenceID = oStatementSetup.StatementSetupID;                    
                    LedgerBreakDownDA.Delete(tc, oLedgerBreakDown, EnumDBOperation.Delete, nUserID, sLedgerBreakDownIDs, true);
                    sLedgerBreakDownIDs = "";
                }

                #endregion

                #region Operation Category Setup with ledger group and Ledger BrekDown
                if (oOperationCategorySetups != null)
                {
                    foreach (OperationCategorySetup oItem in oOperationCategorySetups)
                    {
                        #region Operation Category Setup
                        IDataReader readerOperationCategory;
                        int nOperationCategorySetupID = 0;
                        oItem.StatementSetupID = oStatementSetup.StatementSetupID;                        
                        if (oItem.OperationCategorySetupID <= 0)
                        {
                            readerOperationCategory = OperationCategorySetupDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerOperationCategory = OperationCategorySetupDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderOperationCategorySetup = new NullHandler(readerOperationCategory);
                        if (readerOperationCategory.Read())
                        {
                            nOperationCategorySetupID = oReaderOperationCategorySetup.GetInt32("OperationCategorySetupID");
                            sOperationCategorySetupIDs = sOperationCategorySetupIDs + oReaderOperationCategorySetup.GetString("OperationCategorySetupID") + ",";
                        }
                        readerOperationCategory.Close();
                        #endregion

                        #region ledger Group
                        if (oItem.LedgerGroupSetups != null)
                        {
                            foreach (LedgerGroupSetup oLGSItem in oItem.LedgerGroupSetups)
                            {
                                IDataReader readerLedgerGroupSetup;
                                int nLedgerGroupSetupID = 0;
                                oLGSItem.OCSID = nOperationCategorySetupID;                                
                                if (oLGSItem.LedgerGroupSetupID <= 0)
                                {
                                    readerLedgerGroupSetup = LedgerGroupSetupDA.InsertUpdate(tc, oLGSItem, EnumDBOperation.Insert, nUserID, "");
                                }
                                else
                                {
                                    readerLedgerGroupSetup = LedgerGroupSetupDA.InsertUpdate(tc, oLGSItem, EnumDBOperation.Update, nUserID, "");
                                }
                                NullHandler oReaderLGS = new NullHandler(readerLedgerGroupSetup);
                                if (readerLedgerGroupSetup.Read())
                                {
                                    nLedgerGroupSetupID = oReaderLGS.GetInt32("LedgerGroupSetupID");
                                    sLedgerGroupSetupIDs = sLedgerGroupSetupIDs + oReaderLGS.GetString("LedgerGroupSetupID") + ",";
                                }
                                readerLedgerGroupSetup.Close();

                                #region Ledger BreakDown for Ledger Group
                                if (oLGSItem.LedgerBreakDowns != null)
                                {
                                    LedgerBreakDownDA.DeleteLedgerBreakdown(tc, nLedgerGroupSetupID);
                                    foreach (LedgerBreakDown oLBDn in oLGSItem.LedgerBreakDowns)
                                    {
                                        IDataReader readerLBD;
                                        oLBDn.ReferenceID = nLedgerGroupSetupID;                                        
                                        readerLBD = LedgerBreakDownDA.InsertUpdate(tc, oLBDn, EnumDBOperation.Insert, nUserID, "", false);                                      
                                        NullHandler oReaderDetail = new NullHandler(readerLBD);
                                        if (readerLBD.Read())
                                        {
                                            
                                        }
                                        readerLBD.Close();
                                    }
                                }
                                #endregion
                            }
                            if (sLedgerGroupSetupIDs.Length > 0)
                            {
                                sLedgerGroupSetupIDs = sLedgerGroupSetupIDs.Remove(sLedgerGroupSetupIDs.Length - 1, 1);
                            }
                            oLedgerGroupSetup = new LedgerGroupSetup();
                            oLedgerGroupSetup.OCSID = nOperationCategorySetupID;                            
                            LedgerGroupSetupDA.Delete(tc, oLedgerGroupSetup, EnumDBOperation.Delete, nUserID, sLedgerGroupSetupIDs);
                            sLedgerGroupSetupIDs = "";
                        }
                        #endregion
                    }

                    if (sOperationCategorySetupIDs.Length > 0)
                    {
                        sOperationCategorySetupIDs = sOperationCategorySetupIDs.Remove(sOperationCategorySetupIDs.Length - 1, 1);
                    }
                    oOperationCategorySetup = new OperationCategorySetup();
                    oOperationCategorySetup.StatementSetupID = oStatementSetup.StatementSetupID;                    
                    OperationCategorySetupDA.Delete(tc, oOperationCategorySetup, EnumDBOperation.Delete, nUserID, sOperationCategorySetupIDs);
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oStatementSetup = new StatementSetup();
                oStatementSetup.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oStatementSetup;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                StatementSetup oStatementSetup = new StatementSetup();
                oStatementSetup.StatementSetupID = id;                
                StatementSetupDA.Delete(tc, oStatementSetup, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                #endregion
            }
            return "Data delete successfully";
        }

        public StatementSetup Get(int id, int nUserId)
        {
            StatementSetup oAccountHead = new StatementSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = StatementSetupDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get StatementSetup", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<StatementSetup> Gets(int nUserID)
        {
            List<StatementSetup> oStatementSetups = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = StatementSetupDA.Gets(tc);
                oStatementSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get StatementSetup", e);
                #endregion
            }

            return oStatementSetups;
        }
        public List<StatementSetup> Gets(string sSQL, int nUserID)
        {
            List<StatementSetup> oStatementSetups = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = StatementSetupDA.Gets(tc, sSQL);
                oStatementSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get StatementSetup", e);
                #endregion
            }

            return oStatementSetups;
        }
        #endregion
    }
}
