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
    #region DeliveryOrderRegister
    public class DeliveryOrderRegister : BusinessObject
    {
        public DeliveryOrderRegister()
        {
            DeliveryOrderDetailID = 0;
            DeliveryOrderID = 0;
            ProductID = 0;
            Qty = 0;
            BUID = 0;
            RefID = 0;
            DONo = "";
            DODate = DateTime.Now;
            DOStatus = EnumDOStatus.Initialized;
            ContractorID  = 0;
            Note= "";
            ApproveBy  = 0;
            DeliveryDate =DateTime.Now;
            DeliveryPoint = "";
            ProductCode = "";
            ProductName= "";
            MUnitID = 0;
            MUSymbol = "";
            ContractorName= "";
            ApproveByName= "";
            PINo = "";
            ExportLCNo = "";
            ReportLayout = EnumReportLayout.None;
        }

        #region Properties
        public int DeliveryOrderDetailID { get; set; }
        public int DeliveryOrderID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public string ChallanNo { get; set; }
        public int BUID { get; set; }
        public DateTime DODate { get; set; }
        public EnumDOStatus DOStatus { get; set; }
        public int DOStatusInInt { get; set; }
        public int ContractorID { get; set; }
        public int ContactPersonnelID { get; set; }
        public int RefID { get; set; }
        public int StoreInchargeID { get; set; }
        public EnumChallanType ChallanType { get; set; }
        public int ApproveBy { get; set; }
        public DateTime DeliveryDate { get; set; }    
        
        public string Note { get; set; }
        public string DeliveryPoint { get; set; }
        public string PINo { get; set; }
        public string ExportLCNo { get; set; }
  
        public string ApproveByName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }
        public string ContractorName { get; set; }
         
        public string DONo { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchingData { get; set; }
     
        public EnumReportLayout ReportLayout { get; set; }
        #endregion

        #region Derived Property

        public string DODateSt
        {
            get 
            {
                if (this.DODate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.DODate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ApproveDateSt
        {
            get
            {
                if (this.DeliveryDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.DeliveryDate.ToString("dd MMM yyyy");
                }
            }
        }
      
        public string DOStatusSt
        {
            get
            {
                return EnumObject.jGet(this.DOStatus);
            }
        }
        public string DeliveryOrderTypeSt
        {
            get
            {
                return EnumObject.jGet(this.ChallanType);
            }
        }
        
        #endregion

        #region Functions
        public static List<DeliveryOrderRegister> Gets(string sSQL, long nUserID)
        {
            return DeliveryOrderRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDeliveryOrderRegisterService Service
        {
            get { return (IDeliveryOrderRegisterService)Services.Factory.CreateService(typeof(IDeliveryOrderRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IDeliveryOrderRegister interface

    public interface IDeliveryOrderRegisterService
    {
        List<DeliveryOrderRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
