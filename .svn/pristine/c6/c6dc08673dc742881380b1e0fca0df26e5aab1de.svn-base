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
    #region ChangesEquitySetup
    public class ChangesEquitySetup : BusinessObject
    {
        public ChangesEquitySetup()
        {
            ChangesEquitySetupID = 0;
            EquityCategory = EnumEquityCategory.None;
            EquityCategoryInt = 0;
            Remarks = "";
            ErrorMessage = "";
            ChangesEquitySetupDetails = new List<ChangesEquitySetupDetail>();
            EquityCategorys = new List<EnumObject>();
        }
        #region Properties
        public int ChangesEquitySetupID { get; set; }
        public EnumEquityCategory EquityCategory { get; set; }
        public int EquityCategoryInt { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<ChangesEquitySetupDetail> ChangesEquitySetupDetails { get; set; }
        public List<EnumObject> EquityCategorys { get; set; }
        public string EquityCategoryInString
        {
            get 
            {
                return EnumObject.jGet(this.EquityCategory);
            }
        }
        #endregion

        #region Functions

        public ChangesEquitySetup Get(int id, int nUserID)
        {
            return ChangesEquitySetup.Service.Get(id, nUserID);
        }
        public ChangesEquitySetup Save(int nUserID)
        {
            return ChangesEquitySetup.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return ChangesEquitySetup.Service.Delete(id, nUserID);
        }
        public static List<ChangesEquitySetup> Gets(int nUserID)
        {
            return ChangesEquitySetup.Service.Gets(nUserID);
        }

        public static List<ChangesEquitySetup> Gets(string sSQL, int nUserID)
        {
            return ChangesEquitySetup.Service.Gets(sSQL, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IChangesEquitySetupService Service
        {
            get { return (IChangesEquitySetupService)Services.Factory.CreateService(typeof(IChangesEquitySetupService)); }
        }
        #endregion
    }
    #endregion

    #region IChangesEquitySetup interface
    public interface IChangesEquitySetupService
    {
        ChangesEquitySetup Get(int id, int nUserID);
        List<ChangesEquitySetup> Gets(int nUserID);
        string Delete(int id, int nUserID);
        ChangesEquitySetup Save(ChangesEquitySetup oChangesEquitySetup, int nUserID);
        List<ChangesEquitySetup> Gets(string sSQL, int nUserID);
    }
    #endregion
}