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
    public class OrderTrackingService : MarshalByRefObject, IOrderTrackingService
    {
        #region Private functions and declaration
        private OrderTracking MapObject(NullHandler oReader)
        {
            OrderTracking oOrderTracking = new OrderTracking();
            oOrderTracking.ExportSCDetailID = oReader.GetInt32("ExportSCDetailID");
            oOrderTracking.ExportSCID = oReader.GetInt32("ExportSCID");
            oOrderTracking.ExportPIDetailID = oReader.GetInt32("ExportPIDetailID");
            oOrderTracking.ExportPIID = oReader.GetInt32("ExportPIID");
            oOrderTracking.ColorID = oReader.GetInt32("ColorID");
            oOrderTracking.ProductID = oReader.GetInt32("ProductID");
            oOrderTracking.PIDate = oReader.GetDateTime("PIDate");

            oOrderTracking.ColorName = oReader.GetString("ColorName");
            oOrderTracking.DODate = oReader.GetDateTime("DODate");
            oOrderTracking.ExportLCFileNo = oReader.GetString("ExportLCFileNo");
            oOrderTracking.SizeName = oReader.GetString("SizeName");
            oOrderTracking.BuyerID = oReader.GetInt32("BuyerID");
            oOrderTracking.ContractorID = oReader.GetInt32("ContractorID");
            oOrderTracking.PIQty = oReader.GetDouble("PIQty");
            oOrderTracking.POQty = oReader.GetDouble("POQty");
            oOrderTracking.DOQty = oReader.GetDouble("DOQty");
            oOrderTracking.YetToDO = oReader.GetDouble("YetToDO");
            oOrderTracking.ChallanQty = oReader.GetDouble("ChallanQty");
            oOrderTracking.TotalWeight = oReader.GetDouble("TotalWeight");
            oOrderTracking.HangerWeight = oReader.GetDouble("HangerWeight");
            oOrderTracking.StockQty = oReader.GetDouble("StockQty");
            oOrderTracking.YetToChallanQty = oReader.GetDouble("YetToChallanQty");
            oOrderTracking.YetToPO = oReader.GetDouble("YetToPO");
            oOrderTracking.WastagePercentWithWeight = oReader.GetDouble("WastagePercentWithWeight");
            oOrderTracking.ContractorName = oReader.GetString("ContractorName");
            oOrderTracking.BuyerName = oReader.GetString("BuyerName");
            oOrderTracking.ProductName = oReader.GetString("ProductName");
            oOrderTracking.ProductCode = oReader.GetString("ProductCode");
            oOrderTracking.ExportLCNo = oReader.GetString("ExportLCNo");
            oOrderTracking.StyleRef = oReader.GetString("StyleRef");
            oOrderTracking.PINo = oReader.GetString("PINo");
            oOrderTracking.Remarks = oReader.GetString("Remarks");
            oOrderTracking.BUID = oReader.GetInt32("BUID");
            oOrderTracking.MUName = oReader.GetString("MUName");
            oOrderTracking.StockUnitName = oReader.GetString("StockUnitName");
            oOrderTracking.PSQty = oReader.GetDouble("PSQty");
            oOrderTracking.PipeLineQty = oReader.GetDouble("PipeLineQty");
            oOrderTracking.ReturnQty = oReader.GetDouble("ReturnQty");
            oOrderTracking.PTUTransferQty = oReader.GetDouble("PTUTransferQty");
            oOrderTracking.PTUTransferUnitName =oReader.GetString("PTUTransferUnitName");
            return oOrderTracking;
        }

        private OrderTracking CreateObject(NullHandler oReader)
        {
            OrderTracking oOrderTracking = new OrderTracking();
            oOrderTracking = MapObject(oReader);
            return oOrderTracking;
        }

        private List<OrderTracking> CreateObjects(IDataReader oReader)
        {
            List<OrderTracking> oOrderTracking = new List<OrderTracking>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                OrderTracking oItem = CreateObject(oHandler);
                oOrderTracking.Add(oItem);
            }
            return oOrderTracking;
        }

        #endregion

        #region Interface implementation
        public OrderTrackingService() { }


        public List<OrderTracking> Gets(string sSQL, int nLayoutType, bool bIsYetToDO, bool bIsYetToChallan, bool bIsYetToChallanWithDOEntry, bool bIsPTUTransferQty, Int64 nUserID)
        {
            List<OrderTracking> oOrderTrackings = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OrderTrackingDA.Gets(tc, sSQL, nLayoutType, bIsYetToDO, bIsYetToChallan, bIsYetToChallanWithDOEntry, bIsPTUTransferQty);
                oOrderTrackings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get OrderTrackings", e);
                #endregion
            }

            return oOrderTrackings;
        }

   
       
  
     
        #endregion


     
    }
}
