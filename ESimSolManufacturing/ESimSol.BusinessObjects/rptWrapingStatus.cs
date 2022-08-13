using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region rptWrapingStatus
    public  class rptWrapingStatus:BusinessObject
    {
        public rptWrapingStatus()
        {
            FEOID = 0;
            FEONo = "";
            BuyerName = "";
            Construction = "";
            TotalEnds = 0;
            ProcessType = "";
            TtlLength = 0;
            CompleteLength = 0;
            YarnReceiveDate = DateTime.MinValue;
            ErrorMessage = "";
            IsInHouse = true;
            OrderType = EnumOrderType.None;

        }
        #region Properties
        public int FEOID { get; set; }
        public string FEONo { get; set; }
        public string BuyerName { get; set; }
        public string Construction { get; set; }
        public double TotalEnds { get; set; }
        public string ProcessType { get; set; }
        public double TtlLength { get; set; }
        public double CompleteLength { get; set; }
        public DateTime YarnReceiveDate { get; set; }
        public string ErrorMessage {get; set;}
        public DateTime LastYarnReceiveDate { get; set; }
        public bool IsInHouse { get; set; }
        public EnumOrderType OrderType { get; set; }
        #endregion
        #region Derived Properties
        public string OrderNo
        {
            get
            {
                //string sPrifix = "";
                //if (this.FEOID > 0)
                //{
                //    if (this.IsInHouse) { sPrifix = "EXE"; } else sPrifix = "SCW";

                //    if (this.OrderType == EnumOrderType.Bulk) { sPrifix = sPrifix + "-BLK-"; }
                //    else if (this.OrderType == EnumOrderType.AkijExtra) { sPrifix = sPrifix + "-EXT-"; }
                //    else if (this.OrderType == EnumOrderType.Development) { sPrifix = sPrifix + "-DEV-"; }
                //    else if (this.OrderType == EnumOrderType.SMS) { sPrifix = sPrifix + "-SMS-"; }
                //    else { sPrifix = sPrifix + "-"; }

                //    return sPrifix + this.FEONo;

                //}
                //else return "";
                return "Please Check BO";

            }
        }
        #endregion

        #region Functions
        public static List<rptWrapingStatus> Gets(int nOrderType, Int64 nUserID)
        {
            return rptWrapingStatus.Service.Gets(nOrderType, nUserID);
        }


        #endregion
        #region ServiceFactory
        internal static IrptWrapingStatusService Service
        {
            get { return (IrptWrapingStatusService)Services.Factory.CreateService(typeof(IrptWrapingStatusService)); }
        }
        #endregion
        
    }
    #endregion

    #region
    public interface IrptWrapingStatusService
    {
        List<rptWrapingStatus> Gets(int nOrderType, Int64 nUserID);

    }
    #endregion
}
