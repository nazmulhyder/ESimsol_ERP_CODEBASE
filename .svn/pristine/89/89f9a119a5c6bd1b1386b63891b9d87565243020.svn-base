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

    public class SampleInvoiceApproveService : MarshalByRefObject, ISampleInvoiceApproveService
    {
        #region Private functions and declaration
      
        private SampleInvoiceApprove MapObject(NullHandler oReader)
        {
            SampleInvoiceApprove oSampleInvoiceApprove = new SampleInvoiceApprove();
            oSampleInvoiceApprove.SampleInvoiceDate = oReader.GetDateTime("SampleInvoiceDate");
            oSampleInvoiceApprove.ContractorName = oReader.GetString("ContractorName");
            oSampleInvoiceApprove.ProductName = oReader.GetString("ProductName");
            oSampleInvoiceApprove.Color = oReader.GetString("Color");
            oSampleInvoiceApprove.Qty = oReader.GetDouble("Qty");
            oSampleInvoiceApprove.UnitPrice = oReader.GetDouble("UnitPrice");
            oSampleInvoiceApprove.Amount = oReader.GetDouble("Amount");
            oSampleInvoiceApprove.MKTPName = oReader.GetString("MKTPName");
            oSampleInvoiceApprove.PartyConcernPerson = oReader.GetString("PartyConcernPerson");
            oSampleInvoiceApprove.SampleInvoiceNo = oReader.GetString("SampleInvoiceNo");
            oSampleInvoiceApprove.PINo = oReader.GetString("PINo");
            oSampleInvoiceApprove.Outstanding = oReader.GetDouble("Outstanding");
            oSampleInvoiceApprove.ApproveByName = oReader.GetString("ApproveByName");
            oSampleInvoiceApprove.LCNo = oReader.GetString("LCNo");
            oSampleInvoiceApprove.CurrentStatus = oReader.GetInt32("CurrentStatus");
            oSampleInvoiceApprove.PaymentType = oReader.GetInt32("PaymentType");
            oSampleInvoiceApprove.SampleInvoiceType = oReader.GetInt32("SampleInvoiceType");

            return oSampleInvoiceApprove;
        }

        private SampleInvoiceApprove CreateObject(NullHandler oReader)
        {
            SampleInvoiceApprove oSampleInvoiceApprove = new SampleInvoiceApprove();
            oSampleInvoiceApprove = MapObject(oReader);
            return oSampleInvoiceApprove;
        }

        private List<SampleInvoiceApprove> CreateObjects(IDataReader oReader)
        {
            List<SampleInvoiceApprove> oSampleInvoiceApprove = new List<SampleInvoiceApprove>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SampleInvoiceApprove oItem = CreateObject(oHandler);
                oSampleInvoiceApprove.Add(oItem);
            }
            return oSampleInvoiceApprove;
        }
        #region InvoiceWise


        private SampleInvoiceApprove MapObject_InvoiceWise(NullHandler oReader)
        {
            SampleInvoiceApprove oSampleInvoiceApprove = new SampleInvoiceApprove();

            oSampleInvoiceApprove.SampleInvoiceDate = oReader.GetDateTime("SampleInvoiceDate");
            oSampleInvoiceApprove.ContractorName = oReader.GetString("ContractorName");
            //oSampleInvoiceApprove.ProductName = oReader.GetString("ProductName");
            oSampleInvoiceApprove.CurrentStatus = oReader.GetInt32("CurrentStatus");
            oSampleInvoiceApprove.SampleInvoiceID = oReader.GetDouble("SampleInvoiceID");
            oSampleInvoiceApprove.ContractorID = oReader.GetDouble("ContractorID");
            oSampleInvoiceApprove.Amount = oReader.GetDouble("Amount");
            oSampleInvoiceApprove.MKTPName = oReader.GetString("MKTPName");
            oSampleInvoiceApprove.PartyConcernPerson = oReader.GetString("PartyConcernPerson");
            oSampleInvoiceApprove.SampleInvoiceNo = oReader.GetString("SampleInvoiceNo");
            oSampleInvoiceApprove.PINo = oReader.GetString("PINo");
            oSampleInvoiceApprove.LCNo = oReader.GetString("LCNo");
            oSampleInvoiceApprove.Outstanding = oReader.GetDouble("Outstanding");
            oSampleInvoiceApprove.ApproveByName = oReader.GetString("ApproveByName");
            oSampleInvoiceApprove.PaymentType = oReader.GetInt32("PaymentType");
            oSampleInvoiceApprove.SampleInvoiceType = oReader.GetInt32("SampleInvoiceType");

            return oSampleInvoiceApprove;
        }

        private SampleInvoiceApprove CreateObject_InvoiceWise(NullHandler oReader)
        {
            SampleInvoiceApprove oSampleInvoiceApprove = new SampleInvoiceApprove();
            oSampleInvoiceApprove = MapObject_InvoiceWise(oReader);
            return oSampleInvoiceApprove;
        }

        private List<SampleInvoiceApprove> CreateObjects_InvoiceWise(IDataReader oReader)
        {
            List<SampleInvoiceApprove> oSampleInvoiceApprove = new List<SampleInvoiceApprove>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SampleInvoiceApprove oItem = CreateObject_InvoiceWise(oHandler);
                oSampleInvoiceApprove.Add(oItem);
            }
            return oSampleInvoiceApprove;
        }
        #endregion
        
        #endregion

        #region Interface implementation
        public SampleInvoiceApproveService() { }

        //public List<SampleInvoiceApprove> Gets(DateTime dStartDate, DateTime dEndDate, int nCurrentStatus, int nReportType, Int64 nUserId)
        //{
        //    List<SampleInvoiceApprove> oSampleInvoiceApproves = null;

        //    TransactionContext tc = null;

        //    try
        //    {
        //        tc = TransactionContext.Begin();

        //        IDataReader reader = null;
        //        reader = SampleInvoiceApproveDA.Gets(tc, dStartDate, dEndDate,nCurrentStatus, nReportType);
        //        if (nReportType == 1)
        //        {
        //            oSampleInvoiceApproves = CreateObjects_InvoiceWise(reader);
        //        }
        //        else
        //        {
        //            oSampleInvoiceApproves = CreateObjects(reader);
        //        }
               
        //        reader.Close();
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();

        //        ExceptionLog.Write(e);
        //        throw new ServiceException("Failed to Get SampleInvoiceApprove", e);
        //        #endregion
        //    }

        //    return oSampleInvoiceApproves;
        //}
        public List<SampleInvoiceApprove> Gets(string sSQL, int nReportType, Int64 nUserId)
        {
            List<SampleInvoiceApprove> oSampleInvoiceApproves = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleInvoiceApproveDA.Gets(tc, sSQL, nReportType);
                if (nReportType == 1)
                {
                    oSampleInvoiceApproves = CreateObjects_InvoiceWise(reader);
                }
                else
                {
                    oSampleInvoiceApproves = CreateObjects(reader);
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
                throw new ServiceException("Failed to Get SampleInvoiceApprove", e);
                #endregion
            }

            return oSampleInvoiceApproves;
        }

        #endregion
    }    
    
  
}
