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
    public class ExportOutstanding
    {
        public ExportOutstanding()
        {
            OperationStage = EnumOperationStage.None;
            Qty = 0;
            Amount = 0;
            Remarks = "";
            ErrorMessage = "";

            FromDate = DateTime.Today;
            ToDate = DateTime.Today;
            BUID = 0;
            OperationStageSt = "";

            GroupBy = 0;
            MakeString = "";

            ContractorID = 0;
            ContractorName = "";
            BankID = 0;
            BankName = "";
            TotalPI = 0;
            TotalPIQty = 0;
            TotalPIAmount = 0;
            TempID = 0; //ContractorID or BankID

            ExportPIID = 0;
            PINo = "";
            PIDate = DateTime.Today;
            PartyName = "";
            PIQty = 0;
            PIAmount = 0;
            DOQty = 0;
            ChallanQty = 0;
            LCNo = "";
            BillNo = "";
            LDBCNo = "";
            MaturityDate = DateTime.Today;
            ReceiveDate = DateTime.Today;
            BUID = 0;
            NegoBankName = "";
            BUName = "";
            OperationStageInt = 0;
        }
        #region Properties
      
        public EnumOperationStage OperationStage { get; set; }
        public int OperationStageInt { get; set; }
        public double Qty { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int BUID { get; set; }
        public string BUName { get; set; }
        public string OperationStageSt { get; set; }
        public int GroupBy { get; set; }
        public int TempID { get; set; } //ContractorID or BankID
        public string MakeString { get; set; }

        #region For Group wise
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public int BankID { get; set; }
        public string BankName { get; set; }
        public double TotalPI { get; set; }
        public double TotalPIQty { get; set; }
        public double TotalPIAmount { get; set; }
        #endregion

        #region For Detail
        public int ExportPIID { get; set; }
        public string PINo { get; set; }
        public DateTime PIDate { get; set; }
        public string PartyName { get; set; }
        public double PIQty { get; set; }
        public double PIAmount { get; set; }
        public double DOQty { get; set; }
        public double ChallanQty { get; set; }
        public string LCNo { get; set; }
        public string BillNo { get; set; }
        public string LDBCNo { get; set; }
        public DateTime MaturityDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string NegoBankName { get; set; }

        #endregion

        #endregion

        #region Derive Properties
        public string PIDateSt
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                DateTime MinValue1 = new DateTime(0001, 01, 01);
                if (this.PIDate == MinValue || this.PIDate == MinValue1)
                {
                    return "-";
                }
                return this.PIDate.ToString("dd MMM yyyy");
            }
        }
        public string MaturityDateSt
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                DateTime MinValue1 = new DateTime(0001, 01, 01);
                if (this.MaturityDate == MinValue || this.MaturityDate == MinValue1)
                {
                    return "-";
                }
                return this.MaturityDate.ToString("dd MMM yyyy");
            }
        }
        public string ReceiveDateSt
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                DateTime MinValue1 = new DateTime(0001, 01, 01);
                if (this.ReceiveDate == MinValue || this.ReceiveDate == MinValue1)
                {
                    return "-";
                }
                return this.ReceiveDate.ToString("dd MMM yyyy");
            }
        }
        public string OperationStageFromEnum
        {
            get
            {
                return ((EnumOperationStage)(this.OperationStage)).ToString();
            }
        }
        public string QtySt
        {
            get
            {
                return Global.MillionFormat(this.Qty);
            }
        }
        public string AmountSt
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
        public string FromDateSt
        {
            get
            {
                return this.FromDate.ToString("dd MMM yyyy");
            }
        }
        public string ToDateSt
        {
            get
            {
                return this.ToDate.ToString("dd MMM yyyy");
            }
        }
        #endregion
        #region Functions
        public static List<ExportOutstanding> Gets(DateTime dFromDODate, DateTime dToDODate, int nTextileUnit, long nUserID)
        {
            return ExportOutstanding.Service.Gets(dFromDODate, dToDODate, nTextileUnit, nUserID);
        }
        public static List<ExportOutstanding> GetsListByGroup(ExportOutstanding oExportOutstanding, long nUserID)
        {
            return ExportOutstanding.Service.GetsListByGroup(oExportOutstanding, nUserID);
        }
        public static List<ExportOutstanding> GetsListChallanDetail(ExportOutstanding oExportOutstanding, long nUserID)
        {
            return ExportOutstanding.Service.GetsListChallanDetail(oExportOutstanding, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportOutstandingService Service
        {
            get { return (IExportOutstandingService)Services.Factory.CreateService(typeof(IExportOutstandingService)); }
        }
        #endregion
    }

    #region IExportOutstanding interface
    public interface IExportOutstandingService
    {
        List<ExportOutstanding> Gets(DateTime dFromDODate, DateTime dToDODate, int nTextileUnit, long nUserID);
        List<ExportOutstanding> GetsListByGroup(ExportOutstanding oExportOutstanding, long nUserID);
        List<ExportOutstanding> GetsListChallanDetail(ExportOutstanding oExportOutstanding, long nUserID);
    }
    #endregion
}
