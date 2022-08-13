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
    public class VoucherRefReportService : MarshalByRefObject, IVoucherRefReportService
    {
        #region Private functions and declaration
        private VoucherRefReport MapObject(NullHandler oReader)
        {
            VoucherRefReport oVoucherRefReport = new VoucherRefReport();
            // oVoucherRefReport.CCTID = oReader.GetInt32("CCTID");
            oVoucherRefReport.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oVoucherRefReport.DebitAmount = oReader.GetDouble("DebitAmount");
            oVoucherRefReport.CreditAmount = oReader.GetDouble("CreditAmount");
            oVoucherRefReport.OpeningBalance = oReader.GetDouble("OpeningBalance");
            oVoucherRefReport.ClosingBalance = oReader.GetDouble("ClosingBalance");
            oVoucherRefReport.IsDebit = oReader.GetBoolean("IsDebit");
            oVoucherRefReport.BillNo = oReader.GetString("BillNo");

            oVoucherRefReport.BillDate = oReader.GetDateTime("BillDate");
            oVoucherRefReport.DueDate = oReader.GetDateTime("DueDate");

            oVoucherRefReport.VoucherBillID = oReader.GetInt32("VoucherBillID");

              oVoucherRefReport.Amount = oReader.GetDouble("Amount");
            oVoucherRefReport.RemainingBalance = oReader.GetDouble("RemainingBalance");
            oVoucherRefReport.CreditDays = oReader.GetInt32("CreditDays");
            oVoucherRefReport.ComponentType = (EnumComponentType)oReader.GetInt16("ComponentType");
            oVoucherRefReport.OverdueByDays = oReader.GetInt32("OverdueByDays");
            oVoucherRefReport.CCID = oReader.GetInt32("CCID");

            oVoucherRefReport.VoucherID = oReader.GetInt32("VoucherID");
            oVoucherRefReport.VoucherNo = oReader.GetString("VoucherNo");
            oVoucherRefReport.VoucherDate = oReader.GetDateTime("VoucherDate");
            oVoucherRefReport.AccountHeadName = oReader.GetString("AccountHeadName");
            oVoucherRefReport.SubledgerCode = oReader.GetString("SubledgerCode");
            oVoucherRefReport.SubledgerName = oReader.GetString("SubledgerName");
            return oVoucherRefReport;
        }

        private VoucherRefReport CreateObject(NullHandler oReader)
        {
            VoucherRefReport oVoucherRefReport = new VoucherRefReport();
            oVoucherRefReport = MapObject(oReader);
            return oVoucherRefReport;
        }

        private List<VoucherRefReport> CreateObjects(IDataReader oReader)
        {
            List<VoucherRefReport> oVoucherRefReport = new List<VoucherRefReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VoucherRefReport oItem = CreateObject(oHandler);
                oVoucherRefReport.Add(oItem);
            }
            return oVoucherRefReport;
        }

       

        #endregion

        #region Interface implementation

        public VoucherRefReportService() { }

   

       

        public List<VoucherRefReport> GetsVoucherBillBreakDown(VoucherRefReport oVRR, int nUserID)
        {
            List<VoucherRefReport> oVoucherRefReports = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VoucherRefReportDA.GetsVoucherBillBreakDown(tc, oVRR);
                oVoucherRefReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Report", e);
                #endregion
            }
            return oVoucherRefReports;
        }
        public List<VoucherRefReport> GetsVoucherBillDetails(VoucherRefReport oVRR, int nUserID)
        {
            List<VoucherRefReport> oVoucherRefReports = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VoucherRefReportDA.GetsVoucherBillDetails(tc, oVRR);
                oVoucherRefReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Report", e);
                #endregion
            }
            return oVoucherRefReports;
        }
        #endregion
    }   

   
}
