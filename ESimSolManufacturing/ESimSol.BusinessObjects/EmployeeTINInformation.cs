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
    #region EmployeeTINInformation

    public class EmployeeTINInformation : BusinessObject
    {
        public EmployeeTINInformation()
        {

            ETINID = 0;
            EmployeeID = 0;
            TIN = "";
            ETIN = "";
            TaxArea = EnumTaxArea.None;
            Circle = "";
            Zone = "";
            IsNonResident = false;
            ErrorMessage = "";

        }

        #region Properties

        public int ETINID { get; set; }
        public int EmployeeID { get; set; }
        public string TIN { get; set; }
        public string ETIN { get; set; }
        public EnumTaxArea TaxArea { get; set; }
        public string Circle { get; set; }
        public string Zone { get; set; }
        public bool IsNonResident { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<ITaxRateSlab> ITaxRateSlabs { get; set; }
        public string ResidencyInString { get { if (IsNonResident)return "Non Resident"; else return "Resident"; } }
        public int TaxAreaInint { get; set; }
        public string TaxAreaString
        {
            get
            {
                return TaxArea.ToString();
            }
        }

        #endregion

        #region Functions
        public static EmployeeTINInformation Get(int EmpID, long nUserID)
        {
            return EmployeeTINInformation.Service.Get(EmpID, nUserID);
        }
        public EmployeeTINInformation IUD(int nDBOperation, long nUserID)
        {
            return EmployeeTINInformation.Service.IUD(this, nDBOperation, nUserID);
        }
        public string Upload( long nUserID)
        {
            return EmployeeTINInformation.Service.Upload(this, nUserID);
        }
        public static EmployeeTINInformation Get(string sSQL, long nUserID)
        {
            return EmployeeTINInformation.Service.Get(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeTINInformationService Service
        {
            get { return (IEmployeeTINInformationService)Services.Factory.CreateService(typeof(IEmployeeTINInformationService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeTINInformation interface
    public interface IEmployeeTINInformationService
    {
        EmployeeTINInformation Get(int EmpID, Int64 nUserID);
        EmployeeTINInformation Get(string sSQL, Int64 nUserID);
        EmployeeTINInformation IUD(EmployeeTINInformation oEmployeeTINInformation, int nDBOperation, Int64 nUserID);
        string Upload(EmployeeTINInformation oEmployeeTINInformation, Int64 nUserID);
    }
    #endregion
}
