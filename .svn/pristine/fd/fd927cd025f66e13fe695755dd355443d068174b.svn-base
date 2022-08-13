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
    public class FabricSCHistory
    {
        public FabricSCHistory()
        {
            FabricSCHistoryID = 0;
            FabricSCID = 0;
            FabricSCDetailID = 0;
            FSCStatus = EnumFabricPOStatus.None;
            FSCDStatus = EnumPOState.None;
            FSCStatus_Prv = EnumFabricPOStatus.None;
            FSCDStatus_Prv = EnumPOState.None;
            Note = "";
            ErrorMessage = "";
        }

        #region Properties
        public int FabricSCHistoryID { get; set; }
        public int FabricSCID { get; set; }
        public int FabricSCDetailID { get; set; }
        public EnumFabricPOStatus FSCStatus { get; set; }
        public EnumPOState FSCDStatus { get; set; }
        public EnumFabricPOStatus FSCStatus_Prv { get; set; }
        public EnumPOState FSCDStatus_Prv { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        
        #endregion
        #region Derived
        public int FSCStatusInt { get { return (int)this.FSCStatus; } }
        public int FSCDStatusInt { get { return (int)this.FSCDStatus; } }
        public int FSCStatus_PrvInt { get { return (int)this.FSCStatus_Prv; } }
        public int FSCDStatus_PrvInt { get { return (int)this.FSCDStatus_Prv; } }
        public string FSCStatusSt
        {
            get
            {
                return EnumObject.jGet(this.FSCStatus);
            }
        }
        public string FSCDStatusSt
        {
            get
            {
                return EnumObject.jGet(this.FSCDStatus);
            }
        }
        public string FSCStatus_PrvSt
        {
            get
            {
                return EnumObject.jGet(this.FSCStatus_Prv);
            }
        }
        public string FSCDStatus_PrvSt
        {
            get
            {
                return EnumObject.jGet(this.FSCDStatus_Prv);
            }
        }
        #endregion

        #region Functions
        public static List<FabricSCHistory> Gets(int nFSCID, int nUserID)
        {
            return FabricSCHistory.Service.Gets(nFSCID,nUserID);
        }
        public static List<FabricSCHistory> Gets(string sSQL, int nUserID)
        {
            return FabricSCHistory.Service.Gets(sSQL,nUserID);
        }
        public FabricSCHistory Save(int nUserID)
        {
            return FabricSCHistory.Service.Save(this, nUserID);
        }
        public FabricSCHistory Get(int nEPIDID, int nUserID)
        {
            return FabricSCHistory.Service.Get(nEPIDID, nUserID);
        }
        public string Delete(int nId, int nUserID)
        {
            return FabricSCHistory.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricSCHistoryService Service
        {
            get { return (IFabricSCHistoryService)Services.Factory.CreateService(typeof(IFabricSCHistoryService)); }
        }
        #endregion
    }

    #region IFabricSCHistory interface
    public interface IFabricSCHistoryService
    {
        List<FabricSCHistory> Gets(int nFSCID,int nUserID);
        List<FabricSCHistory> Gets(string sSQL, int nUserID);
        FabricSCHistory Save(FabricSCHistory oFabricSCHistory, int nUserID);
        FabricSCHistory Get(int nEPIDID, int nUserID);
        string Delete(int id, int nUserID);
    }
    #endregion
}
