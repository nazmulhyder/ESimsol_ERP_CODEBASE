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
    #region EmployeeEducation

    public class EmployeeEducation : BusinessObject
    {
        public EmployeeEducation()
        {
            EmployeeEducationID = 0;
            EmployeeID = 0;
            Degree = "";
            Major = "";
            Session = "";
            PassingYear = 0;
            Sequence = 0;
            BoardUniversity = "";
            Institution = "";
            Result = "";
            Country = "";
            ErrorMessage = "";

        }

        #region Properties
        public int EmployeeEducationID { get; set; }
        public int EmployeeID { get; set; }
        public int Sequence { get; set; }
        public string Degree { get; set; }
        public string Major { get; set; }
        public string Session { get; set; }
        public int PassingYear { get; set; }
        public string BoardUniversity { get; set; }
        public string Institution { get; set; }
        public string Result { get; set; }
        public string Country { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions

        public static List<EmployeeEducation> Gets(int nEmployeeID, long nUserID)
        {
            return EmployeeEducation.Service.Gets(nEmployeeID, nUserID);
        }
        public static List<EmployeeEducation> Gets(string sSql, long nUserID)
        {
            return EmployeeEducation.Service.Gets(sSql, nUserID);
        }
        public EmployeeEducation Get(int id, long nUserID) //EmployeeEducationID
        {
            return EmployeeEducation.Service.Get(id, nUserID);
        }

        public EmployeeEducation IUD(int nDBOperation, long nUserID)
        {
            return EmployeeEducation.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeEducationService Service
        {
            get { return (IEmployeeEducationService)Services.Factory.CreateService(typeof(IEmployeeEducationService)); }
        }

        #endregion
    }
    #endregion

    #region Report Study
    public class EmployeeEducationList : List<EmployeeEducation>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IEmployeeEducation interface

    public interface IEmployeeEducationService
    {
        EmployeeEducation Get(int id, Int64 nUserID);//EmployeeEducationID
        List<EmployeeEducation> Gets(int nEmployeeID, Int64 nUserID);
        List<EmployeeEducation> Gets(string sSql, Int64 nUserID);
        EmployeeEducation IUD(EmployeeEducation oEmployeeEducation, int nDBOperation, Int64 nUserID);
    }
    #endregion
}