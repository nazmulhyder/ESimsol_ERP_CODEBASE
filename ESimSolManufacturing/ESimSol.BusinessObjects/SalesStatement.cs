using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    public class SalesStatement :BusinessObject
    {
        public SalesStatement()
        {
            BUName = string.Empty;
            Amount_SaleBudget = 0;
            Qty_Production = 0;
            Qty_Delivery_In = 0;
            Qty_Delivery_Out = 0;
            Amount_Delivery = 0;
            Amount_Delivery_BC = 0;
            CurrencyID = 0;
            Currency = "";
            Qty_PI = 0;
            Amount_PI = 0;
            Count_PI = 0;
            Amount_PI = 0;
            BUID = 0;
            Qty_Cash = 0;
            Amount_Cash = 0;
            Qty_LC = 0;
            Amount_LC = 0;
            Count_LC = 0;
            Params = string.Empty;
            Bank_Nego = string.Empty;
            ErrorMessage = string.Empty;
            nReportType = 0;
            dateType = 0;
        }
        #region properties
        public int BUID { get; set; }
        public int nReportType { get; set; }
        public int dateType { get; set; }
        public String BUName { get; set; }
        public String Bank_Nego { get; set; }
        public double Amount_SaleBudget {get; set;}
        public double Qty_Production {get; set;}
        public double Qty_Delivery_In {get; set;}
        public double Qty_Delivery_Out {get; set;}
        public double Amount_Delivery {get; set;}
        public double Amount_Delivery_BC {get; set;}
        public double Qty_PI {get; set;}
        public double Amount_PI {get; set;}
        public double Count_PI {get; set;}
        public double Count_Cash { get; set; }
        public double Qty_Cash {get; set;}
        public double Amount_Cash {get; set;}
        public double Qty_LC {get; set;}
        public double Amount_LC {get; set;}
        public double Count_LC { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Currency { get; set; }
        public int CurrencyID { get; set; } // as per LC Currency
        public int BankBranchID { get; set; }
        public string BankName { get; set; }
        public double BOinHand { get; set; }
        public double BOInCusHand { get; set; }
        public double AcceptadBill { get; set; }
        public double NegoTransit { get; set; }
        public double NegotiatedBill { get; set; }
        public double Discounted { get; set; }
        public double PaymentDone { get; set; }
        public double BFDDRecd { get; set; }
        public double Amount_Due { get; set; }
        public double Amount_ODue { get; set; }
        public double Amount { get; set; }
        public string Params { get; set; }
        public string ErrorMessage { get; set; }

        #region  Properties For CHTML View
        public string Name { get; set; }
        public string Name_R{ get; set; }
        public string Name_Y { get; set; }

        public int Part { get; set; }
        public int Count { get; set; }
        public int Count_R { get; set; }
        public int Count_Y { get; set; }

        public double Qty { get; set; }
        public double Qty_R { get; set; }
        public double Qty_Y { get; set; }
        public double Amount_R { get; set; }
        public double Amount_Y { get; set; }
        #endregion

        #endregion

        #region Derived Properties
        public double Amount_LCtoBill { get { return this.Amount_LC + this.BOinHand + this.BOInCusHand + this.AcceptadBill; } }
        public string AmountWithCur
        {
            get
            {
                return (this.Currency + " " + this.Amount.ToString("#,##0.00;(#,##0.00)"));
            }
        }
        public string AmountCashWithCur
        {
            get
            {
                return (this.Currency + " " + String.Format("{0:0.00}", this.Amount_Cash));
            }
        }
        public string StartDateStr { get { return (this.StartDate == DateTime.MinValue) ? "" : this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateStr { get { return (this.EndDate == DateTime.MinValue) ? "" : this.EndDate.ToString("dd MMM yyyy"); } }
        public string SDMStr { get; set; }
        public string StartDateMonthStr { get { return (this.StartDate == DateTime.MinValue) ? "" : this.StartDate.ToString("MMMM yy"); } }
        #endregion
        #region Functions
        public static List<SalesStatement> GetsSalesStatement(int nBUID, DateTime StartDate, DateTime EndDate, long nUserID)
        {
            return SalesStatement.Service.GetsSalesStatement(nBUID, StartDate, EndDate, nUserID);
        }
        public static List<SalesStatement> Gets_Summary(int nBUID, Int64 nUserID)
        {
            return SalesStatement.Service.Gets_Summary(nBUID, nUserID);
        }
        public static List<SalesStatement> Gets_BillRealize(int nBUID, DateTime dStartDate, DateTime dEndDate, Int64 nUserID)
        {
            return SalesStatement.Service.Gets_BillRealize(nBUID, dStartDate, dEndDate, nUserID);
        }
        public static List<SalesStatement> Gets_BillMaturity(int nBUID, DateTime dStartDate, DateTime dEndDate, Int64 nUserID)
        {
            return SalesStatement.Service.Gets_BillMaturity(nBUID, dStartDate, dEndDate, nUserID);
        }
        public static List<SalesStatement> Gets(string sSQL, Int64 nUserID)
        {
            return SalesStatement.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ISalesStatementService Service
        {
            get { return (ISalesStatementService)Services.Factory.CreateService(typeof(ISalesStatementService)); }
        }

        #endregion
    }
    #region  TipsType interface
    public interface ISalesStatementService
    {
        List<SalesStatement> GetsSalesStatement(int nBUID, DateTime StartDate, DateTime EndDate, long nUserID);
        List<SalesStatement> Gets_Summary(int nBUID, Int64 nUserID);
        List<SalesStatement> Gets(string sSQL, Int64 nUserID);
        List<SalesStatement> Gets_BillRealize(int nBUID, DateTime dStartDate, DateTime dEndDate, Int64 nUserID);
        List<SalesStatement> Gets_BillMaturity(int nBUID, DateTime dStartDate, DateTime dEndDate, Int64 nUserID);
    }
    #endregion
}
