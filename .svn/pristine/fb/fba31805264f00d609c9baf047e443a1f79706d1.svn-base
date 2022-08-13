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
    #region AdjustmentRegister
    public class AdjustmentRegister : BusinessObject
    {
        public AdjustmentRegister()
        {
            AdjustmentRequisitionSlipDetailID = 0;
            AdjustmentRequisitionSlipID = 0;
            Qty = 0;
            LotID = 0;
            Note = "";
            LotNo = "";
            LogNo = "";
            ColorName = "";
            CurrentBalance = 0;
            ProductID = 0;
            ProductCode = "";
            ProductName = "";
            ProductCategoryID = 0;
            StyleNo = "";
            StyleID = 0;
            SupplierID = 0;
            BUID = 0;
            SupplierName = "";
            ARSlipNo = "";
            Date = DateTime.Now;
            MUName = "";
            WorkingUnitID = 0;
            WUName = "";
            ReportLayout = EnumReportLayout.None;
        }

        #region Properties
        public int AdjustmentRequisitionSlipDetailID { get; set; }
        public int AdjustmentRequisitionSlipID { get; set; }
        public int ProductID { get; set; }
        public int ProductCategoryID { get; set; }
        public int StyleID { get; set; }
        public int SupplierID { get; set; }
        public string LotNo { get; set; }
        public string LogNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string StyleNo { get; set; }
        public string SupplierName { get; set; }
        public double Qty { get; set; }
        public double CurrentBalance { get; set; }
        public int LotID { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        public string ColorName { get; set; }
        public int BUID { get; set; }
        public DateTime ExecutionDate { get; set; }
        public string ARSlipNo { get; set; }
        public DateTime Date { get; set; }
        public int WorkingUnitID { get; set; }
        public string WUName { get; set; }
        public string MUName { get; set; }
        public string SearchingData { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        #endregion

        #region Derived Property

        public string ExecutionDateSt
        {
            get 
            {
                if (this.ExecutionDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ExecutionDate.ToString("dd MMM yyyy");
                }
            }
        }
             
      
      
        
        #endregion

        #region Functions
        public static List<AdjustmentRegister> Gets(string sSQL, long nUserID)
        {
            return AdjustmentRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IAdjustmentRegisterService Service
        {
            get { return (IAdjustmentRegisterService)Services.Factory.CreateService(typeof(IAdjustmentRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IAdjustmentRegister interface

    public interface IAdjustmentRegisterService
    {
        List<AdjustmentRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
