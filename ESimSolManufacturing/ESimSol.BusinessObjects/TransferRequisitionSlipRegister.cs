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
    #region TransferRequisitionSlipRegister
    public class TransferRequisitionSlipRegister : BusinessObject
    {
        public TransferRequisitionSlipRegister()
        {
            
            TRSDetailID = 0;
            TRSID = 0;
            ProductID = 0;
            MUnitID = 0;
            Qty = 0;
            RequisitionType = EnumRequisitionType.None;
            IssueBUID = 0;
            TRSNO = "";
            IssueDateTime = DateTime.Now;
            TransferStatus = EnumTransferStatus.Initialized;
            ReceivedBUID  = 0;
            IssueWorkingUnitID = 0;
            ReceivedWorkingUnitID = 0;
            GatePassNo = "";
            DisburseByUserID = 0;
            VehicleNo = "";
            ReceivedByUserID = 0;
            ReceivedBUName = "";
            AuthorisedByID = 0;
            AythorisedByName = "";
            IssueStoreName = "";
            ReceivedStoreName = "";
            ReceivedByName = "";
            DriverName = "";
            PreparedByName = "";
            StyleNo = "";
            ProductCode = "";
            ProductName = "";
            MUSymbol = "";
            Remark = "";
            ErrorMessage = "";
            SearchingData = "";
            DisbursedByName = "";
            ProductCategoryID = 0;
            RequisitionTypeInInt = 0;
            ContractorName = "";
            ReportLayout = EnumReportLayout.None;
        }

        #region Properties
        public int TRSDetailID { get; set; }
        public int TRSID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public string TRSNO { get; set; }
        public int IssueBUID { get; set; }
        public DateTime IssueDateTime { get; set; }
        public EnumTransferStatus TransferStatus { get; set; }
        public int TransferStatusInInt { get; set; }
        public EnumRequisitionType RequisitionType { get; set; }
        public int ReceivedBUID { get; set; }
        public int IssueWorkingUnitID { get; set; }
        public int ReceivedWorkingUnitID { get; set; }
        public int DisburseByUserID { get; set; }
        public int ReceivedByUserID { get; set; }
        public int AuthorisedByID { get; set; }
        public string DisbursedByName { get; set; }
        public string ReceivedBUName { get; set; }
        public string ReceivedByName { get; set; }
        public string AythorisedByName { get; set; }
        public string StyleNo { get; set; }
        public string IssueStoreName { get; set; }
        public string ReceivedStoreName { get; set; }
        public string ApproveByName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }
        public string PreparedByName { get; set; }
        public string DriverName { get; set; }
        
        public string VehicleNo { get; set; }
        public string Remark { get; set; }
        public string GatePassNo { get; set; }

        public string ErrorMessage { get; set; }
        public string SearchingData { get; set; }
        public string ColorName { get; set; }
        public string LotNo { get; set; }
        public string SupplierSName { get; set; }
        public int ProductCategoryID { get; set; }
        public int RequisitionTypeInInt { get; set; }
        public string ContractorName { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        #endregion

        #region Derived Property

        public string IssueDateTimeSt
        {
            get 
            {
                if (this.IssueDateTime == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.IssueDateTime.ToString("dd MMM yyyy");
                }
            }
        }

        public string TransferStatusSt
        {
            get
            {
                return EnumObject.jGet(this.TransferStatus);
            }
        }
        public string RequisitionTypeSt
        {
            get
            {
                return EnumObject.jGet(this.RequisitionType);
            }
        }
        
        #endregion

        #region Functions
        public static List<TransferRequisitionSlipRegister> Gets(string sSQL, long nUserID)
        {
            return TransferRequisitionSlipRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ITransferRequisitionSlipRegisterService Service
        {
            get { return (ITransferRequisitionSlipRegisterService)Services.Factory.CreateService(typeof(ITransferRequisitionSlipRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region ITransferRequisitionSlipRegister interface

    public interface ITransferRequisitionSlipRegisterService
    {
        List<TransferRequisitionSlipRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
