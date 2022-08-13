using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region Notice

    public class Notice : BusinessObject
    {
        public Notice()
        {
            NoticeID = 0 ;
            NoticeNo = "";
            Title = "";
            Description = "";
            IssueDate = DateTime.Now;
            ExpireDate = DateTime.Now;
            PostedBy = "";
            ErrorMessage = "";
            Params = "";
        }

        #region Properties

        public int NoticeID { get; set; }
        public string NoticeNo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpireDate { get; set; }

        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion

        #region Derived Property

        public string PostedBy { get; set; }

        public string Activity { get { if (this.IsActive) return "Active"; else return "Inactive"; } }
        public string IssueDateInStr { get { return this.IssueDate.ToString("dd MMM yyyy"); } }
        public string ExpireDateInStr { get { return this.IssueDate.ToString("dd MMM yyyy"); } }

        public string DescriptionSummary
        {
            get
            {
                if (this.Description.Length >= 200)
                {
                    return this.Description.Substring(0, 200) + "......";
                }
                else
                {
                    return this.Description;
                }
            }
        }


        #endregion

        #region Functions

        public static Notice Get(int nNoticeID, long nUserID)
        {
            return Notice.Service.Get(nNoticeID, nUserID);
        }
        public static List<Notice> Gets(string sSQL, long nUserID)
        {
            return Notice.Service.Gets(sSQL, nUserID);
        }
        public Notice IUD(int nDBOperation, long nUserID)
        {
            return Notice.Service.IUD(this, nDBOperation, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static INoticeService Service
        {
            get { return (INoticeService)Services.Factory.CreateService(typeof(INoticeService)); }
        }

        #endregion
    }
    #endregion

    #region INotice interface

    public interface INoticeService
    {

        Notice Get(int nNoticeID, Int64 nUserID);
        List<Notice> Gets(string sSQL, Int64 nUserID);
        Notice IUD(Notice oNotice, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
