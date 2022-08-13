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
    #region WUProductionDailyInspection

    public class WUProductionDailyInspection : BusinessObject
    {
        #region  Constructor
        public WUProductionDailyInspection()
        {
            FEOID = 0;
            FEONo = string.Empty;
            BuyerID = 0;
            BuyerName = string.Empty;
            Construction = string.Empty;
            ProcessType = string.Empty;
            GreyWidth = string.Empty;
            GradeA = 0;
            GradeB = 0;
            Reject = 0;
            Remarks = string.Empty;
            ReedCount = string.Empty;
            ErrorMessage = string.Empty;
            Params = "";
            FabricWeaveName = string.Empty;
        }
        #endregion

        #region Properties
        public int FEOID { get; set; }
        public string FEONo { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public string Construction { get; set; }
        public string ProcessType { get; set; }
        public string GreyWidth { get; set; }
        public double GradeA { get; set; }
        public double GradeB { get; set; }
        public double Reject { get; set; }
        public string Remarks { get; set; }
        public string ReedCount { get; set; }
        public string ErrorMessage { get; set; }
        public int TSUID { get; set; }
        public string TSUName { get; set; }
        public DateTime ProductionDate { get; set; }
        public string ProductionDateStr { get { return this.ProductionDate.ToString("dd MMM yyy"); } }
        public string FabricWeaveName { get; set; }
        public bool IsInHouse { get; set; }

        public EnumOrderType OrderType { get; set; }
        public string OrderNo
        {
            get
            {
                string sPrifix = "";
                if (this.FEOID > 0)
                {
                    if (this.IsInHouse) { sPrifix = "EXE"; } else sPrifix = "SCW";

                    //if (this.OrderType == EnumOrderType.Bulk) { sPrifix = sPrifix + "-BLK-"; }
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

        #region
        public string Params { get; set; }
        public double TotalInspection { get { return this.GradeA + this.GradeB + this.Reject; } }

        #endregion

        #region Functions
        public static List<WUProductionDailyInspection> Gets(DateTime dtFrom, string sFEOID, string sBuyerID, string sFMID,DateTime dtTO,int TSUID, long nUserID)
        {
            return WUProductionDailyInspection.Service.Gets(dtFrom, sFEOID, sBuyerID, sFMID,dtTO,TSUID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IWUProductionDailyInspectionService Service
        {
            get { return (IWUProductionDailyInspectionService)Services.Factory.CreateService(typeof(IWUProductionDailyInspectionService)); }
        }

        #endregion
    }
    #endregion

    #region IWUProductionDailyInspection interface

    public interface IWUProductionDailyInspectionService
    {
        List<WUProductionDailyInspection> Gets(DateTime dtFrom, string sFEOID, string sBuyerID, string sFMID,DateTime dtTO,int TSUID, long nUserID);

    }
    #endregion
}
