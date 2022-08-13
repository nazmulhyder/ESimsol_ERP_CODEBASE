using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region VoucherMapping
    public class VoucherMapping : BusinessObject
    {
        public VoucherMapping()
        {
            VoucherMappingID = 0;
            TableName = "";
            PKColumnName = "";
            PKValue = 0;
            VoucherSetup = EnumVoucherSetup.None;
            VoucherSetupInt = 0;
            VoucherID = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int VoucherMappingID { get; set; }
        public string TableName { get; set; }
        public string PKColumnName { get; set; }
        public int PKValue { get; set; }
        public EnumVoucherSetup VoucherSetup { get; set; }
        public int VoucherSetupInt { get; set; }
        public long VoucherID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions
        public VoucherMapping Get(int id, int nUserID)
        {
            return VoucherMapping.Service.Get(id, nUserID);
        }
        public VoucherMapping Save(int nUserID)
        {
            return VoucherMapping.Service.Save(this, nUserID);
        }
        public string Delete(int nVoucherMappingID, int nUserID)
        {
            return VoucherMapping.Service.Delete(nVoucherMappingID, nUserID);
        }
        public static List<VoucherMapping> Gets(int nUserID)
        {
            return VoucherMapping.Service.Gets(nUserID);
        }
        public static List<VoucherMapping> Gets(string sSQL, int nUserID)
        {
            return VoucherMapping.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IVoucherMappingService Service
        {
            get { return (IVoucherMappingService)Services.Factory.CreateService(typeof(IVoucherMappingService)); }
        }
        #endregion
    }
    #endregion

    #region IVoucherMapping interface
    public interface IVoucherMappingService
    {
        VoucherMapping Get(int id, int nUserID);
        List<VoucherMapping> Gets(int nUserID);
        List<VoucherMapping> Gets(string sSQL, int nUserID);
        VoucherMapping Save(VoucherMapping oVoucherMapping, int nUserID);
        string Delete(int nVoucherMappingID, int nUserID);
    }
    #endregion
}
