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
    #region EmployeeAuthentication

    public class EmployeeAuthentication : BusinessObject
    {
        public EmployeeAuthentication()
        {
            EmployeeAuthenticationID = 0;
            EmployeeID = 0;
            FingerPrint = null;
            CardNo = "";
            Password = "";
            IsActive = true;
            ErrorMessage = "";

        }


        #region Properties

        public int EmployeeAuthenticationID { get; set; }
        public int EmployeeID { get; set; }
        public byte[] FingerPrint { get; set; }
        public string CardNo { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string Activity { get { if (IsActive)return "Active"; else return "Inactive"; } }
        #endregion

        #region Functions
        public static List<EmployeeAuthentication> Gets(int nEmployeeID, long nUserID)
        {
            return EmployeeAuthentication.Service.Gets(nEmployeeID, nUserID);
        }
        public static List<EmployeeAuthentication> Gets(string sSQL, long nUserID)
        {
            return EmployeeAuthentication.Service.Gets(sSQL, nUserID);
        }
        public EmployeeAuthentication Get(int id, long nUserID)
        {
            return EmployeeAuthentication.Service.Get(id, nUserID);
        }

        public EmployeeAuthentication IUD(int nDBOperation, long nUserID)
        {
            return EmployeeAuthentication.Service.IUD(this, nDBOperation, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IEmployeeAuthenticationService Service
        {
            get { return (IEmployeeAuthenticationService)Services.Factory.CreateService(typeof(IEmployeeAuthenticationService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeAuthentication interface

    public interface IEmployeeAuthenticationService
    {
        List<EmployeeAuthentication> Gets(int nEmployeeID, Int64 nUserID);
        List<EmployeeAuthentication> Gets(string sSQL, Int64 nUserID);
        EmployeeAuthentication Get(int id, Int64 nUserID);
        EmployeeAuthentication IUD(EmployeeAuthentication oEmployeeAuthentication, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
