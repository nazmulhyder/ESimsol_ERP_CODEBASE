using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region Clause
    [DataContract]
    public class ImportLCClauseSetup : BusinessObject
    {
        #region  Constructor
        public ImportLCClauseSetup()
        {
            ImportLCClauseSetupID = 0;
            Activity = false;
            //Text = "";
            Clause = "";
            BUID = 0;
            DBUserID = 0;
            DBServerDate = DateTime.Today;
            SL = 0;
            LCPaymentType = EnumLCPaymentType.None;
            LCAppType = EnumLCAppType.None;
            ProductType = EnumProductNature.Dyeing;
            IsMandatory = false;
        }
        #endregion

        #region Properties
        public int ImportLCClauseSetupID { get; set; }
        public bool Activity { get; set; }
        //public string Text { get; set; }
        public string Clause { get; set; }
        public int BUID { get; set; }
        public string ErrorMessage { get; set; }
        public double DBUserID { get; set; }
        public DateTime DBServerDate { get; set; }
        public int SL  {get; set;}
        public EnumLCPaymentType   LCPaymentType {get; set;}
        public EnumLCAppType  LCAppType {get; set;}
        public EnumProductNature ProductType {get; set;}
        public bool  IsMandatory {get; set;}
        public string ActivityInString 
        {
            get
            {
                if (Activity != false)
                {
                    return ("Active");
                }
                else
                {
                    return ("Inactive");
                }
            }
        }
        public string LCPaymentTypeStr { get { return this.LCPaymentType.ToString(); } }
        public string LCAppTypeStr { get { return this.LCAppType.ToString(); } }
        public string ProductTypeStr { get { return this.ProductType.ToString(); } }
        #endregion

        #region Functions
        public ImportLCClauseSetup Get(int id, int nUserID)
        {
            return ImportLCClauseSetup.Service.Get(id, nUserID);
        }
        public static List<ImportLCClauseSetup> Gets( int nUserID)
        {
            return ImportLCClauseSetup.Service.Gets( nUserID);
        }
        public static List<ImportLCClauseSetup> Gets(int nBUID,int nUserID)
        {
            return ImportLCClauseSetup.Service.Gets(nUserID);
        }
        public static List<ImportLCClauseSetup> GetsActiveImportLCClause(int nUserID)
        {
            return ImportLCClauseSetup.Service.GetsActiveImportLCClause(nUserID);
        }
        public static List<ImportLCClauseSetup> GetsWithSQL(String sSQL, int nUserID)
        {
            return ImportLCClauseSetup.Service.GetsWithSQL(sSQL, nUserID);
        }
        public ImportLCClauseSetup Save(int nUserID)
        {
            return ImportLCClauseSetup.Service.Save(this, nUserID);
        }
        public string Delete(int nUserID)
        {
            return ImportLCClauseSetup.Service.Delete(this, nUserID);
        }
        public ImportLCClauseSetup IUD(int nDBOperation, long nUserID)
        {
            return ImportLCClauseSetup.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IImportLCClauseSetupService Service
        {
            get { return (IImportLCClauseSetupService)Services.Factory.CreateService(typeof(IImportLCClauseSetupService)); }
        }
        #endregion
    }
    #endregion


    #region IImportLCClauseSetup interface
    public interface IImportLCClauseSetupService
    {
        ImportLCClauseSetup Get(int id, Int64 nUserID);
        List<ImportLCClauseSetup> Gets(Int64 nUserID);
        List<ImportLCClauseSetup> GetsActiveImportLCClause(Int64 nUserID);
        List<ImportLCClauseSetup> Gets(int nBUID, Int64 nUserID);
        string Delete(ImportLCClauseSetup oImportLCClause, Int64 nUserID);
        ImportLCClauseSetup Save(ImportLCClauseSetup oImportLCClause, Int64 nUserID);

        ImportLCClauseSetup IUD(ImportLCClauseSetup oImportLCClauseSetup, int nDBOperation, Int64 nUserID);
        List<ImportLCClauseSetup> GetsWithSQL(string sSQL, Int64 nUserID);

    }
    #endregion

}
