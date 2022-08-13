using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region DUDeliverySummaryRPT
    public class DUDeliverySummaryRPT : BusinessObject
    {
        public DUDeliverySummaryRPT()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            OrderType = 0;
            OrderTypeSt = "";
            RefID = 0;
            RefName = "";
            QtyIn = 0;
            QtyOut = 0;
            AmountIn = 0;
            AmountOut = 0;
            Remarks = "";
            ReportLayout = 0;
            BUID = 0;
            ErrorMessage = "";
        }

        #region Property
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int OrderType { get; set; }
        public string OrderTypeSt { get; set; }
        public int RefID { get; set; }
        public string RefName { get; set; }
        public double QtyIn { get; set; }
        public double QtyOut { get; set; }
        public double AmountIn { get; set; }
        public double AmountOut { get; set; }
        public string Remarks { get; set; }
        public int BUID { get; set; }
        public int DateFormat { get; set; }
        public string ErrorMessage { get; set; }
        public int ReportLayout { get; set; }
        #endregion

        #region Derived Property
        public double AvgQtyIn
        {
            get
            {
                int days = DateTime.DaysInMonth(this.StartDate.Year, this.StartDate.Month);
                return (QtyIn / days);
            }
        }
        public double AvgQtyOut
        {
            get
            {
                int days = DateTime.DaysInMonth(this.StartDate.Year, this.StartDate.Month);
                return (QtyOut / days);
            }
        }
        public double AvgTotalQty
        {
            get
            {
                int days = DateTime.DaysInMonth(this.StartDate.Year, this.StartDate.Month);
                return ((QtyIn / days) + (QtyOut / days));
            }
        }
        public double AvgAmountIn
        {
            get
            {
                int days = DateTime.DaysInMonth(this.StartDate.Year, this.StartDate.Month);
                return (AmountIn / days);
            }
        }
        public double AvgAmountOut
        {
            get
            {
                int days = DateTime.DaysInMonth(this.StartDate.Year, this.StartDate.Month);
                return (AmountOut / days);
            }
        }
        public double AvgTotalAmount
        {
            get
            {
                int days = DateTime.DaysInMonth(this.StartDate.Year, this.StartDate.Month);
                return ((AmountIn / days) + (AmountOut / days));
            }
        }
        public double TotalQty
        {
            get
            {
                return QtyIn + QtyOut;
            }
        }
        public double TotalAmount
        {
            get
            {
                return AmountIn + AmountOut;
            }
        }
        public string StartDateInString
        {
            get
            {
                if (StartDate == DateTime.MinValue) return "";
                return StartDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateInString
        {
            get
            {
                if (EndDate == DateTime.MinValue) return "";
                return EndDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<DUDeliverySummaryRPT> GetsData(DUDeliverySummaryRPT oDUDeliverySummaryRPT, long nUserID)
        {
            return DUDeliverySummaryRPT.Service.GetsData(oDUDeliverySummaryRPT, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDUDeliverySummaryRPTService Service
        {
            get { return (IDUDeliverySummaryRPTService)Services.Factory.CreateService(typeof(IDUDeliverySummaryRPTService)); }
        }
        #endregion

        public List<DUDeliverySummaryRPT> DUDeliverySummaryRPTs { get; set; }
    }
    #endregion

    #region IDUDeliverySummaryRPT interface
    public interface IDUDeliverySummaryRPTService
    {
        List<DUDeliverySummaryRPT> GetsData(DUDeliverySummaryRPT oDUDeliverySummaryRPT, Int64 nUserID);
    }
    #endregion
}
