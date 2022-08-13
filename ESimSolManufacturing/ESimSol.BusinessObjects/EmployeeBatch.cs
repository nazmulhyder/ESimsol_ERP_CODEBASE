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
    public class EmployeeBatch : BusinessObject
    {
        public EmployeeBatch()
        {
            EmployeeBatchID = 0;
            BatchNo = "";
            BatchName = "";
            CauseOfCreation = "";
            CreateDate = DateTime.Today;
            CreateBy = 0;
            Remarks = "";
            ApprovedBy = 0;
            ApprovedByName = "";
            CreateByName = "";
            EmpCount = 0;
            ErrorMessage = "";
            EmployeeBatchDetails = new List<EmployeeBatchDetail>();
        }
        #region Properties
        
        public int EmployeeBatchID { get; set; }
        public string BatchNo { get; set; }
        public string BatchName { get; set; }
        public string CauseOfCreation { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public int EmpCount { get; set; }
        public string Remarks { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public string CreateByName { get; set; }
        public string ErrorMessage { get; set; }
        public List<EmployeeBatchDetail> EmployeeBatchDetails { get; set; }
        #endregion
        #region derived properties
        public string CreateDateInString
        {
            get
            {
                return this.CreateDate.ToString("dd MMM yyyy");
            }
        }
        public string EmployeeBatchNoCount
        {
            get
            {
                return this.BatchNo + " / " + this.BatchName + " / Emp Count(" + this.EmpCount + ")";
            }
        }
        #endregion
        public EmployeeBatch Save(long nUserID)
        {
            return EmployeeBatch.Service.Save(this, nUserID);
        }
        public static List<EmployeeBatch> Gets(string sSQL, long nUserID)
        {
            return EmployeeBatch.Service.Gets(sSQL, nUserID);
        }
        public EmployeeBatch Get(int id, long nUserID)
        {
            return EmployeeBatch.Service.Get(id, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return EmployeeBatch.Service.Delete(id, nUserID);
        }

        public EmployeeBatch Approve(long nUserID)
        {
            return EmployeeBatch.Service.Approve(this, nUserID);
        }
        public EmployeeBatch UndoApprove(long nUserID)
        {
            return EmployeeBatch.Service.UndoApprove(this, nUserID);
        }
        #region ServiceFactory
        internal static IEmployeeBatchService Service
        {
            get { return (IEmployeeBatchService)Services.Factory.CreateService(typeof(IEmployeeBatchService)); }
        }
        #endregion
    }
    #region IEmployeeBatchService interface

    public interface IEmployeeBatchService
    {
        EmployeeBatch Save(EmployeeBatch oEmployeeBatch, Int64 nUserID);
        List<EmployeeBatch> Gets(string sSQL, long nUserID);
        EmployeeBatch Get(int id, long nUserID);
        string Delete(int id, long nUserID);
        EmployeeBatch Approve(EmployeeBatch oEmployeeBatch, Int64 nUserID);
        EmployeeBatch UndoApprove(EmployeeBatch oEmployeeBatch, Int64 nUserID);
    }
    #endregion
}
