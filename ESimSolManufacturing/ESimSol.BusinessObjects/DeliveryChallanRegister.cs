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
    #region DeliveryChallanRegister
    public class DeliveryChallanRegister : BusinessObject
    {
        public DeliveryChallanRegister()
        {
            DeliveryChallanDetailID = 0;
            DeliveryChallanID = 0;
            ProductID = 0;
            MUnitID = 0;
            Qty = 0;
            ChallanNo = "";
            BUID = 0;
            ChallanDate = DateTime.MinValue;
            ChallanStatus = EnumChallanStatus.Initialized;
            ContractorID = 0;
            ContactPersonnelID = 0;
            DeliveryOrderID = 0;
            WorkingUnitID = 0;
            StoreInchargeID = 0;
            ChallanType = EnumChallanType.Regular;
            ApproveBy = 0;
            ApproveDate = DateTime.MinValue;
            ValidityDate = DateTime.MinValue;
            DeliveryToAddress = "";
            ReceivedByName = "";
            Remarks = "";
            ApproveByName = "";
            ProductCode = "";
            ProductName = "";
            MUName = "";
            MUSymbol = "";
            ContractorName = "";
            SCPName = "";
            CPName = "";
            VehicleNo = "";
            VehicleName = "";
            GatePassNo = "";
            DONo = "";
            PINo = "";
            ExportLCNo = "";
            ErrorMessage = "";
            SearchingData = "";
            ProductCategoryID = 0;
            VehicleDateTime = DateTime.Now;
            ReportLayout = EnumReportLayout.None;
            SalePrice = 0;
            RateUnit = 0;

        }

        #region Properties
        public int DeliveryChallanDetailID { get; set; }
        public int DeliveryChallanID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public string ChallanNo { get; set; }
        public int BUID { get; set; }
        public DateTime ChallanDate { get; set; }
        public EnumChallanStatus ChallanStatus { get; set; }
        public int ChallanStatusInInt { get; set; }
        public int ContractorID { get; set; }
        public int ContactPersonnelID { get; set; }
        public int DeliveryOrderID { get; set; }
        public int WorkingUnitID { get; set; }
        public int StoreInchargeID { get; set; }
        public EnumChallanType ChallanType { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }    
        public DateTime ValidityDate { get; set; }
        public string DeliveryToAddress { get; set; }
        public string ReceivedByName { get; set; }
        public string PINo { get; set; }
        public string ExportLCNo { get; set; }
        public string Remarks { get; set; }
        public string ApproveByName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }
        public string ContractorName { get; set; }
        public string SCPName { get; set; }
        public string CPName { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleName { get; set; }
        public string GatePassNo { get; set; }
        public string DeliveryToName { get; set; }
        public string DONo { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchingData { get; set; }
        public int ProductCategoryID { get; set; }
        public DateTime VehicleDateTime { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        public double SalePrice { get; set; }
        public double RateUnit { get; set; }
        #endregion

        #region Derived Property
        public string VehicleDateTimeInString
        {
            get
            {
                return this.VehicleDateTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }

      public string VehicleTimeInString
      {
          get
          {
              return this.VehicleDateTime.ToString("hh:mm tt");
          }
      }

        public string ChallanDateSt
        {
            get 
            {
                if (this.ChallanDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ChallanDate.ToString("dd MMM yyyy");
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
      
        public string ChallanStatusSt
        {
            get
            {
                return EnumObject.jGet(this.ChallanStatus);
            }
        }
        public string DeliveryChallanTypeSt
        {
            get
            {
                return EnumObject.jGet(this.ChallanType);
            }
        }
        
        #endregion

        #region Functions
        public static List<DeliveryChallanRegister> Gets(string sSQL, long nUserID)
        {
            return DeliveryChallanRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDeliveryChallanRegisterService Service
        {
            get { return (IDeliveryChallanRegisterService)Services.Factory.CreateService(typeof(IDeliveryChallanRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IDeliveryChallanRegister interface

    public interface IDeliveryChallanRegisterService
    {
        List<DeliveryChallanRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
