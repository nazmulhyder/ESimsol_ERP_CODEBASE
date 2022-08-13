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
    #region SUDeliveryChallan
    public class SUDeliveryChallan : BusinessObject
    {
        public SUDeliveryChallan()
        {
            SUDeliveryChallanID = 0;
            SUDeliveryOrderID = 0;
            ChallanNo = "";
            ChallanStatus = EnumDeliveryChallan.Initialize;
            ChallanStatusInInt = (int)EnumDeliveryChallan.Initialize;
            ChallanDate = DateTime.Now;
            BuyerID = 0;
            DeliveryTo = 0;
            CheckedBy = 0;
            VehicleNo = "";
            Remarks = "";
            DriverName = "";
            DriverContactNo = "";
            GatePassNo = "";
            StorePhoneNo = "";
            ApprovedBy = 0;
            DeliveredBy = 0;
            BuyerName = "";
            BuyerShortName = "";
            DeliveryToName = "";
            ApprovedByName = "";
            DeliveredByName = "";
            Qty = 0;
            StoreID = 0;

            DONo = "";
            PINo = "";
            DOType = 0;
            DODate = DateTime.Now;
            WUName = "";
            DeliveryPoint = "";

            ErrorMessage = "";
            SUDeliveryChallanDetails = new List<SUDeliveryChallanDetail>();

            BuyerContractorType = (int)EnumContractorType.None;
            DeliveryToContractorType = (int)EnumContractorType.None;
            ContactPersonnelName = "";
            ContactPersonnelPhone = "";

            DBUserName = "";
            TotalAttachment = 0;

            Params = "";
            SUDeliveryChallanIDs = "";
            ReturnQty=0;
            IsUnitPriceAndAmount = false;

            DEONo = "";
            FEONo = "";
            FEOID = 0;
            IsInHouse = true;
            OrderType = EnumOrderType.None;
            CountDetail = 0;
            CountYarnRcv = 0;
            LCNo = "";
            DExeNo = string.Empty;
        }

        #region Properties
        public int SUDeliveryChallanID { get; set; }
        public int SUDeliveryOrderID { get; set; }
        public string ChallanNo { get; set; }
        public EnumDeliveryChallan ChallanStatus { get; set; }
        public int ChallanStatusInInt { get; set; }
        public DateTime ChallanDate { get; set; }
        public int BuyerID { get; set; }
        public int DeliveryTo { get; set; }
        public int CheckedBy { get; set; }
        public string VehicleNo { get; set; }
        public string Remarks { get; set; }
        public string DriverName { get; set; }
        public string DriverContactNo { get; set; }
        public string GatePassNo { get; set; }
        public string StorePhoneNo { get; set; }
        public int ApprovedBy { get; set; }
        public int DeliveredBy { get; set; }
        public string BuyerName { get; set; }
        public string BuyerShortName { get; set; }
        public string DeliveryToName { get; set; }
        public string ApprovedByName { get; set; }
        public string DeliveredByName { get; set; }
        public double Qty { get; set; }
        public string DONo { get; set; }
        public string PINo { get; set; }
        public int DOType { get; set; }
        public DateTime DODate { get; set; }
        public string ErrorMessage { get; set; }
        public int BuyerContractorType { get; set; }
        public int DeliveryToContractorType { get; set; }
        public int StoreID { get; set; }
        public string WUName { get; set; }
        public string DeliveryPoint { get; set; }
        public string ContactPersonnelName { get; set; }
        public string ContactPersonnelPhone { get; set; }
        public string DBUserName { get; set; }
        public int TotalAttachment { get; set; }
        public double ReturnQty { get; set; }
        public bool IsUnitPriceAndAmount { get; set; }
        public string DEONo { get; set; }
        public string FEONo { get; set; }
        public int FEOID { get; set; }
        public bool IsInHouse { get; set; }
        public EnumOrderType OrderType { get; set; }
        public int CountDetail { get; set; }
        public int CountYarnRcv { get; set; }
        public string LCNo { get; set; }

        #endregion

        #region Derived Property 
        public string DExeNo { get; set; }
        public string Params { get; set; }
        public string SUDeliveryChallanIDs { get; set; }
        public string OrderNo
        {
            get
            {
                string sPrifix = "";
                if (this.FEOID > 0)
                {
                    //if (this.IsInHouse) { sPrifix = "EXE"; } else sPrifix = "SCW";
                    //if (this.OrderType == EnumOrderType.Bulk) { sPrifix = sPrifix + "-BLK-"; }
                    //else if (this.OrderType == EnumOrderType.Development) { sPrifix = sPrifix + "-DEV-"; }
                    //else if (this.OrderType == EnumOrderType.SMS) { sPrifix = sPrifix + "-SMS-"; }
                    //else { sPrifix = sPrifix + "-"; }
                    return sPrifix + this.FEONo;
                }
                else return "";
            }
        }
        public bool IsReceiveOrNot
        {
            get
            {
                if (this.CountDetail == this.CountYarnRcv)
                {
                    return true;
                }
                return false;
            }
        }
        public string ReturnQtySt
        {
            get
            {
                return Global.MillionFormat(this.ReturnQty);
            }
        }
        public string TotalAttachmentWithDcID
        {
            get
            {
                return this.TotalAttachment + "~" + this.SUDeliveryChallanID;
            }
        }
        public Company Company { get; set; }
        public List<SUDeliveryChallan> SUDeliveryChallans { get; set; }
        public List<SUDeliveryChallanDetail> SUDeliveryChallanDetails { get; set; }
        public string DODateSt
        {
            get
            {
                return this.DODate.ToString("dd MMM yyyy");
            }
        }
        public string ChallanDateSt
        {
            get
            {
                return this.ChallanDate.ToString("dd MMM yyyy");
            }
        }
        public string ChallanStatusSt
        {
            get
            {
                
                return  EnumObject.jGet(this.ChallanStatus);
            }
        }
        public string QtySt
        {
            get
            {
                return Global.MillionFormat(this.Qty);
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// added by fahim0abir on date: 4 AUG 2015
        /// for geting list of challans of a specific SUDeliveryOrder
        /// </summary>
        /// <param name="nUserID"></param>
        /// <returns></returns>
        public static List<SUDeliveryChallan> GetsBySUDeliveryOrder(int nSUDOID, long nUserID)
        {
            return SUDeliveryChallan.Service.GetsBySUDeliveryOrder(nSUDOID, nUserID);
        }
        /// <summary>
        /// added by fahim0abir on date: 30 JUL 2015
        /// for geting list of challans that have ChallanStatus=EnumDeliveryChallan.Approve(1) aka pending challan
        /// </summary>
        /// <returns></returns>
        public static List<SUDeliveryChallan> GetsPendingChallan(long nUserID)
        {
            return SUDeliveryChallan.Service.GetsPendingChallan(nUserID);
        }
        public static List<SUDeliveryChallan> Gets(long nUserID)
        {
            return SUDeliveryChallan.Service.Gets(nUserID);
        }
        public static List<SUDeliveryChallan> Gets(string sSQL, long nUserID)
        {
            return SUDeliveryChallan.Service.Gets(sSQL, nUserID);
        }
        public SUDeliveryChallan Get(int nSUDeliveryChallanId, long nUserID)
        {
            return SUDeliveryChallan.Service.Get(nSUDeliveryChallanId, nUserID);
        }
        public SUDeliveryChallan UpdateStatus(int SUDeliveryChallanID, int nNewStatus, long nUserID)
        {
            return SUDeliveryChallan.Service.UpdateStatus(SUDeliveryChallanID, nNewStatus, nUserID);
        }
        public SUDeliveryChallan Save(long nUserID)
        {
            return SUDeliveryChallan.Service.Save(this, nUserID);
        }
        public string Delete(int nSUDeliveryChallanId, long nUserID)
        {
            return SUDeliveryChallan.Service.Delete(nSUDeliveryChallanId, nUserID);
        }
        public SUDeliveryChallan SUDeliveryChallanDisburse(int nSUDeliveryChallanId, long nUserID)
        {
            return SUDeliveryChallan.Service.SUDeliveryChallanDisburse(nSUDeliveryChallanId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ISUDeliveryChallanService Service
        {
            get { return (ISUDeliveryChallanService)Services.Factory.CreateService(typeof(ISUDeliveryChallanService)); }
        }
        #endregion
    }
    #endregion

    #region ISUDeliveryChallan interface
    public interface ISUDeliveryChallanService
    {
        List<SUDeliveryChallan> GetsBySUDeliveryOrder(int nSUDOID, long nUserID);
        List<SUDeliveryChallan> GetsPendingChallan(long nUserID);
        List<SUDeliveryChallan> Gets(long nUserID);
        List<SUDeliveryChallan> Gets(string sSQL, long nUserID);
        SUDeliveryChallan Get(int nSUDeliveryChallanId, long nUserID);
        SUDeliveryChallan UpdateStatus(int SUDeliveryChallanID, int nNewStatus, long nUserID);
        SUDeliveryChallan Save(SUDeliveryChallan oSUDeliveryChallan, long nUserID);
        string Delete(int nSUDeliveryChallanId, long nUserID);
        SUDeliveryChallan SUDeliveryChallanDisburse(int nSUDeliveryChallanId, long nUserID);
    }
    #endregion
}
