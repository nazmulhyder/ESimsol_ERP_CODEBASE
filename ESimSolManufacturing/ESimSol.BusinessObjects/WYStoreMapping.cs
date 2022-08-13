using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region WYStoreMapping
    
    public class WYStoreMapping : BusinessObject
    {
        public WYStoreMapping()
        {
            WYStoreMappingID = 0;
            WYarnType = EnumWYarnType.None;
            WYStoreType = EnumStoreType.None;
            WorkingUnitID = 0;
            BUID = 0;
            Note = "";
            ErrorMessage = "";
        }

        #region Properties
        public int WYStoreMappingID { get; set; }
        public EnumWYarnType WYarnType { get; set; }
        public EnumStoreType WYStoreType { get; set; }
        public int WorkingUnitID { get; set; }
        public int BUID { get; set; }
        public string Note { get; set; }
        public int WYarnTypeInt { get; set; }
        public int WYStoreTypeInt { get; set; }
        public string ErrorMessage { get; set; }

        #region Derived Property
        public string WYStoreTypeSt
        {
            get
            {
                return EnumObject.jGet(this.WYStoreType);
            }
        }
        public string WYarnTypeSt
        {
            get
            {
                return EnumObject.jGet(this.WYarnType);
            }
        }
        #endregion

        #endregion

        #region Functions
        public static List<WYStoreMapping> Gets(long nUserID)
        {
            return WYStoreMapping.Service.Gets(nUserID);
        }
        public static List<WYStoreMapping> Gets(string sSQL, long nUserID)
        {
            return WYStoreMapping.Service.Gets(sSQL, nUserID);
        }

        public WYStoreMapping Get(int id, long nUserID)
        {
            return WYStoreMapping.Service.Get(id, nUserID);
        }
        public static List<WYStoreMapping> GetsActive(long nUserID)
        {
            return WYStoreMapping.Service.GetsActive(nUserID);
        }
        public WYStoreMapping ToggleActivity(long nUserID)
        {
            return WYStoreMapping.Service.ToggleActivity(this, nUserID);
        }
        public WYStoreMapping Save(long nUserID)
        {
            return WYStoreMapping.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return WYStoreMapping.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IWYStoreMappingService Service
        {
            get { return (IWYStoreMappingService)Services.Factory.CreateService(typeof(IWYStoreMappingService)); }
        }
        #endregion

        public static List<WYStoreMapping> GetsByModule(int nBUID, string sModuleIDs, long nUserID)
        {
            return WYStoreMapping.Service.GetsByModule(nBUID, sModuleIDs, nUserID);
        }

        public string StoreName { get; set; }
    }
    #endregion

    #region IWYStoreMapping interface
    
    public interface IWYStoreMappingService
    {
        WYStoreMapping Get(int id, Int64 nUserID);
        List<WYStoreMapping> GetsActive(Int64 nUserID);
        List<WYStoreMapping> Gets(Int64 nUserID);
        List<WYStoreMapping> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        WYStoreMapping Save(WYStoreMapping oWYStoreMapping, Int64 nUserID);
        WYStoreMapping ToggleActivity(WYStoreMapping oWYStoreMapping, Int64 nUserID);
        List<WYStoreMapping> GetsByModule(int nBUID, string sModuleIDs, Int64 nUserID);
    }
    #endregion
}