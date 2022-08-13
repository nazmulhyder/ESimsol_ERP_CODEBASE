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
    #region ELEncashCompliance
    public class ELEncashCompliance
    {
        public ELEncashCompliance()
        {
            ELEncashCompID = 0;
            DeclarationDate = DateTime.MinValue;
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
            BUID = 0;
            LocationID = 0;
            Note = "";
            ConsiderELCount = 0;
            ApproveBy = 0;
            ApproveDate = DateTime.MinValue;
            BUName = "";
            LocationName = "";
            ApproveByName = "";
            ErrorMessage = "";
        }

        #region Properties

        public int ELEncashCompID { get; set; }

        public DateTime DeclarationDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int BUID { get; set; }

        public int LocationID { get; set; }

        public string Note { get; set; }

        public string ErrorMessage { get; set; }

        public int ConsiderELCount { get; set; }

        public int ApproveBy { get; set; }

        public DateTime ApproveDate { get; set; }

        public string BUName { get; set; }

        public string LocationName { get; set; }

        public string ApproveByName { get; set; }

        public string StartDateInString
        {
            get
            {
                return StartDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateInString
        {
            get
            {
                return EndDate.ToString("dd MMM yyyy");
            }
        }
        public string DeclarationDateInString
        {
            get
            {
                return DeclarationDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions


        public ELEncashCompliance Save(int nUserID)
        {
            return ELEncashCompliance.Service.Save(this, nUserID);
        }
        public static List<ELEncashCompliance> Gets(int nUserID)
        {
            return ELEncashCompliance.Service.Gets(nUserID);
        }
        public static List<ELEncashCompliance> Gets(string sSQL, int nUserID)
        {
            return ELEncashCompliance.Service.Gets(sSQL, nUserID);
        }
        public static ELEncashCompliance Get(string sSQL, long nUserID)
        {
            return ELEncashCompliance.Service.Get(sSQL, nUserID);
        }
        public int ELEncashComplianceSave(int nIndex, int EASPID, long nUserID)
        {
            return ELEncashCompliance.Service.ELEncashComplianceSave(nIndex, EASPID, nUserID);
        }
        public ELEncashCompliance Approve(long nUserID)
        {
            return ELEncashCompliance.Service.Approve(this, nUserID);
        }
        public ELEncashCompliance Delete(long nUserID)
        {
            return ELEncashCompliance.Service.Delete(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IELEncashComplianceService Service
        {
            get { return (IELEncashComplianceService)Services.Factory.CreateService(typeof(IELEncashComplianceService)); }
        }
        #endregion

    }
    #endregion

    #region IELEncashCompliance interface

    public interface IELEncashComplianceService
    {
        ELEncashCompliance Save(ELEncashCompliance oELEncashCompliance, int nUserID);
        List<ELEncashCompliance> Gets(int nUserID);
        List<ELEncashCompliance> Gets(string sSQL, int nUserID);
        ELEncashCompliance Get(string sSQL, Int64 nUserID);
        int ELEncashComplianceSave(int nIndex, int EASPID, long nUserID);
        ELEncashCompliance Approve(ELEncashCompliance oELEncashCompliance, Int64 nUserID);
        ELEncashCompliance Delete(ELEncashCompliance oELEncashCompliance, Int64 nUserID);
       
      
    }
    #endregion
}

