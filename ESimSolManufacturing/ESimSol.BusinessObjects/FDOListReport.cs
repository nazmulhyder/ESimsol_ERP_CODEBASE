using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class FDOListReport
    {
        public FDOListReport()
        {
            BuyerName = "";
            GarmentsName = "";
            FEOID = 0;
            FEONo = "";
            FEODate = new DateTime(1900, 01, 01);
            FDOType = EnumDOType.None;
            FDODate = new DateTime(1900, 01, 01);
            FDONo = "";
            PINo = "";
            LCNo = "";
            LCDate = new DateTime(1900, 01, 01);
            Construction = "";
            OrderQty = 0;
            Weave = "";
            Color = "";
            BuyerRef = "";
            PPSample = 0;
            BulkDelivered = 0;
            TotalDelivered = 0;
            Balance = 0;
            OrderStock = 0;
            PPSampleDate = new DateTime(1900, 01, 01);
            BulkStartDate = new DateTime(1900, 01, 01);
            BulkEndDate = new DateTime(1900, 01, 01);
            DelStartDate = new DateTime(1900, 01, 01); 
            DelEndDate = new DateTime(1900, 01, 01);
            ChallanDate = new DateTime(1900, 01, 01);
            ErrorMessage = "";
            Params = "";
            CountChallan = 0;
            FDODQty = 0;
            FDOCQty = 0;
            DelCompleteDate = new DateTime(1900, 01, 01);
            OrderRef = "";
        }

        #region Properties
        public string BuyerName { get; set; }
        public string GarmentsName { get; set; }
        public int FEOID { get; set; }
        public string FEONo { get; set; }
        public DateTime FEODate { get; set; }
        public DateTime FDODate { get; set; }
        public string FDONo { get; set; }
        public string PINo { get; set; }
        public string LCNo { get; set; }
        public DateTime LCDate { get; set; }
        public string Construction { get; set; }
        public double OrderQty { get; set; }
        public string Weave { get; set; }
        public string Color { get; set; }
        public string BuyerRef { get; set; }
        public double PPSample { get; set; }
        public double BulkDelivered { get; set; }
        public double TotalDelivered { get; set; }
        public double Balance { get; set; }
        public double OrderStock { get; set; }
        public DateTime PPSampleDate { get; set; }
        public DateTime BulkStartDate { get; set; }
        public DateTime BulkEndDate { get; set; }
        public DateTime DelStartDate { get; set; }
        public DateTime DelEndDate { get; set; }        
        public DateTime ChallanDate { get; set; }
        public string ErrorMessage { get; set; }
        public EnumDOType FDOType { get; set; }
        public string Params { get; set; }
        public int CountChallan { get; set; }
        public double FDODQty { get; set; }
        public double FDOCQty { get; set; }
        public DateTime DelCompleteDate { get; set; }
        public string OrderRef { get; set; }
        #endregion

        #region Derive Properties
        public string FEODateSt
        {
            get
            {
                return this.ConvertDateTimeInString(this.FEODate);
            }
        }
        public string FDODateSt
        {
            get
            {
                return this.ConvertDateTimeInString(this.FDODate);
            }
        }
        public string DelCompleteDateSt
        {
            get
            {
                return this.ConvertDateTimeInString(this.DelCompleteDate);
            }
        }
        public string LCDateSt
        {
            get
            {
                return this.ConvertDateTimeInString(this.LCDate);
            }
        }
        public string PPSampleDateSt
        {
            get
            {
                return this.ConvertDateTimeInString(this.PPSampleDate);
            }
        }
        public string BulkStartDateSt
        {
            get
            {
                return this.ConvertDateTimeInString(this.BulkStartDate);
            }
        }
        public string BulkEndDateSt
        {
            get
            {
                return this.ConvertDateTimeInString(this.BulkEndDate);
            }
        }
        public string DelStartDateSt
        {
            get
            {
                return this.ConvertDateTimeInString(this.DelStartDate);
            }
        }
        public string DelEndDateSt
        {
            get
            {
                return this.ConvertDateTimeInString(this.DelEndDate);
            }
        }
        public string ChallanDateSt
        {
            get
            {
                return this.ConvertDateTimeInString(this.ChallanDate);
            }
        }
        public string OrderQtySt
        {
            get
            {
                return this.ConvertQtyInString(this.OrderQty);
            }
        }
        public string PPSampleSt
        {
            get
            {
                return this.ConvertQtyInString(this.PPSample);
            }
        }
        public string BulkDeliveredSt
        {
            get
            {
                return this.ConvertQtyInString(this.BulkDelivered);
            }
        }
        public string TotalDeliveredSt
        {
            get
            {
                return this.ConvertQtyInString(this.TotalDelivered);
            }
        }
        public string BalanceSt
        {
            get
            {
                return this.ConvertQtyInString(this.Balance);
            }
        }
        public string OrderStockSt
        {
            get
            {
                return this.ConvertQtyInString(this.OrderStock);
            }
        }
        private string ConvertDateTimeInString(DateTime dDateTime)
        {
            return ((dDateTime == new DateTime(1900, 01, 01) || dDateTime == new DateTime(0001, 01, 01)) ? "-" : dDateTime.ToString("dd MMM yyyy"));
        }
        private string ConvertQtyInString(double nQty)
        {
            if (nQty < 0) return "(" + Global.MillionFormat(nQty * (-1)) + ")";
            else if (nQty == 0) return "-";
            else return Global.MillionFormat(nQty);
        }
        #endregion

        #region Functions
        public static List<FDOListReport> Gets(long nUserID)
        {
            return FDOListReport.Service.Gets(nUserID);
        }
        public static List<FDOListReport> GetsBysp(string Params, long nUserID)
        {
            return FDOListReport.Service.GetsBysp(Params, nUserID);
        }
        public static List<FDOListReport> Gets(string sSQL, long nUserID)
        {
            return FDOListReport.Service.Gets(sSQL, nUserID);
        }
        public FDOListReport Get(int nEPIDID, long nUserID)
        {
            return FDOListReport.Service.Get(nEPIDID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFDOListReportService Service
        {
            get { return (IFDOListReportService)Services.Factory.CreateService(typeof(IFDOListReportService)); }
        }
        #endregion
    }

    #region IFDOListReport interface
    public interface IFDOListReportService
    {
        List<FDOListReport> Gets(long nUserID);
        List<FDOListReport> Gets(string sSQL, long nUserID);
        FDOListReport Get(int nEPIDID, long nUserID);
        List<FDOListReport> GetsBysp(string sParams, long nUserID);
    }
    #endregion
}
