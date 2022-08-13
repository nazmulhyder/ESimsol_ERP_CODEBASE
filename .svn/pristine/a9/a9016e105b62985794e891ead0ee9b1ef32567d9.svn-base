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

    #region CIStatementSetup

    public class CIStatementSetup : BusinessObject
    {
        public CIStatementSetup()
        {
            CIStatementSetupID = 0;
            CIHeadType = EnumCISSetup.None;
            CIHeadTypeInt = 0;
            AccountHeadName = "";
            AccountHeadID = 0;
            DisplayCaption = "";
            AccountHeadValue = 0;
            AccountHeadValue_ForSession = 0;
            ComponentType =  EnumComponentType.None;
            Label = 2;
            RatioComponent = EnumRatioComponent.None;
            ErrorMessage = "";
            CIStatementSetups = new List<CIStatementSetup>();
        }

        #region Properties

        public int CIStatementSetupID { get; set; }
        public EnumCISSetup CIHeadType { get; set; }
        public int CIHeadTypeInt { get; set; }
        public EnumComponentType ComponentType { get; set; }
        public string DisplayCaption { get; set; }
        public string AccountHeadName { get; set; }
        public string AccountCode { get; set; }
        public int AccountHeadID { get; set; }
        public double AccountHeadValue { get; set; }
        public double AccountHeadValue_ForSession { get; set; }        
        public string Note { get; set; }
        public int Label { get; set; }
        public EnumRatioComponent RatioComponent { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<CIStatementSetup> CIStatementSetups { get; set; }
        public List<EnumObject> CISSetupObjs { get; set; }
        public Company Company { get; set; }
        public string AccountHeadValueSt { get { return this.AccountHeadValue < 0 ? "(" + Global.MillionFormat(this.AccountHeadValue * (-1)) + ")" : this.AccountHeadValue == 0 ? "-" : Global.MillionFormat(this.AccountHeadValue); } }
        public string AccountHeadValue_ForSessionSt { get { return this.AccountHeadValue_ForSession < 0 ? "(" + Global.MillionFormat(this.AccountHeadValue_ForSession * (-1)) + ")" : this.AccountHeadValue_ForSession == 0 ? "-" : Global.MillionFormat(this.AccountHeadValue_ForSession); } }
        
        public string CIHeadTypeInString
        {
            get
            {
                return EnumObject.jGet(this.CIHeadType);
            }
        }
        public string ComponentTypeSt
        {
            get
            {
                return EnumObject.jGet(this.ComponentType);
            }
        }
        #endregion

        #region Functions

        public static List<CIStatementSetup> Gets(long nUserID)
        {
            return CIStatementSetup.Service.Gets(nUserID);
        }
        public static List<CIStatementSetup> Gets(string sSQL, Int64 nUserID)
        {
            return CIStatementSetup.Service.Gets(sSQL, nUserID);
        }

        public CIStatementSetup Get(int nId, long nUserID)
        {
            return CIStatementSetup.Service.Get(nId, nUserID);
        }

        public List<CIStatementSetup> Save(long nUserID)
        {
            return CIStatementSetup.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return CIStatementSetup.Service.Delete(nId, nUserID);
        }
       
        #endregion

        #region ServiceFactory
        internal static ICIStatementSetupService Service
        {

            get { return (ICIStatementSetupService)Services.Factory.CreateService(typeof(ICIStatementSetupService)); }
        }
        #endregion
    }
    #endregion

    #region ICIStatementSetup interface

    public interface ICIStatementSetupService
    {

        CIStatementSetup Get(int id, long nUserID);

        List<CIStatementSetup> Gets(long nUserID);
        List<CIStatementSetup> Gets(string sSQL, Int64 nUserID);

        string Delete(int id, long nUserID);

        List<CIStatementSetup> Save(CIStatementSetup oCIStatementSetup, long nUserID);
    }
    #endregion
    
    
}
