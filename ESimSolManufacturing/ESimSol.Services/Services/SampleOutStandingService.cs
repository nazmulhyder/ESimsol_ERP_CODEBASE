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
    public class SampleOutStandingService : MarshalByRefObject, ISampleOutStandingService
    {
        #region Private functions and declaration
        

        #region Party Wise
        private SampleOutStanding MapObject_Party(NullHandler oReader)
        {
            SampleOutStanding oSampleOutStanding = new SampleOutStanding();
            oSampleOutStanding.OrderDate = oReader.GetDateTime("StartDate");
            oSampleOutStanding.OrderDate = oReader.GetDateTime("EndDate");
            oSampleOutStanding.ContractorName = oReader.GetString("ContractorName");
            oSampleOutStanding.ContractorID = oReader.GetInt32("ContractorID");
            oSampleOutStanding.Opening = oReader.GetDouble("Opening");
            oSampleOutStanding.Debit = oReader.GetDouble("Debit");
            oSampleOutStanding.Credit = oReader.GetDouble("Credit");
            oSampleOutStanding.Closing = oReader.GetDouble("Closing");
            oSampleOutStanding.Currency = oReader.GetString("Currency");
            oSampleOutStanding.MarketingAccountID = oReader.GetInt32("MarketingAccountID");
            oSampleOutStanding.MAName = oReader.GetString("MAName");

            return oSampleOutStanding;
        }

        private SampleOutStanding CreateObject_Party(NullHandler oReader)
        {
            SampleOutStanding oSampleOutStanding = new SampleOutStanding();
            oSampleOutStanding = MapObject_Party(oReader);
            return oSampleOutStanding;
        }

        private List<SampleOutStanding> CreateObjects_Party(IDataReader oReader)
        {
            List<SampleOutStanding> oSampleOutStanding = new List<SampleOutStanding>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SampleOutStanding oItem = CreateObject_Party(oHandler);
                oSampleOutStanding.Add(oItem);
            }
            return oSampleOutStanding;
        }

        #endregion
        #region Debit
        private SampleOutStanding MapObject_DR(NullHandler oReader)
        {
            SampleOutStanding oSampleOutStanding = new SampleOutStanding();
            oSampleOutStanding.OrderNo = oReader.GetString("OrderNo");
            oSampleOutStanding.OrderDate = oReader.GetDateTime("OrderDate");
            oSampleOutStanding.ContractorID = oReader.GetInt32("ContractorID");
            oSampleOutStanding.ContractorName = oReader.GetString("ContractorName");
            oSampleOutStanding.ProductName = oReader.GetString("ProductName");
            oSampleOutStanding.Qty = oReader.GetDouble("Qty");
            oSampleOutStanding.UnitPrice = oReader.GetDouble("UnitPrice");
            oSampleOutStanding.Amount = oReader.GetDouble("Amount");
            //oSampleOutStanding.DeliveryToName = oReader.GetString("DeliveryToName");
            oSampleOutStanding.MKTPName = oReader.GetString("MKTPName");
            oSampleOutStanding.CPName = oReader.GetString("CPName");
            oSampleOutStanding.Note = oReader.GetString("Note");
            oSampleOutStanding.RefNo = oReader.GetString("RefNo");
            oSampleOutStanding.RefDate = oReader.GetDateTime("RefDate");
            //oSampleOutStanding.SampleInvoiceNo = oReader.GetString("SampleInvoiceNo");
            //oSampleOutStanding.Credit = oReader.GetDouble("Credit");
            //oSampleOutStanding.Debit = oReader.GetDouble("Debit");
            //oSampleOutStanding.MUName = oReader.GetString("MUName");
            //oSampleOutStanding.PINo = oReader.GetString("PINo");
            //oSampleOutStanding.ExportLCNo = oReader.GetString("ExportLCNo");
            //oSampleOutStanding.OrderType = oReader.GetInt32("DyeingOrderType");
            oSampleOutStanding.InvoiceType = oReader.GetInt32("InvoiceType");
            oSampleOutStanding.PINo = oReader.GetString("PINo");
            oSampleOutStanding.LCNo = oReader.GetString("LCNo");
            oSampleOutStanding.InvoiceType = oReader.GetInt32("InvoiceType");
            oSampleOutStanding.PaymentType = oReader.GetInt32("PaymentType");
            oSampleOutStanding.Currency = oReader.GetString("Currency");

            oSampleOutStanding.Credit = oReader.GetDouble("Credit");
            oSampleOutStanding.Debit = oReader.GetDouble("Debit");
            oSampleOutStanding.Opening = oReader.GetDouble("Opening");
            //oSampleOutStanding.AmendmentRequired = oReader.GetBoolean("AmendmentRequired");
            return oSampleOutStanding;
        }

        private SampleOutStanding CreateObject_DR(NullHandler oReader)
        {
            SampleOutStanding oSampleOutStanding = new SampleOutStanding();
            oSampleOutStanding = MapObject_DR(oReader);
            return oSampleOutStanding;
        }

        private List<SampleOutStanding> CreateObjects_DR(IDataReader oReader)
        {
            List<SampleOutStanding> oSampleOutStanding = new List<SampleOutStanding>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SampleOutStanding oItem = CreateObject_DR(oHandler);
                oSampleOutStanding.Add(oItem);
            }
            return oSampleOutStanding;
        }
        #endregion
        #region Cr
        private SampleOutStanding MapObject_CR(NullHandler oReader)
        {
            SampleOutStanding oSampleOutStanding = new SampleOutStanding();
            oSampleOutStanding.SampleInvoiceNo = oReader.GetString("SampleInvoiceNo");
            oSampleOutStanding.SampleInvoiceDate = oReader.GetDateTime("SampleInvoiceDate");
            oSampleOutStanding.ContractorID = oReader.GetInt32("ContractorID");
            oSampleOutStanding.ContractorName = oReader.GetString("ContractorName");
            oSampleOutStanding.ProductName = oReader.GetString("ProductName");
            oSampleOutStanding.Qty = oReader.GetDouble("Qty");
            oSampleOutStanding.UnitPrice = oReader.GetDouble("UnitPrice");
            oSampleOutStanding.Amount = oReader.GetDouble("Amount");
            //oSampleOutStanding.DeliveryToName = oReader.GetString("DeliveryToName");
            oSampleOutStanding.MKTPName = oReader.GetString("MKTPName");
            oSampleOutStanding.CPName = oReader.GetString("CPName");
            oSampleOutStanding.Note = oReader.GetString("Note");
            //oSampleOutStanding.MUName = oReader.GetString("MUName");
            oSampleOutStanding.PINo = oReader.GetString("PINo");
            oSampleOutStanding.LCNo = oReader.GetString("LCNo");
            oSampleOutStanding.InvoiceType = oReader.GetInt32("InvoiceType");
            oSampleOutStanding.PaymentType = oReader.GetInt32("PaymentType");
            //oSampleOutStanding.Currency = oReader.GetString("Currency");
            //oSampleOutStanding.AmendmentRequired = oReader.GetBoolean("AmendmentRequired");
            return oSampleOutStanding;
        }

        private SampleOutStanding CreateObject_CR(NullHandler oReader)
        {
            SampleOutStanding oSampleOutStanding = new SampleOutStanding();
            oSampleOutStanding = MapObject_CR(oReader);
            return oSampleOutStanding;
        }

        private List<SampleOutStanding> CreateObjects_CR(IDataReader oReader)
        {
            List<SampleOutStanding> oSampleOutStanding = new List<SampleOutStanding>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SampleOutStanding oItem = CreateObject_CR(oHandler);
                oSampleOutStanding.Add(oItem);
            }
            return oSampleOutStanding;
        }
        #endregion
        #endregion

      

        #region Interface implementation
        public SampleOutStandingService() { }

        public List<SampleOutStanding> Gets(string sSQL, bool bIsDr,Int64 nUserID)
        {
            List<SampleOutStanding> oSampleOutStanding = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleOutStandingDA.Gets(tc, sSQL, bIsDr);
                if (bIsDr)
                {
                    oSampleOutStanding = CreateObjects_DR(reader);
                }
                else
                {
                    oSampleOutStanding = CreateObjects_CR(reader);
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
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oSampleOutStanding;
        }
        public List<SampleOutStanding> Gets(DateTime dStartDate, DateTime dEndDate, string sContractorIDs, Int64 nUserID)
        {
            List<SampleOutStanding> oSampleOutStanding = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleOutStandingDA.Gets(tc,  dStartDate,  dEndDate,  sContractorIDs);
                oSampleOutStanding = CreateObjects_Party(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oSampleOutStanding;
        }
        public List<SampleOutStanding> GetsWithMkt(DateTime dStartDate, DateTime dEndDate, string sContractorIDs, Int64 nUserID)
        {
            List<SampleOutStanding> oSampleOutStanding = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleOutStandingDA.GetsWithMkt(tc, dStartDate, dEndDate, sContractorIDs);
                oSampleOutStanding = CreateObjects_Party(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oSampleOutStanding;
        }
        public List<SampleOutStanding> GetsMktDetail(DateTime dStartDate, DateTime dEndDate, string sContractorIDs, Int64 nUserID)
        {
            List<SampleOutStanding> oSampleOutStanding = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleOutStandingDA.GetsMktDetail(tc, dStartDate, dEndDate, sContractorIDs);
                oSampleOutStanding = CreateObjects_DR(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oSampleOutStanding;
        }

        #endregion
    }
}