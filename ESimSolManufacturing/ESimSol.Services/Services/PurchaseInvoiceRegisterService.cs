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
    public class PurchaseInvoiceRegisterService : MarshalByRefObject, IPurchaseInvoiceRegisterService
    {
        #region Private functions and declaration
        private PurchaseInvoiceRegister MapObject(NullHandler oReader)
        {
            PurchaseInvoiceRegister oPurchaseInvoiceRegister = new PurchaseInvoiceRegister();
            oPurchaseInvoiceRegister.PurchaseInvoiceDetailID = oReader.GetInt32("PurchaseInvoiceDetailID");
            oPurchaseInvoiceRegister.PurchaseInvoiceID = oReader.GetInt32("PurchaseInvoiceID");
            oPurchaseInvoiceRegister.ProductID = oReader.GetInt32("ProductID");
            oPurchaseInvoiceRegister.MUnitID = oReader.GetInt32("MUnitID");
            oPurchaseInvoiceRegister.UnitPrice = oReader.GetDouble("UnitPrice");
            oPurchaseInvoiceRegister.Qty = oReader.GetDouble("Qty");
            oPurchaseInvoiceRegister.Amount = oReader.GetDouble("Amount");
            oPurchaseInvoiceRegister.PurchaseInvoiceNo = oReader.GetString("PurchaseInvoiceNo");
            oPurchaseInvoiceRegister.BUID = oReader.GetInt32("BUID");
            oPurchaseInvoiceRegister.BillNo = oReader.GetString("BillNo");
            oPurchaseInvoiceRegister.ApprovedDate = oReader.GetDateTime("ApprovedDate");
            oPurchaseInvoiceRegister.InvoiceStatus = (EnumPInvoiceStatus)oReader.GetInt32("InvoiceStatus");
            oPurchaseInvoiceRegister.ContractorID = oReader.GetInt32("ContractorID");
            oPurchaseInvoiceRegister.ContractorPersonalID = oReader.GetInt32("ContractorPersonalID");
            oPurchaseInvoiceRegister.CurrencyID = oReader.GetInt32("CurrencyID");
            oPurchaseInvoiceRegister.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oPurchaseInvoiceRegister.DateofInvoice = oReader.GetDateTime("DateofInvoice");
            oPurchaseInvoiceRegister.DateofMaturity = oReader.GetDateTime("DateofMaturity");
            oPurchaseInvoiceRegister.ShipmentBy = (EnumShipmentBy)oReader.GetInt32("ShipmentBy");
            oPurchaseInvoiceRegister.Remarks = oReader.GetString("Remarks");
            oPurchaseInvoiceRegister.ApprovedByName = oReader.GetString("ApprovedByName");
            oPurchaseInvoiceRegister.ProductCode = oReader.GetString("ProductCode");
            oPurchaseInvoiceRegister.ProductName = oReader.GetString("ProductName");
            oPurchaseInvoiceRegister.MUName = oReader.GetString("MUName");
            oPurchaseInvoiceRegister.MUSymbol = oReader.GetString("MUSymbol");
            oPurchaseInvoiceRegister.SupplierName = oReader.GetString("SupplierName");
            oPurchaseInvoiceRegister.SCPName = oReader.GetString("SCPName");
            oPurchaseInvoiceRegister.CurrencyName = oReader.GetString("CurrencyName");
            oPurchaseInvoiceRegister.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oPurchaseInvoiceRegister.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oPurchaseInvoiceRegister.GRNID = oReader.GetInt32("GRNID");
            oPurchaseInvoiceRegister.InvoiceType = (EnumPInvoiceType)oReader.GetInt32("InvoiceType");
            oPurchaseInvoiceRegister.InvoicePaymentMode = (EnumInvoicePaymentMode)oReader.GetInt32("InvoicePaymentMode");
            oPurchaseInvoiceRegister.BuyerName = oReader.GetString("BuyerName");
            oPurchaseInvoiceRegister.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oPurchaseInvoiceRegister.StyleNo = oReader.GetString("StyleNo");
            oPurchaseInvoiceRegister.BillToName = oReader.GetString("BillToName");
            oPurchaseInvoiceRegister.DeliveryToName = oReader.GetString("DeliveryToName");
            return oPurchaseInvoiceRegister;
        }

        private PurchaseInvoiceRegister CreateObject(NullHandler oReader)
        {
            PurchaseInvoiceRegister oPurchaseInvoiceRegister = new PurchaseInvoiceRegister();
            oPurchaseInvoiceRegister = MapObject(oReader);
            return oPurchaseInvoiceRegister;
        }

        private List<PurchaseInvoiceRegister> CreateObjects(IDataReader oReader)
        {
            List<PurchaseInvoiceRegister> oPurchaseInvoiceRegister = new List<PurchaseInvoiceRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PurchaseInvoiceRegister oItem = CreateObject(oHandler);
                oPurchaseInvoiceRegister.Add(oItem);
            }
            return oPurchaseInvoiceRegister;
        }

        #endregion

        #region Interface implementation
        public PurchaseInvoiceRegisterService() { }        
        public List<PurchaseInvoiceRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<PurchaseInvoiceRegister> oPurchaseInvoiceRegister = new List<PurchaseInvoiceRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PurchaseInvoiceRegisterDA.Gets(tc, sSQL);
                oPurchaseInvoiceRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseInvoiceRegister", e);
                #endregion
            }

            return oPurchaseInvoiceRegister;
        }
        #endregion
    }
}
