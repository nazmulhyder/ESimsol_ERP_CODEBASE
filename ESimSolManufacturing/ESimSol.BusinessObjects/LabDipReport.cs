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
    #region LabDipReport

    public class LabDipReport : BusinessObject
    {
        public LabDipReport()
        {
            LabDipID = 0;
            LabdipNo = string.Empty;
            BuyerRefNo = string.Empty;
            PriorityLevel = 0;
            Note = string.Empty;
            OrderStatus = 0;
            LabDipFormat = 0;
            OrderReferenceType =0;
            SeekingDate = DateTime.Today;
            OrderDate = DateTime.Today;
            ISTwisted = false;
            ColorSet = 3;
            ShadeCount = 3;
            KnitPlyYarn = 0;
            ColorName = string.Empty;
            RefNo = string.Empty;
            PantonNo = string.Empty;
            RGB = string.Empty;
            ColorNo = "";
            ChallanNo = "";
            Combo = 1;
            LotNo = string.Empty;
            ContractorCPName = string.Empty;
            LDNo="";
            EndBuyer = "";
           
        }

        #region Properties
        public int LabDipID { get; set; }
        public int LabDipDetailID { get; set; }
        public string LabdipNo { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime SeekingDate { get; set; }
        public DateTime DeliveyDate { get; set; }
        public string ContractorName { get; set; }
        public string DeliveryToName { get; set; }
        public string MktPerson { get; set; }
        public int OrderStatus { get; set; }
        public short ProductID { get; set; }
        public string ContractorCPName { get; set; }
        public int NoOfColor { get; set; }
        public int PriorityLevel { get; set; }
        public bool ISTwisted { get; set; }
        public string BuyerRefNo { get; set; }
        public string Note { get; set; }
        public string ChallanNo { get; set; }
        public int LabDipFormat { get; set; }
        public int OrderReferenceType { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public Int16 ColorSet { get; set; }
        public Int16 ShadeCount { get; set; }
        public int KnitPlyYarn { get; set; }
        public string ColorName { get; set; }
        public string RefNo { get; set; }
        public string PantonNo { get; set; }
        public string EndBuyer { get; set; }
        public string RGB { get; set; }
        public string ColorNo { get; set; }
        public Int16 Combo { get; set; }
        public string LotNo { get; set; }
        #endregion

        #region Derive
        public string LDNo { get; set; }
        public string DeliveryZoneName { get; set; }
        public string LightSourceName { get; set; }
        public string DeliveryToCPName { get; set; }
        public string ErrorMessage { get; set; }
        public string PriorityLevelStr
        {
            get
            {
                return EnumObject.jGet((EnumPriorityLevel)this.PriorityLevel);
            }
        }
        public string OrderStatusStr
        {
            get
            {
                return ((EnumLabdipOrderStatus)this.OrderStatus).ToString();
            }
        }
        public string LabDipFormatStr
        {
            get
            {
                return EnumObject.jGet((EnumLabdipFormat)this.LabDipFormat);
            }
        }
        public string KnitPlyYarnSt
        {
            get
            {
                return EnumObject.jGet((EnumKnitPlyYarn)this.KnitPlyYarn);
            }
        }
        public string OrderReferenceTypeStr
        {
            get
            {
                return EnumObject.jGet((EnumOrderType)this.OrderReferenceType);
               
            }
        }
        public string SeekingDateStr
        {
            get
            {
                return this.SeekingDate.ToString("dd MMM yyyy");
            }
        }
        public string OrderDateStr
        {
            get
            {
                return this.OrderDate.ToString("dd MMM yyyy");
            }
        }
        public string DeliveyDateStr
        {
            get
            {
                if (this.DeliveyDate==DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                return this.OrderDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string TwistedStr
        {
            get
            {
                return (this.ISTwisted) ? "Yes" : "No";
            }
        }


        #endregion


    #endregion


        #region Functions

 
        public static List<LabDipReport> Gets(string sSQL, Int64 nUserID)
        {
            return LabDipReport.Service.Gets(sSQL, nUserID);
        }
        public static List<LabDipReport> Gets_Product(string sSQL, Int64 nUserID)
        {
            return LabDipReport.Service.Gets_Product(sSQL, nUserID);
        }
        public static List<LabDipReport> GetsSql(string sSQL, Int64 nUserID)
        {
            return LabDipReport.Service.GetsSql(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static ILabDipReportService Service
        {
            get { return (ILabDipReportService)Services.Factory.CreateService(typeof(ILabDipReportService)); }
        }
        #endregion


    }

    #region ILabDipReport interface
    public interface ILabDipReportService
    {
        List<LabDipReport> Gets(string sSQL, Int64 nUserID);
        List<LabDipReport> GetsSql(string sSQL, Int64 nUserID);
        List<LabDipReport> Gets_Product(string sSQL, Int64 nUserID);
        
    }
    #endregion
}
