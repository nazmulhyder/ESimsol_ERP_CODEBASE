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
    #region Subcontract

    public class Subcontract : BusinessObject
    {
        public Subcontract()
        {
            SubcontractID = 0;
            SubcontractNo = "";
            ContractStatus = EnumSubContractStatus.Initialized;
            ContractStatusInt = 0;
            IssueBUID = 0;
            ContractBUID = 0;
            PTU2ID = 0;
            IssueDate = DateTime.MinValue;
            ExportSCID = 0;
            ExportSCDetailID = 0;
            ProductID = 0;
            ColorID = 0;
            MoldRefID = 0;
            UintID = 0;
            Qty = 0;
            RateUnit = 1;
            UnitPrice = 0;
            CurrencyID = 0;
            CRate = 0;
            ApprovedBy = 0;
            Remarks = "";
            IssueBUName = "";
            IssueBUShortName = "";
            ContarctBUName = "";
            ContarctBUShortName = "";
            PINo = "";
            PIStatus = EnumPIStatus.Initialized;
            PIStatusInt = 0;
            ExportSCDate = DateTime.MinValue;
            ContractorName = "";
            BuyerName = "";
            ProductCode = "";
            ProductName = "";
            ColorName = "";
            UnitName = "";
            UnitSymbol = "";
            MoldName = "";
            ApprovedByName = "";
            ApprovedDate = DateTime.Now;
            ReceivedBy = 0;
            ReceivedByName = "";
            ReceivedDate = DateTime.Now;
            CurrencyName = "";
            CurrencySymbol = "";
            ProductionCapacity = 0;
            ErrorMessage = "";
            SubcontractList = new List<Subcontract>();
            Company = new Company();
            SCT = 0;
        }

        #region Properties
        public int SubcontractID { get; set; }
        public string SubcontractNo { get; set; }
        public EnumSubContractStatus ContractStatus { get; set; }
        public int ContractStatusInt { get; set; }
        public int IssueBUID { get; set; }
        public int ContractBUID { get; set; }
        public int PTU2ID { get; set; }
        public DateTime IssueDate { get; set; }
        public int ExportSCID { get; set; }
        public int ExportSCDetailID { get; set; }
        public int ProductID { get; set; }
        public int ColorID { get; set; }
        public int MoldRefID { get; set; }
        public int UintID { get; set; }
        public double Qty { get; set; }
        public int RateUnit { get; set; }
        public double UnitPrice { get; set; }
        public int CurrencyID { get; set; }
        public double CRate { get; set; }
        public int ApprovedBy { get; set; }
        public string Remarks { get; set; }
        public string IssueBUName { get; set; }
        public string IssueBUShortName { get; set; }
        public string ContarctBUName { get; set; }
        public string ContarctBUShortName { get; set; }
        public string PINo { get; set; }
        public EnumPIStatus PIStatus { get; set; }
        public int PIStatusInt { get; set; }
        public DateTime ExportSCDate { get; set; }
        public string ContractorName { get; set; }
        public string BuyerName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public string UnitName { get; set; }
        public string UnitSymbol { get; set; }
        public string MoldName { get; set; }
        public string ApprovedByName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public double ProductionCapacity { get; set; }
       public int  ReceivedBy{ get; set; } 
        public string    ReceivedByName { get; set; }
       public DateTime    ReceivedDate { get; set; }
       public DateTime ApprovedDate { get; set; }
        public string ErrorMessage { get; set; }
        public int SCT { get; set; } //0 Means Issue & 1 Means Received
        #endregion

        #region Derived Property
        public string ExportSCDateSt
        {
            get
            {
                return this.ExportSCDate.ToString("dd MMM yyyy");
            }
        }
        public string IssueDateSt
        {
            get
            {
                return this.IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string ApprovedDateSt
        {
            get
            {
                return this.ApprovedDate.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string ReceivedDateSt
        {
            get
            {
                return this.ReceivedDate.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string RateUnitSt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.RateUnit);
            }
        }
        public string ProductionCapacitySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.ProductionCapacity);
            }
        }
        public string QtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.Qty);
            }
        }
        public string UnitPriceSt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.UnitPrice);
            }
        }
        public double Amount
        {
            get
            {
                return ((this.Qty / this.RateUnit) * this.UnitPrice);
            }
        }
        public string AmountSt
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
        public string CRateSt
        {
            get
            {
                return Global.MillionFormat(this.CRate);
            }
        }
        public string ContractStatusSt
        {
            get
            {
                return EnumObject.jGet(this.ContractStatus);
            }
        }
        public List<Subcontract> SubcontractList { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public Company Company { get; set; }
        #endregion

        #region Functions

        public static List<Subcontract> Gets(long nUserID)
        {
            return Subcontract.Service.Gets(nUserID);
        }
        public static List<Subcontract> Gets(string sSQL, Int64 nUserID)
        {
            return Subcontract.Service.Gets(sSQL, nUserID);
        }

        public Subcontract Get(int nId, long nUserID)
        {
            return Subcontract.Service.Get(nId, nUserID);
        }

        public Subcontract Save(long nUserID)
        {
            return Subcontract.Service.Save(this, nUserID);
        }

        public Subcontract Approved(long nUserID)
        {
            return Subcontract.Service.Approved(this, nUserID);
        }
        public Subcontract Received(long nUserID)
        {
            return Subcontract.Service.Received(this, nUserID);
        }
        public Subcontract SendToProduction(long nUserID)
        {
            return Subcontract.Service.SendToProduction(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return Subcontract.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ISubcontractService Service
        {
            get { return (ISubcontractService)Services.Factory.CreateService(typeof(ISubcontractService)); }
        }
        #endregion
    }
    #endregion

    #region ISubcontract interface

    public interface ISubcontractService
    {
        Subcontract Get(int id, long nUserID);
        List<Subcontract> Gets(long nUserID);
        List<Subcontract> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        Subcontract Save(Subcontract oSubcontract, long nUserID);
        Subcontract Approved(Subcontract oSubcontract, long nUserID);
        Subcontract Received(Subcontract oSubcontract, long nUserID);
        Subcontract SendToProduction(Subcontract oSubcontract, long nUserID);
    }
    #endregion
}