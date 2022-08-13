using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region FabricQtyAllow
    public class FabricQtyAllow : BusinessObject
    {
        public FabricQtyAllow()
        {
            FabricQtyAllowID = 0;
            AllowType = EnumFabricQtyAllowType.None;
            Qty_From = 0.0;
            Qty_To=0.0;
            Percentage=0.0;
            MunitID = 0;
            LastUpdateBy = 0;
            LastUpdateDateTime = DateTime.Now;
            ErrorMessage = "";
            LastUpdateByName = "";
            MUnitName = "";
            Note = "";
            AllowTypeInInt = 0;
        }

        #region Properties
        public int FabricQtyAllowID { get; set; }
        public EnumFabricQtyAllowType AllowType { get; set; }
        public int AllowTypeInInt { get; set; }
        public EnumFabricRequestType OrderType { get; set; }
        public int OrderTypeInt { get; set; }
        public EnumWarpWeft WarpWeftType { get; set; }
        public int WarpWeftTypeInt { get; set; }
        public double Qty_From { get; set; }
        public double Qty_To { get; set; }
        public double Percentage { get; set; }
        public int MunitID { get; set; }
        public string Note { get; set; }
        public int LastUpdateBy { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string LastUpdateByName { get; set; }
        public string MUnitName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        #region derieved properties
        public string Qty_FromSt
        {
            get
            {
                return Global.MillionFormat(this.Qty_From);
            }
        }
        public string Qty_ToSt
        {
            get
            {
                return Global.MillionFormat(this.Qty_To);
            }
        }
        public string PercentageSt
        {
            get
            {
                return Global.MillionFormat(this.Percentage);
            }
        }
        public string AllowTypeSt
        {
            get
            {
               return EnumObject.jGet((EnumFabricQtyAllowType)this.AllowType);
            }
        }
        public string OrderTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumFabricRequestType)this.OrderType);
            }
        }
        public string WarpWeftTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumWarpWeft)this.WarpWeftType);
            }
        }
        public string LastUpdateDateTimeInString
        {
            get
            {
                return LastUpdateDateTime.ToString("dd MMM yyyy");
            }
        }
   
        #endregion
        #region Functions
        public static List<FabricQtyAllow> Gets(int nUserID)
        {
            return FabricQtyAllow.Service.Gets(nUserID);
        }
        public static List<FabricQtyAllow> Gets(string sSQL, int nUserID)
        {
            return FabricQtyAllow.Service.Gets(sSQL, nUserID);
        }
        public FabricQtyAllow Save(FabricQtyAllow oFabricQtyAllow, int nUserID)
        {
            return FabricQtyAllow.Service.Save(oFabricQtyAllow, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricQtyAllow.Service.Delete(nId, nUserID);
        }
        public static FabricQtyAllow Get(int nId, int nUserID)
        {
            return FabricQtyAllow.Service.Get(nId, nUserID);
        }
        public static List<FabricQtyAllow> Getsby(int nFSCDID, int nUserID)
        {
            return FabricQtyAllow.Service.Getsby(nFSCDID, nUserID);
        }

        #endregion
        #region ServiceFactory
        internal static IFabricQtyAllowService Service
        {
            get { return (IFabricQtyAllowService)Services.Factory.CreateService(typeof(IFabricQtyAllowService)); }
        }
        #endregion
    }
    #endregion
    public interface IFabricQtyAllowService
    {
        List<FabricQtyAllow> Gets(long nUserID);
        List<FabricQtyAllow> Gets(string sSQL, long nUserID);
        List<FabricQtyAllow> Getsby(int nFSCDID, long nUserID);
        FabricQtyAllow Get(int nId, long nUserID);
        FabricQtyAllow Save(FabricQtyAllow oFabricQtyAllow, long nUserID);
        string Delete(int id, long nUserID);

        
    }
}