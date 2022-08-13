using ICS.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    public class EmployeeActivityNote : BusinessObject
    {
        public EmployeeActivityNote()
        {
            EANID = 0;
            EACID = 0;
            Code = "";
            Name = "";
            EmployeeID = 0;
            DRPID = 0;
            DesignationID = 0;
            Note = "";
            ActivityDate = DateTime.Today;
            ApproveBy = 0;
            ApproveByDate = DateTime.Today;

            DepartmentName = "";
            DesignationName = "";
            ActivityCategory = "";
            ApproveByName = "";
            ErrorMessage = "";
            Params = "";
            LocationName = "";

        }


        #region Properties
        public int EANID { get; set; }
        public int EACID { get; set; }
        public string LocationName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int EmployeeID { get; set; }
        public int DRPID { get; set; }
        public int DesignationID { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ActivityDate { get; set; }
        public int ApproveBy { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string ActivityCategory { get; set; }
        public string ApproveByName { get; set; }
        public DateTime ApproveByDate { get; set; }

        public string Params { get; set; }

        #endregion



        public string OfficialInfo
        {
            get
            {
                return (this.DepartmentName + ", " + this.DesignationName);
            }
        }
        public string ApproveByDateInStr
        {
            get { return (this.ApproveBy > 0) ? this.ApproveByDate.ToString("dd MMM yyyy") : ""; }
        }
        public string ActivityDateInStr
        {
            get
            {
                return this.ActivityDate.ToString("dd MMM yyyy");
            }
        }


        #region Functions
        public static List<EmployeeActivityNote> Gets(Int64 nUserID)
        {
            return EmployeeActivityNote.Service.Gets(nUserID);
        }

        public static List<EmployeeActivityNote> Gets(string sSql, int nUserID)
        {
            return EmployeeActivityNote.Service.Gets(sSql, nUserID);
        }
        public EmployeeActivityNote Save(Int64 nUserID)
        {
            return EmployeeActivityNote.Service.Save(this, nUserID);
        }
        public EmployeeActivityNote Get(int nId, long nUserID)
        {
            return EmployeeActivityNote.Service.Get(nId, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return EmployeeActivityNote.Service.Delete(nId, nUserID);
        }
        public EmployeeActivityNote Approve(long nUserID)
        {
            return EmployeeActivityNote.Service.Approve(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeActivityNoteService Service
        {
            get { return (IEmployeeActivityNoteService)Services.Factory.CreateService(typeof(IEmployeeActivityNoteService)); }
        }
        #endregion

        #region IEmployeeActivityNote interface
        public interface IEmployeeActivityNoteService
        {
            List<EmployeeActivityNote> Gets(Int64 nUserID);

            List<EmployeeActivityNote> Gets(string sSQL, long nUserID);


            EmployeeActivityNote Save(EmployeeActivityNote oEmployeeActivityNote, Int64 nUserID);

            string Delete(int id, long nUserID);

            EmployeeActivityNote Get(int id, long nUserID);
            EmployeeActivityNote Approve(EmployeeActivityNote oEmployeeActivityNote, Int64 nUserID);

        #endregion
        }
    }
}
