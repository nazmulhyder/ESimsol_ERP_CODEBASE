using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region Account_Balance
    public class Account_Balance : BusinessObject
    {
        public Account_Balance()
        {
            Account_Balance_ID = 0;
            Bank_ID = 0;
            ERQAC = 0;
            FCAC = 0;
            LTR = 0;
            PAD = 0;
            Saleable_Amount = 0;
            Date = DateTime.Now;
        }

        #region Properties
        public int Account_Balance_ID { get; set; }
        public int Bank_ID { get; set; }
        public double ERQAC { get; set; }
        public double FCAC { get; set; }
        public double LTR { get; set; }
        public double PAD { get; set; }
        public double Saleable_Amount { get; set; }
        public DateTime Date { get; set; }
        public string BankName { get; set; }
        public string BankNickName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion


        #region Functions
        public Account_Balance Save(int nUserID)
        {            
            return Account_Balance.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return Account_Balance.Service.Delete(id, nUserID);
        }
        public static bool UpdateField(string sDBField, object oValue, TypeCode oType,int nUserID)
        {
            return Account_Balance.Service.UpdateField(sDBField, oValue, oType, nUserID);
        }
        public static List<Account_Balance> Gets(int nUserID)
        {
            return Account_Balance.Service.Gets(nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IAccount_BalanceService Service
        {
            get { return (IAccount_BalanceService)Services.Factory.CreateService(typeof(IAccount_BalanceService)); }
        }
        #endregion
    }
    #endregion

    #region IAccount_Balance interface
    public interface IAccount_BalanceService
    {
        Account_Balance Save(Account_Balance oAccount_Balance, int nUserID);
        string Delete(int id, int nUserID);
        bool UpdateField(string sDBField, object oValue, TypeCode oType, int nUserID);
        List<Account_Balance> Gets(int nUserID);
    }
    #endregion
   
}
