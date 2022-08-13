using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region FUProcess

    public class FNBatchHistory : BusinessObject
    {
        public FNBatchHistory()
        {
            FNBatchHistoryID = 0;
            FNBatchID = 0;
            PreviousStatus = 0;
            CurrentStatus = 0;
            ErrorMessage = "";
        }

        #region Properties

        public int FNBatchHistoryID { get; set; }
        public int FNBatchID { get; set; }
        public int PreviousStatus { get; set; }
        public int CurrentStatus { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion

        #region Derived Property
        public string PreviousStatusStr { get { return this.PreviousStatus.ToString(); } }
        public string CurrentStatusStr { get { return this.CurrentStatus.ToString(); } }

        #endregion

        #region Functions

        public static FNBatchHistory Get(int nFNBatchHistoryID, long nUserID)
        {
            return FNBatchHistory.Service.Get(nFNBatchHistoryID, nUserID);
        }
        public static List<FNBatchHistory> Gets(string sSQL, long nUserID)
        {
            return FNBatchHistory.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFNBatchHistoryService Service
        {
            get { return (IFNBatchHistoryService)Services.Factory.CreateService(typeof(IFNBatchHistoryService)); }
        }

        #endregion
    }
    #endregion

    #region IFUProcess interface

    public interface IFNBatchHistoryService
    {
        FNBatchHistory Get(int nFNBatchHistoryID, Int64 nUserID);
        List<FNBatchHistory> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
