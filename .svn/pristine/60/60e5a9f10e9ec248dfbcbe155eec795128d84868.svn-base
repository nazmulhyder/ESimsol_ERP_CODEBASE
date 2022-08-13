

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
    #region DUHardWinding

    public class DUHardWinding : BusinessObject
    {
        public DUHardWinding()
        {
            DUHardWindingID=0;	
            ReceiveDate=DateTime.MinValue;		
            DyeingOrderID=0;	
            DURequisitionID=0;	
            ProductID=0;	
            LotID=0;	
            Qty=0;	
            MUnitID=0;	
            BagNo=0;	
            Note="";
            LotNo = "";
            Qty_Order = 0;
            Qty_RSOut = 0;
            NumOfCone = 0;
            RSCone = 0;
            Status=EnumWindingStatus.None;	
            StartDate=DateTime.Now;
            EndDate = DateTime.Now;
            OrderDate = DateTime.MinValue;
            QCDate = DateTime.MinValue;		
            MachineDes="";		
            Operator="";
            ColorName = "";
            RouteSheetID = 0;
            RouteSheetNo = "";
            IsInHouse = false;
            IsRewinded = false;
            Balance = 0;
            RSState = EnumRSState.InFloor;
            EntryByName = "";
            DBServerDateTime = DateTime.MinValue;
            IsQCDone = false;

        }

        #region Properties
        public int DUHardWindingID { get; set; }
        public DateTime ReceiveDate { get; set; }
        public DateTime QCDate { get; set; }
        public DateTime DBServerDateTime { get; set; }/// For forward To HW Store Date
        public int DyeingOrderID { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public int DURequisitionID { get; set; }
        public int ProductID { get; set; }
        public int RouteSheetID { get; set; }
        public int LotID { get; set; }
        public double Qty { get; set; }
        public int MUnitID { get; set; }
        public double BagNo { get; set; }
        public string Note { get; set; }
        public string LotNo { get; set; }
        public double Qty_Order { get; set; }
        public double Qty_RSOut { get; set; }
        public double Balance { get; set; }
        public string RouteSheetNo { get; set; }
        public EnumWindingStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime OrderDate { get; set; }
        public string MachineDes { get; set; }
        public string ColorName { get; set; }
        public string Operator { get; set; }
        public string DyeingOrderNo { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string MUnit { get; set; }
        public string ContractorName { get; set; }
        public string BuyerName { get; set; }
        public string OrderType { get; set; }
        public string Param { get; set; }
        public int NumOfCone { get; set; }       
        public int MachineID { get; set; }
        public int RSShiftID { get; set; }
        public double Dia { get; set; }
        public int RSCone { get; set; }
        public string MachineName { get; set; }
        public string RSShitName { get; set; }
        public string EntryByName { get; set; }
        public EnumWarpWeft WarpWeftType { get; set; }
        public int WarpWeftTypeInt { get; set; }
        public bool IsInHouse { get; set; }
        public EnumRSState RSState { get; set; }
        public bool IsRewinded { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsQCDone { get; set; }
        

        #endregion

        #region Derived Property
        public string ReceiveDateST
        {
            get
            {
                if (this.ReceiveDate == DateTime.MinValue)
                    return "-";
                else
                    return ReceiveDate.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string DBServerDateTimeST
        {
            get
            {
                if (this.DBServerDateTime == DateTime.MinValue)
                    return "-";
                else
                    return this.DBServerDateTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string QCDateST
        {
            get
            {
                if (this.QCDate == DateTime.MinValue)
                    return "-";
                else
                    return QCDate.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string StartDateST
        {
            get
            {
                if (this.StartDate == DateTime.MinValue)
                    return "-";
                else
                    return StartDate.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string EndDateST
        {
            get
            {
                if (this.EndDate == DateTime.MinValue)
                    return "-";
                else
                    return EndDate.ToString("dd MMM yyyy hh:mm tt");
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
        public string IsInHouseST
        {
            get
            {
                if (this.IsInHouse==true)
                    return "InHouse";
                else
                    return "Out Side";
            }
        }
        public int StatusInt { get { return (int)Status; } }
        //public string StatusST { get { return EnumObject.jGet(this.Status); } }
        public string StatusST
        {
            get
            {
                if (this.ReceiveDate == DateTime.MinValue && Status <= EnumWindingStatus.Initialize) { return "Wating For Receive"; }
                else if (this.ReceiveDate != DateTime.MinValue && Status <= EnumWindingStatus.Initialize) { return "Received"; }
                else return (EnumObject.jGet(this.Status));
            }
        }
        public string WarpWeftTypeST { get { return EnumObject.jGet(this.WarpWeftType); } }
        public string RSStateSt { get { return EnumObject.jGet(this.RSState); } }
        #endregion

        #region Functions
       
        public static List<DUHardWinding> Gets(long nUserID)
        {
            return DUHardWinding.Service.Gets(nUserID);
        }
        public static List<DUHardWinding> Gets(string sSQL, Int64 nUserID)
        {
            return DUHardWinding.Service.Gets(sSQL, nUserID);
        }
        public DUHardWinding Get(int nId, long nUserID)
        {
            return DUHardWinding.Service.Get(nId, nUserID);
        }
        public List<RouteSheet> YarnOut(string sRSIDs, long nUserID) 
        {
            return DUHardWinding.Service.YarnOut(sRSIDs, nUserID);
        }
        public DUHardWinding Save(long nUserID)
        {
            return DUHardWinding.Service.Save(this, nUserID);
        }
        public DUHardWinding Rewinding(long nUserID)
        {
            return DUHardWinding.Service.Rewinding(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return DUHardWinding.Service.Delete(nId, nUserID);
        }

        public DUHardWinding Receive(long nUserID)
        {
            return DUHardWinding.Service.Receive(this, nUserID);
        }
        public DUHardWinding RSQCDOne(long nUserID)
        {
            return DUHardWinding.Service.RSQCDOne(this, nUserID);
        }
        public DUHardWinding SendToDelivery(long nUserID)
        {
            return DUHardWinding.Service.SendToDelivery(this, nUserID);
        }
        public DUHardWinding SendToHWStore(DUHardWinding oDUHardWinding, long nUserID)
        {
            return DUHardWinding.Service.SendToHWStore(oDUHardWinding, nUserID);
        }
        public DUHardWinding DODAssign(long nUserID)
        {
            return DUHardWinding.Service.DODAssign(this, nUserID);
        }
        public DUHardWinding UpdateReceivedate(long nUserID)
        {
            return DUHardWinding.Service.UpdateReceivedate(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDUHardWindingService Service
        {
            get { return (IDUHardWindingService)Services.Factory.CreateService(typeof(IDUHardWindingService)); }
        }
        #endregion




    }
    #endregion

    #region IDUHardWinding interface
    public interface IDUHardWindingService
    {
        DUHardWinding Get(int id, long nUserID);
        List<DUHardWinding> Gets(long nUserID);
        List<DUHardWinding> Gets(string sSQL, Int64 nUserID);
        List<RouteSheet> YarnOut(string sRSIDs, long nUserID);
        string Delete(int id, long nUserID);
        DUHardWinding Save(DUHardWinding oDUHardWinding, long nUserID);
        DUHardWinding Rewinding(DUHardWinding oDUHardWinding, long nUserID);
        DUHardWinding RSQCDOne(DUHardWinding oDUHardWinding, long nUserID);
        DUHardWinding SendToDelivery(DUHardWinding oDUHardWinding, long nUserID);
        DUHardWinding Receive(DUHardWinding oDUHardWinding, long nUserID);
        DUHardWinding DODAssign(DUHardWinding oDUHardWinding, long nUserID);
        DUHardWinding UpdateReceivedate(DUHardWinding oDUHardWinding, long nUserID);
        DUHardWinding SendToHWStore(DUHardWinding oDUHardWinding, long nUserID);
    }
    #endregion
}
