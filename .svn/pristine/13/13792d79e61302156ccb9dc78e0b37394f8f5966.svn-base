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


    public class OrderDetailsSummaryService : MarshalByRefObject, IOrderDetailsSummaryService
    {
        #region Private functions and declaration
        private OrderDetailsSummary MapObject(NullHandler oReader)
        {
            OrderDetailsSummary oOrderDetailsSummary = new OrderDetailsSummary();

            oOrderDetailsSummary.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oOrderDetailsSummary.OrderRecapDate = oReader.GetDateTime("OrderRecapDate");
            oOrderDetailsSummary.StyleNo = oReader.GetString("StyleNo");
            oOrderDetailsSummary.BuyerName = oReader.GetString("BuyerName");
            oOrderDetailsSummary.RecapNo = oReader.GetString("RecapNo");
            oOrderDetailsSummary.RecapQty = oReader.GetDouble("RecapQty");
            oOrderDetailsSummary.RecapValue = oReader.GetDouble("RecapValue");
            oOrderDetailsSummary.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oOrderDetailsSummary.ODSNo = oReader.GetString("ODSNo");
            oOrderDetailsSummary.ONSNo = oReader.GetString("ONSNo");
            oOrderDetailsSummary.ExportPINo = oReader.GetString("ExportPINo");
            oOrderDetailsSummary.MasterLCNo = oReader.GetString("MasterLCNo");
            oOrderDetailsSummary.LCTransferNo = oReader.GetString("LCTransferNo");
            oOrderDetailsSummary.ProductionOrderQty = oReader.GetDouble("ProductionOrderQty");
            oOrderDetailsSummary.ProductionQty = oReader.GetDouble("ProductionQty");
            oOrderDetailsSummary.QCStatus = oReader.GetString("QCStatus");
            oOrderDetailsSummary.ShipmentQty = oReader.GetDouble("ShipmentQty");
            oOrderDetailsSummary.ComercialInvoiceQty = oReader.GetDouble("ComercialInvoiceQty");
            oOrderDetailsSummary.YarnValue = oReader.GetDouble("YarnValue");
            oOrderDetailsSummary.AccessoriesValue = oReader.GetDouble("AccessoriesValue");
            oOrderDetailsSummary.CMValue = oReader.GetDouble("CMValue");
            oOrderDetailsSummary.EndosmentCommission = oReader.GetDouble("EndosmentCommission");
            oOrderDetailsSummary.B2BCommission = oReader.GetDouble("B2BCommission");
            oOrderDetailsSummary.TotalCommission = oReader.GetDouble("TotalCommission");
            oOrderDetailsSummary.CommissionRealise = oReader.GetDouble("CommissionRealise");
            return oOrderDetailsSummary;
        }

        private OrderDetailsSummary CreateObject(NullHandler oReader)
        {
            OrderDetailsSummary oOrderDetailsSummary = new OrderDetailsSummary();
            oOrderDetailsSummary = MapObject(oReader);
            return oOrderDetailsSummary;
        }

        private List<OrderDetailsSummary> CreateObjects(IDataReader oReader)
        {
            List<OrderDetailsSummary> oOrderDetailsSummary = new List<OrderDetailsSummary>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                OrderDetailsSummary oItem = CreateObject(oHandler);
                oOrderDetailsSummary.Add(oItem);
            }
            return oOrderDetailsSummary;
        }

        #endregion

        #region Interface implementation
        public List<OrderDetailsSummary> Gets(string sIDs, Int64 nUserID)
        {
            List<OrderDetailsSummary> oOrderDetailsSummary = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OrderDetailsSummaryDA.Gets(tc, sIDs);
                oOrderDetailsSummary = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get OrderDetailsSummary", e);
                #endregion
            }

            return oOrderDetailsSummary;
        }

        #endregion
    }   
    
  
}
