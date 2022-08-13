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
    #region SingleSalaryProcess
    public class SingleSalaryProcess
    {
        public SingleSalaryProcess()
        {

            EmployeeID = 0;
            Name = "";
            Code = "";
            DepartmentName = "";
            DesignationName = "";
            LocationName = "";
            BUName = "";
            ErrorMessage = "";
        }

        #region Properties
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string LocationName { get; set; }
        public string BUName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions


        public static List<SingleSalaryProcess> Gets(string sSQL, long nUserID)
        {
            return SingleSalaryProcess.Service.Gets(sSQL, nUserID);
        }
        public static SingleSalaryProcess Get(string sSQL, long nUserID)
        {
            return SingleSalaryProcess.Service.Get(sSQL, nUserID);
        }
 
        #endregion

        #region ServiceFactory

        internal static ISingleSalaryProcessService Service
        {
            get { return (ISingleSalaryProcessService)Services.Factory.CreateService(typeof(ISingleSalaryProcessService)); }
        }
        #endregion
    }
    #endregion

    #region ISingleSalaryProcess interface

    public interface ISingleSalaryProcessService
    {
        List<SingleSalaryProcess> Gets(string sSQL, Int64 nUserID);
        SingleSalaryProcess Get(string sSQL, Int64 nUserID);
       
      
    }
    #endregion
}

