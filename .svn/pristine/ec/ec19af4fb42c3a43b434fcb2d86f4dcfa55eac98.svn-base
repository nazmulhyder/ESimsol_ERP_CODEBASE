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
    #region TotalEmployeeDetail
    [DataContract]
    public class TotalEmployeeDetail : BusinessObject
    {
        public TotalEmployeeDetail()
        {
            BUName = "";
            LocationName ="";
            DepartmentName ="";
            TotalEmp =0;
	        NewEmployee =0;
            LeftyEmployee =0;
            PFEmployee =0;
            YetToPFEmployee =0;
            GratuityEmployee =0;
            Male =0;
            Female =0;
	        Permanent =0;
            Probationary =0;
            Contractual =0;
            Seasonal =0;
            ErrorMessage = "";
        }

        #region Properties

        public string BUName { get; set; }
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public int TotalEmp { get; set; }
        public int NewEmployee { get; set; }
        public int LeftyEmployee { get; set; }
        public int PFEmployee { get; set; }
        public int YetToPFEmployee { get; set; }
        public int GratuityEmployee { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int Permanent { get; set; }
        public int Probationary { get; set; }
        public int Contractual { get; set; }
        public int Seasonal { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        #endregion

        #region Functions
        public static List<TotalEmployeeDetail> Gets(DateTime StartDate,DateTime EndDate, long nUserID)
        {
            return TotalEmployeeDetail.Service.Gets(StartDate,EndDate,nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ITotalEmployeeDetailService Service
        {
            get { return (ITotalEmployeeDetailService)Services.Factory.CreateService(typeof(ITotalEmployeeDetailService)); }
        }
        #endregion
    }
    #endregion

    #region ITotalEmployeeDetail interface

    public interface ITotalEmployeeDetailService
    {
        List<TotalEmployeeDetail> Gets(DateTime StartDate, DateTime EndDate, Int64 nUserID);

    }
    #endregion
}
