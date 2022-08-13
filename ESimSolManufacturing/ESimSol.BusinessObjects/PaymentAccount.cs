using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region PaymentAccount
    public class PaymentAccount : BusinessObject
    {
        public PaymentAccount()
        {
            PaymentAccountID = 0;
            BUID = 0;
            AccountHeadID = 0;
            IsDefault = false;
            AccountCode = "";
            AccountHeadName = "";
            BUName = "";
            ErrorMessage = "";
            PaymentAccounts = new List<PaymentAccount>();
            ChartsOfAccounts = new List<ChartsOfAccount>();

        }

        #region Properties
        public int PaymentAccountID { get; set; }
        public int BUID { get; set; }
        public int AccountHeadID { get; set; }
        public bool IsDefault { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public string BUName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
        public string DefaultSt
        {
            get
            {
                if (this.IsDefault)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public List<PaymentAccount> PaymentAccounts { get; set; }
        public List<ChartsOfAccount> ChartsOfAccounts { get; set; }
        #endregion

        #region Functions
        public PaymentAccount Get(int id, int nUserID)
        {
            return PaymentAccount.Service.Get(id, nUserID);
        }
        public PaymentAccount Save(int nUserID)
        {
            return PaymentAccount.Service.Save(this, nUserID);
        }
        public PaymentAccount SetDefault(int nUserID)
        {
            return PaymentAccount.Service.SetDefault(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return PaymentAccount.Service.Delete(id, nUserID);
        }
        public static List<PaymentAccount> Gets(int nUserID)
        {
            return PaymentAccount.Service.Gets(nUserID);
        }
        public static List<PaymentAccount> Gets(string sSQL, int nUserID)
        {
            return PaymentAccount.Service.Gets(sSQL, nUserID);
        }
        public static List<PaymentAccount> GetsByBU(int nBUID, int nUserID)
        {
            return PaymentAccount.Service.GetsByBU(nBUID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IPaymentAccountService Service
        {
            get { return (IPaymentAccountService)Services.Factory.CreateService(typeof(IPaymentAccountService)); }
        }
        #endregion
    }
    #endregion

    #region IPaymentAccount interface
    public interface IPaymentAccountService
    {
        PaymentAccount Get(int id, int nUserID);
        List<PaymentAccount> Gets(int nUserID);
        string Delete(int id, int nUserID);
        PaymentAccount Save(PaymentAccount oPaymentAccount, int nUserID);
        PaymentAccount SetDefault(PaymentAccount oPaymentAccount, int nUserID);
        List<PaymentAccount> Gets(string sSQL, int nUserID);
        List<PaymentAccount> GetsByBU(int nBUID, int nUserID);
    }
    #endregion
}

