using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{
    public class LandingCostRegisterService : MarshalByRefObject, ILandingCostRegisterService
    {
        #region Private functions and declaration
        private LandingCostRegister MapObject(NullHandler oReader)
        {
            LandingCostRegister oLandingCostRegister = new LandingCostRegister();
            oLandingCostRegister.PurchaseInvoiceID = oReader.GetInt32("PurchaseInvoiceID");
            oLandingCostRegister.BUID = oReader.GetInt32("BUID");
            oLandingCostRegister.BillNo = oReader.GetString("BillNo");
            oLandingCostRegister.DateofBill = oReader.GetDateTime("DateofBill");
            oLandingCostRegister.DateofInvoice = oReader.GetDateTime("DateofInvoice");
            oLandingCostRegister.InvoiceType = (EnumPInvoiceType)oReader.GetInt32("InvoiceType"); ;
            oLandingCostRegister.RefType = (EnumInvoiceReferenceType)oReader.GetInt32("RefType");
            oLandingCostRegister.PaymentMethod = (EnumPaymentMethod)oReader.GetInt32("PaymentMethod");
            oLandingCostRegister.InvoicePaymentMode = (EnumInvoicePaymentMode)oReader.GetInt32("InvoicePaymentMode");
            oLandingCostRegister.InvoiceStatus = (EnumPInvoiceStatus)oReader.GetInt32("InvoiceStatus");
            oLandingCostRegister.ContractorID = oReader.GetInt32("ContractorID");
            oLandingCostRegister.CurrencyID = oReader.GetInt32("CurrencyID");
            oLandingCostRegister.ConvertionRate = oReader.GetDouble("ConvertionRate");
            oLandingCostRegister.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oLandingCostRegister.ApprovedDate = oReader.GetDateTime("ApprovedDate");
            oLandingCostRegister.BankAccountID = oReader.GetInt32("BankAccountID");
            oLandingCostRegister.PurchaseInvoiceDetailID = oReader.GetInt32("PurchaseInvoiceDetailID");
            oLandingCostRegister.ProductID = oReader.GetInt32("ProductID");
            oLandingCostRegister.LCID = oReader.GetInt32("LCID");
            oLandingCostRegister.InvoiceID = oReader.GetInt32("InvoiceID");
            oLandingCostRegister.CostHeadID = oReader.GetInt32("CostHeadID");
            oLandingCostRegister.Remarks = oReader.GetString("Remarks");
            oLandingCostRegister.LandingCostType = (EnumLandingCostType)oReader.GetInt32("LandingCostType");
            oLandingCostRegister.Amount = oReader.GetDouble("Amount");
            oLandingCostRegister.BUName = oReader.GetString("BUName");
            oLandingCostRegister.BUShortName = oReader.GetString("BUShortName");
            oLandingCostRegister.SupplierName = oReader.GetString("SupplierName");
            oLandingCostRegister.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oLandingCostRegister.ApprovedByName = oReader.GetString("ApprovedByName");
            oLandingCostRegister.BankAccountNo = oReader.GetString("BankAccountNo");
            oLandingCostRegister.ProductCode = oReader.GetString("ProductCode");
            oLandingCostRegister.ProductName = oReader.GetString("ProductName");
            oLandingCostRegister.ImportLCNo = oReader.GetString("ImportLCNo");
            oLandingCostRegister.ImportLCDate = oReader.GetDateTime("ImportLCDate");
            oLandingCostRegister.LCCurrencyID = oReader.GetInt32("LCCurrencyID");
            oLandingCostRegister.LCCurrencySymbol = oReader.GetString("LCCurrencySymbol");
            oLandingCostRegister.ImportLCAmount = oReader.GetDouble("ImportLCAmount");
            oLandingCostRegister.ImportInvoiceNo = oReader.GetString("ImportInvoiceNo");
            oLandingCostRegister.ImportInvoiceDate = oReader.GetDateTime("ImportInvoiceDate");
            oLandingCostRegister.ImportInvoiceAmount = oReader.GetDouble("ImportInvoiceAmount");
            oLandingCostRegister.CostHeadCode = oReader.GetString("CostHeadCode");
            oLandingCostRegister.CostHeadName = oReader.GetString("CostHeadName");
            return oLandingCostRegister;
        }

        private LandingCostRegister CreateObject(NullHandler oReader)
        {
            LandingCostRegister oLandingCostRegister = new LandingCostRegister();
            oLandingCostRegister = MapObject(oReader);
            return oLandingCostRegister;
        }

        private List<LandingCostRegister> CreateObjects(IDataReader oReader)
        {
            List<LandingCostRegister> oLandingCostRegister = new List<LandingCostRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LandingCostRegister oItem = CreateObject(oHandler);
                oLandingCostRegister.Add(oItem);
            }
            return oLandingCostRegister;
        }

        #endregion

        #region Interface implementation
        public LandingCostRegisterService() { }        
        public List<LandingCostRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<LandingCostRegister> oLandingCostRegister = new List<LandingCostRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LandingCostRegisterDA.Gets(tc, sSQL);
                oLandingCostRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LandingCostRegister", e);
                #endregion
            }

            return oLandingCostRegister;
        }
        #endregion
    }
}
