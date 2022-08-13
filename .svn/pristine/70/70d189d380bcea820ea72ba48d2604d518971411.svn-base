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
    #region ITaxLedgerSalaryHead

    public class ITaxLedgerSalaryHead : BusinessObject
    {
        public ITaxLedgerSalaryHead()
        {

            ITaxLSHID = 0;
            ITaxLedgerID = 0;
            ITaxHeadConfigureID = 0;
            TaxableAmount = 0;
            SalaryHeadName = "";
            SalaryHeadAmount = 0;
            ErrorMessage = "";
            SalaryHeadID = 0;
            
        }

        #region Properties

        public int ITaxLSHID { get; set; }
        public int ITaxLedgerID { get; set; }
        public int ITaxHeadConfigureID { get; set; }
        public double TaxableAmount { get; set; }
        public string SalaryHeadName { get; set; }
        public double SalaryHeadAmount { get; set; }
        public string ErrorMessage { get; set; }
        public int SalaryHeadID { get; set; }
        #endregion

        #region Derived Property
      
        #endregion

        #region Functions
        public static ITaxLedgerSalaryHead Get(int Id, long nUserID)
        {
            return ITaxLedgerSalaryHead.Service.Get(Id, nUserID);
        }
        public static ITaxLedgerSalaryHead Get(string sSQL, long nUserID)
        {
            return ITaxLedgerSalaryHead.Service.Get(sSQL, nUserID);
        }
        public static List<ITaxLedgerSalaryHead> Gets(long nUserID)
        {
            return ITaxLedgerSalaryHead.Service.Gets(nUserID);
        }

        public static List<ITaxLedgerSalaryHead> Gets(string sSQL, long nUserID)
        {
            return ITaxLedgerSalaryHead.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IITaxLedgerSalaryHeadService Service
        {
            get { return (IITaxLedgerSalaryHeadService)Services.Factory.CreateService(typeof(IITaxLedgerSalaryHeadService)); }
        }

        #endregion
    }
    #endregion

    #region IITaxLedgerSalaryHead interface

    public interface IITaxLedgerSalaryHeadService
    {
        ITaxLedgerSalaryHead Get(int id, Int64 nUserID);
        ITaxLedgerSalaryHead Get(string sSQL, Int64 nUserID);
        List<ITaxLedgerSalaryHead> Gets(Int64 nUserID);
        List<ITaxLedgerSalaryHead> Gets(string sSQL, Int64 nUserID);
       
    }
    #endregion
}
