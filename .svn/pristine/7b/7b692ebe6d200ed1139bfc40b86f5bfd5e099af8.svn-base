using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region WYRequisition
    
    public class WYRequisition : BusinessObject
    {
        public WYRequisition()
        {
            WYRequisitionID = 0;
            RequisitionNo = "";
            IssueDate = DateTime.Now;
            IssueStoreID= 0;
            ReceiveStoreID= 0;
            ReceiveStoreName= "";
            IssueStoreName= "";
            Remarks = "";
            BUID = 0;
            ApprovedBy =0;
            ApprovedByName = "";
            DisburseBy = 0;
            DisburseByName = "";
            ReceivedBy = 0;
            ReceivedByName = "";
            RequisitionByName = "";
            WYarnType = EnumWYarnType.None;
            RequisitionType = EnumInOutType.Receive;
            WYarnTypeInt = 0;
            ReceiveDate = DateTime.MinValue;
            FEOYSList = new List<FabricExecutionOrderYarnReceive>();
            WarpWeftType = EnumWarpWeft.None;
            DispoNo = "";
        }

        #region Properties    
        public int WYRequisitionID { get; set; }
        public string RequisitionNo { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public int IssueStoreID { get; set; }
        public string IssueStoreName { get; set; }
        public int ReceiveStoreID { get; set; }
        public string ReceiveStoreName { get; set; }
        public string Remarks { get; set; }
        public int BUID { get; set; }
        public int  ApprovedBy{ get; set; }
        public string ApprovedByName{ get; set; }
        public int DisburseBy{ get; set; }
        public string DisburseByName{ get; set; }
        public int ReceivedBy { get; set; }
        public string ReceivedByName { get; set; }
        public string ColorName { get; set; }
        public string DispoNo { get; set; }
        public EnumWYarnType WYarnType { get; set; }
        public int WYarnTypeInt { get; set; }
        public EnumWarpWeft WarpWeftType { get; set; }
        public List<FabricExecutionOrderYarnReceive> FEOYSList { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchStringDate { get; set; }
        public string Params { get; set; }

        public string RequisitionByName { get; set; }
        public int RequisitionTypeInt { get; set; }
        public EnumInOutType RequisitionType { get; set; }
        public string IssueDateSt
        {
            get
            {
                if (this.IssueDate == DateTime.MinValue) return "";
                return this.IssueDate.ToString("dd MMM yyyy");
            }           
        }
        public string IssueDateTimeSt
        {
            get
            {
                if (this.IssueDate == DateTime.MinValue) return "";
                return this.IssueDate.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string ReceiveDateTimeSt
        {
            get
            {
                if (this.ReceiveDate == DateTime.MinValue) return "";
                if (this.ReceivedBy ==0) return "";
                return this.ReceiveDate.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string WarpWeftTypeSt
        {
            get
            {
                return EnumObject.jGet(this.WarpWeftType);
            }
        }
        public string RequisitionTypeSt
        {
            get
            {
                if (EnumInOutType.Receive == this.RequisitionType) return "SRS";
                else if (EnumInOutType.Disburse == this.RequisitionType) return "SRM";
                else return "-";
            }
        }
        #endregion

        #region Derived Property        

        public List<WYRequisition> WYRequisitions { get; set;}
        public string WYarnTypeStr { get { return EnumObject.jGet(this.WYarnType); } }
        #endregion

        #region Functions

        public static List<WYRequisition> BUWiseGets(int Buid, long nUserID)
        {
            return WYRequisition.Service.BUWiseGets(Buid, nUserID);
        }
   
        public static List<WYRequisition> GetsByName(string sName,  long nUserID)
        {
            return WYRequisition.Service.GetsByName(sName,  nUserID);
        }
   
        public WYRequisition Get(int id, long nUserID)
        {
            return WYRequisition.Service.Get(id, nUserID);
        }
    
        public WYRequisition Save(long nUserID)
        {
            return WYRequisition.Service.Save(this, nUserID);
        }
        public WYRequisition UndoApprove(EnumDBOperation eOpt, long nUserID)
        {
            return WYRequisition.Service.UndoApprove(this,eOpt, nUserID);
        }
        public WYRequisition Approve(long nUserID)
        {
            return WYRequisition.Service.Approve(this, nUserID);
        }
        public WYRequisition Disburse(long nUserID)
        {
            return WYRequisition.Service.Disburse(this, nUserID);
        }
           public WYRequisition Receive(long nUserID)
        {
            return WYRequisition.Service.Receive(this, nUserID);
        }
        public static List<WYRequisition> Gets(string sSQL, long nUserID)
        {
            return WYRequisition.Service.Gets(sSQL, nUserID);
        }
        public string Delete( long nUserID)
        {
            return WYRequisition.Service.Delete(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IWYRequisitionService Service
        {
            get { return (IWYRequisitionService)Services.Factory.CreateService(typeof(IWYRequisitionService)); }
        }

        #endregion


    }
    #endregion

    #region IWYRequisition interface
     
    public interface IWYRequisitionService
    {
        WYRequisition Get(int id, Int64 nUserID);
        List<WYRequisition> BUWiseGets(int buid, Int64 nUserID);
        List<WYRequisition> Gets(string sSQL, Int64 nUserID);
        string Delete(WYRequisition oWYRequisition, Int64 nUserID);
        WYRequisition Save(WYRequisition oWYRequisition, Int64 nUserID);
        WYRequisition Approve(WYRequisition oWYRequisition, Int64 nUserID);
        WYRequisition UndoApprove(WYRequisition oWYRequisition, EnumDBOperation eOpt, Int64 nUserID);
        WYRequisition Receive(WYRequisition oWYRequisition, Int64 nUserID);
        WYRequisition Disburse(WYRequisition oWYRequisition, Int64 nUserID);
        List<WYRequisition> GetsByName(string sName,  Int64 nUserID);
    }
    #endregion
}