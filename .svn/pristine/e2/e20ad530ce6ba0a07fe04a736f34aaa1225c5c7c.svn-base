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
    #region ImportPIHistory
    [DataContract]
    public class ImportPIHistory : BusinessObject
    {

        #region  Constructor
        public ImportPIHistory()
        {
            ImportPIHistoryID = 0;
            ImportPIID = 0;
            State = EnumImportPIState.Initialized;
            PreviousState = EnumImportPIState.Initialized;
            Note="";
            ErrorMsg = "";
            DBUserID = 0;
            DateTime = DateTime.Now;
        }
        #endregion

        #region Properties
        [DataMember]
        public int ImportPIHistoryID { get; set; }
        [DataMember]
        public int ImportPIID { get; set; }
        [DataMember]
        public EnumImportPIState State { get; set; }
        [DataMember]
        public EnumImportPIState PreviousState { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string NoteSystem { get; set; }
        [DataMember]
        public int DBUserID { get; set; }
        [DataMember]
        public DateTime DateTime { get; set; }
        [DataMember]
        public string ErrorMsg { get; set; }
        
        #endregion
        #region Derived Properties
        [DataMember]
        public string UserName { get; set; }
        public List<ImportPIHistory> ImportPIHistorys { get; set; }
        public string DateTimeInString { get{return DateTime.ToString("dd MMM yyyy hh:mm tt");} }
        public string StateInString { get { return State.ToString(); } }
        public string PreviousStateInString { get { return PreviousState.ToString(); } }

       
         #endregion

        #region Functions
        public ImportPIHistory Get(int nId, Int64 nUserID)
        {
            return ImportPIHistory.Service.Get(nId, nUserID);
        }
      
        #region  Collection Functions

        public static List<ImportPIHistory> Gets(int nImportPIID, Int64 nUserID)
        {
            return ImportPIHistory.Service.Gets(nImportPIID, nUserID);
        }
     

     
        #endregion
        #endregion

        #region ServiceFactory

        internal static IImportPIHistoryService Service
        {
            get { return (IImportPIHistoryService)Services.Factory.CreateService(typeof(IImportPIHistoryService)); }
        }

        #endregion
    }
    #endregion

    

    #region IImportPIHistory interface
    [ServiceContract]
    public interface IImportPIHistoryService
    {
        ImportPIHistory Get(int id, Int64 nUserID);
        List<ImportPIHistory> Gets(int nImportPIID, Int64 nUserID);
     
        
    }
    #endregion
}
