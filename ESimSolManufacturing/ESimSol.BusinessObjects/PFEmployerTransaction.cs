using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Globalization;



namespace ESimSol.BusinessObjects
{
    #region PFEmployerTransaction

    public class PFEmployerTransaction : BusinessObject
    {
        public PFEmployerTransaction()
        {
            PETID = 0;
            Year = 0;
            Month = 1;
            PETType = EnumPETType.None;
            Amount = 0;
            Note = "";
            OperatorBy = 0;
            OperateDate = DateTime.Now;
            ApproveByDate = DateTime.MinValue;
            DistributedDate = DateTime.MinValue;

        }
        #region Properties

        public int PETID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public EnumPETType PETType { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
        public int OperatorBy { get; set; }
        public DateTime OperateDate { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveByDate { get; set; }
        public int DistributedBy { get; set; }
        public DateTime DistributedDate { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derive


        public string OperateByName { get; set; }
        public string ApproveByName { get; set; }
        public string DistributedByName { get; set; }

        public string DistributeByNameInStr { get { return (this.DistributedBy > 0) ? this.DistributedByName.ToString() : "--"; } }
        public string ApproveByNameInStr { get { return (this.ApproveBy > 0) ? this.ApproveByName.ToString() : "--"; } }
        public int PETTypeInInt { get { return (int)this.PETType; } }
        public string PETTypeInStr { get { return this.PETType.ToString(); } }
        public string ApproveByDateInStr { get { return (this.ApproveBy > 0) ? this.ApproveByDate.ToString("dd MMM yyyy") : "--"; } }

        public string DistributedByDateInStr { get { return (this.DistributedBy > 0) ? this.DistributedDate.ToString("dd MMM yyyy") : "--"; } }

        public string OperateDateInStr { get { return (this.OperatorBy > 0) ? this.OperateDate.ToString("dd MMM yyyy") : "--"; } }


        public string MonthOfYear
        {
            get
            {
                if (this.Month > 0 && this.Year > 0)
                    return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(this.Month) + " " + this.Year.ToString();
                else return "";
            }
        }

        #endregion


        #region Functions
        public static PFEmployerTransaction Distribute(int EnumBDOperation, int nPFMCID, Int64 nUserID)
        {
            return PFEmployerTransaction.Service.Distribute(EnumBDOperation, nPFMCID, nUserID);
        }
        public static PFEmployerTransaction Get(int nPETID, long nUserID)
        {
            return PFEmployerTransaction.Service.Get(nPETID, nUserID);
        }
       public static List<PFEmployerTransaction> Gets(string sSQL, long nUserID)
        {
            return PFEmployerTransaction.Service.Gets(sSQL, nUserID);
        }
        public PFEmployerTransaction IUD(int nDBOperation, long nUserID)
        {
            return PFEmployerTransaction.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IPFEmployerTransactionService Service
        {
            get { return (IPFEmployerTransactionService)Services.Factory.CreateService(typeof(IPFEmployerTransactionService)); }
        }
        #endregion
    }
    #endregion

    #region IPFEmployerTransaction interface

    public interface IPFEmployerTransactionService
    {
        PFEmployerTransaction Distribute(int EnumBDOperation, int nPFMCID, Int64 nUserID);
        PFEmployerTransaction Get(int nPETID, Int64 nUserID);
       List<PFEmployerTransaction> Gets(string sSQL, Int64 nUserID);
        PFEmployerTransaction IUD(PFEmployerTransaction oPFEmployerTransaction, int nDBOperation, Int64 nUserID);
    
    }
    #endregion
}
