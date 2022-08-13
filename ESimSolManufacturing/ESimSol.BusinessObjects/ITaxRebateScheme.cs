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
    #region ITaxRebateScheme

    public class ITaxRebateScheme : BusinessObject
    {
        public ITaxRebateScheme()
        {

            ITaxRebateSchemeID = 0;
            ITaxRateSchemeID = 0;
            ITaxRebateType = EnumITaxRebateType.None;
            MaxRebateAmount = 0;
            PercentOfTaxIncome = 0;
            RebateInPercent = 0;
            ErrorMessage = "";

        }

        #region Properties

        public int ITaxRebateSchemeID { get; set; }
        public int ITaxRateSchemeID { get; set; }
        public EnumITaxRebateType ITaxRebateType { get; set; }
        public double MaxRebateAmount { get; set; }
        public double PercentOfTaxIncome { get; set; }
        public double RebateInPercent { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public int ITaxRebateTypeInint { get; set; }
        public string ITaxRebateTypeString
        {
            get
            {
                return ITaxRebateType.ToString();
            }
        }
        public string MaxRebateAmountST
        {
            get
            {
                return Global.MillionFormat(MaxRebateAmount);
            }
        }
        public string RebateST
        {
            get
            {
                return "Max." + this.MaxRebateAmountST + "(BDT) or " + this.PercentOfTaxIncome.ToString() + "% of taxable income whichever is lower"+
                    " rebate is " + this.RebateInPercent + "% of " + this.ITaxRebateType.ToString();
            }
        }
        #endregion

        #region Functions
        public static ITaxRebateScheme Get(int Id, long nUserID)
        {
            return ITaxRebateScheme.Service.Get(Id, nUserID);
        }
        public static ITaxRebateScheme Get(string sSQL, long nUserID)
        {
            return ITaxRebateScheme.Service.Get(sSQL, nUserID);
        }
        public static List<ITaxRebateScheme> Gets(long nUserID)
        {
            return ITaxRebateScheme.Service.Gets(nUserID);
        }

        public static List<ITaxRebateScheme> Gets(string sSQL, long nUserID)
        {
            return ITaxRebateScheme.Service.Gets(sSQL, nUserID);
        }

        public ITaxRebateScheme IUD(int nDBOperation, long nUserID)
        {
            return ITaxRebateScheme.Service.IUD(this, nDBOperation, nUserID);
        }



        #endregion

        #region ServiceFactory
        internal static IITaxRebateSchemeService Service
        {
            get { return (IITaxRebateSchemeService)Services.Factory.CreateService(typeof(IITaxRebateSchemeService)); }
        }

        #endregion
    }
    #endregion

    #region IITaxRebateScheme interface
    public interface IITaxRebateSchemeService
    {
        ITaxRebateScheme Get(int id, Int64 nUserID);
        ITaxRebateScheme Get(string sSQL, Int64 nUserID);
        List<ITaxRebateScheme> Gets(Int64 nUserID);
        List<ITaxRebateScheme> Gets(string sSQL, Int64 nUserID);
        ITaxRebateScheme IUD(ITaxRebateScheme oITaxRebateScheme, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
