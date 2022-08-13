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

    #region FinancialPositionSetup

    public class FinancialPositionSetup : BusinessObject
    {
        public FinancialPositionSetup()
        {
            FinancialPositionSetupID = 0;
            Sequence = 0;
            AccountHeadName = "";
            AccountHeadID = 0;
            AccountType = EnumAccountType.None;  
			ComponentID  = 0;
            ErrorMessage = "";
            AssetSetups = new List<FinancialPositionSetup>();
            LiabilityWithOwnersEquitySetups = new List<FinancialPositionSetup>();
        }

        #region Properties

        public int FinancialPositionSetupID { get; set; }
        public int Sequence { get; set; }
        public string AccountHeadName { get; set; }
        public string AccountCode { get; set; }
        public int AccountHeadID { get; set; }
        public EnumAccountType  AccountType {get;set;}
        public int ComponentID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<FinancialPositionSetup> AssetSetups { get; set; }
        public List<FinancialPositionSetup> LiabilityWithOwnersEquitySetups { get; set; }
        
        public Company Company { get; set; }
        public string AccountTypeInString
        {
            get
            {
                return this.AccountType.ToString();
            }
        }
     
        #endregion

        #region Functions

        public static List<FinancialPositionSetup> Gets(long nUserID)
        {
            return FinancialPositionSetup.Service.Gets(nUserID);
        }
        public static List<FinancialPositionSetup> Gets(string sSQL, Int64 nUserID)
        {
            return FinancialPositionSetup.Service.Gets(sSQL, nUserID);
        }

        public FinancialPositionSetup Get(int nId, long nUserID)
        {
            return FinancialPositionSetup.Service.Get(nId, nUserID);
        }

        public List<FinancialPositionSetup> Save(long nUserID)
        {
            return FinancialPositionSetup.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return FinancialPositionSetup.Service.Delete(nId, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFinancialPositionSetupService Service
        {

            get { return (IFinancialPositionSetupService)Services.Factory.CreateService(typeof(IFinancialPositionSetupService)); }
        }
        #endregion
    }
    #endregion

    #region IFinancialPositionSetup interface

    public interface IFinancialPositionSetupService
    {

        FinancialPositionSetup Get(int id, long nUserID);

        List<FinancialPositionSetup> Gets(long nUserID);
        List<FinancialPositionSetup> Gets(string sSQL, Int64 nUserID);

        string Delete(int id, long nUserID);

        List<FinancialPositionSetup> Save(FinancialPositionSetup oFinancialPositionSetup, long nUserID);
    }
    #endregion
    
    
   }
