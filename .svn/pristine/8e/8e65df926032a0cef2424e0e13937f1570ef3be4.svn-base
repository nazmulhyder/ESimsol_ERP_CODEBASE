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
    public class FabricTransferRequisitionSlipDetailService : MarshalByRefObject, IFabricTransferRequisitionSlipDetailService
    {
        #region Private functions and declaration

        private FabricTransferRequisitionSlipDetail MapObject(NullHandler oReader)
        {
            FabricTransferRequisitionSlipDetail oFabricTransferRequisitionSlipDetail = new FabricTransferRequisitionSlipDetail();
            oFabricTransferRequisitionSlipDetail.FabricTRSDetailID = oReader.GetInt32("FabricTRSDetailID");
            oFabricTransferRequisitionSlipDetail.FabricTRSID = oReader.GetInt32("FabricTRSID");
            oFabricTransferRequisitionSlipDetail.ProductID = oReader.GetInt32("ProductID");
            oFabricTransferRequisitionSlipDetail.LotID = oReader.GetInt32("LotID");
            oFabricTransferRequisitionSlipDetail.Qty = oReader.GetDouble("Qty");
            oFabricTransferRequisitionSlipDetail.RollQty = oReader.GetInt32("RollQty");
            oFabricTransferRequisitionSlipDetail.MUnitID = oReader.GetInt32("MUnitID");
            oFabricTransferRequisitionSlipDetail.BagBales = oReader.GetDouble("BagBales");
            oFabricTransferRequisitionSlipDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oFabricTransferRequisitionSlipDetail.CurrencyID = oReader.GetInt32("CurrencyID");
            oFabricTransferRequisitionSlipDetail.Remarks = oReader.GetString("Remarks");
            oFabricTransferRequisitionSlipDetail.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oFabricTransferRequisitionSlipDetail.DestinationLotID = oReader.GetInt32("DestinationLotID");
            oFabricTransferRequisitionSlipDetail.LotNo = oReader.GetString("LotNo");
            oFabricTransferRequisitionSlipDetail.FSCDID = oReader.GetInt32("FSCDID");
            oFabricTransferRequisitionSlipDetail.ProductName = oReader.GetString("ProductName");
            oFabricTransferRequisitionSlipDetail.DispoNo = oReader.GetString("DispoNo");
            oFabricTransferRequisitionSlipDetail.FabricID = oReader.GetInt32("FabricID");
            oFabricTransferRequisitionSlipDetail.FabricNo = oReader.GetString("FabricNo");
            oFabricTransferRequisitionSlipDetail.Construction = oReader.GetString("Construction");
            oFabricTransferRequisitionSlipDetail.FinishType = oReader.GetInt32("FinishType");
            oFabricTransferRequisitionSlipDetail.FinishTypeName = oReader.GetString("FinishTypeName");
            oFabricTransferRequisitionSlipDetail.FabricWeave = oReader.GetInt32("FabricWeave");
            oFabricTransferRequisitionSlipDetail.FabricWeaveName = oReader.GetString("FabricWeaveName");
            return oFabricTransferRequisitionSlipDetail;
        }

        private FabricTransferRequisitionSlipDetail CreateObject(NullHandler oReader)
        {
            FabricTransferRequisitionSlipDetail oFabricTransferRequisitionSlipDetail = new FabricTransferRequisitionSlipDetail();
            oFabricTransferRequisitionSlipDetail = MapObject(oReader);
            return oFabricTransferRequisitionSlipDetail;
        }

        private List<FabricTransferRequisitionSlipDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricTransferRequisitionSlipDetail> oFabricTransferRequisitionSlipDetail = new List<FabricTransferRequisitionSlipDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricTransferRequisitionSlipDetail oItem = CreateObject(oHandler);
                oFabricTransferRequisitionSlipDetail.Add(oItem);
            }
            return oFabricTransferRequisitionSlipDetail;
        }

        #endregion

        #region Interface implementation
        public FabricTransferRequisitionSlipDetail Get(int id, Int64 nUserId)
        {
            FabricTransferRequisitionSlipDetail oFabricTransferRequisitionSlipDetail = new FabricTransferRequisitionSlipDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricTransferRequisitionSlipDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricTransferRequisitionSlipDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricTransferRequisitionSlipDetail", e);
                #endregion
            }
            return oFabricTransferRequisitionSlipDetail;
        }

        public List<FabricTransferRequisitionSlipDetail> Gets(Int64 nUserID)
        {
            List<FabricTransferRequisitionSlipDetail> oFabricTransferRequisitionSlipDetails = new List<FabricTransferRequisitionSlipDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricTransferRequisitionSlipDetailDA.Gets(tc);
                oFabricTransferRequisitionSlipDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricTransferRequisitionSlipDetail oFabricTransferRequisitionSlipDetail = new FabricTransferRequisitionSlipDetail();
                oFabricTransferRequisitionSlipDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricTransferRequisitionSlipDetails;
        }

        public List<FabricTransferRequisitionSlipDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricTransferRequisitionSlipDetail> oFabricTransferRequisitionSlipDetails = new List<FabricTransferRequisitionSlipDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricTransferRequisitionSlipDetailDA.Gets(tc, sSQL);
                oFabricTransferRequisitionSlipDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricTransferRequisitionSlipDetail", e);
                #endregion
            }
            return oFabricTransferRequisitionSlipDetails;
        }

        #endregion
    }

}
