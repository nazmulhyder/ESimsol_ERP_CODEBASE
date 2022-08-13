using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region ProductionBonus

    public class ProductionBonus
    {
        public ProductionBonus()
        {
            ProductionBonusID = 0;
            SalarySchemeID = 0;
            MinAmount = 0;
            MaxAmount = 0;
            Value = 0;
            IsPercent = false;
            IsActive = false;
            ErrorMessage = "";
           
        }

        #region Properties
        public int ProductionBonusID { get; set; }
        public int SalarySchemeID { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
        public double Value { get; set; }
        public bool IsPercent { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ProductionSlotInString
        {
            get
            {
                return this.MinAmount.ToString()+"-"+this.MaxAmount.ToString();
            }
        }
        public string ProductioBonusInString
        {
            get
            {
                if(this.IsPercent)
                return this.Value.ToString() + " % of production";
                else return this.Value.ToString();
            }
        }
        public string Activity { get { if (IsActive)return "Active"; else return "Inactive"; } }
        #endregion

        #region Functions


        public static List<ProductionBonus> Gets(string sSQL, long nUserID)
        {
            return ProductionBonus.Service.Gets(sSQL, nUserID);
        }
        public ProductionBonus IUD(int nDBOperation, long nUserID)
        {
            return ProductionBonus.Service.IUD(this, nDBOperation, nUserID);
        }
        public static ProductionBonus ActivityStatus(int nId, bool Active, long nUserID)
        {
            return ProductionBonus.Service.ActivityStatus(nId, Active, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IProductionBonusService Service
        {
            get { return (IProductionBonusService)Services.Factory.CreateService(typeof(IProductionBonusService)); }
        }
        #endregion
    }
    #endregion

    #region IProductionBonus interface

    public interface IProductionBonusService
    {
        List<ProductionBonus> Gets(string sSQL, Int64 nUserID);
        ProductionBonus IUD(ProductionBonus oProductionBonus, int nDBOperation, Int64 nUserID);
        ProductionBonus ActivityStatus(int nId, bool Active, Int64 nUserID);
    }
    #endregion
}
