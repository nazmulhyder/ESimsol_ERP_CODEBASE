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
    #region ITaxRebateItem

    public class ITaxRebateItem : BusinessObject
    {
        public ITaxRebateItem()
        {

            ITaxRebateItemID = 0;
            ITaxRebateType = EnumITaxRebateType.None;
            Description = "";
            IsActive = true;
            ErrorMessage = "";

        }

        #region Properties

        public int ITaxRebateItemID { get; set; }
        public EnumITaxRebateType ITaxRebateType { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string Activity { get { if (IsActive)return "Active"; else return "Inactive"; } }
        public int ITaxRebateTypeInint { get; set; }
        public string ITaxRebateTypeString
        {
            get
            {
                return ITaxRebateType.ToString();
            }
        }
        #endregion

        #region Functions
        public static ITaxRebateItem Get(int Id, long nUserID)
        {
            return ITaxRebateItem.Service.Get(Id, nUserID);
        }
        public static ITaxRebateItem Get(string sSQL, long nUserID)
        {
            return ITaxRebateItem.Service.Get(sSQL, nUserID);
        }
        public static List<ITaxRebateItem> Gets(long nUserID)
        {
            return ITaxRebateItem.Service.Gets(nUserID);
        }

        public static List<ITaxRebateItem> Gets(string sSQL, long nUserID)
        {
            return ITaxRebateItem.Service.Gets(sSQL, nUserID);
        }

        public ITaxRebateItem IUD(int nDBOperation, long nUserID)
        {
            return ITaxRebateItem.Service.IUD(this, nDBOperation, nUserID);
        }

        public static ITaxRebateItem Activite(int nId, bool Active, long nUserID)
        {
            return ITaxRebateItem.Service.Activite(nId, Active, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IITaxRebateItemService Service
        {
            get { return (IITaxRebateItemService)Services.Factory.CreateService(typeof(IITaxRebateItemService)); }
        }

        #endregion
    }
    #endregion

    #region IITaxRebateItem interface
    public interface IITaxRebateItemService
    {
        ITaxRebateItem Get(int id, Int64 nUserID);
        ITaxRebateItem Get(string sSQL, Int64 nUserID);
        List<ITaxRebateItem> Gets(Int64 nUserID);
        List<ITaxRebateItem> Gets(string sSQL, Int64 nUserID);
        ITaxRebateItem IUD(ITaxRebateItem oITaxRebateItem, int nDBOperation, Int64 nUserID);
        ITaxRebateItem Activite(int nId, bool Active, Int64 nUserID);
    }
    #endregion
}
