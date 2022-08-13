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
    #region IntegrationSetupDetail
    [DataContract]
    public class IntegrationSetupDetail : BusinessObject
    {
        public IntegrationSetupDetail()
        {
            IntegrationSetupDetailID = 0;
            IntegrationSetupID = 0;
            VoucherTypeID = 0;
            BusinessUnitSetup = "";
            VoucherDateSetup = "";           
            NarrationSetup = "";
            ReferenceNoteSetup = "";
            UpdateTable = "";
            KeyColumn = "";
            Note = "";
            VoucherName = "";
            ErrorMessage = "";
            DebitCreditSetups = new List<DebitCreditSetup>();
            DataCollectionSetups = new List<DataCollectionSetup>();
            VoucherTypes = new List<VoucherType>();
        }

        #region Properties
        
        public int IntegrationSetupDetailID { get; set; }
        
        public int IntegrationSetupID { get; set; }
        
        public int VoucherTypeID { get; set; }

        public string BusinessUnitSetup { get; set; }
        
        public string VoucherDateSetup { get; set; }       
        
        public string NarrationSetup { get; set; }
        
        public string ReferenceNoteSetup { get; set; }
        
        public string UpdateTable { get; set; }
        
        public string KeyColumn { get; set; }
        
        public string Note { get; set; }
        
        public string VoucherName { get; set; }
        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        
        public List<DebitCreditSetup> DebitCreditSetups { get; set; }
        public DebitCreditSetup DebitCreditSetup { get; set; }
        
        public List<DataCollectionSetup> DataCollectionSetups { get; set; }
        public List<VoucherType> VoucherTypes { get; set; }
        #endregion

        #region Functions
        public static List<IntegrationSetupDetail> Gets(int nIntegrationSetupID, long nUserID)
        {
            return IntegrationSetupDetail.Service.Gets(nIntegrationSetupID, nUserID);            
        }
        public static List<IntegrationSetupDetail> Gets(string sSQL, long nUserID)
        {
            return IntegrationSetupDetail.Service.Gets(sSQL, nUserID);            
        }
        public IntegrationSetupDetail Get(int nIntegrationSetupDetailID, long nUserID)
        {
            return IntegrationSetupDetail.Service.Get(nIntegrationSetupDetailID, nUserID);            
        }
        public IntegrationSetupDetail Save(long nUserID)
        {
            return IntegrationSetupDetail.Service.Save(this, nUserID);            
        }
        #endregion

        #region ServiceFactory
        internal static IIntegrationSetupDetailService Service
        {
            get { return (IIntegrationSetupDetailService)Services.Factory.CreateService(typeof(IIntegrationSetupDetailService)); }
        }
        #endregion
        
    }
    #endregion

    #region IIntegrationSetupDetail interface
    
    public interface IIntegrationSetupDetailService
    {
        
        IntegrationSetupDetail Get(int nIntegrationSetupDetailID, Int64 nUserID);
        
        List<IntegrationSetupDetail> Gets(int nIntegrationSetupID, Int64 nUserID);
        
        List<IntegrationSetupDetail> Gets(string sSQL, Int64 nUserID);
        
        IntegrationSetupDetail Save(IntegrationSetupDetail oIntegrationSetupDetail, Int64 nUserID);
    }
    #endregion
}
