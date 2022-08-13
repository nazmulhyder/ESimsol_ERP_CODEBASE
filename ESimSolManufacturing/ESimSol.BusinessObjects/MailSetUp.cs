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
    #region MailSetUp

    public class MailSetUp : BusinessObject
    {
        public MailSetUp()
        {
            MSID = 0;
            ReportID = 0;
            Subject = "";
            MailType = MailReportingType.None;
            MailTime = DateTime.MinValue;
            IsActive = false;
            NextTimeToMail = DateTime.Now;
            LastMailTime = DateTime.MinValue;
            IsMailSend = false;
            IsMail = true;
            ErrorMessage = "";
            ToMail = new MailAssignedPerson();
            CCMails = new List<MailAssignedPerson>();
            MailReportings = new List<MailReporting>();
            ModuleType = EnumModuleName.FabricSalesContract;
            Params = "";
        }

        #region Properties

        public int MSID { get; set; }
        public int ReportID { get; set; }
        public string Subject { get; set; }
        public MailReportingType MailType { get; set; }
        public DateTime MailTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime NextTimeToMail { get; set; }
        public DateTime LastMailTime { get; set; }
        public bool IsMailSend { get; set; }
        public bool IsMail { get; set;}

        public int ModuleTypeInt { get; set; }
        public EnumModuleName ModuleType { get; set; }
        public string ModuleName { get { return EnumObject.jGet(this.ModuleType); } }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion

        #region

        public string ReportName { get; set; }
        public string ControllerName { get; set; }
        public string FunctionName { get; set; }

        public int MailTypeInInt { get { return (int)this.MailType; } }
        public string MailTypeInStr { get { return this.MailType.ToString(); } }
        public string MailTimeInStr { get { return this.MailTime.ToString("HH:mm"); }}
        public string NextTimeToMailInStr { get { return this.NextTimeToMail.ToString("dd MMM yyyy hh:mm tt"); }}
        public string ActivityStatus { get { return (this.IsActive) ? "Active" : "Inactive"; } }

        public MailAssignedPerson ToMail { get; set; }
        public List<MailAssignedPerson> CCMails { get; set; }
        public List<MailReporting> MailReportings { get; set; }

        #endregion


        #region Functions
      
        public static MailSetUp Get(int nMSID, long nUserID)
        {
            return MailSetUp.Service.Get(nMSID, nUserID);
        }
        public static MailSetUp GetByModule(int nModuleD, long nUserID)
        {
            return MailSetUp.Service.GetByModule(nModuleD, nUserID);
        }
        public static List<MailSetUp> Gets(string sSQL, long nUserID)
        {
            return MailSetUp.Service.Gets(sSQL, nUserID);
        }
        public MailSetUp IUD(int nDBOperation, long nUserID)
        {
            return MailSetUp.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IMailSetUpService Service
        {
            get { return (IMailSetUpService)Services.Factory.CreateService(typeof(IMailSetUpService)); }
        }
        #endregion

    }
    #endregion

    #region IMailSetUp interface

    public interface IMailSetUpService
    {
        MailSetUp Get(int nMSID, Int64 nUserID);
        MailSetUp GetByModule(int nModuleD, Int64 nUserID);
        List<MailSetUp> Gets(string sSQL, Int64 nUserID);
        MailSetUp IUD(MailSetUp oMailSetUp, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
