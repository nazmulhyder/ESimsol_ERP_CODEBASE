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
    #region FabricRequisition
    public class FabricRequisition : BusinessObject
    {
        public FabricRequisition()
        {
            FabricRequisitionID = 0;
            RequisitionType = 0;
            ReqNo = "";
            ReqDate = DateTime.Now;
            BUID = 0;
            IssueStoreID = 0;
            ReceiveStoreID = 0;
            Note = "";
            ApprovedBy = 0;
            DisburseBy = 0;
            ReceivedBy = 0;
            ApproveDate = DateTime.MinValue;
            DisburseDate = DateTime.MinValue;
            ReceiveDate = DateTime.MinValue;
            ErrorMessage = "";
            FabricRequisitionDetails = new List<FabricRequisitionDetail>();
            IssueStoreName = "";      
	        ReceiveStoreName="";
	        ApprovedByName="";
	        ReceivedByName="";
            DisburseByName = "";
            FabricRequisitionRoll = new FabricRequisitionRoll();
        }

        #region Property
        public int FabricRequisitionID { get; set; }
        public int RequisitionType { get; set; }
        public string ReqNo { get; set; }
        public DateTime ReqDate { get; set; }
        public int BUID { get; set; }
        public int IssueStoreID { get; set; }
        public int ReceiveStoreID { get; set; }
        public string Note { get; set; }
        public int ApprovedBy { get; set; }
        public int DisburseBy { get; set; }
        public int ReceivedBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public DateTime DisburseDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public bool IsFabricRequisitionRoll { get; set; }
        public FabricRequisitionRoll FabricRequisitionRoll { get; set; }
        public string IssueStoreName { get; set; }
        public string ReceiveStoreName { get; set; }
        public string ApprovedByName { get; set; }
        public string ReceivedByName { get; set; }
        public string DisburseByName { get; set; }
        public List<FabricRequisitionDetail> FabricRequisitionDetails { get; set; }
        
        public string RequisitionTypeInString
        {
            get
            {
                if (this.RequisitionType == 101) return "SRS";
                else if (this.RequisitionType == 102) return "SRM";
                else return "";
            }
        }
        public string ReqDateInString
        {
            get
            {
                return ReqDate.ToString("dd MMM yyyy");
            }
        }
        public string ApproveDateInString
        {
            get
            {
                if (ApproveDate == DateTime.MinValue) return "";
                return ApproveDate.ToString("dd MMM yyyy");
            }
        }
        public string DisburseDateInString
        {
            get
            {
                if (DisburseDate == DateTime.MinValue) return "";
                return DisburseDate.ToString("dd MMM yyyy");
            }
        }
        public string ReceiveDateInString
        {
            get
            {
                if (ReceiveDate == DateTime.MinValue) return "";
                return ReceiveDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<FabricRequisition> Gets(long nUserID)
        {
            return FabricRequisition.Service.Gets(nUserID);
        }
        public static List<FabricRequisition> Gets(string sSQL, long nUserID)
        {
            return FabricRequisition.Service.Gets(sSQL, nUserID);
        }
        public FabricRequisition Get(int id, long nUserID)
        {
            return FabricRequisition.Service.Get(id, nUserID);
        }
        public FabricRequisition Save(long nUserID)
        {
            return FabricRequisition.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FabricRequisition.Service.Delete(id, nUserID);
        }
        public FabricRequisition ChangeStatus(int nUserID)
        {
            return FabricRequisition.Service.ChangeStatus(this, nUserID);
        }
        public FabricRequisition Receive(int nUserID)
        {
            return FabricRequisition.Service.Receive(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricRequisitionService Service
        {
            get { return (IFabricRequisitionService)Services.Factory.CreateService(typeof(IFabricRequisitionService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricRequisition interface
    public interface IFabricRequisitionService
    {
        FabricRequisition Get(int id, Int64 nUserID);
        List<FabricRequisition> Gets(Int64 nUserID);
        List<FabricRequisition> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FabricRequisition Save(FabricRequisition oFabricRequisition, Int64 nUserID);
        FabricRequisition Receive(FabricRequisition oFabricRequisition, int nUserID);
        FabricRequisition ChangeStatus(FabricRequisition oFabricRequisition, int nUserID);
    }
    #endregion
}
