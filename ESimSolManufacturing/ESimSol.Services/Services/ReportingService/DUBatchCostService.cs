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
    public class DUBatchCostService : MarshalByRefObject, IDUBatchCostService
    {
        #region Private functions and declaration
        private DUBatchCost MapObject(NullHandler oReader)
        {
            DUBatchCost oDUBatchCost = new DUBatchCost();
            oDUBatchCost.RSNo = oReader.GetString("RSNo");
            oDUBatchCost.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oDUBatchCost.FactoryName = oReader.GetString("Factory");
            oDUBatchCost.Buyer = oReader.GetString("Buyer");
            oDUBatchCost.OrderNo = oReader.GetString("OrderNo");
            oDUBatchCost.RSDate =  oReader.GetDateTime("RSDate");
            oDUBatchCost.Qty_Dyes = oReader.GetDouble("Qty_Dyes");
            oDUBatchCost.Qty_Chemical = oReader.GetDouble("Qty_Chemical");
            oDUBatchCost.Qty_Yarn = oReader.GetDouble("Qty_Yarn");

            oDUBatchCost.ProductID = oReader.GetInt32("ProductID");
            oDUBatchCost.ProductName = oReader.GetString("ProductName");
            oDUBatchCost.Value_Dyes = oReader.GetDouble("Value_Dyes");
            oDUBatchCost.Value_Chemical = oReader.GetDouble("Value_Chemical");
            oDUBatchCost.Value_Yarn = oReader.GetDouble("Value_Yarn");
            oDUBatchCost.LocationName = oReader.GetString("LocationName");
            oDUBatchCost.LotNo = oReader.GetString("LotNo");
            oDUBatchCost.OperationUnitName = oReader.GetString("OperationUnit");
            oDUBatchCost.QtyDC = oReader.GetDouble("QtyDC");
            oDUBatchCost.RecycleQty = oReader.GetDouble("RecycleQty");
            oDUBatchCost.WastageQty = oReader.GetDouble("WastageQty");
            oDUBatchCost.Qty_Finishid = oReader.GetDouble("Qty_Finishid");
            oDUBatchCost.ValueRecycle = oReader.GetDouble("ValueRecycle");
            
            oDUBatchCost.YarnCount = oReader.GetString("YarnCount");
            oDUBatchCost.WUnit = oReader.GetString("WUnit");
            oDUBatchCost.NumberOfAddition = oReader.GetInt32("NumberOfAddition");
            oDUBatchCost.MachineID = oReader.GetInt32("MachineID");
            oDUBatchCost.MachineName = oReader.GetString("MachineName");
            oDUBatchCost.LocationID = oReader.GetInt32("LocationID");
            oDUBatchCost.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oDUBatchCost.WUName = oReader.GetString("WUName");
            oDUBatchCost.ShadeName = oReader.GetString("ShadeName");
            oDUBatchCost.ShadePertage = oReader.GetDouble("ShadePertage");
            oDUBatchCost.MLoadTime = oReader.GetDateTime("MLoadTime");
            oDUBatchCost.MUnLoadTime = oReader.GetDateTime("MUnLoadTime");
            oDUBatchCost.LotID_Yarn = oReader.GetInt32("LotID_Yarn");
            //DETAIL FROM RS COST
            oDUBatchCost.MUnit = oReader.GetString("MUnit");
            oDUBatchCost.CurrencySymbol = oReader.GetString("Currency"); 
            oDUBatchCost.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oDUBatchCost.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDUBatchCost.ContractorID = oReader.GetInt32("ContractorID");
            oDUBatchCost.ExportPIID = oReader.GetInt32("ExportPIID");
            oDUBatchCost.BuyerID = oReader.GetInt32("BuyerID");
            oDUBatchCost.OrderType = oReader.GetInt32("OrderType");
            oDUBatchCost.ContractorName = oReader.GetString("ContractorName");
            oDUBatchCost.ColorName = oReader.GetString("ColorName");
            oDUBatchCost.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oDUBatchCost.AdditionalQty = oReader.GetDouble("AdditionalQty");
            oDUBatchCost.AdditionalRate = oReader.GetDouble("AdditionalRate");
            oDUBatchCost.NumberOfAddition = oReader.GetInt32("NumberOfAddition");
            oDUBatchCost.PINo = oReader.GetString("PINo");
            oDUBatchCost.UnitPrice = oReader.GetDouble("UnitPrice");
            oDUBatchCost.MCapacity = oReader.GetDouble("MCapacity");
            oDUBatchCost.PShort = oReader.GetDouble("PShort");
            oDUBatchCost.IsInHouse = oReader.GetBoolean("IsInHouse");
            oDUBatchCost.IsReDyeing = oReader.GetBoolean("IsReDyeing");
            oDUBatchCost.RSState = (EnumRSState)oReader.GetInt32("RSState");
            oDUBatchCost.WUName = oReader.GetString("WUName");
            
            return oDUBatchCost;
        }
        private DUBatchCost CreateObject(NullHandler oReader)
        {
            DUBatchCost oDUBatchCost = new DUBatchCost();
            oDUBatchCost = MapObject(oReader);
            return oDUBatchCost;
        }
        private List<DUBatchCost> CreateObjects(IDataReader oReader)
        {
            List<DUBatchCost> oDUBatchCost = new List<DUBatchCost>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUBatchCost oItem = CreateObject(oHandler);
                oDUBatchCost.Add(oItem);
            }
            return oDUBatchCost;
        }

        #endregion

        #region RS Detail
        private DUBatchCost MapObjectDetail(NullHandler oReader)
        {
            DUBatchCost oDUBatchCost = new DUBatchCost();
            oDUBatchCost.RSNo = oReader.GetString("RSNo");
            oDUBatchCost.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oDUBatchCost.FactoryName = oReader.GetString("Factory");
            oDUBatchCost.Buyer = oReader.GetString("Buyer");
            oDUBatchCost.OrderNo = oReader.GetString("OrderNo");
            oDUBatchCost.YarnCount = oReader.GetString("YarnCount");
            oDUBatchCost.RSDate = oReader.GetDateTime("RSDate");

            oDUBatchCost.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oDUBatchCost.ShadeName = oReader.GetString("ShadeName");
            oDUBatchCost.ShadePertage = oReader.GetDouble("ShadePertage");
            oDUBatchCost.RouteSheetDetailID = oReader.GetInt32("RouteSheetDetailID");
            oDUBatchCost.ProductID = oReader.GetInt32("ProductID");
            oDUBatchCost.ProductName = oReader.GetString("ProductName");
            oDUBatchCost.Qty = oReader.GetDouble("Qty");
            oDUBatchCost.AdditionalQty = oReader.GetDouble("AdditionalQty");
            oDUBatchCost.ReturnQty = oReader.GetDouble("ReturnQty");
            oDUBatchCost.TotalQty = oReader.GetDouble("TotalQty");
            oDUBatchCost.TotalRate = oReader.GetDouble("TotalRate");

          
            oDUBatchCost.Value = oReader.GetDouble("Value");
            oDUBatchCost.TotalQtyLotID = oReader.GetInt32("TotalQtyLotID");
            //oDUBatchCost.AddOneLotID = oReader.GetInt32("AddOneLotID");
            //oDUBatchCost.AddTwoLotID = oReader.GetInt32("AddTwoLotID");
            //oDUBatchCost.AddThreeLotID = oReader.GetInt32("AddThreeLotID");
            //oDUBatchCost.ReturnLotID = oReader.GetInt32("ReturnLotID");
            
            //oDUBatchCost.AddOne = oReader.GetDouble("AddOneQty");
            //oDUBatchCost.AddTwo = oReader.GetDouble("AddTwoQty");
            //oDUBatchCost.AddThree = oReader.GetDouble("AddThreeQty");
          
            //oDUBatchCost.AddOneRate = oReader.GetDouble("AddOneRate");
            //oDUBatchCost.AddTwoRate = oReader.GetDouble("AddTwoRate");
            //oDUBatchCost.AddThreeRate = oReader.GetDouble("AddThreeRate");
            //oDUBatchCost.ReturnRate = oReader.GetDouble("ReturnRate");
            
            //oDUBatchCost.AdditionalRate = oReader.GetDouble("AdditionalRate");
            oDUBatchCost.NumberOfAddition = oReader.GetInt32("NumberOfAddition");
            oDUBatchCost.MUnitID = oReader.GetInt32("MUnitID");
            oDUBatchCost.MUnit = oReader.GetString("MUnit");
            oDUBatchCost.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oDUBatchCost.CurrencyID = oReader.GetInt32("CurrencyID");
            oDUBatchCost.InvoiceDetailID = oReader.GetInt32("InvoiceDetailID");
            oDUBatchCost.InvoiceTypeID = oReader.GetInt32("InvoiceTypeID");
            oDUBatchCost.Qty_Finishid = oReader.GetDouble("Qty_Finishid");
            oDUBatchCost.LotNo = oReader.GetString("LotNo");
           
            
            return oDUBatchCost;
        }
        private DUBatchCost CreateObject_Detail(NullHandler oReader)
        {
            DUBatchCost oDUBatchCost = new DUBatchCost();
            oDUBatchCost = MapObjectDetail(oReader);
            return oDUBatchCost;
        }
        private List<DUBatchCost> CreateObjects_Detail(IDataReader oReader)
        {
            List<DUBatchCost> oDUBatchCost = new List<DUBatchCost>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUBatchCost oItem = CreateObject_Detail(oHandler);
                oDUBatchCost.Add(oItem);
            }
            return oDUBatchCost;
        }
        #endregion

        #region Interface implementation
        public List<DUBatchCost> Gets(DateTime dStartDate, DateTime dEndDate, string sSQL, int nRouteSheetID, int nReportType, Int64 nUserId)
        {
            List<DUBatchCost> oDUBatchCosts = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUBatchCostDA.Gets(tc, dStartDate, dEndDate, sSQL, nRouteSheetID, nReportType);
                oDUBatchCosts = CreateObjects(reader);
                //if (nReportType == 1)
                //{
                //    oDUBatchCosts = CreateObjects(reader);
                //}
                //else
                //{
                //    oDUBatchCosts = CreateObjects_RS(reader);
                //}
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUBatchCost", e);
                #endregion
            }

            return oDUBatchCosts;
        }
        public List<DUBatchCost> GetsDetail(string sSQL, int nRouteSheetID, Int64 nUserId)
        {
            List<DUBatchCost> oDUBatchCosts = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUBatchCostDA.GetsDetail(tc, nRouteSheetID, sSQL);
                oDUBatchCosts = CreateObjects_Detail(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUBatchCost", e);
                #endregion
            }

            return oDUBatchCosts;
        }
        #endregion
    }    
    
  
}
