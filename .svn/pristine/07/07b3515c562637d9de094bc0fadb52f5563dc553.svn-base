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
    #region ImportLC_Request
    public class ImportLC_Request : BusinessObject
    {
        #region  Constructor
        public ImportLC_Request()
        {
            ImportLC_RequestID = 0;
            ImportLCID = 0;
            ClauseID = 0;
            Selected = true;
            ClauseText = "";
        }
        #endregion

        #region Properties
        public int ImportLC_RequestID { get; set; }
        public int ImportLCID { get; set; }
        public int ClauseID { get; set; }
        public bool Selected { get; set; }
        public string ClauseText { get; set; }
        public string Clause { get; set; }
        public string Text { get; set; }
        public string ErrorMessage { get; set; }
        
        #endregion

        #region Functions
        public ImportLC_Request Save(List<ImportLC_Request> oImportLC_Requests,int nUserID)
        {
            return ImportLC_Request.Service.Save(oImportLC_Requests, nUserID);
        }
        public static List<ImportLC_Request> GetsBySQL(string sSQL, int nUserID)
        {
            return ImportLC_Request.Service.GetsBySQL(sSQL, nUserID);
        }
        public static List<ImportLC_Request> Gets(int nImportLCID, int nUserID)
        {
            return ImportLC_Request.Service.Gets(nImportLCID, nUserID);
        }
        public static List<ImportLC_Request> GetsByImportLCID(int nImportLCID, int nUserID)
        {
            return ImportLC_Request.Service.GetsByImportLCID(nImportLCID, nUserID);
        }
       
        #endregion

        //#region NonDB Functions
        public string ImportLC_ClajseIDInString(List<ImportLC_Request> lstPLRLC)
        {
            string sReturn = "";
            foreach (ImportLC_Request oItem in lstPLRLC)
            {
                sReturn = sReturn + oItem.ClauseID.ToString() + ",";
            }
            if (sReturn.Length > 0)
            {
                sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            }
            return sReturn;
        }
        public ImportLC_Request GetImportLC_RequestForLC(int nCloseID, List<ImportLC_Request> lstPLRLC)
        {
            ImportLC_Request oImportLC_RR = new ImportLC_Request();
            foreach (ImportLC_Request oItem in lstPLRLC)
            {
                if (oItem.ClauseID == nCloseID)
                {
                    oImportLC_RR = oItem;
                    break;

                }
            }
            return oImportLC_RR;

        }
        //#endregion
        
        #region ServiceFactory
        internal static IImportLC_RequestService Service
        {
            get { return (IImportLC_RequestService)Services.Factory.CreateService(typeof(IImportLC_RequestService)); }
        }
        #endregion
    }
    #endregion

    
    #region IImportLC_Request interface
    
    public interface IImportLC_RequestService
    {
        List<ImportLC_Request> Gets(int nImportLCID, Int64 nUserId);
        ImportLC_Request Save(List<ImportLC_Request> oImportLC_Requests, Int64 nUserId);
        List<ImportLC_Request> GetsByImportLCID(int nImportLCID, Int64 nUserId);
        List<ImportLC_Request> GetsBySQL(string sSQL, Int64 nUserId);
    }
    #endregion
    
}
