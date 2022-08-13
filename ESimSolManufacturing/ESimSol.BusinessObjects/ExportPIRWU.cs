using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ExportPIRWU
    public class ExportPIRWU : BusinessObject
    {
        public ExportPIRWU()
        {
            ExportPIDetailID = 0;
            PINo ="";
            ContractorName = "";
            BuyerName = "";
            MKTPName = "";
            ProductName = "";
            PONo = "";
            FabricNo = "";
            FSCNo = "";
            Compossion = "";
            Construction = "";
            //FabricType = 
            MUName = "";
            FinishType = EnumFinishType.None;
            ErrorMessage = "";
            IssueDate = DateTime.MinValue;
            FabricID = 0;
            FabricSalesContractDetailID = 0;
            ExeNo = "";
            ExportPIID = 0;
 
			SCDate		= DateTime.MinValue;		
			FabricWeave	= "";				
			ExportLCNo		= "";
			ExportLCDate	= DateTime.MinValue;	
			ShipmentDate	= DateTime.MinValue;	
			ExpiryDate	= DateTime.MinValue;
            PIStatus = EnumPIStatus.Initialized;
			DeliveryStartDate	= DateTime.MinValue;
            DeliveryCompleteDate = DateTime.MinValue;
            BankName = "";
        }

        #region Properties

        public int ExportPIDetailID { get; set; }
        public int ExportPIID { get; set; }
        public string PINo { get; set; }
        public string ContractorName { get; set; }
        public string BuyerName { get; set; }
        public string MKTPName { get; set; }
        public string ProductName { get; set; }
        public string PONo { get; set; }
        public string FabricNo { get; set; }
        public string FSCNo { get; set; }
        public string MUName { get; set; }
        public string BankName { get; set; }
        public EnumFinishType FinishType { get; set; }
        public string Construction { get; set; }
        public string Compossion { get; set; }
        public int OrderSheetDetailID { get; set; }
        public string FabricTypeName { get; set; }
        public string Wave { get; set; }
        public string ColorInfo { get; set; }
        public string StyleNo { get; set; }
        public string BuyerRef { get; set; }
        public int BUID { get; set; }
        public string FinishTypeName { get; set; }
        public double Qty { get; set; }
        public double Qty_PO { get; set; }
        public double Qty_DO { get; set; }
        public double Qty_DC { get; set; }
        public double UnitPrice { get; set; }
        public double UnitPrice_Avg { get; set; }
        public DateTime IssueDate { get; set; }
        public string ErrorMessage { get; set; }
        public string Currency { get; set; }
        public string ExeNo { get; set; }
        public int FabricID { get; set; }
        public int FabricSalesContractDetailID { get; set; }

        public DateTime SCDate { get; set; }
        public string FabricWeave { get; set; }
        public string ExportLCNo { get; set; }
        public DateTime ExportLCDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public EnumPIStatus PIStatus { get; set; }
        public DateTime DeliveryStartDate { get; set; }
        public DateTime DeliveryCompleteDate { get; set; }
        public double Amount
        {
            get
            {
                return this.Qty * this.UnitPrice;
            }
        }

        public string IssueDateSt
        {
            get
            {
                return this.GetDate(this.IssueDate);
            }
        }
        public string SCDateSt
        {
            get
            {
                return this.GetDate(this.SCDate);
            }
        }
        public string ExpiryDateSt
        {
            get
            {
                return this.GetDate(this.ExpiryDate);
            }
        }
        public string ShipmentDateSt
        {
            get
            {
                return this.GetDate(this.ShipmentDate);
            }
        }
        public string ExportLCDateSt
        {
            get
            {
                return this.GetDate(this.ExportLCDate);
            }
        }
        public string DeliveryStartDateSt
        {
            get
            {
                return this.GetDate(this.DeliveryStartDate);
            }
        }
        public string DeliveryCompleteDateSt
        {
            get
            {
                return this.GetDate(this.DeliveryCompleteDate);
            }
        }
        public string PIStatusSt
        {
            get
            {
                return EnumObject.jGet(this.PIStatus);
            }
        }
        public double YetToDo
        {
            get
            {
                return this.Qty - this.Qty_DO;
            }
        }
        public double YetToDelivery
        {
            get
            {
                return this.Qty - this.Qty_DC;
            }
        }
        
        
        #endregion

        #region Functions

        public static List<ExportPIRWU> Gets(int nUserID)
        {
            return ExportPIRWU.Service.Gets( nUserID);
        }
        public ExportPIRWU SetPO(ExportPIRWU oExportPIRWU, Int64 nUserID)
        {
            return ExportPIRWU.Service.SetPO(oExportPIRWU, nUserID);
        }
        public static List<ExportPIRWU> Gets(string sSql, int nUserID)
        {
            return ExportPIRWU.Service.Gets(sSql, nUserID);
        }
        public ExportPIRWU Get(int id, int nUserID)
        {
            return ExportPIRWU.Service.Get(id, nUserID);
        }
        public List<ExportPIRWU> GetByPINo(ExportPIRWU oExportPIRWU, long nUserID)
        {
            return ExportPIRWU.Service.GetByPINo(oExportPIRWU, nUserID);
        }
        public List<ExportPIRWU> GetByPONo(ExportPIRWU oExportPIRWU, long nUserID)
        {
            return ExportPIRWU.Service.GetByPONo(oExportPIRWU, nUserID);
        }
        public ExportPIRWU RemoveDispoNo(ExportPIRWU oExportPIRWU, int nUserID)
        {
            return ExportPIRWU.Service.RemoveDispoNo(oExportPIRWU, nUserID);
        }
        private string GetDate(DateTime dDate)
        {
            DateTime MinValue = new DateTime(1900, 01, 01);
            DateTime MinValue1 = new DateTime(0001, 01, 01);
            if (dDate == MinValue || dDate == MinValue1 || dDate == DateTime.MinValue)
            {
                return "-";
            }
            else
            {
                return dDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region ServiceFactory

        internal static IExportPIRWUService Service
        {
            get { return (IExportPIRWUService)Services.Factory.CreateService(typeof(IExportPIRWUService)); }
        }
        #endregion


    }
    #endregion

    #region IExportPIRWU interface
    public interface IExportPIRWUService
    {
        List<ExportPIRWU> Gets(Int64 nUserID);
        ExportPIRWU SetPO(ExportPIRWU oExportPIRWU, Int64 nUserID);
        List<ExportPIRWU> Gets(string sSql, Int64 nUserID);
        ExportPIRWU Get(int id, Int64 nUserID);
        List<ExportPIRWU> GetByPINo(ExportPIRWU oExportPIRWU, Int64 nUserID);
        List<ExportPIRWU> GetByPONo(ExportPIRWU oExportPIRWU, Int64 nUserID);
        ExportPIRWU RemoveDispoNo(ExportPIRWU oExportPIRWU, Int64 nUserID);

        //ExportPIRWU List<ExportPIRWU>(ExportPIRWU oExportPIRWU, Int64 nUserID);
    }
    #endregion
}
