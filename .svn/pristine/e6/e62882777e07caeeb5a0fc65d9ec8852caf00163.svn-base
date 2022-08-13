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
    
    public class ExportCommercialDocService : MarshalByRefObject, IExportCommercialDocService
    {
        #region Private functions and declaration
        private static ExportCommercialDoc MapObject(NullHandler oReader)
        {
            DateTime dDate = new DateTime();
            ExportCommercialDoc oExportCommercialDoc = new ExportCommercialDoc();            
            oExportCommercialDoc.ExportBillID = oReader.GetInt32("ExportBillID");
            oExportCommercialDoc.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportCommercialDoc.ApplicantID = oReader.GetInt32("ApplicantID");
            oExportCommercialDoc.ExportBillNo = oReader.GetString("ExportBillNo");
            oExportCommercialDoc.Amount_Bill = oReader.GetDouble("Amount_Bill");
            dDate = oReader.GetDateTime("ExportBillDate");
            oExportCommercialDoc.ExportBillDate = dDate.ToString("dd MMM yyyy");
            dDate = oReader.GetDateTime("DocDate");
            if (dDate != DateTime.MinValue)
            {
                oExportCommercialDoc.DocDate = dDate.ToString("dd MMM yyyy");
            }
            // Drive property 
            //Export LC
            oExportCommercialDoc.ExportLCNo = oReader.GetString("ExportLCNo");
            oExportCommercialDoc.Amount_LC = oReader.GetDouble("Amount_LC");
            dDate = oReader.GetDateTime("LCOpeningDate");
            oExportCommercialDoc.LCOpeningDate = dDate.ToString("dd MMM yyyy");
            dDate = oReader.GetDateTime("LCRecivedDate");
            oExportCommercialDoc.LCRecivedDate = dDate.ToString("dd MMM yyyy");
         
            oExportCommercialDoc.ApplicantName = oReader.GetString("ApplicantName");
      
            oExportCommercialDoc.OverDueRate = oReader.GetDouble("OverdueRate");
            oExportCommercialDoc.ApplicantAddress = oReader.GetString("ApplicantAddress");
            oExportCommercialDoc.Currency = oReader.GetString("Currency");
            oExportCommercialDoc.CurrencyName = oReader.GetString("CurrencyName");
            oExportCommercialDoc.BusinessUnitType = oReader.GetInt32("BusinessUnitType");
            oExportCommercialDoc.BUID = oReader.GetInt32("BUID");

            oExportCommercialDoc.IRC = oReader.GetString("IRC");
            oExportCommercialDoc.ERC = oReader.GetString("ERC");
            oExportCommercialDoc.GarmentsQty = oReader.GetString("GarmentsQty");
            oExportCommercialDoc.HSCode = oReader.GetString("HSCode");
            oExportCommercialDoc.AreaCode = oReader.GetString("AreaCode");
            oExportCommercialDoc.Remark = oReader.GetString("Remark");
            oExportCommercialDoc.SpecialNote = oReader.GetString("SpecialNote");
            oExportCommercialDoc.TIN = oReader.GetString("TIN");
            oExportCommercialDoc.Vat_ReqNo = oReader.GetString("Vat_ReqNo");
            oExportCommercialDoc.FrightPrepaid = oReader.GetString("FrightPrepaid");

            oExportCommercialDoc.LCTermsName = oReader.GetString("LCTermsName");
            oExportCommercialDoc.PaymentInstruction = (EnumPaymentInstruction)oReader.GetInt32("PaymentInstruction");

            oExportCommercialDoc.DriverName = oReader.GetString("DriverName");
            oExportCommercialDoc.TruckNo = oReader.GetString("TruckNo");

            oExportCommercialDoc.BUName = oReader.GetString("BUName");
            oExportCommercialDoc.BUAddress = oReader.GetString("BUAddress");


            oExportCommercialDoc.DeliveryToName = oReader.GetString("DeliveryToName");
            oExportCommercialDoc.DeliveryToAddress = oReader.GetString("DeliveryToAddress");
            oExportCommercialDoc.FactoryAddress = oReader.GetString("FactoryAddress");
            oExportCommercialDoc.CommercialInvoiceNo = oReader.GetString("CommercialInvoiceNo");

            oExportCommercialDoc.Term = oReader.GetString("Term");
            oExportCommercialDoc.DeliveryBy = oReader.GetString("DeliveryBy");
            oExportCommercialDoc.IsDeliveryBy = oReader.GetBoolean("IsDeliveryBy");
            oExportCommercialDoc.IsPrintOriginal = oReader.GetBoolean("IsPrintOriginal");
            oExportCommercialDoc.IsTerm = oReader.GetBoolean("IsTerm");
            oExportCommercialDoc.IsPrintGrossNetWeight = oReader.GetBoolean("IsPrintGrossNetWeight");
            oExportCommercialDoc.WeightPerBag = oReader.GetDouble("PerCartonWeight");
            oExportCommercialDoc.BagWeight = oReader.GetDouble("MeasurementCarton");
          
             /// Bank 
            oExportCommercialDoc.BankName_Nego = oReader.GetString("BankName_Nego");
            oExportCommercialDoc.BBranchName_Nego = oReader.GetString("BBranchName_Nego");
            oExportCommercialDoc.BankAddress_Nego = oReader.GetString("BankAddress_Nego");
            oExportCommercialDoc.BankName_Advice = oReader.GetString("BankName_Advice");
            oExportCommercialDoc.BBranchName_Advice = oReader.GetString("BBranchName_Advice");
            oExportCommercialDoc.BankAddress_Advice = oReader.GetString("BankAddress_Advice");

            oExportCommercialDoc.BankName_Issue = oReader.GetString("BankName_Issue");
            oExportCommercialDoc.BBranchName_Issue = oReader.GetString("BBranchName_Issue");
            oExportCommercialDoc.BankAddress_Issue = oReader.GetString("BankAddress_Issue");

            oExportCommercialDoc.BankName_Forwarding = oReader.GetString("BankName_Forwarding");
            oExportCommercialDoc.BBranchName_Forwarding = oReader.GetString("BBranchName_Forwarding");
            oExportCommercialDoc.BankAddress_Forwarding = oReader.GetString("BankAddress_Forwarding");

            oExportCommercialDoc.BankName_Endorse = oReader.GetString("BankName_Endorse");
            oExportCommercialDoc.BBranchName_Endorse = oReader.GetString("BBranchName_Endorse");
            oExportCommercialDoc.BankAddress_Endorse = oReader.GetString("BankAddress_Endorse");

            oExportCommercialDoc.MasterLCID = oReader.GetInt32("MasterLCID");
            oExportCommercialDoc.NotifyBy = (EnumNotifyBy)oReader.GetInt16("NotifyBy");
            oExportCommercialDoc.CTPApplicant = oReader.GetString("CTPApplicant");
            //oExportCommercialDoc.Certification_Entry = oReader.GetString("Certification");
            oExportCommercialDoc.GRPNoDate = oReader.GetString("GRPNoDate");
            oExportCommercialDoc.SendToBankDate = oReader.GetDateTime("SendToBank");
            oExportCommercialDoc.GrossWeightPTage = oReader.GetDouble("GrossWeightPTage");
            
                  
            return oExportCommercialDoc;            
        }

          public static  ExportCommercialDoc CreateObject(NullHandler oReader)
        {
            ExportCommercialDoc oExportCommercialDoc = new ExportCommercialDoc();
            oExportCommercialDoc=MapObject(oReader);
            return oExportCommercialDoc;
        }

        private List<ExportCommercialDoc> CreateObjects(IDataReader oReader)
        {
            List<ExportCommercialDoc> oExportCommercialDocs = new List<ExportCommercialDoc>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportCommercialDoc oItem = CreateObject(oHandler);
                oExportCommercialDocs.Add(oItem);
            }
            return oExportCommercialDocs;
        }
        #endregion

        #region Interface implementation
        public ExportCommercialDocService() { }



        public ExportCommercialDoc Get(int nExportBillID, Int64 nUserID)
        {
            ExportCommercialDoc oExportCommercialDoc = new ExportCommercialDoc();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportCommercialDocDA.Get(tc, nExportBillID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportCommercialDoc = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportCommercialDoc", e);
                #endregion
            }

            return oExportCommercialDoc;
        }


        public ExportCommercialDoc GetForBuying(int nCommercialInvoiceID, Int64 nUserID)
        {
            ExportCommercialDoc oExportCommercialDoc = new ExportCommercialDoc();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportCommercialDocDA.GetForBuying(tc, nCommercialInvoiceID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportCommercialDoc = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportCommercialDoc", e);
                #endregion
            }

            return oExportCommercialDoc;
        }
        #endregion
    }
}
