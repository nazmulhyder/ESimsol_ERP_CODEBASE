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
    #region AdjustmentRequisitionSlip
    
    public class AdjustmentRequisitionSlip : BusinessObject
    {
        #region  Constructor
        public AdjustmentRequisitionSlip()
        {
            AdjustmentRequisitionSlipID = 0;
            ARSlipNo = "";
            Date = DateTime.Now;
            RequestedByID = 0;
            RequestedTime = DateTime.Now;
            AprovedByID = 0;
            ApprovedTime = DateTime.Now;
            Note = "";
            WorkingUnitID = 0;
            InoutType = EnumInOutType.None;
            InoutTypeInInt = 0;
            AdjustmentType = EnumAdjustmentType.None;
            AdjustmentTypeInt = 0;
            IsWillVoucherEffect = true;
            BUID = 0;
            ErrorMessage = "";
        }
        #endregion

        #region Properties

        public int AdjustmentRequisitionSlipID { get; set; }
        public string ARSlipNo { get; set; }
        public DateTime Date { get; set; }
        public int BUID { get; set; }
        public int RequestedByID { get; set; }
        public DateTime RequestedTime { get; set; }
        public int AprovedByID { get; set; }
        public DateTime ApprovedTime { get; set; }
        public string Note { get; set; }
        public int WorkingUnitID { get; set; }
        public EnumAdjustmentType AdjustmentType { get; set; }
        public int AdjustmentTypeInt { get; set; }        
        public EnumInOutType InoutType { get; set; }
        public int InoutTypeInInt { get; set; }
        public bool IsWillVoucherEffect { get; set; }
        public string ErrorMessage { get; set; }
        #region derived
        public string PreaperByName { get; set; }
        public string RequestedByName { get; set; }
        public string AprovedByName { get; set; }
        public string WUName { get; set; }

        //public string OUShortName { get; set; }
        //public string LocationShortName { get; set; }

        //public string StoreShortName
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(this.LocationShortName))
        //        {
        //            if (!string.IsNullOrEmpty(this.OUShortName))
        //                return this.LocationShortName + "[" + this.OUShortName + "]";
        //            else
        //                return this.LocationShortName;
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(this.OUShortName))
        //                return "[" + this.OUShortName + "]";
        //            else
        //                return "";
        //        }
        //    }
        //}

        public string AdjustmentTypeSt
        {
            get
            {
                return EnumObject.jGet(this.AdjustmentType);
            }
        }
        public string InOutTypeSt
        {
            get
            {
                return InoutType.ToString();
            }
        }
        public string Status
        {
            get
            {
                if (this.RequestedByID > 0 && this.AprovedByID == 0)
                {
                    return "Requested";
                }
                else if (this.AprovedByID != 0)
                {
                    return "Accepted";
                }
                else //if(this.RequestedByEMPID <= 0 && this.AprovedByEmpID <= 0)
                {
                    return "Initialize";
                }
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
        #region SlipDetail
        public List<AdjustmentRequisitionSlipDetail> ARSDetails { get; set; }
        #endregion

        public string DateSt
        {
            get
            {
                return this.Date.ToString("dd MMM yyyy");
            }
        }
     
        #endregion

        #endregion

        #region Functions

        public static AdjustmentRequisitionSlip Get(int nId, long nUserID)
        {
            return AdjustmentRequisitionSlip.Service.Get(nId, nUserID);
        }
        public static List<AdjustmentRequisitionSlip> Gets(string sSQL, long nUserID)
        {
            return AdjustmentRequisitionSlip.Service.Gets(sSQL, nUserID);
        }        
        public AdjustmentRequisitionSlip Save(long nUserID)
        {
            return AdjustmentRequisitionSlip.Service.Save(this, nUserID);
        }
        public AdjustmentRequisitionSlip UpdateVoucherEffect(long nUserID)
        {
            return AdjustmentRequisitionSlip.Service.UpdateVoucherEffect(this, nUserID);
        }
        public AdjustmentRequisitionSlip Approve(long nUserID)
        {
            return AdjustmentRequisitionSlip.Service.Approve(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return AdjustmentRequisitionSlip.Service.Delete(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IAdjustmentRequisitionSlipService Service
        {
            get { return (IAdjustmentRequisitionSlipService)Services.Factory.CreateService(typeof(IAdjustmentRequisitionSlipService)); }
        }
        #endregion
    }
    #endregion

    #region IAdjustmentRequisitionSlip interface
    
    public interface IAdjustmentRequisitionSlipService
    {

        AdjustmentRequisitionSlip Get(int id, long nUserID);
        List<AdjustmentRequisitionSlip> Gets(string sSQL, long nUserID);       
        AdjustmentRequisitionSlip Save(AdjustmentRequisitionSlip oAdjustmentRequisitionSlip, long nUserID);
        AdjustmentRequisitionSlip Approve(AdjustmentRequisitionSlip oAdjustmentRequisitionSlip, long nUserID);
        string Delete(AdjustmentRequisitionSlip oAdjustmentRequisitionSlip, long nUserID);
        AdjustmentRequisitionSlip UpdateVoucherEffect(AdjustmentRequisitionSlip oAdjustmentRequisitionSlip, Int64 nUserID);   

    }
    #endregion
}
