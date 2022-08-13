using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region VoucherCode
    public class VoucherCode : BusinessObject
    {
        public VoucherCode()
        {
            VoucherCodeID = 0;
            VoucherTypeID = 0;
            VoucherCodeType = EnumVoucherCodeType.None;
            Value = "";
            Length = 0;
            Restart = EnumRestartPeriod.None;
            Sequence = 0;          
        }

        #region Properties
        public int VoucherCodeID { get; set; }
        public int VoucherTypeID { get; set; }
        public EnumVoucherCodeType VoucherCodeType { get; set; }
        public string Value { get; set; }
        public int Length { get; set; }
        public EnumRestartPeriod Restart { get; set; }
        public int Sequence { get; set; }
        public int VoucherCodeTypeInInt  {get;set;}
        public int RestartInInt { get; set; }

        #region Derived Property
        public string VoucherCodeTypeInString
        {
            get { return VoucherCodeType.ToString(); }            
        }
        public string RestartInString
        {
            get { return Restart.ToString(); }            
        }
        #endregion

        #endregion

        #region Functions
        public VoucherCode Get(int id, int nUserID)
        {
            return VoucherCode.Service.Get(id, nUserID);
        }
        public VoucherCode Save(int nUserID)
        {
            return VoucherCode.Service.Save(this, nUserID);
        }
        public bool Delete(int id, int nUserID)
        {
            return VoucherCode.Service.Delete(id, nUserID);
        }
        public static List<VoucherCode> Gets(int nUserID)
        {
            return VoucherCode.Service.Gets(nUserID);
        }
        public static List<VoucherCode> Gets(string sSQL, int nUserID)
        {
            return VoucherCode.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IVoucherCodeService Service
        {
            get { return (IVoucherCodeService)Services.Factory.CreateService(typeof(IVoucherCodeService)); }
        }
        #endregion
    }
    #endregion

    #region IVoucherCode interface
    public interface IVoucherCodeService
    {
        VoucherCode Get(int id, int nUserID);
        List<VoucherCode> Gets(int nUserID);
        List<VoucherCode> Gets(string sSQL, int nUserID);
        bool Delete(int id, int nUserID);
        VoucherCode Save(VoucherCode oVoucherCode, int nUserID);
    }
    #endregion
}
