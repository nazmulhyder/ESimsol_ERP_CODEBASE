using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region Company
    public class Company : BusinessObject
    {
        public Company()
        {
            CompanyID = 0;
            Name = "";
            Address = "";
            FactoryAddress = "";
            Phone = "";
            Email = "";
            Note = "";
            WebAddress = "";
            OrganizationLogo = null;
            ImageSize = 0;
            BaseCurrencyID = 0;
            ParentID = 0;
            FaxNo = "";
            CurrencyNameSymbol = "";
            IsRemoveLogo = false;
            OrganizationTitle = null;
            TitleImageSize = 0;
            BaseAddress = "";
            ErrorMessage = "";
            CompanyRegNo = "";
            AuthorizedSignature = null;
            VatRegNo = "";
            PostalCode = "";
            Country = "";
            NameInBangla = "";
            AddressInBangla = "";
            EncryptCompanyID = "";
            IsOrganizationLogo = false;
            IsAuthorizedSignature = false;
            IsCompanyTitle = false;
            IsRemoveTitle = false;
            IsRemoveSignature = false;
        }

        #region Properties
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string FactoryAddress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Note { get; set; }
        public string WebAddress { get; set; }
        public string FaxNo { get; set; }
        public byte[] OrganizationLogo { get; set; }
        public int ImageSize { get; set; }
        public int BaseCurrencyID { get; set; }
        public int ParentID { get; set; }
        public string CurrencyNameSymbol { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public bool IsRemoveLogo { get; set; }
        public byte[] OrganizationTitle { get; set; }
        public int TitleImageSize { get; set; }
        public bool IsRemoveTitle { get; set; }
        public string BaseAddress { get; set; }
        public string ErrorMessage { get; set; }
        public string CompanyRegNo { get; set; }
        public byte[] AuthorizedSignature { get; set; }
        public bool IsRemoveSignature { get; set; }
        public string VatRegNo { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string NameInBangla { get; set; }
        public string AddressInBangla { get; set; }
        public bool IsOrganizationLogo { get; set; }
        public bool IsAuthorizedSignature { get; set; }
        public bool IsCompanyTitle { get; set; }

        #endregion

        #region Derive Property
        public List<Currency> Currencys { get; set; }
        public string EncryptCompanyID { get; set; }

        public string ByteInString { get; set; }
        public string ByteInString_CompanyTitle { get; set; }
        public string ByteInString_Signature { get; set; }
        public string CompanyLogoPath { get; set; }

        #endregion

        #region  PringReportHead

        private string _sPringReportHead = "";
        public string PringReportHead
        {
            get
            {
                _sPringReportHead = "";
                if (!string.IsNullOrEmpty(this.Address))
                {
                    _sPringReportHead = _sPringReportHead + "" + this.Address;
                }
                if (!string.IsNullOrEmpty(this.FactoryAddress))
                {
                    _sPringReportHead = _sPringReportHead + "" + this.FactoryAddress;
                }
                if (!string.IsNullOrEmpty(this.Phone))
                {
                    _sPringReportHead = _sPringReportHead + "\nTel: " + this.Phone;
                }
                if (this.Fax!=null && this.Fax.Length > 0)
                {
                    _sPringReportHead = _sPringReportHead + " Fax: " + this.Fax;
                }
                if (this.Email!=null && this.Email.Length > 0)
                {
                    _sPringReportHead = _sPringReportHead + " E-Mail: " + this.Email;
                }
                if (this.WebAddress!=null && this.WebAddress.Length > 0)
                {
                    _sPringReportHead = _sPringReportHead + " " + this.WebAddress;
                }

                return _sPringReportHead;
            }

        }
        

        #endregion                      
        #region  PrintAddress

        private string _sPrintAddress = "";
        public string PrintAddress
        {
            get
            {

                if (!string.IsNullOrEmpty(this.Address))
                {
                    _sPrintAddress = _sPrintAddress + "Head Office: " + this.Address;
                }
                if (!string.IsNullOrEmpty(this.FactoryAddress))
                {
                    _sPrintAddress = _sPrintAddress + "\nFactory: " + this.FactoryAddress;
                }

                return _sPrintAddress;
            }

        }


        #endregion                      

        #region Property For Image
        public System.Drawing.Image CompanyLogo { get; set; }
        public System.Drawing.Image CompanyTitle { get; set; }
        public System.Drawing.Image AuthSig { get; set; }
        public System.Drawing.Image OrganizationTitleLogo { get; set; }

        #endregion

        #region Functions

        public static List<Company> Gets(long nUserID)
        {
            return Company.Service.Gets(nUserID);
        }

        public Company Get(int nId, long nUserID)
        {
            return Company.Service.Get(nId,nUserID);
        }

        public Company Save(long nUserID)
        {
            return Company.Service.Save(this,nUserID);
        }

        public string Delete(int nID, long nUserID)
        {
            return Company.Service.Delete(nID,nUserID);
        }
        public Company GetCompanyLogo(int id, long nUserID)
        {
            return Company.Service.GetCompanyLogo(id, nUserID);
        }
        public static List<Company> Gets(string sSQL, long nUserID)
        {
            return Company.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICompanyService Service
        {
            get { return (ICompanyService)Services.Factory.CreateService(typeof(ICompanyService)); }
        }
        #endregion
    }
    #endregion

    #region ICompany interface
    
    public interface ICompanyService
    {
        Company Get(int id, long nUserID);
        List<Company> Gets(long nUserID);
        string Delete(int nID, long nUserID);
        Company Save(Company oOrganizationalInformation, long nUserID);
        Company GetCompanyLogo(int id, Int64 nUserID);

        List<Company> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
