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
    #region MailAssignedPerson

    public class MailAssignedPerson : BusinessObject
    {
        public MailAssignedPerson()
        {
            MAPID = 0;
            MSID = 0;
            MailTo = "";
            IsCCMail = false;
            ErrorMessage = "";
        }

        #region Properties

        public int MAPID { get; set; }
        public int MSID { get; set; }
        public String MailTo { get; set; }
        public bool IsCCMail { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derive Properties
        public MailSetUp MS { get; set; }
        #endregion


        #region Functions

        public static MailAssignedPerson Get(int nMAPID, long nUserID)
        {
            return MailAssignedPerson.Service.Get(nMAPID, nUserID);
        }
        public static List<MailAssignedPerson> Gets(string sSQL, long nUserID)
        {
            return MailAssignedPerson.Service.Gets(sSQL, nUserID);
        }
        public MailAssignedPerson IUD(int nDBOperation, long nUserID)
        {
            return MailAssignedPerson.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IMailAssignedPersonService Service
        {
            get { return (IMailAssignedPersonService)Services.Factory.CreateService(typeof(IMailAssignedPersonService)); }
        }
        #endregion
    }
    #endregion

    #region IMailAssignedPerson interface

    public interface IMailAssignedPersonService
    {
        MailAssignedPerson Get(int nMAPID, Int64 nUserID);
        List<MailAssignedPerson> Gets(string sSQL, Int64 nUserID);
        MailAssignedPerson IUD(MailAssignedPerson oMailAssignedPerson, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
