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
    #region InventoryTrackingWIP
    public class InventoryTrackingWIP : BusinessObject
    {
        public InventoryTrackingWIP()
        {
            BUID = 0;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            WorkingUnitID = 0;
            ProductID = 0;
            ProductName = "";
            ProductCode = "";
            OpeningQty = 0;
            ClosingQty = 0;
            InQty = 0;
            OutQty = 0;
            PCategoryID = 0;
            PCategoryName = "";
            WorkingUnitName = "";
            ErrorMessage = "";
            MainList = new List<InventoryTrackingWIP>();
            IssueList = new List<InventoryTrackingWIP>();
            ReceiveList = new List<InventoryTrackingWIP>();
            QtyPacking = 0;
            Qty_RS = 0;
            QtyRecycle = 0;
            QtyWastage = 0;
            QtyShort = 0;
            LotID = 0;
            MUnitID = 0;
            TransactionTime = DateTime.MinValue;
            StoreName = "";
            LotNo = "";
            TriggerParentID = 0;
            RefNo = "";
            USymbol = "";
        }

        #region Property
        public int BUID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int WorkingUnitID { get; set; }
        public string WorkingUnitName { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public double OpeningQty { get; set; }
        public double ClosingQty { get; set; }
        public double Qty { get; set; }
        public double InQty { get; set; }
        public double OutQty { get; set; }
        public int PCategoryID { get; set; }
        public string PCategoryName { get; set; }
        public double QtyPacking { get; set; }
        public double Qty_RS { get; set; }
        public double QtyRecycle { get; set; }
        public double QtyWastage { get; set; }
        public double QtyShort { get; set; }

        public int InOutType { get; set; }
        public int LotID { get; set; }
        public int MUnitID { get; set; }
        public DateTime TransactionTime { get; set; }
        public string StoreName { get; set; }
        public string LotNo { get; set; }
        public int TriggerParentID { get; set; }
        public string RefNo { get; set; }
        public string USymbol { get; set; }

        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string TransactionTimeInSt
        {
            get
            {
                return TransactionTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string StartDateInSt
        {
            get
            {
                return StartDate.ToString("dd MMM yyyy");
            }
        }
        public string QtySt
        {
            get
            {
                return Qty.ToString("#,##0.000;(#,##0.000)");
            }
        }
        public string QtyPackingSt
        {
            get
            {
                return QtyPacking.ToString("#,##0.000;(#,##0.000)");
            }
        }
        public string Qty_RSSt
        {
            get
            {
                return Qty_RS.ToString("#,##0.000;(#,##0.000)");
            }
        }
        public string QtyRecycleSt
        {
            get
            {
                return QtyRecycle.ToString("#,##0.000;(#,##0.000)");
            }
        }
        public string QtyWastageSt
        {
            get
            {
                return QtyWastage.ToString("#,##0.000;(#,##0.000)");
            }
        }
        public string QtyShortSt
        {
            get
            {
                return QtyShort.ToString("#,##0.000;(#,##0.000)");
            }
        }
        public List<InventoryTrackingWIP> MainList { get; set; }
        public List<InventoryTrackingWIP> IssueList { get; set; }
        public List<InventoryTrackingWIP> ReceiveList { get; set; }
        public List<InventoryTrackingWIP> ITWIPsForIssue { get; set; }
        public List<InventoryTrackingWIP> ITWIPsForReceive { get; set; }
        public string EndDateInSt
        {
            get
            {
                return EndDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<InventoryTrackingWIP> Gets(InventoryTrackingWIP oITWIWP,long nUserID)
        {
            return InventoryTrackingWIP.Service.Gets(oITWIWP, nUserID);
        }
        public static List<InventoryTrackingWIP> Gets(string sSQL, long nUserID)
        {
            return InventoryTrackingWIP.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IInventoryTrackingWIPService Service
        {
            get { return (IInventoryTrackingWIPService)Services.Factory.CreateService(typeof(IInventoryTrackingWIPService)); }
        }
        #endregion
    }
    #endregion

    #region IInventoryTrackingWIP interface
    public interface IInventoryTrackingWIPService
    {
        List<InventoryTrackingWIP> Gets(InventoryTrackingWIP oITWIWP, Int64 nUserID);
        List<InventoryTrackingWIP> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
