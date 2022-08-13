using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Data;


namespace ESimSol.BusinessObjects
{
    #region Contractor
    public class Contractor : BusinessObject
    {
        public Contractor()
        {
            ContractorID = 0;
            ContractorType= EnumContractorType.None;
            Name = "";
            Origin = "";
            Address= "";
            Address2 = "";
            Phone= "";
            Email= "";
            ShortName = "";
            Fax = "";
            ActualBalance = 0;
            Phone2 = "";
            TIN = "";
            VAT ="";
            GroupName = "";
            Note = "";
            MultipleItemReturn = false;
            ContractorTypeInInt = 0;
            Activity = true;
            Params = "";
            CountryID = 0;
            CountryShortName = "";
            CountryCode = "";
            CountryName = "";
            IssueFigures = new List<IssueFigure>();
            BuyerConcerns = new List<BuyerConcern>();
            LastUpdateUserID = 0;
            LastUpdateDateTime = DateTime.MinValue;
            UpdateByName = "";
        }

        #region Properties
        public int ContractorID { get; set; }
        public EnumContractorType ContractorType { get; set; }
        public string Name { get; set; }
        public string Origin { get; set; }
        public string Zone { get; set; }
        public string Address { get; set; }// PI Address
        public string Address2 { get; set; }// HO
        public string Address3 { get; set; }// Factory
        
        public string Phone { get; set; }
        public string Phone2{get;set;}
        public string TIN {get;set;}
        public string VAT  {get;set;}
        public string GroupName {get;set;}
        public string Email { get; set; }
        public string ShortName { get; set; }
        public bool Activity { get; set; }
      
        public double ActualBalance { get; set; }

        public bool IsNeedCutOff { get; set; }
    
        public string Fax { get; set; }
      
        public string Abbreviation { get; set; }
        public string Params { get; set; }
       public int  CountryID { get; set; }
       public string    CountryShortName { get; set; }
       public string    CountryCode { get; set; }
       public string CountryName { get; set; }
        public string Note { get; set; }
        public int ContractorTypeInInt { get; set; }
        public int LastUpdateUserID { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string UpdateByName { get; set; }
        public string ErrorMessage { get; set; }
        public string NameCode
        {
            get
            {
                 string sNameCode = "";
                if (this.ContractorID > 0)
                {
                    sNameCode = this.Name + "[" + this.ContractorID.ToString() + "]";
                }
                else
                {
                    sNameCode = this.Name;
                }
                return sNameCode;
            }           
        }
        public string ActiveInActiveInString
        {
            get
            {
                 if(this.Activity== true)
                 {
                     return "Active";
                 }
                 else{
                     return "In Active";
                 }
            }
        }

        public string IsNeedCutOffInString
        {
            get
            {
                if (this.IsNeedCutOff == true)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }

        #endregion

        #region Derived Property
        public List<PartyWiseBank> PartyWiseBanks { get; set; }
        public List<IssueFigure> IssueFigures { get; set; }
        public List<ContractorType> ContractorTypes { get; set; }
        public List<ContactPersonnel> ContactPersonnels { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }
        public List<BUWiseParty> BUWisePartys { get; set; }
        public List<BuyerConcern> BuyerConcerns { get; set; }
        public bool MultipleItemReturn { get; set; }
        public List<Contractor> Contractors { get; set;}

        public string CountryCodeWithName
        {
            get
            {
                if (this.CountryID != 0)
                {
                    return this.CountryName + "[" + this.CountryCode + "]";
                }
                else
                {
                    return "";
                }
            }
        }
        public string NameType
        {
            get
            {
                return this.Name + "[" + this.ContractorType.ToString() + "]";
            }
        }
        public string LastUpdateDateTimeSt
        {
            get
            {
                if (this.LastUpdateDateTime == DateTime.MinValue) return " ";
                return this.LastUpdateDateTime.ToString("dd MMM yyyy");
            }
        }
        public string ContractorTypeInString
        {
            get
            {
                return this.ContractorType.ToString();
            }
        }
        public int IsActiveInInt
        {
            get
            {
                if (this.Activity == true)
                    return 1;
                else
                    return 0;
            }

        }
        #endregion

        #region Functions
        public static List<Contractor> GetsByName(string sName, int nContractorType, int nUserID)
        {
            return Contractor.Service.GetsByName(sName, nContractorType, nUserID);
        }
        public static List<Contractor> GetsByNamenType(string sName, string nContractorType, int nBUID, int nUserID)
        {
            return Contractor.Service.GetsByNamenType(sName, nContractorType, nBUID, nUserID);
        }
        public Contractor CommitActivity(int id, bool ActiveInActive, int nUserID)
        {
            return Contractor.Service.CommitActivity(id, ActiveInActive, nUserID);
        }        
        public static List<Contractor> GetsForAccount(int nContractorType, int nReferenceType, int nUserID)
        {
            return Contractor.Service.GetsForAccount(nContractorType, nReferenceType, nUserID);
        }
        public Contractor Get(int id, int nUserID)
        {
            return Contractor.Service.Get(id, nUserID);
        }
        public Contractor Save(int nUserID)
        {
            return Contractor.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return Contractor.Service.Delete(id, nUserID);
        }
        public static List<Contractor> Gets(int nUserID)
        {
            return Contractor.Service.Gets(nUserID);
        }
        public static List<Contractor> GetsByBU(int nBUID, int nUserID)
        {
            return Contractor.Service.GetsByBU(nBUID, nUserID);
        }
        public static List<Contractor> Gets(string sSQL, int nUserID)
        {
            return Contractor.Service.Gets(sSQL, nUserID);
        }
        public static List<Contractor> Gets(int eContractorType, int nUserID)
        {
            return Contractor.Service.Gets(eContractorType, nUserID);
        }

        public static string IDInString(List<Contractor> lstContractor)
        {
            string sReturn = "";
            foreach (Contractor oItem in lstContractor)
            {
                sReturn = sReturn + oItem.ContractorID.ToString() + ",";
            }
            if (sReturn == "") return "";
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }

        public Contractor UpdateNeedCutOff(long nUserID)
        {
            return Contractor.Service.UpdateNeedCutOff(this, nUserID);
        }
        public Contractor UpdateCountry(long nUserID)
        {
            return Contractor.Service.UpdateCountry(this, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IContractorService Service
        {
            get { return (IContractorService)Services.Factory.CreateService(typeof(IContractorService)); }
        }
        #endregion

        
    }
    #endregion

    #region IContractor interface
    public interface IContractorService
    {
        Contractor Get(int id, int nUserID);
        Contractor CommitActivity(int id, bool ActiveInActive, int nUserID);     
        List<Contractor> Gets(int nUserID);
        List<Contractor> GetsByBU(int nBUID, int nUserID);
        List<Contractor> Gets(string sSQL, int nUserID);
        string Delete(int id, int nUserID);
        Contractor Save(Contractor oContractor, int nUserID);
        List<Contractor> Gets(int eContractorType, int nUserID);
        List<Contractor> GetsByName(string sName, int eContractorType, int nUserID);
        List<Contractor> GetsByNamenType(string sName, string eContractorType, int nBUID, int nUserID);        
        List<Contractor> GetsForAccount(int nContractorType, int nReferenceType, int nUserID);

        Contractor UpdateNeedCutOff(Contractor oContractor, Int64 nUserID);
        Contractor UpdateCountry(Contractor oContractor, Int64 nUserID);
        
    }
    #endregion
}