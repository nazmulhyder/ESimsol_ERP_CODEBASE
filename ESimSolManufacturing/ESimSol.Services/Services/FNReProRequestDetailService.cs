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
    public class FNReProRequestDetailService : MarshalByRefObject, IFNReProRequestDetailService
    {
        #region Private functions and declaration

        private FNReProRequestDetail MapObject(NullHandler oReader)
        {
            FNReProRequestDetail oFNReProRequestDetail = new FNReProRequestDetail();
            oFNReProRequestDetail.FNReProRequestDetailID = oReader.GetInt32("FNReProRequestDetailID");
            oFNReProRequestDetail.FNReProRequestID = oReader.GetInt32("FNReProRequestID");
            oFNReProRequestDetail.FNBatchCardID = oReader.GetInt32("FNBatchCardID");
            oFNReProRequestDetail.Qty = oReader.GetDouble("Qty");
            oFNReProRequestDetail.Note = oReader.GetString("Note");
            oFNReProRequestDetail.FNBatchID = oReader.GetInt32("FNBatchID");
            oFNReProRequestDetail.BatchNo = oReader.GetString("BatchNo");
            oFNReProRequestDetail.FNTreatmentProcessID = oReader.GetInt32("FNTreatmentProcessID");
            oFNReProRequestDetail.FNProcess = oReader.GetString("FNProcess");
            oFNReProRequestDetail.Qty_Prod = oReader.GetDouble("Qty_Prod");
            oFNReProRequestDetail.Qty_YetTo = oReader.GetDouble("Qty_YetTo");
            oFNReProRequestDetail.FNExeNo = oReader.GetString("FNExeNo");
            
            return oFNReProRequestDetail;
        }

        private FNReProRequestDetail CreateObject(NullHandler oReader)
        {
            FNReProRequestDetail oFNReProRequestDetail = new FNReProRequestDetail();
            oFNReProRequestDetail = MapObject(oReader);
            return oFNReProRequestDetail;
        }

        private List<FNReProRequestDetail> CreateObjects(IDataReader oReader)
        {
            List<FNReProRequestDetail> oFNReProRequestDetail = new List<FNReProRequestDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNReProRequestDetail oItem = CreateObject(oHandler);
                oFNReProRequestDetail.Add(oItem);
            }
            return oFNReProRequestDetail;
        }

        #endregion

        #region Interface implementation


        public FNReProRequestDetail Get(int id, Int64 nUserId)
        {
            FNReProRequestDetail oFNReProRequestDetail = new FNReProRequestDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FNReProRequestDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNReProRequestDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FNReProRequestDetail", e);
                #endregion
            }
            return oFNReProRequestDetail;
        }

        public List<FNReProRequestDetail> Gets(int nFNReProRequestID, Int64 nUserID)
        {
            List<FNReProRequestDetail> oFNReProRequestDetails = new List<FNReProRequestDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FNReProRequestDetailDA.Gets(tc, nFNReProRequestID);
                oFNReProRequestDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FNReProRequestDetail oFNReProRequestDetail = new FNReProRequestDetail();
                oFNReProRequestDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFNReProRequestDetails;
        }

        public List<FNReProRequestDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<FNReProRequestDetail> oFNReProRequestDetails = new List<FNReProRequestDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FNReProRequestDetailDA.Gets(tc, sSQL);
                oFNReProRequestDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNReProRequestDetail", e);
                #endregion
            }
            return oFNReProRequestDetails;
        }

        #endregion
    }

}
