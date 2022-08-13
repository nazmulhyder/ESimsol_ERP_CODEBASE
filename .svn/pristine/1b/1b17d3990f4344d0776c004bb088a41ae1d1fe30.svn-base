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
    #region ExportPartyInfo

    public class ExportPartyInfo : BusinessObject
    {

        #region  Constructor
        public ExportPartyInfo()
        {
            ExportPartyInfoID = 0;
            Name = "";
            ErrorMessage = "";
        }
        #endregion
        #region Properties
        // ExportPartyInfo 
        public int ExportPartyInfoID { get; set; }
        public string Name { get; set; }
        #region Derived Properties
        #region Derived
      
        public string ErrorMessage { get; set; }
        #endregion

        #endregion
        #endregion

        #region Functions
        public ExportPartyInfo Get(int nId, Int64 nUserID)
        {
            return ExportPartyInfo.Service.Get(nId, nUserID);
        }
        public ExportPartyInfo Save(Int64 nUserID)
        {
            return ExportPartyInfo.Service.Save(this, nUserID);
        }
        public string Delete(Int64 nUserID)
        {
            return ExportPartyInfo.Service.Delete(this, nUserID);
        }


        #region Collecion Functions
    
        public static List<ExportPartyInfo> Gets( Int64 nUserID)
        {
            return ExportPartyInfo.Service.Gets( nUserID);
        }
        public static List<ExportPartyInfo> Gets(int nContractorID,Int64 nUserID)
        {
            return ExportPartyInfo.Service.Gets(nContractorID,nUserID);
        }
        #region Non DB Functions

        public static string IDInString(List<ExportPartyInfo> oExportPartyInfos)
        {
            string sReturn = "";
            foreach (ExportPartyInfo oItem in oExportPartyInfos)
            {
                sReturn = sReturn + oItem.ExportPartyInfoID.ToString() + ",";
            }
            if (sReturn == "") return "";
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }

       
        public static int GetIndex(List<ExportPartyInfo> oExportPartyInfos, int nExportPartyInfoID)
        {
            int index = -1, i = 0;

            foreach (ExportPartyInfo oItem in oExportPartyInfos)
            {
                if (oItem.ExportPartyInfoID == nExportPartyInfoID)
                {
                    index = i; break;
                }
                i++;
            }
            return index;
        }
        //--End of Modification

        #endregion
        #endregion
        #endregion

        #region ServiceFactory

        internal static IExportPartyInfoService Service
        {
            get { return (IExportPartyInfoService)Services.Factory.CreateService(typeof(IExportPartyInfoService)); }
        }

        #endregion
    }
    #endregion



    #region IExportPartyInfo interface
    [ServiceContract]
    public interface IExportPartyInfoService
    {
        ExportPartyInfo Save(ExportPartyInfo oExportPartyInfo, Int64 nUserID);
        ExportPartyInfo Get(int id, Int64 nUserID);
        List<ExportPartyInfo> Gets( Int64 nUserID);
        List<ExportPartyInfo> Gets( int nContractorID,Int64 nUserID);
        string Delete(ExportPartyInfo oExportPartyInfo, Int64 nUserID);

    }
    #endregion
}