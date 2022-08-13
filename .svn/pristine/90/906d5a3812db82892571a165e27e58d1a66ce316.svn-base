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
    #region ITransactionGRN
 
    public class ITransactionGRN : BusinessObject
    {
        #region  Constructor
        public ITransactionGRN()
        {
            StartDate = DateTime.Now;
            InvoiceNo = "";
            LCNo = "";
            LocationName = "";
            OperationUnitName = "";
            ProductCode = "";
            ProductName = "";
            UnitName = "";
            CurrencyName = "";
            ContractorName = "";
            GRNType = EnumGRNType.None;
            Qty=0;
            Qty_Invoice = 0;
            LotNo = "";
            GRNNo = "";
            BUID = 0;
            RefObjectID = 0;
            ErrorMessage = "";
        }
        #endregion

        #region Properties
        public string ContractorName { get; set; }
        public EnumGRNType GRNType { get; set; }
        public int GRNTypeint { get; set; }
        public int BUID { get; set; }
        public string LocationName { get; set; }
        public string OperationUnitName { get; set; }
        public string LCNo { get; set; }
        public string LotNo { get; set; }
        public string InvoiceNo { get; set; }
        public string GRNNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public double Qty { get; set; }
        public double Qty_Invoice { get; set; }
        public double RefObjectID { get; set; }
        
        public double UnitPrice { get; set; }
        public string UnitName { get; set; }
        public string CurrencyName { get; set; }
        public string ErrorMessage { get; set; }
        public int ProductUsageInt { get; set; }
        public string GRNTypeSt
        {
            get
            {
                return GRNType.ToString();
            }
        }
        public string StoreName
        {
            get
            {
                return this.LocationName + "[" + this.OperationUnitName+"]";
            }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartDateSt
        {
            get
            {
                return this.StartDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateSt
        {
            get
            {
                return this.EndDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<ITransactionGRN> Gets(DateTime dStartDate, DateTime dEndDate, int nBUID, int nGRNType, int nProductType, Int64 nUserID)
        {
            return ITransactionGRN.Service.Gets(dStartDate, dEndDate, nBUID, nGRNType, nProductType, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IITransactionGRNService Service
        {
            get { return (IITransactionGRNService)Services.Factory.CreateService(typeof(IITransactionGRNService)); }
        }
        #endregion
    }
    #endregion


    #region IITransactionGRN interface

    public interface IITransactionGRNService
    {
        List<ITransactionGRN> Gets(DateTime dStartDate, DateTime dEndDate, int nBUID, int nGRNType, int ProductType, Int64 nUserID);
    }

    #endregion

}