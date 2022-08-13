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

    public class SalesProfitService : MarshalByRefObject, ISalesProfitService
    {
        #region Private functions and declaration
        private SalesProfit MapObject(NullHandler oReader)
        {
            SalesProfit oSalesProfit = new SalesProfit();
            oSalesProfit.VoucherDetailID = oReader.GetInt32("VoucherDetailID");
            oSalesProfit.VoucherID = oReader.GetInt32("VoucherID");
            oSalesProfit.VoucherNo = oReader.GetString("VoucherNo");
            oSalesProfit.VoucherDate = oReader.GetDateTime("VoucherDate");
            oSalesProfit.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oSalesProfit.ComponentID = oReader.GetInt32("ComponentID");
            oSalesProfit.OrderID = oReader.GetInt32("OrderID");
            oSalesProfit.VOReferenceID = oReader.GetInt32("VOReferenceID");
            oSalesProfit.AccountHeadName = oReader.GetString("AccountHeadName");
            oSalesProfit.AccountHeadCode = oReader.GetString("AccountHeadCode");
            oSalesProfit.OrderNo = oReader.GetString("OrderNo");
            oSalesProfit.OrderDate = oReader.GetDateTime("OrderDate");
            oSalesProfit.Amount = oReader.GetDouble("Amount");
            return oSalesProfit;
        }

        private SalesProfit CreateObject(NullHandler oReader)
        {
            SalesProfit oSalesProfit = new SalesProfit();
            oSalesProfit = MapObject(oReader);
            return oSalesProfit;
        }

        private List<SalesProfit> CreateObjects(IDataReader oReader)
        {
            List<SalesProfit> oSalesProfit = new List<SalesProfit>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalesProfit oItem = CreateObject(oHandler);
                oSalesProfit.Add(oItem);
            }
            return oSalesProfit;
        }

        #endregion

        #region Interface implementation
        public SalesProfitService() { }
        public List<SalesProfit> Gets(int nOrderID, DateTime StartDate, DateTime EndDate, int nUserID)
        {
            List<SalesProfit> oSalesProfits = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalesProfitDA.Gets(tc, nOrderID, StartDate, EndDate);
                oSalesProfits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalesProfit", e);
                #endregion
            }

            return oSalesProfits;
        }


        #endregion
    } 
    
    
}
