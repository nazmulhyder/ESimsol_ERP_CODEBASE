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
    #region SalarySheetSignature

    public class SalarySheetSignature : BusinessObject
    {
        public SalarySheetSignature()
        {
            SignatureID = 0;
            SignatureName = string.Empty;
            ErrorMessage = "";
        }

        #region Properties

        public int SignatureID { get; set; }
        public string SignatureName { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        #endregion

        #region Functions
        public SalarySheetSignature Get(int nId, long nUserID)
        {
            return SalarySheetSignature.Service.Get(nId, nUserID);
        }
        public static List<SalarySheetSignature> Gets(string sSQL, long nUserID)
        {
            return SalarySheetSignature.Service.Gets(sSQL, nUserID);
        }
        public SalarySheetSignature IUD(short nDBOperation,long nUserID)
        {
            return SalarySheetSignature.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ISalarySheetSignatureService Service
        {
            get { return (ISalarySheetSignatureService)Services.Factory.CreateService(typeof(ISalarySheetSignatureService)); }
        }

        #endregion
    }
    #endregion

    #region ISalarySheetSignature interface

    public interface ISalarySheetSignatureService
    {
        SalarySheetSignature Get(int nId, Int64 nUserID);
        List<SalarySheetSignature> Gets(string sSQL, Int64 nUserID);
        SalarySheetSignature IUD(SalarySheetSignature oSalarySheetSignature, short nDBOperation, Int64 nUserID);
    }
    #endregion
}
