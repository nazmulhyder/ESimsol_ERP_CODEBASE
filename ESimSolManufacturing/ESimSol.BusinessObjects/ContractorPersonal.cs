using System;
using System.IO;
using ICS.Base.Client.BOFoundation;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Base.Client.ServiceVessel;

namespace ESimSol.BusinessObjects
{
    #region ContractorPersonal
    [DataContract]
    public class ContractorPersonal : BusinessObject
    {
        public ContractorPersonal()
        {
            ContractorPersonalID = 0;
            ContractorID=0;
            Name = "";
            Address = "";
            Phone = "";
            Email = "";
            Note= "";
        }

        #region Properties
        [DataMember]
        public int ContractorPersonalID { get; set; }
        [DataMember]
        public int ContractorID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Note { get; set; }
       
        #endregion

        #region Derived Property
        public string SelectedContractor { get; set; }
        public List<ContractorPersonal> ContractorPersonnelForSelectedContractor { get; set; }
        #endregion


        #region Functions

        public static List<ContractorPersonal> Gets(Guid wcfSessionid)
        {
            return (List<ContractorPersonal>)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "Gets")[0];
        }

        public static List<ContractorPersonal> GetsByContractor(int nContractorID, Guid wcfSessionid)
        {
            return (List<ContractorPersonal>)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "GetsByContractor", nContractorID)[0];
        }

        public ContractorPersonal Get(int id, Guid wcfSessionid)
        {
            return (ContractorPersonal)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "Get", id)[0];
        }

        public ContractorPersonal Save(Guid wcfSessionid)
        {
            return (ContractorPersonal)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "Save", this)[0];
        }

        public bool Delete(int id, Guid wcfSessionid)
        {
            return (bool)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "Delete", id)[0];
        }

        #endregion

        #region ServiceFactory

        internal static Type ServiceType
        {
            get
            {
                return typeof(IContractorPersonalService);
            }
        }
        #endregion
    }
    #endregion

    #region ContractorPersonals
    public class ContractorPersonals : IndexedBusinessObjects
    {
        #region Collection Class Methods
        public void Add(ContractorPersonal item)
        {
            base.AddItem(item);
        }
        public void Remove(ContractorPersonal item)
        {
            base.RemoveItem(item);
        }
        public ContractorPersonal this[int index]
        {
            get { return (ContractorPersonal)GetItem(index); }
        }
        public int GetIndex(int id)
        {
            return base.GetIndex(new ID(id));
        }
        #endregion
    }
    #endregion

    #region IContractorPersonal interface
    [ServiceContract]
    public interface IContractorPersonalService
    {
        [OperationContract]
        ContractorPersonal Get(int id, Int64 nUserID);
        [OperationContract]
        List<ContractorPersonal> Gets(Int64 nUserID);
        [OperationContract]
        List<ContractorPersonal> GetsByContractor(int nContractorID, Int64 nUserID);
        [OperationContract]
        bool Delete(int id, Int64 nUserID);
        [OperationContract]
        ContractorPersonal Save(ContractorPersonal oContractorPersonal, Int64 nUserID);
    }
    #endregion
}