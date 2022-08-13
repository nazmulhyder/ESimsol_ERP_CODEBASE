using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region ExportPartyInfoDetail
    public class ExportPartyInfoDetail : BusinessObject
    {
        public ExportPartyInfoDetail()
        {
            ExportPartyInfoDetailID = 0;
            ExportPartyInfoID = 0;
            ContractorID = 0;
            RefNo = "";
            RefDate = "";
            Activity = false;
            InfoCaption = "";
            PartyName = "";
            ErrorMessage = "";
            BankBranchID = 0;
            ExportPartyInfos = new List<ExportPartyInfo>();
            ExportPartyInfoDetails = new List<ExportPartyInfoDetail>();
            IsBank = false;
        }

        #region Properties
        public int ExportPartyInfoDetailID { get; set; }
        public int ExportPartyInfoID { get; set; }
        public int ContractorID { get; set; }
        public int BankBranchID { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public bool IsBank { get; set; }
        public bool Activity { get; set; }
        public string InfoCaption { get; set; }
        public string PartyName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
        public string IsBankSt
        {
            get
            {
                if (this.IsBank)
                {
                    return "Bank";
                }
                else
                {
                    return "Applicant";
                }
            }
        }
        public string ActivitySt
        {
            get
            {
                if (this.Activity)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public List<ExportPartyInfo> ExportPartyInfos { get; set; }
        public List<ExportPartyInfoDetail> ExportPartyInfoDetails { get; set; }
        #endregion

        #region Functions
        public ExportPartyInfoDetail Get(int id, int nUserID)
        {
            return ExportPartyInfoDetail.Service.Get(id, nUserID);
        }
        public ExportPartyInfoDetail Save(int nUserID)
        {
            return ExportPartyInfoDetail.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return ExportPartyInfoDetail.Service.Delete(id, nUserID);
        }
        public static List<ExportPartyInfoDetail> Gets(int nUserID)
        {
            return ExportPartyInfoDetail.Service.Gets(nUserID);
        }
        public static List<ExportPartyInfoDetail> Gets(string sSQL, int nUserID)
        {
            return ExportPartyInfoDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<ExportPartyInfoDetail> GetsByParty(int nPartyID, int nUserID)
        {
            return ExportPartyInfoDetail.Service.GetsByParty(nPartyID, nUserID);
        }
        public static List<ExportPartyInfoDetail> GetsBy(int nPartyID,int nBankBranchID, int nUserID)
        {
            return ExportPartyInfoDetail.Service.GetsBy(nPartyID, nBankBranchID, nUserID);
        }
        public static List<ExportPartyInfoDetail> Gets(int nPartyID, int nBankBranchID,string sIDs, int nUserID)
        {
            return ExportPartyInfoDetail.Service.Gets(nPartyID, nBankBranchID, sIDs, nUserID);
        }
        public static List<ExportPartyInfoDetail> GetsByPartyAndBill(int nPartyID, int nExportLCID, int nRefType,  int nUserID)
        {
            return ExportPartyInfoDetail.Service.GetsByPartyAndBill(nPartyID, nExportLCID, nRefType, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportPartyInfoDetailService Service
        {
            get { return (IExportPartyInfoDetailService)Services.Factory.CreateService(typeof(IExportPartyInfoDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IExportPartyInfoDetail interface
    public interface IExportPartyInfoDetailService
    {
        ExportPartyInfoDetail Get(int id, int nUserID);
        List<ExportPartyInfoDetail> Gets(int nUserID);
        string Delete(int id, int nUserID);
        ExportPartyInfoDetail Save(ExportPartyInfoDetail oExportPartyInfoDetail, int nUserID);
        List<ExportPartyInfoDetail> Gets(string sSQL, int nUserID);
        List<ExportPartyInfoDetail> GetsByParty(int nPartyID, int nUserID);
        List<ExportPartyInfoDetail> GetsBy(int nPartyID, int nBankBranchID, int nUserID);
        List<ExportPartyInfoDetail> Gets(int nPartyID, int nBankBranchID, string sIDs, int nUserID);
        List<ExportPartyInfoDetail> GetsByPartyAndBill(int nPartyID, int nExportLCID, int nRefType, int nUserID);
    }
    #endregion
}
