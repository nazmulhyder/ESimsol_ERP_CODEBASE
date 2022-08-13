using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{

    #region ReportStockSummery
    [DataContract]
    public class Rpt_DailyStockSummery : BusinessObject
    {
        public Rpt_DailyStockSummery()
        {
            RSSID = 0;
            ProductID = 0;
            LotID = 0;
            MeasurementUnitID = 0;
            TransactionDate = DateTime.Today;
            OpeningBalance = 0;
            ReceiveQty = 0;
            ReturnQty = 0;
            TransferIn = 0;
            TransferOut = 0;
            ConsumedQty = 0;
            ClosingBalance = 0;
            UnitPrice = 0;
            LocationID = 0;
            WorkingUnitID = 0;
            PTMID = 0;
            ProductCategoryID = 0;
            ProductBaseID = 0;
            ProductCategoryName = "";
            ProductBaseName = "";
            ProductName = "";
            MUName = "";
            ErrorMessage = "";
            
            WorkingUnits = new List<WorkingUnit>();
        }

        #region Properties
        
        public int RSSID { get; set; }
        
        public int ProductID { get; set; }
        
        public int LotID { get; set; }
        
        public int MeasurementUnitID { get; set; }
        
        public DateTime TransactionDate { get; set; }
        
        public double OpeningBalance { get; set; }
        
        public double ReceiveQty { get; set; }
        
        public double ReturnQty { get; set; }
        
        public double TransferIn { get; set; }
        
        public double TransferOut { get; set; }
        
        public double ConsumedQty { get; set; }
        
        public double ClosingBalance { get; set; }
        
        public double UnitPrice { get; set; }
        
        public int LocationID { get; set; }
        
        public int WorkingUnitID { get; set; }
        
        public int PTMID { get; set; }
        
        public int ProductCategoryID { get; set; }
        
        public int ProductBaseID { get; set; }
        
        public string ProductCategoryName { get; set; }
        
        public string ProductBaseName { get; set; }
        
        public string ProductName { get; set; }
        
        public string MUName { get; set; }
        
        public string ErrorMessage { get; set; }

        #endregion
        #region Derive Property
        
        
        public List<WorkingUnit> WorkingUnits { get; set; }
        public List<Rpt_DailyStockSummery> StockSummaryforPrint { get; set; }
        public List<Company> Companys { get; set; }

        public string OpeningValue { get { return Global.MillionFormat(this.OpeningBalance * this.UnitPrice); } }
        public string ReceiveValue { get { return Global.MillionFormat(this.ReceiveQty * this.UnitPrice); } }
        public string ReturnValue { get { return Global.MillionFormat(this.ReturnQty * this.UnitPrice); } }
        public string ConsumedValue { get { return Global.MillionFormat(this.ConsumedQty * this.UnitPrice); } }
        public string ClosingValue { get { return Global.MillionFormat(this.ClosingBalance * this.UnitPrice); } }
        
        #endregion

        #region Functions

        public static List<Rpt_DailyStockSummery> Gets(string sSQL, long nUserID)
        {
            return Rpt_DailyStockSummery.Service.Gets(sSQL, nUserID);
        }
        #endregion
        #region Non DB Function
       
        
       
        #endregion

        #region ServiceFactory
            internal static IRpt_DailyStockSummeryService Service
        {
            get { return (IRpt_DailyStockSummeryService)Services.Factory.CreateService(typeof(IRpt_DailyStockSummeryService)); }
        }

        #endregion
    }
    #endregion

    #region IReportStockSummery interface
    
    public interface IRpt_DailyStockSummeryService
    {
        List<Rpt_DailyStockSummery> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
    
}
