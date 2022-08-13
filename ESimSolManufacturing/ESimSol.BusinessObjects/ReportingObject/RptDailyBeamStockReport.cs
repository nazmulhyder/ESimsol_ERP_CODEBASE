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
    public class RptDailyBeamStockReport
    {
        #region Constructor
        public RptDailyBeamStockReport()
        {
            StartTime = DateTime.Today;
            WeavingProcess = EnumWeavingProcess.Loom;
            Construction = "";
            ReedCount = 0;
            Weave = "";
            TotalEnds = 0;
            Buyer = "";
            FEONo = "";
            Option = "";
            WeftColor = "";
            BeamStock = "";
            LoomNo = "";
            Remarks = "";
            ErrorMessage = "";

            BuyerID = 0;
            FEOID = 0;
            IsInHouse = true;
            OrderType = EnumFabricRequestType.None;
        }
        #endregion

        #region Properties 
        public DateTime StartTime { get; set; }
        public EnumWeavingProcess WeavingProcess { get; set; }
        public string Construction { get; set; }
        public int ReedCount { get; set; }
        public string Weave { get; set; }
        public int TotalEnds { get; set; }
        public string Buyer { get; set; }
        public string FEONo { get; set; }
        public string Option { get; set; }
        public string WeftColor { get; set; }
        public string BeamStock { get; set; }
        public string LoomNo { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        public int BuyerID { get; set; }
        public int FEOID { get; set; }
        public bool IsInHouse { get; set; }
        public EnumFabricRequestType OrderType { get; set; }
        #endregion

        #region Derive Properties
        public string OrderNo
        {
            get
            {
                string sPrifix = "";
                if (this.FEOID > 0)
                {
                    //if (this.IsInHouse) { sPrifix = "EXE"; } else sPrifix = "SCW";

                    //if (this.OrderType == EnumFabricRequestType .Bulk) { sPrifix = sPrifix + "-BLK-"; }
                    //else if (this.OrderType == EnumOrderType.AkijExtra) { sPrifix = sPrifix + "-EXT-"; }
                    //else if (this.OrderType == EnumOrderType.Development) { sPrifix = sPrifix + "-DEV-"; }
                    //else if (this.OrderType == EnumOrderType.SMS) { sPrifix = sPrifix + "-SMS-"; }
                    //else { sPrifix = sPrifix + "-"; }

                    return sPrifix + this.FEONo;

                }
                else return "";
            }
        }
        #endregion

        #region Functions
        public static List<RptDailyBeamStockReport> Gets(long nUserID)
        {
            return RptDailyBeamStockReport.Service.Gets(nUserID);
        }
        public static List<RptDailyBeamStockReport> Gets(string sSQL, long nUserID)
        {
            return RptDailyBeamStockReport.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IRptDailyBeamStockReportService Service
        {
            get { return (IRptDailyBeamStockReportService)Services.Factory.CreateService(typeof(IRptDailyBeamStockReportService)); }
        }
        #endregion
    }

    #region IRptDailyBeamStockReport interface
    public interface IRptDailyBeamStockReportService
    {
        List<RptDailyBeamStockReport> Gets(long nUserID);
        List<RptDailyBeamStockReport> Gets(string sSQL, long nUserID);
    }
    #endregion
}
