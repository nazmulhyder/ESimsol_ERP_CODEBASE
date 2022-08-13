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
    #region ReportComments
    
    public class ReportComments : BusinessObject
    {
        public ReportComments()
        {
            RCID = 0;
            CommentDate = DateTime.Now;
            Note = "";
            ErrorMessage = "";

        }

        #region Properties
        
        public int RCID { get; set; }
        
        public DateTime CommentDate { get; set; }
        
        public string Note { get; set; }
        
        public string ErrorMessage { get; set; }
        #endregion


        #region Derive

        public string CommentDateInStr { get { return this.CommentDate.ToString("dd MMM yyyy"); } }

        #endregion



        #region Functions

        public static ReportComments Get(int nRCID, long nUserID)
        {
            return ReportComments.Service.Get(nRCID, nUserID);
        }
        public static List<ReportComments> Gets(string sSQL, long nUserID)
        {
            return ReportComments.Service.Gets(sSQL, nUserID);
        }
        public ReportComments IUD(int nDBOperation,long nUserID)
        {
            return ReportComments.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IReportCommentsService Service
        {
            get { return (IReportCommentsService)Services.Factory.CreateService(typeof(IReportCommentsService)); }
        }
        #endregion
    }
    #endregion

    #region IReportComments interface
    
    public interface IReportCommentsService
    {
        
        ReportComments Get(int nRCID, long nUserID);
        
        List<ReportComments> Gets(string sSql, long nUserID);
        
        ReportComments IUD(ReportComments oReportComments, int nDBOperation, long nUserID);
    }
    #endregion
}
