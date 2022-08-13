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
    public class ImportInvoiceIBPService : MarshalByRefObject, IImportInvoiceIBPService
    {
        #region Private functions and declaration
        private ImportInvoiceIBP MapObject(NullHandler oReader)
        {
            ImportInvoiceIBP oPIIBP = new ImportInvoiceIBP();

            oPIIBP.ImportLCNo = oReader.GetString("ImportLCNo");
            oPIIBP.ImportInvoiceNo = oReader.GetString("ImportInvoiceNo");
            oPIIBP.ContractorName = oReader.GetString("ContractorName");
            oPIIBP.BankName = oReader.GetString("BankName_Nego");
            oPIIBP.BankNickName = oReader.GetString("BankName_Nego");
            oPIIBP.Amount_LC = oReader.GetDouble("Amount_LC");
            oPIIBP.Amount = oReader.GetDouble("Amount");
            oPIIBP.DateofMaturity = oReader.GetDateTime("DateofMaturity");
            oPIIBP.ContractorName = oReader.GetString("ContractorName");
            oPIIBP.Currency = oReader.GetString("Currency");
            //oImportInvoice.BLNo = oReader.GetString("BLNo");
            //oImportInvoice.BLDate = oReader.GetDateTime("BLDate");
            ////For Import Invoice Management
            //oImportInvoice.Origin = oReader.GetString("Origin");
            oPIIBP.ImportLCNo = oReader.GetString("ImportLCNo");
            oPIIBP.ImportLCDate = oReader.GetDateTime("ImportLCDate");
            oPIIBP.BUName = oReader.GetString("BUName");
            oPIIBP.ABPNo = oReader.GetString("ABPNo");
            oPIIBP.Tenor = oReader.GetInt32("Tenor");
            oPIIBP.DateofAcceptance = oReader.GetDateTime("DateofAcceptance");
            oPIIBP.CurrencyID = oReader.GetInt32("CurrencyID_LC");
            oPIIBP.BankBranchID = oReader.GetInt32("BankBranchID_Nego");
            oPIIBP.CurrentStatus = (EnumInvoiceEvent)oReader.GetInt32("CurrentStatus");
            oPIIBP.BankStatus = (EnumInvoiceBankStatus)oReader.GetInt32("BankStatus");
            oPIIBP.LCPaymentType = (EnumLCPaymentType)oReader.GetInt32("LCPaymentType");
            oPIIBP.CCRate = oReader.GetDouble("CCrate");
            oPIIBP.CurrencyName = oReader.GetString("CurrencyName");
            oPIIBP.Currency = oReader.GetString("Currency");
            oPIIBP.LCCurrentStatus = (EnumLCCurrentStatus)oReader.GetInt32("LCCurrentStatus");

            return oPIIBP;
        }

        private ImportInvoiceIBP CreateObject(NullHandler oReader)
        {
            ImportInvoiceIBP oPIIBP = new ImportInvoiceIBP();
            oPIIBP = MapObject(oReader);
            return oPIIBP;
        }

        private List<ImportInvoiceIBP> CreateObjects(IDataReader oReader)
        {
            List<ImportInvoiceIBP> oPIIBP = new List<ImportInvoiceIBP>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportInvoiceIBP oItem = CreateObject(oHandler);
                oPIIBP.Add(oItem);
            }
            return oPIIBP;
        }

        #endregion

        #region Interface implementation
        public ImportInvoiceIBPService() { }



        public List<ImportInvoiceIBP> Gets(int nBUID, Int64 nUserID)
        {
            List<ImportInvoiceIBP> oPIIBP = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportInvoiceIBPDA.Gets(tc,nBUID);
                oPIIBP = CreateObjects(reader);
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

            return oPIIBP;
        }

        public List<ImportInvoiceIBP> Gets(string sSQL, Int64 nUserID)
        {
            List<ImportInvoiceIBP> oPIIBP = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportInvoiceIBPDA.Gets(tc, sSQL);
                oPIIBP = CreateObjects(reader);
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

            return oPIIBP;
        }
      
        public List<ImportInvoiceIBP> GetsForGraph(string sYear, int nBankBranchID,int nBUID, Int64 nUserId)
        {
            List<ImportInvoiceIBP> oImportInvoiceIBPs = new List<ImportInvoiceIBP>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportInvoiceIBPDA.GetsForGraph(tc, sYear, nBankBranchID, nBUID);
                oImportInvoiceIBPs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                //#region Handle Exception
                //if (tc != null)
                //    tc.HandleError();
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get View_PurchaseLCReport", e);
                //#endregion
                oImportInvoiceIBPs = new List<ImportInvoiceIBP>();
                ImportInvoiceIBP oImportInvoiceIBP = new ImportInvoiceIBP();
                oImportInvoiceIBP.ErrorMessage = e.Message;
                oImportInvoiceIBPs.Add(oImportInvoiceIBP);
            }
            return oImportInvoiceIBPs;
        }

        #endregion
    }
}