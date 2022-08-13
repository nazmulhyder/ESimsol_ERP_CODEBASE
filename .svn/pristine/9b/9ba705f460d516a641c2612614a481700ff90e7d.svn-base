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
    #region ExportBankForwarding
    [DataContract]
    public class ExportBankForwarding : BusinessObject
    {
        public ExportBankForwarding()
        {
            ExportBankForwardingID = 0;
            Name_Print = "";
            Copies = 1;
            Name_Bank = "";
            ExportBillID =0;
            Selected = false;
            Copies_Original = 1;
            ErrorMessage = "";
            ExportBankForwardings = new List<ExportBankForwarding>();
        }

        #region Properties
        public int ExportBankForwardingID { get; set; }
        public string Name_Print { get; set; }
        public string Name_Bank { get; set; }
        public int Copies { get; set; }
        public int Copies_Original { get; set; }
        public int ExportBillID { get; set; }
        public bool Selected { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public List<ExportBankForwarding> ExportBankForwardings { get; set; }
        #endregion

        #region Functions
     
        public ExportBankForwarding Get(int nId, Int64 nUserID)
        {
            return ExportBankForwarding.Service.Get(nId, nUserID);
        }
  
        public static List<ExportBankForwarding> Gets(int nExportBillID, int nUserID)
        {
            return ExportBankForwarding.Service.Gets(nExportBillID, nUserID);
        }
     
        public ExportBankForwarding Save(Int64 nUserID)
        {
            return ExportBankForwarding.Service.Save(this, nUserID);
        }
        public string Delete(int nExportBankForwardingID, Int64 nUserID)
        {
            return ExportBankForwarding.Service.Delete(nExportBankForwardingID, nUserID);
        }
        public ExportBankForwarding DeleteBYExportBillID(int nExportBillID, Int64 nUserID)
        {
            return ExportBankForwarding.Service.DeleteBYExportBillID(nExportBillID, nUserID);
        }
        #endregion

        #region ServiceFactory

     
        internal static IExportBankForwardingService Service
        {
            get { return (IExportBankForwardingService)Services.Factory.CreateService(typeof(IExportBankForwardingService)); }
        }
        #endregion
    }
    #endregion

    #region IExportBankForwarding interface
    
    public interface IExportBankForwardingService
    {
        ExportBankForwarding Get(int id, Int64 nUserID);
        List<ExportBankForwarding> Gets(int nExportBillID, Int64 nUserID);
        string Delete(int nExportBankForwardingID, Int64 nUserID);
        ExportBankForwarding Save(ExportBankForwarding oExportBankForwarding, Int64 nUserID);
        ExportBankForwarding DeleteBYExportBillID(int nExportBillID, Int64 nUserID);
        
    }
    #endregion
}
