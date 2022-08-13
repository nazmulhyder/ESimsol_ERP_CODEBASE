using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{
    public class CashFlowService : MarshalByRefObject, ICashFlowService
    {
        #region Private functions and declaration
        private CashFlow MapObject(NullHandler oReader)
        {
            CashFlow oCashFlow = new CashFlow();
            oCashFlow.CashFlowID = oReader.GetInt32("CashFlowID");
            oCashFlow.CEVoucherDetailID = oReader.GetInt32("CEVoucherDetailID");
            oCashFlow.CEIsDebit = oReader.GetBoolean("CEIsDebit");
            oCashFlow.CEAmount = oReader.GetDouble("CEAmount");
            oCashFlow.CashFlowHeadID = oReader.GetInt32("CashFlowHeadID");
            oCashFlow.VoucherDetailID = oReader.GetInt32("VoucherDetailID");
            oCashFlow.IsDebit = oReader.GetBoolean("IsDebit");
            oCashFlow.Amount = oReader.GetDouble("Amount");
            oCashFlow.BUID = oReader.GetInt32("BUID");
            oCashFlow.AccountHeadCode = oReader.GetString("AccountHeadCode");
            oCashFlow.AccountHeadName = oReader.GetString("AccountHeadName");
            oCashFlow.VoucherID = oReader.GetInt32("VoucherID"); 
            oCashFlow.Narration = oReader.GetString("Narration");
            oCashFlow.VoucherDate = oReader.GetDateTime("VoucherDate");           
            oCashFlow.VoucherNo = oReader.GetString("VoucherNo");
            oCashFlow.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oCashFlow.DisplayCaption = oReader.GetString("DisplayCaption");
            oCashFlow.SubGroupID = oReader.GetInt32("SubGroupID");
            oCashFlow.SubGroupName = oReader.GetString("SubGroupName");
            oCashFlow.SubGroupCode = oReader.GetString("SubGroupCode");
            oCashFlow.AccountHeadID = oReader.GetInt32("AccountHeadID");
            return oCashFlow;
        }

        private CashFlow CreateObject(NullHandler oReader)
        {
            CashFlow oCashFlow = new CashFlow();
            oCashFlow = MapObject(oReader);
            return oCashFlow;
        }

        private List<CashFlow> CreateObjects(IDataReader oReader)
        {
            List<CashFlow> oCashFlow = new List<CashFlow>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CashFlow oItem = CreateObject(oHandler);
                oCashFlow.Add(oItem);
            }
            return oCashFlow;
        }

        #endregion

        #region Interface implementation
        public CashFlowService() { }

        public CashFlow Save(CashFlow oCashFlow, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oCashFlow.CashFlowID <= 0)
                {
                    reader = CashFlowDA.InsertUpdate(tc, oCashFlow, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    reader = CashFlowDA.InsertUpdate(tc, oCashFlow, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCashFlow = new CashFlow();
                    oCashFlow = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save CashFlow. Because of " + e.Message, e);
                #endregion
            }
            return oCashFlow;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);                
                CashFlow oCashFlow = new CashFlow();
                oCashFlow.CashFlowID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.CashFlow, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "CashFlow", id);
                CashFlowDA.Delete(tc, oCashFlow, EnumDBOperation.Delete,nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                return e.Message.Split('!')[0];
                #endregion
            }
            return "deleted";
        }

        public CashFlow Get(int id, Int64 nUserId)
        {
            CashFlow oAccountHead = new CashFlow();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CashFlowDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get CashFlow", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<CashFlow> Gets(Int64 nUserId)
        {
            List<CashFlow> oCashFlow = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CashFlowDA.Gets(tc);
                oCashFlow = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CashFlow", e);
                #endregion
            }

            return oCashFlow;
        }

        public List<CashFlow> UpdateCashFlows(string sCashFlowIDs, int CashFlowHeadID, Int64 nUserId)
        {
            List<CashFlow> oCashFlows = new List<CashFlow>();
            TransactionContext tc = null;
            string sSQL = "SELECT * FROM View_CashFlow WHERE CashFlowID IN (" + sCashFlowIDs + ") ";
            string[] sIDs = sCashFlowIDs.Split(',') ;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                for (int i = 0; i < sIDs.Length;i++ )
                {
                    CashFlowDA.UpdateCashFlow(tc, Convert.ToInt32(sIDs[i]), CashFlowHeadID);
                }
                reader = CashFlowDA.Gets(tc, sSQL);
                oCashFlows = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CashFlow", e);
                #endregion
            }

            return oCashFlows;
        }

        public List<CashFlow> GetsForCashManage(CashFlow oCashFlow, Int64 nUserId)
        {
            List<CashFlow> oCashFlows = new List<CashFlow>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                CashFlowDA.CashFlowManage(tc, oCashFlow, nUserId);
                IDataReader reader = null;
                reader = CashFlowDA.Gets(tc, oCashFlow.ErrorMessage);//Here ErrorMessage Use As a SQL Crraiar
                oCashFlows = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CashFlow", e);
                #endregion
            }

            return oCashFlows;
        }

        public List<CashFlow> Gets(string sSQL, Int64 nUserId)
        {
            List<CashFlow> oCashFlow = new List<CashFlow>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();               
                IDataReader reader = null;
                reader = CashFlowDA.Gets(tc, sSQL);
                oCashFlow = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CashFlow", e);
                #endregion
            }

            return oCashFlow;
        }

        public List<CashFlow> Gets(CashFlow oCashFlow, Int64 nUserId)
        {
            List<CashFlow> oCashFlows = new List<CashFlow>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = CashFlowDA.Gets(tc, oCashFlow);
                oCashFlows = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CashFlow", e);
                #endregion
            }

            return oCashFlows;
        }
        
        public List<CashFlow> GetsByName(string sName,  Int64 nUserId)
        {
            List<CashFlow> oCashFlows = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CashFlowDA.GetsByName(tc, sName );
                oCashFlows = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CashFlows", e);
                #endregion
            }

            return oCashFlows;
        }
        public List<CashFlow> GetCashFlowBreakDowns(CashFlowDmSetup oCashFlowDmSetup,  Int64 nUserId)
        {
            List<CashFlow> oCashFlows = new List<CashFlow>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = CashFlowDA.GetCashFlowBreakDowns(tc, oCashFlowDmSetup);
                oCashFlows = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CashFlow Break Down!", e);
                #endregion
            }
            return oCashFlows;
        }
        #endregion
    } 
}