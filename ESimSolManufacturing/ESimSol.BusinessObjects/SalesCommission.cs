using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region SalesCommission
    public class SalesCommission : BusinessObject
    {
        public SalesCommission()
        {
            SalesCommissionID = 0;
            ExportPIID=0;
            ContactPersonnelID = 0;
            BUID = 0;
            CurrencyID = 0;
            Percentage = 0;
            TotalAmount = 0;
            Note = "";
            CommissionAmount = 0;
            Percentage_Maturity = 0;
            ContractorName = "";
            ExportLCNo = "";
            Status = EnumLSalesCommissionStatus.Initialize;
            ContractNo = "";
            ContractorFax = "";
            ContractorEmail = "";
            MKTPName = "";
            Currency = "";
            ErrorMessage = "";
            ApproveByName = "";
            CommissionOn = 0;
            ContractorID = 0;
        }

        #region Properties
      
        public int SalesCommissionID { get; set; }
        public int ExportPIID { get; set; }
        public int ContactPersonnelID { get; set; }
        public int BUID { get; set; }
        public int CurrencyID { get; set; }
        public double Percentage { get; set; }
        public double TotalAmount { get; set; }
        public double CommissionAmount { get; set; }
        public double Percentage_Maturity { get; set; }
        public EnumLSalesCommissionStatus Status { get; set; }
        public string Note { get; set; }
        public int ContractorID{ get; set; }
        public string ContractorName { get; set; }
        public string CPName { get; set; }
        public string ExportLCNo { get; set; }
        public string ContractNo { get; set; }
        public string ContractorFax { get; set; }
        public string ContractorEmail { get; set; }
        public string MKTPName { get; set; }
        public string Currency { get; set; }
        public bool Activity { get; set; }
        public string ApproveByName { get; set; }
        public string ErrorMessage { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public int RequestedBy { get; set; }
        public int PayableBy { get; set; }
        public string RequestedByName { get; set; }
        public int CommissionOn { get; set; }
      
        
        #endregion

        #region Derive Property

        #region CommissionAmountSt
        public string CommissionAmountSt
        {
            get
            {
                //return this.Currency + "" + Global.MillionFormat(Amount,4);)
                return this.Currency + "" + this.CommissionAmount.ToString("#,##0.0000");
            }
        }
        #endregion
        public string ApproveDateSt
        {
            get
            {
                
                return  (this.ApproveDate == DateTime.MinValue)? "-" : this.ApproveDate.ToString("dd MM yyyy");

            }
        }
        public string StatusStr { get { return this.Status.ToString(); } }
        
        #endregion
      
        #region Functions
        public SalesCommission Get(int id, Int64 nUserID)
        {
            return SalesCommission.Service.Get(id, nUserID);
        }
        public static List<SalesCommission> Gets(int nPIID, Int64 nUserID)
        {
            return SalesCommission.Service.Gets(nPIID,nUserID);
        }
        public static List<SalesCommission> Gets(string sSQL, Int64 nUserID)
        {
            return SalesCommission.Service.Gets(sSQL, nUserID);
        }
        public SalesCommission Save(Int64 nUserID)
        {
            return SalesCommission.Service.Save(this, nUserID);
        }
        public static List<SalesCommission> SaveAll(List<SalesCommission> oSalesCommissions, long nUserID)
        {
            return SalesCommission.Service.SaveAll(oSalesCommissions, nUserID);
        }
        public static List<SalesCommission> RequestedAll(List<SalesCommission> oSalesCommissions, long nUserID)
        {
            return SalesCommission.Service.RequestedAll(oSalesCommissions, nUserID);
        }
       
        public static List<SalesCommission> ApproveAll(List<SalesCommission> oSalesCommissions, long nUserID)
        {
            return SalesCommission.Service.ApproveAll(oSalesCommissions, nUserID);
        }
        public SalesCommission Approved(Int64 nUserID)
        {
            return SalesCommission.Service.Approved(this, nUserID);
        }
        public string Delete(Int64 nUserID)
        {
            return SalesCommission.Service.Delete(this, nUserID);
        }
        #endregion
        #region Non DB Functions
        #endregion

        #region ServiceFactory
        internal static ISalesCommissionService Service
        {
            get { return (ISalesCommissionService)Services.Factory.CreateService(typeof(ISalesCommissionService)); }
        }
        #endregion
    }
    #endregion

    #region ISalesCommission interface
    public interface ISalesCommissionService
    {
        SalesCommission Get(int id, Int64 nUserID);
        List<SalesCommission> Gets(int nPIID, Int64 nUserID);
        List<SalesCommission> Gets(string sSQL, Int64 nUserID);
        SalesCommission Save(SalesCommission oSalesCommission, Int64 nUserID);
        List<SalesCommission> SaveAll(List<SalesCommission> oSalesCommissions, Int64 nUserID);
        SalesCommission Approved(SalesCommission oSalesCommission, Int64 nUserID);
        List<SalesCommission> ApproveAll(List<SalesCommission> oSalesCommissions, Int64 nUserID);
        List<SalesCommission> RequestedAll(List<SalesCommission> oSalesCommissions, Int64 nUserID);
     

        string Delete(SalesCommission oSalesCommission, Int64 nUserID);
       
    }
    #endregion
}
