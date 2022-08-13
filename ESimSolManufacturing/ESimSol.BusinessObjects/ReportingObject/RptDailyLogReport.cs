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
    public class RptDailyLogReport
    {
         public RptDailyLogReport()
         {
             StartTime = new DateTime(1900, 01, 01, 1, 1, 1);
             EndTime = new DateTime(1900, 01, 01, 1, 1, 1);
             FEOID = 0;
             FEONo = "";
             OrderType = EnumFabricRequestType.None;
             IsInHouse = true;
             Construction = "";
             BuyerName = "";
             TotalEnds = 0;
             FabricWeaveName = "";
             ReedCount = 0;
             Dent = "";
             MachineCode = "";
             RefNo = "";
             RunLoom = 0;
             StopLoom = 0;
             Remark = "";
             IsYarnDyed = true;
             BuyerID = 0;
             ErrorMessage = "";
             FabricWeave = 0;
             ProcessType = 0;
             StopLoomNo = string.Empty;
             DailyLogs = new List<RptDailyLogReport>();
             TSUID = 0;
             TSUName = string.Empty;
             WarpColor = string.Empty;
             WeftColor = string.Empty;
             FBPID = 0;
             StopLoomFMIDs = string.Empty;
             RunLoomFMIDs = string.Empty;

         }
         #region Properties
         public string StopLoomString { get; set; }
         public string WarpLot { get; set; }
         public string WeftLot { get; set; }
         public int FBPID { get; set; }
         public DateTime StartTime { get; set; } 
         public DateTime EndTime { get; set; } 
         public int FEOID { get; set; }
         public int BuyerID { get; set; }
         public string FEONo { get; set; }
         public EnumFabricRequestType OrderType { get; set; }
         public bool IsInHouse { get; set; }
         public string Construction { get; set; }
         public string BuyerName { get; set; }
         public int TotalEnds { get; set; }
         public string FabricWeaveName { get; set; }
         public double ReedCount { get; set; }
         public string Dent { get; set; }
         public string MachineCode { get; set; }
         public string RefNo { get; set; }
         public int RunLoom { get; set; }
         public int StopLoom { get; set; }
         public string Remark { get; set; }
         public bool IsYarnDyed { get; set; }
         public string ErrorMessage { get; set; }
         public int ProcessType { get; set; }
         public int FabricWeave { get; set; }
         public string StopLoomNo { get; set; }
         public string WarpColor { get; set; }
         public string WeftColor { get; set; }
         public string StopLoomFMIDs { get; set; }
         public string RunLoomFMIDs { get; set; } 
         #endregion

         #region Derive Properties EndTime
 

         public List<RptDailyLogReport> DailyLogs { get; set; }
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
         public string OrderNo
         {
             get
             {
                 string sPrifix = "";
                 if (this.FEOID > 0)
                 {
                     //if (this.IsInHouse) { sPrifix = "EXE"; } else sPrifix = "SCW";

                     //if (this.OrderType == EnumOrderType.Bulk) { sPrifix = sPrifix + "-BLK-"; }
                     //else if (this.OrderType == EnumOrderType.AkijExtra) { sPrifix = sPrifix + "-EXT-"; }
                     //else if (this.OrderType == EnumOrderType.Development) { sPrifix = sPrifix + "-DEV-"; }
                     //else if (this.OrderType == EnumOrderType.SMS) { sPrifix = sPrifix + "-SMS-"; }
                     return sPrifix + this.FEONo;

                 }
                 else return "";
             }
         }
         public int PPI
         {
             get
             {
                 if (!string.IsNullOrEmpty(this.Construction))
                 {
                     string[] spliteCons = this.Construction.ToUpper().Split('X');
                     if (spliteCons.Length > 0)
                     {
                         string sGetLastPart = spliteCons[spliteCons.Length - 1];
                         int n;
                         bool isNumeric = int.TryParse(sGetLastPart, out n);
                         if (isNumeric) return Convert.ToInt32(sGetLastPart);
                         else return 0;
                     }
                 }
                 return 0;
             }
         }
         public int TotalPick
         {
             get
             {
                 if (!string.IsNullOrEmpty(this.MachineCode) && this.PPI > 0)
                 {
                     string[] spliteItems = this.MachineCode.Split('~');
                     return this.PPI * spliteItems.Length;
                 }
                 return 0;
             }
         }
         public string MachineCodesSt
         {
             get
             {
                 if (!string.IsNullOrEmpty(this.MachineCode))
                 {
                     string[] arrCodes = this.MachineCode.Split('~');
                     return MachineCodeSetup.GenerateCode(arrCodes);
                    
                 }
                 return "-";
             }
         }
         public int TotalLoom
         {
             get
             {
                 return (this.RunLoom + this.StopLoom);
             }
         }

         public string ReedCountWithDent
         {
             get
             {
                 if (this.ReedCount > 0)
                 {
                     return this.ReedCount.ToString().Split('.')[0] + "/" + this.Dent;
                 }
                 else
                 {
                     return "";
                 }
             }
         }

         public int TSUID { get; set; }
         public string TSUName { get; set; }
        #endregion

         #region Functions
         public static List<RptDailyLogReport> Gets(long nUserID)
         {
             return RptDailyLogReport.Service.Gets(nUserID);
         }
         public static List<RptDailyLogReport> Gets(string sSQL, long nUserID)
         {
             return RptDailyLogReport.Service.Gets(sSQL, nUserID);
         }
         public static List<RptDailyLogReport> Gets(DateTime dtLoomStart, string sFEOIDs, string sBuyerIDs, int nFabricWeave, int nProcessType, string sConstruction, int nTsuid,double ReedCount, long nUserID)
         {
             return RptDailyLogReport.Service.Gets(dtLoomStart, sFEOIDs, sBuyerIDs, nFabricWeave, nProcessType, sConstruction, nTsuid,ReedCount, nUserID);
         }
         #endregion

         #region ServiceFactory
         internal static IRptDailyLogReportService Service
         {
             get { return (IRptDailyLogReportService)Services.Factory.CreateService(typeof(IRptDailyLogReportService)); }
         }
         #endregion
    }

    #region IRptDailyLogReport interface
    public interface IRptDailyLogReportService
    {
        List<RptDailyLogReport> Gets(long nUserID);
        List<RptDailyLogReport> Gets(string sSQL, long nUserID);
        List<RptDailyLogReport> Gets(DateTime dtLoomStart, string sFEOIDs, string sBuyerIDs, int nFabricWeave, int nProcessType, string sConstruction, int nTsuid,double ReedCount, long nUserID);
    }
    #endregion

    public class MachineCodeSetup
    {
        public static string GenerateCode(string[] arrCodes)
        {
            if (arrCodes.Length > 0)
            {
                List<int> numCodes = new List<int>();
                List<string> strCodes = new List<string>();
                foreach (string val in arrCodes)
                {
                    int num = 0;
                    if (Int32.TryParse(val, out num))
                    {
                        numCodes.Add(num);
                    }
                    else
                    {
                        strCodes.Add(val);
                    }
                }
                string res = string.Join(" | ", numCodes.OrderBy(x => x).ToList().Select(x => x.ToString())) + ((numCodes.Any() && strCodes.Any()) ? " | " : "") + string.Join(" | ", strCodes.OrderBy(x => x).ToList());
                return "[" + res + "]";
            }
            else
            {
                return "";
            }
           
        }
    }
}
