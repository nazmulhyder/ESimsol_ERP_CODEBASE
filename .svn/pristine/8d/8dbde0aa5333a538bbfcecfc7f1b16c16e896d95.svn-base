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
    #region DUOrderSetup
    
    public class DUOrderSetup : BusinessObject
    {
        public DUOrderSetup()
        {
            DUOrderSetupID = 0;
            BUID = 0;
            IsPIMendatory = false;
            IsRateMendatory = false;
            IsInvoiceMendatory = false;
            IsApplyOutside = false;
            IsApplyDyeingStep = false;
            IsSaveLabDip = false;
            IsApplyFabric = false;
            IsOpenRawLot = false;
            BUName = "";
            NoteFixed = "";
            ErrorMessage = "";
            OrderType = 0;
            PrintNo = 0;
            Activity = true;
            MUnitID = 0;
            MUnitID_Alt = 0;
            OrderName = "";
            DONoCode = "";
            ShortName = "";
            MUName = "";
            CurrencyID = 0;
            CurrencySY = "";
            MUName_Alt = "";
            ComboNoDC = 0;
            ComboNo = 0;
            DeliveryGrace = 0;
            DeliveryValidation = EnumDeliveryValidation.ColorQty;
        }

        #region Properties
        public int DUOrderSetupID { get; set; }
        public int BUID { get; set; }
        public int CurrencyID { get; set; }
        public int OrderType { get; set; } //EnumOrderType
        public string NoCode { get; set; }
        public string ShortName { get; set; }
        public string DONoCode { get; set; }
        public string OrderName { get; set; }
        public string PrintName { get; set; }
        public string BUName { get; set; }
        public int PrintNo { get; set; }
        public int ComboNo { get; set; }
        public int ComboNoDC { get; set; }
        public bool IsInHouse { get; set; }
        public string NoteFixed { get; set; }
        public bool Activity { get; set; }
        public bool IsOpenRawLot { get; set; }
        public bool IsPIMendatory { get; set; }
        public bool IsApplyOutside { get; set; }
        public bool IsApplyDyeingStep { get; set; }
        public bool IsRateMendatory { get; set; }
        public bool IsInvoiceMendatory { get; set; }
        public bool IsSaveLabDip{ get; set; }
        public bool IsApplyFabric { get; set; }
        public int MUnitID { get; set; }
        public int MUnitID_Alt { get; set; }
        public string MUName { get; set; }
        public string MUName_Alt { get; set; }
        
        public string CurrencySY { get; set; }
        public string ErrorMessage { get; set; }
        public double DeliveryGrace { get; set; }
        public EnumDeliveryValidation DeliveryValidation { get; set; }
        
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
        public static List<DUOrderSetup> Gets(long nUserID)
        {
            return DUOrderSetup.Service.Gets(nUserID);
        }
        public static List<DUOrderSetup> GetsActive( int nBUID,long nUserID)
        {
            return DUOrderSetup.Service.GetsActive(nBUID,nUserID);
        }
        public static List<DUOrderSetup> Gets(string sSQL, long nUserID)
        {
            return DUOrderSetup.Service.Gets(sSQL, nUserID);
        }
        public DUOrderSetup Get(int id, long nUserID)
        {
            return DUOrderSetup.Service.Get(id, nUserID);
        }
        public DUOrderSetup GetByType(int nOrderType, long nUserID)
        {
            return DUOrderSetup.Service.GetByType(nOrderType, nUserID);
        }
        public static List<DUOrderSetup> GetByOrderTypes(int nBUID,bool bIsInHouse, string sOrderType,long nUserID)
        {
            return DUOrderSetup.Service.GetByOrderTypes( nBUID, bIsInHouse,  sOrderType,nUserID);
        }
        public DUOrderSetup Save(long nUserID)
        {
            return DUOrderSetup.Service.Save(this, nUserID);
        }
        public DUOrderSetup Activate(Int64 nUserID)
        {
            return DUOrderSetup.Service.Activate(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return DUOrderSetup.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDUOrderSetupService Service
        {
            get { return (IDUOrderSetupService)Services.Factory.CreateService(typeof(IDUOrderSetupService)); }
        }
        #endregion
    }
    #endregion

    #region IDUOrderSetup interface
    
    public interface IDUOrderSetupService
    {
        DUOrderSetup Get(int id, Int64 nUserID);
        DUOrderSetup GetByType(int nOrderType, Int64 nUserID);
        List<DUOrderSetup> Gets(string sSQL, long nUserID);
        List<DUOrderSetup> GetByOrderTypes(int nBUID,bool bIsInHouse, string sOrderType, long nUserID);
        List<DUOrderSetup> Gets(Int64 nUserID);
        List<DUOrderSetup> GetsActive( int nBUID, Int64 nUserID);
        string Delete(DUOrderSetup oDUOrderSetup, Int64 nUserID);
        DUOrderSetup Save(DUOrderSetup oDUOrderSetup, Int64 nUserID);
        DUOrderSetup Activate(DUOrderSetup oDUOrderSetup, Int64 nUserID);
    }
    #endregion
}