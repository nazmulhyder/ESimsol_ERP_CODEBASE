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
    public class PurchaseOrderRegisterService : MarshalByRefObject, IPurchaseOrderRegisterService
    {
        #region Private functions and declaration
        private PurchaseOrderRegister MapObject(NullHandler oReader)
        {
            PurchaseOrderRegister oPurchaseOrderRegister = new PurchaseOrderRegister();
            oPurchaseOrderRegister.POID = oReader.GetInt32("POID");
            oPurchaseOrderRegister.BUID = oReader.GetInt32("BUID");
            oPurchaseOrderRegister.PONo = oReader.GetString("PONO");
            oPurchaseOrderRegister.PODate = oReader.GetDateTime("PODate");
            oPurchaseOrderRegister.RefType = (EnumPOReferenceType)oReader.GetInt32("RefType");
            oPurchaseOrderRegister.RefTypeInt = oReader.GetInt32("RefType");
            oPurchaseOrderRegister.RefID = oReader.GetInt32("RefID");
            oPurchaseOrderRegister.Status = (EnumPOStatus)oReader.GetInt32("Status");
            oPurchaseOrderRegister.StatusInt = oReader.GetInt32("Status");
            oPurchaseOrderRegister.ContractorID = oReader.GetInt32("ContractorID");
            oPurchaseOrderRegister.ContractorName = oReader.GetString("ContractorName");
            oPurchaseOrderRegister.ContractorShortName = oReader.GetString("ContractorShortName");
            oPurchaseOrderRegister.ApprovedByName = oReader.GetString("ApprovedByName");
            oPurchaseOrderRegister.PrepareByName = oReader.GetString("PrepareByName");
            oPurchaseOrderRegister.ConcernPersonName = oReader.GetString("ConcernPersonName");
            oPurchaseOrderRegister.ContactPersonName = oReader.GetString("ContactPersonName");
            oPurchaseOrderRegister.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oPurchaseOrderRegister.BUCode = oReader.GetString("BUCode");
            oPurchaseOrderRegister.BUName = oReader.GetString("BUName");
            oPurchaseOrderRegister.RefNo = oReader.GetString("RefNo");
            oPurchaseOrderRegister.RefDate = oReader.GetDateTime("RefDate");
            oPurchaseOrderRegister.RefBy = oReader.GetString("RefBy");
            oPurchaseOrderRegister.Amount = oReader.GetDouble("Amount");
            oPurchaseOrderRegister.YetToGRNQty = oReader.GetDouble("YetToGRNQty");
            oPurchaseOrderRegister.YetToInvocieQty = oReader.GetDouble("YetToInvocieQty");
            oPurchaseOrderRegister.PaymentTermID = oReader.GetInt32("PaymentTermID");
            oPurchaseOrderRegister.ShipBy = oReader.GetString("ShipBy");
            oPurchaseOrderRegister.TradeTerm = oReader.GetString("TradeTerm");
            oPurchaseOrderRegister.DeliveryToName = oReader.GetString("DeliveryToName");
            oPurchaseOrderRegister.DeliveryToContactPersonName = oReader.GetString("DeliveryToContactPersonName");
            oPurchaseOrderRegister.BillToName = oReader.GetString("BillToName");
            oPurchaseOrderRegister.BIllToContactPersonName = oReader.GetString("BIllToContactPersonName");
            oPurchaseOrderRegister.PaymentTermText = oReader.GetString("PaymentTermText");
            oPurchaseOrderRegister.CRate = oReader.GetDouble("CRate");
            oPurchaseOrderRegister.LotBalance = oReader.GetDouble("LotBalance");
            oPurchaseOrderRegister.YetToPurchaseReturnQty = oReader.GetDouble("YetToPurchaseReturnQty");
            oPurchaseOrderRegister.DiscountInAmount = oReader.GetDouble("DiscountInAmount");
            oPurchaseOrderRegister.DiscountInPercent = oReader.GetDouble("DiscountInPercent");

            oPurchaseOrderRegister.ProductID = oReader.GetInt32("ProductID");
            oPurchaseOrderRegister.Qty = oReader.GetDouble("Qty");
            oPurchaseOrderRegister.UnitPrice = oReader.GetDouble("UnitPrice");
            oPurchaseOrderRegister.ProductCode = oReader.GetString("ProductCode");
            oPurchaseOrderRegister.ProductName = oReader.GetString("ProductName");
            oPurchaseOrderRegister.UnitSymbol = oReader.GetString("UnitSymbol");
            oPurchaseOrderRegister.UnitName = oReader.GetString("UnitName");
            oPurchaseOrderRegister.Qty_Invoice = oReader.GetDouble("Qty_Invoice");
            oPurchaseOrderRegister.GRNValue = oReader.GetDouble("GRNValue");
            oPurchaseOrderRegister.YetToInvoiceQty = oReader.GetDouble("YetToInvoiceQty");
            oPurchaseOrderRegister.AdvInvoice = oReader.GetDouble("AdvInvoice");
            oPurchaseOrderRegister.AdvanceSettle = oReader.GetDouble("AdvanceSettle");
            oPurchaseOrderRegister.InvoiceValue = oReader.GetDouble("InvoiceValue");
            oPurchaseOrderRegister.BuyerName = oReader.GetString("BuyerName");
            oPurchaseOrderRegister.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oPurchaseOrderRegister.StyleNo = oReader.GetString("StyleNo");
            oPurchaseOrderRegister.LotNo = oReader.GetString("LotNo");
            oPurchaseOrderRegister.LotID = oReader.GetInt32("LotID");
            oPurchaseOrderRegister.ModelShortName = oReader.GetString("ModelShortName");
            oPurchaseOrderRegister.ApproveDate = oReader.GetDateTime("ApproveDate");
            return oPurchaseOrderRegister;
        }

        private PurchaseOrderRegister CreateObject(NullHandler oReader)
        {
            PurchaseOrderRegister oPurchaseOrderRegister = new PurchaseOrderRegister();
            oPurchaseOrderRegister = MapObject(oReader);
            return oPurchaseOrderRegister;
        }

        private List<PurchaseOrderRegister> CreateObjects(IDataReader oReader)
        {
            List<PurchaseOrderRegister> oPurchaseOrderRegister = new List<PurchaseOrderRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PurchaseOrderRegister oItem = CreateObject(oHandler);
                oPurchaseOrderRegister.Add(oItem);
            }
            return oPurchaseOrderRegister;
        }

        #endregion

        #region Interface implementation
        public PurchaseOrderRegisterService() { }        
        public List<PurchaseOrderRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<PurchaseOrderRegister> oPurchaseOrderRegister = new List<PurchaseOrderRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PurchaseOrderRegisterDA.Gets(tc, sSQL);
                oPurchaseOrderRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseOrderRegister", e);
                #endregion
            }

            return oPurchaseOrderRegister;
        }
        #endregion
    }
}
