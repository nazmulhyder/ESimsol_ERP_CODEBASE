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

    public class ExportBillReportService : MarshalByRefObject, IExportBillReportService
    {
        #region Private functions and declaration
        private ExportBillReport MapObject(NullHandler oReader)
        {
            ExportBillReport oExportBillReport = new ExportBillReport();
            oExportBillReport.ExportBillID = oReader.GetInt32("ExportBillID");
            oExportBillReport.ExportBillNo = oReader.GetString("ExportBillNo");
            oExportBillReport.Amount = oReader.GetDouble("Amount");
            oExportBillReport.UnitPrice = oReader.GetDouble("UnitPrice");
            oExportBillReport.State = (EnumLCBillEvent)oReader.GetInt16("State");
            oExportBillReport.StateInt = oReader.GetInt16("State");
            oExportBillReport.StartDate = oReader.GetDateTime("StartDate");
            oExportBillReport.SendToParty = oReader.GetDateTime("SendToParty");
            oExportBillReport.RecdFromParty = oReader.GetDateTime("RecdFromParty");
            oExportBillReport.SendToBankDate = oReader.GetDateTime("SendToBank");
            oExportBillReport.RecedFromBankDate = oReader.GetDateTime("RecdFromBank");
            oExportBillReport.IsActive = oReader.GetBoolean("IsActive");
            oExportBillReport.Bill = oReader.GetString("Bill");
            oExportBillReport.MasterLCNos = oReader.GetString("MasterLCNos");
            oExportBillReport.Sequence = oReader.GetInt32("Sequence");
            oExportBillReport.DocPrepareDate = oReader.GetDateTime("DBServerDate");
           // oExportBillReport.Qty_Bill = oReader.GetDouble("Qty_Bill");
            //oExportBillReport.DBServerUserID = oReader.GetInt32("DBServerUserID");
            //oExportBillReport.DBServerDate = oReader.GetDateTime("DBServerDate");

            // Drive property 
            ///Export LC
            oExportBillReport.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportBillReport.BUID = oReader.GetInt32("BUID");
            oExportBillReport.ExportLCNo = oReader.GetString("ExportLCNo");
            //oExportBillReport.CreditAvailableDays = oReader.GetInt32("CreditAvailableDays");
            oExportBillReport.LCOpeningDate = oReader.GetDateTime("LCOpeningDate");
            oExportBillReport.LCRecivedDate = oReader.GetDateTime("LCRecivedDate");
            //oExportBillReport.AtSightDiffered = oReader.GetBoolean("AtSightDiffered");
            oExportBillReport.Amount_LC = oReader.GetDouble("Amount_LC");
            oExportBillReport.ApplicantName = oReader.GetString("ApplicantName");
            oExportBillReport.ApplicantID = oReader.GetInt32("ApplicantID");
            oExportBillReport.BankBranchID_Nego = oReader.GetInt32("BankBranchID_Negotiation");
            oExportBillReport.BankBranchID_Advice = oReader.GetInt32("BankBranchID_Advice");
            oExportBillReport.BankBranchID_Issue = oReader.GetInt32("BankBranchID_Issue");
            oExportBillReport.OverDueRate = oReader.GetDouble("OverdueRate");
            oExportBillReport.ApplicantAddress = oReader.GetString("ApplicantAddress");
            oExportBillReport.Currency = oReader.GetString("Currency");
            oExportBillReport.PINo = oReader.GetString("PINo");

            oExportBillReport.BankAddress_Issue = oReader.GetString("BankAddress_Issue");
            ///LDBC
            oExportBillReport.LDBCID = oReader.GetInt32("ExportLDBCID");
            oExportBillReport.MaturityDate = oReader.GetDateTime("MaturityDate");
            oExportBillReport.MaturityReceivedDate = oReader.GetDateTime("MaturityReceivedDate");
            oExportBillReport.LDBCNo = oReader.GetString("LDBCNo");
            oExportBillReport.LDBPNo = oReader.GetString("LDBPNo");
            oExportBillReport.LDBPAmount = oReader.GetDouble("LDBPAmount");
            oExportBillReport.LDBCDate = oReader.GetDateTime("LDBCDate");
            oExportBillReport.AcceptanceDate = oReader.GetDateTime("AcceptanceDate");
            oExportBillReport.DiscountedDate = oReader.GetDateTime("DiscountedDate");
            oExportBillReport.BankFDDRecDate = oReader.GetDateTime("BankFDDRecDate");
            oExportBillReport.RelizationDate = oReader.GetDateTime("RelizationDate");
            oExportBillReport.EncashmentDate = oReader.GetDateTime("EncashmentDate");
            oExportBillReport.LCTramsID = oReader.GetInt32("LCTramsID");
            //oExportBillReport.TrgNote = oReader.GetString("TrgNote");     

            /// Bank 
            oExportBillReport.BankName_Nego = oReader.GetString("BankName_Nego");
            oExportBillReport.BBranchName_Nego = oReader.GetString("BBranchName_Nego");
            oExportBillReport.BankAddress_Nego = oReader.GetString("BankAddress_Nego");
            oExportBillReport.BankName_Advice = oReader.GetString("BankName_Advice");
            oExportBillReport.BBranchName_Advice = oReader.GetString("BBranchName_Advice");
            oExportBillReport.BankAddress_Advice = oReader.GetString("BankAddress_Advice");
            oExportBillReport.BankName_Issue = oReader.GetString("BankName_Issue");
            oExportBillReport.BBranchName_Issue = oReader.GetString("BBranchName_Issue");
            oExportBillReport.BankAddress_Issue = oReader.GetString("BankAddress_Issue");

            oExportBillReport.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oExportBillReport.ExpiryDate = oReader.GetDateTime("ExpiryDate");
            oExportBillReport.MKTPName = oReader.GetString("MKTPName");
            oExportBillReport.ProductName = oReader.GetString("ProductName");//ForExportBillRegister
            oExportBillReport.Qty = oReader.GetDouble("Qty");//ForExportBillRegister
            oExportBillReport.Qty_Bill = oReader.GetDouble("Qty_Bill");//ForExportBillRegister
            oExportBillReport.ExportLCType = (EnumExportLCType)oReader.GetInt32("ExportLCType");
            if (oExportBillReport.ExportLCType == EnumExportLCType.FDD || oExportBillReport.ExportLCType == EnumExportLCType.TT) { oExportBillReport.ExportLCNo = oExportBillReport.ExportLCType.ToString() + "" + oExportBillReport.ExportLCNo; }
            return oExportBillReport;
        }

        private ExportBillReport CreateObject(NullHandler oReader)
        {
            ExportBillReport oExportBillReport = new ExportBillReport();
            oExportBillReport = MapObject(oReader);
            return oExportBillReport;
        }

        private List<ExportBillReport> CreateObjects(IDataReader oReader)
        {
            List<ExportBillReport> oExportBillReports = new List<ExportBillReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportBillReport oItem = CreateObject(oHandler);
                oExportBillReports.Add(oItem);
            }
            return oExportBillReports;
        }
        #endregion

        #region Interface implementation
        public ExportBillReportService() { }

        public ExportBillReport Get(int id, Int64 nUserID)
        {
            ExportBillReport oExportBillReport = new ExportBillReport();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportBillReportDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBillReport = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportBillReport", e);
                #endregion
            }

            return oExportBillReport;
        }

        public List<ExportBillReport> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportBillReport> oExportBillReports = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportBillReportDA.Gets(tc, sSQL);
                oExportBillReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBillReports", e);
                #endregion
            }

            return oExportBillReports;
        }

        public ExportBillReport GetByLDBCNo(string sLDBCNo, Int64 nUserID)
        {
            ExportBillReport oExportBillReport = new ExportBillReport();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportBillReportDA.GetByLDBCNo(tc, sLDBCNo);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBillReport = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportBillReport", e);
                #endregion
            }

            return oExportBillReport;
        }

        #endregion
    }
}
