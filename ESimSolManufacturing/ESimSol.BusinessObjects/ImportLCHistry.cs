using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ImportLCHistry
    public class ImportLCHistry : BusinessObject
    {
        #region  Constructor
        public ImportLCHistry()
        {
           
           
        }
        #endregion

        #region Properties
        public int ImportLCHistryID { get; set; }
        public int ImportLCID { get; set; }
        public EnumLCCurrentStatus CurrentStatus { get; set; }
        public EnumLCCurrentStatus PrevioustStatus { get; set; }
        public DateTime OperationDate { get; set; }
        public string Note { get; set; }
        public int DBUserID { get; set; }
        public string UserName { get; set; }

        
        #endregion

        #region Derived Properties
        
        
       
         #endregion

        #region Functions
        public ImportLCHistry Get(int nPLCID, EnumLCCurrentStatus eEvent, int nUserID)
        {
            return ImportLCHistry.Service.Get( nPLCID,  eEvent, nUserID);
        }
        public ImportLCHistry Save(int nUserID)
        {
            return ImportLCHistry.Service.Save(this, nUserID);
        }
        public static List<ImportLCHistry> Gets(int nPLCID, int nUserID)
        {
            return ImportLCHistry.Service.Gets(nPLCID, nUserID);
        }
        public static List<ImportLCHistry> Gets(int nLCID, string eEvent, int nUserID)
        {
            return ImportLCHistry.Service.Gets( nLCID, eEvent, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IImportLCHistryService Service
        {
            get { return (IImportLCHistryService)Services.Factory.CreateService(typeof(IImportLCHistryService)); }
        }
        #endregion
    }
    #endregion

    #region IImportLCHistry interface
    public interface IImportLCHistryService
    {
        ImportLCHistry Get(int nPLCID, EnumLCCurrentStatus eEvent,Int64 nUserId);
        List<ImportLCHistry> Gets(int nPLCID, Int64 nUserId);
        List<ImportLCHistry> Gets(int nLCID, string eStatus, Int64 nUserId);
        ImportLCHistry Save(ImportLCHistry oImportLCHistry, Int64 nUserId);
    }
    #endregion
}
