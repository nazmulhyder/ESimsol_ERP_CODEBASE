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
    #region FabricReturnChallan
    public class FabricReturnChallan : BusinessObject
    {
        public FabricReturnChallan()
        {
            FabricReturnChallanID=0;
            FabricDeliveryChallanID=0;
            ReturnNo="";
            ReturnStatus=EnumChallanStatus.Initialized;
            ReturnDate=DateTime.Now;
            BuyerID=0;
            StoreID=0;
            Remarks="";
            ApprovedBy=0;
            ReceivedBy=0;
            ReceivedDate = DateTime.Now;
            PartyChallanNo="";
            VehicleInfo = "";
            GetInNo = "";
            ReturnPerson = "";
            ErrorMessage = "";
            Params = "";
        }

        #region Properties
        public int FabricReturnChallanID{ get; set; }
        public int FabricDeliveryChallanID{ get; set; }
        public string ReturnNo{ get; set; }
        public EnumChallanStatus ReturnStatus { get; set; }
        public DateTime ReturnDate{ get; set; }
        public int BuyerID{ get; set; }
        public int StoreID{ get; set; }
        public string Remarks{ get; set; }
        public int ApprovedBy{ get; set; }
        public DateTime ApprovedDate{ get; set; }
        public int ReceivedBy{ get; set; }
        public DateTime ReceivedDate{ get; set; }
        public string PartyChallanNo { get; set; }
        public string VehicleInfo { get; set; }
        public string GetInNo { get; set; }
        public string ReturnPerson { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        #endregion

        #region Derived Property  
        public string StoreName { get; set; }

        public string ChallanNo { get; set; }
        public string ApprovedByName { get; set; }
        public string ReceivedByName { get; set; }
        public string IssuedByName { get; set; }
        public string BuyerName { get; set; }
        public DateTime ChallanDate { get; set; }
        public double Qty { get; set; }
        public List<FabricReturnChallanDetail> FRCDetails { get; set; }
        public int WorkingUnitID { get; set; }
        public string QtySt
        {
            get {
                if (this.Qty < 0) return "(" + Global.MillionFormat(this.Qty * (-1), 2) + ")";
                else if (this.Qty == 0) return "-";
                else return Global.MillionFormat(this.Qty, 2);
            }
        }
        public string ReturnDateSt
        {
            get { return (this.ReturnDate == DateTime.MinValue) ? "-" : this.ReturnDate.ToString("dd MMM yyyy"); }
        }
        public string ApproveByDateSt
        {
            get { return (this.ApprovedDate == DateTime.MinValue) ? "-" : this.ApprovedDate.ToString("dd MMM yyyy"); }
        }
        public string ReceiveByDateSt
        {
            get { return (this.ReceivedDate == DateTime.MinValue) ? "-" : this.ReceivedDate.ToString("dd MMM yyyy"); }
        }
        public string ChallanDateSt
        {
            get { return (this.ChallanDate == DateTime.MinValue) ? "-" : this.ChallanDate.ToString("dd MMM yyyy"); }
        }
        #endregion

        #region Functions
        public static List<FabricReturnChallan> Gets(long nUserID)
        {
            return FabricReturnChallan.Service.Gets(nUserID);
        }
        public static List<FabricReturnChallan> Gets(string sSQL, long nUserID)
        {
            return FabricReturnChallan.Service.Gets(sSQL, nUserID);
        }
        public FabricReturnChallan Get(int nId, long nUserID)
        {
            return FabricReturnChallan.Service.Get(nId, nUserID);
        }
        public FabricReturnChallan Save(long nUserID)
        {
            return FabricReturnChallan.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricReturnChallan.Service.Delete(nId, nUserID);
        }
        public FabricReturnChallan ApproveOrReceive(int nDBOperation, long nUserID)
        {
            return FabricReturnChallan.Service.ApproveOrReceive(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricReturnChallanService Service
        {
            get { return (IFabricReturnChallanService)Services.Factory.CreateService(typeof(IFabricReturnChallanService)); }
        }
        #endregion

    }
    #endregion

    #region IFabric interface
    public interface IFabricReturnChallanService
    {
        FabricReturnChallan Get(int id, long nUserID);
        List<FabricReturnChallan> Gets(long nUserID);
        List<FabricReturnChallan> Gets(string sSQL, long nUserID);
        string Delete(int id, long nUserID);
        FabricReturnChallan Save(FabricReturnChallan oFabricReturnChallan, long nUserID);
        FabricReturnChallan ApproveOrReceive(FabricReturnChallan oFabricReturnChallan, int nDBOperation, long nUserID);
    }
    #endregion
}
