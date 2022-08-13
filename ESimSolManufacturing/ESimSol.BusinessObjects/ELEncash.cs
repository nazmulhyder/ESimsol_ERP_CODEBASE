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
    #region ELEncash

    public class ELEncash : BusinessObject
    {
        public ELEncash()
        {
            ELEncashID = 0;
            EmpLeaveLedgerID = 0;
            DeclarationDate = DateTime.Now;
            ConsiderELCount = 0;
            EncashELCount = 0;
            DPTName = "";
            DSGName = "";
            DateOfJoin = DateTime.Now;
            GrossSalary = 0;
            BasicAmount = 0;
            EncashAmount = 0;
            Stamp = 0;
            ApproveBy = 0;
            ApproveDate = DateTime.Now;
            ErrorMessage = "";
            PresencePerLeave = 0.0;
        }

        #region Properties
        public int ELEncashID { get; set; }
        public int EmpLeaveLedgerID { get; set; }
        public DateTime DeclarationDate { get; set; }
        public int ConsiderELCount { get; set; }
        public double EncashELCount { get; set; }
        public string DPTName { get; set; }
        public string DSGName { get; set; }
        public DateTime DateOfJoin { get; set; }
        public double GrossSalary { get; set; }
        public double BasicAmount { get; set; }
        public double EncashAmount { get; set; }
        public double Stamp { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public double PresencePerLeave { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string ApproveByName { get; set; }
        public string DateOfJoinInString
        {
            get
            {
                return DateOfJoin.ToString("dd MMM yyyy");
            }
        }
        public string DeclarationDateInString
        {
            get
            {
                return DeclarationDate.ToString("dd MMM yyyy");
            }
        }
        public string ApproveDateInString
        {
            get
            {
                if (this.ApproveBy <= 0) return "";
                else
                return ApproveDate.ToString("dd MMM yyyy");
            }
        }
        public double TotalDays
        {
            get
            {
                if (this.EncashELCount > 0 && this.PresencePerLeave > 0)
                {
                    return Math.Round(this.EncashELCount * this.PresencePerLeave,2);                    
                }
                else { return 0; }
            }
        }
        #endregion

        #region Functions
        public static List<ELEncash> Gets(string sSQL, long nUserID)
        {
            return ELEncash.Service.Gets(sSQL, nUserID);
        }
        public static string RollBackEncashedEL(string sELEncashIDs, DateTime dtDeclarationDate, long nUserID)
        {
            return ELEncash.Service.RollBackEncashedEL(sELEncashIDs, dtDeclarationDate, nUserID);
        }
        public static List<ELEncash> ApproveEncashedEL(string sELEncashIDs, DateTime dtDeclarationDate, long nUserID)
        {
            return ELEncash.Service.ApproveEncashedEL(sELEncashIDs, dtDeclarationDate, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IELEncashService Service
        {
            get { return (IELEncashService)Services.Factory.CreateService(typeof(IELEncashService)); }
        }

        #endregion
    }
    #endregion

    #region IELEncash interface
    public interface IELEncashService
    {
        List<ELEncash> Gets(string sSQL, Int64 nUserID);
        string RollBackEncashedEL(string sELEncashIDs, DateTime dtDeclarationDate, Int64 nUserID);
        List<ELEncash> ApproveEncashedEL(string sELEncashIDs, DateTime dtDeclarationDate, Int64 nUserID);
    }
    #endregion
}
