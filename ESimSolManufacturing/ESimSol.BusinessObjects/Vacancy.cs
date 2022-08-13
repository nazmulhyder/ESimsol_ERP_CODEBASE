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
    #region Vacancy

    public class Vacancy : BusinessObject
    {
        public Vacancy()
        {

            DepartmentID = 0;
            DepartmentName = "";
            DesignationID = 0;
            DesignationName = "";
            ExistPerson = 0;
            RequiredPerson = 0;
            ShiftID = 0;
            ShiftName = "";
            ErrorMessage = "";

        }

        #region Properties

        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public int ExistPerson { get; set; }
        public int RequiredPerson { get; set; }
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public string ErrorMessage { get; set; }
        public int NoOfVacancy { get { return RequiredPerson - ExistPerson; } }

        #endregion

        #region Functions

        public static List<Vacancy> Gets(string sSQL, long nUserID)
        {
            return Vacancy.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IVacancyService Service
        {
            get { return (IVacancyService)Services.Factory.CreateService(typeof(IVacancyService)); }
        }

        #endregion
    }
    #endregion

    #region IVacancy interface

    public interface IVacancyService
    {
        List<Vacancy> Gets(string sSQL, Int64 nUserID);

    }
    #endregion
}
