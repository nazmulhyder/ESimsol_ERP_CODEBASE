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
    public class TradingPaymentDetailService : MarshalByRefObject, ITradingPaymentDetailService
    {        

        #region Private functions and declaration
        private TradingPaymentDetail MapObject(NullHandler oReader)
        {
            TradingPaymentDetail oTradingPaymentDetail = new TradingPaymentDetail();
            oTradingPaymentDetail.TradingPaymentDetailID = oReader.GetInt32("TradingPaymentDetailID");
            oTradingPaymentDetail.TradingPaymentID = oReader.GetInt32("TradingPaymentID");
            oTradingPaymentDetail.ReferenceID = oReader.GetInt32("ReferenceID");
            oTradingPaymentDetail.Amount = oReader.GetDouble("Amount");
            oTradingPaymentDetail.Remarks = oReader.GetString("Remarks");
            oTradingPaymentDetail.InvoiceNo = oReader.GetString("InvoiceNo");
            oTradingPaymentDetail.InvoiceDate = oReader.GetDateTime("InvoiceDate");
            oTradingPaymentDetail.InvoiceAmount = oReader.GetDouble("InvoiceAmount");
            oTradingPaymentDetail.ReferenceType = (EnumPaymentRefType)oReader.GetInt32("ReferenceType");
            oTradingPaymentDetail.ReferenceTypeInt = oReader.GetInt32("ReferenceType");            
            return oTradingPaymentDetail;
        }

        private TradingPaymentDetail CreateObject(NullHandler oReader)
        {
            TradingPaymentDetail oTradingPaymentDetail = new TradingPaymentDetail();
            oTradingPaymentDetail = MapObject(oReader);
            return oTradingPaymentDetail;
        }

        private List<TradingPaymentDetail> CreateObjects(IDataReader oReader)
        {
            List<TradingPaymentDetail> oTradingPaymentDetails = new List<TradingPaymentDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TradingPaymentDetail oItem = CreateObject(oHandler);
                oTradingPaymentDetails.Add(oItem);
            }
            return oTradingPaymentDetails;
        }
        #endregion

        #region Interface implementation
        public TradingPaymentDetailService() { }

        public TradingPaymentDetail Save(TradingPaymentDetail oTradingPaymentDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oTradingPaymentDetail.TradingPaymentDetailID <= 0)
                {
                    reader = TradingPaymentDetailDA.InsertUpdate(tc, oTradingPaymentDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = TradingPaymentDetailDA.InsertUpdate(tc, oTradingPaymentDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingPaymentDetail = new TradingPaymentDetail();
                    oTradingPaymentDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new Exception("Failed to Save TradingPaymentDetail \n" + e.Message, e);
                #endregion
            }
            return oTradingPaymentDetail;
        }
        public string Delete(TradingPaymentDetail oTradingPaymentDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TradingPaymentDetailDA.Delete(tc, oTradingPaymentDetail, EnumDBOperation.Delete, nUserID, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                //throw new Exception(e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public TradingPaymentDetail Get(int nTradingPaymentDetailID, int nUserID)
        {
            TradingPaymentDetail oTradingPaymentDetail = new TradingPaymentDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TradingPaymentDetailDA.Get(tc, nTradingPaymentDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTradingPaymentDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new Exception("Failed to Get TradingPaymentDetail \n" + e.Message, e);
                #endregion
            }
            return oTradingPaymentDetail;
        }

        public List<TradingPaymentDetail> Gets(int nUserID)
        {
            List<TradingPaymentDetail> oTradingPaymentDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingPaymentDetailDA.Gets(tc);
                oTradingPaymentDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingPaymentDetail \n" + e.Message, e);
                #endregion
            }

            return oTradingPaymentDetails;
        }
        public List<TradingPaymentDetail> Gets(int nPaymentID, int nUserID)
        {
            List<TradingPaymentDetail> oTradingPaymentDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingPaymentDetailDA.Gets(tc, nPaymentID);
                oTradingPaymentDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingPaymentDetail \n" + e.Message, e);
                #endregion
            }

            return oTradingPaymentDetails;
        }
        public List<TradingPaymentDetail> Gets(string sSQL, int nUserID)
        {
            List<TradingPaymentDetail> oTradingPaymentDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TradingPaymentDetailDA.Gets(tc, sSQL);
                oTradingPaymentDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get TradingPaymentDetails \n" + e.Message, e);
                #endregion
            }
            return oTradingPaymentDetails;
        }
        #endregion
    }
}
