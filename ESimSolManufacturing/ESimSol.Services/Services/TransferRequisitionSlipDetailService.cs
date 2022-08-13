using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;


using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    [Serializable]
    public class TransferRequisitionSlipDetailService : MarshalByRefObject, ITransferRequisitionSlipDetailService
    {
        #region Private functions and declaration
        private TransferRequisitionSlipDetail MapObject(NullHandler oReader)
        {
            TransferRequisitionSlipDetail oRequisitionSlipDetail = new TransferRequisitionSlipDetail();
            oRequisitionSlipDetail.TRSDetailID = oReader.GetInt32("TRSDetailID");
            oRequisitionSlipDetail.TRSID = oReader.GetInt32("TRSID");
            oRequisitionSlipDetail.StyleID = oReader.GetInt32("StyleID");
            oRequisitionSlipDetail.ProductID = oReader.GetInt32("ProductID");
            oRequisitionSlipDetail.LotID = oReader.GetInt32("LotID");
            oRequisitionSlipDetail.QTY = oReader.GetDouble("QTY");
            oRequisitionSlipDetail.MUnitID = oReader.GetInt32("MUnitID");
            oRequisitionSlipDetail.BagBales = oReader.GetDouble("BagBales");
            oRequisitionSlipDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oRequisitionSlipDetail.CurrencyID = oReader.GetInt32("CurrencyID");
            oRequisitionSlipDetail.SuggestLotNo = oReader.GetString("SuggestLotNo");
            oRequisitionSlipDetail.Remark = oReader.GetString("Remark");
            oRequisitionSlipDetail.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oRequisitionSlipDetail.DestinationLotID = oReader.GetInt32("DestinationLotID");
            oRequisitionSlipDetail.MUName = oReader.GetString("MUName");
            oRequisitionSlipDetail.MUSymbol = oReader.GetString("MUSymbol");
            oRequisitionSlipDetail.CSymbol = oReader.GetString("CSymbol");
            oRequisitionSlipDetail.LotNo = oReader.GetString("LotNo");
            oRequisitionSlipDetail.ColorName = oReader.GetString("ColorName");
            oRequisitionSlipDetail.SizeName = oReader.GetString("SizeName");
            oRequisitionSlipDetail.DestinationLotNo = oReader.GetString("DestinationLotNo");
            oRequisitionSlipDetail.LotCurrentBalance = oReader.GetDouble("LotCurrentBalance");
            oRequisitionSlipDetail.ProductName = oReader.GetString("ProductName");
            oRequisitionSlipDetail.ProductCode = oReader.GetString("ProductCode");
            oRequisitionSlipDetail.StyleNo = oReader.GetString("StyleNo");
            oRequisitionSlipDetail.BuyerName = oReader.GetString("BuyerName");
            oRequisitionSlipDetail.SupplierSName = oReader.GetString("SupplierSName");
            return oRequisitionSlipDetail;
        }

        private TransferRequisitionSlipDetail CreateObject(NullHandler oReader)
        {
            TransferRequisitionSlipDetail oRequisitionSlipDetail = new TransferRequisitionSlipDetail();
            oRequisitionSlipDetail = MapObject(oReader);
            return oRequisitionSlipDetail;
        }

        private List<TransferRequisitionSlipDetail> CreateObjects(IDataReader oReader)
        {
            List<TransferRequisitionSlipDetail> oRequisitionSlipDetails = new List<TransferRequisitionSlipDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TransferRequisitionSlipDetail oItem = CreateObject(oHandler);
                oRequisitionSlipDetails.Add(oItem);
            }
            return oRequisitionSlipDetails;
        }
        #endregion

        #region Interface implementation
        public TransferRequisitionSlipDetailService() { }

        public string Delete(TransferRequisitionSlipDetail oTransferRequisitionSlipDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TransferRequisitionSlipDetailDA.Delete(tc, oTransferRequisitionSlipDetail, EnumDBOperation.Delete, nUserID, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Bank. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }
        public TransferRequisitionSlipDetail Get(int nID, Int64 nUserID)
        {
            TransferRequisitionSlipDetail oRequisitionSlipDetail = new TransferRequisitionSlipDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TransferRequisitionSlipDetailDA.Get(tc, nID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRequisitionSlipDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get RequisitionSlipDetail", e);
                #endregion
            }

            return oRequisitionSlipDetail;
        }

        public List<TransferRequisitionSlipDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<TransferRequisitionSlipDetail> oRequisitionSlipDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TransferRequisitionSlipDetailDA.Gets(tc, sSQL);
                oRequisitionSlipDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RequisitionSlipDetails", e);
                #endregion
            }

            return oRequisitionSlipDetails;
        }

        public List<TransferRequisitionSlipDetail> Gets(int nTRSID, Int64 nUserID)
        {
            List<TransferRequisitionSlipDetail> oRequisitionSlipDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TransferRequisitionSlipDetailDA.Gets(tc, nTRSID);
                oRequisitionSlipDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oRequisitionSlipDetails;
        }
        #endregion
    }
}
