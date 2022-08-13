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

    public class DUDeliveryStockService : MarshalByRefObject, IDUDeliveryStockService
    {
        #region Private functions and declaration
        DateTime dDate = new DateTime();
        double nUnManageQty = 0;
        private DUDeliveryStock MapObject(NullHandler oReader)
        {
            DUDeliveryStock oDUDeliveryStock = new DUDeliveryStock();
            oDUDeliveryStock.LotNo = oReader.GetString("LotNo");
            dDate = oReader.GetDateTime("RSDate");
            oDUDeliveryStock.RSDate = dDate.ToString("dd MMM yyyy");
            oDUDeliveryStock.Qty = oReader.GetDouble("Qty");
            oDUDeliveryStock.Qty_Tr = oReader.GetDouble("Qty");
            oDUDeliveryStock.Product = oReader.GetString("ProductName");
            oDUDeliveryStock.OrderNo = oReader.GetString("OrderNo");
            oDUDeliveryStock.PINo = oReader.GetString("PINo");
            oDUDeliveryStock.FactN = oReader.GetString("ContractorName");
            oDUDeliveryStock.Buyer = oReader.GetString("BuyerName");
            oDUDeliveryStock.WorkingUnit = oReader.GetString("WUName");
            oDUDeliveryStock.WorkingUnitID = oReader.GetInt32("WUID");
            oDUDeliveryStock.LotID = oReader.GetInt32("LotID");
            oDUDeliveryStock.OrderType = oReader.GetInt32("OrderType");
            nUnManageQty = oReader.GetDouble("UnManagedQty");
            if(nUnManageQty>0)
            {
                oDUDeliveryStock.IsManage = "Un Manage";
            }
            else
            {
                oDUDeliveryStock.IsManage = "Manage";
            }
          
            return oDUDeliveryStock;
        }

        private DUDeliveryStock CreateObject(NullHandler oReader)
        {
            DUDeliveryStock oDUDeliveryStock = new DUDeliveryStock();
            oDUDeliveryStock = MapObject(oReader);
            return oDUDeliveryStock;
        }

        private List<DUDeliveryStock> CreateObjects(IDataReader oReader)
        {
            List<DUDeliveryStock> oDUDeliveryStock = new List<DUDeliveryStock>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDeliveryStock oItem = CreateObject(oHandler);
                oDUDeliveryStock.Add(oItem);
            }
            return oDUDeliveryStock;
        }

        #endregion

        #region Interface implementation
        public DUDeliveryStockService() { }

        public List<DUDeliveryStock> Gets(int nOrderType, int nWorkingUnitID,string sSQL, Int64 nUserId)
        {
            List<DUDeliveryStock> oDUDeliveryStocks = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryStockDA.Gets(tc, nOrderType,  nWorkingUnitID, sSQL);
                oDUDeliveryStocks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryStock", e);
                #endregion
            }

            return oDUDeliveryStocks;
        }

        public string SendToRequsitionToDelivery(DUDeliveryStock oDUDeliveryStock, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                DUDeliveryStockDA.SendToRequsitionToDelivery(tc, oDUDeliveryStock, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return "";
        }
        public List<DUDeliveryStock> GetsAvalnDelivery(int nOrderType, int nWorkingUnitID, string sSQL, Int64 nUserId)
        {
            List<DUDeliveryStock> oDUDeliveryStocks = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryStockDA.GetsAvalnDelivery(tc, nOrderType, nWorkingUnitID, sSQL);
                oDUDeliveryStocks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryStock", e);
                #endregion
            }

            return oDUDeliveryStocks;
        }

        #endregion
    }    
    
  
}
