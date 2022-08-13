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
    #region CommissionMaterial

    public class CommissionMaterial : BusinessObject
    {
        public CommissionMaterial()
        {

            CMID = 0;
            Name = "";
            SearchWhat = "";
            IsActive = true;
            ErrorMessage = "";

        }

        #region Properties

        public int CMID { get; set; }

        public string Name { get; set; }
        public string SearchWhat { get; set; }
        public bool IsActive { get; set; }

        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string EncryptedID { get; set; }
        public string ActivityInString { get { if (IsActive)return "Active"; else return "Inactive"; } }
        public List<CommissionMaterial> CommissionMaterials { get; set; }
        public Company Company { get; set; }
        #endregion

        #region Functions
        public static CommissionMaterial Get(int Id, long nUserID)
        {
            return CommissionMaterial.Service.Get(Id, nUserID);
        }
        public static CommissionMaterial Get(string sSQL, long nUserID)
        {
            return CommissionMaterial.Service.Get(sSQL, nUserID);
        }
        public static List<CommissionMaterial> Gets(long nUserID)
        {
            return CommissionMaterial.Service.Gets(nUserID);
        }

        public static List<CommissionMaterial> Gets(string sSQL, long nUserID)
        {
            return CommissionMaterial.Service.Gets(sSQL, nUserID);
        }

        public CommissionMaterial IUD(int nDBOperation, long nUserID)
        {
            return CommissionMaterial.Service.IUD(this, nDBOperation, nUserID);
        }

        public static CommissionMaterial Activite(int nId, long nUserID)
        {
            return CommissionMaterial.Service.Activite(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICommissionMaterialService Service
        {
            get { return (ICommissionMaterialService)Services.Factory.CreateService(typeof(ICommissionMaterialService)); }
        }

        #endregion
    }
    #endregion

    #region ICommissionMaterial interface

    public interface ICommissionMaterialService
    {
        CommissionMaterial Get(int id, Int64 nUserID);
        CommissionMaterial Get(string sSQL, Int64 nUserID);
        List<CommissionMaterial> Gets(Int64 nUserID);
        List<CommissionMaterial> Gets(string sSQL, Int64 nUserID);
        CommissionMaterial IUD(CommissionMaterial oCommissionMaterial, int nDBOperation, Int64 nUserID);
        CommissionMaterial Activite(int nId, Int64 nUserID);
    }
    #endregion
}
