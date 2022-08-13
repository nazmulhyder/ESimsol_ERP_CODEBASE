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
    public class ArchiveSalaryStrucDtl : BusinessObject
    {
        public ArchiveSalaryStrucDtl()
        {
            ArchiveSalaryStrucDtlID = 0;
            ArchiveSalaryStrucID = 0;
            SalaryHeadID = 0;
            Amount = 0;
            UserNameCode = "";
            CompAmount = 0;
            DBUSerID = 0;
            DBServerDateTime = DateTime.Now;
            IsProcessDependent = false;
            ErrorMessage = "";
            Equation = "";
            EntryUserName = "";
        }
        #region Properties
        public int ArchiveSalaryStrucDtlID { get; set; }
        public int ArchiveSalaryStrucID { get; set; }
        public int SalaryHeadID { get; set; }
        public double Amount { get; set; }
        public string UserNameCode { get; set; }
        public double CompAmount { get; set; }
        public int DBUSerID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string SalaryHeadName { get; set; }
        public string EntryUserName { get; set; }
        public string SalaryHeadNameInBangla { get; set; }
        public bool IsProcessDependent { get; set; }

        public string ErrorMessage { get; set; }
        public string Equation { get; set; }
        #endregion
        public string IssueDateSt
        {
            get
            {
                return this.DBServerDateTime.ToString("dd MMM yyyy");
            }
        }

        public EnumSalaryHeadType SalaryHeadType { get; set; }
        public string AllowanceName { get { if (this.SalaryHeadType == EnumSalaryHeadType.Addition)return SalaryHeadName + "(+)"; else return this.SalaryHeadName + "(-)"; } set { } }
        public string AllowanceType { get { return this.SalaryHeadType.ToString(); } }
        public EnumAllowanceCondition Condition { get; set; }
        public int ConditionInt { get; set; }

        public EnumPeriod Period { get; set; }

        public int Times { get; set; }
        public int PeriodInt { get; set; }

        public int DeferredDay { get; set; }

        public EnumRecruitmentEvent ActivationAfter { get; set; }
        public int ActivationAfterInt { get; set; }

     
      
        #region Derived Properties
        public string SalaryHeadTypeInString
        {
            get
            {
                return SalaryHeadType.ToString();
            }
        }

        public string ConditionInString
        {
            get
            {
                return Condition.ToString();
            }
        }
        public string PeriodInString
        {
            get
            {
                return Times + " Times" + " / " + Period;
            }
        }

        public string ActivationAfterInString
        {
            get
            {
                return DeferredDay + " Days off " + ActivationAfter;
            }
        }
        #endregion

        #region Function
        public static List<ArchiveSalaryStrucDtl> Gets(int id, long nUserID)
        {
            return ArchiveSalaryStrucDtl.Service.Gets(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IArchiveSalaryStrucDtlService Service
        {
            get { return (IArchiveSalaryStrucDtlService)Services.Factory.CreateService(typeof(IArchiveSalaryStrucDtlService)); }
        }
        #endregion
    }

    #region IArchiveSalaryStrucDtl interface
    public interface IArchiveSalaryStrucDtlService
    {
        List<ArchiveSalaryStrucDtl> Gets(int id, long nUserID);
    }
    #endregion
}
