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
    public class PETransactionService : MarshalByRefObject, IPETransactionService
    {
        #region Private functions and declaration
        private PETransaction MapObject(NullHandler oReader)
        {
            PETransaction oPETransaction = new PETransaction();
            oPETransaction.PETransactionID = oReader.GetInt32("PETransactionID");
            oPETransaction.ProductionExecutionID = oReader.GetInt32("ProductionExecutionID");
            oPETransaction.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oPETransaction.Quantity = oReader.GetDouble("Quantity");
            oPETransaction.TransactionDate = oReader.GetDateTime("TransactionDate");
            oPETransaction.UnitName = oReader.GetString("UnitName");
            oPETransaction.UnitSymbol = oReader.GetString("UnitSymbol");
            oPETransaction.EntryByName = oReader.GetString("EntryByName");
            oPETransaction.CycleTime = oReader.GetInt32("CycleTime");
            oPETransaction.Cavity = oReader.GetInt32("Cavity");
            oPETransaction.ShortCounter = oReader.GetInt32("ShortCounter");
            oPETransaction.ProductionHour = oReader.GetInt32("ProductionHour");
            oPETransaction.Remarks = oReader.GetString("Remarks");
            oPETransaction.OperationEmpID = oReader.GetInt32("OperationEmpID");
            oPETransaction.OperationEmpByName = oReader.GetString("OperationEmpByName");
            oPETransaction.ShiftID = oReader.GetInt32("ShiftID");
            oPETransaction.MachineID = oReader.GetInt32("MachineID");
            oPETransaction.BUID = oReader.GetInt32("BUID");
            oPETransaction.OperatorPerMachine = oReader.GetInt32("OperatorPerMachine");
            oPETransaction.ProductionStepID = oReader.GetInt32("ProductionStepID");
            oPETransaction.ProductNatureInInt =  oReader.GetInt32("ProductNature");
            oPETransaction.ShiftName = oReader.GetString("ShiftName");
            oPETransaction.MachineName = oReader.GetString("MachineName");
            return oPETransaction;
        }

        private PETransaction CreateObject(NullHandler oReader)
        {
            PETransaction oPETransaction = new PETransaction();
            oPETransaction = MapObject(oReader);
            return oPETransaction;
        }

        private List<PETransaction> CreateObjects(IDataReader oReader)
        {
            List<PETransaction> oPETransaction = new List<PETransaction>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PETransaction oItem = CreateObject(oHandler);
                oPETransaction.Add(oItem);
            }
            return oPETransaction;
        }

        #endregion

        #region Interface implementation
        public PETransactionService() { }

        public PETransaction Save(PETransaction oPETransaction, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = PETransactionDA.InsertUpdate(tc, oPETransaction, EnumDBOperation.Insert, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPETransaction = new PETransaction();
                    oPETransaction = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save PETransaction. Because of " + e.Message.Split('!')[0], e);
                #endregion
            }
            return oPETransaction;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PETransaction oPETransaction = new PETransaction();
                oPETransaction.PETransactionID = id;
                PETransactionDA.Delete(tc, oPETransaction, EnumDBOperation.Delete, nUserId);
                tc.End();
                return "Delete sucessfully";
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                PETransaction oPETransaction = new PETransaction();
                oPETransaction.ErrorMessage = e.Message;
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete PETransaction. Because of " + e.Message, e);
                #endregion
            }

        }

        public double GetYetToProductionHour(string sSQL, Int64 nUserId)
        {
            double nYetToProductionHour = 0.00;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = PETransactionDA.Gets(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    nYetToProductionHour = oReader.GetDouble("YetToProductionHour");
                    if (nYetToProductionHour < 0)
                    {
                        nYetToProductionHour = 0;
                    }
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                nYetToProductionHour = 0;
                #endregion
            }

            return nYetToProductionHour;
        }

        

        public PETransaction Get(int id, Int64 nUserId)
        {
            PETransaction oAccountHead = new PETransaction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PETransactionDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get PETransaction", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<PETransaction> Gets(string sSQL, Int64 nUserId)
        {
            List<PETransaction> oPETransaction = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PETransactionDA.Gets(tc, sSQL);
                oPETransaction = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PETransaction", e);
                #endregion
            }

            return oPETransaction;
        }

        public List<PETransaction> Gets(int nRecipeID, Int64 nUserId)
        {
            List<PETransaction> oPETransaction = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PETransactionDA.Gets(tc, nRecipeID);
                oPETransaction = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PETransaction", e);
                #endregion
            }

            return oPETransaction;
        }
        #endregion
    }
    
   
}
