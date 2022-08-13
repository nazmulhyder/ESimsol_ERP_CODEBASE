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
    public class WUBatchCostService : MarshalByRefObject, IWUBatchCostService
    {
        #region Private functions and declaration
        private WUBatchCost MapObject(NullHandler oReader)
        {
            WUBatchCost oWUBatchCost = new WUBatchCost();
            oWUBatchCost.FEOID = oReader.GetInt32("FEOID");
            oWUBatchCost.ProDate = oReader.GetDateTime("ProDate");
            oWUBatchCost.FBID = oReader.GetInt32("FBID");
            oWUBatchCost.Qty_Batch = oReader.GetDouble("Qty_Batch");
            oWUBatchCost.Value = oReader.GetDouble("Value");
            oWUBatchCost.CurrencyID = oReader.GetInt32("CurrencyID");
            oWUBatchCost.Currency = oReader.GetString("Currency");
            oWUBatchCost.ID = oReader.GetInt32("ID");
            oWUBatchCost.Name = oReader.GetString("Name");
            return oWUBatchCost;
        }
        private WUBatchCost CreateObject(NullHandler oReader)
        {
            WUBatchCost oWUBatchCost = new WUBatchCost();
            oWUBatchCost = MapObject(oReader);
            return oWUBatchCost;
        }
        private List<WUBatchCost> CreateObjects(IDataReader oReader)
        {
            List<WUBatchCost> oWUBatchCost = new List<WUBatchCost>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                WUBatchCost oItem = CreateObject(oHandler);
                oWUBatchCost.Add(oItem);
            }
            return oWUBatchCost;
        }

        #endregion

        #region RS Detail
        private WUBatchCost MapObjectDetail(NullHandler oReader)
        {
            WUBatchCost oWUBatchCost = new WUBatchCost();
            oWUBatchCost.FEOID = oReader.GetInt32("FEOID");
            oWUBatchCost.FBID = oReader.GetInt32("FBID");
            oWUBatchCost.FabricID = oReader.GetInt32("FabricID");
            oWUBatchCost.ProDate = oReader.GetDateTime("ProDate");
            oWUBatchCost.OrderNo = oReader.GetString("OrderNo");
            oWUBatchCost.ExcNo = oReader.GetString("ExcNo");
            oWUBatchCost.Construction = oReader.GetString("Construction");
            oWUBatchCost.ContractorName = oReader.GetString("ContractorName");
            oWUBatchCost.BuyerName = oReader.GetString("BuyerName");
            oWUBatchCost.Color = oReader.GetString("Color");
            oWUBatchCost.OrderDate = oReader.GetDateTime("OrderDate");
            oWUBatchCost.ContractorID = oReader.GetInt32("ContractorID");
            oWUBatchCost.Qty_Order = oReader.GetDouble("Qty_Order");
            oWUBatchCost.Qty = oReader.GetDouble("Qty");
            oWUBatchCost.Qty_Batch = oReader.GetDouble("Qty_Batch");
            oWUBatchCost.OrderType = oReader.GetInt32("OrderType");
            oWUBatchCost.PIID = oReader.GetInt32("PIID");
            oWUBatchCost.PINo = oReader.GetString("PINo");
            oWUBatchCost.BuyerID = oReader.GetInt32("BuyerID");
            oWUBatchCost.LotID = oReader.GetInt32("LotID");
            oWUBatchCost.CurrencyID = oReader.GetInt32("CurrencyID");
            oWUBatchCost.ProductID = oReader.GetInt32("ProductID");
            oWUBatchCost.MUnitID = oReader.GetInt32("MUnitID");
            oWUBatchCost.MUnit = oReader.GetString("MUnit");
            oWUBatchCost.PINo = oReader.GetString("PINo");
            oWUBatchCost.MachineID = oReader.GetInt32("MachineID");
            oWUBatchCost.BuyerID = oReader.GetInt32("BuyerID");
            oWUBatchCost.UnitPrice = oReader.GetDouble("UnitPrice");
            oWUBatchCost.UnitPricePI = oReader.GetDouble("UnitPricePI");
            oWUBatchCost.ProductName = oReader.GetString("ProductName");
            oWUBatchCost.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oWUBatchCost.MachineName = oReader.GetString("MachineName");
            oWUBatchCost.Process = oReader.GetString("Process");
            oWUBatchCost.ProcessType = oReader.GetInt16("ProcessType");
            oWUBatchCost.WeavingProcess = (EnumWeavingProcess)oReader.GetInt16("WeavingProcess");
							
            return oWUBatchCost;
        }
        private WUBatchCost CreateObject_Detail(NullHandler oReader)
        {
            WUBatchCost oWUBatchCost = new WUBatchCost();
            oWUBatchCost = MapObjectDetail(oReader);
            return oWUBatchCost;
        }
        private List<WUBatchCost> CreateObjects_Detail(IDataReader oReader)
        {
            List<WUBatchCost> oWUBatchCost = new List<WUBatchCost>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                WUBatchCost oItem = CreateObject_Detail(oHandler);
                oWUBatchCost.Add(oItem);
            }
            return oWUBatchCost;
        }
        #endregion

        #region Interface implementation
        public List<WUBatchCost> Gets(DateTime dStartDate, DateTime dEndDate, string sSQL, int nRouteSheetID, int nReportType, Int64 nUserId)
        {
            List<WUBatchCost> oWUBatchCosts = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WUBatchCostDA.Gets(tc, dStartDate, dEndDate, sSQL, nRouteSheetID, nReportType);

                oWUBatchCosts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WUBatchCost", e);
                #endregion
            }
            return oWUBatchCosts;
        }
        public List<WUBatchCost> GetsDetail(DateTime dStartDate, DateTime dEndDate, string sSQL, int nRouteSheetID, int nReportType, Int64 nUserId)
        {
            List<WUBatchCost> oWUBatchCosts = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WUBatchCostDA.GetsDetail(tc, dStartDate, dEndDate, sSQL, nRouteSheetID, nReportType);
                oWUBatchCosts = CreateObjects_Detail(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WUBatchCost", e);
                #endregion
            }
            return oWUBatchCosts;
        }
        #endregion
    }    
}
