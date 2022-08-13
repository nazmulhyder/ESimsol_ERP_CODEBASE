using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Collections;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ACConfig
    public class ACConfig : BusinessObject
    {
        public ACConfig()
        {
            ACConfigID = 0;
            ConfigureType = EnumConfigureType.None;
            ConfigureValueType = EnumConfigureValueType.None;
            ConfigureValue = "";
            ErrorMessage = "";
            ConfigureTypeInInt = 0;
            ConfigureValueTypeInInt = 0;            
        }
        #region Properties
        public int ACConfigID { get; set; }
        public EnumConfigureType ConfigureType { get; set; }
        public EnumConfigureValueType ConfigureValueType { get; set; }
        public string ConfigureValue { get; set; }
        public string ErrorMessage { get; set; }
        public int ConfigureTypeInInt { get; set; }
        public int ConfigureValueTypeInInt { get; set; }        
        #endregion

        #region Derived Property
        public List<EnumObject> ConfigureTypeObjs { get; set; }
        public List<ACConfig> ACConfigs { get; set; }
        #endregion

        #region Functions
        public ACConfig Get(int id, int nCompanyID, int nUserID)
        {
            return ACConfig.Service.Get(id,nUserID);
        }
        public ACConfig Save(int nUserID)
        {
            return ACConfig.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return ACConfig.Service.Delete(id, nUserID);
        }
        public static List<ACConfig> Gets(int nUserID)
        {
            return ACConfig.Service.Gets(nUserID);
        }
        public static List<ACConfig> Gets(string sSQL, int nUserID)
        {
            return ACConfig.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IACConfigService Service
        {
            get { return (IACConfigService)Services.Factory.CreateService(typeof(IACConfigService)); }
        }
        #endregion
    }
    #endregion

    #region IACConfig interface
    public interface IACConfigService
    {
        ACConfig Save(ACConfig oACConfig,int nUserID);
        ACConfig Get(int id, int nUserID);
        List<ACConfig> Gets(int nUserID);
        List<ACConfig> Gets(string sSQL, int nUserID);
        string Delete(int id, int nUserID);
    }
    #endregion
}
