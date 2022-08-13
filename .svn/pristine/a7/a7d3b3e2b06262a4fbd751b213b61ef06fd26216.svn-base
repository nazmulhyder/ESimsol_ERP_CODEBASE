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
    public class VoucherReportService : MarshalByRefObject, IVoucherReportService
    {
        #region Private functions and declaration
        private VoucherReport MapObject(NullHandler oReader)
        {
            VoucherReport oVoucherReport = new VoucherReport();
            oVoucherReport.VoucherID = oReader.GetInt64("VoucherReportID");
          
            oVoucherReport.VoucherTypeID = oReader.GetInt32("VoucherReportTypeID");
            oVoucherReport.VoucherNo = oReader.GetString("VoucherReportNo");
          
            oVoucherReport.VoucherDate = oReader.GetDateTime("VoucherReportDate");
            oVoucherReport.CurrencyId = oReader.GetInt32("CurrencyID");
            oVoucherReport.CurrencyConversionRate = oReader.GetDouble("CurrencyConversionRate");
           
            oVoucherReport.VoucherReportAmount = oReader.GetDouble("VoucherReportAmount");

            //Derive
            oVoucherReport.VoucherReportName = oReader.GetString("VoucherReportName");
            oVoucherReport.NumberMethod = (EnumNumberMethod)oReader.GetInt32("NumberMethod");
            oVoucherReport.CurrencyNameSymbol = oReader.GetString("CurrencyNameSymbol");
            oVoucherReport.BaseCurrencyId = oReader.GetInt32("BaseCurrencyId");
            oVoucherReport.BaseCurrencyNameSymbol = oReader.GetString("BaseCurrencyNameSymbol");            
            oVoucherReport.AuthorizedByName = oReader.GetString("AuthorizedByName");
            oVoucherReport.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oVoucherReport.AmountInBaseCurrency = oReader.GetDouble("AmountInBaseCurrency");
            
            return oVoucherReport;
        }
        private VoucherReport CreateObject(NullHandler oReader)
        {
            VoucherReport oVoucherReport = new VoucherReport();
            oVoucherReport = MapObject(oReader);
            return oVoucherReport;
        }

        private List<VoucherReport> CreateObjects(IDataReader oReader)
        {
            List<VoucherReport> oVoucherReport = new List<VoucherReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VoucherReport oItem = CreateObject(oHandler);
                oVoucherReport.Add(oItem);
            }
            return oVoucherReport;
        }
        #endregion


    
        #region Interface implementation
        public VoucherReportService() { }




        public DataSet Gets_Report(DateTime dStartDate, DateTime dEndDate, int nVoucherTypeID, int nReportType, int nUserID)
        {
            TransactionContext tc = null;
            DataSet oDataSet = new DataSet();
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherReportDA.Gets_Report(tc,  dStartDate,  dEndDate, nVoucherTypeID,  nReportType);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[2]);
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FROM View _Voucher_Report", e);
                #endregion
            }
            return oDataSet;
        }

     
        #endregion
    }   
}