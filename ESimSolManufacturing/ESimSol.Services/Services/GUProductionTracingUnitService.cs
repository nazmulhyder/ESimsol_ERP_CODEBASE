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
    public class GUProductionTracingUnitService : MarshalByRefObject, IGUProductionTracingUnitService
    {
        #region Private functions and declaration
        private GUProductionTracingUnit MapObject(NullHandler oReader)
        {
            GUProductionTracingUnit oGUProductionTracingUnit = new GUProductionTracingUnit();
            oGUProductionTracingUnit.GUProductionTracingUnitID = oReader.GetInt32("GUProductionTracingUnitID");
            oGUProductionTracingUnit.SaleOrderID = oReader.GetInt32("SaleOrderID");
            oGUProductionTracingUnit.GUProductionOrderID = oReader.GetInt32("GUProductionOrderID");
            oGUProductionTracingUnit.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oGUProductionTracingUnit.ColorID = oReader.GetInt32("ColorID");
            oGUProductionTracingUnit.SizeID = oReader.GetInt32("SizeID");

            oGUProductionTracingUnit.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oGUProductionTracingUnit.OrderQty = oReader.GetDouble("OrderQty");
            oGUProductionTracingUnit.StyleNo = oReader.GetString("StyleNo");
            oGUProductionTracingUnit.SaleOrderNo = oReader.GetString("SaleOrderNo");
            oGUProductionTracingUnit.BuyerID = oReader.GetInt32("BuyerID");
            oGUProductionTracingUnit.BuyerName = oReader.GetString("BuyerName");
            oGUProductionTracingUnit.BuyerContactPerson = oReader.GetString("BuyerContactPerson");
            oGUProductionTracingUnit.ProductID = oReader.GetInt32("ProductID");
            oGUProductionTracingUnit.ProductName = oReader.GetString("ProductName");
            oGUProductionTracingUnit.ColorName = oReader.GetString("ColorName");
            oGUProductionTracingUnit.SizeName = oReader.GetString("SizeName");
            oGUProductionTracingUnit.GUProductionOrderNo = oReader.GetString("GUProductionOrderNo");
            oGUProductionTracingUnit.MeasurementUnitName = oReader.GetString("MeasurementUnitName");
            oGUProductionTracingUnit.GUProductionProcedureID = oReader.GetInt32("GUProductionProcedureID");
            oGUProductionTracingUnit.ProductionStepID = oReader.GetInt32("ProductionStepID");
            oGUProductionTracingUnit.ExecutionQty = oReader.GetDouble("ExecutionQty");
            oGUProductionTracingUnit.YetToExecutionQty = oReader.GetDouble("YetToExecutionQty");
            oGUProductionTracingUnit.ExecutionStartDate = oReader.GetDateTime("ExecutionStartDate");
            oGUProductionTracingUnit.StepName = oReader.GetString("StepName");
            oGUProductionTracingUnit.PreviousStepName = oReader.GetString("PreviousStepName");
            oGUProductionTracingUnit.PreviousStepExecutionQty = oReader.GetDouble("PreviousStepExecutionQty");
            oGUProductionTracingUnit.PreviousStepSequence = oReader.GetInt32("PreviousStepSequence");
            return oGUProductionTracingUnit;
        }

        private GUProductionTracingUnit CreateObject(NullHandler oReader)
        {
            GUProductionTracingUnit oGUProductionTracingUnit = new GUProductionTracingUnit();
            oGUProductionTracingUnit = MapObject(oReader);
            return oGUProductionTracingUnit;
        }

        private List<GUProductionTracingUnit> CreateObjects(IDataReader oReader)
        {
            List<GUProductionTracingUnit> oGUProductionTracingUnit = new List<GUProductionTracingUnit>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GUProductionTracingUnit oItem = CreateObject(oHandler);
                oGUProductionTracingUnit.Add(oItem);
            }
            return oGUProductionTracingUnit;
        }

        #endregion

        #region Interface implementation
        public GUProductionTracingUnitService() { }

        public List<GUProductionTracingUnit> CommitProductionExecution(List<PTUTransection> oPTUTransections, Int64 nUserId)
        {
            List<GUProductionTracingUnit> oGUProductionTracingUnits = new List<GUProductionTracingUnit>();
            GUProductionTracingUnit oGUProductionTracingUnit = new GUProductionTracingUnit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                foreach (PTUTransection oItem in oPTUTransections)
                {
                    reader = GUProductionTracingUnitDA.CommitProductionExecution(tc, oItem, nUserId);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oGUProductionTracingUnit = CreateObject(oReader);
                        oGUProductionTracingUnits.Add(oGUProductionTracingUnit);
                    }
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oGUProductionTracingUnits = new List<GUProductionTracingUnit>();
                oGUProductionTracingUnit = new GUProductionTracingUnit();
                oGUProductionTracingUnit.ErrorMessage = e.Message.Split('~')[0];
                oGUProductionTracingUnits.Add(oGUProductionTracingUnit);
                #endregion
            }
            return oGUProductionTracingUnits;
        }


        public GUProductionTracingUnit Save(GUProductionTracingUnit oGUProductionTracingUnit, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oGUProductionTracingUnit.GUProductionTracingUnitID <= 0)
                {
                    reader = GUProductionTracingUnitDA.InsertUpdate(tc, oGUProductionTracingUnit, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = GUProductionTracingUnitDA.InsertUpdate(tc, oGUProductionTracingUnit, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGUProductionTracingUnit = new GUProductionTracingUnit();
                    oGUProductionTracingUnit = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save GUProductionTracingUnit. Because of " + e.Message, e);
                #endregion
            }
            return oGUProductionTracingUnit;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                GUProductionTracingUnit oGUProductionTracingUnit = new GUProductionTracingUnit();
                oGUProductionTracingUnit.GUProductionTracingUnitID = id;
                GUProductionTracingUnitDA.Delete(tc, oGUProductionTracingUnit, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete GUProductionTracingUnit. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public GUProductionTracingUnit Get(int id, Int64 nUserId)
        {
            GUProductionTracingUnit oAccountHead = new GUProductionTracingUnit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GUProductionTracingUnitDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get GUProductionTracingUnit", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<GUProductionTracingUnit> GetsByGUProductionOrder(int nGUProductionOrderID, Int64 nUserID)
        {
            List<GUProductionTracingUnit> oGUProductionTracingUnits = new List<GUProductionTracingUnit>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GUProductionTracingUnitDA.GetsByGUProductionOrder(tc, nGUProductionOrderID);
                oGUProductionTracingUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Production", e);
                #endregion
            }
            return oGUProductionTracingUnits;
        }

        public List<GUProductionTracingUnit> GetsPTU(int nGUProductionOrderID, int nProductionStepID, Int64 nUserID)
        {
            List<GUProductionTracingUnit> oGUProductionTracingUnits = new List<GUProductionTracingUnit>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GUProductionTracingUnitDA.GetsPTU(tc, nGUProductionOrderID, nProductionStepID);
                oGUProductionTracingUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Production", e);
                #endregion
            }
            return oGUProductionTracingUnits;
        }
        public List<GUProductionTracingUnit> GetsPTU(int nGUProductionOrderID, Int64 nUserID)
        {
            List<GUProductionTracingUnit> oGUProductionTracingUnits = new List<GUProductionTracingUnit>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GUProductionTracingUnitDA.GetsPTU(tc, nGUProductionOrderID);
                oGUProductionTracingUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Production", e);
                #endregion
            }
            return oGUProductionTracingUnits;
        }

        public List<GUProductionTracingUnit> GetPTUViewDetails(DateTime nOperationDate, int nExcutionFatoryId, int nGUProductionOrderID, Int64 nUserID)
        {
            List<GUProductionTracingUnit> oGUProductionTracingUnit = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GUProductionTracingUnitDA.GetPTUViewDetails(tc, nOperationDate, nExcutionFatoryId, nGUProductionOrderID);
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

        public List<GUProductionTracingUnit> Gets(string sSQL, Int64 nUserId)
        {
            List<GUProductionTracingUnit> oGUProductionTracingUnit = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GUProductionTracingUnitDA.Gets(tc, sSQL);
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
                throw new ServiceException("Failed to Get SaleOrder", e);
                #endregion
            }

            return oGUProductionTracingUnit;
        }

        public List<GUProductionTracingUnit> GetsByOrderRecap(int nOrderRecapID, Int64 nUserID)
        {
            List<GUProductionTracingUnit> oGUProductionTracingUnit = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GUProductionTracingUnitDA.GetsByOrderRecap(tc, nOrderRecapID);
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
                throw new ServiceException("Failed to Get SaleOrder", e);
                #endregion
            }

            return oGUProductionTracingUnit;
        }

        public List<GUProductionTracingUnit> Gets_byPOIDs(string sPOIDs, Int64 nUserID)
        {
            List<GUProductionTracingUnit> oGUProductionTracingUnit = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GUProductionTracingUnitDA.Gets_byPOIDs(tc, sPOIDs);
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
                throw new ServiceException("Failed to Get SaleOrder", e);
                #endregion
            }

            return oGUProductionTracingUnit;
        }

        #endregion
    }
}
