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
    public class CommercialInvoiceRegisterService : MarshalByRefObject, ICommercialInvoiceRegisterService
    {
        #region Private functions and declaration
        private static CommercialInvoiceRegister MapObject(NullHandler oReader)
        {
            CommercialInvoiceRegister oCommercialInvoiceRegister = new CommercialInvoiceRegister();
            oCommercialInvoiceRegister.CommercialInvoiceDetailID = oReader.GetInt32("CommercialInvoiceDetailID");
            oCommercialInvoiceRegister.CommercialInvoiceID = oReader.GetInt32("CommercialInvoiceID");
            oCommercialInvoiceRegister.ReferenceDetailID = oReader.GetInt32("ReferenceDetailID");
            oCommercialInvoiceRegister.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oCommercialInvoiceRegister.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oCommercialInvoiceRegister.ShipmentDate = oReader.GetDateTime("ShipmentDate");
		    oCommercialInvoiceRegister.InvoiceQty = oReader.GetDouble("InvoiceQty");
		    oCommercialInvoiceRegister.FOB = oReader.GetDouble("FOB");
            oCommercialInvoiceRegister.Discount = oReader.GetDouble("Discount");
            oCommercialInvoiceRegister.UnitPrice = oReader.GetDouble("UnitPrice");
            oCommercialInvoiceRegister.Amount = oReader.GetDouble("Amount");
            oCommercialInvoiceRegister.InvoiceNo = oReader.GetString("InvoiceNo");
            oCommercialInvoiceRegister.MasterLCID = oReader.GetInt32("MasterLCID");
            oCommercialInvoiceRegister.InvoiceDate = oReader.GetDateTime("InvoiceDate");
            oCommercialInvoiceRegister.InvoiceStatus = (EnumCommercialInvoiceStatus)oReader.GetInt32("InvoiceStatus");
            oCommercialInvoiceRegister.InvoiceStatusInInt = oReader.GetInt32("InvoiceStatus");
            oCommercialInvoiceRegister.BuyerID = oReader.GetInt32("BuyerID");
            oCommercialInvoiceRegister.InvoiceAmount = oReader.GetDouble("InvoiceAmount");
            oCommercialInvoiceRegister.DiscountAmount = oReader.GetDouble("DiscountAmount");
            oCommercialInvoiceRegister.AdditionAmount = oReader.GetDouble("AdditionAmount");
            oCommercialInvoiceRegister.DiscrepancyCharge = oReader.GetDouble("DiscrepancyCharge");
            oCommercialInvoiceRegister.CartonQty = oReader.GetDouble("CartonQty");
            oCommercialInvoiceRegister.NetInvoiceAmount = oReader.GetDouble("NetInvoiceAmount");
            oCommercialInvoiceRegister.MasterLCNo = oReader.GetString("MasterLCNo");
            oCommercialInvoiceRegister.BuyerName = oReader.GetString("BuyerName");
            oCommercialInvoiceRegister.GSP = oReader.GetBoolean("GSP");
            oCommercialInvoiceRegister.IC = oReader.GetBoolean("IC");
            oCommercialInvoiceRegister.BL = oReader.GetBoolean("BL");
            oCommercialInvoiceRegister.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oCommercialInvoiceRegister.StyleNo = oReader.GetString("StyleNo");
            oCommercialInvoiceRegister.MUnit = oReader.GetString("MUnit");
            oCommercialInvoiceRegister.BUName = oReader.GetString("BUName");
            oCommercialInvoiceRegister.ApprovedByName = oReader.GetString("ApprovedByName");
            oCommercialInvoiceRegister.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oCommercialInvoiceRegister.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oCommercialInvoiceRegister.ShipmentMode = (EnumTransportType) oReader.GetInt32("ShipmentMode");
            oCommercialInvoiceRegister.BUID = oReader.GetInt32("BUID");
            oCommercialInvoiceRegister.Qty = oReader.GetInt32("Qty");

            return oCommercialInvoiceRegister;
        }

        public static CommercialInvoiceRegister CreateObject(NullHandler oReader)
        {
            CommercialInvoiceRegister oCommercialInvoiceRegister = new CommercialInvoiceRegister();
            oCommercialInvoiceRegister = MapObject(oReader);
            return oCommercialInvoiceRegister;
        }

        private List<CommercialInvoiceRegister> CreateObjects(IDataReader oReader)
        {
            List<CommercialInvoiceRegister> oCommercialInvoiceRegisters = new List<CommercialInvoiceRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CommercialInvoiceRegister oItem = CreateObject(oHandler);
                oCommercialInvoiceRegisters.Add(oItem);
            }
            return oCommercialInvoiceRegisters;
        }

        #endregion

        #region Interface implementation
        public CommercialInvoiceRegisterService() { }
        public List<CommercialInvoiceRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<CommercialInvoiceRegister> oCommercialInvoiceRegisters = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = CommercialInvoiceRegisterDA.Gets(tc, sSQL);
                oCommercialInvoiceRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CommercialInvoice", e);
                #endregion
            }
            return oCommercialInvoiceRegisters;
        }
        public CommercialInvoiceRegister Get(int CommercialInvoiceDetailID, Int64 nUserId)
        {
            CommercialInvoiceRegister oCommercialInvoiceRegister = new CommercialInvoiceRegister();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CommercialInvoiceRegisterDA.Get(tc, CommercialInvoiceDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCommercialInvoiceRegister = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get CommercialInvoiceRegister", e);
                #endregion
            }

            return oCommercialInvoiceRegister;
        }

        #endregion
    }
}
