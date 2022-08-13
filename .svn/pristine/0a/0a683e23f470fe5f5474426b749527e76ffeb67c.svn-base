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
    #region AccountingActivity
    public class AccountingActivity : BusinessObject
    {
        public AccountingActivity()
        {
            UserID = 0;
            UserName = "";
            VoucherTypeID = 0;
            VoucherName = "";
            Added = 0;
            Edited = 0;
            Approved = 0;
            Total = 0;

            AccountingActivitys=new List<AccountingActivity>();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            ErrorMessage = "";
        }
        #region Properties
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int VoucherTypeID { get; set; }
        public string VoucherName { get; set; }
        public int Added { get; set; }
        public int Edited { get; set; }
        public int Approved { get; set; }
        public int Total { get; set; }


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<AccountingActivity> AccountingActivitys { get; set; }
        public string StartDateSt { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateSt { get { return this.EndDate.ToString("dd MMM yyyy"); } }
        #endregion

        #region Functions

        public static List<AccountingActivity> Gets(int nRoleUserID, DateTime dStartDate, DateTime dEnddate, int nUserID)
        {
            return AccountingActivity.Service.Gets(nRoleUserID, dStartDate, dEnddate, nUserID);
        }

        #endregion


        #region ServiceFactory
        internal static IAccountingActivityService Service
        {
            get { return (IAccountingActivityService)Services.Factory.CreateService(typeof(IAccountingActivityService)); }
        }
        #endregion
    }
    #endregion



    #region IAccountingActivity interface
    public interface IAccountingActivityService
    {

        List<AccountingActivity> Gets(int nRoleUserID, DateTime dStartDate, DateTime dEnddate, int nUserID);
    }
    #endregion


}