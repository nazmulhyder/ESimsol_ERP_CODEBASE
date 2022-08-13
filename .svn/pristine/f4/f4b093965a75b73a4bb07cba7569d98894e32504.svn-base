using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region ELSetup
    public class ELSetup : BusinessObject
    {
        public ELSetup()
        {
            ELSetupID = 0;
            IsConsiderLeave = false;
            IsConsiderDayOff = false;
            IsConsiderHoliday = false;
            IsConsiderAbsent = false;
            InactiveBy = 0;
            InactiveByName = "";
            InactiveDate = DateTime.Now;
            ApproveBy = 0;
            ApproveByName = "";
            ApproveDate = DateTime.Now;
            ErrorMessage = "";
        }
        #region Properties
        public int ELSetupID { get; set; }
        public bool IsConsiderLeave { get; set; }
        public bool IsConsiderDayOff { get; set; }
        public bool IsConsiderHoliday { get; set; }
        public bool IsConsiderAbsent { get; set; }
        public int InactiveBy { get; set; }
        public string InactiveByName { get; set; }
        public DateTime InactiveDate { get; set; }
        public int ApproveBy { get; set; }
        public string ApproveByName { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string InactiveDateInString
        {
            get
            {
                return (InactiveBy == 0) ? "-" :this.InactiveDate.ToString("dd MMM yyyy");
            }
        }
        public string ApproveDateInString
        {
            get
            {
                return (ApproveBy == 0) ? "-" : this.ApproveDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public ELSetup Save(int nUserID)
        {
            return ELSetup.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return ELSetup.Service.Delete(id, nUserID);
        }
        public static List<ELSetup> Gets(int nUserID)
        {
            return ELSetup.Service.Gets(nUserID);
        }
        public static List<ELSetup> Gets(string sSQL, int nUserID)
        {
            return ELSetup.Service.Gets(sSQL, nUserID);
        }
        public ELSetup Approve(long nUserID)
        {
            return ELSetup.Service.Approve(this, nUserID);
        }
        public ELSetup Inactive(long nUserID)
        {
            return ELSetup.Service.Inactive(this, nUserID);
        }
        public static ELSetup Get(string sSQL, long nUserID)
        {
            return ELSetup.Service.Get(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IELSetupService Service
        {
            get { return (IELSetupService)Services.Factory.CreateService(typeof(IELSetupService)); }
        }
        #endregion
    }
    #endregion

    #region IELSetup interface
    public interface IELSetupService
    {
        ELSetup Save(ELSetup oELSetup, int nUserID);
        List<ELSetup> Gets(int nUserID);
        List<ELSetup> Gets(string sSQL, int nUserID);
        string Delete(int id, int nUserID);
        ELSetup Approve(ELSetup oELSetup, Int64 nUserID);
        ELSetup Inactive(ELSetup oELSetup, Int64 nUserID);
        ELSetup Get(string sSQL, Int64 nUserID);
    }
    #endregion
}
