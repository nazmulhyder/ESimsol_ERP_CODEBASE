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
    
    public class ContractorType : BusinessObject
    {
        public ContractorType()
        {
           
            ContractorID=0;

            ContractorTypeID = 0;
            ContractorID = 0;
            ErrorMessage = "";
         
        }

        #region Properties
        
        public int ContractorTypeID { get; set; }
        
        public int ContractorID { get; set; }
        public string TypeName { get; set; }
        public string ErrorMessage { get; set; }



       
        #endregion

        #region Derived Property
    

  
        #endregion


        #region Functions
        public  ContractorType Get(int nId, long nUserID)
        {
            return ContractorType.Service.Get(nId, nUserID);
        }
        public ContractorType Save(long nUserID)
        {
            return ContractorType.Service.Save(this, nUserID);
        }

        public string Delete(int nId, long nUserID)
        {
            return ContractorType.Service.Delete(nId, nUserID);
        }
 

        public static List<ContractorType> Gets(string sSQL, long nUserID)
        {
            return ContractorType.Service.Gets(sSQL,nUserID);
        }

        public static List<ContractorType> Gets(int nContractorID, long nUserID)
        {
            return ContractorType.Service.Gets(nContractorID, nUserID);
        }
      

        #endregion

        #region Non DB Property
        public static string IDInString(List<ContractorType> oContractorTypes)
        {
            string sReturn = "";
            foreach (ContractorType oItem in oContractorTypes)
            {
                sReturn = sReturn + oItem.ContractorTypeID.ToString() + ",";
            }
            if (sReturn == "") return "";
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

        #region ServiceFactory

        internal static IContractorTypeService Service
        {
            get { return (IContractorTypeService)Services.Factory.CreateService(typeof(IContractorTypeService)); }
        }
        #endregion
    }
    #endregion

    

    #region IContractorPersonal interface
    
    public interface IContractorTypeService
    {
        ContractorType Get(int id, long nUserID);
        string Delete(int id, long nUserID);
        ContractorType Save(ContractorType oContractorPersonal, long nUserID);
        List<ContractorType> Gets(string sSQL, long nUserID);
        List<ContractorType> Gets(int nContractorID, long nUserID);
        
     
    }
    #endregion
}