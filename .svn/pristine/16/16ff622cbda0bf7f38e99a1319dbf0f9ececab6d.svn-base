using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region SampleInvoiceDetail
    [DataContract]
    public class SampleInvoiceDetail : BusinessObject
    {

        #region  Constructor
        public SampleInvoiceDetail()
        {
            SampleInvoiceDetailID = 0;
            SampleInvoiceID = 0;
            Qty = 0;
            Amount = 0;
            Description = "";
            DyeingOrderID = 0;
            ProductID = 0;
            ContractorID = 0;
            MUnitID = 0;
            PaymentType = 0;
            ContractorPersopnnalID = 0;
            MUnitID = 0;
            OrderType = 0;
            ContractorName = "";
            ContractorPersopnnalName = "";
            Description = "";
            ProductName = "";
            MUName = "";
            ProductCode = "";
            ColorNo = "";
            ColorName = "";
            PantonNO = "";
            Currency = "";
            StyleNo = "";
            Shade = EnumShade.NA;
            ErrorMessage = "";
            OrderTypeSt = "";
            CurrencyID = 0;
        }
        #endregion

        #region Properties
        public int SampleInvoiceDetailID { get; set; }
        public int SampleInvoiceID { get; set; }
        public int DyeingOrderID { get; set; }
        public int ProductID { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public int PaymentType { get; set; }
        public int OrderType { get; set; }
        public int CurrencyID { get; set; }
        public string OrderTypeSt { get; set; }
        public int ContractorType { get; set; }
        public string OrderNo { get; set; }
        
        public DateTime OrderDate { get; set; }
        public string OrderDateSt
        {
            get
            {
                return OrderDate.ToString("dd MMM yyyy");
            }
        }

        public int ContractorPersopnnalID { get; set; }
        public int MUnitID { get; set; }
        public string ContractorPersopnnalName { get; set; }

        #region Derive Property
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUName { get; set; }
       
        #region AmountSt
        private string _sAmountSt = "";
        public string AmountSt
        {
            get
            {
                _sAmountSt ="$"+ Global.MillionFormat( this.Amount);
                return _sAmountSt;
            }
        }

        #endregion
        #region QtySt
        private string _sQtySt = "";
        public string QtySt
        {
            get
            {
                _sQtySt = Global.MillionFormat(this.Qty);
                return _sQtySt;
            }
        }

        #endregion
        public string PaymentTypeSt
        {
            get
            {
                return EnumObject.jGet((EnumOrderPaymentType)this.PaymentType);
            }
        }
        #region Carry Property
        public string ColorNo { get; set; }
        public string ColorName { get; set; }
        public string PantonNO { get; set; }
        public EnumShade Shade { get; set; }
        public string Currency { get; set; }
        public string StyleNo { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        #endregion

        #endregion

        #region Functions

        public  SampleInvoiceDetail Get(int nId, long nUserID)
        {
            return SampleInvoiceDetail.Service.Get(nId, nUserID);
        }
        public static List<SampleInvoiceDetail> Gets(int nSampleInvoiceID, long nUserID)
        {
            return SampleInvoiceDetail.Service.Gets(nSampleInvoiceID,nUserID);
        }

        public static List<SampleInvoiceDetail> Gets(string sql, long nUserID)
        {
            return SampleInvoiceDetail.Service.Gets(sql, nUserID);
        }
      
        public SampleInvoiceDetail Save(long nUserID)
        {
            return SampleInvoiceDetail.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return SampleInvoiceDetail.Service.Delete(this, nUserID);
        }
        public string Delete_Adj(long nUserID)
        {
            return SampleInvoiceDetail.Service.Delete(this, nUserID);
        }
        public string UpdateInvoiceID(long nUserID)
        {
            return SampleInvoiceDetail.Service.UpdateInvoiceID(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static ISampleInvoiceDetailService Service
        {
            get { return (ISampleInvoiceDetailService)Services.Factory.CreateService(typeof(ISampleInvoiceDetailService)); }
        }
       
        #endregion
    }
    #endregion

    #region ISampleInvoiceDetail interface
    [ServiceContract]
    public interface ISampleInvoiceDetailService
    {
        SampleInvoiceDetail Get(int nID, Int64 nUserID);
        List<SampleInvoiceDetail> Gets(int nSampleInvoiceID, Int64 nUserID);
        List<SampleInvoiceDetail> Gets(string sql, Int64 nUserID);
        List<SampleInvoiceDetail> GetsBy(string sPaymentContractIDs,string nPIID, Int64 nUserID);
        List<SampleInvoiceDetail> Gets(Int64 nUserID);
        SampleInvoiceDetail Save(SampleInvoiceDetail oSampleInvoiceDetail, Int64 nUserID);
        string Delete(SampleInvoiceDetail oSampleInvoiceDetail, Int64 nUserID);
        string UpdateInvoiceID(SampleInvoiceDetail oSampleInvoiceDetail, Int64 nUserID);
        string Delete_Adj(SampleInvoiceDetail oSampleInvoiceDetail, Int64 nUserID);
     
     

    }
    #endregion
}
