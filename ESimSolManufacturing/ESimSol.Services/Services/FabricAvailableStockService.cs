using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class FabricAvailableStockService : MarshalByRefObject, IFabricAvailableStockService
    {
        #region Private functions and declaration

        private FabricAvailableStock MapObject(NullHandler oReader)
        {
            FabricAvailableStock oFabricAvailableStock = new FabricAvailableStock();
            oFabricAvailableStock.FSCDID = oReader.GetInt32("FSCDID");
            oFabricAvailableStock.FSCID = oReader.GetInt32("FSCID");
            oFabricAvailableStock.PONo = oReader.GetString("PONo");
            oFabricAvailableStock.PODate = oReader.GetDateTime("PODate");
            oFabricAvailableStock.DispoNo = oReader.GetString("DispoNo");
            oFabricAvailableStock.DispoQty = oReader.GetDouble("DispoQty");
            oFabricAvailableStock.ProductID = oReader.GetInt32("ProductID");
            oFabricAvailableStock.ProductName = oReader.GetString("ProductName");
            oFabricAvailableStock.BuyerID = oReader.GetInt32("BuyerID");
            oFabricAvailableStock.BuyerName = oReader.GetString("BuyerName");
            oFabricAvailableStock.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFabricAvailableStock.ColorName = oReader.GetString("ColorName");
            oFabricAvailableStock.MUnitID = oReader.GetInt32("MUnitID");
            oFabricAvailableStock.LotID = oReader.GetInt32("LotID");
            oFabricAvailableStock.LotNo = oReader.GetString("LotNo");
            oFabricAvailableStock.LotBalance = oReader.GetDouble("LotBalance");
            oFabricAvailableStock.WUName = oReader.GetString("WUName");
            oFabricAvailableStock.WUID = oReader.GetInt32("WUID");
            oFabricAvailableStock.FNBatchQCDetailID = oReader.GetInt32("FNBatchQCDetailID");
            oFabricAvailableStock.FNBatchQCID = oReader.GetInt32("FNBatchQCID");
            oFabricAvailableStock.QCQty = oReader.GetDouble("QCQty");
            oFabricAvailableStock.Construction = oReader.GetString("Construction");
            oFabricAvailableStock.Composition = oReader.GetString("Composition");
            oFabricAvailableStock.FabricID = oReader.GetInt32("FabricID");
            oFabricAvailableStock.FabricNo = oReader.GetString("FabricNo");
            oFabricAvailableStock.FinishType = oReader.GetInt32("FinishType");
            oFabricAvailableStock.FinishTypeName = oReader.GetString("FinishTypeName");
            oFabricAvailableStock.RollQty = oReader.GetInt32("RollQty");
            oFabricAvailableStock.WeaveType = oReader.GetInt32("WeaveType");
            oFabricAvailableStock.WeaveTypeName = oReader.GetString("WeaveTypeName");

            return oFabricAvailableStock;
        }

        private FabricAvailableStock CreateObject(NullHandler oReader)
        {
            FabricAvailableStock oFabricAvailableStock = new FabricAvailableStock();
            oFabricAvailableStock = MapObject(oReader);
            return oFabricAvailableStock;
        }

        private List<FabricAvailableStock> CreateObjects(IDataReader oReader)
        {
            List<FabricAvailableStock> oFabricAvailableStock = new List<FabricAvailableStock>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricAvailableStock oItem = CreateObject(oHandler);
                oFabricAvailableStock.Add(oItem);
            }
            return oFabricAvailableStock;
        }

        #endregion

        #region Interface implementation
        public FabricAvailableStock Get(int id, Int64 nUserId)
        {
            FabricAvailableStock oFabricAvailableStock = new FabricAvailableStock();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricAvailableStockDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricAvailableStock = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricAvailableStock", e);
                #endregion
            }
            return oFabricAvailableStock;
        }

        public List<FabricAvailableStock> Gets(Int64 nUserID)
        {
            List<FabricAvailableStock> oFabricAvailableStocks = new List<FabricAvailableStock>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricAvailableStockDA.Gets(tc);
                oFabricAvailableStocks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricAvailableStock oFabricAvailableStock = new FabricAvailableStock();
                oFabricAvailableStock.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricAvailableStocks;
        }

        public List<FabricAvailableStock> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricAvailableStock> oFabricAvailableStocks = new List<FabricAvailableStock>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricAvailableStockDA.Gets(tc, sSQL);
                oFabricAvailableStocks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricAvailableStock", e);
                #endregion
            }
            return oFabricAvailableStocks;
        }

        #endregion
    }

}
