using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Data;

namespace ESimSol.BusinessObjects
{

    #region ImportFormat
    [DataContract]
    public class ImportFormat : BusinessObject
    {
        public ImportFormat()
        {
            ImportFormatID = 0;
            ImportFormatType = EnumImportFormatType.None;
            AttatchmentName = "";
            AttatchFile = null;
            FileType = "";
            Remarks = "";
            ErrorMessage = "";
            ImportFormats = new List<ImportFormat>();
        }

        #region Properties
       
        public int ImportFormatID { get; set; }
       
        public EnumImportFormatType ImportFormatType { get; set; }
       
        public string AttatchmentName { get; set; }
       
        public byte[] AttatchFile { get; set; }
        
       
        public string FileType { get; set; }
       
        public string Remarks { get; set; }
        
       
        public string ErrorMessage { get; set; }
        #endregion

        #region derived property
       
        public List<ImportFormat> ImportFormats { get; set; }
        public List<EnumObject> ImportFormatTypes { get; set; }
        public int ImportFormatTypeInInt { get { return (int)this.ImportFormatType; } }
        public string ImportFormatTypeInString { get { return EnumObject.jGet(this.ImportFormatType); } }

        public string AttatchFileinString { get { return ImportFormatID.ToString(); } }
        #endregion

        #region Functions

        public static List<ImportFormat> Gets(int nUserID)
        {
            return ImportFormat.Service.Gets(nUserID);
        }
        public ImportFormat Get(int id, int nUserID)
        {
            return ImportFormat.Service.Get(id, nUserID);
        }
        public ImportFormat Save(int nUserID)
        {
            return ImportFormat.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return ImportFormat.Service.Delete(id, nUserID);
        }
        public static List<ImportFormat> Gets(string sSQL, int nUserID)
        {
            return ImportFormat.Service.Gets(sSQL, nUserID);
        }

        public static ImportFormat GetByType(EnumImportFormatType eIFT, int nUserID) 
        {
            return ImportFormat.Service.GetByType(eIFT, nUserID);
        }
        public static ImportFormat GetWithAttachFile(int id, int nUserID) 
        {
            return ImportFormat.Service.GetWithAttachFile(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IImportFormatService Service
        {
            get { return (IImportFormatService)Services.Factory.CreateService(typeof(IImportFormatService)); }
        }
        #endregion
    }
    #endregion


    #region IImportFormatService interface
    
    public interface IImportFormatService
    {
        List<ImportFormat> Gets(string sSQL, int nUserID);
        List<ImportFormat> Gets(int nUserID);
        

        ImportFormat Get(int id, int nUserID);   //ImportFormatID
        ImportFormat GetByType(EnumImportFormatType eIFT, int nUserID);
        ImportFormat GetWithAttachFile(int id, int nUserID);   //ImportFormatID

        ImportFormat Save(ImportFormat oImportFormat, int nUserID);

        string Delete(int id, int nUserID);
    }
    #endregion
    
   
}
