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
    #region DUClaimOrder
    
    public class DUClaimOrder : BusinessObject
    {
        #region  Constructor
        public DUClaimOrder()
        {
            DUClaimOrderID = 0;
            ClaimOrderNo = "";
            OrderDate = DateTime.Now;
            CheckedDate = DateTime.Now;
            CheckedBy = 0;
            ApproveDate = DateTime.Now;
            Note = "";
            ApproveBy = 0;
            IsRevise = false;
            ErrorMessage = "";
            OrderType = 0;
            PaymentType = 0;
            ParentDOID = 0;
            ContactPersonnelName = "";
            DeliveryZone = "";
            StyleNo = "";
            RefNo = "";
            DUReturnChallanNo = "";
            DUReturnChallanID = 0;
            IsClose = false;
            StatusDo = EnumDyeingOrderState.None;
        }
        #endregion

        #region Properties

        public int DUClaimOrderID { get; set; }
        public string ClaimOrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public int BUID { get; set; }
        public int OrderType { get; set; }
        public int ClaimType { get; set; }
        public int CheckedBy { get; set; }
        public DateTime CheckedDate { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public string Note { get; set; }
        public string Note_Checked { get; set; }
        public string Note_Approve { get; set; }
        public double Qty { get; set; }
        public int PaymentType { get; set; }
        public string ErrorMessage { get; set; }
        #region derived
        public int DyeingOrderID { get; set; }
        public int ParentDOID { get; set; }
        public int ExportPIID { get; set; }
         public int DUReturnChallanID { get; set; }
        public string ParentDONo { get; set; }
        public string DUReturnChallanNo { get; set; }
        public string PINo { get; set; }
        public string LCNo { get; set; }
        public string ContractorName { get; set; }
        public string PreaperByName { get; set; }
        public string CheckedByName { get; set; }
        public string ApproveByName { get; set; }
        public int ContractorID { get; set; }
        public bool IsRevise { get; set; }
        public bool IsClose { get; set; }
        public EnumDyeingOrderState StatusDo { get; set; }
        private string sStatus = "";
        public string Status
        {
            get
            {
                if (this.CheckedBy == 0 && this.ApproveBy == 0)
                {
                    sStatus= "Initialize";
                }
                if (this.CheckedBy != 0 && this.ApproveBy == 0)
                {
                    sStatus= "Verified";
                }
                else if (this.CheckedBy != 0 && this.ApproveBy!= 0)
                {
                    sStatus= "Accepted";
                }
                else 
                {
                    sStatus= "Initialize";
                }
                if (this.StatusDo == EnumDyeingOrderState.Cancelled)
                {
                    sStatus = this.StatusDo.ToString();
                }
                return sStatus;
            }
        }
        public string RefNo { get; set; }
        public string StyleNo { get; set; }
        public string ContactPersonnelName { get; set; }
        public string DeliveryZone { get; set; }
        #region SlipDetail
        public List<DUClaimOrderDetail> DUClaimOrderDetails { get; set; }
        #endregion
        public string IsCloseSt
        {
            get
            {
                if (this.IsClose == true) return "Close";
                else if (this.IsClose == false) return "Running ";
                else return "-";
            }
        }

        public string OrderDateSt
        {
            get
            {
                return this.OrderDate.ToString("dd MMM yyyy");
            }
        }
        public string ClaimTypeSt
        {
            get
            {
                return ((EnumClaimOrderType)this.ClaimType).ToString();
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

        #endregion

        #region Functions

        public  DUClaimOrder Get(int nId, long nUserID)
        {
            return DUClaimOrder.Service.Get(nId, nUserID);
        }
        public static List<DUClaimOrder> Gets(string sSQL, long nUserID)
        {
            return DUClaimOrder.Service.Gets(sSQL, nUserID);
        }

        public static List<DUClaimOrder> GetsBy(string sContractorID, long nUserID)
        {
            return DUClaimOrder.Service.GetsBy(sContractorID, nUserID);
        }
        public static List<DUClaimOrder> GetsByPI(int nExportPIID, long nUserID)
        {
            return DUClaimOrder.Service.GetsByPI(nExportPIID, nUserID);
        }
        public DUClaimOrder Save(long nUserID)
        {
            return DUClaimOrder.Service.Save(this, nUserID);
        }
        public DUClaimOrder Save_Log(long nUserID)
        {
            return DUClaimOrder.Service.Save_Log(this, nUserID);
        }

        public DUClaimOrder Checked(long nUserID)
        {
            return DUClaimOrder.Service.Checked(this, nUserID);
        }
        public DUClaimOrder Approve(long nUserID)
        {
            return DUClaimOrder.Service.Approve(this, nUserID);
        }
        public DUClaimOrder DOSave_Auto(long nUserID)
        {
            return DUClaimOrder.Service.DOSave_Auto(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return DUClaimOrder.Service.Delete(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDUClaimOrderService Service
        {
            get { return (IDUClaimOrderService)Services.Factory.CreateService(typeof(IDUClaimOrderService)); }
        }
        #endregion
    }
    #endregion

    #region IDUClaimOrder interface
    
    public interface IDUClaimOrderService
    {
        DUClaimOrder Get(int id, long nUserID);
        List<DUClaimOrder> Gets(string sSQL, long nUserID);
        List<DUClaimOrder> GetsBy(string sContractorIDs, long nUserID);
        List<DUClaimOrder> GetsByPI(int nExportPIID, long nUserID);
        DUClaimOrder Save(DUClaimOrder oDUClaimOrder, long nUserID);
        DUClaimOrder Save_Log(DUClaimOrder oDUClaimOrder, long nUserID);
        DUClaimOrder Checked(DUClaimOrder oDUClaimOrder, long nUserID);
        DUClaimOrder Approve(DUClaimOrder oDUClaimOrder, long nUserID);
        DUClaimOrder DOSave_Auto(DUClaimOrder oDyeingOrder, long nUserID);
        string Delete(DUClaimOrder oDUClaimOrder, long nUserID);

    }
    #endregion
}
