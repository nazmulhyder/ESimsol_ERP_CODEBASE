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
    #region EmployeeReference

    public class EmployeeReference : BusinessObject
    {
        public EmployeeReference()
        {
            EmployeeReferenceID = 0;
            EmployeeID = 0;
            Name = "";
            Designation = "";
            Organization = "";
            ContactNo = "";
            Email = "";
            Address = "";
            Relation = "";
            Description = "";
            ErrorMessage = "";

        }

        #region Properties
        public int EmployeeReferenceID { get; set; }
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Organization { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Relation { get; set; }
        public string Description { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions

        public static List<EmployeeReference> Gets(int nEmployeeID, long nUserID)
        {
            return EmployeeReference.Service.Gets(nEmployeeID, nUserID);
        }

        public EmployeeReference Get(int id, long nUserID) //EmployeeReferenceID
        {
            return EmployeeReference.Service.Get(id, nUserID);
        }

        public EmployeeReference IUD(int nDBOperation, long nUserID)
        {
            return EmployeeReference.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeReferenceService Service
        {
            get { return (IEmployeeReferenceService)Services.Factory.CreateService(typeof(IEmployeeReferenceService)); }
        }

        #endregion
    }
    #endregion

    #region Report Study
    public class EmployeeReferenceList : List<EmployeeReference>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IEmployeeReference interface

    public interface IEmployeeReferenceService
    {
        EmployeeReference Get(int id, Int64 nUserID);//EmployeeReferenceID
        List<EmployeeReference> Gets(int nEmployeeID, Int64 nUserID);
        EmployeeReference IUD(EmployeeReference oEmployeeReference, int nDBOperation, Int64 nUserID);
    }
    #endregion
}