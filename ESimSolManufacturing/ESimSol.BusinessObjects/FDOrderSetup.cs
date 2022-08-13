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
    #region FDOrderSetup
    
    public class FDOrderSetup : BusinessObject
    {
        public FDOrderSetup()
        {
            FDOrderSetupID = 0;
            BUID = 0;
            FDOType = EnumDOType.None;
            PrintNo = 0;
            Activity = true;
            MUnitID = 0;
            MUnitID_Alt = 0;
            FDOName = "";
            DONoCode = "";
            ShortName = "";
            MUName = "";
            CurrencyID = 0;
            PrintFormat = 0;
            CurrencySY = "";
            BUName = "";
            NoteFixed = "";
            ErrorMessage = "";
        }

        #region Properties
        public int FDOrderSetupID { get; set; }
        public int BUID { get; set; }
        public int CurrencyID { get; set; }
        public int FDOTypeInt { get; set; }
        public EnumDOType FDOType { get; set; }
        public string NoCode { get; set; }
        public string ShortName { get; set; }
        public string DONoCode { get; set; }
        public string FDOName { get; set; }
        public string PrintName { get; set; }
        public string BUName { get; set; }
        public int PrintNo { get; set; }
        public int ComboNo { get; set; }
        public string NoteFixed { get; set; }
        public bool Activity { get; set; }
        public int MUnitID { get; set; }
        public int MUnitID_Alt { get; set; }
        public string MUName { get; set; }
        public string CurrencySY { get; set; }
        public int PrintFormat { get; set; }
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
        public string FDOTypeSt { get { return EnumObject.jGet(this.FDOType); } }
        #endregion

        #endregion

        #region Functions
        public static List<FDOrderSetup> Gets(long nUserID)
        {
            return FDOrderSetup.Service.Gets(nUserID);
        }
        public static List<FDOrderSetup> GetsActive( int nBUID,long nUserID)
        {
            return FDOrderSetup.Service.GetsActive(nBUID,nUserID);
        }
        public static List<FDOrderSetup> Gets(string sSQL, long nUserID)
        {
            return FDOrderSetup.Service.Gets(sSQL, nUserID);
        }
        public FDOrderSetup Get(int id, long nUserID)
        {
            return FDOrderSetup.Service.Get(id, nUserID);
        }
        public FDOrderSetup GetByType(int nOrderType, long nUserID)
        {
            return FDOrderSetup.Service.GetByType(nOrderType, nUserID);
        }
        public static List<FDOrderSetup> GetByOrderTypes(int nBUID,bool bIsInHouse, string sOrderType,long nUserID)
        {
            return FDOrderSetup.Service.GetByOrderTypes( nBUID, bIsInHouse,  sOrderType,nUserID);
        }
        public FDOrderSetup Save(long nUserID)
        {
            return FDOrderSetup.Service.Save(this, nUserID);
        }
        public FDOrderSetup Activate(Int64 nUserID)
        {
            return FDOrderSetup.Service.Activate(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return FDOrderSetup.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFDOrderSetupService Service
        {
            get { return (IFDOrderSetupService)Services.Factory.CreateService(typeof(IFDOrderSetupService)); }
        }
        #endregion

    }
    #endregion

    #region IFDOrderSetup interface
    
    public interface IFDOrderSetupService
    {
        FDOrderSetup Get(int id, Int64 nUserID);
        FDOrderSetup GetByType(int nOrderType, Int64 nUserID);
        List<FDOrderSetup> Gets(string sSQL, long nUserID);
        List<FDOrderSetup> GetByOrderTypes(int nBUID,bool bIsInHouse, string sOrderType, long nUserID);
        List<FDOrderSetup> Gets(Int64 nUserID);
        List<FDOrderSetup> GetsActive( int nBUID, Int64 nUserID);
        string Delete(FDOrderSetup oFDOrderSetup, Int64 nUserID);
        FDOrderSetup Save(FDOrderSetup oFDOrderSetup, Int64 nUserID);
        FDOrderSetup Activate(FDOrderSetup oFDOrderSetup, Int64 nUserID);
    }
    #endregion
}