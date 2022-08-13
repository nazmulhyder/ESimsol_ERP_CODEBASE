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
    #region KnittingFabricReceive
    public class KnittingFabricReceive : BusinessObject
    {
        public KnittingFabricReceive()
        {
            KnittingFabricReceiveID = 0;
            KnittingOrderID = 0;
            ReceiveNo = "";
            ReceiveDate = DateTime.Now;
            PartyChallanNo = "";
            Remarks = "";
            ApprovedBy = 0;
            ApprovedByName = "";
            BUID = 0;
            ErrorMessage = "";
            KnittingOrderDate = DateTime.Now;
            KnittingOrderNo = "";
            BuyerName = "";
            StyleNo = "";
            KnittingOrderQty = 0;
            KnittingFabricReceiveDetails = new List<KnittingFabricReceiveDetail>();
            OrderType = EnumKnittingOrderType.None;
            StartDate = DateTime.Now;
            BusinessSessionName = "";
            FactoryName = "";
        }

        #region Property
        public int KnittingFabricReceiveID { get; set; }
        public int KnittingOrderID { get; set; }
        public string ReceiveNo { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string PartyChallanNo { get; set; }
        public string Remarks { get; set; }
        public int ApprovedBy { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string KnittingOrderNo { get; set; }
        public DateTime KnittingOrderDate { get; set; }
        public string BuyerName { get; set; }
        public string ApprovedByName { get; set; }
        public string StyleNo { get; set; }
        public double KnittingOrderQty { get; set; }
        public int BUID { get; set; }
        public string BusinessSessionName { get; set; }
        public string FactoryName { get; set; }
        public DateTime StartDate { get; set; }
        public EnumKnittingOrderType OrderType { get; set; }
        public List<KnittingFabricReceiveDetail> KnittingFabricReceiveDetails { get; set; }
        public string OrderTypeInString
        {
            get
            {
                return EnumObject.jGet(this.OrderType);
            }
        }
        public string StartDateInString
        {
            get
            {
                return StartDate.ToString("dd MMM yyyy");
            }
        }
        public string ReceiveDateInString
        {
            get
            {
                return ReceiveDate.ToString("dd MMM yyyy");
            }
        }
        public string KnittingOrderDateInString
        {
            get
            {
                return KnittingOrderDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<KnittingFabricReceive> Gets(long nUserID)
        {
            return KnittingFabricReceive.Service.Gets(nUserID);
        }
        public static List<KnittingFabricReceive> Gets(string sSQL, long nUserID)
        {
            return KnittingFabricReceive.Service.Gets(sSQL, nUserID);
        }
        public KnittingFabricReceive Get(int id, long nUserID)
        {
            return KnittingFabricReceive.Service.Get(id, nUserID);
        }
        public KnittingFabricReceive Save(long nUserID)
        {
            return KnittingFabricReceive.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return KnittingFabricReceive.Service.Delete(id, nUserID);
        }
        public KnittingFabricReceive Approve(long nUserID)
        {
            return KnittingFabricReceive.Service.Approve(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IKnittingFabricReceiveService Service
        {
            get { return (IKnittingFabricReceiveService)Services.Factory.CreateService(typeof(IKnittingFabricReceiveService)); }
        }
        #endregion
    }
    #endregion

    #region IKnittingFabricReceive interface
    public interface IKnittingFabricReceiveService
    {
        KnittingFabricReceive Get(int id, Int64 nUserID);
        List<KnittingFabricReceive> Gets(Int64 nUserID);
        List<KnittingFabricReceive> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        KnittingFabricReceive Save(KnittingFabricReceive oKnittingFabricReceive, Int64 nUserID);
        KnittingFabricReceive Approve(KnittingFabricReceive oKnittingFabricReceive, Int64 nUserID);
    }
    #endregion
}
