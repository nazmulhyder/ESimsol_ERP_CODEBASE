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

    public class ExportOutstandingDetailService : MarshalByRefObject, IExportOutstandingDetailService
    {
        #region Private functions and declaration
        private ExportOutstandingDetail MapObject(NullHandler oReader)
        {
            ExportOutstandingDetail oExportOutstandingDetail = new ExportOutstandingDetail();
            oExportOutstandingDetail.ExportBillNo = oReader.GetString("ExportBillNo");
            oExportOutstandingDetail.ExportBillID = oReader.GetInt32("ExportBillID");
            oExportOutstandingDetail.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportOutstandingDetail.Amount = oReader.GetDouble("Amount");
            oExportOutstandingDetail.Amount_LC = oReader.GetDouble("Amount_LC");
            oExportOutstandingDetail.DeliveryValue = oReader.GetDouble("DeliveryValue");
            oExportOutstandingDetail.DeliveryQty = oReader.GetDouble("DeliveryQty");
            oExportOutstandingDetail.Qty = oReader.GetDouble("Qty");
            oExportOutstandingDetail.State = (EnumLCBillEvent)oReader.GetInt16("State");
            oExportOutstandingDetail.LCStatus = (EnumExportLCStatus)oReader.GetInt16("LCStatus");
            //oExportOutstandingDetail.StateInt = oReader.GetInt16("State");
            oExportOutstandingDetail.StartDate = oReader.GetDateTime("StartDate");
            oExportOutstandingDetail.SendToParty = oReader.GetDateTime("SendToParty");
            oExportOutstandingDetail.RecdFromParty = oReader.GetDateTime("RecdFromParty");
            oExportOutstandingDetail.SendToBankDate = oReader.GetDateTime("SendToBank");
            oExportOutstandingDetail.RecedFromBankDate = oReader.GetDateTime("RecdFromBank");
            oExportOutstandingDetail.BBranchName_Nego = oReader.GetString("BBranchName_Nego");
            oExportOutstandingDetail.BBranchID_Nego = oReader.GetInt32("BBranchID_Nego");
            oExportOutstandingDetail.BankName_Issue = oReader.GetString("BankName_Issue");
            oExportOutstandingDetail.ExportLCNo = oReader.GetString("ExportLCNo");
            oExportOutstandingDetail.BankName_Nego = oReader.GetString("BankName_Nego");
            oExportOutstandingDetail.ContractorName = oReader.GetString("ContractorName");
            oExportOutstandingDetail.MKTPName = oReader.GetString("MKTPName");
            oExportOutstandingDetail.OverDueRate = oReader.GetDouble("OverdueRate");
            oExportOutstandingDetail.MaturityDate = oReader.GetDateTime("MaturityDate");
            oExportOutstandingDetail.MaturityReceivedDate = oReader.GetDateTime("MaturityReceivedDate");
            oExportOutstandingDetail.LDBCNo = oReader.GetString("LDBCNo");
            oExportOutstandingDetail.LDBPNo = oReader.GetString("LDBPNo");
            oExportOutstandingDetail.LDBPAmount = oReader.GetDouble("LDBPAmount");
            oExportOutstandingDetail.LDBCDate = oReader.GetDateTime("LDBCDate");
            oExportOutstandingDetail.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oExportOutstandingDetail.LCOpeningDate = oReader.GetDateTime("LCOpeningDate");
            oExportOutstandingDetail.ContractorID = oReader.GetInt32("ContractorID");
            oExportOutstandingDetail.BBranchName_Issue = oReader.GetString("BBranchName_Issue");
            oExportOutstandingDetail.PINo = oReader.GetString("PINo"); //State
            oExportOutstandingDetail.MKTPName = oReader.GetString("MKTPName");
            oExportOutstandingDetail.Currency = oReader.GetString("Currency");

            //oExportOutstandingDetail.CreditAvailableDays = oReader.GetInt32("CreditAvailableDays");
            //oExportOutstandingDetail.LCRecivedDate = oReader.GetDateTime("LCRecivedDate");
            //oExportOutstandingDetail.AtSightDiffered = oReader.GetBoolean("AtSightDiffered");

            //oExportOutstandingDetail.BankBranchID_Nego = oReader.GetInt32("BankBranchID_Negotiation");
            //oExportOutstandingDetail.BankBranchID_Advice = oReader.GetInt32("BankBranchID_Advice");
            //oExportOutstandingDetail.BankBranchID_Issue = oReader.GetInt32("BankBranchID_Issue");
            //oExportOutstandingDetail.ApplicantAddress = oReader.GetString("ApplicantAddress");
            //oExportOutstandingDetail.Currency = oReader.GetString("Currency");
            ///LDBC
            //oExportOutstandingDetail.LDBCID = oReader.GetInt32("ExportLDBCID");

            //oExportOutstandingDetail.AcceptanceDate = oReader.GetDateTime("AcceptanceDate");
            //oExportOutstandingDetail.DiscountedDate = oReader.GetDateTime("DiscountedDate");
            //oExportOutstandingDetail.BankFDDRecDate = oReader.GetDateTime("BankFDDRecDate");
            //oExportOutstandingDetail.RelizationDate = oReader.GetDateTime("RelizationDate");
            //oExportOutstandingDetail.EncashmentDate = oReader.GetDateTime("EncashmentDate");
            //oExportOutstandingDetail.LCTramsID = oReader.GetInt32("LCTramsID");
            //oExportOutstandingDetail.TrgNote = oReader.GetString("TrgNote");     

            //oExportOutstandingDetail.BankName_Advice = oReader.GetString("BankName_Advice");
            //oExportOutstandingDetail.BBranchName_Advice = oReader.GetString("BBranchName_Advice");


            return oExportOutstandingDetail;
        }

        private ExportOutstandingDetail CreateObject(NullHandler oReader)
        {
            ExportOutstandingDetail oExportOutstandingDetail = new ExportOutstandingDetail();
            oExportOutstandingDetail = MapObject(oReader);
            return oExportOutstandingDetail;
        }

        private List<ExportOutstandingDetail> CreateObjects(IDataReader oReader)
        {
            List<ExportOutstandingDetail> oExportOutstandingDetails = new List<ExportOutstandingDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportOutstandingDetail oItem = CreateObject(oHandler);
                oExportOutstandingDetails.Add(oItem);
            }
            return oExportOutstandingDetails;
        }
        #endregion

        #region Interface implementation
        public ExportOutstandingDetailService() { }


        public List<ExportOutstandingDetail> Gets(Int64 nUserID)
        {
            List<ExportOutstandingDetail> oExportOutstandingDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportOutstandingDetailDA.Gets(tc);
                oExportOutstandingDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportOutstandingDetails", e);
                #endregion
            }

            return oExportOutstandingDetails;
        }
        public List<ExportOutstandingDetail> Gets(int nReportType, int nBUID, DateTime dFromDate, DateTime dToDate, int nDiscountType, Int64 nUserID)
        {
            List<ExportOutstandingDetail> oExportOutstandingDetails = new List<ExportOutstandingDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportOutstandingDetailDA.GetsReport(tc, nReportType, nBUID, dFromDate, dToDate, nDiscountType);
                oExportOutstandingDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExportOutstandingDetail oExportOutstandingDetail = new ExportOutstandingDetail();
                oExportOutstandingDetail.ErrorMessage = e.Message.Split('!')[0];
                oExportOutstandingDetails = new List<ExportOutstandingDetail>();
                oExportOutstandingDetails.Add(oExportOutstandingDetail);
                #endregion
            }

            return oExportOutstandingDetails;
        }
        public List<ExportOutstandingDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportOutstandingDetail> oExportOutstandingDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportOutstandingDetailDA.Gets(tc, sSQL);
                oExportOutstandingDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportOutstandingDetails", e);
                #endregion
            }

            return oExportOutstandingDetails;
        }


        #endregion
    }
}
