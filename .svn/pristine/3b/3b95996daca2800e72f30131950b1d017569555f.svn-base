using ICS.Core.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class RptDUOrderStatus : BusinessObject
    {
        public RptDUOrderStatus()
        {
            DyeingOrderID = 0;
            ProductID = 0;
            ProductBaseID = 0;
            ProductCategoryID = 0;
            ProductName = "";
            CategoryName = ""; 
            ProductBaseName = ""; 
            OrderQty = 0;
            SRSQty = 0; 
            SRSQty = 0; 
            SRMQty = 0;
            OrderType = 0; 
            QtyQC = 0; 
            QtyDC = 0;
            QtyRC = 0;
            Startdate = DateTime.Now;
            Enddate = DateTime.Now;
            ErrorMessage = "";
            ReportLayout = 0;
            BUID = 0;
            QtyDyeing = 0;
            Qty_Hydro = 0;
            Qty_Drier = 0;
            Qty_WQC = 0;
            Qty_RS = 0;
            RecycleQty = 0;
            WastageQty = 0;
            ContractorID = 0;
            ContractorName = "";
            OrderName = "";
            PINo = "";
            OrderNo = "";
            OrderDate = DateTime.Now;
            YarnReceive = 0;
 
        }
        #region Properties
        public int DyeingOrderID { get; set; }
        public int ProductID { get; set; }
        public int ProductBaseID { get; set; }
        public int ProductCategoryID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string CategoryName { get; set; }
        public string ProductBaseName { get; set; }
        public double OrderQty { get; set; }
        public double SRSQty { get; set; }
        public double SRMQty { get; set; }
        public int OrderType { get; set; }
        public double YarnOut { get; set; }
        public double QtyQC { get; set; }
        public double QtyDC { get; set; }
        public double QtyRC{ get; set; }      
        public string ErrorMessage { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public int ReportLayout { get; set; }
        public int BUID { get; set; }
        public double QtyDyeing { get; set; }
        public double Qty_Hydro { get; set; }
        public double Qty_Drier { get; set; }
        public double Qty_WQC { get; set; }
        public double Qty_RS { get; set; }
        public double RecycleQty { get; set; }
        public double WastageQty { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public string OrderName { get; set; }
        public string PINo { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public double YarnReceive { get; set; }


        #endregion
        #region Derived Property
        public double PendingSRSST
        {
            get
            {
                return (this.OrderQty - this.SRSQty) + this.SRMQty;
            }
        }
        public double WIPST
        {
            get
            {
                return (this.QtyDyeing + this.Qty_Hydro + this.Qty_Drier + this.Qty_WQC);
            }
        }
        public double PendingYarnOutST
        {
            get
            {
                return (this.SRSQty - this.SRMQty) - this.YarnOut;
            }

        }
        public double PendingDeliveryST
        {
            get
            {
                return (this.OrderQty + this.QtyRC) - this.QtyDC;
            }
        }

        public string StartDateInString
        {
            get
            {
                return Startdate.ToString("dd MMM yyyy hh:mm");
            }
        }
        public string EndDateInString
        {
            get
            {
                return Enddate.ToString("dd MMM yyyy hh:mm");
            }
        }
        public string OrderTypeInST
        {
            get
            {
                return this.OrderType.ToString();
            }
        }

        #endregion
        #region Functions
        public static List<RptDUOrderStatus> MailContent(string ProductIDs,int nReportType, DateTime StartTime, DateTime EndTime,int BUID, long nUserID)
        {
            return RptDUOrderStatus.Service.MailContent(ProductIDs,nReportType, StartTime, EndTime,BUID, nUserID);
        }
        public static DataSet Gets(string sSQL, long nUserID)
        {
            return RptDUOrderStatus.Service.Gets(sSQL, nUserID);
        }

        #endregion
        #region ServiceFactory
        internal static IRptDUOrderStatusService Service
        {
            get { return (IRptDUOrderStatusService)Services.Factory.CreateService(typeof(IRptDUOrderStatusService)); }
        }

        #endregion
    }
    #region IRptDUOrderStatus interface

    public interface IRptDUOrderStatusService
    {
        List<RptDUOrderStatus> MailContent(string ProductIDs, int nReportType, DateTime StartTime, DateTime EndTime, int BUID, long nUserID);
        DataSet Gets(string  sSQL, long nUserID);
    }
    #endregion
}
