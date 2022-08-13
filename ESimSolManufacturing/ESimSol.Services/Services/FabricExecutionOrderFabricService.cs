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
    public class FabricExecutionOrderFabricService : MarshalByRefObject, IFabricExecutionOrderFabricService
    {
        #region Private functions and declaration
        private FabricExecutionOrderFabric MapObject(NullHandler oReader)
        {
            FabricExecutionOrderFabric oFabricExecutionOrderFabric = new FabricExecutionOrderFabric();
            oFabricExecutionOrderFabric.FEOFID = oReader.GetInt32("FEOFID");
            oFabricExecutionOrderFabric.FEOID = oReader.GetInt32("FEOID");
            oFabricExecutionOrderFabric.ExportPIDetailID = oReader.GetInt32("ExportPIDetailID");
            oFabricExecutionOrderFabric.Qty = oReader.GetDouble("Qty");
            oFabricExecutionOrderFabric.Amount = oReader.GetDouble("Amount");
            oFabricExecutionOrderFabric.UnitPrice = oReader.GetDouble("UnitPrice");
            oFabricExecutionOrderFabric.FabricID = oReader.GetInt32("FabricID");
            oFabricExecutionOrderFabric.ProductID = oReader.GetInt32("ProductID");
            oFabricExecutionOrderFabric.FactoryID = oReader.GetInt32("FactoryID");
            oFabricExecutionOrderFabric.ExportPIID = oReader.GetInt32("ExportPIID"); 
            oFabricExecutionOrderFabric.PINo = oReader.GetString("PINo");
            oFabricExecutionOrderFabric.PIDate = oReader.GetDateTime("PIDate");
            oFabricExecutionOrderFabric.FabricNo = oReader.GetString("FabricNo");
            oFabricExecutionOrderFabric.LCNo = oReader.GetString("LCNo");
            oFabricExecutionOrderFabric.LCDate = oReader.GetDateTime("LCDate");
            oFabricExecutionOrderFabric.FactoryName = oReader.GetString("FactoryName");
            oFabricExecutionOrderFabric.FEONo = oReader.GetString("FEONo");
            oFabricExecutionOrderFabric.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFabricExecutionOrderFabric.OrderType = (EnumOrderType)oReader.GetInt16("OrderType");
            oFabricExecutionOrderFabric.ColorInfo = oReader.GetString("ColorInfo");

            oFabricExecutionOrderFabric.Description = oReader.GetString("Description");
            oFabricExecutionOrderFabric.StyleNo = oReader.GetString("StyleNo");
            oFabricExecutionOrderFabric.BuyerReference = oReader.GetString("BuyerReference");
            oFabricExecutionOrderFabric.ProductCode = oReader.GetString("ProductCode");
            oFabricExecutionOrderFabric.ProductName = oReader.GetString("ProductName");
            oFabricExecutionOrderFabric.ProductCount = oReader.GetString("ProductCount");
            oFabricExecutionOrderFabric.MUnitID = oReader.GetInt32("MUnitID");
            oFabricExecutionOrderFabric.MUName = oReader.GetString("MUName");
            oFabricExecutionOrderFabric.Currency = oReader.GetString("Currency");
            oFabricExecutionOrderFabric.FabricWidth = oReader.GetString("FabricWidth");
            oFabricExecutionOrderFabric.Construction = oReader.GetString("Construction");
            oFabricExecutionOrderFabric.ProcessType = oReader.GetInt32("ProcessType");
            oFabricExecutionOrderFabric.FabricWeave = oReader.GetInt32("FabricWeave");
            oFabricExecutionOrderFabric.FinishType = oReader.GetInt32("FinishType");
            oFabricExecutionOrderFabric.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oFabricExecutionOrderFabric.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oFabricExecutionOrderFabric.FinishTypeName = oReader.GetString("FinishTypeName");
            oFabricExecutionOrderFabric.ExportPiTotalAmount = oReader.GetDouble("ExportPiTotalAmount");
            oFabricExecutionOrderFabric.BuyerName = oReader.GetString("BuyerName");
            oFabricExecutionOrderFabric.VersionNo = oReader.GetInt32("VersionNo");
            oFabricExecutionOrderFabric.FabricDeliveryOrderQty = oReader.GetDouble("FabricDeliveryOrderQty");
            oFabricExecutionOrderFabric.FEOQty = oReader.GetDouble("FEOQty");
            oFabricExecutionOrderFabric.PIDetailQty = oReader.GetDouble("PIDetailQty");
            return oFabricExecutionOrderFabric;
        }

        private FabricExecutionOrderFabric CreateObject(NullHandler oReader)
        {
            FabricExecutionOrderFabric oFabricExecutionOrderFabric = new FabricExecutionOrderFabric();
            oFabricExecutionOrderFabric = MapObject(oReader);
            return oFabricExecutionOrderFabric;
        }

        private List<FabricExecutionOrderFabric> CreateObjects(IDataReader oReader)
        {
            List<FabricExecutionOrderFabric> oFabricExecutionOrderFabric = new List<FabricExecutionOrderFabric>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricExecutionOrderFabric oItem = CreateObject(oHandler);
                oFabricExecutionOrderFabric.Add(oItem);
            }
            return oFabricExecutionOrderFabric;
        }

        #endregion

        #region Interface implementation
        public FabricExecutionOrderFabricService() { }

        public FabricExecutionOrderFabric Save(FabricExecutionOrderFabric oFabricExecutionOrderFabric, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricExecutionOrderFabric.FEOFID <= 0)
                {
                    reader = FabricExecutionOrderFabricDA.InsertUpdate(tc, oFabricExecutionOrderFabric, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    reader = FabricExecutionOrderFabricDA.InsertUpdate(tc, oFabricExecutionOrderFabric, EnumDBOperation.Update, nUserID,"");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricExecutionOrderFabric = new FabricExecutionOrderFabric();
                    oFabricExecutionOrderFabric = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save FabricExecutionOrderFabric. Because of " + e.Message, e);
                #endregion
            }
            return oFabricExecutionOrderFabric;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricExecutionOrderFabric oFabricExecutionOrderFabric = new FabricExecutionOrderFabric();
                oFabricExecutionOrderFabric.FEOFID = id;
                FabricExecutionOrderFabricDA.Delete(tc, oFabricExecutionOrderFabric, EnumDBOperation.Delete, nUserId,"");
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
            return Global.DeleteMessage;
        }
        public FabricExecutionOrderFabric Get(int id, Int64 nUserId)
        {
            FabricExecutionOrderFabric oFabricExecutionOrderFabric = new FabricExecutionOrderFabric();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricExecutionOrderFabricDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricExecutionOrderFabric = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricExecutionOrderFabric", e);
                #endregion
            }
            return oFabricExecutionOrderFabric;
        }
        public List<FabricExecutionOrderFabric> Gets(Int64 nUserID)
        {
            List<FabricExecutionOrderFabric> oFabricExecutionOrderFabrics = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricExecutionOrderFabricDA.Gets(tc);
                oFabricExecutionOrderFabrics = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricExecutionOrderFabric", e);
                #endregion
            }
            return oFabricExecutionOrderFabrics;
        }
        public List<FabricExecutionOrderFabric> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricExecutionOrderFabric> oFabricExecutionOrderFabrics = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricExecutionOrderFabricDA.Gets(tc, sSQL);
                oFabricExecutionOrderFabrics = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricExecutionOrderFabric", e);
                #endregion
            }
            return oFabricExecutionOrderFabrics;
        }

        public List<FabricExecutionOrderFabric> Gets(int nFEOID, Int64 nUserID)
        {
            List<FabricExecutionOrderFabric> oFabricExecutionOrderFabrics = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricExecutionOrderFabricDA.Gets(tc, nFEOID);
                oFabricExecutionOrderFabrics = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Fabric Execution Order Fabric", e);
                #endregion
            }
            return oFabricExecutionOrderFabrics;
        }
        #endregion
    }
}
