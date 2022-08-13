using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Base.DataAccess;
using ICS.Base.Client.BOFoundation;
using ICS.Base.Utility;
using ICS.Base.FrameWork;
using ICS.Base.Client.Utility;

namespace ESimSol.Services.Services
{
    [Serializable]
    public class RequisitionSlipDetailService : MarshalByRefObject, IRequisitionSlipDetailService
    {
        #region Private functions and declaration
        private TransferRequisitionSlipDetail MapObject( NullHandler oReader)
        {
            TransferRequisitionSlipDetail oRequisitionSlipDetail = new TransferRequisitionSlipDetail();
            oRequisitionSlipDetail.RequisitionSlipDetailID = oReader.GetInt32("RequisitionSlipDetailID");
            oRequisitionSlipDetail.RequisitionSlipID = oReader.GetInt32("RequisitionSlipID");
            oRequisitionSlipDetail.ProductID = oReader.GetInt32("ProductID");
            oRequisitionSlipDetail.LotID = oReader.GetInt32("LotID");
            oRequisitionSlipDetail.QTY = oReader.GetDouble("QTY");
            oRequisitionSlipDetail.Remark = oReader.GetString("Remark");
            oRequisitionSlipDetail.DestinationLotID = oReader.GetInt32("DestinationLotID");
            oRequisitionSlipDetail.ProductName = oReader.GetString("ProductName");
            oRequisitionSlipDetail.ProductCode = oReader.GetString("ProductCode");
            oRequisitionSlipDetail.BagBales = oReader.GetDouble("BagBales");
            return oRequisitionSlipDetail;
            
        }
      
        private TransferRequisitionSlipDetail CreateObject(NullHandler oReader)
        {
            TransferRequisitionSlipDetail oRequisitionSlipDetail = new TransferRequisitionSlipDetail();
            oRequisitionSlipDetail=  MapObject(oReader);
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
        public RequisitionSlipDetailService() { }


        public TransferRequisitionSlipDetail Save(TransferRequisitionSlipDetail oYRSD, Int64 nUserID)
        {
            TransferRequisitionSlipDetail oRequisitionSlipDetail = new TransferRequisitionSlipDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader;
                if (oRequisitionSlipDetail.RequisitionSlipID <= 0)
                {
                    reader = TransferRequisitionSlipDetailDA.RSD_IUD(tc, oYRSD, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = TransferRequisitionSlipDetailDA.RSD_IUD(tc, oYRSD, EnumDBOperation.Update, nUserID);
                }
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oRequisitionSlipDetail;
        }
        public bool Delete(int nID, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TransferRequisitionSlipDetailDA.Delete(tc, nID);
                tc.End();
                return true;
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new ServiceException(e.Message, e);
                #endregion
            }
            return false;
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

        public List<TransferRequisitionSlipDetail> Gets( string sSQL,Int64 nUserID)
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

        public List<TransferRequisitionSlipDetail> Gets(int nYarnRequisitionSlipID, Int64 nUserID)
        {
            List<TransferRequisitionSlipDetail> oRequisitionSlipDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TransferRequisitionSlipDetailDA.Gets(tc, nYarnRequisitionSlipID);
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
        public List<TransferRequisitionSlipDetail> UpdateAutoLotReceiving(int nYarnRequisitionSlipID, Int64 nUserID)
        {
            List<TransferRequisitionSlipDetail> oRequisitionSlipDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TransferRequisitionSlipDetailDA.UpdateAutoLotReceiving(tc, nYarnRequisitionSlipID);
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
        public bool UpdateStatus(int nID, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TransferRequisitionSlipDetailDA.UpdateStatus(tc, nID);
                tc.End();
                return true;
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new ServiceException(e.Message, e);
                #endregion
            }
            return false;
        }
        #endregion
    }
}
