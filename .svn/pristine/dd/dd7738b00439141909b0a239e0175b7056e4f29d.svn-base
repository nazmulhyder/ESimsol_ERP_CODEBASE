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
    #region ITaxRebateSchemeSlab

    public class ITaxRebateSchemeSlab : BusinessObject
    {
        public ITaxRebateSchemeSlab()
        {
            ITaxRSSID = 0;
            ITaxRebateSchemeID = 0;
            MinAmount = 0;
            MaxAmount = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int ITaxRSSID { get; set; }
        public int ITaxRebateSchemeID { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string AmountRangeST
        {
            get
            {
                return Global.MillionFormat(MinAmount) + "-" + Global.MillionFormat(MaxAmount);
            }
        }
        #endregion

        #region Functions
        public static ITaxRebateSchemeSlab Get(string sSQL, long nUserID)
        {
            return ITaxRebateSchemeSlab.Service.Get(sSQL, nUserID);
        }
        public static List<ITaxRebateSchemeSlab> Gets(string sSQL, long nUserID)
        {
            return ITaxRebateSchemeSlab.Service.Gets(sSQL, nUserID);
        }
        public ITaxRebateSchemeSlab IUD(int nDBOperation, long nUserID)
        {
            return ITaxRebateSchemeSlab.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IITaxRebateSchemeSlabService Service
        {
            get { return (IITaxRebateSchemeSlabService)Services.Factory.CreateService(typeof(IITaxRebateSchemeSlabService)); }
        }

        #endregion
    }
    #endregion

    #region IITaxRebateSchemeSlab interface

    public interface IITaxRebateSchemeSlabService
    {
        ITaxRebateSchemeSlab Get(string sSQL, Int64 nUserID);
        List<ITaxRebateSchemeSlab> Gets(string sSQL, Int64 nUserID);
        ITaxRebateSchemeSlab IUD(ITaxRebateSchemeSlab oITaxRebateSchemeSlab, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
