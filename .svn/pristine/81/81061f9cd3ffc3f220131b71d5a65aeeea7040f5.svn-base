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
    #region DataCollectionSetup
    [DataContract]
    public class DataCollectionSetup : BusinessObject
    {
        public DataCollectionSetup()
        {
            DataCollectionSetupID = 0;
            DataReferenceType = EnumDataReferenceType.None;
            DataReferenceTypeInInt = 0;
            DataReferenceID = 0;
            DataSetupType = EnumDataSetupType.None;
            DataSetupTypeInInt = 0;
            DataGenerateType = EnumDataGenerateType.None;
            DataGenerateTypeInInt = 0;
            QueryForValue = "";
            ReferenceValueFields = "";
            FixedText = "";
            Note = "";
            ErrorMessage = "";
        }

        #region Properties
        public int DataCollectionSetupID { get; set; }        
        public EnumDataReferenceType DataReferenceType { get; set; }        
        public int DataReferenceTypeInInt { get; set; }        
        public int DataReferenceID { get; set; }        
        public EnumDataSetupType DataSetupType { get; set; }        
        public int DataSetupTypeInInt { get; set; }
        public EnumDataGenerateType DataGenerateType { get; set; }        
        public int DataGenerateTypeInInt { get; set; }
        public string QueryForValue { get; set; }
        public string ReferenceValueFields { get; set; }        
        public string FixedText { get; set; }        
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property        
        public List<DataCollectionSetup> DataCollectionSetups { get; set; }
        public string DataGenerateTypeInString
        {
            get
            {
                return this.DataGenerateType.ToString();
            }
        }
        #endregion

        #region Functions

        public static List<DataCollectionSetup> Gets(int nDataReferenceID, EnumDataReferenceType eEnumDataReferenceType, long nUserID)
        {
            return DataCollectionSetup.Service.Gets(nDataReferenceID, (int)eEnumDataReferenceType, nUserID);            
        }

        public static List<DataCollectionSetup> GetsByIntegrationSetup(int nIntegrationSetupID, EnumDataReferenceType eEnumDataReferenceType, long nUserID)
        {
            return DataCollectionSetup.Service.GetsByIntegrationSetup(nIntegrationSetupID, (int)eEnumDataReferenceType, nUserID);            
        }
        public static List<DataCollectionSetup> Gets(string sSQL, long nUserID)
        {
            return DataCollectionSetup.Service.Gets(sSQL, nUserID);            
        }

        public DataCollectionSetup Get(int nDataCollectionSetupID, long nUserID)
        {
            return DataCollectionSetup.Service.Get(nDataCollectionSetupID, nUserID);            
        }

        public DataCollectionSetup Save(long nUserID)
        {
            return DataCollectionSetup.Service.Save(this, nUserID);            
        }
        #endregion

        #region ServiceFactory
        internal static IDataCollectionSetupService Service
        {
            get { return (IDataCollectionSetupService)Services.Factory.CreateService(typeof(IDataCollectionSetupService)); }
        }
        #endregion        
    }
    #endregion

    #region IDataCollectionSetup interface
    
    public interface IDataCollectionSetupService
    {
        
        DataCollectionSetup Get(int nDataCollectionSetupID, Int64 nUserID);
        
        List<DataCollectionSetup> Gets(int nDataReferenceID, int nDataReferenceType, Int64 nUserID);
        
        List<DataCollectionSetup> GetsByIntegrationSetup(int nIntegrationSetupID, int nDataReferenceType, Int64 nUserID);
        
        List<DataCollectionSetup> Gets(string sSQL, Int64 nUserID);
        
        DataCollectionSetup Save(DataCollectionSetup oDataCollectionSetup, Int64 nUserID);
    }
    #endregion
}
