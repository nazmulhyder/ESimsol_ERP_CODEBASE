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
    #region EmployeeGroup

    public class EmployeeGroup : BusinessObject
    {
        public EmployeeGroup()
        {
            EGID = 0;
            EmployeeID = 0;
            EmployeeTypeID = 0;
            ErrorMessage = "";
            Name = "";
            EmployeeTypeIDs = "";
            IsBlock = false;
        }

        #region Properties


        public int EmployeeID { get; set; }
        public bool IsBlock { get; set; }
        public int EGID { get; set; }
        public int EmployeeTypeID { get; set; }
        public string ErrorMessage { get; set; }
        public string Name { get; set; }
        public string EmployeeTypeIDs { get; set; }
        #endregion

        #region Functions
        public static List<EmployeeGroup> Gets(long nUserID)
        {
            return EmployeeGroup.Service.Gets(nUserID);
        }
        public static List<EmployeeGroup> Gets(string sSQL, long nUserID)
        {
            return EmployeeGroup.Service.Gets(sSQL, nUserID);
        }
        public EmployeeGroup Get(int id, long nUserID)
        {
            return EmployeeGroup.Service.Get(id, nUserID);
        }
        public EmployeeGroup Save(long nUserID)
        {
            return EmployeeGroup.Service.Save(this, nUserID);
        }
        public string Upload(long nUserID)
        {
            return EmployeeGroup.Service.Upload(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return EmployeeGroup.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IEmployeeGroupService Service
        {
            get { return (IEmployeeGroupService)Services.Factory.CreateService(typeof(IEmployeeGroupService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeType interface

    public interface IEmployeeGroupService
    {
        List<EmployeeGroup> Gets(string sSQL, Int64 nUserID);
        EmployeeGroup Get(int id, Int64 nUserID);
        List<EmployeeGroup> Gets(Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        EmployeeGroup Save(EmployeeGroup oEmployeeGroup, Int64 nUserID);
        string Upload(EmployeeGroup oEmployeeGroup, Int64 nUserID);
    }
    #endregion
}

