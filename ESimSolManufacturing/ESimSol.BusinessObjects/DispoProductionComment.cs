using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region DispoProductionComment
    public class DispoProductionComment : BusinessObject
    {
        public DispoProductionComment()
        {
            DispoProductionCommentID = 0;
            FSCDID = 0;
            CommentDate = DateTime.Now;
            UserID = 0;
            Comment = "";
            ErrorMessage = "";
            UserName = "";
            IsOwn = true;
            ExeNo = "";
            DispoProductionCommentViewers = new List<DispoProductionCommentViewer>();
        }

        #region Property
        public int DispoProductionCommentID { get; set; }        
        public int FSCDID { get; set; }
        public int UserID { get; set; }
        public DateTime CommentDate { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public bool IsOwn { get; set; }
        public string ExeNo { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<DispoProductionAttachment> DispoProductionAttachments { get; set; }
        public List<DispoProductionCommentViewer> DispoProductionCommentViewers { get; set; }
        public string IsOwnInString
        {
            get
            {
                if (IsOwn) return "Own";
                return "All";
            }
        }
        public string CommentDateInString
        {
            get
            {
                return CommentDate.ToString("dd MMM yyyy hh:mm tt");
            }
        }
       
        #endregion

        #region Functions
        public static List<DispoProductionComment> Gets(long nUserID)
        {
            return DispoProductionComment.Service.Gets(nUserID);
        }
        public static List<DispoProductionComment> Gets(string sSQL, long nUserID)
        {
            return DispoProductionComment.Service.Gets(sSQL, nUserID);
        }
        public static List<DispoProductionComment> GetsBySP(int nFSCDID, long nUserID)
        {
            return DispoProductionComment.Service.GetsBySP(nFSCDID, nUserID);
        }
        public DispoProductionComment Get(int id, long nUserID)
        {
            return DispoProductionComment.Service.Get(id, nUserID);
        }
        public DispoProductionComment Save(long nUserID)
        {
            return DispoProductionComment.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return DispoProductionComment.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDispoProductionCommentService Service
        {
            get { return (IDispoProductionCommentService)Services.Factory.CreateService(typeof(IDispoProductionCommentService)); }
        }
        #endregion
    }
    #endregion

    #region IDispoProductionComment interface
    public interface IDispoProductionCommentService
    {
        DispoProductionComment Get(int id, Int64 nUserID);
        List<DispoProductionComment> Gets(Int64 nUserID);
        List<DispoProductionComment> Gets(string sSQL, Int64 nUserID);
        List<DispoProductionComment> GetsBySP(int nFSCDID, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        DispoProductionComment Save(DispoProductionComment oDispoProductionComment, Int64 nUserID);


    }
    #endregion
}
