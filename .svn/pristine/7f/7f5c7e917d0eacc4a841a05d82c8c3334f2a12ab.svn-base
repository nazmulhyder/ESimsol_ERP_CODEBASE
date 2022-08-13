using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;




namespace ESimSol.Services.Services
{
    
    public class ExportBillPendingReportService : MarshalByRefObject, IExportBillPendingReportService
    {
        #region Private functions and declaration
        private ExportBillPendingReport MapObject(NullHandler oReader)
        {
            ExportBillPendingReport oExportBillPendingReport = new ExportBillPendingReport();
            oExportBillPendingReport.ExportBillNo = oReader.GetString("ExportBillNo");
            oExportBillPendingReport.ExportBillID = oReader.GetInt32("ExportBillID");
            oExportBillPendingReport.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportBillPendingReport.Amount = oReader.GetDouble("Amount");
            oExportBillPendingReport.Amount_LC = oReader.GetDouble("Amount_LC");
            oExportBillPendingReport.DeliveryValue = oReader.GetDouble("DeliveryValue");
            oExportBillPendingReport.Qty = oReader.GetDouble("Qty");
            oExportBillPendingReport.State = (EnumLCBillEvent)oReader.GetInt16("State");
            oExportBillPendingReport.LCStatus = (EnumExportLCStatus)oReader.GetInt16("LCStatus");
            //oExportBillPendingReport.StateInt = oReader.GetInt16("State");
            oExportBillPendingReport.StartDate = oReader.GetDateTime("StartDate");
            oExportBillPendingReport.SendToParty = oReader.GetDateTime("SendToParty");
            oExportBillPendingReport.RecdFromParty = oReader.GetDateTime("RecdFromParty");
            oExportBillPendingReport.SendToBankDate = oReader.GetDateTime("SendToBank");
            oExportBillPendingReport.RecedFromBankDate = oReader.GetDateTime("RecdFromBank");
            oExportBillPendingReport.BBranchName_Nego = oReader.GetString("BBranchName_Nego");
            oExportBillPendingReport.BBranchID_Nego = oReader.GetInt32("BBranchID_Nego");
            oExportBillPendingReport.BankName_Issue = oReader.GetString("BankName_Issue");
            oExportBillPendingReport.ExportLCNo = oReader.GetString("ExportLCNo");
            oExportBillPendingReport.BankName_Nego = oReader.GetString("BankName_Nego");
            oExportBillPendingReport.ApplicantName = oReader.GetString("ContractorName");
            oExportBillPendingReport.MKTPName = oReader.GetString("MKTPName");
            oExportBillPendingReport.OverDueRate = oReader.GetDouble("OverdueRate");
            oExportBillPendingReport.MaturityDate = oReader.GetDateTime("MaturityDate");
            oExportBillPendingReport.MaturityReceivedDate = oReader.GetDateTime("MaturityReceivedDate");
            oExportBillPendingReport.LDBCNo = oReader.GetString("LDBCNo");
            oExportBillPendingReport.LDBPNo = oReader.GetString("LDBPNo");
            oExportBillPendingReport.LDBPAmount = oReader.GetDouble("LDBPAmount");
            oExportBillPendingReport.LDBCDate = oReader.GetDateTime("LDBCDate");
            oExportBillPendingReport.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oExportBillPendingReport.LCOpeningDate = oReader.GetDateTime("LCOpeningDate");
            oExportBillPendingReport.ApplicantID = oReader.GetInt32("ContractorID");
            oExportBillPendingReport.BBranchName_Issue = oReader.GetString("BBranchName_Issue");
        

            //oExportBillPendingReport.CreditAvailableDays = oReader.GetInt32("CreditAvailableDays");
            //oExportBillPendingReport.LCRecivedDate = oReader.GetDateTime("LCRecivedDate");
            //oExportBillPendingReport.AtSightDiffered = oReader.GetBoolean("AtSightDiffered");

            //oExportBillPendingReport.BankBranchID_Nego = oReader.GetInt32("BankBranchID_Negotiation");
            //oExportBillPendingReport.BankBranchID_Advice = oReader.GetInt32("BankBranchID_Advice");
            //oExportBillPendingReport.BankBranchID_Issue = oReader.GetInt32("BankBranchID_Issue");
            //oExportBillPendingReport.ApplicantAddress = oReader.GetString("ApplicantAddress");
            //oExportBillPendingReport.Currency = oReader.GetString("Currency");
            ///LDBC
            //oExportBillPendingReport.LDBCID = oReader.GetInt32("ExportLDBCID");
        
            //oExportBillPendingReport.AcceptanceDate = oReader.GetDateTime("AcceptanceDate");
            //oExportBillPendingReport.DiscountedDate = oReader.GetDateTime("DiscountedDate");
            //oExportBillPendingReport.BankFDDRecDate = oReader.GetDateTime("BankFDDRecDate");
            //oExportBillPendingReport.RelizationDate = oReader.GetDateTime("RelizationDate");
            //oExportBillPendingReport.EncashmentDate = oReader.GetDateTime("EncashmentDate");
            //oExportBillPendingReport.LCTramsID = oReader.GetInt32("LCTramsID");
            //oExportBillPendingReport.TrgNote = oReader.GetString("TrgNote");     

            //oExportBillPendingReport.BankName_Advice = oReader.GetString("BankName_Advice");
            //oExportBillPendingReport.BBranchName_Advice = oReader.GetString("BBranchName_Advice");


            return oExportBillPendingReport;            
        }

        private ExportBillPendingReport CreateObject(NullHandler oReader)
        {
            ExportBillPendingReport oExportBillPendingReport = new ExportBillPendingReport();
            oExportBillPendingReport=MapObject(oReader);
            return oExportBillPendingReport;
        }

        private List<ExportBillPendingReport> CreateObjects(IDataReader oReader)
        {
            List<ExportBillPendingReport> oExportBillPendingReports = new List<ExportBillPendingReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportBillPendingReport oItem = CreateObject(oHandler);
                oExportBillPendingReports.Add(oItem);
            }
            return oExportBillPendingReports;
        }
        #endregion

        #region Interface implementation
        public ExportBillPendingReportService() { }

  
        public List<ExportBillPendingReport> Gets( Int64 nUserID)
        {
            List<ExportBillPendingReport> oExportBillPendingReports = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportBillPendingReportDA.Gets(tc);
                oExportBillPendingReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBillPendingReports", e);
                #endregion
            }

            return oExportBillPendingReports;
        }
        public List<ExportBillPendingReport> Gets(int nReportType, int nDiscountType, Int64 nUserID)
        {
            List<ExportBillPendingReport> oExportBillPendingReports = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportBillPendingReportDA.GetsReport(tc, nReportType, nDiscountType);
                oExportBillPendingReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBillPendingReports", e);
                #endregion
            }

            return oExportBillPendingReports;
        }
        public List<ExportBillPendingReport> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportBillPendingReport> oExportBillPendingReports = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportBillPendingReportDA.Gets(tc, sSQL);
                oExportBillPendingReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBillPendingReports", e);
                #endregion
            }

            return oExportBillPendingReports;
        }
  
       
        #endregion
    }
}
