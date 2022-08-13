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

    public class AdjustmentRequisitionSlipDetailService : MarshalByRefObject, IAdjustmentRequisitionSlipDetailService
    {
        #region Private functions and declaration
        private AdjustmentRequisitionSlipDetail MapObject(NullHandler oReader)
        {
            AdjustmentRequisitionSlipDetail oAdjustmentRequisitionSlipDetail = new AdjustmentRequisitionSlipDetail();
            oAdjustmentRequisitionSlipDetail.AdjustmentRequisitionSlipDetailID = oReader.GetInt32("AdjustmentRequisitionSlipDetailID");
            oAdjustmentRequisitionSlipDetail.AdjustmentRequisitionSlipID = oReader.GetInt32("AdjustmentRequisitionSlipID");           
            oAdjustmentRequisitionSlipDetail.Qty = oReader.GetDouble("Qty");
            oAdjustmentRequisitionSlipDetail.LotID = oReader.GetInt32("LotID");
            oAdjustmentRequisitionSlipDetail.CurrentBalance = oReader.GetDouble("CurrentBalance");
            oAdjustmentRequisitionSlipDetail.ProductName = oReader.GetString("ProductName");
            oAdjustmentRequisitionSlipDetail.ProductCode = oReader.GetString("ProductCode");
            oAdjustmentRequisitionSlipDetail.LotNo = oReader.GetString("LotNo");           
            oAdjustmentRequisitionSlipDetail.Note = oReader.GetString("Note");
            oAdjustmentRequisitionSlipDetail.ColorName = oReader.GetString("ColorName");
            oAdjustmentRequisitionSlipDetail.WorkingUnitID = oReader.GetString("WorkingUnitID");
            oAdjustmentRequisitionSlipDetail.LocationName = oReader.GetString("LocationName");
            oAdjustmentRequisitionSlipDetail.LocationShortName = oReader.GetString("LocationShortName");
            oAdjustmentRequisitionSlipDetail.OperationUnitName = oReader.GetString("OperationUnitName");
            oAdjustmentRequisitionSlipDetail.OUShortName = oReader.GetString("OUShortName");
            return oAdjustmentRequisitionSlipDetail;

        }

        private AdjustmentRequisitionSlipDetail CreateObject(NullHandler oReader)
        {
            AdjustmentRequisitionSlipDetail oAdjustmentRequisitionSlipDetail = new AdjustmentRequisitionSlipDetail();
            oAdjustmentRequisitionSlipDetail = MapObject(oReader);
            return oAdjustmentRequisitionSlipDetail;
        }

        private List<AdjustmentRequisitionSlipDetail> CreateObjects(IDataReader oReader)
        {
            List<AdjustmentRequisitionSlipDetail> oAdjustmentRequisitionSlipDetails = new List<AdjustmentRequisitionSlipDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AdjustmentRequisitionSlipDetail oItem = CreateObject(oHandler);
                oAdjustmentRequisitionSlipDetails.Add(oItem);
            }
            return oAdjustmentRequisitionSlipDetails;
        }
        #endregion

        #region Interface implementation
        public AdjustmentRequisitionSlipDetailService() { }

        public AdjustmentRequisitionSlipDetail Save(AdjustmentRequisitionSlipDetail oAdjustmentRequisitionSlipDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oAdjustmentRequisitionSlipDetail.AdjustmentRequisitionSlipDetailID <= 0)
                {
                    reader = AdjustmentRequisitionSlipDetailDA.InsertUpdate(tc, oAdjustmentRequisitionSlipDetail, EnumDBOperation.Insert, nUserID, "");
                }
                else
                {
                    reader = AdjustmentRequisitionSlipDetailDA.InsertUpdate(tc, oAdjustmentRequisitionSlipDetail, EnumDBOperation.Update, nUserID, "");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAdjustmentRequisitionSlipDetail = new AdjustmentRequisitionSlipDetail();
                    oAdjustmentRequisitionSlipDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oAdjustmentRequisitionSlipDetail = new AdjustmentRequisitionSlipDetail();
                oAdjustmentRequisitionSlipDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oAdjustmentRequisitionSlipDetail;
        }
        public string Delete(AdjustmentRequisitionSlipDetail oAdjustmentRequisitionSlipDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                AdjustmentRequisitionSlipDetailDA.Delete(tc, oAdjustmentRequisitionSlipDetail, EnumDBOperation.Delete, nUserId, "");
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
        public AdjustmentRequisitionSlipDetail Get(int id, Int64 nUserId)
        {
            AdjustmentRequisitionSlipDetail oAdjustmentRequisitionSlipDetail = new AdjustmentRequisitionSlipDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AdjustmentRequisitionSlipDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAdjustmentRequisitionSlipDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get AdjustmentRequisitionSlipDetail", e);
                #endregion
            }

            return oAdjustmentRequisitionSlipDetail;
        }

        public List<AdjustmentRequisitionSlipDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<AdjustmentRequisitionSlipDetail> oAdjustmentRequisitionSlipDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AdjustmentRequisitionSlipDetailDA.Gets(sSQL, tc);
                oAdjustmentRequisitionSlipDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AdjustmentRequisitionSlipDetail", e);
                #endregion
            }
            return oAdjustmentRequisitionSlipDetail;
        }
        public List<AdjustmentRequisitionSlipDetail> Gets(int nDUDeliveryChallanID, Int64 nUserID)
        {
            List<AdjustmentRequisitionSlipDetail> oAdjustmentRequisitionSlipDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AdjustmentRequisitionSlipDetailDA.Gets(tc, nDUDeliveryChallanID);
                oAdjustmentRequisitionSlipDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AdjustmentRequisitionSlipDetail", e);
                #endregion
            }
            return oAdjustmentRequisitionSlipDetail;
        }


        #endregion
    }
}
