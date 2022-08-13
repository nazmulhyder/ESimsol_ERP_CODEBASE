using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ImportSetup
    
    public class ImportSetup : BusinessObject
    {
        public ImportSetup()
        {
            ImportSetupID = 0;
            BUID = 0;
            IsApplyPO = false;
            IsApplyTT = false;
            IsApplyMasterLC = false;
            BUName = "";
            Note = "";
            ErrorMessage = "";
            Currency = "";
            CurrencyName = "";
            IsFreightRate = false;
            IsApplyRateOn = false;
            CoverNoteNumber = "";
            CoverNoteDate = DateTime.MinValue;
            FileType = 0;
            ExpireDay = 0;
            DaysCalculateOn = 0;
        }

        #region Properties
        public int ImportSetupID { get; set; }
        public int BUID { get; set; }
        public bool IsApplyPO { get; set; }
        public bool IsApplyTT { get; set; }
        public bool IsApplyMasterLC { get; set; }/// for Account Effece
        public string CoverNoteNumber { get; set; }
        public DateTime CoverNoteDate { get; set; }
        public int CurrencyID { get; set; }
        public bool Activity { get; set; }
        public string Note { get; set; }
        public string BUName { get; set; }
        public string Currency { get; set; }
        public string CurrencyName { get; set; }
        public bool IsFreightRate { get; set; }
        public bool IsApplyRateOn { get; set; }
        public EnumImportFileType FileType { get; set; }
        public EnumImportDateCalBy DaysCalculateOn { get; set; }
        public int FileTypeInt { get; set; }
        public int DaysCalculateOnInt { get; set; }
        public int ShipmentDay { get; set; }
        public int ExpireDay { get; set; }
        public string ErrorMessage { get; set; }
        
        #region Derived Property
        public string ActivitySt
        {
            get
            {
                if (this.Activity == true) return "Active";
                else if (this.Activity == false) return "Inactive";
                else return "-";
            }
        }
        public string CoverNoteDateST  
        {
            get
            {
                if (this.CoverNoteDate == DateTime.MinValue) return "";
                else return this.CoverNoteDate.ToString("dd MMM yyyy");
            }

        }
        #endregion

        #endregion

        #region Functions
        public static List<ImportSetup> Gets(long nUserID)
        {
            return ImportSetup.Service.Gets(nUserID);
        }
        public static List<ImportSetup> Gets(int nBUID,long nUserID)
        {
            return ImportSetup.Service.Gets(nBUID, nUserID);
        }
        public ImportSetup Get(int id, long nUserID)
        {
            return ImportSetup.Service.Get(id, nUserID);
        }
        public ImportSetup GetByBU(int nBUID, long nUserID)
        {
            return ImportSetup.Service.GetByBU(nBUID, nUserID);
        }

        public ImportSetup Save(long nUserID)
        {
            return ImportSetup.Service.Save(this, nUserID);
        }
        public ImportSetup Activate(Int64 nUserID)
        {
            return ImportSetup.Service.Activate(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return ImportSetup.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IImportSetupService Service
        {
            get { return (IImportSetupService)Services.Factory.CreateService(typeof(IImportSetupService)); }
        }
        #endregion
    }
    #endregion


    #region IImportSetup interface
    
    public interface IImportSetupService
    {
        
        ImportSetup Get(int id, Int64 nUserID);
        ImportSetup GetByBU(int nBUID, Int64 nUserID);
        List<ImportSetup> Gets(Int64 nUserID);
        List<ImportSetup> Gets(int nBUID,Int64 nUserID);
        string Delete(ImportSetup oImportSetup, Int64 nUserID);
        ImportSetup Save(ImportSetup oImportSetup, Int64 nUserID);
        ImportSetup Activate(ImportSetup oImportSetup, Int64 nUserID);
    }
    #endregion
}