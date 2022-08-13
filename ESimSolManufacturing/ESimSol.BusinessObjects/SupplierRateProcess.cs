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
    #region SupplierRateProcess

    public class SupplierRateProcess : BusinessObject
    {
        public SupplierRateProcess()
        {
           
            ProductID = 0;
            ProductName = "";
            RequiredQty = 0;
            PurchaseQty = 0;
            MUnitID = 0;
            UnitName = "";
            UnitSymbol = "";
            SupplierID = 0;
            SupplierID1 = 0;
            SupplierName1 = "";
            Rate1 = "";
            SupplierID2 = 0;
            SupplierName2 = "";
            Rate2 = "";
            SupplierID3 = 0;
            SupplierName3 = "";
            Rate3 = "";
            SupplierID4 = 0;
            SupplierName4 = "";
            Rate4 = "";
            SupplierID5 = 0;
            SupplierName5 = "";
            Rate5 = "";
            SupplierID6 = 0;
            SupplierName6 = "";
            Rate6 = "";
            SupplierID7 = 0;
            SupplierName7 = "";
            Rate7 = "";
            SupplierID8 = 0;
            SupplierName8 = "";
            Rate8 = "";
            SupplierID9 = 0;
            SupplierName9 = "";
            Rate9 = "";
            SupplierID10 = 0;
            SupplierName10 = "";
            Rate10 = "";
            SupplierID11 = 0;
            SupplierName11 = "";
            Rate11 = "";
            SupplierID12 = 0;
            SupplierName12 = "";
            Rate12 = "";
            SupplierID13 = 0;
            SupplierName13 = "";
            Rate13 = "";
            SupplierID14 = 0;
            SupplierName14 = "";
            Rate14 = "";
            SupplierID15 = 0;
            SupplierName15 = "";
            Rate15 = "";
            PQDetailID1 = 0;
            PQDetailID2 = 0;
            PQDetailID3 = 0;
            PQDetailID4 = 0;
            PQDetailID5 = 0;
            PQDetailID6 = 0;
            PQDetailID7 = 0;
            PQDetailID8 = 0;
            PQDetailID9 = 0; 
            PQDetailID10 = 0;
            PQDetailID11 = 0;
            PQDetailID12 = 0;
            PQDetailID13 = 0;
            PQDetailID14 = 0;
            PQDetailID15 = 0;
            MaxSupplierCount = 0;
            UnitPrice = 0;
            Note = "";
            DeliveryFromStockQty = 0;
            UnitSymbol = "";
            ErrorMessage = "";
            SupplierRateProcesss = new List<SupplierRateProcess>();
        }

        #region Properties

     
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double RequiredQty { get; set; }
        public double PurchaseQty { get; set; }
        public int MUnitID { get; set; }
        public string UnitName { get; set; }
        public int SupplierID1 { get; set; }
        public string SupplierName1 { get; set; }
        public string Rate1 { get; set; }
        public int SupplierID2 { get; set; }
        public string SupplierName2 { get; set; }
        public string Rate2 { get; set; }
        public int SupplierID3 { get; set; }
        public string SupplierName3 { get; set; }
        public string Rate3 { get; set; }
        public int SupplierID4 { get; set; }
        public string SupplierName4 { get; set; }
        public string Rate4 { get; set; }
        public int SupplierID5 { get; set; }
        public string SupplierName5 { get; set; }
        public string Rate5 { get; set; }
        public int SupplierID6 { get; set; }
        public string SupplierName6 { get; set; }
        public string Rate6 { get; set; }
        public int SupplierID7 { get; set; }
        public string SupplierName7 { get; set; }
        public string Rate7 { get; set; }
        public int SupplierID8 { get; set; }
        public string SupplierName8 { get; set; }
        public string Rate8 { get; set; }
        public int SupplierID9 { get; set; }
        public string SupplierName9 { get; set; }
        public string Rate9 { get; set; }
        public int SupplierID10 { get; set; }
        public string SupplierName10 { get; set; }
        public string Rate10 { get; set; }
        public int SupplierID11 { get; set; }
        public string SupplierName11 { get; set; }
        public string Rate11 { get; set; }
        public int SupplierID12 { get; set; }
        public string SupplierName12 { get; set; }
        public string Rate12 { get; set; }
        public int SupplierID13 { get; set; }
        public string SupplierName13 { get; set; }
        public string Rate13 { get; set; }
        public int SupplierID14 { get; set; }
        public string SupplierName14 { get; set; }
        public string Rate14 { get; set; }
        public int SupplierID15 { get; set; }
        public string SupplierName15 { get; set; }
        public string Rate15 { get; set; }
        public int NOADetailID { get; set; }
        public int PQDetailID1 { get; set; }
        public int PQDetailID2 { get; set; }
        public int PQDetailID3 { get; set; }
        public int PQDetailID4 { get; set; }
        public int PQDetailID5 { get; set; }
        public int PQDetailID6 { get; set; }
        public int PQDetailID7 { get; set; }
        public int PQDetailID8 { get; set; }
        public int PQDetailID9 { get; set; }
        public int PQDetailID10 { get; set; }
        public int PQDetailID11 { get; set; }
        public int PQDetailID12 { get; set; }
        public int PQDetailID13 { get; set; }
        public int PQDetailID14 { get; set; }
        public int PQDetailID15 { get; set; }
        public int MaxSupplierCount { get; set; }
        public double UnitPrice { get; set; }
        public double   DeliveryFromStockQty {get;set;}
        public string UnitSymbol { get; set; }
        public double Amount { 
            get
            {
                return this.RequiredQty * this.UnitPrice;
            }
        }
        public double ApprovedAmount
        {
            get
            {
                return this.PurchaseQty * this.UnitPrice;
            }
        }
        public string Note { get; set; }
        public int SupplierID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public string RequiredQtyInString
        {
            get
            {
                return this.RequiredQty + "~" + this.ProductID;
            }
        }
        public List<SupplierRateProcess> SupplierRateProcesss { get; set; }
        public Company Company { get; set; }
        #endregion

        #region Functions

        public static List<SupplierRateProcess> Gets(int nNOAID, long nUserID)
        {
            return SupplierRateProcess.Service.Gets(nNOAID, nUserID);
        }
        public static List<SupplierRateProcess> GetsByLog(int nNOAID, long nUserID)
        {
            return SupplierRateProcess.Service.GetsByLog(nNOAID, nUserID);
        }
        public static List<SupplierRateProcess> GetsBy(int nNOAID, int nNOASignatoryID,long nUserID)
        {
            return SupplierRateProcess.Service.GetsBy(nNOAID,nNOASignatoryID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ISupplierRateProcessService Service
        {
            get { return (ISupplierRateProcessService)Services.Factory.CreateService(typeof(ISupplierRateProcessService)); }
        }
        #endregion
    }
    #endregion

    #region ISupplierRateProcess interface

    public interface ISupplierRateProcessService
    {
        List<SupplierRateProcess> Gets(int nNOAID, long nUserID);
        List<SupplierRateProcess> GetsByLog(int nNOAID, long nUserID);
        List<SupplierRateProcess> GetsBy(int nNOAID,int nNOASignatoryID, long nUserID);
    }
    #endregion
    
    
 
}
