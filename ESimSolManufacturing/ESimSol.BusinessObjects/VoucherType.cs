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
    #region VoucherType
    public class VoucherType : BusinessObject
    {
        public VoucherType()
        {
            VoucherTypeID = 0;
            VoucherName = "";
            VoucherCategory = EnumVoucherCategory.None;
            NumberMethod = EnumNumberMethod.None;
            NumberConfigureID = 0;
            MustNarrationEntry = false;
            PrintAfterSave = false;
            IsProductRequired = false;
            IsDepartmentRequired = false;
            IsPaymentCheque = true;
            VoucherNumberFormat = "";
        }

        #region Properties
        public int VoucherTypeID { get; set; }
        public string VoucherName { get; set; }        
        public EnumVoucherCategory VoucherCategory { get; set; }
        public EnumNumberMethod NumberMethod { get; set; }
        public int NumberConfigureID { get; set; }
        public bool MustNarrationEntry { get; set; }
        public bool PrintAfterSave { get; set; }
        public bool IsProductRequired { get; set; } //IsProductRequired use as a printing page size determine A4 or Half of A4
        public bool IsDepartmentRequired { get; set; }
        public bool IsPaymentCheque { get; set; }
        public string ErrorMessage { get; set; }

        #region View Property        
        public int VoucherCategoryInInt { get; set; }
        public int NumberMethodInInt { get; set; }
        public string VoucherNumberFormat { get; set; }
        public string VoucherCategoryInString
        {
            get
            {
                return VoucherCategory.ToString();
            }
        }
        public string NumberMethodInString
        {
            get
            {
                return NumberMethod.ToString();
            }
        }
        #endregion

        #region Derive Property
        
        public string VoucherCodesInString { get; set; }
        public List<VoucherCode> VoucherCodes { get; set; }
        #endregion
        
        #endregion

        #region Functions
        public VoucherType Get(int id, int nUserID)
        {
            return VoucherType.Service.Get(id, nUserID);
        }
        public VoucherType Save(int nUserID)
        {
            return VoucherType.Service.Save(this, nUserID);
        }
        public string Delete(int id,int nUserID)
        {
            return VoucherType.Service.Delete(id, nUserID);
        }
        public static List<VoucherType> Gets(int nUserID)
        {
            return VoucherType.Service.Gets(nUserID);
        }
        public static List<VoucherType> Gets(string sSQL, int nUserID)
        {
            return VoucherType.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IVoucherTypeService Service
        {
            get { return (IVoucherTypeService)Services.Factory.CreateService(typeof(IVoucherTypeService)); }
        }
        #endregion
    }
    #endregion

    #region IVoucherType interface
    public interface IVoucherTypeService
    {
        VoucherType Get(int id, int nUserID);
        List<VoucherType> Gets(int nUserID);
        List<VoucherType> Gets(string sSQL, int nUserID);
        string Delete(int id, int nUserID);
        VoucherType Save(VoucherType oVoucherType, int nUserID);
    }
    #endregion
}