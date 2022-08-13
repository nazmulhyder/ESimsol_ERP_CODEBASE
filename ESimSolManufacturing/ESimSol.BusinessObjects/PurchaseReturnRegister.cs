using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region PurchaseReturnRegister
    
    public class PurchaseReturnRegister : BusinessObject
    {
        public PurchaseReturnRegister()
        {
            PurchaseReturnID = 0;
            PurchaseReturnNo = "";
            ReturnDate = DateTime.Now;
            ProductID = 0;
            ProductName = "";
            LotID = 0;
            LotNo = "";
            Qty = 0;
            Rate = 0;
            StoreName = "";
            RefTypeInt = 0;
            RefType = EnumPurchaseReturnType.None;
            RefNo = "";
            RefDate = DateTime.Now;
            SupplierID = 0;
            SupplierName = "";
            LCNo = "";
            StyleNo = "";
            MUSymbol = "";
        }

        #region Properties

        public int PurchaseReturnID { get; set; }
        public string PurchaseReturnNo { get; set; }
        public DateTime ReturnDate { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public double Qty { get; set; }
        public double Rate { get; set; }
        public string StoreName { get; set; }
        public int RefTypeInt { get; set; }
        public string RefNo { get; set; }
        public DateTime RefDate { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string LCNo { get; set; }
        public string StyleNo { get; set; }
        public string MUSymbol { get; set; }
        public EnumPurchaseReturnType RefType { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ReturnDateSt
        {
            get
            {
                return ReturnDate.ToString("dd MMM yyyy");
            }
        }
        public string RefDateSt
        {
            get
            {
                return RefDate.ToString("dd MMM yyyy");
            }
        }
        public double Amount
        {
            get
            {
                return this.Qty * this.Rate;
            }
        }
        public string RefTypeSt
        {
            get
            {
                return RefType.ToString();
            }
        }
        #endregion

        #region Functions

        public static List<PurchaseReturnRegister> GetsPurchaseReturnRegister(string sSQL, Int64 nUserID)
        {
            return PurchaseReturnRegister.Service.GetsPurchaseReturnRegister(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IPurchaseReturnRegisterService Service
        {
            get { return (IPurchaseReturnRegisterService)Services.Factory.CreateService(typeof(IPurchaseReturnRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IPurchaseReturnRegister interface
    
    public interface IPurchaseReturnRegisterService
    {
        List<PurchaseReturnRegister> GetsPurchaseReturnRegister(string sSQL, Int64 nUserID);
    }
    #endregion
}
