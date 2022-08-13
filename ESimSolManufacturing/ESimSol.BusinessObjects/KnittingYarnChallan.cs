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
    #region KnittingYarnChallan
    public class KnittingYarnChallan : BusinessObject
    {
        public KnittingYarnChallan()
        {
            KnittingYarnChallanID = 0;
            KnittingOrderID = 0;
            ChallanNo = "";
            ChallanDate = DateTime.Now;
            CarNumber = "";
            DriverName = "";
            DeliveryPoint = "";
            Remarks = "";
            ApprovedBy = 0;
            ErrorMessage = "";
            KnittingOrderDate = DateTime.Now;
            KnittingOrderNo = "";
            BuyerName = "";
            StyleNo =  "";
            KnittingOrderQty = 0;
            KnittingYarnChallanDetails = new List<KnittingYarnChallanDetail>();
        }

        #region Property
        public int KnittingYarnChallanID { get; set; }
        public int KnittingOrderID { get; set; }
        public string ChallanNo { get; set; }
        public DateTime KnittingOrderDate { get; set; }
        public string BuyerName { get; set; }
        public string StyleNo { get; set; }
        public double KnittingOrderQty { get; set; }
        public DateTime ChallanDate { get; set; }
        public string DriverName { get; set; }
        public string CarNumber { get; set; }
        public string DeliveryPoint { get; set; }
        public string Remarks { get; set; }
        public int ApprovedBy { get; set; }
        public string ErrorMessage { get; set; }
        public string KnittingOrderNo { get; set; }
        public string ApproveUser { get; set; }
        #endregion
        
        #region Derived Property
        public int BUID { get; set; }
        public double OrderQty { get; set; }
        public string FactoryName { get; set; }
        public int PAM { get; set; }
        public List<KnittingYarnChallanDetail> KnittingYarnChallanDetails { get; set; }
        public string ChallanDateInString
        {
            get
            {
                return ChallanDate.ToString("dd MMM yyyy");
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
        public static List<KnittingYarnChallan> Gets(long nUserID)
        {
            return KnittingYarnChallan.Service.Gets(nUserID);
        }
        public static List<KnittingYarnChallan> Gets(string sSQL, long nUserID)
        {
            return KnittingYarnChallan.Service.Gets(sSQL, nUserID);
        }
        public KnittingYarnChallan Get(int id, long nUserID)
        {
            return KnittingYarnChallan.Service.Get(id, nUserID);
        }
        public KnittingYarnChallan Save(long nUserID)
        {
            return KnittingYarnChallan.Service.Save(this, nUserID);
        }
        public KnittingYarnChallan Approve(long nUserID)
        {
            return KnittingYarnChallan.Service.Approve(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return KnittingYarnChallan.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IKnittingYarnChallanService Service
        {
            get { return (IKnittingYarnChallanService)Services.Factory.CreateService(typeof(IKnittingYarnChallanService)); }
        }
        #endregion
    }
    #endregion

    #region IKnittingYarnChallan interface
    public interface IKnittingYarnChallanService
    {
        KnittingYarnChallan Get(int id, Int64 nUserID);
        List<KnittingYarnChallan> Gets(Int64 nUserID);
        List<KnittingYarnChallan> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        KnittingYarnChallan Save(KnittingYarnChallan oKnittingYarnChallan, Int64 nUserID);
        KnittingYarnChallan Approve(KnittingYarnChallan oKnittingYarnChallan, Int64 nUserID);
    }
    #endregion
}
