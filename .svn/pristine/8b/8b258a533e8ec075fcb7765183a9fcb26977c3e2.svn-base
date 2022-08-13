

using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DUSoftWinding

    public class DUSoftWinding : BusinessObject
    {
        public DUSoftWinding()
        {
            DUSoftWindingID=0;	
            ReceiveDate=DateTime.MinValue;		
            DyeingOrderID=0;	
            DURequisitionID=0;	
            ProductID=0;	
            LotID=0;	
            Qty=0;
            MUnitID = 0;
            MachineID = 0;	
            BagNo=0;	
            Note="";
            LotNo = "";
            Qty_Order = 0;
            Qty_RSOut = 0;
            NumOfCone = 0;
            Status=EnumWindingStatus.None;	
            StartDate=DateTime.Now;
            EndDate = DateTime.Now;
            OrderDate = DateTime.MinValue;
            MachineName="";		
            Operator="";
            RSShiftID = 0;
            WorkingUnitID = 0;
            Qty_In = 0;
            QtyGainLoss = 0;
            GainLossType = EnumInOutType.None;
            GainLossTypeInt = 0;
        }

        #region Properties

        public int DUSoftWindingID { get; set; }
        public DateTime ReceiveDate { get; set; }
        public int DyeingOrderID { get; set; }
        public int DURequisitionID { get; set; }
        public int ProductID { get; set; }
        public int LotID { get; set; }
        public int WorkingUnitID { get; set; }
        public double Qty_In { get; set; }
        public double Qty { get; set; }
        public int MUnitID { get; set; }
        public double BagNo { get; set; }
        public string Note { get; set; }
        public string LotNo { get; set; }
        public double Qty_Order { get; set; }
        public double Qty_RSOut { get; set; }
        
        public EnumWindingStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime OrderDate { get; set; }
        public string MachineName { get; set; }
        public string Operator { get; set; }
        public string DyeingOrderNo { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string MUnit { get; set; }
        public string ContractorName { get; set; }
        public string OrderType { get; set; }
        public int MachineID { get; set; }
        public int RSShiftID { get; set; }
        public string Param { get; set; }
        public int NumOfCone { get; set; }
        public string ErrorMessage { get; set; }
        public double QtyGainLoss { get; set; }
        public EnumInOutType GainLossType { get; set; }
        public int GainLossTypeInt { get; set; }
        #endregion

        #region Reporting Property
        public double Qty_SW { get; set; }
        public double Qty_Req { get; set; }
        public double QtySRS { get; set; }
        public double QtySRM { get; set; }
		public int ContractorID  { get; set; }
		public int DestinationLotID  { get; set; }
        public string DestinationLotNo { get; set; }
        #endregion

        #region Derived Property
        public double Balance { get; set; }
        public double QtyTalRed 
        {
            get
            {
                if (this.Qty_In < this.Qty) return this.Qty;
                else return (this.Qty_In - this.Qty);
            }
        }
        public string ReceiveDateST
        {
            get
            {
                if (this.ReceiveDate == DateTime.MinValue)
                    return "-";
                else
                    return ReceiveDate.ToString("dd MMM yyyy");
            }
        }
        public string StartDateST
        {
            get
            {
                if (this.StartDate == DateTime.MinValue)
                    return "-";
                else
                    return StartDate.ToString("dd MMM yyyy hh:MM tt");
            }
        }
        public string EndDateST
        {
            get
            {
                if (this.EndDate == DateTime.MinValue)
                    return "-";
                else
                    return EndDate.ToString("dd MMM yyyy hh:MM tt");
            }
        }
        public string OrderDateST
        {
            get
            {
                if (this.OrderDate == DateTime.MinValue)
                    return "-";
                else
                    return OrderDate.ToString("dd MMM yyyy");
            }
        }
        public int StatusInt { get { return (int)Status; } }
        public string StatusST { get { return EnumObject.jGet(this.Status); } }
        #endregion

        #region Functions
       
        public static List<DUSoftWinding> Gets(long nUserID)
        {
            return DUSoftWinding.Service.Gets(nUserID);
        }
        public static List<DUSoftWinding> Gets(string sSQL, Int64 nUserID)
        {
            return DUSoftWinding.Service.Gets(sSQL, nUserID);
        }
        public static List<DUSoftWinding> Gets_Report(string sSQL, Int64 nUserID)
        {
            return DUSoftWinding.Service.Gets_Report(sSQL, nUserID);
        }
        public DUSoftWinding Get(int nId, long nUserID)
        {
            return DUSoftWinding.Service.Get(nId, nUserID);
        }
        public List<RouteSheet> YarnOut(string sRSIDs, long nUserID) 
        {
            return DUSoftWinding.Service.YarnOut(sRSIDs, nUserID);
        }
        public List<RouteSheet> YarnOut_Multi(string sRSIDs, long nUserID)
        {
            return DUSoftWinding.Service.YarnOut_Multi(sRSIDs, nUserID);
        }
        public DUSoftWinding Save(long nUserID)
        {
            return DUSoftWinding.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return DUSoftWinding.Service.Delete(nId, nUserID);
        }
        public string UpdateRSLot(int nDUSoftWindingID,long nUserID)
        {
            return DUSoftWinding.Service.UpdateRSLot(nDUSoftWindingID, nUserID);
        }
       
        #endregion

        #region ServiceFactory
        internal static IDUSoftWindingService Service
        {
            get { return (IDUSoftWindingService)Services.Factory.CreateService(typeof(IDUSoftWindingService)); }
        }
        #endregion


    }
    #endregion

    #region IDUSoftWinding interface
    public interface IDUSoftWindingService
    {
        DUSoftWinding Get(int id, long nUserID);
        List<DUSoftWinding> Gets(long nUserID);
        List<DUSoftWinding> Gets(string sSQL, Int64 nUserID);
        List<DUSoftWinding> Gets_Report(string sSQL, Int64 nUserID);
        List<RouteSheet> YarnOut(string sRSIDs, long nUserID);
        List<RouteSheet> YarnOut_Multi(string sRSIDs, long nUserID);
        string Delete(int id, long nUserID);
        string UpdateRSLot(int nDUSoftWindingID, long nUserID);
        DUSoftWinding Save(DUSoftWinding oDUSoftWinding, long nUserID);
    }
    #endregion
}
