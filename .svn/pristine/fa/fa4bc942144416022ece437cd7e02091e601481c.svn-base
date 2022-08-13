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
    #region MoldRegister
    public class MoldRegister : BusinessObject
    {
        public MoldRegister()
        {
            CRID = 0;
            Cavity = 0;
            Code = "";
            Name = "";
            BUID = 0;
            RackID = 0;
            LocationID = 0;
            ResourcesType = EnumResourcesType.None;
            ShelfName = "";
            RackNo = "";
            Remarks = "";
            ContractorName = "";
            LocationName = "";
            ContractorName = "";
        }
        
        #region Properties
        public int CRID { get; set; }
        public double Cavity { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int BUID { get; set; }
        public EnumResourcesType ResourcesType { get; set; }
        public string ShelfName { get; set; }
        public string RackNo { get; set; }
        public string LocationName { get; set; }
        public string ContractorName { get; set; }
        public string Remarks { get; set; }
        public int  RackID { get; set; }
        public int LocationID { get; set; }
        public int ResourcesTypeInInt { get; set; }

        #endregion

        #region Derived Property

        public string SearchingData { get; set; }

        public string ResourcesTypeSt
        {
            get
            {
                return EnumObject.jGet(this.ResourcesType);
            }
        }
      
        
        #endregion

        #region Functions
        public static List<MoldRegister> Gets(string sSQL, long nUserID)
        {
            return MoldRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IMoldRegisterService Service
        {
            get { return (IMoldRegisterService)Services.Factory.CreateService(typeof(IMoldRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IMoldRegister interface

    public interface IMoldRegisterService
    {
        List<MoldRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
