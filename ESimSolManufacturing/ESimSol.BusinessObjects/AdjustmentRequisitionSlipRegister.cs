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
    #region AdjustmentRequisitionSlipRegister
    public class AdjustmentRequisitionSlipRegister : BusinessObject
    {
        public AdjustmentRequisitionSlipRegister()
        {
            AdjustmentRequisitionSlipDetailID = 0;
            AdjustmentRequisitionSlipID = 0;
            ProductID = 0;
            MUnitID = 0;
            Qty = 0;
            ARSlipNo = "";
            BUID = 0;
            RequestedByID = 0;
            WorkingUnitID = 0;
            AdjustmentType = EnumAdjustmentType.None;
            AprovedByID = 0;
            ApproveDate = DateTime.MinValue;
            RequestedDate = DateTime.MinValue;  
            RequestByName = "";
            Remarks = "";
            ApproveByName = "";
            ProductCode = "";
            ProductName = "";
            MUName = "";
            MUSymbol = "";
            WUName = "";
            LotNo = "";
            ErrorMessage = "";
            SearchingData = "";
            InOutType = EnumInOutType.None;
            ProductCategoryID = 0;
            ReportLayout = EnumReportLayout.None;
        }

        #region Properties
        public int AdjustmentRequisitionSlipDetailID { get; set; }
        public int AdjustmentRequisitionSlipID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public string ARSlipNo { get; set; }
        public int BUID { get; set; }
        public DateTime Date { get; set; }
        public int RequestedByID { get; set; }
        public int WorkingUnitID { get; set; }
        public EnumAdjustmentType AdjustmentType { get; set; }
        public int AprovedByID { get; set; }
        public DateTime ApproveDate { get; set; }
        public DateTime RequestedDate { get; set; }
        public string RequestByName { get; set; }   
        public string Remarks { get; set; }
        public string ApproveByName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }
        public string LotNo { get; set; }
        public string WUName { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchingData { get; set; }
        public int ProductCategoryID { get; set; }
        public EnumInOutType InOutType { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        #endregion

        #region Derived Property
        public string RequestedDateTimeInString
        {
            get
            {
                return this.RequestedDate.ToString("dd MMM yyyy hh:mm tt");
            }
        }

        public string RequestedTimeInString
      {
          get
          {
              return this.RequestedDate.ToString("hh:mm tt");
          }
      }

      public string DateSt
        {
            get 
            {
                if (this.Date == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.Date.ToString("dd MMM yyyy hh:mm tt");
                }
            }
        }
        public string ApproveDateSt
        {
            get
            {
                if (this.ApproveDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ApproveDate.ToString("dd MMM yyyy");
                }
            }
        }
      
  
        public string AdjustmentRequisitionSlipTypeSt
        {
            get
            {
                return EnumObject.jGet(this.AdjustmentType);
            }
        }
        
        #endregion

        #region Functions
        public static List<AdjustmentRequisitionSlipRegister> Gets(string sSQL, long nUserID)
        {
            return AdjustmentRequisitionSlipRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IAdjustmentRequisitionSlipRegisterService Service
        {
            get { return (IAdjustmentRequisitionSlipRegisterService)Services.Factory.CreateService(typeof(IAdjustmentRequisitionSlipRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IAdjustmentRequisitionSlipRegister interface

    public interface IAdjustmentRequisitionSlipRegisterService
    {
        List<AdjustmentRequisitionSlipRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
