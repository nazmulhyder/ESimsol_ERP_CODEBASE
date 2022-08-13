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
    #region DeliveryOrder
    public class DeliveryOrder : BusinessObject
    {
        public DeliveryOrder()
        {
            DeliveryOrderID = 0;
            DeliveryOrderLogID = 0;
            BUID  = 0;
            DONo = string.Empty;
            DODate = DateTime.Today;
            DOStatus = EnumDOStatus.Initialized;
            RefType = EnumRefType.ExportPI;
            RefID = 0;
            ContractorID = 0;
            DeliveryDate = DateTime.Today;
            DeliveryPoint = string.Empty;
            Note = string.Empty;
            ApproveBy = 0;
            ReviseNo = 0;
            RefNo = "";
            ErrorMessage = "";
            PrepareBy = 0;
            MDApproveBy = 0;
            MDApproveByName = "";
            ReceiveBy = 0;
            ReceiveByName = "";
            DeliveryToName = "";
            DeliveryToAddress = "";
            PrepareByName = "";
            IsNewVersion = false;
            BuyerID = 0;
            BuyerName = "";
            ExportLCNo = "";
            LCTermsName = "";
            ApprovedDate = DateTime.Now;
            ContractorContactPerson = 0;
            ProductNature = EnumProductNature.Hanger;
            DeliveryOrderDetails = new List<DeliveryOrderDetail>();
        }

        #region Property
        public int DeliveryOrderID {get; set;}
        public int DeliveryOrderLogID { get; set; }
        public int BUID {get; set;}
        public bool IsNewVersion { get; set; }
        public string DONo { get; set; }
        public DateTime DODate {get; set;}
        public EnumDOStatus DOStatus {get; set;}
        public int DOStatusInInt { get; set; }
        public EnumRefType RefType { get; set; }
        public int RefID {get; set;}
        public int ContractorID {get; set;}
        public DateTime DeliveryDate {get; set;}
        public string DeliveryPoint { get; set; }
        public string Note { get; set; }
        public int ApproveBy {get; set;}
        public int ReviseNo { get; set; }
        public string RefNo { get; set; }
        public int PrepareBy { get; set; }
        public int MDApproveBy { get; set; }
        public string MDApproveByName { get; set; }
        public int ReceiveBy { get; set; }
        public string ReceiveByName { get; set; }
        public DateTime ApprovedDate { get; set; }
        public EnumProductNature ProductNature { get; set; }
        public int ProductNatureInInt { get; set; }
        public string   DeliveryToName { get; set; }
        public string DeliveryToAddress { get; set; }
        public string PrepareByName { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public int ContractorContactPerson { get; set; }
        public string ExportLCNo { get; set; }
        public string LCTermsName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ApprovedDateInString
        {
            get
            {
                if(this.ApprovedDate==DateTime.MinValue)
                {
                    return "-";
                }else{
                    return this.ApprovedDate.ToString("dd MMM yyyy");
                }
                
            }
        }
        public List<DeliveryOrderDetail> DeliveryOrderDetails { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public ApprovalRequest ApprovalRequest { get; set; }
        public Company Company { get; set; }
        public string ActionTypeExtra { get; set; }
        public EnumDOActionType DOActionType { get; set; }
        public string ContractorName { get; set; }
        public string ApprovedByName { get; set; }
        public string BUName { get; set; }
        public double YetToDeliveryChallanQty { get; set; }
        

        public string DOStatusStr
        {
            get
            {
                return Global.EnumerationFormatter(this.DOStatus.ToString());
            }
        }
        public string RefTypeStr
        {
            get
            {
                return Global.EnumerationFormatter(this.RefType.ToString());
            }
        }
        public string DODateStr
        {
            get
            {
                return this.DODate.ToString("dd MMM yyyy");
            }
        }
        public string DeliveryDateStr
        {
            get
            {
                return this.DeliveryDate.ToString("dd MMM yyyy");
            }
        }

        #endregion

        #region Functions
       
        public DeliveryOrder Get(int id, long nUserID)
        {
            return DeliveryOrder.Service.Get(id, nUserID);
        }
        public static List<DeliveryOrder> Gets(string sSQL, long nUserID)
        {
            return DeliveryOrder.Service.Gets(sSQL, nUserID);
        }
        public DeliveryOrder IUD(short nDBOperation, long nUserID)
        {
            return DeliveryOrder.Service.IUD(this, nDBOperation, nUserID);
        }
        public DeliveryOrder AcceptRevise(short nDBOperation, long nUserID)
        {
            return DeliveryOrder.Service.AcceptRevise(this, nDBOperation, nUserID);
        }
        
        public DeliveryOrder Approve(long nUserID)
        {
            return DeliveryOrder.Service.Approve(this, nUserID);
        }
        public DeliveryOrder ChangeStatus(long nUserID)
        {
            return DeliveryOrder.Service.ChangeStatus(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDeliveryOrderService Service
        {
            get { return (IDeliveryOrderService)Services.Factory.CreateService(typeof(IDeliveryOrderService)); }
        }
        #endregion
    }
    #endregion

    #region IDeliveryOrder interface
    public interface IDeliveryOrderService
    {
        DeliveryOrder Get(int id, Int64 nUserID);
        List<DeliveryOrder> Gets(string sSQL, Int64 nUserID);
        DeliveryOrder IUD(DeliveryOrder oDeliveryOrder, short nDBOperation, Int64 nUserID);
        DeliveryOrder AcceptRevise(DeliveryOrder oDeliveryOrder, short nDBOperation, Int64 nUserID);

        DeliveryOrder Approve(DeliveryOrder oDeliveryOrder, Int64 nUserID);

        DeliveryOrder ChangeStatus(DeliveryOrder oDeliveryOrder, Int64 nUserID);
    }
    #endregion
}
