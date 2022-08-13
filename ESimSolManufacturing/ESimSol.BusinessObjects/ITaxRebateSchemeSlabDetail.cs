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
    #region ITaxRebateSchemeSlabDetail

    public class ITaxRebateSchemeSlabDetail : BusinessObject
    {
        public ITaxRebateSchemeSlabDetail()
        {
            ITaxRSSDID = 0;
            ITaxRSSID = 0;
            UptoAmount = 0;
            SlabRebateInPercent = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int ITaxRSSDID { get; set; }
        public int ITaxRSSID { get; set; }
        public double UptoAmount { get; set; }
        public double SlabRebateInPercent { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        #endregion

        #region Functions
        public static ITaxRebateSchemeSlabDetail Get(string sSQL, long nUserID)
        {
            return ITaxRebateSchemeSlabDetail.Service.Get(sSQL, nUserID);
        }
        public static List<ITaxRebateSchemeSlabDetail> Gets(string sSQL, long nUserID)
        {
            return ITaxRebateSchemeSlabDetail.Service.Gets(sSQL, nUserID);
        }
        public ITaxRebateSchemeSlabDetail IUD(int nDBOperation, long nUserID)
        {
            return ITaxRebateSchemeSlabDetail.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IITaxRebateSchemeSlabDetailService Service
        {
            get { return (IITaxRebateSchemeSlabDetailService)Services.Factory.CreateService(typeof(IITaxRebateSchemeSlabDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IITaxRebateSchemeSlabDetail interface

    public interface IITaxRebateSchemeSlabDetailService
    {
        ITaxRebateSchemeSlabDetail Get(string sSQL, Int64 nUserID);
        List<ITaxRebateSchemeSlabDetail> Gets(string sSQL, Int64 nUserID);
        ITaxRebateSchemeSlabDetail IUD(ITaxRebateSchemeSlabDetail oITaxRebateSchemeSlabDetail, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
