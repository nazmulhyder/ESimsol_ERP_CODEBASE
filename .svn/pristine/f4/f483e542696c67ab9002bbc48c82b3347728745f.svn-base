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
    #region IntegrationSetup
    [DataContract]
    public class IntegrationSetup : BusinessObject
    {
        public IntegrationSetup()
        {
            IntegrationSetupID = 0;
            SetupNo = "";
            VoucherSetup =  EnumVoucherSetup.None;
            VoucherSetupInt = 0;
            DataCollectionSQL = "";
            KeyColumn = "";           
            Note = "";            
            Sequence = 0;
            SetupType = EnumSetupType.Import;
            SetupTypeInInt = 0;            
            BUID =0;
            BUName ="";
            BUSName = "";
            ErrorMessage = "";
            IntegrationSetupDetails = new List<IntegrationSetupDetail>();
            DataCollectionSetups = new List<DataCollectionSetup>();            
            IntegrationSetups = new List<IntegrationSetup>();
        }

        #region Properties        
        public int IntegrationSetupID { get; set; }        
        public string SetupNo { get; set; }
        public EnumVoucherSetup VoucherSetup { get; set; }
        public int VoucherSetupInt { get; set; }
        public string DataCollectionSQL { get; set; }  
        public string KeyColumn { get; set; }
        public string Note { get; set; }
        public int Sequence { get; set; }        
        public EnumSetupType SetupType { get; set; }        
        public int SetupTypeInInt { get; set; }
        public int BUID { get; set; }
        public string BUName { get; set; }
        public string BUSName { get; set; }
        public string ErrorMessage { get; set; }        
        #endregion

        #region Derived Property
        public int DateType { get; set; }        
        public DateTime StartDate  { get; set; }        
        public DateTime EndDate { get; set; }        
        public List<IntegrationSetup> IntegrationSetups { get; set; }        
        public List<IntegrationSetupDetail> IntegrationSetupDetails { get; set; }
        public IntegrationSetupDetail IntegrationSetupDetail { get; set; }        
        public List<DataCollectionSetup> DataCollectionSetups { get; set; }

        #region Derive Property
        public string VoucherSetupString
        {
            get
            {
                return EnumObject.jGet(this.VoucherSetup);
            }
        }
        #endregion
        #endregion

        #region Functions
        public static List<IntegrationSetup> Gets(long nUserID)
        {
            return IntegrationSetup.Service.Gets(nUserID);            
        }
        public static List<IntegrationSetup> GetsBySetupType(EnumSetupType eEnumSetupType, long nUserID)
        {
            return IntegrationSetup.Service.GetsBySetupType(eEnumSetupType, nUserID);            
        }
        public static List<IntegrationSetup> GetsByBU(int nBUID, long nUserID)
        {
            return IntegrationSetup.Service.GetsByBU(nBUID, nUserID);
        }        
        public static List<IntegrationSetup> Gets(string sSQL, long nUserID)
        {
            return IntegrationSetup.Service.Gets(sSQL, nUserID);            
        }
        public IntegrationSetup Get(int id, long nUserID)
        {
            return IntegrationSetup.Service.Get(id, nUserID);            
        }
        public IntegrationSetup Save(long nUserID)
        {
            return IntegrationSetup.Service.Save(this, nUserID);            
        }
        public static List<IntegrationSetup> UpdateSequence(IntegrationSetup oIntegrationSetup, long nUserID)
        {
            return IntegrationSetup.Service.UpdateSequence(oIntegrationSetup, nUserID);            
        }
        public string Delete(int nIntegrationSetupID, long nUserID)
        {
            return IntegrationSetup.Service.Delete(nIntegrationSetupID, nUserID);            
        }        
        #endregion

        #region Global Usable Functions
        public IntegrationSetup GetByVoucherSetup(EnumVoucherSetup eEnumVoucherSetup, long nUserID) //this function use for only & only if auto voucher process
        {
            IntegrationSetup oIntegrationSetup = IntegrationSetup.Service.GetByVoucherSetup(eEnumVoucherSetup, nUserID);
            List<IntegrationSetupDetail> oIntegrationSetupDetails = IntegrationSetupDetail.Gets(oIntegrationSetup.IntegrationSetupID, nUserID);
            List<DebitCreditSetup> oDebitCreditSetups = DebitCreditSetup.GetsByIntegrationSetup(oIntegrationSetup.IntegrationSetupID, nUserID);
            List<DataCollectionSetup> oDetailDataCollectionSetups = DataCollectionSetup.GetsByIntegrationSetup(oIntegrationSetup.IntegrationSetupID, EnumDataReferenceType.IntegrationDetail, nUserID); // Get for Details
            List < DataCollectionSetup> oDrCrDataCollectionSetups = DataCollectionSetup.GetsByIntegrationSetup(oIntegrationSetup.IntegrationSetupID, EnumDataReferenceType.DebitCreditSetup, nUserID); // Get for DebitCredit
            oIntegrationSetup.IntegrationSetupDetails = GetSetupDetails(oIntegrationSetupDetails, oDebitCreditSetups, oDetailDataCollectionSetups, oDrCrDataCollectionSetups);
            return oIntegrationSetup;
        }
        private List<IntegrationSetupDetail> GetSetupDetails(List<IntegrationSetupDetail> oIntegrationSetupDetails, List<DebitCreditSetup> oDebitCreditSetups, List<DataCollectionSetup> oDetailDataCollectionSetups, List<DataCollectionSetup> oDrCrDataCollectionSetups)
        {

            foreach (IntegrationSetupDetail oItem in oIntegrationSetupDetails)
            {
                oItem.DebitCreditSetups = GetDebitCreditSetups(oItem.IntegrationSetupDetailID, oDebitCreditSetups, oDrCrDataCollectionSetups);
                oItem.DataCollectionSetups = GetDataCollectionSetups(oItem.IntegrationSetupDetailID, oDetailDataCollectionSetups); // Set Data Collection for single Integration setup Detail

            }
            return oIntegrationSetupDetails;
        }
        private List<DebitCreditSetup> GetDebitCreditSetups(int id, List<DebitCreditSetup> oDebitCreditSetups, List<DataCollectionSetup> oDrCrDataCollectionSetups)
        {
            List<DebitCreditSetup> oTempDebitCreditSetups = new List<DebitCreditSetup>();

            foreach (DebitCreditSetup oItem in oDebitCreditSetups)
            {
                if (oItem.IntegrationSetupDetailID == id)
                {
                    oItem.DataCollectionSetups = GetDataCollectionSetups(oItem.DebitCreditSetupID, oDrCrDataCollectionSetups); // Set Data Collection for single Debit Credit setup
                    oTempDebitCreditSetups.Add(oItem);
                }
            }
            return oTempDebitCreditSetups;
        }
        private List<DataCollectionSetup> GetDataCollectionSetups(int id, List<DataCollectionSetup> oDataCollectionSetups)
        {
            List<DataCollectionSetup> oTempDataCollectionSetups = new List<DataCollectionSetup>();
            foreach (DataCollectionSetup oItem in oDataCollectionSetups)
            {
                if (id == oItem.DataReferenceID)
                {
                    oTempDataCollectionSetups.Add(oItem);
                }
            }
            return oTempDataCollectionSetups;
        }
        #endregion

        #region ServiceFactory
        internal static IIntegrationSetupService Service
        {
            get { return (IIntegrationSetupService)Services.Factory.CreateService(typeof(IIntegrationSetupService)); }
        }
        #endregion        
    }
    #endregion

    #region IIntegrationSetup interface
    
    public interface IIntegrationSetupService
    {        
        IntegrationSetup Get(int id, Int64 nUserID);        
        List<IntegrationSetup> Gets(Int64 nUserID);        
        List<IntegrationSetup> GetsBySetupType(EnumSetupType eEnumSetupType, Int64 nUserID);
        List<IntegrationSetup> GetsByBU(int nBUID, Int64 nUserID);
        List<IntegrationSetup> Gets(string sSQL, Int64 nUserID);        
        IntegrationSetup Save(IntegrationSetup oIntegrationSetup, Int64 nUserID);        
        List<IntegrationSetup> UpdateSequence(IntegrationSetup oIntegrationSetup, Int64 nUserID);        
        string Delete(int nIntegrationSetupID, Int64 nUserID);
        IntegrationSetup GetByVoucherSetup(EnumVoucherSetup eEnumVoucherSetup, Int64 nUserID);
    }
    #endregion
}
