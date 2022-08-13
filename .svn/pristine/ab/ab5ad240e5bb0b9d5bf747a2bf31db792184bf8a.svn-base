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
    public class RawmaterialStatusService : MarshalByRefObject, IRawmaterialStatusService
    {
        #region Private functions and declaration
        private RawmaterialStatus MapObject(NullHandler oReader)
        {
            RawmaterialStatus oRawmaterialStatus = new RawmaterialStatus();
            oRawmaterialStatus.SaleOrderID = oReader.GetInt32("SaleOrderID");            
            oRawmaterialStatus.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oRawmaterialStatus.SaleOrderNo = oReader.GetString("SaleOrderNo");
            oRawmaterialStatus.ProductionFactoryName = oReader.GetString("ProductionFactoryName");
            oRawmaterialStatus.StyleNo = oReader.GetString("StyleNo");
            oRawmaterialStatus.PINo = oReader.GetString("PINo");
            oRawmaterialStatus.ProductID = oReader.GetInt32("ProductID");
            oRawmaterialStatus.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oRawmaterialStatus.LabDipOrderDetailID = oReader.GetInt32("LabDipOrderDetailID");
            oRawmaterialStatus.ProductName = oReader.GetString("ProductName");
            oRawmaterialStatus.SupplierID = oReader.GetInt32("SupplierID");
            oRawmaterialStatus.SupplierName = oReader.GetString("SupplierName");
            oRawmaterialStatus.PIReceiveDate = oReader.GetDateTime("PIReceiveDate");
            oRawmaterialStatus.LCDate = oReader.GetDateTime("LCDate");
            oRawmaterialStatus.DyeingOrderNo = oReader.GetString("DyeingOrderNo");
            oRawmaterialStatus.ColorName = oReader.GetString("ColorName");
            oRawmaterialStatus.OrderQty = oReader.GetDouble("OrderQty");
            oRawmaterialStatus.UnitID = oReader.GetInt32("UnitID");
            oRawmaterialStatus.UnitName = oReader.GetString("UnitName");
            oRawmaterialStatus.ReceiveQty = oReader.GetDouble("ReceiveQty");
            oRawmaterialStatus.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oRawmaterialStatus.Balance = oReader.GetDouble("Balance");
            oRawmaterialStatus.Remarks = oReader.GetString("Remarks");
            oRawmaterialStatus.BuyerName = oReader.GetString("BuyerName");
            oRawmaterialStatus.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oRawmaterialStatus.SaleOrderQty = oReader.GetDouble("SaleOrderQty");
            oRawmaterialStatus.YarnCount = oReader.GetString("YarnCount");
            oRawmaterialStatus.GG = oReader.GetString("GG");
            oRawmaterialStatus.ColorTrackNo = oReader.GetInt32("ColorTrackNo");
            oRawmaterialStatus.ColorTrackYr = oReader.GetString("ColorTrackYr");
            oRawmaterialStatus.ApproveShadeName = oReader.GetString("ApproveShadeName");
            oRawmaterialStatus.ReLabNo = oReader.GetInt32("ReLabNo");
            oRawmaterialStatus.ReLabOn = oReader.GetInt32("ReLabOn");
            oRawmaterialStatus.ConSumptionWestage = oReader.GetString("ConSumptionWestage");
            oRawmaterialStatus.ChallanNoLotNo = oReader.GetString("ChallanNoLotNo");
            oRawmaterialStatus.SaleOrderQty = oReader.GetDouble("SaleOrderQty");
            oRawmaterialStatus.ApproveShade = oReader.GetInt32("ApproveShade");
            return oRawmaterialStatus;
        }

        private RawmaterialStatus CreateObject(NullHandler oReader)
        {
            RawmaterialStatus oRawmaterialStatus = new RawmaterialStatus();
            oRawmaterialStatus = MapObject(oReader);
            return oRawmaterialStatus;
        }

        private List<RawmaterialStatus> CreateObjects(IDataReader oReader)
        {
            List<RawmaterialStatus> oRawmaterialStatus = new List<RawmaterialStatus>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RawmaterialStatus oItem = CreateObject(oHandler);
                oRawmaterialStatus.Add(oItem);
            }
            return oRawmaterialStatus;
        }

        #endregion


        #region Map for Acccesores 
        private RawmaterialStatus AccesoriesMapObject(NullHandler oReader)
        {
            RawmaterialStatus oRawmaterialStatus = new RawmaterialStatus();
            oRawmaterialStatus.SaleOrderID = oReader.GetInt32("SaleOrderID");
            oRawmaterialStatus.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oRawmaterialStatus.SaleOrderNo = oReader.GetString("SaleOrderNo");
            oRawmaterialStatus.ProductionFactoryName = oReader.GetString("ProductionFactoryName");
            oRawmaterialStatus.StyleNo = oReader.GetString("StyleNo");
            oRawmaterialStatus.ProductID = oReader.GetInt32("ProductID");
            oRawmaterialStatus.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oRawmaterialStatus.ProductName = oReader.GetString("ProductName");
            oRawmaterialStatus.SupplierID = oReader.GetInt32("SupplierID");
            oRawmaterialStatus.SupplierName = oReader.GetString("SupplierName");
            oRawmaterialStatus.LCDate = oReader.GetDateTime("LCDate");
            oRawmaterialStatus.ColorName = oReader.GetString("ColorName");
            oRawmaterialStatus.OrderQty = oReader.GetDouble("OrderQty");
            oRawmaterialStatus.UnitID = oReader.GetInt32("UnitID");
            oRawmaterialStatus.UnitName = oReader.GetString("UnitName");
            oRawmaterialStatus.ReceiveQty = oReader.GetDouble("ReceiveQty");
            oRawmaterialStatus.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oRawmaterialStatus.Balance = oReader.GetDouble("Balance");
            oRawmaterialStatus.Remarks = oReader.GetString("Remarks");
            oRawmaterialStatus.BuyerName = oReader.GetString("BuyerName");
            oRawmaterialStatus.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oRawmaterialStatus.SaleOrderQty = oReader.GetDouble("SaleOrderQty");
            oRawmaterialStatus.YarnCount = oReader.GetString("YarnCount");
            oRawmaterialStatus.GG = oReader.GetString("GG");
            oRawmaterialStatus.ChallanNoLotNo = oReader.GetString("ChallanNoLotNo");
            oRawmaterialStatus.SaleOrderQty = oReader.GetDouble("SaleOrderQty");
            oRawmaterialStatus.WorkOrderNo = oReader.GetString("WorkOrderNo");
            return oRawmaterialStatus;
        }

        private RawmaterialStatus AccesoriesCreateObject(NullHandler oReader)
        {
            RawmaterialStatus oRawmaterialStatus = new RawmaterialStatus();
            oRawmaterialStatus = AccesoriesMapObject(oReader);
            return oRawmaterialStatus;
        }

        private List<RawmaterialStatus> AccessoriesCreateObjects(IDataReader oReader)
        {
            List<RawmaterialStatus> oRawmaterialStatus = new List<RawmaterialStatus>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RawmaterialStatus oItem = AccesoriesCreateObject(oHandler);
                oRawmaterialStatus.Add(oItem);
            }
            return oRawmaterialStatus;
        }

        #endregion

        #region Interface implementation
        public RawmaterialStatusService() { }



        public List<RawmaterialStatus> GetYarnBySaleOrderIDs(string sSaleOrderIDs, Int64 nUserID)
        {
            List<RawmaterialStatus> oRawmaterialStatus = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RawmaterialStatusDA.GetYarnBySaleOrderIDs(sSaleOrderIDs, tc);
                oRawmaterialStatus = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RawmaterialStatus", e);
                #endregion
            }

            return oRawmaterialStatus;
        }


        public List<RawmaterialStatus> GetAccessoriesBySaleOrderIDs(string sSaleOrderIDs, Int64 nUserID)
        {
            List<RawmaterialStatus> oRawmaterialStatus = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RawmaterialStatusDA.GetAccessoriesBySaleOrderIDs(sSaleOrderIDs, tc);
                oRawmaterialStatus = AccessoriesCreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RawmaterialStatus", e);
                #endregion
            }

            return oRawmaterialStatus;
        }
        
        #endregion
    }
    

}
