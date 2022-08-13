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
    #region MasterPIMapping
    public class MasterPIMapping : BusinessObject
    {
        public MasterPIMapping()
        {
            MasterPIMappingID = 0;
            ExportPIID = 0;
            MasterPIID = 0;
            RateUnit = 0;
           
            CurrencyID = 0;
            Amount = 0;
            ContractorName = "";
            BankAccountNo = "";
            AccountName = "";
            PINo = "";
            MasterPINo = "";
            Currency = "";
            ErrorMessage = "";
            IssueDate =DateTime.Now;
            ValidityDate = DateTime.Now;
            BranchAddress = ""; 
            MKTPName = "";
            UnitSymbol = "";
            MasterPIMappings = new List<MasterPIMapping>();
            IssueDate = DateTime.Now;
            BranchName = "";
            BuyerName = "";
        }

        #region Properties
        public int MasterPIMappingID { get; set; }
        public int ExportPIID { get; set; }
        public int MasterPIID { get; set; }
        public int RateUnit { get; set; }
        public double Qty { get; set; }
        public int CurrencyID { get; set; }
        public double Amount { get; set; }
        public string BuyerName { get; set; }
        public string ContractorName { get; set; }
        public string BankAccountNo { get; set; }
        public string AccountName { get; set; }
        public string PINo { get; set; }
        public string MasterPINo { get; set; }
        public string Currency { get; set; }
        public string ErrorMessage { get; set; }     
        public DateTime IssueDate { get; set; }
        public DateTime ValidityDate { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string MKTPName { get; set; }
        public string UnitSymbol { get; set; }
        #endregion

        #region Derived Property
        public List<MasterPIMapping> MasterPIMappings { get; set; }
        public string IssueDateInString
        {
            get
            {
                return this.IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string ValidityDateInString
        {
            get
            {
                return this.ValidityDate.ToString("dd MMM yyyy");
            }
        }
        public string AmountSt
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
      
        public string QtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.Qty);
            }
        }
      
        public string QtyWithSymbol
        {
            get
            {
                return Global.MillionFormatActualDigit(this.Qty) + " " + this.UnitSymbol;
            }
        }


        #endregion

        #region Functions
        public static List<MasterPIMapping> Gets(Int64 nUserID)
        {
            return MasterPIMapping.Service.Gets(nUserID);
        }
        public static List<MasterPIMapping> GetsByMasterPI(int nExportPIID, Int64 nUserID)
        {
            return MasterPIMapping.Service.GetsByMasterPI(nExportPIID, nUserID);
        }
        public static List<MasterPIMapping> Gets(string sSQL, Int64 nUserID)
        {
            return MasterPIMapping.Service.Gets(sSQL, nUserID);
        }
        public MasterPIMapping Get(int id, Int64 nUserID)
        {
            return MasterPIMapping.Service.Get(id, nUserID);
        }
      
        #endregion

        #region conversion
      
        #endregion

        #region ServiceFactory
        internal static IMasterPIMappingService Service
        {
            get { return (IMasterPIMappingService)Services.Factory.CreateService(typeof(IMasterPIMappingService)); }
        }
        #endregion
    }
    #endregion

    #region IMasterPIMapping interface
    public interface IMasterPIMappingService
    {
        MasterPIMapping Get(int id, Int64 nUserID);
        List<MasterPIMapping> Gets(Int64 nUserID);
        List<MasterPIMapping> GetsByMasterPI(int nExportPIID, Int64 nUserID);
        List<MasterPIMapping> Gets(string sSQL, Int64 nUserID);
     
    }
    #endregion
}
