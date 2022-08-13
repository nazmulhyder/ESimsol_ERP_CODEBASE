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
    #region CertificateSheet

    public class Certificate : BusinessObject
    {
        public Certificate()
        {
            CertificateID = 0;
            CertificateNo = "";
            Description = "";
            RequiredFor = "";
            CertificateType = EnumCertificateType.None;
            IsActive = true;
            ErrorMessage = "";

        }

        #region Properties
        public int CertificateID { get; set; }
        public string CertificateNo { get; set; }
        public string Description { get; set; }
        public string RequiredFor { get; set; }
        public EnumCertificateType CertificateType { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public int CertificateTypeInt { get; set; }
        public string CertificateTypeInString
        {
            get
            {
                return CertificateType.ToString();
            }
        }
        public string ActivityStatus { get { if (IsActive)return "Active"; else return "Inactive"; } }
        #endregion

        #region Functions
        public static Certificate Get(int id, long nUserID)
        {
            return Certificate.Service.Get(id, nUserID);
        }

        public static Certificate Get(string sSQL, long nUserID)
        {
            return Certificate.Service.Get(sSQL, nUserID);
        }

        public static List<Certificate> Gets(long nUserID)
        {
            return Certificate.Service.Gets(nUserID);
        }

        public static List<Certificate> Gets(string sSQL, long nUserID)
        {
            return Certificate.Service.Gets(sSQL, nUserID);
        }

        public Certificate IUD(int nDBOperation, long nUserID)
        {
            return Certificate.Service.IUD(this, nDBOperation, nUserID);
        }

        public static Certificate Activity(int nId, bool bActive, long nUserID)
        {
            return Certificate.Service.Activity(nId, bActive, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ICertificateService Service
        {
            get { return (ICertificateService)Services.Factory.CreateService(typeof(ICertificateService)); }
        }

        #endregion
    }
    #endregion

    #region ICertificate interface

    public interface ICertificateService
    {
        Certificate Get(int id, Int64 nUserID);
        Certificate Get(string sSQL, Int64 nUserID);
        List<Certificate> Gets(Int64 nUserID);
        List<Certificate> Gets(string sSQL, Int64 nUserID);
        Certificate IUD(Certificate oCertificateSheet, int nDBOperation, Int64 nUserID);
        Certificate Activity(int nId, bool bActive, Int64 nUserID);


    }
    #endregion
}
