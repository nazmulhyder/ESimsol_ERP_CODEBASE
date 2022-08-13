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
    #region EmployeeForIDCard
    public class EmployeeForIDCard
    {
        public EmployeeForIDCard()
        {
            EmployeeID=0;
			Code="";           
			Name="";
			ParmanentAddress="";
			PresentAddress="";
			ContactNo="";
			Email="";
			BloodGroup="";
			Photo=null;
			Signature=null;
			NationalID="";
			ChildrenInfo="";
			Village="";
			PostOffice="";
			Thana="";
			District="";
			PostCode="";
			NameInBangla="";
            PermVillageBangla = "";
            PermPostOfficeBangla = "";
            PermThanaBangla = "";
            PermDistrictBangla = "";
            NationalIDBangla = "";
            DistrictBangla ="";
			ThanaBangla = "";
            PostOfficeBangla = "";
			VillageBangla = "";
            CodeBangla = "";
            BloodGroupBangla  = "";

			OtherPhoneNo="";
			LocationID=0;	
			LocationName=""; 
			DepartmentID=0;
			DepartmentName="";
            CodeBangla = "";
            BloodGroupBangla = "";
			DepartmentNameInBangla="";
			LocationNameInBangla="";
			BusinessUnitNameInBangla="";
			BusinessUnitName="";

            BusinessUnitID = 0;
			BUAddressInBangla="";
			BUAddress="";
			BUPhone="";
			BUFaxNo="";

			DesignationName="";
			DesignationNameInBangla="";
			DesignationID=0;

			JoiningDate=DateTime.Now;
			ConfirmationDate=DateTime.Now;
            DateOfBirth = DateTime.Now;
            CardNo = "";
            DRPID = 0;
            HRResponsibility = new List<HRResponsibility>();
            HRResp = "";

            AuthSigPath = "";
            CompanyLogoPath = "";
        }

        #region Properties
        public int DRPID { get; set; }
        public int EmployeeID { get; set; }
        public int BusinessUnitID { get; set; }
        public int DesignationID { get; set; }
        public int DepartmentID { get; set; }
        public int LocationID { get; set; }
        public string Code { get; set; }
        public string CardNo { get; set; }
        public string HRResp { get; set; }
        public List<HRResponsibility> HRResponsibility { get; set; }
        public string DesignationNameInBangla { get; set; }
        public string DesignationName { get; set; }
        public string BUFaxNo { get; set; }
        public string BUPhone { get; set; }
        public string BUAddress { get; set; }
        public string BUAddressInBangla { get; set; }
        public string BusinessUnitName { get; set; }
        public string BusinessUnitNameInBangla { get; set; }
        public string LocationNameInBangla { get; set; }
        public string DepartmentNameInBangla { get; set; }
        public string DepartmentName { get; set; }
        public string LocationName { get; set; }
        public string OtherPhoneNo { get; set; }
        public string NameInBangla { get; set; }
        public string PermVillageBangla { get; set; }
        public string PermPostOfficeBangla { get; set; }
        public string PermThanaBangla { get; set; }
        public string PermDistrictBangla { get; set; }
        public string NationalIDBangla { get; set; }
        public string DistrictBangla { get; set; }
        public string ThanaBangla { get; set; }
        public string PostOfficeBangla { get; set; }
        public string VillageBangla { get; set; }      
        public string PostCode { get; set; }
        public string District { get; set; }
        public string Thana { get; set; }
        public string PostOffice { get; set; }
        public string Village { get; set; }
        public string ChildrenInfo { get; set; }
        public string NationalID { get; set; }
        public string BloodGroup { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string PresentAddress { get; set; }
        public string ParmanentAddress { get; set; }
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public byte[] Signature { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public System.Drawing.Image AuthSig { get; set; }
        public System.Drawing.Image EmployeePhoto { get; set; }
        public System.Drawing.Image EmployeeSiganture { get; set; }
        public string AuthSigPath { get; set; }
        public string CompanyLogoPath { get; set; }
        public string EmployeePhotoPath { get; set; }
        public string EmployeeSiganturePath { get; set; }
        public string NameCode
        {
            get
            {
                return this.Name + " [" + this.Code + "] ";
            }
        }
        public string IssueDate
        {
            get
            {
                return DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        public string DateOfJoinInString
        {
            get
            {
                return JoiningDate.ToString("dd/MM/yyyy");
            }
        }

        public DateTime ConfirmationDate { get; set; }

        public string ConfirmationDateInString
        {
            get
            {
                return ConfirmationDate.ToString("dd MMM yyyy");
            }
        }
        public string DateOfBirthInString
        {
            get
            {
                return DateOfBirth.ToString("dd/MM/yyyy");
            }
        }
        public string BloodGroupST
        {
            get
            {
                string SBG = "";
                if (this.BloodGroup == "1") { SBG = "A+"; }
                else if (this.BloodGroup == "2") { SBG = "A-"; }
                else if (this.BloodGroup == "3") { SBG = "B+"; }
                else if (this.BloodGroup == "4") { SBG = "B-"; }
                else if (this.BloodGroup == "5") { SBG = "O+"; }
                else if (this.BloodGroup == "6") { SBG = "O-"; }
                else if (this.BloodGroup == "7") { SBG = "AB+"; }
                else if (this.BloodGroup == "8") { SBG = "AB-"; }
                else { SBG = ""; }
                return SBG;
            }
        }
        public string CodeBangla { get; set; }
        public string BloodGroupBangla { get; set; }

        #endregion

        #region Functions


        public static List<EmployeeForIDCard> Gets(string sSQL, long nUserID)
        {
            return EmployeeForIDCard.Service.Gets(sSQL, nUserID);
        }
        public static EmployeeForIDCard Get(string sSQL, long nUserID)
        {
            return EmployeeForIDCard.Service.Get(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IEmployeeForIDCardService Service
        {
            get { return (IEmployeeForIDCardService)Services.Factory.CreateService(typeof(IEmployeeForIDCardService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceDaily interface

    public interface IEmployeeForIDCardService
    {
        List<EmployeeForIDCard> Gets(string sSQL, Int64 nUserID);
        EmployeeForIDCard Get(string sSQL, Int64 nUserID);
       
      
    }
    #endregion
}


