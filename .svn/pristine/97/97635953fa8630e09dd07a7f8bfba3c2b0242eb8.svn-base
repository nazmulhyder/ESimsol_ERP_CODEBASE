using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region GRNLandingCost

    public class GRNLandingCost : BusinessObject
    {
        public GRNLandingCost()
        {
            GRNID = 0;
            GRNDetailID = 0;
            ImportInvoiceID = 0;
            ProductID = 0;
            Qty = 0;
            TotalInvoiceAmount = 0;
            GRNEffectedInvoiceAmount = 0;
            UnitPrice = 0;
            PurchaseInvoiceID = 0;
            AccountHeadID = 0;
            AccountHeadName = "";
            CurrencyID = 0;
            AmountInCurrency = 0;
            CRate = 0;
            TotalLandingAmount = 0;
            HeadWiseLandingCost = 0;
            ProductName = "";
            ImportLCID = 0;
            ImportLCNo = "";
            GRNDate = DateTime.Now;
            
            ErrorMessage = "";
        }

        #region Properties

        public int GRNID { get; set; }
        public int GRNDetailID { get; set; }

        public int ImportInvoiceID { get; set; }
        public int ProductID { get; set; }
        public double Qty { get; set; }
        public double TotalInvoiceAmount { get; set; }
        public double GRNEffectedInvoiceAmount { get; set; }
        public double UnitPrice { get; set; }
        public double GRNAmount { get; set; }
        public int PurchaseInvoiceID { get; set; }
        public int AccountHeadID { get; set; }
        public string AccountHeadName { get; set; }
        public int CurrencyID { get; set; }
        public double AmountInCurrency { get; set; }
        public double CRate { get; set; }
        public double TotalLandingAmount { get; set; }

        public double HeadWiseLandingCost { get; set; }
        public string ProductName { get; set; }
        public int ImportLCID { get; set; }
        public string ImportLCNo { get; set; }
        public DateTime GRNDate { get; set; }

        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        #endregion

        #region Functions

        public static List<GRNLandingCost> Gets(long nUserID)
        {

            return GRNLandingCost.Service.Gets(nUserID);
        }

        public GRNLandingCost Get(int id, long nUserID)
        {
            return GRNLandingCost.Service.Get(id, nUserID);
        }

        public GRNLandingCost Save(long nUserID)
        {
            return GRNLandingCost.Service.Save(this, nUserID);
        }

        public static List<GRNLandingCost> Gets(string sSQL, long nUserID)
        {
            return GRNLandingCost.Service.Gets(sSQL, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return GRNLandingCost.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory


        internal static IGRNLandingCostService Service
        {
            get { return (IGRNLandingCostService)Services.Factory.CreateService(typeof(IGRNLandingCostService)); }
        }

        #endregion
    }
    #endregion

    #region IGRNLandingCost interface

    public interface IGRNLandingCostService
    {
        GRNLandingCost Get(int id, Int64 nUserID);

        List<GRNLandingCost> Gets(Int64 nUserID);

        List<GRNLandingCost> Gets(string sSQL, Int64 nUserID);

        string Delete(int id, Int64 nUserID);

        GRNLandingCost Save(GRNLandingCost oGRNLandingCost, Int64 nUserID);
    }
    #endregion
}
