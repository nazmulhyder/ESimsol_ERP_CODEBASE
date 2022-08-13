using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;



namespace ESimSol.BusinessObjects
{
    #region MailReporting

    public class MailReporting : BusinessObject
    {
        public MailReporting()
        {
            ReportID = 0;
            Name="";
            ControllerName = "";
            FunctionName = "";
            IsActive = true;
            IsMail = true;
            ErrorMessage = "";
        }

        #region Properties

        public int ReportID { get; set; }
        public string Name { get; set; }
        public string ControllerName { get; set; }
        public string FunctionName { get; set; }
        public bool IsActive { get; set; }
        public bool IsMail { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derive Properties
        public string ActivityStatus { get { return (this.IsActive) ? "Active" : "Inactive"; } }
        #endregion

        #region Functions

        public static MailReporting Get(int nReportID, long nUserID)
        {
            return MailReporting.Service.Get(nReportID, nUserID);
        }
        public static List<MailReporting> Gets(string sSQL, long nUserID)
        {
            return MailReporting.Service.Gets(sSQL, nUserID);
        }
        public MailReporting IUD(int nDBOperation, long nUserID)
        {
            return MailReporting.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IMailReportingService Service
        {
            get { return (IMailReportingService)Services.Factory.CreateService(typeof(IMailReportingService)); }
        }
        #endregion
    }
    #endregion

    #region IMailReporting interface

    public interface IMailReportingService
    {
        MailReporting Get(int nReportID, Int64 nUserID);
        List<MailReporting> Gets(string sSQL, Int64 nUserID);
        MailReporting IUD(MailReporting oMailReporting, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
