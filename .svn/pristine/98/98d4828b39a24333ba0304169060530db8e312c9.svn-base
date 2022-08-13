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
    public class FabricDeliveryChallanBillService : MarshalByRefObject, IFabricDeliveryChallanBillService
    {
        #region Private functions and declaration
        private FabricDeliveryChallanBill MapObject(NullHandler oReader)
        {
            FabricDeliveryChallanBill oFabricDeliveryChallanBill = new FabricDeliveryChallanBill();
            oFabricDeliveryChallanBill.FSCID = oReader.GetInt32("FSCID");
            oFabricDeliveryChallanBill.FSCDID = oReader.GetInt32("FSCDID");
            oFabricDeliveryChallanBill.SCNo = oReader.GetString("SCNo");
            oFabricDeliveryChallanBill.SCDate = oReader.GetDateTime("SCDate");
            oFabricDeliveryChallanBill.OrderType = oReader.GetInt32("OrderType");
            oFabricDeliveryChallanBill.BuyerID = oReader.GetInt32("BuyerID");
            oFabricDeliveryChallanBill.BuyerName = oReader.GetString("BuyerName");
            oFabricDeliveryChallanBill.OrderName = oReader.GetString("OrderName");
            oFabricDeliveryChallanBill.ExeNo = oReader.GetString("ExeNo");
            oFabricDeliveryChallanBill.Construction = oReader.GetString("Construction");
            oFabricDeliveryChallanBill.ProductID = oReader.GetInt32("ProductID");
            oFabricDeliveryChallanBill.ProductName = oReader.GetString("ProductName");
            oFabricDeliveryChallanBill.ContractorID = oReader.GetInt32("ContractorID");
            oFabricDeliveryChallanBill.ContractorName = oReader.GetString("ContractorName");
            oFabricDeliveryChallanBill.MKTPersonID = oReader.GetInt32("MKTPersonID");
            oFabricDeliveryChallanBill.MKTPersonName = oReader.GetString("MKTPersonName");
            oFabricDeliveryChallanBill.MUnitID = oReader.GetInt32("MUnitID");
            oFabricDeliveryChallanBill.MUnitName = oReader.GetString("MUnitName");
            oFabricDeliveryChallanBill.UnitPrice_DC = oReader.GetInt32("UnitPrice_DC");
            oFabricDeliveryChallanBill.CurrencyID = oReader.GetInt32("CurrencyID");
            oFabricDeliveryChallanBill.CurrencyName = oReader.GetString("CurrencyName");
            oFabricDeliveryChallanBill.QTY = oReader.GetDouble("QTY");
            oFabricDeliveryChallanBill.UnitPrice = oReader.GetDouble("UnitPrice");
            oFabricDeliveryChallanBill.IsDeduct = oReader.GetBoolean("IsDeduct");
            oFabricDeliveryChallanBill.Qty_DC = oReader.GetDouble("Qty_DC");
            oFabricDeliveryChallanBill.MUnit_DC = oReader.GetInt32("MUnit_DC");
            oFabricDeliveryChallanBill.MUnitName_DC = oReader.GetString("MUnitName_DC");
            oFabricDeliveryChallanBill.Currency_DC = oReader.GetInt32("Currency_DC");
            oFabricDeliveryChallanBill.CurrencyName_DC = oReader.GetString("CurrencyName_DC");
            oFabricDeliveryChallanBill.DiscountAmount = oReader.GetDouble("DiscountAmount");
            oFabricDeliveryChallanBill.AdditionalAmount = oReader.GetDouble("AdditionalAmount");
            oFabricDeliveryChallanBill.PaymentAmount = oReader.GetDouble("PaymentAmount");
            oFabricDeliveryChallanBill.FabricOrderType = oReader.GetInt32("FabricOrderType");

            return oFabricDeliveryChallanBill;
        }
        private FabricDeliveryChallanBill CreateObject(NullHandler oReader)
        {
            FabricDeliveryChallanBill oFabricDeliveryChallanBill = new FabricDeliveryChallanBill();
            oFabricDeliveryChallanBill = MapObject(oReader);
            return oFabricDeliveryChallanBill;
        }
        private List<FabricDeliveryChallanBill> CreateObjects(IDataReader oReader)
        {
            List<FabricDeliveryChallanBill> oFabricDeliveryChallanBill = new List<FabricDeliveryChallanBill>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricDeliveryChallanBill oItem = CreateObject(oHandler);
                oFabricDeliveryChallanBill.Add(oItem);
            }
            return oFabricDeliveryChallanBill;
        }

        #endregion

        #region Interface implementation
        public FabricDeliveryChallanBillService() { }
        public List<FabricDeliveryChallanBill> Gets(string sSQL,Int64 nUserID)
        {
            List<FabricDeliveryChallanBill> oFabricDeliveryChallanBills = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricDeliveryChallanBillDA.Gets(tc,sSQL);
                oFabricDeliveryChallanBills = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricDeliveryChallanBill", e);
                #endregion
            }
            return oFabricDeliveryChallanBills;
        }
        #endregion
    }   
}



