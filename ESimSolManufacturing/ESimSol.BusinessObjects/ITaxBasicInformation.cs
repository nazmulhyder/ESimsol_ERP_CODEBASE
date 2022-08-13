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
    #region ITaxBasicInformation

    public class ITaxBasicInformation : BusinessObject
    {
        public ITaxBasicInformation()
        {

            ITaxBasicInformationID = 0;
            EmployeeID = 0;
            TIN = "";
            ETIN = "";
            NationalID = "";
            TaxArea = EnumTaxArea.None;
            Cercile = "";
            Zone = "";
            IsNonResident = false;
            IsSelf = false;
            ErrorMessage = "";

        }

        #region Properties


        public int ITaxBasicInformationID { get; set; }

        public int EmployeeID { get; set; }

        public string TIN { get; set; }

        public string ETIN { get; set; }

        public string NationalID { get; set; }

        public EnumTaxArea TaxArea { get; set; }

        public string Cercile { get; set; }

        public string Zone { get; set; }

        public bool IsNonResident { get; set; }

        public bool IsSelf { get; set; }

        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public int TaxAreaInt { get; set; }
        public string TaxAreaInString
        {
            get
            {
                return TaxArea.ToString();
            }
        }

        #endregion

        #region Functions
        public static ITaxBasicInformation Get(int Id, long nUserID)
        {
            return ITaxBasicInformation.Service.Get(Id, nUserID);
        }
        public static ITaxBasicInformation Get(string sSQL, long nUserID)
        {
            return ITaxBasicInformation.Service.Get(sSQL, nUserID);
        }
        public static List<ITaxBasicInformation> Gets(long nUserID)
        {
            return ITaxBasicInformation.Service.Gets(nUserID);
        }

        public static List<ITaxBasicInformation> Gets(string sSQL, long nUserID)
        {
            return ITaxBasicInformation.Service.Gets(sSQL, nUserID);
        }

        public ITaxBasicInformation IUD(int nDBOperation, long nUserID)
        {
            return ITaxBasicInformation.Service.IUD(this, nDBOperation, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IITaxBasicInformationService Service
        {
            get { return (IITaxBasicInformationService)Services.Factory.CreateService(typeof(IITaxBasicInformationService)); }
        }

        #endregion
    }
    #endregion

    #region IITaxBasicInformation interface

    public interface IITaxBasicInformationService
    {
        ITaxBasicInformation Get(int id, Int64 nUserID);
        ITaxBasicInformation Get(string sSQL, Int64 nUserID);
        List<ITaxBasicInformation> Gets(Int64 nUserID);
        List<ITaxBasicInformation> Gets(string sSQL, Int64 nUserID);
        ITaxBasicInformation IUD(ITaxBasicInformation oITaxBasicInformation, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
