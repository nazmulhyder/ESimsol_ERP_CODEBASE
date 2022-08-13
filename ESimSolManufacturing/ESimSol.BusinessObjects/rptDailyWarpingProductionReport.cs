using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Linq;


namespace ESimSol.BusinessObjects.ReportingObject
{
    #region rptDailyWarpingProductionReport
    public class rptDailyWarpingProductionReport
    {
        public rptDailyWarpingProductionReport()
        {
            FBPID =0;
            StartTime =DateTime.MinValue;
            EndTime = DateTime.MinValue;
            FMID =0;
            Qty =0;
            FEOID =0;
            FEONo =string.Empty;
            ReedCount =string.Empty;
            Dent =string.Empty;
            FBID =0;
            BuyerName = string.Empty;
            Construction =string.Empty;
            Code =string.Empty;
            WarpDoneQty =0;
            FabricBatchQty  =0;
            ProcessName = string.Empty;
            TotalEnds = 0;
            TtlLength = 0;
            CompleteLength = 0;
        }

        #region Properties
        public int FBPID {get; set;}
        public DateTime StartTime  {get; set;}
        public DateTime EndTime { get; set; } 
        public int FMID {get; set;}
        public double Qty {get; set;}
        public int FEOID {get; set;}
        public string FEONo {get; set;}
        public string  ReedCount {get; set;}
        public string Dent {get; set;}
        public int FBID {get; set;}
        public string BuyerName { get; set; }
        public string  Construction{get; set;}
        public string Code {get; set;}
        public double  WarpDoneQty {get; set;}
        public double  FabricBatchQty  {get; set;}
        public string ProcessName { get; set; }
        public string Remark { get; set; }
        public string ErrorMessage { get; set; }
        public double TotalEnds { get; set; }
        public double TtlLength { get; set; }
        public double CompleteLength { get; set; }
        #endregion

        #region Derive Properties
        public string StartTimeSt
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01, 1, 1, 1);
                if (this.StartTime == MinValue)
                {
                    return "";
                }
                return StartTime.ToString("dd MMM yyyy");
            }
        }
        public string EndTimeSt
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01, 1, 1, 1);
                if (this.EndTime == MinValue)
                {
                    return "";
                }
                return EndTime.ToString("dd MMM yyyy");
            }
        }
      
    
  
        #endregion

        #region Functions

        public static List<rptDailyWarpingProductionReport> Gets(DateTime dtDate, long nUserID)
        {
            return rptDailyWarpingProductionReport.Service.Gets(dtDate, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IrptDailyWarpingProductionReportService Service
        {
            get { return (IrptDailyWarpingProductionReportService)Services.Factory.CreateService(typeof(IrptDailyWarpingProductionReportService)); }
        }
        #endregion
    }
    #endregion

    #region IRptDailyLogReport interface
    public interface IrptDailyWarpingProductionReportService
    {

        List<rptDailyWarpingProductionReport> Gets(DateTime dtDate, long nUserID);
    }
    #endregion
}
