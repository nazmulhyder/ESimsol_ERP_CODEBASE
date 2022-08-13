using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects.ReportingObject
{
    #region RptFEOSalesStatement

    public class RptFEOSalesStatement : BusinessObject
    {
        #region  Constructor
        public RptFEOSalesStatement()
        {
            OrderDate = DateTime.MinValue;
            InHouseExeQty = 0;
            OutsideExeQty = 0;
            InHouseSalesQty = 0; 
            OutsideSalesQty = 0;  
            InHouseSalesValue = 0;
            OutsideSalesValue = 0; 
            TotalProduction = 0;
            ErrorMessage = string.Empty;
            Params = "";
        }
        #endregion

        #region Properties
        public DateTime OrderDate { get; set; }
        public double InHouseExeQty { get; set; }
        public double OutsideExeQty { get; set; }
        public double InHouseSalesQty { get; set; }
        public double OutsideSalesQty { get; set; } 
        public double InHouseSalesValue  { get; set; }
        public double OutsideSalesValue { get; set; }
        public double TotalProduction { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region
        public string Params { get; set; }
        public double TotalExeQty { get { return this.InHouseExeQty + this.OutsideExeQty; } }
        public double TotalSalesQty { get { return this.InHouseSalesQty + this.OutsideSalesQty; } }
        public double TotalSalesValue { get { return this.InHouseSalesValue + this.OutsideSalesValue; } }

        public string OrderDateStr { get { return (this.OrderDate==DateTime.MinValue)? "": this.OrderDate.ToString("MMM yyyy"); } }

        #endregion

        #region Functions
        public static List<RptFEOSalesStatement> Gets(DateTime dtFrom, DateTime dtTo, Int16 nExeType, long nUserID)
        {
            return RptFEOSalesStatement.Service.Gets(dtFrom, dtTo, nExeType, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IRptFEOSalesStatementService Service
        {
            get { return (IRptFEOSalesStatementService)Services.Factory.CreateService(typeof(IRptFEOSalesStatementService)); }
        }

        #endregion
    }
    #endregion

    #region IRptFEOSalesStatement interface

    public interface IRptFEOSalesStatementService
    {
        List<RptFEOSalesStatement> Gets(DateTime dtFrom, DateTime dtTo, Int16 nExeType, long nUserID);

    }
    #endregion
}