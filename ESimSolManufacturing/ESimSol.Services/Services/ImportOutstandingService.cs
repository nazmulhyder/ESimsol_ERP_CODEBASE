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
    public class ImportOutstandingService : MarshalByRefObject, IImportOutstandingService
    {
        #region Private functions and declaration
        private ImportOutstanding MapObject(NullHandler oReader)
        {
            ImportOutstanding oImportOutstanding = new ImportOutstanding();

            oImportOutstanding.LCOpen = Math.Round(oReader.GetDouble("LCOpen"), 2);
            oImportOutstanding.ShipmenmentInTransit = Math.Round(oReader.GetDouble("ShipmenmentInTransit"), 2);
            oImportOutstanding.ShipmenDone = Math.Round(oReader.GetDouble("ShipmenDone"), 2);
            oImportOutstanding.DocInBank = Math.Round(oReader.GetDouble("DocInBank"), 2);
            oImportOutstanding.DocInHand = Math.Round(oReader.GetDouble("DocInOurHand"), 2);
            oImportOutstanding.DocInCnF = Math.Round(oReader.GetDouble("DocInCnF"), 2);
            oImportOutstanding.GoodsInTransit = Math.Round(oReader.GetDouble("GoodsInTransit"), 2);
            oImportOutstanding.Accpt_WithoutStockIn = Math.Round(oReader.GetDouble("Accpt_WithoutStockIn"), 2);
            oImportOutstanding.Accpt_WithStockIn = Math.Round(oReader.GetDouble("Accpt_WithStockIn"), 2);
            oImportOutstanding.ABP_WithoutStockIn = Math.Round(oReader.GetDouble("ABP_WithoutStockIn"), 2);
            oImportOutstanding.ABP_WithStockIn = Math.Round(oReader.GetDouble("ABP_WithStockIn"), 2);
            oImportOutstanding.BUName = oReader.GetString("BUName");
            oImportOutstanding.BankBranchID = oReader.GetInt32("BankBranchID");
            oImportOutstanding.BankShortName = oReader.GetString("BankShortName");
            oImportOutstanding.BankName = oReader.GetString("BankName");
            oImportOutstanding.CurrencyName = oReader.GetString("CurrencyName");
            oImportOutstanding.CurrencyID = oReader.GetInt32("CurrencyID");
            oImportOutstanding.BUID = oReader.GetInt32("BUID");
            oImportOutstanding.LCPaymentType = (EnumLCPaymentType)oReader.GetInt32("LCPaymentType");
            
            return oImportOutstanding;
        }
        private ImportOutstanding CreateObject(NullHandler oReader)
        {
            ImportOutstanding oImportOutstanding = new ImportOutstanding();
            oImportOutstanding = MapObject(oReader);
            return oImportOutstanding;
        }
        private List<ImportOutstanding> CreateObjects(IDataReader oReader)
        {
            List<ImportOutstanding> oImportOutstandings = new List<ImportOutstanding>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportOutstanding oItem = CreateObject(oHandler);
                oImportOutstandings.Add(oItem);
            }
            return oImportOutstandings;
        }
        #endregion

        #region Interface implementation
        public ImportOutstandingService() { }

        public List<ImportOutstanding> Gets(DateTime dFromDODate, DateTime dToDODate, int nBUID, int nCurrencyID,int nDate, Int64 nUserId)
        {
            List<ImportOutstanding> oImportOutstandings = new List<ImportOutstanding>(); ;
            ImportOutstanding oImportOutstanding= new ImportOutstanding();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ImportOutstandingDA.Gets(tc, dFromDODate, dToDODate, nBUID, nCurrencyID, nDate);
                oImportOutstandings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oImportOutstanding.ErrorMessage = ex.Message;
                oImportOutstandings = new List<ImportOutstanding>();
                oImportOutstandings.Add(oImportOutstanding);
                #endregion
            }
            return oImportOutstandings;
        }
        public List<ImportOutstanding> GetsImportOutstandingReport(string sSQL, Int64 nUserID)
        {
            List<ImportOutstanding> oImportOutstandings = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportOutstandingDA.GetsImportOutstandingReport(tc, sSQL);
                oImportOutstandings = CreateObjectsImportOutstandingReport(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportOutstanding", e);
                #endregion
            }

            return oImportOutstandings;
        }
        private ImportOutstanding MapObjectForImportOutstandingReport(NullHandler oReader)
        {
            ImportOutstanding oImportOutstanding = new ImportOutstanding();
            oImportOutstanding.Amount = oReader.GetDouble("Amount");
            oImportOutstanding.Year_Maturity = oReader.GetInt32("Year_Maturity");
            oImportOutstanding.Month_Maturity = oReader.GetInt32("Month_Maturity");
            oImportOutstanding.MonthName = oReader.GetString("MonthName");
            oImportOutstanding.BankBranchID = oReader.GetInt32("BankBranchID_Nego");
            oImportOutstanding.BankName = oReader.GetString("BankName_Nego");
            oImportOutstanding.LCPaymentType = ((EnumLCPaymentType)oReader.GetInt32("LCPaymentType"));
            return oImportOutstanding;
        }
        private ImportOutstanding CreateObjectImportOutstandingReport(NullHandler oReader)
        {
            ImportOutstanding oImportOutstanding = new ImportOutstanding();
            oImportOutstanding = MapObjectForImportOutstandingReport(oReader);
            return oImportOutstanding;
        }
        private List<ImportOutstanding> CreateObjectsImportOutstandingReport(IDataReader oReader)
        {
            List<ImportOutstanding> oImportOutstandings = new List<ImportOutstanding>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportOutstanding oItem = CreateObjectImportOutstandingReport(oHandler);
                oImportOutstandings.Add(oItem);
            }
            return oImportOutstandings;
        }

        #endregion
    }
}
