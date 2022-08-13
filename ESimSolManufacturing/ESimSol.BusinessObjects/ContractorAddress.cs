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
    #region ContractorPersonal
    
    public class ContractorAddress : BusinessObject
    {
        public ContractorAddress()
        {
            ContractorAddressID = 0;
            ContractorID=0;
            AddressType = EnumAddressType.None;
            AddressTypeInt = (int)EnumAddressType.None;
            Address = "";
            Note= "";
            ContractorName = "";
            ErrorMessage = "";
            ContactPersonnels = new List<ContactPersonnel>();
        }

        #region Properties
        
        public int ContractorAddressID { get; set; }
        
        public int ContractorID { get; set; }
        public int AddressTypeInt { get; set; }
        public EnumAddressType AddressType { get; set; }
        public List<ContactPersonnel> ContactPersonnels { get; set; }
        
        public string Address { get; set; }
         
        public string Note { get; set; }
    
        public string ContractorName { get; set; }
        
        public string ErrorMessage { get; set; }



       
        #endregion

        #region Derived Property
        public string SelectedContractor { get; set; }

        public string AddressTypeSt
        {
            get
            {
                return this.AddressType.ToString();
            }
        }
        public string AddressAndType
        {
            get
            {
                return this.AddressType.ToString() + ": " + this.Address;
            }
        }
        #endregion


        #region Functions
        public  ContractorAddress Get(int nId, long nUserID)
        {
            return ContractorAddress.Service.Get(nId, nUserID);
        }
        public ContractorAddress Save(long nUserID)
        {
            return ContractorAddress.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return ContractorAddress.Service.Delete(nId, nUserID);
        }
        public static List<ContractorAddress> Gets(long nUserID)
        {
            return ContractorAddress.Service.Gets(nUserID);
        }

        public static List<ContractorAddress> Gets(string sSQL, long nUserID)
        {
            return ContractorAddress.Service.Gets(sSQL,nUserID);
        }

        public static List<ContractorAddress> GetsByContractor(int nContractorID, long nUserID)
        {
            return ContractorAddress.Service.GetsByContractor(nContractorID,nUserID);
        }
        public static List<ContractorAddress> GetsBy(int nContractorID, string sAddtessType,long nUserID)
        {
            return ContractorAddress.Service.GetsBy(nContractorID,sAddtessType, nUserID);
        }


        #endregion

        #region Non DB Property
        public static string IDInString(List<ContractorAddress> oContractorAddresss)
        {
            string sReturn = "";
            foreach (ContractorAddress oItem in oContractorAddresss)
            {
                sReturn = sReturn + oItem.ContractorAddressID.ToString() + ",";
            }
            if (sReturn == "") return "";
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

        #region ServiceFactory

        internal static IContractorAddressService Service
        {
            get { return (IContractorAddressService)Services.Factory.CreateService(typeof(IContractorAddressService)); }
        }
        #endregion
    }
    #endregion

    

    #region IContractorPersonal interface
    
    public interface IContractorAddressService
    {
        ContractorAddress Get(int id, long nUserID);
        string Delete(int id, long nUserID);
        ContractorAddress Save(ContractorAddress oContractorPersonal, long nUserID);
        List<ContractorAddress> Gets(long nUserID);
        List<ContractorAddress> Gets(string sSQL, long nUserID);
        List<ContractorAddress> GetsByContractor(int nContractorID, long nUserID);
        List<ContractorAddress> GetsBy(int nContractorID, string sAddtessType,long nUserID);
     
    }
    #endregion
}