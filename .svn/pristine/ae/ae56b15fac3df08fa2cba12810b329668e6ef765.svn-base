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
    public class FNBatchCostService : MarshalByRefObject, IFNBatchCostService
    {
        #region Private functions and declaration
        private FNBatchCost MapObject(NullHandler oReader)
        {
            FNBatchCost oFNBatchCost = new FNBatchCost();
            oFNBatchCost.FNExOID = oReader.GetInt32("FNExOID");
            oFNBatchCost.ProDate = oReader.GetDateTime("ProDate");
            oFNBatchCost.FNBatchID = oReader.GetInt32("FNBatchID");
            oFNBatchCost.Qty_Batch = oReader.GetDouble("Qty_Batch");
            oFNBatchCost.Qty_Production = oReader.GetDouble("Qty_Production");
            oFNBatchCost.Qty = oReader.GetDouble("Qty");
            oFNBatchCost.Value = oReader.GetDouble("Value");
            oFNBatchCost.CurrencyID = oReader.GetInt32("CurrencyID");
            oFNBatchCost.Currency = oReader.GetString("Currency");
            oFNBatchCost.ID = oReader.GetInt32("ID");
            oFNBatchCost.Name = oReader.GetString("Name");
            return oFNBatchCost;
        }
        private FNBatchCost CreateObject(NullHandler oReader)
        {
            FNBatchCost oFNBatchCost = new FNBatchCost();
            oFNBatchCost = MapObject(oReader);
            return oFNBatchCost;
        }
        private List<FNBatchCost> CreateObjects(IDataReader oReader)
        {
            List<FNBatchCost> oFNBatchCost = new List<FNBatchCost>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNBatchCost oItem = CreateObject(oHandler);
                oFNBatchCost.Add(oItem);
            }
            return oFNBatchCost;
        }

        #endregion

        #region RS Detail
        private FNBatchCost MapObjectDetail(NullHandler oReader)
        {
            FNBatchCost oFNBatchCost = new FNBatchCost();
            oFNBatchCost.FNExOID = oReader.GetInt32("FNExOID");
            oFNBatchCost.FNBatchID = oReader.GetInt32("FNBatchID");
            oFNBatchCost.FabricID = oReader.GetInt32("FabricID");
            oFNBatchCost.ProDate = oReader.GetDateTime("ProDate");
            oFNBatchCost.SCNo = oReader.GetString("SCNo");
            oFNBatchCost.DispoNo = oReader.GetString("DispoNo");
            oFNBatchCost.FNBatchNo = oReader.GetString("FNBatchNo");
            
            oFNBatchCost.Construction = oReader.GetString("Construction");
            oFNBatchCost.ContractorName = oReader.GetString("ContractorName");
            oFNBatchCost.BuyerName = oReader.GetString("BuyerName");
            oFNBatchCost.Color = oReader.GetString("Color");
            oFNBatchCost.OrderDate = oReader.GetDateTime("OrderDate");
            oFNBatchCost.ContractorID = oReader.GetInt32("ContractorID");
            oFNBatchCost.Qty_Order = oReader.GetDouble("Qty_Order");
            oFNBatchCost.Qty = oReader.GetDouble("Qty");
            oFNBatchCost.Qty_Batch = oReader.GetDouble("Qty_Batch");
            oFNBatchCost.Qty_Production = oReader.GetDouble("Qty_Production");
            oFNBatchCost.Value = oReader.GetDouble("Value");
            oFNBatchCost.OrderType = oReader.GetInt32("OrderType");
            oFNBatchCost.PIID = oReader.GetInt32("PIID");
            oFNBatchCost.PINo = oReader.GetString("PINo");
            oFNBatchCost.BuyerID = oReader.GetInt32("BuyerID");
            oFNBatchCost.LotID = oReader.GetInt32("LotID");
            oFNBatchCost.CurrencyID = oReader.GetInt32("CurrencyID");
            oFNBatchCost.ProductID = oReader.GetInt32("ProductID");
            oFNBatchCost.MUnitID = oReader.GetInt32("MUnitID");
            oFNBatchCost.MUnit = oReader.GetString("MUnit");
            oFNBatchCost.FNCode = oReader.GetString("FNCode");
            oFNBatchCost.MachineID = oReader.GetInt32("MachineID");
            oFNBatchCost.BuyerID = oReader.GetInt32("BuyerID");
            oFNBatchCost.UnitPrice = oReader.GetDouble("UnitPrice");
            oFNBatchCost.UnitPricePI = oReader.GetDouble("UnitPricePI");
            oFNBatchCost.ProductName = oReader.GetString("ProductName");
            oFNBatchCost.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oFNBatchCost.MachineName = oReader.GetString("MachineName");
            oFNBatchCost.FNProcess = oReader.GetString("FNProcess");
            oFNBatchCost.FNTreatment = (EnumFNTreatment)oReader.GetInt16("FNTreatment");
            oFNBatchCost.IsProduction = oReader.GetBoolean("IsProduction");
            oFNBatchCost.FNPBatchID = oReader.GetInt32("FNPBatchID");
            oFNBatchCost.MktName = oReader.GetString("MktName");
            oFNBatchCost.MktAccountID = oReader.GetInt32("MktAccountID");
							
            return oFNBatchCost;
        }
        private FNBatchCost CreateObject_Detail(NullHandler oReader)
        {
            FNBatchCost oFNBatchCost = new FNBatchCost();
            oFNBatchCost = MapObjectDetail(oReader);
            return oFNBatchCost;
        }
        private List<FNBatchCost> CreateObjects_Detail(IDataReader oReader)
        {
            List<FNBatchCost> oFNBatchCost = new List<FNBatchCost>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNBatchCost oItem = CreateObject_Detail(oHandler);
                oFNBatchCost.Add(oItem);
            }
            return oFNBatchCost;
        }
        #endregion

        #region Interface implementation
        public List<FNBatchCost> Gets(DateTime dStartDate, DateTime dEndDate, string sSQL, int nRouteSheetID, int nReportType, Int64 nUserId)
        {
            List<FNBatchCost> oFNBatchCosts = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNBatchCostDA.Gets(tc, dStartDate, dEndDate, sSQL, nRouteSheetID, nReportType);

                oFNBatchCosts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNBatchCost", e);
                #endregion
            }
            return oFNBatchCosts;
        }
        public List<FNBatchCost> GetsDetail(string sSQL, Int64 nUserId)
        {
            List<FNBatchCost> oFNBatchCosts = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNBatchCostDA.GetsDetail(tc,sSQL);
                oFNBatchCosts = CreateObjects_Detail(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNBatchCost", e);
                #endregion
            }
            return oFNBatchCosts;
        }
        public List<FNBatchCost> GetsSQL(string sSQL, Int64 nUserId)
        {
            List<FNBatchCost> oFNBatchCosts = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNBatchCostDA.GetsSQL(tc, sSQL);
                oFNBatchCosts = CreateObjects_Detail(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNBatchCost", e);
                #endregion
            }
            return oFNBatchCosts;
        }
        #endregion
    }    
}
