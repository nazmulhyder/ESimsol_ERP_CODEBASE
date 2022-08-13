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
   public class SalesStatementService :MarshalByRefObject ,ISalesStatementService
    {
        #region Private functions and declaration
        private static SalesStatement MapObject(NullHandler oReader)
        {
            SalesStatement oSalesStatement = new SalesStatement();
            oSalesStatement.BUID = oReader.GetInt32("BUID");
            oSalesStatement.BUName = oReader.GetString("BUName");
            oSalesStatement.CurrencyID = oReader.GetInt32("CurrencyID");
            oSalesStatement.Currency = oReader.GetString("Currency");
            oSalesStatement.Amount_SaleBudget = oReader.GetDouble("Amount_SaleBudget");
            oSalesStatement.Qty_Production = oReader.GetDouble("Qty_Production");
            oSalesStatement.Qty_Delivery_In = oReader.GetDouble("Qty_Delivery_In");
            oSalesStatement.Qty_Delivery_Out = oReader.GetDouble("Qty_Delivery_Out");
            oSalesStatement.Amount_Delivery = oReader.GetDouble("Amount_Delivery");
            oSalesStatement.Amount_Delivery_BC = oReader.GetDouble("Amount_Delivery_BC");
            oSalesStatement.Qty_PI = oReader.GetDouble("Qty_PI");
            oSalesStatement.Amount_PI = oReader.GetDouble("Amount_PI");
            oSalesStatement.Count_PI = oReader.GetDouble("Count_PI");
            oSalesStatement.Count_Cash = oReader.GetDouble("Count_Cash");
            oSalesStatement.Qty_Cash = oReader.GetDouble("Qty_Cash");
            oSalesStatement.Amount_Cash = oReader.GetDouble("Amount_Cash");
            oSalesStatement.Amount_Due = oReader.GetDouble("Amount_Due");
            oSalesStatement.Amount_ODue = oReader.GetDouble("Amount_ODue");
            oSalesStatement.Qty_LC = oReader.GetDouble("Qty_LC");
            oSalesStatement.Amount_LC = oReader.GetDouble("Amount_LC");
            oSalesStatement.Count_LC = oReader.GetDouble("Count_LC");

            oSalesStatement.Name = oReader.GetString("Name");
            oSalesStatement.Name_R = oReader.GetString("Name_R");
            oSalesStatement.Name_Y = oReader.GetString("Name_Y");
            oSalesStatement.Part = oReader.GetInt16("Part");
            oSalesStatement.Count = oReader.GetInt16("Count");
            oSalesStatement.Count_R = oReader.GetInt16("Count_R");
            oSalesStatement.Count_Y = oReader.GetInt16("Count_Y");
            oSalesStatement.Qty = oReader.GetDouble("Qty");
            oSalesStatement.Qty_R = oReader.GetDouble("Qty_R");
            oSalesStatement.Qty_Y = oReader.GetDouble("Qty_Y");

            return oSalesStatement;
        }

        public static SalesStatement CreateObject(NullHandler oReader)
        {
            SalesStatement oSalesStatement = MapObject(oReader);
            return oSalesStatement;
        }

        private List<SalesStatement> CreateObjects(IDataReader oReader)
        {
            List<SalesStatement> oSalesStatements = new List<SalesStatement>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalesStatement oItem = CreateObject(oHandler);
                oSalesStatements.Add(oItem);
            }
            return oSalesStatements;
        }

        #endregion
        #region Summary
        private static SalesStatement MapObject_Sum(NullHandler oReader)
        {
            SalesStatement oSalesStatement = new SalesStatement();
            oSalesStatement.CurrencyID = oReader.GetInt32("CurrencyID");
            oSalesStatement.Currency = oReader.GetString("Currency");
            oSalesStatement.BankBranchID = oReader.GetInt32("BankBranchID");
            oSalesStatement.BankName = oReader.GetString("BankName");
            oSalesStatement.CurrencyID = oReader.GetInt32("CurrencyID");
            oSalesStatement.Currency = oReader.GetString("Currency");
            oSalesStatement.BUID = oReader.GetInt32("BUID");
            oSalesStatement.BUName = oReader.GetString("BUName");
            oSalesStatement.Amount_LC = oReader.GetDouble("Amount_LC");
            oSalesStatement.BOinHand = oReader.GetDouble("BOinHand");
            oSalesStatement.BOInCusHand = oReader.GetDouble("BOInCusHand");
            oSalesStatement.AcceptadBill = oReader.GetDouble("AcceptadBill");
            oSalesStatement.NegoTransit = oReader.GetDouble("NegoTransit");
            oSalesStatement.NegotiatedBill = oReader.GetDouble("NegotiatedBill");
            oSalesStatement.Discounted = oReader.GetDouble("Discounted");
            oSalesStatement.PaymentDone = oReader.GetDouble("PaymentDone");
            oSalesStatement.BFDDRecd = oReader.GetDouble("BFDDRecd");
            oSalesStatement.Amount_Due = oReader.GetDouble("Amount_Due");
            oSalesStatement.Amount_ODue = oReader.GetDouble("Amount_ODue");
            return oSalesStatement;
        }
        public static SalesStatement CreateObject_Sum(NullHandler oReader)
        {
            SalesStatement oSalesStatement = new SalesStatement();
            oSalesStatement = MapObject_Sum(oReader);
            return oSalesStatement;
        }
        private List<SalesStatement> CreateObjects_Sum(IDataReader oReader)
        {
            List<SalesStatement> oSalesStatements = new List<SalesStatement>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalesStatement oItem = CreateObject_Sum(oHandler);
                oSalesStatements.Add(oItem);
            }
            return oSalesStatements;
        }
        #endregion

        #region Month Amount
        private static SalesStatement MapObject_Month(NullHandler oReader)
        {
            SalesStatement oSalesStatement = new SalesStatement();
            oSalesStatement.StartDate = oReader.GetDateTime("StartDate");
            oSalesStatement.Amount = oReader.GetDouble("Amount");
            oSalesStatement.Amount_Cash = oReader.GetDouble("Amount_Cash");
            oSalesStatement.CurrencyID = oReader.GetInt32("CurrencyID");
            oSalesStatement.Currency = oReader.GetString("Currency");
            oSalesStatement.BankName = oReader.GetString("BankName");
            oSalesStatement.Bank_Nego = oReader.GetString("Bank_Nego");
           
            return oSalesStatement;
        }
        public static SalesStatement CreateObject_Month(NullHandler oReader)
        {
            SalesStatement oSalesStatement = new SalesStatement();
            oSalesStatement = MapObject_Month(oReader);
            return oSalesStatement;
        }
        private List<SalesStatement> CreateObjects_Month(IDataReader oReader)
        {
            List<SalesStatement> oSalesStatements = new List<SalesStatement>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalesStatement oItem = CreateObject_Month(oHandler);
                oSalesStatements.Add(oItem);
            }
            return oSalesStatements;
        }
        #endregion
        #region Function
        public List<SalesStatement> GetsSalesStatement(int nBUID, DateTime StartDate ,DateTime Enddate, Int64 nUserID)
        {
            List<SalesStatement> oSalesStatements = new List<SalesStatement>();
            SalesStatement oSalesStatement = new SalesStatement();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalesStatementDA.GetsSalesStatement(tc, nBUID, StartDate, Enddate);
                oSalesStatements = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oSalesStatement.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oSalesStatements.Add(oSalesStatement);
                #endregion
            }

            return oSalesStatements;
        }
        public List<SalesStatement> Gets(string sSQL, Int64 nUserID)
        {
            List<SalesStatement> oSalesStatements = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalesStatementDA.Gets(tc, sSQL);
                oSalesStatements = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalesStatements", e);
                #endregion
            }

            return oSalesStatements;
        }
        public List<SalesStatement> Gets_Summary(int nBUID, Int64 nUserID)
        {
            List<SalesStatement> oSalesStatements = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalesStatementDA.Gets_Summary(tc, nBUID);
                oSalesStatements = CreateObjects_Sum(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalesStatements", e);
                #endregion
            }

            return oSalesStatements;
        }
        public List<SalesStatement> Gets_BillRealize(int nBUID, DateTime dStartDate, DateTime dEndDate, Int64 nUserID)
        {
            List<SalesStatement> oSalesStatements = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalesStatementDA.Gets_BillRealize(tc, nBUID, dStartDate, dEndDate);
                oSalesStatements = CreateObjects_Month(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalesStatements", e);
                #endregion
            }

            return oSalesStatements;
        }
        public List<SalesStatement> Gets_BillMaturity(int nBUID, DateTime dStartDate, DateTime dEndDate, Int64 nUserID)
        {
            List<SalesStatement> oSalesStatements = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalesStatementDA.Gets_BillMaturity(tc, nBUID, dStartDate, dEndDate);
                oSalesStatements = CreateObjects_Month(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalesStatements", e);
                #endregion
            }

            return oSalesStatements;
        }
        #endregion
    }
}
