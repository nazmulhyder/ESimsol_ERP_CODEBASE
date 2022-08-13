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
    public class CommentsHistory
    {
        public CommentsHistory()
        {
            CommentsHistoryID = 0;
            CommentsBy = "";
            ModuleID = 0;
            ModuleObjID = 0;
            CommentsText = "";
            CommentsDateTime = DateTime.Now;
            ErrorMessage = "";
        }

        #region Properties
        public int CommentsHistoryID { get; set; }
        public string CommentsBy { get; set; }
        public int ModuleID { get; set; }
        public int ModuleObjID { get; set; }
        public string CommentsText { get; set; }
        public DateTime CommentsDateTime { get; set; }
        public string CommentsByName { get; set; }
        public string ErrorMessage { get; set; }

        #endregion
        #region Derived Property
        public string CommentsDateSt
        {
            get
            {
                return this.CommentsDateTime.ToString("dd MMM yyyy H:mm tt");
            }
        }
        public string ModuleName
        {
            get
            {
                return EnumObject.jGet((EnumModuleName)this.ModuleID);
            }
        }
        #endregion

        #region Functions
        public static List<CommentsHistory> Gets(int nUserID)
        {
            return CommentsHistory.Service.Gets(nUserID);
        }
        public static List<CommentsHistory> Gets(string sSQL, int nUserID)
        {
            return CommentsHistory.Service.Gets(sSQL, nUserID);
        }
        public CommentsHistory Save(int nUserID)
        {
            return CommentsHistory.Service.Save(this, nUserID);
        }
        public CommentsHistory Get(int nEPIDID, int nUserID)
        {
            return CommentsHistory.Service.Get(nEPIDID, nUserID);
        }
        public string Delete(int nId, int nUserID)
        {
            return CommentsHistory.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICommentsHistoryService Service
        {
            get { return (ICommentsHistoryService)Services.Factory.CreateService(typeof(ICommentsHistoryService)); }
        }
        #endregion
    }

    #region ICommentsHistory interface
    public interface ICommentsHistoryService
    {
        List<CommentsHistory> Gets(int nUserID);
        List<CommentsHistory> Gets(string sSQL, int nUserID);
        CommentsHistory Save(CommentsHistory oCommentsHistory, int nUserID);
        CommentsHistory Get(int nEPIDID, int nUserID);
        string Delete(int id, int nUserID);
    }
    #endregion
}
