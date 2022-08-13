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
    #region KnittingOrder
    public class KnittingOrder : BusinessObject
    {
        public KnittingOrder()
        {
            KnittingOrderID = 0;
            BusinessSessionID = 0;
            OrderNo = "";
            BUID = 0;
            OrderDate = DateTime.Now;
            YarnDeliveryStatus = EnumFinishYarnChallan.Yet_To_Start;
            FabricReceivedStatus = EnumFinishFabricReceive.Yet_To_Start;
            FactoryID = 0;
            StartDate = DateTime.Now;
            ApproxCompleteDate = DateTime.Now;
            ActualCompleteDate = DateTime.Now;
            CurrencyID = 0;
            Amount = 0;
            IssueQty = 0;
            ApprovedBy = 0;
            Remarks = "";
            KnittingInstruction = "";
            ErrorMessage = "";
            KnittingOrderDetails = new List<KnittingOrderDetail>();
            KnittingOrderTermsAndConditions = new List<KnittingOrderTermsAndCondition>();
            KnittingYarnChallanDetails = new List<KnittingYarnChallanDetail>();
            OrderType = EnumKnittingOrderType.None;
        }

        #region Property
        public int KnittingOrderID { get; set; }
        public int BusinessSessionID { get; set; }
        public string OrderNo { get; set; }
        public int BUID { get; set; }
        public DateTime OrderDate { get; set; }
        public EnumFinishYarnChallan YarnDeliveryStatus { get; set; }
        public EnumFinishFabricReceive FabricReceivedStatus { get; set; }
        public EnumKnittingOrderType OrderType { get; set; }
        public int FactoryID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ApproxCompleteDate { get; set; }
        public DateTime ActualCompleteDate { get; set; }
        public int CurrencyID { get; set; }
        public double Amount { get; set; }
        public double IssueQty { get; set; }
        public int ApprovedBy { get; set; }
        public string Remarks { get; set; }
        public string KnittingInstruction { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string BusinessSessionName { get; set; }
        public string FactoryName { get; set; }
        public string CurrencyName { get; set; }
        public string ApprovedByName { get; set; }
        public int ReportLayout { get; set; }
        public string Params { get; set; }
        public string OrderTypeInString
        {
            get
            {
                return EnumObject.jGet(this.OrderType);
            }
        }
        public string AmountInString
        {
            get
            {
                return Amount.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string IssueQtyInString
        {
            get
            {
                return IssueQty.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string OrderDateInString
        {
            get
            {
                return OrderDate.ToString("dd MMM yyyy");
            }
        }
        public string StartDateInString
        {
            get
            {
                return StartDate.ToString("dd MMM yyyy");
            }
        }
        public string ApproxCompleteDateInString
        {
            get
            {
                return ApproxCompleteDate.ToString("dd MMM yyyy");
            }
        }
        public string ActualCompleteDateInString
        {
            get
            {
                return ActualCompleteDate.ToString("dd MMM yyyy");
            }
        }
        public string YarnDeliveryStatusInString
        {
            get
            {
                return EnumObject.jGet(this.YarnDeliveryStatus);
            }
        }
        public string FabricReceivedStatusInString
        {
            get
            {
                return EnumObject.jGet(this.FabricReceivedStatus);
            }
        }
        public List<KnittingOrderDetail> KnittingOrderDetails { get; set; }
        public List<KnittingOrderTermsAndCondition> KnittingOrderTermsAndConditions { get; set; }
        public List<KnittingYarnChallanDetail> KnittingYarnChallanDetails { get; set; }
        #endregion

        #region Functions
        public static List<KnittingOrder> Gets(long nUserID)
        {
            return KnittingOrder.Service.Gets(nUserID);
        }
        public static List<KnittingOrder> Gets(string sSQL, long nUserID)
        {
            return KnittingOrder.Service.Gets(sSQL, nUserID);
        }
        public KnittingOrder Get(int id, long nUserID)
        {
            return KnittingOrder.Service.Get(id, nUserID);
        }
        public KnittingOrder Save(long nUserID)
        {
            return KnittingOrder.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return KnittingOrder.Service.Delete(id, nUserID);
        }
        public KnittingOrder Approve(long nUserID)
        {
            return KnittingOrder.Service.Approve(this, nUserID);
        }
        public KnittingOrder UnApprove(long nUserID)
        {
            return KnittingOrder.Service.UnApprove(this, nUserID);
        }
        public KnittingOrder FinishYarnChallan(long nUserID)
        {
            return KnittingOrder.Service.FinishYarnChallan(this, nUserID);
        }
        public KnittingOrder FinishFabricReceive(long nUserID)
        {
            return KnittingOrder.Service.FinishFabricReceive(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IKnittingOrderService Service
        {
            get { return (IKnittingOrderService)Services.Factory.CreateService(typeof(IKnittingOrderService)); }
        }
        #endregion
    }
    #endregion

    #region IKnittingOrder interface
    public interface IKnittingOrderService
    {
        KnittingOrder Get(int id, Int64 nUserID);
        List<KnittingOrder> Gets(Int64 nUserID);
        List<KnittingOrder> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        KnittingOrder Save(KnittingOrder oKnittingOrder, Int64 nUserID);
        KnittingOrder Approve(KnittingOrder oKnittingOrder, Int64 nUserID);
        KnittingOrder UnApprove(KnittingOrder oKnittingOrder, Int64 nUserID);
        KnittingOrder FinishYarnChallan(KnittingOrder oKnittingOrder, Int64 nUserID);
        KnittingOrder FinishFabricReceive(KnittingOrder oKnittingOrder, Int64 nUserID);
    }
    #endregion  
}
