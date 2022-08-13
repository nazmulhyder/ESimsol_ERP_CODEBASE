using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class DUOrderTrackerService : MarshalByRefObject, IDUOrderTrackerService
    {
        #region Private functions and declaration
        private DUOrderTracker MapObject(NullHandler oReader)
        {
            DUOrderTracker oDUOrderTracker = new DUOrderTracker();
            oDUOrderTracker.PTUID = oReader.GetInt32("PTUID");
            oDUOrderTracker.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDUOrderTracker.OrderType = oReader.GetInt16("OrderType");
            oDUOrderTracker.ProductID = oReader.GetInt32("ProductID");
            oDUOrderTracker.LabDipDetailID = oReader.GetInt32("LabDipDetailID");
            oDUOrderTracker.OrderNo = oReader.GetString("ColorName");
            oDUOrderTracker.ColorName = oReader.GetString("ColorName");
            oDUOrderTracker.PantonNo = oReader.GetString("PantonNo");
            oDUOrderTracker.ColorNo = oReader.GetString("ColorNo");
            oDUOrderTracker.LabdipNo = oReader.GetString("LabdipNo");
            oDUOrderTracker.Shade = oReader.GetInt16("Shade");
            oDUOrderTracker.BuyerID = oReader.GetInt32("BuyerID");
            oDUOrderTracker.ContractorID = oReader.GetInt32("ContractorID");
            oDUOrderTracker.State = oReader.GetInt16("State");
            oDUOrderTracker.Qty_PI = oReader.GetDouble("Qty_PI");
            oDUOrderTracker.OrderQty = oReader.GetDouble("OrderQty");
            oDUOrderTracker.Qty_ProIssue = oReader.GetDouble("Qty_ProIssue");
            oDUOrderTracker.Pro_PipeLineQty = oReader.GetDouble("ProductionPipeLineQty");
            oDUOrderTracker.ProductionFinishedQty = oReader.GetDouble("ProductionFinishedQty");
            oDUOrderTracker.YetToProduction = oReader.GetDouble("YetToProduction");
            oDUOrderTracker.YetToDelivery = oReader.GetDouble("YetToDelivery");
            oDUOrderTracker.ReadyStockInhand = oReader.GetDouble("ReadyStockInhand");
            oDUOrderTracker.StockInHand = oReader.GetDouble("StockInHand");
            oDUOrderTracker.ReturnQty = oReader.GetDouble("ReturnQty");
            oDUOrderTracker.ClaimOrderQty = oReader.GetDouble("ClaimOrderQty");
            oDUOrderTracker.Qty_SC = oReader.GetDouble("Qty_SC");
            oDUOrderTracker.ActualDeliveryQty = oReader.GetDouble("ActualDeliveryQty");
            //oDUOrderTracker.Shade = oReader.GetInt32("Shade");
            oDUOrderTracker.OrderNo = oReader.GetString("OrderNo");
            oDUOrderTracker.ContractorName = oReader.GetString("ContractorName");
            oDUOrderTracker.BuyerName = oReader.GetString("BuyerName");
            oDUOrderTracker.ProductName = oReader.GetString("ProductName");
            oDUOrderTracker.ProductCode = oReader.GetString("ProductCode");
            oDUOrderTracker.YarnCount = oReader.GetString("YarnCount");
            oDUOrderTracker.LCNo = oReader.GetString("LCNo");
            oDUOrderTracker.MKT = oReader.GetString("MKT");
            oDUOrderTracker.PINo = oReader.GetString("PINo");

            oDUOrderTracker.RowNo = oReader.GetInt32("RowNo");

            oDUOrderTracker.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oDUOrderTracker.OrderDate = oReader.GetDateTime("OrderDate");
            if (oDUOrderTracker.RowNo > 1 )//&& (oDUOrderTracker.OrderType == (int)EnumOrderType.BulkOrder || oDUOrderTracker.OrderType == (int)EnumOrderType.DyeingOnly))
            {
                oDUOrderTracker.Qty_PI = 0;
            }
            
            return oDUOrderTracker;
        }

        private DUOrderTracker CreateObject(NullHandler oReader)
        {
            DUOrderTracker oDUOrderTracker = new DUOrderTracker();
            oDUOrderTracker = MapObject(oReader);
            return oDUOrderTracker;
        }

        private List<DUOrderTracker> CreateObjects(IDataReader oReader)
        {
            List<DUOrderTracker> oDUOrderTracker = new List<DUOrderTracker>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUOrderTracker oItem = CreateObject(oHandler);
                oDUOrderTracker.Add(oItem);
            }
            return oDUOrderTracker;
        }

        #endregion

        #region Interface implementation
        public DUOrderTrackerService() { }

     
        public List<DUOrderTracker> Gets(string sSQL, int nReportType, bool bIsSample, Int64 nUserID)
        {
            List<DUOrderTracker> oDUOrderTrackers = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if (bIsSample)
                {
                    reader = DUOrderTrackerDA.Gets_Sample(tc, sSQL, nReportType);
                }
                else
                {
                    reader = DUOrderTrackerDA.Gets(tc, sSQL, nReportType);
                }
                
                oDUOrderTrackers = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUOrderTrackers", e);
                #endregion
            }

            return oDUOrderTrackers;
        }

   
       
  
     
        #endregion


     
    }
}
