using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region EmployeeCommissionMaterial

    public class EmployeeCommissionMaterial : BusinessObject
    {
        public EmployeeCommissionMaterial()
        {
            ECMID = 0;
            CMID = 0;
            EmployeeID = 0;
            SearchWhatValue = "";
            Note = "";
            EffectDate = DateTime.Now;
            ApproveBy = 0;
            ApproveByName = "";
            ApproveByDate = DateTime.Now;
            IsActive = true;
            InactiveDate = DateTime.Now;
            ErrorMessage = "";
            MaterialName = "";

        }

        #region Properties
        public int ECMID { get; set; }
        public int CMID { get; set; }
        public int EmployeeID { get; set; }
        public string SearchWhatValue { get; set; }
        public string Note { get; set; }
        public DateTime EffectDate { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveByDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime InactiveDate { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string EncryptedID { get; set; }
        public string ActivityInString { get { if (IsActive)return "Active"; else return "Inactive"; } }
        public string ApproveByName { get; set; }
        public string MaterialName { get; set; }
        public List<EmployeeCommissionMaterial> EmployeeCommissionMaterials { get; set; }
        public Employee Employee { get; set; }
        public Company Company { get; set; }
        public string EffectDateInString
        {
            get
            {
                return EffectDate.ToString("dd MMM yyyy");
            }
        }
        public string ApproveByDateInString
        {
            get
            {
                if (this.ApproveByDate > Convert.ToDateTime("01 jan 1800"))
                    return ApproveByDate.ToString("dd MMM yyyy");
                else
                    return "";
            }
        }
        public string InactiveDateInString
        {
            get
            {
                return InactiveDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static EmployeeCommissionMaterial Get(int Id, long nUserID)
        {
            return EmployeeCommissionMaterial.Service.Get(Id, nUserID);
        }
        public static EmployeeCommissionMaterial Get(string sSQL, long nUserID)
        {
            return EmployeeCommissionMaterial.Service.Get(sSQL, nUserID);
        }
        public static List<EmployeeCommissionMaterial> Gets(long nUserID)
        {
            return EmployeeCommissionMaterial.Service.Gets(nUserID);
        }

        public static List<EmployeeCommissionMaterial> Gets(string sSQL, long nUserID)
        {
            return EmployeeCommissionMaterial.Service.Gets(sSQL, nUserID);
        }

        public EmployeeCommissionMaterial IUD(int nDBOperation, long nUserID)
        {
            return EmployeeCommissionMaterial.Service.IUD(this, nDBOperation, nUserID);
        }

        public static EmployeeCommissionMaterial Activite(int nId, long nUserID)
        {
            return EmployeeCommissionMaterial.Service.Activite(nId, nUserID);
        }
        public static EmployeeCommissionMaterial Approve(string sSql, long nUserID)
        {
            return EmployeeCommissionMaterial.Service.Approve(sSql, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IEmployeeCommissionMaterialService Service
        {
            get { return (IEmployeeCommissionMaterialService)Services.Factory.CreateService(typeof(IEmployeeCommissionMaterialService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeCommissionMaterial interface

    public interface IEmployeeCommissionMaterialService
    {
        EmployeeCommissionMaterial Get(int id, Int64 nUserID);
        EmployeeCommissionMaterial Get(string sSQL, Int64 nUserID);
        List<EmployeeCommissionMaterial> Gets(Int64 nUserID);
        List<EmployeeCommissionMaterial> Gets(string sSQL, Int64 nUserID);
        EmployeeCommissionMaterial IUD(EmployeeCommissionMaterial oEmployeeCommissionMaterial, int nDBOperation, Int64 nUserID);
        EmployeeCommissionMaterial Activite(int nId, Int64 nUserID);
        EmployeeCommissionMaterial Approve(string sSql, Int64 nUserID);
    }
    #endregion
}
