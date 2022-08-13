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
    public class PTUTransectionService : MarshalByRefObject, IPTUTransectionService
    {
        #region Private functions and declaration
        private PTUTransection MapObject(NullHandler oReader)
        {
            PTUTransection oPTUTransection = new PTUTransection();
            oPTUTransection.PTUTransectionID = oReader.GetInt32("PTUTransectionID");
            oPTUTransection.GUProductionTracingUnitDetailID = oReader.GetInt32("GUProductionTracingUnitDetailID");
            oPTUTransection.ProductionStepID = oReader.GetInt32("ProductionStepID");
            oPTUTransection.MeasurementUnitID = oReader.GetInt32("MeasurmentUnitID");
            oPTUTransection.Quantity = oReader.GetDouble("Quantity");
            oPTUTransection.PLineConfigureID = oReader.GetInt32("PLineConfigureID");
            oPTUTransection.Note = oReader.GetString("Note");
            oPTUTransection.OperationDate = oReader.GetDateTime("OperationDate");
            oPTUTransection.LineNo = oReader.GetString("LineNo");
            oPTUTransection.OperationBy = oReader.GetString("OperationBy");
            oPTUTransection.ColorName = oReader.GetString("ColorName");
            oPTUTransection.SizeName = oReader.GetString("SizeName");
            oPTUTransection.ColorID = oReader.GetInt32("ColorID");
            oPTUTransection.SizeID = oReader.GetInt32("SizeID");
            oPTUTransection.GUProductionProcedureID = oReader.GetInt32("GUProductionProcedureID");
            oPTUTransection.MeasurementUnitName = oReader.GetString("MeasurementUnitName");
            oPTUTransection.StyleNo = oReader.GetString("StyleNo");
            oPTUTransection.GUProductionOrderNo = oReader.GetString("GUProductionOrderNo");
            oPTUTransection.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oPTUTransection.ActualWorkingHour = oReader.GetDouble("ActualWorkingHour");
            oPTUTransection.UseHelper = oReader.GetInt32("UseHelper");
            oPTUTransection.UseOperator = oReader.GetInt32("UseOperator");
            return oPTUTransection; 
        }

        private PTUTransection CreateObject(NullHandler oReader)
        {
            PTUTransection oPTUTransection = new PTUTransection();
            oPTUTransection = MapObject(oReader);
            return oPTUTransection;
        }

        private List<PTUTransection> CreateObjects(IDataReader oReader)
        {
            List<PTUTransection> oPTUTransection = new List<PTUTransection>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PTUTransection oItem = CreateObject(oHandler);
                oPTUTransection.Add(oItem);
            }
            return oPTUTransection;
        }

        #endregion

        #region Interface implementation
        public PTUTransectionService() { }

        public List<PTUTransection> GetPTUTransactionHistory(int nGUProductionOrderID, int nProductionStepID, Int64 nUserId)
        {
            List<PTUTransection> oPTUTransections = new List<PTUTransection>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PTUTransectionDA.GetPTUTransactionHistory(tc, nGUProductionOrderID, nProductionStepID);
                oPTUTransections = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PTUTransection", e);
                #endregion
            }

            return oPTUTransections;
        }

        public List<PTUTransection> Gets(string sSQL, Int64 nUserID)
        {
            List<PTUTransection> oPTUTransections = new List<PTUTransection>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PTUTransectionDA.Gets(tc, sSQL);
                oPTUTransections = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PTUTransection", e);
                #endregion
            }

            return oPTUTransections;
        }

        public List<PTUTransection> Gets_byPOIDs(string sPOIDs, Int64 nUserID)
        {
            List<PTUTransection> oPTUTransections = new List<PTUTransection>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PTUTransectionDA.Gets_byPOIDs(tc, sPOIDs);
                oPTUTransections = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PTUTransection", e);
                #endregion
            }

            return oPTUTransections;
        }

        public List<PTUTransection> GetPTUViewDetails(int nProductionStepID, DateTime dOperationDate, int nExcutionFatoryId, int nGUProductionOrderID, Int64 nUserID)
        {
            List<PTUTransection> oGUProductionTracingUnit = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PTUTransectionDA.GetPTUViewDetails(tc, nProductionStepID, dOperationDate, nExcutionFatoryId, nGUProductionOrderID);
                oGUProductionTracingUnit = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Production Tracing Unit Details!!", e);
                #endregion
            }

            return oGUProductionTracingUnit;
        }
        public List<PTUTransection> GetPTUViewDetails(int nGUProductionTracingUnitDetailID, Int64 nUserID)
        {
            List<PTUTransection> oGUProductionTracingUnit = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PTUTransectionDA.GetPTUViewDetails(tc, nGUProductionTracingUnitDetailID);
                oGUProductionTracingUnit = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Production Tracing Unit Details!!", e);
                #endregion
            }

            return oGUProductionTracingUnit;
        }

        public PTUTransection UpdatePTUTransaction (PTUTransection oPTUTransection, Int64 nUserID)
        {
            PTUTransection oNewPTUTransection = new PTUTransection();
            List<PTUTransection> oPTUTransections = new List<PTUTransection>();
            oPTUTransections = oPTUTransection.PTUTransections;
            string sPTUTransactionIDs = "";
            double nExecutionQty = 0;
            double nPriviousStepExecutionQty = 0;
            int nTempSequence = 0, nTempGUProductionTracingUnitID = 0, nTempGUProductionTracingUnitDetailID = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader;
                foreach (PTUTransection oItem in oPTUTransections)
                {
                    PTUTransectionDA.UpdatePTUTransaction(tc, oItem);
                    sPTUTransactionIDs += oItem.PTUTransectionID+ ",";
                }
                nTempGUProductionTracingUnitDetailID = oPTUTransections[0].GUProductionTracingUnitDetailID;//Set roductin Tracing Unit Detail ID
                reader = GUProductionTracingUnitDetailDA.Get(tc, nTempGUProductionTracingUnitDetailID);//Get Productin Tracing Unit Detail
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    nTempSequence = oReader.GetInt32("Sequence");
                    nTempGUProductionTracingUnitID = oReader.GetInt32("GUProductionTracingUnitID");
                }
                reader.Close();
                if (nTempSequence > 1)
                {
                    reader = GUProductionTracingUnitDetailDA.GetBySequence(tc, nTempGUProductionTracingUnitID, nTempSequence - 1);//Get for ProductionTracing Unit and Privious Sequence
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        nPriviousStepExecutionQty = oReader.GetDouble("ExecutionQty");
                    }
                    reader.Close();
                }
                reader = PTUTransectionDA.GetBuyPTUDetailID(tc, nTempGUProductionTracingUnitDetailID);
                oPTUTransections = CreateObjects(reader);
                reader.Close();
                foreach (PTUTransection oItem in oPTUTransections)
                {
                    nExecutionQty += oItem.Quantity;
                }
                if (nPriviousStepExecutionQty!=0)
                {
                    if (nExecutionQty > nPriviousStepExecutionQty)
                    {
                        throw new System.ArgumentException("Sorry, Qty Execeed the Privious Step Qty. Please Try for more Small Qty OR update Privous Step Qty. ");
                    }
                    else
                    {
                        GUProductionTracingUnitDetailDA.UpdateExecutionQty(tc, oPTUTransections[0].GUProductionTracingUnitDetailID, nExecutionQty);//Update Production Tracing Unit Detail Qty
                    }
                }
                else
                {
                    GUProductionTracingUnitDetailDA.UpdateExecutionQty(tc, oPTUTransections[0].GUProductionTracingUnitDetailID, nExecutionQty);//Update Production Tracing Unit Detail Qty
                }
                sPTUTransactionIDs = sPTUTransactionIDs.Remove(sPTUTransactionIDs.Length - 1, 1);
                reader = PTUTransectionDA.GetPTUTransactionBYIDs(tc, sPTUTransactionIDs);
                oPTUTransections = CreateObjects(reader);
                reader.Close();
                oNewPTUTransection.PTUTransections = oPTUTransections;
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oNewPTUTransection.ErrorMessage = e.Message;
                #endregion
            }

            return oNewPTUTransection;
        }

        public DataSet GetDailyProductionReport(DateTime  StartDate, DateTime EndDate, int BUID, int ProductionUnitID, Int64 nUserID)
        {
            TransactionContext tc = null;
            DataSet oDataSet = new DataSet();
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = PTUTransectionDA.GetsDataSet(tc, StartDate, EndDate, BUID, ProductionUnitID);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[1]);
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DataSet", e);
                #endregion
            }
            return oDataSet;
        }
        #endregion
    }
}
