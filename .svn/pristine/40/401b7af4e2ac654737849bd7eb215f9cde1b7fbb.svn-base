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
    #region TotalEmployee
    [DataContract]
    public class TotalEmployee : BusinessObject
    {
        public TotalEmployee()
        {
            BUName = "";
            LocationName = "";
            DepartmentName = "";
            TotalEmp = 0;
            ErrorMessage = "";
        }

        #region Properties
        public string BUName { get; set; }
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public int TotalEmp { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        #endregion

        #region Functions
        public static List<TotalEmployee> Gets(long nUserID)
        {
            return TotalEmployee.Service.Gets(nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ITotalEmployeeService Service
        {
            get { return (ITotalEmployeeService)Services.Factory.CreateService(typeof(ITotalEmployeeService)); }
        }
        #endregion
    }
    #endregion

    #region ITotalEmployee interface

    public interface ITotalEmployeeService
    {
        List<TotalEmployee> Gets(Int64 nUserID);

    }
    #endregion
}
