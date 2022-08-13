using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.BusinessObjects
{
    public class ArchiveData : BusinessObject
    {
        public ArchiveData()
        {
            ArchiveDataID = 0;
            ArchiveNo = "";
            ArchiveDate = DateTime.Now;
            ArchiveMonthID = EnumMonth.None;
            ArchiveYearID = 0;
            ArchiveStatus = EnumArchiveStatus.InitializeValue;
            ApprovedBy = 0;
            EmpCount = 0;
            Remarks = "";
            ApprovedByName = "";
            ErrorMessage = "";
            ArchiveSalaryStrucs = new List<ArchiveSalaryStruc>();
            EmployeeBatchDetails = new List<EmployeeBatchDetail>();
        }
        #region Properties
        public int ArchiveDataID { get; set; }
        public string ArchiveNo { get; set; }
        public DateTime ArchiveDate { get; set; }
        public EnumMonth ArchiveMonthID { get; set; }
        public int ArchiveYearID { get; set; }
        public int EmpCount { get; set; }
        public EnumArchiveStatus ArchiveStatus { get; set; }
        public int ApprovedBy { get; set; }
        public string Remarks { get; set; }
        public string ApprovedByName { get; set; }
        public string ErrorMessage { get; set; }
        public List<ArchiveSalaryStruc> ArchiveSalaryStrucs { get; set; }
        public List<EmployeeBatchDetail> EmployeeBatchDetails { get; set; }
       #endregion
        #region Derived Properties
        public string ArchiveDateSt
        {
            get
            {
                return this.ArchiveDate.ToString("dd MMM yyyy");
            }
        }
        public string ArchiveStatusName
        {
            get
            {
                return EnumObject.jGet(this.ArchiveStatus);
            }

        }
        public string ArchiveMonthName
        {
            get
            {
                return EnumObject.jGet(this.ArchiveMonthID);   
            }
        }
        public string ArchiveDataNoMonthYear
        {
            get
            {
                return this.ArchiveNo + " / " + this.ArchiveMonthName + " / " + this.ArchiveYearID.ToString();
            }
        }
        #endregion

        #region Function
        public ArchiveData Save(long nUserID)
        {
            return ArchiveData.Service.Save(this, nUserID);
        }
        public static List<ArchiveData> Gets(string sSQL, long nUserID)
        {
            return ArchiveData.Service.Gets(sSQL, nUserID);
        }
        public ArchiveData Get(int id, long nUserID)
        {
            return ArchiveData.Service.Get(id, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ArchiveData.Service.Delete(id, nUserID);
        }

        public ArchiveData Approve(long nUserID)
        {
            return ArchiveData.Service.Approve(this, nUserID);
        }

        public ArchiveData Backup(long nUserID)
        {
            return ArchiveData.Service.Backup(this, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IArchiveDataService Service
        {
            get { return (IArchiveDataService)Services.Factory.CreateService(typeof(IArchiveDataService)); }
        }
        #endregion
    }
    #region IArchiveDataService interface

    public interface IArchiveDataService
    {
        ArchiveData Save(ArchiveData oArchiveData, Int64 nUserID);
        ArchiveData Approve(ArchiveData oArchiveData, Int64 nUserID);
        ArchiveData Backup(ArchiveData oArchiveData, Int64 nUserID);
        List<ArchiveData> Gets(string sSQL, long nUserID);
        ArchiveData Get(int id, long nUserID);
        string Delete(int id, long nUserID);
    }
    #endregion
}
