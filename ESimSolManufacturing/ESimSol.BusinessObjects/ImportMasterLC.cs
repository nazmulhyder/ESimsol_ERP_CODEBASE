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
    public class ImportMasterLC
    {
        public ImportMasterLC()
        {
            ImportMasterLCID = 0;
            MasterLCID = 0;
            ImportLCID = 0;
            BUID = 0;
            MasterLCNo = "";
            MasterLCDate = DateTime.MinValue;
            ErrorMessage = "";
            MasterLCObj = new MasterLC();
        }

        #region Properties
        public int ImportMasterLCID { get; set; }
        public int MasterLCID { get; set; }
        public int ImportLCID { get; set; }
        public int BUID { get; set; }
        public string MasterLCNo { get; set; }
        public DateTime MasterLCDate { get; set; }
        public MasterLC MasterLCObj { get; set; }
        public string ErrorMessage { get; set; }
        private string _sMasterLCDate = "";
        public string MasterLCDateSt
        {
            get
            {
                if (this.MasterLCDate == DateTime.MinValue)
                {
                    _sMasterLCDate ="";
                }
                else
                {
                    _sMasterLCDate = this.MasterLCDate.ToString("dd MMM yyyy");
                }
                return _sMasterLCDate;
            }
        }
    
        #endregion

        #region Functions
        public static List<ImportMasterLC> Gets(long nUserID)
        {
            return ImportMasterLC.Service.Gets(nUserID);
        }
        public static List<ImportMasterLC> Gets(int nImportLCID, long nUserID)
        {
            return ImportMasterLC.Service.Gets(nImportLCID, nUserID);
        }
        public ImportMasterLC Save(long nUserID)
        {
            return ImportMasterLC.Service.Save(this, nUserID);
        }
        public ImportMasterLC SaveWithMasterLC(Int64 nUserID)
        {
            return ImportMasterLC.Service.SaveWithMasterLC(this, nUserID);
        }
        public ImportMasterLC Get(int nID, long nUserID)
        {
            return ImportMasterLC.Service.Get(nID, nUserID);
        }
        public string Delete( long nUserID)
        {
            return ImportMasterLC.Service.Delete(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IImportMasterLCService Service
        {
            get { return (IImportMasterLCService)Services.Factory.CreateService(typeof(IImportMasterLCService)); }
        }
        #endregion
    }

    #region IImportMasterLC interface
    public interface IImportMasterLCService
    {
        List<ImportMasterLC> Gets(int nImportLCID, long nUserID);
        List<ImportMasterLC> Gets(long nUserID);
        ImportMasterLC Save(ImportMasterLC oImportMasterLC, long nUserID);
        ImportMasterLC SaveWithMasterLC(ImportMasterLC oImportMasterLC, long nUserID);
        ImportMasterLC Get(int nID, long nUserID);
        string Delete(ImportMasterLC oImportMasterLC, long nUserID);
    }
    #endregion
}
