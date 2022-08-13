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
    #region FabricOrderSetup
    
    public class FabricOrderSetup : BusinessObject
    {
        public FabricOrderSetup()
        {
            FabricOrderSetupID = 0;
            FabricOrderType = EnumFabricRequestType.None;
            OrderName = "";
            ShortName = "";
            PrintNo = 0;
            POPrintName = "";
            CodeNo = "";
            CodeName = "";
            IsApplyPO = false;
            IsRateApply = false;
            IsLocal = false;
            BUName = "";
            ErrorMessage = "";
            PrintNo = EnumExcellColumn.A;
            Activity = true;
            CurrencyID = 0;
            BUID = 0;
            MUnitID = 0;
            MUnitID_Alt = 0;
            MUName = "";
            MUName_Alt = "";
            CurrencySymbol = "";
            CodeNo_Lab = "";
            CodeName_Lab = "";
        }

        #region Properties
        public int FabricOrderSetupID { get; set; }
        public EnumFabricRequestType FabricOrderType { get; set; }
        public string OrderName { get; set; }
        public string ShortName { get; set; }
        public EnumExcellColumn PrintNo { get; set; }
        public string POPrintName { get; set; }
        public string CodeNo { get; set; }
        public string CodeName { get; set; }
        public int ComboNo { get; set; }
        public int CurrencyID { get; set; }
        public int MUnitID { get; set; }
        public int MUnitID_Alt { get; set; }
        public int BUID { get; set; }
        public bool IsApplyPO { get; set; }
        public bool Activity { get; set; }
        public string MUName { get; set; }
        public string MUName_Alt { get; set; }
        public string BUName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string CodeNo_Lab { get; set; }
        public string CodeName_Lab { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsRateApply { get; set; }
        public bool IsLocal { get; set; }
        
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
        public int PrintNoInt
        {
            get
            { return (int)PrintNo;}
        }
        public string PrintNoSt
        {
            get
            { return EnumObject.jGet(this.PrintNo); }
        }
        #endregion

        #endregion

        #region Functions
        public static List<FabricOrderSetup> Gets(long nUserID)
        {
            return FabricOrderSetup.Service.Gets(nUserID);
        }
        public static List<FabricOrderSetup> GetsActive( int nBUID,long nUserID)
        {
            return FabricOrderSetup.Service.GetsActive(nBUID,nUserID);
        }
        public static List<FabricOrderSetup> Gets(string sSQL, long nUserID)
        {
            return FabricOrderSetup.Service.Gets(sSQL, nUserID);
        }
        public FabricOrderSetup Get(int id, long nUserID)
        {
            return FabricOrderSetup.Service.Get(id, nUserID);
        }
        public FabricOrderSetup GetByType(int nOrderType, long nUserID)
        {
            return FabricOrderSetup.Service.GetByType(nOrderType, nUserID);
        }
        public static List<FabricOrderSetup> GetByOrderTypes(int nBUID,bool bIsApplyPO,long nUserID)
        {
            return FabricOrderSetup.Service.GetByOrderTypes( nBUID, bIsApplyPO,  nUserID);
        }
        public FabricOrderSetup Save(long nUserID)
        {
            return FabricOrderSetup.Service.Save(this, nUserID);
        }
        public FabricOrderSetup Activate(Int64 nUserID)
        {
            return FabricOrderSetup.Service.Activate(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return FabricOrderSetup.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFabricOrderSetupService Service
        {
            get { return (IFabricOrderSetupService)Services.Factory.CreateService(typeof(IFabricOrderSetupService)); }
        }
        #endregion



      
    }
    #endregion

    #region IFabricOrderSetup interface
    
    public interface IFabricOrderSetupService
    {
        FabricOrderSetup Get(int id, Int64 nUserID);
        FabricOrderSetup GetByType(int nOrderType, Int64 nUserID);
        List<FabricOrderSetup> Gets(string sSQL, long nUserID);
        List<FabricOrderSetup> GetByOrderTypes(int nBUID, bool bIsApplyPO, long nUserID);
        List<FabricOrderSetup> Gets(Int64 nUserID);
        List<FabricOrderSetup> GetsActive( int nBUID, Int64 nUserID);
        string Delete(FabricOrderSetup oFabricOrderSetup, Int64 nUserID);
        FabricOrderSetup Save(FabricOrderSetup oFabricOrderSetup, Int64 nUserID);
        FabricOrderSetup Activate(FabricOrderSetup oFabricOrderSetup, Int64 nUserID);
    }
    #endregion
}