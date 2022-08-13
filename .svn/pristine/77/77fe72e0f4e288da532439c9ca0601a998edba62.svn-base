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

    public class DUDeliveryLotService : MarshalByRefObject, IDUDeliveryLotService
    {
        #region Private functions and declaration
        #region LotWise
        DateTime dDate = new DateTime();
        private DUDeliveryLot MapObject(NullHandler oReader)
        {
            DUDeliveryLot oDUDeliveryLot = new DUDeliveryLot();
            oDUDeliveryLot.LotNo = oReader.GetString("LotNo");
            dDate = oReader.GetDateTime("RSDate");
            oDUDeliveryLot.RSDate = dDate.ToString("dd MMM yyyy");
            oDUDeliveryLot.Balance = oReader.GetDouble("Balance");
            oDUDeliveryLot.Qty_DC = oReader.GetDouble("Qty_DC");
            oDUDeliveryLot.Qty_Order = oReader.GetDouble("Qty_Order");
            oDUDeliveryLot.Product = oReader.GetString("ProductName");
            oDUDeliveryLot.OrderNo = oReader.GetString("OrderNo");
            oDUDeliveryLot.Contractor = oReader.GetString("ContractorName");
            oDUDeliveryLot.ColorName = oReader.GetString("ColorName");
            oDUDeliveryLot.OrderType = oReader.GetInt32("OrderType");
            oDUDeliveryLot.OrderTypeSt = oReader.GetString("OrderTypeSt");
            oDUDeliveryLot.PINo = oReader.GetString("PINo");
            oDUDeliveryLot.LCNo = oReader.GetString("LCNo");
            oDUDeliveryLot.LotID = oReader.GetInt32("LotID");
            oDUDeliveryLot.LotLocationID = oReader.GetInt32("LotLocationID");
            oDUDeliveryLot.BagNo = oReader.GetInt32("BagNo");
            oDUDeliveryLot.LocationName = oReader.GetString("LocationName");

            //DUHardWindingStock
            oDUDeliveryLot.Qty_Batch = oReader.GetDouble("Qty_Batch");
            oDUDeliveryLot.Qty_Warp = oReader.GetDouble("Qty_Warp");
            oDUDeliveryLot.Qty_Weft = oReader.GetDouble("Qty_Weft");
            oDUDeliveryLot.DOID = oReader.GetInt32("DOID");
            oDUDeliveryLot.DODID = oReader.GetInt32("DODID");

            return oDUDeliveryLot;
        }

        private DUDeliveryLot CreateObject(NullHandler oReader)
        {
            DUDeliveryLot oDUDeliveryLot = new DUDeliveryLot();
            oDUDeliveryLot = MapObject(oReader);
            return oDUDeliveryLot;
        }

        private List<DUDeliveryLot> CreateObjects(IDataReader oReader)
        {
            List<DUDeliveryLot> oDUDeliveryLot = new List<DUDeliveryLot>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDeliveryLot oItem = CreateObject(oHandler);
                oDUDeliveryLot.Add(oItem);
            }
            return oDUDeliveryLot;
        }
        #endregion
        #region ProductWise
        private DUDeliveryLot MapObject_Product(NullHandler oReader)
        {
            DUDeliveryLot oDUDeliveryLot = new DUDeliveryLot();
          
            oDUDeliveryLot.Balance = oReader.GetDouble("Balance");
            oDUDeliveryLot.Qty_DC = oReader.GetDouble("Qty_DC");
            oDUDeliveryLot.Product = oReader.GetString("ProductName");
            oDUDeliveryLot.Qty_Order = oReader.GetDouble("Qty_Order");

            return oDUDeliveryLot;
        }

        private DUDeliveryLot CreateObject_Product(NullHandler oReader)
        {
            DUDeliveryLot oDUDeliveryLot = new DUDeliveryLot();
            oDUDeliveryLot = MapObject_Product(oReader);
            return oDUDeliveryLot;
        }

        private List<DUDeliveryLot> CreateObjects_Product(IDataReader oReader)
        {
            List<DUDeliveryLot> oDUDeliveryLot = new List<DUDeliveryLot>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDeliveryLot oItem = CreateObject_Product(oHandler);
                oDUDeliveryLot.Add(oItem);
            }
            return oDUDeliveryLot;
        }
        #endregion
        #region BuyerWise
       
        private DUDeliveryLot MapObject_Buyer(NullHandler oReader)
        {
            DUDeliveryLot oDUDeliveryLot = new DUDeliveryLot();

            oDUDeliveryLot.Balance = oReader.GetDouble("Balance");
            oDUDeliveryLot.Qty_DC = oReader.GetDouble("Qty_DC");
            oDUDeliveryLot.Contractor = oReader.GetString("ContractorName");
            oDUDeliveryLot.Qty_Order = oReader.GetDouble("Qty_Order");


            return oDUDeliveryLot;
        }

        private DUDeliveryLot CreateObject_Buyer(NullHandler oReader)
        {
            DUDeliveryLot oDUDeliveryLot = new DUDeliveryLot();
            oDUDeliveryLot = MapObject_Buyer(oReader);
            return oDUDeliveryLot;
        }

        private List<DUDeliveryLot> CreateObjects_Buyer(IDataReader oReader)
        {
            List<DUDeliveryLot> oDUDeliveryLot = new List<DUDeliveryLot>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDeliveryLot oItem = CreateObject_Buyer(oHandler);
                oDUDeliveryLot.Add(oItem);
            }
            return oDUDeliveryLot;
        }
        #endregion
        #endregion

        #region Interface implementation
        public DUDeliveryLotService() { }
        
        public List<DUDeliveryLot> Gets(int nOrderType, int nWorkingUnitID, int nReportType, Int64 nUserId)
        {
            List<DUDeliveryLot> oDUDeliveryLots = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryLotDA.Gets(tc, nOrderType, nWorkingUnitID, nReportType);
             
                if (nReportType == 1)
                { oDUDeliveryLots = CreateObjects(reader); }
                if (nReportType == 2)
                { oDUDeliveryLots = CreateObjects_Product(reader); }
                if (nReportType == 3)
                { oDUDeliveryLots = CreateObjects_Buyer(reader); }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryLot", e);
                #endregion
            }

            return oDUDeliveryLots;
        }

        public List<DUDeliveryLot> GetsFromAdv(string sSQL, int nReportType, Int64 nUserId)
        {
            List<DUDeliveryLot> oDUDeliveryLots = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryLotDA.GetsFromAdv(tc, sSQL, nReportType);
             
                if (nReportType == 1)
                { oDUDeliveryLots = CreateObjects(reader); }
                if (nReportType == 2)
                { oDUDeliveryLots = CreateObjects_Product(reader); }
                if (nReportType == 3)
                { oDUDeliveryLots = CreateObjects_Buyer(reader); }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryLot", e);
                #endregion
            }

            return oDUDeliveryLots;
        }
        public List<DUDeliveryLot> GetsDUHardWindingStock(string sSql, int nReportType, Int64 nUserId)
        {
            List<DUDeliveryLot> oDUDeliveryLots = new List<DUDeliveryLot>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DUDeliveryLotDA.GetsDUHardWindingStock(tc, sSql, nReportType);
                oDUDeliveryLots = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUHardWinding Stock  ", e);

                #endregion
            }
            return oDUDeliveryLots;
        }
        public List<DUDeliveryLot> Gets(string sSQL, int nUserId)
        {
            List<DUDeliveryLot> oDUDeliveryLots = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryLotDA.Gets(tc, sSQL);
                oDUDeliveryLots = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryLots", e);
                #endregion
            }

            return oDUDeliveryLots;
        }
        #endregion
    }    
    
  
}
