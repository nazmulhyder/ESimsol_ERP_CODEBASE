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
    #region DeliveryChallan
    public class DeliveryChallan
    {
        public DeliveryChallan()
        {
            DeliveryChallanID = 0;
            BUID = 0;
            ChallanType = EnumChallanType.Regular;
            ChallanNo = string.Empty;
            ChallanDate = DateTime.Today;
            ChallanStatus = EnumChallanStatus.Initialized;
            DeliveryOrderID = 0;
            ContractorID = 0;
            ContactPersonnelID = 0;
            GatePassNo = string.Empty;
            VehicleName = string.Empty;
            VehicleNo = string.Empty;
            ReceivedByName = string.Empty;
            Note = string.Empty;
            ApproveBy = 0;
            ApproveDate = DateTime.Today;
            WorkingUnitID = 0;
            StoreInchargeID = 0;
            DeliveryToName = "";
            ErrorMessage = string.Empty;
            RefNo = "";
            DeliveryToAddress = "";
            ProductNatureInInt = 0;
            YetToReturnChallanQty = 0;
            PIID = 0;
            BuyerID = 0;
            BuyerName = "";
            VehicleDateTime = DateTime.Now;
            DeliveryChallanDetails = new List<DeliveryChallanDetail>();
        }

        
        #region Property
        public int DeliveryChallanID { get; set; }
        public int BUID {get; set;}
        public EnumChallanType ChallanType { get; set; }
        public string ChallanNo {get; set;}
        public DateTime ChallanDate {get; set;}
        public EnumChallanStatus ChallanStatus {get; set;}
        public int DeliveryOrderID {get; set;}
        public int ContractorID {get; set;}
        public int ContactPersonnelID {get; set;}
        public string GatePassNo {get; set;}
        public string VehicleName {get; set;}
        public string VehicleNo {get; set;}
        public string ReceivedByName {get; set;}
        public string Note {get; set;}
        public int ApproveBy {get; set;}
        public DateTime  ApproveDate {get; set;}
        public int WorkingUnitID {get; set;}
        public int StoreInchargeID {get; set;}
        public string DeliveryToName { get; set; }
        public string RefNo { get; set; }
        public string DeliveryToAddress { get; set; }
        public double YetToReturnChallanQty { get; set; }
        public int ProductNatureInInt { get; set; }
        public int PIID { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public DateTime VehicleDateTime { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<DeliveryChallanDetail> DeliveryChallanDetails { get; set; }
        public List<DeliveryChallan> DeliveryChallans { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public Company Company { get; set; }
        public string BUName { get; set; }
        public string DONo { get; set; }
        public string ContractorName { get; set; }
        public string ApproveByName { get; set; }
        public string WUName { get; set; }
        public int ProductID { get; set; }

        public string ChallanTypeStr
        {
            get
            {
                return Global.EnumerationFormatter(this.ChallanType.ToString());
            }
        }
        public string ChallanStatusStr
        {
            get
            {
                return Global.EnumerationFormatter(this.ChallanStatus.ToString());
            }
        }
        public string ChallanDateStr
        {
            get
            {
                return this.ChallanDate.ToString("dd MMM yyyy");
            }
        }
        public string VehicleDateTimeInString
        {
            get { return VehicleDateTime.ToString("dd MMM yyyy hh:mm:ss tt"); }
        }

        #endregion

        #region Functions

        public DeliveryChallan Get(int id, long nUserID)
        {
            return DeliveryChallan.Service.Get(id, nUserID);
        }
        public static List<DeliveryChallan> Gets(string sSQL, long nUserID)
        {
            return DeliveryChallan.Service.Gets(sSQL, nUserID);
        }
        public DeliveryChallan IUD(short nDBOperation, long nUserID)
        {
            return DeliveryChallan.Service.IUD(this, nDBOperation, nUserID);
        }
        public DeliveryChallan Approve(long nUserID)
        {
            return DeliveryChallan.Service.Approve(this, nUserID);
        }
        public DeliveryChallan UpdateVehicleTime(long nUserID)
        {
            return DeliveryChallan.Service.UpdateVehicleTime(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDeliveryChallanService Service
        {
            get { return (IDeliveryChallanService)Services.Factory.CreateService(typeof(IDeliveryChallanService)); }
        }
        #endregion
    }
    #endregion

    #region IDeliveryChallan interface
    public interface IDeliveryChallanService
    {
        DeliveryChallan Get(int id, Int64 nUserID);
        List<DeliveryChallan> Gets(string sSQL, Int64 nUserID);
        DeliveryChallan IUD(DeliveryChallan oDeliveryChallan, short nDBOperation, Int64 nUserID);
        DeliveryChallan Approve(DeliveryChallan oDeliveryChallan, Int64 nUserID);
        DeliveryChallan UpdateVehicleTime(DeliveryChallan oDeliveryChallan, Int64 nUserID);
        
    }
    #endregion
}
