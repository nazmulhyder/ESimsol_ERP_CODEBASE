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
    #region VoucherHistory
    public class VoucherHistory : BusinessObject
    {
        public VoucherHistory()
        {
            VoucherHistoryID = 0;
            VoucherID = 0;
            UserID = 0;
            TransactionDate = DateTime.Now;
            ActionType = EnumRoleOperationType.None;
            Remarks = "";
            UserName = "";
            EmployeeNameCode = "";
            VoucherTypeID = 0;
            VoucherNo = "";
            Narration = "";
            VoucherName = "";
            VoucherDate = DateTime.Now;
            PostingDate = DateTime.Today;
            AuthorizedByName = "";
            PreparedByName = "";
            VoucherAmount = 0;
            VoucherHistorys = new List<VoucherHistory>();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            ErrorMessage = "";
        }
        #region Properties
        public int VoucherHistoryID { get; set; }
        public int VoucherID { get; set; }
        public int UserID { get; set; }
        public DateTime TransactionDate { get; set; }
        public EnumRoleOperationType ActionType { get; set; }
        public string Remarks { get; set; }
        public string UserName { get; set; }
        public string EmployeeNameCode { get; set; }
        public int VoucherTypeID { get; set; }
        public string VoucherNo { get; set; }
        public string Narration { get; set; }
        public string VoucherName { get; set; }
        public DateTime VoucherDate { get; set; }
        public DateTime PostingDate { get; set; }
        public string AuthorizedByName { get; set; }
        public string PreparedByName { get; set; }
        public Double VoucherAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<VoucherHistory> VoucherHistorys { get; set; }
        public string StartDateSt { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateSt { get { return this.EndDate.ToString("dd MMM yyyy"); } }
        public string PostingDateSt { get { return this.PostingDate.ToString("dd MMM yyyy hh:mm tt"); } }
        public string VoucherDateSt { get { return this.VoucherDate.ToString("dd MMM yyyy"); } }        
        public string ActionTypeSt { get { return this.ActionType.ToString(); } }
        public string VoucherAmountSt { get { return Global.MillionFormat(this.VoucherAmount); } }
        #endregion

        #region Functions

        public VoucherHistory Get(int id, int nUserID)
        {
            return VoucherHistory.Service.Get(id, nUserID);
        }
        public VoucherHistory Save(int nUserID)
        {
            return VoucherHistory.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return VoucherHistory.Service.Delete(id, nUserID);
        }
        public static List<VoucherHistory> Gets(int nUserID)
        {
            return VoucherHistory.Service.Gets(nUserID);
        }

        public static List<VoucherHistory> Gets(string sSQL, int nUserID)
        {
            return VoucherHistory.Service.Gets(sSQL, nUserID);
        }
        public static List<VoucherHistory> Gets(VoucherHistory oVoucherHistory, int nUserID)
        {
            return VoucherHistory.Service.Gets(oVoucherHistory, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IVoucherHistoryService Service
        {
            get { return (IVoucherHistoryService)Services.Factory.CreateService(typeof(IVoucherHistoryService)); }
        }
        #endregion
    }
    #endregion



    #region IVoucherHistory interface
    public interface IVoucherHistoryService
    {
        VoucherHistory Get(int id, int nUserID);
        List<VoucherHistory> Gets(int nUserID);

        string Delete(int id, int nUserID);
        VoucherHistory Save(VoucherHistory oVoucherHistory, int nUserID);

        List<VoucherHistory> Gets(string sSQL, int nUserID);
        List<VoucherHistory> Gets(VoucherHistory oVoucherHistory, int nUserID);

    }
    #endregion


}