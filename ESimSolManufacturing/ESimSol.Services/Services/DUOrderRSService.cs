using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    public class DUOrderRSService : MarshalByRefObject, IDUOrderRSService
    {
        #region Private functions and declaration
        private DUOrderRS MapObject(NullHandler oReader)
        {
            DUOrderRS oDUOrderRS = new DUOrderRS();
            oDUOrderRS.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oDUOrderRS.ProductID = oReader.GetInt32("ProductID");
            oDUOrderRS.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oDUOrderRS.RSDate = oReader.GetDateTime("RSDate");
            oDUOrderRS.QCDate = oReader.GetDateTime("QCDate");
            oDUOrderRS.ProductCode = oReader.GetString("ProductCode");
            oDUOrderRS.ProductName = oReader.GetString("ProductName");
            oDUOrderRS.Qty_RS = oReader.GetDouble("Qty_RS");
            oDUOrderRS.FreshDyedYarnQty = oReader.GetDouble("FreshDyedYarnQty");
            oDUOrderRS.UnManagedQty = oReader.GetDouble("UnManagedQty");
            oDUOrderRS.ManagedQty = oReader.GetDouble("ManagedQty");
            oDUOrderRS.DeliveryQty = oReader.GetDouble("DeliveryQty");
            oDUOrderRS.StockInHand = oReader.GetDouble("StockInHand");
            oDUOrderRS.GainLoss = oReader.GetDouble("GainLoss");
            oDUOrderRS.Pro_Pipline = oReader.GetDouble("Pro_Pipline");
            oDUOrderRS.OrderNo = oReader.GetString("OrderNo");
          
            oDUOrderRS.ColorName = oReader.GetString("ColorName");
            oDUOrderRS.BagCount = oReader.GetInt32("BagCount");
            oDUOrderRS.OrderType = oReader.GetInt16("OrderType");
            oDUOrderRS.RSState = (EnumRSState)oReader.GetInt16("RSState");
            oDUOrderRS.LotNo = oReader.GetString("LotNo");
            oDUOrderRS.WUName = oReader.GetString("WUName");
            oDUOrderRS.BuyerName = oReader.GetString("BuyerName");
            oDUOrderRS.ProductCategoryName = oReader.GetString("ProductCategoryName");

            oDUOrderRS.Qty = oReader.GetDouble("Qty");
            oDUOrderRS.MachineID = oReader.GetInt32("MachineID");
            oDUOrderRS.MachineName = oReader.GetString("MachineName");
            oDUOrderRS.LocationID = oReader.GetInt32("LocationID");
            oDUOrderRS.LocationName = oReader.GetString("LocationName");
            oDUOrderRS.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oDUOrderRS.StartTime = oReader.GetDateTime("StartTime");
            oDUOrderRS.EndTime = oReader.GetDateTime("EndTime");
            oDUOrderRS.LotID_Yarn = oReader.GetInt32("LotID_Yarn");
            oDUOrderRS.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oDUOrderRS.MUnitID = oReader.GetInt32("MUnitID");
            oDUOrderRS.IsReDyeing = oReader.GetBoolean("IsReDyeing");
            oDUOrderRS.HanksCone = oReader.GetInt16("HanksCone");
            oDUOrderRS.ExportPIID = oReader.GetInt32("ExportPIID");
            oDUOrderRS.BuyerID = oReader.GetInt32("BuyerID");
            oDUOrderRS.UnitPrice = oReader.GetDouble("UnitPrice");
            oDUOrderRS.IsInHouse = oReader.GetBoolean("IsInHouse");
            oDUOrderRS.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oDUOrderRS.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDUOrderRS.PINo = oReader.GetString("PINo");
            oDUOrderRS.MUnit = oReader.GetString("MUnit");
            oDUOrderRS.DUPScheduleID = oReader.GetInt32("DUPScheduleID");
            oDUOrderRS.OrderQty = oReader.GetDouble("OrderQty");
            oDUOrderRS.FinishQty = oReader.GetDouble("FinishQty");
            oDUOrderRS.PackingQty = oReader.GetDouble("PackingQty");
            oDUOrderRS.ShadingQty = oReader.GetDouble("ShadingQty");
            oDUOrderRS.WastageQty = oReader.GetDouble("WastageQty");
            oDUOrderRS.ColorMisQty = oReader.GetDouble("ColorMisQty");
            oDUOrderRS.RecycleQty = oReader.GetDouble("RecycleQty");
            oDUOrderRS.QtyDC = oReader.GetDouble("QtyDC");
            oDUOrderRS.QtyRC = oReader.GetDouble("QtyRC");
            oDUOrderRS.QtyTR = oReader.GetDouble("QtyTR");
            oDUOrderRS.RSShiftID = oReader.GetInt32("RSShiftID");
            oDUOrderRS.ShiftName = oReader.GetString("ShiftName");
            oDUOrderRS.QCApproveByName = oReader.GetString("QCApproveByName");
            oDUOrderRS.ContractorName = oReader.GetString("ContractorName");
            
            
            return oDUOrderRS;
        }
        private DUOrderRS CreateObject(NullHandler oReader)
        {
            DUOrderRS oDUOrderRS = new DUOrderRS();
            oDUOrderRS = MapObject(oReader);
            return oDUOrderRS;
        }
        private List<DUOrderRS> CreateObjects(IDataReader oReader)
        {
            List<DUOrderRS> oDUOrderRS = new List<DUOrderRS>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUOrderRS oItem = CreateObject(oHandler);
                oDUOrderRS.Add(oItem);
            }
            return oDUOrderRS;
        }
        #region Product
        private DUOrderRS MapObject_Product(NullHandler oReader)
        {
            DUOrderRS oDUOrderRS = new DUOrderRS();
            oDUOrderRS.ProductID = oReader.GetInt32("ProductID");
            oDUOrderRS.ProductName = oReader.GetString("ProductName");
            oDUOrderRS.Qty_RS = oReader.GetDouble("Qty_RS");
            oDUOrderRS.FreshDyedYarnQty = oReader.GetDouble("FreshDyedYarnQty");
            oDUOrderRS.UnManagedQty = oReader.GetDouble("UnManagedQty");
            oDUOrderRS.ManagedQty = oReader.GetDouble("ManagedQty");
            oDUOrderRS.BagCount = oReader.GetInt16("BagCount");
            return oDUOrderRS;
        }
        private DUOrderRS CreateObject_Product(NullHandler oReader)
        {
            DUOrderRS oDUOrderRS = new DUOrderRS();
            oDUOrderRS = MapObject_Product(oReader);
            return oDUOrderRS;
        }
        private List<DUOrderRS> CreateObjects_Product(IDataReader oReader)
        {
            List<DUOrderRS> oDUOrderRS = new List<DUOrderRS>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUOrderRS oItem = CreateObject_Product(oHandler);
                oDUOrderRS.Add(oItem);
            }
            return oDUOrderRS;
        }
        #endregion
        #endregion

        #region Interface implementation
        public DUOrderRSService() { }

        public List<DUOrderRS> Gets(string sDyeingOrderDetailID, int nRSID, int nOrderType, int nReportType, Int64 nUserID)
        {
            List<DUOrderRS> oDUOrderRS = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUOrderRSDA.Gets(tc, sDyeingOrderDetailID,  nRSID,  nOrderType,  nReportType);
                oDUOrderRS = CreateObjects(reader); 
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oDUOrderRS;
        }
        public List<DUOrderRS> Gets(string sSql, int nReportType, Int64 nUserID)
        {
            List<DUOrderRS> oDUOrderRS = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUOrderRSDA.Gets(tc, sSql, nReportType);
                oDUOrderRS = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oDUOrderRS;
        }
        public List<DUOrderRS> GetsQC(string sSql, int nReportType, Int64 nUserID)
        {
            List<DUOrderRS> oDUOrderRS = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUOrderRSDA.GetsQC(tc, sSql, nReportType);
                oDUOrderRS = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oDUOrderRS;
        }
        public List<DUOrderRS> GetsQCByRaqLot(int nRawLotID, int nReportType, Int64 nUserID)
        {
            List<DUOrderRS> oDUOrderRS = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUOrderRSDA.GetsQCByRaqLot(tc, nRawLotID, nReportType);
                oDUOrderRS = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oDUOrderRS;
        }

        #endregion
    }
}