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
    #region ImportLC_Clause
    public class ImportLC_Clause : BusinessObject
    {
        #region  Constructor
        public ImportLC_Clause()
        {
            ImportLC_ClauseID = 0;
            ImportLCID = 0;
            ImportLCLogID = 0;
            ClauseID = 0;
            Caption = "";
            LCCurrentStatusInt = 0;
            LCCurrentStatus = EnumLCCurrentStatus.LC_Open;
        }
        #endregion

        #region Properties
        public int ImportLC_ClauseID { get; set; }
        public int ImportLCID { get; set; }
        public int ImportLCLogID { get; set; }
        public int ClauseID { get; set; }
        public string Clause { get; set; }
        public string Caption { get; set; }
        public EnumLCCurrentStatus LCCurrentStatus { get; set; }
        public int LCCurrentStatusInt { get; set; }
        public string ErrorMessage { get; set; }
        
        #endregion

        #region Functions
        public ImportLC_Clause Save(List<ImportLC_Clause> oImportLC_Clauses,int nUserID)
        {
            return ImportLC_Clause.Service.Save(oImportLC_Clauses, nUserID);
        }
        public static List<ImportLC_Clause> SaveAll(List<ImportLC_Clause> oImportLC_Clauses,ImportLC oImportLC, long nUserID)
        {
            return ImportLC_Clause.Service.SaveAll(oImportLC_Clauses, oImportLC, nUserID);
        }
        public static List<ImportLC_Clause> GetsBySQL(string sSQL, int nUserID)
        {
            return ImportLC_Clause.Service.GetsBySQL(sSQL, nUserID);
        }
        public static List<ImportLC_Clause> Gets(int nImportLCID, int nImportLCLogID, int nLCCurrentStatus, int nUserID)
        {
            return ImportLC_Clause.Service.Gets( nImportLCID,  nImportLCLogID,  nLCCurrentStatus, nUserID);
        }
        public static List<ImportLC_Clause> GetsByImportLCID(int nImportLCID, int nUserID)
        {
            return ImportLC_Clause.Service.GetsByImportLCID(nImportLCID, nUserID);
        }
       
        #endregion

        //#region NonDB Functions
      
       
        
        #region ServiceFactory
        internal static IImportLC_ClauseService Service
        {
            get { return (IImportLC_ClauseService)Services.Factory.CreateService(typeof(IImportLC_ClauseService)); }
        }
        #endregion
    }
    #endregion

    
    #region IImportLC_Clause interface
    
    public interface IImportLC_ClauseService
    {
        List<ImportLC_Clause> Gets(int nImportLCID, int nImportLCLogID, int nLCCurrentStatus, Int64 nUserId);
        ImportLC_Clause Save(List<ImportLC_Clause> oImportLC_Clauses, Int64 nUserId);
        List<ImportLC_Clause> SaveAll(List<ImportLC_Clause> oImportLC_Clauses, ImportLC oImportLC, long nUserID);
        List<ImportLC_Clause> GetsByImportLCID(int nImportLCID, Int64 nUserId);
        List<ImportLC_Clause> GetsBySQL(string sSQL, Int64 nUserId);
    }
    #endregion
    
}
