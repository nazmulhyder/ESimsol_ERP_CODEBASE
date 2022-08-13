using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    public class MaxOTCEmployeeType : BusinessObject
    {
        public MaxOTCEmployeeType()
        {
            MaxOTCEmployeeTypeID = 0;
            MaxOTConfigurationID = 0;
            EmployeeTypeID = 0;
            Remarks = "";
            EmployeeTypeName = "";
            DBUserID = 0;
            ErrorMessage = "";
        }
        #region Properties
        public int MaxOTCEmployeeTypeID { get; set; }
        public int MaxOTConfigurationID { get; set; }
        public int EmployeeTypeID { get; set; }
        public string Remarks { get; set; }
        public string EmployeeTypeName { get; set; }
        public int DBUserID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions
        public static List<MaxOTCEmployeeType> Gets(int id, long nUserID)
        {
            return MaxOTCEmployeeType.Service.Gets(id, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IMaxOTCEmployeeTypeService Service
        {
            get { return (IMaxOTCEmployeeTypeService)Services.Factory.CreateService(typeof(IMaxOTCEmployeeTypeService)); }
        }
        #endregion
    }
    #region IMaxOTCEmployeeTypeService interface

    public interface IMaxOTCEmployeeTypeService
    {
        List<MaxOTCEmployeeType> Gets(int id, long nUserID);

    }
    #endregion
}
