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
    public class WeavingYarnStockService : MarshalByRefObject, IWeavingYarnStockService
    {
        #region Private functions and declaration

        private WeavingYarnStock MapObject(NullHandler oReader)
        {
            WeavingYarnStock oWeavingYarnStock = new WeavingYarnStock();
            oWeavingYarnStock.FSCID = oReader.GetInt32("FSCID");
            oWeavingYarnStock.FSCDetailID = oReader.GetInt32("FSCDetailID");
            oWeavingYarnStock.FEOSID = oReader.GetInt32("FEOSID");
            oWeavingYarnStock.Qty = oReader.GetDouble("Qty");
            oWeavingYarnStock.ExeNo = oReader.GetString("ExeNo");
            oWeavingYarnStock.ExeDate = oReader.GetDateTime("ExeDate");
            oWeavingYarnStock.SCNoFull = oReader.GetString("SCNoFull");
            oWeavingYarnStock.Qty_Dispo = oReader.GetDouble("Qty_Dispo");
            oWeavingYarnStock.Qty_Order = oReader.GetDouble("Qty_Order");
            oWeavingYarnStock.BuyerID = oReader.GetInt32("BuyerID");
            oWeavingYarnStock.BuyerName = oReader.GetString("BuyerName");
            oWeavingYarnStock.ContractorID = oReader.GetInt32("ContractorID");
            oWeavingYarnStock.ContractorName = oReader.GetString("ContractorName");
            oWeavingYarnStock.WarpCount = oReader.GetString("WarpCount");
            oWeavingYarnStock.WeftCount = oReader.GetString("WeftCount");
            oWeavingYarnStock.RequiredWarpLength = oReader.GetDouble("RequiredWarpLength");
            oWeavingYarnStock.RequiredWarpLengthLB = oReader.GetDouble("RequiredWarpLengthLB");
            oWeavingYarnStock.TotalWarpProduction = oReader.GetDouble("TotalWarpProduction");
            oWeavingYarnStock.ProductID = oReader.GetInt32("ProductID");
            oWeavingYarnStock.ProductName = oReader.GetString("ProductName");
            oWeavingYarnStock.Construction = oReader.GetString("Construction");
            oWeavingYarnStock.StyleNo = oReader.GetString("StyleNo");
            oWeavingYarnStock.FabricID = oReader.GetInt32("FabricID");
            oWeavingYarnStock.FabricNo = oReader.GetString("Qty_Order");
            oWeavingYarnStock.GreyYarnReqWarp = oReader.GetDouble("GreyYarnReqWarp");
            oWeavingYarnStock.DyedYarnReqWarp = oReader.GetDouble("DyedYarnReqWarp");
            oWeavingYarnStock.GreyYarnReqWeft = oReader.GetDouble("GreyYarnReqWeft");
            oWeavingYarnStock.DyedYarnReqWeft = oReader.GetDouble("DyedYarnReqWeft");
            oWeavingYarnStock.ReqDyedYarn = oReader.GetDouble("ReqDyedYarn");
            oWeavingYarnStock.ReqGreyFabrics = oReader.GetDouble("ReqGreyFabrics");
            oWeavingYarnStock.TotalGreyProduction = oReader.GetDouble("TotalGreyProduction");
            oWeavingYarnStock.ReqFinishedFabrics = oReader.GetDouble("ReqFinishedFabrics");
            oWeavingYarnStock.ColorWarp = oReader.GetInt32("ColorWarp");
            oWeavingYarnStock.ColorWeft = oReader.GetInt32("ColorWeft");
            oWeavingYarnStock.SWQty = oReader.GetDouble("SWQty");
            oWeavingYarnStock.WYReqWarp = oReader.GetDouble("WYReqWarp");
            oWeavingYarnStock.WYReqWeft = oReader.GetDouble("WYReqWeft");
            oWeavingYarnStock.BeamID = oReader.GetInt32("BeamID");
            oWeavingYarnStock.BeamNo = oReader.GetString("BeamNo");
            oWeavingYarnStock.LoomNo = oReader.GetString("LoomNo");
            oWeavingYarnStock.Shade = oReader.GetString("Shade");
            oWeavingYarnStock.ShadeWarp = oReader.GetString("ShadeWarp");
            oWeavingYarnStock.Remarks = oReader.GetString("Remarks");
            oWeavingYarnStock.StoreRcvQty = oReader.GetDouble("StoreRcvQty");
            oWeavingYarnStock.WeavingRcvQty = oReader.GetDouble("WeavingRcvQty");
            oWeavingYarnStock.DyeProductionQty = oReader.GetDouble("DyeProductionQty");

            return oWeavingYarnStock;
        }

        private WeavingYarnStock CreateObject(NullHandler oReader)
        {
            WeavingYarnStock oWeavingYarnStock = new WeavingYarnStock();
            oWeavingYarnStock = MapObject(oReader);
            return oWeavingYarnStock;
        }

        private List<WeavingYarnStock> CreateObjects(IDataReader oReader)
        {
            List<WeavingYarnStock> oWeavingYarnStock = new List<WeavingYarnStock>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                WeavingYarnStock oItem = CreateObject(oHandler);
                oWeavingYarnStock.Add(oItem);
            }
            return oWeavingYarnStock;
        }

        #endregion

        #region Interface implementation
        public WeavingYarnStock Get(int id, Int64 nUserId)
        {
            WeavingYarnStock oWeavingYarnStock = new WeavingYarnStock();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = WeavingYarnStockDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWeavingYarnStock = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get WeavingYarnStock", e);
                #endregion
            }
            return oWeavingYarnStock;
        }

        public List<WeavingYarnStock> Gets(Int64 nUserID)
        {
            List<WeavingYarnStock> oWeavingYarnStocks = new List<WeavingYarnStock>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = WeavingYarnStockDA.Gets(tc);
                oWeavingYarnStocks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                WeavingYarnStock oWeavingYarnStock = new WeavingYarnStock();
                oWeavingYarnStock.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oWeavingYarnStocks;
        }

        public List<WeavingYarnStock> Gets(string sSQL, int nType, Int64 nUserID)
        {
            List<WeavingYarnStock> oWeavingYarnStocks = new List<WeavingYarnStock>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = WeavingYarnStockDA.Gets(tc, sSQL, nType);
                oWeavingYarnStocks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WeavingYarnStock", e);
                #endregion
            }
            return oWeavingYarnStocks;
        }

        #endregion
    }

}
