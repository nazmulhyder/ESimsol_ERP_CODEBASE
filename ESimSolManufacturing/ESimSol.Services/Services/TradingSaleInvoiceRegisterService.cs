using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class TradingSaleInvoiceRegisterService : MarshalByRefObject, ITradingSaleInvoiceRegisterService
    {
        private TradingSaleInvoiceRegister MapObject(NullHandler oReader)
        {
            TradingSaleInvoiceRegister oTradingSaleInvoiceRegister = new TradingSaleInvoiceRegister();
            oTradingSaleInvoiceRegister.TradingSaleInvoiceID = oReader.GetInt32("TradingSaleInvoiceID");
            oTradingSaleInvoiceRegister.TradingSaleInvoiceDetailID = oReader.GetInt32("TradingSaleInvoiceDetailID");
            oTradingSaleInvoiceRegister.InvoicePaymentID = oReader.GetInt32("InvoicePaymentID");
            oTradingSaleInvoiceRegister.InvoiceNo = oReader.GetString("InvoiceNo");
            oTradingSaleInvoiceRegister.InvoiceDate = oReader.GetDateTime("InvoiceDate");
            oTradingSaleInvoiceRegister.CustomerName = oReader.GetString("CustomerName");
            oTradingSaleInvoiceRegister.SalseType = (EnumSalesType)oReader.GetInt16("SalseType");
            oTradingSaleInvoiceRegister.ContactPersonName = oReader.GetString("ContactPersonName");
            oTradingSaleInvoiceRegister.BuyerID = oReader.GetInt32("BuyerID");
            oTradingSaleInvoiceRegister.ProductID = oReader.GetInt32("ProductID");
            oTradingSaleInvoiceRegister.BuyerName = oReader.GetString("BuyerName");
            oTradingSaleInvoiceRegister.ProductName = oReader.GetString("Productname");
            oTradingSaleInvoiceRegister.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oTradingSaleInvoiceRegister.Symbol = oReader.GetString("Symbol");
            oTradingSaleInvoiceRegister.InvoiceQty = oReader.GetDouble("InvoiceQty");
            oTradingSaleInvoiceRegister.UnitPrice = oReader.GetDouble("UnitPrice");
            oTradingSaleInvoiceRegister.Amount = oReader.GetDouble("Amount");
            oTradingSaleInvoiceRegister.ItemDiscount = oReader.GetDouble("ItemDiscount");
            oTradingSaleInvoiceRegister.ItemNetAmount = oReader.GetDouble("ItemNetAmount");
            oTradingSaleInvoiceRegister.GrossAmount = oReader.GetDouble("GrossAmount");
            oTradingSaleInvoiceRegister.DiscountAmount = oReader.GetDouble("DiscountAmount");
            oTradingSaleInvoiceRegister.VatAmount = oReader.GetDouble("VatAmount");
            oTradingSaleInvoiceRegister.ServiceCharge = oReader.GetDouble("ServiceCharge");
            oTradingSaleInvoiceRegister.NetAmount = oReader.GetDouble("NetAmount");
            oTradingSaleInvoiceRegister.ReceivedAmount = oReader.GetDouble("ReceivedAmount");
            oTradingSaleInvoiceRegister.TotalReceivedAmount = oReader.GetDouble("TotalReceivedAmount");
            oTradingSaleInvoiceRegister.DueAmount = oReader.GetDouble("DueAmount");
            oTradingSaleInvoiceRegister.PurchasePrice = oReader.GetDouble("PurchasePrice");
            oTradingSaleInvoiceRegister.ProfitAmount = oReader.GetDouble("ProfitAmount");
            oTradingSaleInvoiceRegister.ReceivedDate = oReader.GetDateTime("ReceivedDate");
            oTradingSaleInvoiceRegister.ReceivedNo = oReader.GetString("ReceivedNo");
            oTradingSaleInvoiceRegister.ProductCode = oReader.GetString("ProductCode");
            return oTradingSaleInvoiceRegister;
        }
        private TradingSaleInvoiceRegister CreateObject(NullHandler oReader)
        {
            TradingSaleInvoiceRegister oTradingSaleInvoiceRegister = new TradingSaleInvoiceRegister();
            oTradingSaleInvoiceRegister = MapObject(oReader);
            return oTradingSaleInvoiceRegister;
        }
        private List<TradingSaleInvoiceRegister> CreateObjects(IDataReader oReader)
        {
            List<TradingSaleInvoiceRegister> oTradingSaleInvoiceRegisters = new List<TradingSaleInvoiceRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TradingSaleInvoiceRegister oItem = CreateObject(oHandler);
                oTradingSaleInvoiceRegisters.Add(oItem);
            }
            return oTradingSaleInvoiceRegisters;
        }
        #region Implement Interface
        public List<TradingSaleInvoiceRegister> Gets(string sSQL, int ReportLayout)
        {
            List<TradingSaleInvoiceRegister> oTradingSaleInvoiceRegisters = new List<TradingSaleInvoiceRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = TradingSaleInvoiceRegisterDA.Gets(tc, sSQL, ReportLayout);
                oTradingSaleInvoiceRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TradingSaleInvoiceRegister", e);
                #endregion
            }
            return oTradingSaleInvoiceRegisters;
        }
        #endregion
    }
}
