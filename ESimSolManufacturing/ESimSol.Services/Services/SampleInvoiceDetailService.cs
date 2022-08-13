using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;




namespace ESimSol.Services.Services
{
    
    public class SampleInvoiceDetailService : MarshalByRefObject, ISampleInvoiceDetailService
    {
        #region Private functions and declaration
        private SampleInvoiceDetail MapObject(NullHandler oReader)
        {
            
            SampleInvoiceDetail oSampleInvoiceDetail = new SampleInvoiceDetail();
            oSampleInvoiceDetail.SampleInvoiceDetailID = oReader.GetInt32("SampleInvoiceDetailID");
            oSampleInvoiceDetail.SampleInvoiceID = oReader.GetInt32("SampleInvoiceID");
            oSampleInvoiceDetail.OrderNo = oReader.GetString("OrderNo");
            oSampleInvoiceDetail.OrderDate = oReader.GetDateTime("OrderDate");
            oSampleInvoiceDetail.PaymentType = oReader.GetInt32("PaymentType");
            oSampleInvoiceDetail.Qty = oReader.GetDouble("Qty");
            oSampleInvoiceDetail.Amount = oReader.GetDouble("Amount");
            oSampleInvoiceDetail.Description = oReader.GetString("Description");
            oSampleInvoiceDetail.MUName = oReader.GetString("MUName");
            oSampleInvoiceDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oSampleInvoiceDetail.ContractorID = oReader.GetInt32("ContractorID");
            oSampleInvoiceDetail.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oSampleInvoiceDetail.ProductID = oReader.GetInt32("ProductID");
            oSampleInvoiceDetail.ProductCode = oReader.GetString("ProductCode");
            oSampleInvoiceDetail.ProductName = oReader.GetString("ProductName");
            return oSampleInvoiceDetail;
            
        }
        private SampleInvoiceDetail MapObject_Detail(NullHandler oReader)
        {

            SampleInvoiceDetail oSampleInvoiceDetail = new SampleInvoiceDetail();
            oSampleInvoiceDetail.UnitPrice = oReader.GetDouble("Rate");
            oSampleInvoiceDetail.Qty = oReader.GetDouble("Qty");

            oSampleInvoiceDetail.ProductName = oReader.GetString("ProductName");
            oSampleInvoiceDetail.ColorName = oReader.GetString("ColorName");
            oSampleInvoiceDetail.ColorNo = oReader.GetString("ColorNo");
            oSampleInvoiceDetail.Shade = (EnumShade)oReader.GetInt16("ShadeA");

            oSampleInvoiceDetail.StyleNo = oReader.GetString("StyleNo");
            

            return oSampleInvoiceDetail;

        }

        private SampleInvoiceDetail CreateObject(NullHandler oReader)
        {
            SampleInvoiceDetail oSampleInvoiceDetail = new SampleInvoiceDetail();
            oSampleInvoiceDetail=MapObject( oReader);
            return oSampleInvoiceDetail;
        }


        private List<SampleInvoiceDetail> CreateObjects(IDataReader oReader)
        {
            List<SampleInvoiceDetail> oSampleInvoiceDetails = new List<SampleInvoiceDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SampleInvoiceDetail oItem = CreateObject(oHandler);
                oSampleInvoiceDetails.Add(oItem);
            }
            return oSampleInvoiceDetails;
        }

        private SampleInvoiceDetail CreateObject_Detail(NullHandler oReader)
        {
            SampleInvoiceDetail oSampleInvoiceDetail = new SampleInvoiceDetail();
            oSampleInvoiceDetail = MapObject_Detail(oReader);
            return oSampleInvoiceDetail;
        }
        private List<SampleInvoiceDetail> CreateObjects_Detail(IDataReader oReader)
        {
            List<SampleInvoiceDetail> oSampleInvoiceDetails = new List<SampleInvoiceDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SampleInvoiceDetail oItem = CreateObject_Detail(oHandler);
                oSampleInvoiceDetails.Add(oItem);
            }
            return oSampleInvoiceDetails;
        }

        #endregion

        #region Interface implementation
        public SampleInvoiceDetailService() { }

        public SampleInvoiceDetail Save(SampleInvoiceDetail oSampleInvoiceDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                if (oSampleInvoiceDetail.SampleInvoiceDetailID <= 0)
                {
                    SampleInvoiceDetailDA.InsertUpdate(tc, oSampleInvoiceDetail, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    SampleInvoiceDetailDA.InsertUpdate(tc, oSampleInvoiceDetail, EnumDBOperation.Update, nUserId);
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save SampleInvoiceDetail", e);
                #endregion
            }
            return oSampleInvoiceDetail;
        }

     
        public string Delete(SampleInvoiceDetail oSampleInvoiceDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VoucherDA.CheckVoucherReference(tc, "SampleInvoice", "SampleInvoiceID", oSampleInvoiceDetail.SampleInvoiceID);
                SampleInvoiceDetailDA.Delete(tc, oSampleInvoiceDetail, EnumDBOperation.Delete, nUserId);
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
        public string UpdateInvoiceID(SampleInvoiceDetail oSampleInvoiceDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VoucherDA.CheckVoucherReference(tc, "SampleInvoice", "SampleInvoiceID", oSampleInvoiceDetail.SampleInvoiceID);
                SampleInvoiceDetailDA.UpdateInvoiceID(tc, oSampleInvoiceDetail.SampleInvoiceID, oSampleInvoiceDetail.DyeingOrderID);
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
        public string Delete_Adj(SampleInvoiceDetail oSampleInvoiceDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                SampleInvoiceDetailDA.Delete_Adj(tc, oSampleInvoiceDetail, EnumDBOperation.Delete, nUserId);
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
  
        public SampleInvoiceDetail Get(int id, Int64 nUserId)
        {
            SampleInvoiceDetail oSampleInvoiceDetail = new SampleInvoiceDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SampleInvoiceDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoiceDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SampleInvoiceDetail", e);
                #endregion
            }

            return oSampleInvoiceDetail;
        }
        public List<SampleInvoiceDetail> Gets( Int64 nUserId)
        {
            List<SampleInvoiceDetail> oSampleInvoiceDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleInvoiceDetailDA.Gets(tc);
                oSampleInvoiceDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleInvoiceDetails", e);
                #endregion
            }

            return oSampleInvoiceDetails;
        }

        public List<SampleInvoiceDetail> Gets(int nPaymentContractID, Int64 nUserId)
        {
            List<SampleInvoiceDetail> oSampleInvoiceDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleInvoiceDetailDA.Gets(tc, nPaymentContractID);
                oSampleInvoiceDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleInvoiceDetails", e);
                #endregion
            }

            return oSampleInvoiceDetails;
        }

        public List<SampleInvoiceDetail> Gets(string sql, Int64 nUserId)
        {
            List<SampleInvoiceDetail> oSampleInvoiceDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleInvoiceDetailDA.Gets(tc, sql);
                oSampleInvoiceDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleInvoiceDetails", e);
                #endregion
            }

            return oSampleInvoiceDetails;
        }
        public List<SampleInvoiceDetail> GetsBy(string sPaymentContractIDs,string nPIID, Int64 nUserId)
        {
            List<SampleInvoiceDetail> oSampleInvoiceDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleInvoiceDetailDA.GetsBy(tc, sPaymentContractIDs, nPIID);
                oSampleInvoiceDetails = CreateObjects_Detail(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleInvoiceDetails", e);
                #endregion
            }

            return oSampleInvoiceDetails;
        }

       
        #endregion
    }    
}
