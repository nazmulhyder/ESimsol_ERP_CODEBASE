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
    #region EmployeeType

    public class EmployeeType : BusinessObject
    {
        public EmployeeType()
        {
            EmployeeTypeID = 0;
            Code = 0;
            Name = "";
            NameInBangla = "";
            Description = "";
            IsActive = true;
            ErrorMessage = "";
            EmployeeGrouping = EnumEmployeeGrouping.None;
        }

        #region Properties
        public int EmployeeTypeID { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string NameInBangla { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public EnumEmployeeGrouping EmployeeGrouping { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string Activity { get { if (IsActive)return "Active"; else return "Inactive"; } }

        public string EmpGroupingInStr { get { return this.EmployeeGrouping.ToString(); } }

        public int EmpGroupingInInt { get { return (int)this.EmployeeGrouping; } }

        public string EncryptEmpTypeID { get; set; }
        public List<Holiday> oHolidays = new List<Holiday>();
        #endregion

        #region Functions
        public static List<EmployeeType> Gets(long nUserID)
        {
            return EmployeeType.Service.Gets(nUserID);
        }

        public static List<EmployeeType> Gets(string sSQL, long nUserID)
        {
            return EmployeeType.Service.Gets(sSQL, nUserID);
        }
        public EmployeeType Get(int id, long nUserID)
        {
            return EmployeeType.Service.Get(id, nUserID);
        }

        public EmployeeType Save(long nUserID)
        {
            return EmployeeType.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return EmployeeType.Service.Delete(id, nUserID);
        }

        public string ChangeActiveStatus(EmployeeType oEmployeeType, long nUserID)
        {
            return EmployeeType.Service.ChangeActiveStatus(oEmployeeType, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IEmployeeTypeService Service
        {
            get { return (IEmployeeTypeService)Services.Factory.CreateService(typeof(IEmployeeTypeService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeType interface

    public interface IEmployeeTypeService
    {
        List<EmployeeType> Gets(string sSQL, Int64 nUserID);
        EmployeeType Get(int id, Int64 nUserID);
        List<EmployeeType> Gets(Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        EmployeeType Save(EmployeeType oEmployeeType, Int64 nUserID);
        string ChangeActiveStatus(EmployeeType oEmployeeType, Int64 nUserID);
    }
    #endregion
}
