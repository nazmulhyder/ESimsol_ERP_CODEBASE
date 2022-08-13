using ICS.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    #region ImportLCCode
    public class ImportLCCode : BusinessObject
    {
        public ImportLCCode()
        {
            ImportLCCodeID = 0;
            LCCode = "";
            LCNature = "";
            Remarks = "";
            DBUserID = 0;
            DBServerDateTime = DateTime.Now;
            ErrorMessage = "";
        }

        #region Properties
        public int ImportLCCodeID { get; set; }
        public string LCCode { get; set; }
        public string LCNature { get; set; }       
        public string Remarks { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string ErrorMessage { get; set; }
        #endregion


        #region Functions
        public static List<ImportLCCode> Gets(long nUserID)
        {
            return ImportLCCode.Service.Gets(nUserID);
        }
        public ImportLCCode Get(int id, long nUserID)
        {
            return ImportLCCode.Service.Get(id, nUserID);
        }

        public ImportLCCode Save(long nUserID)
        {
            return ImportLCCode.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ImportLCCode.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IImportLCCodeService Service
        {
            get { return (IImportLCCodeService)Services.Factory.CreateService(typeof(IImportLCCodeService)); }
        }
        #endregion
    }
    #endregion

    #region IImportLCCode interface
    public interface IImportLCCodeService
    {
        List<ImportLCCode> Gets(Int64 nUserID);
        ImportLCCode Get(int id, Int64 nUserID);
        ImportLCCode Save(ImportLCCode oImportLCCode, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
    }
    #endregion
}