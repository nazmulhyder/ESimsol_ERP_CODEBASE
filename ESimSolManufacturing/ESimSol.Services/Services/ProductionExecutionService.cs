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
    public class ProductionExecutionService : MarshalByRefObject, IProductionExecutionService
    {
        #region Private functions and declaration
        private ProductionExecution MapObject(NullHandler oReader)
        {
            ProductionExecution oProductionExecution = new ProductionExecution();
            oProductionExecution.ProductionExecutionID = oReader.GetInt32("ProductionExecutionID");
            oProductionExecution.ProductionProcedureID = oReader.GetInt32("ProductionProcedureID");
            oProductionExecution.ProductionStepID = oReader.GetInt32("ProductionStepID");
            oProductionExecution.ExecutionQty = oReader.GetDouble("ExecutionQty");
            oProductionExecution.YetToExecution = oReader.GetDouble("YetToExecution");
            oProductionExecution.ProductID = oReader.GetInt32("ProductID");
            oProductionExecution.ExecutionDate = oReader.GetDateTime("ExecutionDate");
            oProductionExecution.StepName = oReader.GetString("StepName");
            oProductionExecution.StepShortName = oReader.GetString("StepShortName");
            oProductionExecution.ProductCode = oReader.GetString("ProductCode");
            oProductionExecution.ProductName = oReader.GetString("ProductName");
            oProductionExecution.Sequence = oReader.GetInt32("Sequence");
            oProductionExecution.ProductionStepType = (EnumProductionStepType)oReader.GetInt32("ProductionStepType");
            oProductionExecution.ProductionStepTypeInInt = oReader.GetInt32("ProductionStepType");
             
            return oProductionExecution;
        }

        private ProductionExecution CreateObject(NullHandler oReader)
        {
            ProductionExecution oProductionExecution = new ProductionExecution();
            oProductionExecution = MapObject(oReader);
            return oProductionExecution;
        }

        private List<ProductionExecution> CreateObjects(IDataReader oReader)
        {
            List<ProductionExecution> oProductionExecution = new List<ProductionExecution>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionExecution oItem = CreateObject(oHandler);
                oProductionExecution.Add(oItem);
            }
            return oProductionExecution;
        }

        #endregion

        #region Interface implementation
        public ProductionExecutionService() { }

        public ProductionExecution Save(ProductionExecution oProductionExecution, Int64 nUserId)
        {
            TransactionContext tc = null;
            List<PETransaction> oPETransactions = new List<PETransaction>();
            oPETransactions = oProductionExecution.PETransactions;
            string sPETransactionIDs = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oProductionExecution.ProductionExecutionID <= 0)
                {
                    reader = ProductionExecutionDA.InsertUpdate(tc, oProductionExecution, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = ProductionExecutionDA.InsertUpdate(tc, oProductionExecution, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionExecution = new ProductionExecution();
                    oProductionExecution = CreateObject(oReader);
                }
                reader.Close();
                #region ProductionExecution 
                if (oPETransactions != null)
                {
                    foreach (PETransaction oItem in oPETransactions)
                    {
                        IDataReader readerdetail;
                        oItem.ProductionExecutionID = oProductionExecution.ProductionExecutionID;
                        if (oItem.PETransactionID <= 0)
                        {
                            readerdetail = PETransactionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                        }
                        else
                        {
                            readerdetail = PETransactionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sPETransactionIDs = sPETransactionIDs + oReaderDetail.GetString("PETransactionID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sPETransactionIDs.Length > 0)
                    {
                        sPETransactionIDs = sPETransactionIDs.Remove(sPETransactionIDs.Length - 1, 1);
                    }
                }
                PETransaction oPETransaction = new PETransaction();
                oPETransaction.ProductionExecutionID = oProductionExecution.ProductionExecutionID;
                PETransactionDA.Delete(tc, oPETransaction, EnumDBOperation.Delete, nUserId);
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oProductionExecution.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ProductionExecution. Because of " + e.Message, e);
                #endregion
            }
            return oProductionExecution;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProductionExecution oProductionExecution = new ProductionExecution();
                oProductionExecution.ProductionExecutionID = id;
                ProductionExecutionDA.Delete(tc, oProductionExecution, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ProductionExecution oProductionExecution = new ProductionExecution();
                oProductionExecution.ErrorMessage = e.Message;
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ProductionExecution. Because of " + e.Message, e);
                #endregion
            }
            return "Delete sucessfully";
        }

        public ProductionExecution Get(int id, Int64 nUserId)
        {
            ProductionExecution oAccountHead = new ProductionExecution();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductionExecutionDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ProductionExecution", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<ProductionExecution> Gets(Int64 nUserId)
        {
            List<ProductionExecution> oProductionExecutions = new List<ProductionExecution>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionExecutionDA.Gets(tc);
                oProductionExecutions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionExecution", e);
                #endregion
            }

            return oProductionExecutions;
        }
        public List<ProductionExecution> Gets(int nPSID, Int64 nUserId)
        {
            List<ProductionExecution> oProductionExecutions = new List<ProductionExecution>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionExecutionDA.Gets(tc, nPSID);
                oProductionExecutions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionExecution", e);
                #endregion
            }

            return oProductionExecutions;
        }

        public List<ProductionExecution> Gets(string sSQL, Int64 nUserId)
        {
            List<ProductionExecution> oProductionExecution = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionExecutionDA.Gets(tc, sSQL);
                oProductionExecution = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionExecution", e);
                #endregion
            }

            return oProductionExecution;
        }
        #endregion
    }
    
   
}
