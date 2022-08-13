using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region AddressConfig
    public class AddressConfig
    {
        public AddressConfig()
        {

            AddressConfigID = 0;
            NameInEnglish = "";
            NameInBangla = "";
            AddressType = EnumConfig_AddressType.District;
            ParentAddressID = 0;
            Remarks = "";
            ErrorMessage = "";
            Code = "";
        }

        #region Properties
        public int AddressConfigID { get; set; }
        public EnumConfig_AddressType AddressType { get; set; }
        public int AddressTypeInInt { get; set; }
        public string NameInEnglish { get; set; }
        public string Code { get; set; }
        public string NameInBangla { get; set; }
        public int ParentAddressID { get; set; }
        public string ErrorMessage { get; set; }
        public string Remarks { get; set; }
        #endregion

        #region Derived
        public string AddressTypeInStr
        {
            get
            {
                return AddressType.ToString();
            }
        }
        #endregion

        #region Functions


        public static List<AddressConfig> Gets(string sSQL, long nUserID)
        {
            return AddressConfig.Service.Gets(sSQL, nUserID);
        }
        public static AddressConfig Get(string sSQL, long nUserID)
        {
            return AddressConfig.Service.Get(sSQL, nUserID);
        }
        public AddressConfig IUD(int nDBOperation, long nUserID)
        {
            return AddressConfig.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IAddressConfigService Service
        {
            get { return (IAddressConfigService)Services.Factory.CreateService(typeof(IAddressConfigService)); }
        }
        #endregion
    }
    #endregion

    #region IAddressConfigDaily interface

    public interface IAddressConfigService
    {
        List<AddressConfig> Gets(string sSQL, Int64 nUserID);
        AddressConfig Get(string sSQL, Int64 nUserID);
        AddressConfig IUD(AddressConfig oApprovalHead, int nDBOperation, Int64 nUserID);
       
      
    }
    #endregion
}

