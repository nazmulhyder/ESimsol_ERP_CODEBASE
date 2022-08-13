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
    #region EmployeeExperience

    public class EmployeeExperience : BusinessObject
    {
        public EmployeeExperience()
        {
            EmployeeExperienceID = 0;
            EmployeeID = 0;
            ContractorID = 0;
            Organization = "";
            OrganizationType = "";
            Address = "";
            DepartmentID = 0;
            DesignationID = 0;
            Designation = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            StartDateExFormatType = EnumCustomDateFormat.dd_MMM_yyyy;
            EndDateExFormatType = EnumCustomDateFormat.dd_MMM_yyyy;
            Duration = "";
            MajorResponsibility = "";
            VesselID = 0;
            ErrorMessage = "";
            VesselType = EnumVesselType.None;
            GRT = "";
            DWT = "";
            EngineType = "";
            BHP = "";

        }

        #region Properties
        public int EmployeeExperienceID { get; set; }
        public int EmployeeID { get; set; }
        public int ContractorID { get; set; }
        public string Organization { get; set; }
        public string OrganizationType { get; set; }
        public string Address { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public string Designation { get; set; }
        public DateTime StartDate { get; set; }
        public EnumCustomDateFormat StartDateExFormatType { get; set; }
        public EnumCustomDateFormat EndDateExFormatType { get; set; }
        public DateTime EndDate { get; set; }
        public string Duration { get; set; }
        public string MajorResponsibility { get; set; }
        public int VesselID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derive
        public string DurationString
        {
            get
            {
                return this.StartDateString + " to " + this.EndDateString;
            }
        }
        public string StartDateString
        {
            get
            {
                if ((int)StartDateExFormatType==(int)EnumCustomDateFormat.dd_MMM_yyyy)
                {
                    return this.StartDate.ToString("dd MMM yyyy");
                }
                else if ((int)StartDateExFormatType == (int)EnumCustomDateFormat.MMM_yyyy)
                {
                    return this.StartDate.ToString("MMM yyyy");
                }
                else
                {
                    return this.StartDate.ToString("yyyy");
                }
            }
        }
        public string EndDateString
        {
            get
            {
                if ((int)EndDateExFormatType == (int)EnumCustomDateFormat.dd_MMM_yyyy)
                {
                    return this.EndDate.ToString("dd MMM yyyy");
                }
                else if ((int)EndDateExFormatType ==(int) EnumCustomDateFormat.MMM_yyyy)
                {
                    return this.EndDate.ToString("MMM yyyy");
                }
                else
                {
                    return this.EndDate.ToString("yyyy");
                }
            }
        }

        public string ContractorName { get; set; }

        public string VesselName { get; set; }

        public string Vessel { get; set; }

        public string DepartmentName { get; set; }

        public string DesignationName { get; set; }

        public EnumVesselType VesselType { get; set; }
        public int VesselTypeInt { get; set; }
        public string VesselTypeInString
        {
            get
            {
                return VesselType.ToString();
            }
        }
        public string GRT { get; set; }
        public string DWT { get; set; }
        public string EngineType { get; set; }
        public string BHP { get; set; }
        public string Country { get; set; }
        public List<Department> Departments { get; set; }
        #endregion

        #region Functions

        public static List<EmployeeExperience> Gets(int nEmployeeID, long nUserID)
        {
            return EmployeeExperience.Service.Gets(nEmployeeID, nUserID);
        }
        public EmployeeExperience Get(int id, long nUserID) //EmployeeExperienceID
        {
            return EmployeeExperience.Service.Get(id, nUserID);
        }
        public EmployeeExperience IUD(int nDBOperation, long nUserID)
        {
            return EmployeeExperience.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IEmployeeExperienceService Service
        {
            get { return (IEmployeeExperienceService)Services.Factory.CreateService(typeof(IEmployeeExperienceService)); }
        }

        #endregion
    }
    #endregion

    #region Report Study
    public class EmployeeExperienceList : List<EmployeeExperience>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IEmployeeExperience interface

    public interface IEmployeeExperienceService
    {
        EmployeeExperience Get(int id, Int64 nUserID);//EmployeeExperienceID
        List<EmployeeExperience> Gets(int nEmployeeID, Int64 nUserID);
        EmployeeExperience IUD(EmployeeExperience oEmployeeExperience, int nDBOperation, Int64 nUserID);
    }
    #endregion
}