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
    public class FabricBatchQCService : MarshalByRefObject, IFabricBatchQCService
    {
        #region Private functions and declaration
        public static FabricBatchQC MapObject(NullHandler oReader)
        {
            FabricBatchQC oFabricBatchQC = new FabricBatchQC();
            oFabricBatchQC.FBQCID = oReader.GetInt32("FBQCID");
            oFabricBatchQC.FBID = oReader.GetInt32("FBID");
            oFabricBatchQC.BatchNo = oReader.GetString("BatchNo");
            oFabricBatchQC.QCInCharge = oReader.GetInt32("QCInCharge");
            oFabricBatchQC.InChargeName = oReader.GetString("InChargeName");
            oFabricBatchQC.FabricBatchQty = oReader.GetDouble("FabricBatchQty");
            oFabricBatchQC.FabricBatchStatus = (EnumFabricBatchState)oReader.GetInt32("FabricBatchStatus");
            oFabricBatchQC.StatusInInt = oReader.GetInt32("FabricBatchStatus");
            oFabricBatchQC.QCStartDateTime = oReader.GetDateTime("QCStartDateTime");
            oFabricBatchQC.FEOID = oReader.GetInt32("FEOID");
            oFabricBatchQC.FEONo = oReader.GetString("FEONo");
            oFabricBatchQC.BuyerName = oReader.GetString("BuyerName");
            oFabricBatchQC.OrderType = (EnumOrderType)oReader.GetInt32("OrderType");
            oFabricBatchQC.QCEndDateTime = oReader.GetDateTime("QCEndDateTime");
            oFabricBatchQC.Construction = oReader.GetString("Construction");
            oFabricBatchQC.TotalLength = oReader.GetDouble("TotalLength");
            oFabricBatchQC.FabricType = oReader.GetString("FabricType");
            oFabricBatchQC.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFabricBatchQC.CW = oReader.GetString("CW");
            oFabricBatchQC.PINo = oReader.GetString("PINo");
            oFabricBatchQC.CountNotRecDetail = oReader.GetInt32("CountNotRecDetail");
            oFabricBatchQC.GreyWidth = oReader.GetDouble("GreyWidth");
            oFabricBatchQC.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oFabricBatchQC.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oFabricBatchQC.FabricBatchLoomID = oReader.GetInt32("FabricBatchLoomID");
            oFabricBatchQC.FEOSID = oReader.GetInt32("FEOSID");
            oFabricBatchQC.LoomQty = oReader.GetDouble("LoomQty");
            oFabricBatchQC.FMID = oReader.GetInt32("FMID");
            oFabricBatchQC.FabricMachineName = oReader.GetString("FabricMachineName");
            oFabricBatchQC.InsQty = oReader.GetDouble("InsQty");

            return oFabricBatchQC;
        }
        public static FabricBatchQC CreateObject(NullHandler oReader)
        {
            FabricBatchQC oFabricBatchQC = new FabricBatchQC();
            oFabricBatchQC = MapObject(oReader);
            return oFabricBatchQC;
        }
        private List<FabricBatchQC> CreateObjects(IDataReader oReader)
        {
            List<FabricBatchQC> oFabricBatchQC = new List<FabricBatchQC>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricBatchQC oItem = CreateObject(oHandler);
                oFabricBatchQC.Add(oItem);
            }
            return oFabricBatchQC;
        }
        #endregion

        #region Interface implementation
        public FabricBatchQC QCDone(FabricBatchQC oFabricBatchQC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                 reader = FabricBatchQCDA.InsertUpdate(tc, oFabricBatchQC, EnumDBOperation.Approval, nUserID);//Qc Done for enumdbOperation Update :2
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchQC = new FabricBatchQC();
                    oFabricBatchQC = CreateObject(oReader);
                }
                reader.Close();
                #region Get Details
                
                IDataReader redaterDetail;
                redaterDetail = FabricBatchQCDetailDA.Gets(tc, oFabricBatchQC.FBQCID);
                oFabricBatchQC.FabricBatchQCDetails = FabricBatchQCDetailService.CreateObjects(redaterDetail);
                redaterDetail.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save Fabric Production Batch. Because of " + e.Message, e);
                #endregion
            }
            return oFabricBatchQC;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchQC oFabricBatchQC = new FabricBatchQC();
                oFabricBatchQC.FBID = id;
                FabricBatchQCDA.Delete(tc, oFabricBatchQC, EnumDBOperation.Delete, nUserId);
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
        public FabricBatchQC Get(int id, Int64 nUserId)
        {
            FabricBatchQC oFabricBatchQC = new FabricBatchQC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricBatchQCDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchQC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricBatchQC", e);
                #endregion
            }
            return oFabricBatchQC;
        }
        
        public FabricBatchQC GetByBatch(int id,  Int64 nUserId)
        {
            FabricBatchQC oFabricBatchQC = new FabricBatchQC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricBatchQCDA.GetByBatch(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchQC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricBatchQC", e);
                #endregion
            }
            return oFabricBatchQC;
        }
        public FabricBatchQC GetByProduction(int id, Int64 nUserId)
        {
            FabricBatchQC oFabricBatchQC = new FabricBatchQC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricBatchQCDA.GetByProduction(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchQC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricBatchQC", e);
                #endregion
            }
            return oFabricBatchQC;
        }
        
        public List<FabricBatchQC> Gets(Int64 nUserID)
        {
            List<FabricBatchQC> oFabricBatchQCs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchQCDA.Gets(tc);
                oFabricBatchQCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricBatchQC", e);
                #endregion
            }
            return oFabricBatchQCs;
        }
        public List<FabricBatchQC> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricBatchQC> oFabricBatchQCs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchQCDA.Gets(tc, sSQL);
                oFabricBatchQCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricBatchQC", e);
                #endregion
            }
            return oFabricBatchQCs;
        }

  
        #endregion
    }
}
