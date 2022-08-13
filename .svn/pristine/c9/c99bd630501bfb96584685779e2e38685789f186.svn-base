using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region ContractorPersonal
    public class ContactPersonnel : BusinessObject
    {
        public ContactPersonnel()
        {
            ContactPersonnelID = 0;
            ContractorID=0;
            WorkingUnitID = 0;
            Name = "";
            Address = "";
            Phone = "";
            Email = "";
            CPGroupID = 0;
            Note= "";
            RefUpdated = false;
            CommissionAmount=0;
            CommissionApproveAmount=0;
            PayableAmount=0;
            PaidAmount = 0;
            BUID = 0;
            ContractorName = "";
            ErrorMessage = "";                
            Photo=null;            
            Signature=null;
            Designation="";
            IsPhoto = false;
            PhotpInBase64String = "";
            IsAuthenticate=false;
            BusinessUnits = new List<BusinessUnit>();
            Contractors = new List<Contractor>();
            //DesignationInEnum = EnumContactPersonDesignation.Select_Designation;
        }

        #region Properties
        public int ContactPersonnelID { get; set; }
        public int ContractorID { get; set; }
        public int WorkingUnitID { get; set; }
        public int BUID { get; set; }
        public string Name { get; set; }
        //public EnumContactPersonDesignation DesignationInEnum { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int CPGroupID { get; set; }
        public string Note { get; set; }
        public bool RefUpdated { get; set; }
        public double CommissionAmount { get; set; }
        public double CommissionApproveAmount { get; set; }
        public double PayableAmount { get; set; }
        public double PaidAmount { get; set; }
        public string ContractorName { get; set; }
        public string ErrorMessage { get; set; }        
        public byte[] Photo { get; set; }        
        public byte[] Signature { get; set; }
        public string Designation { get; set; }
        public bool IsPhoto { get; set; }
        public bool IsAuthenticate { get; set; }
        #endregion

        #region Derived Property
        public string ContactPersonnelIDs { get; set; }
        public string SelectedContractor { get; set; }
        public List<ContactPersonnel> ContractorPersonnelForSelectedContractor { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }
        public List<Contractor> Contractors { get; set; }
        public string NameWithContractor
        {
            get
            {
                return this.Name + "[" + this.ContractorName + "]";
            }
        }
        public string PhotpInBase64String { get; set; }
        public string IsAuthenticateInString
        {
            get
            {
                if (IsAuthenticate)
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

        #region Functions
        public static ContactPersonnel GetWithImage(int id, int nUserID)
        {
            return ContactPersonnel.Service.GetWithImage(id, nUserID);
        }
        public string DeleteImage(int nUserID)
        {
            return ContactPersonnel.Service.DeleteImage(this,nUserID);
        }
        public static List<ContactPersonnel> Gets(int nUserID)
        {
            return ContactPersonnel.Service.Gets(nUserID);
        }
        public static List<ContactPersonnel> GetsByContractor(int nContractorID, int nUserID)
        {
            return ContactPersonnel.Service.GetsByContractor(nContractorID, nUserID);
        }
        public static List<ContactPersonnel> GetsOnlyCommission(string sContractorIDs, int nUserID)
        {
            return ContactPersonnel.Service.GetsOnlyCommission(sContractorIDs, nUserID);
        }
        public ContactPersonnel Get(int id, int nUserID)
        {
            return ContactPersonnel.Service.Get(id, nUserID);
        }
        public ContactPersonnel Save(int nUserID)
        {
            return ContactPersonnel.Service.Save(this, nUserID);
        }
        public ContactPersonnel IUDContractor(EnumDBOperation eEnumDBOperation, long nUserId)
        {
            return ContactPersonnel.Service.IUDContractor(this, eEnumDBOperation, nUserId);
        }

        public ContactPersonnel MergeCP(int nUserID)
        {
            return ContactPersonnel.Service.MergeCP(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return ContactPersonnel.Service.Delete(id, nUserID);
        }
        public static List<ContactPersonnel> Gets(int nContractorID, int nUserID)
        {
            return ContactPersonnel.Service.Gets(nContractorID, nUserID);
        }
        public static List<ContactPersonnel> Gets(string sSQL, int nUserID)
        {
            return ContactPersonnel.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region Non DB Property
        public static string IDInString(List<ContactPersonnel> oContactPersonnels)
        {
            string sReturn = "";
            foreach (ContactPersonnel oItem in oContactPersonnels)
            {
                sReturn = sReturn + oItem.ContactPersonnelID.ToString() + ",";
            }
            if (sReturn == "") return "";
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

     
        #region ServiceFactory
        internal static IContactPersonnelService Service
        {
            get { return (IContactPersonnelService)Services.Factory.CreateService(typeof(IContactPersonnelService)); }
        }
        #endregion

    }
    #endregion

    
    #region IContractorPersonal interface
    public interface IContactPersonnelService
    {
        ContactPersonnel Get(int id, int nUserID);
        ContactPersonnel GetWithImage(int id, int nUserID);
        List<ContactPersonnel> Gets(int nUserID);
        List<ContactPersonnel> Gets(string sSQL, int nUserID);
        List<ContactPersonnel> GetsByContractor(int nContractorID, int nUserID);
        string Delete(int id, int nUserID);
        string DeleteImage(ContactPersonnel oContactPersonnel, int nUserID);
        ContactPersonnel Save(ContactPersonnel oContractorPersonal, int nUserID);
        ContactPersonnel MergeCP(ContactPersonnel oContractorPersonal, int nUserID);
        ContactPersonnel IUDContractor(ContactPersonnel oContractorPersonal, EnumDBOperation eEnumDBOperation, long nUserId);
        List<ContactPersonnel> GetsOnlyCommission(string sContractorIDs, int nUserID);
        List<ContactPersonnel> Gets(int nContractorID, int nUserID);
    }
    #endregion
}