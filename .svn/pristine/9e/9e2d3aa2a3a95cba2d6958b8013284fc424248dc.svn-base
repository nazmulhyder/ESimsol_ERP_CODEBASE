using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{

    #region SalesProfit
    public class SalesProfit : BusinessObject
    {
        public SalesProfit()
        {
            VoucherDetailID = 0;
            VoucherID = 0;
            VoucherNo = "";
            VoucherDate = DateTime.Today;
            AccountHeadID = 0;
            ComponentID = 0;
            OrderID = 0;
            VOReferenceID = 0;
            AccountHeadName = "";
            AccountHeadCode = "";
            OrderNo = "";
            OrderDate = DateTime.Today;
            Amount = 0;
            IsForCurrentDate = false;
            ErrorMessage = "";
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
        }

        #region Properties     
        public int VoucherDetailID { get; set; }
        public int VoucherID { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public int AccountHeadID { get; set; }
        public int ComponentID { get; set; }
        public int OrderID { get; set; }
        public int VOReferenceID { get; set; }
        public string AccountHeadName { get; set; }
        public string AccountHeadCode { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public double Amount { get; set; }
        public bool IsForCurrentDate { get; set; }
        public string ErrorMessage { get; set; }        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }        
        #endregion

        #region Derived Property
        public string StartDateSt { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateSt { get { return this.EndDate.ToString("dd MMM yyyy"); } }        
        public string OrderDateSt { get { return this.OrderDate.ToString("dd MMM yyyy"); } }
        public string VoucherDateSt { get { return this.VoucherDate.ToString("dd MMM yyyy"); } }        
        public  List<SalesProfit> SalesProfits { get; set; }
        public List<SalesProfit> Incomes { get; set; }
        public List<SalesProfit> Expenses { get; set; }
        public string AmountSt { get { return Global.MillionFormat(this.Amount); } }        
        #endregion

        #region Functions
        public static List<SalesProfit> Gets(int nOrderID, DateTime StartDate, DateTime EndDate, int nUserID)
        {
            return SalesProfit.Service.Gets(nOrderID, StartDate, EndDate, nUserID);
        }
      
        #endregion

        #region ServiceFactory
        internal static ISalesProfitService Service
        {
            get { return (ISalesProfitService)Services.Factory.CreateService(typeof(ISalesProfitService)); }
        }
        #endregion
    }
    #endregion

    #region ISalesProfit interface
    public interface ISalesProfitService
    {
        List<SalesProfit> Gets(int nOrderID, DateTime StartDate, DateTime EndDate, int nUserID);
    }
    #endregion
    
   
}
