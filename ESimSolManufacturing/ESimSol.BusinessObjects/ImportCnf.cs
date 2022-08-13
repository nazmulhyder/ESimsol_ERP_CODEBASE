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
    public class ImportCnf
    {
        public ImportCnf()
        {
            ImportCnfID = 0;
            FileNo = "";
            Note = "";
            ContractorName = "";
            SendDate = DateTime.MinValue;
            ContractorID = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int ImportCnfID { get; set; }
        public int ImportInvoiceID { get; set; }
        public int ContractorID { get; set; }
        public DateTime SendDate { get; set; }
        public string FileNo { get; set; }
        public string Note { get; set; }
        public string ContractorName { get; set; }
        public string ErrorMessage { get; set; }
        public string SendDateSt
        {
            get
            {
                if (this.SendDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return SendDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion

        #region Functions
        public ImportCnf Get(int nID, long nUserID)
        {
            return ImportCnf.Service.Get(nID, nUserID);
        }
        public ImportCnf GetBy(int nImportInvoiceID, long nUserID)
        {
            return ImportCnf.Service.GetBy(nImportInvoiceID, nUserID);
        }
        public ImportCnf Save(long nUserID)
        {
            return ImportCnf.Service.Save(this, nUserID);
        }
      
        public string Delete(int nId, long nUserID)
        {
            return ImportCnf.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IImportCnfService Service
        {
            get { return (IImportCnfService)Services.Factory.CreateService(typeof(IImportCnfService)); }
        }
        #endregion
    }

    #region IImportCnf interface
    public interface IImportCnfService
    {
        ImportCnf GetBy(int nImportInvoiceID, long nUserID);
        List<ImportCnf> Gets(long nUserID);
        ImportCnf Save(ImportCnf oImportCnf, long nUserID);
        ImportCnf Get(int nID, long nUserID);
        string Delete(int id, long nUserID);
    }
    #endregion
}
