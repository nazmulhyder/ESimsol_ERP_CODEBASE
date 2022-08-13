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
    public class SampleRequestDetailService : MarshalByRefObject, ISampleRequestDetailService
    {
        #region Private functions and declaration

        private SampleRequestDetail MapObject(NullHandler oReader)
        {
            SampleRequestDetail oSampleRequestDetail = new SampleRequestDetail();
            oSampleRequestDetail.SampleRequestDetailID = oReader.GetInt32("SampleRequestDetailID");
            oSampleRequestDetail.SampleRequestID = oReader.GetInt32("SampleRequestID");
            oSampleRequestDetail.ProductID = oReader.GetInt32("ProductID");
            oSampleRequestDetail.ColorCategoryID = oReader.GetInt32("ColorCategoryID");
            oSampleRequestDetail.MUnitID = oReader.GetInt32("MUnitID");
            oSampleRequestDetail.LotID = oReader.GetInt32("LotID");
            oSampleRequestDetail.LotNo = oReader.GetString("LotNo");
            oSampleRequestDetail.LotBalance = oReader.GetDouble("Balance");
            oSampleRequestDetail.Quantity = oReader.GetDouble("Quantity");
            oSampleRequestDetail.Remarks = oReader.GetString("Remarks");
            oSampleRequestDetail.ProductName = oReader.GetString("ProductName");
            oSampleRequestDetail.ProductCode = oReader.GetString("ProductCode");
            oSampleRequestDetail.ColorName = oReader.GetString("ColorName");
            oSampleRequestDetail.MUnitName = oReader.GetString("MUnitName");
            return oSampleRequestDetail;
        }

        private SampleRequestDetail CreateObject(NullHandler oReader)
        {
            SampleRequestDetail oSampleRequestDetail = new SampleRequestDetail();
            oSampleRequestDetail = MapObject(oReader);
            return oSampleRequestDetail;
        }

        private List<SampleRequestDetail> CreateObjects(IDataReader oReader)
        {
            List<SampleRequestDetail> oSampleRequestDetail = new List<SampleRequestDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SampleRequestDetail oItem = CreateObject(oHandler);
                oSampleRequestDetail.Add(oItem);
            }
            return oSampleRequestDetail;
        }

        #endregion

        #region Interface implementation


        public SampleRequestDetail Get(int id, Int64 nUserId)
        {
            SampleRequestDetail oSampleRequestDetail = new SampleRequestDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = SampleRequestDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleRequestDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SampleRequestDetail", e);
                #endregion
            }
            return oSampleRequestDetail;
        }

        public List<SampleRequestDetail> Gets(int nSampleRequestID, Int64 nUserID)
        {
            List<SampleRequestDetail> oSampleRequestDetails = new List<SampleRequestDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SampleRequestDetailDA.Gets(tc, nSampleRequestID);
                oSampleRequestDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                SampleRequestDetail oSampleRequestDetail = new SampleRequestDetail();
                oSampleRequestDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oSampleRequestDetails;
        }

        public List<SampleRequestDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<SampleRequestDetail> oSampleRequestDetails = new List<SampleRequestDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SampleRequestDetailDA.Gets(tc, sSQL);
                oSampleRequestDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleRequestDetail", e);
                #endregion
            }
            return oSampleRequestDetails;
        }

        #endregion
    }

}
