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
    #region EmployeeNominee

    public class EmployeeNominee : BusinessObject
    {
        public EmployeeNominee()
        {
            ENID = 0;
            EmployeeID = 0;
            Name = "";
            Address = "";
            Email = "";
            ContactNo = "";
            Relation = "";
            Percentage = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int ENID { get; set; }
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Relation { get; set; }
        public double Percentage { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derive

        #endregion

        #region Functions

        public static List<EmployeeNominee> Gets(int nEmployeeID, long nUserID)
        {
            return EmployeeNominee.Service.Gets(nEmployeeID, nUserID);
        }
        public EmployeeNominee Get(int id, long nUserID) //ENID
        {
            return EmployeeNominee.Service.Get(id, nUserID);
        }
        public EmployeeNominee IUD(int nDBOperation, long nUserID)
        {
            return EmployeeNominee.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IEmployeeNomineeService Service
        {
            get { return (IEmployeeNomineeService)Services.Factory.CreateService(typeof(IEmployeeNomineeService)); }
        }

        #endregion
    }
    #endregion

    #region Report Study
    public class EmployeeNomineeList : List<EmployeeNominee>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IEmployeeNominee interface

    public interface IEmployeeNomineeService
    {
        EmployeeNominee Get(int id, Int64 nUserID);//ENID
        List<EmployeeNominee> Gets(int nEmployeeID, Int64 nUserID);
        EmployeeNominee IUD(EmployeeNominee oEmployeeNominee, int nDBOperation, Int64 nUserID);
    }
    #endregion
}