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
    #region ConsumptionRequisition

    public class ConsumptionRequisition : BusinessObject
    {
        public ConsumptionRequisition()
        {
            ConsumptionRequisitionID = 0;
            RequisitionNo = "";
            BUID = 0;
            RefNo = "";
            CRType = EnumConsumptionType.GeneralConsumption;
            CRTypeInt = 0;
            RequisitionBy = 0;
            CRStatus = EnumCRStatus.Initiallize;
            CRStatusInt = 0;
            IssueDate = DateTime.Today;
            RequisitionFor = 0;
            StoreID = 0;
            Remarks = "";
            DeliveryBy = 0;
            ApprovedBy = 0;
            StoreCode = "";
            StoreName = "";
            RequisitionByName = "";
            ApprovedByName = "";
            DeliveryByName = "";
            RequisitionForName = "";
            Amount = 0;
            ConsumptionRequisitionLogID = 0;
            Shift = EnumShift.None;
            SubLedgerID = 0;
            SubLedgerName = "";
            ErrorMessage = "";
            IsWillVoucherEffect = true;
            ConsumptionRequisitionDetails = new List<ConsumptionRequisitionDetail>();
            Company = new Company();
            BusinessUnit = new BusinessUnit();
            ClientOperationSetting = new ClientOperationSetting();
            RefType = EnumCRRefType.None;
            RefTypeInt = 0;
            RefObjID = 0;
            RefObjNo = "";


            FabricSalesContractID = 0;
            FabricSalesContractDetailID = 0;
            SCNoFull = "";
            ExeNoFull = "";
            SCDate = DateTime.MinValue;
            BuyerName = "";
            Qty = 0;
            ColorInfo = "";
            Construction = "";
            ProductName = "";
            FabricNo = "";
            OrderName = "";

        }

        #region Properties

        public int FabricSalesContractID { get; set; }
        public int FabricSalesContractDetailID { get; set; }
        public string SCNoFull { get; set; }
        public string ExeNoFull { get; set; }
        public string BuyerName { get; set; }
        public string OrderName { get; set; }
        public string ColorInfo { get; set; }
        public string Construction { get; set; }
        public string ProductName { get; set; }
        public string FabricNo { get; set; }
        public double Qty { get; set; }
        public DateTime SCDate { get; set; }



        public int ConsumptionRequisitionID { get; set; }
        public int RefObjID { get; set; }
        public string RequisitionNo { get; set; }
        public int BUID { get; set; }
        public string RefNo { get; set; }
        public EnumConsumptionType CRType { get; set; }
        public EnumCRRefType RefType { get; set; }
        public int CRTypeInt { get; set; }
        public int RefTypeInt { get; set; }
        public int RequisitionBy { get; set; }
        public EnumCRStatus CRStatus { get; set; }
        public int CRStatusInt { get; set; }
        public DateTime IssueDate { get; set; }
        public int RequisitionFor { get; set; }
        public int StoreID { get; set; }
        public string Remarks { get; set; }
        public int DeliveryBy { get; set; }
        public int ApprovedBy { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string RequisitionByName { get; set; }
        public string ApprovedByName { get; set; }
        public string DeliveryByName { get; set; }
        public string RequisitionForName { get; set; }
        public double Amount { get; set; }
        public int ConsumptionRequisitionLogID { get; set; }
        public string ActionTypeExtra { get; set; }
        public EnumCRActionType CRActionType { get; set; }
        public int CRActionTypeInt { get; set; }
        public EnumShift Shift { get; set; }
        public int ShiftInInt { get; set; }
        public ClientOperationSetting ClientOperationSetting { get; set; }
        public int SubLedgerID { get; set; }
        public string SubLedgerName { get; set; }
        public bool IsWillVoucherEffect { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string RefObjNo { get; set; }
        public string RefTypeSt
        {
            get
            {
                return EnumObject.jGet(this.RefType);
            }
        }
        public string ShiftSt
        {
            get
            {
                return EnumObject.jGet(this.Shift);
            }
        }
        public string CRStatusSt
        {
            get
            {
                return EnumObject.jGet(this.CRStatus);
            }
        }
        public string CRTypeSt
        {
            get
            {
                return EnumObject.jGet(this.CRType);
            }
        }

        public string SCDateSt
        {
            get
            {
                return (this.SCDate == DateTime.MinValue)?"-":this.SCDate.ToString("dd MMM yyyy");
            }
        }
        public string IssueDateSt
        {
            get
            {
                return this.IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string RefWithRefObjNo
        {
            get
            {
                return (this.RefType == EnumCRRefType.None) ? "" : this.RefTypeSt + "-" + this.RefObjNo;
            }
        }
        public string IsWillVoucherEffectSt
        {
            get
            {
                if (this.IsWillVoucherEffect)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public List<ConsumptionRequisition> ConsumptionRequisitions { get; set; }
        public List<ConsumptionRequisitionDetail> ConsumptionRequisitionDetails { get; set; }
        public List<User> Users { get; set; }
        public Company Company { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        #endregion

        #region Functions
        public static List<ConsumptionRequisition> Gets(long nUserID)
        {
            return ConsumptionRequisition.Service.Gets(nUserID);
        }
        public static List<ConsumptionRequisition> Gets(string sSQL, long nUserID)
        {
            return ConsumptionRequisition.Service.Gets(sSQL, nUserID);
        }
        public ConsumptionRequisition Get(int id, long nUserID)
        {
            return ConsumptionRequisition.Service.Get(id, nUserID);
        }
        public ConsumptionRequisition GetLog(int id, long nUserID)
        {
            return ConsumptionRequisition.Service.GetLog(id, nUserID);
        }
        public ConsumptionRequisition ChangeStatus(long nUserID)
        {
            return ConsumptionRequisition.Service.ChangeStatus(this, nUserID);
        }
        public ConsumptionRequisition Save(long nUserID)
        {
            return ConsumptionRequisition.Service.Save(this, nUserID);
        }
        public ConsumptionRequisition Delivery(long nUserID)
        {
            return ConsumptionRequisition.Service.Delivery(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return ConsumptionRequisition.Service.Delete(id, nUserID);
        }
        public ConsumptionRequisition AcceptConsumptionRequisitionRevise(long nUserID)
        {
            return ConsumptionRequisition.Service.AcceptConsumptionRequisitionRevise(this, nUserID);
        }
        public ConsumptionRequisition UpdateVoucherEffect(long nUserID)
        {
            return ConsumptionRequisition.Service.UpdateVoucherEffect(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IConsumptionRequisitionService Service
        {
            get { return (IConsumptionRequisitionService)Services.Factory.CreateService(typeof(IConsumptionRequisitionService)); }
        }
        #endregion
    }
    #endregion


    #region IConsumptionRequisition interface

    public interface IConsumptionRequisitionService
    {
        ConsumptionRequisition Get(int id, Int64 nUserID);
        ConsumptionRequisition GetLog(int id, Int64 nUserID);
        List<ConsumptionRequisition> Gets(Int64 nUserID);
        List<ConsumptionRequisition> Gets(string sSQL, Int64 nUserID);
        ConsumptionRequisition ChangeStatus(ConsumptionRequisition oConsumptionRequisition, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        ConsumptionRequisition AcceptConsumptionRequisitionRevise(ConsumptionRequisition oConsumptionRequisition, Int64 nUserID);
        ConsumptionRequisition Save(ConsumptionRequisition oConsumptionRequisition, Int64 nUserID);
        ConsumptionRequisition Delivery(ConsumptionRequisition oConsumptionRequisition, Int64 nUserID);
        ConsumptionRequisition UpdateVoucherEffect(ConsumptionRequisition oConsumptionRequisition, Int64 nUserID);   
    }
    #endregion
}