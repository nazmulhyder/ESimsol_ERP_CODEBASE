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
    #region Circular

    public class Circular : BusinessObject
    {
        public Circular()
        {
            CircularID = 0;
            DepartmentID = 0;
            DesignationID = 0;
            NoOfPosition = 0;
            Description = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            ApproveBy = 0;
            ApproveByDate = DateTime.Now;
            IsActive = true;
            ErrorMessage = "";
            ApproveByName = "";
            DepartmentName = "";
            DesignationName = "";

        }

        #region Properties
        public int CircularID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public int NoOfPosition { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveByDate { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string ActivityStatus { get { if (IsActive)return "Active"; else return "Inactive"; } }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string ApproveByName { get; set; }
        public string StartDateInString
        {
            get { return StartDate.ToString("dd MMM yyyy"); }
        }
        public string EndDateInString
        {
            get { return EndDate.ToString("dd MMM yyyy"); }
        }
        public string ApproveByDateString
        {
            get { return ApproveByDate.ToString("dd MMM yyyy"); }
        }

        #endregion

        #region Functions

        public static Circular Get(int id, long nUserID)
        {
            return Circular.Service.Get(id, nUserID);
        }
        public static Circular Get(string sSQL, long nUserID)
        {
            return Circular.Service.Get(sSQL, nUserID);
        }
        public static List<Circular> Gets(long nUserID)
        {
            return Circular.Service.Gets(nUserID);
        }
        public static List<Circular> Gets(string sSQL, long nUserID)
        {
            return Circular.Service.Gets(sSQL, nUserID);
        }
        public Circular IUD(int nDBOperation, long nUserID)
        {
            return Circular.Service.IUD(this, nDBOperation, nUserID);
        }
        public static Circular Activity(int nId, long nUserID)
        {
            return Circular.Service.Activity(nId, nUserID);
        }
        public static List<Circular> GetNewCirculars(string sSQL, long nUserID)
        {
            return Circular.Service.GetNewCirculars(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ICircularService Service
        {
            get { return (ICircularService)Services.Factory.CreateService(typeof(ICircularService)); }
        }

        #endregion
    }
    #endregion

    #region ICircular interface

    public interface ICircularService
    {
        Circular Get(int id, Int64 nUserID);
        Circular Get(string sSQL, Int64 nUserID);
        List<Circular> Gets(Int64 nUserID);
        List<Circular> Gets(string sSQL, Int64 nUserID);
        Circular IUD(Circular oCircular, int nDBOperation, Int64 nUserID);
        Circular Activity(int nId, Int64 nUserID);
        List<Circular> GetNewCirculars(string sSQL, Int64 nUserID);
    }
    #endregion
}
