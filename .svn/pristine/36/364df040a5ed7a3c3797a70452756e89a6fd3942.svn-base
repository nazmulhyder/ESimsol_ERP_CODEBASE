using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region FabricPOSetup
    
    public class FabricPOSetup : BusinessObject
    {
        public FabricPOSetup()
        {
            FabricPOSetupID = 0;
            ErrorMessage = "";
            PrintNo = 0;
            Activity = true;
            IsLDApply = false;
            IsNeedCheckBy = false;
            IsNeedExpDelivery = false;
            CurrencyID = 0;
            OrderName = "";
            FabricCode = "";
            CurrencyName = "";
        }

        #region Properties
        public int FabricPOSetupID { get; set; }
        public string NoCode { get; set; }
        public string FabricCode { get; set; }
        public string OrderName { get; set; }
        public string POPrintName { get; set; }
        public int PrintNo { get; set; }
        public bool Activity { get; set; }
        public bool IsLDApply { get; set; }
        public bool IsNeedCheckBy { get; set; }
        public bool IsNeedExpDelivery { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string ErrorMessage { get; set; }
        
        #region Derived Property
        public string ActivitySt
        {
            get
            {
                if (this.Activity == true) return "Active";
                else if (this.Activity == false) return "Inactive";
                else return "-";
            }
        }
        #endregion

        #endregion

        #region Functions
        public static List<FabricPOSetup> Gets(long nUserID)
        {
            return FabricPOSetup.Service.Gets(nUserID);
        }
        public FabricPOSetup GetsActive(long nUserID)
        {
            return FabricPOSetup.Service.GetsActive(nUserID);
        }
        public static List<FabricPOSetup> Gets(string sSQL, long nUserID)
        {
            return FabricPOSetup.Service.Gets(sSQL, nUserID);
        }
        public FabricPOSetup Get(int id, long nUserID)
        {
            return FabricPOSetup.Service.Get(id, nUserID);
        }
       
        public FabricPOSetup Save(long nUserID)
        {
            return FabricPOSetup.Service.Save(this, nUserID);
        }
        public FabricPOSetup Activate(Int64 nUserID)
        {
            return FabricPOSetup.Service.Activate(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return FabricPOSetup.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFabricPOSetupService Service
        {
            get { return (IFabricPOSetupService)Services.Factory.CreateService(typeof(IFabricPOSetupService)); }
        }
        #endregion
    }
    #endregion


    #region IFabricPOSetup interface
    
    public interface IFabricPOSetupService
    {
        
        FabricPOSetup Get(int id, Int64 nUserID);
        List<FabricPOSetup> Gets(string sSQL, long nUserID);
        List<FabricPOSetup> Gets(Int64 nUserID);
        FabricPOSetup GetsActive( Int64 nUserID);
        string Delete(FabricPOSetup oFabricPOSetup, Int64 nUserID);
        FabricPOSetup Save(FabricPOSetup oFabricPOSetup, Int64 nUserID);
        FabricPOSetup Activate(FabricPOSetup oFabricPOSetup, Int64 nUserID);
    }
    #endregion
}