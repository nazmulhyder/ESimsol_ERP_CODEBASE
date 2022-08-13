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
    #region DUReturnChallan
    public class DUReturnChallan : BusinessObject
    {
        public DUReturnChallan()
        {
            DUReturnChallanID = 0;
            BUID = 0;
            DUReturnChallanNo = string.Empty;
            ReturnDate = DateTime.Today;
            DUDeliveryChallanID = 0;
            ReceivedByName = string.Empty;
            PreaperByName = string.Empty;
            ApprovedByName = string.Empty;
            Note = string.Empty;
            ApprovedBy = 0;
            ReceivedBy = 0;
            OrderNo = "";
            ContractorName = "";
            DeliveryChallanNo = "";
            WorkingUnitID = 0;
            OrderType = 0;
            PINo = "";
            RefChallanNo = "";
            VehicleInfo = "";
            ErrorMessage = string.Empty;
            DUReturnChallanDetails = new List<DUReturnChallanDetail>();
        }

        #region Property
        public int DUReturnChallanID { get; set; }
        public int BUID {get; set;}
        public string DUReturnChallanNo { get; set; }
        public string ContractorName { get; set; }
        public DateTime ReturnDate { get; set; }
        public int DUDeliveryChallanID { get; set; }
        public string ReceivedByName {get; set;}
        public string Note {get; set;}
        public int ApprovedBy { get; set; }
        public int ReceivedBy { get; set; }
        public int OrderType { get; set; }
        public int WorkingUnitID { get; set; }
        public string DeliveryChallanNo { get; set; }
        public string RefChallanNo { get; set; }
        public string VehicleInfo { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<DUReturnChallanDetail> DUReturnChallanDetails { get; set; }
        public string BUName { get; set; }
        public string DONo { get; set; }
        public string OrderNo { get; set; }
        public string ApprovedByName { get; set; }
        public string PINo { get; set; }
        public string PreaperByName { get; set; }
        public double Qty { get; set; }
        public string WUName { get; set; }
        public string ReturnDateSt
        {
            get
            {
                return this.ReturnDate.ToString("dd MMM yyyy");
            }
        }
        public string OrderTypeSt
        {
            get
            {
                return ((EnumOrderType)this.OrderType).ToString();
            }
        }

        #endregion

        #region Functions

        public DUReturnChallan Get(int id, long nUserID)
        {
            return DUReturnChallan.Service.Get(id, nUserID);
        }
        public static List<DUReturnChallan> Gets(string sSQL, long nUserID)
        {
            return DUReturnChallan.Service.Gets(sSQL, nUserID);
        }
        public DUReturnChallan Save(long nUserID)
        {
            return DUReturnChallan.Service.Save(this, nUserID);
        }

        public DUReturnChallan Approve(long nUserID)
        {
            return DUReturnChallan.Service.Approve(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return DUReturnChallan.Service.Delete(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDUReturnChallanService Service
        {
            get { return (IDUReturnChallanService)Services.Factory.CreateService(typeof(IDUReturnChallanService)); }
        }
        #endregion

    }
    #endregion

    #region IDUReturnChallan interface
    public interface IDUReturnChallanService
    {
        DUReturnChallan Get(int id, Int64 nUserID);
        List<DUReturnChallan> Gets(string sSQL, Int64 nUserID);
        DUReturnChallan Save(DUReturnChallan oDUReturnChallan, Int64 nUserID);
        DUReturnChallan Approve(DUReturnChallan oDUReturnChallan, Int64 nUserID);
        string Delete(DUReturnChallan oDUReturnChallan, long nUserID);
    }
    #endregion
}
