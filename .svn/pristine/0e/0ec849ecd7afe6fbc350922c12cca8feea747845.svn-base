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
    #region EmployeeDocumentAcceptance
    public class EmployeeDocumentAcceptance
    {
        public EmployeeDocumentAcceptance()
        {
            EDAID = 0;
            EmployeeID = 0;
            ErrorMessage = "";
            EmployeeName = "";
        }

        #region Properties
        public int EDAID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions


        public static List<EmployeeDocumentAcceptance> Gets(string sSQL, long nUserID)
        {
            return EmployeeDocumentAcceptance.Service.Gets(sSQL, nUserID);
        }
        public static EmployeeDocumentAcceptance Get(string sSQL, long nUserID)
        {
            return EmployeeDocumentAcceptance.Service.Get(sSQL, nUserID);
        }
        public EmployeeDocumentAcceptance IUD(int nDBOperation, long nUserID)
        {
            return EmployeeDocumentAcceptance.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IEmployeeDocumentAcceptanceService Service
        {
            get { return (IEmployeeDocumentAcceptanceService)Services.Factory.CreateService(typeof(IEmployeeDocumentAcceptanceService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceDaily interface

    public interface IEmployeeDocumentAcceptanceService
    {
        List<EmployeeDocumentAcceptance> Gets(string sSQL, Int64 nUserID);
        EmployeeDocumentAcceptance Get(string sSQL, Int64 nUserID);
        EmployeeDocumentAcceptance IUD(EmployeeDocumentAcceptance oEmployeeDocumentAcceptance, int nDBOperation, Int64 nUserID);
       
      
    }
    #endregion
}


