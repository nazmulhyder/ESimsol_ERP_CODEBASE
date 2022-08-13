using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region OTExceptCompliance

    public class OTExceptCompliance : BusinessObject
    {

        #region  Constructor
        public OTExceptCompliance()
        {
            EmployeeID = 0;
            LocationID = 0;
            GrossAmount = 0;
            CompGrossAmount = 0;
            BasicAmount = 0;
            OTRatePerHour = 0;
            OTInMinute = 0;
            AdditionalOTInMinute = 0;
            Name = string.Empty;
            Code = string.Empty;
            DateOfJoin = DateTime.MinValue;
            LocationName = string.Empty;
            DepartmentName = string.Empty;
            DesignationName = string.Empty;
            ErrorMessage = string.Empty;
            BusinessUnitID = 0;
            CompOTInMinute = 0;
            BusinessUnitName = "";
            OTGroupList = new List<OTExceptCompliance>();
            BusinessUnitAddress = "";
            CompOTRatePerHour = 0;
        }
        #endregion

        #region Properties

        public double CompOTInMinute { get; set; }
        public int EmployeeID { get; set; }
        public int BusinessUnitID { get; set; }
        public double CompOTRatePerHour { get; set; }
        public int LocationID { get; set; }  
        public double GrossAmount { get; set; }
        public double CompGrossAmount { get; set; }
        public double BasicAmount { get; set; } 
        public double OTRatePerHour { get; set; }
        public double OTInMinute { get; set; }
        public double AdditionalOTInMinute { get; set; }
        public string BusinessUnitName { get; set; }
        public string Name { get; set; }
        public string BusinessUnitAddress { get; set; }
        public string Code { get; set; }
        public DateTime DateOfJoin { get; set; } 
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string ErrorMessage { get; set; }

        public double OTHour
        {
            get
            {
                return (this.OTInMinute <= 0) ? 0 : this.OTInMinute / Convert.ToDouble(60);
            }
        }
        public double CompOTHour
        {
            get
            {
                return (this.CompOTInMinute <= 0) ? 0 : this.CompOTInMinute / Convert.ToDouble(60);
            }
        }

        public double AdditionalOTHour
        {
            get
            {
                return (this.AdditionalOTInMinute <= 0) ? 0 : this.AdditionalOTInMinute / Convert.ToDouble(60);
            }
        }

        public double OTAmountExceptComp
        {
            get
            {
                return (this.OTRatePerHour <= 0) ? 0 : this.OTRatePerHour * ((this.AdditionalOTInMinute <= 0) ? 0 : this.AdditionalOTInMinute / Convert.ToDouble(60));
            }
        }
        public double OTAmount
        {
            get
            {
                return (this.OTRatePerHour <= 0) ? 0 : this.OTRatePerHour * ((this.OTInMinute <= 0) ? 0 : this.OTInMinute / Convert.ToDouble(60));
            }
        }
        public double CompOTAmount
        {
            get
            {
                return (this.CompOTRatePerHour <= 0) ? 0 : this.CompOTRatePerHour * ((this.CompOTInMinute <= 0) ? 0 : this.CompOTInMinute / Convert.ToDouble(60));
            }
        }
        public string DateOfJoinInStr
        {
            get
            {
                return (this.DateOfJoin==DateTime.MinValue) ? "" : this.DateOfJoin.ToString("dd MMM yyyy");
            }
        }
        #endregion
        public List<OTExceptCompliance> OTGroupList { get; set; }

        #region Functions


        public static List<OTExceptCompliance> Gets(string sBU, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, string sEmpIDs, string sBMMIDs, int nPayType, int nMonthID, int nYear, bool bNewJoin, bool bExceptComp, long nUserID)
        {
            return OTExceptCompliance.Service.Gets(sBU, sLocationID, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, sEmpIDs, sBMMIDs, nPayType, nMonthID, nYear, bNewJoin, bExceptComp, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IOTExceptComplianceService Service
        {
            get { return (IOTExceptComplianceService)Services.Factory.CreateService(typeof(IOTExceptComplianceService)); }
        }
        #endregion
    }
  
    #endregion


    #region IOTExceptCompliance interface
    public interface IOTExceptComplianceService
    {
        List<OTExceptCompliance> Gets(string sBU, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, string sEmpIDs, string sBMMIDs, int nPayType, int nMonthID, int nYear, bool bNewJoin, bool bExceptComp, long nUserID);
    }
    #endregion
}
