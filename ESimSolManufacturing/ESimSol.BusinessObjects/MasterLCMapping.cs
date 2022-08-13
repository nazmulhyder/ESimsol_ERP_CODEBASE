using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region MasterLCMapping
    [DataContract]
    public class MasterLCMapping : BusinessObject
    {
        public MasterLCMapping()
        {
            MasterLCMappingID = 0;
            MasterLCID = 0;
            ExportLCID = 0;
            MasterLCDate = DateTime.MinValue;
            MasterLCType = EnumMasterLCType.None;
            MasterLCTypeInInt = (int)EnumMasterLCType.None;
            MasterLCObj = new MasterLC();
        }

        #region Properties
        public int MasterLCMappingID { get; set; }
        public int ExportLCID { get; set; }
        public int MasterLCID { get; set; }
        public int ContractorID { get; set; }
        public DateTime MasterLCDate { get; set; }
        public string MasterLCNo { get; set; }
        public string ErrorMessage { get; set; }
        public EnumMasterLCType MasterLCType { get; set; }
        public int MasterLCTypeInInt { get; set; }
       
        #endregion
       
        #region Derived Propertiese
        public MasterLC MasterLCObj { get; set; }
    
        public string MasterLCDateSt
        {
            get
            {
                if (this.MasterLCDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.MasterLCDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string MasterLCTypeSt
        {
            get
            {
                return this.MasterLCType.ToString();
            }
        }
        #endregion

        #region Functions
     
        public static List<MasterLCMapping> Gets(int nELCID,Int64 nUserID)
        {
            return MasterLCMapping.Service.Gets(nELCID,nUserID);
        }
        public static List<MasterLCMapping> Gets(string sSQL, Int64 nUserID)
        {
            return MasterLCMapping.Service.Gets(sSQL, nUserID);
        }

        public MasterLCMapping Get(int id, Int64 nUserID)
        {
            return MasterLCMapping.Service.Get(id, nUserID);
        }
        public MasterLCMapping Save(Int64 nUserID)
        {
            return MasterLCMapping.Service.Save(this, nUserID);
        }
        public MasterLCMapping SaveWithMasterLC(Int64 nUserID)
        {
            return MasterLCMapping.Service.SaveWithMasterLC(this, nUserID);
        }
        public string Delete( Int64 nUserID)
        {
            return MasterLCMapping.Service.Delete(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IMasterLCMappingService Service
        {
            get { return (IMasterLCMappingService)Services.Factory.CreateService(typeof(IMasterLCMappingService)); }
        }
        #endregion
    }
    #endregion

    #region IMasterLCMapping interface
    public interface IMasterLCMappingService
    {
        MasterLCMapping Get(int id, Int64 nUserID);
        List<MasterLCMapping> Gets(int nELCID, Int64 nUserID);
        List<MasterLCMapping> Gets(string sSQL, Int64 nUserID);
        MasterLCMapping Save(MasterLCMapping oMasterLCMapping, Int64 nUserID);
        MasterLCMapping SaveWithMasterLC(MasterLCMapping oMasterLCMapping, Int64 nUserID);
        
        string Delete(MasterLCMapping oMasterLCMapping, Int64 nUserID);
    }
    #endregion
}
