using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region DymanicHeadSetup
    public class DymanicHeadSetup : BusinessObject
    {
        public DymanicHeadSetup()
        {
            DymanicHeadSetupID = 0;
            Name = "";
            ReferenceType = EnumReferenceType.None;
            MappingID=0;
            MappingType = EnumACMappingType.None;
            Activity = true;
            ErrorMessage = "";
        }

        #region Properties
       
 
        public int DymanicHeadSetupID { get; set; }
        public string Name { get; set; }
        public EnumReferenceType ReferenceType { get; set; }
        public int ReferenceTypeInt { get; set; }
        public int MappingID { get; set; }
        public EnumACMappingType MappingType { get; set; }
        public int MappingTypeInt { get; set; }
        public string Note { get; set; }
        public string MappingName { get; set; }
        public bool Activity { get; set; }
        public string ErrorMessage { get; set; }
        public string ReferenceTypeSt{get{return EnumObject.jGet(this.ReferenceType);}}
        public string MappingTypeSt { get { return EnumObject.jGet(this.MappingType); } }
        public string ActivityInString { get { return this.Activity ? "Active" : "Inactive"; } }
        public List<EnumObject> ReferenceTypeObjObjs { get; set; }
        public List<EnumObject> ACMappingTypeObjs { get; set; }
     
        #endregion


        #region Functions
        public static List<DymanicHeadSetup> Gets( int nUserID)
        {
            return DymanicHeadSetup.Service.Gets(nUserID);
        }
        public static List<DymanicHeadSetup> Gets(bool bActivity, int nUserID)
        {
            return DymanicHeadSetup.Service.Gets(bActivity,nUserID);
        }
        public DymanicHeadSetup Get(int id, Int64 nUserID)
        {
            return DymanicHeadSetup.Service.Get(id, nUserID);
        }
        public DymanicHeadSetup GetByRef(EnumReferenceType eEnumReferenceType, Int64 nUserID)
        {
            return DymanicHeadSetup.Service.GetByRef(eEnumReferenceType, nUserID);
        }

        public DymanicHeadSetup Save(int nUserID)
        {
            return DymanicHeadSetup.Service.Save(this, nUserID);
        }


        public string Delete( int nUserID)
        {
            return DymanicHeadSetup.Service.Delete(this, nUserID);
        }
        public string Process(int nUserID)
        {
            return DymanicHeadSetup.Service.Process(this, nUserID);
        }

        #endregion

        #region ServiceFactory

      
        internal static IDymanicHeadSetupService Service
        {
            get { return (IDymanicHeadSetupService)Services.Factory.CreateService(typeof(IDymanicHeadSetupService)); }
        }
        #endregion
    }
    #endregion

    #region IDymanicHeadSetup interface
    public interface IDymanicHeadSetupService
    {
        DymanicHeadSetup Get(int id, Int64 nUserID);
        DymanicHeadSetup GetByRef(EnumReferenceType eEnumReferenceType, Int64 nUserID);
        List<DymanicHeadSetup> Gets(Int64 nUserID);
        List<DymanicHeadSetup> Gets(bool bActivity,Int64 nUserID);
        DymanicHeadSetup Save(DymanicHeadSetup oDymanicHeadSetup, Int64 nUserID);
         string ActivateDymanicHeadSetup(DymanicHeadSetup oDymanicHeadSetup, Int64 nUserID);
         string Delete(DymanicHeadSetup oDymanicHeadSetup, Int64 nUserID);
         string Process(DymanicHeadSetup oDymanicHeadSetup, Int64 nUserID);
    }
    #endregion
}
